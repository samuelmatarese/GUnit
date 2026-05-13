using System.Text.Json;
using Gunit.CLI.Helper;
using GUnit.Shared.Models;

namespace Gunit.CLI.Commands;

public class ConfigurationCommand : ICommand
{
    public Task Execute(List<CommandParameter> commandParameters)
    {
        Console.WriteLine("Path to your Godot Executable:");
        var godotExecutablePath = Console.ReadLine();

        if (string.IsNullOrEmpty(godotExecutablePath))
        {
            Console.WriteLine("No Path for GodotExecutable assigned");
            return Task.CompletedTask;    
        }

        var config = new GUnitConfig(godotExecutablePath);
        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        ConfigurationHelper.CreateConfig(json);
        return Task.CompletedTask;
    }
}
