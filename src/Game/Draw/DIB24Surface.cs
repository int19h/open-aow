// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// a 24 bit DIB section. FHandle, FWidth, FHeight, FBits, FPitch, FInfo
public partial class DIB24Surface : Surface
{
    internal int FHandle;
    internal int FWidth;
    internal int FHeight;
    internal byte[] FBits;
    internal int FPitch;
    internal BitmapInfoHeader FInfo;

    // GFXE.pas:7238
    public override int GetWidth() => FWidth;

    // GFXE.pas:7243
    public override int GetHeight() => FHeight;

}
