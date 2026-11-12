using Aow.Engine;

using Aow.Game.Combat;
using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Grids;
using Aow.Game.World.Map;

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
        var previousLoaded = engine._onHsmLoadedStore;
        var previousClose = engine._onHsmCloseStore;
        engine._onHsmLoadedStore = new EventStore<HSMap>();
        engine._onHsmCloseStore = new EventStore<HSMap>();
        GameTables.GameEngine = engine;
        AoWHSMap.Current = null!;
        CombatMap.Current = null!;
        var owner = new Component(null!);
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
            engine._onHsmLoadedStore = previousLoaded;
            engine._onHsmCloseStore = previousClose;
        }
    }
}
