// Translated from HSEngine.pas.

namespace Aow.Game.World.Map;

// { The records a map field passes by reference to the methods of its sprites. }
// { THexagonSprite.UpdateMapField (8 bytes) }
public struct HexSpriteMapFieldInfo
{
    public MapField MapField { get; init; }
    public sbyte HexNumber { get; set; }
}
