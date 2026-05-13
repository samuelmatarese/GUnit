using System.Text.Json;
using GUnit.Shared.Models;

namespace Gunit.CLI.Helper;

public class ConfigurationHelper
{
    private const string ConfigurationPath = ".config/gunit";
    private const string ConfigurationName = "config.json";

    public static void CreateConfig(string content)
    {
        var configDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ConfigurationPath);

        Directory.CreateDirectory(configDir);

        var path = Path.Combine(configDir, "config.json");
        File.WriteAllText(path, content);
    }

    public static GUnitConfig ReadConfig()
    {
        var configFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ConfigurationPath,
            ConfigurationName);

        if (!File.Exists(configFile))
        {
            throw new NullReferenceException($"No ConfigFile found in '{configFile}'");
        }

        var text = File.ReadAllText(configFile);
        return JsonSerializer.Deserialize<GUnitConfig>(text)
            ?? throw new NullReferenceException("Config file is corrupted");
    }
}