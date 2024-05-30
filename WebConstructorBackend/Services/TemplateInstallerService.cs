using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace WebConstructorBackend.Services;

public class TemplateInstallerService(
	IConfiguration configuration,
	IOptions<ConstructorOptions> options,
	IHttpClientFactory clientFactory) : BackgroundService
{
	private ConstructorOptions Options { get; } = options.Value;
	private IConfiguration Configuration { get; } = configuration;
	private IHttpClientFactory HttpClientFactory { get; } = clientFactory;

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		// TODO: При указании пути, делать ту же логику, только в другую папку
		if (Options.TemplatePath != null)
			throw new NotImplementedException();

		var templateDirectoryLocation = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ??
		                                throw new NotImplementedException();

		var templatePath = System.IO.Path.Combine(templateDirectoryLocation, "template-engine-master");
		Options.TemplatePath = templatePath;

		if (Directory.Exists(templatePath))
			return;

		var client = HttpClientFactory.CreateClient();
		var stream = await client.GetStreamAsync(Options.TemplateUrl, stoppingToken);

		var zipArchive = new ZipArchive(stream);

		var zipDirectory = zipArchive.Entries.First().FullName;

		await Task.Run(
			() => zipArchive.ExtractToDirectory(templateDirectoryLocation, overwriteFiles: true),
			stoppingToken);

		templatePath = System.IO.Path.Combine(templateDirectoryLocation, zipDirectory);
		Options.TemplatePath = templatePath;

		var shell = OperatingSystem.IsWindows() ? "cmd" : "sh";
		var args = OperatingSystem.IsWindows() ? "/c npm install" : "-c \"npm install\"";
		var process = new Process
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = shell,
				Arguments = args,
				WorkingDirectory = templatePath,
				UseShellExecute = false,
				RedirectStandardOutput = true
			}
		};

		process.OutputDataReceived += (_, eventArgs) => Console.WriteLine(eventArgs.Data);
		process.Exited += (_, _) => Console.WriteLine("Exited npm install");
		
		process.Start();
		process.BeginOutputReadLine();
		
		// TODO: Сделать асинхронным, но так чтобы при build зависимости уже были
		await process.WaitForExitAsync(stoppingToken);
	}
}