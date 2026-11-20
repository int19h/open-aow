
using System.Runtime.InteropServices;
using Aow.Graphics;

namespace Aow.Game.Draw;

internal static class PixelRemap
{
    internal static void RemapSurface16To16(SurfaceView source, SurfaceView destination, ReadOnlySpan<byte> table, int width, int height) => Blit.RemapSurface16To16(source, d: destination, table, w: width, h: height);

    internal static void RemapSurface16To08(SurfaceView source, SurfaceView destination, ReadOnlySpan<byte> table, int width, int height) => Blit.RemapSurface16To08(source, d: destination, table, w: width, h: height);

    internal static void RemapSurface16To32(SurfaceView source, SurfaceView destination, ReadOnlySpan<byte> table, int width, int height) => Blit.RemapSurface16To32(source, d: destination, table, w: width, h: height);

    internal static void RemapSurface555To32(SurfaceView source, SurfaceView destination, int width, int height) => Blit.RemapSurface555To32(source, d: destination, w: width, h: height);

    internal static void RemapSurface565To32(SurfaceView source, SurfaceView destination, int width, int height) => Blit.RemapSurface565To32(source, d: destination, w: width, h: height);

    internal static void RemapSurface555To565(SurfaceView source, SurfaceView destination, int width, int height) => Blit.RemapSurface555To565(source, d: destination, w: width, h: height);

    internal static void RemapSurface565To555(SurfaceView source, SurfaceView destination, int width, int height) => Blit.RemapSurface565To555(source, d: destination, w: width, h: height);

    internal static void RemapSurface24To565(SurfaceView source, SurfaceView destination, int width, int height)
    {
        for (int y = 0; y <= height - 1; y++)
        {
            Span<byte> s = source.Row(y, count: width * 3);
            Span<ushort> d = PixelFormats.Words(destination.Row(y, count: width * 2));
            for (int x = 0; x <= width - 1; x++)
            {
                int i = x * 3;
                d[x] = (ushort)(((s[i + 2] & 248) << 8) + ((s[i + 1] & 252) << 3) + (s[i] >> 3));
            }
        }
    }

    internal static void RemapSurface24To555(SurfaceView source, SurfaceView destination, int width, int height)
    {
        for (int y = 0; y <= height - 1; y++)
        {
            Span<byte> s = source.Row(y, count: width * 3);
            Span<ushort> d = PixelFormats.Words(destination.Row(y, count: width * 2));
            for (int x = 0; x <= width - 1; x++)
            {
                int i = x * 3;
                d[x] = (ushort)(((s[i + 2] & 248) << 7) + ((s[i + 1] & 248) << 2) + (s[i] >> 3));
            }
        }
    }

    internal static void RemapSurface555To24(SurfaceView source, SurfaceView destination, int width, int height)
    {
        for (int y = 0; y <= height - 1; y++)
        {
            Span<ushort> s = PixelFormats.Words(source.Row(y, count: width * 2));
            Span<byte> d = destination.Row(y, count: width * 3);
            for (int x = 0; x <= width - 1; x++)
            {
                int i = x * 3;
                d[i + 2] = (byte)((s[x] & 31744) >> 7);
                d[i + 1] = (byte)((s[x] & 992) >> 2);
                d[i] = (byte)((s[x] & 31) << 3);
            }
        }
    }

    internal static void RemapSurface565To24(SurfaceView source, SurfaceView destination, int width, int height)
    {
        for (int y = 0; y <= height - 1; y++)
        {
            Span<ushort> s = PixelFormats.Words(source.Row(y, count: width * 2));
            Span<byte> d = destination.Row(y, count: width * 3);
            for (int x = 0; x <= width - 1; x++)
            {
                int i = x * 3;
                d[i + 2] = (byte)((s[x] & 63488) >> 8);
                d[i + 1] = (byte)((s[x] & 2016) >> 3);
                d[i] = (byte)((s[x] & 31) << 3);
            }
        }
    }

    internal static void RemapSurface32To565(SurfaceView source, SurfaceView destination, int width, int height) => RemapSurface32To555(source, destination, width, height);

    internal static void RemapSurface32To555(SurfaceView source, SurfaceView destination, int width, int height)
    {
        for (int y = 0; y <= height - 1; y++)
        {
            Span<byte> s = source.Row(y, count: width * 4);
            Span<ushort> d = PixelFormats.Words(destination.Row(y, count: width * 2));
            for (int x = 0; x <= width - 1; x++)
            {
                int i = x * 4;
                d[x] = (ushort)(((s[i + 1] & 248) << 7) + ((s[i + 2] & 248) << 2) + (s[i + 3] >> 3));
            }
        }
    }

    internal static void RemapSurface08To24(SurfaceView source, SurfaceView destination, int width, int height, Palette palette)
    {
        for (int y = 0; y <= height - 1; y++)
        {
            Span<byte> s = source.Row(y, count: width);
            Span<byte> d = destination.Row(y, count: width * 3);
            for (int x = 0; x <= width - 1; x++)
            {
                Palette.Entry e = palette[s[x]];
                int i = x * 3;
                d[i + 2] = e.Red;
                d[i + 1] = e.Green;
                d[i] = e.Blue;
            }
        }
    }

    internal static int RemapPixelInfo(int pixelInfo, PixelFormat sourcePixelFormatId, Palette sourcePalette, PixelFormat destinationPixelFormatId, Palette destinationPalette)
    {
        byte r;
        byte g;
        byte b;
        byte a;
        switch (sourcePixelFormatId)
        {
            case PixelFormat.Pal8:
                if (sourcePalette is not null)
                {
                    (r, g, b) = (sourcePalette[pixelInfo].Red, sourcePalette[pixelInfo].Green, sourcePalette[pixelInfo].Blue);
                    a = 0;
                }
                else
                {
                    throw new InvalidOperationException("Source Palette needed (GFXE)");
                }

                break;
            case PixelFormat.Rgb565:
                r = (byte)((pixelInfo >>> 8) & 248);
                g = (byte)((pixelInfo >>> 3) & 252);
                b = (byte)((pixelInfo << 3) & 248);
                a = 0;
                break;
            case PixelFormat.Rgb555:
                r = (byte)((pixelInfo >>> 7) & 248);
                g = (byte)((pixelInfo >>> 2) & 248);
                b = (byte)((pixelInfo << 3) & 248);
                a = 0;
                break;
            case PixelFormat.Argb1555:
                r = (byte)((pixelInfo >>> 7) & 248);
                g = (byte)((pixelInfo >>> 2) & 248);
                b = (byte)((pixelInfo << 3) & 248);
                a = (byte)((pixelInfo >>> 8) & 1);
                break;
            case PixelFormat.Rgb24:
                r = (byte)((pixelInfo >>> 16) & 255);
                g = (byte)((pixelInfo >>> 8) & 255);
                b = (byte)(pixelInfo & 255);
                a = 0;
                break;
            case PixelFormat.Bgr24:
                r = (byte)(pixelInfo & 255);
                g = (byte)((pixelInfo >>> 8) & 255);
                b = (byte)((pixelInfo >>> 16) & 255);
                a = 0;
                break;
            case PixelFormat.Rgb32:
                a = (byte)((pixelInfo >>> 24) & 255);
                r = (byte)((pixelInfo >>> 16) & 255);
                g = (byte)((pixelInfo >>> 8) & 255);
                b = (byte)(pixelInfo & 255);
                break;
            case PixelFormat.None:
            case PixelFormat.Xrgb32:
            default:
                throw new NotSupportedException("Source PixelFormat not supported");
        }

        return destinationPixelFormatId switch
        {
            PixelFormat.Pal8 => destinationPalette is not null
                                ? destinationPalette.GetNearestColor(r + (g << 8) + (b << 16))
                                : throw new InvalidOperationException("Destination Palette needed"),
            PixelFormat.Rgb565 => ((r & 248) << 8) + ((g & 252) << 3) + ((b & 248) >>> 3),
            PixelFormat.Rgb555 => ((r & 248) << 7) + ((g & 248) << 2) + ((b & 248) >>> 3),
            PixelFormat.Argb1555 => ((a & 128) << 8) + ((r & 248) << 7) + ((g & 248) << 2) + ((b & 248) >>> 3),
            PixelFormat.Rgb24 => (r << 16) + (g << 8) + b,
            PixelFormat.Bgr24 => r + (g << 8) + (b << 16),
            PixelFormat.Rgb32 => (a << 24) + (r << 16) + (g << 8) + b,
            PixelFormat.None => throw new NotSupportedException("Destination PixelFormat not supported"),
            PixelFormat.Xrgb32 => throw new NotSupportedException("Destination PixelFormat not supported"),
            _ => throw new NotSupportedException("Destination PixelFormat not supported"),
        };
    }

    internal static void RemapSurface08To555(SurfaceView source, SurfaceView destination, int width, int height, Palette palette)
            => RemapIndexedSurface(source, destination, width, height, palette, rgb565: false);

    internal static void RemapSurface08To565(SurfaceView source, SurfaceView destination, int width, int height, Palette palette)
            => RemapIndexedSurface(source, destination, width, height, palette, rgb565: true);

    internal static void RemapIndexedSurface(SurfaceView source, SurfaceView destination, int width, int height, Palette palette, bool rgb565)
    {
        for (int y = 0; y < height; y++)
        {
            Span<byte> sourceLocal = source.Row(y, count: width);
            Span<ushort> destinationLocal = MemoryMarshal.Cast<byte, ushort>(destination.Row(y, count: width * 2));
            for (int x = 0; x < width; x++)
            {
                Palette.Entry entry = palette[sourceLocal[x]];
                destinationLocal[x] = (ushort)(rgb565
                    ? ((entry.Red & 248) << 8) + ((entry.Green & 252) << 3) + (entry.Blue >> 3)
                    : ((entry.Red & 248) << 7) + ((entry.Green & 248) << 2) + (entry.Blue >> 3));
            }
        }
    }
}
