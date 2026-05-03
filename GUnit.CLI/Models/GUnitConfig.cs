namespace Gunit.CLI.Models;

public class GUnitConfig(string godotExecutablePath)
{
    public string GodotExecutablePath {get; set;} = godotExecutablePath;
}