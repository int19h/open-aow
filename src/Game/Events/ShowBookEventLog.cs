// Translated from EventLog.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Events;

public partial class ShowBookEventLog(CustomNode owner) : EventLog(owner)
{

    // EventLog.pas:818
    public new const int ClassId = 131_818;

    // EventLog.pas:821
    public override bool Visible => false;

}
