using Aow.Engine;

using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Grids;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class UiConstructionFailureTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void PlainComponentsPublishCompletedState(int family)
    {
        GameEngine engine = EngineFixture.Engine;
        var previousLoaded = engine.FOnHSMLoadedStore;
        var previousClose = engine.FOnHSMCloseStore;
        engine.FOnHSMLoadedStore = new EventStore<HSMap>();
        engine.FOnHSMCloseStore = new EventStore<HSMap>();
        GameTables.AoWEngine = engine;
        AoWHSMap.Current = null!;
        AoWCombatMap.Current = null!;
        var owner = new Component(null!);
        try
        {
            Component value = family switch
            {
                0 => new AoWMapControl(owner),
                1 => new AoWCombatMapControl(owner),
                2 => new HexagonSpriteEngine(owner),
                3 => new AgeOfWondersEngine(owner),
                4 => new InterfaceIL(owner),
                _ => new HSLibrary(owner)
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
            engine.FOnHSMLoadedStore = previousLoaded;
            engine.FOnHSMCloseStore = previousClose;
        }
    }
}
