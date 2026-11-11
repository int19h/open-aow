
namespace Aow.Game.Draw;

public partial class DataSurface
{
    public override int Lock(ref LockedSurface SD)
    { SD = MemorySurfaces.Describe(FWidth, FHeight, FPitch, FPixelFormatID, FMem, FPalette); return PixelFormats.GFXE_OK; }
}
