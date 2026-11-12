// Translated from EventLog.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Events;

public partial class UserEventLog : EventLog
{
    public UserEventLog(ECustomNode owner) : base(owner)
    { }

    public object Data { get; set; }
}
