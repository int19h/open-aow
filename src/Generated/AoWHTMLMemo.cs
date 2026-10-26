// Generated from AoWHTMLMemo.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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

    // { AoWHTMLMemo.TChangeInfo (instance size 32): a style change in the text. +4 FHeight (the line
//     height), +8 FColor, +$C FFont (the font file, '' for the current font), +$10 FAlign, +$14
//     FPos (the character of the line where it starts), +$18 FLine, +$1C FSpacing }
    public partial class TChangeInfo : Aow.Rtl.TObject
    {
        public int FHeight;
        public int FColor;
        public Aow.Rtl.AnsiString FFont;
        public Aow.Units.AoWComp.TaowTextAlign FAlign;
        public int FPos;
        public int FLine;
        public int FSpacing;
        // System.pas:111
        public static Aow.Units.AoWHTMLMemo.TChangeInfo Create()
        {
            return new Aow.Units.AoWHTMLMemo.TChangeInfo();
        }
    }

    // { AoWHTMLMemo.TaowHTMLMemo (instance size 548). Fields: +$1EC F1EC (a list that is created and
//     freed only), +$1F0 FChanges (TChangeInfo), +$1F4 FLines (the wrapped lines), +$1F8 FItems
//     (the text), +$1FC FVScrollBar, +$200 FFilename, +$204 FScrollWidth, +$208 FBackDrawn (the
//     back surface holds the text), +$20C FImages (the background textures), +$210 FTopItem (the
//     first pixel row shown), +$214 FFonts (the fonts loaded, TImageLibraryFont), +$218 FAOWWindow,
//     +$21C FTextWidth, +$220 FTextHeight (the size of the back surface). }
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
        // { TaowHTMLMemo }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
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
            var call__6 = Aow.Units.AOWInterfaceImgLib.TInterfaceIL.Create(this);
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
            Ptr<byte> P = default;
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
                N = Aow.Units.AoWHTMLMemo.TChangeInfo.Create();
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
                CI = Aow.Units.AoWHTMLMemo.TChangeInfo.Create();
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
                        P = Ptr<byte>.Unbox(call__22);
                        Last = AnsiString.FromPChar(P).Length - 1;
                        TextStart = 0;
                        TextEnd = -1;
                        TagStart = 0;
                        {
                            J = 0;
                            int end__23 = Last;
                            for (; J <= end__23; J += 1)
                            {
                                C = unchecked((byte)P[J]);
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
                                                ParseTag(Aow.Rtl.Rtl.Copy(AnsiString.FromPChar(P), TagStart + 1, J - TagStart));
                                                State = Aow.Units.AoWHTMLMemo.TIntParseMode.pNone;
                                                TextStart = J + 1;
                                                break;
                                            case 32:
                                            case 61:
                                                ParseTag(Aow.Rtl.Rtl.Copy(AnsiString.FromPChar(P), TagStart + 1, J - TagStart));
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
                                            CI.FFont = Aow.Rtl.Rtl.Copy(AnsiString.FromPChar(P), TagStart + 1, J - TagStart);
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
                                                CI.FHeight = Aow.Rtl.Rtl.StrToInt(Aow.Rtl.Rtl.Copy(AnsiString.FromPChar(P), TagStart + 1, J - TagStart));
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
                                                        V = (int)P[TagStart + K];
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
                                                CI.FSpacing = Aow.Rtl.Rtl.StrToInt(Aow.Rtl.Rtl.Copy(AnsiString.FromPChar(P), TagStart + 1, J - TagStart));
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
                                            S = Aow.Rtl.Rtl.UpperCase(Aow.Rtl.Rtl.Copy(AnsiString.FromPChar(P), TagStart + 1, J - TagStart));
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
                                            var call__33 = Aow.Rtl.Rtl.FileExists(call__32 + Aow.Rtl.Rtl.Copy(AnsiString.FromPChar(P), TagStart + 1, J - TagStart));
                                            if (call__33)
                                            {
                                                var call__34 = Aow.Rtl.Rtl.Application.ExeName;
                                                var call__35 = Aow.Rtl.Rtl.ExtractFilePath(call__34);
                                                FImages.LoadFromFile(call__35 + Aow.Rtl.Rtl.Copy(AnsiString.FromPChar(P), TagStart + 1, J - TagStart));
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
                                                FILIndexTexture = Aow.Rtl.Rtl.StrToInt(Aow.Rtl.Rtl.Copy(AnsiString.FromPChar(P), TagStart + 1, J - TagStart));
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
                                    AddText(Aow.Rtl.Rtl.Copy(AnsiString.FromPChar(P), TextStart + 1, (TextEnd - TextStart) + 1));
                                    TextStart = TextEnd;
                                }
                            }
                        }

                        switch (State)
                        {
                            case Aow.Units.AoWHTMLMemo.TIntParseMode.pNone:
                                if (TextStart <= Last)
                                {
                                    AddText(Aow.Rtl.Rtl.Copy(AnsiString.FromPChar(P), TextStart + 1, (Last - TextStart) + 1));
                                }

                                break;
                            case Aow.Units.AoWHTMLMemo.TIntParseMode.pStart:
                                if (TagStart <= Last)
                                {
                                    ParseTag(Aow.Rtl.Rtl.Copy(AnsiString.FromPChar(P), TagStart + 1, Last - TagStart));
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
            // { Tried Self/field casts, receiver scope and receiver/local staging; Self/temporary registers still differ. }
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
            // { Tried Self cast, receiver scope and rectangle rebase; extra receiver/rectangle saves remain. }
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
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
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
            // { Tried parameter identity, with and typed/TObject locals; original scrollbar save still differs. }
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
            bool bool__40 = (Value != FFilename);
            if (bool__40)
            {
                var call__41 = Aow.Rtl.Rtl.FileExists(Value);
                bool__40 = call__41;
            }

            if (bool__40)
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
            var call__42 = Aow.Rtl.Rtl.Application.ExeName;
            var call__43 = Aow.Rtl.Rtl.ExtractFilePath(call__42);
            FileName = call__43 + FileName;
            Font = null;
            I = 0;
            while ((I < FFonts.Count) && (Font == null))
            {
                var call__44 = FFonts[I];
                if (((Aow.Units.ImageLib.TImageLibraryFont)call__44).FileName == FileName)
                {
                    var call__45 = FFonts[I];
                    Font = (Aow.Units.ImageLib.TImageLibraryFont)call__45;
                }

                I += 1;
            }

            if ((Font == null) && Aow.Rtl.Rtl.FileExists(FileName))
            {
                Font = Aow.Units.ImageLib.TImageLibraryFont.Create(this);
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

        // AoWHTMLMemo.pas:60
        public static new Aow.Units.AoWHTMLMemo.TaowHTMLMemo Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.AoWHTMLMemo.TaowHTMLMemo o = new Aow.Units.AoWHTMLMemo.TaowHTMLMemo();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
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
