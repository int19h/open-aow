// Translated from AoWHex.pas.
using Aow.Engine.Buffers;

using Aow.Game.Draw;

namespace Aow.Game.World.Hexes;

public struct IsoFields
{
    public FixedArray_byte_16 Reserved;
    public DynamicIsometricHexagon Dynamic;
    public byte OpenSides;
    public byte Sides;
}
