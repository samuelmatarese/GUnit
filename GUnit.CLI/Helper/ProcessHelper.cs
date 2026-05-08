using System.Diagnostics;
using Gunit.CLI.Models;

namespace Gunit.CLI.Helper;

public class ProcessHelper
{
    public static async Task<Process> RunProcess(GUnitConfig config, string arguments)
    {
           var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = config.GodotExecutablePath,
                Arguments = arguments,
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
        return process;
    }
}