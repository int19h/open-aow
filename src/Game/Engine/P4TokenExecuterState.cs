// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

namespace Aow.Game.Engine;

[StructLayout(LayoutKind.Explicit, Size = 36)]
public struct P4TokenExecuterState
{
    [FieldOffset(0)]
    public FixedArray_byte_32 Reserved;
    [FieldOffset(32)]
    public int ExecuteCount;
}
