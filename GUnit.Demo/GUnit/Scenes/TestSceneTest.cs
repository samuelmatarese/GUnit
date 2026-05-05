using System.Threading.Tasks;
using Godot;
using GUnit.Library.Assertions;
using GUnit.Library.Attributes;
using GUnit.Library.Base;

namespace GUnit.Demo.Gunit.Scenes;

public class TestSceneTest : BaseTest
{
    [Test]
    public async Task _Input()
    {
        // arrange
        var testScene = new TestScene();
        Root.AddChild(testScene);
        await WaitForFrame();

        // act
        Input.ParseInputEvent(new InputEventMouseMotion());
        await WaitForFrame();

        // assert
        Assert.OfType<InputEventMouseMotion>(testScene.RegisteredInput);
    }
}