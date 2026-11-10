// Translated from GFXE.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Game.Draw;

// a DIB section of any format (8, 16 or 24 bit). FHandle, FWidth, FHeight, FMem, FBits, FPitch,
// FPixelFormatID, FInfo (BITMAPINFOHEADER and 256 colors), FPalette
public partial class DIBSurface : Surface
{
    internal int FHandle;
    internal int FWidth;
    internal int FHeight;
    internal byte[] FMem;
    internal byte[] FBits;
    internal int FPitch;
    internal int FPixelFormatID;
    internal BitmapInfoHeader FInfo;
    internal FixedArray_Aow_Rtl_TRGBQuad_256 FColors;
    internal GFXEPalette FPalette;

    // GFXE.pas:6407
    public override int GetWidth() => FWidth;

    // GFXE.pas:6412
    public override int GetHeight() => FHeight;

    // GFXE.pas:6767
    public void SaveToBMP24(string FileName)
    {
        SurfaceDesc SD = default;
        Lock(ref SD);
        SaveSurfaceDescToBMP24(SD, SD.ClipRect, FileName);
        Unlock();
    }

    public byte[] Bits { get => FBits; }
    public int Pitch { get => FPitch; }
    public GFXEPalette Palette { get => FPalette; }

    private static int SaveSurfaceDescToBMP24(in SurfaceDesc SD, Rect R, string FileName)
    {
        throw new NotImplementedException();
    }
}
