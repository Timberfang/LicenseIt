﻿namespace LicenseIt;

internal static class LicenseService
{
	private static readonly string s_licenseTemplatePath = Path.Join(AppContext.BaseDirectory, "Templates");

	private static void Generate(string authorName, string projectName, string template, int year = -1,
		string email = "",
		string destination = "")
	{
		switch (year)
		{
			case -1:
				year = DateTime.Now.Year;
				break;
			case < 0:
				ErrorHandler.WriteError("Year must be greater than or equal to zero.");
				break;
		}

		if (destination == "")
		{
			destination = Path.Join(Environment.CurrentDirectory, projectName, "LICENSE");
		}

		if (File.Exists(destination))
		{
			throw new ArgumentException($"'{destination}' already exists.");
		}

		string licenseText = template
			.Replace(@"${AUTHOR_NAME}", authorName)
			.Replace(@"${AUTHOR_EMAIL}", email)
			.Replace(@"${PROJECT_NAME}", projectName)
			.Replace(@"${YEAR}", year.ToString());
		try
		{
			Directory.CreateDirectory(Directory.GetParent(destination)?.FullName ?? throw new NullReferenceException());
			File.WriteAllText(destination, licenseText);
		}
		catch (Exception e)
		{
			ErrorHandler.WriteError($"Could not write license file: {e.Message}");
			throw;
		}
	}

	internal static void GenerateFromFile(string authorName, string projectName, string template, int year = -1,
		string email = "",
		string destination = "")
	{
		if (!File.Exists(template))
		{
			throw new FileNotFoundException($"Template file '{template}' not found.");
		}

		Generate(authorName, projectName, File.ReadAllText(template), year, email, destination);
	}

	internal static void GenerateFromSpdx(string authorName, string projectName, string code, int year = -1,
		string email = "",
		string destination = "")
	{
		if (!List().Contains(code))
		{
			throw new FileNotFoundException($"License code '{code}' not found.");
		}

		string template = File.ReadAllText(Directory
			.GetFiles(s_licenseTemplatePath, $"{code}.txt", SearchOption.AllDirectories).First());
		Generate(authorName, projectName, template, year, email, destination);
	}

	internal static IEnumerable<string?> List() => Directory
		.GetFiles(s_licenseTemplatePath, "*.txt", SearchOption.AllDirectories)
		.Select(Path.GetFileNameWithoutExtension);
}
