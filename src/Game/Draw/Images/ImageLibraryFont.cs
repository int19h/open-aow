// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// an image library whose image N is the character with code N. FCharacterDistance (pixels
// between two characters), FHeight (the largest image height, set by LoadFromStream),
// FCharCase, FColor, FFontColorPaletteIndex (the entry of the first palette that SetColor sets
// to Color, -1 for none). A character without an image is 5 pixels wide
public partial class ImageLibraryFont : CustomImageLibrary
{

    private int _characterDistance;
    private int _height;
    private EditCharCase _charCase;
    private int _color;
    private int _fontColorPaletteIndex;

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

    // ImageLib.pas:6258
    public ImageLibraryFont(Component owner) : base(owner)
    {
        _characterDistance = 2;
        _fontColorPaletteIndex = -1;
        _charCase = EditCharCase.Normal;
    }

    // ImageLib.pas:6291
    public override void LoadFromStream(Stream stream)
    {
        int I;
        base.LoadFromStream(stream);
        _height = 0;
        I = 0;
        while (I < Count)
        {
            if (this[I] != null)
            {
                if (this[I].Height > _height)
                {
                    _height = this[I].Height;
                }
            }

            I += 1;
        }
    }

    // ImageLib.pas:6367
    public int GetTextWidth(string text)
    {
        int result;
        switch (_charCase)
        {
            case EditCharCase.Normal:
                result = TextWidth(text);
                break;
            case EditCharCase.LowerCase:
                result = TextWidth(LowerCase(text));
                break;
            case EditCharCase.UpperCase:
                result = TextWidth(UpperCase(text));
                break;
            default:
                result = 0;
                break;
        }

        return result;
    }

    // ImageLib.pas:6378
    public int GetTextHeight(string text)
    {
        int H;
        int C;
        string S;
        switch (_charCase)
        {
            case EditCharCase.LowerCase:
                S = LowerCase(text);
                break;
            case EditCharCase.UpperCase:
                S = UpperCase(text);
                break;
            default:
                S = text;
                break;
        }

        H = 0;
        for (int I = 1; I <= S.Length; I++)
        {
            C = Cp1252.Byte(S[I - 1]);
            if (C < Count && this[C] != null && this[C].Height > H)
            {
                H = this[C].Height;
            }
        }

        return H;
    }

    // ImageLib.pas:6399
    public void SetText(int X, int Y, string text, in LockedSurface lockedSurface)
    {
        switch (_charCase)
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
        }
    }

    // ImageLib.pas:6408
    public void SetTextCentered(int X, int Y, string text, in LockedSurface lockedSurface)
    {
        string S;
        switch (_charCase)
        {
            case EditCharCase.Normal:
                X = X - (int)(((uint)TextWidth(text)) >> 1);
                ShowText(X, Y, text, lockedSurface);
                break;
            case EditCharCase.LowerCase:
                S = LowerCase(text);
                X = X - (int)(((uint)TextWidth(S)) >> 1);
                ShowText(X, Y, S, lockedSurface);
                break;
            case EditCharCase.UpperCase:
                S = UpperCase(text);
                X = X - (int)(((uint)TextWidth(S)) >> 1);
                ShowText(X, Y, S, lockedSurface);
                break;
        }
    }

    // ImageLib.pas:6433
    public void SetText(int X, int Y, string text, in LockedSurface lockedSurface, Set<SetTextModeElement> mode, in ShowInfo showInfo)
    {
        int C;
        string S;
        switch (_charCase)
        {
            case EditCharCase.LowerCase:
                S = LowerCase(text);
                break;
            case EditCharCase.UpperCase:
                S = UpperCase(text);
                break;
            default:
                S = text;
                break;
        }

        if (mode.Contains(SetTextModeElement.Center))
        {
            X = X - (int)(((uint)TextWidth(S)) >> 1);
        }

        for (int I = 1; I <= S.Length; I++)
        {
            C = Cp1252.Byte(S[I - 1]);
            if (C < Count && this[C] != null)
            {
                this[C].Show(X, Y, lockedSurface, showInfo);
                X += this[C].Width + _characterDistance;
            }
            else
            {
                X += _characterDistance + 5;
            }
        }
    }

    public int Height { get => _height; }

    [Published]
    public int CharacterDistance { get => _characterDistance; set => _characterDistance = value; }

    [Published]
    public EditCharCase CharCase { get => _charCase; set => _charCase = value; }

    [Published]
    public int Color { get => _color; set => SetColor(value); }

    [Published]
    public int FontColorPaletteIndex { get => _fontColorPaletteIndex; set => SetFontColorPaletteIndex(value); }

}
