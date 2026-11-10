// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// a 32 bit bitmap in memory (bottom-up). FWidth, FHeight, FMem, FBits, FPitch, FInfo
public partial class DIB32Surface : Surface
{
    internal int FWidth;
    internal int FHeight;
    internal byte[] FMem;
    internal byte[] FBits;
    internal int FPitch;
    internal BitmapInfoHeader FInfo;

    // GFXE.pas:7446
    public override int GetWidth() => FWidth;

    // GFXE.pas:7451
    public override int GetHeight() => FHeight;

}
