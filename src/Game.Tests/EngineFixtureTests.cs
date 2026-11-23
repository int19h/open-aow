using Aow.Engine;
using Aow.Game.Combat;
using Aow.Game.Draw.Images;
using Aow.Game.World.Map;
using Xunit.Sdk;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class EngineFixtureTests
{
    [Fact]
    public void StartupAndTestServicesUseTheSameIsolatedRootAndGamePack()
    {
        Assert.Equal(EngineFixture.Root, RtlHost.Files.CurrentDirectory);
        Assert.Equal(Path.Combine(EngineFixture.Root, "AoW.exe"), LegacyApplication.Application.ExeName);
        Assert.Equal(EngineFixture.Root, EngineFixture.Engine.StartupDirectory.TrimEnd('\\', '/'));
        Assert.Same(EngineFixture.GamePack, EngineFixture.GlobalPack);
        _ = Assert.Throws<TrueException>(() => RtlHost.Files.FileExists(Path.Combine(Path.GetTempPath(), "outside-fixture")));
    }

    [Fact]
    public void SlotScopesSetAndRestoreBothRepresentations()
    {
        StrategicMap? previousMap = EngineFixture.Engine.StrategicMap;
        StrategicMap map = new(null, holder: EngineFixture.Engine);
        using (EngineFixture.UseMap(map))
        {
            Assert.Same(map, EngineFixture.Engine.StrategicMap);
            Assert.Same(map, EngineFixture.GlobalMap);
        }
        map.Dispose();
        Assert.Same(previousMap, EngineFixture.Engine.StrategicMap);
        Assert.Same(previousMap, EngineFixture.GlobalMap);

        GamePack previousPack = EngineFixture.Engine.GamePack;
        using (EngineFixture.UsePack(null))
        {
            Assert.Null(EngineFixture.Engine.GamePack);
            Assert.Null(EngineFixture.GlobalPack);
        }
        Assert.Same(previousPack, EngineFixture.Engine.GamePack);
        Assert.Same(previousPack, EngineFixture.GlobalPack);

        CombatMap? previousCombat = EngineFixture.Engine.CombatMap;
        using (EngineFixture.UseCombatMap(null))
        {
            CombatMap combat = new(null, holder: EngineFixture.Engine);
            Assert.Same(combat, EngineFixture.Engine.CombatMap);
            Assert.Same(combat, EngineFixture.GlobalCombatMap);
            combat.Dispose();
            Assert.Null(EngineFixture.Engine.CombatMap);
            Assert.Null(EngineFixture.GlobalCombatMap);
        }
        Assert.Same(previousCombat, EngineFixture.Engine.CombatMap);
        Assert.Same(previousCombat, EngineFixture.GlobalCombatMap);
    }

    [Fact]
    public void CombatMapSlotFollowsEveryConstructionRouteAndDisposalClearsOnlyThatMap()
    {
        static void Check(CombatMap map)
        {
            Assert.Same(map, EngineFixture.Engine.CombatMap);
            Assert.Same(map, EngineFixture.GlobalCombatMap);
            map.SetOwner(null);
            map.Dispose();
            Assert.Null(EngineFixture.Engine.CombatMap);
            Assert.Null(EngineFixture.GlobalCombatMap);
        }

        Check(Assert.IsType<CombatMap>(EngineClass.Of<CombatMap>().CreateInstance(null, holder: EngineFixture.Engine)));
        Check(new CombatMap(EngineFixture.Engine, holder: EngineFixture.Engine));
        Check(new CombatMap(null, holder: EngineFixture.Engine));

        CombatMap older = new(null, holder: EngineFixture.Engine);
        CombatMap newer = new(null, holder: EngineFixture.Engine);
        older.SetOwner(null);
        older.Dispose();
        Assert.Same(newer, EngineFixture.Engine.CombatMap);
        newer.SetOwner(null);
        newer.Dispose();
        Assert.Null(EngineFixture.Engine.CombatMap);
        Assert.Null(EngineFixture.GlobalCombatMap);
    }

    [Fact]
    public void ScopesRestoreDivergentValuesAfterAnException()
    {
        StrategicMap globalMap = new(null, holder: EngineFixture.Engine);
        StrategicMap slotMap = new(null, holder: EngineFixture.Engine);
        EngineFixture.SetMap(globalMap);
        EngineFixture.Engine.StrategicMap = slotMap;
        bool mapThrown;
        try
        {
            using (EngineFixture.UseMap(null))
            {
                Assert.Null(EngineFixture.Engine.StrategicMap);
                Assert.Null(EngineFixture.GlobalMap);
                throw new InvalidOperationException("map scope");
            }
        }
        catch (InvalidOperationException)
        {
            mapThrown = true;
        }
        Assert.True(mapThrown);
        Assert.Same(globalMap, EngineFixture.GlobalMap);
        Assert.Same(slotMap, EngineFixture.Engine.StrategicMap);
        EngineFixture.SetMap(null);
        globalMap.Dispose();
        slotMap.Dispose();

        GamePack originalPack = EngineFixture.GlobalPack!;
        ImageLibraryManager manager = new(null);
        GamePack slotPack = EngineFixture.CreateGamePack(manager);
        EngineFixture.SetPack(originalPack);
        EngineFixture.Engine.GamePack = slotPack;
        bool packThrown;
        try
        {
            using (EngineFixture.UsePack(null))
            {
                Assert.Null(EngineFixture.Engine.GamePack);
                Assert.Null(EngineFixture.GlobalPack);
                throw new InvalidOperationException("pack scope");
            }
        }
        catch (InvalidOperationException)
        {
            packThrown = true;
        }
        Assert.True(packThrown);
        Assert.Same(originalPack, EngineFixture.GlobalPack);
        Assert.Same(slotPack, EngineFixture.Engine.GamePack);
        EngineFixture.SetPack(originalPack);
        slotPack.Dispose();
        manager.Dispose();

        CombatMap globalCombat = new(null, holder: EngineFixture.Engine);
        EngineFixture.SetCombatMap(globalCombat);
        EngineFixture.Engine.CombatMap = null;
        bool combatThrown;
        try
        {
            using (EngineFixture.UseCombatMap(null))
            {
                Assert.Null(EngineFixture.Engine.CombatMap);
                Assert.Null(EngineFixture.GlobalCombatMap);
                throw new InvalidOperationException("combat scope");
            }
        }
        catch (InvalidOperationException)
        {
            combatThrown = true;
        }
        Assert.True(combatThrown);
        Assert.Same(globalCombat, EngineFixture.GlobalCombatMap);
        Assert.Null(EngineFixture.Engine.CombatMap);
        globalCombat.SetOwner(null);
        globalCombat.Dispose();
        Assert.Null(EngineFixture.GlobalCombatMap);
        Assert.Null(EngineFixture.Engine.CombatMap);
    }
}
