// Translated from HSEngine.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

using Aow.Rtl;

using static Aow.Rtl.ShortStrings;

namespace Aow.Game.World.Map;

// { The header of an .hss file (52 bytes): the set follows at Size, a CRC32 of everything before it
//     closes the file }
[StoredLayout]
[StructLayout(LayoutKind.Explicit, Size = 52)]
public struct HssHeader
{
    [FieldOffset(0)]
    public int Size;
    [FieldOffset(4)]
    public Array5<byte> IdImage;

    public string Id
    {
        readonly get => ReadShortString(IdImage); set
        {
            Span<byte> image = IdImage;
            int length = Math.Min(value.Length, image.Length - 1);
            image[0] = (byte)length;
            Cp1252.Encode(value).AsSpan()[..length].CopyTo(image[1..]);
        }
    }

    [FieldOffset(9)]
    public Array41<byte> NameImage;

    public string Name
    {
        readonly get => ReadShortString(NameImage); set
        {
            Span<byte> image = NameImage;
            int length = Math.Min(value.Length, image.Length - 1);
            image[0] = (byte)length;
            Cp1252.Encode(value).AsSpan()[..length].CopyTo(image[1..]);
        }
    }
}
