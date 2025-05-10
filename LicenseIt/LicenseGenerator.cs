namespace LicenseIt;

public class LicenseGenerator
{
	public LicenseGenerator(string authorName, string programName, int year)
	{
		AuthorName = authorName;
		ProgramName = programName;
		Year = year;
		OutputPath = Path.Join(Environment.CurrentDirectory, "Output", ProgramName, "LICENSE");
	}

	private string AuthorName { get; }
	private string ProgramName { get; }
	private int Year { get; }
	public string OutputPath { get; }

	public void Generate(string template)
	{
		// TODO: Support author email
		string licenseText = File.ReadAllText(template)
			.Replace(@"${AUTHOR_NAME}", AuthorName)
			.Replace(@"${PROGRAM_NAME}", ProgramName)
			.Replace(@"${YEAR}", Year.ToString()
			);
		Directory.CreateDirectory(Directory.GetParent(OutputPath)?.FullName ?? throw new NullReferenceException());
		File.WriteAllText(OutputPath, licenseText);
	}
}
