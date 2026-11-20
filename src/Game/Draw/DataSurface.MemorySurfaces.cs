
namespace Aow.Game.Draw;

public sealed partial class DataSurface
{
    public override GraphicsResult Lock(ref LockedSurface lockedSurface)
    {
        lockedSurface = MemorySurfaces.Describe(_width, h: _height, Pitch, format: _pixelFormatId, bits: Data, _palette);
        return GraphicsResult.Ok;
    }
}
