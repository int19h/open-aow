// Translated from Engine.pas.
using System.Runtime.CompilerServices;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// The parameter slots of an engine message. The count is the arity of the message protocol
// (Engine.pas TEMsg.Param), not a count of content.
[InlineArray(Arity)]
public struct EngineMessageParameters
{
    public const int Arity = 3;

    private MsgArg _element0;
}
