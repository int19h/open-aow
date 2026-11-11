// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// a DirectDraw surface. FOnSurfaceLost, FPixelFormatID, FWidth, FHeight, FSurface,
// FCreationFlags, FStatus, FLockCount, FLockExCount, FSurfaceDesc (the description of the lock)
public partial class DirectDrawSurface : Surface
{
    protected internal NotifyEvent<object> FOnSurfaceLost;
    protected internal int FPixelFormatID;
    protected internal int FWidth;
    protected internal int FHeight;
    protected internal Set<DirectDrawSurfaceCreationFlagsElement> FCreationFlags;
    protected internal Set<DirectDrawSurfaceStatusElement> FStatus;
    protected internal int FLockCount;
    protected internal int FLockExCount;
    protected internal LockedSurface FSurfaceDesc;

    // GFXE.pas:9279
    public override int GetPixelFormatID() => FPixelFormatID;

    // GFXE.pas:9284
    public override int GetWidth() => FWidth;

    // GFXE.pas:9289
    public override int GetHeight() => FHeight;

    public Set<DirectDrawSurfaceStatusElement> Status { get => FStatus; }
    public Set<DirectDrawSurfaceCreationFlagsElement> CreationFlags { get => FCreationFlags; }
    public NotifyEvent<object> OnSurfaceLost { get => FOnSurfaceLost; set => FOnSurfaceLost = value; }

    public const DirectDrawSurfaceStatusElement ssSystemMemory = DirectDrawSurfaceStatusElement.Created;
}
