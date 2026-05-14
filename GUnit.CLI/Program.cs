using Gunit.CLI.Commands;
using GUnit.Shared.Models;

if (args.Length < 1)
{
    await new HelpCommand().Execute([]);
    return;
}

ICommand command = args[0] switch
{
    "init" => new InitCommand(),
    "config" => new ConfigurationCommand(),
    "test" => new TestCommand(),
    "help" => new HelpCommand(),
    _ => new HelpCommand()
};

await command.Execute(CommandParameter.ConvertToParameters([.. args[1..]]));