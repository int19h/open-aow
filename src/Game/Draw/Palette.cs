// Translated from GFXE.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public partial class Palette
{
    public int PixelFormatID;
    public Entry[] Entries = new Entry[256];

    // GFXE.pas:11050
    public void LoadFromStream(Stream Stream)
    {
        Stream.ReadExactly(Bytes.Of(ref PixelFormatID, 4));
        Stream.ReadExactly(MemoryMarshal.AsBytes((Span<Entry>)Entries).Slice(0, 1024));
    }

    // GFXE.pas:11056
    public void SaveToStream(Stream Stream)
    {
        Stream.Write(Bytes.Of(ref PixelFormatID, 4));
        Stream.Write(MemoryMarshal.AsBytes((ReadOnlySpan<Entry>)Entries).Slice(0, 1024));
    }

    // GFXE.pas:11068
    // The original returns a pointer to the entry, a reference here.
    public ref Entry GetEntry(int Index) => ref Entries[Index];

    // GFXE.pas:11073
    public int Remap(int NewPixelFormatID)
    {
        int Result = PixelFormats.GFXE_OK;
        if (NewPixelFormatID != PixelFormatID)
        {
            switch (PixelFormatID)
            {
                case PixelFormats.pfBGR24:
                    switch (NewPixelFormatID)
                    {
                        case PixelFormats.pfRGB565:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref Entry paletteEntry = ref Entries[I];
                                    paletteEntry.W = (ushort)(((paletteEntry.R & 248) << 8) + ((paletteEntry.G & 252) << 3) + (int)(((uint)paletteEntry.B) >> 3));
                                }
                            }

                            break;
                        case PixelFormats.pfRGB555:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref Entry paletteEntry = ref Entries[I];
                                    paletteEntry.W = (ushort)(((paletteEntry.R & 248) << 7) + ((paletteEntry.G & 248) << 2) + (int)(((uint)paletteEntry.B) >> 3));
                                }
                            }

                            break;
                        case PixelFormats.pfXRGB32:
                            Result = PixelFormats.GFXE_OK;
                            break;
                        default:
                            Result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case PixelFormats.pfRGB24:
                    switch (NewPixelFormatID)
                    {
                        case PixelFormats.pfRGB565:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref Entry paletteEntry = ref Entries[I];
                                    paletteEntry.W = (ushort)(((paletteEntry.B & 248) << 8) + ((paletteEntry.G & 252) << 3) + (int)(((uint)paletteEntry.R) >> 3));
                                }
                            }

                            break;
                        case PixelFormats.pfRGB555:
                            {
                                for (int I = 0; I <= 255; I++)
                                {
                                    ref Entry paletteEntry = ref Entries[I];
                                    paletteEntry.W = (ushort)(((paletteEntry.B & 248) << 7) + ((paletteEntry.G & 248) << 2) + (int)(((uint)paletteEntry.R) >> 3));
                                }
                            }

                            break;
                        case PixelFormats.pfXRGB32:
                            Result = PixelFormats.GFXE_OK;
                            break;
                        default:
                            Result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                default:
                    Result = PixelFormats.GFXE_NOTSUPPORTED;
                    break;
            }

            if (Succeeded(Result))
            {
                PixelFormatID = NewPixelFormatID;
            }
        }
        else
        {
            Result = PixelFormats.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:11140
    public bool Compare(Palette Palette)
    {
        bool Result;
        int I;
        if (Palette.PixelFormatID == PixelFormatID)
        {
            switch (PixelFormatID & 31)
            {
                case 8:
                    Result = true;
                    I = 255;
                    while (I >= 0 && Result)
                    {
                        Result = Palette.Entries[I].W == Entries[I].W;
                        I -= 1;
                    }

                    break;
                case 16:
                    Result = true;
                    I = 255;
                    while (I >= 0 && Result)
                    {
                        Result = Palette.Entries[I].W == Entries[I].W;
                        I -= 1;
                    }

                    break;
                case 24:
                    Result = true;
                    I = 255;
                    while (I >= 0 && Result)
                    {
                        Result = Palette.Entries[I].W == Entries[I].W && Palette.Entries[I].B == Entries[I].B;
                        I -= 1;
                    }

                    break;
                case 32:
                    Result = CompareMem(MemoryMarshal.AsBytes((ReadOnlySpan<Entry>)Entries).Slice(0, 1024), MemoryMarshal.AsBytes((ReadOnlySpan<Entry>)Palette.Entries).Slice(0, 1024), 1024);
                    break;
                default:
                    Result = false;
                    break;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct Entry
    {
        [FieldOffset(0)]
        public byte R;
        [FieldOffset(1)]
        public byte G;
        [FieldOffset(2)]
        public byte B;
        [FieldOffset(3)]
        public byte Reserved;
        [FieldOffset(0)]
        public ushort W;
        [FieldOffset(0)]
        public int Value;
    }
}
