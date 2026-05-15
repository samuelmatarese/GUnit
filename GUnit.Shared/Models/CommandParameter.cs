using System.Text.Json;
using GUnit.Shared.Constants;
using System.Collections.Generic;

namespace GUnit.Shared.Models;

public class CommandParameter(string identifier, string? value = null)
{
    private static readonly string ArgumentIdentifierPrefix = "--";

    public string Identifier {get; set;} = identifier;
    public string? Value {get; set;} = value;

    public static List<CommandParameter> ConvertToParameters(List<string> args)
    {
        var commandParameters = new List<CommandParameter>();

        for(var i = 0; i < args.Count; i++)
        {
            var arg = args[i];

            if (arg.StartsWith(ArgumentIdentifierPrefix))
            {
                var commandParameter = new CommandParameter(arg);
            
                if(args.Count > i + 1 && !args[i + 1].StartsWith(ArgumentIdentifierPrefix))
                {
                    commandParameter.Value = args[i + 1];
                    i++;      
                }

                commandParameters.Add(commandParameter);
            }
            else
            {
                throw new InvalidOperationException($"The argument '{arg}' was not valid");
            }
        }

        return commandParameters;
    }

    public static List<CommandParameter> GetFromEnvironment()
    {
        var parameterText = Environment.GetEnvironmentVariable(ParameterNames.GUnitParameters);
        
        if (string.IsNullOrEmpty(parameterText))
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<CommandParameter>>(parameterText)
            ?? [];
    }
}