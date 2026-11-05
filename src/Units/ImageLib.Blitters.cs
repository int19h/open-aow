using Aow.Rtl;
using Aow.Units.GFXE;

namespace Aow.Units.ImageLib;

public static partial class ImageLibUnit
{
    public static void RemapRLE565to555(object Data, int Height, int ColorKey) => Graphics.Blit.RemapRLE565to555(RtlUnit.BytePointer(Data), Height, ColorKey);
    public static void RemapRLE555to565(object Data, int Height, int ColorKey) => Graphics.Blit.RemapRLE555to565(RtlUnit.BytePointer(Data), Height, ColorKey);
    public static void BltRLE16(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height, int ColorKey) => Graphics.Blit.BltRLE16(X, Y, SD, RtlUnit.BytePointer(Data), Width, Height, ColorKey);
    public static void BlendRLE565(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height, int ColorKey) => Graphics.Blit.BlendRLE565(X, Y, SD, RtlUnit.BytePointer(Data), Width, Height, ColorKey);
    public static void BlendRLE555(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height, int ColorKey) => Graphics.Blit.BlendRLE555(X, Y, SD, RtlUnit.BytePointer(Data), Width, Height, ColorKey);
    public static void ShowBitMask16(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height) => Graphics.Blit.ShowBitMask16(X, Y, SD, RtlUnit.BytePointer(Data), Width, Height);
    public static void ShowShadow565(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height) => Graphics.Blit.ShowShadow565(X, Y, SD, RtlUnit.BytePointer(Data), Width, Height);
    public static void ShowShadow555(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height) => Graphics.Blit.ShowShadow555(X, Y, SD, RtlUnit.BytePointer(Data), Width, Height);
    public static void ShowRLE08(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height, byte ColorKey) => Graphics.Blit.ShowRLE08(X, Y, SD, RtlUnit.BytePointer(Data), Width, Height, ColorKey);
    public static void ShowRLE08to16(int X, int Y, in TSurfaceDesc SD, object Data, int Width, int Height, byte ColorKey, object Palette) => Graphics.Blit.ShowRLE08to16(X, Y, SD, RtlUnit.BytePointer(Data), Width, Height, ColorKey, RtlUnit.BytePointer(Palette));
}
