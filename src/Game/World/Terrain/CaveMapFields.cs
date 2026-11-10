// Translated from Cave.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.World.Terrain;

[StructLayout(LayoutKind.Explicit, Size = 166)]
public struct CaveMapFields
{
    [FieldOffset(0)]
    public FixedArray_byte_165 Reserved;
    [FieldOffset(165)]
    public sbyte SeatedPlayer;
}
