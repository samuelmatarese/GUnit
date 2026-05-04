using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using GUnit.Library.Attributes;
using GUnit.Library.Models;

namespace GUnit.Library.Base;

public class TestEngine(SceneTree tree)
{

    public async Task<TestResult> RunAll()
    {
        var result = new TestResult();
        var tests = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(BaseTest).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var type in tests)
        {
            var testInstance = (BaseTest)Activator.CreateInstance(type);
            var methods = type.GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(TestAttribute), false).Any());

            foreach (var method in methods)
            {
                result.Total++;

                try
                {
                    await testInstance.RunMethod(tree, method);
                    result.Passed++;
                }
                catch (Exception e)
                {
                    var innerException = e is TargetInvocationException tie && tie.InnerException != null
                        ? tie.InnerException
                        : e;
                        
                    result.Failed++;
                    result.Errors.Add(innerException);
                }   
            }
        }

        Console.WriteLine(result.ToString());
        return result;
    }
}