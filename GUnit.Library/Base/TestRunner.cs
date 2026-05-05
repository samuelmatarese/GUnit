using System;
using Godot;
using GUnit.Library.Base;
using System.Threading.Tasks;

internal partial class TestRunner : SceneTree
{
    public override async void _Initialize()
    {
        var engine = new TestEngine(this);
        await engine.RunAll();
        await Task.Delay(100);
        Quit();
    }
}