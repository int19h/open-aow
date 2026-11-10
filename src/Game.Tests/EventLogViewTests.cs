using System.Reflection;
using System.Text;
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Ui.Dialogs;
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
    private const int PrisonerID = 77;

    private static Type[] EventLogTypes() => EClass.All
        .Where(c => typeof(EventLog).IsAssignableFrom(c.Type))
        .Select(c => c.Type).OrderBy(t => t.Name, StringComparer.Ordinal).ToArray();

    // The arms of the old cascade in their order.
    private static readonly Type[] OldArms =
    [
        typeof(CombatEventLog), typeof(ArmyEventLog), typeof(CityRebellionEventLog), typeof(DiplomaticActionEventLog),
        typeof(NegotiateRequestEventLog), typeof(GameOverEventLog), typeof(NewTurnEventLog), typeof(CombatTypeRequestEventLog),
        typeof(ShowBookEventLog), typeof(PlayerMagicEventLog), typeof(HeroUpgradeEventLog), typeof(HeroJoinEventLog),
        typeof(ArmyInfoEventLog), typeof(PrisonersJoinEventLog), typeof(EnterWizardsTowerEventLog), typeof(EnterArenaEventLog),
        typeof(MessageEventLog), typeof(TutorialMessageEventLog), typeof(FlagMessageEventLog), typeof(HelpMessageEventLog),
        typeof(SaveGameEventLog), typeof(SendEmailGameEventLog), typeof(SMTPEventLog), typeof(PasswordEventLog),
        typeof(ItemExplorationSiteEventLog), typeof(CityProductionEventLog), typeof(ProductionPlaceEventLog), typeof(UserEventLog),
    ];

    // aowMapEvents.pas:198 TheMapExecuteEventLog, past the transient windows.
    private static string OldView(EventLog Event)
    {
        if (Event is CombatEventLog) return "Combat";
        if (Event is ArmyEventLog) return "ArmyEvent";
        if (Event is CityRebellionEventLog) return "CityRebellion";
        if (Event is DiplomaticActionEventLog) return "DiplomaticAction";
        if (Event is NegotiateRequestEventLog)
        {
            var negotiateRequestEventLog = (NegotiateRequestEventLog)Event;
            if (negotiateRequestEventLog.FObject == NegotiateObject.City) return "CityNegotiation";
            if (negotiateRequestEventLog.FObject == NegotiateObject.Army)
            {
                var ArmyHS = (ArmyHS)AoWHSMap.Current.MapContainer.GetField(negotiateRequestEventLog.FX, negotiateRequestEventLog.FY, negotiateRequestEventLog.FL).FindOwnedChild(131607);
                if (ArmyHS != null)
                {
                    var joinSelection = ArmyHS.FArmy.GetJoinSelection(negotiateRequestEventLog.FPlayer);
                    return ArmyHS.FArmy.Hero(joinSelection, RosterTables.AllHeroTypes) == null ? "PartyJoin" : "HeroNegotiation";
                }
            }
            return "None";
        }
        if (Event is GameOverEventLog) return AoWHSMap.Current.FSeatedPlayer == Event.FPlayer ? "GameOver" : "ReceivedMessage";
        if (Event is NewTurnEventLog) return "NewTurn";
        if (Event is CombatTypeRequestEventLog) return "CombatType";
        if (Event is ShowBookEventLog) return "InformationBook";
        if (Event is PlayerMagicEventLog)
        {
            switch (((PlayerMagicEventLog)Event).FMode)
            {
                case PlayerMagicEventLog.PlayerMagicEventMode.SpellResearched: return "SpellResearched";
                case PlayerMagicEventLog.PlayerMagicEventMode.OutOfMana: return "OutOfMana";
                case PlayerMagicEventLog.PlayerMagicEventMode.StartResearch: return "StartResearch";
            }
            return "None";
        }
        if (Event is HeroUpgradeEventLog) return "HeroUpgrade";
        if (Event is HeroJoinEventLog) return "HeroJoin";
        if (Event is ArmyInfoEventLog)
        {
            var armyInfoEventLog = (ArmyInfoEventLog)Event;
            return armyInfoEventLog.FArmyView.Hero(armyInfoEventLog.FSelection, RosterTables.AllHeroTypes) == null ? "ArmyInfo" : "HeroInfo";
        }
        if (Event is PrisonersJoinEventLog)
        {
            var UnitObject = AoWHSMap.Current.FUnitControl.FindUnit(((PrisonersJoinEventLog)Event).FFirstUnitID);
            return UnitObject != null && UnitObject is Hero ? "PrisonerHero" : "PrisonersJoin";
        }
        if (Event is EnterWizardsTowerEventLog) return "WizardsTower";
        if (Event is EnterArenaEventLog) return "Arena";
        if (Event is MessageEventLog) return "Message";
        if (Event is TutorialMessageEventLog) return "Tutorial";
        if (Event is FlagMessageEventLog) return "FlagMessage";
        if (Event is HelpMessageEventLog) return "Help";
        if (Event is SaveGameEventLog) return "SaveGame";
        if (Event is SendEmailGameEventLog) return "SendEmailGame";
        if (Event is SMTPEventLog) return "SMTP";
        if (Event is PasswordEventLog) return "Password";
        if (Event is ItemExplorationSiteEventLog) return "ItemsFound";
        if (Event is CityProductionEventLog) return "CityProduction";
        if (Event is ProductionPlaceEventLog) return "ProductionPlace";
        if (Event is UserEventLog) return "Acknowledge";
        return "None";
    }

    // AI.pas:1626 TProcessEventlogAIPA.Process, the call for one log.
    private static string OldExecuteAI(EventLog Log)
    {
        if (Log is HeroEmergeEventLog) return nameof(HeroEmergeEventLog);
        if (Log is DiplomaticActionEventLog) return nameof(DiplomaticActionEventLog);
        return "None";
    }

    // What the old cascade showed, class by class, in every state it consulted (the states of
    // States below). Recorded from OldView before the cascade went.
    private static readonly Dictionary<string, string> Expected = new(StringComparer.Ordinal)
    {
        ["ArmyEventLog"] = "-=ArmyEvent",
        ["ArmyInfoEventLog"] = "selection=unit=ArmyInfo selection=hero=HeroInfo selection=none=ArmyInfo",
        ["CityProductionEventLog"] = "-=CityProduction",
        ["CityRebellionEventLog"] = "-=CityRebellion",
        ["CombatEventLog"] = "-=Combat",
        ["CombatRestoreHotseatEventLog"] = "-=None",
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
        ["SMTPEventLog"] = "-=SMTP",
        ["SaveGameEventLog"] = "-=SaveGame",
        ["SendEmailGameEventLog"] = "-=SendEmailGame",
        ["ShowBookEventLog"] = "-=InformationBook",
        ["ShowMessageEventLog"] = "-=None",
        ["SpellCastEventLog"] = "-=None",
        ["TextEventLog"] = "-=None",
        ["TutorialMessageEventLog"] = "-=Tutorial",
        ["UserEventLog"] = "-=Acknowledge",
    };

    // A hero whose wish to join the seated player the test sets.
    private sealed class JoiningHero : Hero
    {
        internal bool Joins;

        internal JoiningHero(bool joins) : base(null!)
        {
            Joins = joins;
            FHeroResource = new HeroResource(null!) { FHeroType = HeroType.Hero };
        }

        public override bool CanJoin(sbyte Player) => Joins;
        public override bool OfferToJoin(sbyte Player) => Joins;
        public override bool GetTransporter() => false;
    }

    // Records the player each AI answer comes for.
    private sealed class RecordingAction() : DiplomaticAction(null!)
    {
        internal readonly List<sbyte> Players = [];

        public override void ExecuteAI(sbyte Player) => Players.Add(Player);
    }

    // Counts the unit lookups, the first query of the hero emerge AI.
    private sealed class CountingUnitTable : IntegerHashTable256
    {
        internal int Finds;

        public override object Find(int Key)
        {
            Finds++;
            return base.Find(Key);
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
            yield return new State("seated", (e, _) => e.FPlayer = Seated);
            yield return new State("other", (e, _) => e.FPlayer = OtherPlayer);
        }
        else if (typeof(PlayerMagicEventLog).IsAssignableFrom(type))
        {
            foreach (var mode in new[] { PlayerMagicEventLog.PlayerMagicEventMode.SpellResearched, PlayerMagicEventLog.PlayerMagicEventMode.OutOfMana, PlayerMagicEventLog.PlayerMagicEventMode.StartResearch, (PlayerMagicEventLog.PlayerMagicEventMode)3 })
                yield return new State(mode.ToString(), (e, _) => ((PlayerMagicEventLog)e).FMode = mode);
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
        private readonly AoWHSMap previousMap = AoWHSMap.Current;
        private readonly List<EObject> created = [];
        internal readonly AoWHSMap Map;
        internal readonly MapField Field;
        internal readonly CountingUnitTable Units = new();

        internal Scene()
        {
            Map = new AoWHSMap(null!);
            Map.FSeatedPlayer = Seated;
            Map.FUnitControl = new UnitControl(null!);
            Map.FUnitControl.FUnitTable = Units;
            var container = new MapContainer(null!);
            var level = new MapLevel(container);
            level.CreateMapFieldList(1, 1);
            Map.FMapContainer = container;
            Field = container.GetField(0, 0, 0);
            AoWHSMap.Current = Map;
        }

        internal EventLog Create(Type type)
        {
            var log = (EventLog)EClass.Of(type).Create(null!);
            Assert.IsType(type, log);
            created.Add(log);
            return log;
        }

        // The negotiation at the field, with no army there (Joins null) or an army of one hero.
        internal void Negotiate(EventLog log, NegotiateObject what, bool? Joins)
        {
            var request = (NegotiateRequestEventLog)log;
            request.FObject = what;
            request.FPlayer = Seated;
            request.FX = 0;
            request.FY = 0;
            request.FL = 0;
            while (Field.FindOwnedChild(ArmyHS.ClassID) is HexagonSprite standing)
            {
                Field.RemoveHS(standing);
            }
            if (Joins is bool joins)
            {
                var army = new ArmyHS(Field);
                army.FArmy.FChildren.Add(new JoiningHero(joins));
                Assert.Same(army, Field.FindOwnedChild(ArmyHS.ClassID));
            }
        }

        internal void SelectArmyView(EventLog log, byte selection)
        {
            var info = (ArmyInfoEventLog)log;
            if (info.FArmyView.GetCount() == 0)
            {
                info.FArmyView.FChildren.Add(new Unit(null!));
                info.FArmyView.FChildren.Add(new JoiningHero(false));
            }
            info.FSelection = selection;
        }

        internal void Prisoner(EventLog log, AbstractUnit? unit)
        {
            Units.Delete(PrisonerID);
            if (unit != null) Units.Add(unit, PrisonerID);
            ((PrisonersJoinEventLog)log).FFirstUnitID = PrisonerID;
        }

        public void Dispose()
        {
            AoWHSMap.Current = previousMap;
            foreach (var log in created) _ = Record.Exception(log.Dispose);
        }
    }

    [Fact]
    public void EveryEventLogClassIsCovered()
    {
        var types = EventLogTypes();
        Assert.Equal(44, types.Length);
        Assert.Contains(typeof(EventLog), types);
        Assert.Equal(types.Select(t => t.Name).Order(StringComparer.Ordinal), Expected.Keys.Order(StringComparer.Ordinal));
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
            if (line != Expected[type.Name]) mismatches.Add($"{type.Name}: {line}, recorded {Expected[type.Name]}");
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
        for (int i = 0; i < OldArms.Length; i++)
            for (int j = i + 1; j < OldArms.Length; j++)
                Assert.False(OldArms[i].IsAssignableFrom(OldArms[j]), $"{OldArms[i].Name} precedes its subclass {OldArms[j].Name}");
        var resolved = new StringBuilder();
        foreach (var type in EventLogTypes())
        {
            var declaring = type.GetMethod(nameof(EventLog.View))!.DeclaringType!;
            Type? arm = null;
            for (Type? t = type; t != null && arm == null; t = t.BaseType)
                if (OldArms.Contains(t)) arm = t;
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
        diplomatic.FAction = action;
        diplomatic.ExecuteAI(3);
        Assert.Equal(new[] { (sbyte)3 }, action.Players);

        // The hero emerge AI looks up its hero first. No unit holds the id, so it stops there.
        var emerge = (HeroEmergeEventLog)scene.Create(typeof(HeroEmergeEventLog));
        emerge.FUnitID = 4321;
        int finds = scene.Units.Finds;
        emerge.ExecuteAI(3);
        Assert.Equal(finds + 1, scene.Units.Finds);

        // Every other class leaves the unit table alone.
        foreach (var type in EventLogTypes().Where(t => t != typeof(HeroEmergeEventLog) && t != typeof(DiplomaticActionEventLog)))
        {
            finds = scene.Units.Finds;
            scene.Create(type).ExecuteAI(3);
            Assert.Equal(finds, scene.Units.Finds);
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
        var keys = typeof(EventView).GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(f => (EventView)f.GetValue(null)!).ToArray();
        Assert.Equal(36, keys.Length);
        Assert.Equal(keys.Length, keys.Select(k => k.Name).Distinct().Count());
        foreach (var key in keys.Where(k => k != EventView.None))
            Assert.True(MapEvents.EventRenderers.TryGet(key, out _), $"no renderer for {key}");
        var duplicate = Assert.Throws<InvalidOperationException>(() =>
            MapEvents.EventRenderers.Add(EventView.Message, static (_, _, _, _) => { }));
        Assert.Contains("map event view", duplicate.Message);
    }
}
