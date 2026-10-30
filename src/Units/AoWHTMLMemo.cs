// Translated from AoWHTMLMemo.pas.
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

namespace Aow.Units.AoWHTMLMemo
{
    public enum TIntParseMode : byte
    {
        pNone = 0,
        pStart = 1,
        pFont = 2,
        pItemHeight = 3,
        pColor = 4,
        pBG = 5,
        pInBG = 6,
        pBGIndex = 7,
        pSpace = 8,
        pAlign = 9
    }

    public enum TaowAlign : byte
    {
        aLeft = 0,
        aCenter = 1,
        aRight = 2
    }

    // a style change in the text. FHeight (the line height), FColor, FFont (the font file, '' for
    // the current font), FAlign, FPos (the character of the line where it starts), FLine, FSpacing
    public partial class TChangeInfo : Aow.Rtl.TObject
    {
        public int FHeight;
        public int FColor;
        public Aow.Rtl.AnsiString FFont;
        public Aow.Units.AoWComp.TaowTextAlign FAlign;
        public int FPos;
        public int FLine;
        public int FSpacing;
    }

    // Fields: F1EC (a list that is created and freed only), FChanges (TChangeInfo), FLines (the
    // wrapped lines), FItems (the text), FVScrollBar, FFilename, FScrollWidth, FBackDrawn (the back
    // surface holds the text), FImages (the background textures), FTopItem (the first pixel row
    // shown), FFonts (the fonts loaded, TImageLibraryFont), FAOWWindow, FTextWidth, FTextHeight
    // (the size of the back surface)
    public partial class TaowHTMLMemo : Aow.Units.AoWBaseWin.TAOWBaseWindow
    {
        public Aow.Rtl.TList F1EC;
        public Aow.Rtl.TList FChanges;
        public Aow.Rtl.TStringList FLines;
        public Aow.Rtl.TStringList FItems;
        public Aow.Units.AoWVScroll.TAOWVScrollBar FVScrollBar;
        public Aow.Rtl.AnsiString FFilename;
        public int FScrollWidth;
        public bool FBackDrawn;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FImages;
        public int FTopItem;
        public Aow.Rtl.TList FFonts;
        public Aow.Units.AoWBaseWin.TAOWBaseWindow FAOWWindow;
        public int FTextWidth;
        public int FTextHeight;
        // AoWHTMLMemo.pas:93
        public TaowHTMLMemo(TComponent owner) : base(owner)
        {
            var call__1 = new Aow.Rtl.TStringList();
            FLines = call__1;
            var call__2 = new Aow.Rtl.TStringList();
            FItems = call__2;
            FItems.OnChange = OnChangeText;
            var call__3 = new Aow.Rtl.TList();
            FChanges = call__3;
            var call__4 = new Aow.Rtl.TList();
            F1EC = call__4;
            var call__5 = new Aow.Rtl.TList();
            FFonts = call__5;
            var call__6 = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this);
            FImages = call__6;
        }

        // AoWHTMLMemo.pas:105
        public override void Destroy()
        {
            int I = default;
            FLines.Destroy();
            FItems.Destroy();
            F1EC.Free();
            {
                I = 0;
                int end__7 = FChanges.Count - 1;
                for (; I <= end__7; I += 1)
                {
                    var call__8 = FChanges[I];
                    ((Aow.Units.AoWHTMLMemo.TChangeInfo)call__8).Free();
                }
            }

            {
                I = 0;
                int end__9 = FFonts.Count - 1;
                for (; I <= end__9; I += 1)
                {
                    var call__10 = FFonts[I];
                    ((Aow.Units.ImageLib.TImageLibraryFont)call__10).Free();
                }
            }

            FFonts.Free();
            FChanges.Free();
            FImages.Free();
            base.Destroy();
        }

        // AoWHTMLMemo.pas:174
        public override void OnChangeText(Aow.Rtl.TObject Sender)
        {
            SetListOff(0);
            FSized = false;
        }

        // AoWHTMLMemo.pas:210
        // { SetSize parses the text into FLines and FChanges, wrapping the lines at the width of the
//   window, and sets up the scroll bar }
        public override void SetSize()
        {
            Aow.Units.AoWHTMLMemo.TChangeInfo CI = default;
            int H = default;
            int LineIdx = default;
            Aow.Units.AoWHTMLMemo.TIntParseMode State = default;
            int I = default;
            int J = default;
            int Last = default;
            int TextStart = default;
            int TextEnd = default;
            int TagStart = default;
            byte C = default;
            ReadOnlySpan<byte> text = default;
            Aow.Rtl.AnsiString S = default;
            int K = default;
            int V = default;
            void NewLine()
            {
                FLines.Add((AnsiString)"");
                H = H + CI.FHeight;
                LineIdx += 1;
            }

            void AddText(Aow.Rtl.AnsiString S)
            {
                int TW = default;
                int W = default;
                int Start = default;
                Aow.Rtl.AnsiString Word = default;
                int I = default;
                int Len = default;
                if (FAOWFont != null)
                {
                    I = 0;
                    Start = 0;
                    Len = S.Length;
                    var call__11 = FLines[LineIdx];
                    var call__12 = FAOWFont.GetTextWidth(call__11);
                    W = (call__12 + (FBorderWidth * 2)) + FScrollWidth;
                    while (I < Len)
                    {
                        if ((S[I] == 32) || (I == (Len - 1)))
                        {
                            if (I == (Len - 1))
                            {
                                I += 1;
                            }

                            Word = Aow.Rtl.Rtl.Copy(S, Start + 1, I - Start);
                            var call__13 = FAOWFont.GetTextWidth(Word);
                            TW = call__13;
                            if ((W + TW) > FWinWidth)
                            {
                                NewLine();
                                var call__14 = FLines[LineIdx];
                                FLines[LineIdx] = call__14 + Word;
                                W = ((FBorderWidth * 2) + FScrollWidth) + TW;
                            }
                            else
                            {
                                var call__15 = FLines[LineIdx];
                                FLines[LineIdx] = call__15 + Word;
                                W = W + TW;
                            }

                            Start = I;
                        }

                        I += 1;
                    }
                }
            }

            void NewChange()
            {
                Aow.Units.AoWHTMLMemo.TChangeInfo N = default;
                N = new Aow.Units.AoWHTMLMemo.TChangeInfo();
                N.FHeight = CI.FHeight;
                N.FColor = CI.FColor;
                N.FFont = CI.FFont;
                N.FAlign = CI.FAlign;
                N.FSpacing = CI.FSpacing;
                N.FPos = CI.FPos;
                N.FLine = CI.FLine;
                FChanges.Add(N);
                CI = N;
            }

            void ParseTag(Aow.Rtl.AnsiString Tag)
            {
                Tag = Aow.Rtl.Rtl.UpperCase(Tag);
                switch (State)
                {
                    case Aow.Units.AoWHTMLMemo.TIntParseMode.pInBG:
                        if (Tag == ((AnsiString)"INDEX"))
                        {
                            State = Aow.Units.AoWHTMLMemo.TIntParseMode.pBGIndex;
                        }

                        break;
                    default:
                        if (Tag == AnsiString.FromBytes(80))
                        {
                            NewLine();
                            NewLine();
                        }
                        else
                        {
                            if (Tag == ((AnsiString)"BR"))
                            {
                                NewLine();
                            }
                            else
                            {
                                if (Tag == ((AnsiString)"FONT"))
                                {
                                    State = Aow.Units.AoWHTMLMemo.TIntParseMode.pFont;
                                }
                                else
                                {
                                    if (Tag == ((AnsiString)"ITEMHEIGHT"))
                                    {
                                        State = Aow.Units.AoWHTMLMemo.TIntParseMode.pItemHeight;
                                    }
                                    else
                                    {
                                        if (Tag == ((AnsiString)"COLOR"))
                                        {
                                            State = Aow.Units.AoWHTMLMemo.TIntParseMode.pColor;
                                        }
                                        else
                                        {
                                            if (Tag == ((AnsiString)"SPACING"))
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pSpace;
                                            }
                                            else
                                            {
                                                if (Tag == ((AnsiString)"ALIGN"))
                                                {
                                                    State = Aow.Units.AoWHTMLMemo.TIntParseMode.pAlign;
                                                }
                                                else
                                                {
                                                    if (Tag == ((AnsiString)"BACKGROUND"))
                                                    {
                                                        State = Aow.Units.AoWHTMLMemo.TIntParseMode.pBG;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;
                }
            }

            FScrollWidth = 0;
            if (FVScrollBar != null)
            {
                FScrollWidth = FVScrollBar.WinWidth;
                FVScrollBar.Position = FTopItem;
            }

            H = FTextHeight;
            FLines.Clear();
            State = Aow.Units.AoWHTMLMemo.TIntParseMode.pNone;
            var call__16 = FItems.Count;
            bool bool__17 = (call__16 > 0);
            if (bool__17)
            {
                var call__18 = FItems[0];
                bool__17 = (Aow.Rtl.Rtl.UpperCase(Aow.Rtl.Rtl.Copy(call__18, 1, 5)) == ((AnsiString)"<INT>"));
            }

            if (bool__17)
            {
                FLines.Add((AnsiString)"");
                H = FBorderHeight * 2;
                FImages.Clear();
                {
                    I = 0;
                    int end__19 = FChanges.Count - 1;
                    for (; I <= end__19; I += 1)
                    {
                        var call__20 = FChanges[I];
                        ((Aow.Units.AoWHTMLMemo.TChangeInfo)call__20).Free();
                    }
                }

                FChanges.Clear();
                CI = new Aow.Units.AoWHTMLMemo.TChangeInfo();
                CI.FHeight = 10;
                CI.FColor = 16777215;
                CI.FFont = (AnsiString)"";
                CI.FAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft;
                CI.FPos = 0;
                CI.FLine = 0;
                CI.FSpacing = 0;
                FChanges.Add(CI);
                LineIdx = 0;
                {
                    I = 0;
                    int end__21 = FItems.Count - 1;
                    for (; I <= end__21; I += 1)
                    {
                        var call__22 = FItems[I];
                        AnsiString item = Aow.Rtl.Rtl.NullTerminatedText(call__22);
                        text = Aow.Rtl.Rtl.StringBytes(item);
                        Last = text.Length - 1;
                        TextStart = 0;
                        TextEnd = -1;
                        TagStart = 0;
                        {
                            J = 0;
                            int end__23 = Last;
                            for (; J <= end__23; J += 1)
                            {
                                C = unchecked((byte)text[J]);
                                switch (State)
                                {
                                    case Aow.Units.AoWHTMLMemo.TIntParseMode.pNone:
                                        switch (C)
                                        {
                                            case 60:
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pStart;
                                                TextEnd = J - 1;
                                                TagStart = J + 1;
                                                break;
                                        }

                                        break;
                                    case Aow.Units.AoWHTMLMemo.TIntParseMode.pStart:
                                    case Aow.Units.AoWHTMLMemo.TIntParseMode.pInBG:
                                        switch (C)
                                        {
                                            case 62:
                                                ParseTag(Aow.Rtl.Rtl.Copy(item, TagStart + 1, J - TagStart));
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pNone;
                                                TextStart = J + 1;
                                                break;
                                            case 32:
                                            case 61:
                                                ParseTag(Aow.Rtl.Rtl.Copy(item, TagStart + 1, J - TagStart));
                                                TagStart = J + 1;
                                                break;
                                        }

                                        break;
                                    case Aow.Units.AoWHTMLMemo.TIntParseMode.pFont:
                                        if (TSet<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                        {
                                            var call__24 = FLines[LineIdx];
                                            CI.FPos = call__24.Length;
                                            CI.FLine = LineIdx;
                                            CI.FFont = Aow.Rtl.Rtl.Copy(item, TagStart + 1, J - TagStart);
                                            if (CI.FFont != ((AnsiString)""))
                                            {
                                                var call__25 = LoadFont(CI.FFont);
                                                if (!call__25)
                                                {
                                                    CI.FFont = (AnsiString)"";
                                                }
                                            }

                                            NewChange();
                                            if (C == 62)
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pNone;
                                                TextStart = J + 1;
                                            }
                                            else
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pStart;
                                                TagStart = J + 1;
                                            }
                                        }

                                        break;
                                    case Aow.Units.AoWHTMLMemo.TIntParseMode.pItemHeight:
                                        if (TSet<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                        {
                                            var call__26 = FLines[LineIdx];
                                            CI.FPos = call__26.Length;
                                            CI.FLine = LineIdx;
                                            try
                                            {
                                                CI.FHeight = Aow.Rtl.Rtl.StrToInt(Aow.Rtl.Rtl.Copy(item, TagStart + 1, J - TagStart));
                                            }
                                            catch (System.Exception)
                                            {
                                            }

                                            NewChange();
                                            if (C == 62)
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pNone;
                                                TextStart = J + 1;
                                            }
                                            else
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pStart;
                                                TagStart = J + 1;
                                            }
                                        }

                                        break;
                                    case Aow.Units.AoWHTMLMemo.TIntParseMode.pColor:
                                        if (TSet<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                        {
                                            var call__27 = FLines[LineIdx];
                                            CI.FPos = call__27.Length;
                                            CI.FLine = LineIdx;
                                            if ((J - TagStart) == 6)
                                            {
                                                CI.FColor = 0;
                                                {
                                                    K = 0;
                                                    byte end__28 = 5;
                                                    for (; K <= end__28; K += 1)
                                                    {
                                                        V = (int)text[TagStart + K];
                                                        if (V < 58)
                                                        {
                                                            V = V - 48;
                                                        }
                                                        else
                                                        {
                                                            V = V - 55;
                                                        }

                                                        CI.FColor = (CI.FColor << 4) + V;
                                                    }
                                                }

                                                NewChange();
                                            }

                                            if (C == 62)
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pNone;
                                                TextStart = J + 1;
                                            }
                                            else
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pStart;
                                                TagStart = J + 1;
                                            }
                                        }

                                        break;
                                    case Aow.Units.AoWHTMLMemo.TIntParseMode.pSpace:
                                        if (TSet<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                        {
                                            var call__29 = FLines[LineIdx];
                                            CI.FPos = call__29.Length;
                                            CI.FLine = LineIdx;
                                            try
                                            {
                                                CI.FSpacing = Aow.Rtl.Rtl.StrToInt(Aow.Rtl.Rtl.Copy(item, TagStart + 1, J - TagStart));
                                                if (FAOWFont != null)
                                                {
                                                    FAOWFont.CharacterDistance = CI.FSpacing;
                                                }
                                            }
                                            catch (System.Exception)
                                            {
                                            }

                                            NewChange();
                                            if (C == 62)
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pNone;
                                                TextStart = J + 1;
                                            }
                                            else
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pStart;
                                                TagStart = J + 1;
                                            }
                                        }

                                        break;
                                    case Aow.Units.AoWHTMLMemo.TIntParseMode.pAlign:
                                        if (TSet<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                        {
                                            S = Aow.Rtl.Rtl.UpperCase(Aow.Rtl.Rtl.Copy(item, TagStart + 1, J - TagStart));
                                            var call__30 = FLines[LineIdx];
                                            CI.FPos = call__30.Length;
                                            CI.FLine = LineIdx;
                                            if (S == ((AnsiString)"RIGHT"))
                                            {
                                                CI.FAlign = Aow.Units.AoWComp.TaowTextAlign.taRight;
                                            }
                                            else
                                            {
                                                if (S == ((AnsiString)"CENTER"))
                                                {
                                                    CI.FAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter;
                                                }
                                                else
                                                {
                                                    CI.FAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft;
                                                }
                                            }

                                            NewChange();
                                            if (C == 62)
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pNone;
                                                TextStart = J + 1;
                                            }
                                            else
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pStart;
                                                TagStart = J + 1;
                                            }
                                        }

                                        break;
                                    case Aow.Units.AoWHTMLMemo.TIntParseMode.pBG:
                                        if (TSet<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                        {
                                            var call__31 = Aow.Rtl.Rtl.Application.ExeName;
                                            var call__32 = Aow.Rtl.Rtl.ExtractFilePath(call__31);
                                            var call__33 = Aow.Rtl.Rtl.FileExists(call__32 + Aow.Rtl.Rtl.Copy(item, TagStart + 1, J - TagStart));
                                            if (call__33)
                                            {
                                                var call__34 = Aow.Rtl.Rtl.Application.ExeName;
                                                var call__35 = Aow.Rtl.Rtl.ExtractFilePath(call__34);
                                                FImages.LoadFromFile(call__35 + Aow.Rtl.Rtl.Copy(item, TagStart + 1, J - TagStart));
                                            }

                                            FILIndexTexture = 0;
                                            if (C == 62)
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pNone;
                                                TextStart = J + 1;
                                            }
                                            else
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pInBG;
                                                TagStart = J + 1;
                                            }
                                        }

                                        break;
                                    case Aow.Units.AoWHTMLMemo.TIntParseMode.pBGIndex:
                                        if (TSet<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                        {
                                            try
                                            {
                                                FILIndexTexture = Aow.Rtl.Rtl.StrToInt(Aow.Rtl.Rtl.Copy(item, TagStart + 1, J - TagStart));
                                            }
                                            catch (System.Exception)
                                            {
                                            }

                                            if (C == 62)
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pNone;
                                                TextStart = J + 1;
                                            }
                                            else
                                            {
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pInBG;
                                                TagStart = J + 1;
                                            }
                                        }

                                        break;
                                }

                                if (TextEnd >= TextStart)
                                {
                                    AddText(Aow.Rtl.Rtl.Copy(item, TextStart + 1, (TextEnd - TextStart) + 1));
                                    TextStart = TextEnd;
                                }
                            }
                        }

                        switch (State)
                        {
                            case Aow.Units.AoWHTMLMemo.TIntParseMode.pNone:
                                if (TextStart <= Last)
                                {
                                    AddText(Aow.Rtl.Rtl.Copy(item, TextStart + 1, (Last - TextStart) + 1));
                                }

                                break;
                            case Aow.Units.AoWHTMLMemo.TIntParseMode.pStart:
                                if (TagStart <= Last)
                                {
                                    ParseTag(Aow.Rtl.Rtl.Copy(item, TagStart + 1, Last - TagStart));
                                }

                                break;
                        }
                    }
                }

                H = H + CI.FHeight;
            }

            if (((FWinWidth - FScrollWidth) != FTextWidth) || (FTextHeight != H))
            {
                FTextWidth = FWinWidth - FScrollWidth;
                FTextHeight = H;
                if (FTextHeight < FWinHeight)
                {
                    FTextHeight = FWinHeight;
                }

                if (FBackSurface != null)
                {
                    FBackSurface.Resize(FTextWidth, FTextHeight);
                }
            }

            FSurfaceValid = false;
            FBackDrawn = false;
            if (FVScrollBar != null)
            {
                if (FAOWWindow != FVScrollBar.AOWWindow)
                {
                    FVScrollBar.AOWWindow = FAOWWindow;
                }

                FVScrollBar.Min = 0;
                FVScrollBar.Max = FTextHeight;
                FVScrollBar.Priority = FPriority + 1;
                FVScrollBar.Position = FTopItem;
                FVScrollBar.NumberVisible = FWinHeight;
                FVScrollBar.WinHeight = FWinHeight;
                FVScrollBar.LargeChange = FWinHeight;
                FVScrollBar.SmallChange = 20;
                FVScrollBar.Visible = FVisible;
                FVScrollBar.FSized = false;
                FVScrollBar.WinLeft = (FWinLeft + FWinWidth) - FVScrollBar.WinWidth;
                FVScrollBar.WinTop = FWinTop;
            }

            base.SetSize();
        }

        // AoWHTMLMemo.pas:747
        // { Update follows the scroll bar; a masked memo asks its owner to redraw it under the text }
        public override void Update()
        {
            Aow.Rtl.TRect R = default;
            if (FVisible)
            {
                bool bool__36 = (FVScrollBar != null);
                if (bool__36)
                {
                    var call__37 = FLines.Count;
                    bool__36 = (call__37 > 0);
                }

                if (bool__36)
                {
                    if (FVScrollBar.Position != FTopItem)
                    {
                        SetListOff(FVScrollBar.Position);
                    }
                }

                if (!FDrawn)
                {
                    FSurfaceValid = false;
                }

                FDrawn = false;
            }

            base.Update();
            if ((FVisible && FMasked) && !FSurfaceValid)
            {
                if (FAOWOwner.FAOWOwner.FAOWOwner == null)
                {
                    var call__38 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, (FWinLeft + FWinWidth) - 1, (FWinTop + FWinHeight) - 1);
                    R = call__38;
                }
                else
                {
                    var call__39 = Aow.Rtl.Rtl.Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, ((FAOWOwner.FWinLeft + FWinLeft) + FWinWidth) - 1, ((FAOWOwner.FWinTop + FWinTop) + FWinHeight) - 1);
                    R = call__39;
                }

                FAOWOwner.AddDrawRect(ref R, this);
            }
        }

        // AoWHTMLMemo.pas:122
        public override void InvalidateRect(int Index, ref Aow.Rtl.TRect ARect, Aow.Units.AoWComp.TAoWComponent Sender)
        {
            if (FValid && FVisible)
            {
                AddDrawRect(ref ARect, Sender);
            }
        }

        // AoWHTMLMemo.pas:128
        public override void SetOwner(Aow.Units.AoWComp.TAoWComponent Value)
        {
            base.SetOwner(Value);
            FAOWWindow = (Aow.Units.AoWBaseWin.TAOWBaseWindow)FAOWOwner;
        }

        // AoWHTMLMemo.pas:584
        // { Draw renders the text into the back surface when it changed, then blits the dirty rectangles
//   of the window (or the whole visible part) from it }
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int I = default;
            int OldLeft = default;
            int OldTop = default;
            int Pos = default;
            int LH = default;
            int X = default;
            int Y = default;
            Aow.Units.AoWComp.TaowTextAlign Align = default;
            Aow.Rtl.AnsiString FontName = default;
            Aow.Units.GFXE.TSurfaceDesc BackSD = default;
            Aow.Rtl.TRect SrcR = default;
            int Line = default;
            Aow.Units.AoWHTMLMemo.TChangeInfo CI = default;
            Aow.Units.AoWComp.TaowRect R = default;
            try
            {
                if (FValid && FVisible)
                {
                    OldLeft = FWinLeft;
                    OldTop = FWinTop;
                    FWinLeft = 0;
                    FWinTop = 0;
                    if (FBackSurface != null)
                    {
                        var call__40 = FBackSurface.PixelFormatID;
                        if (call__40 != SurfDesc.PixelFormatID)
                        {
                            FBackSurface.Free();
                            FBackSurface = null;
                        }
                    }

                    if (FBackSurface == null)
                    {
                        var call__41 = new Aow.Units.GFXE.TDataSurface(FTextWidth, FTextHeight, SurfDesc.PixelFormatID, null);
                        FBackSurface = call__41;
                        FBackDrawn = false;
                    }

                    if (!FBackDrawn)
                    {
                        if (FImages.Count <= FILIndexTexture)
                        {
                            if (FMasked)
                            {
                                FBackSurface.Cls(FMaskColor);
                            }
                            else
                            {
                                FBackSurface.Cls(0);
                            }

                            FBackSurface.Lock(ref BackSD);
                        }
                        else
                        {
                            FBackSurface.Lock(ref BackSD);
                            var call__42 = Aow.Rtl.Rtl.Rect(0, 0, FTextWidth, FTextHeight);
                            FImages.DrawILT(FILIndexTexture, call__42, ref BackSD);
                        }

                        Line = 1;
                        var call__43 = FChanges[0];
                        CI = (Aow.Units.AoWHTMLMemo.TChangeInfo)call__43;
                        Align = Aow.Units.AoWComp.TaowTextAlign.taLeft;
                        FontName = (AnsiString)"";
                        X = FBorderWidth;
                        Y = FBorderHeight;
                        LH = 10;
                        {
                            I = 0;
                            int end__44 = FLines.Count - 1;
                            for (; I <= end__44; I += 1)
                            {
                                Pos = 0;
                                switch (Align)
                                {
                                    case Aow.Units.AoWComp.TaowTextAlign.taLeft:
                                        X = FBorderWidth;
                                        break;
                                    case Aow.Units.AoWComp.TaowTextAlign.taCenter:
                                        X = 0;
                                        break;
                                    case Aow.Units.AoWComp.TaowTextAlign.taRight:
                                        X = FTextWidth - FBorderWidth;
                                        break;
                                }

                                while ((CI.FLine == I) && (Line <= FChanges.Count))
                                {
                                    if ((FontName != ((AnsiString)"")) && (CI.FPos > Pos))
                                    {
                                        switch (Align)
                                        {
                                            case Aow.Units.AoWComp.TaowTextAlign.taLeft:
                                                var call__45 = FLines[I];
                                                FAOWFont.SetText(X, Y, Aow.Rtl.Rtl.Copy(call__45, Pos + 1, CI.FPos - Pos), BackSD);
                                                var call__46 = FLines[I];
                                                var call__47 = FAOWFont.GetTextWidth(Aow.Rtl.Rtl.Copy(call__46, Pos + 1, CI.FPos - Pos));
                                                X = X + call__47;
                                                break;
                                            case Aow.Units.AoWComp.TaowTextAlign.taCenter:
                                                var call__48 = FLines[I];
                                                var call__49 = FAOWFont.GetTextWidth(Aow.Rtl.Rtl.Copy(call__48, Pos + 1, CI.FPos - Pos));
                                                var call__50 = FLines[I];
                                                FAOWFont.SetText((FTextWidth / 2) - (call__49 / 2), Y, Aow.Rtl.Rtl.Copy(call__50, Pos + 1, CI.FPos - Pos), BackSD);
                                                break;
                                            case Aow.Units.AoWComp.TaowTextAlign.taRight:
                                                var call__51 = FLines[I];
                                                var call__52 = FAOWFont.GetTextWidth(Aow.Rtl.Rtl.Copy(call__51, Pos + 1, CI.FPos - Pos));
                                                var call__53 = FLines[I];
                                                FAOWFont.SetText(X - call__52, Y, Aow.Rtl.Rtl.Copy(call__53, Pos + 1, CI.FPos - Pos), BackSD);
                                                var call__54 = FLines[I];
                                                var call__55 = FAOWFont.GetTextWidth(Aow.Rtl.Rtl.Copy(call__54, Pos + 1, CI.FPos - Pos));
                                                X = X - call__55;
                                                break;
                                        }
                                    }

                                    Pos = CI.FPos;
                                    if (CI.FFont != ((AnsiString)""))
                                    {
                                        LoadFont(CI.FFont);
                                    }

                                    FAOWFont.FontColorPaletteIndex = 1;
                                    FAOWFont.Color = CI.FColor;
                                    FAOWFont.CharacterDistance = CI.FSpacing;
                                    FontName = CI.FFont;
                                    LH = CI.FHeight;
                                    if (CI.FAlign != Align)
                                    {
                                        switch (Align)
                                        {
                                            case Aow.Units.AoWComp.TaowTextAlign.taLeft:
                                                X = FBorderWidth;
                                                break;
                                            case Aow.Units.AoWComp.TaowTextAlign.taCenter:
                                                X = 0;
                                                break;
                                            case Aow.Units.AoWComp.TaowTextAlign.taRight:
                                                X = FTextWidth - FBorderWidth;
                                                break;
                                        }
                                    }

                                    Align = CI.FAlign;
                                    if (Line < FChanges.Count)
                                    {
                                        var call__56 = FChanges[Line];
                                        CI = (Aow.Units.AoWHTMLMemo.TChangeInfo)call__56;
                                    }

                                    Line += 1;
                                }

                                if (FontName != ((AnsiString)""))
                                {
                                    switch (Align)
                                    {
                                        case Aow.Units.AoWComp.TaowTextAlign.taLeft:
                                            var call__57 = FLines[I];
                                            var call__58 = FLines[I];
                                            FAOWFont.SetText(X, Y, Aow.Rtl.Rtl.Copy(call__58, Pos + 1, call__57.Length - Pos), BackSD);
                                            break;
                                        case Aow.Units.AoWComp.TaowTextAlign.taCenter:
                                            var call__59 = FLines[I];
                                            var call__60 = FLines[I];
                                            var call__61 = FAOWFont.GetTextWidth(Aow.Rtl.Rtl.Copy(call__60, Pos + 1, call__59.Length - Pos));
                                            var call__62 = FLines[I];
                                            var call__63 = FLines[I];
                                            FAOWFont.SetText((FTextWidth / 2) - (call__61 / 2), Y, Aow.Rtl.Rtl.Copy(call__63, Pos + 1, call__62.Length - Pos), BackSD);
                                            break;
                                        case Aow.Units.AoWComp.TaowTextAlign.taRight:
                                            var call__64 = FLines[I];
                                            var call__65 = FLines[I];
                                            var call__66 = FAOWFont.GetTextWidth(Aow.Rtl.Rtl.Copy(call__65, Pos + 1, call__64.Length - Pos));
                                            var call__67 = FLines[I];
                                            var call__68 = FLines[I];
                                            FAOWFont.SetText(X - call__66, Y, Aow.Rtl.Rtl.Copy(call__68, Pos + 1, call__67.Length - Pos), BackSD);
                                            break;
                                    }
                                }

                                Y = Y + LH;
                            }
                        }

                        FBackSurface.Unlock();
                        FBackDrawn = true;
                    }

                    FWinLeft = OldLeft;
                    FWinTop = OldTop;
                    if (FOnPreDraw != null)
                    {
                        FOnPreDraw(this, SurfDesc);
                    }

                    FBackSurface.Lock(ref BackSD);
                    if (FSurfaceValid)
                    {
                        {
                            I = 0;
                            int end__69 = FDrawCount - 1;
                            for (; I <= end__69; I += 1)
                            {
                                var call__70 = FDrawList[I];
                                R = (Aow.Units.AoWComp.TaowRect)call__70;
                                if ((FAOWOwner.FWinLeft + FWinLeft) > R.R.Left)
                                {
                                    R.R.Left = FAOWOwner.FWinLeft + FWinLeft;
                                }

                                if ((FAOWOwner.FWinTop + FWinTop) > R.R.Top)
                                {
                                    R.R.Top = FAOWOwner.FWinTop + FWinTop;
                                }

                                if (((FAOWOwner.FWinLeft + FWinLeft) + FWinWidth) < R.R.Right)
                                {
                                    R.R.Right = (FAOWOwner.FWinLeft + FWinLeft) + FWinWidth;
                                }

                                if (((FAOWOwner.FWinTop + FWinTop) + FWinHeight) < R.R.Bottom)
                                {
                                    R.R.Bottom = (FAOWOwner.FWinTop + FWinTop) + FWinHeight;
                                }

                                var call__71 = Aow.Rtl.Rtl.Rect((R.R.Left - FAOWOwner.FWinLeft) - FWinLeft, ((FTopItem + R.R.Top) - FAOWOwner.FWinTop) - FWinTop, (R.R.Right - FAOWOwner.FWinLeft) - FWinLeft, ((FTopItem + R.R.Bottom) - FAOWOwner.FWinTop) - FWinTop);
                                SrcR = call__71;
                                if (FMasked)
                                {
                                    Aow.Units.GFXE.GFXE.BltSpriteClipped16(BackSD, SrcR, SurfDesc, R.R.Left, R.R.Top, FMaskColor);
                                }
                                else
                                {
                                    Aow.Units.GFXE.GFXE.BltSDRectClipped(BackSD, SrcR, SurfDesc, R.R.Left, R.R.Top);
                                }
                            }
                        }
                    }
                    else
                    {
                        var call__72 = Aow.Rtl.Rtl.Rect(0, FTopItem, FWinWidth, FTopItem + FWinHeight);
                        SrcR = call__72;
                        if (FMasked)
                        {
                            Aow.Units.GFXE.GFXE.BltSpriteClipped16(BackSD, SrcR, SurfDesc, FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FMaskColor);
                        }
                        else
                        {
                            Aow.Units.GFXE.GFXE.BltSDRectClipped(BackSD, SrcR, SurfDesc, FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop);
                        }
                    }

                    FBackSurface.Unlock();
                    if (FOnDraw != null)
                    {
                        FOnDraw(this, SurfDesc);
                    }

                    FSurfaceValid = true;
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during Draw", ((AnsiString)"Error - ") + Name, (int)0);
            }
        }

        // AoWHTMLMemo.pas:134
        public void SetAOWWindow(Aow.Units.AoWBaseWin.TAOWBaseWindow Value)
        {
            SetOwner(Value);
        }

        // AoWHTMLMemo.pas:139
        public void SetFStrings(Aow.Rtl.TStringList Value)
        {
            FItems.Clear();
            FItems.AddStrings(Value);
        }

        // AoWHTMLMemo.pas:145
        public void SetVScroll(Aow.Units.AoWVScroll.TAOWVScrollBar Value)
        {
            if (Value != FVScrollBar)
            {
                FVScrollBar = Value;
                if (FVScrollBar != null)
                {
                    FVScrollBar.Priority = FPriority;
                }

                FSized = false;
            }
        }

        // AoWHTMLMemo.pas:156
        public void SetListOff(int Value)
        {
            if (Value != FTopItem)
            {
                FTopItem = Value;
                FSurfaceValid = false;
            }
        }

        // AoWHTMLMemo.pas:165
        public void SetFileName(Aow.Rtl.AnsiString Value)
        {
            bool bool__73 = (Value != FFilename);
            if (bool__73)
            {
                var call__74 = Aow.Rtl.Rtl.FileExists(Value);
                bool__73 = call__74;
            }

            if (bool__73)
            {
                FFilename = Value;
                FItems.LoadFromFile(FFilename);
            }
        }

        // AoWHTMLMemo.pas:182
        // { LoadFont makes the font in FileName (relative to the program) the current font, loading it
//   the first time }
        public bool LoadFont(Aow.Rtl.AnsiString FileName)
        {
            bool Result = default;
            Aow.Units.ImageLib.TImageLibraryFont Font = default;
            int I = default;
            ;
            var call__75 = Aow.Rtl.Rtl.Application.ExeName;
            var call__76 = Aow.Rtl.Rtl.ExtractFilePath(call__75);
            FileName = call__76 + FileName;
            Font = null;
            I = 0;
            while ((I < FFonts.Count) && (Font == null))
            {
                var call__77 = FFonts[I];
                if (((Aow.Units.ImageLib.TImageLibraryFont)call__77).FileName == FileName)
                {
                    var call__78 = FFonts[I];
                    Font = (Aow.Units.ImageLib.TImageLibraryFont)call__78;
                }

                I += 1;
            }

            if ((Font == null) && Aow.Rtl.Rtl.FileExists(FileName))
            {
                Font = new Aow.Units.ImageLib.TImageLibraryFont(this);
                Font.FontColorPaletteIndex = 1;
                Font.LoadFromFile(FileName);
                FFonts.Add(Font);
            }

            if (Font != null)
            {
                FAOWFont = Font;
            }

            Result = Font != null;
            return Result;
        }

        public Aow.Rtl.TStringList Items { get => FItems; set => SetFStrings(value); }
        public int ListOffset { get => FTopItem; set => SetListOff(value); }

        [Published]
        public Aow.Units.AoWBaseWin.TAOWBaseWindow AOWWindow { get => FAOWWindow; set => SetAOWWindow(value); }

        [Published]
        public Aow.Units.AoWVScroll.TAOWVScrollBar VScrollBar { get => FVScrollBar; set => SetVScroll(value); }

        [Published]
        public Aow.Rtl.AnsiString Filename { get => FFilename; set => SetFileName(value); }

        [Published]
        public bool FreeSurfaceOnHide { get => FFreeSurfaceOnHide; set => FFreeSurfaceOnHide = value; }

        [Published]
        public int BorderWidth { get => FBorderWidth; set => FBorderWidth = value; }

        [Published]
        public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

        [Published]
        public int MaskColor { get => FMaskColor; set => FMaskColor = value; }
    }

    public static partial class AoWHTMLMemo
    {
        // AoWHTMLMemo.pas:772
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AoWHTMLMemo.TaowHTMLMemo)]);
        }
    }
}
