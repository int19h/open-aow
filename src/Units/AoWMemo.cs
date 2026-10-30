// Translated from AoWMemo.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AOWToolTip;
using static Aow.Units.AOWToolTip.AOWToolTip;
using Aow.Units.AoWControl;
using static Aow.Units.AoWControl.AoWControl;
using Aow.Units.AOWWinManager;
using static Aow.Units.AOWWinManager.AOWWinManager;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AOWInterfaceImgLib;
using static Aow.Units.AOWInterfaceImgLib.AOWInterfaceImgLib;
using Aow.Units.AoWWinControl;
using static Aow.Units.AoWWinControl.AoWWinControl;
using Aow.Units.AoWBaseWin;
using static Aow.Units.AoWBaseWin.AoWBaseWin;
using Aow.Units.AoWTabPanel;
using static Aow.Units.AoWTabPanel.AoWTabPanel;

namespace Aow.Units.AoWMemo
{
    public enum TMemoType : byte
    {
        mtNormal = 0,
        mtBlank = 1
    }

    // { AoWMemo.TColorChange (instance size 12): the color of a wrapped line. +4 FHasColor, +8
//     FColor }
    public partial class TColorChange : Aow.Rtl.TObject
    {
        public bool FHasColor;
        public int FColor;
    }

    // { AoWMemo.TAOWMemo (instance size 396). Fields: +$114 F114 (cleared by CheckMouseUp, not read),
//     +$118 FItems (the text, one paragraph a line), +$11C FColorList (TColorChange of every wrapped
//     line), +$120 FVScrollBar, +$124 FNumVisible (lines that fit), +$128 FTopItem (the first line
//     shown), +$12C FScrollSet, +$130 FOnDrawItem, +$138 FScrollWidth (the width taken at the right
//     by the frame or the scroll bar), +$13C FScrollVisible, +$13D FCursorChar, +$140 FBlink (the
//     cursor is shown), +$144 FBlinkDelay, +$145 FBlinkCount, +$148 FCurCol, +$14C FCurLine (the
//     cursor in FLines), +$150 FRealCol, +$154 FRealLine (the cursor in FItems), +$158 FEditable,
//     +$15C FILFrame, +$160 FILTexture, +$164 FItemHeight, +$168 FColorChangeChar, +$169
//     FTextAlign, +$16A FNoFrame (Validate found no frame or texture: drawn transparent), +$16C
//     FILIndexFrame, +$170 FILIndexTexture, +$174 FDrawIndex, +$178 FItemRect (the line being
//     drawn, for OnDrawItem), +$188 FLines (the wrapped lines). FRedraw (+$94) is the focus: the
//     memo takes the keys while it is set.
//     VMT (new): SetILFrame, SetILTexture }
    public partial class TAOWMemo : Aow.Units.AoWControl.TAOWControl
    {
        public int F114;
        public Aow.Rtl.TStringList FItems;
        public Aow.Rtl.TList FColorList;
        public Aow.Units.AoWVScroll.TAOWVScrollBar FVScrollBar;
        public int FNumVisible;
        public int FTopItem;
        public bool FScrollSet;
        public Aow.Rtl.TNotifyEvent FOnDrawItem;
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
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILFrame;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILTexture;
        public int FItemHeight;
        public byte FColorChangeChar;
        public Aow.Units.AoWComp.TaowTextAlign FTextAlign;
        public bool FNoFrame;
        public int FILIndexFrame;
        public int FILIndexTexture;
        public int FDrawIndex;
        public Aow.Rtl.TRect FItemRect;
        public Aow.Rtl.TStringList FLines;
        // AoWMemo.pas:126
        // { TAOWMemo }
        public TAOWMemo(TComponent owner) : base(owner)
        {
            var call__1 = new Aow.Rtl.TStringList();
            FLines = call__1;
            var call__2 = new Aow.Rtl.TStringList();
            FItems = call__2;
            FItems.OnChange = OnChangeText;
            var call__3 = new Aow.Rtl.TList();
            FColorList = call__3;
            FNumVisible = 3;
            FCursorChar = 124;
            FEditable = true;
        }

        // AoWMemo.pas:138
        public override void Destroy()
        {
            int I = default;
            FLines.Free();
            FItems.Free();
            {
                I = 0;
                int end__4 = FColorList.Count - 1;
                for (; I <= end__4; I += 1)
                {
                    var call__5 = FColorList[I];
                    ((Aow.Units.AoWMemo.TColorChange)call__5).Free();
                }
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
                    var call__6 = FILFrame.CheckIL(FILIndexFrame, 8, this);
                    FILFrame = call__6;
                }

                if (FILTexture != null)
                {
                    var call__7 = FILTexture.CheckIL(FILIndexTexture, 1, this);
                    FILTexture = call__7;
                }

                if ((FILFrame != null) && (FILTexture != null))
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
            int I = default;
            int Len = default;
            int W = default;
            ReadOnlySpan<byte> text = default;
            Aow.Rtl.AnsiString S = default;
            int J = default;
            int Start = default;
            int TW = default;
            Aow.Units.AoWMemo.TColorChange CC = default;
            if (FItemHeight < 1)
            {
                FItemHeight = 10;
            }

            switch (FNoFrame)
            {
                case false:
                    var call__8 = FILFrame[FILIndexFrame];
                    var call__9 = FILFrame[FILIndexFrame + 5];
                    if ((call__8.Height + call__9.Height) > FWinHeight)
                    {
                        var call__10 = FILFrame[FILIndexFrame];
                        var call__11 = FILFrame[FILIndexFrame + 5];
                        FWinHeight = call__10.Height + call__11.Height;
                    }

                    var call__12 = FILFrame[FILIndexFrame];
                    if (call__12.Width > FWinWidth)
                    {
                        var call__13 = FILFrame[FILIndexFrame];
                        FWinWidth = call__13.Width;
                    }

                    var call__14 = FILFrame[FILIndexFrame + 1];
                    var call__15 = FILFrame[FILIndexFrame + 6];
                    FNumVisible = ((FWinHeight - call__14.Height) - call__15.Height) / FItemHeight;
                    var call__16 = FILFrame[FILIndexFrame + 4];
                    FScrollWidth = call__16.Width;
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

            {
                I = 0;
                int end__17 = FColorList.Count - 1;
                for (; I <= end__17; I += 1)
                {
                    var call__18 = FColorList[I];
                    CC = (Aow.Units.AoWMemo.TColorChange)call__18;
                    CC.Free();
                }
            }

            FColorList.Clear();
            FLines.Clear();
            {
                I = 0;
                int end__19 = FItems.Count - 1;
                for (; I <= end__19; I += 1)
                {
                    FLines.Add((AnsiString)"");
                    var call__20 = new Aow.Units.AoWMemo.TColorChange();
                    FColorList.Add(call__20);
                    J = 0;
                    Start = 0;
                    if (!FNoFrame)
                    {
                        var call__21 = FILFrame[FILIndexFrame + 3];
                        W = (call__21.Width + FScrollWidth) + (FBorderWidth * 2);
                    }
                    else
                    {
                        W = FBorderWidth * 2;
                    }

                    var call__22 = FItems[I];
                    AnsiString item = Aow.Rtl.Rtl.NullTerminatedText(call__22);
                    text = Aow.Rtl.Rtl.StringBytes(item);
                    Len = text.Length;
                    if ((Len > 6) && (text[0] == FColorChangeChar))
                    {
                        var call__23 = FColorList[FColorList.Count - 1];
                        CC = (Aow.Units.AoWMemo.TColorChange)call__23;
                        CC.FHasColor = true;
                        try
                        {
                            var call__24 = FItems[I];
                            CC.FColor = Aow.Rtl.Rtl.StrToInt(AnsiString.FromBytes(36) + Aow.Rtl.Rtl.Copy(call__24, 2, 6));
                        }
                        catch (System.Exception)
                        {
                        }

                        J = 8;
                        Start = 7;
                    }

                    while (J < Len)
                    {
                        if ((text[J] == 32) || (J == (Len - 1)))
                        {
                            S = Aow.Rtl.Rtl.Copy(item, Start + 1, (J + 1) - Start);
                            var call__25 = FAOWFont.GetTextWidth(S);
                            TW = call__25;
                            if ((W + TW) > FWinWidth)
                            {
                                FLines.Add(S);
                                var call__26 = new Aow.Units.AoWMemo.TColorChange();
                                FColorList.Add(call__26);
                                if (!FNoFrame)
                                {
                                    var call__27 = FILFrame[FILIndexFrame + 3];
                                    W = TW + ((call__27.Width + FScrollWidth) + (FBorderWidth * 2));
                                }
                                else
                                {
                                    W = TW + (FBorderWidth * 2);
                                }
                            }
                            else
                            {
                                var call__28 = FLines.Count;
                                var call__29 = FLines[call__28 - 1];
                                var call__30 = FLines.Count;
                                FLines[call__30 - 1] = call__29 + S;
                                W = W + TW;
                            }

                            Start = J + 1;
                        }

                        J += 1;
                    }
                }
            }

            var call__31 = FLines.Count;
            if (call__31 < (FTopItem + FNumVisible))
            {
                var call__32 = FLines.Count;
                FTopItem = call__32 - FNumVisible;
            }

            if (FTopItem < 0)
            {
                FTopItem = 0;
            }

            FScrollSet = false;
            if (FScrollSet == false)
            {
                if (!FNoFrame)
                {
                    var call__33 = FILFrame[FILIndexFrame + 4];
                    FScrollWidth = call__33.Width;
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
                    var call__34 = FLines.Count;
                    FVScrollBar.Max = call__34 - 2;
                    FVScrollBar.Priority = FPriority + 1;
                    FVScrollBar.Position = FTopItem;
                    FVScrollBar.NumberVisible = FNumVisible - 1;
                    FVScrollBar.WinHeight = FWinHeight;
                    FVScrollBar.LargeChange = FNumVisible - 1;
                    FVScrollBar.Visible = FVisible;
                    bool bool__35 = (FVScrollBar.VanishWhenFull == true);
                    if (bool__35)
                    {
                        var call__36 = FLines.Count;
                        bool__35 = ((call__36 - 1) < FNumVisible);
                    }

                    if (bool__35)
                    {
                        FVScrollBar.Visible = false;
                    }
                    else
                    {
                        FScrollWidth = FVScrollBar.WinWidth;
                    }

                    FVScrollBar.FSized = false;
                    FVScrollBar.WinLeft = (FWinLeft + FWinWidth) - FVScrollBar.WinWidth;
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
            int P = default;
            base.Update();
            if (FVisible)
            {
                bool bool__37 = (FVScrollBar != null);
                if (bool__37)
                {
                    var call__38 = FLines.Count;
                    bool__37 = (call__38 > 0);
                }

                if (bool__37)
                {
                    if (FVScrollBar.Position != FTopItem)
                    {
                        P = FVScrollBar.Position;
                        FTopItem = P;
                        var call__39 = FLines.Count;
                        if (call__39 < (P + FNumVisible))
                        {
                            var call__40 = FLines.Count;
                            FTopItem = call__40 - FNumVisible;
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

                if ((FEditable && (FBlinkDelay > 0)) && (FRedraw == true))
                {
                    FBlinkCount = (byte)(((int)FBlinkCount) + 1);
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
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int X = default;
            int Y = default;
            int W = default;
            Aow.Rtl.TRect OldClip = default;
            int H = default;
            int I = default;
            Aow.Units.AoWMemo.TColorChange CC = default;
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
                            var call__41 = Aow.Rtl.Rtl.Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                            FILFrame.DrawILF(FILIndexFrame, call__41, ref SurfDesc);
                            var call__42 = FILFrame[FILIndexFrame + 1];
                            var call__43 = FILFrame[FILIndexFrame + 6];
                            var call__44 = FILFrame[FILIndexFrame + 3];
                            var call__45 = FILFrame[FILIndexFrame + 1];
                            var call__46 = FILFrame[FILIndexFrame + 3];
                            var call__47 = Aow.Rtl.Rtl.Rect((FAOWWindow.FWinLeft + FWinLeft) + call__46.Width, (FAOWWindow.FWinTop + FWinTop) + call__45.Height, (FWinWidth - call__44.Width) - FScrollWidth, (FWinHeight - call__42.Height) - call__43.Height);
                            FILTexture.DrawILT(FILIndexTexture, call__47, ref SurfDesc);
                            var call__48 = FILFrame[FILIndexFrame + 3];
                            X = (FAOWWindow.FWinLeft + FWinLeft) + call__48.Width;
                            var call__49 = FILFrame[FILIndexFrame + 1];
                            Y = (FAOWWindow.FWinTop + FWinTop) + call__49.Height;
                            var call__50 = FILFrame[FILIndexFrame + 1];
                            var call__51 = FILFrame[FILIndexFrame + 6];
                            H = (FWinHeight - call__50.Height) - call__51.Height;
                            var call__52 = FILFrame[FILIndexFrame + 3];
                            W = (FWinWidth - call__52.Width) - FScrollWidth;
                            break;
                        default:
                            X = FAOWWindow.FWinLeft + FWinLeft;
                            Y = FAOWWindow.FWinTop + FWinTop;
                            H = FWinHeight;
                            W = FWinWidth - FScrollWidth;
                            break;
                    }

                    if ((X + W) < OldClip.Right)
                    {
                        SurfDesc.ClipRect.Right = X + W;
                    }

                    if ((Y + H) < OldClip.Bottom)
                    {
                        SurfDesc.ClipRect.Bottom = Y + H;
                    }

                    {
                        I = 0;
                        int end__53 = FNumVisible;
                        for (; I <= end__53; I += 1)
                        {
                            var call__54 = FLines.Count;
                            if ((FTopItem + I) < call__54)
                            {
                                var call__55 = FColorList[FTopItem + I];
                                CC = (Aow.Units.AoWMemo.TColorChange)call__55;
                                if (CC.FHasColor)
                                {
                                    FAOWFont.Color = CC.FColor;
                                }

                                switch (FTextAlign)
                                {
                                    case Aow.Units.AoWComp.TaowTextAlign.taLeft:
                                        var call__56 = FLines[FTopItem + I];
                                        FAOWFont.SetText(FBorderWidth + X, (FBorderHeight + Y) + (FItemHeight * I), call__56, SurfDesc);
                                        break;
                                    case Aow.Units.AoWComp.TaowTextAlign.taRight:
                                        var call__57 = FLines[FTopItem + I];
                                        var call__58 = FAOWFont.GetTextWidth(call__57);
                                        var call__59 = FLines[FTopItem + I];
                                        FAOWFont.SetText(((X + W) - FBorderWidth) - call__58, (FBorderHeight + Y) + (FItemHeight * I), call__59, SurfDesc);
                                        break;
                                    default:
                                        var call__60 = FLines[FTopItem + I];
                                        var call__61 = FAOWFont.GetTextWidth(call__60);
                                        var call__62 = FLines[FTopItem + I];
                                        FAOWFont.SetText((X + (W / 2)) - (call__61 / 2), (FBorderHeight + Y) + (FItemHeight * I), call__62, SurfDesc);
                                        break;
                                }

                                FDrawIndex = FTopItem + I;
                                var call__63 = Aow.Rtl.Rtl.Rect(X, Y + (FItemHeight * I), X + W, Y + ((I + 1) * FItemHeight));
                                FItemRect = call__63;
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
                    }

                    SurfDesc.ClipRect = OldClip;
                    bool bool__64 = ((FCurLine - FTopItem) > -1) && ((FCurLine - FTopItem) < FNumVisible);
                    if (bool__64)
                    {
                        var call__65 = FLines.Count;
                        bool__64 = ((FCurLine - FTopItem) < call__65);
                    }

                    if (((bool__64 && (FRedraw == true)) && FEditable) && (FBlink == true))
                    {
                        var call__66 = FLines[FCurLine];
                        var call__67 = FAOWFont.GetTextWidth(Aow.Rtl.Rtl.Copy(call__66, 1, FCurCol));
                        FAOWFont.SetText((FBorderWidth + X) + call__67, (FBorderHeight + Y) + ((FCurLine - FTopItem) * FItemHeight), AnsiString.FromBytes(FCursorChar), SurfDesc);
                    }

                    if (FOnDraw != null)
                    {
                        FOnDraw(this, SurfDesc);
                    }
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during Draw", ((AnsiString)"Error - ") + Name, (int)0);
            }
        }

        // AoWMemo.pas:497
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseDown(Button, Shift, X, Y);
            FHandled = false;
            if ((((X > ((FAOWWindow.FWinLeft + FWinLeft) - 1)) && (X < ((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
            {
                FHandled = true;
                if (FOnMouseDown != null)
                {
                    FOnMouseDown(this, Button, Shift, X, Y);
                }
            }
        }

        // AoWMemo.pas:610
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            base.CheckMouseMove(Shift, X, Y);
            FHandled = false;
            if ((((FVisible && (X > ((FAOWWindow.FWinLeft + FWinLeft) - 1))) && (X < (((FAOWWindow.FWinLeft + FWinLeft) + FWinWidth) - FScrollWidth))) && (Y > ((FAOWWindow.FWinTop + FWinTop) - 1))) && (Y < ((FAOWWindow.FWinTop + FWinTop) + FWinHeight)))
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
            bool Changed = default;
            bool Moved = default;
            byte C = default;
            FHandled = FRedraw;
            Changed = false;
            Moved = false;
            if (((FEditable && (FRedraw == true)) && (FValid == true)) && (FVisible == true))
            {
                switch (Key)
                {
                    case 8:
                        if (FRealCol > 0)
                        {
                            var call__68 = FItems[FRealLine];
                            var call__69 = FItems[FRealLine];
                            var call__70 = FItems[FRealLine];
                            FItems[FRealLine] = Aow.Rtl.Rtl.Copy(call__70, 1, FRealCol - 1) + Aow.Rtl.Rtl.Copy(call__69, FRealCol + 1, (call__68.Length - FRealCol) + 1);
                            FRealCol -= 1;
                            Changed = true;
                        }
                        else
                        {
                            if (FRealLine > 0)
                            {
                                var call__71 = FItems[FRealLine - 1];
                                FRealCol = call__71.Length;
                                var call__72 = FItems[FRealLine - 1];
                                var call__73 = FItems[FRealLine];
                                FItems[FRealLine - 1] = call__72 + call__73;
                                FItems.Delete(FRealLine);
                                FRealLine -= 1;
                                Changed = true;
                            }
                        }

                        break;
                    case 13:
                        var call__74 = FLines[FCurLine];
                        if (call__74.Length == FCurCol)
                        {
                            FItems.Insert(FRealLine + 1, (AnsiString)"");
                            var call__75 = FItems[FRealLine];
                            FItems[FRealLine] = Aow.Rtl.Rtl.Copy(call__75, 1, FRealCol + 1);
                        }
                        else
                        {
                            var call__76 = FItems[FRealLine];
                            var call__77 = FItems[FRealLine];
                            FItems.Insert(FRealLine + 1, Aow.Rtl.Rtl.Copy(call__77, FRealCol + 1, call__76.Length));
                            var call__78 = FItems[FRealLine];
                            FItems[FRealLine] = Aow.Rtl.Rtl.Copy(call__78, 1, FRealCol);
                        }

                        FRealCol = 0;
                        FRealLine += 1;
                        Changed = true;
                        break;
                    case >= 0 and <= 7:
                    case >= 9 and <= 12:
                    case >= 14 and <= 31:
                        ;
                        break;
                    default:
                    {
                        C = unchecked((byte)Key);
                        var call__79 = FLines[FCurLine];
                        if (call__79.Length == FCurCol)
                        {
                            var call__80 = FItems[FRealLine];
                            var call__81 = FItems[FRealLine];
                            var call__82 = FItems[FRealLine];
                            FItems[FRealLine] = (Aow.Rtl.Rtl.Copy(call__82, 1, FRealCol + 1) + AnsiString.FromBytes(C)) + Aow.Rtl.Rtl.Copy(call__81, FRealCol + 2, call__80.Length - FRealCol);
                        }
                        else
                        {
                            var call__83 = FItems[FRealLine];
                            var call__84 = FItems[FRealLine];
                            var call__85 = FItems[FRealLine];
                            FItems[FRealLine] = (Aow.Rtl.Rtl.Copy(call__85, 1, FRealCol) + AnsiString.FromBytes(C)) + Aow.Rtl.Rtl.Copy(call__84, FRealCol + 1, call__83.Length - FRealCol);
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
        public override void CheckKeyDown(ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            bool Changed = default;
            bool Moved = default;
            FHandled = FRedraw;
            Changed = false;
            Moved = false;
            if (((FEditable && (FRedraw == true)) && (FValid == true)) && (FVisible == true))
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
                                var call__86 = FItems[FRealLine];
                                FRealCol = call__86.Length;
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
                                var call__87 = FLines[FCurLine];
                                FCurCol = call__87.Length;
                            }
                        }

                        break;
                    case 38:
                        if (FCurLine > 0)
                        {
                            FCurLine -= 1;
                            var call__88 = FLines[FCurLine];
                            if (call__88.Length < FCurCol)
                            {
                                var call__89 = FLines[FCurLine];
                                FCurCol = call__89.Length;
                            }

                            Moved = true;
                        }

                        break;
                    case 39:
                        var call__90 = FLines[FCurLine];
                        if (call__90.Length > FCurCol)
                        {
                            FCurCol += 1;
                        }
                        else
                        {
                            var call__91 = FLines.Count;
                            if ((call__91 - 1) > FCurLine)
                            {
                                FCurCol = 0;
                                FCurLine += 1;
                            }
                        }

                        var call__92 = FItems[FRealLine];
                        if (call__92.Length > FRealCol)
                        {
                            FRealCol += 1;
                        }
                        else
                        {
                            var call__93 = FItems.Count;
                            if ((call__93 - 1) > FRealLine)
                            {
                                FRealCol = 0;
                                FRealLine += 1;
                            }
                        }

                        break;
                    case 40:
                        var call__94 = FLines.Count;
                        if ((call__94 - 1) > FCurLine)
                        {
                            FCurLine += 1;
                            var call__95 = FLines[FCurLine];
                            if (call__95.Length < FCurCol)
                            {
                                var call__96 = FLines[FCurLine];
                                FCurCol = call__96.Length;
                            }

                            Moved = true;
                        }

                        break;
                    case 46:
                        var call__97 = FItems[FRealLine];
                        if (call__97.Length > FRealCol)
                        {
                            var call__98 = FItems[FRealLine];
                            var call__99 = FItems[FRealLine];
                            var call__100 = FItems[FRealLine];
                            FItems[FRealLine] = Aow.Rtl.Rtl.Copy(call__100, 1, FRealCol) + Aow.Rtl.Rtl.Copy(call__99, FRealCol + 2, call__98.Length - FRealCol);
                            Changed = true;
                        }
                        else
                        {
                            var call__101 = FItems.Count;
                            if ((call__101 - 1) > FRealLine)
                            {
                                var call__102 = FItems[FRealLine];
                                var call__103 = FItems[FRealLine + 1];
                                FItems[FRealLine] = call__102 + call__103;
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
        public virtual void SetILFrame(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILFrame)
            {
                FValid = false;
                FILFrame = Value;
            }
        }

        // AoWMemo.pas:349
        public virtual void SetILTexture(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILTexture)
            {
                FValid = false;
                FILTexture = Value;
            }
        }

        // AoWMemo.pas:296
        public void SetFStrings(Aow.Rtl.TStringList Value)
        {
            int I = default;
            FLines.Clear();
            FItems.Clear();
            {
                I = 0;
                int end__104 = Value.Count - 1;
                for (; I <= end__104; I += 1)
                {
                    var call__105 = Value[I];
                    FLines.Add(call__105);
                    var call__106 = Value[I];
                    FItems.Add(call__106);
                }
            }

            var call__107 = FLines.Count;
            if (call__107 < FNumVisible)
            {
                var call__108 = FLines.Count;
                FNumVisible = call__108;
            }

            var call__109 = FLines.Count;
            if (call__109 < (FTopItem + FNumVisible))
            {
                var call__110 = FLines.Count;
                FTopItem = call__110 - FNumVisible;
            }

            FSized = false;
        }

        // AoWMemo.pas:314
        public void SetListOff(int Value)
        {
            var call__111 = FLines.Count;
            if (Value >= call__111)
            {
                var call__112 = FLines.Count;
                Value = call__112 - 1;
            }

            if (Value != FTopItem)
            {
                FTopItem = Value;
                FDrawn = false;
            }
        }

        // AoWMemo.pas:325
        public void SetVScroll(Aow.Units.AoWVScroll.TAOWVScrollBar Value)
        {
            // { Tried parameter identity, with and typed/TObject locals; original scrollbar save still differs. }
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
            int Len = default;
            int I = default;
            var call__113 = FItems[0];
            Len = call__113.Length;
            FRealCol = 0;
            FRealLine = 0;
            I = 0;
            while (I <= FCurLine)
            {
                var call__114 = FLines[FCurLine];
                FRealCol = FRealCol + call__114.Length;
                if (FRealCol >= Len)
                {
                    FRealCol = 0;
                    FRealLine += 1;
                    var call__115 = FItems[FRealLine];
                    Len = call__115.Length;
                }

                I += 1;
            }

            FRealCol = FRealCol + FCurCol;
        }

        // AoWMemo.pas:812
        // { RealToCur wraps the text again and finds the cursor in FLines from the cursor in FItems }
        public void RealToCur()
        {
            int Len = default;
            int I = default;
            SetSize();
            var call__116 = FItems[0];
            Len = call__116.Length;
            FCurCol = 0;
            FCurLine = 0;
            I = 0;
            while (I < FRealLine)
            {
                var call__117 = FLines[FCurLine];
                FCurCol = FCurCol + call__117.Length;
                if (FCurCol >= Len)
                {
                    FCurCol = 0;
                    I += 1;
                    var call__118 = FItems[I];
                    Len = call__118.Length;
                }

                FCurLine += 1;
            }

            var call__119 = FLines[FCurLine];
            I = call__119.Length;
            while (true)
            {
                bool bool__120 = (I < FRealCol);
                if (bool__120)
                {
                    var call__121 = FLines.Count;
                    bool__120 = (call__121 > FCurLine);
                }

                if (!bool__120)
                    break;
                {
                    FCurLine += 1;
                    var call__122 = FLines[FCurLine];
                    I = I + call__122.Length;
                }
            }

            var call__123 = FLines[FCurLine];
            FCurCol = (call__123.Length + FRealCol) - I;
        }

        // AoWMemo.pas:842
        // { KeyStuff scrolls the cursor into view and shows it }
        public void KeyStuff()
        {
            if (((FNumVisible + FTopItem) - 1) < FCurLine)
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
        public Aow.Units.ImageLib.TImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

        [Published]
        public Aow.Rtl.TStringList Items { get => FItems; set => SetFStrings(value); }

        [Published]
        public int BorderWidth { get => FBorderWidth; set => FBorderWidth = value; }

        [Published]
        public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

        [Published]
        public Aow.Units.AoWVScroll.TAOWVScrollBar VScrollBar { get => FVScrollBar; set => SetVScroll(value); }

        [Published]
        public Aow.Rtl.TNotifyEvent OnDrawItem { get => FOnDrawItem; set => FOnDrawItem = value; }

        [Published]
        public byte CursorChar { get => FCursorChar; set => FCursorChar = value; }

        [Published]
        public byte BlinkDelay { get => FBlinkDelay; set => FBlinkDelay = value; }

        [Published]
        public bool Editable { get => FEditable; set => FEditable = value; }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILFrame { get => FILFrame; set => SetILFrame(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILTexture { get => FILTexture; set => SetILTexture(value); }

        [Published]
        public int ILIndexFrame { get => FILIndexFrame; set => SetILFIndex(value); }

        [Published]
        public int ILIndexTexture { get => FILIndexTexture; set => SetILTIndex(value); }

        [Published]
        public int ItemHeight { get => FItemHeight; set => FItemHeight = value; }

        [Published]
        public byte ColorChangeChar { get => FColorChangeChar; set => FColorChangeChar = value; }

        [Published]
        public Aow.Units.AoWComp.TaowTextAlign TextAlign { get => FTextAlign; set => FTextAlign = value; }
    }

    public static partial class AoWMemo
    {
        // AoWMemo.pas:864
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AoWMemo.TAOWMemo)]);
        }
    }
}
