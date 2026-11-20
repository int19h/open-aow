// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.Draw.Images;

// an image library whose image N is the character with code N. FCharacterDistance (pixels
// between two characters), FHeight (the largest image height, set by LoadFromStream),
// FCharCase, FColor, FFontColorPaletteIndex (the entry of the first palette that SetColor sets
// to Color, -1 for none). A character without an image is 5 pixels wide
public partial class ImageLibraryFont(Component owner) : CustomImageLibrary(owner)
{

    private int _color;
    private int _fontColorPaletteIndex = -1;

    // ImageLib.pas:6266
    internal void SetFontColorPaletteIndex(int value)
    {
        if (value > 255)
        {
            value = 255;
        }

        if (value < 0)
        {
            value = -1;
        }

        _fontColorPaletteIndex = value;
    }

    // ImageLib.pas:6291
    public override void LoadFromStream(Stream stream)
    {
        base.LoadFromStream(stream);
        Height = 0;
        int i = 0;
        while (i < Count)
        {
            if (this[i] is not null)
            {
                if (this[i].Height > Height)
                {
                    Height = this[i].Height;
                }
            }

            i += 1;
        }
    }

    // ImageLib.pas:6367
    public int GetTextWidth(string text)
    {
        return CharCase switch
        {
            EditCharCase.Normal => TextWidth(text),
            EditCharCase.LowerCase => TextWidth(LowerCase(text)),
            EditCharCase.UpperCase => TextWidth(UpperCase(text)),
            _ => 0,
        };
    }

    // ImageLib.pas:6378
    public int GetTextHeight(string text)
    {
        string s = CharCase switch
        {
            EditCharCase.LowerCase => LowerCase(text),
            EditCharCase.UpperCase => UpperCase(text),
            EditCharCase.Normal or _ => text,
        };
        int h = 0;
        for (int i = 1; i <= s.Length; i++)
        {
            int c = Cp1252.Byte(s[i - 1]);
            if (c < Count && this[c] is not null && this[c].Height > h)
            {
                h = this[c].Height;
            }
        }

        return h;
    }

    // ImageLib.pas:6399
    public void SetText(int X, int Y, string text, in LockedSurface lockedSurface)
    {
        switch (CharCase)
        {
            case EditCharCase.Normal:
                ShowText(X, Y, text, lockedSurface);
                break;
            case EditCharCase.LowerCase:
                ShowText(X, Y, LowerCase(text), lockedSurface);
                break;
            case EditCharCase.UpperCase:
                ShowText(X, Y, UpperCase(text), lockedSurface);
                break;
            default:
                break;
        }
    }

    // ImageLib.pas:6408
    public void SetTextCentered(int X, int Y, string text, in LockedSurface lockedSurface)
    {
        string s;
        switch (CharCase)
        {
            case EditCharCase.Normal:
                X -= (int)(((uint)TextWidth(text)) >> 1);
                ShowText(X, Y, text, lockedSurface);
                break;
            case EditCharCase.LowerCase:
                s = LowerCase(text);
                X -= (int)(((uint)TextWidth(s)) >> 1);
                ShowText(X, Y, s, lockedSurface);
                break;
            case EditCharCase.UpperCase:
                s = UpperCase(text);
                X -= (int)(((uint)TextWidth(s)) >> 1);
                ShowText(X, Y, s, lockedSurface);
                break;
            default:
                break;
        }
    }

    // ImageLib.pas:6433
    public void SetText(int X, int Y, string text, in LockedSurface lockedSurface, Set<SetTextModeElement> mode, in ShowInfo showInfo)
    {
        string s = CharCase switch
        {
            EditCharCase.LowerCase => LowerCase(text),
            EditCharCase.UpperCase => UpperCase(text),
            EditCharCase.Normal or _ => text,
        };
        if (mode.Contains(SetTextModeElement.Center))
        {
            X -= (int)(((uint)TextWidth(s)) >> 1);
        }

        for (int i = 1; i <= s.Length; i++)
        {
            int c = Cp1252.Byte(s[i - 1]);
            if (c < Count && this[c] is not null)
            {
                this[c].Show(X, Y, lockedSurface, showInfo);
                X += this[c].Width + CharacterDistance;
            }
            else
            {
                X += CharacterDistance + 5;
            }
        }
    }

    public int Height { get; private set; }

    [Published]
    public int CharacterDistance { get; set; } = 2;

    [Published]
    public EditCharCase CharCase { get; set; } = EditCharCase.Normal;

    [Published]
    public int Color { get => _color; set => SetColor(value); }

    [Published]
    public int FontColorPaletteIndex { get => _fontColorPaletteIndex; set => SetFontColorPaletteIndex(value); }

}
