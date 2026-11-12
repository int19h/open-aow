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
    public static int GetPixelFormatId(string fileName)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:697
    public static Surface CreateSurface(string fileName)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:698
    public static int LoadToSurfaceDescription(string fileName, ref LockedSurface lockedSurface, int X, int Y)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

}
