using System.Text;
using Aow.Units.AoWE;
using Aow.Units.aowMapEvents;
using Aow.Units.Arena;
using Aow.Units.City;
using Aow.Units.Dungeon;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using Aow.Units.FlagEvents;
using Aow.Units.HSEngine;
using Aow.Units.ItemExpS;
using Aow.Units.PrdPlace;
using Aow.Units.WizardsTower;
using Xunit.Abstractions;
using static Aow.Units.E2CampaignSupport.E2CampaignSupportUnit;
using Game = Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Tests;

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
        .Where(c => typeof(TEventLog).IsAssignableFrom(c.Type))
        .Select(c => c.Type).OrderBy(t => t.Name, StringComparer.Ordinal).ToArray();

    // The arms of the old cascade in their order.
    private static readonly Type[] OldArms =
    [
        typeof(TCombatEventLog), typeof(TArmyEventLog), typeof(TCityRebellionEventLog), typeof(TDiplomaticActionEventLog),
        typeof(TNegotiateRequestEventLog), typeof(TGameOverEventLog), typeof(TNewTurnEventLog), typeof(TCombatTypeRequestEventLog),
        typeof(TShowBookEventLog), typeof(TPlayerMagicEventLog), typeof(THeroUpgradeEventLog), typeof(THeroJoinEventLog),
        typeof(TArmyInfoEventLog), typeof(TPrisonersJoinEventLog), typeof(TEnterWizardsTowerEventLog), typeof(TEnterArenaEventLog),
        typeof(TMessageEventLog), typeof(TTutorialMessageEventLog), typeof(TFlagMessageEventLog), typeof(THelpMessageEventLog),
        typeof(TSaveGameEventLog), typeof(TSendEmailGameEventLog), typeof(TSMTPEventLog), typeof(TPasswordEventLog),
        typeof(TItemExplorationSiteEventLog), typeof(TCityProductionEventLog), typeof(TProductionPlaceEventLog), typeof(TUserEventLog),
    ];

    // aowMapEvents.pas:198 TheMapExecuteEventLog, past the transient windows.
    private static string OldView(TEventLog Event)
    {
        if (Event is TCombatEventLog) return "Combat";
        if (Event is TArmyEventLog) return "ArmyEvent";
        if (Event is TCityRebellionEventLog) return "CityRebellion";
        if (Event is TDiplomaticActionEventLog) return "DiplomaticAction";
        if (Event is TNegotiateRequestEventLog)
        {
            var negotiateRequestEventLog = (TNegotiateRequestEventLog)Event;
            if (negotiateRequestEventLog.FObject == TNegotiateObject.noCity) return "CityNegotiation";
            if (negotiateRequestEventLog.FObject == TNegotiateObject.noArmy)
            {
                var ArmyHS = (TArmyHS)Game.AoWHSMap.MapContainer.GetField(negotiateRequestEventLog.FX, negotiateRequestEventLog.FY, negotiateRequestEventLog.FL).FindOwnedChild(131607);
                if (ArmyHS != null)
                {
                    var joinSelection = ArmyHS.FArmy.GetJoinSelection(negotiateRequestEventLog.FPlayer);
                    return ArmyHS.FArmy.Hero(joinSelection, AllHeroTypes) == null ? "PartyJoin" : "HeroNegotiation";
                }
            }
            return "None";
        }
        if (Event is TGameOverEventLog) return Game.AoWHSMap.FSeatedPlayer == Event.FPlayer ? "GameOver" : "ReceivedMessage";
        if (Event is TNewTurnEventLog) return "NewTurn";
        if (Event is TCombatTypeRequestEventLog) return "CombatType";
        if (Event is TShowBookEventLog) return "InformationBook";
        if (Event is TPlayerMagicEventLog)
        {
            switch (((TPlayerMagicEventLog)Event).FMode)
            {
                case TPlayerMagicEventMode.pmemSpellResearched: return "SpellResearched";
                case TPlayerMagicEventMode.pmemOutOfMana: return "OutOfMana";
                case TPlayerMagicEventMode.pmemStartResearch: return "StartResearch";
            }
            return "None";
        }
        if (Event is THeroUpgradeEventLog) return "HeroUpgrade";
        if (Event is THeroJoinEventLog) return "HeroJoin";
        if (Event is TArmyInfoEventLog)
        {
            var armyInfoEventLog = (TArmyInfoEventLog)Event;
            return armyInfoEventLog.FArmyView.Hero(armyInfoEventLog.FSelection, AllHeroTypes) == null ? "ArmyInfo" : "HeroInfo";
        }
        if (Event is TPrisonersJoinEventLog)
        {
            var UnitObject = Game.AoWHSMap.FUnitControl.FindUnit(((TPrisonersJoinEventLog)Event).FFirstUnitID);
            return UnitObject != null && UnitObject is THero ? "PrisonerHero" : "PrisonersJoin";
        }
        if (Event is TEnterWizardsTowerEventLog) return "WizardsTower";
        if (Event is TEnterArenaEventLog) return "Arena";
        if (Event is TMessageEventLog) return "Message";
        if (Event is TTutorialMessageEventLog) return "Tutorial";
        if (Event is TFlagMessageEventLog) return "FlagMessage";
        if (Event is THelpMessageEventLog) return "Help";
        if (Event is TSaveGameEventLog) return "SaveGame";
        if (Event is TSendEmailGameEventLog) return "SendEmailGame";
        if (Event is TSMTPEventLog) return "SMTP";
        if (Event is TPasswordEventLog) return "Password";
        if (Event is TItemExplorationSiteEventLog) return "ItemsFound";
        if (Event is TCityProductionEventLog) return "CityProduction";
        if (Event is TProductionPlaceEventLog) return "ProductionPlace";
        if (Event is TUserEventLog) return "Acknowledge";
        return "None";
    }

    // AI.pas:1626 TProcessEventlogAIPA.Process, the call for one log.
    private static string OldExecuteAI(TEventLog Log)
    {
        if (Log is THeroEmergeEventLog) return nameof(THeroEmergeEventLog);
        if (Log is TDiplomaticActionEventLog) return nameof(TDiplomaticActionEventLog);
        return "None";
    }

    // What the old cascade showed, class by class, in every state it consulted (the states of
    // States below). Recorded from OldView before the cascade went.
    private static readonly Dictionary<string, string> Expected = new(StringComparer.Ordinal)
    {
        ["TArmyEventLog"] = "-=ArmyEvent",
        ["TArmyInfoEventLog"] = "selection=unit=ArmyInfo selection=hero=HeroInfo selection=none=ArmyInfo",
        ["TCityProductionEventLog"] = "-=CityProduction",
        ["TCityRebellionEventLog"] = "-=CityRebellion",
        ["TCombatEventLog"] = "-=Combat",
        ["TCombatRestoreHotseatEventLog"] = "-=None",
        ["TCombatTypeRequestEventLog"] = "-=CombatType",
        ["TDiplomaticActionEventLog"] = "-=DiplomaticAction",
        ["TEndTurnEventLog"] = "-=None",
        ["TEnterArenaEventLog"] = "-=Arena",
        ["TEnterCaveEventLog"] = "-=Message",
        ["TEnterWizardsTowerEventLog"] = "-=WizardsTower",
        ["TEventLog"] = "-=None",
        ["TFlagMessageEventLog"] = "-=FlagMessage",
        ["TGameOverEventLog"] = "seated=GameOver other=ReceivedMessage",
        ["TGlobalEnchantmentEventLog"] = "-=None",
        ["THelpMessageEventLog"] = "-=Help",
        ["THeroEmergeEventLog"] = "-=None",
        ["THeroJoinEventLog"] = "-=HeroJoin",
        ["THeroUpgradeEventLog"] = "-=HeroUpgrade",
        ["TItemExplorationSiteEventLog"] = "-=ItemsFound",
        ["TJoinEventLog"] = "-=None",
        ["TLocationTextEventLog"] = "-=None",
        ["TMessageEventLog"] = "-=Message",
        ["TNegotiateRequestEventLog"] = "noHero=None noCity=CityNegotiation noArmy,army=none=None noArmy,army=staying=PartyJoin noArmy,army=hero=HeroNegotiation",
        ["TNewDayEventLog"] = "-=None",
        ["TNewTurnEventLog"] = "-=NewTurn",
        ["TPasswordEventLog"] = "-=Password",
        ["TPlayerConnectionEventLog"] = "-=Message",
        ["TPlayerMagicEventLog"] = "pmemSpellResearched=SpellResearched pmemOutOfMana=OutOfMana pmemStartResearch=StartResearch 3=None",
        ["TPrisonersJoinEventLog"] = "prisoner=absent=PrisonersJoin prisoner=unit=PrisonersJoin prisoner=hero=PrisonerHero",
        ["TProductionEventLog"] = "-=None",
        ["TProductionPlaceEventLog"] = "-=ProductionPlace",
        ["TRazeStructureEventLog"] = "-=Message",
        ["TRestartGameEventLog"] = "-=Message",
        ["TSMTPEventLog"] = "-=SMTP",
        ["TSaveGameEventLog"] = "-=SaveGame",
        ["TSendEmailGameEventLog"] = "-=SendEmailGame",
        ["TShowBookEventLog"] = "-=InformationBook",
        ["TShowMessageEventLog"] = "-=None",
        ["TSpellCastEventLog"] = "-=None",
        ["TTextEventLog"] = "-=None",
        ["TTutorialMessageEventLog"] = "-=Tutorial",
        ["TUserEventLog"] = "-=Acknowledge",
    };

    // A hero whose wish to join the seated player the test sets.
    private sealed class JoiningHero : THero
    {
        internal bool Joins;

        internal JoiningHero(bool joins) : base(null!)
        {
            Joins = joins;
            FHeroResource = new THeroResource(null!) { FHeroType = THeroType.htHero };
        }

        public override bool CanJoin(sbyte Player) => Joins;
        public override bool OfferToJoin(sbyte Player) => Joins;
        public override bool GetTransporter() => false;
    }

    // Records the player each AI answer comes for.
    private sealed class RecordingAction() : TDiplomaticAction(null!)
    {
        internal readonly List<sbyte> Players = [];

        public override void ExecuteAI(sbyte Player) => Players.Add(Player);
    }

    // Counts the unit lookups, the first query of the hero emerge AI.
    private sealed class CountingUnitTable : TIntegerHashTable256
    {
        internal int Finds;

        public override object Find(int Key)
        {
            Finds++;
            return base.Find(Key);
        }
    }

    // One consulted state. The name lists the state, "-" for a class that consults none.
    private sealed record State(string Name, Action<TEventLog, Scene> Apply)
    {
        public override string ToString() => Name;
    }

    private static IEnumerable<State> States(Type type)
    {
        if (typeof(TNegotiateRequestEventLog).IsAssignableFrom(type))
        {
            yield return new State("noHero", (e, s) => s.Negotiate(e, TNegotiateObject.noHero, null));
            yield return new State("noCity", (e, s) => s.Negotiate(e, TNegotiateObject.noCity, null));
            yield return new State("noArmy,army=none", (e, s) => s.Negotiate(e, TNegotiateObject.noArmy, null));
            yield return new State("noArmy,army=staying", (e, s) => s.Negotiate(e, TNegotiateObject.noArmy, false));
            yield return new State("noArmy,army=hero", (e, s) => s.Negotiate(e, TNegotiateObject.noArmy, true));
        }
        else if (typeof(TGameOverEventLog).IsAssignableFrom(type))
        {
            yield return new State("seated", (e, _) => e.FPlayer = Seated);
            yield return new State("other", (e, _) => e.FPlayer = OtherPlayer);
        }
        else if (typeof(TPlayerMagicEventLog).IsAssignableFrom(type))
        {
            foreach (var mode in new[] { TPlayerMagicEventMode.pmemSpellResearched, TPlayerMagicEventMode.pmemOutOfMana, TPlayerMagicEventMode.pmemStartResearch, (TPlayerMagicEventMode)3 })
                yield return new State(mode.ToString(), (e, _) => ((TPlayerMagicEventLog)e).FMode = mode);
        }
        else if (typeof(TArmyInfoEventLog).IsAssignableFrom(type))
        {
            // The army view holds a unit and a hero, the selection picks either or neither.
            yield return new State("selection=unit", (e, s) => s.SelectArmyView(e, 1));
            yield return new State("selection=hero", (e, s) => s.SelectArmyView(e, 2));
            yield return new State("selection=none", (e, s) => s.SelectArmyView(e, 0));
        }
        else if (typeof(TPrisonersJoinEventLog).IsAssignableFrom(type))
        {
            yield return new State("prisoner=absent", (e, s) => s.Prisoner(e, null));
            yield return new State("prisoner=unit", (e, s) => s.Prisoner(e, new TUnit(null!)));
            yield return new State("prisoner=hero", (e, s) => s.Prisoner(e, new THero(null!)));
        }
        else
        {
            yield return new State("-", (_, _) => { });
        }
    }

    private sealed class Scene : IDisposable
    {
        private readonly TAoWHSMap previousMap = Game.AoWHSMap;
        private readonly List<TEObject> created = [];
        internal readonly TAoWHSMap Map;
        internal readonly TMapField Field;
        internal readonly CountingUnitTable Units = new();

        internal Scene()
        {
            Map = new TAoWHSMap(null!);
            Map.FSeatedPlayer = Seated;
            Map.FUnitControl = new TUnitControl(null!);
            Map.FUnitControl.FUnitTable = Units;
            var container = new TMapContainer(null!);
            var level = new TMapLevel(container);
            level.CreateMapFieldList(1, 1);
            Map.FMapContainer = container;
            Field = container.GetField(0, 0, 0);
            Game.AoWHSMap = Map;
        }

        internal TEventLog Create(Type type)
        {
            var log = (TEventLog)EClass.Of(type).Create(null!);
            Assert.IsType(type, log);
            created.Add(log);
            return log;
        }

        // The negotiation at the field, with no army there (Joins null) or an army of one hero.
        internal void Negotiate(TEventLog log, TNegotiateObject what, bool? Joins)
        {
            var request = (TNegotiateRequestEventLog)log;
            request.FObject = what;
            request.FPlayer = Seated;
            request.FX = 0;
            request.FY = 0;
            request.FL = 0;
            while (Field.FindOwnedChild(TArmyHS.ClassID) is THexagonSprite standing)
            {
                Field.RemoveHS(standing);
            }
            if (Joins is bool joins)
            {
                var army = new TArmyHS(Field);
                army.FArmy.FChildren.Add(new JoiningHero(joins));
                Assert.Same(army, Field.FindOwnedChild(TArmyHS.ClassID));
            }
        }

        internal void SelectArmyView(TEventLog log, byte selection)
        {
            var info = (TArmyInfoEventLog)log;
            if (info.FArmyView.GetCount() == 0)
            {
                info.FArmyView.FChildren.Add(new TUnit(null!));
                info.FArmyView.FChildren.Add(new JoiningHero(false));
            }
            info.FSelection = selection;
        }

        internal void Prisoner(TEventLog log, TAbstractUnit? unit)
        {
            Units.Delete(PrisonerID);
            if (unit != null) Units.Add(unit, PrisonerID);
            ((TPrisonersJoinEventLog)log).FFirstUnitID = PrisonerID;
        }

        public void Dispose()
        {
            Game.AoWHSMap = previousMap;
            foreach (var log in created) _ = Record.Exception(log.Dispose);
        }
    }

    [Fact]
    public void EveryEventLogClassIsCovered()
    {
        var types = EventLogTypes();
        Assert.Equal(44, types.Length);
        Assert.Contains(typeof(TEventLog), types);
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
                    Assert.True(TMapEvents.EventRenderers.TryGet(view, out _), $"{type.Name} {state}: no renderer for {view}");
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
            var declaring = type.GetMethod(nameof(TEventLog.View))!.DeclaringType!;
            Type? arm = null;
            for (Type? t = type; t != null && arm == null; t = t.BaseType)
                if (OldArms.Contains(t)) arm = t;
            Assert.Equal(arm ?? typeof(TEventLog), declaring);
            resolved.AppendLine($"{type.Name}: View {declaring.Name}, old arm {arm?.Name ?? "none"}, ExecuteAI {type.GetMethod(nameof(TEventLog.ExecuteAI), [typeof(sbyte)])!.DeclaringType!.Name}");
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
            var declaring = type.GetMethod(nameof(TEventLog.ExecuteAI), [typeof(sbyte)])!.DeclaringType!;
            var log = scene.Create(type);
            string old = OldExecuteAI(log);
            string actual = declaring == typeof(TEventLog) ? "None" : declaring.Name;
            if (actual != old) mismatches.Add($"{type.Name}: {actual}, old cascade {old}");
        }
        Assert.Empty(mismatches);

        var diplomatic = (TDiplomaticActionEventLog)scene.Create(typeof(TDiplomaticActionEventLog));
        var action = new RecordingAction();
        diplomatic.FAction = action;
        diplomatic.ExecuteAI(3);
        Assert.Equal(new[] { (sbyte)3 }, action.Players);

        // The hero emerge AI looks up its hero first. No unit holds the id, so it stops there.
        var emerge = (THeroEmergeEventLog)scene.Create(typeof(THeroEmergeEventLog));
        emerge.FUnitID = 4321;
        int finds = scene.Units.Finds;
        emerge.ExecuteAI(3);
        Assert.Equal(finds + 1, scene.Units.Finds);

        // Every other class leaves the unit table alone.
        foreach (var type in EventLogTypes().Where(t => t != typeof(THeroEmergeEventLog) && t != typeof(TDiplomaticActionEventLog)))
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
        var screen = new TMapEvents(null!);
        var done = new List<TEventLog>();
        var user = scene.Create(typeof(TUserEventLog));
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
        Assert.Equal(35, TMapEvents.EventRenderers.All.Count);
        Assert.False(TMapEvents.EventRenderers.TryGet(EventView.None, out _));
        var keys = typeof(EventView).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            .Select(f => (EventView)f.GetValue(null)!).ToArray();
        Assert.Equal(36, keys.Length);
        Assert.Equal(keys.Length, keys.Select(k => k.Name).Distinct().Count());
        foreach (var key in keys.Where(k => k != EventView.None))
            Assert.True(TMapEvents.EventRenderers.TryGet(key, out _), $"no renderer for {key}");
        var duplicate = Assert.Throws<InvalidOperationException>(() =>
            TMapEvents.EventRenderers.Add(EventView.Message, static (_, _, _, _) => { }));
        Assert.Contains("map event view", duplicate.Message);
    }
}
