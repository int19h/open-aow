
using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class Dib16Surface { private bool _memoryLocked; }

public partial class Dib16Surface
{
    public Dib16Surface(int width, int height, int pixelFormatId)
    { SetPixelFormatId(pixelFormatId); Resize(width, height); }

    internal void SetPixelFormatId(int value) { if (value != PixelFormats.pfRGB555 && value != PixelFormats.pfRGB565 && value != PixelFormats.pfARGB1555) throw new NotSupportedException("Pixel format"); if (_memoryLocked && value != _pixelFormatId) throw new InvalidOperationException("Pixel format after Lock"); _pixelFormatId = value; }

    public override int Resize(int width, int height) { if (_width == width && _height == height && _bits != null) return PixelFormats.GFXE_OK; (_pitch, _bits) = MemorySurfaces.Allocate(width, height, _pixelFormatId, 4); _width = width; _height = height; return PixelFormats.GFXE_OK; }

    public override int Lock(ref LockedSurface lockedSurface) { _memoryLocked = true; lockedSurface = MemorySurfaces.Describe(_width, _height, _pitch, _pixelFormatId, _bits, null); return PixelFormats.GFXE_OK; }

    public override int Unlock() => PixelFormats.GFXE_OK;
    public override int Blit(Surface destination, Rect? sourceRectangle, int X, int Y) => base.Blit(destination, sourceRectangle, X, Y);
    public override int Clear(int color) => base.Clear(color);
    public override int BlitDisplayController(int displayController, Rect? sourceRectangle, Rect? destinationRectangle) => throw new NotSupportedException("Native device context");

    public override void Dispose() { _bits = null; _width = _height = _pitch = 0; base.Dispose(); }

    public int Handle => throw new NotSupportedException("Native surface handle");
}
