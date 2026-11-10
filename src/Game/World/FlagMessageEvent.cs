// Translated from FlagEvents.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World;

// FHelpFileName, FMessage
public partial class FlagMessageEvent : FlagEvent
{
    public FlagMessageEvent(ECustomNode owner) : base(owner)
    {
        FMessage = new StringList();
    }

    // FlagEvents.pas:192
    public new const int ClassID = 132461;

    public string FHelpFileName = "";
    public StringList FMessage;

    // FlagEvents.pas:186
    public override void Dispose()
    {
        FMessage?.Dispose();
        base.Dispose();
    }

    // FlagEvents.pas:197
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FMessage, 40);
        Stream.rwString(ref FHelpFileName, 41);
    }

    // FlagEvents.pas:180

    // FlagEvents.pas:204
    public override bool Trigger(sbyte Player)
    {
        FlagMessageEventLog Log;
        if (AoWHSMap.Current.FSeatedPlayer == Player)
        {
            Log = new FlagMessageEventLog(null);
            Log.FHelpFileName = FHelpFileName;
            var text = FMessage.Text;
            Log.FMessage.Text = text;
            AoWHSMap.Current.TriggerExecuteEventLog(Log, FTriggerMode == TriggerMode.Instant, null);
            Log.Release();
        }

        return true;
    }

    public new partial class Resource : FlagEventResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // FlagEvents.pas:218
        public new const int ClassID = 132462;

        // FlagEvents.pas:228
        public override int ResourceUserClassID() => 132461;

        // FlagEvents.pas:223
        public override string ResourceEditName() => AbilityRules.TranslateRStr(FlagMessageEventRStr);

        private static readonly string FlagMessageEventRStr = "Flag Message Event";

    }

    // FlagEvents.pas:62

}
