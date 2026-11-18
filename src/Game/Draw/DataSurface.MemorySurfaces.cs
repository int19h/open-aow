
namespace Aow.Game.Draw;

public partial class DataSurface
{
    public override GraphicsResult Lock(ref LockedSurface lockedSurface)
    { lockedSurface = MemorySurfaces.Describe(_width, _height, Pitch, _pixelFormatId, Data, _palette); return GraphicsResult.Ok; }
}
