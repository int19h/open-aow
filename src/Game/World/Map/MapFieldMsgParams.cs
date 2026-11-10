// Translated from HSEngine.pas.
using System.Runtime.CompilerServices;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// The parameter slots of a map field message. The count is the arity of the message protocol
// (HSEngine.pas TMapFieldMsg.Param), not a count of content.
[InlineArray(Arity)]
public struct MapFieldMsgParams
{
    public const int Arity = 4;

    private MsgArg element0;
}
