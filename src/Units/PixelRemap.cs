using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

internal static class PixelRemap
{
    internal static void RemapSurface16to16(SurfaceView Src, SurfaceView Dst, ReadOnlySpan<byte> Table, int Width, int Height) => Blit.RemapSurface16to16(Src, Dst, Table, Width, Height);

    internal static void RemapSurface16to08(SurfaceView Src, SurfaceView Dst, ReadOnlySpan<byte> Table, int Width, int Height) => Blit.RemapSurface16to08(Src, Dst, Table, Width, Height);

    internal static void RemapSurface16to32(SurfaceView Src, SurfaceView Dst, ReadOnlySpan<byte> Table, int Width, int Height) => Blit.RemapSurface16to32(Src, Dst, Table, Width, Height);

    internal static void RemapSurface555to32(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.RemapSurface555to32(Src, Dst, Width, Height);

    internal static void RemapSurface565to32(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.RemapSurface565to32(Src, Dst, Width, Height);

    internal static void RemapSurface555to565(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.RemapSurface555to565(Src, Dst, Width, Height);

    internal static void RemapSurface565to555(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.RemapSurface565to555(Src, Dst, Width, Height);

    internal static void RemapSurface24to565(SurfaceView Src, SurfaceView Dst, int Width, int Height)
    {
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            var S = Src.Row(Y, Width * 3);
            var D = PixelFormats.Words(Dst.Row(Y, Width * 2));
            for (int X = 0; X <= Width - 1; X++)
            {
                int I = X * 3;
                D[X] = (ushort)(((S[I + 2] & 248) << 8) + ((S[I + 1] & 252) << 3) + (S[I] >> 3));
            }
        }
    }

    internal static void RemapSurface24to555(SurfaceView Src, SurfaceView Dst, int Width, int Height)
    {
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            var S = Src.Row(Y, Width * 3);
            var D = PixelFormats.Words(Dst.Row(Y, Width * 2));
            for (int X = 0; X <= Width - 1; X++)
            {
                int I = X * 3;
                D[X] = (ushort)(((S[I + 2] & 248) << 7) + ((S[I + 1] & 248) << 2) + (S[I] >> 3));
            }
        }
    }

    internal static void RemapSurface555to24(SurfaceView Src, SurfaceView Dst, int Width, int Height)
    {
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            var S = PixelFormats.Words(Src.Row(Y, Width * 2));
            var D = Dst.Row(Y, Width * 3);
            for (int X = 0; X <= Width - 1; X++)
            {
                int I = X * 3;
                D[I + 2] = (byte)((S[X] & 31744) >> 7);
                D[I + 1] = (byte)((S[X] & 992) >> 2);
                D[I] = (byte)((S[X] & 31) << 3);
            }
        }
    }

    internal static void RemapSurface565to24(SurfaceView Src, SurfaceView Dst, int Width, int Height)
    {
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            var S = PixelFormats.Words(Src.Row(Y, Width * 2));
            var D = Dst.Row(Y, Width * 3);
            for (int X = 0; X <= Width - 1; X++)
            {
                int I = X * 3;
                D[I + 2] = (byte)((S[X] & 63488) >> 8);
                D[I + 1] = (byte)((S[X] & 2016) >> 3);
                D[I] = (byte)((S[X] & 31) << 3);
            }
        }
    }

    internal static void RemapSurface32to565(SurfaceView Src, SurfaceView Dst, int Width, int Height) => RemapSurface32to555(Src, Dst, Width, Height);

    internal static void RemapSurface32to555(SurfaceView Src, SurfaceView Dst, int Width, int Height)
    {
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            var S = Src.Row(Y, Width * 4);
            var D = PixelFormats.Words(Dst.Row(Y, Width * 2));
            for (int X = 0; X <= Width - 1; X++)
            {
                int I = X * 4;
                D[X] = (ushort)(((S[I + 1] & 248) << 7) + ((S[I + 2] & 248) << 2) + (S[I + 3] >> 3));
            }
        }
    }

    internal static void RemapSurface08to24(SurfaceView Src, SurfaceView Dst, int Width, int Height, GFXEPalette Palette)
    {
        for (int Y = 0; Y <= Height - 1; Y++)
        {
            var S = Src.Row(Y, Width);
            var D = Dst.Row(Y, Width * 3);
            for (int X = 0; X <= Width - 1; X++)
            {
                ref GFXEPalette.PaletteEntry E = ref Palette.Entries[S[X]];
                int I = X * 3;
                D[I + 2] = E.R;
                D[I + 1] = E.G;
                D[I] = E.B;
            }
        }
    }

    internal static int RemapPixelInfo(int PixelInfo, int SrcPixelFormatID, GFXEPalette SrcPalette, int DestPixelFormatID, GFXEPalette DestPalette)
    {
        int Result;
        byte R;
        byte G;
        byte B;
        byte A;
        switch (SrcPixelFormatID)
        {
            case PixelFormats.pfPAL8:
                if (SrcPalette != null)
                {
                    R = SrcPalette.Entries[PixelInfo].R;
                    G = SrcPalette.Entries[PixelInfo].G;
                    B = SrcPalette.Entries[PixelInfo].B;
                    A = 0;
                }
                else
                {
                    throw new InvalidOperationException("Source Palette needed (GFXE)");
                }

                break;
            case PixelFormats.pfRGB565:
                R = (byte)(((int)(((uint)PixelInfo) >> 8)) & 248);
                G = (byte)(((int)(((uint)PixelInfo) >> 3)) & 252);
                B = (byte)((PixelInfo << 3) & 248);
                A = 0;
                break;
            case PixelFormats.pfRGB555:
                R = (byte)(((int)(((uint)PixelInfo) >> 7)) & 248);
                G = (byte)(((int)(((uint)PixelInfo) >> 2)) & 248);
                B = (byte)((PixelInfo << 3) & 248);
                A = 0;
                break;
            case PixelFormats.pfARGB1555:
                R = (byte)(((int)(((uint)PixelInfo) >> 7)) & 248);
                G = (byte)(((int)(((uint)PixelInfo) >> 2)) & 248);
                B = (byte)((PixelInfo << 3) & 248);
                A = (byte)(((int)(((uint)PixelInfo) >> 8)) & 1);
                break;
            case PixelFormats.pfRGB24:
                R = (byte)(((int)(((uint)PixelInfo) >> 16)) & 255);
                G = (byte)(((int)(((uint)PixelInfo) >> 8)) & 255);
                B = (byte)(PixelInfo & 255);
                A = 0;
                break;
            case PixelFormats.pfBGR24:
                R = (byte)(PixelInfo & 255);
                G = (byte)(((int)(((uint)PixelInfo) >> 8)) & 255);
                B = (byte)(((int)(((uint)PixelInfo) >> 16)) & 255);
                A = 0;
                break;
            case PixelFormats.pfRGB32:
                A = (byte)(((int)(((uint)PixelInfo) >> 24)) & 255);
                R = (byte)(((int)(((uint)PixelInfo) >> 16)) & 255);
                G = (byte)(((int)(((uint)PixelInfo) >> 8)) & 255);
                B = (byte)(PixelInfo & 255);
                break;
            default:
                throw new NotSupportedException("Source PixelFormat not supported");
        }

        switch (DestPixelFormatID)
        {
            case PixelFormats.pfPAL8:
                if (DestPalette != null)
                {
                    Result = DestPalette.GetNearestColor(R + (G << 8) + (B << 16));
                }
                else
                {
                    throw new InvalidOperationException("Destination Palette needed");
                }

                break;
            case PixelFormats.pfRGB565:
                Result = ((R & 248) << 8) + ((G & 252) << 3) + (int)(((uint)(B & 248)) >> 3);
                break;
            case PixelFormats.pfRGB555:
                Result = ((R & 248) << 7) + ((G & 248) << 2) + (int)(((uint)(B & 248)) >> 3);
                break;
            case PixelFormats.pfARGB1555:
                Result = ((A & 128) << 8) + ((R & 248) << 7) + ((G & 248) << 2) + (int)(((uint)(B & 248)) >> 3);
                break;
            case PixelFormats.pfRGB24:
                Result = (R << 16) + (G << 8) + B;
                break;
            case PixelFormats.pfBGR24:
                Result = R + (G << 8) + (B << 16);
                break;
            case PixelFormats.pfRGB32:
                Result = (A << 24) + (R << 16) + (G << 8) + B;
                break;
            default:
                throw new NotSupportedException("Destination PixelFormat not supported");
        }

        return Result;
    }

    internal static void RemapSurface08to555(SurfaceView source, SurfaceView destination, int width, int height, GFXEPalette palette) =>
            RemapIndexedSurface(source, destination, width, height, palette, false);

    internal static void RemapSurface08to565(SurfaceView source, SurfaceView destination, int width, int height, GFXEPalette palette) =>
            RemapIndexedSurface(source, destination, width, height, palette, true);

    internal static void RemapIndexedSurface(SurfaceView source, SurfaceView destination, int width, int height, GFXEPalette palette, bool rgb565)
    {
        for (int y = 0; y < height; y++)
        {
            var src = source.Row(y, width);
            var dst = MemoryMarshal.Cast<byte, ushort>(destination.Row(y, width * 2));
            for (int x = 0; x < width; x++)
            {
                ref GFXEPalette.PaletteEntry entry = ref palette.Entries[src[x]];
                dst[x] = (ushort)(rgb565
                    ? ((entry.R & 248) << 8) + ((entry.G & 252) << 3) + (entry.B >> 3)
                    : ((entry.R & 248) << 7) + ((entry.G & 248) << 2) + (entry.B >> 3));
            }
        }
    }

}
