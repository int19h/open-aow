#nullable disable

using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class Dib16Surface { private bool _memoryLocked; }

public partial class Dib16Surface
{
    public Dib16Surface(int width, int height, PixelFormat pixelFormatId)
    {
        SetPixelFormatId(pixelFormatId);
        _ = Resize(width, height);
    }

    internal void SetPixelFormatId(PixelFormat value)
    {
        if (value is not PixelFormat.Rgb555 and not PixelFormat.Rgb565 and not PixelFormat.Argb1555)
        {
            throw new NotSupportedException("Pixel format");
        }

        if (_memoryLocked && value != _pixelFormatId)
        {
            throw new InvalidOperationException("Pixel format after Lock");
        }

        _pixelFormatId = value;
    }

    public override GraphicsResult Resize(int width, int height)
    {
        if (_width == width && _height == height && _bits is not null)
        {
            return GraphicsResult.Ok;
        }

        (_pitch, _bits) = MemorySurfaces.Allocate(width, h: height, format: _pixelFormatId, alignment: 4);
        _width = width;
        _height = height;
        return GraphicsResult.Ok;
    }

    public override GraphicsResult Lock(ref LockedSurface lockedSurface)
    {
        _memoryLocked = true;
        lockedSurface = MemorySurfaces.Describe(_width, h: _height, _pitch, format: _pixelFormatId, _bits, palette: null);
        return GraphicsResult.Ok;
    }

    public override GraphicsResult Unlock() => GraphicsResult.Ok;
    public override GraphicsResult Blit(Surface destination, Rect? sourceRectangle, int X, int Y) => base.Blit(destination, sourceRectangle, X, Y);
    public override GraphicsResult Clear(int color) => base.Clear(color);
    public override int BlitDisplayController(int displayController, Rect? sourceRectangle, Rect? destinationRectangle) => throw new NotSupportedException("Native device context");

    public override void Dispose()
    {
        _bits = null;
        _width = _height = _pitch = 0;
        base.Dispose();
    }

    public int Handle => throw new NotSupportedException("Native surface handle");
}
