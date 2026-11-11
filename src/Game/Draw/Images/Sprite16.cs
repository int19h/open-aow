// Translated from ImageLib.pas.
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// (ClassID $16): a 16 bit picture with a transparent color, cut to the smallest rectangle
// holding its opaque pixels. FPitch, FBuffer (as TPicture16), FTransparentColor (in
// FStoredPixelFormatID), FColorKey (the transparent color in FPixelFormatID), FSpriteX,
// FSpriteY (of the pixels in the grabbed rectangle), FSpriteWidth, FSpriteHeight (of the
// pixels)
public partial class Sprite16 : Image16, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 22;
    static string ILibraryImageClass.Name => "Sprite (16bpp)";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new Sprite16(owner);

    internal int FPitch;
    internal byte[] FBuffer;
    internal int FTransparentColor;
    internal int FColorKey;
    internal int FSpriteX;
    internal int FSpriteY;
    internal int FSpriteWidth;
    internal int FSpriteHeight;

    // The rows of the sprite: FBuffer from its first byte, FPitch bytes apart.
    internal SurfaceView View => new(FBuffer, 0, FPitch);

    // ImageLib.pas:4302
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

    // ImageLib.pas:4247
    public Sprite16(CustomImageLibrary owner) : base(owner)
    {
    }

    // ImageLib.pas:4262
    public override void SetXshift(int Value)
    {
        if (Value < -FSpriteX)
        {
            Value = -FSpriteX;
        }

        base.SetXshift(Value);
        SetWidth(FWidth);
    }

    // ImageLib.pas:4269
    public override void SetYshift(int Value)
    {
        if (Value < -FSpriteY)
        {
            Value = -FSpriteY;
        }

        base.SetYshift(Value);
        SetHeight(FHeight);
    }

    // ImageLib.pas:4276
    public override void SetWidth(int Value)
    {
        int W;
        W = FXshift + FSpriteX + FSpriteWidth;
        if (Value < W)
        {
            Value = W;
        }

        base.SetWidth(Value);
    }

    // ImageLib.pas:4285
    public override void SetHeight(int Value)
    {
        int W;
        W = FYshift + FSpriteY + FSpriteHeight;
        if (Value < W)
        {
            Value = W;
        }

        base.SetHeight(Value);
    }

    // ImageLib.pas:4294
    public override void GetVisualDimensions(ref Rect R)
    {
        R.Left = FXshift + FSpriteX;
        R.Right = R.Left + FSpriteWidth;
        R.Top = FYshift + FSpriteY;
        R.Bottom = R.Top + FSpriteHeight;
    }

    // ImageLib.pas:4315
    public override void ReleaseDiscardableImageData()
    {
        if (FDataLoaded)
        {
            FBuffer = null;
            FDataLoaded = false;
        }
    }

    // ImageLib.pas:4324
    public override void ReadImageData(Stream Stream)
    {
        base.ReadImageData(Stream);
        Stream.ReadExactly(Bytes.Of(ref FSpriteWidth, 4));
        Stream.ReadExactly(Bytes.Of(ref FSpriteHeight, 4));
        Stream.ReadExactly(Bytes.Of(ref FSpriteX, 4));
        Stream.ReadExactly(Bytes.Of(ref FSpriteY, 4));
        Stream.ReadExactly(Bytes.Of(ref FTransparentColor, 4));
    }

    // ImageLib.pas:4334
    public override void WriteImageData(Stream Stream)
    {
        base.WriteImageData(Stream);
        Stream.Write(Bytes.Of(ref FSpriteWidth, 4));
        Stream.Write(Bytes.Of(ref FSpriteHeight, 4));
        Stream.Write(Bytes.Of(ref FSpriteX, 4));
        Stream.Write(Bytes.Of(ref FSpriteY, 4));
        Stream.Write(Bytes.Of(ref FTransparentColor, 4));
    }

    // ImageLib.pas:4344
    public override void ReadDiscardableImageData(Stream Stream)
    {
        base.ReadDiscardableImageData(Stream);
        AllocateDiscardableImageData(FSpriteWidth, FSpriteHeight);
        for (int Y = 0, last = FSpriteHeight - 1; Y <= last; Y++)
        {
            Stream.ReadExactly(FBuffer.AsSpan(Y * FPitch, FSpriteWidth * 2));
        }

        FColorKey = FTransparentColor;
    }

    // ImageLib.pas:4355
    public override void WriteDiscardableImageData(Stream Stream)
    {
        base.WriteDiscardableImageData(Stream);
        for (int Y = 0, last = FSpriteHeight - 1; Y <= last; Y++)
        {
            Stream.Write(FBuffer.AsSpan(Y * FPitch, FSpriteWidth * 2));
        }
    }

    // ImageLib.pas:4364
    public override int RemapToPixelFormat(int PixelFormatID)
    {
        int Result = 0;
        if (PixelFormatID != FPixelFormatID)
        {
            switch (PixelFormatID)
            {
                case PixelFormats.pfRGB565:
                    PixelRemap.RemapSurface555to565(View, View, FSpriteWidth, FSpriteHeight);
                    FColorKey = PixelRemap.RemapPixelInfo(FColorKey, PixelFormats.pfRGB555, null, PixelFormats.pfRGB565, null);
                    FPixelFormatID = PixelFormats.pfRGB565;
                    break;
                case PixelFormats.pfRGB555:
                    PixelRemap.RemapSurface565to555(View, View, FSpriteWidth, FSpriteHeight);
                    FColorKey = PixelRemap.RemapPixelInfo(FColorKey, PixelFormats.pfRGB565, null, PixelFormats.pfRGB555, null);
                    FPixelFormatID = PixelFormats.pfRGB555;
                    break;
                default:
                    Result = PixelFormats.GFXE_NOTSUPPORTED;
                    break;
            }
        }

        return Result;
    }

    // ImageLib.pas:4386
    public override bool SupportShowMode(ShowMode ShowMode, int PixelFormatID) => (PixelFormatID == 0 || PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555) && Set<ShowMode>.Of(ShowMode.Opaque, ShowMode.Transparent, ShowMode.Blended).Contains(ShowMode);

    // ImageLib.pas:4392
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
        X += FSpriteX;
        DX = FXshift + X;
        W = FSpriteWidth;
        Right = FSpriteWidth + DX;
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
            Y += FSpriteY;
            DY = Y + FYshift;
            H = FSpriteHeight;
            Bottom = DY + FSpriteHeight;
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
                        BlitRects.BltColorKey16(Src, Dst, W, H, FColorKey);
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BltColorKey16(Src, Dst, W, H, FColorKey);
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
                        BlitRects.BltColorKey16(Src, Dst, W, H, FColorKey);
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BltColorKey16(Src, Dst, W, H, FColorKey);
                    }
                }
            }
        }
    }

    // ImageLib.pas:4457
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
        X += FSpriteX;
        DX = FXshift + X;
        W = FSpriteWidth;
        Right = FSpriteWidth + DX;
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
            Y += FSpriteY;
            DY = Y + FYshift;
            H = FSpriteHeight;
            Bottom = DY + FSpriteHeight;
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
                        switch (FPixelFormatID)
                        {
                            case PixelFormats.pfRGB555:
                                BlendColorKey555(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendColorKey565(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        switch (FPixelFormatID)
                        {
                            case PixelFormats.pfRGB555:
                                BlendColorKey555(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendColorKey565(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
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
                        switch (FPixelFormatID)
                        {
                            case PixelFormats.pfRGB555:
                                BlendColorKey555(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendColorKey565(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        switch (FPixelFormatID)
                        {
                            case PixelFormats.pfRGB555:
                                BlendColorKey555(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendColorKey565(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                        }
                    }
                }
            }
        }
    }

    // ImageLib.pas:4543
    // { ShowTransparent shows the sprite blended 50% }
    public override void ShowTransparent(int X, int Y, in SurfaceDesc SD)
    {
        ShowBlended(X, Y, SD, GfxState.AlphaBlendTable.LookupTables[50]);
    }

    // ImageLib.pas:4548
    public override bool SupportGrabPixelFormat(int PixelFormatID) => PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555 || PixelFormatID == PixelFormats.pfRGB24 || PixelFormatID == PixelFormats.pfPAL8;

    // ImageLib.pas:4554
    public override Form CreateGrabForm(Component AOwner, in SurfaceDesc SD, ImagesGrabbedEvent OnImagesGrabbed) => null;

    // ImageLib.pas:4561
    // { GrabImage locks the conversion surface and never unlocks it (an original bug) }
    public override bool GrabImage(in SurfaceDesc SD, in Rect R, GrabInfo GrabInfo)
    {
        DataSurface Surface;
        SurfaceDesc LSD = default;
        Rect GR = default;
        Rect MR = default;
        int Left;
        int Top;
        int Bottom;
        // { Absolute views preserve frame slots and the separate GR coordinate reads. }
        bool Result = true;
        try
        {
            Surface = null;
            try
            {
                if (SD.BitCount != 16)
                {
                    Surface = new DataSurface(R.Right - R.Left, R.Bottom - R.Top, PixelFormats.pfRGB565, null);
                    GR.Left = 0;
                    GR.Top = 0;
                    var width = Surface.GetWidth();
                    GR.Right = width;
                    var height = Surface.GetHeight();
                    GR.Bottom = height;
                    Surface.Lock(ref LSD);
                    var surfaceWidth = Surface.GetWidth();
                    var surfaceHeight = Surface.GetHeight();
                    if (SurfaceDesc.RemapSurfacePart(SD, R.Left, R.Top, surfaceWidth, surfaceHeight, LSD, 0, 0) != 0)
                    {
                        throw new ArgumentException("Invalid PixelFormat");
                    }

                    FTransparentColor = PixelRemap.RemapPixelInfo(GrabInfo.TransparentPixel, SD.PixelFormatID, SD.Palette, LSD.PixelFormatID, null);
                }
                else
                {
                    LSD = SD;
                    GR = R;
                    FTransparentColor = GrabInfo.TransparentPixel;
                }

                Clear();
                FDataLoaded = true;
                FDataWidth = GR.Right - GR.Left;
                FDataHeight = GR.Bottom - GR.Top;
                FWidth = FDataWidth;
                FHeight = FDataHeight;
                FFileName = ExtractFileName(GrabInfo.FileName);
                FFileIndex = GrabInfo.FileIndex;
                FStoredPixelFormatID = LSD.PixelFormatID;
                FPixelFormatID = FStoredPixelFormatID;
                FColorKey = FTransparentColor;
                SurfaceDesc.MinimizeTransparentRect(LSD, GR, FTransparentColor, ref MR);
                Left = MR.Left;
                FSpriteX = Left - GR.Left;
                Top = MR.Top;
                FSpriteY = Top - GR.Top;
                FSpriteWidth = MR.Right - Left;
                Bottom = MR.Bottom;
                FSpriteHeight = Bottom - Top;
                FDataSize = FSpriteWidth * FSpriteHeight * 2;
                AllocateDiscardableImageData(FSpriteWidth, FSpriteHeight);
                for (int Y = Top; Y <= Bottom - 1; Y++)
                {
                    LSD.View().Bytes(LSD.Pitch * Y + MR.Left * 2, FSpriteWidth * 2).CopyTo(FBuffer.AsSpan((Y - MR.Top) * FPitch, FSpriteWidth * 2));
                }
            }
            finally
            {
                if (Surface != null)
                {
                    Surface.Dispose();
                }
            }
        }
        catch
        {
            ReleaseDiscardableImageData();
            Result = false;
        }

        return Result;
    }

    private static void BlendColorKey555(SurfaceView Src, SurfaceView Dst, int Width, int Height, int ColorKey, ReadOnlySpan<byte> Table)
    {
        Blit.BlendColorKey555(Src, Dst, Width, Height, ColorKey, Table);
    }

    private static void BlendColorKey565(SurfaceView Src, SurfaceView Dst, int Width, int Height, int ColorKey, ReadOnlySpan<byte> Table)
    {
        Blit.BlendColorKey565(Src, Dst, Width, Height, ColorKey, Table);
    }

}
