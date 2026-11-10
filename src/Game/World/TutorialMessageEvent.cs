// Translated from FlagEvents.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World;

// FMessage
public partial class TutorialMessageEvent : FlagEvent
{
    public TutorialMessageEvent(ECustomNode owner) : base(owner)
    {
        FMessage = new StringList();
    }

    // FlagEvents.pas:245
    public new const int ClassID = 132459;

    public StringList FMessage;

    // FlagEvents.pas:239
    public override void Dispose()
    {
        FMessage?.Dispose();
        base.Dispose();
    }

    // FlagEvents.pas:250
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FMessage, 40);
    }

    // FlagEvents.pas:233

    // FlagEvents.pas:256
    public override bool Trigger(sbyte Player)
    {
        TutorialMessageEventLog Log;
        if (AoWHSMap.Current.FSeatedPlayer == Player)
        {
            Log = new TutorialMessageEventLog(null);
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

        // FlagEvents.pas:269
        public new const int ClassID = 132460;

        // FlagEvents.pas:279
        public override int ResourceUserClassID() => 132459;

        // FlagEvents.pas:274
        public override string ResourceEditName() => AbilityRules.TranslateRStr(TutorialMessageEventRStr);

        private static readonly string TutorialMessageEventRStr = "Tutorial Message Event";

    }

    // FlagEvents.pas:81

}
