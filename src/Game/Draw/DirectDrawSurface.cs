// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// a DirectDraw surface. _onSurfaceLost, _pixelFormatId, _width, _height, FSurface,
// FCreationFlags, FStatus, FLockCount, FLockExCount, FSurfaceDesc (the description of the lock)
public partial class DirectDrawSurface : Surface
{
    protected internal NotifyEvent<object> _onSurfaceLost;
    protected internal int _pixelFormatId;
    protected internal int _width;
    protected internal int _height;
    protected internal Set<DirectDrawSurfaceCreationFlagsElement> _creationFlags;
    protected internal Set<DirectDrawSurfaceStatusElement> _status;
    protected internal int LockCount;
    protected internal int RectangleLockCount;
    protected internal LockedSurface LockedSurface;

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
