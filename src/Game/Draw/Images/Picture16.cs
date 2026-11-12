// Translated from ImageLib.pas.
using System.Runtime.InteropServices;

using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// (ClassID $10): an opaque 16 bit picture. _pitch (bytes per row, a multiple of 4, or of 8 from
// 32 pixels on), FBuffer (the rows from its first byte. The original's FData, FBuffer aligned to
// 8 bytes, is FBuffer itself in the port)
public partial class Picture16 : Image16, ILibraryImageClass
{
    static int ILibraryImageClass.ClassId => 16;
    static string ILibraryImageClass.Name => "Picture (16bpp)";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new Picture16(owner);

    public Picture16(CustomImageLibrary owner) : base(owner)
    { }

    internal int _pitch;
    internal byte[] _buffer;

    // The rows of the picture: FBuffer from its first byte, FPitch bytes apart.
    internal SurfaceView View => new(_buffer, 0, _pitch);

    // ImageLib.pas:2206
    internal void AllocateDiscardableImageData(int width, int height)
    {
        int size;
        if (width < 32)
        {
            _pitch = ((width << 1) + 3) & ~3;
        }
        else
        {
            _pitch = ((width << 1) + 7) & ~7;
        }

        // The rows start at the first byte of FBuffer, as TPicture08's (graphics.md 6.8).
        size = _pitch * height + 7;
        _buffer = MemorySurfaces.Reallocate(_buffer, size);
    }

    // ImageLib.pas:2174
    public override void SetXshift(int value)
    {
        if (value < 0)
        {
            value = 0;
        }

        base.SetXshift(value);
        SetWidth(_width);
    }

    // ImageLib.pas:2181
    public override void SetYshift(int value)
    {
        if (value < 0)
        {
            value = 0;
        }

        base.SetYshift(value);
        SetHeight(_height);
    }

    // ImageLib.pas:2188
    public override void SetWidth(int value)
    {
        if (value < _xshift + _dataWidth)
        {
            value = _xshift + _dataWidth;
        }

        base.SetWidth(value);
    }

    // ImageLib.pas:2194
    public override void SetHeight(int value)
    {
        if (value < _yshift + _dataHeight)
        {
            value = _yshift + _dataHeight;
        }

        base.SetHeight(value);
    }

    // ImageLib.pas:2200
    public override bool SupportShowMode(ShowMode showMode, int pixelFormatId) => (pixelFormatId == 0 || pixelFormatId == PixelFormats.pfRGB565 || pixelFormatId == PixelFormats.pfRGB555) && Set<ShowMode>.Of(ShowMode.Opaque, ShowMode.Transparent, ShowMode.Blended).Contains(showMode);

    // ImageLib.pas:2219
    public override void ReleaseDiscardableImageData()
    {
        if (_dataLoaded)
        {
            _buffer = null;
            _dataLoaded = false;
        }
    }

    // ImageLib.pas:2228
    public override void ReadDiscardableImageData(Stream stream)
    {
        base.ReadDiscardableImageData(stream);
        AllocateDiscardableImageData(_dataWidth, _dataHeight);
        for (int Y = 0, last = _dataHeight - 1; Y <= last; Y++)
        {
            stream.ReadExactly(_buffer.AsSpan(Y * _pitch, _dataWidth * 2));
        }
    }

    // ImageLib.pas:2238
    public override void WriteDiscardableImageData(Stream stream)
    {
        base.WriteDiscardableImageData(stream);
        for (int Y = 0, last = _dataHeight - 1; Y <= last; Y++)
        {
            stream.Write(_buffer.AsSpan(Y * _pitch, _dataWidth * 2));
        }
    }

    // ImageLib.pas:2247
    public override Form CreateGrabForm(Component owner, in LockedSurface lockedSurface, ImagesGrabbedEvent onImagesGrabbed) => null;

    // ImageLib.pas:2253
    public override bool SupportGrabPixelFormat(int pixelFormatId) => pixelFormatId == PixelFormats.pfRGB565 || pixelFormatId == PixelFormats.pfRGB555 || pixelFormatId == PixelFormats.pfRGB24 || pixelFormatId == PixelFormats.pfPAL8;

    // ImageLib.pas:2259
    public override bool GrabImage(in LockedSurface lockedSurface, in Rect R, GrabInfo grabInfo)
    {
        bool result;
        try
        {
            Clear();
            result = true;
            _dataLoaded = true;
            _dataWidth = R.Right - R.Left;
            _dataHeight = R.Bottom - R.Top;
            _width = _dataWidth;
            _height = _dataHeight;
            _fileName = ExtractFileName(grabInfo.FileName);
            _fileIndex = grabInfo.FileIndex;
            _dataSize = _dataWidth * _dataHeight * 2;
            AllocateDiscardableImageData(_width, _height);
            switch (lockedSurface.PixelFormatId)
            {
                case PixelFormats.pfPAL8:
                    {
                        for (int Y = 0, last = _height - 1; Y <= last; Y++)
                        {
                            var S = lockedSurface.View().Bytes((R.Top + Y) * lockedSurface.Pitch + R.Left, _width);
                            var D = MemoryMarshal.Cast<byte, ushort>(_buffer.AsSpan(_pitch * Y, _width * 2));
                            for (int X = 0, lastX = _width - 1; X <= lastX; X++)
                            {
                                ref readonly Palette.Entry P = ref lockedSurface.Palette.Entries[S[X]];
                                D[X] = (ushort)(((P.Red & 248) << 8) + ((P.Green & 252) << 3) + (int)(((uint)P.Blue) >> 3));
                            }
                        }
                    }

                    _storedPixelFormatId = PixelFormats.pfRGB565;
                    break;
                case PixelFormats.pfRGB565:
                case PixelFormats.pfRGB555:
                    {
                        for (int I = 0, last = _height - 1; I <= last; I++)
                        {
                            lockedSurface.View().Bytes((R.Top + I) * lockedSurface.Pitch + R.Left * 2, _width * 2).CopyTo(_buffer.AsSpan(_pitch * I, _width * 2));
                        }
                    }

                    _storedPixelFormatId = lockedSurface.PixelFormatId;
                    break;
                case PixelFormats.pfRGB24:
                    {
                        for (int Y = 0, last = _height - 1; Y <= last; Y++)
                        {
                            var D = MemoryMarshal.Cast<byte, ushort>(_buffer.AsSpan(_pitch * Y, _width * 2));
                            var s24 = lockedSurface.View().Bytes((R.Top + Y) * lockedSurface.Pitch + R.Left * 3, _width * 3);
                            for (int X = 0, lastX = _width - 1; X <= lastX; X++)
                            {
                                D[X] = (ushort)(((s24[X * 3 + 2] & 248) << 8) + ((s24[X * 3 + 1] & 252) << 3) + (int)(((uint)s24[X * 3]) >> 3));
                            }
                        }
                    }

                    _storedPixelFormatId = PixelFormats.pfRGB565;
                    break;
                default:
                    result = false;
                    break;
            }

            _pixelFormatId = _storedPixelFormatId;
        }
        catch
        {
            ReleaseDiscardableImageData();
            result = false;
        }

        return result;
    }

    // ImageLib.pas:2329
    public override int RemapToPixelFormat(int pixelFormatId)
    {
        int result = 0;
        if (pixelFormatId != _pixelFormatId)
        {
            switch (pixelFormatId)
            {
                case PixelFormats.pfRGB565:
                    PixelRemap.RemapSurface555To565(View, View, _dataWidth, _dataHeight);
                    _pixelFormatId = PixelFormats.pfRGB565;
                    break;
                case PixelFormats.pfRGB555:
                    PixelRemap.RemapSurface565To555(View, View, _dataWidth, _dataHeight);
                    _pixelFormatId = PixelFormats.pfRGB555;
                    break;
                default:
                    result = PixelFormats.GFXE_NOTSUPPORTED;
                    break;
            }
        }

        return result;
    }

    // ImageLib.pas:2349
    public override void ShowOpaque(int X, int Y, in LockedSurface lockedSurface)
    {
        int destinationX;
        int right;
        int W;
        int H;
        int sx;
        int sy;
        int destinationY;
        int bottom;
        SurfaceView source;
        SurfaceView destination;
        destinationX = X + _xshift;
        W = _dataWidth;
        right = _dataWidth + destinationX;
        if (destinationX < lockedSurface.ClipRectangle.Left)
        {
            sx = lockedSurface.ClipRectangle.Left - X;
            destinationX = lockedSurface.ClipRectangle.Left;
            W -= sx;
        }
        else
        {
            sx = 0;
        }

        if (right > lockedSurface.ClipRectangle.Right)
        {
            W -= right - lockedSurface.ClipRectangle.Right;
        }

        if (W > 0)
        {
            destinationY = Y + _yshift;
            H = _dataHeight;
            bottom = destinationY + _dataHeight;
            if (destinationY < lockedSurface.ClipRectangle.Top)
            {
                sy = lockedSurface.ClipRectangle.Top - Y;
                destinationY = lockedSurface.ClipRectangle.Top;
                H -= sy;
            }
            else
            {
                sy = 0;
            }

            if (bottom > lockedSurface.ClipRectangle.Bottom)
            {
                H -= bottom - lockedSurface.ClipRectangle.Bottom;
            }

            if (H > 0)
            {
                if (!_dataLoaded)
                {
                    LoadDiscardableImageData();
                    source = View.At(sx * 2 + sy * _pitch);
                    destination = lockedSurface.View((destinationX << 1) + lockedSurface.Pitch * destinationY);
                    if (lockedSurface.PixelFormatId == _pixelFormatId)
                    {
                        BlitRects.BlitRectangle(source, destination, W * 2, H);
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BlitRectangle(source, destination, W * 2, H);
                    }

                    if (_loadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    source = View.At(sx * 2 + sy * _pitch);
                    destination = lockedSurface.View((destinationX << 1) + lockedSurface.Pitch * destinationY);
                    if (lockedSurface.PixelFormatId == _pixelFormatId)
                    {
                        BlitRects.BlitRectangle(source, destination, W * 2, H);
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BlitRectangle(source, destination, W * 2, H);
                    }
                }
            }
        }
    }

    // ImageLib.pas:2412
    public override void ShowBlended(int X, int Y, in LockedSurface lockedSurface, BlendTable table)
    {
        int destinationX;
        int right;
        int W;
        int H;
        int sx;
        int sy;
        int destinationY;
        int bottom;
        SurfaceView source;
        SurfaceView destination;
        destinationX = X + _xshift;
        W = _dataWidth;
        right = _dataWidth + destinationX;
        if (destinationX < lockedSurface.ClipRectangle.Left)
        {
            sx = lockedSurface.ClipRectangle.Left - X;
            destinationX = lockedSurface.ClipRectangle.Left;
            W -= sx;
        }
        else
        {
            sx = 0;
        }

        if (right > lockedSurface.ClipRectangle.Right)
        {
            W -= right - lockedSurface.ClipRectangle.Right;
        }

        if (W > 0)
        {
            destinationY = Y + _yshift;
            H = _dataHeight;
            bottom = destinationY + _dataHeight;
            if (destinationY < lockedSurface.ClipRectangle.Top)
            {
                sy = lockedSurface.ClipRectangle.Top - Y;
                destinationY = lockedSurface.ClipRectangle.Top;
                H -= sy;
            }
            else
            {
                sy = 0;
            }

            if (bottom > lockedSurface.ClipRectangle.Bottom)
            {
                H -= bottom - lockedSurface.ClipRectangle.Bottom;
            }

            if (H > 0)
            {
                if (!_dataLoaded)
                {
                    LoadDiscardableImageData();
                    source = View.At(sx * 2 + sy * _pitch);
                    destination = lockedSurface.View((destinationX << 1) + lockedSurface.Pitch * destinationY);
                    if (lockedSurface.PixelFormatId == _pixelFormatId)
                    {
                        if (table.Bits == 5)
                        {
                            switch (_pixelFormatId)
                            {
                                case PixelFormats.pfRGB565:
                                    BlendSurface565(source, destination, W, H, ((BlendTable5Bit)table).Bytes);
                                    break;
                                case PixelFormats.pfRGB555:
                                    BlendSurface555(source, destination, W, H, ((BlendTable5Bit)table).Bytes);
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
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (table.Bits == 5)
                        {
                            switch (_pixelFormatId)
                            {
                                case PixelFormats.pfRGB565:
                                    BlendSurface565(source, destination, W, H, ((BlendTable5Bit)table).Bytes);
                                    break;
                                case PixelFormats.pfRGB555:
                                    BlendSurface555(source, destination, W, H, ((BlendTable5Bit)table).Bytes);
                                    break;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid BlendTable format");
                        }
                    }

                    if (_loadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    source = View.At(sx * 2 + sy * _pitch);
                    destination = lockedSurface.View((destinationX << 1) + lockedSurface.Pitch * destinationY);
                    if (lockedSurface.PixelFormatId == _pixelFormatId)
                    {
                        if (table.Bits == 5)
                        {
                            switch (_pixelFormatId)
                            {
                                case PixelFormats.pfRGB565:
                                    BlendSurface565(source, destination, W, H, ((BlendTable5Bit)table).Bytes);
                                    break;
                                case PixelFormats.pfRGB555:
                                    BlendSurface555(source, destination, W, H, ((BlendTable5Bit)table).Bytes);
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
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (table.Bits == 5)
                        {
                            switch (_pixelFormatId)
                            {
                                case PixelFormats.pfRGB565:
                                    BlendSurface565(source, destination, W, H, ((BlendTable5Bit)table).Bytes);
                                    break;
                                case PixelFormats.pfRGB555:
                                    BlendSurface555(source, destination, W, H, ((BlendTable5Bit)table).Bytes);
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
    public override void ShowTransparent(int X, int Y, in LockedSurface lockedSurface)
    {
        int destinationX;
        int right;
        int W;
        int H;
        int sx;
        int sy;
        int destinationY;
        int bottom;
        SurfaceView source;
        SurfaceView destination;
        destinationX = X + _xshift;
        W = _dataWidth;
        right = _dataWidth + destinationX;
        if (destinationX < lockedSurface.ClipRectangle.Left)
        {
            sx = lockedSurface.ClipRectangle.Left - X;
            destinationX = lockedSurface.ClipRectangle.Left;
            W -= sx;
        }
        else
        {
            sx = 0;
        }

        if (right > lockedSurface.ClipRectangle.Right)
        {
            W -= right - lockedSurface.ClipRectangle.Right;
        }

        if (W > 0)
        {
            destinationY = Y + _yshift;
            H = _dataHeight;
            bottom = destinationY + _dataHeight;
            if (destinationY < lockedSurface.ClipRectangle.Top)
            {
                sy = lockedSurface.ClipRectangle.Top - Y;
                destinationY = lockedSurface.ClipRectangle.Top;
                H -= sy;
            }
            else
            {
                sy = 0;
            }

            if (bottom > lockedSurface.ClipRectangle.Bottom)
            {
                H -= bottom - lockedSurface.ClipRectangle.Bottom;
            }

            if (H > 0)
            {
                if (!_dataLoaded)
                {
                    LoadDiscardableImageData();
                    source = View.At(sx * 2 + sy * _pitch);
                    destination = lockedSurface.View((destinationX << 1) + lockedSurface.Pitch * destinationY);
                    if (lockedSurface.PixelFormatId == _pixelFormatId)
                    {
                        if (_pixelFormatId == PixelFormats.pfRGB565)
                        {
                            BlitTransparent565(source, destination, W, H);
                        }
                        else
                        {
                            BlitTransparent555(source, destination, W, H);
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (_pixelFormatId == PixelFormats.pfRGB565)
                        {
                            BlitTransparent565(source, destination, W, H);
                        }
                        else
                        {
                            BlitTransparent555(source, destination, W, H);
                        }
                    }

                    if (_loadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    source = View.At(sx * 2 + sy * _pitch);
                    destination = lockedSurface.View((destinationX << 1) + lockedSurface.Pitch * destinationY);
                    if (lockedSurface.PixelFormatId == _pixelFormatId)
                    {
                        if (_pixelFormatId == PixelFormats.pfRGB565)
                        {
                            BlitTransparent565(source, destination, W, H);
                        }
                        else
                        {
                            BlitTransparent555(source, destination, W, H);
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (_pixelFormatId == PixelFormats.pfRGB565)
                        {
                            BlitTransparent565(source, destination, W, H);
                        }
                        else
                        {
                            BlitTransparent555(source, destination, W, H);
                        }
                    }
                }
            }
        }
    }

    private static void BlitTransparent565(SurfaceView source, SurfaceView destination, int width, int height)
    {
        Blit.BlitTransparent565(source, destination, width, height);
    }

    private static void BlitTransparent555(SurfaceView source, SurfaceView destination, int width, int height)
    {
        Blit.BlitTransparent555(source, destination, width, height);
    }

    internal static void BlitStretched16(SurfaceView source, ReadOnlySpan<int> rowTable, ReadOnlySpan<int> columnTable, SurfaceView destination, int width, int height)
    {
        Blit.BlitStretched16(source, rowTable, columnTable, destination, width, height);
    }

    private static void BlendSurface555(SurfaceView source, SurfaceView destination, int width, int height, ReadOnlySpan<byte> table)
    {
        Blit.BlendSurface555(source, destination, width, height, table);
    }

    private static void BlendSurface565(SurfaceView source, SurfaceView destination, int width, int height, ReadOnlySpan<byte> table)
    {
        Blit.BlendSurface565(source, destination, width, height, table);
    }

    internal static void BlendStretched555(SurfaceView source, ReadOnlySpan<int> rowTable, ReadOnlySpan<int> columnTable, SurfaceView destination, int width, int height, ReadOnlySpan<byte> table)
    {
        Blit.BlendStretched555(source, rowTable, columnTable, destination, width, height, table);
    }

    private static void BlendStretched565(SurfaceView source, ReadOnlySpan<int> rowTable, ReadOnlySpan<int> columnTable, SurfaceView destination, int width, int height, ReadOnlySpan<byte> table)
    {
        Blit.BlendStretched565(source, rowTable, columnTable, destination, width, height, table);
    }

}
