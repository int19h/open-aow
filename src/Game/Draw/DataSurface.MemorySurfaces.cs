
namespace Aow.Game.Draw;

public partial class DataSurface
{
    public override int Lock(ref LockedSurface lockedSurface)
    { lockedSurface = MemorySurfaces.Describe(_width, _height, _pitch, _pixelFormatId, _memory, _palette); return PixelFormats.GFXE_OK; }
}
