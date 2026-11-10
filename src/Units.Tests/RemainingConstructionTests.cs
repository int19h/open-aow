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
        private readonly AoWHSSet previousSet = AoWHSSet.Current;
        private readonly AoWEngine previousEngine = Game.Magic.Abilities.GameTables.AoWEngine;
        private readonly AoWHSMap previousMap = AoWHSMap.Current;
        internal readonly ImageLibraryManager Manager = new ImageLibraryManager(null!);
        internal readonly MapObjectResourceList CombatResources = new MapObjectResourceList(null!);

        internal Fixture()
        {
            var set = EngineFixture.Configure(EngineFixture.CreateGameSet(Manager), value => { value.FCombatResources = CombatResources; });
            AoWHSSet.Current = set;
            Game.Magic.Abilities.GameTables.AoWEngine = null!;
            AoWHSMap.Current = null!;
            AoWHSSet.Current.FUnitGFXResources = null!;
            Runtime.RandSeed = 123456789;
        }

        public void Dispose()
        {
            AoWHSSet.Current = previousSet;
            Game.Magic.Abilities.GameTables.AoWEngine = previousEngine;
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
        "Aow.Game.Engine.Collections.ListObject",
        "Aow.Game.Ai.Groups.AIGroupRazeControl",
        "Aow.Game.Ai.Groups.AIGroupDefendTargetSelector",
        "Aow.Game.Ai.Groups.AIGroupWanderControl",
        "Aow.Game.Ui.Controls.AOWAlignment",
        "Aow.Game.Ui.Controls.TaowRect",
        "Aow.Game.Draw.XYZ",
        "Aow.Game.Draw.XYZList",
        "Aow.Game.Ui.Windows.TaowHTMLMemo+ChangeInfo",
        "Aow.Game.Ui.Controls.ColorChange",
        "Aow.Game.Session.SetupPackage",
        "Aow.Game.Session.RemotePlayer",
        "Aow.Game.Session.RemotePlayerList",
        "Aow.Game.Session.PlayerSetupSettings",
        "Aow.Game.Session.SetupSettings",
        "Aow.Game.Session.CustomizeLeaderControl",
        "Aow.Game.Combat.CombatAbilityRef",
        "Aow.Game.Combat.ROb",
        "Aow.Game.Combat.TaiMove",
        "Aow.Game.Combat.CAI",
        "Aow.Game.Combat.UnitPositionControl",
        "Aow.Game.Combat.CombatMoveControlSettings",
        "Aow.Game.Combat.TacticalCombatUnit",
        "Aow.Game.Combat.TacticalCombatPlayer",
        "Aow.Game.Combat.TacticalCombatPlayerList",
        "Aow.Game.Combat.TacticalCombat",
        "Aow.Game.Combat.RndTacHsm",
        "Aow.Game.Ui.Dialogs.WMCampaignScenarioPage",
        "Aow.Game.Ui.Dialogs.WMCampaignScenarioPageList",
        "Aow.Game.Session.WMCampaignScenario",
        "Aow.Game.Session.WMCampaign",
        "Aow.Game.Ui.Controls.FileTime",
        "Aow.Game.Ai.Groups.AbstractBehaviorAG",
        "Aow.Game.Ai.Groups.AbstractBehaviorAGC",
        "Aow.Game.Ai.Groups.SkirmishAG",
        "Aow.Game.Ai.Groups.SkirmishAGC",
        "Aow.Game.Ai.Groups.NormalAG",
        "Aow.Game.Ai.Groups.NormalAGC",
        "Aow.Game.Ai.Groups.DefenderAG",
        "Aow.Game.Ai.Groups.DefenderAGC",
        "Aow.Game.Ai.Groups.AggressorAG",
        "Aow.Game.Ai.Groups.AggressorAGC",
        "Aow.Game.Ai.Groups.ExpanderAG",
        "Aow.Game.Ai.Groups.ExpanderAGC",
        "Aow.Game.Ai.Groups.ScorcherAG",
        "Aow.Game.Ai.Groups.ScorcherAGC",
        "Aow.Game.World.Structures.CityProductionControl",
        "Aow.Game.Units.MigrationItem",
        "Aow.Game.World.Structures.MigrationList",
        "Aow.Game.World.Structures.CityEnchantment",
        "Aow.Game.Ai.AICityUpgradeBudget",
        "Aow.Game.Ai.AICityResearchBudget",
        "Aow.Game.Ai.AICityFortifyBudget",
        "Aow.Game.Ai.CityAIPA",
        "Aow.Game.Units.PrisonersArmy",
        "Aow.Game.Events.ExecuteEventLogCallBackList",
        "Aow.Game.Events.EventLogList",
        "Aow.Game.Events.EventLogbook",
        "Aow.Game.World.Structures.DefendersArmy",
        "Aow.Game.Engine.Hash.IntegerHashList",
        "Aow.Game.Engine.Hash.IntegerHashTable256",
        "Aow.Game.Engine.Hash.StringHashList",
        "Aow.Game.Engine.Hash.StringHashTable256",
        "Aow.Game.Ai.Groups.GuardAG",
        "Aow.Game.Ai.Groups.GuardAGC",
        "Aow.Game.Ai.Groups.GuardAreaAG",
        "Aow.Game.Ai.Groups.GuardAreaAGC",
        "Aow.Game.Ai.Groups.PatrolAG",
        "Aow.Game.Ai.Groups.PatrolAGC",
        "Aow.Game.Ai.Groups.RefugeAG",
        "Aow.Game.Ai.Groups.RefugeAGC",
        "Aow.Game.Ai.Groups.RaidAG",
        "Aow.Game.Ai.Groups.RaidAGC",
        "Aow.Game.Ai.Groups.ScoutAG",
        "Aow.Game.Ai.Groups.ScoutAGC",
        "Aow.Game.Units.ExplorationSiteItemList",
        "Aow.Game.Ui.Grids.ItemListView",
        "Aow.Game.Ai.Groups.PassiveAG",
        "Aow.Game.Ai.Groups.PassiveAGC",
        "Aow.Game.Ai.Groups.FortifyAG",
        "Aow.Game.Ai.Groups.FortifyAGC",
        "Aow.Game.Ai.Groups.SuicidalAG",
        "Aow.Game.Ai.Groups.SuicidalAGC",
        "Aow.Game.Ai.Groups.TransportAG",
        "Aow.Game.Ai.Groups.TransportAGC",
        "Aow.Game.World.Terrain.CrystalMagic",
        "Aow.Game.Ui.Grids.PageTurnControl",
        "Aow.Game.World.Structures.ProductionPlaceControl",
        "Aow.Game.World.Structures.ShipyardConstructionControl",
        "Aow.Game.Magic.Spells.SmokeCloudEnchantment",
        "Aow.Game.Magic.Spells.StormSpell",
        "Aow.Game.Magic.Spells.FireStormSpell",
        "Aow.Game.Magic.Spells.DivineStormSpell",
        "Aow.Game.Magic.Spells.DeathStormSpell",
        "Aow.Game.Magic.Spells.IceStormSpell",
        "Aow.Game.Magic.Spells.LightningStormSpell",
        "Aow.Game.Magic.Spells.PestilenceStormSpell",
        "Aow.Game.World.Structures.TowerConstructionControl",
        "Aow.Game.Magic.Abilities.UnitView",
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

    private static object Create(Type type, ECustomNode owner, int route)
    {
        if (route == 1) return EClass.Of(type).Create(owner);
        if (route == 2)
        {
            if (EngineHost.Engines is null) EngineHost.Initialize();
            using var engine = new Engine(null!);
            var meta = EClass.Of(type);
            engine.Options -= Set<EngineModeElement>.Of(Engine.eoVerifyClasses);
            try { engine.UnregisterEClasses([engine.GetEClass(meta.ClassID)]); }
            catch (System.Exception error) when (PortedExceptions.Is(error)) { }
            engine.RegisterEClasses([EClass.Of(type)]);
            return engine.CreateEObject(meta.ClassID, owner);
        }
        if (route == 3)
        {
            var meta = EClass.Of(type);
            return (object)Invoke(() => meta.GetType().GetMethod("Create", [typeof(ECustomNode)])!.Invoke(meta, [owner]))!;
        }
        var factory = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).SingleOrDefault(x => x.Name == "Create");
        Type[] signature = typeof(EObject).IsAssignableFrom(type) ? [typeof(ECustomNode)] : [];
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
        Type type = typeof(Spell).Assembly.GetType(name)!;
        bool engineObject = typeof(EObject).IsAssignableFrom(type);
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
                    if (engineObject) Assert.Same(owner, ((EObject)value).Owner);
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
        var value = new ListObject();
        Assert.IsType<ListObject>(value);
        Assert.Equal(Snapshot(new ListObject()), Snapshot(value));
        value.Dispose();
    }

    [Theory]
    [MemberData(nameof(GuardedClasses))]
    public void PlainStormConstructorsKeepOwnerAndSpellState(string name)
    {
        Type type = typeof(Spell).Assembly.GetType(name)!;
        for (int route = 0; route < 4; route++)
        {
            using var fixture = new Fixture();
            var owner = new RemainingOwner();
            var value = Assert.IsAssignableFrom<Spell>(Create(type, owner, route));
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
