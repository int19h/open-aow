
using System.Runtime.InteropServices;
using Aow.Graphics;

namespace Aow.Game.Draw;

internal static class PixelRemap
{
    internal static void RemapSurface16To16(SurfaceView source, SurfaceView destination, ReadOnlySpan<byte> table, int width, int height) => Blit.RemapSurface16To16(source, destination, table, width, height);

    internal static void RemapSurface16To08(SurfaceView source, SurfaceView destination, ReadOnlySpan<byte> table, int width, int height) => Blit.RemapSurface16To08(source, destination, table, width, height);

    internal static void RemapSurface16To32(SurfaceView source, SurfaceView destination, ReadOnlySpan<byte> table, int width, int height) => Blit.RemapSurface16To32(source, destination, table, width, height);

    internal static void RemapSurface555To32(SurfaceView source, SurfaceView destination, int width, int height) => Blit.RemapSurface555To32(source, destination, width, height);

    internal static void RemapSurface565To32(SurfaceView source, SurfaceView destination, int width, int height) => Blit.RemapSurface565To32(source, destination, width, height);

    internal static void RemapSurface555To565(SurfaceView source, SurfaceView destination, int width, int height) => Blit.RemapSurface555To565(source, destination, width, height);

    internal static void RemapSurface565To555(SurfaceView source, SurfaceView destination, int width, int height) => Blit.RemapSurface565To555(source, destination, width, height);

    internal static void RemapSurface24To565(SurfaceView source, SurfaceView destination, int width, int height)
    {
        for (int Y = 0; Y <= height - 1; Y++)
        {
            var S = source.Row(Y, width * 3);
            var D = PixelFormats.Words(destination.Row(Y, width * 2));
            for (int X = 0; X <= width - 1; X++)
            {
                int I = X * 3;
                D[X] = (ushort)(((S[I + 2] & 248) << 8) + ((S[I + 1] & 252) << 3) + (S[I] >> 3));
            }
        }
    }

    internal static void RemapSurface24To555(SurfaceView source, SurfaceView destination, int width, int height)
    {
        for (int Y = 0; Y <= height - 1; Y++)
        {
            var S = source.Row(Y, width * 3);
            var D = PixelFormats.Words(destination.Row(Y, width * 2));
            for (int X = 0; X <= width - 1; X++)
            {
                int I = X * 3;
                D[X] = (ushort)(((S[I + 2] & 248) << 7) + ((S[I + 1] & 248) << 2) + (S[I] >> 3));
            }
        }
    }

    internal static void RemapSurface555To24(SurfaceView source, SurfaceView destination, int width, int height)
    {
        for (int Y = 0; Y <= height - 1; Y++)
        {
            var S = PixelFormats.Words(source.Row(Y, width * 2));
            var D = destination.Row(Y, width * 3);
            for (int X = 0; X <= width - 1; X++)
            {
                int I = X * 3;
                D[I + 2] = (byte)((S[X] & 31744) >> 7);
                D[I + 1] = (byte)((S[X] & 992) >> 2);
                D[I] = (byte)((S[X] & 31) << 3);
            }
        }
    }

    internal static void RemapSurface565To24(SurfaceView source, SurfaceView destination, int width, int height)
    {
        for (int Y = 0; Y <= height - 1; Y++)
        {
            var S = PixelFormats.Words(source.Row(Y, width * 2));
            var D = destination.Row(Y, width * 3);
            for (int X = 0; X <= width - 1; X++)
            {
                int I = X * 3;
                D[I + 2] = (byte)((S[X] & 63488) >> 8);
                D[I + 1] = (byte)((S[X] & 2016) >> 3);
                D[I] = (byte)((S[X] & 31) << 3);
            }
        }
    }

    internal static void RemapSurface32To565(SurfaceView source, SurfaceView destination, int width, int height) => RemapSurface32To555(source, destination, width, height);

    internal static void RemapSurface32To555(SurfaceView source, SurfaceView destination, int width, int height)
    {
        for (int Y = 0; Y <= height - 1; Y++)
        {
            var S = source.Row(Y, width * 4);
            var D = PixelFormats.Words(destination.Row(Y, width * 2));
            for (int X = 0; X <= width - 1; X++)
            {
                int I = X * 4;
                D[X] = (ushort)(((S[I + 1] & 248) << 7) + ((S[I + 2] & 248) << 2) + (S[I + 3] >> 3));
            }
        }
    }

    internal static void RemapSurface08To24(SurfaceView source, SurfaceView destination, int width, int height, Palette palette)
    {
        for (int Y = 0; Y <= height - 1; Y++)
        {
            var S = source.Row(Y, width);
            var D = destination.Row(Y, width * 3);
            for (int X = 0; X <= width - 1; X++)
            {
                ref Palette.Entry E = ref palette.Entries[S[X]];
                int I = X * 3;
                D[I + 2] = E.Red;
                D[I + 1] = E.Green;
                D[I] = E.Blue;
            }
        }
    }

    internal static int RemapPixelInfo(int pixelInfo, int sourcePixelFormatId, Palette sourcePalette, int destinationPixelFormatId, Palette destinationPalette)
    {
        int result;
        byte R;
        byte G;
        byte B;
        byte A;
        switch (sourcePixelFormatId)
        {
            case PixelFormats.pfPAL8:
                if (sourcePalette != null)
                {
                    R = sourcePalette.Entries[pixelInfo].Red;
                    G = sourcePalette.Entries[pixelInfo].Green;
                    B = sourcePalette.Entries[pixelInfo].Blue;
                    A = 0;
                }
                else
                {
                    throw new InvalidOperationException("Source Palette needed (GFXE)");
                }

                break;
            case PixelFormats.pfRGB565:
                R = (byte)(((int)(((uint)pixelInfo) >> 8)) & 248);
                G = (byte)(((int)(((uint)pixelInfo) >> 3)) & 252);
                B = (byte)((pixelInfo << 3) & 248);
                A = 0;
                break;
            case PixelFormats.pfRGB555:
                R = (byte)(((int)(((uint)pixelInfo) >> 7)) & 248);
                G = (byte)(((int)(((uint)pixelInfo) >> 2)) & 248);
                B = (byte)((pixelInfo << 3) & 248);
                A = 0;
                break;
            case PixelFormats.pfARGB1555:
                R = (byte)(((int)(((uint)pixelInfo) >> 7)) & 248);
                G = (byte)(((int)(((uint)pixelInfo) >> 2)) & 248);
                B = (byte)((pixelInfo << 3) & 248);
                A = (byte)(((int)(((uint)pixelInfo) >> 8)) & 1);
                break;
            case PixelFormats.pfRGB24:
                R = (byte)(((int)(((uint)pixelInfo) >> 16)) & 255);
                G = (byte)(((int)(((uint)pixelInfo) >> 8)) & 255);
                B = (byte)(pixelInfo & 255);
                A = 0;
                break;
            case PixelFormats.pfBGR24:
                R = (byte)(pixelInfo & 255);
                G = (byte)(((int)(((uint)pixelInfo) >> 8)) & 255);
                B = (byte)(((int)(((uint)pixelInfo) >> 16)) & 255);
                A = 0;
                break;
            case PixelFormats.pfRGB32:
                A = (byte)(((int)(((uint)pixelInfo) >> 24)) & 255);
                R = (byte)(((int)(((uint)pixelInfo) >> 16)) & 255);
                G = (byte)(((int)(((uint)pixelInfo) >> 8)) & 255);
                B = (byte)(pixelInfo & 255);
                break;
            default:
                throw new NotSupportedException("Source PixelFormat not supported");
        }

        switch (destinationPixelFormatId)
        {
            case PixelFormats.pfPAL8:
                if (destinationPalette != null)
                {
                    result = destinationPalette.GetNearestColor(R + (G << 8) + (B << 16));
                }
                else
                {
                    throw new InvalidOperationException("Destination Palette needed");
                }

                break;
            case PixelFormats.pfRGB565:
                result = ((R & 248) << 8) + ((G & 252) << 3) + (int)(((uint)(B & 248)) >> 3);
                break;
            case PixelFormats.pfRGB555:
                result = ((R & 248) << 7) + ((G & 248) << 2) + (int)(((uint)(B & 248)) >> 3);
                break;
            case PixelFormats.pfARGB1555:
                result = ((A & 128) << 8) + ((R & 248) << 7) + ((G & 248) << 2) + (int)(((uint)(B & 248)) >> 3);
                break;
            case PixelFormats.pfRGB24:
                result = (R << 16) + (G << 8) + B;
                break;
            case PixelFormats.pfBGR24:
                result = R + (G << 8) + (B << 16);
                break;
            case PixelFormats.pfRGB32:
                result = (A << 24) + (R << 16) + (G << 8) + B;
                break;
            default:
                throw new NotSupportedException("Destination PixelFormat not supported");
        }

        return result;
    }

    internal static void RemapSurface08To555(SurfaceView source, SurfaceView destination, int width, int height, Palette palette) =>
            RemapIndexedSurface(source, destination, width, height, palette, false);

    internal static void RemapSurface08To565(SurfaceView source, SurfaceView destination, int width, int height, Palette palette) =>
            RemapIndexedSurface(source, destination, width, height, palette, true);

    internal static void RemapIndexedSurface(SurfaceView source, SurfaceView destination, int width, int height, Palette palette, bool rgb565)
    {
        for (int y = 0; y < height; y++)
        {
            var sourceLocal = source.Row(y, width);
            var destinationLocal = MemoryMarshal.Cast<byte, ushort>(destination.Row(y, width * 2));
            for (int x = 0; x < width; x++)
            {
                ref Palette.Entry entry = ref palette.Entries[sourceLocal[x]];
                destinationLocal[x] = (ushort)(rgb565
                    ? ((entry.Red & 248) << 8) + ((entry.Green & 252) << 3) + (entry.Blue >> 3)
                    : ((entry.Red & 248) << 7) + ((entry.Green & 248) << 2) + (entry.Blue >> 3));
            }
        }
    }

}
