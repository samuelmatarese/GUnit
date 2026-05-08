using System.Diagnostics;
using Gunit.CLI.Helper;
using Gunit.CLI.Models;

namespace Gunit.CLI.Commands;

public class TestCommand : ICommand
{
    public async Task Execute()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var config = ConfigurationHelper.ReadConfig();
        var testRunnerPath = Path.Combine(
            currentDirectory,
            "GUnit/GodotTestRunner.cs"
        );

        await ProcessHelper.RunProcess(config, $"--headless --path {currentDirectory} --build-solutions --quit");

        Console.WriteLine($"Running TestRunner: {testRunnerPath}");
        
        var testPrcocess = await ProcessHelper.RunProcess(
            config, 
            $"--headless --path {currentDirectory} --script {testRunnerPath} --quiet --disable-crash-handler --quit-on-finish"
        );

        if (testPrcocess.ExitCode != 0)
        {
            throw new Exception($"GUnit Test Run failed (ExitCode: {testPrcocess.ExitCode})");
        }
    }
}