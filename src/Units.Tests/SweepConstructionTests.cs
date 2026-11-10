using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Spells;
using Aow.Game.World;
using Aow.Game.World.Map;

namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed class SweepConstructionTests
{
    private sealed class SweepOwner : EngineFixture.Owner
    {

        internal readonly List<string> Observations = [];

        public override int AddChild(TEObject child)
        {
            base.AddChild(child);
            Observations.Add(Snapshot(child));
            return Members.Count - 1;
        }

        public override int RemoveChild(TEObject child) { return base.RemoveChild(child) < 0 ? -1 : 0; }
    }

    private sealed class Fixture : IDisposable
    {
        private readonly int previousSeed = Runtime.RandSeed;
        private readonly TAoWHSSet previousSet = Aow.Game.Engine.TAoWHSSet.AoWHSSet;
        private readonly TAoWEngine previousEngine = Aow.Game.Magic.Abilities.GameTables.AoWEngine;
        private readonly TAoWHSMap previousMap = Aow.Game.World.Map.TAoWHSMap.AoWHSMap;
        internal readonly TImageLibraryManager Manager = new TImageLibraryManager(null!);
        internal readonly TMapObjectResourceList CombatResources = new TMapObjectResourceList(null!);

        internal Fixture()
        {
            Runtime.RandSeed = 123456789;
            var set = EngineFixture.Configure(EngineFixture.CreateGameSet(Manager), value => { value.FCombatResources = CombatResources; });
            Aow.Game.Engine.TAoWHSSet.AoWHSSet = set;
            Aow.Game.Magic.Abilities.GameTables.AoWEngine = EngineFixture.Engine;
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap = null!;
            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitGFXResources = null!;
            Runtime.RandSeed = 123456789;
        }

        public void Dispose()
        {
            Runtime.RandSeed = previousSeed;

            Aow.Game.Engine.TAoWHSSet.AoWHSSet = previousSet;
            Aow.Game.Magic.Abilities.GameTables.AoWEngine = previousEngine;
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap = previousMap;
            CombatResources.Dispose();
            Manager.Dispose();
        }
    }

    private static string Snapshot(object value)
    {
        var visited = new Dictionary<object, string>(ReferenceEqualityComparer.Instance);
        object? State(object? item, string path)
        {
            if (item is null) return null;
            if (item is TAoWEngine) return "$engine";
            Type type = item.GetType();
            if (item is string text) return text;
            if (type.IsEnum) return Convert.ToInt64(item);
            if (type.IsPrimitive || item is string || item is decimal) return item;
            if (item is Type represented) return represented.FullName;
            if (item is Delegate handler) return handler.GetInvocationList().Select(x => x.Method.DeclaringType!.FullName + "." + x.Method.Name).ToArray();
            if (!type.IsValueType)
            {
                if (visited.TryGetValue(item, out string? found)) return "$ref:" + found;
                visited.Add(item, path);
            }
            if (item is Array array) return array.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            if (item is IList list) return list.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            var fields = new SortedDictionary<string, object?>(StringComparer.Ordinal) { ["$type"] = type.FullName };
            for (Type? current = type; current is not null && current != typeof(object) && current != typeof(object); current = current.BaseType)
                foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(x => x.Name, StringComparer.Ordinal))
                {
                    if (field.Name == "captureOrdinal") continue;
                    if (field.Name == "eclass") continue; // the descriptor cache is not constructor state
                    string key = current.Name + "." + field.Name;
                    object? child = field.GetValue(item);
                    if (field.Name == "FOwner") { fields[key] = child is null ? null : "$owner"; continue; }
                    fields[key] = State(child, path + "." + key);
                }
            return fields;
        }
        return JsonSerializer.Serialize(State(value, "$"));
    }

    private static readonly string[] Names =
    [
        "Aow.Game.World.Structures.TAirNode",
        "Aow.Game.World.Structures.TAirNodeResource",
        "Aow.Game.World.Structures.TAltarOfDeath",
        "Aow.Game.World.Structures.TAltarOfDeathResource",
        "Aow.Game.World.Structures.TAltarOfFire",
        "Aow.Game.World.Structures.TAltarOfFireResource",
        "Aow.Game.World.Structures.TAltarOfIce",
        "Aow.Game.World.Structures.TAltarOfIceResource",
        "Aow.Game.World.Structures.TAltarOfLife",
        "Aow.Game.World.Structures.TAltarOfLifeResource",
        "Aow.Game.World.Structures.TAltarOfLightning",
        "Aow.Game.World.Structures.TAltarOfLightningResource",
        "Aow.Game.World.Structures.TAltarOfPestilence",
        "Aow.Game.World.Structures.TAltarOfPestilenceResource",
        "Aow.Game.World.Structures.TAltar",
        "Aow.Game.World.Structures.TAltarResource",
        "Aow.Game.Draw.Animation.TAnimation",
        "Aow.Game.Commands.TAnimationTE",
        "Aow.Game.Draw.Animation.TImageSequenceAnimation",
        "Aow.Game.Draw.Animation.TTwinkleAnimation",
        "Aow.Game.World.Hexes.TAoWBorderHexagon",
        "Aow.Game.World.Hexes.TAoWHexagon",
        "Aow.Game.World.Hexes.TAoWHexagonResource",
        "Aow.Game.World.Hexes.TAoWWaterHexagon",
        "Aow.Game.World.Hexes.TAoWWaterHexagonResource",
        "Aow.Game.World.Hexes.TAoWRoad",
        "Aow.Game.Combat.TAoWCombatMapField",
        "Aow.Game.Combat.TAoWCombatMapLevel",
        "Aow.Game.Draw.TCBloodHS",
        "Aow.Game.Draw.TCCorpseHS",
        "Aow.Game.Draw.TCItemHS",
        "Aow.Game.Draw.TCOozeHS",
        "Aow.Game.Draw.TRangedPathHS",
        "Aow.Game.Draw.TTacticalCombatUnitHS",
        "Aow.Game.Draw.TUnitPositionHS",
        "Aow.Game.Draw.TfxHS",
        "Aow.Game.World.Hexes.TAoWTerrain",
        "Aow.Game.World.Hexes.TAoWTerrainResource",
        "Aow.Game.World.Structures.TArena",
        "Aow.Game.World.Structures.TArenaResource",
        "Aow.Game.Commands.TArenaTE",
        "Aow.Game.World.Structures.TBuildersGuild",
        "Aow.Game.World.Structures.TBuildersGuildResource",
        "Aow.Game.World.Structures.TBloodShrine",
        "Aow.Game.Draw.Animation.TBloodShrineAnimation",
        "Aow.Game.World.Structures.TBloodShrineResource",
        "Aow.Game.World.Hexes.TBorderHexagon",
        "Aow.Game.World.Hexes.TBorderHexagonResource",
        "Aow.Game.World.TBridge",
        "Aow.Game.World.TBridgeResource",
        "Aow.Game.World.Structures.TCave",
        "Aow.Game.World.Structures.TCaveResource",
        "Aow.Game.World.TCenterViewLocation",
        "Aow.Game.World.TCenterViewLocationResource",
        "Aow.Game.World.Structures.TCity",
        "Aow.Game.Draw.TCityOverlayHS",
        "Aow.Game.World.Structures.TCityResource",
        "Aow.Game.World.Terrain.TCityWall",
        "Aow.Game.World.Terrain.TCityWallDoor",
        "Aow.Game.World.Terrain.TCityWallDoorResource",
        "Aow.Game.World.Terrain.TCityWallResource",
        "Aow.Game.Draw.TCombatFire",
        "Aow.Game.Draw.TCombatFireResource",
        "Aow.Game.World.Hexes.TCombatHexagon",
        "Aow.Game.World.Hexes.TCombatHexagonResource",
        "Aow.Game.World.Hexes.TWaterCombatHexagon",
        "Aow.Game.World.Hexes.TWaterCombatHexagonResource",
        "Aow.Game.World.Terrain.TCombatFixedILTerrain",
        "Aow.Game.World.Terrain.TCombatFixedILTerrainResource",
        "Aow.Game.World.Terrain.TCombatILTerrain",
        "Aow.Game.World.Terrain.TCombatILTerrainResource",
        "Aow.Game.Draw.TCombatILTerrainShadow",
        "Aow.Game.World.TCombatObstacle",
        "Aow.Game.Draw.TCombatObstacleOverlay",
        "Aow.Game.World.TCombatObstacleResource",
        "Aow.Game.Draw.TCombatObstacleShadow",
        "Aow.Game.World.Hexes.TCombatRoad",
        "Aow.Game.World.Hexes.TCombatRoadResource",
        "Aow.Game.World.Terrain.TCombatStructure",
        "Aow.Game.World.Terrain.TCombatStructureResource",
        "Aow.Game.Commands.TCAbRangedTE",
        "Aow.Game.Commands.TCAbTouchMoveTE",
        "Aow.Game.Commands.TCEndCombatTE",
        "Aow.Game.Commands.TCEndTurnTE",
        "Aow.Game.Commands.TCMeleeMoveTE",
        "Aow.Game.Commands.TCombatMoveTE",
        "Aow.Game.Commands.TTerminateCombatTE",
        "Aow.Game.World.Hexes.TCombatTerrain",
        "Aow.Game.World.Hexes.TCombatTerrainResource",
        "Aow.Game.Draw.TCombatTerrainShadow",
        "Aow.Game.World.Terrain.TCrop",
        "Aow.Game.World.Terrain.TCropResource",
        "Aow.Game.World.Structures.TPlayerCropStructure",
        "Aow.Game.World.Structures.TPlayerCropStructureResource",
        "Aow.Game.World.Structures.TCrypt",
        "Aow.Game.World.Structures.TCryptResource",
        "Aow.Game.World.Structures.TDeathNode",
        "Aow.Game.World.Structures.TDeathNodeResource",
        "Aow.Game.World.Structures.TDungeon",
        "Aow.Game.World.Structures.TDungeonResource",
        "Aow.Game.World.Structures.TEarthNode",
        "Aow.Game.World.Structures.TEarthNodeResource",
        "Aow.Game.World.Structures.TExplorationSite",
        "Aow.Game.World.Structures.TExplorationSiteResource",
        "Aow.Game.World.Structures.TFarm",
        "Aow.Game.World.Structures.TFarmResource",
        "Aow.Game.World.Structures.TFireNode",
        "Aow.Game.World.Structures.TFireNodeResource",
        "Aow.Game.World.TFlagEvent",
        "Aow.Game.World.TFlagEventResource",
        "Aow.Game.World.TFlagMessageEvent",
        "Aow.Game.World.TFlagMessageEventResource",
        "Aow.Game.World.TTutorialMessageEvent",
        "Aow.Game.World.TTutorialMessageEventResource",
        "Aow.Game.World.TFleeLocation",
        "Aow.Game.World.TFleeLocationResource",
        "Aow.Game.Draw.TMovementHS",
        "Aow.Game.World.Terrain.TGems",
        "Aow.Game.World.Terrain.TGemsResource",
        "Aow.Game.Draw.TLibraryHS",
        "Aow.Game.World.Hexes.THexagon",
        "Aow.Game.World.Hexes.THexagonResource",
        "Aow.Game.World.THillMO",
        "Aow.Game.World.THillMOR",
        "Aow.Game.World.Terrain.THolyGround",
        "Aow.Game.World.Terrain.THolyGroundResource",
        "Aow.Game.Magic.Spells.GlobalTarget.THolyGroundSpell",
        "Aow.Game.Commands.THolyGroundSpellTE",
        "Aow.Game.World.Terrain.THouse",
        "Aow.Game.World.Terrain.THouseResource",
        "Aow.Game.World.Hexes.THexagonBridge",
        "Aow.Game.World.Hexes.THexagonBridgeResource",
        "Aow.Game.World.Terrain.TFixedILTerrainMO",
        "Aow.Game.World.Terrain.TFixedILTerrainMOR",
        "Aow.Game.World.Terrain.TILTerrainMO",
        "Aow.Game.World.Terrain.TILTerrainMOR",
        "Aow.Game.World.Hexes.TIsometricCombatHexagon",
        "Aow.Game.World.Hexes.TIsometricCombatHexagonResource",
        "Aow.Game.World.Structures.TItemExplorationSite",
        "Aow.Game.World.Structures.TItemExplorationSiteResource",
        "Aow.Game.World.Structures.TLifeNode",
        "Aow.Game.World.Structures.TLifeNodeResource",
        "Aow.Game.World.Structures.TMagicSanctuary",
        "Aow.Game.Draw.TMagicSanctuaryOverlayHS",
        "Aow.Game.World.Structures.TMagicSanctuaryResource",
        "Aow.Game.World.Terrain.TMainStructure",
        "Aow.Game.World.Terrain.TMainStructureResource",
        "Aow.Game.World.Terrain.TManaCrystals",
        "Aow.Game.World.Terrain.TManaCrystalsResource",
        "Aow.Game.World.Structures.TMerchant",
        "Aow.Game.World.Structures.TMerchantResource",
        "Aow.Game.World.Structures.TMine",
        "Aow.Game.World.Structures.TMineResource",
        "Aow.Game.World.Structures.TMonsterlair",
        "Aow.Game.World.Structures.TMonsterlairResource",
        "Aow.Game.World.TMountainMO",
        "Aow.Game.World.TMountainMOR",
        "Aow.Game.Magic.Spells.GlobalTarget.TRaiseTerrain",
        "Aow.Game.Commands.TRaiseTerrainTE",
        "Aow.Game.World.Terrain.TRaisedMountain",
        "Aow.Game.World.Terrain.TRaisedMountainResource",
        "Aow.Game.World.Movement.TChangeSongMoveOnEvent",
        "Aow.Game.World.Movement.TChangeSongMoveOnEventResource",
        "Aow.Game.World.Movement.TMoveOnEvent",
        "Aow.Game.World.Movement.TMoveOnEventResource",
        "Aow.Game.World.Movement.TPlaySampleMoveOnEvent",
        "Aow.Game.World.Movement.TPlaySampleMoveOnEventResource",
        "Aow.Game.World.Movement.TShowMessageMoveOnEvent",
        "Aow.Game.World.Movement.TShowMessageMoveOnEventResource",
        "Aow.Game.World.Terrain.TOverlayObject",
        "Aow.Game.World.Terrain.TOverlayObjectResource",
        "Aow.Game.Draw.TOverviewUnitHS",
        "Aow.Game.World.Terrain.TPad",
        "Aow.Game.World.Terrain.TPadResource",
        "Aow.Game.World.TPoisonCloud",
        "Aow.Game.World.TPoisonCloudResource",
        "Aow.Game.World.Terrain.TPoisonPlant",
        "Aow.Game.World.Terrain.TPoisonPlantResource",
        "Aow.Game.Magic.Spells.GlobalTarget.TPoisonPlantSpell",
        "Aow.Game.Commands.TPoisonPlantSpellTE",
        "Aow.Game.World.Structures.TPowerNode",
        "Aow.Game.World.Structures.TPowerNodeResource",
        "Aow.Game.World.Structures.TProductionPlace",
        "Aow.Game.World.Structures.TProductionPlaceResource",
        "Aow.Game.World.Structures.TPyramid",
        "Aow.Game.World.Structures.TPyramidResource",
        "Aow.Game.World.Structures.TRandomNode",
        "Aow.Game.World.Structures.TRandomNodeResource",
        "Aow.Game.World.Structures.TReflectingPool",
        "Aow.Game.World.Structures.TReflectingPoolResource",
        "Aow.Game.World.Hexes.TRndHexagonResource",
        "Aow.Game.World.Hexes.TRoad",
        "Aow.Game.World.Hexes.TRoadResource",
        "Aow.Game.World.Structures.TRuin",
        "Aow.Game.World.Structures.TRuinResource",
        "Aow.Game.World.Structures.TShipyard",
        "Aow.Game.World.Structures.TShipyardResource",
        "Aow.Game.World.Structures.TSign",
        "Aow.Game.World.Structures.TSignResource",
        "Aow.Game.Commands.TUpdateSignTE",
        "Aow.Game.World.Hexes.TSingleCombatHexagon",
        "Aow.Game.World.Hexes.TSingleCombatHexagonResource",
        "Aow.Game.World.TSmokeCloud",
        "Aow.Game.World.TSmokeCloudResource",
        "Aow.Game.World.Structures.TSpringOfLife",
        "Aow.Game.Draw.Animation.TSpringOfLifeAnimation",
        "Aow.Game.World.Structures.TSpringOfLifeResource",
        "Aow.Game.Draw.Animation.TBlastStorm",
        "Aow.Game.Draw.Animation.TDeathStorm",
        "Aow.Game.Draw.Animation.TDivineStorm",
        "Aow.Game.Draw.Animation.TFireStorm",
        "Aow.Game.Draw.Animation.TIceStorm",
        "Aow.Game.Draw.Animation.TLightningStorm",
        "Aow.Game.Draw.Animation.TPestilenceStorm",
        "Aow.Game.Draw.Animation.TStorm",
        "Aow.Game.Commands.TStormSpellTE",
        "Aow.Game.World.Structures.TTeleport",
        "Aow.Game.World.Structures.TTeleportResource",
        "Aow.Game.World.Hexes.TTerrainMO",
        "Aow.Game.World.Hexes.TTerrainMOR",
        "Aow.Game.World.Structures.TTower",
        "Aow.Game.World.Structures.TTowerResource",
        "Aow.Game.World.Terrain.TTownhall",
        "Aow.Game.World.Terrain.TTownhallResource",
        "Aow.Game.World.Terrain.TUnHolyGround",
        "Aow.Game.World.Terrain.TUnHolyGroundResource",
        "Aow.Game.Magic.Spells.GlobalTarget.TUnholyGroundSpell",
        "Aow.Game.Commands.TUnholyGroundSpellTE",
        "Aow.Game.World.Structures.TWaterNode",
        "Aow.Game.World.Structures.TWaterNodeResource",
        "Aow.Game.World.Structures.TWizardsTower",
        "Aow.Game.World.Structures.TWizardsTowerResource",
        "Aow.Game.World.Terrain.TWorkshop",
        "Aow.Game.World.Terrain.TWorkshopResource",
        "Aow.Game.World.TMultiHexMO",
        "Aow.Game.World.TMultiHexMOR",
        "Aow.Game.Draw.THexagonSprite",
        "Aow.Game.World.TMapObject",
        "Aow.Game.World.TMapObjectResource",
        "Aow.Game.Draw.TSingleHS",
        "Aow.Game.World.Map.TAbstractRoad",
        "Aow.Game.World.Map.TAbstractRoadResource",
        "Aow.Game.World.Map.TAbstractHexagonResource",
        "Aow.Game.World.Structures.TPlayerStructure",
        "Aow.Game.World.Structures.TPlayerStructureResource",
        "Aow.Game.World.Structures.TStructure",
        "Aow.Game.World.Structures.TStructureResource",
        "Aow.Game.Commands.TTokenEvent",
        "Aow.Game.Draw.TUnitHS",
        "Aow.Game.Draw.TItemHS",
        "Aow.Game.Commands.TMoveTE",
        "Aow.Game.Commands.TSpellTE",
        "Aow.Game.Commands.TStructureTE",
        "Aow.Game.World.Map.TAoWMapField",
        "Aow.Game.World.Map.TAoWMapLevel",
        "Aow.Game.World.Map.TAbstractBorderHexagonResource",
        "Aow.Game.World.Hexes.TIsometricHexagon",
        "Aow.Game.World.Hexes.TLowerIsometricHexagon",
        "Aow.Game.World.Hexes.TUpperIsometricHexagon",
        "Aow.Game.World.Hexes.TIsometricHexagonResource",
        "Aow.Game.World.Hexes.TLowerIsometricHexagonResource",
        "Aow.Game.World.Hexes.TUpperIsometricHexagonResource",
        "Aow.Game.Commands.TGlobalTargetSpellTE",
    ];

    public static IEnumerable<object[]> Classes => Names.Select(name => new object[] { name });

    private static object? Invoke(Func<object?> action)
    {
        try { return action(); }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }

    private static TEObject Direct(Type type, TECustomNode owner)
    {
        var constructor = type.GetConstructor([typeof(TECustomNode)]);
        if (constructor is not null) return (TEObject)Invoke(() => constructor.Invoke([owner]))!;
        var factory = type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, [typeof(TECustomNode)]);
        if (factory is not null) return (TEObject)Invoke(() => factory.Invoke(null, [owner]))!;
        return (TEObject)Invoke(() => Activator.CreateInstance(type, [owner]))!;
    }

    [Theory]
    [MemberData(nameof(Classes))]
    public void StateOwnerObservationAndTeardownMatchTheOriginal(string name)
    {
        Type type = typeof(TSpell).Assembly.GetType(name)!;

        for (int owned = 0; owned < 2; owned++)
            for (int route = 0; route < 4; route++)
            {
                using var fixture = new Fixture();
                var owner = owned == 0 ? null : new SweepOwner();
                TEObject? value = null;
                var states = new List<string>();
                try
                {
                    value = route == 1 ? EClass.Of(type).Create(owner!)
                        : route == 0 ? Direct(type, owner!)
                        : route == 2 ? EClass.Of(type).Create(owner!)
                        : Saved(type, owner!);
                    Assert.IsType(type, value);
                    Assert.Same(owner, value.Owner);
                    states.Add("success:" + Snapshot(value));
                }
                catch (System.Exception error) { states.Add("failure:" + error.GetType().FullName + ":" + error.Message); }
                finally
                {
                    try { value?.Dispose(); }
                    catch (System.Exception error) { states.Add("destroy:" + error.GetType().FullName + ":" + error.Message); }
                }
                states.Add("owner:" + (owner?.Members.Count ?? 0));
                if (owner is not null) states.Add(JsonSerializer.Serialize(owner.Observations));
                string actual = string.Join("\n", states);
                string key = name + ".owner" + owned;
                ConstructionSnapshot.Check("SweepConstruction", key, actual);
            }
    }

    private static TEObject Saved(Type type, TECustomNode owner)
    {
        if (Aow.Game.Engine.Nodes.EngineHost.Engines is null) Aow.Game.Engine.Nodes.EngineHost.Initialize();
        using var engine = new TEngine(null!);
        var descriptor = EClass.Of(type);
        engine.Options -= TSet<TEngineModeElement>.Of(Aow.Game.Engine.TEngine.eoVerifyClasses);
        try { engine.UnregisterEClasses([engine.GetEClass(descriptor.ClassID)]); }
        catch (System.Exception error) when (PortedExceptions.Is(error)) { }
        engine.RegisterEClasses([descriptor]);
        return engine.CreateEObject(descriptor.ClassID, owner);
    }

    [Theory]
    [InlineData("Aow.Game.World.Map.TAbstractAoWHSMapMouse")]
    [InlineData("Aow.Game.Combat.TAoWCombatMapMouse")]
    [InlineData("Aow.Game.Combat.TAoWNewCombatMapSettings")]
    [InlineData("Aow.Game.Combat.TCombatMoveControl")]
    public void HelperStateMatchesTheOriginalFactory(string name)
    {
        using var fixture = new Fixture();
        Type type = typeof(TSpell).Assembly.GetType(name)!;
        if (name.EndsWith(".TAoWNewCombatMapSettings", StringComparison.Ordinal))
            typeof(THSEngine).GetField("FHSSFileName", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(Aow.Game.Magic.Abilities.GameTables.AoWEngine, "");
        Type[] signature = name.EndsWith(".TCombatMoveControl", StringComparison.Ordinal) ? [typeof(THSMap)] : [];
        var value = (object)Invoke(() => type.GetConstructor(signature)!.Invoke(signature.Length == 0 ? [] : [null]))!;
        try
        {
            ConstructionSnapshot.Check("SweepConstruction", name, Snapshot(value));
        }
        finally { ((value ?? throw new NullReferenceException()) as IDisposable)?.Dispose(); }
    }

}
