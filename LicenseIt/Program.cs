using System.CommandLine;
using System.Text;

namespace LicenseIt;

internal static class Program
{
	private static async Task<int> Main(string[] args)
	{
		RootCommand rootCommand = new("License creator.");
		string licenseTemplatePath = Path.Join(Environment.CurrentDirectory, "Templates");
		Option<string> authorOption = new("--author-name")
		{
			IsRequired = true,
			Description = "Name(s) of the author(s). If multiple names are needed, separate them by commas."
		};
		Option<string> projectOption = new("--project-name")
		{
			IsRequired = true,
			Description = "Name of the project the license will be applied to."
		};
		Option<int> yearOption = new("--year")
		{
			Description = "Year when development began on the project."
		};
		Option<string> licenseOption = new("--license-name")
		{
			IsRequired = true,
			Description = "Name of the license (e.g. MIT). To view all valid license names, use the 'list' argument."
		};
		yearOption.SetDefaultValue(DateTime.Now.Year);

		Command createCommand = new("new", "Create a new LICENSE file")
		{
			authorOption, projectOption, yearOption, licenseOption
		};
		Command listCommand = new("list", "List all available templates");
		rootCommand.AddCommand(createCommand);
		rootCommand.AddCommand(listCommand);

		// Create license from template
		createCommand.SetHandler((authorName, projectName, year, license) =>
			{
				LicenseGenerator generator = new(authorName, projectName, year);
				if (File.Exists(generator.OutputPath))
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Error.WriteLine("License already exists: " + generator.OutputPath);
					Console.ResetColor();
				}
				else
				{
					// string template = File.ReadAllText(
					// 	Directory.GetFiles(licenseTemplatePath, $"{license}.txt", SearchOption.AllDirectories).First());
					string template = Directory
						.GetFiles(licenseTemplatePath, $"{license}.txt", SearchOption.AllDirectories).First();
					generator.Generate(template);
					Console.WriteLine($"License generated at {generator.OutputPath}.");
				}
			},
			authorOption, projectOption, yearOption, licenseOption);

		// List available templates
		listCommand.SetHandler(() =>
		{
			string[] licenses = Directory.GetFiles(licenseTemplatePath, "*.txt", SearchOption.AllDirectories);
			StringBuilder output = new();
			foreach (string license in licenses)
			{
				output.AppendLine(Path.GetFileNameWithoutExtension(license));
			}

			Console.WriteLine(output.ToString());
		});

		return await rootCommand.InvokeAsync(args);
	}
}
