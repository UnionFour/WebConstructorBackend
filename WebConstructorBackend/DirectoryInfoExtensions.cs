namespace WebConstructorBackend;

public static class DirectoryInfoExtensions
{
	public static void DeepCopy(this DirectoryInfo source, DirectoryInfo destination)
	{
		foreach (var dir in source.GetDirectories())
			DeepCopy(dir, destination.CreateSubdirectory(dir.Name));

		foreach (var file in source.GetFiles())
			file.CopyTo(Path.Combine(destination.FullName, file.Name), true);
	}
}