// Translated from FileSecurity.pas.
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Saves;

[StructLayout(LayoutKind.Explicit, Size = 106)]
public struct TSecurityData
{
    [FieldOffset(0)]
    public int Reserved00;
    [FieldOffset(4)]
    public byte Key;
    [FieldOffset(5)]
    public FixedArray_byte_51 ID__image;

    public string ID { get => ReadShortString(ID__image); set => WriteShortString(ID__image, value); }

    [FieldOffset(56)]
    public FixedArray_byte_50 Reserved38;
}

[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct TSecurityFooter
{
    [FieldOffset(0)]
    public int CRC;
    [FieldOffset(4)]
    public int Size;
    [FieldOffset(8)]
    public FixedArray_byte_4 Signature;
}
