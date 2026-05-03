using System;
using System.Collections.Generic;
using System.Text;

namespace GUnit.Library.Models;

public class TestResult
{
    public int Total {get; set;} = 0;
    public int Passed {get; set;} = 0;
    public int Failed {get; set;} = 0;
    public List<Exception> Errors {get; set;} = new();

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        foreach(var error in Errors)
        {
            stringBuilder.AppendLine($"❌ {error.GetType().Name}: {error.Message}");
            stringBuilder.AppendLine(error.StackTrace);
        }

        stringBuilder.AppendLine("------------------------------------------------------");
        stringBuilder.AppendLine($"Total: {Total}");
        stringBuilder.AppendLine($"✅Passed: {Passed}");
        stringBuilder.AppendLine($"❌ Failed: {Failed}");
        stringBuilder.AppendLine("------------------------------------------------------");

        return stringBuilder.ToString();
    }
}