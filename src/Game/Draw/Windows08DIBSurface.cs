// Translated from DisplayC.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// an 8 bit bottom-up DIB of a TWindows08DC, drawn with the bitmap info and palette of its
// owner. FWidth, FHeight, FMem, FBits (FMem aligned to 8), FPitch (negative), FOwner. The port's
// FMem is the pixel array and FBits, its first byte, has no field of its own (graphics.md 3.4).
public partial class Windows08DIBSurface : Surface
{
    internal int FWidth;
    internal int FHeight;
    internal byte[] FMem;
    internal int FPitch;
    internal Windows08DC FOwner;

    // DisplayC.pas:2011
    public Windows08DIBSurface(int AWidth, int AHeight, Windows08DC owner)
    {
        FOwner = owner;
        Resize(AWidth, AHeight);
    }

    // DisplayC.pas:2018
    public override void Dispose()
    {
        FMem = null;
        base.Dispose();
    }

    // DisplayC.pas:2024
    public override int GetWidth() => FWidth;

    // DisplayC.pas:2029
    public override int GetHeight() => FHeight;

    // DisplayC.pas:2036
    // { Copies SrcRect to the top left of DestRect (no stretching); a GDI surface that cannot be
    //   locked is drawn on with BltDC. }
    public override int BltStretch(Surface Dest, Rect? SrcRect, Rect? DestRect)
    {
        /* TODO: Implement Windows.GetDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:2078
    public override int BltDC(int DC, Rect? SrcRect, Rect? DestRect)
    {
        /* TODO: Implement Windows.GetClientRect in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:2104
    public override int Lock(ref SurfaceDesc SD)
    {
        SD = default;
        SD.Width = FWidth;
        SD.Height = FHeight;
        SD.BitCount = 8;
        SD.PixelFormatID = PixelFormats.pfPAL8;
        SD.ClipRect.Left = 0;
        SD.ClipRect.Right = FWidth;
        SD.ClipRect.Top = 0;
        SD.ClipRect.Bottom = FHeight;
        SD.Pitch = FPitch;
        SD.Palette = FOwner.FPalette;
        SD.Data = FMem;
        SD.DataIndex = Math.Abs(FHeight * FPitch) + FPitch;
        return 0;
    }

    // DisplayC.pas:2121
    public override int Unlock() => 0;

    // DisplayC.pas:2126
    public override int UpdatePalette()
    {
        FOwner.UpdatePalette();
        return 0;
    }

    // DisplayC.pas:2134
    // { Bug kept from the original: fills from the field FBits instead of the bits (with a negative
    //   count, so nothing happens). }
    public override int Cls(int Color)
    {
        FOwner.FPalette.GetNearestColor(Color);
        /* TODO: Implement the pointer byte image. */
        throw new NotImplementedException();
        return 0;
    }

    // DisplayC.pas:2143
    public override int Resize(int AWidth, int AHeight)
    {
        int Result;
        if (AWidth != FWidth || AHeight != FHeight)
        {
            FWidth = AWidth;
            FHeight = AHeight;
            FPitch = -((FWidth + 7) & ~7);
            ref var bitmapInfoHeader = ref FOwner.FBitmapInfo;
            bitmapInfoHeader.biWidth = Math.Abs(FPitch);
            bitmapInfoHeader.biHeight = FHeight;

            FMem = MemorySurfaces.Reallocate(FMem, Math.Abs(FPitch * FHeight));
            Result = 0;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

}
