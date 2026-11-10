// Translated from Sign.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.World;

[StructLayout(LayoutKind.Explicit, Size = 315)]
public struct SignMapFields
{
    [FieldOffset(0)]
    public FixedArray_byte_164 Reserved;
    [FieldOffset(164)]
    public sbyte CurrentPlayer;
    [FieldOffset(165)]
    public sbyte SeatedPlayer;
    [FieldOffset(166)]
    public FixedArray_byte_148 ReservedA6;
    [FieldOffset(314)]
    public byte StorageType;
}
