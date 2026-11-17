// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Events;

// mode, player
public partial class SendEmailGameEventLog(CustomNode owner) : EventLog(owner)
{
    public byte Mode { get; set; }
    public sbyte SendPlayer { get; set; }
}
