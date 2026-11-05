// Translated from AoWEdit.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.AoWControl;
using Aow.Units.AOWInterfaceImgLib;

namespace Aow.Units.AoWEdit;

// { The AoWEdit unit of aowInt.dpl: TAOWEdit, a one-line edit box drawn from an edit library (a
//   horizontal line of three images) with its text in AOWFont. It scrolls the text so that the
//   part after FFirstChar fits, shows a blinking cursor character while it has the input focus
//   (FRedraw), takes the focus on a click and loses it on a click outside, and handles the
//   cursor keys, Delete, Backspace, Return and Ctrl+V (paste the clipboard text). }
// Fields: FMaxChars (0: no limit), FCursorChar, FPassWordChar (#0: show the text), FCursorShown
// (the blink state of the cursor), FBlinkDelay (updates between two blinks), FBlinkCount,
// FShownChars (the characters from FFirstChar to the end), FFirstChar (the first character
// shown), FEditable, FILEdit, FOnExit, FOnReturn, FILIndex, FCursorPos (the cursor position
// counted from FFirstChar). FRedraw is the input focus
public partial class TAOWEdit : TAOWControl
{
    public int FMaxChars;
    public byte FCursorChar;
    public byte FPassWordChar;
    public bool FCursorShown;
    public int FBlinkDelay;
    public int FBlinkCount;
    public int FShownChars;
    public int FFirstChar;
    public bool FEditable;
    public TInterfaceIL FILEdit;
    public TNotifyEvent FOnExit;
    public TNotifyEvent FOnReturn;
    public int FILIndex;
    public int FCursorPos;
    // AoWEdit.pas:79
    public TAOWEdit(TComponent owner) : base(owner)
    {
        FCursorChar = 124;
        FCursorShown = true;
        FEditable = true;
    }

    // AoWEdit.pas:87
    public override void Validate()
    {
        if (FILEdit != null)
        {
            FILEdit = FILEdit.CheckIL(FILIndex, 3, this);
        }

        if (FILEdit != null && FAOWFont != null)
        {
            FValid = true;
        }

        base.Validate();
    }

    // AoWEdit.pas:96
    public override void SetSize()
    {
        var ilEditWidth = FILEdit[FILIndex];
        var ilEdit = FILEdit[FILIndex + 2];
        if (ilEditWidth.Width + ilEdit.Width > FWinWidth)
        {
            var ilEdit2 = FILEdit[FILIndex];
            var ilEdit3 = FILEdit[FILIndex + 2];
            FWinWidth = ilEdit2.Width + ilEdit3.Width;
        }

        FWinHeight = FILEdit[FILIndex].Height;
        FFirstChar = 1;
        if (FText.Length < FCursorPos)
        {
            FCursorPos = FText.Length;
        }

        if (FAOWFont.GetTextWidth(Copy(FText, FFirstChar, FText.Length - FFirstChar)) > FWinWidth - 2 * FBorderWidth - 1)
        {
            while (FAOWFont.GetTextWidth(Copy(FText, FFirstChar, FText.Length)) > FWinWidth - 2 * FBorderWidth - 1)
            {
                FFirstChar += 1;
            }
        }

        FShownChars = FText.Length - FFirstChar + 1;
        base.SetSize();
    }

    // AoWEdit.pas:137
    public override void Update()
    {
        base.Update();
        if (FVisible && FEditable && FBlinkDelay > 0 && FRedraw == true)
        {
            FBlinkCount += 1;
            if (FBlinkCount > FBlinkDelay)
            {
                FDrawn = false;
                FBlinkCount = 0;
                if (FCursorShown == true)
                {
                    FCursorShown = false;
                }
                else
                {
                    FCursorShown = true;
                }
            }
        }
    }

    // AoWEdit.pas:155
    public override void Draw(TSurfaceDesc SurfDesc)
    {
        string S = "";
        TRect OldClip = default;
        try
        {
            if (FValid && FVisible)
            {
                if (FOnPreDraw != null)
                {
                    FOnPreDraw(this, SurfDesc);
                }

                var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                FILEdit.DrawILLh(FILIndex, rect, ref SurfDesc);
                OldClip = SurfDesc.ClipRect;
                var ilEditWidth = FILEdit[FILIndex + 2];
                if (FAOWWindow.FWinLeft + FWinLeft + FWinWidth - ilEditWidth.Width < OldClip.Right)
                {
                    var ilEdit = FILEdit[FILIndex + 2];
                    SurfDesc.ClipRect.Right = FAOWWindow.FWinLeft + FWinLeft + FWinWidth - ilEdit.Width;
                }

                FAOWFont.Color = FColor;
                if (FPassWordChar == 0)
                {
                    FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FBorderWidth, FAOWWindow.FWinTop + FWinTop + FBorderHeight, Copy(FText, FFirstChar, FShownChars), SurfDesc);
                }
                else
                {
                    S = "";
                    for (int I = 0, last = FShownChars - 1; I <= last; I++)
                    {
                        S = S + Cp1252.Text(FPassWordChar);
                    }

                    FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FBorderWidth, FAOWWindow.FWinTop + FWinTop + FBorderHeight, S, SurfDesc);
                }

                SurfDesc.ClipRect = OldClip;
                if (FEditable && FRedraw == true && FCursorShown == true)
                {
                    var textWidth = FAOWFont.GetTextWidth(Copy(FText, FFirstChar, FCursorPos));
                    FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FBorderWidth + textWidth, FAOWWindow.FWinTop + FWinTop + FBorderHeight, Cp1252.Text(FCursorChar), SurfDesc);
                }

                if (FOnDraw != null)
                {
                    FOnDraw(this, SurfDesc);
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AoWEdit.pas:197
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        int W = default;
        int I = default;
        bool OldRedraw;
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        FMouseDown = false;
        OldRedraw = FRedraw;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FValid == true && FVisible == true)
            {
                if (Button == TMouseButton.mbLeft)
                {
                    if (FEditable == true)
                    {
                        FRedraw = true;
                    }

                    var textWidth = FAOWFont.GetTextWidth(Copy(FText, FFirstChar, FText.Length));
                    if (X > FAOWWindow.FWinLeft + FWinLeft + FBorderWidth + textWidth)
                    {
                        FFirstChar = 1;
                        if (FAOWFont.GetTextWidth(Copy(FText, FFirstChar, FText.Length - FFirstChar)) > FWinWidth - 2 * FBorderWidth - 1)
                        {
                            while (FAOWFont.GetTextWidth(Copy(FText, FFirstChar, FText.Length)) > FWinWidth - 2 * FBorderWidth - 1)
                            {
                                FFirstChar += 1;
                            }
                        }

                        FCursorPos = FText.Length - FFirstChar + 1;
                    }
                    else
                    {
                        I = 0;
                        W = FAOWFont.GetTextWidth(Copy(FText, FFirstChar, I + 1));
                        while (X > FAOWWindow.FWinLeft + FWinLeft + FBorderWidth + W && FText.Length > I + FFirstChar - 1)
                        {
                            I += 1;
                            W = FAOWFont.GetTextWidth(Copy(FText, FFirstChar, I + 1));
                        }

                        FCursorPos = I;
                    }

                    FCursorShown = FEditable;
                    FBlinkCount = 0;
                    FDrawn = false;
                }

                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }
        else
        {
            if (FCursorShown)
            {
                FCursorShown = false;
                if (FOnExit != null)
                {
                    FOnExit(this);
                }
            }

            FRedraw = false;
        }

        if (FRedraw != OldRedraw)
        {
            FDrawn = false;
        }
    }

    // AoWEdit.pas:261
    public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWEdit.pas:273
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        base.CheckMouseMove(Shift, X, Y);
        FHandled = false;
        if (FVisible && X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FEditable == true)
            {
                FAOWWindow.FCursor = -4;
            }

            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }
    }

    // AoWEdit.pas:289
    public override void CheckKeyPress(ref byte Key)
    {
        byte C = default;
        bool Scroll;
        FHandled = FRedraw;
        Scroll = true;
        if (FEditable && FRedraw == true && FValid == true && FVisible == true)
        {
            FCursorShown = true;
            switch (Key)
            {
                case 8:
                    if (FCursorPos > 0)
                    {
                        FText = Copy(FText, 1, FCursorPos + FFirstChar - 2) + Copy(FText, FCursorPos + FFirstChar, FText.Length - FCursorPos - FFirstChar + 1);
                        FCursorPos = FCursorPos + FFirstChar - 2;
                    }

                    FFirstChar = 1;
                    break;
                case 13:
                    FCursorShown = false;
                    FRedraw = false;
                    if (FOnReturn != null)
                    {
                        FOnReturn(this);
                    }

                    break;
                case >= 0 and <= 7:
                case >= 9 and <= 12:
                case >= 14 and <= 31:
                    break;
                default:
                    C = Key;
                    if (FText.Length < FMaxChars || FMaxChars == 0)
                    {
                        FText = Copy(FText, 1, FFirstChar + FCursorPos - 1) + Cp1252.Text(C) + Copy(FText, FFirstChar + FCursorPos, FText.Length - FCursorPos - FFirstChar + 1);
                        FCursorPos += 1;
                        FCursorPos = FCursorPos + FFirstChar - 1;
                        FFirstChar = 1;
                    }

                    break;
            }

            if (Scroll == true)
            {
                while (FAOWFont.GetTextWidth(Copy(FText, FFirstChar, FText.Length)) > FWinWidth - 2 * FBorderWidth - 1)
                {
                    FFirstChar += 1;
                    FCursorPos -= 1;
                }

                FShownChars = FText.Length - FFirstChar + 1;
                if (FCursorPos < 0)
                {
                    FCursorPos = 0;
                }
            }

            FBlinkCount = 0;
            FDrawn = false;
            if (FOnKeyPress != null)
            {
                FOnKeyPress(this, ref Key);
            }

            if (FOnChange != null)
            {
                FOnChange(this);
            }
        }
    }

    // AoWEdit.pas:119
    public virtual void SetILEdit(TInterfaceIL Value)
    {
        if (Value != FILEdit)
        {
            FValid = false;
            FILEdit = Value;
        }
    }

    // AoWEdit.pas:113
    public void SetGText(string Value)
    {
        FText = Value;
        FSized = false;
    }

    // AoWEdit.pas:128
    public void SetILIndex(int Value)
    {
        if (Value != FILIndex)
        {
            FILIndex = Value;
            FValid = false;
        }
    }

    [Published]
    public TImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

    [Published]
    public string Text { get => FText; set => SetGText(value); }

    [Published]
    public int BorderWidth { get => FBorderWidth; set => FBorderWidth = value; }

    [Published]
    public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

    [Published]
    public int MaxChars { get => FMaxChars; set => FMaxChars = value; }

    [Published]
    public byte CursorChar { get => FCursorChar; set => FCursorChar = value; }

    [Published]
    public int BlinkDelay { get => FBlinkDelay; set => FBlinkDelay = value; }

    [Published]
    public bool Editable { get => FEditable; set => FEditable = value; }

    [Published]
    public TInterfaceIL ILEdit { get => FILEdit; set => SetILEdit(value); }

    [Published]
    public int ILIndex { get => FILIndex; set => SetILIndex(value); }

    [Published]
    public TNotifyEvent OnExit { get => FOnExit; set => FOnExit = value; }

    [Published]
    public TNotifyEvent OnReturn { get => FOnReturn; set => FOnReturn = value; }

    [Published]
    public byte PassWordChar { get => FPassWordChar; set => FPassWordChar = value; }

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
                    FText = Copy(FText, 1, FCursorPos + FFirstChar - 1) +
                        Copy(FText, FCursorPos + FFirstChar + 1, FText.Length - FCursorPos - FFirstChar);
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

public static partial class AoWEditUnit
{
    // AoWEdit.pas:399
    public static void Register()
    {
        RegisterComponents("AOW Interface", [typeof(TAOWEdit)]);
    }
}
