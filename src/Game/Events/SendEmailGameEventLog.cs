// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Session;

namespace Aow.Game.Events;

// mode, player
public partial class SendEmailGameEventLog(CustomNode owner, EngineObject holder) : EventLog(owner, holder: holder)
{
    public int Mode { get; set; }
    public PlayerId? SendPlayer { get; init; } = PlayerId.Independent;
}
