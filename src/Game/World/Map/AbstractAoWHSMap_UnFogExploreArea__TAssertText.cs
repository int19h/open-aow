// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

namespace Aow.Game.World.Map;

[StructLayout(LayoutKind.Explicit, Size = 136)]
public struct AbstractAoWHSMap_UnFogExploreArea__TAssertText
{
    [FieldOffset(0)]
    public int RefCount;
    [FieldOffset(4)]
    public int Len;
    [FieldOffset(8)]
    public FixedArray_byte_128 Text;
}
