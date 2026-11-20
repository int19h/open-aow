// Translated from GFXE.pas.

namespace Aow.Game.Draw;

// the Windows bitmap file format
public class BmpImageFile : ImageFile, IImageFileClass
{
    // GFXE.pas:9043
    public static new string Extension => "BMP";

    // GFXE.pas:9048
    public static new string Description => "Bitmap files (*.bmp)";

    // GFXE.pas:9053
    public static new PixelFormat GetPixelFormatId(string fileName) => GetBmpPixelFormat(fileName);

    // GFXE.pas:9058
    public static new Surface CreateSurface(string fileName) => CreateSurfaceFromBmp(fileName);

    // GFXE.pas:9063
    public static new GraphicsResult LoadToSurfaceDescription(string fileName, ref readonly LockedSurface lockedSurface, int X, int Y) => LoadBmp(fileName, in lockedSurface, X, Y);

    private static GraphicsResult LoadBmp(string fileName, ref readonly LockedSurface lockedSurface, int X, int Y) => throw new NotImplementedException();

    private static Surface CreateSurfaceFromBmp(string fileName) => throw new NotImplementedException();

    private static PixelFormat GetBmpPixelFormat(string fileName) => throw new NotImplementedException();

}
