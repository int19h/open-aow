#nullable disable warnings
// Translated from EventLog.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Events;

public partial class UserEventLog(CustomNode? owner, EngineObject holder) : EventLog(owner, holder: holder)
{
    public object Data { get; init; }
}
