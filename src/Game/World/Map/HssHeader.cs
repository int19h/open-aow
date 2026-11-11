// Translated from HSEngine.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

// { The header of an .hss file (52 bytes): the set follows at Size, a CRC32 of everything before it
//     closes the file }
[StructLayout(LayoutKind.Explicit, Size = 52)]
public struct HssHeader
{
    [FieldOffset(0)]
    public int Size;
    [FieldOffset(4)]
    public ByteArray5 ID__image;

    public string ID { get => ReadShortString(ID__image); set => WriteShortString(ID__image, value); }

    [FieldOffset(9)]
    public ByteArray41 Name__image;

    public string Name { get => ReadShortString(Name__image); set => WriteShortString(Name__image, value); }
}
