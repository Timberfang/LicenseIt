using Spectre.Console;

namespace LicenseIt;

internal static class Program
{
	private static void Main()
	{
		LicenseGenerator generator = new (
			AnsiConsole.Ask<string>("Name of the author(s):"),
			AnsiConsole.Ask<string>("Name of the project:"),
			AnsiConsole.Ask<int>("Year of release, or current year if not released:")
		);
		
		// Get license types (e.g. Media, Fonts, Software)
		// GetFileNameWithoutExtension works for getting final section of directory path as well
		string licenseTemplatePath = Path.Join(Environment.CurrentDirectory, "Templates");
		string chosenType = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.AddChoices(Directory.GetDirectories(licenseTemplatePath))
			.UseConverter(Path.GetFileNameWithoutExtension));

		// Get license templates
		string[] licenses = Directory.GetFiles(chosenType, "*.txt");
		string chosenLicense = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.AddChoices(licenses)
			.UseConverter(Path.GetFileNameWithoutExtension));

		// Create license from template
		if (File.Exists(generator.OutputPath))
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Error.WriteLine("License already exists: " + generator.OutputPath);
			Console.ResetColor();
		}
		else
		{
			generator.Generate(chosenLicense);
			AnsiConsole.MarkupLine($"License generated at {generator.OutputPath}.");
		}
	}
}