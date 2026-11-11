// Translated from HSEngine.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

namespace Aow.Game.World.Map;

// { The header of an .hsm file (16 bytes) in front of the stored map, inside the compressed stream }
[StructLayout(LayoutKind.Explicit, Size = 16)]
public struct HsmHeader
{
    [FieldOffset(0)]
    public int Size;
    [FieldOffset(4)]
    public ByteArray4 ID;
    [FieldOffset(8)]
    public int Reserved;
    [FieldOffset(12)]
    public int ClassID;
}
