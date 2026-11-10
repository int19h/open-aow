// Translated from FileSecurity.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

namespace Aow.Game.Saves;

[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct SecurityFooter
{
    [FieldOffset(0)]
    public int CRC;
    [FieldOffset(4)]
    public int Size;
    [FieldOffset(8)]
    public FixedArray_byte_4 Signature;
}
