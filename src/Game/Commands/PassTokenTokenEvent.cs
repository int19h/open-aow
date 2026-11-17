// Translated from NetworkE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Net;

namespace Aow.Game.Commands;

// ends the turn of the token control that prefetched it
public partial class PassTokenTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = NetworkSession.ClassIdPassTokenTokenEvent;
}
