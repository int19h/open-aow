#nullable disable
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
public class FlagMessageEvent(CustomNode owner) : FlagEvent(owner)
{

    // FlagEvents.pas:192
    public new const int ClassId = 132_461;

    private string _helpFileName = "";
    private readonly DelphiStringList _message = new();

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

    // FlagEvents.pas:204
    public override bool Trigger(sbyte player)
    {
        if (StrategicMap.Current.SeatedPlayer == player)
        {
            FlagMessageEventLog log = new(null, holder: CreationHolder())
            {
                HelpFileName = _helpFileName
            };
            string text = _message.Text;
            log.Message.Text = text;
            StrategicMap.Current.TriggerExecuteEventLog(log, immediate: TriggerMode == FlagEventTriggerMode.Instant, done: null);
            _ = log.Release();
        }

        return true;
    }

    public new class Resource(CustomNode owner) : FlagEventResource(owner)
    {

        // FlagEvents.pas:218
        public new const int ClassId = 132_462;

        // FlagEvents.pas:228
        public override int ResourceUserClassId() => FlagMessageEvent.ClassId;

        // FlagEvents.pas:223
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_flagMessageEventText);

        private static readonly string s_flagMessageEventText = "Flag Message Event";

    }
}
