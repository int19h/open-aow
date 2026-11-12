// Translated from GFXE.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public partial class Palette
{
    public int PixelFormatId;
    public Entry[] Entries = new Entry[256];

    // GFXE.pas:11050
    public void LoadFromStream(Stream stream)
    {
        stream.ReadExactly(Bytes.Of(ref PixelFormatId, 4));
        stream.ReadExactly(MemoryMarshal.AsBytes((Span<Entry>)Entries).Slice(0, 1024));
    }

    // GFXE.pas:11056
    public void SaveToStream(Stream stream)
    {
        stream.Write(Bytes.Of(ref PixelFormatId, 4));
        stream.Write(MemoryMarshal.AsBytes((ReadOnlySpan<Entry>)Entries).Slice(0, 1024));
    }

    // GFXE.pas:11068
    // The original returns a pointer to the entry, a reference here.
    public ref Entry GetEntry(int index) => ref Entries[index];

    // GFXE.pas:11073
    public int Remap(int newPixelFormatId)
    {
        int result = PixelFormats.GFXE_OK;
        if (newPixelFormatId != PixelFormatId)
        {
            switch (PixelFormatId)
            {
                case PixelFormats.pfBGR24:
                    switch (newPixelFormatId)
                    {
                        case PixelFormats.pfRGB565:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref Entry paletteEntry = ref Entries[I];
                                    paletteEntry.PackedColor = (ushort)(((paletteEntry.Red & 248) << 8) + ((paletteEntry.Green & 252) << 3) + (int)(((uint)paletteEntry.Blue) >> 3));
                                }
                            }

                            break;
                        case PixelFormats.pfRGB555:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref Entry paletteEntry = ref Entries[I];
                                    paletteEntry.PackedColor = (ushort)(((paletteEntry.Red & 248) << 7) + ((paletteEntry.Green & 248) << 2) + (int)(((uint)paletteEntry.Blue) >> 3));
                                }
                            }

                            break;
                        case PixelFormats.pfXRGB32:
                            result = PixelFormats.GFXE_OK;
                            break;
                        default:
                            result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case PixelFormats.pfRGB24:
                    switch (newPixelFormatId)
                    {
                        case PixelFormats.pfRGB565:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref Entry paletteEntry = ref Entries[I];
                                    paletteEntry.PackedColor = (ushort)(((paletteEntry.Blue & 248) << 8) + ((paletteEntry.Green & 252) << 3) + (int)(((uint)paletteEntry.Red) >> 3));
                                }
                            }

                            break;
                        case PixelFormats.pfRGB555:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref Entry paletteEntry = ref Entries[I];
                                    paletteEntry.PackedColor = (ushort)(((paletteEntry.Blue & 248) << 7) + ((paletteEntry.Green & 248) << 2) + (int)(((uint)paletteEntry.Red) >> 3));
                                }
                            }

                            break;
                        case PixelFormats.pfXRGB32:
                            result = PixelFormats.GFXE_OK;
                            break;
                        default:
                            result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                default:
                    result = PixelFormats.GFXE_NOTSUPPORTED;
                    break;
            }

            if (Succeeded(result))
            {
                PixelFormatId = newPixelFormatId;
            }
        }
        else
        {
            result = PixelFormats.GFXE_OK;
        }

        return result;
    }

    // GFXE.pas:11140
    public bool Compare(Palette palette)
    {
        bool result;
        int I;
        if (palette.PixelFormatId == PixelFormatId)
        {
            switch (PixelFormatId & 31)
            {
                case 8:
                    result = true;
                    I = 255;
                    while (I >= 0 && result)
                    {
                        result = palette.Entries[I].PackedColor == Entries[I].PackedColor;
                        I -= 1;
                    }

                    break;
                case 16:
                    result = true;
                    I = 255;
                    while (I >= 0 && result)
                    {
                        result = palette.Entries[I].PackedColor == Entries[I].PackedColor;
                        I -= 1;
                    }

                    break;
                case 24:
                    result = true;
                    I = 255;
                    while (I >= 0 && result)
                    {
                        result = palette.Entries[I].PackedColor == Entries[I].PackedColor && palette.Entries[I].Blue == Entries[I].Blue;
                        I -= 1;
                    }

                    break;
                case 32:
                    result = CompareMem(MemoryMarshal.AsBytes((ReadOnlySpan<Entry>)Entries).Slice(0, 1024), MemoryMarshal.AsBytes((ReadOnlySpan<Entry>)palette.Entries).Slice(0, 1024), 1024);
                    break;
                default:
                    result = false;
                    break;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct Entry
    {
        [FieldOffset(0)]
        public byte Red;
        [FieldOffset(1)]
        public byte Green;
        [FieldOffset(2)]
        public byte Blue;
        [FieldOffset(3)]
        public byte Reserved;
        [FieldOffset(0)]
        public ushort PackedColor;
        [FieldOffset(0)]
        public int Value;
    }
}
