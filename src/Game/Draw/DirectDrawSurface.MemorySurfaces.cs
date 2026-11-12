
using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class DirectDrawSurface
{
    private int _memoryPitch;
    private byte[] _memoryBits;

    public int CreateSurface(int width, int height, int pixelFormatId, Set<DirectDrawSurfaceCreationFlagsElement> flags) { if (_pixelFormatId != pixelFormatId) _memoryBits = null; _pixelFormatId = pixelFormatId; _creationFlags = flags; _status = Set<DirectDrawSurfaceStatusElement>.Of(SystemMemory); return Resize(width, height); }

    public override int Resize(int width, int height) { if (_width == width && _height == height && _memoryBits != null) return PixelFormats.DD_OK; (_memoryPitch, _memoryBits) = MemorySurfaces.Allocate(width, height, _pixelFormatId, 8); _width = width; _height = height; return PixelFormats.DD_OK; }

    public override int Lock(ref LockedSurface lockedSurface) { lockedSurface = MemorySurfaces.Describe(_width, _height, _memoryPitch, _pixelFormatId, _memoryBits); return PixelFormats.DD_OK; }

    public override int Blit(Surface destination, Rect? sourceRectangle, int X, int Y) => base.Blit(destination, sourceRectangle, X, Y);
    public override int BlitColorKey(Surface destination, Rect? sourceRectangle, int X, int Y, int colorKey) => base.BlitColorKey(destination, sourceRectangle, X, Y, colorKey);
    public override int Clear(int color) => base.Clear(color);
    public int AsyncClear(int color) => Clear(color);
    public bool Lost() => false;
    public int Restore() => PixelFormats.DD_OK;
    public override int Unlock() => PixelFormats.DD_OK;
    public override int Lock(ref LockedSurface lockedSurface, in Rect R) => base.Lock(ref lockedSurface, R);
    public override int Unlock(in LockedSurface lockedSurface) => Unlock();

    public override void Dispose() { _memoryBits = null; _width = _height = 0; base.Dispose(); }
}
