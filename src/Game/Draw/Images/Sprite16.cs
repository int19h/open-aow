#nullable disable
// Translated from ImageLib.pas.
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.DelphiPaths;

namespace Aow.Game.Draw.Images;

// (ClassID $16): a 16 bit picture with a transparent color, cut to the smallest rectangle
// holding its opaque pixels. FPitch, FBuffer (as TPicture16), FTransparentColor (in
// FStoredPixelFormatID), FColorKey (the transparent color in FPixelFormatID), FSpriteX,
// FSpriteY (of the pixels in the grabbed rectangle), FSpriteWidth, FSpriteHeight (of the
// pixels)
public class Sprite16(CustomImageLibrary owner) : Image16(owner), ILibraryImageClass
{
    static int ILibraryImageClass.ClassId => 22;
    static string ILibraryImageClass.Name => "Sprite (16bpp)";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new Sprite16(owner);

    private int _pitch;
    private byte[] _buffer;
    private int _transparentColor;
    private int _colorKey;
    private int _spriteX;
    private int _spriteY;
    private int _spriteWidth;
    private int _spriteHeight;

    // The rows of the sprite: FBuffer from its first byte, FPitch bytes apart.
    internal SurfaceView View => new(_buffer, start: 0, _pitch);

    // ImageLib.pas:4302
    internal void AllocateDiscardableImageData(int width, int height)
    {
        _pitch = width < 32 ? ((width << 1) + 3) & ~0b0011 : ((width << 1) + 7) & ~0b0111;

        // The rows start at the first byte of FBuffer, as TPicture08's (graphics.md 6.8).
        int size = (_pitch * height) + 7;
        _buffer = MemorySurfaces.Reallocate(_buffer, size);
    }

    // ImageLib.pas:4262
    public override void SetXshift(int value)
    {
        if (value < -_spriteX)
        {
            value = -_spriteX;
        }

        base.SetXshift(value);
        SetWidth(Width);
    }

    // ImageLib.pas:4269
    public override void SetYshift(int value)
    {
        if (value < -_spriteY)
        {
            value = -_spriteY;
        }

        base.SetYshift(value);
        SetHeight(Height);
    }

    // ImageLib.pas:4276
    public override void SetWidth(int value)
    {
        int w = Xshift + _spriteX + _spriteWidth;
        if (value < w)
        {
            value = w;
        }

        base.SetWidth(value);
    }

    // ImageLib.pas:4285
    public override void SetHeight(int value)
    {
        int w = Yshift + _spriteY + _spriteHeight;
        if (value < w)
        {
            value = w;
        }

        base.SetHeight(value);
    }

    // ImageLib.pas:4294
    public override void GetVisualDimensions(out Rect R)
    {
        int left = Xshift + _spriteX;
        int top = Yshift + _spriteY;
        R = new() { Left = left, Right = left + _spriteWidth, Top = top, Bottom = top + _spriteHeight };
    }

    // ImageLib.pas:4315
    public override void ReleaseDiscardableImageData()
    {
        if (DataLoaded)
        {
            _buffer = null;
            DataLoaded = false;
        }
    }

    // ImageLib.pas:4324
    public override void ReadImageData(Stream stream)
    {
        base.ReadImageData(stream);
        stream.ReadExactly(Bytes.Of(ref _spriteWidth));
        stream.ReadExactly(Bytes.Of(ref _spriteHeight));
        stream.ReadExactly(Bytes.Of(ref _spriteX));
        stream.ReadExactly(Bytes.Of(ref _spriteY));
        stream.ReadExactly(Bytes.Of(ref _transparentColor));
    }

    // ImageLib.pas:4334
    public override void WriteImageData(Stream stream)
    {
        base.WriteImageData(stream);
        stream.Write(Bytes.Of(ref _spriteWidth));
        stream.Write(Bytes.Of(ref _spriteHeight));
        stream.Write(Bytes.Of(ref _spriteX));
        stream.Write(Bytes.Of(ref _spriteY));
        stream.Write(Bytes.Of(ref _transparentColor));
    }

    // ImageLib.pas:4344
    public override void ReadDiscardableImageData(Stream stream)
    {
        base.ReadDiscardableImageData(stream);
        AllocateDiscardableImageData(_spriteWidth, _spriteHeight);
        for (int y = 0, last = _spriteHeight - 1; y <= last; y++)
        {
            stream.ReadExactly(_buffer.AsSpan(y * _pitch, length: _spriteWidth * 2));
        }

        _colorKey = _transparentColor;
    }

    // ImageLib.pas:4355
    public override void WriteDiscardableImageData(Stream stream)
    {
        base.WriteDiscardableImageData(stream);
        for (int y = 0, last = _spriteHeight - 1; y <= last; y++)
        {
            stream.Write(_buffer.AsSpan(y * _pitch, length: _spriteWidth * 2));
        }
    }

    // ImageLib.pas:4364
    public override GraphicsResult RemapToPixelFormat(PixelFormat pixelFormatId)
    {
        if (pixelFormatId != PixelFormatId)
        {
            switch (pixelFormatId)
            {
                case PixelFormat.Rgb565:
                    PixelRemap.RemapSurface555To565(View, destination: View, _spriteWidth, _spriteHeight);
                    _colorKey = PixelRemap.RemapPixelInfo(_colorKey, sourcePixelFormatId: PixelFormat.Rgb555, sourcePalette: null, destinationPixelFormatId: PixelFormat.Rgb565, destinationPalette: null);
                    PixelFormatId = PixelFormat.Rgb565;
                    break;
                case PixelFormat.Rgb555:
                    PixelRemap.RemapSurface565To555(View, destination: View, _spriteWidth, _spriteHeight);
                    _colorKey = PixelRemap.RemapPixelInfo(_colorKey, sourcePixelFormatId: PixelFormat.Rgb565, sourcePalette: null, destinationPixelFormatId: PixelFormat.Rgb555, destinationPalette: null);
                    PixelFormatId = PixelFormat.Rgb555;
                    break;
                case PixelFormat.None:
                case PixelFormat.Pal8:
                case PixelFormat.Argb1555:
                case PixelFormat.Rgb24:
                case PixelFormat.Bgr24:
                case PixelFormat.Rgb32:
                case PixelFormat.Xrgb32:
                default:
                    return GraphicsResult.NotSupported;
            }
        }

        return GraphicsResult.Ok;
    }

    // ImageLib.pas:4386
    public override bool SupportShowMode(ShowMode showMode, PixelFormat pixelFormatId) => (pixelFormatId is PixelFormat.None or PixelFormat.Rgb565 or PixelFormat.Rgb555) && Set<ShowMode>.Of(ShowMode.Opaque, ShowMode.Transparent, ShowMode.Blended).Contains(showMode);

    // ImageLib.pas:4392
    public override void ShowOpaque(int X, int Y, in LockedSurface lockedSurface)
    {
        X += _spriteX;
        int destinationX = Xshift + X;
        int w = _spriteWidth;
        int right = _spriteWidth + destinationX;
        int sx;
        if (destinationX < lockedSurface.ClipRectangle.Left)
        {
            sx = lockedSurface.ClipRectangle.Left - X;
            destinationX = lockedSurface.ClipRectangle.Left;
            w -= sx;
        }
        else
        {
            sx = 0;
        }

        if (right > lockedSurface.ClipRectangle.Right)
        {
            w -= right - lockedSurface.ClipRectangle.Right;
        }

        if (w > 0)
        {
            Y += _spriteY;
            int destinationY = Y + Yshift;
            int h = _spriteHeight;
            int bottom = destinationY + _spriteHeight;
            int sy;
            if (destinationY < lockedSurface.ClipRectangle.Top)
            {
                sy = lockedSurface.ClipRectangle.Top - Y;
                destinationY = lockedSurface.ClipRectangle.Top;
                h -= sy;
            }
            else
            {
                sy = 0;
            }

            if (bottom > lockedSurface.ClipRectangle.Bottom)
            {
                h -= bottom - lockedSurface.ClipRectangle.Bottom;
            }

            if (h > 0)
            {
                SurfaceView source;
                SurfaceView destination;
                if (!DataLoaded)
                {
                    LoadDiscardableImageData();
                    source = View.At((sx * 2) + (sy * _pitch));
                    destination = lockedSurface.View((destinationX << 1) + (lockedSurface.Pitch * destinationY));
                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        BlitRects.BlitColorKey16(source, destination, w, h, _colorKey);
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BlitColorKey16(source, destination, w, h, _colorKey);
                    }

                    if (LoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    source = View.At((sx * 2) + (sy * _pitch));
                    destination = lockedSurface.View((destinationX << 1) + (lockedSurface.Pitch * destinationY));
                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        BlitRects.BlitColorKey16(source, destination, w, h, _colorKey);
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BlitColorKey16(source, destination, w, h, _colorKey);
                    }
                }
            }
        }
    }

    // ImageLib.pas:4457
    public override void ShowBlended(int X, int Y, in LockedSurface lockedSurface, BlendTable table)
    {
        X += _spriteX;
        int destinationX = Xshift + X;
        int w = _spriteWidth;
        int right = _spriteWidth + destinationX;
        int sx;
        if (destinationX < lockedSurface.ClipRectangle.Left)
        {
            sx = lockedSurface.ClipRectangle.Left - X;
            destinationX = lockedSurface.ClipRectangle.Left;
            w -= sx;
        }
        else
        {
            sx = 0;
        }

        if (right > lockedSurface.ClipRectangle.Right)
        {
            w -= right - lockedSurface.ClipRectangle.Right;
        }

        if (w > 0)
        {
            Y += _spriteY;
            int destinationY = Y + Yshift;
            int h = _spriteHeight;
            int bottom = destinationY + _spriteHeight;
            int sy;
            if (destinationY < lockedSurface.ClipRectangle.Top)
            {
                sy = lockedSurface.ClipRectangle.Top - Y;
                destinationY = lockedSurface.ClipRectangle.Top;
                h -= sy;
            }
            else
            {
                sy = 0;
            }

            if (bottom > lockedSurface.ClipRectangle.Bottom)
            {
                h -= bottom - lockedSurface.ClipRectangle.Bottom;
            }

            if (h > 0)
            {
                SurfaceView source;
                SurfaceView destination;
                if (!DataLoaded)
                {
                    LoadDiscardableImageData();
                    source = View.At((sx * 2) + (sy * _pitch));
                    destination = lockedSurface.View((destinationX << 1) + (lockedSurface.Pitch * destinationY));
                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        switch (PixelFormatId)
                        {
                            case PixelFormat.Rgb555:
                                BlendColorKey555(source, destination, w, h, _colorKey, table: table.Bytes);
                                break;
                            case PixelFormat.Rgb565:
                                BlendColorKey565(source, destination, w, h, _colorKey, table: table.Bytes);
                                break;
                            case PixelFormat.None:
                            case PixelFormat.Pal8:
                            case PixelFormat.Argb1555:
                            case PixelFormat.Rgb24:
                            case PixelFormat.Bgr24:
                            case PixelFormat.Rgb32:
                            case PixelFormat.Xrgb32:
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        switch (PixelFormatId)
                        {
                            case PixelFormat.Rgb555:
                                BlendColorKey555(source, destination, w, h, _colorKey, table: table.Bytes);
                                break;
                            case PixelFormat.Rgb565:
                                BlendColorKey565(source, destination, w, h, _colorKey, table: table.Bytes);
                                break;
                            case PixelFormat.None:
                            case PixelFormat.Pal8:
                            case PixelFormat.Argb1555:
                            case PixelFormat.Rgb24:
                            case PixelFormat.Bgr24:
                            case PixelFormat.Rgb32:
                            case PixelFormat.Xrgb32:
                            default:
                                break;
                        }
                    }

                    if (LoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    source = View.At((sx * 2) + (sy * _pitch));
                    destination = lockedSurface.View((destinationX << 1) + (lockedSurface.Pitch * destinationY));
                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        switch (PixelFormatId)
                        {
                            case PixelFormat.Rgb555:
                                BlendColorKey555(source, destination, w, h, _colorKey, table: table.Bytes);
                                break;
                            case PixelFormat.Rgb565:
                                BlendColorKey565(source, destination, w, h, _colorKey, table: table.Bytes);
                                break;
                            case PixelFormat.None:
                            case PixelFormat.Pal8:
                            case PixelFormat.Argb1555:
                            case PixelFormat.Rgb24:
                            case PixelFormat.Bgr24:
                            case PixelFormat.Rgb32:
                            case PixelFormat.Xrgb32:
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        switch (PixelFormatId)
                        {
                            case PixelFormat.Rgb555:
                                BlendColorKey555(source, destination, w, h, _colorKey, table: table.Bytes);
                                break;
                            case PixelFormat.Rgb565:
                                BlendColorKey565(source, destination, w, h, _colorKey, table: table.Bytes);
                                break;
                            case PixelFormat.None:
                            case PixelFormat.Pal8:
                            case PixelFormat.Argb1555:
                            case PixelFormat.Rgb24:
                            case PixelFormat.Bgr24:
                            case PixelFormat.Rgb32:
                            case PixelFormat.Xrgb32:
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }

    // ImageLib.pas:4543
    // { ShowTransparent shows the sprite blended 50% }
    public override void ShowTransparent(int X, int Y, in LockedSurface lockedSurface) => ShowBlended(X, Y, lockedSurface, table: GraphicsState.AlphaBlendTable.LookupTables[50]);

    // ImageLib.pas:4548
    public override bool SupportGrabPixelFormat(PixelFormat pixelFormatId) => pixelFormatId is PixelFormat.Rgb565 or PixelFormat.Rgb555 or PixelFormat.Rgb24 or PixelFormat.Pal8;

    // ImageLib.pas:4561
    // { GrabImage locks the conversion surface and never unlocks it (an original bug) }
    public override bool GrabImage(in LockedSurface lockedSurface, in Rect R, GrabInfo grabInfo)
    {
        Rect gr = default;
        // { Absolute views preserve frame slots and the separate GR coordinate reads. }
        try
        {
            DataSurface surface = null;
            try
            {
                LockedSurface lSD = default;
                if (lockedSurface.BitCount != 16)
                {
                    surface = new DataSurface(R.Right - R.Left, R.Bottom - R.Top, pixelFormatId: PixelFormat.Rgb565, palette: null);
                    gr.Left = 0;
                    gr.Top = 0;
                    int width = surface.Width;
                    gr.Right = width;
                    int height = surface.Height;
                    gr.Bottom = height;
                    _ = surface.Lock(ref lSD);
                    if (LockedSurface.RemapSurfacePart(width: surface.Width, height: surface.Height, source: lockedSurface, sourceX: R.Left, sourceY: R.Top, destination: lSD, destinationX: 0, destinationY: 0) != 0)
                    {
                        throw new ArgumentException("Invalid PixelFormat");
                    }

                    _transparentColor = PixelRemap.RemapPixelInfo(grabInfo.TransparentPixel, sourcePixelFormatId: lockedSurface.PixelFormatId, sourcePalette: lockedSurface.Palette, destinationPixelFormatId: lSD.PixelFormatId, destinationPalette: null);
                }
                else
                {
                    lSD = lockedSurface;
                    gr = R;
                    _transparentColor = grabInfo.TransparentPixel;
                }

                Clear();
                DataLoaded = true;
                DataWidth = gr.Right - gr.Left;
                DataHeight = gr.Bottom - gr.Top;
                SetWidthWithoutOverrides(DataWidth);
                SetHeightWithoutOverrides(DataHeight);
                FileName = ExtractFileName(grabInfo.FileName);
                FileIndex = grabInfo.FileIndex;
                StoredPixelFormatId = lSD.PixelFormatId;
                PixelFormatId = StoredPixelFormatId;
                _colorKey = _transparentColor;
                LockedSurface.MinimizeTransparentRectangle(lSD, R: gr, _transparentColor, result: out Rect mr);
                int left = mr.Left;
                _spriteX = left - gr.Left;
                int top = mr.Top;
                _spriteY = top - gr.Top;
                _spriteWidth = mr.Right - left;
                int bottom = mr.Bottom;
                _spriteHeight = bottom - top;
                DataSize = _spriteWidth * _spriteHeight * 2;
                AllocateDiscardableImageData(_spriteWidth, _spriteHeight);
                for (int y = top; y <= bottom - 1; y++)
                {
                    lSD.View().Bytes((lSD.Pitch * y) + (mr.Left * 2), count: _spriteWidth * 2).CopyTo(_buffer.AsSpan((y - mr.Top) * _pitch, length: _spriteWidth * 2));
                }
            }
            finally
            {
                surface?.Dispose();
            }
        }
        catch
        {
            ReleaseDiscardableImageData();
            return false;
        }

        return true;
    }

    private static void BlendColorKey555(SurfaceView source, SurfaceView destination, int width, int height, int colorKey, ReadOnlySpan<byte> table) => Blit.BlendColorKey555(source, d: destination, w: width, h: height, k: colorKey, t: table);

    private static void BlendColorKey565(SurfaceView source, SurfaceView destination, int width, int height, int colorKey, ReadOnlySpan<byte> table) => Blit.BlendColorKey565(source, d: destination, w: width, h: height, k: colorKey, t: table);

}
