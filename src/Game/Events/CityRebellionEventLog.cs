// Translated from City.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Events;

// HexX, HexY, HexLevel, CityName
public partial class CityRebellionEventLog : EventLog
{
    public CityRebellionEventLog(ECustomNode owner) : base(owner)
    { }

    // City.pas:2675
    public new const int ClassId = 131346;

    public int HexX;
    public int HexY;
    public int HexLevel;
    public string CityName = "";

    // City.pas:2680
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref HexX, 20);
        stream.Int32(ref HexY, 21);
        stream.Int32(ref HexLevel, 22);
        stream.String(ref CityName, 23);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        HexX = persister.Int32("HexX", value: HexX);
        HexY = persister.Int32("HexY", value: HexY);
        HexLevel = persister.Int32("HexLevel", value: HexLevel);
        CityName = persister.String("CityName", value: CityName);
    }

    // City.pas:2689
    public override string GetText() => RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), s_cityRebellionFormat), [CityName]);

    // City.pas:2699
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool result = default;
        bool showEvent;
        showEvent = trigger == ExecuteTrigger.User;
        showEvent = showEvent || StrategicMap.Current.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.Rebellion);
        showEvent = showEvent || StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.Rebellion);
        if (showEvent)
        {
            result = base.Execute(mode, trigger, done);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // City.pas:2694
    public override void ViewLocation()
    {
        CenterViewTo((sbyte)HexX, (sbyte)HexY, (sbyte)HexLevel);
    }

    private static readonly string s_cityRebellionFormat = "%s rebelled";

}
