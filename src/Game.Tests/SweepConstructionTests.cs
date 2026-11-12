using System.Collections;
using System.Reflection;
using System.Runtime.ExceptionServices;

using System.Text.Json;
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class SweepConstructionTests
{
    private sealed class SweepOwner : EngineFixture.Owner
    {

        internal readonly List<string> Observations = [];

        public override int AddChild(EngineObject child)
        {
            base.AddChild(child);
            Observations.Add(Snapshot(child));
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child) { return base.RemoveChild(child) < 0 ? -1 : 0; }
    }

    private sealed class Fixture : IDisposable
    {
        private readonly int _previousSeed = Runtime.RandSeed;
        private readonly AoWHSSet _previousSet = AoWHSSet.Current;
        private readonly GameEngine _previousEngine = GameTables.GameEngine;
        private readonly AoWHSMap _previousMap = AoWHSMap.Current;
        internal readonly ImageLibraryManager Manager = new ImageLibraryManager(null!);
        internal readonly MapObjectResourceList CombatResources = new MapObjectResourceList(null!);

        internal Fixture()
        {
            Runtime.RandSeed = 123456789;
            var set = EngineFixture.Configure(EngineFixture.CreateGameSet(Manager), value => { value.CombatResources = CombatResources; });
            AoWHSSet.Current = set;
            GameTables.GameEngine = EngineFixture.Engine;
            AoWHSMap.Current = null!;
            AoWHSSet.Current.UnitGraphicsResources = null!;
            Runtime.RandSeed = 123456789;
        }

        public void Dispose()
        {
            Runtime.RandSeed = _previousSeed;

            AoWHSSet.Current = _previousSet;
            GameTables.GameEngine = _previousEngine;
            AoWHSMap.Current = _previousMap;
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
            if (item is GameEngine) return "$engine";
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
                    if (field.Name == "_engineClass") continue; // the descriptor cache is not constructor state
                    string key = current.Name + "." + field.Name;
                    object? child = field.GetValue(item);
                    if (field.Name == "_owner") { fields[key] = child is null ? null : "$owner"; continue; }
                    fields[key] = State(child, path + "." + key);
                }
            return fields;
        }
        return JsonSerializer.Serialize(State(value, "$"));
    }

    private static readonly string[] s_names =
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
        "Aow.Game.Draw.Animations.Animation",
        "Aow.Game.Commands.AnimationTokenEvent",
        "Aow.Game.Draw.Animations.ImageSequenceAnimation",
        "Aow.Game.Draw.Animations.TwinkleAnimation",
        "Aow.Game.World.Hexes.AoWBorderHexagon",
        "Aow.Game.World.Hexes.AoWHexagon",
        "Aow.Game.World.Hexes.AoWHexagon+Resource",
        "Aow.Game.World.Hexes.WaterHex",
        "Aow.Game.World.Hexes.WaterHex+Resource",
        "Aow.Game.World.Hexes.AoWRoad",
        "Aow.Game.Combat.CombatMapLevel",
        "Aow.Game.Draw.TacticalBattleBloodHexSprite",
        "Aow.Game.Draw.TacticalBattleCorpseHexSprite",
        "Aow.Game.Draw.TacticalBattleItemHexSprite",
        "Aow.Game.Draw.TacticalBattleOozeHexSprite",
        "Aow.Game.Draw.RangedPathHexSprite",
        "Aow.Game.Draw.TacticalBattleUnitHexSprite",
        "Aow.Game.Draw.UnitPositionHexSprite",
        "Aow.Game.Draw.BattleEffectSprite",
        "Aow.Game.World.Hexes.Terrain",
        "Aow.Game.World.Hexes.Terrain+Resource",
        "Aow.Game.World.Structures.Arena",
        "Aow.Game.World.Structures.Arena+Resource",
        "Aow.Game.Commands.ArenaTokenEvent",
        "Aow.Game.World.Structures.BuildersGuild",
        "Aow.Game.World.Structures.BuildersGuild+Resource",
        "Aow.Game.World.Structures.BloodShrine",
        "Aow.Game.Draw.Animations.BloodShrineAnimation",
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
        "Aow.Game.Draw.CityOverlayHexSprite",
        "Aow.Game.World.Structures.Resource",
        "Aow.Game.World.Terrain.CityWall",
        "Aow.Game.World.Terrain.CityWallDoor",
        "Aow.Game.World.Terrain.CityWallDoor+Resource",
        "Aow.Game.World.Terrain.CityWall+Resource",
        "Aow.Game.Draw.CombatFire",
        "Aow.Game.World.Hexes.CombatHex",
        "Aow.Game.World.Hexes.CombatHex+Resource",
        "Aow.Game.World.Hexes.WaterCombatHex",
        "Aow.Game.World.Hexes.WaterCombatHex+Resource",
        "Aow.Game.World.Terrain.CombatFixedImageLibraryTerrain",
        "Aow.Game.World.Terrain.CombatFixedImageLibraryTerrain+Resource",
        "Aow.Game.World.Terrain.CombatImageLibraryTerrain",
        "Aow.Game.World.Terrain.CombatImageLibraryTerrainResource",
        "Aow.Game.Draw.CombatImageLibraryTerrainShadow",
        "Aow.Game.World.CombatObstacle",
        "Aow.Game.Draw.CombatObstacleOverlay",
        "Aow.Game.World.CombatObstacle+Resource",
        "Aow.Game.Draw.CombatObstacleShadow",
        "Aow.Game.World.Hexes.CombatRoad",
        "Aow.Game.World.Hexes.CombatRoad+Resource",
        "Aow.Game.World.Terrain.CombatStructure",
        "Aow.Game.World.Terrain.CombatStructureResource",
        "Aow.Game.Commands.CombatAbilityRangedTokenEvent",
        "Aow.Game.Commands.CombatAbilityTouchMoveTokenEvent",
        "Aow.Game.Commands.TacticalBattleEndCombatTokenEvent",
        "Aow.Game.Commands.TacticalBattleEndTurnTokenEvent",
        "Aow.Game.Commands.TacticalBattleMeleeMoveTokenEvent",
        "Aow.Game.Commands.CombatMoveTokenEvent",
        "Aow.Game.Commands.TerminateCombatTokenEvent",
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
        "Aow.Game.Draw.MovementHexSprite",
        "Aow.Game.World.Terrain.Gems",
        "Aow.Game.World.Terrain.Gems+Resource",
        "Aow.Game.World.Hexes.Hexagon",
        "Aow.Game.World.Hexes.HexagonResource",
        "Aow.Game.World.Terrain.HolyGround",
        "Aow.Game.World.Terrain.HolyGround+Resource",
        "Aow.Game.Magic.Spells.HolyGroundSpell",
        "Aow.Game.Commands.HolyGroundSpellTokenEvent",
        "Aow.Game.World.Terrain.House",
        "Aow.Game.World.Terrain.House+Resource",
        "Aow.Game.World.Hexes.HexBridge",
        "Aow.Game.World.Hexes.HexBridge+Resource",
        "Aow.Game.World.Terrain.FixedImageLibraryTerrainMapObject",
        "Aow.Game.World.Terrain.FixedImageLibraryTerrainMapObjectResource",
        "Aow.Game.World.Terrain.ImageLibraryTerrainMapObject",
        "Aow.Game.World.Terrain.ImageLibraryTerrainMapObjectResource",
        "Aow.Game.World.Hexes.IsometricCombatHex",
        "Aow.Game.World.Hexes.IsometricCombatHex+Resource",
        "Aow.Game.World.Structures.ItemExplorationSite",
        "Aow.Game.World.Structures.ItemExplorationSiteResource",
        "Aow.Game.World.Structures.LifeNode",
        "Aow.Game.World.Structures.LifeNode+Resource",
        "Aow.Game.World.Structures.MagicSanctuary",
        "Aow.Game.Draw.MagicSanctuaryOverlayHexSprite",
        "Aow.Game.World.Structures.MagicSanctuary+Resource",
        "Aow.Game.World.Terrain.MainStructure",
        "Aow.Game.World.Terrain.MainStructure+Resource",
        "Aow.Game.World.Terrain.ManaCrystals",
        "Aow.Game.World.Terrain.ManaCrystals+Resource",
        "Aow.Game.World.Structures.Mine",
        "Aow.Game.World.Structures.Mine+Resource",
        "Aow.Game.World.Structures.MonsterLair",
        "Aow.Game.World.Structures.MonsterLair+Resource",
        "Aow.Game.World.MountainMapObject",
        "Aow.Game.World.MountainMapObjectResource",
        "Aow.Game.Magic.Spells.RaiseTerrain",
        "Aow.Game.Commands.RaiseTerrainTokenEvent",
        "Aow.Game.World.Terrain.RaisedMountain",
        "Aow.Game.World.Terrain.RaisedMountain+Resource",
        "Aow.Game.World.Terrain.OverlayObject",
        "Aow.Game.World.Terrain.OverlayObject+Resource",
        "Aow.Game.Draw.OverviewUnitHexSprite",
        "Aow.Game.World.Terrain.Pad",
        "Aow.Game.World.Terrain.Pad+Resource",
        "Aow.Game.World.PoisonCloud",
        "Aow.Game.World.PoisonCloud+Resource",
        "Aow.Game.World.Terrain.PoisonPlant",
        "Aow.Game.World.Terrain.PoisonPlant+Resource",
        "Aow.Game.Magic.Spells.PoisonPlantSpell",
        "Aow.Game.Commands.PoisonPlantSpellTokenEvent",
        "Aow.Game.World.Structures.PowerNode",
        "Aow.Game.World.Structures.PowerNode+Resource",
        "Aow.Game.World.Structures.ProductionPlace",
        "Aow.Game.World.Structures.ProductionPlaceResource",
        "Aow.Game.World.Structures.Pyramid",
        "Aow.Game.World.Structures.Pyramid+Resource",
        "Aow.Game.World.Structures.ReflectingPool",
        "Aow.Game.World.Structures.ReflectingPool+Resource",
        "Aow.Game.World.Hexes.Road",
        "Aow.Game.World.Hexes.Road+Resource",
        "Aow.Game.World.Structures.Ruin",
        "Aow.Game.World.Structures.Ruin+Resource",
        "Aow.Game.World.Structures.Shipyard",
        "Aow.Game.World.Structures.Shipyard+Resource",
        "Aow.Game.World.Structures.Sign",
        "Aow.Game.World.Structures.Sign+Resource",
        "Aow.Game.Commands.UpdateSignTokenEvent",
        "Aow.Game.World.Hexes.SingleCombatHex",
        "Aow.Game.World.Hexes.SingleCombatHex+Resource",
        "Aow.Game.World.SmokeCloud",
        "Aow.Game.World.SmokeCloud+Resource",
        "Aow.Game.World.Structures.SpringOfLife",
        "Aow.Game.Draw.Animations.SpringOfLifeAnimation",
        "Aow.Game.World.Structures.SpringOfLife+Resource",
        "Aow.Game.Draw.Animations.BlastStorm",
        "Aow.Game.Draw.Animations.DeathStorm",
        "Aow.Game.Draw.Animations.DivineStorm",
        "Aow.Game.Draw.Animations.FireStorm",
        "Aow.Game.Draw.Animations.IceStorm",
        "Aow.Game.Draw.Animations.LightningStorm",
        "Aow.Game.Draw.Animations.PestilenceStorm",
        "Aow.Game.Draw.Animations.Storm",
        "Aow.Game.Commands.StormSpellTokenEvent",
        "Aow.Game.World.Structures.Teleport",
        "Aow.Game.World.Structures.Teleport+Resource",
        "Aow.Game.World.Hexes.TerrainMapObject",
        "Aow.Game.World.Hexes.TerrainMapObjectResource",
        "Aow.Game.World.Structures.Tower",
        "Aow.Game.World.Structures.Tower+Resource",
        "Aow.Game.World.Terrain.UnholyGround",
        "Aow.Game.World.Terrain.UnholyGround+Resource",
        "Aow.Game.Magic.Spells.UnholyGroundSpell",
        "Aow.Game.Commands.UnholyGroundSpellTokenEvent",
        "Aow.Game.World.Structures.WaterNode",
        "Aow.Game.World.Structures.WaterNode+Resource",
        "Aow.Game.World.Structures.WizardsTower",
        "Aow.Game.World.Structures.WizardsTower+Resource",
        "Aow.Game.World.MultiHexMapObject",
        "Aow.Game.World.MultiHexMapObjectResource",
        "Aow.Game.Draw.HexSprite",
        "Aow.Game.World.MapObject",
        "Aow.Game.World.MapObjectResource",
        "Aow.Game.Draw.SingleHexSprite",
        "Aow.Game.World.Map.AbstractRoad",
        "Aow.Game.World.Map.AbstractRoadResource",
        "Aow.Game.World.Map.AbstractHexagonResource",
        "Aow.Game.World.Structures.PlayerStructure",
        "Aow.Game.World.Structures.PlayerStructureResource",
        "Aow.Game.World.Structures.Structure",
        "Aow.Game.World.Structures.StructureResource",
        "Aow.Game.Commands.TokenEvent",
        "Aow.Game.Draw.UnitHexSprite",
        "Aow.Game.Draw.ItemHexSprite",
        "Aow.Game.Commands.MoveTokenEvent",
        "Aow.Game.Commands.SpellTokenEvent",
        "Aow.Game.Commands.StructureTokenEvent",
        "Aow.Game.World.Map.AoWMapField",
        "Aow.Game.World.Map.AoWMapLevel",
        "Aow.Game.World.Map.AbstractBorderHexagon+Resource",
        "Aow.Game.World.Hexes.IsometricHex",
        "Aow.Game.World.Hexes.LowerIsometricHex",
        "Aow.Game.World.Hexes.UpperIsometricHex",
        "Aow.Game.World.Hexes.IsometricHexResource",
        "Aow.Game.World.Hexes.LowerIsometricHex+Resource",
        "Aow.Game.World.Hexes.UpperIsometricHex+Resource",
        "Aow.Game.Commands.GlobalTargetSpellTokenEvent",
    ];

    public static IEnumerable<object[]> Classes => s_names.Select(name => new object[] { name });

    private static object? Invoke(Func<object?> action)
    {
        try { return action(); }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }

    private static EngineObject Direct(Type type, ECustomNode owner)
    {
        var constructor = type.GetConstructor([typeof(ECustomNode)]);
        if (constructor is not null) return (EngineObject)Invoke(() => constructor.Invoke([owner]))!;
        var factory = type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, [typeof(ECustomNode)]);
        if (factory is not null) return (EngineObject)Invoke(() => factory.Invoke(null, [owner]))!;
        return (EngineObject)Invoke(() => Activator.CreateInstance(type, [owner]))!;
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
                EngineObject? value = null;
                var states = new List<string>();
                try
                {
                    value = route == 1 ? EngineClass.Of(type).CreateInstance(owner!)
                        : route == 0 ? Direct(type, owner!)
                        : route == 2 ? EngineClass.Of(type).CreateInstance(owner!)
                        : Saved(type, owner!);
                    Assert.IsType(type, value);
                    Assert.Same(owner, value.Owner);
                    states.Add("success:" + Snapshot(value));
                }
                catch (Exception error) { states.Add("failure:" + error.GetType().FullName + ":" + error.Message); }
                finally
                {
                    try { value?.Dispose(); }
                    catch (Exception error) { states.Add("destroy:" + error.GetType().FullName + ":" + error.Message); }
                }
                states.Add("owner:" + (owner?.Members.Count ?? 0));
                if (owner is not null) states.Add(JsonSerializer.Serialize(owner.Observations));
                string actual = string.Join("\n", states);
                string key = name + ".owner" + owned;
                ConstructionSnapshot.Check("SweepConstruction", key, actual);
            }
    }

    private static EngineObject Saved(Type type, ECustomNode owner)
    {
        if (EngineHost.Engines is null) EngineHost.Initialize();
        using GameEngine engine = new GameEngine(null!);
        var descriptor = EngineClass.Of(type);
        engine.Options -= Set<EngineModeElement>.Of(GameEngine.VerifyClasses);
        try { engine.UnregisterEngineClasses([engine.GetEngineClass(descriptor.ClassId)]); }
        catch (Exception error) when (PortedExceptions.Is(error)) { }
        engine.RegisterEngineClasses([descriptor]);
        return engine.CreateEngineObject(descriptor.ClassId, owner);
    }

    [Theory]
    [InlineData("Aow.Game.World.Map.GameMapMouse")]
    [InlineData("Aow.Game.Combat.CombatMapMouse")]
    [InlineData("Aow.Game.Combat.CombatMoveControl")]
    public void HelperStateMatchesTheOriginalFactory(string name)
    {
        using var fixture = new Fixture();
        Type type = typeof(Spell).Assembly.GetType(name)!;
        Type[] signature = name.EndsWith(".CombatMoveControl", StringComparison.Ordinal) ? [typeof(HSMap)] : [];
        var value = (object)Invoke(() => type.GetConstructor(signature)!.Invoke(signature.Length == 0 ? [] : [null]))!;
        try
        {
            ConstructionSnapshot.Check("SweepConstruction", name, Snapshot(value));
        }
        finally { ((value ?? throw new NullReferenceException()) as IDisposable)?.Dispose(); }
    }

}
