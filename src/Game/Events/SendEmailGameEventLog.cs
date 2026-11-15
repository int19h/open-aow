// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Events;

// mode, player
public partial class SendEmailGameEventLog : EventLog
{
    public SendEmailGameEventLog(CustomNode owner) : base(owner)
    { }

    public byte Mode { get; set; }
    public sbyte SendPlayer { get; set; }
}
