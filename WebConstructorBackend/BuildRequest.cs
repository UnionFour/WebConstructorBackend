using System.ComponentModel.DataAnnotations;

namespace WebConstructorBackend;

public class BuildRequest
{
	[Display(Name = "organisation")]
	public required string Organisation { get; set; }

	[Display(Name = "config")]
	public required IFormFile Config { get; set; }

	public required IFormFileCollection Files { get; set; }
}