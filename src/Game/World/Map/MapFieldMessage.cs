// Translated from HSEngine.pas.
using Aow.Rtl;

namespace Aow.Game.World.Map;

public struct MapFieldMessage
{
    public int MessageId { get; set; }
    public MapField MapField { get; set; }

    public MapFieldMessageParameters Param;

    public int HexNumber { get; set; }
    public MsgArg Result { get; set; }
}
