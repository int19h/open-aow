using Aow.Rtl;
using Aow.Units.GFXE;

namespace Aow.Units.ImageLib;

public partial class TImageLibraryFont
{
    // ImageLib.pas:6308 and 6337 store a reference index in an image variable.
    internal int TextWidth(AnsiString text)
    {
        int width = 0;
        foreach (byte character in Aow.Rtl.Rtl.StringBytes(text))
        {
            var image = GetSafe(character);
            width += (image?.Width ?? 5) + FCharacterDistance;
        }
        return width;
    }

    internal void ShowText(int x, int y, AnsiString text, in TSurfaceDesc sd)
    {
        foreach (byte character in Aow.Rtl.Rtl.StringBytes(text))
        {
            var image = GetSafe(character);
            if (image != null) image.Show(x, y, sd);
            x += (image?.Width ?? 5) + FCharacterDistance;
        }
    }

    internal void SetColor(int value)
    {
        if (FFontColorPaletteIndex >= 0 && FPaletteManager.FPalettes.Count != 0)
        {
            var palette = (TLibraryImagePalette)FPaletteManager.FPalettes.List[0];
            palette.FPalette.Entries[FFontColorPaletteIndex].Value =
                GFXE.GFXE.Color2PixelInfo(value, palette.FPalette.PixelFormatID, null);
            if (palette.FRemapPalette != null)
                palette.FRemapPalette.Entries[FFontColorPaletteIndex].Value =
                    GFXE.GFXE.Color2PixelInfo(value, palette.FRemapPalette.PixelFormatID, null);
        }
        FColor = value;
    }
}
