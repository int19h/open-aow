using Aow.Game.Draw.Images;
using Aow.Game.UI.Grids;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class UIConstructionFailureTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void PlainComponentsPublishCompletedState(int family)
    {
        GameEngine engine = EngineFixture.Engine;
        Delegate? previousLoaded = EventHandlers.Swap(engine, declaringType: typeof(GameEngine), eventName: nameof(GameEngine.HexSpriteMapLoaded), handlers: null);
        Delegate? previousClose = EventHandlers.Swap(engine, declaringType: typeof(GameEngine), eventName: nameof(GameEngine.HexSpriteMapClosing), handlers: null);
        EngineFixture.SetMap(null);
        EngineFixture.SetCombatMap(null);
        Component owner = new(null);
        try
        {
            Component value = family switch
            {
                0 => new MapControl(owner),
                1 => new CombatMapControl(owner),
                _ => new InterfaceImageLibrary(owner)
            };
            Assert.Same(owner, value.Owner);
            Assert.Same(value, owner.Components[0]);
            Assert.Equal(1, owner.ComponentCount);
            value.Dispose();
            Assert.Equal(0, owner.ComponentCount);
        }
        finally
        {
            owner.Dispose();
            _ = EventHandlers.Swap(engine, declaringType: typeof(GameEngine), eventName: nameof(GameEngine.HexSpriteMapLoaded), handlers: previousLoaded);
            _ = EventHandlers.Swap(engine, declaringType: typeof(GameEngine), eventName: nameof(GameEngine.HexSpriteMapClosing), handlers: previousClose);
        }
    }
}
