// Translated from City.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Events;

// FXhx, FYhx, FLhx, FCityName
public partial class CityRebellionEventLog : EventLog
{
    public CityRebellionEventLog(ECustomNode owner) : base(owner)
    { }

    // City.pas:2675
    public new const int ClassID = 131346;

    public int FXhx;
    public int FYhx;
    public int FLhx;
    public string FCityName = "";

    // City.pas:2680
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FXhx, 20);
        Stream.rwInteger(ref FYhx, 21);
        Stream.rwInteger(ref FLhx, 22);
        Stream.rwString(ref FCityName, 23);
    }

    // City.pas:2689
    public override string GetText() => RtlUnit.Format(AbilityRules.TranslateRStr(CityRebellionRStr), [FCityName]);

    // City.pas:2699
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool Result = default;
        bool ShowEvent;
        ShowEvent = Trigger == ExecuteTrigger.User;
        ShowEvent = ShowEvent || AoWHSMap.Current.FPlayMode == PlayMode.Turn && GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.Rebellion);
        ShowEvent = ShowEvent || AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous && GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.Rebellion);
        if (ShowEvent)
        {
            Result = base.Execute(Mode, Trigger, Done);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // City.pas:2694
    public override void ViewLocation()
    {
        CenterViewTo((sbyte)FXhx, (sbyte)FYhx, (sbyte)FLhx);
    }

    private static readonly string CityRebellionRStr = "%s rebelled";

}
