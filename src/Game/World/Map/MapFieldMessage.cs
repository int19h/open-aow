// Translated from HSEngine.pas.
using Aow.Rtl;

namespace Aow.Game.World.Map;

public struct MapFieldMessage
{
    public int MessageId;
    public MapField MapField;
    public MapFieldMessageParameters Param;
    public int HexNumber;
    public MsgArg Result;
}
