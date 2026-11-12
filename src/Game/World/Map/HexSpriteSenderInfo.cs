// Translated from HSEngine.pas.

namespace Aow.Game.World.Map;

// { THexagonSprite.CanChangeTerrain, CanMoveOver, MoveExclusive (12 bytes): Sender is the object
//     that asks (the sprite that changes the terrain, the mover) }
public struct HexSpriteSenderInfo
{
    public object Sender { get; init; }
    public MapField MapField { get; init; }
    public sbyte HexNumber { get; set; }
}
