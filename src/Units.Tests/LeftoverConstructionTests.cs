using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Aow.Game.Ai;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Spells;
using Aow.Game.World;
using Aow.Game.World.Map;

namespace Aow.Units.Tests;

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
        "Aow.Game.Combat.Combat",
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
        "Aow.Game.Commands.SpellTECallViewP4",
        "Aow.Game.Audio.StartupSongList",
        "Aow.Game.Draw.StructureShadow",
        "Aow.Game.Commands.TerminatePlayerTE",
        "Aow.Game.Magic.Abilities.UnitEnchantmentAbility",
        "Aow.Game.Draw.UnitShadowHS",
        "Aow.Game.Magic.Spells.Unit.UnitSpell",
        "Aow.Game.Commands.UnitSpellTE",
        "Aow.Game.Draw.WorldFXHS",
        "Aow.Game.Commands.CityEnchantmentSpellTE",
        "Aow.Game.Commands.CityTargetSpellTE",
        "Aow.Game.Commands.TownGateTE",
        "Aow.Game.Engine.Nodes.EChangeNotifyEventList",
        "Aow.Game.Engine.Nodes.EChangeNotifyNode",
        "Aow.Game.Engine.Nodes.EControlStyleInverseSortNode",
        "Aow.Game.Engine.Nodes.EControlStyleNode",
        "Aow.Game.Engine.Nodes.ECustomNode",
        "Aow.Game.Engine.Nodes.ECustomOwner",
        "Aow.Game.Engine.Nodes.EInverseSortNode",
        "Aow.Game.Engine.Nodes.ENode",
        "Aow.Game.Engine.Nodes.EObjectEditor",
        "Aow.Game.Engine.Nodes.EReferenceNode",
        "Aow.Game.Engine.Nodes.EResource",
        "Aow.Game.Engine.Nodes.EResourceList",
        "Aow.Game.Engine.Nodes.ESortNode",
        "Aow.Game.Commands.GlobalEnchantmentSpellTE",
        "Aow.Game.Draw.BirdsViewHS",
        "Aow.Game.Draw.ConcealAreaHS",
        "Aow.Game.Commands.DisjunctionTE",
        "Aow.Game.Commands.WarpPartyTE",
        "Aow.Game.Commands.FireBarrierTE",
        "Aow.Game.Commands.FreezeWaterTE",
        "Aow.Game.Magic.Spells.GlobalTarget.GlobalTargetSpell",
        "Aow.Game.Magic.Spells.GlobalTarget.GlobalTargetSpellCalls",
        "Aow.Game.Commands.HealingShowersTE",
        "Aow.Game.Commands.LevelTerrainTE",
        "Aow.Game.Commands.RejuvenateTE",
        "Aow.Game.Commands.TornadoTE",
        "Aow.Game.Commands.VortexTE",
        "Aow.Game.Draw.WatcherHS",
        "Aow.Game.Commands.WatcherTE",
        "Aow.Game.Draw.FixedVisibleHS",
        "Aow.Game.Draw.HPBasedHS",
        "Aow.Game.World.Map.HSSet",
        "Aow.Game.World.Map.MapField",
        "Aow.Game.World.Map.MapFieldTandO",
        "Aow.Game.World.Map.MapLevel",
        "Aow.Game.World.MapObjectResource",
        "Aow.Game.World.MultiHexMOR",
        "Aow.Game.World.Terrain.ILTerrainMO",
        "Aow.Game.Net.ENodeView",
        "Aow.Game.Commands.TokenEvent",
        "Aow.Game.Audio.ENodeAccess",
        "Aow.Game.Audio.Songlist",
        "Aow.Game.Commands.SummonSpellTE",
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
                catch (System.Exception error) { states.Add("failure:" + error.GetType().FullName + ":" + error.Message); }
                finally
                {
                    try { value?.Dispose(); }
                    catch (System.Exception error) { states.Add("destroy:" + error.GetType().FullName + ":" + error.Message); }
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
        if (Aow.Game.Engine.Nodes.EngineHost.Engines is null) Aow.Game.Engine.Nodes.EngineHost.Initialize();
        using var engine = new Engine(null!);
        var descriptor = EClass.Of(type);
        engine.Options -= Set<EngineModeElement>.Of(Aow.Game.Engine.Engine.eoVerifyClasses);
        try { engine.UnregisterEClasses([engine.GetEClass(descriptor.ClassID)]); }
        catch (System.Exception error) when (PortedExceptions.Is(error)) { }
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
    public void MapSettingsAndMouseKeepTheirInheritedInitialization()
    {
        var settings = new AoWNewHSMapSettings();
        Assert.Equal((64, 64, 1), (settings.Width, settings.Height, settings.Levels));
        Assert.Equal(AoWHSMap.ClassID, settings.HSMapClassID);
        Assert.Equal(MapContainer.ClassID, settings.MapContainerClassID);
        Assert.Equal(AoWMapLevel.ClassID, settings.MapLevelClassID);
        using var mouse = new AoWHSMapMouse();
        Assert.Equal((-1, -1, -1, -1), (mouse.FSelectionRectangle.Left, mouse.FSelectionRectangle.Top,
            mouse.FSelectionRectangle.Right, mouse.FSelectionRectangle.Bottom));
        Assert.Equal(0, mouse.FRectangleSelectEventsStore.Count);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void HashTablesAllocateADistinctListPerBucket(bool strings)
    {
        object table;
        var buckets = new List<object>();
        if (strings)
        {
            var value = new StringHashTable256();
            table = value;
            for (int i = 0; i < 256; i++) buckets.Add(value.FLists[i]);
        }
        else
        {
            var value = new IntegerHashTable256();
            table = value;
            for (int i = 0; i < 256; i++) buckets.Add(value.FLists[i]);
        }
        Assert.Equal(256, buckets.Count);
        Assert.Equal(256, new HashSet<object>(buckets, ReferenceEqualityComparer.Instance).Count);
        Assert.DoesNotContain(table, buckets);
        ((table ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
    }
}
