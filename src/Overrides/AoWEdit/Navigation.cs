using Aow.Rtl;

namespace Aow.Units.AoWEdit;

public partial class TAOWEdit
{
    // AoWEdit.pas:350. Clipboard integration has no host in this port.
    public override void CheckKeyDown(ref ushort key, TSet<TShiftState> shift)
    {
        FHandled = FRedraw;
        if (!FEditable || !FRedraw || !FValid || !FVisible) return;
        switch (key)
        {
            case 37:
                if (FCursorPos > 0) FCursorPos--;
                else if (FFirstChar > 1) FFirstChar--;
                break;
            case 39:
                if (FShownChars > FCursorPos) FCursorPos++;
                else if (FFirstChar + FShownChars < FText.Length) FFirstChar++;
                break;
            case 46:
                if (FCursorPos < FText.Length)
                {
                    FText = Aow.Rtl.Rtl.Copy(FText, 1, FCursorPos + FFirstChar - 1) +
                        Aow.Rtl.Rtl.Copy(FText, FCursorPos + FFirstChar + 1, FText.Length - FCursorPos - FFirstChar);
                    FCursorPos += FFirstChar - 1;
                    FFirstChar = 1;
                }
                break;
        }
        FShownChars = FText.Length - FFirstChar + 1;
        FCursorPos = Math.Max(FCursorPos, 0);
        FCursorShown = true;
        FBlinkCount = 0;
        FDrawn = false;
        FOnKeyDown?.Invoke(this, ref key, shift);
        FOnChange?.Invoke(this);
    }
}
