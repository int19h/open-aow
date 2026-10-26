using Aow.Rtl;
using Aow.Units.GFXE;

namespace Aow.Units.ImageLib;

public static partial class ImageLib
{
    // Ilpack/ImageLib.pas:3202. Milestone 3 supplies the pixel implementation.
    public static void RemapRLE565to555(object Data, int Height, int ColorKey) => throw new NotImplementedException("ImageLib.RemapRLE565to555");
    // Ilpack/ImageLib.pas:3256. Milestone 3 supplies the pixel implementation.
    public static void RemapRLE555to565(object Data, int Height, int ColorKey) => throw new NotImplementedException("ImageLib.RemapRLE555to565");
    // Ilpack/ImageLib.pas:3310. Milestone 3 supplies the pixel implementation.
    public static void BltRLE16(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height, int ColorKey) => throw new NotImplementedException("ImageLib.BltRLE16");
    // Ilpack/ImageLib.pas:3615. Milestone 3 supplies the pixel implementation.
    public static void BlendRLE565(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height, int ColorKey) => throw new NotImplementedException("ImageLib.BlendRLE565");
    // Ilpack/ImageLib.pas:3930. Milestone 3 supplies the pixel implementation.
    public static void BlendRLE555(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height, int ColorKey) => throw new NotImplementedException("ImageLib.BlendRLE555");
    // Ilpack/ImageLib.pas:4870. Milestone 3 supplies the pixel implementation.
    public static void ShowBitMask16(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height) => throw new NotImplementedException("ImageLib.ShowBitMask16");
    // Ilpack/ImageLib.pas:5150. Milestone 3 supplies the pixel implementation.
    public static void ShowShadow565(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height) => throw new NotImplementedException("ImageLib.ShowShadow565");
    // Ilpack/ImageLib.pas:5462. Milestone 3 supplies the pixel implementation.
    public static void ShowShadow555(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height) => throw new NotImplementedException("ImageLib.ShowShadow555");
    // Ilpack/ImageLib.pas:7771. Milestone 3 supplies the pixel implementation.
    public static void ShowRLE08(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height, byte ColorKey) => throw new NotImplementedException("ImageLib.ShowRLE08");
    // Ilpack/ImageLib.pas:7830. Milestone 3 supplies the pixel implementation.
    public static void ShowRLE08to16(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height, byte ColorKey, object Palette) => throw new NotImplementedException("ImageLib.ShowRLE08to16");
}
