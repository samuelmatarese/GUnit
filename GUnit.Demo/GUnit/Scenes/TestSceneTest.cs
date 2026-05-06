using System;
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
        Assert.NotNull(testScene.RegisteredInput);
        Assert.OfType<InputEventMouseMotion>(testScene.RegisteredInput);
    }

    [Theory]
    [SimpleData(typeof(InputEventAction))]
    [SimpleData(typeof(InputEventKey))]
    [SimpleData(typeof(InputEventMouseMotion))]
    public async Task _Input_Theory(Type inputType)
    {
        // arrange
        var testScene = new TestScene();
        Root.AddChild(testScene);
        await WaitForFrame();

        // act
        var inputEvent = (InputEvent)Activator.CreateInstance(inputType);
        Input.ParseInputEvent(inputEvent);
        await WaitForFrame();

        // assert
        Assert.OfType(inputType, testScene.RegisteredInput);
    }
}