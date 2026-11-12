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

    internal int _rleLeft;
    internal int _rleTop;
    internal int _rleWidth;
    internal int _rleHeight;

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
        if (value < -_rleLeft)
        {
            value = -_rleLeft;
        }

        base.SetXshift(value);
        SetWidth(_width);
    }

    // ImageLib.pas:7528
    public override void SetYshift(int value)
    {
        if (value < -_rleTop)
        {
            value = -_rleTop;
        }

        base.SetYshift(value);
        SetHeight(_height);
    }

    // ImageLib.pas:7535
    public override void SetWidth(int value)
    {
        int W;
        W = _xshift + _rleLeft + _rleWidth;
        if (value < W)
        {
            value = W;
        }

        base.SetWidth(value);
    }

    // ImageLib.pas:7544
    public override void SetHeight(int value)
    {
        int H;
        H = _yshift + _rleTop + _rleHeight;
        if (value < H)
        {
            value = H;
        }

        base.SetHeight(value);
    }

    // ImageLib.pas:7553
    public override void GetVisualDimensions(out Rect R)
    {
        R.Left = _xshift + _rleLeft;
        R.Right = R.Left + _rleWidth;
        R.Top = _yshift + _rleTop;
        R.Bottom = R.Top + _rleHeight;
    }

    // ImageLib.pas:7561
    public override void ReadImageData(Stream stream)
    {
        base.ReadImageData(stream);
        stream.ReadExactly(Bytes.Of(ref _rleWidth, 4));
        stream.ReadExactly(Bytes.Of(ref _rleHeight, 4));
        stream.ReadExactly(Bytes.Of(ref _rleLeft, 4));
        stream.ReadExactly(Bytes.Of(ref _rleTop, 4));
        stream.ReadExactly(Bytes.Of(ref _transparentColor, 4));
        stream.ReadExactly(Bytes.Of(ref _unknownBytes, 4));
    }

    // ImageLib.pas:7572
    public override void WriteImageData(Stream stream)
    {
        base.WriteImageData(stream);
        stream.Write(Bytes.Of(ref _rleWidth, 4));
        stream.Write(Bytes.Of(ref _rleHeight, 4));
        stream.Write(Bytes.Of(ref _rleLeft, 4));
        stream.Write(Bytes.Of(ref _rleTop, 4));
        stream.Write(Bytes.Of(ref _transparentColor, 4));
        stream.Write(Bytes.Of(ref _unknownBytes, 4));
    }

    // ImageLib.pas:7583
    public override void ReadDiscardableImageData(Stream stream)
    {
        base.ReadDiscardableImageData(stream);
        Data = MemorySurfaces.Reallocate(Data, _dataSize);
        stream.ReadExactly(Data.AsSpan(0, _dataSize));
    }

    // ImageLib.pas:7590
    public override void WriteDiscardableImageData(Stream stream)
    {
        base.WriteDiscardableImageData(stream);
        stream.Write(Data.AsSpan(0, _dataSize));
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
                ShowRle08(X + _xshift + _rleLeft, Y + _yshift + _rleTop, lockedSurface, Data, _rleWidth, _rleHeight, (byte)_transparentColor);
            }
            else
            {
                if (lockedSurface.PixelFormatId != _remapPalette.PixelFormatId && RemapToPixelFormat(lockedSurface.PixelFormatId) != PixelFormats.GFXE_OK)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                switch (lockedSurface.PixelFormatId)
                {
                    case PixelFormats.pfRGB555:
                    case PixelFormats.pfRGB565:
                        ShowRle08To16(X + _xshift + _rleLeft, Y + _yshift + _rleTop, lockedSurface, Data, _rleWidth, _rleHeight, (byte)_transparentColor, MemoryMarshal.AsBytes(_remapPalette.Entries.AsSpan()));
                        break;
                    default:
                        throw new NotSupportedException("PixelFormat not supported");
                }
            }

            if (_loadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            if (lockedSurface.PixelFormatId == PixelFormats.pfPAL8)
            {
                ShowRle08(X + _xshift + _rleLeft, Y + _yshift + _rleTop, lockedSurface, Data, _rleWidth, _rleHeight, (byte)_transparentColor);
            }
            else
            {
                if (lockedSurface.PixelFormatId != _remapPalette.PixelFormatId && RemapToPixelFormat(lockedSurface.PixelFormatId) != PixelFormats.GFXE_OK)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                switch (lockedSurface.PixelFormatId)
                {
                    case PixelFormats.pfRGB555:
                    case PixelFormats.pfRGB565:
                        ShowRle08To16(X + _xshift + _rleLeft, Y + _yshift + _rleTop, lockedSurface, Data, _rleWidth, _rleHeight, (byte)_transparentColor, MemoryMarshal.AsBytes(_remapPalette.Entries.AsSpan()));
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
        int B;
        int left;
        int top;
        int bottom;
        void AddByte(byte B)
        {
            if (_dataSize <= position)
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
            if (_dataSize <= position)
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
                    _dataWidth = R.Right - R.Left;
                    _dataHeight = R.Bottom - R.Top;
                    _width = _dataWidth;
                    _height = _dataHeight;
                    _fileName = ExtractFileName(grabInfo.FileName);
                    _fileIndex = grabInfo.FileIndex;
                    position = 0;
                    count = 0;
                    LockedSurface.MinimizeTransparentRectangle(lockedSurface, R, _transparentColor, out minRectangle);
                    left = minRectangle.Left;
                    _rleLeft = left - R.Left;
                    top = minRectangle.Top;
                    _rleTop = top - R.Top;
                    _rleWidth = minRectangle.Right - left;
                    bottom = minRectangle.Bottom;
                    _rleHeight = bottom - top;
                    for (int Y = top; Y <= bottom - 1; Y++)
                    {
                        // Line is the byte index of row Y in the surface's memory.
                        line = lockedSurface.DataIndex + lockedSurface.Pitch * Y;
                        StartLine();
                        for (int X = minRectangle.Left, last = minRectangle.Right - 1; X <= last; X++)
                        {
                            B = lockedSurface.Data[line + X];
                            if (B != _transparentColor)
                            {
                                FlushTransparent();
                                AddByte((byte)B);
                            }
                            else
                            {
                                count += 1;
                            }
                        }

                        EndLine();
                    }

                    Data = MemorySurfaces.Reallocate(Data, position);
                    _dataSize = position;
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

    private static void ShowRle08To16(int X, int Y, in LockedSurface lockedSurface, ReadOnlySpan<byte> data, int width, int height, byte colorKey, ReadOnlySpan<byte> palette)
    {
        Blit.ShowRle08To16(X, Y, lockedSurface.View(), data, width, height, colorKey, palette);
    }

}
