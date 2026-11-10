using Aow.Game.Draw;
using Aow.Graphics;

namespace Aow.Game.Draw.Images;

// The assembler sprite routines of ImageLib.pas (graphics.md 5.2). The encoded image data and the
// palette are byte spans, the descriptor gives the destination view and its clip rectangle.
public static partial class ImageLibUnit
{
    public static void RemapRLE565to555(Span<byte> Data, int Height, int ColorKey) => Blit.RemapRLE565to555(Data, Height, ColorKey);

    public static void RemapRLE555to565(Span<byte> Data, int Height, int ColorKey) => Blit.RemapRLE555to565(Data, Height, ColorKey);

    public static void BltRLE16(int X, int Y, in Aow.Game.Draw.TSurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height, int ColorKey) => Blit.BltRLE16(X, Y, SD.View(), Data, Width, Height, ColorKey);

    public static void BlendRLE565(int X, int Y, in Aow.Game.Draw.TSurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height, int ColorKey) => Blit.BlendRLE565(X, Y, SD.View(), Data, Width, Height, ColorKey);

    public static void BlendRLE555(int X, int Y, in Aow.Game.Draw.TSurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height, int ColorKey) => Blit.BlendRLE555(X, Y, SD.View(), Data, Width, Height, ColorKey);

    public static void ShowBitMask16(int X, int Y, in Aow.Game.Draw.TSurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height) => Blit.ShowBitMask16(X, Y, SD.View(), Data, Width, Height);

    public static void ShowShadow565(int X, int Y, in Aow.Game.Draw.TSurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height) => Blit.ShowShadow565(X, Y, SD.View(), Data, Width, Height);

    public static void ShowShadow555(int X, int Y, in Aow.Game.Draw.TSurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height) => Blit.ShowShadow555(X, Y, SD.View(), Data, Width, Height);

    public static void ShowRLE08(int X, int Y, in Aow.Game.Draw.TSurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height, byte ColorKey) => Blit.ShowRLE08(X, Y, SD.View(), Data, Width, Height, ColorKey);

    public static void ShowRLE08to16(int X, int Y, in Aow.Game.Draw.TSurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height, byte ColorKey, ReadOnlySpan<byte> Palette) => Blit.ShowRLE08to16(X, Y, SD.View(), Data, Width, Height, ColorKey, Palette);
}
