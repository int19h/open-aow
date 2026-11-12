using System.Collections;
using System.Reflection;
using System.Runtime.ExceptionServices;

using System.Text.Json;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class LeftoverConstructionTests
{
    private sealed class LeftoverOwner : EngineFixture.Owner
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
                    // CF-2 stores priority as a delegate. Keep its value in the base recording's key.
                    if (item is AIPlayerActionClass actionClass && field.Name == "_priority")
                    {
                        fields[current.Name + ".<Priority>k__BackingField"] = actionClass.Priority;
                        continue;
                    }
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
        "Aow.Game.AI.Groups.AIGroup",
        "Aow.Game.AI.Groups.AIGroupControl",
        "Aow.Game.AI.Groups.AIGroupTask",
        "Aow.Game.Magic.Abilities.Ability",
        "Aow.Game.Magic.Abilities.AbilityOwner",
        "Aow.Game.Session.Campaign",
        "Aow.Game.Session.CampaignScenario",
        "Aow.Game.World.Map.AoWHSMap",
        "Aow.Game.Units.Army",
        "Aow.Game.Commands.ArmyCombatMoveTokenEvent",
        "Aow.Game.Commands.ArmyCombineMoveTokenEvent",
        "Aow.Game.Commands.ArmyDefaultMoveTokenEvent",
        "Aow.Game.Commands.ArmyNegotiateMoveTokenEvent",
        "Aow.Game.Commands.ChangeGlobalMagicTokenEvent",
        "Aow.Game.Commands.ChangePlayerMagicTokenEvent",
        "Aow.Game.Commands.ChangePlayerSettingsTokenEvent",
        "Aow.Game.Commands.CheatTokenEvent",
        "Aow.Game.Combat.Battle",
        "Aow.Game.Combat.Actions.CombatAction",
        "Aow.Game.Commands.CombatFlowControlTokenEvent",
        "Aow.Game.Combat.CombatObject",
        "Aow.Game.Combat.CombatUnit",
        "Aow.Game.Commands.ConstructStructureTokenEvent",
        "Aow.Game.Commands.DiplomaticActionTokenEvent",
        "Aow.Game.Commands.DisbandUnitTokenEvent",
        "Aow.Game.Commands.DispelMagicAbilityTokenEvent",
        "Aow.Game.Combat.Actions.DispelMagicCombatAction",
        "Aow.Game.Magic.Abilities.DurationAbility",
        "Aow.Game.Commands.EndTurnTokenEvent",
        "Aow.Game.Magic.Abilities.EnhancementAbility",
        "Aow.Game.World.Terrain.Fire",
        "Aow.Game.World.Terrain.Fire+Resource",
        "Aow.Game.Draw.FrozenWaterHexSprite",
        "Aow.Game.Commands.GameOverTokenEvent",
        "Aow.Game.Magic.GlobalEnchantment",
        "Aow.Game.Commands.HealingTokenEvent",
        "Aow.Game.Commands.HeroUpdateTokenEvent",
        "Aow.Game.Commands.HeroUpgradeTokenEvent",
        "Aow.Game.Units.ItemList",
        "Aow.Game.Commands.ItemTokenEvent",
        "Aow.Game.Units.LeaderSetupControl",
        "Aow.Game.Commands.MoveArmyTokenEvent",
        "Aow.Game.World.Movement.MoveControlSettings",
        "Aow.Game.Commands.MoveTokenEvent",
        "Aow.Game.Magic.Abilities.MultiLevelAbility",
        "Aow.Game.Commands.MultiMoveTokenEvent",
        "Aow.Game.Commands.NegotiateTokenEvent",
        "Aow.Game.Session.PlayerMagicControl",
        "Aow.Game.World.Structures.ProductionControl",
        "Aow.Game.Commands.ProductionTokenEvent",
        "Aow.Game.Commands.SaveGameTokenEvent",
        "Aow.Game.Magic.Spells.Spell",
        "Aow.Game.Magic.Spells.SpellCaster",
        "Aow.Game.Audio.StartupSongList",
        "Aow.Game.Draw.StructureShadow",
        "Aow.Game.Commands.TerminatePlayerTokenEvent",
        "Aow.Game.Magic.Abilities.UnitEnchantmentAbility",
        "Aow.Game.Draw.UnitShadowHexSprite",
        "Aow.Game.Magic.Spells.UnitSpell",
        "Aow.Game.Commands.UnitSpellTokenEvent",
        "Aow.Game.Draw.WorldEffectHexSprite",
        "Aow.Game.Commands.CityEnchantmentSpellTokenEvent",
        "Aow.Game.Commands.CityTargetSpellTokenEvent",
        "Aow.Game.Commands.TownGateTokenEvent",
        "Aow.Engine.Nodes.EChangeNotifyEventList",
        "Aow.Engine.Nodes.EChangeNotifyNode",
        "Aow.Engine.Nodes.EControlStyleInverseSortNode",
        "Aow.Engine.Nodes.EControlStyleNode",
        "Aow.Engine.Nodes.ECustomNode",
        "Aow.Engine.Nodes.ECustomOwner",
        "Aow.Engine.Nodes.EInverseSortNode",
        "Aow.Engine.Nodes.ENode",
        "Aow.Engine.Nodes.EngineObjectEditor",
        "Aow.Engine.Nodes.EReferenceNode",
        "Aow.Engine.Nodes.EngineResource",
        "Aow.Engine.Nodes.EResourceList",
        "Aow.Engine.Nodes.ESortNode",
        "Aow.Game.Commands.GlobalEnchantmentSpellTokenEvent",
        "Aow.Game.Draw.BirdsViewHexSprite",
        "Aow.Game.Draw.ConcealAreaHexSprite",
        "Aow.Game.Commands.DisjunctionTokenEvent",
        "Aow.Game.Commands.WarpPartyTokenEvent",
        "Aow.Game.Commands.FireBarrierTokenEvent",
        "Aow.Game.Commands.FreezeWaterTokenEvent",
        "Aow.Game.Magic.Spells.GlobalTargetSpell",
        "Aow.Game.Commands.HealingShowersTokenEvent",
        "Aow.Game.Commands.LevelTerrainTokenEvent",
        "Aow.Game.Commands.RejuvenateTokenEvent",
        "Aow.Game.Commands.TornadoTokenEvent",
        "Aow.Game.Commands.VortexTokenEvent",
        "Aow.Game.Draw.WatcherHexSprite",
        "Aow.Game.Commands.WatcherTokenEvent",
        "Aow.Game.Draw.FixedVisibleHexSprite",
        "Aow.Game.World.Map.HSSet",
        "Aow.Game.World.Map.MapField",
        "Aow.Game.World.Map.MapLevel",
        "Aow.Game.World.MapObjectResource",
        "Aow.Game.World.MultiHexMapObjectResource",
        "Aow.Game.World.Terrain.ImageLibraryTerrainMapObject",
        "Aow.Game.Commands.TokenEvent",
        "Aow.Game.Audio.SongList",
        "Aow.Game.Commands.SummonSpellTokenEvent",
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
        return EngineClass.Of(type).CreateInstance(owner);
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
                var owner = owned == 0 ? null : new LeftoverOwner();
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
                string key = name + ".owner" + owned + ".route" + route;
                ConstructionSnapshot.Check("LeftoverConstruction", key, actual);
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

    private sealed class RejectingOwner : EngineFixture.Owner
    {
        public RejectingOwner() : base() { }

        internal AoWHSMap? Captured;

        public override int AddChild(EngineObject child)
        {
            Captured = Assert.IsType<AoWHSMap>(child);
            Assert.Equal(0, Captured.MapChangedLockCount);
            throw new InvalidOperationException("reject after insertion");
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void PlainWorldMapPublishesItsCompletedState(int route)
    {
        using var fixture = new Fixture();
        var value = Assert.IsType<AoWHSMap>(route switch
        {
            0 => new AoWHSMap(null!),
            1 => EngineClass.Of<AoWHSMap>().CreateInstance(null!),
            2 => EngineClass.Of(typeof(AoWHSMap)).CreateInstance(null!),
            _ => Saved(typeof(AoWHSMap), null!)
        });
        Assert.Equal(0, value.MapChangedLockCount);
        Assert.NotNull(value.ItemControl);
        Assert.NotNull(value.Effects);
        value.Dispose();
    }

    [Fact]
    public void MapMouseKeepsItsInheritedInitialization()
    {
        using var mouse = new AowHexSpriteMapMouse();
        Assert.Equal((-1, -1, -1, -1), (mouse.SelectionRectangle.Left, mouse.SelectionRectangle.Top,
            mouse.SelectionRectangle.Right, mouse.SelectionRectangle.Bottom));
        Assert.Equal(0, mouse._rectangleSelectEventsStore.Count);
    }
}
