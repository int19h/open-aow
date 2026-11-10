
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

public partial class ImageLibraryFont
{
    // ImageLib.pas:6308 and 6337 store a reference index in an image variable.
    internal int TextWidth(string text)
    {
        int width = 0;
        // One table lookup per char, no allocation (strings.md 2.4). IsEmpty reads the null switch as Encode did.
        if (RtlUnit.IsEmpty(text)) return width;
        foreach (char c in text)
        {
            var image = GetSafe(Cp1252.Byte(c));
            width += (image?.Width ?? 5) + FCharacterDistance;
        }
        return width;
    }

    internal void ShowText(int x, int y, string text, in SurfaceDesc sd)
    {
        if (RtlUnit.IsEmpty(text)) return;
        foreach (char c in text)
        {
            var image = GetSafe(Cp1252.Byte(c));
            if (image != null) image.Show(x, y, sd);
            x += (image?.Width ?? 5) + FCharacterDistance;
        }
    }

    internal void SetColor(int value)
    {
        if (FFontColorPaletteIndex >= 0 && FPaletteManager.FPalettes.Count != 0)
        {
            var palette = (LibraryImagePalette)FPaletteManager.FPalettes.List[0];
            palette.FPalette.Entries[FFontColorPaletteIndex].Value =
                PixelFormats.Color2PixelInfo(value, palette.FPalette.PixelFormatID, null);
            if (palette.FRemapPalette != null)
                palette.FRemapPalette.Entries[FFontColorPaletteIndex].Value =
                    PixelFormats.Color2PixelInfo(value, palette.FRemapPalette.PixelFormatID, null);
        }
        FColor = value;
    }
}
