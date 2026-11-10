// Translated from HSEngine.pas.
using System.Runtime.InteropServices;

namespace Aow.Game.World.Map;

// { an XYL value (TMapField.GetXYL, PackXYL) as its bytes }
[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct XYLRec
{
    [FieldOffset(0)]
    public sbyte X;
    [FieldOffset(1)]
    public sbyte Y;
    [FieldOffset(2)]
    public sbyte L;
    [FieldOffset(3)]
    public sbyte Reserved;
}
