#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from EventLog.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Events;

public class EndTurnEventLog(CustomNode owner, EngineObject holder) : EventLog(owner, holder: holder)
{

    // EventLog.pas:753
    public new const int ClassId = 131_811;

    // EventLog.pas:750
    public override bool Visible => false;

    // EventLog.pas:756
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done) => false;

}
