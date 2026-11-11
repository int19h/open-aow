using System.Collections;
using System.Reflection;
using System.Runtime.ExceptionServices;

using System.Text.Json;
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Game.Ai;
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
        private readonly AoWHSSet previousSet = AoWHSSet.Current;
        private readonly GameEngine previousEngine = GameTables.AoWEngine;
        private readonly AoWHSMap previousMap = AoWHSMap.Current;
        internal readonly ImageLibraryManager Manager = new ImageLibraryManager(null!);
        internal readonly MapObjectResourceList CombatResources = new MapObjectResourceList(null!);

        internal Fixture()
        {
            Runtime.RandSeed = 123456789;
            var set = EngineFixture.Configure(EngineFixture.CreateGameSet(Manager), value => { value.FCombatResources = CombatResources; });
            AoWHSSet.Current = set;
            GameTables.AoWEngine = EngineFixture.Engine;
            AoWHSMap.Current = null!;
            AoWHSSet.Current.FUnitGFXResources = null!;
            Runtime.RandSeed = 123456789;
        }

        public void Dispose()
        {
            Runtime.RandSeed = previousSeed;

            AoWHSSet.Current = previousSet;
            GameTables.AoWEngine = previousEngine;
            AoWHSMap.Current = previousMap;
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
                    if (field.Name == "eclass") continue; // the descriptor cache is not constructor state
                    // CF-2 stores priority as a delegate. Keep its value in the base recording's key.
                    if (item is AIPlayerActionClass actionClass && field.Name == "priority")
                    {
                        fields[current.Name + ".<Priority>k__BackingField"] = actionClass.Priority;
                        continue;
                    }
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
        "Aow.Game.Ai.Groups.AIGroup",
        "Aow.Game.Ai.Groups.AIGroupControl",
        "Aow.Game.Ai.Groups.AIGroupControlPlugin",
        "Aow.Game.Magic.Abilities.Ability",
        "Aow.Game.Magic.Abilities.AbilityOwner",
        "Aow.Game.Session.AoWCampaign",
        "Aow.Game.Session.AoWCampaignScenario",
        "Aow.Game.World.Map.AoWHSMap",
        "Aow.Game.Units.Army",
        "Aow.Game.Commands.ArmyCombatMoveTE",
        "Aow.Game.Commands.ArmyCombineMoveTE",
        "Aow.Game.Commands.ArmyDefaultMoveTE",
        "Aow.Game.Commands.ArmyNegotiateMoveTE",
        "Aow.Game.Commands.ChangeGlobalMagicTE",
        "Aow.Game.Commands.ChangePlayerMagicTE",
        "Aow.Game.Commands.ChangePlayerSettingsTE",
        "Aow.Game.Commands.CheatTE",
        "Aow.Game.Combat.Battle",
        "Aow.Game.Combat.Actions.CombatAction",
        "Aow.Game.Commands.CombatFlowControlTE",
        "Aow.Game.Combat.CombatObject",
        "Aow.Game.Combat.CombatUnit",
        "Aow.Game.Commands.ConstructStructureTE",
        "Aow.Game.Commands.DiplomaticActionTE",
        "Aow.Game.Commands.DisbandUnitTE",
        "Aow.Game.Commands.DispelMagicAbilityTE",
        "Aow.Game.Combat.Actions.DispelMagicCA",
        "Aow.Game.Magic.Abilities.DurationAbility",
        "Aow.Game.Commands.EndTurnTE",
        "Aow.Game.Magic.Abilities.EnhancementAbility",
        "Aow.Game.World.Terrain.Fire",
        "Aow.Game.World.Terrain.Fire+Resource",
        "Aow.Game.Draw.FrozenWaterHS",
        "Aow.Game.Commands.GameOverTE",
        "Aow.Game.Magic.GlobalEnchantment",
        "Aow.Game.Commands.HealingTE",
        "Aow.Game.Commands.HeroUpdateTE",
        "Aow.Game.Commands.HeroUpgradeTE",
        "Aow.Game.Units.ItemList",
        "Aow.Game.Commands.ItemTE",
        "Aow.Game.Units.LeaderSetupControl",
        "Aow.Game.Commands.MoveArmyTE",
        "Aow.Game.World.Movement.MoveControlSettings",
        "Aow.Game.Commands.MoveTE",
        "Aow.Game.Magic.Abilities.MultiLevelAbility",
        "Aow.Game.Commands.MultiMoveTE",
        "Aow.Game.Commands.NegotiateTE",
        "Aow.Game.Session.PlayerMagicControl",
        "Aow.Game.World.Structures.ProductionControl",
        "Aow.Game.Commands.ProductionTE",
        "Aow.Game.Commands.SaveGameTE",
        "Aow.Game.Magic.Spells.Spell",
        "Aow.Game.Magic.Spells.SpellCaster",
        "Aow.Game.Audio.StartupSongList",
        "Aow.Game.Draw.StructureShadow",
        "Aow.Game.Commands.TerminatePlayerTE",
        "Aow.Game.Magic.Abilities.UnitEnchantmentAbility",
        "Aow.Game.Draw.UnitShadowHS",
        "Aow.Game.Magic.Spells.UnitSpell",
        "Aow.Game.Commands.UnitSpellTE",
        "Aow.Game.Draw.WorldFXHS",
        "Aow.Game.Commands.CityEnchantmentSpellTE",
        "Aow.Game.Commands.CityTargetSpellTE",
        "Aow.Game.Commands.TownGateTE",
        "Aow.Engine.Nodes.EChangeNotifyEventList",
        "Aow.Engine.Nodes.EChangeNotifyNode",
        "Aow.Engine.Nodes.EControlStyleInverseSortNode",
        "Aow.Engine.Nodes.EControlStyleNode",
        "Aow.Engine.Nodes.ECustomNode",
        "Aow.Engine.Nodes.ECustomOwner",
        "Aow.Engine.Nodes.EInverseSortNode",
        "Aow.Engine.Nodes.ENode",
        "Aow.Engine.Nodes.EObjectEditor",
        "Aow.Engine.Nodes.EReferenceNode",
        "Aow.Engine.Nodes.EResource",
        "Aow.Engine.Nodes.EResourceList",
        "Aow.Engine.Nodes.ESortNode",
        "Aow.Game.Commands.GlobalEnchantmentSpellTE",
        "Aow.Game.Draw.BirdsViewHS",
        "Aow.Game.Draw.ConcealAreaHS",
        "Aow.Game.Commands.DisjunctionTE",
        "Aow.Game.Commands.WarpPartyTE",
        "Aow.Game.Commands.FireBarrierTE",
        "Aow.Game.Commands.FreezeWaterTE",
        "Aow.Game.Magic.Spells.GlobalTargetSpell",
        "Aow.Game.Commands.HealingShowersTE",
        "Aow.Game.Commands.LevelTerrainTE",
        "Aow.Game.Commands.RejuvenateTE",
        "Aow.Game.Commands.TornadoTE",
        "Aow.Game.Commands.VortexTE",
        "Aow.Game.Draw.WatcherHS",
        "Aow.Game.Commands.WatcherTE",
        "Aow.Game.Draw.FixedVisibleHS",
        "Aow.Game.World.Map.HSSet",
        "Aow.Game.World.Map.MapField",
        "Aow.Game.World.Map.MapLevel",
        "Aow.Game.World.MapObjectResource",
        "Aow.Game.World.MultiHexMOR",
        "Aow.Game.World.Terrain.ILTerrainMO",
        "Aow.Game.Commands.TokenEvent",
        "Aow.Game.Audio.Songlist",
        "Aow.Game.Commands.SummonSpellTE",
    ];

    public static IEnumerable<object[]> Classes => Names.Select(name => new object[] { name });

    private static object? Invoke(Func<object?> action)
    {
        try { return action(); }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }

    private static EObject Direct(Type type, ECustomNode owner)
    {
        var constructor = type.GetConstructor([typeof(ECustomNode)]);
        if (constructor is not null) return (EObject)Invoke(() => constructor.Invoke([owner]))!;
        var factory = type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, [typeof(ECustomNode)]);
        if (factory is not null) return (EObject)Invoke(() => factory.Invoke(null, [owner]))!;
        return EClass.Of(type).Create(owner);
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

    private static EObject Saved(Type type, ECustomNode owner)
    {
        if (EngineHost.Engines is null) EngineHost.Initialize();
        using GameEngine engine = new GameEngine(null!);
        var descriptor = EClass.Of(type);
        engine.Options -= Set<EngineModeElement>.Of(GameEngine.eoVerifyClasses);
        try { engine.UnregisterEClasses([engine.GetEClass(descriptor.ClassID)]); }
        catch (Exception error) when (PortedExceptions.Is(error)) { }
        engine.RegisterEClasses([descriptor]);
        return engine.CreateEObject(descriptor.ClassID, owner);
    }

    private sealed class RejectingOwner : EngineFixture.Owner
    {
        public RejectingOwner() : base() { }

        internal AoWHSMap? Captured;

        public override int AddChild(EObject child)
        {
            Captured = Assert.IsType<AoWHSMap>(child);
            Assert.Equal(0, Captured.FMapChangedLockCount);
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
            1 => EClass.Of<AoWHSMap>().Create(null!),
            2 => EClass.Of(typeof(AoWHSMap)).Create(null!),
            _ => Saved(typeof(AoWHSMap), null!)
        });
        Assert.Equal(0, value.FMapChangedLockCount);
        Assert.NotNull(value.FItemControl);
        Assert.NotNull(value.FEffects);
        value.Dispose();
    }

    [Fact]
    public void MapMouseKeepsItsInheritedInitialization()
    {
        using var mouse = new AoWHSMapMouse();
        Assert.Equal((-1, -1, -1, -1), (mouse.FSelectionRectangle.Left, mouse.FSelectionRectangle.Top,
            mouse.FSelectionRectangle.Right, mouse.FSelectionRectangle.Bottom));
        Assert.Equal(0, mouse.FRectangleSelectEventsStore.Count);
    }
}
