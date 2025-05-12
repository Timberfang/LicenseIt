using System.CommandLine;

namespace LicenseIt;

internal static class Program
{
	private static async Task<int> Main(string[] args)
	{
		RootCommand rootCommand = new("License creator.");
		Option<string> authorOption = new("--author-name",
			"Name(s) of the author(s). If multiple names are needed, separate them by commas.") { IsRequired = true };
		Option<string> projectOption = new("--project-name",
			"Name of the project the license will be applied to.") { IsRequired = true };
		Option<string> licenseOption = new("--license-name",
			"Name of the license (e.g. MIT). Use the 'list' command to view all licenses.") { IsRequired = true };
		Option<int> yearOption = new("--year",
			"Year when development began on the project.");
		Option<string> emailOption = new("--email-address",
			"Email address(es) of the author(s).");
		Option<string> outputOption = new("--output",
			"Path to the output file. Defaults to ${working-directory} -> ${project-name} -> LICENSE.");
		yearOption.SetDefaultValue(DateTime.Now.Year);
		authorOption.AddAlias("--author");
		authorOption.AddAlias("-a");
		projectOption.AddAlias("--project");
		projectOption.AddAlias("-p");
		emailOption.AddAlias("--email");
		emailOption.AddAlias("-e");
		licenseOption.AddAlias("--license");
		licenseOption.AddAlias("--spdx");
		licenseOption.AddAlias("-l");
		outputOption.AddAlias("--destination");
		outputOption.AddAlias("-o");

		Command createCommand = new("new", "Create a new LICENSE file")
		{
			authorOption,
			projectOption,
			yearOption,
			emailOption,
			licenseOption,
			outputOption
		};
		Command listCommand = new("list", "List all available templates");
		rootCommand.AddCommand(createCommand);
		rootCommand.AddCommand(listCommand);

		// Create license from template
		createCommand.SetHandler(
			(authorName, projectName, year, email, license, destination) =>
			{
				if (string.IsNullOrEmpty(destination))
				{
					destination = Path.Join(Environment.CurrentDirectory, projectName, "LICENSE");
				}

				try
				{
					LicenseService.GenerateFromSpdx(authorName, projectName, license, year, email, destination);
				}
				catch (ArgumentException e)
				{
					ErrorHandler.WriteError(e.Message);
				}
				catch (FileNotFoundException e)
				{
					ErrorHandler.WriteError(e.Message);
				}
			},
			authorOption, projectOption, yearOption, emailOption, licenseOption, outputOption);

		// List available templates
		listCommand.SetHandler(() => { Console.WriteLine(string.Join(Environment.NewLine, LicenseService.List())); });

		return await rootCommand.InvokeAsync(args);
	}
}
