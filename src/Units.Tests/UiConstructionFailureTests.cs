using Aow.Units.AoWE;
using Aow.Units.AOWInterfaceImgLib;
using Aow.Units.CMapCtrl;
using Aow.Units.HSEngine;
using Aow.Units.HSLib;
using Aow.Units.MapCtrl;
using Game = Aow.Units.AoWE.AoWEUnit;

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
        var previousLoaded = engine.FOnHSMLoaded;
        var previousClose = engine.FOnHSMClose;
        engine.FOnHSMLoaded = new Aow.Units.Engine.TEventList();
        engine.FOnHSMClose = new Aow.Units.Engine.TEventList();
        Game.AoWEngine = engine;
        Game.AoWHSMap = null!;
        AoWTC.AoWTCUnit.AoWCombatMap = null!;
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
            value.Destroy();
            Assert.Equal(0, owner.ComponentCount);
        }
        finally
        {
            owner.Destroy();
            engine.FOnHSMLoaded.Destroy();
            engine.FOnHSMClose.Destroy();
            engine.FOnHSMLoaded = previousLoaded;
            engine.FOnHSMClose = previousClose;
        }
    }
}
