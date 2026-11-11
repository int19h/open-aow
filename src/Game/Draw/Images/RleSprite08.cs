// Translated from ImageLib.pas.
using System.Buffers.Binary;
using System.Runtime.InteropServices;

using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// an 8 bit sprite, run length encoded (ClassID 2). FTransparentColor (the palette index of the
// transparent pixels), FReserved (only stored), FData (the encoded rows: per row a length
// dword, then the opaque pixels and, for a transparent run, the transparent index and the run
// length), FRLELeft, FRLETop, FRLEWidth, FRLEHeight (the rectangle of the encoded pixels in the
// data rectangle)
public partial class RleSprite08 : Image08, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 2;
    static string ILibraryImageClass.Name => "RLE Sprite (08bpp)";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new RleSprite08(owner);

    public RleSprite08(CustomImageLibrary owner) : base(owner)
    { }

    internal int FTransparentColor;
    internal int FReserved;
    internal byte[] FData;
    internal int FRLELeft;
    internal int FRLETop;
    internal int FRLEWidth;
    internal int FRLEHeight;

    // ImageLib.pas:7512
    public override void ReleaseDiscardableImageData()
    {
        if (FDataLoaded)
        {
            FData = null;
            FDataLoaded = false;
        }
    }

    // ImageLib.pas:7521
    public override void SetXshift(int Value)
    {
        if (Value < -FRLELeft)
        {
            Value = -FRLELeft;
        }

        base.SetXshift(Value);
        SetWidth(FWidth);
    }

    // ImageLib.pas:7528
    public override void SetYshift(int Value)
    {
        if (Value < -FRLETop)
        {
            Value = -FRLETop;
        }

        base.SetYshift(Value);
        SetHeight(FHeight);
    }

    // ImageLib.pas:7535
    public override void SetWidth(int Value)
    {
        int W;
        W = FXshift + FRLELeft + FRLEWidth;
        if (Value < W)
        {
            Value = W;
        }

        base.SetWidth(Value);
    }

    // ImageLib.pas:7544
    public override void SetHeight(int Value)
    {
        int H;
        H = FYshift + FRLETop + FRLEHeight;
        if (Value < H)
        {
            Value = H;
        }

        base.SetHeight(Value);
    }

    // ImageLib.pas:7553
    public override void GetVisualDimensions(ref Rect R)
    {
        R.Left = FXshift + FRLELeft;
        R.Right = R.Left + FRLEWidth;
        R.Top = FYshift + FRLETop;
        R.Bottom = R.Top + FRLEHeight;
    }

    // ImageLib.pas:7561
    public override void ReadImageData(Stream Stream)
    {
        base.ReadImageData(Stream);
        Stream.ReadExactly(Bytes.Of(ref FRLEWidth, 4));
        Stream.ReadExactly(Bytes.Of(ref FRLEHeight, 4));
        Stream.ReadExactly(Bytes.Of(ref FRLELeft, 4));
        Stream.ReadExactly(Bytes.Of(ref FRLETop, 4));
        Stream.ReadExactly(Bytes.Of(ref FTransparentColor, 4));
        Stream.ReadExactly(Bytes.Of(ref FReserved, 4));
    }

    // ImageLib.pas:7572
    public override void WriteImageData(Stream Stream)
    {
        base.WriteImageData(Stream);
        Stream.Write(Bytes.Of(ref FRLEWidth, 4));
        Stream.Write(Bytes.Of(ref FRLEHeight, 4));
        Stream.Write(Bytes.Of(ref FRLELeft, 4));
        Stream.Write(Bytes.Of(ref FRLETop, 4));
        Stream.Write(Bytes.Of(ref FTransparentColor, 4));
        Stream.Write(Bytes.Of(ref FReserved, 4));
    }

    // ImageLib.pas:7583
    public override void ReadDiscardableImageData(Stream Stream)
    {
        base.ReadDiscardableImageData(Stream);
        FData = MemorySurfaces.Reallocate(FData, FDataSize);
        Stream.ReadExactly(FData.AsSpan(0, FDataSize));
    }

    // ImageLib.pas:7590
    public override void WriteDiscardableImageData(Stream Stream)
    {
        base.WriteDiscardableImageData(Stream);
        Stream.Write(FData.AsSpan(0, FDataSize));
    }

    // ImageLib.pas:7596
    public override bool SupportShowMode(ShowMode ShowMode, int PixelFormatID) => (PixelFormatID == 0 || PixelFormatID == PixelFormats.pfRGB555 || PixelFormatID == PixelFormats.pfRGB565) && ShowMode == ShowMode.Opaque;

    // ImageLib.pas:7602
    public override void ShowOpaque(int X, int Y, in LockedSurface SD)
    {
        if (!FDataLoaded)
        {
            LoadDiscardableImageData();
            if (SD.PixelFormatID == PixelFormats.pfPAL8)
            {
                ShowRLE08(X + FXshift + FRLELeft, Y + FYshift + FRLETop, SD, FData, FRLEWidth, FRLEHeight, (byte)FTransparentColor);
            }
            else
            {
                if (SD.PixelFormatID != FRemapPalette.PixelFormatID && RemapToPixelFormat(SD.PixelFormatID) != PixelFormats.GFXE_OK)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                switch (SD.PixelFormatID)
                {
                    case PixelFormats.pfRGB555:
                    case PixelFormats.pfRGB565:
                        ShowRLE08to16(X + FXshift + FRLELeft, Y + FYshift + FRLETop, SD, FData, FRLEWidth, FRLEHeight, (byte)FTransparentColor, MemoryMarshal.AsBytes(FRemapPalette.Entries.AsSpan()));
                        break;
                    default:
                        throw new NotSupportedException("PixelFormat not supported");
                }
            }

            if (FLoadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            if (SD.PixelFormatID == PixelFormats.pfPAL8)
            {
                ShowRLE08(X + FXshift + FRLELeft, Y + FYshift + FRLETop, SD, FData, FRLEWidth, FRLEHeight, (byte)FTransparentColor);
            }
            else
            {
                if (SD.PixelFormatID != FRemapPalette.PixelFormatID && RemapToPixelFormat(SD.PixelFormatID) != PixelFormats.GFXE_OK)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                switch (SD.PixelFormatID)
                {
                    case PixelFormats.pfRGB555:
                    case PixelFormats.pfRGB565:
                        ShowRLE08to16(X + FXshift + FRLELeft, Y + FYshift + FRLETop, SD, FData, FRLEWidth, FRLEHeight, (byte)FTransparentColor, MemoryMarshal.AsBytes(FRemapPalette.Entries.AsSpan()));
                        break;
                    default:
                        throw new NotSupportedException("PixelFormat not supported");
                }
            }
        }
    }

    // ImageLib.pas:7643
    public override bool SupportGrabPixelFormat(int PixelFormatID) => PixelFormatID == PixelFormats.pfPAL8;

    // ImageLib.pas:7648
    public override Form CreateGrabForm(Component AOwner, in LockedSurface SD, ImagesGrabbedEvent OnImagesGrabbed) => null;

    // ImageLib.pas:7659
    // { GrabImage encodes the rows of the smallest rectangle holding the opaque pixels: per row a
    //   length dword (only its low word is written), the opaque pixels, and the runs of transparent
    //   pixels as the transparent index and the run length (up to 254). The buffer grows by 4 KB
    //   whenever the position reaches FDataSize, which stays 0 until the end (an original slip: every
    //   byte reallocates). Temp is never created. }
    public override bool GrabImage(in LockedSurface SD, in Rect R, GrabInfo GrabInfo)
    {
        bool Result;
        int Pos;
        int LineStart;
        int Count;
        object Temp;
        int Line;
        Rect MinRect = default;
        int B;
        int Left;
        int Top;
        int Bottom;
        void AddByte(byte B)
        {
            if (FDataSize <= Pos)
            {
                FData = MemorySurfaces.Reallocate(FData, Pos + 4096);
            }

            FData[Pos] = B;
            Pos += 1;
        }

        void StartLine()
        {
            LineStart = Pos;
            AddByte(0);
            AddByte(0);
            AddByte(0);
            AddByte(0);
        }

        void FlushTransparent()
        {
            while (Count != 0)
            {
                AddByte((byte)FTransparentColor);
                if (Count > 254)
                {
                    Count -= 254;
                    AddByte(254);
                }
                else
                {
                    AddByte((byte)Count);
                    Count = 0;
                }
            }
        }

        void EndLine()
        {
            FlushTransparent();
            if (FDataSize <= Pos)
            {
                FData = MemorySurfaces.Reallocate(FData, Pos + 4096);
            }

            BinaryPrimitives.WriteUInt16LittleEndian(FData.AsSpan(LineStart, 2), (ushort)(Pos - LineStart));
        }

        if (SD.BitCount == 8)
        {
            Result = true;
            try
            {
                Temp = null;
                try
                {
                    FTransparentColor = GrabInfo.TransparentPixel;
                    Clear();
                    FDataLoaded = true;
                    FDataWidth = R.Right - R.Left;
                    FDataHeight = R.Bottom - R.Top;
                    FWidth = FDataWidth;
                    FHeight = FDataHeight;
                    FFileName = ExtractFileName(GrabInfo.FileName);
                    FFileIndex = GrabInfo.FileIndex;
                    Pos = 0;
                    Count = 0;
                    LockedSurface.MinimizeTransparentRect(SD, R, FTransparentColor, ref MinRect);
                    Left = MinRect.Left;
                    FRLELeft = Left - R.Left;
                    Top = MinRect.Top;
                    FRLETop = Top - R.Top;
                    FRLEWidth = MinRect.Right - Left;
                    Bottom = MinRect.Bottom;
                    FRLEHeight = Bottom - Top;
                    for (int Y = Top; Y <= Bottom - 1; Y++)
                    {
                        // Line is the byte index of row Y in the surface's memory.
                        Line = SD.DataIndex + SD.Pitch * Y;
                        StartLine();
                        for (int X = MinRect.Left, last = MinRect.Right - 1; X <= last; X++)
                        {
                            B = SD.Data[Line + X];
                            if (B != FTransparentColor)
                            {
                                FlushTransparent();
                                AddByte((byte)B);
                            }
                            else
                            {
                                Count += 1;
                            }
                        }

                        EndLine();
                    }

                    FData = MemorySurfaces.Reallocate(FData, Pos);
                    FDataSize = Pos;
                    CreateLibraryImagePalette(SD.Palette);
                }
                finally
                {
                    if (Temp != null)
                    {
                        ((Temp ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
                    }
                }
            }
            catch
            {
                ReleaseDiscardableImageData();
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static void ShowRLE08(int X, int Y, in LockedSurface SD, ReadOnlySpan<byte> Data, int Width, int Height, byte ColorKey)
    {
        Blit.ShowRLE08(X, Y, SD.View(), Data, Width, Height, ColorKey);
    }

    private static void ShowRLE08to16(int X, int Y, in LockedSurface SD, ReadOnlySpan<byte> Data, int Width, int Height, byte ColorKey, ReadOnlySpan<byte> Palette)
    {
        Blit.ShowRLE08to16(X, Y, SD.View(), Data, Width, Height, ColorKey, Palette);
    }

}
