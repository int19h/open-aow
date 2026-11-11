// Translated from ImageLib.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// { The header of an .ILB file (24 bytes). Up to version 3 the images follow it; from version 4
//     on the image index (HeaderSize to IndexEnd) is followed by the discardable image data. }
[StructLayout(LayoutKind.Explicit, Size = 24)]
public struct ImageLibraryHeader
{
    [FieldOffset(0)]
    public ByteArray5 ID__image;

    public string ID { get => ReadShortString(ID__image); set => WriteShortString(ID__image, value); }

    [FieldOffset(8)]
    public float Version;
    [FieldOffset(12)]
    public int HeaderSize;
    [FieldOffset(16)]
    public int IndexEnd;
    [FieldOffset(20)]
    public int Size;
}
