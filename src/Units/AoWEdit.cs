// Translated from AoWEdit.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.AoWControl;
using Aow.Units.AOWInterfaceImgLib;

namespace Aow.Units.AoWEdit
{
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
                var call__1 = FILEdit.CheckIL(FILIndex, 3, this);
                FILEdit = call__1;
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
            var call__2 = FILEdit[FILIndex];
            var call__3 = FILEdit[FILIndex + 2];
            if (call__2.Width + call__3.Width > FWinWidth)
            {
                var call__4 = FILEdit[FILIndex];
                var call__5 = FILEdit[FILIndex + 2];
                FWinWidth = call__4.Width + call__5.Width;
            }

            var call__6 = FILEdit[FILIndex];
            FWinHeight = call__6.Height;
            FFirstChar = 1;
            if (FText.Length < FCursorPos)
            {
                FCursorPos = FText.Length;
            }

            var call__7 = FAOWFont.GetTextWidth(Copy(FText, FFirstChar, FText.Length - FFirstChar));
            if (call__7 > FWinWidth - 2 * FBorderWidth - 1)
            {
                while (true)
                {
                    var call__8 = FAOWFont.GetTextWidth(Copy(FText, FFirstChar, FText.Length));
                    if (!(call__8 > FWinWidth - 2 * FBorderWidth - 1))
                        break;
                    {
                        FFirstChar += 1;
                    }
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
            AnsiString S = default;
            TRect OldClip = default;
            int I = default;
            try
            {
                if (FValid && FVisible)
                {
                    if (FOnPreDraw != null)
                    {
                        FOnPreDraw(this, SurfDesc);
                    }

                    var call__9 = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                    FILEdit.DrawILLh(FILIndex, call__9, ref SurfDesc);
                    OldClip = SurfDesc.ClipRect;
                    var call__10 = FILEdit[FILIndex + 2];
                    if (FAOWWindow.FWinLeft + FWinLeft + FWinWidth - call__10.Width < OldClip.Right)
                    {
                        var call__11 = FILEdit[FILIndex + 2];
                        SurfDesc.ClipRect.Right = FAOWWindow.FWinLeft + FWinLeft + FWinWidth - call__11.Width;
                    }

                    FAOWFont.Color = FColor;
                    if (FPassWordChar == 0)
                    {
                        FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FBorderWidth, FAOWWindow.FWinTop + FWinTop + FBorderHeight, Copy(FText, FFirstChar, FShownChars), SurfDesc);
                    }
                    else
                    {
                        S = (AnsiString)"";
                        {
                            I = 0;
                            int end__12 = FShownChars - 1;
                            for (; I <= end__12; I += 1)
                            {
                                S = S + AnsiString.FromBytes(FPassWordChar);
                            }
                        }

                        FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FBorderWidth, FAOWWindow.FWinTop + FWinTop + FBorderHeight, S, SurfDesc);
                    }

                    SurfDesc.ClipRect = OldClip;
                    if (FEditable && FRedraw == true && FCursorShown == true)
                    {
                        var call__13 = FAOWFont.GetTextWidth(Copy(FText, FFirstChar, FCursorPos));
                        FAOWFont.SetText(FAOWWindow.FWinLeft + FWinLeft + FBorderWidth + call__13, FAOWWindow.FWinTop + FWinTop + FBorderHeight, AnsiString.FromBytes(FCursorChar), SurfDesc);
                    }

                    if (FOnDraw != null)
                    {
                        FOnDraw(this, SurfDesc);
                    }
                }
            }
            catch (System.Exception)
            {
                MessageBox(0, (AnsiString)"Exception during Draw", (AnsiString)"Error - " + Name, 0);
            }
        }

        // AoWEdit.pas:197
        public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            int W = default;
            int I = default;
            bool OldRedraw = default;
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

                        var call__14 = FAOWFont.GetTextWidth(Copy(FText, FFirstChar, FText.Length));
                        if (X > FAOWWindow.FWinLeft + FWinLeft + FBorderWidth + call__14)
                        {
                            FFirstChar = 1;
                            var call__15 = FAOWFont.GetTextWidth(Copy(FText, FFirstChar, FText.Length - FFirstChar));
                            if (call__15 > FWinWidth - 2 * FBorderWidth - 1)
                            {
                                while (true)
                                {
                                    var call__16 = FAOWFont.GetTextWidth(Copy(FText, FFirstChar, FText.Length));
                                    if (!(call__16 > FWinWidth - 2 * FBorderWidth - 1))
                                        break;
                                    {
                                        FFirstChar += 1;
                                    }
                                }
                            }

                            FCursorPos = FText.Length - FFirstChar + 1;
                        }
                        else
                        {
                            I = 0;
                            var call__17 = FAOWFont.GetTextWidth(Copy(FText, FFirstChar, I + 1));
                            W = call__17;
                            while (X > FAOWWindow.FWinLeft + FWinLeft + FBorderWidth + W && FText.Length > I + FFirstChar - 1)
                            {
                                I += 1;
                                var call__18 = FAOWFont.GetTextWidth(Copy(FText, FFirstChar, I + 1));
                                W = call__18;
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
            bool Scroll = default;
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
                        ;
                        break;
                    default:
                        C = Key;
                        if (FText.Length < FMaxChars || FMaxChars == 0)
                        {
                            FText = Copy(FText, 1, FFirstChar + FCursorPos - 1) + AnsiString.FromBytes(C) + Copy(FText, FFirstChar + FCursorPos, FText.Length - FCursorPos - FFirstChar + 1);
                            FCursorPos += 1;
                            FCursorPos = FCursorPos + FFirstChar - 1;
                            FFirstChar = 1;
                        }

                        break;
                }

                if (Scroll == true)
                {
                    while (true)
                    {
                        var call__19 = FAOWFont.GetTextWidth(Copy(FText, FFirstChar, FText.Length));
                        if (!(call__19 > FWinWidth - 2 * FBorderWidth - 1))
                            break;
                        {
                            FFirstChar += 1;
                            FCursorPos -= 1;
                        }
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
        public void SetGText(AnsiString Value)
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
        public AnsiString Text { get => FText; set => SetGText(value); }

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
    }

    public static partial class AoWEdit
    {
        // AoWEdit.pas:399
        public static void Register()
        {
            RegisterComponents((AnsiString)"AOW Interface", [typeof(TAOWEdit)]);
        }
    }
}
