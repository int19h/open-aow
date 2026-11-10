using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.Json;
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
public sealed class RemainingConstructionTests
{
    private sealed class RemainingOwner : EngineFixture.Owner
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
        private readonly TAoWHSSet previousSet = Aow.Game.Engine.TAoWHSSet.AoWHSSet;
        private readonly TAoWEngine previousEngine = Aow.Game.Magic.Abilities.GameTables.AoWEngine;
        private readonly TAoWHSMap previousMap = Aow.Game.World.Map.TAoWHSMap.AoWHSMap;
        internal readonly TImageLibraryManager Manager = new TImageLibraryManager(null!);
        internal readonly TMapObjectResourceList CombatResources = new TMapObjectResourceList(null!);

        internal Fixture()
        {
            var set = EngineFixture.Configure(EngineFixture.CreateGameSet(Manager), value => { value.FCombatResources = CombatResources; });
            Aow.Game.Engine.TAoWHSSet.AoWHSSet = set;
            Aow.Game.Magic.Abilities.GameTables.AoWEngine = null!;
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap = null!;
            Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitGFXResources = null!;
            Runtime.RandSeed = 123456789;
        }

        public void Dispose()
        {
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
                    if (field.Name is "eclass" or "actionClass" or "groupClass") continue; // the descriptor caches are not constructor state
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
        "Aow.Game.Engine.Collections.TListObject",
        "Aow.Game.Ai.Groups.TAIGroupRazeControl",
        "Aow.Game.Ai.Groups.TAIGroupDefendTargetSelector",
        "Aow.Game.Ai.Groups.TAIGroupWanderControl",
        "Aow.Game.Ui.Controls.TAOWAlignment",
        "Aow.Game.Ui.Controls.TaowRect",
        "Aow.Game.Draw.TXYZ",
        "Aow.Game.Draw.TXYZList",
        "Aow.Game.Ui.Windows.TChangeInfo",
        "Aow.Game.Ui.Controls.TColorChange",
        "Aow.Game.Session.TSetupPackage",
        "Aow.Game.Session.TRemotePlayer",
        "Aow.Game.Session.TRemotePlayerList",
        "Aow.Game.Session.TPlayerSetupSettings",
        "Aow.Game.Session.TSetupSettings",
        "Aow.Game.Session.TCustomizeLeaderControl",
        "Aow.Game.Combat.TTCAbility",
        "Aow.Game.Combat.TROb",
        "Aow.Game.Combat.TaiMove",
        "Aow.Game.Combat.TCAI",
        "Aow.Game.Combat.TUnitPositionControl",
        "Aow.Game.Combat.TCombatMoveControlSettings",
        "Aow.Game.Combat.TTacticalCombatUnit",
        "Aow.Game.Combat.TTacticalCombatPlayer",
        "Aow.Game.Combat.TTacticalCombatPlayerList",
        "Aow.Game.Combat.TTacticalCombat",
        "Aow.Game.Combat.TRndTacHsm",
        "Aow.Game.Ui.Dialogs.TWMCampaignScenarioPage",
        "Aow.Game.Ui.Dialogs.TWMCampaignScenarioPageList",
        "Aow.Game.Session.TWMCampaignScenario",
        "Aow.Game.Session.TWMCampaign",
        "Aow.Game.Ui.Controls.TFileTime",
        "Aow.Game.Ai.Groups.TAbstractBehaviorAG",
        "Aow.Game.Ai.Groups.TAbstractBehaviorAGC",
        "Aow.Game.Ai.Groups.TSkirmishAG",
        "Aow.Game.Ai.Groups.TSkirmishAGC",
        "Aow.Game.Ai.Groups.TNormalAG",
        "Aow.Game.Ai.Groups.TNormalAGC",
        "Aow.Game.Ai.Groups.TDefenderAG",
        "Aow.Game.Ai.Groups.TDefenderAGC",
        "Aow.Game.Ai.Groups.TAggressorAG",
        "Aow.Game.Ai.Groups.TAggressorAGC",
        "Aow.Game.Ai.Groups.TExpanderAG",
        "Aow.Game.Ai.Groups.TExpanderAGC",
        "Aow.Game.Ai.Groups.TScorcherAG",
        "Aow.Game.Ai.Groups.TScorcherAGC",
        "Aow.Game.World.Structures.TCityProductionControl",
        "Aow.Game.Units.TMigrationItem",
        "Aow.Game.World.Structures.TMigrationList",
        "Aow.Game.World.Structures.TCityEnchantment",
        "Aow.Game.Ai.TAICityUpgradeBudget",
        "Aow.Game.Ai.TAICityResearchBudget",
        "Aow.Game.Ai.TAICityFortifyBudget",
        "Aow.Game.Ai.TCityAIPA",
        "Aow.Game.Units.TPrisonersArmy",
        "Aow.Game.Events.TExecuteEventLogCallBackList",
        "Aow.Game.Events.TEventLogList",
        "Aow.Game.Events.TEventLogbook",
        "Aow.Game.World.Structures.TDefendersArmy",
        "Aow.Game.Engine.Hash.TIntegerHashList",
        "Aow.Game.Engine.Hash.TIntegerHashTable256",
        "Aow.Game.Engine.Hash.TStringHashList",
        "Aow.Game.Engine.Hash.TStringHashTable256",
        "Aow.Game.Ai.Groups.TGuardAG",
        "Aow.Game.Ai.Groups.TGuardAGC",
        "Aow.Game.Ai.Groups.TGuardAreaAG",
        "Aow.Game.Ai.Groups.TGuardAreaAGC",
        "Aow.Game.Ai.Groups.TPatrolAG",
        "Aow.Game.Ai.Groups.TPatrolAGC",
        "Aow.Game.Ai.Groups.TRefugeAG",
        "Aow.Game.Ai.Groups.TRefugeAGC",
        "Aow.Game.Ai.Groups.TRaidAG",
        "Aow.Game.Ai.Groups.TRaidAGC",
        "Aow.Game.Ai.Groups.TScoutAG",
        "Aow.Game.Ai.Groups.TScoutAGC",
        "Aow.Game.Units.TExplorationSiteItemList",
        "Aow.Game.Ui.Grids.TItemListView",
        "Aow.Game.Ai.Groups.TPassiveAG",
        "Aow.Game.Ai.Groups.TPassiveAGC",
        "Aow.Game.Ai.Groups.TFortifyAG",
        "Aow.Game.Ai.Groups.TFortifyAGC",
        "Aow.Game.Ai.Groups.TSuicidalAG",
        "Aow.Game.Ai.Groups.TSuicidalAGC",
        "Aow.Game.Ai.Groups.TTransportAG",
        "Aow.Game.Ai.Groups.TTransportAGC",
        "Aow.Game.World.Terrain.TCrystalMagic",
        "Aow.Game.Ui.Grids.TPageTurnControl",
        "Aow.Game.World.Structures.TProductionPlaceControl",
        "Aow.Game.World.Structures.TShipyardConstructionControl",
        "Aow.Game.Magic.Spells.TSmokeCloudEnchantment",
        "Aow.Game.Magic.Spells.TStormSpell",
        "Aow.Game.Magic.Spells.TFireStormSpell",
        "Aow.Game.Magic.Spells.TDivineStormSpell",
        "Aow.Game.Magic.Spells.TDeathStormSpell",
        "Aow.Game.Magic.Spells.TIceStormSpell",
        "Aow.Game.Magic.Spells.TLightningStormSpell",
        "Aow.Game.Magic.Spells.TPestilenceStormSpell",
        "Aow.Game.World.Structures.TTowerConstructionControl",
        "Aow.Game.Magic.Abilities.TUnitView",
    ];

    public static IEnumerable<object[]> Classes => Names.Select(name => new object[] { name });
    public static IEnumerable<object[]> GuardedClasses => Names.Where(name => name.StartsWith("Aow.Game.Magic.Spells.", StringComparison.Ordinal) && name.EndsWith("StormSpell", StringComparison.Ordinal)).Select(name => new object[] { name });

    private static object? Invoke(Func<object?> action)
    {
        try { return action(); }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }

    private static object Create(Type type, TECustomNode owner, int route)
    {
        if (route == 1) return EClass.Of(type).Create(owner);
        if (route == 2)
        {
            if (Aow.Game.Engine.Nodes.EngineHost.Engines is null) Aow.Game.Engine.Nodes.EngineHost.Initialize();
            using var engine = new TEngine(null!);
            var meta = EClass.Of(type);
            engine.Options -= TSet<TEngineModeElement>.Of(Aow.Game.Engine.TEngine.eoVerifyClasses);
            try { engine.UnregisterEClasses([engine.GetEClass(meta.ClassID)]); }
            catch (System.Exception error) when (PortedExceptions.Is(error)) { }
            engine.RegisterEClasses([EClass.Of(type)]);
            return engine.CreateEObject(meta.ClassID, owner);
        }
        if (route == 3)
        {
            var meta = Aow.Game.Engine.EClass.Of(type);
            return (object)Invoke(() => meta.GetType().GetMethod("Create", [typeof(TECustomNode)])!.Invoke(meta, [owner]))!;
        }
        var factory = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).SingleOrDefault(x => x.Name == "Create");
        Type[] signature = typeof(TEObject).IsAssignableFrom(type) ? [typeof(TECustomNode)] : [];
        var constructor = type.GetConstructor(signature);
        if (factory is not null) return (object)Invoke(() => factory.Invoke(null, factory.GetParameters().Length == 0 ? [] : [owner]))!;
        if (constructor is not null) return (object)Invoke(() => constructor.Invoke(signature.Length == 0 ? [] : [owner]))!;
        throw new InvalidOperationException(type.FullName + " has no declared completed construction signature.");
    }

    private static void Check(string name, string key, string actual)
    {
        ConstructionSnapshot.Check("RemainingConstruction", name + "." + key, actual);
    }

    [Theory]
    [MemberData(nameof(Classes))]
    public void ConstructorStateAndCreationRoutesMatchTheBaseCommit(string name)
    {
        Type type = typeof(TSpell).Assembly.GetType(name)!;
        bool engineObject = typeof(TEObject).IsAssignableFrom(type);
        for (int ownerMode = 0; ownerMode < (engineObject ? 2 : 1); ownerMode++)
            for (int route = 0; route < (engineObject ? 4 : 1); route++)
            {
                using var fixture = new Fixture();
                var owner = ownerMode == 0 ? null : new RemainingOwner();
                object? value = null;
                string state;
                var states = new List<string>();
                try
                {
                    value = Create(type, owner!, route);
                    Assert.IsType(type, value);
                    if (engineObject) Assert.Same(owner, ((TEObject)value).Owner);
                    state = "success:" + Snapshot(value);
                }
                catch (System.Exception error) { state = "failure:" + error.GetType().FullName + ":" + error.Message; }
                finally
                {
                    try { (value as IDisposable)?.Dispose(); }
                    catch (System.Exception error) { states.Add("destroy:" + error.GetType().FullName + ":" + error.Message); }
                }
                states.Add(state);
                states.Add("owner:" + (owner?.Members.Count ?? 0));
                if (owner is not null) states.Add(JsonSerializer.Serialize(owner.Observations));
                Check(name, "owner" + ownerMode + "route" + route, string.Join("\n", states));
                Assert.StartsWith("success:", state);
            }
    }

    [Fact]
    public void ListObjectMetaclassCreatesTheExactType()
    {
        var value = new Aow.Game.Engine.Collections.TListObject();
        Assert.IsType<Aow.Game.Engine.Collections.TListObject>(value);
        Assert.Equal(Snapshot(new Aow.Game.Engine.Collections.TListObject()), Snapshot(value));
        value.Dispose();
    }

    [Theory]
    [MemberData(nameof(GuardedClasses))]
    public void PlainStormConstructorsKeepOwnerAndSpellState(string name)
    {
        Type type = typeof(TSpell).Assembly.GetType(name)!;
        for (int route = 0; route < 4; route++)
        {
            using var fixture = new Fixture();
            var owner = new RemainingOwner();
            var value = Assert.IsAssignableFrom<TSpell>(Create(type, owner, route));
            Assert.IsType(type, value);
            Assert.Same(owner, value.Owner);
            Assert.Same(value, Assert.Single(owner.Members));
            Assert.NotNull(value.FSpellText);
            Assert.NotNull(value.FCastImages);
            value.Dispose();
            Assert.Empty(owner.Members);
            Assert.Equal(1, owner.Removals);
        }
    }
}
