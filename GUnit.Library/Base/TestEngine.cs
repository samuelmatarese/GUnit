using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using GUnit.Library.Attributes;
using GUnit.Library.Models;

namespace GUnit.Library.Base;

public class TestEngine(SceneTree tree)
{

    public async Task RunAll()
    {
        var result = new TestResult();
        var tests = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(BaseTest).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var type in tests)
        {
            var testInstance = (BaseTest)Activator.CreateInstance(type);
            var testCases = type.GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(TestAttribute), false).Any())
                .Select(m => new TestCase(m))
                .Concat(ConvertTheoriesToNormalTests(type));

            foreach (var testCase in testCases)
            {
                result.Total++;

                try
                {
                    await testInstance.RunMethod(tree, testCase.Method, testCase.Parameters);
                    result.Passed++;
                }
                catch (Exception e)
                {
                    var innerException = e is TargetInvocationException tie && tie.InnerException != null
                        ? tie.InnerException
                        : e;
                        
                    result.Failed++;
                    testCase.EncounteredException = innerException;
                }   

                result.TestCases.Add(testCase);
            }
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "GUnit/gunit-test-result.txt");
        await File.WriteAllTextAsync(filePath, result.ToString());
        throw new Exception(filePath);
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
}