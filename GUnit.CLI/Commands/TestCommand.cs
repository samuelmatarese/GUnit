using System.Diagnostics;
using Gunit.CLI.Helper;

namespace Gunit.CLI.Commands;

public class TestCommand : ICommand
{
    public async Task Execute()
    {
        var testRunnerPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "GUnit/GodotTestRunner.cs"
        );

        Console.WriteLine("Test");
        Console.WriteLine(testRunnerPath);
        var config = ConfigurationHelper.ReadConfig();
        
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = config.GodotExecutablePath,
                Arguments = $"--headless --path {Directory.GetCurrentDirectory()} --script {testRunnerPath} --quiet --disable-crash-handler",
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

        if (!ValidateTestResult())
        {
            throw new Exception($"GUnit Test Run failed (ExitCode: {process.ExitCode})");
        }
    }

    private bool ValidateTestResult()
    {
        var resultFile = Path.Combine(
            Directory.GetCurrentDirectory(),
            "GUnit/gunit-test-result.txt"
        );

        if (File.Exists(resultFile))
        {
            var content = File.ReadAllText(resultFile);
            Console.WriteLine(content);

            if (content.Contains("Failed: 0") == true)
            {
                return true;
            }
        }
        else
        {
            Console.WriteLine("No result file found.");
        }

        return false;
    }
}