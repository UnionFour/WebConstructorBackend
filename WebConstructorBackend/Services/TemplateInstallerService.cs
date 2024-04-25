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
		var client = HttpClientFactory.CreateClient();
		var stream = await client.GetStreamAsync(Options.TemplateUrl, stoppingToken);

		var zipArchive = new ZipArchive(stream);

		var templateDirectoryLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ??
		                                throw new NotImplementedException();
		var zipDirectory = zipArchive.Entries.First().FullName;

		await Task.Run(
			() => zipArchive.ExtractToDirectory(templateDirectoryLocation, overwriteFiles: true),
			stoppingToken);

		Options.TemplateDirectory = Path.Combine(templateDirectoryLocation, zipDirectory);
		Configuration.GetSection(ConstructorOptions.Name).Bind(Options);
	}
}