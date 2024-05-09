using System.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebConstructorBackend;
using WebConstructorBackend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddHostedService<TemplateInstallerService>();

builder.Services.Configure<ConstructorOptions>(
	builder.Configuration.GetSection(ConstructorOptions.Name));

builder.Services.Configure<FormOptions>(options =>
{
	// Set the limit to 256 MB
	options.MultipartBodyLengthLimit = 268435456;
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/build",
		async ([FromServices] IOptions<ConstructorOptions> options, [FromForm] BuildRequest request) =>
		{
			var sitesDirectory = new DirectoryInfo(options.Value.SitesPath);
			var tmpDirectory = new DirectoryInfo(Path.GetTempPath());
			var templateDirectory = new DirectoryInfo(options.Value.TemplatePath ?? throw new NotImplementedException());

			var srcDirectory = tmpDirectory.CreateSubdirectory(request.Organisation);
			var buildDirectory = sitesDirectory.CreateSubdirectory(request.Organisation);

			Console.WriteLine("Copy dependencies");
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

app.Run();