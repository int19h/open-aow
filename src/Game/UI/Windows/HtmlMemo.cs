// Translated from AoWHTMLMemo.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Windows;

// Fields: F1EC (a list that is created and freed only), FChanges (TChangeInfo), FLines (the
// wrapped lines), FItems (the text), FVScrollBar, FFilename, FScrollWidth, FBackDrawn (the back
// surface holds the text), FImages (the background textures), FTopItem (the first pixel row
// shown), FFonts (the fonts loaded, TImageLibraryFont), FAOWWindow, FTextWidth, FTextHeight
// (the size of the back surface)
public partial class HtmlMemo : BaseWindow
{
    public DelphiList UnusedList;
    public List<ChangeInfo> Changes;
    public StringList Lines;
    public StringList _items;
    public VerticalScrollBar _verticalScrollBar;
    public string _fileName = "";
    public int ScrollWidth;
    public bool BackDrawn;
    public InterfaceImageLibrary Images;
    public int TopItem;
    public List<ImageLibraryFont> Fonts;
    public BaseWindow _window;
    public int TextWidth;
    public int TextHeight;

    // AoWHTMLMemo.pas:93
    public HtmlMemo(Component owner) : base(owner)
    {
        Lines = new StringList();
        _items = new StringList();
        _items.OnChange = OnChangeText;
        Changes = new List<ChangeInfo>();
        UnusedList = new DelphiList();
        Fonts = new List<ImageLibraryFont>();
        Images = new InterfaceImageLibrary(this);
    }

    // AoWHTMLMemo.pas:105
    public override void Dispose()
    {
        Lines.Dispose();
        _items.Dispose();
        UnusedList?.Dispose();
        for (int I = 0, last = Changes.Count - 1; I <= last; I++)
        {
        }

        for (int I = 0, last = Fonts.Count - 1; I <= last; I++)
        {
            (Fonts[I])?.Dispose();
        }

        Fonts?.Clear();
        Changes?.Clear();
        Images?.Dispose();
        base.Dispose();
    }

    // AoWHTMLMemo.pas:174
    public override void OnChangeText(object sender)
    {
        SetListOff(0);
        Sized = false;
    }

    // AoWHTMLMemo.pas:210
    // { SetSize parses the text into FLines and FChanges, wrapping the lines at the width of the
    //   window, and sets up the scroll bar }
    public override void SetSize()
    {
        ChangeInfo ci;
        int H;
        int lineIndex;
        IntegerParseMode state;
        int Last;
        int textStart;
        int textEnd;
        int tagStart;
        byte C;
        ReadOnlySpan<byte> text;
        string S;
        int V;
        void NewLine()
        {
            Lines.Add("");
            H = H + ci.Height;
            lineIndex += 1;
        }

        void AddText(string S)
        {
            int tw;
            int W;
            int start;
            string word;
            int I;
            int length;
            if (_font != null)
            {
                I = 0;
                start = 0;
                length = S.Length;
                var line = Lines[lineIndex];
                W = _font.GetTextWidth(line) + _borderWidth * 2 + ScrollWidth;
                while (I < length)
                {
                    if (S[I - 1] == 32 || I == length - 1)
                    {
                        if (I == length - 1)
                        {
                            I += 1;
                        }

                        word = Copy(S, start + 1, I - start);
                        tw = _font.GetTextWidth(word);
                        if (W + tw > _winWidth)
                        {
                            NewLine();
                            var line2 = Lines[lineIndex];
                            Lines[lineIndex] = line2 + word;
                            W = _borderWidth * 2 + ScrollWidth + tw;
                        }
                        else
                        {
                            var line2 = Lines[lineIndex];
                            Lines[lineIndex] = line2 + word;
                            W = W + tw;
                        }

                        start = I;
                    }

                    I += 1;
                }
            }
        }

        void NewChange()
        {
            ChangeInfo N;
            N = new ChangeInfo();
            N.Height = ci.Height;
            N.Color = ci.Color;
            N.Font = ci.Font;
            N.Align = ci.Align;
            N.Spacing = ci.Spacing;
            N.Position = ci.Position;
            N.Line = ci.Line;
            Changes.Add(N);
            ci = N;
        }

        void ParseTag(string tag)
        {
            tag = UpperCase(tag);
            switch (state)
            {
                case IntegerParseMode.InBackground:
                    if (tag == "INDEX")
                    {
                        state = IntegerParseMode.BackgroundIndex;
                    }

                    break;
                default:
                    if (tag == "P")
                    {
                        NewLine();
                        NewLine();
                    }
                    else
                    {
                        if (tag == "BR")
                        {
                            NewLine();
                        }
                        else
                        {
                            if (tag == "FONT")
                            {
                                state = IntegerParseMode.Font;
                            }
                            else
                            {
                                if (tag == "ITEMHEIGHT")
                                {
                                    state = IntegerParseMode.ItemHeight;
                                }
                                else
                                {
                                    if (tag == "COLOR")
                                    {
                                        state = IntegerParseMode.Color;
                                    }
                                    else
                                    {
                                        if (tag == "SPACING")
                                        {
                                            state = IntegerParseMode.Space;
                                        }
                                        else
                                        {
                                            if (tag == "ALIGN")
                                            {
                                                state = IntegerParseMode.Align;
                                            }
                                            else
                                            {
                                                if (tag == "BACKGROUND")
                                                {
                                                    state = IntegerParseMode.Background;
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

        ScrollWidth = 0;
        if (_verticalScrollBar != null)
        {
            ScrollWidth = _verticalScrollBar.WinWidth;
            _verticalScrollBar.Position = TopItem;
        }

        H = TextHeight;
        Lines.Clear();
        state = IntegerParseMode.None;
        if (_items.Count > 0 && UpperCase(Copy(_items[0], 1, 5)) == "<INT>")
        {
            Lines.Add("");
            H = _borderHeight * 2;
            Images.Clear();
            for (int I = 0, last = Changes.Count - 1; I <= last; I++)
            {
            }

            Changes.Clear();
            ci = new ChangeInfo();
            ci.Height = 10;
            ci.Color = 16777215;
            ci.Font = "";
            ci.Align = TextAlignment.Left;
            ci.Position = 0;
            ci.Line = 0;
            ci.Spacing = 0;
            Changes.Add(ci);
            lineIndex = 0;
            for (int I = 0, last = _items.Count - 1; I <= last; I++)
            {
                string item = NullTerminatedText(_items[I]);
                text = Cp1252.Encode(item);
                Last = text.Length - 1;
                textStart = 0;
                textEnd = -1;
                tagStart = 0;
                for (int J = 0; J <= Last; J++)
                {
                    C = text[J];
                    switch (state)
                    {
                        case IntegerParseMode.None:
                            switch (C)
                            {
                                case 60:
                                    state = IntegerParseMode.Start;
                                    textEnd = J - 1;
                                    tagStart = J + 1;
                                    break;
                            }

                            break;
                        case IntegerParseMode.Start:
                        case IntegerParseMode.InBackground:
                            switch (C)
                            {
                                case 62:
                                    ParseTag(Copy(item, tagStart + 1, J - tagStart));
                                    state = IntegerParseMode.None;
                                    textStart = J + 1;
                                    break;
                                case 32:
                                case 61:
                                    ParseTag(Copy(item, tagStart + 1, J - tagStart));
                                    tagStart = J + 1;
                                    break;
                            }

                            break;
                        case IntegerParseMode.Font:
                            if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                            {
                                var line = Lines[lineIndex];
                                ci.Position = line.Length;
                                ci.Line = lineIndex;
                                ci.Font = Copy(item, tagStart + 1, J - tagStart);
                                if (!IsEmpty(ci.Font))
                                {
                                    if (!LoadFont(ci.Font))
                                    {
                                        ci.Font = "";
                                    }
                                }

                                NewChange();
                                if (C == 62)
                                {
                                    state = IntegerParseMode.None;
                                    textStart = J + 1;
                                }
                                else
                                {
                                    state = IntegerParseMode.Start;
                                    tagStart = J + 1;
                                }
                            }

                            break;
                        case IntegerParseMode.ItemHeight:
                            if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                            {
                                var line = Lines[lineIndex];
                                ci.Position = line.Length;
                                ci.Line = lineIndex;
                                try
                                {
                                    ci.Height = StrToInt(Copy(item, tagStart + 1, J - tagStart));
                                }
                                catch
                                {
                                }

                                NewChange();
                                if (C == 62)
                                {
                                    state = IntegerParseMode.None;
                                    textStart = J + 1;
                                }
                                else
                                {
                                    state = IntegerParseMode.Start;
                                    tagStart = J + 1;
                                }
                            }

                            break;
                        case IntegerParseMode.Color:
                            if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                            {
                                var line = Lines[lineIndex];
                                ci.Position = line.Length;
                                ci.Line = lineIndex;
                                if (J - tagStart == 6)
                                {
                                    ci.Color = 0;
                                    for (int K = 0; K <= 5; K++)
                                    {
                                        V = text[tagStart + K];
                                        if (V < 58)
                                        {
                                            V = V - 48;
                                        }
                                        else
                                        {
                                            V = V - 55;
                                        }

                                        ci.Color = (ci.Color << 4) + V;
                                    }

                                    NewChange();
                                }

                                if (C == 62)
                                {
                                    state = IntegerParseMode.None;
                                    textStart = J + 1;
                                }
                                else
                                {
                                    state = IntegerParseMode.Start;
                                    tagStart = J + 1;
                                }
                            }

                            break;
                        case IntegerParseMode.Space:
                            if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                            {
                                var line = Lines[lineIndex];
                                ci.Position = line.Length;
                                ci.Line = lineIndex;
                                try
                                {
                                    ci.Spacing = StrToInt(Copy(item, tagStart + 1, J - tagStart));
                                    if (_font != null)
                                    {
                                        _font.CharacterDistance = ci.Spacing;
                                    }
                                }
                                catch
                                {
                                }

                                NewChange();
                                if (C == 62)
                                {
                                    state = IntegerParseMode.None;
                                    textStart = J + 1;
                                }
                                else
                                {
                                    state = IntegerParseMode.Start;
                                    tagStart = J + 1;
                                }
                            }

                            break;
                        case IntegerParseMode.Align:
                            if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                            {
                                S = UpperCase(Copy(item, tagStart + 1, J - tagStart));
                                var line = Lines[lineIndex];
                                ci.Position = line.Length;
                                ci.Line = lineIndex;
                                if (S == "RIGHT")
                                {
                                    ci.Align = TextAlignment.Right;
                                }
                                else
                                {
                                    if (S == "CENTER")
                                    {
                                        ci.Align = TextAlignment.Center;
                                    }
                                    else
                                    {
                                        ci.Align = TextAlignment.Left;
                                    }
                                }

                                NewChange();
                                if (C == 62)
                                {
                                    state = IntegerParseMode.None;
                                    textStart = J + 1;
                                }
                                else
                                {
                                    state = IntegerParseMode.Start;
                                    tagStart = J + 1;
                                }
                            }

                            break;
                        case IntegerParseMode.Background:
                            if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                            {
                                if (FileExists(ExtractFilePath(RtlUnit.Application.ExeName) + Copy(item, tagStart + 1, J - tagStart)))
                                {
                                    var filePath = ExtractFilePath(RtlUnit.Application.ExeName);
                                    Images.LoadFromFile(filePath + Copy(item, tagStart + 1, J - tagStart));
                                }

                                _imageLibraryIndexTexture = 0;
                                if (C == 62)
                                {
                                    state = IntegerParseMode.None;
                                    textStart = J + 1;
                                }
                                else
                                {
                                    state = IntegerParseMode.InBackground;
                                    tagStart = J + 1;
                                }
                            }

                            break;
                        case IntegerParseMode.BackgroundIndex:
                            if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(C))
                            {
                                try
                                {
                                    _imageLibraryIndexTexture = StrToInt(Copy(item, tagStart + 1, J - tagStart));
                                }
                                catch
                                {
                                }

                                if (C == 62)
                                {
                                    state = IntegerParseMode.None;
                                    textStart = J + 1;
                                }
                                else
                                {
                                    state = IntegerParseMode.InBackground;
                                    tagStart = J + 1;
                                }
                            }

                            break;
                    }

                    if (textEnd >= textStart)
                    {
                        AddText(Copy(item, textStart + 1, textEnd - textStart + 1));
                        textStart = textEnd;
                    }
                }

                switch (state)
                {
                    case IntegerParseMode.None:
                        if (textStart <= Last)
                        {
                            AddText(Copy(item, textStart + 1, Last - textStart + 1));
                        }

                        break;
                    case IntegerParseMode.Start:
                        if (tagStart <= Last)
                        {
                            ParseTag(Copy(item, tagStart + 1, Last - tagStart));
                        }

                        break;
                }
            }

            H = H + ci.Height;
        }

        if (_winWidth - ScrollWidth != TextWidth || TextHeight != H)
        {
            TextWidth = _winWidth - ScrollWidth;
            TextHeight = H;
            if (TextHeight < _winHeight)
            {
                TextHeight = _winHeight;
            }

            if (BackSurface != null)
            {
                BackSurface.Resize(TextWidth, TextHeight);
            }
        }

        SurfaceValid = false;
        BackDrawn = false;
        if (_verticalScrollBar != null)
        {
            if (_window != _verticalScrollBar.Window)
            {
                _verticalScrollBar.Window = _window;
            }

            _verticalScrollBar.Min = 0;
            _verticalScrollBar.Max = TextHeight;
            _verticalScrollBar.Priority = _priority + 1;
            _verticalScrollBar.Position = TopItem;
            _verticalScrollBar.NumberVisible = _winHeight;
            _verticalScrollBar.WinHeight = _winHeight;
            _verticalScrollBar.LargeChange = _winHeight;
            _verticalScrollBar.SmallChange = 20;
            _verticalScrollBar.Visible = _visible;
            _verticalScrollBar.Sized = false;
            _verticalScrollBar.WinLeft = _winLeft + _winWidth - _verticalScrollBar.WinWidth;
            _verticalScrollBar.WinTop = _winTop;
        }

        base.SetSize();
    }

    // AoWHTMLMemo.pas:747
    // { Update follows the scroll bar; a masked memo asks its owner to redraw it under the text }
    public override void Update()
    {
        Rect R;
        if (_visible)
        {
            if (_verticalScrollBar != null && Lines.Count > 0)
            {
                if (_verticalScrollBar.Position != TopItem)
                {
                    SetListOff(_verticalScrollBar.Position);
                }
            }

            if (!Drawn)
            {
                SurfaceValid = false;
            }

            Drawn = false;
        }

        base.Update();
        if (_visible && _masked && !SurfaceValid)
        {
            if (_ownerComponent._ownerComponent._ownerComponent == null)
            {
                R = new Rect(_winLeft, _winTop, _winLeft + _winWidth - 1, _winTop + _winHeight - 1);
            }
            else
            {
                R = new Rect(_ownerComponent._winLeft + _winLeft, _ownerComponent._winTop + _winTop, _ownerComponent._winLeft + _winLeft + _winWidth - 1, _ownerComponent._winTop + _winTop + _winHeight - 1);
            }

            _ownerComponent.AddDrawRectangle(ref R, this);
        }
    }

    // AoWHTMLMemo.pas:122
    public override void InvalidateRectangle(int index, ref Rect rectangle, UIComponent sender)
    {
        if (Valid && _visible)
        {
            AddDrawRectangle(ref rectangle, sender);
        }
    }

    // AoWHTMLMemo.pas:128
    public override void SetOwner(UIComponent value)
    {
        base.SetOwner(value);
        _window = (BaseWindow)_ownerComponent;
    }

    // AoWHTMLMemo.pas:584
    // { Draw renders the text into the back surface when it changed, then blits the dirty rectangles
    //   of the window (or the whole visible part) from it }
    public override void Draw(LockedSurface lockedSurface)
    {
        int oldLeft;
        int oldTop;
        int position = default;
        int lh = default;
        int X = default;
        int Y = default;
        TextAlignment align = default;
        string fontName = "";
        LockedSurface backSurfaceDescription = default;
        Rect sourceR = default;
        int Line = default;
        ChangeInfo ci = default;
        DirtyRectangle R = default;
        try
        {
            if (Valid && _visible)
            {
                oldLeft = _winLeft;
                oldTop = _winTop;
                _winLeft = 0;
                _winTop = 0;
                if (BackSurface != null)
                {
                    if (BackSurface.PixelFormatId != lockedSurface.PixelFormatId)
                    {
                        BackSurface?.Dispose();
                        BackSurface = null;
                    }
                }

                if (BackSurface == null)
                {
                    BackSurface = new DataSurface(TextWidth, TextHeight, lockedSurface.PixelFormatId, null);
                    BackDrawn = false;
                }

                if (!BackDrawn)
                {
                    if (Images.Count <= _imageLibraryIndexTexture)
                    {
                        if (_masked)
                        {
                            BackSurface.Clear(_maskColor);
                        }
                        else
                        {
                            BackSurface.Clear(0);
                        }

                        BackSurface.Lock(ref backSurfaceDescription);
                    }
                    else
                    {
                        BackSurface.Lock(ref backSurfaceDescription);
                        var rectangle = new Rect(0, 0, TextWidth, TextHeight);
                        Images.DrawTiled(_imageLibraryIndexTexture, rectangle, ref backSurfaceDescription);
                    }

                    Line = 1;
                    ci = Changes[0];
                    align = TextAlignment.Left;
                    fontName = "";
                    X = _borderWidth;
                    Y = _borderHeight;
                    lh = 10;
                    for (int I = 0, last = Lines.Count - 1; I <= last; I++)
                    {
                        position = 0;
                        switch (align)
                        {
                            case TextAlignment.Left:
                                X = _borderWidth;
                                break;
                            case TextAlignment.Center:
                                X = 0;
                                break;
                            case TextAlignment.Right:
                                X = TextWidth - _borderWidth;
                                break;
                        }

                        while (ci.Line == I && Line <= Changes.Count)
                        {
                            if (!IsEmpty(fontName) && ci.Position > position)
                            {
                                switch (align)
                                {
                                    case TextAlignment.Left:
                                        var line = Lines[I];
                                        _font.SetText(X, Y, Copy(line, position + 1, ci.Position - position), backSurfaceDescription);
                                        var line2 = Lines[I];
                                        X = X + _font.GetTextWidth(Copy(line2, position + 1, ci.Position - position));
                                        break;
                                    case TextAlignment.Center:
                                        var line3 = Lines[I];
                                        var textWidth = _font.GetTextWidth(Copy(line3, position + 1, ci.Position - position));
                                        var line4 = Lines[I];
                                        _font.SetText(TextWidth / 2 - textWidth / 2, Y, Copy(line4, position + 1, ci.Position - position), backSurfaceDescription);
                                        break;
                                    case TextAlignment.Right:
                                        var line5 = Lines[I];
                                        var fontTextWidth = _font.GetTextWidth(Copy(line5, position + 1, ci.Position - position));
                                        var line6 = Lines[I];
                                        _font.SetText(X - fontTextWidth, Y, Copy(line6, position + 1, ci.Position - position), backSurfaceDescription);
                                        var line7 = Lines[I];
                                        X = X - _font.GetTextWidth(Copy(line7, position + 1, ci.Position - position));
                                        break;
                                }
                            }

                            position = ci.Position;
                            if (!IsEmpty(ci.Font))
                            {
                                LoadFont(ci.Font);
                            }

                            _font.FontColorPaletteIndex = 1;
                            _font.Color = ci.Color;
                            _font.CharacterDistance = ci.Spacing;
                            fontName = ci.Font;
                            lh = ci.Height;
                            if (ci.Align != align)
                            {
                                switch (align)
                                {
                                    case TextAlignment.Left:
                                        X = _borderWidth;
                                        break;
                                    case TextAlignment.Center:
                                        X = 0;
                                        break;
                                    case TextAlignment.Right:
                                        X = TextWidth - _borderWidth;
                                        break;
                                }
                            }

                            align = ci.Align;
                            if (Line < Changes.Count)
                            {
                                ci = Changes[Line];
                            }

                            Line += 1;
                        }

                        if (!IsEmpty(fontName))
                        {
                            switch (align)
                            {
                                case TextAlignment.Left:
                                    var lineLength = Lines[I];
                                    var line = Lines[I];
                                    _font.SetText(X, Y, Copy(line, position + 1, lineLength.Length - position), backSurfaceDescription);
                                    break;
                                case TextAlignment.Center:
                                    var line2 = Lines[I];
                                    var line3 = Lines[I];
                                    var textWidth = _font.GetTextWidth(Copy(line3, position + 1, line2.Length - position));
                                    var line4 = Lines[I];
                                    var line5 = Lines[I];
                                    _font.SetText(TextWidth / 2 - textWidth / 2, Y, Copy(line5, position + 1, line4.Length - position), backSurfaceDescription);
                                    break;
                                case TextAlignment.Right:
                                    var line6 = Lines[I];
                                    var line7 = Lines[I];
                                    var fontTextWidth = _font.GetTextWidth(Copy(line7, position + 1, line6.Length - position));
                                    var line8 = Lines[I];
                                    var line9 = Lines[I];
                                    _font.SetText(X - fontTextWidth, Y, Copy(line9, position + 1, line8.Length - position), backSurfaceDescription);
                                    break;
                            }
                        }

                        Y = Y + lh;
                    }

                    BackSurface.Unlock();
                    BackDrawn = true;
                }

                _winLeft = oldLeft;
                _winTop = oldTop;
                if (_onPreDraw != null)
                {
                    _onPreDraw(this, lockedSurface);
                }

                BackSurface.Lock(ref backSurfaceDescription);
                if (SurfaceValid)
                {
                    for (int I = 0, last = DrawCount - 1; I <= last; I++)
                    {
                        R = DrawList[I];
                        if (_ownerComponent._winLeft + _winLeft > R.Rectangle.Left)
                        {
                            R.Rectangle.Left = _ownerComponent._winLeft + _winLeft;
                        }

                        if (_ownerComponent._winTop + _winTop > R.Rectangle.Top)
                        {
                            R.Rectangle.Top = _ownerComponent._winTop + _winTop;
                        }

                        if (_ownerComponent._winLeft + _winLeft + _winWidth < R.Rectangle.Right)
                        {
                            R.Rectangle.Right = _ownerComponent._winLeft + _winLeft + _winWidth;
                        }

                        if (_ownerComponent._winTop + _winTop + _winHeight < R.Rectangle.Bottom)
                        {
                            R.Rectangle.Bottom = _ownerComponent._winTop + _winTop + _winHeight;
                        }

                        sourceR = new Rect(R.Rectangle.Left - _ownerComponent._winLeft - _winLeft, TopItem + R.Rectangle.Top - _ownerComponent._winTop - _winTop, R.Rectangle.Right - _ownerComponent._winLeft - _winLeft, TopItem + R.Rectangle.Bottom - _ownerComponent._winTop - _winTop);
                        if (_masked)
                        {
                            LockedSurface.BlitSpriteClipped16(backSurfaceDescription, sourceR, lockedSurface, R.Rectangle.Left, R.Rectangle.Top, _maskColor);
                        }
                        else
                        {
                            LockedSurface.BlitSurfaceDescriptionRectangleClipped(backSurfaceDescription, sourceR, lockedSurface, R.Rectangle.Left, R.Rectangle.Top);
                        }
                    }
                }
                else
                {
                    sourceR = new Rect(0, TopItem, _winWidth, TopItem + _winHeight);
                    if (_masked)
                    {
                        LockedSurface.BlitSpriteClipped16(backSurfaceDescription, sourceR, lockedSurface, _window._winLeft + _winLeft, _window._winTop + _winTop, _maskColor);
                    }
                    else
                    {
                        LockedSurface.BlitSurfaceDescriptionRectangleClipped(backSurfaceDescription, sourceR, lockedSurface, _window._winLeft + _winLeft, _window._winTop + _winTop);
                    }
                }

                BackSurface.Unlock();
                if (_onDraw != null)
                {
                    _onDraw(this, lockedSurface);
                }

                SurfaceValid = true;
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AoWHTMLMemo.pas:134
    public void SetWindow(BaseWindow value)
    {
        SetOwner(value);
    }

    // AoWHTMLMemo.pas:139
    public void SetFStrings(StringList value)
    {
        _items.Clear();
        _items.AddStrings(value);
    }

    // AoWHTMLMemo.pas:145
    public void SetVScroll(VerticalScrollBar value)
    {
        if (value != _verticalScrollBar)
        {
            _verticalScrollBar = value;
            if (_verticalScrollBar != null)
            {
                _verticalScrollBar.Priority = _priority;
            }

            Sized = false;
        }
    }

    // AoWHTMLMemo.pas:156
    public void SetListOff(int value)
    {
        if (value != TopItem)
        {
            TopItem = value;
            SurfaceValid = false;
        }
    }

    // AoWHTMLMemo.pas:165
    public void SetFileName(string value)
    {
        if (value != _fileName && FileExists(value))
        {
            _fileName = value;
            _items.LoadFromFile(_fileName);
        }
    }

    // AoWHTMLMemo.pas:182
    // { LoadFont makes the font in FileName (relative to the program) the current font, loading it
    //   the first time }
    public bool LoadFont(string fileName)
    {
        ImageLibraryFont font;
        int I;
        fileName = ExtractFilePath(RtlUnit.Application.ExeName) + fileName;
        font = null;
        I = 0;
        while (I < Fonts.Count && font == null)
        {
            if ((Fonts[I]).FileName == fileName)
            {
                font = Fonts[I];
            }

            I += 1;
        }

        if (font == null && FileExists(fileName))
        {
            font = new ImageLibraryFont(this);
            font.FontColorPaletteIndex = 1;
            font.LoadFromFile(fileName);
            Fonts.Add(font);
        }

        if (font != null)
        {
            _font = font;
        }

        return font != null;
    }

    public StringList Items { get => _items; set => SetFStrings(value); }
    public int ListOffset { get => TopItem; set => SetListOff(value); }

    [Published]
    public BaseWindow Window { get => _window; set => SetWindow(value); }

    [Published]
    public VerticalScrollBar VerticalScrollBar { get => _verticalScrollBar; set => SetVScroll(value); }

    [Published]
    public string FileName { get => _fileName; set => SetFileName(value); }

    [Published]
    public bool FreeSurfaceOnHide { get => _freeSurfaceOnHide; set => _freeSurfaceOnHide = value; }

    [Published]
    public int BorderWidth { get => _borderWidth; set => _borderWidth = value; }

    [Published]
    public int BorderHeight { get => _borderHeight; set => _borderHeight = value; }

    [Published]
    public int MaskColor { get => _maskColor; set => _maskColor = value; }

    public new NotifyEvent<HtmlMemo> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<HtmlMemo> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<HtmlMemo> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<HtmlMemo> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<HtmlMemo> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<HtmlMemo> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<HtmlMemo> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<HtmlMemo> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<HtmlMemo> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }

    public new AoWOnDrawBackEvent<HtmlMemo> OnDrawBack
    {
        get => EventSlot.Get(this, _onDrawBack);
        set => EventSlot.Set(this, ref _onDrawBack, value);
    }

    public new NotifyEvent<HtmlMemo> OnEnter
    {
        get => EventSlot.Get(this, _onEnter);
        set => EventSlot.Set(this, ref _onEnter, value);
    }

    public new NotifyEvent<HtmlMemo> OnExit
    {
        get => EventSlot.Get(this, _onExit);
        set => EventSlot.Set(this, ref _onExit, value);
    }

    public new NotifyEvent<HtmlMemo> OnHide
    {
        get => EventSlot.Get(this, _onHide);
        set => EventSlot.Set(this, ref _onHide, value);
    }

    public new NotifyEvent<HtmlMemo> OnShow
    {
        get => EventSlot.Get(this, _onShow);
        set => EventSlot.Set(this, ref _onShow, value);
    }

    // a style change in the text. FHeight (the line height), FColor, FFont (the font file, '' for
    // the current font), FAlign, FPos (the character of the line where it starts), FLine, FSpacing
    public partial class ChangeInfo
    {
        public int Height;
        public int Color;
        public string Font = "";
        public TextAlignment Align;
        public int Position;
        public int Line;
        public int Spacing;
    }
}
