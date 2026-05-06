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

        var config = ConfigurationHelper.ReadConfig();
        
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = config.GodotExecutablePath,
                Arguments = $"--headless --path {Directory.GetCurrentDirectory()} --script {testRunnerPath} --quiet --disable-crash-handler --quit-on-finish",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.OutputDataReceived += (_, data) =>
        {
            Console.WriteLine(data.Data);
        };

        process.ErrorDataReceived += (_, data) =>
        {
            Console.WriteLine(data.Data);
        };
        
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new Exception($"GUnit Test Run failed (ExitCode: {process.ExitCode})");
        }
    }
}