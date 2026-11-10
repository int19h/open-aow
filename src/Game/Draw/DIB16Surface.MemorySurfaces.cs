
using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class DIB16Surface { private bool memoryLocked; }

public partial class DIB16Surface
{
    public DIB16Surface(int AWidth, int AHeight, int APixelFormatID)
    { SetPixelFormatID(APixelFormatID); Resize(AWidth, AHeight); }

    internal void SetPixelFormatID(int Value) { if (Value != PixelFormats.pfRGB555 && Value != PixelFormats.pfRGB565 && Value != PixelFormats.pfARGB1555) throw new NotSupportedException("Pixel format"); if (memoryLocked && Value != FPixelFormatID) throw new InvalidOperationException("Pixel format after Lock"); FPixelFormatID = Value; }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return PixelFormats.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, FPixelFormatID, 4); FWidth = AWidth; FHeight = AHeight; return PixelFormats.GFXE_OK; }

    public override int Lock(ref SurfaceDesc SD) { memoryLocked = true; SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, FPixelFormatID, FBits, null); return PixelFormats.GFXE_OK; }

    public override int Unlock() => PixelFormats.GFXE_OK;
    public override int Blt(Surface Dest, Rect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, Rect? SrcRect, Rect? DestRect) => throw new NotSupportedException("Native device context");

    public override void Dispose() { FBits = null; FWidth = FHeight = FPitch = 0; base.Dispose(); }

    public int Handle => throw new NotSupportedException("Native surface handle");
}
