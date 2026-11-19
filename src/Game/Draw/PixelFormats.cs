#nullable disable

using System.Runtime.InteropServices;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Draw;

public static class PixelFormats
{
    internal static void RegisterBlitRectangle(SurfaceView source, SurfaceView destination, int width, int height) => Blit.RegisterBlitRectangle(source, destination, width, height);

    internal static void RepMovsdBlitRectangle(SurfaceView source, SurfaceView destination, int width, int height) => Blit.RepMovsdBlitRectangle(source, destination, width, height);

    internal static int GetElapsedMilliseconds() => (int)Clock.TickCount;

    internal static int GetElapsedMicroseconds() => (int)Clock.TickCount * 1000;

    internal static bool ValidImageFile(string fileName)
    {
        string extractFileExt = ExtractFileExt(fileName);
        return GraphicsState.ImageFileClassList.FindClass(extractFileExt) != null;
    }

    internal static Surface CreateSurfaceFromImageFile(string fileName)
    {
        Surface result;
        ImageFileClass c;
        string extractFileExt = ExtractFileExt(fileName);
        c = GraphicsState.ImageFileClassList.FindClass(extractFileExt);
        result = c?.CreateSurface(fileName);

        return result;
    }

    internal static PixelFormat MaskToPixelFormatId(int bitCount, int rMask, int gMask, int bMask, int mask)
    {
        Span<byte> order = stackalloc byte[4];
        bool swapped;
        Span<int> masks = stackalloc int[4];
        int bits;
        int t;
        int result = bitCount;
        masks[0] = mask;
        order[3] = 3;
        masks[1] = bMask;
        order[0] = 0;
        masks[2] = gMask;
        order[1] = 1;
        masks[3] = rMask;
        order[2] = 2;
        do
        {
            swapped = false;
            for (int i = 0; i <= 2; i++)
            {
                if (((masks[order[i]] < 0 || masks[order[i + 1]] < masks[order[i]]) && masks[order[i + 1]] > 0) || (masks[order[i]] == 0 && masks[order[i + 1]] != 0))
                {
                    t = order[i];
                    order[i] = order[i + 1];
                    order[i + 1] = (byte)t;
                    swapped = true;
                }
            }
        }
        while (!!swapped);
        for (int i = 0; i <= 3; i++)
        {
            result += i << ((order[i] * 2) + 8);
        }

        for (int i = 0; i <= 3; i++)
        {
            bits = 0;
            while (masks[i] != 0)
            {
                if ((masks[i] & 1) != 0)
                {
                    bits += 1;
                }

                masks[i] = (int)(((uint)masks[i]) >> 1);
            }

            result += bits << ((i * 4) + 16);
        }

        return (PixelFormat)result;
    }

    internal static Span<ushort> Words(Span<byte> row) => MemoryMarshal.Cast<byte, ushort>(row);

    internal static int PixelInfo2Color(int pixelInfo, PixelFormat pixelFormatId, Palette palette) => PixelRemap.RemapPixelInfo(pixelInfo, pixelFormatId, palette, PixelFormat.Bgr24, null);

    internal static string GetErrorMessage(GraphicsResult errorCode)
    {
        string result = errorCode switch
        {
            GraphicsResult.Ok => "No ERROR occurred!",
            GraphicsResult.Error => "Undefine ERROR occurred",
            GraphicsResult.NotSupported => "Function is not supported",
            GraphicsResult.PixelFormatNotSupported => "PixelFormat is not supported",
            GraphicsResult.BitmapFormatNotSupported => "Bitmap format is not supported",
            GraphicsResult.CannotLock => $"Undocumented Error (Error Code:{IntToStr((int)errorCode)})",
            _ => $"Undocumented Error (Error Code:{IntToStr((int)errorCode)})",
        };
        return result;
    }

    internal static void ForceOptimal16BitPixelFormatId(PixelFormat pixelFormatId) => GraphicsState.ForcedPixelFormatId = pixelFormatId is PixelFormat.Rgb565 or PixelFormat.Rgb555
        ? pixelFormatId
        : PixelFormat.None;

    internal static PixelFormat GetOptimalPixelFormatId(int bitCount) =>
        /* TODO: Implement Forms.Width in the platform or editor service. */
        throw new NotImplementedException();

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

    internal static void FillWord(Span<byte> destination, int count, int value) => Blit.FillWord(destination, count, value);

    internal static void FillTripleByte(Span<byte> destination, int count, int value) => Blit.FillTripleByte(destination, count, value);

    public static int ReadTimeStampCounter() => 0;

    public static bool CpuIdSupported() => false;

    public static bool MmxSupport() => false;

    internal static int Color2PixelInfo(int color, PixelFormat pixelFormatId, Palette palette)
    {
        int r = color & 255, g = (color >> 8) & 255, b = (color >> 16) & 255;
        return pixelFormatId switch
        {
            PixelFormat.Pal8 => palette is null ? throw new InvalidOperationException("Palette required") : palette.GetNearestColor(color),
            PixelFormat.Rgb555 or PixelFormat.Argb1555 => ((r & 248) << 7) + ((g & 248) << 2) + (b >> 3),
            PixelFormat.Rgb565 => ((r & 248) << 8) + ((g & 252) << 3) + (b >> 3),
            PixelFormat.Rgb24 or PixelFormat.Rgb32 or PixelFormat.Xrgb32 => (r << 16) + (g << 8) + b,
            PixelFormat.Bgr24 => color & 0xFFFFFF,
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

        int pixel = Color2PixelInfo(color, lockedSurface.PixelFormatId, null);
        for (int y = top; y < bottom; y++)
        {
            Blit.FillWord(lockedSurface.Data.AsSpan(lockedSurface.DataIndex + (y * lockedSurface.Pitch) + (left * 2)), right - left, pixel);
        }

        return GraphicsResult.Ok;
    }

}
