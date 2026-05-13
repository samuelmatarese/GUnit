using System.Reflection;
using System.Text;
using GUnit.Shared.Models;

namespace Gunit.CLI.Commands;

public class HelpCommand : ICommand
{
    public Task Execute(List<CommandParameter> commandParameters)
    {
        var sb = new StringBuilder();
        var version = Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion
            ?.Split('+')[0] ?? "unknown";

        sb.AppendLine("========================================");
        sb.AppendLine("         Godot Test Runner CLI");
        sb.AppendLine($"                v{version}");
        sb.AppendLine("========================================");
        sb.AppendLine();

        sb.AppendLine("Commands");
        sb.AppendLine("--------");
        sb.AppendLine("  init        Creates a new GodotTestRunner setup");
        sb.AppendLine("  config      Generates a default configuration file");
        sb.AppendLine("  test        Runs all discovered tests");
        sb.AppendLine("  help        Shows a quick summary of all available features");
        sb.AppendLine();

        sb.AppendLine("Parameters");
        sb.AppendLine("----------");
        sb.AppendLine("  --filter    Runs only tests matching the filter");
        sb.AppendLine("              Example: test --filter PlayerTests");
        sb.AppendLine();

        Console.WriteLine(sb.ToString());
        return Task.CompletedTask;
    }
}
