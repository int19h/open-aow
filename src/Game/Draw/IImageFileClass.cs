
namespace Aow.Game.Draw;

public interface IImageFileClass
{
    static abstract string Name { get; }
    static abstract string Extension { get; }
    static abstract string Description { get; }

    static abstract PixelFormat GetPixelFormatId(string fileName);
    static abstract Surface CreateSurface(string fileName);
    static abstract GraphicsResult LoadToSurfaceDescription(string fileName, ref readonly LockedSurface surface, int x, int y);
}
