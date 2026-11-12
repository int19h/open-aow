// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// a DirectDraw surface. OnSurfaceLost, _pixelFormatId, _width, _height, FSurface,
// FCreationFlags, FStatus, FLockCount, FLockExCount, FSurfaceDesc (the description of the lock)
public partial class DirectDrawSurface : Surface
{
    // Nothing in the port raises OnSurfaceLost. The bound handler is kept.
    private NotifyEvent<object> _onSurfaceLost;

    public event NotifyEvent<object> OnSurfaceLost { add => _onSurfaceLost += value; remove => _onSurfaceLost -= value; }

    private int _pixelFormatId;
    private int _width;
    private int _height;
    private Set<DirectDrawSurfaceCreationFlagsElement> _creationFlags;
    private Set<DirectDrawSurfaceStatusElement> _status;

    // GFXE.pas:9279
    public override int GetPixelFormatId() => _pixelFormatId;

    // GFXE.pas:9284
    public override int GetWidth() => _width;

    // GFXE.pas:9289
    public override int GetHeight() => _height;

    public Set<DirectDrawSurfaceStatusElement> Status { get => _status; }
    public Set<DirectDrawSurfaceCreationFlagsElement> CreationFlags { get => _creationFlags; }

    public const DirectDrawSurfaceStatusElement SystemMemory = DirectDrawSurfaceStatusElement.Created;
}
