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

    internal int FCharacterDistance;
    internal int FHeight;
    internal EditCharCase FCharCase;
    internal int FColor;
    internal int FFontColorPaletteIndex;

    // ImageLib.pas:6266
    internal void SetFontColorPaletteIndex(int Value)
    {
        if (Value > 255)
        {
            Value = 255;
        }

        if (Value < 0)
        {
            Value = -1;
        }

        FFontColorPaletteIndex = Value;
    }

    // ImageLib.pas:6258
    public ImageLibraryFont(Component owner) : base(owner)
    {
        FCharacterDistance = 2;
        FFontColorPaletteIndex = -1;
        FCharCase = EditCharCase.Normal;
    }

    // ImageLib.pas:6291
    public override void LoadFromStream(Stream Stream)
    {
        int I;
        base.LoadFromStream(Stream);
        FHeight = 0;
        I = 0;
        while (I < FCount)
        {
            if (this[I] != null)
            {
                if (this[I].Height > FHeight)
                {
                    FHeight = this[I].Height;
                }
            }

            I += 1;
        }
    }

    // ImageLib.pas:6367
    public int GetTextWidth(string Text)
    {
        int Result;
        switch (FCharCase)
        {
            case EditCharCase.Normal:
                Result = TextWidth(Text);
                break;
            case EditCharCase.LowerCase:
                Result = TextWidth(LowerCase(Text));
                break;
            case EditCharCase.UpperCase:
                Result = TextWidth(UpperCase(Text));
                break;
            default:
                Result = 0;
                break;
        }

        return Result;
    }

    // ImageLib.pas:6378
    public int GetTextHeight(string Text)
    {
        int H;
        int C;
        string S;
        switch (FCharCase)
        {
            case EditCharCase.LowerCase:
                S = LowerCase(Text);
                break;
            case EditCharCase.UpperCase:
                S = UpperCase(Text);
                break;
            default:
                S = Text;
                break;
        }

        H = 0;
        for (int I = 1; I <= S.Length; I++)
        {
            C = Cp1252.Byte(S[I - 1]);
            if (C < FCount && this[C] != null && this[C].Height > H)
            {
                H = this[C].Height;
            }
        }

        return H;
    }

    // ImageLib.pas:6399
    public void SetText(int X, int Y, string Text, in SurfaceDesc SD)
    {
        switch (FCharCase)
        {
            case EditCharCase.Normal:
                ShowText(X, Y, Text, SD);
                break;
            case EditCharCase.LowerCase:
                ShowText(X, Y, LowerCase(Text), SD);
                break;
            case EditCharCase.UpperCase:
                ShowText(X, Y, UpperCase(Text), SD);
                break;
        }
    }

    // ImageLib.pas:6408
    public void SetTextCentered(int X, int Y, string Text, in SurfaceDesc SD)
    {
        string S;
        switch (FCharCase)
        {
            case EditCharCase.Normal:
                X = X - (int)(((uint)TextWidth(Text)) >> 1);
                ShowText(X, Y, Text, SD);
                break;
            case EditCharCase.LowerCase:
                S = LowerCase(Text);
                X = X - (int)(((uint)TextWidth(S)) >> 1);
                ShowText(X, Y, S, SD);
                break;
            case EditCharCase.UpperCase:
                S = UpperCase(Text);
                X = X - (int)(((uint)TextWidth(S)) >> 1);
                ShowText(X, Y, S, SD);
                break;
        }
    }

    // ImageLib.pas:6433
    public void SetTextEx(int X, int Y, string Text, in SurfaceDesc SD, Set<SetTextModeElement> Mode, in ShowInfo ShowInfo)
    {
        int C;
        string S;
        switch (FCharCase)
        {
            case EditCharCase.LowerCase:
                S = LowerCase(Text);
                break;
            case EditCharCase.UpperCase:
                S = UpperCase(Text);
                break;
            default:
                S = Text;
                break;
        }

        if (Mode.Contains(SetTextModeElement.Center))
        {
            X = X - (int)(((uint)TextWidth(S)) >> 1);
        }

        for (int I = 1; I <= S.Length; I++)
        {
            C = Cp1252.Byte(S[I - 1]);
            if (C < FCount && this[C] != null)
            {
                this[C].ShowEx(X, Y, SD, ShowInfo);
                X += this[C].Width + FCharacterDistance;
            }
            else
            {
                X += FCharacterDistance + 5;
            }
        }
    }

    public int Height { get => FHeight; }

    [Published]
    public int CharacterDistance { get => FCharacterDistance; set => FCharacterDistance = value; }

    [Published]
    public EditCharCase CharCase { get => FCharCase; set => FCharCase = value; }

    [Published]
    public int Color { get => FColor; set => SetColor(value); }

    [Published]
    public int FontColorPaletteIndex { get => FFontColorPaletteIndex; set => SetFontColorPaletteIndex(value); }

}
