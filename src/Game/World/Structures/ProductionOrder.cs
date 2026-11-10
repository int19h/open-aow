
using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

public abstract partial record ProductionOrder
{
    private static readonly Catalog<int, Func<int, int, int, ProductionOrder>> Kinds = new("production order");

    // The production id, parameter and group id the original format stores.
    public abstract (int Id, int Parameter, int GroupId) ToFields();

    // The order the three stored fields name. An id no kind registered reads as UnknownOrder,
    // which writes the three fields back unchanged.
    public static ProductionOrder FromFields(int Id, int Parameter, int GroupId) =>
        Kinds.TryGet(Id, out var create) ? create(Id, Parameter, GroupId) : new UnknownOrder(Id, Parameter, GroupId);

    // Registers the kind of a production id. A second registration of an id throws.
    internal static void Register(int Id, Func<int, int, int, ProductionOrder> Create) => Kinds.Add(Id, Create);

    // The production ids with a registered kind.
    internal static bool IsRegistered(int Id) => Kinds.TryGet(Id, out _);

    // The city action the city screen and the city queries name this order by (City.pas:3220).
    public virtual CityAction CityAction => CityAction.None;

    // The unit production the screens draw for this order, or null.
    public virtual int? UnitProductionIndex => null;

    // The queue line (Structur.PAS:840).
    public virtual string Name(ProductionControl Control) => AbilityRules.TranslateRStr(GameTables.UnknownRStr);

    // The line of the running order (Structur.PAS:865).
    public virtual string NameProgressive(ProductionControl Control) => AbilityRules.TranslateRStr(GameTables.UnknownRStr);

    // Whether Control can start the order now, with the reason when it cannot (Structur.PAS:361).
    public virtual bool CanExecute(ProductionControl Control, ref string Reason)
    {
        Waiting(Control, ref Reason);
        return false;
    }

    // Starts the order inside the control's update bracket (Structur.PAS:392).
    public virtual void Execute(ProductionControl Control)
    {
    }

    // Starts the order once the control's update bracket closed (City.pas:434, PrdPlace.pas:238).
    public virtual void Executed(ProductionControl Control)
    {
    }

    // The order's turns ran out in the turn of Player (Structur.PAS:804).
    public virtual void Complete(ProductionControl Control, sbyte Player)
    {
    }

    // The order is still running with no turns left after Complete (City.pas:496, PrdPlace.pas:231).
    public virtual void Completed(ProductionControl Control)
    {
    }

    // The running order is cancelled, before the control refunds it (City.pas:573).
    public virtual void Cancel(ProductionControl Control)
    {
    }

    // Whether a looping queue puts the order back at its end (Structur.PAS:837, City.pas:604).
    public virtual bool Loopable(ProductionControl Control) => true;

    // Whether the order in the queue bars further entries (Structur.PAS:834, City.pas:595).
    public virtual bool LocksQueue(ProductionControl Control) => false;

    // The city screen's activity image (AoWCityScreen.pas:904).
    public virtual int ActivityImage(City City) => 40;

    // The production screen's activity image (AoWProdScreen.pas:719).
    public virtual int ActivityImage(ProductionPlace Place) => 4;

    // The image sequence of City's map image while the order runs, Row 0 to 2 from the top
    // (City.pas:2503, 2539, 2575): null for the race image, -1 for no image.
    public virtual int? CitySequence(City City, int Row) => null;

    // The activity column of the realm screen (AoWRealm.pas).
    public virtual string RealmActivity(City City) => "-";

    // The info window's lines for the order at the head of Control's queue (AoWInfoWin.pas:439, 496).
    public virtual void ActivityLines(ProductionControl Control, StringCollection Lines)
    {
        Lines.Text = NameProgressive(Control);
    }

    // The base control's waiting note, which every arm of the base control sets first. The
    // original continues checking costs after assigning the waiting text.
    protected static void Waiting(ProductionControl Control, ref string Reason)
    {
        if (Control.FStatus == ProductionControl.Status.Waiting)
        {
            Reason = "(" + LowerCase(AbilityRules.TranslateRStr(WaitingRStr)) + ")";
        }
    }

    // Changes the relation of City's race to the control's player by Change.
    protected static void ChangeRelation(ProductionControl Control, City City, int Change)
    {
        var playerRelationValue = AoWHSMap.Current.FRaces.GetRace(City.FRace).GetPlayerRelationValue(Control.GetPlayer());
        AoWHSMap.Current.FRaces.GetRace(City.FRace).SetPlayerRelationValue(Control.GetPlayer(), playerRelationValue + Change);
    }

    // Adds Text at City to the logbook of the control's player.
    protected static void Log(ProductionControl Control, City City, string Text)
    {
        var Event = new CityProductionEventLog(null);
        Event.FPlayer = Control.GetPlayer();
        Event.FText = Text;
        var lhx = City.GetLhx();
        var yhx = City.GetYhx();
        Event.SetLocation(City.GetXhx(), yhx, lhx);
        AoWHSMap.Current.FPlayers.GetPlayers(Event.FPlayer).FEventLogbook.AddEvent(Event);
        Event.Release();
    }

    // Charges the settings' costs to the settings' player.
    protected static void Pay(ProductionControl Control)
    {
        var player = AoWHSMap.Current.FPlayers.GetPlayers(Control.FSettings.FPlayer);
        player.SetGems(player.FGems - Control.FSettings.FGold);
        player.FMagicControl.SetMana(player.FMagicControl.FMana - Control.FSettings.FMana);
    }

    private static readonly string WaitingRStr = "Waiting";
}
