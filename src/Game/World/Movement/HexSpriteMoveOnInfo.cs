// Translated from HSEngine.pas.
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// { THexagonSprite.CanMoveOn (16 and 12 bytes) }
public struct HexSpriteMoveOnInfo
{
    public object Sender;
    public MapField MapField;
    public int MoveOverCode;
    public sbyte HexNumber;
}
