
using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class DIB32Surface
{
    public DIB32Surface(int AWidth, int AHeight)
    { Resize(AWidth, AHeight); }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return PixelFormats.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, PixelFormats.pfRGB32, 4); FWidth = AWidth; FHeight = AHeight; return PixelFormats.GFXE_OK; }

    public override int Lock(ref SurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, PixelFormats.pfRGB32, FBits, null); return PixelFormats.GFXE_OK; }

    public override int Unlock() => PixelFormats.GFXE_OK;
    public override int Blt(Surface Dest, Rect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, Rect? SrcRect, Rect? DestRect) => throw new NotSupportedException("Native device context");

    public override void Dispose() { FBits = null; FWidth = FHeight = FPitch = 0; base.Dispose(); }
}
