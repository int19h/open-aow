// Translated from NetworkE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Net;

namespace Aow.Game.Commands;

// ends the turn of the token control that prefetched it
public partial class PassTokenTE : TokenEvent
{
    public new const int ClassID = NetworkSession.CID_PassTokenTE;

    public PassTokenTE(ECustomNode owner) : base(owner)
    { }

}
