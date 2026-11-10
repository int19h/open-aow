// Translated from Mountain.pas.
using System.Runtime.InteropServices;

namespace Aow.Game.Magic.Spells;

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct Location
{
    [FieldOffset(0)]
    public sbyte X;
    [FieldOffset(1)]
    public sbyte Y;
    [FieldOffset(2)]
    public sbyte Layer;
    [FieldOffset(3)]
    public sbyte Reserved;
}
