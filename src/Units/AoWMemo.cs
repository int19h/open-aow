// Translated from AoWMemo.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWControl;
using Aow.Units.AoWComp;
using Aow.Units.AOWInterfaceImgLib;

namespace Aow.Units.AoWMemo;

public enum TMemoType : byte
{
    mtNormal = 0,
    mtBlank = 1
}

// the color of a wrapped line. FHasColor, FColor
public partial class TColorChange : TObject
{
    public bool FHasColor;
    public int FColor;
}

// Fields: F114 (cleared by CheckMouseUp, not read), FItems (the text, one paragraph a line),
// FColorList (TColorChange of every wrapped line), FVScrollBar, FNumVisible (lines that fit),
// FTopItem (the first line shown), FScrollSet, FOnDrawItem, FScrollWidth (the width taken at
// the right by the frame or the scroll bar), FScrollVisible, FCursorChar, FBlink (the cursor is
// shown), FBlinkDelay, FBlinkCount, FCurCol, FCurLine (the cursor in FLines), FRealCol,
// FRealLine (the cursor in FItems), FEditable, FILFrame, FILTexture, FItemHeight,
// FColorChangeChar, FTextAlign, FNoFrame (Validate found no frame or texture: drawn
// transparent), FILIndexFrame, FILIndexTexture, FDrawIndex, FItemRect (the line being drawn,
// for OnDrawItem), FLines (the wrapped lines). FRedraw is the focus: the memo takes the keys
// while it is set
public partial class TAOWMemo : TAOWControl
{
    public int F114;
    public TStringList FItems;
    public TList FColorList;
    public TAOWVScrollBar FVScrollBar;
    public int FNumVisible;
    public int FTopItem;
    public bool FScrollSet;
    public TNotifyEvent FOnDrawItem;
    public int FScrollWidth;
    public bool FScrollVisible;
    public byte FCursorChar;
    public bool FBlink;
    public byte FBlinkDelay;
    public byte FBlinkCount;
    public int FCurCol;
    public int FCurLine;
    public int FRealCol;
    public int FRealLine;
    public bool FEditable;
    public TInterfaceIL FILFrame;
    public TInterfaceIL FILTexture;
    public int FItemHeight;
    public byte FColorChangeChar;
    public TaowTextAlign FTextAlign;
    public bool FNoFrame;
    public int FILIndexFrame;
    public int FILIndexTexture;
    public int FDrawIndex;
    public TRect FItemRect;
    public TStringList FLines;
    // AoWMemo.pas:126
    public TAOWMemo(TComponent owner) : base(owner)
    {
        FLines = new TStringList();
        FItems = new TStringList();
        FItems.OnChange = OnChangeText;
        FColorList = new TList();
        FNumVisible = 3;
        FCursorChar = 124;
        FEditable = true;
    }

    // AoWMemo.pas:138
    public override void Destroy()
    {
        FLines.Free();
        FItems.Free();
        for (int I = 0, last = FColorList.Count - 1; I <= last; I++)
        {
            ((TColorChange)FColorList[I]).Free();
        }

        FColorList.Free();
        base.Destroy();
    }

    // AoWMemo.pas:150
    public override void Validate()
    {
        if (FAOWFont != null)
        {
            if (FILFrame != null)
            {
                FILFrame = FILFrame.CheckIL(FILIndexFrame, 8, this);
            }

            if (FILTexture != null)
            {
                FILTexture = FILTexture.CheckIL(FILIndexTexture, 1, this);
            }

            if (FILFrame != null && FILTexture != null)
            {
                FNoFrame = false;
            }
            else
            {
                FNoFrame = true;
            }

            FValid = true;
        }

        base.Validate();
    }

    // AoWMemo.pas:169
    // { SetSize wraps every item at the width of the control into FLines (at spaces; a word that does
//   not fit starts a new line) and sets up the scroll bar }
    public override void SetSize()
    {
        int Len;
        int W;
        ReadOnlySpan<byte> text;
        string S;
        int J;
        int Start;
        int TW;
        TColorChange CC;
        if (FItemHeight < 1)
        {
            FItemHeight = 10;
        }

        switch (FNoFrame)
        {
            case false:
                var ilFrameHeight = FILFrame[FILIndexFrame];
                var ilFrame = FILFrame[FILIndexFrame + 5];
                if (ilFrameHeight.Height + ilFrame.Height > FWinHeight)
                {
                    var ilFrame2 = FILFrame[FILIndexFrame];
                    var ilFrame3 = FILFrame[FILIndexFrame + 5];
                    FWinHeight = ilFrame2.Height + ilFrame3.Height;
                }

                if (FILFrame[FILIndexFrame].Width > FWinWidth)
                {
                    FWinWidth = FILFrame[FILIndexFrame].Width;
                }

                var ilFrame4 = FILFrame[FILIndexFrame + 1];
                var ilFrame5 = FILFrame[FILIndexFrame + 6];
                FNumVisible = (FWinHeight - ilFrame4.Height - ilFrame5.Height) / FItemHeight;
                FScrollWidth = FILFrame[FILIndexFrame + 4].Width;
                FMasked = false;
                break;
            case true:
                FNumVisible = FWinHeight / FItemHeight;
                FScrollWidth = 0;
                FMasked = true;
                break;
        }

        if (FVScrollBar != null)
        {
            FScrollWidth = FVScrollBar.WinWidth;
            FTopItem = FVScrollBar.Position;
        }

        for (int I = 0, last = FColorList.Count - 1; I <= last; I++)
        {
            CC = (TColorChange)FColorList[I];
            CC.Free();
        }

        FColorList.Clear();
        FLines.Clear();
        for (int I = 0, last = FItems.Count - 1; I <= last; I++)
        {
            FLines.Add("");
            var colorChange = new TColorChange();
            FColorList.Add(colorChange);
            J = 0;
            Start = 0;
            if (!FNoFrame)
            {
                W = FILFrame[FILIndexFrame + 3].Width + FScrollWidth + FBorderWidth * 2;
            }
            else
            {
                W = FBorderWidth * 2;
            }

            string item = NullTerminatedText(FItems[I]);
            text = Cp1252.Encode(item);
            Len = text.Length;
            if (Len > 6 && text[0] == FColorChangeChar)
            {
                CC = (TColorChange)FColorList[FColorList.Count - 1];
                CC.FHasColor = true;
                try
                {
                    var item2 = FItems[I];
                    CC.FColor = StrToInt("$" + Copy(item2, 2, 6));
                }
                catch
                {
                }

                J = 8;
                Start = 7;
            }

            while (J < Len)
            {
                if (text[J] == 32 || J == Len - 1)
                {
                    S = Copy(item, Start + 1, J + 1 - Start);
                    TW = FAOWFont.GetTextWidth(S);
                    if (W + TW > FWinWidth)
                    {
                        FLines.Add(S);
                        var colorChange2 = new TColorChange();
                        FColorList.Add(colorChange2);
                        if (!FNoFrame)
                        {
                            W = TW + (FILFrame[FILIndexFrame + 3].Width + FScrollWidth + FBorderWidth * 2);
                        }
                        else
                        {
                            W = TW + FBorderWidth * 2;
                        }
                    }
                    else
                    {
                        var count = FLines.Count;
                        var line = FLines[count - 1];
                        var fLinesCount = FLines.Count;
                        FLines[fLinesCount - 1] = line + S;
                        W = W + TW;
                    }

                    Start = J + 1;
                }

                J += 1;
            }
        }

        if (FLines.Count < FTopItem + FNumVisible)
        {
            FTopItem = FLines.Count - FNumVisible;
        }

        if (FTopItem < 0)
        {
            FTopItem = 0;
        }

        FScrollSet = false;
        if (!FScrollSet)
        {
            if (!FNoFrame)
            {
                FScrollWidth = FILFrame[FILIndexFrame + 4].Width;
            }
            else
            {
                FScrollWidth = 0;
            }

            FScrollVisible = false;
            if (FVScrollBar != null)
            {
                FScrollSet = true;
                if (FAOWWindow != FVScrollBar.AOWWindow)
                {
                    FVScrollBar.AOWWindow = FAOWWindow;
                }

                FVScrollBar.Min = 0;
                var count = FLines.Count;
                FVScrollBar.Max = count - 2;
                FVScrollBar.Priority = FPriority + 1;
                FVScrollBar.Position = FTopItem;
                FVScrollBar.NumberVisible = FNumVisible - 1;
                FVScrollBar.WinHeight = FWinHeight;
                FVScrollBar.LargeChange = FNumVisible - 1;
                FVScrollBar.Visible = FVisible;
                if (FVScrollBar.VanishWhenFull == true && FLines.Count - 1 < FNumVisible)
                {
                    FVScrollBar.Visible = false;
                }
                else
                {
                    FScrollWidth = FVScrollBar.WinWidth;
                }

                FVScrollBar.FSized = false;
                FVScrollBar.WinLeft = FWinLeft + FWinWidth - FVScrollBar.WinWidth;
                FVScrollBar.WinTop = FWinTop;
                FScrollVisible = FVScrollBar.Visible;
            }
        }

        base.SetSize();
    }

    // AoWMemo.pas:457
    // { Update follows the scroll bar and blinks the cursor }
    public override void Update()
    {
        int P;
        base.Update();
        if (FVisible)
        {
            if (FVScrollBar != null && FLines.Count > 0)
            {
                if (FVScrollBar.Position != FTopItem)
                {
                    P = FVScrollBar.Position;
                    FTopItem = P;
                    if (FLines.Count < P + FNumVisible)
                    {
                        FTopItem = FLines.Count - FNumVisible;
                    }

                    if (FTopItem < 0)
                    {
                        FTopItem = 0;
                    }

                    FDrawn = false;
                    if (FNoFrame == true)
                    {
                        if (FAOWWindow != null)
                        {
                            FAOWWindow.FDrawn = false;
                        }
                    }
                }
            }

            if (FEditable && FBlinkDelay > 0 && FRedraw == true)
            {
                FBlinkCount = (byte)(FBlinkCount + 1);
                if (FBlinkCount > FBlinkDelay)
                {
                    FDrawn = false;
                    if (FNoFrame == true)
                    {
                        if (FAOWWindow != null)
                        {
                            FAOWWindow.FDrawn = false;
                        }
                    }

                    FBlinkCount = 0;
                    if (FBlink == true)
                    {
                        FBlink = false;
                    }
                    else
                    {
                        FBlink = true;
                    }
                }
            }
        }
    }

    // AoWMemo.pas:333
    public override void SetVisible(bool Value)
    {
        base.SetVisible(Value);
        if (FVScrollBar != null)
        {
            FVScrollBar.Visible = FVisible;
        }
    }

    // AoWMemo.pas:378
    // { Draw draws the frame and the texture, the visible lines (clipped to the text area) and the
//   cursor }
    public override void Draw(TSurfaceDesc SurfDesc)
    {
        int X = default;
        int Y = default;
        int W = default;
        TRect OldClip = default;
        int H = default;
        TColorChange CC = default;
        try
        {
            if (FValid && FVisible)
            {
                if (FOnPreDraw != null)
                {
                    FOnPreDraw(this, SurfDesc);
                }

                FAOWFont.Color = FColor;
                OldClip = SurfDesc.ClipRect;
                switch (FNoFrame)
                {
                    case false:
                        var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                        FILFrame.DrawILF(FILIndexFrame, rect, ref SurfDesc);
                        var ilFrameHeight = FILFrame[FILIndexFrame + 1];
                        var ilFrame = FILFrame[FILIndexFrame + 6];
                        var ilFrameWidth = FILFrame[FILIndexFrame + 3];
                        var ilFrame2 = FILFrame[FILIndexFrame + 1];
                        var ilFrame3 = FILFrame[FILIndexFrame + 3];
                        var rect2 = Rect(FAOWWindow.FWinLeft + FWinLeft + ilFrame3.Width, FAOWWindow.FWinTop + FWinTop + ilFrame2.Height, FWinWidth - ilFrameWidth.Width - FScrollWidth, FWinHeight - ilFrameHeight.Height - ilFrame.Height);
                        FILTexture.DrawILT(FILIndexTexture, rect2, ref SurfDesc);
                        var ilFrame4 = FILFrame[FILIndexFrame + 3];
                        X = FAOWWindow.FWinLeft + FWinLeft + ilFrame4.Width;
                        var ilFrame5 = FILFrame[FILIndexFrame + 1];
                        Y = FAOWWindow.FWinTop + FWinTop + ilFrame5.Height;
                        var ilFrame6 = FILFrame[FILIndexFrame + 1];
                        var ilFrame7 = FILFrame[FILIndexFrame + 6];
                        H = FWinHeight - ilFrame6.Height - ilFrame7.Height;
                        var ilFrame8 = FILFrame[FILIndexFrame + 3];
                        W = FWinWidth - ilFrame8.Width - FScrollWidth;
                        break;
                    default:
                        X = FAOWWindow.FWinLeft + FWinLeft;
                        Y = FAOWWindow.FWinTop + FWinTop;
                        H = FWinHeight;
                        W = FWinWidth - FScrollWidth;
                        break;
                }

                if (X + W < OldClip.Right)
                {
                    SurfDesc.ClipRect.Right = X + W;
                }

                if (Y + H < OldClip.Bottom)
                {
                    SurfDesc.ClipRect.Bottom = Y + H;
                }

                for (int I = 0, last = FNumVisible; I <= last; I++)
                {
                    var count = FLines.Count;
                    if (FTopItem + I < count)
                    {
                        CC = (TColorChange)FColorList[FTopItem + I];
                        if (CC.FHasColor)
                        {
                            FAOWFont.Color = CC.FColor;
                        }

                        switch (FTextAlign)
                        {
                            case TaowTextAlign.taLeft:
                                var line = FLines[FTopItem + I];
                                FAOWFont.SetText(FBorderWidth + X, FBorderHeight + Y + FItemHeight * I, line, SurfDesc);
                                break;
                            case TaowTextAlign.taRight:
                                var line2 = FLines[FTopItem + I];
                                var textWidth = FAOWFont.GetTextWidth(line2);
                                var line3 = FLines[FTopItem + I];
                                FAOWFont.SetText(X + W - FBorderWidth - textWidth, FBorderHeight + Y + FItemHeight * I, line3, SurfDesc);
                                break;
                            default:
                                var line4 = FLines[FTopItem + I];
                                var faowFontTextWidth = FAOWFont.GetTextWidth(line4);
                                var line5 = FLines[FTopItem + I];
                                FAOWFont.SetText(X + W / 2 - faowFontTextWidth / 2, FBorderHeight + Y + FItemHeight * I, line5, SurfDesc);
                                break;
                        }

                        FDrawIndex = FTopItem + I;
                        FItemRect = Rect(X, Y + FItemHeight * I, X + W, Y + (I + 1) * FItemHeight);
                        if (FItemRect.Bottom > SurfDesc.ClipRect.Bottom)
                        {
                            FItemRect.Bottom = SurfDesc.ClipRect.Bottom;
                        }

                        if (FOnDrawItem != null)
                        {
                            FOnDrawItem(this);
                        }
                    }
                }

                SurfDesc.ClipRect = OldClip;
                bool condition = FCurLine - FTopItem > -1 && FCurLine - FTopItem < FNumVisible;
                if (condition)
                {
                    var count = FLines.Count;
                    condition = FCurLine - FTopItem < count;
                }

                if (condition && FRedraw == true && FEditable && FBlink == true)
                {
                    var line = FLines[FCurLine];
                    var textWidth = FAOWFont.GetTextWidth(Copy(line, 1, FCurCol));
                    FAOWFont.SetText(FBorderWidth + X + textWidth, FBorderHeight + Y + (FCurLine - FTopItem) * FItemHeight, Cp1252.Text(FCursorChar), SurfDesc);
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

    // AoWMemo.pas:497
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FOnMouseDown != null)
            {
                FOnMouseDown(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWMemo.pas:610
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        base.CheckMouseMove(Shift, X, Y);
        FHandled = false;
        if (FVisible && X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth - FScrollWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
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

    // AoWMemo.pas:715
    // { CheckKeyPress edits the text: Backspace, Enter (splits the paragraph) and the printable
//   characters }
    public override void CheckKeyPress(ref byte Key)
    {
        bool Changed;
        bool Moved;
        byte C = default;
        FHandled = FRedraw;
        Changed = false;
        Moved = false;
        if (FEditable && FRedraw == true && FValid == true && FVisible == true)
        {
            switch (Key)
            {
                case 8:
                    if (FRealCol > 0)
                    {
                        var itemLength = FItems[FRealLine];
                        var item = FItems[FRealLine];
                        var item2 = FItems[FRealLine];
                        FItems[FRealLine] = Copy(item2, 1, FRealCol - 1) + Copy(item, FRealCol + 1, itemLength.Length - FRealCol + 1);
                        FRealCol -= 1;
                        Changed = true;
                    }
                    else
                    {
                        if (FRealLine > 0)
                        {
                            FRealCol = FItems[FRealLine - 1].Length;
                            var item = FItems[FRealLine - 1];
                            var item2 = FItems[FRealLine];
                            FItems[FRealLine - 1] = item + item2;
                            FItems.Delete(FRealLine);
                            FRealLine -= 1;
                            Changed = true;
                        }
                    }

                    break;
                case 13:
                    if (FLines[FCurLine].Length == FCurCol)
                    {
                        FItems.Insert(FRealLine + 1, "");
                        var item = FItems[FRealLine];
                        FItems[FRealLine] = Copy(item, 1, FRealCol + 1);
                    }
                    else
                    {
                        var itemLength = FItems[FRealLine];
                        var item = FItems[FRealLine];
                        FItems.Insert(FRealLine + 1, Copy(item, FRealCol + 1, itemLength.Length));
                        var item2 = FItems[FRealLine];
                        FItems[FRealLine] = Copy(item2, 1, FRealCol);
                    }

                    FRealCol = 0;
                    FRealLine += 1;
                    Changed = true;
                    break;
                case >= 0 and <= 7:
                case >= 9 and <= 12:
                case >= 14 and <= 31:
                    break;
                default:
                {
                    C = Key;
                    if (FLines[FCurLine].Length == FCurCol)
                    {
                        var itemLength = FItems[FRealLine];
                        var item = FItems[FRealLine];
                        var item2 = FItems[FRealLine];
                        FItems[FRealLine] = Copy(item2, 1, FRealCol + 1) + Cp1252.Text(C) + Copy(item, FRealCol + 2, itemLength.Length - FRealCol);
                    }
                    else
                    {
                        var itemLength = FItems[FRealLine];
                        var item = FItems[FRealLine];
                        var item2 = FItems[FRealLine];
                        FItems[FRealLine] = Copy(item2, 1, FRealCol) + Cp1252.Text(C) + Copy(item, FRealCol + 1, itemLength.Length - FRealCol);
                    }

                    FRealCol += 1;
                    Changed = true;
                }

                    break;
            }

            if (Moved == true)
            {
                CurToReal();
            }

            if (Changed == true)
            {
                RealToCur();
            }

            KeyStuff();
            // { tests OnKeyDown and calls OnKeyPress (original bug) }
            if (FOnKeyDown != null)
            {
                FOnKeyPress(this, ref Key);
            }

            if (FOnChange != null)
            {
                FOnChange(this);
            }
        }
    }

    // AoWMemo.pas:627
    // { CheckKeyDown moves the cursor with the arrow keys and deletes with Delete }
    public override void CheckKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        bool Changed;
        bool Moved;
        FHandled = FRedraw;
        Changed = false;
        Moved = false;
        if (FEditable && FRedraw == true && FValid == true && FVisible == true)
        {
            switch (Key)
            {
                case 37:
                    if (FRealCol > 0)
                    {
                        FRealCol -= 1;
                    }
                    else
                    {
                        if (FRealLine > 0)
                        {
                            FRealLine -= 1;
                            FRealCol = FItems[FRealLine].Length;
                        }
                    }

                    if (FCurCol > 0)
                    {
                        FCurCol -= 1;
                    }
                    else
                    {
                        if (FCurLine > 0)
                        {
                            FCurLine -= 1;
                            FCurCol = FLines[FCurLine].Length;
                        }
                    }

                    break;
                case 38:
                    if (FCurLine > 0)
                    {
                        FCurLine -= 1;
                        if (FLines[FCurLine].Length < FCurCol)
                        {
                            FCurCol = FLines[FCurLine].Length;
                        }

                        Moved = true;
                    }

                    break;
                case 39:
                    if (FLines[FCurLine].Length > FCurCol)
                    {
                        FCurCol += 1;
                    }
                    else
                    {
                        if (FLines.Count - 1 > FCurLine)
                        {
                            FCurCol = 0;
                            FCurLine += 1;
                        }
                    }

                    if (FItems[FRealLine].Length > FRealCol)
                    {
                        FRealCol += 1;
                    }
                    else
                    {
                        if (FItems.Count - 1 > FRealLine)
                        {
                            FRealCol = 0;
                            FRealLine += 1;
                        }
                    }

                    break;
                case 40:
                    if (FLines.Count - 1 > FCurLine)
                    {
                        FCurLine += 1;
                        if (FLines[FCurLine].Length < FCurCol)
                        {
                            FCurCol = FLines[FCurLine].Length;
                        }

                        Moved = true;
                    }

                    break;
                case 46:
                    if (FItems[FRealLine].Length > FRealCol)
                    {
                        var itemLength = FItems[FRealLine];
                        var item = FItems[FRealLine];
                        var item2 = FItems[FRealLine];
                        FItems[FRealLine] = Copy(item2, 1, FRealCol) + Copy(item, FRealCol + 2, itemLength.Length - FRealCol);
                        Changed = true;
                    }
                    else
                    {
                        if (FItems.Count - 1 > FRealLine)
                        {
                            var item = FItems[FRealLine];
                            var item2 = FItems[FRealLine + 1];
                            FItems[FRealLine] = item + item2;
                            FItems.Delete(FRealLine + 1);
                            Changed = true;
                        }
                    }

                    break;
            }

            if (Changed == true)
            {
                RealToCur();
            }

            if (Moved == true)
            {
                CurToReal();
            }

            KeyStuff();
            if (FOnKeyDown != null)
            {
                FOnKeyDown(this, ref Key, Shift);
            }

            if (FOnChange != null)
            {
                FOnChange(this);
            }
        }
    }

    // AoWMemo.pas:340
    public virtual void SetILFrame(TInterfaceIL Value)
    {
        if (Value != FILFrame)
        {
            FValid = false;
            FILFrame = Value;
        }
    }

    // AoWMemo.pas:349
    public virtual void SetILTexture(TInterfaceIL Value)
    {
        if (Value != FILTexture)
        {
            FValid = false;
            FILTexture = Value;
        }
    }

    // AoWMemo.pas:296
    public void SetFStrings(TStringList Value)
    {
        FLines.Clear();
        FItems.Clear();
        for (int I = 0, last = Value.Count - 1; I <= last; I++)
        {
            var valueValue = Value[I];
            FLines.Add(valueValue);
            var valueValue2 = Value[I];
            FItems.Add(valueValue2);
        }

        if (FLines.Count < FNumVisible)
        {
            FNumVisible = FLines.Count;
        }

        if (FLines.Count < FTopItem + FNumVisible)
        {
            FTopItem = FLines.Count - FNumVisible;
        }

        FSized = false;
    }

    // AoWMemo.pas:314
    public void SetListOff(int Value)
    {
        if (Value >= FLines.Count)
        {
            Value = FLines.Count - 1;
        }

        if (Value != FTopItem)
        {
            FTopItem = Value;
            FDrawn = false;
        }
    }

    // AoWMemo.pas:325
    public void SetVScroll(TAOWVScrollBar Value)
    {
        FVScrollBar = Value;
        if (Value != null)
        {
            Value.Priority = FPriority;
        }

        FSized = false;
    }

    // AoWMemo.pas:358
    public void SetILFIndex(int Value)
    {
        if (Value != FILIndexFrame)
        {
            FILIndexFrame = Value;
            FValid = false;
        }
    }

    // AoWMemo.pas:367
    public void SetILTIndex(int Value)
    {
        if (Value != FILIndexTexture)
        {
            FILIndexTexture = Value;
            FValid = false;
        }
    }

    // AoWMemo.pas:789
    // { CurToReal finds the cursor in FItems from the cursor in FLines; it adds the length of the
//   cursor's line for every line up to it, not of each line (original bug) }
    public void CurToReal()
    {
        int Len;
        int I;
        Len = FItems[0].Length;
        FRealCol = 0;
        FRealLine = 0;
        I = 0;
        while (I <= FCurLine)
        {
            var line = FLines[FCurLine];
            FRealCol = FRealCol + line.Length;
            if (FRealCol >= Len)
            {
                FRealCol = 0;
                FRealLine += 1;
                Len = FItems[FRealLine].Length;
            }

            I += 1;
        }

        FRealCol = FRealCol + FCurCol;
    }

    // AoWMemo.pas:812
    // { RealToCur wraps the text again and finds the cursor in FLines from the cursor in FItems }
    public void RealToCur()
    {
        int Len;
        int I;
        SetSize();
        Len = FItems[0].Length;
        FCurCol = 0;
        FCurLine = 0;
        I = 0;
        while (I < FRealLine)
        {
            var line = FLines[FCurLine];
            FCurCol = FCurCol + line.Length;
            if (FCurCol >= Len)
            {
                FCurCol = 0;
                I += 1;
                Len = FItems[I].Length;
            }

            FCurLine += 1;
        }

        I = FLines[FCurLine].Length;
        while (I < FRealCol && FLines.Count > FCurLine)
        {
            FCurLine += 1;
            I = I + FLines[FCurLine].Length;
        }

        FCurCol = FLines[FCurLine].Length + FRealCol - I;
    }

    // AoWMemo.pas:842
    // { KeyStuff scrolls the cursor into view and shows it }
    public void KeyStuff()
    {
        if (FNumVisible + FTopItem - 1 < FCurLine)
        {
            if (FVScrollBar != null)
            {
                FVScrollBar.Position = FVScrollBar.Position + 1;
            }

            FTopItem += 1;
        }

        if (FCurLine < FTopItem)
        {
            if (FVScrollBar != null)
            {
                FVScrollBar.Position = FVScrollBar.Position - 1;
            }

            FTopItem -= 1;
        }

        FBlink = true;
        FBlinkCount = 0;
        FDrawn = false;
        if (FNoFrame == true)
        {
            if (FAOWWindow != null)
            {
                FAOWWindow.FDrawn = false;
            }
        }
    }

    public int ListOffset { get => FTopItem; set => SetListOff(value); }

    [Published]
    public TImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

    [Published]
    public TStringList Items { get => FItems; set => SetFStrings(value); }

    [Published]
    public int BorderWidth { get => FBorderWidth; set => FBorderWidth = value; }

    [Published]
    public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

    [Published]
    public TAOWVScrollBar VScrollBar { get => FVScrollBar; set => SetVScroll(value); }

    [Published]
    public TNotifyEvent OnDrawItem { get => FOnDrawItem; set => FOnDrawItem = value; }

    [Published]
    public byte CursorChar { get => FCursorChar; set => FCursorChar = value; }

    [Published]
    public byte BlinkDelay { get => FBlinkDelay; set => FBlinkDelay = value; }

    [Published]
    public bool Editable { get => FEditable; set => FEditable = value; }

    [Published]
    public TInterfaceIL ILFrame { get => FILFrame; set => SetILFrame(value); }

    [Published]
    public TInterfaceIL ILTexture { get => FILTexture; set => SetILTexture(value); }

    [Published]
    public int ILIndexFrame { get => FILIndexFrame; set => SetILFIndex(value); }

    [Published]
    public int ILIndexTexture { get => FILIndexTexture; set => SetILTIndex(value); }

    [Published]
    public int ItemHeight { get => FItemHeight; set => FItemHeight = value; }

    [Published]
    public byte ColorChangeChar { get => FColorChangeChar; set => FColorChangeChar = value; }

    [Published]
    public TaowTextAlign TextAlign { get => FTextAlign; set => FTextAlign = value; }
}

public static partial class AoWMemo
{
    // AoWMemo.pas:864
    public static void Register()
    {
        RegisterComponents("AOW Interface", [typeof(TAOWMemo)]);
    }
}
