// Translated from AoWHTMLMemo.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Controls
{

    public enum IntParseMode : byte
    {
        None = 0,
        Start = 1,
        Font = 2,
        ItemHeight = 3,
        Color = 4,
        BG = 5,
        InBG = 6,
        BGIndex = 7,
        Space = 8,
        Align = 9
    }

    public enum TaowAlign : byte
    {
        Left = 0,
        Center = 1,
        Right = 2
    }
}

namespace Aow.Game.Ui.Windows
{

    // Fields: F1EC (a list that is created and freed only), FChanges (TChangeInfo), FLines (the
    // wrapped lines), FItems (the text), FVScrollBar, FFilename, FScrollWidth, FBackDrawn (the back
    // surface holds the text), FImages (the background textures), FTopItem (the first pixel row
    // shown), FFonts (the fonts loaded, TImageLibraryFont), FAOWWindow, FTextWidth, FTextHeight
    // (the size of the back surface)
    public partial class TaowHTMLMemo : AOWBaseWindow
    {
        public DelphiList F1EC;
        public DelphiList FChanges;
        public StringList FLines;
        public StringList FItems;
        public AOWVScrollBar FVScrollBar;
        public string FFilename = "";
        public int FScrollWidth;
        public bool FBackDrawn;
        public InterfaceIL FImages;
        public int FTopItem;
        public DelphiList FFonts;
        public AOWBaseWindow FAOWWindow;
        public int FTextWidth;
        public int FTextHeight;

        // AoWHTMLMemo.pas:93
        public TaowHTMLMemo(global::Aow.Rtl.Component owner) : base(owner)
        {
            FLines = new StringList();
            FItems = new StringList();
            FItems.OnChange = OnChangeText;
            FChanges = new DelphiList();
            F1EC = new DelphiList();
            FFonts = new DelphiList();
            FImages = new InterfaceIL(this);
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
                ((ImageLibraryFont)FFonts[I])?.Dispose();
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
            ChangeInfo CI;
            int H;
            int LineIdx;
            IntParseMode State;
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
                ChangeInfo N;
                N = new ChangeInfo();
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
                    case IntParseMode.InBG:
                        if (Tag == "INDEX")
                        {
                            State = IntParseMode.BGIndex;
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
                                    State = IntParseMode.Font;
                                }
                                else
                                {
                                    if (Tag == "ITEMHEIGHT")
                                    {
                                        State = IntParseMode.ItemHeight;
                                    }
                                    else
                                    {
                                        if (Tag == "COLOR")
                                        {
                                            State = IntParseMode.Color;
                                        }
                                        else
                                        {
                                            if (Tag == "SPACING")
                                            {
                                                State = IntParseMode.Space;
                                            }
                                            else
                                            {
                                                if (Tag == "ALIGN")
                                                {
                                                    State = IntParseMode.Align;
                                                }
                                                else
                                                {
                                                    if (Tag == "BACKGROUND")
                                                    {
                                                        State = IntParseMode.BG;
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
            State = IntParseMode.None;
            if (FItems.Count > 0 && UpperCase(Copy(FItems[0], 1, 5)) == "<INT>")
            {
                FLines.Add("");
                H = FBorderHeight * 2;
                FImages.Clear();
                for (int I = 0, last = FChanges.Count - 1; I <= last; I++)
                {
                }

                FChanges.Clear();
                CI = new ChangeInfo();
                CI.FHeight = 10;
                CI.FColor = 16777215;
                CI.FFont = "";
                CI.FAlign = TaowTextAlign.Left;
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
                            case IntParseMode.None:
                                switch (C)
                                {
                                    case 60:
                                        State = IntParseMode.Start;
                                        TextEnd = J - 1;
                                        TagStart = J + 1;
                                        break;
                                }

                                break;
                            case IntParseMode.Start:
                            case IntParseMode.InBG:
                                switch (C)
                                {
                                    case 62:
                                        ParseTag(Copy(item, TagStart + 1, J - TagStart));
                                        State = IntParseMode.None;
                                        TextStart = J + 1;
                                        break;
                                    case 32:
                                    case 61:
                                        ParseTag(Copy(item, TagStart + 1, J - TagStart));
                                        TagStart = J + 1;
                                        break;
                                }

                                break;
                            case IntParseMode.Font:
                                if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
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
                                        State = IntParseMode.None;
                                        TextStart = J + 1;
                                    }
                                    else
                                    {
                                        State = IntParseMode.Start;
                                        TagStart = J + 1;
                                    }
                                }

                                break;
                            case IntParseMode.ItemHeight:
                                if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
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
                                        State = IntParseMode.None;
                                        TextStart = J + 1;
                                    }
                                    else
                                    {
                                        State = IntParseMode.Start;
                                        TagStart = J + 1;
                                    }
                                }

                                break;
                            case IntParseMode.Color:
                                if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
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
                                        State = IntParseMode.None;
                                        TextStart = J + 1;
                                    }
                                    else
                                    {
                                        State = IntParseMode.Start;
                                        TagStart = J + 1;
                                    }
                                }

                                break;
                            case IntParseMode.Space:
                                if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
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
                                        State = IntParseMode.None;
                                        TextStart = J + 1;
                                    }
                                    else
                                    {
                                        State = IntParseMode.Start;
                                        TagStart = J + 1;
                                    }
                                }

                                break;
                            case IntParseMode.Align:
                                if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                {
                                    S = UpperCase(Copy(item, TagStart + 1, J - TagStart));
                                    var line = FLines[LineIdx];
                                    CI.FPos = line.Length;
                                    CI.FLine = LineIdx;
                                    if (S == "RIGHT")
                                    {
                                        CI.FAlign = TaowTextAlign.Right;
                                    }
                                    else
                                    {
                                        if (S == "CENTER")
                                        {
                                            CI.FAlign = TaowTextAlign.Center;
                                        }
                                        else
                                        {
                                            CI.FAlign = TaowTextAlign.Left;
                                        }
                                    }

                                    NewChange();
                                    if (C == 62)
                                    {
                                        State = IntParseMode.None;
                                        TextStart = J + 1;
                                    }
                                    else
                                    {
                                        State = IntParseMode.Start;
                                        TagStart = J + 1;
                                    }
                                }

                                break;
                            case IntParseMode.BG:
                                if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                                {
                                    if (FileExists(ExtractFilePath(global::Aow.Rtl.RtlUnit.Application.ExeName) + Copy(item, TagStart + 1, J - TagStart)))
                                    {
                                        var filePath = ExtractFilePath(global::Aow.Rtl.RtlUnit.Application.ExeName);
                                        FImages.LoadFromFile(filePath + Copy(item, TagStart + 1, J - TagStart));
                                    }

                                    FILIndexTexture = 0;
                                    if (C == 62)
                                    {
                                        State = IntParseMode.None;
                                        TextStart = J + 1;
                                    }
                                    else
                                    {
                                        State = IntParseMode.InBG;
                                        TagStart = J + 1;
                                    }
                                }

                                break;
                            case IntParseMode.BGIndex:
                                if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
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
                                        State = IntParseMode.None;
                                        TextStart = J + 1;
                                    }
                                    else
                                    {
                                        State = IntParseMode.InBG;
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
                        case IntParseMode.None:
                            if (TextStart <= Last)
                            {
                                AddText(Copy(item, TextStart + 1, Last - TextStart + 1));
                            }

                            break;
                        case IntParseMode.Start:
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
            global::Aow.Rtl.Rect R;
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
                    R = global::Aow.Rtl.RtlUnit.Rect(FWinLeft, FWinTop, FWinLeft + FWinWidth - 1, FWinTop + FWinHeight - 1);
                }
                else
                {
                    R = global::Aow.Rtl.RtlUnit.Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, FAOWOwner.FWinLeft + FWinLeft + FWinWidth - 1, FAOWOwner.FWinTop + FWinTop + FWinHeight - 1);
                }

                FAOWOwner.AddDrawRect(ref R, this);
            }
        }

        // AoWHTMLMemo.pas:122
        public override void InvalidateRect(int Index, ref global::Aow.Rtl.Rect ARect, AoWComponent Sender)
        {
            if (FValid && FVisible)
            {
                AddDrawRect(ref ARect, Sender);
            }
        }

        // AoWHTMLMemo.pas:128
        public override void SetOwner(AoWComponent Value)
        {
            base.SetOwner(Value);
            FAOWWindow = (AOWBaseWindow)FAOWOwner;
        }

        // AoWHTMLMemo.pas:584
        // { Draw renders the text into the back surface when it changed, then blits the dirty rectangles
        //   of the window (or the whole visible part) from it }
        public override void Draw(SurfaceDesc SurfDesc)
        {
            int OldLeft;
            int OldTop;
            int Pos = default;
            int LH = default;
            int X = default;
            int Y = default;
            TaowTextAlign Align = default;
            string FontName = "";
            SurfaceDesc BackSD = default;
            global::Aow.Rtl.Rect SrcR = default;
            int Line = default;
            ChangeInfo CI = default;
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
                        FBackSurface = new DataSurface(FTextWidth, FTextHeight, SurfDesc.PixelFormatID, null);
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
                            var rect = global::Aow.Rtl.RtlUnit.Rect(0, 0, FTextWidth, FTextHeight);
                            FImages.DrawILT(FILIndexTexture, rect, ref BackSD);
                        }

                        Line = 1;
                        CI = (ChangeInfo)FChanges[0];
                        Align = TaowTextAlign.Left;
                        FontName = "";
                        X = FBorderWidth;
                        Y = FBorderHeight;
                        LH = 10;
                        for (int I = 0, last = FLines.Count - 1; I <= last; I++)
                        {
                            Pos = 0;
                            switch (Align)
                            {
                                case TaowTextAlign.Left:
                                    X = FBorderWidth;
                                    break;
                                case TaowTextAlign.Center:
                                    X = 0;
                                    break;
                                case TaowTextAlign.Right:
                                    X = FTextWidth - FBorderWidth;
                                    break;
                            }

                            while (CI.FLine == I && Line <= FChanges.Count)
                            {
                                if (!IsEmpty(FontName) && CI.FPos > Pos)
                                {
                                    switch (Align)
                                    {
                                        case TaowTextAlign.Left:
                                            var line = FLines[I];
                                            FAOWFont.SetText(X, Y, Copy(line, Pos + 1, CI.FPos - Pos), BackSD);
                                            var line2 = FLines[I];
                                            X = X + FAOWFont.GetTextWidth(Copy(line2, Pos + 1, CI.FPos - Pos));
                                            break;
                                        case TaowTextAlign.Center:
                                            var line3 = FLines[I];
                                            var textWidth = FAOWFont.GetTextWidth(Copy(line3, Pos + 1, CI.FPos - Pos));
                                            var line4 = FLines[I];
                                            FAOWFont.SetText(FTextWidth / 2 - textWidth / 2, Y, Copy(line4, Pos + 1, CI.FPos - Pos), BackSD);
                                            break;
                                        case TaowTextAlign.Right:
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
                                        case TaowTextAlign.Left:
                                            X = FBorderWidth;
                                            break;
                                        case TaowTextAlign.Center:
                                            X = 0;
                                            break;
                                        case TaowTextAlign.Right:
                                            X = FTextWidth - FBorderWidth;
                                            break;
                                    }
                                }

                                Align = CI.FAlign;
                                if (Line < FChanges.Count)
                                {
                                    CI = (ChangeInfo)FChanges[Line];
                                }

                                Line += 1;
                            }

                            if (!IsEmpty(FontName))
                            {
                                switch (Align)
                                {
                                    case TaowTextAlign.Left:
                                        var lineLength = FLines[I];
                                        var line = FLines[I];
                                        FAOWFont.SetText(X, Y, Copy(line, Pos + 1, lineLength.Length - Pos), BackSD);
                                        break;
                                    case TaowTextAlign.Center:
                                        var line2 = FLines[I];
                                        var line3 = FLines[I];
                                        var textWidth = FAOWFont.GetTextWidth(Copy(line3, Pos + 1, line2.Length - Pos));
                                        var line4 = FLines[I];
                                        var line5 = FLines[I];
                                        FAOWFont.SetText(FTextWidth / 2 - textWidth / 2, Y, Copy(line5, Pos + 1, line4.Length - Pos), BackSD);
                                        break;
                                    case TaowTextAlign.Right:
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

                            SrcR = global::Aow.Rtl.RtlUnit.Rect(R.R.Left - FAOWOwner.FWinLeft - FWinLeft, FTopItem + R.R.Top - FAOWOwner.FWinTop - FWinTop, R.R.Right - FAOWOwner.FWinLeft - FWinLeft, FTopItem + R.R.Bottom - FAOWOwner.FWinTop - FWinTop);
                            if (FMasked)
                            {
                                Aow.Game.Draw.SurfaceDesc.BltSpriteClipped16(BackSD, SrcR, SurfDesc, R.R.Left, R.R.Top, FMaskColor);
                            }
                            else
                            {
                                Aow.Game.Draw.SurfaceDesc.BltSDRectClipped(BackSD, SrcR, SurfDesc, R.R.Left, R.R.Top);
                            }
                        }
                    }
                    else
                    {
                        SrcR = global::Aow.Rtl.RtlUnit.Rect(0, FTopItem, FWinWidth, FTopItem + FWinHeight);
                        if (FMasked)
                        {
                            Aow.Game.Draw.SurfaceDesc.BltSpriteClipped16(BackSD, SrcR, SurfDesc, FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FMaskColor);
                        }
                        else
                        {
                            Aow.Game.Draw.SurfaceDesc.BltSDRectClipped(BackSD, SrcR, SurfDesc, FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop);
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
        public void SetAOWWindow(AOWBaseWindow Value)
        {
            SetOwner(Value);
        }

        // AoWHTMLMemo.pas:139
        public void SetFStrings(StringList Value)
        {
            FItems.Clear();
            FItems.AddStrings(Value);
        }

        // AoWHTMLMemo.pas:145
        public void SetVScroll(AOWVScrollBar Value)
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
            ImageLibraryFont Font;
            int I;
            FileName = ExtractFilePath(global::Aow.Rtl.RtlUnit.Application.ExeName) + FileName;
            Font = null;
            I = 0;
            while (I < FFonts.Count && Font == null)
            {
                if (((CustomImageLibrary)FFonts[I]).FileName == FileName)
                {
                    Font = (ImageLibraryFont)FFonts[I];
                }

                I += 1;
            }

            if (Font == null && FileExists(FileName))
            {
                Font = new ImageLibraryFont(this);
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

        public StringList Items { get => FItems; set => SetFStrings(value); }
        public int ListOffset { get => FTopItem; set => SetListOff(value); }

        [Published]
        public AOWBaseWindow AOWWindow { get => FAOWWindow; set => SetAOWWindow(value); }

        [Published]
        public AOWVScrollBar VScrollBar { get => FVScrollBar; set => SetVScroll(value); }

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

        public new NotifyEvent<TaowHTMLMemo> OnChange
        {
            get => EventSlot.Get(this, FOnChange);
            set => EventSlot.Set(this, ref FOnChange, value);
        }

        public new MouseEvent<TaowHTMLMemo> OnMouseDown
        {
            get => EventSlot.Get(this, FOnMouseDown);
            set => EventSlot.Set(this, ref FOnMouseDown, value);
        }

        public new MouseEvent<TaowHTMLMemo> OnMouseUp
        {
            get => EventSlot.Get(this, FOnMouseUp);
            set => EventSlot.Set(this, ref FOnMouseUp, value);
        }

        public new MouseMoveEvent<TaowHTMLMemo> OnMouseMove
        {
            get => EventSlot.Get(this, FOnMouseMove);
            set => EventSlot.Set(this, ref FOnMouseMove, value);
        }

        public new KeyPressEvent<TaowHTMLMemo> OnKeyPress
        {
            get => EventSlot.Get(this, FOnKeyPress);
            set => EventSlot.Set(this, ref FOnKeyPress, value);
        }

        public new KeyEvent<TaowHTMLMemo> OnKeyDown
        {
            get => EventSlot.Get(this, FOnKeyDown);
            set => EventSlot.Set(this, ref FOnKeyDown, value);
        }

        public new AoWOnDrawEvent<TaowHTMLMemo> OnPreDraw
        {
            get => EventSlot.Get(this, FOnPreDraw);
            set => EventSlot.Set(this, ref FOnPreDraw, value);
        }

        public new AoWOnDrawEvent<TaowHTMLMemo> OnDraw
        {
            get => EventSlot.Get(this, FOnDraw);
            set => EventSlot.Set(this, ref FOnDraw, value);
        }

        public new NotifyEvent<TaowHTMLMemo> OnUpdate
        {
            get => EventSlot.Get(this, FOnUpdate);
            set => EventSlot.Set(this, ref FOnUpdate, value);
        }

        public new AoWOnDrawBackEvent<TaowHTMLMemo> OnDrawBack
        {
            get => EventSlot.Get(this, FOnDrawBack);
            set => EventSlot.Set(this, ref FOnDrawBack, value);
        }

        public new NotifyEvent<TaowHTMLMemo> OnEnter
        {
            get => EventSlot.Get(this, FOnEnter);
            set => EventSlot.Set(this, ref FOnEnter, value);
        }

        public new NotifyEvent<TaowHTMLMemo> OnExit
        {
            get => EventSlot.Get(this, FOnExit);
            set => EventSlot.Set(this, ref FOnExit, value);
        }

        public new NotifyEvent<TaowHTMLMemo> OnHide
        {
            get => EventSlot.Get(this, FOnHide);
            set => EventSlot.Set(this, ref FOnHide, value);
        }

        public new NotifyEvent<TaowHTMLMemo> OnShow
        {
            get => EventSlot.Get(this, FOnShow);
            set => EventSlot.Set(this, ref FOnShow, value);
        }

        [LegacyObject]
        // a style change in the text. FHeight (the line height), FColor, FFont (the font file, '' for
        // the current font), FAlign, FPos (the character of the line where it starts), FLine, FSpacing
        public partial class ChangeInfo
        {
            public int FHeight;
            public int FColor;
            public string FFont = "";
            public TaowTextAlign FAlign;
            public int FPos;
            public int FLine;
            public int FSpacing;

            public ChangeInfo()
            {
                CaptureAddress.Assign(this);
            }
        }
    }
}
