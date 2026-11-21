#nullable disable warnings

using System.Runtime.InteropServices;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Draw;

public static class PixelFormats
{
    internal static int ElapsedMilliseconds => (int)Clock.TickCount;

    internal static int ElapsedMicroseconds => (int)Clock.TickCount * 1_000;

    internal static Span<ushort> Words(Span<byte> row) => MemoryMarshal.Cast<byte, ushort>(row);

    internal static IReadOnlyList<(int Width, int Height)> IntegerScaleSizes(int desktopWidth, int desktopHeight)
    {
        List<(int Width, int Height)> sizes = [];
        for (int scale = 8; scale >= 1; scale--)
        {
            (int, int) size = (desktopWidth / scale, desktopHeight / scale);
            if (size.Item1 >= 640 && size.Item2 >= 480 && !sizes.Contains(size))
            {
                sizes.Add(size);
            }
        }
        return sizes;
    }

    internal static int Color2PixelInfo(int color, PixelFormat pixelFormatId, Palette? palette)
    {
        int r = color & 0xFF, g = (color >> 8) & 0xFF, b = (color >> 16) & 0xFF;
        return pixelFormatId switch
        {
            PixelFormat.Pal8 => palette is null ? throw new InvalidOperationException("Palette required") : palette.GetNearestColor(color),
            PixelFormat.Rgb555 or PixelFormat.Argb1555 => ((r & 0b1111_1000) << 7) + ((g & 0b1111_1000) << 2) + (b >> 3),
            PixelFormat.Rgb565 => ((r & 0b1111_1000) << 8) + ((g & 0b1111_1100) << 3) + (b >> 3),
            PixelFormat.Rgb24 or PixelFormat.Rgb32 or PixelFormat.Xrgb32 => (r << 16) + (g << 8) + b,
            PixelFormat.Bgr24 => color & 0xFF_FF_FF,
            PixelFormat.None => throw new NotSupportedException("Pixel format"),
            _ => throw new NotSupportedException("Pixel format")
        };
    }

    public static GraphicsResult ColorFillSurfaceDescription(Rect R, int color, in LockedSurface lockedSurface)
    {
        if (lockedSurface.PixelFormatId is not PixelFormat.Rgb555 and not PixelFormat.Argb1555 and not PixelFormat.Rgb565)
        {
            return GraphicsResult.NotSupported;
        }

        int left = Math.Max(R.Left, lockedSurface.ClipRectangle.Left), right = Math.Min(R.Right, lockedSurface.ClipRectangle.Right);
        int top = Math.Max(R.Top, lockedSurface.ClipRectangle.Top), bottom = Math.Min(R.Bottom, lockedSurface.ClipRectangle.Bottom);
        if (left >= right || top >= bottom)
        {
            return GraphicsResult.Ok;
        }

        int pixel = Color2PixelInfo(color, lockedSurface.PixelFormatId, palette: null);
        for (int y = top; y < bottom; y++)
        {
            Blit.FillWord(lockedSurface.Data.AsSpan(lockedSurface.DataIndex + (y * lockedSurface.Pitch) + (left * 2)), count: right - left, value: pixel);
        }

        return GraphicsResult.Ok;
    }
}
