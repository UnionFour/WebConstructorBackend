namespace WebConstructorBackend;

public class ConstructorOptions
{
	public const string Name = "ConstructorOptions";
	
	public required Uri TemplateUrl { get; set; }
	public required string SitesPath { get; set; }
	public string? TemplatePath { get; set; }
	public string? NodeModulesPath { get; set; }
}