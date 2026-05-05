using System.Diagnostics;
using Gunit.CLI.Helper;

namespace Gunit.CLI.Commands;

public class TestCommand : ICommand
{
    public async Task Execute()
    {
        var config = ConfigurationHelper.ReadConfig();
        
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = config.GodotExecutablePath,
                Arguments = "--headless --path . --script ./GUnit/GodotTestRunner.cs --quiet --disable-crash-handler",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();
        WriteTestResultToConsole();

        if (process.ExitCode != 0)
        {
            throw new Exception($"GUnit Test Run failed (ExitCode: {process.ExitCode})");
        }
    }

    private void WriteTestResultToConsole()
    {
        var resultFile = Path.Combine(
            Directory.GetCurrentDirectory(),
            "GUnit/gunit-test-result.txt"
        );

        if (File.Exists(resultFile))
        {
            var content = File.ReadAllText(resultFile);
            Console.WriteLine(content);
        }
        else
        {
            Console.WriteLine("No result file found.");
        }
    }
}