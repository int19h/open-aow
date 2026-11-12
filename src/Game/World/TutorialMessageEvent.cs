// Translated from FlagEvents.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World;

// Message
public partial class TutorialMessageEvent : FlagEvent
{
    public TutorialMessageEvent(ECustomNode owner) : base(owner)
    {
        _message = new StringList();
    }

    // FlagEvents.pas:245
    public new const int ClassId = 132459;

    private StringList _message;

    // FlagEvents.pas:239
    public override void Dispose()
    {
        _message?.Dispose();
        base.Dispose();
    }

    // FlagEvents.pas:250
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwStrings(_message, 40);
    }

    // FlagEvents.pas:233

    // FlagEvents.pas:256
    public override bool Trigger(sbyte player)
    {
        TutorialMessageEventLog log;
        if (StrategicMap.Current.SeatedPlayer == player)
        {
            log = new TutorialMessageEventLog(null);
            var text = _message.Text;
            log.Message.Text = text;
            StrategicMap.Current.TriggerExecuteEventLog(log, TriggerMode == FlagEventTriggerMode.Instant, null);
            log.Release();
        }

        return true;
    }

    public new partial class Resource : FlagEventResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // FlagEvents.pas:269
        public new const int ClassId = 132460;

        // FlagEvents.pas:279
        public override int ResourceUserClassId() => 132459;

        // FlagEvents.pas:274
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_tutorialMessageEventText);

        private static readonly string s_tutorialMessageEventText = "Tutorial Message Event";

    }

    // FlagEvents.pas:81

}
