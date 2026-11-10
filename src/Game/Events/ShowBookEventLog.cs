// Translated from EventLog.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Events;

public partial class ShowBookEventLog : EventLog
{
    public ShowBookEventLog(ECustomNode owner) : base(owner)
    { }

    // EventLog.pas:818
    public new const int ClassID = 131818;

    // EventLog.pas:821
    public override bool GetVisible() => false;

}
