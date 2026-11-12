// Translated from AoWSetup.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.Session;

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct SetupPlayerBehavior
{
    [FieldOffset(0)]
    public byte PlayerType;
    [FieldOffset(1)]
    public Array3<byte> Reserved;
    [FieldOffset(4)]
    public int Flags;
}
