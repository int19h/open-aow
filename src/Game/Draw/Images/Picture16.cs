// Translated from ImageLib.pas.
using System.Runtime.InteropServices;

using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// (ClassID $10): an opaque 16 bit picture. FPitch (bytes per row, a multiple of 4, or of 8 from
// 32 pixels on), FBuffer (the rows from its first byte. The original's FData, FBuffer aligned to
// 8 bytes, is FBuffer itself in the port)
public partial class Picture16 : Image16, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 16;
    static string ILibraryImageClass.Name => "Picture (16bpp)";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new Picture16(owner);

    public Picture16(CustomImageLibrary owner) : base(owner)
    { }

    internal int FPitch;
    internal byte[] FBuffer;

    // The rows of the picture: FBuffer from its first byte, FPitch bytes apart.
    internal SurfaceView View => new(FBuffer, 0, FPitch);

    // ImageLib.pas:2206
    internal void AllocateDiscardableImageData(int AWidth, int AHeight)
    {
        int Size;
        if (AWidth < 32)
        {
            FPitch = ((AWidth << 1) + 3) & ~3;
        }
        else
        {
            FPitch = ((AWidth << 1) + 7) & ~7;
        }

        // The rows start at the first byte of FBuffer, as TPicture08's (graphics.md 6.8).
        Size = FPitch * AHeight + 7;
        FBuffer = MemorySurfaces.Reallocate(FBuffer, Size);
    }

    // ImageLib.pas:2174
    public override void SetXshift(int Value)
    {
        if (Value < 0)
        {
            Value = 0;
        }

        base.SetXshift(Value);
        SetWidth(FWidth);
    }

    // ImageLib.pas:2181
    public override void SetYshift(int Value)
    {
        if (Value < 0)
        {
            Value = 0;
        }

        base.SetYshift(Value);
        SetHeight(FHeight);
    }

    // ImageLib.pas:2188
    public override void SetWidth(int Value)
    {
        if (Value < FXshift + FDataWidth)
        {
            Value = FXshift + FDataWidth;
        }

        base.SetWidth(Value);
    }

    // ImageLib.pas:2194
    public override void SetHeight(int Value)
    {
        if (Value < FYshift + FDataHeight)
        {
            Value = FYshift + FDataHeight;
        }

        base.SetHeight(Value);
    }

    // ImageLib.pas:2200
    public override bool SupportShowMode(ShowMode ShowMode, int PixelFormatID) => (PixelFormatID == 0 || PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555) && Set<ShowMode>.Of(ShowMode.Opaque, ShowMode.Transparent, ShowMode.Blended).Contains(ShowMode);

    // ImageLib.pas:2219
    public override void ReleaseDiscardableImageData()
    {
        if (FDataLoaded)
        {
            FBuffer = null;
            FDataLoaded = false;
        }
    }

    // ImageLib.pas:2228
    public override void ReadDiscardableImageData(Stream Stream)
    {
        base.ReadDiscardableImageData(Stream);
        AllocateDiscardableImageData(FDataWidth, FDataHeight);
        for (int Y = 0, last = FDataHeight - 1; Y <= last; Y++)
        {
            Stream.ReadBytes(FBuffer.AsSpan(Y * FPitch, FDataWidth * 2));
        }
    }

    // ImageLib.pas:2238
    public override void WriteDiscardableImageData(Stream Stream)
    {
        base.WriteDiscardableImageData(Stream);
        for (int Y = 0, last = FDataHeight - 1; Y <= last; Y++)
        {
            Stream.Write(FBuffer.AsSpan(Y * FPitch, FDataWidth * 2));
        }
    }

    // ImageLib.pas:2247
    public override Form CreateGrabForm(Component AOwner, in SurfaceDesc SD, ImagesGrabbedEvent OnImagesGrabbed) => null;

    // ImageLib.pas:2253
    public override bool SupportGrabPixelFormat(int PixelFormatID) => PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555 || PixelFormatID == PixelFormats.pfRGB24 || PixelFormatID == PixelFormats.pfPAL8;

    // ImageLib.pas:2259
    public override bool GrabImage(in SurfaceDesc SD, in Rect R, GrabInfo GrabInfo)
    {
        bool Result;
        try
        {
            Clear();
            Result = true;
            FDataLoaded = true;
            FDataWidth = R.Right - R.Left;
            FDataHeight = R.Bottom - R.Top;
            FWidth = FDataWidth;
            FHeight = FDataHeight;
            FFileName = ExtractFileName(GrabInfo.FileName);
            FFileIndex = GrabInfo.FileIndex;
            FDataSize = FDataWidth * FDataHeight * 2;
            AllocateDiscardableImageData(FWidth, FHeight);
            switch (SD.PixelFormatID)
            {
                case PixelFormats.pfPAL8:
                    {
                        for (int Y = 0, last = FHeight - 1; Y <= last; Y++)
                        {
                            var S = SD.View().Bytes((R.Top + Y) * SD.Pitch + R.Left, FWidth);
                            var D = MemoryMarshal.Cast<byte, ushort>(FBuffer.AsSpan(FPitch * Y, FWidth * 2));
                            for (int X = 0, lastX = FWidth - 1; X <= lastX; X++)
                            {
                                ref readonly GFXEPalette.PaletteEntry P = ref SD.Palette.Entries[S[X]];
                                D[X] = (ushort)(((P.R & 248) << 8) + ((P.G & 252) << 3) + (int)(((uint)P.B) >> 3));
                            }
                        }
                    }

                    FStoredPixelFormatID = PixelFormats.pfRGB565;
                    break;
                case PixelFormats.pfRGB565:
                case PixelFormats.pfRGB555:
                    {
                        for (int I = 0, last = FHeight - 1; I <= last; I++)
                        {
                            SD.View().Bytes((R.Top + I) * SD.Pitch + R.Left * 2, FWidth * 2).CopyTo(FBuffer.AsSpan(FPitch * I, FWidth * 2));
                        }
                    }

                    FStoredPixelFormatID = SD.PixelFormatID;
                    break;
                case PixelFormats.pfRGB24:
                    {
                        for (int Y = 0, last = FHeight - 1; Y <= last; Y++)
                        {
                            var D = MemoryMarshal.Cast<byte, ushort>(FBuffer.AsSpan(FPitch * Y, FWidth * 2));
                            var S24 = SD.View().Bytes((R.Top + Y) * SD.Pitch + R.Left * 3, FWidth * 3);
                            for (int X = 0, lastX = FWidth - 1; X <= lastX; X++)
                            {
                                D[X] = (ushort)(((S24[X * 3 + 2] & 248) << 8) + ((S24[X * 3 + 1] & 252) << 3) + (int)(((uint)S24[X * 3]) >> 3));
                            }
                        }
                    }

                    FStoredPixelFormatID = PixelFormats.pfRGB565;
                    break;
                default:
                    Result = false;
                    break;
            }

            FPixelFormatID = FStoredPixelFormatID;
        }
        catch
        {
            ReleaseDiscardableImageData();
            Result = false;
        }

        return Result;
    }

    // ImageLib.pas:2329
    public override int RemapToPixelFormat(int PixelFormatID)
    {
        int Result = 0;
        if (PixelFormatID != FPixelFormatID)
        {
            switch (PixelFormatID)
            {
                case PixelFormats.pfRGB565:
                    PixelRemap.RemapSurface555to565(View, View, FDataWidth, FDataHeight);
                    FPixelFormatID = PixelFormats.pfRGB565;
                    break;
                case PixelFormats.pfRGB555:
                    PixelRemap.RemapSurface565to555(View, View, FDataWidth, FDataHeight);
                    FPixelFormatID = PixelFormats.pfRGB555;
                    break;
                default:
                    Result = PixelFormats.GFXE_NOTSUPPORTED;
                    break;
            }
        }

        return Result;
    }

    // ImageLib.pas:2349
    public override void ShowOpaque(int X, int Y, in SurfaceDesc SD)
    {
        int DX;
        int Right;
        int W;
        int H;
        int SX;
        int SY;
        int DY;
        int Bottom;
        SurfaceView Src;
        SurfaceView Dst;
        DX = X + FXshift;
        W = FDataWidth;
        Right = FDataWidth + DX;
        if (DX < SD.ClipRect.Left)
        {
            SX = SD.ClipRect.Left - X;
            DX = SD.ClipRect.Left;
            W -= SX;
        }
        else
        {
            SX = 0;
        }

        if (Right > SD.ClipRect.Right)
        {
            W -= Right - SD.ClipRect.Right;
        }

        if (W > 0)
        {
            DY = Y + FYshift;
            H = FDataHeight;
            Bottom = DY + FDataHeight;
            if (DY < SD.ClipRect.Top)
            {
                SY = SD.ClipRect.Top - Y;
                DY = SD.ClipRect.Top;
                H -= SY;
            }
            else
            {
                SY = 0;
            }

            if (Bottom > SD.ClipRect.Bottom)
            {
                H -= Bottom - SD.ClipRect.Bottom;
            }

            if (H > 0)
            {
                if (!FDataLoaded)
                {
                    LoadDiscardableImageData();
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        BlitRects.BltRect(Src, Dst, W * 2, H);
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BltRect(Src, Dst, W * 2, H);
                    }

                    if (FLoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        BlitRects.BltRect(Src, Dst, W * 2, H);
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BltRect(Src, Dst, W * 2, H);
                    }
                }
            }
        }
    }

    // ImageLib.pas:2412
    public override void ShowBlended(int X, int Y, in SurfaceDesc SD, BlendTable Table)
    {
        int DX;
        int Right;
        int W;
        int H;
        int SX;
        int SY;
        int DY;
        int Bottom;
        SurfaceView Src;
        SurfaceView Dst;
        DX = X + FXshift;
        W = FDataWidth;
        Right = FDataWidth + DX;
        if (DX < SD.ClipRect.Left)
        {
            SX = SD.ClipRect.Left - X;
            DX = SD.ClipRect.Left;
            W -= SX;
        }
        else
        {
            SX = 0;
        }

        if (Right > SD.ClipRect.Right)
        {
            W -= Right - SD.ClipRect.Right;
        }

        if (W > 0)
        {
            DY = Y + FYshift;
            H = FDataHeight;
            Bottom = DY + FDataHeight;
            if (DY < SD.ClipRect.Top)
            {
                SY = SD.ClipRect.Top - Y;
                DY = SD.ClipRect.Top;
                H -= SY;
            }
            else
            {
                SY = 0;
            }

            if (Bottom > SD.ClipRect.Bottom)
            {
                H -= Bottom - SD.ClipRect.Bottom;
            }

            if (H > 0)
            {
                if (!FDataLoaded)
                {
                    LoadDiscardableImageData();
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        if (Table.Bits == 5)
                        {
                            switch (FPixelFormatID)
                            {
                                case PixelFormats.pfRGB565:
                                    BlendSurface565(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                                case PixelFormats.pfRGB555:
                                    BlendSurface555(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid BlendTable format");
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (Table.Bits == 5)
                        {
                            switch (FPixelFormatID)
                            {
                                case PixelFormats.pfRGB565:
                                    BlendSurface565(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                                case PixelFormats.pfRGB555:
                                    BlendSurface555(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid BlendTable format");
                        }
                    }

                    if (FLoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        if (Table.Bits == 5)
                        {
                            switch (FPixelFormatID)
                            {
                                case PixelFormats.pfRGB565:
                                    BlendSurface565(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                                case PixelFormats.pfRGB555:
                                    BlendSurface555(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid BlendTable format");
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (Table.Bits == 5)
                        {
                            switch (FPixelFormatID)
                            {
                                case PixelFormats.pfRGB565:
                                    BlendSurface565(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                                case PixelFormats.pfRGB555:
                                    BlendSurface555(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid BlendTable format");
                        }
                    }
                }
            }
        }
    }

    // ImageLib.pas:2511
    public override void ShowTransparent(int X, int Y, in SurfaceDesc SD)
    {
        int DX;
        int Right;
        int W;
        int H;
        int SX;
        int SY;
        int DY;
        int Bottom;
        SurfaceView Src;
        SurfaceView Dst;
        DX = X + FXshift;
        W = FDataWidth;
        Right = FDataWidth + DX;
        if (DX < SD.ClipRect.Left)
        {
            SX = SD.ClipRect.Left - X;
            DX = SD.ClipRect.Left;
            W -= SX;
        }
        else
        {
            SX = 0;
        }

        if (Right > SD.ClipRect.Right)
        {
            W -= Right - SD.ClipRect.Right;
        }

        if (W > 0)
        {
            DY = Y + FYshift;
            H = FDataHeight;
            Bottom = DY + FDataHeight;
            if (DY < SD.ClipRect.Top)
            {
                SY = SD.ClipRect.Top - Y;
                DY = SD.ClipRect.Top;
                H -= SY;
            }
            else
            {
                SY = 0;
            }

            if (Bottom > SD.ClipRect.Bottom)
            {
                H -= Bottom - SD.ClipRect.Bottom;
            }

            if (H > 0)
            {
                if (!FDataLoaded)
                {
                    LoadDiscardableImageData();
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        if (FPixelFormatID == PixelFormats.pfRGB565)
                        {
                            BltTransparent565(Src, Dst, W, H);
                        }
                        else
                        {
                            BltTransparent555(Src, Dst, W, H);
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (FPixelFormatID == PixelFormats.pfRGB565)
                        {
                            BltTransparent565(Src, Dst, W, H);
                        }
                        else
                        {
                            BltTransparent555(Src, Dst, W, H);
                        }
                    }

                    if (FLoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        if (FPixelFormatID == PixelFormats.pfRGB565)
                        {
                            BltTransparent565(Src, Dst, W, H);
                        }
                        else
                        {
                            BltTransparent555(Src, Dst, W, H);
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (FPixelFormatID == PixelFormats.pfRGB565)
                        {
                            BltTransparent565(Src, Dst, W, H);
                        }
                        else
                        {
                            BltTransparent555(Src, Dst, W, H);
                        }
                    }
                }
            }
        }
    }

    private static void BltTransparent565(SurfaceView Src, SurfaceView Dst, int Width, int Height)
    {
        Blit.BltTransparent565(Src, Dst, Width, Height);
    }

    private static void BltTransparent555(SurfaceView Src, SurfaceView Dst, int Width, int Height)
    {
        Blit.BltTransparent555(Src, Dst, Width, Height);
    }

    internal static void BltStretched16(SurfaceView Source, ReadOnlySpan<int> RowTable, ReadOnlySpan<int> ColTable, SurfaceView Dst, int Width, int Height)
    {
        Blit.BltStretched16(Source, RowTable, ColTable, Dst, Width, Height);
    }

    private static void BlendSurface555(SurfaceView Src, SurfaceView Dst, int Width, int Height, ReadOnlySpan<byte> Table)
    {
        Blit.BlendSurface555(Src, Dst, Width, Height, Table);
    }

    private static void BlendSurface565(SurfaceView Src, SurfaceView Dst, int Width, int Height, ReadOnlySpan<byte> Table)
    {
        Blit.BlendSurface565(Src, Dst, Width, Height, Table);
    }

    internal static void BlendStretched555(SurfaceView Source, ReadOnlySpan<int> RowTable, ReadOnlySpan<int> ColTable, SurfaceView Dst, int Width, int Height, ReadOnlySpan<byte> Table)
    {
        Blit.BlendStretched555(Source, RowTable, ColTable, Dst, Width, Height, Table);
    }

    private static void BlendStretched565(SurfaceView Source, ReadOnlySpan<int> RowTable, ReadOnlySpan<int> ColTable, SurfaceView Dst, int Width, int Height, ReadOnlySpan<byte> Table)
    {
        Blit.BlendStretched565(Source, RowTable, ColTable, Dst, Width, Height, Table);
    }

}
