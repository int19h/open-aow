// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// an image file format, all class functions
public partial class ImageFile : IImageFileClass
{
    // GFXE.pas:693
    public static string Name => throw AbstractErrorException.Create("Abstract Error");

    // GFXE.pas:694
    public static string Extension => throw AbstractErrorException.Create("Abstract Error");

    // GFXE.pas:695
    public static string Description => throw AbstractErrorException.Create("Abstract Error");

    // GFXE.pas:696
    public static int GetPixelFormatID(string FileName)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:697
    public static Surface CreateSurface(string FileName)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:698
    public static int LoadToSurfaceDesc(string FileName, ref SurfaceDesc SD, int X, int Y)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

}
