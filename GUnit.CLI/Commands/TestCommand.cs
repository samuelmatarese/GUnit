using Gunit.CLI.Helper;
using GUnit.Shared.Models;

namespace Gunit.CLI.Commands;

public class TestCommand : ICommand
{
    public async Task Execute(List<CommandParameter> commandParameters)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var config = ConfigurationHelper.ReadConfig();
        var testRunnerPath = Path.Combine(
            currentDirectory,
            "GUnit/GodotTestRunner.cs"
        );

        Console.WriteLine($"Building Project at: {currentDirectory}...");
        await ProcessHelper.RunProcess(config, $"--headless --path {currentDirectory} --build-solutions --quit");

        Console.WriteLine($"Running TestRunner: {testRunnerPath}");
        var testPrcocess = await ProcessHelper.RunProcess(
            config, 
            $"--headless --path {currentDirectory} --script {testRunnerPath} --quiet --disable-crash-handler --quit-on-finish",
            commandParameters
        );

        if (testPrcocess.ExitCode != 0)
        {
            throw new Exception($"GUnit Test Run failed (ExitCode: {testPrcocess.ExitCode})");
        }
    }
}