// Translated from HSEngine.pas.
using System.Runtime.InteropServices;

namespace Aow.Game.World.Map;

// { a column and row offset (HNtoXYTable) }
[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct XYTableEntry
{
    [FieldOffset(0)]
    public int X;
    [FieldOffset(4)]
    public int Y;
}
