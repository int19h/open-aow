// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// a DirectDraw surface. _onSurfaceLost, _pixelFormatId, _width, _height, FSurface,
// FCreationFlags, FStatus, FLockCount, FLockExCount, FSurfaceDesc (the description of the lock)
public partial class DirectDrawSurface : Surface
{
    private NotifyEvent<object> _onSurfaceLost;
    private int _pixelFormatId;
    private int _width;
    private int _height;
    private Set<DirectDrawSurfaceCreationFlagsElement> _creationFlags;
    private Set<DirectDrawSurfaceStatusElement> _status;

    protected internal int LockCount { get; }
    protected internal int RectangleLockCount { get; }
    protected internal LockedSurface LockedSurface { get; }

    // GFXE.pas:9279
    public override int GetPixelFormatId() => _pixelFormatId;

    // GFXE.pas:9284
    public override int GetWidth() => _width;

    // GFXE.pas:9289
    public override int GetHeight() => _height;

    public Set<DirectDrawSurfaceStatusElement> Status { get => _status; }
    public Set<DirectDrawSurfaceCreationFlagsElement> CreationFlags { get => _creationFlags; }
    public NotifyEvent<object> OnSurfaceLost { get => _onSurfaceLost; set => _onSurfaceLost = value; }

    public const DirectDrawSurfaceStatusElement SystemMemory = DirectDrawSurfaceStatusElement.Created;
}
