// Translated from Cave.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.World.Terrain;

[StructLayout(LayoutKind.Explicit, Size = 28)]
public struct CaveFieldFlags
{
    [FieldOffset(0)]
    public FixedArray_byte_24 Reserved;
    [FieldOffset(24)]
    public int Flags;
}
