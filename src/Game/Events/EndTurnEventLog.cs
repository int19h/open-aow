// Translated from EventLog.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Events;

public partial class EndTurnEventLog : EventLog
{
    public EndTurnEventLog(ECustomNode owner) : base(owner)
    { }

    // EventLog.pas:753
    public new const int ClassID = 131811;

    // EventLog.pas:750
    public override bool GetVisible() => false;

    // EventLog.pas:756
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done) => false;

}
