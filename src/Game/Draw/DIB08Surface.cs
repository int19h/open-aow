// Translated from GFXE.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Game.Draw;

// an 8 bit bitmap in memory (bottom-up: the pitch is negative). FWidth, FHeight, FMem, FBits,
// FPitch, FInfo (header and 256 colors), FPalette
public partial class DIB08Surface : Surface
{
    internal int FWidth;
    internal int FHeight;
    internal byte[] FMem;
    internal byte[] FBits;
    internal int FPitch;
    internal BitmapInfoHeader FInfo;
    internal FixedArray_Aow_Rtl_TRGBQuad_256 FColors;
    internal GFXEPalette FPalette;

    // GFXE.pas:6805
    public override int GetPixelFormatID() => PixelFormats.pfPAL8;

    // GFXE.pas:6810
    public override int GetWidth() => FWidth;

    // GFXE.pas:6815
    public override int GetHeight() => FHeight;

    public GFXEPalette Palette { get => FPalette; set => SetPalette(value); }

}
