// Translated from Mine.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.World.Structures;

[StructLayout(LayoutKind.Explicit, Size = 24)]
public struct MineImageData
{
    [FieldOffset(0)]
    public FixedArray_byte_20 Reserved;
    [FieldOffset(20)]
    public int ImageIndex;
}
