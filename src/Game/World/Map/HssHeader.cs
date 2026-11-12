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
    public Array5<byte> IdImage;

    public string Id { get => ReadShortString(IdImage); set => WriteShortString(IdImage, value); }

    [FieldOffset(9)]
    public Array41<byte> NameImage;

    public string Name { get => ReadShortString(NameImage); set => WriteShortString(NameImage, value); }
}
