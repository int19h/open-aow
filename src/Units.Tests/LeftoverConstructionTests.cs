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
        "Aow.Game.Ai.Groups.TAIGroup",
        "Aow.Game.Ai.Groups.TAIGroupControl",
        "Aow.Game.Ai.Groups.TAIGroupControlPlugin",
        "Aow.Game.Magic.Abilities.TAbility",
        "Aow.Game.Magic.Abilities.TAbilityOwner",
        "Aow.Game.Session.TAoWCampaign",
        "Aow.Game.Session.TAoWCampaignScenario",
        "Aow.Game.World.Map.TAoWHSMap",
        "Aow.Game.Units.TArmy",
        "Aow.Game.Commands.TArmyCombatMoveTE",
        "Aow.Game.Commands.TArmyCombineMoveTE",
        "Aow.Game.Commands.TArmyDefaultMoveTE",
        "Aow.Game.Commands.TArmyNegotiateMoveTE",
        "Aow.Game.Commands.TChangeGlobalMagicTE",
        "Aow.Game.Commands.TChangePlayerMagicTE",
        "Aow.Game.Commands.TChangePlayerSettingsTE",
        "Aow.Game.Commands.TCheatTE",
        "Aow.Game.Combat.TCombat",
        "Aow.Game.Combat.Actions.TCombatAction",
        "Aow.Game.Commands.TCombatFlowControlTE",
        "Aow.Game.Combat.TCombatObject",
        "Aow.Game.Combat.TCombatUnit",
        "Aow.Game.Commands.TConstructStructureTE",
        "Aow.Game.Commands.TDiplomaticActionTE",
        "Aow.Game.Commands.TDisbandUnitTE",
        "Aow.Game.Commands.TDispelMagicAbilityTE",
        "Aow.Game.Combat.Actions.TDispelMagicCA",
        "Aow.Game.Magic.Abilities.TDurationAbility",
        "Aow.Game.Commands.TEndTurnTE",
        "Aow.Game.Magic.Abilities.TEnhancementAbility",
        "Aow.Game.World.Terrain.TFire",
        "Aow.Game.World.Terrain.TFireResource",
        "Aow.Game.Draw.TFrozenWaterHS",
        "Aow.Game.Commands.TGameOverTE",
        "Aow.Game.Magic.TGlobalEnchantment",
        "Aow.Game.Commands.THealingTE",
        "Aow.Game.Commands.THeroUpdateTE",
        "Aow.Game.Commands.THeroUpgradeTE",
        "Aow.Game.Units.TItemList",
        "Aow.Game.Commands.TItemTE",
        "Aow.Game.Units.TLeaderSetupControl",
        "Aow.Game.Commands.TMoveArmyTE",
        "Aow.Game.World.Movement.TMoveControlSettings",
        "Aow.Game.Commands.TMoveTE",
        "Aow.Game.Magic.Abilities.TMultiLevelAbility",
        "Aow.Game.Commands.TMultiMoveTE",
        "Aow.Game.Commands.TNegotiateTE",
        "Aow.Game.Session.TPlayerMagicControl",
        "Aow.Game.World.Structures.TProductionControl",
        "Aow.Game.Commands.TProductionTE",
        "Aow.Game.Commands.TSaveGameTE",
        "Aow.Game.Magic.Spells.TSpell",
        "Aow.Game.Magic.Spells.TSpellCaster",
        "Aow.Game.Commands.TSpellTECallViewP4",
        "Aow.Game.Audio.TStartupSongList",
        "Aow.Game.Draw.TStructureShadow",
        "Aow.Game.Commands.TTerminatePlayerTE",
        "Aow.Game.Magic.Abilities.TUnitEnchantmentAbility",
        "Aow.Game.Draw.TUnitShadowHS",
        "Aow.Game.Magic.Spells.Unit.TUnitSpell",
        "Aow.Game.Commands.TUnitSpellTE",
        "Aow.Game.Draw.TWorldFXHS",
        "Aow.Game.Commands.TCityEnchantmentSpellTE",
        "Aow.Game.Commands.TCityTargetSpellTE",
        "Aow.Game.Commands.TTownGateTE",
        "Aow.Game.Engine.Nodes.TEChangeNotifyEventList",
        "Aow.Game.Engine.Nodes.TEChangeNotifyNode",
        "Aow.Game.Engine.Nodes.TEControlStyleInverseSortNode",
        "Aow.Game.Engine.Nodes.TEControlStyleNode",
        "Aow.Game.Engine.Nodes.TECustomNode",
        "Aow.Game.Engine.Nodes.TECustomOwner",
        "Aow.Game.Engine.Nodes.TEInverseSortNode",
        "Aow.Game.Engine.Nodes.TENode",
        "Aow.Game.Engine.Nodes.TEObjectEditor",
        "Aow.Game.Engine.Nodes.TEReferenceNode",
        "Aow.Game.Engine.Nodes.TEResource",
        "Aow.Game.Engine.Nodes.TEResourceList",
        "Aow.Game.Engine.Nodes.TESortNode",
        "Aow.Game.Commands.TGlobalEnchantmentSpellTE",
        "Aow.Game.Draw.TBirdsViewHS",
        "Aow.Game.Draw.TConcealAreaHS",
        "Aow.Game.Commands.TDisjunctionTE",
        "Aow.Game.Commands.TWarpPartyTE",
        "Aow.Game.Commands.TFireBarrierTE",
        "Aow.Game.Commands.TFreezeWaterTE",
        "Aow.Game.Magic.Spells.GlobalTarget.TGlobalTargetSpell",
        "Aow.Game.Magic.Spells.GlobalTarget.TGlobalTargetSpellCalls",
        "Aow.Game.Commands.THealingShowersTE",
        "Aow.Game.Commands.TLevelTerrainTE",
        "Aow.Game.Commands.TRejuvenateTE",
        "Aow.Game.Commands.TTornadoTE",
        "Aow.Game.Commands.TVortexTE",
        "Aow.Game.Draw.TWatcherHS",
        "Aow.Game.Commands.TWatcherTE",
        "Aow.Game.Draw.TFixedVisibleHS",
        "Aow.Game.Draw.THPBasedHS",
        "Aow.Game.World.Map.THSSet",
        "Aow.Game.World.Map.TMapField",
        "Aow.Game.World.Map.TMapFieldTandO",
        "Aow.Game.World.Map.TMapLevel",
        "Aow.Game.World.TMapObjectResource",
        "Aow.Game.World.TMultiHexMOR",
        "Aow.Game.World.Terrain.TILTerrainMO",
        "Aow.Game.Net.TENodeView",
        "Aow.Game.Commands.TTokenEvent",
        "Aow.Game.Audio.TENodeAccess",
        "Aow.Game.Audio.TSonglist",
        "Aow.Game.Commands.TSummonSpellTE",
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
        return EClass.Of(type).Create(owner);
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
                var owner = owned == 0 ? null : new LeftoverOwner();
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
                string key = name + ".owner" + owned + ".route" + route;
                ConstructionSnapshot.Check("LeftoverConstruction", key, actual);
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

    private sealed class RejectingOwner : EngineFixture.Owner
    {
        public RejectingOwner() : base() { }

        internal TAoWHSMap? Captured;

        public override int AddChild(TEObject child)
        {
            Captured = Assert.IsType<TAoWHSMap>(child);
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
        var value = Assert.IsType<TAoWHSMap>(route switch
        {
            0 => new TAoWHSMap(null!),
            1 => EClass.Of<TAoWHSMap>().Create(null!),
            2 => EClass.Of(typeof(TAoWHSMap)).Create(null!),
            _ => Saved(typeof(TAoWHSMap), null!)
        });
        Assert.Equal(0, value.FMapChangedLockCount);
        Assert.NotNull(value.FItemControl);
        Assert.NotNull(value.FEffects);
        value.Dispose();
    }

    [Fact]
    public void MapSettingsAndMouseKeepTheirInheritedInitialization()
    {
        var settings = new TAoWNewHSMapSettings();
        Assert.Equal((64, 64, 1), (settings.Width, settings.Height, settings.Levels));
        Assert.Equal(TAoWHSMap.ClassID, settings.HSMapClassID);
        Assert.Equal(TMapContainer.ClassID, settings.MapContainerClassID);
        Assert.Equal(TAoWMapLevel.ClassID, settings.MapLevelClassID);
        using var mouse = new TAoWHSMapMouse();
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
            var value = new TStringHashTable256();
            table = value;
            for (int i = 0; i < 256; i++) buckets.Add(value.FLists[i]);
        }
        else
        {
            var value = new TIntegerHashTable256();
            table = value;
            for (int i = 0; i < 256; i++) buckets.Add(value.FLists[i]);
        }
        Assert.Equal(256, buckets.Count);
        Assert.Equal(256, new HashSet<object>(buckets, ReferenceEqualityComparer.Instance).Count);
        Assert.DoesNotContain(table, buckets);
        ((table ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
    }
}
