using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using GUnit.Library.Attributes;
using GUnit.Library.Models;
using GUnit.Shared.Constants;
using GUnit.Shared.Models;

namespace GUnit.Library.Base;

public class TestEngine(SceneTree tree)
{

    public async Task<bool> RunAll()
    {
        var result = new TestResult();
        var testTasks = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(BaseTest).IsAssignableFrom(t) && !t.IsAbstract)
            .Select(CreateParallelTasks)
            .ToList();

        await Task.WhenAll(testTasks);

        foreach(var testTask in testTasks)
        {
            var (Failed, Passed, Cases) = await testTask;
            result.Failed += Failed;
            result.Passed += Passed;
            result.Total += Failed + Passed;
            result.TestCases.AddRange(Cases);
        }

        Console.WriteLine(result.ToString());
        return result.Failed == 0;
    }

    private async Task<(int Failed, int Passed, List<TestCase> Cases)> CreateParallelTasks(Type testClass)
    {
        var failed = 0;
        var passed = 0;
        var testInstance = (BaseTest)Activator.CreateInstance(testClass)!;
        var filter = GetTestFilter();
        var testCases = testClass.GetMethods()
            .Where(m => m.GetCustomAttributes(typeof(TestAttribute), false).Any())
            .Select(m => new TestCase(m))
            .Concat(ConvertTheoriesToNormalTests(testClass))
            .Where(t => $"{t.Method.DeclaringType?.FullName}.{t.Method.Name}".Contains(filter ?? ""))
            .ToList();

        foreach (var testCase in testCases)
        {
            try
            {
                await testInstance.RunMethod(tree, testCase.Method, testCase.Parameters);
                passed++;
            }
            catch (Exception e)
            {
                var innerException = e is TargetInvocationException tie && tie.InnerException != null
                    ? tie.InnerException
                    : e;
                        
                failed++;
                testCase.EncounteredException = innerException;
            }   
        }

        return (failed, passed, testCases);
    }

    private IEnumerable<TestCase> ConvertTheoriesToNormalTests(Type classType)
    {
        var tests = new List<TestCase>();
        var theories = classType.GetMethods()
            .Where(m => m.GetCustomAttributes(typeof(TheoryAttribute), false).Any());

        foreach(var theory in theories)
        {
            var dataAttributes = theory.GetCustomAttributes<SimpleDataAttribute>();

            foreach (var data in dataAttributes)
            {
                if (theory.GetParameters().Length != data.Data.Length)
                    throw new Exception("Parameter count mismatch");

                tests.Add(new TestCase(theory, data.Data));
            }
        }

        return tests;
    }

    private string? GetTestFilter()
    {
        return CommandParameter.GetFromEnvironment()
            .FirstOrDefault(c => c.Identifier == ParameterNames.Filter)
            ?.Value;
    }
}