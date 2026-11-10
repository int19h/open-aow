using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Ui.Grids;
using Aow.Game.World.Map;

namespace Aow.Units.Tests;

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
        var engine = EngineFixture.Engine;
        var previousLoaded = engine.FOnHSMLoadedStore;
        var previousClose = engine.FOnHSMCloseStore;
        engine.FOnHSMLoadedStore = new EventStore<THSMap>();
        engine.FOnHSMCloseStore = new EventStore<THSMap>();
        Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine = engine;
        Aow.Game.Magic.Abilities.AoWEUnit.AoWHSMap = null!;
        Aow.Game.Combat.AoWTCUnit.AoWCombatMap = null!;
        var owner = new TComponent(null!);
        try
        {
            TComponent value = family switch
            {
                0 => new TAoWMapControl(owner),
                1 => new TAoWCombatMapControl(owner),
                2 => new THexagonSpriteEngine(owner),
                3 => new TAgeOfWondersEngine(owner),
                4 => new TInterfaceIL(owner),
                _ => new THSLibrary(owner)
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
