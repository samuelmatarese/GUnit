using System;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using GUnit.Library.Base.Root;

namespace GUnit.Library.Base;

public abstract class BaseTest
{
    protected SceneTree? Tree;
    protected RootViewport? Root;

    public async Task RunMethod(SceneTree tree, MethodInfo method, object[]? parameters = null)
    {   
        Tree = tree;
        Root = new();
        tree.Root.AddChild(Root);
        await WaitForFrame();
        
        try
        {
            await Setup();

            var result = method.Invoke(this, parameters);
            
            if (result is Task task)
            {
                await task;
            }

           await Teardown();
        }
        finally
        {
           Root.QueueFree();
           await Task.Yield();
        }
    }

    protected async Task WaitForFrame(int frameAmount = 1)
    {
        if(Tree == null)
        {
            throw new NullReferenceException(nameof(Tree));    
        }

        for(var i = 0; i < frameAmount; i++ )
        {
            await Tree.ToSignal(Tree, SceneTree.SignalName.ProcessFrame);
        }
    }

    protected async Task WaitForPhysicsProcess()
    {
        if(Tree == null)
        {
            throw new NullReferenceException(nameof(Tree));    
        }

        await Tree.ToSignal(Tree, SceneTree.SignalName.PhysicsFrame);
    }

    protected virtual Task Setup() => Task.CompletedTask;
    protected virtual Task Teardown() => Task.CompletedTask;
}