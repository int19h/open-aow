using Aow.Engine;
using Aow.Formats.Tree;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Magic;
using Aow.Game.Session;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;

namespace Aow.Game.Tests;

// G5 guard rule (issue 335). A guard that tested StrategicMap.Current tests the map of the engine
// the object reaches, and an object with no reachable engine takes the original's no-map branch.
[Collection("Engine")]
[EngineState]
public sealed class WorldGuardTests
{
    [Fact]
    public void DetachedStructureDisposeSkipsDestroyedNotice()
    {
        StrategicMap map = new(null, holder: EngineFixture.Engine);
        using IDisposable scope = EngineFixture.UseMap(map);
        int notifiedCount = 0;
        map.StructureDestroyed += _ => notifiedCount++;
        Structure structure = new(null);
        Assert.Null(structure.GetEngine());

        structure.Dispose();

        Assert.Equal(0, notifiedCount);
    }

    [Fact]
    public void DetachedBorderHexLeavesExploredFlagsUnchanged()
    {
        StrategicMap map = new(null, holder: EngineFixture.Engine) { Day = 0 };
        using IDisposable scope = EngineFixture.UseMap(map);
        MapField field = new(null) { FieldFlags = MapField.ExploredFlags };
        BorderHex borderHex = new(null!);
        Assert.Null(borderHex.GetEngine());
        HexSpriteMapFieldInfo fieldInfo = new() { MapField = field, HexNumber = 0 };

        borderHex.UpdateMapField(in fieldInfo);

        Assert.Equal(MapField.ExploredFlags, field.FieldFlags);
    }

    [Fact]
    public void DetachedMapFieldIgnitionChanceSkipsFireMasteryBonus()
    {
        StrategicMap map = new(null, holder: EngineFixture.Engine);
        Assert.NotNull(map.GlobalMagicControl);
        map.GlobalMagicControl.ActiveEnchantments = GlobalEnchantmentTypes.FireMastery;
        using IDisposable scope = EngineFixture.UseMap(map);
        MapField field = new(null);
        field.SetTerrain(TerrainType.Grass);
        UnboundStructureResource structureResource = new()
        {
            ImageSequenceList = new ImageSequenceList(null, imageLibrary: null)
        };
        structureResource.TerrainList.SetTerrain(index: 0, value: 0);
        _ = structureResource.ImageSequenceList.Add(new ImageSequence(null));
        Structure structure = new(null);
        structure.SetResourceWithoutOverrides(structureResource);
        HexSpriteTerrainInfo terrainInfo = new() { MapField = field, HexNumber = 0, TerrainType = TerrainType.Water };
        structure.ChangeTerrain(in terrainInfo);
        _ = field.AddHexSprite(structure, hexNumber: 0);
        Assert.NotNull(field.FindNoneTransparentHexSprite(typeof(Structure)));
        Assert.Null(field.GetEngine());

        int chance = MapField.IgnitionChance(field);

        Assert.Equal(10, chance);
    }

    [Fact]
    public void DetachedGameMapSkipsCityRegistrarMapLoaded()
    {
        GameEngine otherEngine = new(null) { Registry = EngineFixture.Engine.Registry, Dictionary = EngineFixture.Engine.Dictionary };
        StrategicMap unslottedMap = new(null, holder: otherEngine);
        int countBefore = unslottedMap.AIPlayerActionManager.GetCount();
        StrategicMap? previousMap = StrategicMap.Current;
        StrategicMap.Current = unslottedMap;
        try
        {
            CityRegistrar.MapLoaded(unslottedMap);
            GameMap detachedMap = new(null);
            Assert.Null(detachedMap.GetEngine());
            CityRegistrar.MapLoaded(detachedMap);
        }
        finally
        {
            StrategicMap.Current = previousMap;
        }

        Assert.Equal(countBefore, unslottedMap.AIPlayerActionManager.GetCount());
    }

    [Fact]
    public void DetachedSignPersistSkipsTextSeen()
    {
        StrategicMap map = new(null, holder: EngineFixture.Engine) { ConnectionType = PlayerConnectionType.Local };
        using IDisposable scope = EngineFixture.UseMap(map);
        Sign detachedSign = new(null);
        detachedSign.TextSeen[0] = true;
        Assert.Null(detachedSign.GetEngine());

        ObjectNode written = TreeWriter.Write(detachedSign, factory: new EnginePersistableFactory(EngineFixture.Engine));

        Assert.Null(written.Find("TextSeen"));
    }

    [Fact]
    public void DetachedCombatObstacleCreatesShadowWhenGlobalShadowsAreOff()
    {
        StrategicMap map = CreateMapWithShadowsOff();
        using IDisposable scope = EngineFixture.UseMap(map);
        CombatObstacle.Resource resource = new(null!) { ImageSequenceList = CreateImageSequences(index: 40) };
        CombatObstacle obstacle = new(null!);
        obstacle.SetResourceWithoutOverrides(resource);
        Assert.Null(obstacle.GetEngine());

        obstacle.CreateShadow();

        VisibilityList visible = new(null!);
        obstacle.MakeVisible(visible);
        Assert.True(ContainsChild<CombatObstacleShadow>(visible));
    }

    [Fact]
    public void DetachedCombatObstacleCreatesOverlayWhenGlobalShadowsAreOff()
    {
        StrategicMap map = CreateMapWithShadowsOff();
        using IDisposable scope = EngineFixture.UseMap(map);
        CombatObstacle.Resource resource = new(null!) { ImageSequenceList = CreateImageSequences(index: 41) };
        CombatObstacle obstacle = new(null!);
        obstacle.SetResourceWithoutOverrides(resource);
        Assert.Null(obstacle.GetEngine());

        obstacle.CreateOverlay();

        VisibilityList visible = new(null!);
        obstacle.MakeVisible(visible);
        Assert.True(ContainsChild<CombatObstacleOverlay>(visible));
    }

    [Fact]
    public void DetachedCombatTerrainCreatesShadowWhenGlobalShadowsAreOff()
    {
        StrategicMap map = CreateMapWithShadowsOff();
        using IDisposable scope = EngineFixture.UseMap(map);
        CombatTerrain.Resource resource = new(null!) { ImageSequenceList = CreateImageSequences(index: 40) };
        CombatTerrain terrain = new(null!);
        terrain.SetResourceWithoutOverrides(resource);
        Assert.Null(terrain.GetEngine());

        terrain.CreateShadow();

        VisibilityList visible = new(null!);
        terrain.MakeVisible(visible);
        Assert.True(ContainsChild<CombatTerrainShadow>(visible));
    }

    [Fact]
    public void DetachedCombatImageLibraryTerrainCreatesShadowWhenGlobalShadowsAreOff()
    {
        StrategicMap map = CreateMapWithShadowsOff();
        using IDisposable scope = EngineFixture.UseMap(map);
        CombatImageLibraryTerrainResource resource = new(null!) { ImageSequenceList = CreateImageSequences(index: 40) };
        CombatImageLibraryTerrain terrain = new(null);
        terrain.SetResourceWithoutOverrides(resource);
        Assert.Null(terrain.GetEngine());

        terrain.CreateShadow();

        VisibilityList visible = new(null!);
        terrain.MakeVisible(visible);
        Assert.True(ContainsChild<CombatImageLibraryTerrainShadow>(visible));
    }

    private static StrategicMap CreateMapWithShadowsOff()
    {
        ISettingsStore? previousSettings = RtlHost.Settings;
        RtlHost.Settings = new MemorySettingsStore();
        try
        {
            GameSettings settings = new();
            settings.SetDetailLevel(DetailLevel.Low);
            GameEngine engine = new(null) { Registry = settings, Dictionary = EngineFixture.Engine.Dictionary };
            StrategicMap map = new(null, holder: engine);
            Assert.False(map.ShowUnitShadows);
            return map;
        }
        finally
        {
            RtlHost.Settings = previousSettings;
        }
    }

    private static ImageSequenceList CreateImageSequences(int index)
    {
        ImageSequenceList sequences = new(null, imageLibrary: null);
        sequences.SetImageSequence(index, value: new ImageSequence(null));
        return sequences;
    }

    private static bool ContainsChild<T>(VisibilityList visible) where T : HexSprite
    {
        for (int i = 0; i < visible.GetCount(); i++)
        {
            if (visible[i] is T)
            {
                return true;
            }
        }

        return false;
    }

    private sealed class UnboundStructureResource : StructureResource
    {
        public UnboundStructureResource()
            : base(null) => TerrainBound = false;
    }
}
