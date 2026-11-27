#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from EventLog.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Events;

// text
public class TextEventLog(CustomNode owner, EngineObject holder) : EventLog(owner, holder: holder)
{

    // EventLog.pas:713
    public new const int ClassId = 131_809;

    public string Text { get; set; } = "";

    // EventLog.pas:716
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Text = persister.String("Text", value: Text);
    }

    // EventLog.pas:719
    public override string GetText() => Text;

    // EventLog.pas:722
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done) => false;

}
