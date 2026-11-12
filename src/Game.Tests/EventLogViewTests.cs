using System.Reflection;
using System.Text;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Xunit.Abstractions;

namespace Aow.Game.Tests;

// Brief 9.6-3: the event log view virtuals against the two cascades they replaced. OldView copies
// TheMapExecuteEventLog (aowMapEvents.pas:198) and OldExecuteAI the logbook loop of
// TProcessEventlogAIPA (AI.pas:1626) from main 0ecacee, with each screen call replaced by the name
// of its view key, Done(Event) by "Acknowledge" and each AI call by the name of its class. The
// expectation table records what that cascade showed for every event log class in every state it
// consulted.
[Collection("Engine")]
[EngineState]
public sealed class EventLogViewTests(ITestOutputHelper output)
{
    private const sbyte Seated = 1;
    private const sbyte OtherPlayer = 2;
    private const int PrisonerId = 77;

    private static Type[] EventLogTypes() => EngineClass.All
        .Where(c => typeof(EventLog).IsAssignableFrom(c.Type))
        .Select(c => c.Type).OrderBy(t => t.Name, StringComparer.Ordinal).ToArray();

    // The arms of the old cascade in their order.
    private static readonly Type[] s_oldArms =
    [
        typeof(CombatEventLog), typeof(ArmyEventLog), typeof(CityRebellionEventLog), typeof(DiplomaticActionEventLog),
        typeof(NegotiateRequestEventLog), typeof(GameOverEventLog), typeof(NewTurnEventLog), typeof(CombatTypeRequestEventLog),
        typeof(ShowBookEventLog), typeof(PlayerMagicEventLog), typeof(HeroUpgradeEventLog), typeof(HeroJoinEventLog),
        typeof(ArmyInfoEventLog), typeof(PrisonersJoinEventLog), typeof(EnterWizardsTowerEventLog), typeof(EnterArenaEventLog),
        typeof(MessageEventLog), typeof(TutorialMessageEventLog), typeof(FlagMessageEventLog), typeof(HelpMessageEventLog),
        typeof(SaveGameEventLog), typeof(SendEmailGameEventLog), typeof(SmtpEventLog), typeof(PasswordEventLog),
        typeof(ItemExplorationSiteEventLog), typeof(CityProductionEventLog), typeof(ProductionPlaceEventLog), typeof(UserEventLog),
    ];

    // aowMapEvents.pas:198 TheMapExecuteEventLog, past the transient windows.
    private static string OldView(EventLog eventValue)
    {
        if (eventValue is CombatEventLog) return "Combat";
        if (eventValue is ArmyEventLog) return "ArmyEvent";
        if (eventValue is CityRebellionEventLog) return "CityRebellion";
        if (eventValue is DiplomaticActionEventLog) return "DiplomaticAction";
        if (eventValue is NegotiateRequestEventLog)
        {
            var negotiateRequestEventLog = (NegotiateRequestEventLog)eventValue;
            if (negotiateRequestEventLog.Object == NegotiateObject.City) return "CityNegotiation";
            if (negotiateRequestEventLog.Object == NegotiateObject.Army)
            {
                var armyHexSprite = (ArmyHexSprite)StrategicMap.Current.MapContainer.GetField(negotiateRequestEventLog.X, negotiateRequestEventLog.Y, negotiateRequestEventLog.MapLevel).FindOwnedChild(131607);
                if (armyHexSprite != null)
                {
                    var joinSelection = armyHexSprite.Army.GetJoinSelection(negotiateRequestEventLog._player);
                    return armyHexSprite.Army.Hero(joinSelection, RosterTables.AllHeroTypes) == null ? "PartyJoin" : "HeroNegotiation";
                }
            }
            return "None";
        }
        if (eventValue is GameOverEventLog) return StrategicMap.Current.SeatedPlayer == eventValue._player ? "GameOver" : "ReceivedMessage";
        if (eventValue is NewTurnEventLog) return "NewTurn";
        if (eventValue is CombatTypeRequestEventLog) return "CombatType";
        if (eventValue is ShowBookEventLog) return "InformationBook";
        if (eventValue is PlayerMagicEventLog)
        {
            switch (((PlayerMagicEventLog)eventValue).Mode)
            {
                case PlayerMagicEventLog.EventMode.SpellResearched: return "SpellResearched";
                case PlayerMagicEventLog.EventMode.OutOfMana: return "OutOfMana";
                case PlayerMagicEventLog.EventMode.StartResearch: return "StartResearch";
            }
            return "None";
        }
        if (eventValue is HeroUpgradeEventLog) return "HeroUpgrade";
        if (eventValue is HeroJoinEventLog) return "HeroJoin";
        if (eventValue is ArmyInfoEventLog)
        {
            var armyInfoEventLog = (ArmyInfoEventLog)eventValue;
            return armyInfoEventLog.ArmyView.Hero(armyInfoEventLog.Selection, RosterTables.AllHeroTypes) == null ? "ArmyInfo" : "HeroInfo";
        }
        if (eventValue is PrisonersJoinEventLog)
        {
            var troopObject = StrategicMap.Current.UnitControl.FindUnit(((PrisonersJoinEventLog)eventValue).FirstUnitId);
            return troopObject != null && troopObject is Hero ? "PrisonerHero" : "PrisonersJoin";
        }
        if (eventValue is EnterWizardsTowerEventLog) return "WizardsTower";
        if (eventValue is EnterArenaEventLog) return "Arena";
        if (eventValue is MessageEventLog) return "Message";
        if (eventValue is TutorialMessageEventLog) return "Tutorial";
        if (eventValue is FlagMessageEventLog) return "FlagMessage";
        if (eventValue is HelpMessageEventLog) return "Help";
        if (eventValue is SaveGameEventLog) return "SaveGame";
        if (eventValue is SendEmailGameEventLog) return "SendEmailGame";
        if (eventValue is SmtpEventLog) return "SMTP";
        if (eventValue is PasswordEventLog) return "Password";
        if (eventValue is ItemExplorationSiteEventLog) return "ItemsFound";
        if (eventValue is CityProductionEventLog) return "CityProduction";
        if (eventValue is ProductionPlaceEventLog) return "ProductionPlace";
        if (eventValue is UserEventLog) return "Acknowledge";
        return "None";
    }

    // AI.pas:1626 TProcessEventlogAIPA.Process, the call for one log.
    private static string OldExecuteAI(EventLog log)
    {
        if (log is HeroEmergeEventLog) return nameof(HeroEmergeEventLog);
        if (log is DiplomaticActionEventLog) return nameof(DiplomaticActionEventLog);
        return "None";
    }

    // What the old cascade showed, class by class, in every state it consulted (the states of
    // States below). Recorded from OldView before the cascade went.
    private static readonly Dictionary<string, string> s_expected = new(StringComparer.Ordinal)
    {
        ["ArmyEventLog"] = "-=ArmyEvent",
        ["ArmyInfoEventLog"] = "selection=unit=ArmyInfo selection=hero=HeroInfo selection=none=ArmyInfo",
        ["CityProductionEventLog"] = "-=CityProduction",
        ["CityRebellionEventLog"] = "-=CityRebellion",
        ["CombatEventLog"] = "-=Combat",
        ["CombatTypeRequestEventLog"] = "-=CombatType",
        ["DiplomaticActionEventLog"] = "-=DiplomaticAction",
        ["EndTurnEventLog"] = "-=None",
        ["EnterArenaEventLog"] = "-=Arena",
        ["EnterCaveEventLog"] = "-=Message",
        ["EnterWizardsTowerEventLog"] = "-=WizardsTower",
        ["EventLog"] = "-=None",
        ["FlagMessageEventLog"] = "-=FlagMessage",
        ["GameOverEventLog"] = "seated=GameOver other=ReceivedMessage",
        ["GlobalEnchantmentEventLog"] = "-=None",
        ["HelpMessageEventLog"] = "-=Help",
        ["HeroEmergeEventLog"] = "-=None",
        ["HeroJoinEventLog"] = "-=HeroJoin",
        ["HeroUpgradeEventLog"] = "-=HeroUpgrade",
        ["ItemExplorationSiteEventLog"] = "-=ItemsFound",
        ["JoinEventLog"] = "-=None",
        ["LocationTextEventLog"] = "-=None",
        ["MessageEventLog"] = "-=Message",
        ["NegotiateRequestEventLog"] = "noHero=None noCity=CityNegotiation noArmy,army=none=None noArmy,army=staying=PartyJoin noArmy,army=hero=HeroNegotiation",
        ["NewDayEventLog"] = "-=None",
        ["NewTurnEventLog"] = "-=NewTurn",
        ["PasswordEventLog"] = "-=Password",
        ["PlayerConnectionEventLog"] = "-=Message",
        ["PlayerMagicEventLog"] = "SpellResearched=SpellResearched OutOfMana=OutOfMana StartResearch=StartResearch 3=None",
        ["PrisonersJoinEventLog"] = "prisoner=absent=PrisonersJoin prisoner=unit=PrisonersJoin prisoner=hero=PrisonerHero",
        ["ProductionEventLog"] = "-=None",
        ["ProductionPlaceEventLog"] = "-=ProductionPlace",
        ["RazeStructureEventLog"] = "-=Message",
        ["RestartGameEventLog"] = "-=Message",
        ["SmtpEventLog"] = "-=SMTP",
        ["SaveGameEventLog"] = "-=SaveGame",
        ["SendEmailGameEventLog"] = "-=SendEmailGame",
        ["ShowBookEventLog"] = "-=InformationBook",
        ["SpellCastEventLog"] = "-=None",
        ["TextEventLog"] = "-=None",
        ["TutorialMessageEventLog"] = "-=Tutorial",
        ["UserEventLog"] = "-=Acknowledge",
    };

    // A hero whose wish to join the seated player the test sets.
    private sealed class JoiningHero : Hero
    {
        private bool _joins;

        internal JoiningHero(bool joins) : base(null!)
        {
            _joins = joins;
            HeroResource = new HeroResource(null!) { HeroType = HeroType.Hero };
        }

        public override bool CanJoin(sbyte player) => _joins;
        public override bool OfferToJoin(sbyte player) => _joins;
        public override bool GetTransporter() => false;
    }

    // Records the player each AI answer comes for.
    private sealed class RecordingAction() : DiplomaticAction(null!)
    {
        internal List<sbyte> Players { get; } = [];

        public override void ExecuteAI(sbyte player) => Players.Add(player);
    }

    // Counts the unit lookups. An empty dictionary returns a miss without hashing, so the table holds one seed.
    private sealed class CountingInt32Comparer : IEqualityComparer<int>
    {
        internal int Finds { get; private set; }

        public bool Equals(int left, int right) => left == right;

        public int GetHashCode(int value)
        {
            Finds++;
            return value;
        }
    }

    // One consulted state. The name lists the state, "-" for a class that consults none.
    private sealed record State(string Name, Action<EventLog, Scene> Apply)
    {
        public override string ToString() => Name;
    }

    private static IEnumerable<State> States(Type type)
    {
        if (typeof(NegotiateRequestEventLog).IsAssignableFrom(type))
        {
            yield return new State("noHero", (e, s) => s.Negotiate(e, NegotiateObject.Hero, null));
            yield return new State("noCity", (e, s) => s.Negotiate(e, NegotiateObject.City, null));
            yield return new State("noArmy,army=none", (e, s) => s.Negotiate(e, NegotiateObject.Army, null));
            yield return new State("noArmy,army=staying", (e, s) => s.Negotiate(e, NegotiateObject.Army, false));
            yield return new State("noArmy,army=hero", (e, s) => s.Negotiate(e, NegotiateObject.Army, true));
        }
        else if (typeof(GameOverEventLog).IsAssignableFrom(type))
        {
            yield return new State("seated", (e, _) => e._player = Seated);
            yield return new State("other", (e, _) => e._player = OtherPlayer);
        }
        else if (typeof(PlayerMagicEventLog).IsAssignableFrom(type))
        {
            foreach (var mode in new[] { PlayerMagicEventLog.EventMode.SpellResearched, PlayerMagicEventLog.EventMode.OutOfMana, PlayerMagicEventLog.EventMode.StartResearch, (PlayerMagicEventLog.EventMode)3 })
                yield return new State(mode.ToString(), (e, _) => ((PlayerMagicEventLog)e).Mode = mode);
        }
        else if (typeof(ArmyInfoEventLog).IsAssignableFrom(type))
        {
            // The army view holds a unit and a hero, the selection picks either or neither.
            yield return new State("selection=unit", (e, s) => s.SelectArmyView(e, 1));
            yield return new State("selection=hero", (e, s) => s.SelectArmyView(e, 2));
            yield return new State("selection=none", (e, s) => s.SelectArmyView(e, 0));
        }
        else if (typeof(PrisonersJoinEventLog).IsAssignableFrom(type))
        {
            yield return new State("prisoner=absent", (e, s) => s.Prisoner(e, null));
            yield return new State("prisoner=unit", (e, s) => s.Prisoner(e, new Unit(null!)));
            yield return new State("prisoner=hero", (e, s) => s.Prisoner(e, new Hero(null!)));
        }
        else
        {
            yield return new State("-", (_, _) => { });
        }
    }

    private sealed class Scene : IDisposable
    {
        private readonly StrategicMap _previousMap = StrategicMap.Current;
        private readonly List<EngineObject> _created = [];
        private readonly StrategicMap _map;
        private readonly MapField _field;

        internal CountingInt32Comparer UnitFinds { get; } = new();

        private readonly Dictionary<int, Troop> _units;

        internal Scene()
        {
            _map = new StrategicMap(null!);
            _map.SeatedPlayer = Seated;
            _map.UnitControl = new UnitControl(null!);
            _units = new Dictionary<int, Troop>(UnitFinds);
            _units.Add(key: -1, value: new Unit(null!));
            _map.UnitControl.UnitTable = _units;
            var container = new MapContainer(null!);
            var level = new MapLevel(container);
            level.CreateMapFieldList(1, 1);
            _map.MapContainer = container;
            _field = container.GetField(0, 0, 0);
            StrategicMap.Current = _map;
        }

        internal EventLog Create(Type type)
        {
            var log = (EventLog)EngineClass.Of(type).CreateInstance(null!);
            Assert.IsType(type, log);
            _created.Add(log);
            return log;
        }

        // The negotiation at the field, with no army there (Joins null) or an army of one hero.
        internal void Negotiate(EventLog log, NegotiateObject what, bool? joins)
        {
            var request = (NegotiateRequestEventLog)log;
            request.Object = what;
            request._player = Seated;
            request.X = 0;
            request.Y = 0;
            request.MapLevel = 0;
            while (_field.FindOwnedChild(ArmyHexSprite.ClassId) is HexSprite standing)
            {
                _field.RemoveHexSprite(standing);
            }
            if (joins is bool joinsLocal)
            {
                var army = new ArmyHexSprite(_field);
                army.Army.AddChild(new JoiningHero(joinsLocal));
                Assert.Same(army, _field.FindOwnedChild(ArmyHexSprite.ClassId));
            }
        }

        internal void SelectArmyView(EventLog log, byte selection)
        {
            var info = (ArmyInfoEventLog)log;
            if (info.ArmyView.GetCount() == 0)
            {
                info.ArmyView.AddChild(new Unit(null!));
                info.ArmyView.AddChild(new JoiningHero(false));
            }
            info.Selection = selection;
        }

        internal void Prisoner(EventLog log, Troop? troop)
        {
            _units.Remove(PrisonerId);
            if (troop != null) _units.Add(key: PrisonerId, value: troop);
            ((PrisonersJoinEventLog)log).FirstUnitId = PrisonerId;
        }

        public void Dispose()
        {
            StrategicMap.Current = _previousMap;
            foreach (var log in _created) _ = Record.Exception(log.Dispose);
        }
    }

    [Fact]
    public void EveryEventLogClassIsCovered()
    {
        var types = EventLogTypes();
        Assert.Equal(42, types.Length);
        Assert.Contains(typeof(EventLog), types);
        Assert.Equal(types.Select(t => t.Name).Order(StringComparer.Ordinal), s_expected.Keys.Order(StringComparer.Ordinal));
    }

    [Fact]
    public void ViewMatchesTheOldCascade()
    {
        var mismatches = new List<string>();
        var table = new StringBuilder();
        using var scene = new Scene();
        foreach (var type in EventLogTypes())
        {
            var log = scene.Create(type);
            var row = new List<string>();
            foreach (var state in States(type))
            {
                state.Apply(log, scene);
                string old = OldView(log);
                var view = log.View();
                if (view.Name != old) mismatches.Add($"{type.Name} {state}: {view.Name}, old cascade {old}");
                if (view != EventView.None)
                    Assert.True(MapEvents.EventRenderers.TryGet(view, out _), $"{type.Name} {state}: no renderer for {view}");
                row.Add($"{state}={view.Name}");
            }
            string line = string.Join(" ", row);
            if (line != s_expected[type.Name]) mismatches.Add($"{type.Name}: {line}, recorded {s_expected[type.Name]}");
            table.AppendLine($"{type.Name}: {line}");
        }
        output.WriteLine(table.ToString());
        Assert.Empty(mismatches);
    }

    // No arm of the old cascade tests a base before one of its subclasses, so its first match is the
    // nearest ancestor with an arm. Each class resolves View to an override on that ancestor, or to
    // TEventLog where no ancestor has an arm.
    [Fact]
    public void ViewResolvesToTheNearestOldArm()
    {
        for (int i = 0; i < s_oldArms.Length; i++)
            for (int j = i + 1; j < s_oldArms.Length; j++)
                Assert.False(s_oldArms[i].IsAssignableFrom(s_oldArms[j]), $"{s_oldArms[i].Name} precedes its subclass {s_oldArms[j].Name}");
        var resolved = new StringBuilder();
        foreach (var type in EventLogTypes())
        {
            var declaring = type.GetMethod(nameof(EventLog.View))!.DeclaringType!;
            Type? arm = null;
            for (Type? t = type; t != null && arm == null; t = t.BaseType)
                if (s_oldArms.Contains(t)) arm = t;
            Assert.Equal(arm ?? typeof(EventLog), declaring);
            resolved.AppendLine($"{type.Name}: View {declaring.Name}, old arm {arm?.Name ?? "none"}, ExecuteAI {type.GetMethod(nameof(EventLog.ExecuteAI), [typeof(sbyte)])!.DeclaringType!.Name}");
        }
        output.WriteLine(resolved.ToString());
    }

    [Fact]
    public void ExecuteAIReachesTheTwoOverridesOnly()
    {
        using var scene = new Scene();
        var mismatches = new List<string>();
        foreach (var type in EventLogTypes())
        {
            var declaring = type.GetMethod(nameof(EventLog.ExecuteAI), [typeof(sbyte)])!.DeclaringType!;
            var log = scene.Create(type);
            string old = OldExecuteAI(log);
            string actual = declaring == typeof(EventLog) ? "None" : declaring.Name;
            if (actual != old) mismatches.Add($"{type.Name}: {actual}, old cascade {old}");
        }
        Assert.Empty(mismatches);

        var diplomatic = (DiplomaticActionEventLog)scene.Create(typeof(DiplomaticActionEventLog));
        var action = new RecordingAction();
        diplomatic.Action = action;
        diplomatic.ExecuteAI(3);
        Assert.Equal(new[] { (sbyte)3 }, action.Players);

        // The hero emerge AI looks up its hero first. No unit holds the id, so it stops there.
        var emerge = (HeroEmergeEventLog)scene.Create(typeof(HeroEmergeEventLog));
        emerge.UnitId = 4321;
        int finds = scene.UnitFinds.Finds;
        emerge.ExecuteAI(3);
        Assert.Equal(finds + 1, scene.UnitFinds.Finds);

        // Every other class leaves the unit table alone.
        foreach (var type in EventLogTypes().Where(t => t != typeof(HeroEmergeEventLog) && t != typeof(DiplomaticActionEventLog)))
        {
            finds = scene.UnitFinds.Finds;
            scene.Create(type).ExecuteAI(3);
            Assert.Equal(finds, scene.UnitFinds.Finds);
        }
    }

    // None calls no renderer and leaves the event undone, Acknowledge reports it done at once.
    [Fact]
    public void TheScreenAcknowledgesUserEventsAndSkipsNone()
    {
        using var scene = new Scene();
        var screen = new MapEvents(null!);
        var done = new List<EventLog>();
        var user = scene.Create(typeof(UserEventLog));
        screen.TheMapExecuteEventLog(user, true, done.Add);
        Assert.Equal(new[] { user }, done);
        foreach (var type in EventLogTypes())
        {
            var log = scene.Create(type);
            if (log.View() != EventView.None) continue;
            screen.TheMapExecuteEventLog(log, true, done.Add);
        }
        Assert.Equal(new[] { user }, done);
    }

    [Fact]
    public void TheScreenRegistersEveryOriginalView()
    {
        Assert.Equal(35, MapEvents.EventRenderers.All.Count);
        Assert.False(MapEvents.EventRenderers.TryGet(EventView.None, out _));
        var keys = typeof(EventView).GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(property => property.PropertyType == typeof(EventView))
            .Select(property => (EventView)property.GetValue(null)!).ToArray();
        Assert.Equal(36, keys.Length);
        Assert.Equal(keys.Length, keys.Select(k => k.Name).Distinct().Count());
        foreach (var key in keys.Where(k => k != EventView.None))
            Assert.True(MapEvents.EventRenderers.TryGet(key, out _), $"no renderer for {key}");
        var duplicate = Assert.Throws<InvalidOperationException>(() =>
            MapEvents.EventRenderers.Add(EventView.Message, static (_, _, _, _) => { }));
        Assert.Contains("map event view", duplicate.Message);
    }
}
