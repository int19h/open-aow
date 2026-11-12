// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Events;

// mode, player
public partial class SendEmailGameEventLog : EventLog
{
    public SendEmailGameEventLog(ECustomNode owner) : base(owner)
    { }

    public byte Mode;
    public sbyte SendPlayer;
}
