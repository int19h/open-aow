// Translated from GFXE.pas.
using System.Buffers.Binary;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// A palette of 256 entries. Each entry is one 32-bit pixel value whose meaning follows
// PixelFormatId: red, green, blue and a flags byte for pfBGR24 and pfRGB24, a 16-bit colour in the
// low half after Remap to pfRGB565 or pfRGB555, a whole pixel after ImageLibraryFont.SetColor.
public partial class Palette
{
    private const int EntryCount = 256;

    private readonly uint[] _pixels = new uint[EntryCount];

    public int PixelFormatId { get; set; }

    // The pixel values of the entries, in entry order, for the blitters.
    public ReadOnlySpan<uint> Pixels => _pixels;

    public Entry this[int index]
    {
        get => new(_pixels[index]);
        set => _pixels[index] = value.Value;
    }

    // GFXE.pas:11050
    public void LoadFromStream(Stream stream)
    {
        Span<byte> image = stackalloc byte[4 + 4 * EntryCount];
        stream.ReadExactly(image);
        PixelFormatId = BinaryPrimitives.ReadInt32LittleEndian(image);
        for (int i = 0; i < EntryCount; i++) _pixels[i] = BinaryPrimitives.ReadUInt32LittleEndian(image.Slice(4 + 4 * i, 4));
    }

    // GFXE.pas:11056
    public void SaveToStream(Stream stream)
    {
        Span<byte> image = stackalloc byte[4 + 4 * EntryCount];
        BinaryPrimitives.WriteInt32LittleEndian(image, PixelFormatId);
        for (int i = 0; i < EntryCount; i++) BinaryPrimitives.WriteUInt32LittleEndian(image.Slice(4 + 4 * i, 4), _pixels[i]);
        stream.Write(image);
    }

    // GFXE.pas:11068
    // The original returns a pointer to the entry, the entry's value here. Writes go through the
    // indexer.
    public Entry GetEntry(int index) => this[index];

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
                                    Entry paletteEntry = this[I];
                                    this[I] = paletteEntry with { PackedColor = (ushort)(((paletteEntry.Red & 248) << 8) + ((paletteEntry.Green & 252) << 3) + (int)(((uint)paletteEntry.Blue) >> 3)) };
                                }
                            }

                            break;
                        case PixelFormats.pfRGB555:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    Entry paletteEntry = this[I];
                                    this[I] = paletteEntry with { PackedColor = (ushort)(((paletteEntry.Red & 248) << 7) + ((paletteEntry.Green & 248) << 2) + (int)(((uint)paletteEntry.Blue) >> 3)) };
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
                                    Entry paletteEntry = this[I];
                                    this[I] = paletteEntry with { PackedColor = (ushort)(((paletteEntry.Blue & 248) << 8) + ((paletteEntry.Green & 252) << 3) + (int)(((uint)paletteEntry.Red) >> 3)) };
                                }
                            }

                            break;
                        case PixelFormats.pfRGB555:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    Entry paletteEntry = this[I];
                                    this[I] = paletteEntry with { PackedColor = (ushort)(((paletteEntry.Blue & 248) << 7) + ((paletteEntry.Green & 248) << 2) + (int)(((uint)paletteEntry.Red) >> 3)) };
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
                        result = palette[I].PackedColor == this[I].PackedColor;
                        I -= 1;
                    }

                    break;
                case 16:
                    result = true;
                    I = 255;
                    while (I >= 0 && result)
                    {
                        result = palette[I].PackedColor == this[I].PackedColor;
                        I -= 1;
                    }

                    break;
                case 24:
                    result = true;
                    I = 255;
                    while (I >= 0 && result)
                    {
                        result = palette[I].PackedColor == this[I].PackedColor && palette[I].Blue == this[I].Blue;
                        I -= 1;
                    }

                    break;
                // Never reached: and 31 leaves 0 for a 32-bit format, as in the original.
                case 32:
                    result = Pixels.SequenceEqual(palette.Pixels);
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

    // One palette entry, a 32-bit pixel value stored little-endian (GFXE.pas TPaletteEntry). The
    // channels name its bytes: Red, Green, Blue and Flags (the record's Reserved), and PackedColor
    // the low 16 bits (the record's W) that a 16-bit pixel format keeps there.
    public readonly record struct Entry(uint Value)
    {
        public Entry(byte red, byte green, byte blue, byte flags = 0)
            : this(red | ((uint)green << 8) | ((uint)blue << 16) | ((uint)flags << 24))
        {
        }

        public byte Red { get => (byte)Value; init => Value = (Value & 0xFFFF_FF00u) | value; }

        public byte Green { get => (byte)(Value >> 8); init => Value = (Value & 0xFFFF_00FFu) | ((uint)value << 8); }

        public byte Blue { get => (byte)(Value >> 16); init => Value = (Value & 0xFF00_FFFFu) | ((uint)value << 16); }

        public byte Flags { get => (byte)(Value >> 24); init => Value = (Value & 0x00FF_FFFFu) | ((uint)value << 24); }

        public ushort PackedColor { get => (ushort)Value; init => Value = (Value & 0xFFFF_0000u) | value; }
    }
}
