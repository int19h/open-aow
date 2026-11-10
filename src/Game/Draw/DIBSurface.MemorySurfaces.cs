
using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class DIBSurface
{
    public int Setup(int AWidth, int AHeight, int APixelFormatID, GFXEPalette APalette)
    {
        Clear(); FPixelFormatID = APixelFormatID;
        if (APalette != null || (APixelFormatID & 255) == 8)
        { FPalette = new GFXEPalette(); if (APalette != null) FPalette.Assign(APalette); }
        return Resize(AWidth, AHeight);
    }

    public void Clear() { FWidth = FHeight = FPitch = 0; FBits = null; FMem = null; FPalette = null; }

    public override int Resize(int AWidth, int AHeight) { if (FWidth == AWidth && FHeight == AHeight && FBits != null) return PixelFormats.GFXE_OK; (FPitch, FBits) = MemorySurfaces.Allocate(AWidth, AHeight, FPixelFormatID, 4); FWidth = AWidth; FHeight = AHeight; return PixelFormats.GFXE_OK; }

    public override int Lock(ref SurfaceDesc SD) { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, FPixelFormatID, FBits, FPalette); return PixelFormats.GFXE_OK; }

    public override int Unlock() => PixelFormats.GFXE_OK;
    public override int Blt(Surface Dest, Rect? SrcRect, int X, int Y) => base.Blt(Dest, SrcRect, X, Y);
    public override int Cls(int Color) => base.Cls(Color);
    public override int BltDC(int DC, Rect? SrcRect, Rect? DestRect) => throw new NotSupportedException("Native device context");

    public override void Dispose() { FBits = null; FWidth = FHeight = FPitch = 0; base.Dispose(); }

    public override int UpdatePalette() => FPalette != null && FPixelFormatID == PixelFormats.pfPAL8 ? PixelFormats.GFXE_OK : PixelFormats.GFXE_NOTSUPPORTED;

    public int Handle => throw new NotSupportedException("Native surface handle");
}
