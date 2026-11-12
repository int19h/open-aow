
namespace Aow.Game.Draw;

public interface IImageFileClass
{
    static abstract string Name { get; }
    static abstract string Extension { get; }
    static abstract string Description { get; }

    static abstract int GetPixelFormatId(string fileName);
    static abstract Surface CreateSurface(string fileName);
    static abstract int LoadToSurfaceDescription(string fileName, ref LockedSurface surface, int x, int y);
}
