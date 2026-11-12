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
public sealed class RemainingConstructionTests
{
    private sealed class RemainingOwner : EngineFixture.Owner
    {

        internal List<string> Observations { get; } = [];

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
        private readonly AoWHSSet _previousSet = AoWHSSet.Current;
        private readonly GameEngine _previousEngine = GameTables.GameEngine;
        private readonly AoWHSMap _previousMap = AoWHSMap.Current;
        private readonly ImageLibraryManager _manager = new ImageLibraryManager(null!);
        private readonly MapObjectResourceList _combatResources = new MapObjectResourceList(null!);

        internal Fixture()
        {
            var set = EngineFixture.Configure(EngineFixture.CreateGameSet(_manager), value => { value.CombatResources = _combatResources; });
            AoWHSSet.Current = set;
            GameTables.GameEngine = null!;
            AoWHSMap.Current = null!;
            AoWHSSet.Current.UnitGraphicsResources = null!;
            Runtime.RandSeed = 123456789;
        }

        public void Dispose()
        {
            AoWHSSet.Current = _previousSet;
            GameTables.GameEngine = _previousEngine;
            AoWHSMap.Current = _previousMap;
            _combatResources.Dispose();
            _manager.Dispose();
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
                foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(MemberNames.Of, StringComparer.Ordinal))
                {
                    if (field.Name is "_engineClass" or "_actionClass" or "_groupClass") continue; // the descriptor caches are not constructor state
                    string key = current.Name + "." + MemberNames.Of(field);
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
        "Aow.Game.AI.Groups.AIGroupRazeControl",
        "Aow.Game.AI.Groups.AIGroupDefendTargetSelector",
        "Aow.Game.AI.Groups.AIGroupWanderControl",
        "Aow.Game.UI.Controls.Alignment",
        "Aow.Game.UI.Controls.DirtyRectangle",
        "Aow.Game.Draw.Point3D",
        "Aow.Game.Draw.Point3DList",
        "Aow.Game.UI.Windows.HtmlMemo+ChangeInfo",
        "Aow.Game.UI.Controls.ColorChange",
        "Aow.Game.Session.SetupPackage",
        "Aow.Game.Session.RemotePlayer",
        "Aow.Game.Session.RemotePlayerList",
        "Aow.Game.Session.PlayerSetupSettings",
        "Aow.Game.Session.SetupSettings",
        "Aow.Game.Session.CustomizeLeaderControl",
        "Aow.Game.Combat.CombatAbilityRef",
        "Aow.Game.Combat.RangedObstacle",
        "Aow.Game.Combat.BattleAIMove",
        "Aow.Game.Combat.BattleAI",
        "Aow.Game.Combat.UnitPositionControl",
        "Aow.Game.Combat.CombatMoveControlSettings",
        "Aow.Game.Combat.TacticalBattleUnit",
        "Aow.Game.Combat.TacticalBattlePlayer",
        "Aow.Game.Combat.TacticalBattlePlayerList",
        "Aow.Game.Combat.TacticalBattle",
        "Aow.Game.Combat.RandomBattleMapGenerator",
        "Aow.Game.UI.Dialogs.CampaignScenarioPage",
        "Aow.Game.UI.Dialogs.WMCampaignScenarioPageList",
        "Aow.Game.Session.WorldMapCampaignScenario",
        "Aow.Game.Session.WorldMapCampaign",
        "Aow.Game.UI.Controls.FileTime",
        "Aow.Game.AI.Groups.BehaviorAIGroup",
        "Aow.Game.AI.Groups.BehaviorAIGroupControl",
        "Aow.Game.AI.Groups.SkirmishAIGroup",
        "Aow.Game.AI.Groups.SkirmishAIGroupControl",
        "Aow.Game.AI.Groups.NormalAIGroup",
        "Aow.Game.AI.Groups.NormalAIGroupControl",
        "Aow.Game.AI.Groups.DefenderAIGroup",
        "Aow.Game.AI.Groups.DefenderAIGroupControl",
        "Aow.Game.AI.Groups.AggressorAIGroup",
        "Aow.Game.AI.Groups.AggressorAIGroupControl",
        "Aow.Game.AI.Groups.ExpanderAIGroup",
        "Aow.Game.AI.Groups.ExpanderAIGroupControl",
        "Aow.Game.AI.Groups.ScorcherAIGroup",
        "Aow.Game.AI.Groups.ScorcherAIGroupControl",
        "Aow.Game.World.Structures.CityProductionControl",
        "Aow.Game.Units.MigrationItem",
        "Aow.Game.World.Structures.MigrationList",
        "Aow.Game.World.Structures.CityEnchantment",
        "Aow.Game.AI.AICityUpgradeBudget",
        "Aow.Game.AI.AICityResearchBudget",
        "Aow.Game.AI.AICityFortifyBudget",
        "Aow.Game.AI.CityAIPlayerAction",
        "Aow.Game.Units.PrisonersArmy",
        "Aow.Game.Events.ExecuteEventLogCallbackList",
        "Aow.Game.Events.EventLogList",
        "Aow.Game.Events.EventLogbook",
        "Aow.Game.World.Structures.DefendersArmy",
        "Aow.Game.AI.Groups.GuardAIGroup",
        "Aow.Game.AI.Groups.GuardAIGroupControl",
        "Aow.Game.AI.Groups.GuardAreaAIGroup",
        "Aow.Game.AI.Groups.GuardAreaAIGroupControl",
        "Aow.Game.AI.Groups.PatrolAIGroup",
        "Aow.Game.AI.Groups.PatrolAIGroupControl",
        "Aow.Game.AI.Groups.RefugeAIGroup",
        "Aow.Game.AI.Groups.RefugeAIGroupControl",
        "Aow.Game.AI.Groups.RaidAIGroup",
        "Aow.Game.AI.Groups.RaidAIGroupControl",
        "Aow.Game.AI.Groups.ScoutAIGroup",
        "Aow.Game.AI.Groups.ScoutAIGroupControl",
        "Aow.Game.Units.ExplorationSiteItemList",
        "Aow.Game.AI.Groups.PassiveAIGroup",
        "Aow.Game.AI.Groups.PassiveAIGroupControl",
        "Aow.Game.AI.Groups.FortifyAIGroup",
        "Aow.Game.AI.Groups.FortifyAIGroupControl",
        "Aow.Game.AI.Groups.SuicidalAIGroup",
        "Aow.Game.AI.Groups.SuicidalAIGroupControl",
        "Aow.Game.AI.Groups.TransportAIGroup",
        "Aow.Game.AI.Groups.TransportAIGroupControl",
        "Aow.Game.UI.Grids.PageTurnControl",
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
    ];

    public static IEnumerable<object[]> Classes => s_names.Select(name => new object[] { name });
    public static IEnumerable<object[]> GuardedClasses => s_names.Where(name => name.StartsWith("Aow.Game.Magic.Spells.", StringComparison.Ordinal) && name.EndsWith("StormSpell", StringComparison.Ordinal)).Select(name => new object[] { name });

    private static object? Invoke(Func<object?> action)
    {
        try { return action(); }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }

    private static object Create(Type type, ECustomNode owner, int route)
    {
        if (route == 1) return EngineClass.Of(type).CreateInstance(owner);
        if (route == 2)
        {
            if (EngineHost.Engines is null) EngineHost.Initialize();
            using GameEngine engine = new GameEngine(null!);
            var meta = EngineClass.Of(type);
            engine.Options -= Set<EngineModeElement>.Of(GameEngine.VerifyClasses);
            try { engine.UnregisterEngineClasses([engine.GetEngineClass(meta.ClassId)]); }
            catch (Exception error) when (PortedExceptions.Is(error)) { }
            engine.RegisterEngineClasses([EngineClass.Of(type)]);
            return engine.CreateEngineObject(meta.ClassId, owner);
        }
        if (route == 3)
        {
            var meta = EngineClass.Of(type);
            return (object)Invoke(() => meta.GetType().GetMethod("CreateInstance", [typeof(ECustomNode)])!.Invoke(meta, [owner]))!;
        }
        var factory = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).SingleOrDefault(x => x.Name == "Create");
        Type[] signature = typeof(EngineObject).IsAssignableFrom(type) ? [typeof(global::Aow.Engine.Nodes.ECustomNode)] : [];
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
        bool engineObject = typeof(EngineObject).IsAssignableFrom(type);
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
                    if (engineObject) Assert.Same(owner, ((EngineObject)value).Owner);
                    state = "success:" + Snapshot(value);
                }
                catch (Exception error) { state = "failure:" + error.GetType().FullName + ":" + error.Message; }
                finally
                {
                    try { (value as IDisposable)?.Dispose(); }
                    catch (Exception error) { states.Add("destroy:" + error.GetType().FullName + ":" + error.Message); }
                }
                states.Add(state);
                states.Add("owner:" + (owner?.Members.Count ?? 0));
                if (owner is not null) states.Add(JsonSerializer.Serialize(owner.Observations));
                Check(name, "owner" + ownerMode + "route" + route, string.Join("\n", states));
                Assert.StartsWith("success:", state);
            }
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
            Assert.NotNull(value.SpellText);
            Assert.NotNull(value.CastImages);
            value.Dispose();
            Assert.Empty(owner.Members);
            Assert.Equal(1, owner.Removals);
        }
    }
}
