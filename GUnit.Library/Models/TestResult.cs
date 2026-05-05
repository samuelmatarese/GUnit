using System;
using System.Collections.Generic;
using System.Text;

namespace GUnit.Library.Models;

public class TestResult
{
    public int Total {get; set;} = 0;
    public int Passed {get; set;} = 0;
    public int Failed {get; set;} = 0;
    public List<TestCase> TestCases {get; set;} = new();

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        foreach(var testCase in TestCases)
        {
            if(testCase.EncounteredException != null)
            {
                var error = testCase.EncounteredException;
                stringBuilder.AppendLine($"❌ {GetMethodIdentifier(testCase)}");
                stringBuilder.AppendLine($"{error.GetType().Name}: {error.Message}");
                stringBuilder.AppendLine(error.StackTrace);
            }
            else
            {
                stringBuilder.AppendLine($"✅ {GetMethodIdentifier(testCase)}");
            }
        }

        stringBuilder.AppendLine("------------------------------------------------------");
        stringBuilder.AppendLine($"Total: {Total}");
        stringBuilder.AppendLine($"✅Passed: {Passed}");
        stringBuilder.AppendLine($"❌ Failed: {Failed}");
        stringBuilder.AppendLine("------------------------------------------------------");

        return stringBuilder.ToString();
    }

    private string GetMethodIdentifier(TestCase testCase)
    {
        var parameterText = testCase.Parameters.Length > 0
            ? testCase.Parameters.Length > 1 
                ? "(" + string.Join(',', testCase.Parameters.Select(p => p.ToString())) + ")"
                : "(" + testCase.Parameters.First().ToString() + ")"
            : "";

        return $"{testCase.Method.Name} {parameterText}" ;
    }
}