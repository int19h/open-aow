#nullable disable warnings
// Translated from FlagEvents.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World;

// Message
public class TutorialMessageEvent(CustomNode owner) : FlagEvent(owner)
{

    // FlagEvents.pas:245
    public new const int ClassId = 132_459;

    private readonly DelphiStringList _message = new();

    // FlagEvents.pas:239
    public override void Dispose()
    {
        _message?.Dispose();
        base.Dispose();
    }

    // FlagEvents.pas:250
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Lines(name: "Message", lines: _message);
    }

    // FlagEvents.pas:256
    public override bool Trigger(sbyte player)
    {
        if (StrategicMap.Current.SeatedPlayer == player)
        {
            TutorialMessageEventLog log = new(null, holder: CreationHolder());
            string text = _message.Text;
            log.Message.Text = text;
            StrategicMap.Current.TriggerExecuteEventLog(log, immediate: TriggerMode == FlagEventTriggerMode.Instant, done: null);
            _ = log.Release();
        }

        return true;
    }

    public new class Resource(CustomNode owner) : FlagEventResource(owner)
    {

        // FlagEvents.pas:269
        public new const int ClassId = 132_460;

        // FlagEvents.pas:279
        public override int ResourceUserClassId() => TutorialMessageEvent.ClassId;

        // FlagEvents.pas:274
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_tutorialMessageEventText);

        private static readonly string s_tutorialMessageEventText = "Tutorial Message Event";

    }
}
