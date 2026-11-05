using Aow.Units.GFXE;

namespace Aow.Units.ImageLib;

public partial class TPicture08
{
    public override void ShowOpaque(int x, int y, in TSurfaceDesc sd)
    {
        int destX = x + FXshift, destY = y + FYshift;
        int right = destX + FDataWidth, bottom = destY + FDataHeight;
        int width = FDataWidth, height = FDataHeight, srcX = 0, srcY = 0;
        // Preserve the source offsets and byte destination offset of ImageLib.pas:7424.
        if (destX < sd.ClipRect.Left)
        {
            srcX = sd.ClipRect.Left - x;
            destX = sd.ClipRect.Left;
            width -= srcX;
        }
        if (right > sd.ClipRect.Right) width -= right - sd.ClipRect.Right;
        if (width <= 0) return;
        if (destY < sd.ClipRect.Top)
        {
            srcY = sd.ClipRect.Top - y;
            destY = sd.ClipRect.Top;
            height -= srcY;
        }
        if (bottom > sd.ClipRect.Bottom) height -= bottom - sd.ClipRect.Bottom;
        if (height <= 0) return;

        bool loadedHere = !FDataLoaded;
        if (loadedHere) LoadDiscardableImageData();
        var source = (Rtl.Rtl.BytePointer(FBits) + srcX + srcY * FPitch).Box();
        var destination = (Rtl.Rtl.BytePointer(sd.Data) + destX + destY * sd.Pitch).Box();
        if (sd.PixelFormatID == GFXE.GFXE.pfPAL8)
            GFXE.GFXE.BltRect(source, destination, FPitch, sd.Pitch, width, height);
        else
        {
            if (sd.PixelFormatID != FRemapPalette.PixelFormatID &&
                RemapToPixelFormat(sd.PixelFormatID) != GFXE.GFXE.GFXE_OK)
                throw new Rtl.Exception("Surface pixel format not supported!");
            if (sd.PixelFormatID == GFXE.GFXE.pfRGB555)
                GFXE.GFXE.RemapSurface08to555(source, destination, FPitch, sd.Pitch, width, height, FRemapPalette);
            else if (sd.PixelFormatID == GFXE.GFXE.pfRGB565)
                GFXE.GFXE.RemapSurface08to565(source, destination, FPitch, sd.Pitch, width, height, FRemapPalette);
        }
        if (loadedHere && FLoadMode == TLoadMode.lmLoadFreeOnDemand) ReleaseDiscardableImageData();
    }
}
