using Godot;

namespace GUnit.Library.Base.Root;

public partial class RootViewport : SubViewport
{
    public RootViewport()
    {
        HandleInputLocally = true;    
        PhysicsObjectPicking = true;
    }

    public void SimulateInput(InputEvent inputEvent)
    {
        PushInput(inputEvent);
    }
}