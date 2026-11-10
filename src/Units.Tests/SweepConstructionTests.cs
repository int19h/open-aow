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

        public override int AddChild(EObject child)
        {
            base.AddChild(child);
            Observations.Add(Snapshot(child));
            return Members.Count - 1;
        }

        public override int RemoveChild(EObject child) { return base.RemoveChild(child) < 0 ? -1 : 0; }
    }

    private sealed class Fixture : IDisposable
    {
        private readonly int previousSeed = Runtime.RandSeed;
        private readonly AoWHSSet previousSet = Aow.Game.Engine.AoWHSSet.Current;
        private readonly AoWEngine previousEngine = Aow.Game.Magic.Abilities.GameTables.AoWEngine;
        private readonly AoWHSMap previousMap = Aow.Game.World.Map.AoWHSMap.Current;
        internal readonly ImageLibraryManager Manager = new ImageLibraryManager(null!);
        internal readonly MapObjectResourceList CombatResources = new MapObjectResourceList(null!);

        internal Fixture()
        {
            Runtime.RandSeed = 123456789;
            var set = EngineFixture.Configure(EngineFixture.CreateGameSet(Manager), value => { value.FCombatResources = CombatResources; });
            Aow.Game.Engine.AoWHSSet.Current = set;
            Aow.Game.Magic.Abilities.GameTables.AoWEngine = EngineFixture.Engine;
            Aow.Game.World.Map.AoWHSMap.Current = null!;
            Aow.Game.Engine.AoWHSSet.Current.FUnitGFXResources = null!;
            Runtime.RandSeed = 123456789;
        }

        public void Dispose()
        {
            Runtime.RandSeed = previousSeed;

            Aow.Game.Engine.AoWHSSet.Current = previousSet;
            Aow.Game.Magic.Abilities.GameTables.AoWEngine = previousEngine;
            Aow.Game.World.Map.AoWHSMap.Current = previousMap;
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
            if (item is AoWEngine) return "$engine";
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
        "Aow.Game.World.Structures.AirNode",
        "Aow.Game.World.Structures.AirNode+Resource",
        "Aow.Game.World.Structures.AltarOfDeath",
        "Aow.Game.World.Structures.AltarOfDeath+Resource",
        "Aow.Game.World.Structures.AltarOfFire",
        "Aow.Game.World.Structures.AltarOfFire+Resource",
        "Aow.Game.World.Structures.AltarOfIce",
        "Aow.Game.World.Structures.AltarOfIce+Resource",
        "Aow.Game.World.Structures.AltarOfLife",
        "Aow.Game.World.Structures.AltarOfLife+Resource",
        "Aow.Game.World.Structures.AltarOfLightning",
        "Aow.Game.World.Structures.AltarOfLightning+Resource",
        "Aow.Game.World.Structures.AltarOfPestilence",
        "Aow.Game.World.Structures.AltarOfPestilence+Resource",
        "Aow.Game.World.Structures.Altar",
        "Aow.Game.World.Structures.AltarResource",
        "Aow.Game.Draw.Animation.Animation",
        "Aow.Game.Commands.AnimationTE",
        "Aow.Game.Draw.Animation.ImageSequenceAnimation",
        "Aow.Game.Draw.Animation.TwinkleAnimation",
        "Aow.Game.World.Hexes.AoWBorderHexagon",
        "Aow.Game.World.Hexes.AoWHexagon",
        "Aow.Game.World.Hexes.AoWHexagon+Resource",
        "Aow.Game.World.Hexes.AoWWaterHexagon",
        "Aow.Game.World.Hexes.AoWWaterHexagon+Resource",
        "Aow.Game.World.Hexes.AoWRoad",
        "Aow.Game.Combat.AoWCombatMapField",
        "Aow.Game.Combat.AoWCombatMapLevel",
        "Aow.Game.Draw.CBloodHS",
        "Aow.Game.Draw.CCorpseHS",
        "Aow.Game.Draw.CItemHS",
        "Aow.Game.Draw.COozeHS",
        "Aow.Game.Draw.RangedPathHS",
        "Aow.Game.Draw.TacticalCombatUnitHS",
        "Aow.Game.Draw.UnitPositionHS",
        "Aow.Game.Draw.TfxHS",
        "Aow.Game.World.Hexes.AoWTerrain",
        "Aow.Game.World.Hexes.AoWTerrain+Resource",
        "Aow.Game.World.Structures.Arena",
        "Aow.Game.World.Structures.Arena+Resource",
        "Aow.Game.Commands.ArenaTE",
        "Aow.Game.World.Structures.BuildersGuild",
        "Aow.Game.World.Structures.BuildersGuild+Resource",
        "Aow.Game.World.Structures.BloodShrine",
        "Aow.Game.Draw.Animation.BloodShrineAnimation",
        "Aow.Game.World.Structures.BloodShrine+Resource",
        "Aow.Game.World.Hexes.BorderHexagon",
        "Aow.Game.World.Hexes.BorderHexagon+Resource",
        "Aow.Game.World.Bridge",
        "Aow.Game.World.Bridge+Resource",
        "Aow.Game.World.Structures.Cave",
        "Aow.Game.World.Structures.Cave+Resource",
        "Aow.Game.World.CenterViewLocation",
        "Aow.Game.World.CenterViewLocation+Resource",
        "Aow.Game.World.Structures.City",
        "Aow.Game.Draw.CityOverlayHS",
        "Aow.Game.World.Structures.Resource",
        "Aow.Game.World.Terrain.CityWall",
        "Aow.Game.World.Terrain.CityWallDoor",
        "Aow.Game.World.Terrain.CityWallDoor+Resource",
        "Aow.Game.World.Terrain.CityWall+Resource",
        "Aow.Game.Draw.CombatFire",
        "Aow.Game.Draw.CombatFire+Resource",
        "Aow.Game.World.Hexes.CombatHexagon",
        "Aow.Game.World.Hexes.CombatHexagon+Resource",
        "Aow.Game.World.Hexes.WaterCombatHexagon",
        "Aow.Game.World.Hexes.WaterCombatHexagon+Resource",
        "Aow.Game.World.Terrain.CombatFixedILTerrain",
        "Aow.Game.World.Terrain.CombatFixedILTerrain+Resource",
        "Aow.Game.World.Terrain.CombatILTerrain",
        "Aow.Game.World.Terrain.CombatILTerrainResource",
        "Aow.Game.Draw.CombatILTerrainShadow",
        "Aow.Game.World.CombatObstacle",
        "Aow.Game.Draw.CombatObstacleOverlay",
        "Aow.Game.World.CombatObstacle+Resource",
        "Aow.Game.Draw.CombatObstacleShadow",
        "Aow.Game.World.Hexes.CombatRoad",
        "Aow.Game.World.Hexes.CombatRoad+Resource",
        "Aow.Game.World.Terrain.CombatStructure",
        "Aow.Game.World.Terrain.CombatStructureResource",
        "Aow.Game.Commands.CAbRangedTE",
        "Aow.Game.Commands.CAbTouchMoveTE",
        "Aow.Game.Commands.CEndCombatTE",
        "Aow.Game.Commands.CEndTurnTE",
        "Aow.Game.Commands.CMeleeMoveTE",
        "Aow.Game.Commands.CombatMoveTE",
        "Aow.Game.Commands.TerminateCombatTE",
        "Aow.Game.World.Hexes.CombatTerrain",
        "Aow.Game.World.Hexes.CombatTerrain+Resource",
        "Aow.Game.Draw.CombatTerrainShadow",
        "Aow.Game.World.Terrain.Crop",
        "Aow.Game.World.Terrain.Crop+Resource",
        "Aow.Game.World.Structures.PlayerCropStructure",
        "Aow.Game.World.Structures.PlayerCropStructureResource",
        "Aow.Game.World.Structures.Crypt",
        "Aow.Game.World.Structures.Crypt+Resource",
        "Aow.Game.World.Structures.DeathNode",
        "Aow.Game.World.Structures.DeathNode+Resource",
        "Aow.Game.World.Structures.Dungeon",
        "Aow.Game.World.Structures.Dungeon+Resource",
        "Aow.Game.World.Structures.EarthNode",
        "Aow.Game.World.Structures.EarthNode+Resource",
        "Aow.Game.World.Structures.ExplorationSite",
        "Aow.Game.World.Structures.ExplorationSiteResource",
        "Aow.Game.World.Structures.Farm",
        "Aow.Game.World.Structures.Farm+Resource",
        "Aow.Game.World.Structures.FireNode",
        "Aow.Game.World.Structures.FireNode+Resource",
        "Aow.Game.World.FlagEvent",
        "Aow.Game.World.FlagEventResource",
        "Aow.Game.World.FlagMessageEvent",
        "Aow.Game.World.FlagMessageEvent+Resource",
        "Aow.Game.World.TutorialMessageEvent",
        "Aow.Game.World.TutorialMessageEvent+Resource",
        "Aow.Game.World.FleeLocation",
        "Aow.Game.World.FleeLocation+Resource",
        "Aow.Game.Draw.MovementHS",
        "Aow.Game.World.Terrain.Gems",
        "Aow.Game.World.Terrain.Gems+Resource",
        "Aow.Game.Draw.LibraryHS",
        "Aow.Game.World.Hexes.Hexagon",
        "Aow.Game.World.Hexes.HexagonResource",
        "Aow.Game.World.HillMO",
        "Aow.Game.World.HillMOR",
        "Aow.Game.World.Terrain.HolyGround",
        "Aow.Game.World.Terrain.HolyGround+Resource",
        "Aow.Game.Magic.Spells.GlobalTarget.HolyGroundSpell",
        "Aow.Game.Commands.HolyGroundSpellTE",
        "Aow.Game.World.Terrain.House",
        "Aow.Game.World.Terrain.House+Resource",
        "Aow.Game.World.Hexes.HexagonBridge",
        "Aow.Game.World.Hexes.HexagonBridge+Resource",
        "Aow.Game.World.Terrain.FixedILTerrainMO",
        "Aow.Game.World.Terrain.FixedILTerrainMOR",
        "Aow.Game.World.Terrain.ILTerrainMO",
        "Aow.Game.World.Terrain.ILTerrainMOR",
        "Aow.Game.World.Hexes.IsometricCombatHexagon",
        "Aow.Game.World.Hexes.IsometricCombatHexagon+Resource",
        "Aow.Game.World.Structures.ItemExplorationSite",
        "Aow.Game.World.Structures.ItemExplorationSiteResource",
        "Aow.Game.World.Structures.LifeNode",
        "Aow.Game.World.Structures.LifeNode+Resource",
        "Aow.Game.World.Structures.MagicSanctuary",
        "Aow.Game.Draw.MagicSanctuaryOverlayHS",
        "Aow.Game.World.Structures.MagicSanctuary+Resource",
        "Aow.Game.World.Terrain.MainStructure",
        "Aow.Game.World.Terrain.MainStructure+Resource",
        "Aow.Game.World.Terrain.ManaCrystals",
        "Aow.Game.World.Terrain.ManaCrystals+Resource",
        "Aow.Game.World.Structures.Merchant",
        "Aow.Game.World.Structures.Merchant+Resource",
        "Aow.Game.World.Structures.Mine",
        "Aow.Game.World.Structures.Mine+Resource",
        "Aow.Game.World.Structures.Monsterlair",
        "Aow.Game.World.Structures.Monsterlair+Resource",
        "Aow.Game.World.MountainMO",
        "Aow.Game.World.MountainMOR",
        "Aow.Game.Magic.Spells.GlobalTarget.RaiseTerrain",
        "Aow.Game.Commands.RaiseTerrainTE",
        "Aow.Game.World.Terrain.RaisedMountain",
        "Aow.Game.World.Terrain.RaisedMountain+Resource",
        "Aow.Game.World.Movement.ChangeSongMoveOnEvent",
        "Aow.Game.World.Movement.ChangeSongMoveOnEvent+Resource",
        "Aow.Game.World.Movement.MoveOnEvent",
        "Aow.Game.World.Movement.MoveOnEventResource",
        "Aow.Game.World.Movement.PlaySampleMoveOnEvent",
        "Aow.Game.World.Movement.PlaySampleMoveOnEvent+Resource",
        "Aow.Game.World.Movement.ShowMessageMoveOnEvent",
        "Aow.Game.World.Movement.ShowMessageMoveOnEvent+Resource",
        "Aow.Game.World.Terrain.OverlayObject",
        "Aow.Game.World.Terrain.OverlayObject+Resource",
        "Aow.Game.Draw.OverviewUnitHS",
        "Aow.Game.World.Terrain.Pad",
        "Aow.Game.World.Terrain.Pad+Resource",
        "Aow.Game.World.PoisonCloud",
        "Aow.Game.World.PoisonCloud+Resource",
        "Aow.Game.World.Terrain.PoisonPlant",
        "Aow.Game.World.Terrain.PoisonPlant+Resource",
        "Aow.Game.Magic.Spells.GlobalTarget.PoisonPlantSpell",
        "Aow.Game.Commands.PoisonPlantSpellTE",
        "Aow.Game.World.Structures.PowerNode",
        "Aow.Game.World.Structures.PowerNode+Resource",
        "Aow.Game.World.Structures.ProductionPlace",
        "Aow.Game.World.Structures.ProductionPlaceResource",
        "Aow.Game.World.Structures.Pyramid",
        "Aow.Game.World.Structures.Pyramid+Resource",
        "Aow.Game.World.Structures.RandomNode",
        "Aow.Game.World.Structures.RandomNode+Resource",
        "Aow.Game.World.Structures.ReflectingPool",
        "Aow.Game.World.Structures.ReflectingPool+Resource",
        "Aow.Game.World.Hexes.RndHexagonResource",
        "Aow.Game.World.Hexes.Road",
        "Aow.Game.World.Hexes.Road+Resource",
        "Aow.Game.World.Structures.Ruin",
        "Aow.Game.World.Structures.Ruin+Resource",
        "Aow.Game.World.Structures.Shipyard",
        "Aow.Game.World.Structures.Shipyard+Resource",
        "Aow.Game.World.Structures.Sign",
        "Aow.Game.World.Structures.Sign+Resource",
        "Aow.Game.Commands.UpdateSignTE",
        "Aow.Game.World.Hexes.SingleCombatHexagon",
        "Aow.Game.World.Hexes.SingleCombatHexagon+Resource",
        "Aow.Game.World.SmokeCloud",
        "Aow.Game.World.SmokeCloud+Resource",
        "Aow.Game.World.Structures.SpringOfLife",
        "Aow.Game.Draw.Animation.SpringOfLifeAnimation",
        "Aow.Game.World.Structures.SpringOfLife+Resource",
        "Aow.Game.Draw.Animation.BlastStorm",
        "Aow.Game.Draw.Animation.DeathStorm",
        "Aow.Game.Draw.Animation.DivineStorm",
        "Aow.Game.Draw.Animation.FireStorm",
        "Aow.Game.Draw.Animation.IceStorm",
        "Aow.Game.Draw.Animation.LightningStorm",
        "Aow.Game.Draw.Animation.PestilenceStorm",
        "Aow.Game.Draw.Animation.Storm",
        "Aow.Game.Commands.StormSpellTE",
        "Aow.Game.World.Structures.Teleport",
        "Aow.Game.World.Structures.Teleport+Resource",
        "Aow.Game.World.Hexes.TerrainMO",
        "Aow.Game.World.Hexes.TerrainMOR",
        "Aow.Game.World.Structures.Tower",
        "Aow.Game.World.Structures.Tower+Resource",
        "Aow.Game.World.Terrain.Townhall",
        "Aow.Game.World.Terrain.Townhall+Resource",
        "Aow.Game.World.Terrain.UnHolyGround",
        "Aow.Game.World.Terrain.UnHolyGround+Resource",
        "Aow.Game.Magic.Spells.GlobalTarget.UnholyGroundSpell",
        "Aow.Game.Commands.UnholyGroundSpellTE",
        "Aow.Game.World.Structures.WaterNode",
        "Aow.Game.World.Structures.WaterNode+Resource",
        "Aow.Game.World.Structures.WizardsTower",
        "Aow.Game.World.Structures.WizardsTower+Resource",
        "Aow.Game.World.Terrain.Workshop",
        "Aow.Game.World.Terrain.Workshop+Resource",
        "Aow.Game.World.MultiHexMO",
        "Aow.Game.World.MultiHexMOR",
        "Aow.Game.Draw.HexagonSprite",
        "Aow.Game.World.MapObject",
        "Aow.Game.World.MapObjectResource",
        "Aow.Game.Draw.SingleHS",
        "Aow.Game.World.Map.AbstractRoad",
        "Aow.Game.World.Map.AbstractRoadResource",
        "Aow.Game.World.Map.AbstractHexagonResource",
        "Aow.Game.World.Structures.PlayerStructure",
        "Aow.Game.World.Structures.PlayerStructureResource",
        "Aow.Game.World.Structures.Structure",
        "Aow.Game.World.Structures.StructureResource",
        "Aow.Game.Commands.TokenEvent",
        "Aow.Game.Draw.UnitHS",
        "Aow.Game.Draw.ItemHS",
        "Aow.Game.Commands.MoveTE",
        "Aow.Game.Commands.SpellTE",
        "Aow.Game.Commands.StructureTE",
        "Aow.Game.World.Map.AoWMapField",
        "Aow.Game.World.Map.AoWMapLevel",
        "Aow.Game.World.Map.AbstractBorderHexagon+Resource",
        "Aow.Game.World.Hexes.IsometricHexagon",
        "Aow.Game.World.Hexes.LowerIsometricHexagon",
        "Aow.Game.World.Hexes.UpperIsometricHexagon",
        "Aow.Game.World.Hexes.IsometricHexagonResource",
        "Aow.Game.World.Hexes.LowerIsometricHexagon+Resource",
        "Aow.Game.World.Hexes.UpperIsometricHexagon+Resource",
        "Aow.Game.Commands.GlobalTargetSpellTE",
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

    private static EObject Direct(Type type, ECustomNode owner)
    {
        var constructor = type.GetConstructor([typeof(ECustomNode)]);
        if (constructor is not null) return (EObject)Invoke(() => constructor.Invoke([owner]))!;
        var factory = type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, [typeof(ECustomNode)]);
        if (factory is not null) return (EObject)Invoke(() => factory.Invoke(null, [owner]))!;
        return (EObject)Invoke(() => Activator.CreateInstance(type, [owner]))!;
    }

    [Theory]
    [MemberData(nameof(Classes))]
    public void StateOwnerObservationAndTeardownMatchTheOriginal(string name)
    {
        Type type = typeof(Spell).Assembly.GetType(name)!;

        for (int owned = 0; owned < 2; owned++)
            for (int route = 0; route < 4; route++)
            {
                using var fixture = new Fixture();
                var owner = owned == 0 ? null : new SweepOwner();
                EObject? value = null;
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

    private static EObject Saved(Type type, ECustomNode owner)
    {
        if (Aow.Game.Engine.Nodes.EngineHost.Engines is null) Aow.Game.Engine.Nodes.EngineHost.Initialize();
        using var engine = new Engine(null!);
        var descriptor = EClass.Of(type);
        engine.Options -= Set<EngineModeElement>.Of(Aow.Game.Engine.Engine.eoVerifyClasses);
        try { engine.UnregisterEClasses([engine.GetEClass(descriptor.ClassID)]); }
        catch (System.Exception error) when (PortedExceptions.Is(error)) { }
        engine.RegisterEClasses([descriptor]);
        return engine.CreateEObject(descriptor.ClassID, owner);
    }

    [Theory]
    [InlineData("Aow.Game.World.Map.AbstractAoWHSMapMouse")]
    [InlineData("Aow.Game.Combat.AoWCombatMapMouse")]
    [InlineData("Aow.Game.Combat.AoWNewCombatMapSettings")]
    [InlineData("Aow.Game.Combat.CombatMoveControl")]
    public void HelperStateMatchesTheOriginalFactory(string name)
    {
        using var fixture = new Fixture();
        Type type = typeof(Spell).Assembly.GetType(name)!;
        if (name.EndsWith(".AoWNewCombatMapSettings", StringComparison.Ordinal))
            typeof(HSEngine).GetField("FHSSFileName", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(Aow.Game.Magic.Abilities.GameTables.AoWEngine, "");
        Type[] signature = name.EndsWith(".CombatMoveControl", StringComparison.Ordinal) ? [typeof(HSMap)] : [];
        var value = (object)Invoke(() => type.GetConstructor(signature)!.Invoke(signature.Length == 0 ? [] : [null]))!;
        try
        {
            ConstructionSnapshot.Check("SweepConstruction", name, Snapshot(value));
        }
        finally { ((value ?? throw new NullReferenceException()) as IDisposable)?.Dispose(); }
    }

}
