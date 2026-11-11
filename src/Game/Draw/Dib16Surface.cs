// Translated from GFXE.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Game.Draw;

// a 16 bit DIB section (555 or 565). FHandle, FWidth, FHeight, FBits, FPitch, FPixelFormatID,
// FInfo, FMasks
public partial class Dib16Surface : Surface
{
    internal int FHandle;
    internal int FWidth;
    internal int FHeight;
    internal byte[] FBits;
    internal int FPitch;
    internal int FPixelFormatID;
    internal BitmapInfoHeader FInfo;
    internal Int32Array3 FMasks;

    // GFXE.pas:6991
    public override int GetWidth() => FWidth;

    // GFXE.pas:6996
    public override int GetHeight() => FHeight;

    // GFXE.pas:7001
    public override int GetPixelFormatID() => FPixelFormatID;

    public new int PixelFormatID { get => FPixelFormatID; set => SetPixelFormatID(value); }

}
