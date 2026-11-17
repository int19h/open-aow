using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;

namespace Aow.Game.Tests;

// A factory owner of the wrong class, an Assign of another class, and a control whose owner is
// not a window still fail.
[Collection("Engine")]
[EngineState]
public sealed class KeptCastFailureTests
{
    [Fact]
    public void DynamicIsometricHexFactoryChecksItsOwner()
    {
        _ = Assert.Throws<NullReferenceException>(() => EngineClass.Of<DynamicIsometricHex>().CreateInstance(null, holder: EngineFixture.Engine));
        using OwningNode owner = new(null) { Holder = EngineFixture.Engine };
        _ = Assert.Throws<InvalidCastException>(() => EngineClass.Of<DynamicIsometricHex>().CreateInstance(owner, holder: EngineFixture.Engine));
    }

    [Fact]
    public void AssignRequiresTheSameClass()
    {
        using PlayerCollection players = new(null) { Holder = EngineFixture.Engine };
        using EngineObject other = new(null) { Holder = EngineFixture.Engine };
        _ = Assert.Throws<InvalidCastException>(() => players.Assign(other, engine: EngineFixture.Engine));
        _ = Assert.Throws<NullReferenceException>(() => players.Assign(null, engine: EngineFixture.Engine));
    }

    [Fact]
    public void ControlOwnerIsItsWindow()
    {
        WindowManager manager = new(null);
        Control control = new(null);
        try
        {
            _ = Assert.Throws<InvalidCastException>(() => control.SetOwner(manager));
            control.SetOwner(null);
            using BaseWindow window = new(null);
            control.SetOwner(window);
            control.SetOwner(null);
        }
        finally
        {
            control.SetOwner(null);
            control.Dispose();
            manager.Dispose();
        }
    }
}
