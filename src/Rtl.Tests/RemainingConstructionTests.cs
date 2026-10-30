using System.Collections;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.ImageLib;
using Game = Aow.Units.AoWE.AoWE;

namespace Aow.Rtl.Tests;

public sealed class RemainingConstructionTests
{
    private sealed class Owner : TECustomNode
    {
        internal readonly List<TEObject> Members = [];
        internal readonly List<string> Observations = [];
        internal int Removals;
        public override int AddChild(TEObject child)
        {
            Members.Add(child);
            Observations.Add(Snapshot(child));
            return Members.Count - 1;
        }
        public override int RemoveChild(TEObject child) { Removals++; return Members.Remove(child) ? 0 : -1; }
    }
    private sealed class Fixture : IDisposable
    {
        private readonly TAoWHSSet previousSet = Game.AoWHSSet;
        private readonly TAoWEngine previousEngine = Game.AoWEngine;
        private readonly TAoWHSMap previousMap = Game.AoWHSMap;
        internal readonly TImageLibraryManager Manager = TImageLibraryManager.Create(null!);
        internal readonly TMapObjectResourceList CombatResources = new TMapObjectResourceList(null!);
        internal Fixture()
        {
            var set = new TAoWHSSet { FCombatResources = CombatResources };
            typeof(THSSet).GetField("FImageLibraryManager", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(set, Manager);
            Game.AoWHSSet = set;
            Game.AoWEngine = null!;
            Game.AoWHSMap = null!;
        }
        public void Dispose()
        {
            Game.AoWHSSet = previousSet;
            Game.AoWEngine = previousEngine;
            Game.AoWHSMap = previousMap;
            CombatResources.Destroy();
            Manager.Destroy();
        }
    }
    private static string Snapshot(object value)
    {
        var visited = new Dictionary<object, string>(ReferenceEqualityComparer.Instance);
        object? State(object? item, string path)
        {
            if (item is null) return null;
            Type type = item.GetType();
            if (item is AnsiString text) return text.ToString();
            if (type.IsEnum) return Convert.ToInt64(item);
            if (type.IsPrimitive || item is string || item is decimal) return item;
            if (item is Type represented) return represented.FullName;
            if (item is TObject.Meta meta) return meta.Type.FullName;
            if (item is Delegate handler) return handler.GetInvocationList().Select(x => x.Method.DeclaringType!.FullName + "." + x.Method.Name).ToArray();
            if (!type.IsValueType)
            {
                if (visited.TryGetValue(item, out string? found)) return "$ref:" + found;
                visited.Add(item, path);
            }
            if (item is Array array) return array.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            if (item is IList list) return list.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            var fields = new SortedDictionary<string, object?> { ["$type"] = type.FullName };
            for (Type? current = type; current is not null && current != typeof(TObject) && current != typeof(object); current = current.BaseType)
                foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(x => x.Name))
                {
                    if (field.Name is "eclass" or "actionClass" or "groupClass") continue; // the descriptor caches are not constructor state
                    string key = current.Name + "." + field.Name;
                    object? child = field.GetValue(item);
                    if (field.Name == "FOwner") { fields[key] = child is null ? null : "$owner"; continue; }
                    if (field.Name is "F_PlayerList_14" or "F_CombatData_34" or "F_CombatData_2C" or "F_CombatData_28")
                        child = Rtl.ReferenceFromId<TObject>((int)child!);
                    fields[key] = State(child, path + "." + key);
                }
            return fields;
        }
        return JsonSerializer.Serialize(State(value, "$"));
    }


    private static readonly string[] Names =
    [
        "Aow.Units.EList.TListObject",
        "Aow.Units.AGCPlugins.TAIGroupRazeControl",
        "Aow.Units.AGCPlugins.TAIGroupDefendTargetSelector",
        "Aow.Units.AGCPlugins.TAIGroupWanderControl",
        "Aow.Units.AoWComp.TAOWAlignment",
        "Aow.Units.AoWComp.TaowRect",
        "Aow.Units.aowFX.TXYZ",
        "Aow.Units.aowFX.TXYZList",
        "Aow.Units.AoWHTMLMemo.TChangeInfo",
        "Aow.Units.AoWMemo.TColorChange",
        "Aow.Units.AoWSetup.TSetupPackage",
        "Aow.Units.AoWSetup.TRemotePlayer",
        "Aow.Units.AoWSetup.TRemotePlayerList",
        "Aow.Units.AoWSetup.TPlayerSetupSettings",
        "Aow.Units.AoWSetup.TSetupSettings",
        "Aow.Units.AoWSetup.TCustomizeLeaderControl",
        "Aow.Units.AoWTC.TTCAbility",
        "Aow.Units.AoWTC.TROb",
        "Aow.Units.AoWTC.TaiMove",
        "Aow.Units.AoWTC.TCAI",
        "Aow.Units.AoWTC.TUnitPositionControl",
        "Aow.Units.AoWTC.TCombatMoveControlSettings",
        "Aow.Units.AoWTC.TTacticalCombatUnit",
        "Aow.Units.AoWTC.TTacticalCombatPlayer",
        "Aow.Units.AoWTC.TTacticalCombatPlayerList",
        "Aow.Units.AoWTC.TTacticalCombat",
        "Aow.Units.AoWTC.TRndTacHsm",
        "Aow.Units.AoWVideoDlg.TWMCampaignScenarioPage",
        "Aow.Units.AoWVideoDlg.TWMCampaignScenarioPageList",
        "Aow.Units.AoWVideoDlg.TWMCampaignScenario",
        "Aow.Units.AoWVideoDlg.TWMCampaign",
        "Aow.Units.AoWWorldMap.TFileTime",
        "Aow.Units.BehaviorAG.TAbstractBehaviorAG",
        "Aow.Units.BehaviorAG.TAbstractBehaviorAGC",
        "Aow.Units.BehaviorAG.TSkirmishAG",
        "Aow.Units.BehaviorAG.TSkirmishAGC",
        "Aow.Units.BehaviorAG.TNormalAG",
        "Aow.Units.BehaviorAG.TNormalAGC",
        "Aow.Units.BehaviorAG.TDefenderAG",
        "Aow.Units.BehaviorAG.TDefenderAGC",
        "Aow.Units.BehaviorAG.TAggressorAG",
        "Aow.Units.BehaviorAG.TAggressorAGC",
        "Aow.Units.BehaviorAG.TExpanderAG",
        "Aow.Units.BehaviorAG.TExpanderAGC",
        "Aow.Units.BehaviorAG.TScorcherAG",
        "Aow.Units.BehaviorAG.TScorcherAGC",
        "Aow.Units.City.TCityProductionControl",
        "Aow.Units.City.TMigrationItem",
        "Aow.Units.City.TMigrationList",
        "Aow.Units.City.TCityEnchantment",
        "Aow.Units.City.TAICityUpgradeBudget",
        "Aow.Units.City.TAICityResearchBudget",
        "Aow.Units.City.TAICityFortifyBudget",
        "Aow.Units.City.TCityAIPA",
        "Aow.Units.Dungeon.TPrisonersArmy",
        "Aow.Units.EventLog.TExecuteEventLogCallBackList",
        "Aow.Units.EventLog.TEventLogList",
        "Aow.Units.EventLog.TEventLogbook",
        "Aow.Units.ExploreS.TDefendersArmy",
        "Aow.Units.HashTabU.TIntegerHashList",
        "Aow.Units.HashTabU.TIntegerHashTable256",
        "Aow.Units.HashTabU.TStringHashList",
        "Aow.Units.HashTabU.TStringHashTable256",
        "Aow.Units.IndependentAG.TGuardAG",
        "Aow.Units.IndependentAG.TGuardAGC",
        "Aow.Units.IndependentAG.TGuardAreaAG",
        "Aow.Units.IndependentAG.TGuardAreaAGC",
        "Aow.Units.IndependentAG.TPatrolAG",
        "Aow.Units.IndependentAG.TPatrolAGC",
        "Aow.Units.IndependentAG.TRefugeAG",
        "Aow.Units.IndependentAG.TRefugeAGC",
        "Aow.Units.IndependentAG.TRaidAG",
        "Aow.Units.IndependentAG.TRaidAGC",
        "Aow.Units.IndependentAG.TScoutAG",
        "Aow.Units.IndependentAG.TScoutAGC",
        "Aow.Units.ItemExpS.TExplorationSiteItemList",
        "Aow.Units.ItemGrid.TItemListView",
        "Aow.Units.MainAG.TPassiveAG",
        "Aow.Units.MainAG.TPassiveAGC",
        "Aow.Units.MainAG.TFortifyAG",
        "Aow.Units.MainAG.TFortifyAGC",
        "Aow.Units.MainAG.TSuicidalAG",
        "Aow.Units.MainAG.TSuicidalAGC",
        "Aow.Units.MainAG.TTransportAG",
        "Aow.Units.MainAG.TTransportAGC",
        "Aow.Units.ManaCrystals.TCrystalMagic",
        "Aow.Units.PageTurnCtrl.TPageTurnControl",
        "Aow.Units.PrdPlace.TProductionPlaceControl",
        "Aow.Units.Shipyard.TShipyardConstructionControl",
        "Aow.Units.SmokeCloud.TSmokeCloudEnchantment",
        "Aow.Units.Storms.TStormSpell",
        "Aow.Units.Storms.TFireStormSpell",
        "Aow.Units.Storms.TDivineStormSpell",
        "Aow.Units.Storms.TDeathStormSpell",
        "Aow.Units.Storms.TIceStormSpell",
        "Aow.Units.Storms.TLightningStormSpell",
        "Aow.Units.Storms.TPestilenceStormSpell",
        "Aow.Units.Tower.TTowerConstructionControl",
        "Aow.Units.UnitIDlg.TUnitView",
    ];
    public static IEnumerable<object[]> Classes => Names.Select(name => new object[] { name });
    public static IEnumerable<object[]> GuardedClasses => Names.Where(name => name.StartsWith("Aow.Units.Storms.", StringComparison.Ordinal)).Select(name => new object[] { name });
    private static string Hash(string value) => Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
    private static object? Invoke(Func<object?> action)
    {
        try { return action(); }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }
    private static TObject Create(Type type, TECustomNode owner, int route)
    {
        if (route == 1) return ((TEObject.Meta)MetaRegistry.Of(type)).Create(owner);
        if (route == 2)
        {
            if (Aow.Units.Engine.Engine.Engines is null) Aow.Units.Engine.Engine.Initialize();
            var engine = new TEngine(null!);
            try
            {
                var meta = (TEObject.Meta)MetaRegistry.Of(type);
                engine.Options -= TSet<TEngineModeElement>.Of(Aow.Units.Engine.Engine.eoVerifyClasses);
                try { engine.UnregisterEClasses([engine.GetEClass(meta.ClassID())]); }
                catch (Aow.Rtl.Exception) { }
                engine.RegisterEClasses([EClass.Of(type)]);
                return engine.CreateEObject(meta.ClassID(), owner);
            }
            finally { engine.Destroy(); }
        }
        if (route == 3)
        {
            var meta = MetaRegistry.Of(type);
            return (TObject)Invoke(() => meta.GetType().GetMethod("Create", [typeof(TECustomNode)])!.Invoke(meta, [owner]))!;
        }
        var factory = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).SingleOrDefault(x => x.Name == "Create");
        Type[] signature = typeof(TEObject).IsAssignableFrom(type) ? [typeof(TECustomNode)] : [];
        var constructor = type.GetConstructor(signature);
        if (factory is not null) return (TObject)Invoke(() => factory.Invoke(null, factory.GetParameters().Length == 0 ? [] : [owner]))!;
        if (constructor is not null) return (TObject)Invoke(() => constructor.Invoke(signature.Length == 0 ? [] : [owner]))!;
        throw new InvalidOperationException(type.FullName + " has no declared completed construction signature.");
    }
    private static void Check(string name, string key, string actual)
    {
        using var baseline = JsonDocument.Parse(typeof(RemainingConstructionTests).Assembly.GetManifestResourceStream("Aow.Rtl.Tests.RemainingConstructionBaseline.json")!);
        Assert.True(baseline.RootElement.GetProperty(name + "." + key).GetString() == Hash(actual), name + " " + key + "\n" + actual);
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
            var owner = ownerMode == 0 ? null : new Owner();
            TObject? value = null;
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
                try { value?.Destroy(); }
                catch (System.Exception error) { states.Add("destroy:" + error.GetType().FullName + ":" + error.Message); }
            }
            states.Add(state);
            states.Add("owner:" + (owner?.Members.Count ?? 0));
            if (owner is not null) states.Add(JsonSerializer.Serialize(owner.Observations));
            Check(name, "owner" + ownerMode + "route" + route, string.Join("\n", states));
            Assert.StartsWith("success:", state);
        }
        if (engineObject) Assert.Throws<InvalidOperationException>(() => MetaRegistry.Of(type).NewInstance());
        if (name.StartsWith("Aow.Units.Storms.", StringComparison.Ordinal)) Assert.Empty(type.GetConstructors());
    }
    [Fact]
    public void ListObjectMetaclassCreatesTheExactType()
    {
        var value = new Aow.Units.EList.TListObject();
        Assert.IsType<Aow.Units.EList.TListObject>(value);
        Assert.Equal(Snapshot(new Aow.Units.EList.TListObject()), Snapshot(value));
        value.Destroy();
    }
    [Theory]
    [MemberData(nameof(GuardedClasses))]
    public void FailureAfterPublicationMatchesTheBaseCommit(string name)
    {
        Type type = typeof(TSpell).Assembly.GetType(name)!;
        for (int route = 0; route < 4; route++)
        {
            using var fixture = new Fixture();
            Game.AoWHSSet = null!;
            var owner = new Owner();
            TObject? value = null;
            System.Exception? failure = null;
            try { value = Create(type, owner, route); }
            catch (System.Exception error) { failure = error; }
            Assert.Null(value);
            Assert.IsType<NullReferenceException>(failure);
            Assert.Empty(owner.Members);
            Assert.Equal(1, owner.Removals);
            Check(name, "failure" + route, JsonSerializer.Serialize(new { Error = failure!.GetType().FullName, failure.Message, owner.Removals, owner.Observations }));
        }
    }
}
