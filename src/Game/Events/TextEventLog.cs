// Translated from EventLog.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Events;

// text
public partial class TextEventLog : EventLog
{
    public TextEventLog(ECustomNode owner) : base(owner)
    { }

    // EventLog.pas:713
    public new const int ClassId = 131809;

    public string Text = "";

    // EventLog.pas:716
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwString(ref Text, 15);
    }

    // EventLog.pas:719
    public override string GetText() => Text;

    // EventLog.pas:722
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done) => false;

}
