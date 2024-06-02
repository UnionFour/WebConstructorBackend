using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebConstructorBackend;
using WebConstructorBackend.Domain.Services.Auth;
using WebConstructorBackend.Domain.ValueTypes;
using WebConstructorBackend.Services;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Path = System.IO.Path;
using WebConstructorBackend.Domain.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using WebConstructorBackend.Domain.Services.DBContext;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Authorization
var authSection = builder.Configuration.GetSection("Auth");
var authOptions = authSection.Get<AuthOptions>();

builder.Services.Configure<AuthOptions>(authSection);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGymRepository, GymRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<ITrainingRepository, TrainingRepository>();
builder.Services.AddScoped<IUsersTrainingsRepository, UsersTrainingsRepository>();

builder.Services.AddDbContextPool<AppDBContext>(options => options.UseInMemoryDatabase("MockDB"));

builder.Services.AddDataProtection();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authOptions?.Issuer,
            ValidateAudience = true,
            ValidAudience = authOptions?.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = authOptions?.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddAuthorization();
builder.Services.AddHostedService<TemplateInstallerService>();

builder.Services.Configure<ConstructorOptions>(
    builder.Configuration.GetSection(ConstructorOptions.Name));

builder.Services.Configure<FormOptions>(options =>
{
    // Set the limit to 256 MB
    options.MultipartBodyLengthLimit = 268435456;
});

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddProjections()
    .AddMutationConventions()
    .RegisterDbContext<AppDBContext>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/build",
        async ([FromServices] IOptions<ConstructorOptions> options, [FromForm] BuildRequest request) =>
        {
            var sitesDirectory = new DirectoryInfo(options.Value.SitesPath);
            var tmpDirectory = new DirectoryInfo(Path.GetTempPath());
            var templateDirectory = new DirectoryInfo(options.Value.TemplatePath ?? throw new NotImplementedException());

            var srcDirectory = tmpDirectory.CreateSubdirectory(request.Organisation);
            var buildDirectory = sitesDirectory.CreateSubdirectory(request.Organisation);

			Console.WriteLine("Copy template");
			await Task.Run(() => templateDirectory.DeepCopy(srcDirectory));
			Console.WriteLine("Completed copy");

            var configPath = Path.Combine(srcDirectory.FullName, "config.json");
            var configStream = File.Create(configPath);
            await request.Config.CopyToAsync(configStream);
            await configStream.DisposeAsync();

            var contentTypes = new[] { "image/png", "image/svg" };

            foreach (var file in request.Files)
            {
                if (!contentTypes.Any(type => file.ContentType.Contains(type)))
                    continue;

                var imagePath = Path.Combine(srcDirectory.FullName, "src", "assets", file.FileName);
                await using var imageStream = File.Create(imagePath);
                await file.CopyToAsync(imageStream);
            }

			var nodeModulesDirectory = Path.Combine(options.Value.NodeModulesPath ?? throw new NotImplementedException(), "node_modules");
			Directory.CreateSymbolicLink(Path.Combine(srcDirectory.FullName, "node_modules"), nodeModulesDirectory);
			
			var command = $"npm run ng build -- --output-path={buildDirectory.FullName}";
			var shell = OperatingSystem.IsWindows() ? "cmd" : "sh";
			var args = OperatingSystem.IsWindows() ? $"/c {command}" : $"-c \"{command}\"";

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = shell,
                    Arguments = args,
                    WorkingDirectory = srcDirectory.FullName,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                }
            };

            process.OutputDataReceived += (_, eventArgs) => Console.WriteLine(eventArgs.Data);
            process.Exited += (_, _) =>
            {
                Console.WriteLine("Exited npm run build");
                srcDirectory.Delete(true);
            };
            process.Start();
            process.BeginOutputReadLine();

            await process.WaitForExitAsync();

            return Results.Ok();
        })
    .DisableAntiforgery();

app.MapPost("/auth/registration", ([FromServices] IAuthService authService, UserAuthInput input) => { return authService.RegisterUser(input); });
app.MapPost("/auth/authorization", ([FromServices] IAuthService authService, UserAuthInput input) => { return authService.AuthorizeUser(input); });

app.Run();