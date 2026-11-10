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
    public new const int ClassID = 131809;

    public string FText = "";

    // EventLog.pas:716
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwString(ref FText, 15);
    }

    // EventLog.pas:719
    public override string GetText() => FText;

    // EventLog.pas:722
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done) => false;

}
