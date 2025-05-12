using System.CommandLine;

namespace LicenseIt;

internal static class Program
{
	private static async Task<int> Main(string[] args)
	{
		RootCommand rootCommand = new("License creator.");
		Option<string> authorOption = new("--author-name")
		{
			IsRequired = true,
			Description = "Name(s) of the author(s). If multiple names are needed, separate them by commas."
		};
		Option<string> projectOption = new("--project-name")
		{
			IsRequired = true, Description = "Name of the project the license will be applied to."
		};
		Option<int> yearOption = new("--year") { Description = "Year when development began on the project." };
		Option<string> emailOption = new("--email-address") { Description = "Email address(es) of the author(s)." };
		Option<string> licenseOption = new("--license-name")
		{
			IsRequired = true,
			Description =
				"Name of the license (e.g. MIT). To view all valid license names, use the 'list' command."
		};
		Option<string> outputOption = new("--output")
		{
			Description =
				"Path to the output file. Defaults to ${working-directory} -> ${project-name} -> LICENSE."
		};
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
