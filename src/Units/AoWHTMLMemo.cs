// Translated from AoWHTMLMemo.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWComp;
using Aow.Units.AOWInterfaceImgLib;
using Aow.Units.AoWBaseWin;

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
    public partial class TChangeInfo : TObject
    {
        public int FHeight;
        public int FColor;
        public AnsiString FFont;
        public TaowTextAlign FAlign;
        public int FPos;
        public int FLine;
        public int FSpacing;
    }

    // Fields: F1EC (a list that is created and freed only), FChanges (TChangeInfo), FLines (the
    // wrapped lines), FItems (the text), FVScrollBar, FFilename, FScrollWidth, FBackDrawn (the back
    // surface holds the text), FImages (the background textures), FTopItem (the first pixel row
    // shown), FFonts (the fonts loaded, TImageLibraryFont), FAOWWindow, FTextWidth, FTextHeight
    // (the size of the back surface)
    public partial class TaowHTMLMemo : TAOWBaseWindow
    {
        public TList F1EC;
        public TList FChanges;
        public TStringList FLines;
        public TStringList FItems;
        public TAOWVScrollBar FVScrollBar;
        public AnsiString FFilename;
        public int FScrollWidth;
        public bool FBackDrawn;
        public TInterfaceIL FImages;
        public int FTopItem;
        public TList FFonts;
        public TAOWBaseWindow FAOWWindow;
        public int FTextWidth;
        public int FTextHeight;
        // AoWHTMLMemo.pas:93
        public TaowHTMLMemo(TComponent owner) : base(owner)
        {
            var call__1 = new TStringList();
            FLines = call__1;
            var call__2 = new TStringList();
            FItems = call__2;
            FItems.OnChange = OnChangeText;
            var call__3 = new TList();
            FChanges = call__3;
            var call__4 = new TList();
            F1EC = call__4;
            var call__5 = new TList();
            FFonts = call__5;
            var call__6 = new TInterfaceIL(this);
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
                    ((TChangeInfo)call__8).Free();
                }
            }

            {
                I = 0;
                int end__9 = FFonts.Count - 1;
                for (; I <= end__9; I += 1)
                {
                    var call__10 = FFonts[I];
                    ((TImageLibraryFont)call__10).Free();
                }
            }

            FFonts.Free();
            FChanges.Free();
            FImages.Free();
            base.Destroy();
        }

        // AoWHTMLMemo.pas:174
        public override void OnChangeText(TObject Sender)
        {
            SetListOff(0);
            FSized = false;
        }

        // AoWHTMLMemo.pas:210
        // { SetSize parses the text into FLines and FChanges, wrapping the lines at the width of the
//   window, and sets up the scroll bar }
        public override void SetSize()
        {
            TChangeInfo CI = default;
            int H = default;
            int LineIdx = default;
            TIntParseMode State = default;
            int I = default;
            int J = default;
            int Last = default;
            int TextStart = default;
            int TextEnd = default;
            int TagStart = default;
            byte C = default;
            ReadOnlySpan<byte> text = default;
            AnsiString S = default;
            int K = default;
            int V = default;
            void NewLine()
            {
                FLines.Add((AnsiString)"");
                H = H + CI.FHeight;
                LineIdx += 1;
            }

            void AddText(AnsiString S)
            {
                int TW = default;
                int W = default;
                int Start = default;
                AnsiString Word = default;
                int I = default;
                int Len = default;
                if (FAOWFont != null)
                {
                    I = 0;
                    Start = 0;
                    Len = S.Length;
                    var call__11 = FLines[LineIdx];
                    var call__12 = FAOWFont.GetTextWidth(call__11);
                    W = call__12 + FBorderWidth * 2 + FScrollWidth;
                    while (I < Len)
                    {
                        if (S[I] == 32 || I == Len - 1)
                        {
                            if (I == Len - 1)
                            {
                                I += 1;
                            }

                            Word = Copy(S, Start + 1, I - Start);
                            var call__13 = FAOWFont.GetTextWidth(Word);
                            TW = call__13;
                            if (W + TW > FWinWidth)
                            {
                                NewLine();
                                var call__14 = FLines[LineIdx];
                                FLines[LineIdx] = call__14 + Word;
                                W = FBorderWidth * 2 + FScrollWidth + TW;
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
                TChangeInfo N = default;
                N = new TChangeInfo();
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

            void ParseTag(AnsiString Tag)
            {
                Tag = UpperCase(Tag);
                switch (State)
                {
                    case TIntParseMode.pInBG:
                        if (Tag == (AnsiString)"INDEX")
                        {
                            State = TIntParseMode.pBGIndex;
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
                            if (Tag == (AnsiString)"BR")
                            {
                                NewLine();
                            }
                            else
                            {
                                if (Tag == (AnsiString)"FONT")
                                {
                                    State = TIntParseMode.pFont;
                                }
                                else
                                {
                                    if (Tag == (AnsiString)"ITEMHEIGHT")
                                    {
                                        State = TIntParseMode.pItemHeight;
                                    }
                                    else
                                    {
                                        if (Tag == (AnsiString)"COLOR")
                                        {
                                            State = TIntParseMode.pColor;
                                        }
                                        else
                                        {
                                            if (Tag == (AnsiString)"SPACING")
                                            {
                                                State = TIntParseMode.pSpace;
                                            }
                                            else
                                            {
                                                if (Tag == (AnsiString)"ALIGN")
                                                {
                                                    State = TIntParseMode.pAlign;
                                                }
                                                else
                                                {
                                                    if (Tag == (AnsiString)"BACKGROUND")
                                                    {
                                                        State = TIntParseMode.pBG;
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
            State = TIntParseMode.pNone;
            var call__16 = FItems.Count;
            bool bool__17 = call__16 > 0;
            if (bool__17)
            {
                var call__18 = FItems[0];
                bool__17 = UpperCase(Copy(call__18, 1, 5)) == (AnsiString)"<INT>";
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
                        ((TChangeInfo)call__20).Free();
                    }
                }

                FChanges.Clear();
                CI = new TChangeInfo();
                CI.FHeight = 10;
                CI.FColor = 16777215;
                CI.FFont = (AnsiString)"";
                CI.FAlign = TaowTextAlign.taLeft;
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
                        AnsiString item = NullTerminatedText(call__22);
                        text = StringBytes(item);
                        Last = text.Length - 1;
                        TextStart = 0;
                        TextEnd = -1;
                        TagStart = 0;
                        {
                            J = 0;
                            int end__23 = Last;
                            for (; J <= end__23; J += 1)
                            {
                                C = text[J];
                                switch (State)
                                {
                                    case TIntParseMode.pNone:
                                        switch (C)
                                        {
                                            case 60:
                                                State = TIntParseMode.pStart;
                                                TextEnd = J - 1;
                                                TagStart = J + 1;
                                                break;
                                        }

                                        break;
                                    case TIntParseMode.pStart:
                                    case TIntParseMode.pInBG:
                                        switch (C)
                                        {
                                            case 62:
                                                ParseTag(Copy(item, TagStart + 1, J - TagStart));
                                                State = TIntParseMode.pNone;
                                                TextStart = J + 1;
                                                break;
                                            case 32:
                                            case 61:
                                                ParseTag(Copy(item, TagStart + 1, J - TagStart));
                                                TagStart = J + 1;
                                                break;
                                        }

                                        break;
                                    case TIntParseMode.pFont:
                                        if (TSet<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                        {
                                            var call__24 = FLines[LineIdx];
                                            CI.FPos = call__24.Length;
                                            CI.FLine = LineIdx;
                                            CI.FFont = Copy(item, TagStart + 1, J - TagStart);
                                            if (CI.FFont != (AnsiString)"")
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
                                                State = TIntParseMode.pNone;
                                                TextStart = J + 1;
                                            }
                                            else
                                            {
                                                State = TIntParseMode.pStart;
                                                TagStart = J + 1;
                                            }
                                        }

                                        break;
                                    case TIntParseMode.pItemHeight:
                                        if (TSet<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                        {
                                            var call__26 = FLines[LineIdx];
                                            CI.FPos = call__26.Length;
                                            CI.FLine = LineIdx;
                                            try
                                            {
                                                CI.FHeight = StrToInt(Copy(item, TagStart + 1, J - TagStart));
                                            }
                                            catch (System.Exception)
                                            {
                                            }

                                            NewChange();
                                            if (C == 62)
                                            {
                                                State = TIntParseMode.pNone;
                                                TextStart = J + 1;
                                            }
                                            else
                                            {
                                                State = TIntParseMode.pStart;
                                                TagStart = J + 1;
                                            }
                                        }

                                        break;
                                    case TIntParseMode.pColor:
                                        if (TSet<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                        {
                                            var call__27 = FLines[LineIdx];
                                            CI.FPos = call__27.Length;
                                            CI.FLine = LineIdx;
                                            if (J - TagStart == 6)
                                            {
                                                CI.FColor = 0;
                                                {
                                                    K = 0;
                                                    byte end__28 = 5;
                                                    for (; K <= end__28; K += 1)
                                                    {
                                                        V = text[TagStart + K];
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
                                                State = TIntParseMode.pNone;
                                                TextStart = J + 1;
                                            }
                                            else
                                            {
                                                State = TIntParseMode.pStart;
                                                TagStart = J + 1;
                                            }
                                        }

                                        break;
                                    case TIntParseMode.pSpace:
                                        if (TSet<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                        {
                                            var call__29 = FLines[LineIdx];
                                            CI.FPos = call__29.Length;
                                            CI.FLine = LineIdx;
                                            try
                                            {
                                                CI.FSpacing = StrToInt(Copy(item, TagStart + 1, J - TagStart));
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
                                                State = TIntParseMode.pNone;
                                                TextStart = J + 1;
                                            }
                                            else
                                            {
                                                State = TIntParseMode.pStart;
                                                TagStart = J + 1;
                                            }
                                        }

                                        break;
                                    case TIntParseMode.pAlign:
                                        if (TSet<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                        {
                                            S = UpperCase(Copy(item, TagStart + 1, J - TagStart));
                                            var call__30 = FLines[LineIdx];
                                            CI.FPos = call__30.Length;
                                            CI.FLine = LineIdx;
                                            if (S == (AnsiString)"RIGHT")
                                            {
                                                CI.FAlign = TaowTextAlign.taRight;
                                            }
                                            else
                                            {
                                                if (S == (AnsiString)"CENTER")
                                                {
                                                    CI.FAlign = TaowTextAlign.taCenter;
                                                }
                                                else
                                                {
                                                    CI.FAlign = TaowTextAlign.taLeft;
                                                }
                                            }

                                            NewChange();
                                            if (C == 62)
                                            {
                                                State = TIntParseMode.pNone;
                                                TextStart = J + 1;
                                            }
                                            else
                                            {
                                                State = TIntParseMode.pStart;
                                                TagStart = J + 1;
                                            }
                                        }

                                        break;
                                    case TIntParseMode.pBG:
                                        if (TSet<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                        {
                                            var call__31 = Application.ExeName;
                                            var call__32 = ExtractFilePath(call__31);
                                            var call__33 = FileExists(call__32 + Copy(item, TagStart + 1, J - TagStart));
                                            if (call__33)
                                            {
                                                var call__34 = Application.ExeName;
                                                var call__35 = ExtractFilePath(call__34);
                                                FImages.LoadFromFile(call__35 + Copy(item, TagStart + 1, J - TagStart));
                                            }

                                            FILIndexTexture = 0;
                                            if (C == 62)
                                            {
                                                State = TIntParseMode.pNone;
                                                TextStart = J + 1;
                                            }
                                            else
                                            {
                                                State = TIntParseMode.pInBG;
                                                TagStart = J + 1;
                                            }
                                        }

                                        break;
                                    case TIntParseMode.pBGIndex:
                                        if (TSet<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                        {
                                            try
                                            {
                                                FILIndexTexture = StrToInt(Copy(item, TagStart + 1, J - TagStart));
                                            }
                                            catch (System.Exception)
                                            {
                                            }

                                            if (C == 62)
                                            {
                                                State = TIntParseMode.pNone;
                                                TextStart = J + 1;
                                            }
                                            else
                                            {
                                                State = TIntParseMode.pInBG;
                                                TagStart = J + 1;
                                            }
                                        }

                                        break;
                                }

                                if (TextEnd >= TextStart)
                                {
                                    AddText(Copy(item, TextStart + 1, TextEnd - TextStart + 1));
                                    TextStart = TextEnd;
                                }
                            }
                        }

                        switch (State)
                        {
                            case TIntParseMode.pNone:
                                if (TextStart <= Last)
                                {
                                    AddText(Copy(item, TextStart + 1, Last - TextStart + 1));
                                }

                                break;
                            case TIntParseMode.pStart:
                                if (TagStart <= Last)
                                {
                                    ParseTag(Copy(item, TagStart + 1, Last - TagStart));
                                }

                                break;
                        }
                    }
                }

                H = H + CI.FHeight;
            }

            if (FWinWidth - FScrollWidth != FTextWidth || FTextHeight != H)
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
                FVScrollBar.WinLeft = FWinLeft + FWinWidth - FVScrollBar.WinWidth;
                FVScrollBar.WinTop = FWinTop;
            }

            base.SetSize();
        }

        // AoWHTMLMemo.pas:747
        // { Update follows the scroll bar; a masked memo asks its owner to redraw it under the text }
        public override void Update()
        {
            TRect R = default;
            if (FVisible)
            {
                bool bool__36 = FVScrollBar != null;
                if (bool__36)
                {
                    var call__37 = FLines.Count;
                    bool__36 = call__37 > 0;
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
            if (FVisible && FMasked && !FSurfaceValid)
            {
                if (FAOWOwner.FAOWOwner.FAOWOwner == null)
                {
                    var call__38 = Rect(FWinLeft, FWinTop, FWinLeft + FWinWidth - 1, FWinTop + FWinHeight - 1);
                    R = call__38;
                }
                else
                {
                    var call__39 = Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, FAOWOwner.FWinLeft + FWinLeft + FWinWidth - 1, FAOWOwner.FWinTop + FWinTop + FWinHeight - 1);
                    R = call__39;
                }

                FAOWOwner.AddDrawRect(ref R, this);
            }
        }

        // AoWHTMLMemo.pas:122
        public override void InvalidateRect(int Index, ref TRect ARect, TAoWComponent Sender)
        {
            if (FValid && FVisible)
            {
                AddDrawRect(ref ARect, Sender);
            }
        }

        // AoWHTMLMemo.pas:128
        public override void SetOwner(TAoWComponent Value)
        {
            base.SetOwner(Value);
            FAOWWindow = (TAOWBaseWindow)FAOWOwner;
        }

        // AoWHTMLMemo.pas:584
        // { Draw renders the text into the back surface when it changed, then blits the dirty rectangles
//   of the window (or the whole visible part) from it }
        public override void Draw(TSurfaceDesc SurfDesc)
        {
            int I = default;
            int OldLeft = default;
            int OldTop = default;
            int Pos = default;
            int LH = default;
            int X = default;
            int Y = default;
            TaowTextAlign Align = default;
            AnsiString FontName = default;
            TSurfaceDesc BackSD = default;
            TRect SrcR = default;
            int Line = default;
            TChangeInfo CI = default;
            TaowRect R = default;
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
                        var call__41 = new TDataSurface(FTextWidth, FTextHeight, SurfDesc.PixelFormatID, null);
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
                            var call__42 = Rect(0, 0, FTextWidth, FTextHeight);
                            FImages.DrawILT(FILIndexTexture, call__42, ref BackSD);
                        }

                        Line = 1;
                        var call__43 = FChanges[0];
                        CI = (TChangeInfo)call__43;
                        Align = TaowTextAlign.taLeft;
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
                                    case TaowTextAlign.taLeft:
                                        X = FBorderWidth;
                                        break;
                                    case TaowTextAlign.taCenter:
                                        X = 0;
                                        break;
                                    case TaowTextAlign.taRight:
                                        X = FTextWidth - FBorderWidth;
                                        break;
                                }

                                while (CI.FLine == I && Line <= FChanges.Count)
                                {
                                    if (FontName != (AnsiString)"" && CI.FPos > Pos)
                                    {
                                        switch (Align)
                                        {
                                            case TaowTextAlign.taLeft:
                                                var call__45 = FLines[I];
                                                FAOWFont.SetText(X, Y, Copy(call__45, Pos + 1, CI.FPos - Pos), BackSD);
                                                var call__46 = FLines[I];
                                                var call__47 = FAOWFont.GetTextWidth(Copy(call__46, Pos + 1, CI.FPos - Pos));
                                                X = X + call__47;
                                                break;
                                            case TaowTextAlign.taCenter:
                                                var call__48 = FLines[I];
                                                var call__49 = FAOWFont.GetTextWidth(Copy(call__48, Pos + 1, CI.FPos - Pos));
                                                var call__50 = FLines[I];
                                                FAOWFont.SetText(FTextWidth / 2 - call__49 / 2, Y, Copy(call__50, Pos + 1, CI.FPos - Pos), BackSD);
                                                break;
                                            case TaowTextAlign.taRight:
                                                var call__51 = FLines[I];
                                                var call__52 = FAOWFont.GetTextWidth(Copy(call__51, Pos + 1, CI.FPos - Pos));
                                                var call__53 = FLines[I];
                                                FAOWFont.SetText(X - call__52, Y, Copy(call__53, Pos + 1, CI.FPos - Pos), BackSD);
                                                var call__54 = FLines[I];
                                                var call__55 = FAOWFont.GetTextWidth(Copy(call__54, Pos + 1, CI.FPos - Pos));
                                                X = X - call__55;
                                                break;
                                        }
                                    }

                                    Pos = CI.FPos;
                                    if (CI.FFont != (AnsiString)"")
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
                                            case TaowTextAlign.taLeft:
                                                X = FBorderWidth;
                                                break;
                                            case TaowTextAlign.taCenter:
                                                X = 0;
                                                break;
                                            case TaowTextAlign.taRight:
                                                X = FTextWidth - FBorderWidth;
                                                break;
                                        }
                                    }

                                    Align = CI.FAlign;
                                    if (Line < FChanges.Count)
                                    {
                                        var call__56 = FChanges[Line];
                                        CI = (TChangeInfo)call__56;
                                    }

                                    Line += 1;
                                }

                                if (FontName != (AnsiString)"")
                                {
                                    switch (Align)
                                    {
                                        case TaowTextAlign.taLeft:
                                            var call__57 = FLines[I];
                                            var call__58 = FLines[I];
                                            FAOWFont.SetText(X, Y, Copy(call__58, Pos + 1, call__57.Length - Pos), BackSD);
                                            break;
                                        case TaowTextAlign.taCenter:
                                            var call__59 = FLines[I];
                                            var call__60 = FLines[I];
                                            var call__61 = FAOWFont.GetTextWidth(Copy(call__60, Pos + 1, call__59.Length - Pos));
                                            var call__62 = FLines[I];
                                            var call__63 = FLines[I];
                                            FAOWFont.SetText(FTextWidth / 2 - call__61 / 2, Y, Copy(call__63, Pos + 1, call__62.Length - Pos), BackSD);
                                            break;
                                        case TaowTextAlign.taRight:
                                            var call__64 = FLines[I];
                                            var call__65 = FLines[I];
                                            var call__66 = FAOWFont.GetTextWidth(Copy(call__65, Pos + 1, call__64.Length - Pos));
                                            var call__67 = FLines[I];
                                            var call__68 = FLines[I];
                                            FAOWFont.SetText(X - call__66, Y, Copy(call__68, Pos + 1, call__67.Length - Pos), BackSD);
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
                                R = (TaowRect)call__70;
                                if (FAOWOwner.FWinLeft + FWinLeft > R.R.Left)
                                {
                                    R.R.Left = FAOWOwner.FWinLeft + FWinLeft;
                                }

                                if (FAOWOwner.FWinTop + FWinTop > R.R.Top)
                                {
                                    R.R.Top = FAOWOwner.FWinTop + FWinTop;
                                }

                                if (FAOWOwner.FWinLeft + FWinLeft + FWinWidth < R.R.Right)
                                {
                                    R.R.Right = FAOWOwner.FWinLeft + FWinLeft + FWinWidth;
                                }

                                if (FAOWOwner.FWinTop + FWinTop + FWinHeight < R.R.Bottom)
                                {
                                    R.R.Bottom = FAOWOwner.FWinTop + FWinTop + FWinHeight;
                                }

                                var call__71 = Rect(R.R.Left - FAOWOwner.FWinLeft - FWinLeft, FTopItem + R.R.Top - FAOWOwner.FWinTop - FWinTop, R.R.Right - FAOWOwner.FWinLeft - FWinLeft, FTopItem + R.R.Bottom - FAOWOwner.FWinTop - FWinTop);
                                SrcR = call__71;
                                if (FMasked)
                                {
                                    BltSpriteClipped16(BackSD, SrcR, SurfDesc, R.R.Left, R.R.Top, FMaskColor);
                                }
                                else
                                {
                                    BltSDRectClipped(BackSD, SrcR, SurfDesc, R.R.Left, R.R.Top);
                                }
                            }
                        }
                    }
                    else
                    {
                        var call__72 = Rect(0, FTopItem, FWinWidth, FTopItem + FWinHeight);
                        SrcR = call__72;
                        if (FMasked)
                        {
                            BltSpriteClipped16(BackSD, SrcR, SurfDesc, FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FMaskColor);
                        }
                        else
                        {
                            BltSDRectClipped(BackSD, SrcR, SurfDesc, FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop);
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
                MessageBox(0, (AnsiString)"Exception during Draw", (AnsiString)"Error - " + Name, 0);
            }
        }

        // AoWHTMLMemo.pas:134
        public void SetAOWWindow(TAOWBaseWindow Value)
        {
            SetOwner(Value);
        }

        // AoWHTMLMemo.pas:139
        public void SetFStrings(TStringList Value)
        {
            FItems.Clear();
            FItems.AddStrings(Value);
        }

        // AoWHTMLMemo.pas:145
        public void SetVScroll(TAOWVScrollBar Value)
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
        public void SetFileName(AnsiString Value)
        {
            bool bool__73 = Value != FFilename;
            if (bool__73)
            {
                var call__74 = FileExists(Value);
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
        public bool LoadFont(AnsiString FileName)
        {
            bool Result = default;
            TImageLibraryFont Font = default;
            int I = default;
            ;
            var call__75 = Application.ExeName;
            var call__76 = ExtractFilePath(call__75);
            FileName = call__76 + FileName;
            Font = null;
            I = 0;
            while (I < FFonts.Count && Font == null)
            {
                var call__77 = FFonts[I];
                if (((TImageLibraryFont)call__77).FileName == FileName)
                {
                    var call__78 = FFonts[I];
                    Font = (TImageLibraryFont)call__78;
                }

                I += 1;
            }

            if (Font == null && FileExists(FileName))
            {
                Font = new TImageLibraryFont(this);
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

        public TStringList Items { get => FItems; set => SetFStrings(value); }
        public int ListOffset { get => FTopItem; set => SetListOff(value); }

        [Published]
        public TAOWBaseWindow AOWWindow { get => FAOWWindow; set => SetAOWWindow(value); }

        [Published]
        public TAOWVScrollBar VScrollBar { get => FVScrollBar; set => SetVScroll(value); }

        [Published]
        public AnsiString Filename { get => FFilename; set => SetFileName(value); }

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
            RegisterComponents((AnsiString)"AOW Interface", [typeof(TaowHTMLMemo)]);
        }
    }
}
