using Gunit.CLI.Commands;

if(args.Length < 1)
{
    Console.WriteLine("No Arguments defined");
    return;
}

ICommand command = args[0] switch
{
    "init" => new InitCommand(),
    _ => throw new ArgumentException($"No Command for '{args[0]}' defined.")
};

command.Execute();