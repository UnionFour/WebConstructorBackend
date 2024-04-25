namespace WebConstructorBackend;

public class ConstructorOptions
{
	public const string Name = "ConstructorOptions";
	
	public required Uri TemplateUrl { get; set; }
	public required string SitesDirectory { get; set; } = "";
	public string TemplateDirectory { get; set; } = "";
}