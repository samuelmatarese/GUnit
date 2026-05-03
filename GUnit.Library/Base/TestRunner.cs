using System;
using Godot;
using GUnit.Library.Base;

internal partial class TestRunner : SceneTree
{
    public override async void _Initialize()
    {
        var engine = new TestEngine(this);
        var result = await engine.RunAll();

        if (result.Failed > 0)
        {
            foreach(var error in result.Errors)
            {
                Console.WriteLine(error.Message);
                Console.WriteLine(error.StackTrace);
            }

            throw new Exception(result.Failed + " tests did not pass");
        }

        Quit();
    }
}