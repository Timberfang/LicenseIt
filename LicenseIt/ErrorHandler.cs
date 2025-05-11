namespace LicenseIt;

internal static class ErrorHandler
{
	internal static void WriteError(string message)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.Error.WriteLine(message);
		Console.ResetColor();
	}
}
