// Translated from FlagEvents.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World;

// HelpFileName, Message
public partial class FlagMessageEvent : FlagEvent
{
    public FlagMessageEvent(ECustomNode owner) : base(owner)
    {
        _message = new StringList();
    }

    // FlagEvents.pas:192
    public new const int ClassId = 132461;

    private string _helpFileName = "";
    private readonly StringList _message;

    // FlagEvents.pas:186
    public override void Dispose()
    {
        _message?.Dispose();
        base.Dispose();
    }

    // FlagEvents.pas:197
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.Lines(persister, name: "Message", lines: _message);
        _helpFileName = persister.String("HelpFileName", value: _helpFileName);
    }

    // FlagEvents.pas:180

    // FlagEvents.pas:204
    public override bool Trigger(sbyte player)
    {
        FlagMessageEventLog log;
        if (StrategicMap.Current.SeatedPlayer == player)
        {
            log = new FlagMessageEventLog(null, holder: CreationHolder());
            log.HelpFileName = _helpFileName;
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

        // FlagEvents.pas:218
        public new const int ClassId = 132462;

        // FlagEvents.pas:228
        public override int ResourceUserClassId() => 132461;

        // FlagEvents.pas:223
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_flagMessageEventText);

        private static readonly string s_flagMessageEventText = "Flag Message Event";

    }

    // FlagEvents.pas:62

}
