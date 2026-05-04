using System.Reflection;
using System.Threading.Tasks;
using Godot;

namespace GUnit.Library.Base;

public abstract class BaseTest
{
    protected SceneTree Tree;
    protected Node Root;

    public async Task RunMethod(SceneTree tree, MethodInfo method)
    {   
        Tree = tree;
        Root = new Node();
        tree.Root.AddChild(Root);

        try
        {
            await Setup();

            var result = method.Invoke(this, null);
            
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

    protected async Task WaitForFrame()
    {
        await Tree.ToSignal(Tree, SceneTree.SignalName.ProcessFrame);
    }

    protected virtual Task Setup() => Task.CompletedTask;
    protected virtual Task Teardown() => Task.CompletedTask;
}