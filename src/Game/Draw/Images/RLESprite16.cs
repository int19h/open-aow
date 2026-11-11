// Translated from ImageLib.pas.
using System.Buffers.Binary;

using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// (ClassID $11): a run length encoded 16 bit sprite. FColorKey (the transparent color, in
// FPixelFormatID), FData (FDataSize bytes), FSpriteX, FSpriteY, FSpriteWidth, FSpriteHeight (as
// TSprite16). The data is FSpriteHeight rows. a row is a word with its size in bytes (the
// header included), a zero word, then pixels and skips (the color key and the size of the skip
// in bytes), padded with the color key to a dword
public partial class RLESprite16 : Image16, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 17;
    static string ILibraryImageClass.Name => "RLE Sprite (16bpp)";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new RLESprite16(owner);

    internal int FColorKey;
    internal byte[] FData;
    internal int FSpriteX;
    internal int FSpriteY;
    internal int FSpriteWidth;
    internal int FSpriteHeight;

    // ImageLib.pas:2881
    public RLESprite16(CustomImageLibrary owner) : base(owner)
    {
    }

    // ImageLib.pas:2896
    public override void SetXshift(int Value)
    {
        if (Value < -FSpriteX)
        {
            Value = -FSpriteX;
        }

        base.SetXshift(Value);
        SetWidth(FWidth);
    }

    // ImageLib.pas:2903
    public override void SetYshift(int Value)
    {
        if (Value < -FSpriteY)
        {
            Value = -FSpriteY;
        }

        base.SetYshift(Value);
        SetHeight(FHeight);
    }

    // ImageLib.pas:2910
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

    // ImageLib.pas:2919
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

    // ImageLib.pas:2928
    public override void GetVisualDimensions(ref Rect R)
    {
        R.Left = FXshift + FSpriteX;
        R.Right = R.Left + FSpriteWidth;
        R.Top = FYshift + FSpriteY;
        R.Bottom = R.Top + FSpriteHeight;
    }

    // ImageLib.pas:2936
    public override void ReleaseDiscardableImageData()
    {
        if (FDataLoaded)
        {
            FData = null;
            FDataLoaded = false;
        }
    }

    // ImageLib.pas:2947
    // { The last dword of the image data is not used: ReadImageData skips it and WriteImageData
    //   writes an uninitialized local variable (an original bug). }
    public override void ReadImageData(Stream Stream)
    {
        int Reserved = default;
        base.ReadImageData(Stream);
        Stream.ReadExactly(Bytes.Of(ref FSpriteWidth, 4));
        Stream.ReadExactly(Bytes.Of(ref FSpriteHeight, 4));
        Stream.ReadExactly(Bytes.Of(ref FSpriteX, 4));
        Stream.ReadExactly(Bytes.Of(ref FSpriteY, 4));
        Stream.ReadExactly(Bytes.Of(ref FColorKey, 4));
        Stream.ReadExactly(Bytes.Of(ref Reserved, 4));
    }

    // ImageLib.pas:2960
    public override void WriteImageData(Stream Stream)
    {
        int Reserved = default;
        base.WriteImageData(Stream);
        Stream.Write(Bytes.Of(ref FSpriteWidth, 4));
        Stream.Write(Bytes.Of(ref FSpriteHeight, 4));
        Stream.Write(Bytes.Of(ref FSpriteX, 4));
        Stream.Write(Bytes.Of(ref FSpriteY, 4));
        Stream.Write(Bytes.Of(ref FColorKey, 4));
        Stream.Write(Bytes.Of(ref Reserved, 4));
    }

    // ImageLib.pas:2973
    public override void ReadDiscardableImageData(Stream Stream)
    {
        base.ReadDiscardableImageData(Stream);
        FData = MemorySurfaces.Reallocate(FData, FDataSize);
        Stream.ReadExactly(FData.AsSpan(0, FDataSize));
    }

    // ImageLib.pas:2980
    public override void WriteDiscardableImageData(Stream Stream)
    {
        base.WriteDiscardableImageData(Stream);
        Stream.Write(FData.AsSpan(0, FDataSize));
    }

    // ImageLib.pas:2986
    public override int RemapToPixelFormat(int PixelFormatID)
    {
        int Result = 0;
        if (PixelFormatID != FPixelFormatID)
        {
            switch (PixelFormatID)
            {
                case PixelFormats.pfRGB565:
                    RemapRLE555to565(FData, FSpriteHeight, FColorKey);
                    FPixelFormatID = PixelFormats.pfRGB565;
                    break;
                case PixelFormats.pfRGB555:
                    RemapRLE565to555(FData, FSpriteHeight, FColorKey);
                    FPixelFormatID = PixelFormats.pfRGB555;
                    break;
                default:
                    Result = PixelFormats.GFXE_NOTSUPPORTED;
                    break;
            }
        }

        return Result;
    }

    // ImageLib.pas:3006
    public override bool SupportShowMode(ShowMode ShowMode, int PixelFormatID) => (PixelFormatID == 0 || PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555) && Set<ShowMode>.Of(ShowMode.Opaque, ShowMode.Transparent).Contains(ShowMode);

    // ImageLib.pas:3012
    public override void ShowOpaque(int X, int Y, in SurfaceDesc SD)
    {
        if (!FDataLoaded)
        {
            LoadDiscardableImageData();
            if (SD.PixelFormatID == FPixelFormatID)
            {
                BltRLE16(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
            }
            else
            {
                if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                BltRLE16(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
            }

            if (FLoadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            if (SD.PixelFormatID == FPixelFormatID)
            {
                BltRLE16(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
            }
            else
            {
                if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                BltRLE16(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
            }
        }
    }

    // ImageLib.pas:3045
    // { ShowTransparent shows the sprite blended 50% }
    public override void ShowTransparent(int X, int Y, in SurfaceDesc SD)
    {
        if (!FDataLoaded)
        {
            LoadDiscardableImageData();
            if (SD.PixelFormatID != FPixelFormatID && RemapToPixelFormat(SD.PixelFormatID) != 0)
            {
                throw new NotSupportedException("Surface pixel format not supported!");
            }

            switch (FPixelFormatID)
            {
                case PixelFormats.pfRGB565:
                    BlendRLE565(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                    break;
                case PixelFormats.pfRGB555:
                    BlendRLE555(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                    break;
            }

            if (FLoadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            if (SD.PixelFormatID != FPixelFormatID && RemapToPixelFormat(SD.PixelFormatID) != 0)
            {
                throw new NotSupportedException("Surface pixel format not supported!");
            }

            switch (FPixelFormatID)
            {
                case PixelFormats.pfRGB565:
                    BlendRLE565(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                    break;
                case PixelFormats.pfRGB555:
                    BlendRLE555(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                    break;
            }
        }
    }

    // ImageLib.pas:3073
    public override bool SupportGrabPixelFormat(int PixelFormatID) => PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555 || PixelFormatID == PixelFormats.pfRGB24 || PixelFormatID == PixelFormats.pfPAL8;

    // ImageLib.pas:3079
    public override Form CreateGrabForm(Component AOwner, in SurfaceDesc SD, ImagesGrabbedEvent OnImagesGrabbed) => null;

    // ImageLib.pas:3090
    // The sprites take their transparent color from TGrabInfo (declared Reserved in the core),
    // not from TransparentColor (+4, the color GetSurfaceRectangles separates the images with).
    // GrabImage encodes the rows into FData, which grows in 4 KB steps. FDataSize stays 0 until
    // the end, so every word added past the first step reallocates (an original inefficiency)
    public override bool GrabImage(in SurfaceDesc SD, in Rect R, GrabInfo GrabInfo)
    {
        int Pos;
        int RowStart;
        int Skip;
        DataSurface Surface;
        SurfaceDesc LSD = default;
        Rect GR = default;
        Rect MR = default;
        int C;
        void AddWord(ushort W)
        {
            if (FDataSize <= Pos)
            {
                FData = MemorySurfaces.Reallocate(FData, Pos + 4096);
            }

            BinaryPrimitives.WriteUInt16LittleEndian(FData.AsSpan(Pos, 2), W);
            Pos += 2;
        }

        void StartRow()
        {
            RowStart = Pos;
            AddWord(0);
            AddWord(0);
        }

        void FlushSkip()
        {
            if (Skip != 0)
            {
                AddWord((ushort)FColorKey);
                AddWord((ushort)(Skip * 2));
                Skip = 0;
            }
        }

        void EndRow()
        {
            FlushSkip();
            if (FDataSize <= Pos)
            {
                FData = MemorySurfaces.Reallocate(FData, Pos + 4096);
            }

            BinaryPrimitives.WriteUInt16LittleEndian(FData.AsSpan(RowStart, 2), (ushort)(Pos - RowStart));

            if ((Pos & 3) != 0)
            {
                AddWord((ushort)FColorKey);
            }
        }

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

                    FColorKey = PixelRemap.RemapPixelInfo(GrabInfo.TransparentPixel, SD.PixelFormatID, SD.Palette, LSD.PixelFormatID, null);
                }
                else
                {
                    LSD = SD;
                    GR = R;
                    FColorKey = GrabInfo.TransparentPixel;
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
                Pos = 0;
                Skip = 0;
                SurfaceDesc.MinimizeTransparentRect(LSD, GR, FColorKey, ref MR);
                FSpriteX = MR.Left - GR.Left;
                FSpriteY = MR.Top - GR.Top;
                FSpriteWidth = MR.Right - MR.Left;
                FSpriteHeight = MR.Bottom - MR.Top;
                for (int Y = MR.Top, last = MR.Bottom - 1; Y <= last; Y++)
                {
                    int Row = LSD.Pitch * Y;
                    StartRow();
                    for (int X = MR.Left, lastX = MR.Right - 1; X <= lastX; X++)
                    {
                        C = LSD.View().Word(Row + X + X);
                        if (C != FColorKey)
                        {
                            FlushSkip();
                            AddWord((ushort)C);
                        }
                        else
                        {
                            Skip += 1;
                        }
                    }

                    EndRow();
                }

                FData = MemorySurfaces.Reallocate(FData, Pos);
                FDataSize = Pos;
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

    private static void RemapRLE565to555(Span<byte> Data, int Height, int ColorKey)
    {
        Blit.RemapRLE565to555(Data, Height, ColorKey);
    }

    private static void RemapRLE555to565(Span<byte> Data, int Height, int ColorKey)
    {
        Blit.RemapRLE555to565(Data, Height, ColorKey);
    }

    private static void BltRLE16(int X, int Y, in SurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height, int ColorKey)
    {
        Blit.BltRLE16(X, Y, SD.View(), Data, Width, Height, ColorKey);
    }

    private static void BlendRLE565(int X, int Y, in SurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height, int ColorKey)
    {
        Blit.BlendRLE565(X, Y, SD.View(), Data, Width, Height, ColorKey);
    }

    private static void BlendRLE555(int X, int Y, in SurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height, int ColorKey)
    {
        Blit.BlendRLE555(X, Y, SD.View(), Data, Width, Height, ColorKey);
    }

}
