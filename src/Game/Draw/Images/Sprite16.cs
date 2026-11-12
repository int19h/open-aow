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
    static int ILibraryImageClass.ClassId => 22;
    static string ILibraryImageClass.Name => "Sprite (16bpp)";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new Sprite16(owner);

    internal int _pitch;
    internal byte[] _buffer;
    internal int _transparentColor;
    internal int _colorKey;
    internal int _spriteX;
    internal int _spriteY;
    internal int _spriteWidth;
    internal int _spriteHeight;

    // The rows of the sprite: FBuffer from its first byte, FPitch bytes apart.
    internal SurfaceView View => new(_buffer, 0, _pitch);

    // ImageLib.pas:4302
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

    // ImageLib.pas:4247
    public Sprite16(CustomImageLibrary owner) : base(owner)
    {
    }

    // ImageLib.pas:4262
    public override void SetXshift(int value)
    {
        if (value < -_spriteX)
        {
            value = -_spriteX;
        }

        base.SetXshift(value);
        SetWidth(_width);
    }

    // ImageLib.pas:4269
    public override void SetYshift(int value)
    {
        if (value < -_spriteY)
        {
            value = -_spriteY;
        }

        base.SetYshift(value);
        SetHeight(_height);
    }

    // ImageLib.pas:4276
    public override void SetWidth(int value)
    {
        int W;
        W = _xshift + _spriteX + _spriteWidth;
        if (value < W)
        {
            value = W;
        }

        base.SetWidth(value);
    }

    // ImageLib.pas:4285
    public override void SetHeight(int value)
    {
        int W;
        W = _yshift + _spriteY + _spriteHeight;
        if (value < W)
        {
            value = W;
        }

        base.SetHeight(value);
    }

    // ImageLib.pas:4294
    public override void GetVisualDimensions(ref Rect R)
    {
        R.Left = _xshift + _spriteX;
        R.Right = R.Left + _spriteWidth;
        R.Top = _yshift + _spriteY;
        R.Bottom = R.Top + _spriteHeight;
    }

    // ImageLib.pas:4315
    public override void ReleaseDiscardableImageData()
    {
        if (_dataLoaded)
        {
            _buffer = null;
            _dataLoaded = false;
        }
    }

    // ImageLib.pas:4324
    public override void ReadImageData(Stream stream)
    {
        base.ReadImageData(stream);
        stream.ReadExactly(Bytes.Of(ref _spriteWidth, 4));
        stream.ReadExactly(Bytes.Of(ref _spriteHeight, 4));
        stream.ReadExactly(Bytes.Of(ref _spriteX, 4));
        stream.ReadExactly(Bytes.Of(ref _spriteY, 4));
        stream.ReadExactly(Bytes.Of(ref _transparentColor, 4));
    }

    // ImageLib.pas:4334
    public override void WriteImageData(Stream stream)
    {
        base.WriteImageData(stream);
        stream.Write(Bytes.Of(ref _spriteWidth, 4));
        stream.Write(Bytes.Of(ref _spriteHeight, 4));
        stream.Write(Bytes.Of(ref _spriteX, 4));
        stream.Write(Bytes.Of(ref _spriteY, 4));
        stream.Write(Bytes.Of(ref _transparentColor, 4));
    }

    // ImageLib.pas:4344
    public override void ReadDiscardableImageData(Stream stream)
    {
        base.ReadDiscardableImageData(stream);
        AllocateDiscardableImageData(_spriteWidth, _spriteHeight);
        for (int Y = 0, last = _spriteHeight - 1; Y <= last; Y++)
        {
            stream.ReadExactly(_buffer.AsSpan(Y * _pitch, _spriteWidth * 2));
        }

        _colorKey = _transparentColor;
    }

    // ImageLib.pas:4355
    public override void WriteDiscardableImageData(Stream stream)
    {
        base.WriteDiscardableImageData(stream);
        for (int Y = 0, last = _spriteHeight - 1; Y <= last; Y++)
        {
            stream.Write(_buffer.AsSpan(Y * _pitch, _spriteWidth * 2));
        }
    }

    // ImageLib.pas:4364
    public override int RemapToPixelFormat(int pixelFormatId)
    {
        int result = 0;
        if (pixelFormatId != _pixelFormatId)
        {
            switch (pixelFormatId)
            {
                case PixelFormats.pfRGB565:
                    PixelRemap.RemapSurface555To565(View, View, _spriteWidth, _spriteHeight);
                    _colorKey = PixelRemap.RemapPixelInfo(_colorKey, PixelFormats.pfRGB555, null, PixelFormats.pfRGB565, null);
                    _pixelFormatId = PixelFormats.pfRGB565;
                    break;
                case PixelFormats.pfRGB555:
                    PixelRemap.RemapSurface565To555(View, View, _spriteWidth, _spriteHeight);
                    _colorKey = PixelRemap.RemapPixelInfo(_colorKey, PixelFormats.pfRGB565, null, PixelFormats.pfRGB555, null);
                    _pixelFormatId = PixelFormats.pfRGB555;
                    break;
                default:
                    result = PixelFormats.GFXE_NOTSUPPORTED;
                    break;
            }
        }

        return result;
    }

    // ImageLib.pas:4386
    public override bool SupportShowMode(ShowMode showMode, int pixelFormatId) => (pixelFormatId == 0 || pixelFormatId == PixelFormats.pfRGB565 || pixelFormatId == PixelFormats.pfRGB555) && Set<ShowMode>.Of(ShowMode.Opaque, ShowMode.Transparent, ShowMode.Blended).Contains(showMode);

    // ImageLib.pas:4392
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
        X += _spriteX;
        destinationX = _xshift + X;
        W = _spriteWidth;
        right = _spriteWidth + destinationX;
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
            Y += _spriteY;
            destinationY = Y + _yshift;
            H = _spriteHeight;
            bottom = destinationY + _spriteHeight;
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
                        BlitRects.BlitColorKey16(source, destination, W, H, _colorKey);
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BlitColorKey16(source, destination, W, H, _colorKey);
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
                        BlitRects.BlitColorKey16(source, destination, W, H, _colorKey);
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BlitColorKey16(source, destination, W, H, _colorKey);
                    }
                }
            }
        }
    }

    // ImageLib.pas:4457
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
        X += _spriteX;
        destinationX = _xshift + X;
        W = _spriteWidth;
        right = _spriteWidth + destinationX;
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
            Y += _spriteY;
            destinationY = Y + _yshift;
            H = _spriteHeight;
            bottom = destinationY + _spriteHeight;
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
                        switch (_pixelFormatId)
                        {
                            case PixelFormats.pfRGB555:
                                BlendColorKey555(source, destination, W, H, _colorKey, ((BlendTable5Bit)table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendColorKey565(source, destination, W, H, _colorKey, ((BlendTable5Bit)table).Bytes);
                                break;
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        switch (_pixelFormatId)
                        {
                            case PixelFormats.pfRGB555:
                                BlendColorKey555(source, destination, W, H, _colorKey, ((BlendTable5Bit)table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendColorKey565(source, destination, W, H, _colorKey, ((BlendTable5Bit)table).Bytes);
                                break;
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
                        switch (_pixelFormatId)
                        {
                            case PixelFormats.pfRGB555:
                                BlendColorKey555(source, destination, W, H, _colorKey, ((BlendTable5Bit)table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendColorKey565(source, destination, W, H, _colorKey, ((BlendTable5Bit)table).Bytes);
                                break;
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        switch (_pixelFormatId)
                        {
                            case PixelFormats.pfRGB555:
                                BlendColorKey555(source, destination, W, H, _colorKey, ((BlendTable5Bit)table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendColorKey565(source, destination, W, H, _colorKey, ((BlendTable5Bit)table).Bytes);
                                break;
                        }
                    }
                }
            }
        }
    }

    // ImageLib.pas:4543
    // { ShowTransparent shows the sprite blended 50% }
    public override void ShowTransparent(int X, int Y, in LockedSurface lockedSurface)
    {
        ShowBlended(X, Y, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[50]);
    }

    // ImageLib.pas:4548
    public override bool SupportGrabPixelFormat(int pixelFormatId) => pixelFormatId == PixelFormats.pfRGB565 || pixelFormatId == PixelFormats.pfRGB555 || pixelFormatId == PixelFormats.pfRGB24 || pixelFormatId == PixelFormats.pfPAL8;

    // ImageLib.pas:4554
    public override Form CreateGrabForm(Component owner, in LockedSurface lockedSurface, ImagesGrabbedEvent onImagesGrabbed) => null;

    // ImageLib.pas:4561
    // { GrabImage locks the conversion surface and never unlocks it (an original bug) }
    public override bool GrabImage(in LockedSurface lockedSurface, in Rect R, GrabInfo grabInfo)
    {
        DataSurface surface;
        LockedSurface lSD = default;
        Rect gr = default;
        Rect mr = default;
        int left;
        int top;
        int bottom;
        // { Absolute views preserve frame slots and the separate GR coordinate reads. }
        bool result = true;
        try
        {
            surface = null;
            try
            {
                if (lockedSurface.BitCount != 16)
                {
                    surface = new DataSurface(R.Right - R.Left, R.Bottom - R.Top, PixelFormats.pfRGB565, null);
                    gr.Left = 0;
                    gr.Top = 0;
                    var width = surface.GetWidth();
                    gr.Right = width;
                    var height = surface.GetHeight();
                    gr.Bottom = height;
                    surface.Lock(ref lSD);
                    var surfaceWidth = surface.GetWidth();
                    var surfaceHeight = surface.GetHeight();
                    if (LockedSurface.RemapSurfacePart(lockedSurface, R.Left, R.Top, surfaceWidth, surfaceHeight, lSD, 0, 0) != 0)
                    {
                        throw new ArgumentException("Invalid PixelFormat");
                    }

                    _transparentColor = PixelRemap.RemapPixelInfo(grabInfo.TransparentPixel, lockedSurface.PixelFormatId, lockedSurface.Palette, lSD.PixelFormatId, null);
                }
                else
                {
                    lSD = lockedSurface;
                    gr = R;
                    _transparentColor = grabInfo.TransparentPixel;
                }

                Clear();
                _dataLoaded = true;
                _dataWidth = gr.Right - gr.Left;
                _dataHeight = gr.Bottom - gr.Top;
                _width = _dataWidth;
                _height = _dataHeight;
                _fileName = ExtractFileName(grabInfo.FileName);
                _fileIndex = grabInfo.FileIndex;
                _storedPixelFormatId = lSD.PixelFormatId;
                _pixelFormatId = _storedPixelFormatId;
                _colorKey = _transparentColor;
                LockedSurface.MinimizeTransparentRectangle(lSD, gr, _transparentColor, ref mr);
                left = mr.Left;
                _spriteX = left - gr.Left;
                top = mr.Top;
                _spriteY = top - gr.Top;
                _spriteWidth = mr.Right - left;
                bottom = mr.Bottom;
                _spriteHeight = bottom - top;
                _dataSize = _spriteWidth * _spriteHeight * 2;
                AllocateDiscardableImageData(_spriteWidth, _spriteHeight);
                for (int Y = top; Y <= bottom - 1; Y++)
                {
                    lSD.View().Bytes(lSD.Pitch * Y + mr.Left * 2, _spriteWidth * 2).CopyTo(_buffer.AsSpan((Y - mr.Top) * _pitch, _spriteWidth * 2));
                }
            }
            finally
            {
                if (surface != null)
                {
                    surface.Dispose();
                }
            }
        }
        catch
        {
            ReleaseDiscardableImageData();
            result = false;
        }

        return result;
    }

    private static void BlendColorKey555(SurfaceView source, SurfaceView destination, int width, int height, int colorKey, ReadOnlySpan<byte> table)
    {
        Blit.BlendColorKey555(source, destination, width, height, colorKey, table);
    }

    private static void BlendColorKey565(SurfaceView source, SurfaceView destination, int width, int height, int colorKey, ReadOnlySpan<byte> table)
    {
        Blit.BlendColorKey565(source, destination, width, height, colorKey, table);
    }

}
