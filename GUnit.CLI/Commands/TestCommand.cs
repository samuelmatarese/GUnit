using System.Diagnostics;
using Gunit.CLI.Helper;

namespace Gunit.CLI.Commands;

public class TestCommand : ICommand
{
    public async Task Execute()
    {
        var config = ConfigurationHelper.ReadConfig();
        var errorOccurred = false;
        
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = config.GodotExecutablePath,
                Arguments = "--headless --path . --script ./GUnit/GodotTestRunner.cs",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
                Console.WriteLine(e.Data);
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                errorOccurred = true;
                
                try
                {
                    if (!process.HasExited)
                        process.Kill(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error at exiting process: {ex.Message}");
                }
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();

        if (errorOccurred || process.ExitCode != 0)
        {
            throw new Exception($"GUnit Test Run failed (ExitCode: {process.ExitCode})");
        }
    }
}