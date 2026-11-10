// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// a DirectDraw surface. FOnSurfaceLost, FPixelFormatID, FWidth, FHeight, FSurface,
// FCreationFlags, FStatus, FLockCount, FLockExCount, FSurfaceDesc (the description of the lock)
public partial class DDrawSurface : Surface
{
    protected internal NotifyEvent<object> FOnSurfaceLost;
    protected internal int FPixelFormatID;
    protected internal int FWidth;
    protected internal int FHeight;
    protected internal Set<DDSCreationFlagsElement> FCreationFlags;
    protected internal Set<DDrawSurfaceStatusElement> FStatus;
    protected internal int FLockCount;
    protected internal int FLockExCount;
    protected internal SurfaceDesc FSurfaceDesc;

    // GFXE.pas:9279
    public override int GetPixelFormatID() => FPixelFormatID;

    // GFXE.pas:9284
    public override int GetWidth() => FWidth;

    // GFXE.pas:9289
    public override int GetHeight() => FHeight;

    public Set<DDrawSurfaceStatusElement> Status { get => FStatus; }
    public Set<DDSCreationFlagsElement> CreationFlags { get => FCreationFlags; }
    public NotifyEvent<object> OnSurfaceLost { get => FOnSurfaceLost; set => FOnSurfaceLost = value; }

    public const DDrawSurfaceStatusElement ssSystemMemory = DDrawSurfaceStatusElement.Created;
}
