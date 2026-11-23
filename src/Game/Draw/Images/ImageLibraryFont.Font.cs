#nullable disable warnings

using Aow.Graphics;
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

public sealed partial class ImageLibraryFont
{
    // ImageLib.pas:6308 and 6337 store a reference index in an image variable.
    internal int TextWidth(string text)
    {
        int width = 0;
        // One table lookup per char, no allocation (strings.md 2.4). IsEmpty reads the null switch as Encode did.
        if (DelphiStrings.IsEmpty(text))
        {
            return width;
        }

        foreach (char c in text)
        {
            LibraryImage? image = GetSafe(Cp1252.Byte(c));
            width += (image?.Width ?? 5) + CharacterDistance;
        }
        return width;
    }

    internal void ShowText(int x, int y, string text, in LockedSurface lockedSurface)
    {
        if (DelphiStrings.IsEmpty(text))
        {
            return;
        }

        foreach (char c in text)
        {
            LibraryImage? image = GetSafe(Cp1252.Byte(c));
            image?.Show(x, y, lockedSurface);

            x += (image?.Width ?? 5) + CharacterDistance;
        }
    }

    internal void SetColor(Color value)
    {
        if (_fontColorPaletteIndex >= 0 && PaletteManager.Palettes.Count != 0)
        {
            LibraryImagePalette palette = PaletteManager.Palettes[0];
            palette.Palette[_fontColorPaletteIndex] =
                new((uint)PixelFormats.Color2PixelInfo(value, palette.Palette.PixelFormatId, palette: null));
            palette.RemapPaletteValue?[_fontColorPaletteIndex] =
                    new((uint)PixelFormats.Color2PixelInfo(value, palette.RemapPaletteValue.PixelFormatId, palette: null));
        }
        _color = value;
    }
}
