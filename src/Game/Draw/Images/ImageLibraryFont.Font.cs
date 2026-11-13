
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
            width += (image?.Width ?? 5) + _characterDistance;
        }
        return width;
    }

    internal void ShowText(int x, int y, string text, in LockedSurface lockedSurface)
    {
        if (RtlUnit.IsEmpty(text)) return;
        foreach (char c in text)
        {
            var image = GetSafe(Cp1252.Byte(c));
            if (image != null) image.Show(x, y, lockedSurface);
            x += (image?.Width ?? 5) + _characterDistance;
        }
    }

    internal void SetColor(int value)
    {
        if (_fontColorPaletteIndex >= 0 && PaletteManager._palettes.Count != 0)
        {
            var palette = PaletteManager._palettes[0];
            palette.Palette[_fontColorPaletteIndex] =
                new((uint)PixelFormats.Color2PixelInfo(value, palette.Palette.PixelFormatId, null));
            if (palette.RemapPaletteValue != null)
                palette.RemapPaletteValue[_fontColorPaletteIndex] =
                    new((uint)PixelFormats.Color2PixelInfo(value, palette.RemapPaletteValue.PixelFormatId, null));
        }
        _color = value;
    }
}
