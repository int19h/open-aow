// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// a window drawn through GDI. FHandle. Two more virtuals: GetHandle, SetHandle
public partial class GDISurface : Surface
{

    internal int FHandle;

    // GFXE.pas:12293
    public GDISurface(int AHandle)
    {
        FHandle = AHandle;
    }

    // GFXE.pas:12299
    public virtual int GetHandle() => FHandle;

    // GFXE.pas:12304
    public virtual void SetHandle(int Value)
    {
        FHandle = Value;
    }

    // GFXE.pas:12309
    public override int GetWidth()
    {
        /* TODO: Implement Windows.GetClientRect in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12319
    public override int GetHeight()
    {
        /* TODO: Implement Windows.GetClientRect in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12329
    public override int Blt(Surface Dest, Rect? SrcRect, int X, int Y) => PixelFormats.GFXE_NOTSUPPORTED;

    // GFXE.pas:12334
    public override int BltDC(int DC, Rect? SrcRect, Rect? DestRect)
    {
        /* TODO: Implement Windows.GetDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12360
    public override int Resize(int AWidth, int AHeight)
    {
        /* TODO: Implement Windows.SetWindowPos in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:12368
    public override int Cls(int Color)
    {
        /* TODO: Implement Windows.GetDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    public int Handle { get => GetHandle(); set => SetHandle(value); }

}
