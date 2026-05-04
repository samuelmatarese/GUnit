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
                Arguments = "--headless --path . --script ./GUnit/GodotTestRunner.cs",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
                Console.WriteLine(e.Data);
        };

        process.Start();
        process.BeginOutputReadLine();

        await process.WaitForExitAsync();
    }
}