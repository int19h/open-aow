
using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

public abstract partial record ProductionOrder
{
    private static readonly Catalog<int, Func<int, int, int, ProductionOrder>> s_kinds = new("production order");

    // The production id, parameter and group id the original format stores.
    public abstract (int Kind, int Parameter, int GroupId) ToFields();

    // The order the three stored fields name. An id no kind registered reads as UnknownOrder,
    // which writes the three fields back unchanged.
    public static ProductionOrder FromFields(int kind, int parameter, int groupId) =>
        s_kinds.TryGet(kind, out var create) ? create(kind, parameter, groupId) : new UnknownOrder(kind, parameter, groupId);

    // Registers the kind of a production id. A second registration of an id throws.
    internal static void Register(int id, Func<int, int, int, ProductionOrder> create) => s_kinds.Add(id, create);

    // The production ids with a registered kind.
    internal static bool IsRegistered(int id) => s_kinds.TryGet(id, out _);

    // The city action the city screen and the city queries name this order by (City.pas:3220).
    public virtual CityAction CityAction => CityAction.None;

    // The unit production the screens draw for this order, or null.
    public virtual int? UnitProductionIndex => null;

    // The queue line (Structur.PAS:840).
    public virtual string Name(ProductionControl control) => AbilityRules.TranslateResourceString(GameTables.UnknownText);

    // The line of the running order (Structur.PAS:865).
    public virtual string NameProgressive(ProductionControl control) => AbilityRules.TranslateResourceString(GameTables.UnknownText);

    // Whether Control can start the order now, with the reason when it cannot (Structur.PAS:361).
    public virtual bool CanExecute(ProductionControl control, ref string reason)
    {
        Waiting(control, ref reason);
        return false;
    }

    // Starts the order inside the control's update bracket (Structur.PAS:392).
    public virtual void Execute(ProductionControl control)
    {
    }

    // Starts the order once the control's update bracket closed (City.pas:434, PrdPlace.pas:238).
    public virtual void Executed(ProductionControl control)
    {
    }

    // The order's turns ran out in the turn of Player (Structur.PAS:804).
    public virtual void Complete(ProductionControl control, sbyte player)
    {
    }

    // The order is still running with no turns left after Complete (City.pas:496, PrdPlace.pas:231).
    public virtual void Completed(ProductionControl control)
    {
    }

    // The running order is cancelled, before the control refunds it (City.pas:573).
    public virtual void Cancel(ProductionControl control)
    {
    }

    // Whether a looping queue puts the order back at its end (Structur.PAS:837, City.pas:604).
    public virtual bool Loopable(ProductionControl control) => true;

    // Whether the order in the queue bars further entries (Structur.PAS:834, City.pas:595).
    public virtual bool LocksQueue(ProductionControl control) => false;

    // The city screen's activity image (AoWCityScreen.pas:904).
    public virtual int ActivityImage(City city) => 40;

    // The production screen's activity image (AoWProdScreen.pas:719).
    public virtual int ActivityImage(ProductionPlace place) => 4;

    // The image sequence of City's map image while the order runs, Row 0 to 2 from the top
    // (City.pas:2503, 2539, 2575): null for the race image, -1 for no image.
    public virtual int? CitySequence(City city, int row) => null;

    // The activity column of the realm screen (AoWRealm.pas).
    public virtual string RealmActivity(City city) => "-";

    // The info window's lines for the order at the head of Control's queue (AoWInfoWin.pas:439, 496).
    public virtual void ActivityLines(ProductionControl control, StringCollection lines)
    {
        lines.Text = NameProgressive(control);
    }

    // The base control's waiting note, which every arm of the base control sets first. The
    // original continues checking costs after assigning the waiting text.
    protected static void Waiting(ProductionControl control, ref string reason)
    {
        if (control.Status == ProductionControlStatus.Waiting)
        {
            reason = "(" + LowerCase(AbilityRules.TranslateResourceString(s_waitingText)) + ")";
        }
    }

    // Changes the relation of City's race to the control's player by Change.
    protected static void ChangeRelation(ProductionControl control, City city, int change)
    {
        var playerRelationValue = StrategicMap.Current.Races.GetRace(city.Race).GetPlayerRelationValue(control.GetPlayer());
        StrategicMap.Current.Races.GetRace(city.Race).SetPlayerRelationValue(control.GetPlayer(), playerRelationValue + change);
    }

    // Adds Text at City to the logbook of the control's player.
    protected static void Log(ProductionControl control, City city, string text)
    {
        var eventValue = new CityProductionEventLog(null);
        eventValue._player = control.GetPlayer();
        eventValue.Text = text;
        var hexLevel = city.GetHexLevel();
        var hexY = city.GetHexY();
        eventValue.SetLocation(city.GetHexX(), hexY, hexLevel);
        StrategicMap.Current.Players.GetPlayers(eventValue._player).EventLogbook.AddEvent(eventValue);
        eventValue.Release();
    }

    // Charges the settings' costs to the settings' player.
    protected static void Pay(ProductionControl control)
    {
        var player = StrategicMap.Current.Players.GetPlayers(control.Settings.Player);
        player.SetGems(player.Gems - control.Settings.Gold);
        player.MagicControl.SetMana(player.MagicControl.Mana - control.Settings.Mana);
    }

    private static readonly string s_waitingText = "Waiting";
}
