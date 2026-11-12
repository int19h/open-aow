// Translated from GFXE.pas.

namespace Aow.Game.Draw;

// the Windows bitmap file format
public partial class BmpImageFile : ImageFile, IImageFileClass
{
    // GFXE.pas:9043
    public static new string Extension => "BMP";

    // GFXE.pas:9048
    public static new string Description => "Bitmap files (*.bmp)";

    // GFXE.pas:9053
    public static new int GetPixelFormatId(string fileName) => GetBmpPixelFormat(fileName);

    // GFXE.pas:9058
    public static new Surface CreateSurface(string fileName) => CreateSurfaceFromBmp(fileName);

    // GFXE.pas:9063
    public static new int LoadToSurfaceDescription(string fileName, ref LockedSurface lockedSurface, int X, int Y) => LoadBmp(fileName, ref lockedSurface, X, Y);

    private static int LoadBmp(string fileName, ref LockedSurface lockedSurface, int X, int Y)
    {
        throw new NotImplementedException();
    }

    private static Surface CreateSurfaceFromBmp(string fileName)
    {
        throw new NotImplementedException();
    }

    private static int GetBmpPixelFormat(string fileName)
    {
        throw new NotImplementedException();
    }

}
