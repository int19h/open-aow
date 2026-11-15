// Translated from ImageLib.pas.
using System.Buffers.Binary;

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
    static int ILibraryImageClass.ClassId => 2;
    static string ILibraryImageClass.Name => "RLE Sprite (08bpp)";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new RleSprite08(owner);

    public RleSprite08(CustomImageLibrary owner) : base(owner)
    { }

    private int _transparentColor;
    // Four bytes of unknown meaning after the transparent colour in the sprite's image data. The
    // original reads and writes them and uses them nowhere else. They round trip unchanged.
    private int _unknownBytes;

    internal byte[] Data { get; private set; }

    internal int RleLeft;
    internal int RleTop;
    internal int RleWidth;
    internal int RleHeight;

    // ImageLib.pas:7512
    public override void ReleaseDiscardableImageData()
    {
        if (DataLoaded)
        {
            Data = null;
            DataLoaded = false;
        }
    }

    // ImageLib.pas:7521
    public override void SetXshift(int value)
    {
        if (value < -RleLeft)
        {
            value = -RleLeft;
        }

        base.SetXshift(value);
        SetWidth(Width);
    }

    // ImageLib.pas:7528
    public override void SetYshift(int value)
    {
        if (value < -RleTop)
        {
            value = -RleTop;
        }

        base.SetYshift(value);
        SetHeight(Height);
    }

    // ImageLib.pas:7535
    public override void SetWidth(int value)
    {
        int w;
        w = Xshift + RleLeft + RleWidth;
        if (value < w)
        {
            value = w;
        }

        base.SetWidth(value);
    }

    // ImageLib.pas:7544
    public override void SetHeight(int value)
    {
        int h;
        h = Yshift + RleTop + RleHeight;
        if (value < h)
        {
            value = h;
        }

        base.SetHeight(value);
    }

    // ImageLib.pas:7553
    public override void GetVisualDimensions(out Rect R)
    {
        R.Left = Xshift + RleLeft;
        R.Right = R.Left + RleWidth;
        R.Top = Yshift + RleTop;
        R.Bottom = R.Top + RleHeight;
    }

    // ImageLib.pas:7561
    public override void ReadImageData(Stream stream)
    {
        base.ReadImageData(stream);
        stream.ReadExactly(Bytes.Of(ref RleWidth, 4));
        stream.ReadExactly(Bytes.Of(ref RleHeight, 4));
        stream.ReadExactly(Bytes.Of(ref RleLeft, 4));
        stream.ReadExactly(Bytes.Of(ref RleTop, 4));
        stream.ReadExactly(Bytes.Of(ref _transparentColor, 4));
        stream.ReadExactly(Bytes.Of(ref _unknownBytes, 4));
    }

    // ImageLib.pas:7572
    public override void WriteImageData(Stream stream)
    {
        base.WriteImageData(stream);
        stream.Write(Bytes.Of(ref RleWidth, 4));
        stream.Write(Bytes.Of(ref RleHeight, 4));
        stream.Write(Bytes.Of(ref RleLeft, 4));
        stream.Write(Bytes.Of(ref RleTop, 4));
        stream.Write(Bytes.Of(ref _transparentColor, 4));
        stream.Write(Bytes.Of(ref _unknownBytes, 4));
    }

    // ImageLib.pas:7583
    public override void ReadDiscardableImageData(Stream stream)
    {
        base.ReadDiscardableImageData(stream);
        Data = MemorySurfaces.Reallocate(Data, DataSize);
        stream.ReadExactly(Data.AsSpan(0, DataSize));
    }

    // ImageLib.pas:7590
    public override void WriteDiscardableImageData(Stream stream)
    {
        base.WriteDiscardableImageData(stream);
        stream.Write(Data.AsSpan(0, DataSize));
    }

    // ImageLib.pas:7596
    public override bool SupportShowMode(ShowMode showMode, int pixelFormatId) => (pixelFormatId == 0 || pixelFormatId == PixelFormats.pfRGB555 || pixelFormatId == PixelFormats.pfRGB565) && showMode == ShowMode.Opaque;

    // ImageLib.pas:7602
    public override void ShowOpaque(int X, int Y, in LockedSurface lockedSurface)
    {
        if (!DataLoaded)
        {
            LoadDiscardableImageData();
            if (lockedSurface.PixelFormatId == PixelFormats.pfPAL8)
            {
                ShowRle08(X + Xshift + RleLeft, Y + Yshift + RleTop, lockedSurface, Data, RleWidth, RleHeight, (byte)_transparentColor);
            }
            else
            {
                if (lockedSurface.PixelFormatId != RemapPaletteValue.PixelFormatId && RemapToPixelFormat(lockedSurface.PixelFormatId) != PixelFormats.GFXE_OK)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                switch (lockedSurface.PixelFormatId)
                {
                    case PixelFormats.pfRGB555:
                    case PixelFormats.pfRGB565:
                        ShowRle08To16(X + Xshift + RleLeft, Y + Yshift + RleTop, lockedSurface, Data, RleWidth, RleHeight, (byte)_transparentColor, RemapPaletteValue.Pixels);
                        break;
                    default:
                        throw new NotSupportedException("PixelFormat not supported");
                }
            }

            if (LoadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            if (lockedSurface.PixelFormatId == PixelFormats.pfPAL8)
            {
                ShowRle08(X + Xshift + RleLeft, Y + Yshift + RleTop, lockedSurface, Data, RleWidth, RleHeight, (byte)_transparentColor);
            }
            else
            {
                if (lockedSurface.PixelFormatId != RemapPaletteValue.PixelFormatId && RemapToPixelFormat(lockedSurface.PixelFormatId) != PixelFormats.GFXE_OK)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                switch (lockedSurface.PixelFormatId)
                {
                    case PixelFormats.pfRGB555:
                    case PixelFormats.pfRGB565:
                        ShowRle08To16(X + Xshift + RleLeft, Y + Yshift + RleTop, lockedSurface, Data, RleWidth, RleHeight, (byte)_transparentColor, RemapPaletteValue.Pixels);
                        break;
                    default:
                        throw new NotSupportedException("PixelFormat not supported");
                }
            }
        }
    }

    // ImageLib.pas:7643
    public override bool SupportGrabPixelFormat(int pixelFormatId) => pixelFormatId == PixelFormats.pfPAL8;

    // ImageLib.pas:7659
    // { GrabImage encodes the rows of the smallest rectangle holding the opaque pixels: per row a
    //   length dword (only its low word is written), the opaque pixels, and the runs of transparent
    //   pixels as the transparent index and the run length (up to 254). The buffer grows by 4 KB
    //   whenever the position reaches FDataSize, which stays 0 until the end (an original slip: every
    //   byte reallocates). Temp is never created. }
    public override bool GrabImage(in LockedSurface lockedSurface, in Rect R, GrabInfo grabInfo)
    {
        bool result;
        int position;
        int lineStart;
        int count;
        object temporary;
        int line;
        Rect minRectangle = default;
        int b;
        int left;
        int top;
        int bottom;
        void AddByte(byte B)
        {
            if (DataSize <= position)
            {
                Data = MemorySurfaces.Reallocate(Data, position + 4096);
            }

            Data[position] = B;
            position += 1;
        }

        void StartLine()
        {
            lineStart = position;
            AddByte(0);
            AddByte(0);
            AddByte(0);
            AddByte(0);
        }

        void FlushTransparent()
        {
            while (count != 0)
            {
                AddByte((byte)_transparentColor);
                if (count > 254)
                {
                    count -= 254;
                    AddByte(254);
                }
                else
                {
                    AddByte((byte)count);
                    count = 0;
                }
            }
        }

        void EndLine()
        {
            FlushTransparent();
            if (DataSize <= position)
            {
                Data = MemorySurfaces.Reallocate(Data, position + 4096);
            }

            BinaryPrimitives.WriteUInt16LittleEndian(Data.AsSpan(lineStart, 2), (ushort)(position - lineStart));
        }

        if (lockedSurface.BitCount == 8)
        {
            result = true;
            try
            {
                temporary = null;
                try
                {
                    _transparentColor = grabInfo.TransparentPixel;
                    Clear();
                    DataLoaded = true;
                    DataWidth = R.Right - R.Left;
                    DataHeight = R.Bottom - R.Top;
                    SetWidthWithoutOverrides(DataWidth);
                    SetHeightWithoutOverrides(DataHeight);
                    FileName = ExtractFileName(grabInfo.FileName);
                    FileIndex = grabInfo.FileIndex;
                    position = 0;
                    count = 0;
                    LockedSurface.MinimizeTransparentRectangle(lockedSurface, R, _transparentColor, out minRectangle);
                    left = minRectangle.Left;
                    RleLeft = left - R.Left;
                    top = minRectangle.Top;
                    RleTop = top - R.Top;
                    RleWidth = minRectangle.Right - left;
                    bottom = minRectangle.Bottom;
                    RleHeight = bottom - top;
                    for (int y = top; y <= bottom - 1; y++)
                    {
                        // Line is the byte index of row Y in the surface's memory.
                        line = lockedSurface.DataIndex + lockedSurface.Pitch * y;
                        StartLine();
                        for (int x = minRectangle.Left, last = minRectangle.Right - 1; x <= last; x++)
                        {
                            b = lockedSurface.Data[line + x];
                            if (b != _transparentColor)
                            {
                                FlushTransparent();
                                AddByte((byte)b);
                            }
                            else
                            {
                                count += 1;
                            }
                        }

                        EndLine();
                    }

                    Data = MemorySurfaces.Reallocate(Data, position);
                    DataSize = position;
                    CreateLibraryImagePalette(lockedSurface.Palette);
                }
                finally
                {
                    if (temporary != null)
                    {
                        ((temporary ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
                    }
                }
            }
            catch
            {
                ReleaseDiscardableImageData();
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static void ShowRle08(int X, int Y, in LockedSurface lockedSurface, ReadOnlySpan<byte> data, int width, int height, byte colorKey)
    {
        Blit.ShowRle08(X, Y, lockedSurface.View(), data, width, height, colorKey);
    }

    private static void ShowRle08To16(int X, int Y, in LockedSurface lockedSurface, ReadOnlySpan<byte> data, int width, int height, byte colorKey, ReadOnlySpan<uint> palette)
    {
        Blit.ShowRle08To16(X, Y, lockedSurface.View(), data, width, height, colorKey, palette);
    }

}
