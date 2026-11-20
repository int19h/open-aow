#nullable disable
// Translated from ImageLib.pas.
using System.Buffers.Binary;

using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.DelphiPaths;

namespace Aow.Game.Draw.Images;

// an 8 bit sprite, run length encoded (ClassID 2). FTransparentColor (the palette index of the
// transparent pixels), FData (the encoded rows: per row a length dword, then the opaque pixels
// and, for a transparent run, the transparent index and the run length), FRLELeft, FRLETop,
// FRLEWidth, FRLEHeight (the rectangle of the encoded pixels in the data rectangle)
public class RleSprite08(CustomImageLibrary owner) : Image08(owner), ILibraryImageClass
{
    static int ILibraryImageClass.ClassId => 2;
    static string ILibraryImageClass.Name => "RLE Sprite (08bpp)";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new RleSprite08(owner);

    private int _transparentColor;

    internal byte[] Data { get; private set; }

    internal int RleLeft { get; private set; }
    internal int RleTop { get; private set; }
    internal int RleWidth { get; private set; }
    internal int RleHeight { get; private set; }

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
        int w = Xshift + RleLeft + RleWidth;
        if (value < w)
        {
            value = w;
        }

        base.SetWidth(value);
    }

    // ImageLib.pas:7544
    public override void SetHeight(int value)
    {
        int h = Yshift + RleTop + RleHeight;
        if (value < h)
        {
            value = h;
        }

        base.SetHeight(value);
    }

    // ImageLib.pas:7553
    public override void GetVisualDimensions(out Rect R)
    {
        int left = Xshift + RleLeft;
        int top = Yshift + RleTop;
        R = new() { Left = left, Right = left + RleWidth, Top = top, Bottom = top + RleHeight };
    }

    // ImageLib.pas:7561
    // Delphi TRLESprite08.FReserved is always 0 on write and never read outside WriteImageData.
    public override void ReadImageData(Stream stream)
    {
        int reserved = default;
        base.ReadImageData(stream);
        int rleWidth = RleWidth;
        stream.ReadExactly(Bytes.Of(ref rleWidth));
        RleWidth = rleWidth;
        int rleHeight = RleHeight;
        stream.ReadExactly(Bytes.Of(ref rleHeight));
        RleHeight = rleHeight;
        int rleLeft = RleLeft;
        stream.ReadExactly(Bytes.Of(ref rleLeft));
        RleLeft = rleLeft;
        int rleTop = RleTop;
        stream.ReadExactly(Bytes.Of(ref rleTop));
        RleTop = rleTop;
        stream.ReadExactly(Bytes.Of(ref _transparentColor));
        stream.ReadExactly(Bytes.Of(ref reserved));
    }

    // ImageLib.pas:7572
    public override void WriteImageData(Stream stream)
    {
        int reserved = default;
        base.WriteImageData(stream);
        int rleWidth = RleWidth;
        stream.Write(Bytes.Of(ref rleWidth));
        RleWidth = rleWidth;
        int rleHeight = RleHeight;
        stream.Write(Bytes.Of(ref rleHeight));
        RleHeight = rleHeight;
        int rleLeft = RleLeft;
        stream.Write(Bytes.Of(ref rleLeft));
        RleLeft = rleLeft;
        int rleTop = RleTop;
        stream.Write(Bytes.Of(ref rleTop));
        RleTop = rleTop;
        stream.Write(Bytes.Of(ref _transparentColor));
        stream.Write(Bytes.Of(ref reserved));
    }

    // ImageLib.pas:7583
    public override void ReadDiscardableImageData(Stream stream)
    {
        base.ReadDiscardableImageData(stream);
        Data = MemorySurfaces.Reallocate(Data, size: DataSize);
        stream.ReadExactly(Data.AsSpan(0, length: DataSize));
    }

    // ImageLib.pas:7590
    public override void WriteDiscardableImageData(Stream stream)
    {
        base.WriteDiscardableImageData(stream);
        stream.Write(Data.AsSpan(0, length: DataSize));
    }

    // ImageLib.pas:7596
    public override bool SupportShowMode(ShowMode showMode, PixelFormat pixelFormatId) => (pixelFormatId is PixelFormat.None or PixelFormat.Rgb555 or PixelFormat.Rgb565) && showMode == ShowMode.Opaque;

    // ImageLib.pas:7602
    public override void ShowOpaque(int X, int Y, in LockedSurface lockedSurface)
    {
        if (!DataLoaded)
        {
            LoadDiscardableImageData();
            if (lockedSurface.PixelFormatId == PixelFormat.Pal8)
            {
                ShowRle08(X + Xshift + RleLeft, Y + Yshift + RleTop, lockedSurface, Data, RleWidth, RleHeight, colorKey: (byte)_transparentColor);
            }
            else
            {
                if (lockedSurface.PixelFormatId != RemapPaletteValue.PixelFormatId && RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                switch (lockedSurface.PixelFormatId)
                {
                    case PixelFormat.Rgb555:
                    case PixelFormat.Rgb565:
                        ShowRle08To16(X + Xshift + RleLeft, Y + Yshift + RleTop, lockedSurface, Data, RleWidth, RleHeight, colorKey: (byte)_transparentColor, palette: RemapPaletteValue.Pixels);
                        break;
                    case PixelFormat.None:
                    case PixelFormat.Pal8:
                    case PixelFormat.Argb1555:
                    case PixelFormat.Rgb24:
                    case PixelFormat.Bgr24:
                    case PixelFormat.Rgb32:
                    case PixelFormat.Xrgb32:
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
            if (lockedSurface.PixelFormatId == PixelFormat.Pal8)
            {
                ShowRle08(X + Xshift + RleLeft, Y + Yshift + RleTop, lockedSurface, Data, RleWidth, RleHeight, colorKey: (byte)_transparentColor);
            }
            else
            {
                if (lockedSurface.PixelFormatId != RemapPaletteValue.PixelFormatId && RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                switch (lockedSurface.PixelFormatId)
                {
                    case PixelFormat.Rgb555:
                    case PixelFormat.Rgb565:
                        ShowRle08To16(X + Xshift + RleLeft, Y + Yshift + RleTop, lockedSurface, Data, RleWidth, RleHeight, colorKey: (byte)_transparentColor, palette: RemapPaletteValue.Pixels);
                        break;
                    case PixelFormat.None:
                    case PixelFormat.Pal8:
                    case PixelFormat.Argb1555:
                    case PixelFormat.Rgb24:
                    case PixelFormat.Bgr24:
                    case PixelFormat.Rgb32:
                    case PixelFormat.Xrgb32:
                    default:
                        throw new NotSupportedException("PixelFormat not supported");
                }
            }
        }
    }

    // ImageLib.pas:7643
    public override bool SupportGrabPixelFormat(PixelFormat pixelFormatId) => pixelFormatId == PixelFormat.Pal8;

    // ImageLib.pas:7659
    // { GrabImage encodes the rows of the smallest rectangle holding the opaque pixels: per row a
    //   length dword (only its low word is written), the opaque pixels, and the runs of transparent
    //   pixels as the transparent index and the run length (up to 254). The buffer grows by 4 KB
    //   whenever the position reaches FDataSize, which stays 0 until the end (an original slip: every
    //   byte reallocates). Temp is never created. }
    public override bool GrabImage(in LockedSurface lockedSurface, in Rect R, GrabInfo grabInfo)
    {
        int position;
        void AddByte(byte B)
        {
            if (DataSize <= position)
            {
                Data = MemorySurfaces.Reallocate(Data, size: position + 4_096);
            }

            Data[position] = B;
            position += 1;
        }

        int lineStart;
        void StartLine()
        {
            lineStart = position;
            AddByte(0);
            AddByte(0);
            AddByte(0);
            AddByte(0);
        }

        int count;
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
                Data = MemorySurfaces.Reallocate(Data, size: position + 4_096);
            }

            BinaryPrimitives.WriteUInt16LittleEndian(Data.AsSpan(lineStart, length: 2), value: (ushort)(position - lineStart));
        }
        if (lockedSurface.BitCount == 8)
        {
            try
            {
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
                    LockedSurface.MinimizeTransparentRectangle(lockedSurface, R, _transparentColor, result: out Rect minRectangle);
                    int left = minRectangle.Left;
                    RleLeft = left - R.Left;
                    int top = minRectangle.Top;
                    RleTop = top - R.Top;
                    RleWidth = minRectangle.Right - left;
                    int bottom = minRectangle.Bottom;
                    RleHeight = bottom - top;
                    for (int y = top; y <= bottom - 1; y++)
                    {
                        // Line is the byte index of row Y in the surface's memory.
                        int line = lockedSurface.DataIndex + (lockedSurface.Pitch * y);
                        StartLine();
                        for (int x = minRectangle.Left, last = minRectangle.Right - 1; x <= last; x++)
                        {
                            int b = lockedSurface.Data[line + x];
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

                    Data = MemorySurfaces.Reallocate(Data, size: position);
                    DataSize = position;
                    CreateLibraryImagePalette(lockedSurface.Palette);
                }
                finally
                {
                    object temporary = null;
                    if (temporary is not null)
                    {
                        ((temporary ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
                    }
                }
            }
            catch
            {
                ReleaseDiscardableImageData();
                return false;
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    private static void ShowRle08(int X, int Y, in LockedSurface lockedSurface, ReadOnlySpan<byte> data, int width, int height, byte colorKey) => Blit.ShowRle08(X, Y, destination: lockedSurface.View(), data, w: width, h: height, key: colorKey);

    private static void ShowRle08To16(int X, int Y, in LockedSurface lockedSurface, ReadOnlySpan<byte> data, int width, int height, byte colorKey, ReadOnlySpan<uint> palette) => Blit.ShowRle08To16(X, Y, destination: lockedSurface.View(), data, w: width, h: height, key: colorKey, palette);

}
