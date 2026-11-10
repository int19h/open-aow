// Translated from GFXE.pas.

namespace Aow.Game.Draw;

// the Windows bitmap file format
public partial class BMPImageFile : ImageFile, IImageFileClass
{
    // GFXE.pas:9043
    public static new string Extension => "BMP";

    // GFXE.pas:9048
    public static new string Description => "Bitmap files (*.bmp)";

    // GFXE.pas:9053
    public static new int GetPixelFormatID(string FileName) => GetBMPPixelFormat(FileName);

    // GFXE.pas:9058
    public static new Surface CreateSurface(string FileName) => CreateSurfaceFromBMP(FileName);

    // GFXE.pas:9063
    public static new int LoadToSurfaceDesc(string FileName, ref SurfaceDesc SD, int X, int Y) => LoadBMP(FileName, ref SD, X, Y);

    private static int LoadBMP(string FileName, ref SurfaceDesc SD, int X, int Y)
    {
        throw new NotImplementedException();
    }

    private static Surface CreateSurfaceFromBMP(string FileName)
    {
        throw new NotImplementedException();
    }

    private static int GetBMPPixelFormat(string FileName)
    {
        throw new NotImplementedException();
    }

}
