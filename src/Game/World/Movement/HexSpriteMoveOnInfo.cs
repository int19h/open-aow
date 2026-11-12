// Translated from HSEngine.pas.
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// { THexagonSprite.CanMoveOn (16 and 12 bytes) }
public struct HexSpriteMoveOnInfo
{
    public object Sender { get; init; }
    public MapField MapField { get; init; }
    public int MoveOverCode { get; init; }
    public sbyte HexNumber { get; set; }
}
