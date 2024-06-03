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
using WebConstructorBackend.Domain.Entities;

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

// controll area
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

// user
app.MapPost("/user/update", ([FromServices] IUserRepository userRepository, User user) => { return userRepository.UpdateUser(user); });
app.MapGet("/user/get", ([FromServices] IUserRepository userRepository, Guid id) => { return userRepository.GetUser(id); });
app.MapGet("/user/getByEmail", ([FromServices] IUserRepository userRepository, string email) => { return userRepository.GetUserByEmail(email); });

// organization
app.MapGet("organization/{name}", ([FromServices] IOrganizationRepository organizationRepository, string name) => { return organizationRepository.GetOrganizationByName(name); });
app.MapPost("organization/update", ([FromServices] IOrganizationRepository organizationRepository, Organization org) => { return organizationRepository.UpdateOrganization(org); });
app.MapPost("organization/create", ([FromServices] IOrganizationRepository organizationRepository, Organization org) => { return organizationRepository.CreateOrganization(org); });

app.MapPost("organization/gym/add", ([FromServices] IOrganizationRepository organizationRepository, Guid orgID, Gym gym) => { return organizationRepository.AddGym(orgID, gym); });
app.MapDelete("organization/gym/remove", ([FromServices] IOrganizationRepository organizationRepository, Guid orgID, Gym gym) => { organizationRepository.RemoveGym(orgID, gym); });
app.MapGet("Organization/gyms", ([FromServices] IOrganizationRepository organizationRepository, Guid id) => { return organizationRepository.GetGymes(id); });
app.MapGet("Organization/gym", ([FromServices] IOrganizationRepository organizationRepository, Guid orgid, Guid gymid) => { return organizationRepository.GetGym(orgid, gymid); });

app.MapPost("organization/couch/add", ([FromServices] IOrganizationRepository organizationRepository, Guid orgID, Couch couch) => { return organizationRepository.AddCouch(orgID, couch); });
app.MapDelete("organization/couch/remove", ([FromServices] IOrganizationRepository organizationRepository, Guid orgID, Couch couch) => { organizationRepository.RemoveCouch(orgID, couch); });
app.MapGet("organization/couch", ([FromServices] IOrganizationRepository organizationRepository, Guid orgid, Guid couchid) => { return organizationRepository.GetCouch(orgid, couchid); });
app.MapGet("organization/couches", ([FromServices] IOrganizationRepository organizationRepository, Guid id) => { return organizationRepository.GetCouches(id); });

// gym
app.MapPost("gym/create", ([FromServices] IGymRepository gymRepository, Gym gym) => { return gymRepository.CreateGym(gym); });
app.MapPost("gym/update", ([FromServices] IGymRepository gymRepository, Gym gym) => { return gymRepository.UpdateGym(gym); });
app.MapGet("gym/{id}", ([FromServices] IGymRepository gymRepository, Guid id) => { return gymRepository.GetGymById(id); });
app.MapDelete("gym/remove", ([FromServices] IGymRepository gymRepository, Guid id) => { gymRepository.DeleteGym(id); });

// training
app.MapPost("training/create", ([FromServices] ITrainingRepository trainingRepository, Training gym) => { return trainingRepository.CreateTraining(gym); });
app.MapPost("training/update", ([FromServices] ITrainingRepository trainingRepository, Training gym) => { return trainingRepository.UpdateTraining(gym); });
app.MapGet("training/{id}", ([FromServices] ITrainingRepository trainingRepository, Guid id) => { return trainingRepository.GetTraining(id); });
app.MapDelete("training/remove", ([FromServices] ITrainingRepository trainingRepository, Guid id) => { trainingRepository.DeleteTraining(id); });

// endcontroll area
app.Run();