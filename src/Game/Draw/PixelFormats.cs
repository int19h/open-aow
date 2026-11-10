
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public static class PixelFormats
{
    internal static void RegisterBltRect(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.RegisterBltRect(Src, Dst, Width, Height);

    internal static void RepMovsdBltRect(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.RepMovsdBltRect(Src, Dst, Width, Height);

    internal static int GetElapsedMilliSeconds() => (int)RtlUnit.Clock.TickCount;

    internal static int GetElapsedMicroSeconds() => (int)RtlUnit.Clock.TickCount * 1000;

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

    internal static bool ValidImageFile(string FileName)
    {
        var extractFileExt = ExtractFileExt(FileName);
        return GfxState.ImageFileClassList.FindClass(extractFileExt) != null;
    }

    internal static Surface CreateSurfaceFromImageFile(string FileName)
    {
        Surface Result;
        ImageFileClass C;
        var extractFileExt = ExtractFileExt(FileName);
        C = GfxState.ImageFileClassList.FindClass(extractFileExt);
        if (C != null)
        {
            Result = C.CreateSurface(FileName);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    internal static int MASKtoPFID(int BitCount, int RMask, int GMask, int BMask, int AMask)
    {
        FixedArray_byte_4 Order = default;
        bool Swapped;
        FixedArray_int_4 Masks = default;
        int Bits;
        int T;
        int Result = BitCount;
        Masks[0] = AMask;
        Order[3] = 3;
        Masks[1] = BMask;
        Order[0] = 0;
        Masks[2] = GMask;
        Order[1] = 1;
        Masks[3] = RMask;
        Order[2] = 2;
        do
        {
            Swapped = false;
            for (int I = 0; I <= 2; I++)
            {
                if (((Masks[Order[I]] < 0 || Masks[Order[I + 1]] < Masks[Order[I]]) && Masks[Order[I + 1]] > 0) || (Masks[Order[I]] == 0 && Masks[Order[I + 1]] != 0))
                {
                    T = Order[I];
                    Order[I] = Order[I + 1];
                    Order[I + 1] = (byte)T;
                    Swapped = true;
                }
            }
        }
        while (!!Swapped);
        for (int I = 0; I <= 3; I++)
        {
            Result = Result + (I << (Order[I] * 2 + 8));
        }

        for (int I = 0; I <= 3; I++)
        {
            Bits = 0;
            while (Masks[I] != 0)
            {
                if ((Masks[I] & 1) != 0)
                {
                    Bits += 1;
                }

                Masks[I] = (int)(((uint)Masks[I]) >> 1);
            }

            Result = Result + (Bits << (I * 4 + 16));
        }

        return Result;
    }

    internal static Span<ushort> Words(Span<byte> row) => MemoryMarshal.Cast<byte, ushort>(row);

    internal static int PixelInfo2Color(int PixelInfo, int PixelFormatID, GFXEPalette Palette) => PixelRemap.RemapPixelInfo(PixelInfo, PixelFormatID, Palette, pfBGR24, null);

    internal static string GetErrorMsg(int ErrorCode)
    {
        string Result;
        switch (ErrorCode)
        {
            case GFXE_OK:
                Result = "No ERROR occurred!";
                break;
            case GFXE_ERROR:
                Result = "Undefine ERROR occurred";
                break;
            case GFXE_NOTSUPPORTED:
                Result = "Function is not supported";
                break;
            case GFXE_PIXELFORMATNOTSUPPORTED:
                Result = "PixelFormat is not supported";
                break;
            case GFXE_BITMAPFORMATNOTSUPPORTED:
                Result = "Bitmap format is not supported";
                break;
            default:
                Result = "Undocumented Error (Error Code:" + IntToStr(ErrorCode) + ")";
                break;
        }

        return Result;
    }

    internal static void ForceOptimal16BitPixelFormatID(int PixelFormatID)
    {
        if (PixelFormatID == pfRGB565 || PixelFormatID == pfRGB555)
        {
            GfxState.ForcedPixelFormatID = PixelFormatID;
        }
        else
        {
            GfxState.ForcedPixelFormatID = 0;
        }
    }

    internal static int GetOptimalPixelFormatID(int BitCount)
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

    internal static void FillTrippleByte(Span<byte> destination, int count, int value) => Blit.FillTrippleByte(destination, count, value);

    public static int RDTSC() => 0;

    public static bool CPUIDSupported() => false;

    public static bool MMXsupport() => false;

    internal static int Color2PixelInfo(int Color, int PixelFormatID, GFXEPalette Palette)
    {
        int r = Color & 255, g = (Color >> 8) & 255, b = (Color >> 16) & 255;
        return PixelFormatID switch
        {
            pfPAL8 => Palette is null ? throw new InvalidOperationException("Palette required") : Palette.GetNearestColor(Color),
            pfRGB555 or pfARGB1555 => ((r & 248) << 7) + ((g & 248) << 2) + (b >> 3),
            pfRGB565 => ((r & 248) << 8) + ((g & 252) << 3) + (b >> 3),
            pfRGB24 or pfRGB32 or pfXRGB32 => (r << 16) + (g << 8) + b,
            pfBGR24 => Color & 0xFFFFFF,
            _ => throw new NotSupportedException("Pixel format")
        };
    }

    public static int ColorFillSD(Rect R, int Color, in SurfaceDesc SD)
    {
        if (SD.PixelFormatID != pfRGB555 && SD.PixelFormatID != pfARGB1555 && SD.PixelFormatID != pfRGB565) return GFXE_NOTSUPPORTED;
        int left = Math.Max(R.Left, SD.ClipRect.Left), right = Math.Min(R.Right, SD.ClipRect.Right);
        int top = Math.Max(R.Top, SD.ClipRect.Top), bottom = Math.Min(R.Bottom, SD.ClipRect.Bottom);
        if (left >= right || top >= bottom) return GFXE_OK;
        int pixel = Color2PixelInfo(Color, SD.PixelFormatID, null);
        for (int y = top; y < bottom; y++) Blit.FillWord(SD.Data.AsSpan(SD.DataIndex + y * SD.Pitch + left * 2), right - left, pixel);
        return GFXE_OK;
    }

}
