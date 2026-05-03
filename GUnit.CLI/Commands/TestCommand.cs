using System.Diagnostics;
using Gunit.CLI.Helper;

namespace Gunit.CLI.Commands;

public class TestCommand : ICommand
{
    public void Execute()
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

        process.Start();

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        process.WaitForExit();
        Console.WriteLine(output);
        Console.WriteLine(error);
    }
}