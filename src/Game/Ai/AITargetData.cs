// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

namespace Aow.Game.Ai;

[StructLayout(LayoutKind.Explicit, Size = 24)]
public struct AITargetData
{
    [FieldOffset(0)]
    public int Priority;
    [FieldOffset(4)]
    public AITargetType TargetType;
    [FieldOffset(8)]
    public int Cost;
    [FieldOffset(12)]
    public FixedArray_byte_12 Reserved0C;
}
