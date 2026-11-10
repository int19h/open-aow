// Translated from HSEngine.pas.
using System.Runtime.InteropServices;

namespace Aow.Game.World.Map;

// { A map location: hexagon column X, row Y, map level L and a flag byte (LocationsConnected
//     marks the locations it has reached with it); the items of TXYLList, made by PackXYL }
[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct XYL
{
    [FieldOffset(0)]
    public sbyte X;
    [FieldOffset(1)]
    public sbyte Y;
    [FieldOffset(2)]
    public sbyte L;
    [FieldOffset(3)]
    public sbyte Flag;
    [FieldOffset(0)]
    public ushort XY;
    [FieldOffset(0)]
    public int Packed;
}
