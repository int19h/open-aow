// Translated from AoWHTMLMemo.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using static Aow.Game.Draw.GFXEUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Controls
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
}

namespace Aow.Game.Ui.Windows
{

    [LegacyObject]
    // a style change in the text. FHeight (the line height), FColor, FFont (the font file, '' for
    // the current font), FAlign, FPos (the character of the line where it starts), FLine, FSpacing
    public partial class TChangeInfo
    {
        public int FHeight;
        public int FColor;
        public string FFont = "";
        public TaowTextAlign FAlign;
        public int FPos;
        public int FLine;
        public int FSpacing;

        public TChangeInfo()
        {
            CaptureAddress.Assign(this);
        }
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
        public string FFilename = "";
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
            FLines = new TStringList();
            FItems = new TStringList();
            FItems.OnChange = OnChangeText;
            FChanges = new TList();
            F1EC = new TList();
            FFonts = new TList();
            FImages = new TInterfaceIL(this);
        }

        // AoWHTMLMemo.pas:105
        public override void Dispose()
        {
            FLines.Dispose();
            FItems.Dispose();
            F1EC?.Dispose();
            for (int I = 0, last = FChanges.Count - 1; I <= last; I++)
            {
            }

            for (int I = 0, last = FFonts.Count - 1; I <= last; I++)
            {
                ((TImageLibraryFont)FFonts[I])?.Dispose();
            }

            FFonts?.Dispose();
            FChanges?.Dispose();
            FImages?.Dispose();
            base.Dispose();
        }

        // AoWHTMLMemo.pas:174
        public override void OnChangeText(object Sender)
        {
            SetListOff(0);
            FSized = false;
        }

        // AoWHTMLMemo.pas:210
        // { SetSize parses the text into FLines and FChanges, wrapping the lines at the width of the
        //   window, and sets up the scroll bar }
        public override void SetSize()
        {
            TChangeInfo CI;
            int H;
            int LineIdx;
            TIntParseMode State;
            int Last;
            int TextStart;
            int TextEnd;
            int TagStart;
            byte C;
            ReadOnlySpan<byte> text;
            string S;
            int V;
            void NewLine()
            {
                FLines.Add("");
                H = H + CI.FHeight;
                LineIdx += 1;
            }

            void AddText(string S)
            {
                int TW;
                int W;
                int Start;
                string Word;
                int I;
                int Len;
                if (FAOWFont != null)
                {
                    I = 0;
                    Start = 0;
                    Len = S.Length;
                    var line = FLines[LineIdx];
                    W = FAOWFont.GetTextWidth(line) + FBorderWidth * 2 + FScrollWidth;
                    while (I < Len)
                    {
                        if (S[I - 1] == 32 || I == Len - 1)
                        {
                            if (I == Len - 1)
                            {
                                I += 1;
                            }

                            Word = Copy(S, Start + 1, I - Start);
                            TW = FAOWFont.GetTextWidth(Word);
                            if (W + TW > FWinWidth)
                            {
                                NewLine();
                                var line2 = FLines[LineIdx];
                                FLines[LineIdx] = line2 + Word;
                                W = FBorderWidth * 2 + FScrollWidth + TW;
                            }
                            else
                            {
                                var line2 = FLines[LineIdx];
                                FLines[LineIdx] = line2 + Word;
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
                TChangeInfo N;
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

            void ParseTag(string Tag)
            {
                Tag = UpperCase(Tag);
                switch (State)
                {
                    case TIntParseMode.pInBG:
                        if (Tag == "INDEX")
                        {
                            State = TIntParseMode.pBGIndex;
                        }

                        break;
                    default:
                        if (Tag == "P")
                        {
                            NewLine();
                            NewLine();
                        }
                        else
                        {
                            if (Tag == "BR")
                            {
                                NewLine();
                            }
                            else
                            {
                                if (Tag == "FONT")
                                {
                                    State = TIntParseMode.pFont;
                                }
                                else
                                {
                                    if (Tag == "ITEMHEIGHT")
                                    {
                                        State = TIntParseMode.pItemHeight;
                                    }
                                    else
                                    {
                                        if (Tag == "COLOR")
                                        {
                                            State = TIntParseMode.pColor;
                                        }
                                        else
                                        {
                                            if (Tag == "SPACING")
                                            {
                                                State = TIntParseMode.pSpace;
                                            }
                                            else
                                            {
                                                if (Tag == "ALIGN")
                                                {
                                                    State = TIntParseMode.pAlign;
                                                }
                                                else
                                                {
                                                    if (Tag == "BACKGROUND")
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
            if (FItems.Count > 0 && UpperCase(Copy(FItems[0], 1, 5)) == "<INT>")
            {
                FLines.Add("");
                H = FBorderHeight * 2;
                FImages.Clear();
                for (int I = 0, last = FChanges.Count - 1; I <= last; I++)
                {
                }

                FChanges.Clear();
                CI = new TChangeInfo();
                CI.FHeight = 10;
                CI.FColor = 16777215;
                CI.FFont = "";
                CI.FAlign = TaowTextAlign.taLeft;
                CI.FPos = 0;
                CI.FLine = 0;
                CI.FSpacing = 0;
                FChanges.Add(CI);
                LineIdx = 0;
                for (int I = 0, last = FItems.Count - 1; I <= last; I++)
                {
                    string item = NullTerminatedText(FItems[I]);
                    text = Cp1252.Encode(item);
                    Last = text.Length - 1;
                    TextStart = 0;
                    TextEnd = -1;
                    TagStart = 0;
                    for (int J = 0; J <= Last; J++)
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
                                    var line = FLines[LineIdx];
                                    CI.FPos = line.Length;
                                    CI.FLine = LineIdx;
                                    CI.FFont = Copy(item, TagStart + 1, J - TagStart);
                                    if (!IsEmpty(CI.FFont))
                                    {
                                        if (!LoadFont(CI.FFont))
                                        {
                                            CI.FFont = "";
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
                                    var line = FLines[LineIdx];
                                    CI.FPos = line.Length;
                                    CI.FLine = LineIdx;
                                    try
                                    {
                                        CI.FHeight = StrToInt(Copy(item, TagStart + 1, J - TagStart));
                                    }
                                    catch
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
                                    var line = FLines[LineIdx];
                                    CI.FPos = line.Length;
                                    CI.FLine = LineIdx;
                                    if (J - TagStart == 6)
                                    {
                                        CI.FColor = 0;
                                        for (int K = 0; K <= 5; K++)
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
                                    var line = FLines[LineIdx];
                                    CI.FPos = line.Length;
                                    CI.FLine = LineIdx;
                                    try
                                    {
                                        CI.FSpacing = StrToInt(Copy(item, TagStart + 1, J - TagStart));
                                        if (FAOWFont != null)
                                        {
                                            FAOWFont.CharacterDistance = CI.FSpacing;
                                        }
                                    }
                                    catch
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
                                    var line = FLines[LineIdx];
                                    CI.FPos = line.Length;
                                    CI.FLine = LineIdx;
                                    if (S == "RIGHT")
                                    {
                                        CI.FAlign = TaowTextAlign.taRight;
                                    }
                                    else
                                    {
                                        if (S == "CENTER")
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
                                    if (FileExists(ExtractFilePath(Application.ExeName) + Copy(item, TagStart + 1, J - TagStart)))
                                    {
                                        var filePath = ExtractFilePath(Application.ExeName);
                                        FImages.LoadFromFile(filePath + Copy(item, TagStart + 1, J - TagStart));
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
                                    catch
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
            TRect R;
            if (FVisible)
            {
                if (FVScrollBar != null && FLines.Count > 0)
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
                    R = Rect(FWinLeft, FWinTop, FWinLeft + FWinWidth - 1, FWinTop + FWinHeight - 1);
                }
                else
                {
                    R = Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, FAOWOwner.FWinLeft + FWinLeft + FWinWidth - 1, FAOWOwner.FWinTop + FWinTop + FWinHeight - 1);
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
            int OldLeft;
            int OldTop;
            int Pos = default;
            int LH = default;
            int X = default;
            int Y = default;
            TaowTextAlign Align = default;
            string FontName = "";
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
                        if (FBackSurface.PixelFormatID != SurfDesc.PixelFormatID)
                        {
                            FBackSurface?.Dispose();
                            FBackSurface = null;
                        }
                    }

                    if (FBackSurface == null)
                    {
                        FBackSurface = new TDataSurface(FTextWidth, FTextHeight, SurfDesc.PixelFormatID, null);
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
                            var rect = Rect(0, 0, FTextWidth, FTextHeight);
                            FImages.DrawILT(FILIndexTexture, rect, ref BackSD);
                        }

                        Line = 1;
                        CI = (TChangeInfo)FChanges[0];
                        Align = TaowTextAlign.taLeft;
                        FontName = "";
                        X = FBorderWidth;
                        Y = FBorderHeight;
                        LH = 10;
                        for (int I = 0, last = FLines.Count - 1; I <= last; I++)
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
                                if (!IsEmpty(FontName) && CI.FPos > Pos)
                                {
                                    switch (Align)
                                    {
                                        case TaowTextAlign.taLeft:
                                            var line = FLines[I];
                                            FAOWFont.SetText(X, Y, Copy(line, Pos + 1, CI.FPos - Pos), BackSD);
                                            var line2 = FLines[I];
                                            X = X + FAOWFont.GetTextWidth(Copy(line2, Pos + 1, CI.FPos - Pos));
                                            break;
                                        case TaowTextAlign.taCenter:
                                            var line3 = FLines[I];
                                            var textWidth = FAOWFont.GetTextWidth(Copy(line3, Pos + 1, CI.FPos - Pos));
                                            var line4 = FLines[I];
                                            FAOWFont.SetText(FTextWidth / 2 - textWidth / 2, Y, Copy(line4, Pos + 1, CI.FPos - Pos), BackSD);
                                            break;
                                        case TaowTextAlign.taRight:
                                            var line5 = FLines[I];
                                            var faowFontTextWidth = FAOWFont.GetTextWidth(Copy(line5, Pos + 1, CI.FPos - Pos));
                                            var line6 = FLines[I];
                                            FAOWFont.SetText(X - faowFontTextWidth, Y, Copy(line6, Pos + 1, CI.FPos - Pos), BackSD);
                                            var line7 = FLines[I];
                                            X = X - FAOWFont.GetTextWidth(Copy(line7, Pos + 1, CI.FPos - Pos));
                                            break;
                                    }
                                }

                                Pos = CI.FPos;
                                if (!IsEmpty(CI.FFont))
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
                                    CI = (TChangeInfo)FChanges[Line];
                                }

                                Line += 1;
                            }

                            if (!IsEmpty(FontName))
                            {
                                switch (Align)
                                {
                                    case TaowTextAlign.taLeft:
                                        var lineLength = FLines[I];
                                        var line = FLines[I];
                                        FAOWFont.SetText(X, Y, Copy(line, Pos + 1, lineLength.Length - Pos), BackSD);
                                        break;
                                    case TaowTextAlign.taCenter:
                                        var line2 = FLines[I];
                                        var line3 = FLines[I];
                                        var textWidth = FAOWFont.GetTextWidth(Copy(line3, Pos + 1, line2.Length - Pos));
                                        var line4 = FLines[I];
                                        var line5 = FLines[I];
                                        FAOWFont.SetText(FTextWidth / 2 - textWidth / 2, Y, Copy(line5, Pos + 1, line4.Length - Pos), BackSD);
                                        break;
                                    case TaowTextAlign.taRight:
                                        var line6 = FLines[I];
                                        var line7 = FLines[I];
                                        var faowFontTextWidth = FAOWFont.GetTextWidth(Copy(line7, Pos + 1, line6.Length - Pos));
                                        var line8 = FLines[I];
                                        var line9 = FLines[I];
                                        FAOWFont.SetText(X - faowFontTextWidth, Y, Copy(line9, Pos + 1, line8.Length - Pos), BackSD);
                                        break;
                                }
                            }

                            Y = Y + LH;
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
                        for (int I = 0, last = FDrawCount - 1; I <= last; I++)
                        {
                            R = (TaowRect)FDrawList[I];
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

                            SrcR = Rect(R.R.Left - FAOWOwner.FWinLeft - FWinLeft, FTopItem + R.R.Top - FAOWOwner.FWinTop - FWinTop, R.R.Right - FAOWOwner.FWinLeft - FWinLeft, FTopItem + R.R.Bottom - FAOWOwner.FWinTop - FWinTop);
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
                    else
                    {
                        SrcR = Rect(0, FTopItem, FWinWidth, FTopItem + FWinHeight);
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
            catch
            {
                MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
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
        public void SetFileName(string Value)
        {
            if (Value != FFilename && FileExists(Value))
            {
                FFilename = Value;
                FItems.LoadFromFile(FFilename);
            }
        }

        // AoWHTMLMemo.pas:182
        // { LoadFont makes the font in FileName (relative to the program) the current font, loading it
        //   the first time }
        public bool LoadFont(string FileName)
        {
            TImageLibraryFont Font;
            int I;
            FileName = ExtractFilePath(Application.ExeName) + FileName;
            Font = null;
            I = 0;
            while (I < FFonts.Count && Font == null)
            {
                if (((TCustomImageLibrary)FFonts[I]).FileName == FileName)
                {
                    Font = (TImageLibraryFont)FFonts[I];
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

            return Font != null;
        }

        public TStringList Items { get => FItems; set => SetFStrings(value); }
        public int ListOffset { get => FTopItem; set => SetListOff(value); }

        [Published]
        public TAOWBaseWindow AOWWindow { get => FAOWWindow; set => SetAOWWindow(value); }

        [Published]
        public TAOWVScrollBar VScrollBar { get => FVScrollBar; set => SetVScroll(value); }

        [Published]
        public string Filename { get => FFilename; set => SetFileName(value); }

        [Published]
        public bool FreeSurfaceOnHide { get => FFreeSurfaceOnHide; set => FFreeSurfaceOnHide = value; }

        [Published]
        public int BorderWidth { get => FBorderWidth; set => FBorderWidth = value; }

        [Published]
        public int BorderHeight { get => FBorderHeight; set => FBorderHeight = value; }

        [Published]
        public int MaskColor { get => FMaskColor; set => FMaskColor = value; }

        public new TNotifyEvent<TaowHTMLMemo> OnChange
        {
            get => EventSlot.Get(this, FOnChange);
            set => EventSlot.Set(this, ref FOnChange, value);
        }

        public new TMouseEvent<TaowHTMLMemo> OnMouseDown
        {
            get => EventSlot.Get(this, FOnMouseDown);
            set => EventSlot.Set(this, ref FOnMouseDown, value);
        }

        public new TMouseEvent<TaowHTMLMemo> OnMouseUp
        {
            get => EventSlot.Get(this, FOnMouseUp);
            set => EventSlot.Set(this, ref FOnMouseUp, value);
        }

        public new TMouseMoveEvent<TaowHTMLMemo> OnMouseMove
        {
            get => EventSlot.Get(this, FOnMouseMove);
            set => EventSlot.Set(this, ref FOnMouseMove, value);
        }

        public new TKeyPressEvent<TaowHTMLMemo> OnKeyPress
        {
            get => EventSlot.Get(this, FOnKeyPress);
            set => EventSlot.Set(this, ref FOnKeyPress, value);
        }

        public new TKeyEvent<TaowHTMLMemo> OnKeyDown
        {
            get => EventSlot.Get(this, FOnKeyDown);
            set => EventSlot.Set(this, ref FOnKeyDown, value);
        }

        public new TAoWOnDrawEvent<TaowHTMLMemo> OnPreDraw
        {
            get => EventSlot.Get(this, FOnPreDraw);
            set => EventSlot.Set(this, ref FOnPreDraw, value);
        }

        public new TAoWOnDrawEvent<TaowHTMLMemo> OnDraw
        {
            get => EventSlot.Get(this, FOnDraw);
            set => EventSlot.Set(this, ref FOnDraw, value);
        }

        public new TNotifyEvent<TaowHTMLMemo> OnUpdate
        {
            get => EventSlot.Get(this, FOnUpdate);
            set => EventSlot.Set(this, ref FOnUpdate, value);
        }

        public new TAoWOnDrawBackEvent<TaowHTMLMemo> OnDrawBack
        {
            get => EventSlot.Get(this, FOnDrawBack);
            set => EventSlot.Set(this, ref FOnDrawBack, value);
        }

        public new TNotifyEvent<TaowHTMLMemo> OnEnter
        {
            get => EventSlot.Get(this, FOnEnter);
            set => EventSlot.Set(this, ref FOnEnter, value);
        }

        public new TNotifyEvent<TaowHTMLMemo> OnExit
        {
            get => EventSlot.Get(this, FOnExit);
            set => EventSlot.Set(this, ref FOnExit, value);
        }

        public new TNotifyEvent<TaowHTMLMemo> OnHide
        {
            get => EventSlot.Get(this, FOnHide);
            set => EventSlot.Set(this, ref FOnHide, value);
        }

        public new TNotifyEvent<TaowHTMLMemo> OnShow
        {
            get => EventSlot.Get(this, FOnShow);
            set => EventSlot.Set(this, ref FOnShow, value);
        }
    }
}

namespace Aow.Game.Ui.Controls
{

    public static partial class AoWHTMLMemoUnit
    {
    }
}
