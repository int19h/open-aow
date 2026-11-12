
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public static class PixelFormats
{
    internal static void RegisterBlitRectangle(SurfaceView source, SurfaceView destination, int width, int height) => Blit.RegisterBlitRectangle(source, destination, width, height);

    internal static void RepMovsdBlitRectangle(SurfaceView source, SurfaceView destination, int width, int height) => Blit.RepMovsdBlitRectangle(source, destination, width, height);

    internal static int GetElapsedMilliseconds() => (int)RtlUnit.Clock.TickCount;

    internal static int GetElapsedMicroseconds() => (int)RtlUnit.Clock.TickCount * 1000;

    internal const byte GFXE_OK = 0;

    internal const int GFXE_ERROR = -2147483647;

    public const int GFXE_NOTSUPPORTED = -2147483646;

    public const int GFXE_PIXELFORMATNOTSUPPORTED = -2147483645;

    internal const int GFXE_BITMAPFORMATNOTSUPPORTED = -2147483644;

    public const byte pfPAL8 = 8;

    public const int pfRGB555 = 1431343888;

    internal const int pfARGB1555 = 1431409424;

    public const int pfRGB565 = 1448121104;

    internal const int pfRGB24 = -2004839656;

    public const int pfBGR24 = -2004870376;

    internal const int pfRGB32 = -2004315360;

    internal const int pfXRGB32 = -2004839648;

    internal const byte DD_OK = 0;

    internal static bool ValidImageFile(string fileName)
    {
        var extractFileExt = ExtractFileExt(fileName);
        return GraphicsState.ImageFileClassList.FindClass(extractFileExt) != null;
    }

    internal static Surface CreateSurfaceFromImageFile(string fileName)
    {
        Surface result;
        ImageFileClass C;
        var extractFileExt = ExtractFileExt(fileName);
        C = GraphicsState.ImageFileClassList.FindClass(extractFileExt);
        if (C != null)
        {
            result = C.CreateSurface(fileName);
        }
        else
        {
            result = null;
        }

        return result;
    }

    internal static int MaskToPixelFormatId(int bitCount, int rMask, int gMask, int bMask, int mask)
    {
        ByteArray4 order = default;
        bool swapped;
        Int32Array4 masks = default;
        int bits;
        int T;
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
            for (int I = 0; I <= 2; I++)
            {
                if (((masks[order[I]] < 0 || masks[order[I + 1]] < masks[order[I]]) && masks[order[I + 1]] > 0) || (masks[order[I]] == 0 && masks[order[I + 1]] != 0))
                {
                    T = order[I];
                    order[I] = order[I + 1];
                    order[I + 1] = (byte)T;
                    swapped = true;
                }
            }
        }
        while (!!swapped);
        for (int I = 0; I <= 3; I++)
        {
            result = result + (I << (order[I] * 2 + 8));
        }

        for (int I = 0; I <= 3; I++)
        {
            bits = 0;
            while (masks[I] != 0)
            {
                if ((masks[I] & 1) != 0)
                {
                    bits += 1;
                }

                masks[I] = (int)(((uint)masks[I]) >> 1);
            }

            result = result + (bits << (I * 4 + 16));
        }

        return result;
    }

    internal static Span<ushort> Words(Span<byte> row) => MemoryMarshal.Cast<byte, ushort>(row);

    internal static int PixelInfo2Color(int pixelInfo, int pixelFormatId, Palette palette) => PixelRemap.RemapPixelInfo(pixelInfo, pixelFormatId, palette, pfBGR24, null);

    internal static string GetErrorMessage(int errorCode)
    {
        string result;
        switch (errorCode)
        {
            case GFXE_OK:
                result = "No ERROR occurred!";
                break;
            case GFXE_ERROR:
                result = "Undefine ERROR occurred";
                break;
            case GFXE_NOTSUPPORTED:
                result = "Function is not supported";
                break;
            case GFXE_PIXELFORMATNOTSUPPORTED:
                result = "PixelFormat is not supported";
                break;
            case GFXE_BITMAPFORMATNOTSUPPORTED:
                result = "Bitmap format is not supported";
                break;
            default:
                result = "Undocumented Error (Error Code:" + IntToStr(errorCode) + ")";
                break;
        }

        return result;
    }

    internal static void ForceOptimal16BitPixelFormatId(int pixelFormatId)
    {
        if (pixelFormatId == pfRGB565 || pixelFormatId == pfRGB555)
        {
            GraphicsState.s_forcedPixelFormatId = pixelFormatId;
        }
        else
        {
            GraphicsState.s_forcedPixelFormatId = 0;
        }
    }

    internal static int GetOptimalPixelFormatId(int bitCount)
    {
        /* TODO: Implement Forms.Width in the platform or editor service. */
        throw new NotImplementedException();
    }

    internal static IReadOnlyList<(int Width, int Height)> IntegerScaleSizes(int desktopWidth, int desktopHeight)
    {
        var sizes = new List<(int Width, int Height)>();
        for (int scale = 8; scale >= 1; scale--)
        {
            var size = (desktopWidth / scale, desktopHeight / scale);
            if (size.Item1 >= 640 && size.Item2 >= 480 && !sizes.Contains(size)) sizes.Add(size);
        }
        return sizes;
    }

    internal static void FillWord(Span<byte> destination, int count, int value) => Blit.FillWord(destination, count, value);

    internal static void FillTripleByte(Span<byte> destination, int count, int value) => Blit.FillTripleByte(destination, count, value);

    public static int ReadTimeStampCounter() => 0;

    public static bool CpuIdSupported() => false;

    public static bool MmxSupport() => false;

    internal static int Color2PixelInfo(int color, int pixelFormatId, Palette palette)
    {
        int r = color & 255, g = (color >> 8) & 255, b = (color >> 16) & 255;
        return pixelFormatId switch
        {
            pfPAL8 => palette is null ? throw new InvalidOperationException("Palette required") : palette.GetNearestColor(color),
            pfRGB555 or pfARGB1555 => ((r & 248) << 7) + ((g & 248) << 2) + (b >> 3),
            pfRGB565 => ((r & 248) << 8) + ((g & 252) << 3) + (b >> 3),
            pfRGB24 or pfRGB32 or pfXRGB32 => (r << 16) + (g << 8) + b,
            pfBGR24 => color & 0xFFFFFF,
            _ => throw new NotSupportedException("Pixel format")
        };
    }

    public static int ColorFillSurfaceDescription(Rect R, int color, in LockedSurface lockedSurface)
    {
        if (lockedSurface.PixelFormatId != pfRGB555 && lockedSurface.PixelFormatId != pfARGB1555 && lockedSurface.PixelFormatId != pfRGB565) return GFXE_NOTSUPPORTED;
        int left = Math.Max(R.Left, lockedSurface.ClipRectangle.Left), right = Math.Min(R.Right, lockedSurface.ClipRectangle.Right);
        int top = Math.Max(R.Top, lockedSurface.ClipRectangle.Top), bottom = Math.Min(R.Bottom, lockedSurface.ClipRectangle.Bottom);
        if (left >= right || top >= bottom) return GFXE_OK;
        int pixel = Color2PixelInfo(color, lockedSurface.PixelFormatId, null);
        for (int y = top; y < bottom; y++) Blit.FillWord(lockedSurface.Data.AsSpan(lockedSurface.DataIndex + y * lockedSurface.Pitch + left * 2), right - left, pixel);
        return GFXE_OK;
    }

}
