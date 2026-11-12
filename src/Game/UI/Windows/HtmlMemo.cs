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
    public List<ChangeInfo> Changes { get; }

    private StringList _lines;
    private StringList _items;
    private VerticalScrollBar _verticalScrollBar;
    private string _fileName = "";
    private int _scrollWidth;
    private bool _backDrawn;
    private InterfaceImageLibrary _images;
    private int _topItem;
    private List<ImageLibraryFont> _fonts;
    private BaseWindow _window;
    private int _textWidth;
    private int _textHeight;

    // AoWHTMLMemo.pas:93
    public HtmlMemo(Component owner) : base(owner)
    {
        _lines = new StringList();
        _items = new StringList();
        _items.OnChange = OnChangeText;
        Changes = new List<ChangeInfo>();
        _fonts = new List<ImageLibraryFont>();
        _images = new InterfaceImageLibrary(this);
    }

    // AoWHTMLMemo.pas:105
    public override void Dispose()
    {
        _lines.Dispose();
        _items.Dispose();
        for (int I = 0, last = Changes.Count - 1; I <= last; I++)
        {
        }

        for (int I = 0, last = _fonts.Count - 1; I <= last; I++)
        {
            (_fonts[I])?.Dispose();
        }

        _fonts?.Clear();
        Changes?.Clear();
        _images?.Dispose();
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
            _lines.Add("");
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
                var line = _lines[lineIndex];
                W = _font.GetTextWidth(line) + _borderWidth * 2 + _scrollWidth;
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
                        if (W + tw > WinWidth)
                        {
                            NewLine();
                            var line2 = _lines[lineIndex];
                            _lines[lineIndex] = line2 + word;
                            W = _borderWidth * 2 + _scrollWidth + tw;
                        }
                        else
                        {
                            var line2 = _lines[lineIndex];
                            _lines[lineIndex] = line2 + word;
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

        _scrollWidth = 0;
        if (_verticalScrollBar != null)
        {
            _scrollWidth = _verticalScrollBar.WinWidth;
            _verticalScrollBar.Position = _topItem;
        }

        H = _textHeight;
        _lines.Clear();
        state = IntegerParseMode.None;
        if (_items.Count > 0 && UpperCase(Copy(_items[0], 1, 5)) == "<INT>")
        {
            _lines.Add("");
            H = _borderHeight * 2;
            _images.Clear();
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
                                var line = _lines[lineIndex];
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
                                var line = _lines[lineIndex];
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
                                var line = _lines[lineIndex];
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
                                var line = _lines[lineIndex];
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
                                var line = _lines[lineIndex];
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
                                    _images.LoadFromFile(filePath + Copy(item, tagStart + 1, J - tagStart));
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

        if (WinWidth - _scrollWidth != _textWidth || _textHeight != H)
        {
            _textWidth = WinWidth - _scrollWidth;
            _textHeight = H;
            if (_textHeight < WinHeight)
            {
                _textHeight = WinHeight;
            }

            if (BackSurface != null)
            {
                BackSurface.Resize(_textWidth, _textHeight);
            }
        }

        SurfaceValid = false;
        _backDrawn = false;
        if (_verticalScrollBar != null)
        {
            if (_window != _verticalScrollBar.Window)
            {
                _verticalScrollBar.Window = _window;
            }

            _verticalScrollBar.Min = 0;
            _verticalScrollBar.Max = _textHeight;
            _verticalScrollBar.Priority = Priority + 1;
            _verticalScrollBar.Position = _topItem;
            _verticalScrollBar.NumberVisible = WinHeight;
            _verticalScrollBar.WinHeight = WinHeight;
            _verticalScrollBar.LargeChange = WinHeight;
            _verticalScrollBar.SmallChange = 20;
            _verticalScrollBar.Visible = Visible;
            _verticalScrollBar.Sized = false;
            _verticalScrollBar.WinLeft = WinLeft + WinWidth - _verticalScrollBar.WinWidth;
            _verticalScrollBar.WinTop = WinTop;
        }

        base.SetSize();
    }

    // AoWHTMLMemo.pas:747
    // { Update follows the scroll bar; a masked memo asks its owner to redraw it under the text }
    public override void Update()
    {
        Rect R;
        if (Visible)
        {
            if (_verticalScrollBar != null && _lines.Count > 0)
            {
                if (_verticalScrollBar.Position != _topItem)
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
        if (Visible && Masked && !SurfaceValid)
        {
            if (OwnerComponent.OwnerComponent.OwnerComponent == null)
            {
                R = new Rect(WinLeft, WinTop, WinLeft + WinWidth - 1, WinTop + WinHeight - 1);
            }
            else
            {
                R = new Rect(OwnerComponent.WinLeft + WinLeft, OwnerComponent.WinTop + WinTop, OwnerComponent.WinLeft + WinLeft + WinWidth - 1, OwnerComponent.WinTop + WinTop + WinHeight - 1);
            }

            OwnerComponent.AddDrawRectangle(ref R, this);
        }
    }

    // AoWHTMLMemo.pas:122
    public override void InvalidateRectangle(int index, ref Rect rectangle, UIComponent sender)
    {
        if (Valid && Visible)
        {
            AddDrawRectangle(ref rectangle, sender);
        }
    }

    // AoWHTMLMemo.pas:128
    public override void SetOwner(UIComponent value)
    {
        base.SetOwner(value);
        _window = (BaseWindow)OwnerComponent;
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
            if (Valid && Visible)
            {
                oldLeft = WinLeft;
                oldTop = WinTop;
                SetWinLeftWithoutInvalidate(0);
                SetWinTopWithoutInvalidate(0);
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
                    BackSurface = new DataSurface(_textWidth, _textHeight, lockedSurface.PixelFormatId, null);
                    _backDrawn = false;
                }

                if (!_backDrawn)
                {
                    if (_images.Count <= _imageLibraryIndexTexture)
                    {
                        if (Masked)
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
                        var rectangle = new SizedRectangle(0, 0, _textWidth, _textHeight);
                        _images.DrawTiled(_imageLibraryIndexTexture, rectangle, ref backSurfaceDescription);
                    }

                    Line = 1;
                    ci = Changes[0];
                    align = TextAlignment.Left;
                    fontName = "";
                    X = _borderWidth;
                    Y = _borderHeight;
                    lh = 10;
                    for (int I = 0, last = _lines.Count - 1; I <= last; I++)
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
                                X = _textWidth - _borderWidth;
                                break;
                        }

                        while (ci.Line == I && Line <= Changes.Count)
                        {
                            if (!IsEmpty(fontName) && ci.Position > position)
                            {
                                switch (align)
                                {
                                    case TextAlignment.Left:
                                        var line = _lines[I];
                                        _font.SetText(X, Y, Copy(line, position + 1, ci.Position - position), backSurfaceDescription);
                                        var line2 = _lines[I];
                                        X = X + _font.GetTextWidth(Copy(line2, position + 1, ci.Position - position));
                                        break;
                                    case TextAlignment.Center:
                                        var line3 = _lines[I];
                                        var textWidth = _font.GetTextWidth(Copy(line3, position + 1, ci.Position - position));
                                        var line4 = _lines[I];
                                        _font.SetText(_textWidth / 2 - textWidth / 2, Y, Copy(line4, position + 1, ci.Position - position), backSurfaceDescription);
                                        break;
                                    case TextAlignment.Right:
                                        var line5 = _lines[I];
                                        var fontTextWidth = _font.GetTextWidth(Copy(line5, position + 1, ci.Position - position));
                                        var line6 = _lines[I];
                                        _font.SetText(X - fontTextWidth, Y, Copy(line6, position + 1, ci.Position - position), backSurfaceDescription);
                                        var line7 = _lines[I];
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
                                        X = _textWidth - _borderWidth;
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
                                    var lineLength = _lines[I];
                                    var line = _lines[I];
                                    _font.SetText(X, Y, Copy(line, position + 1, lineLength.Length - position), backSurfaceDescription);
                                    break;
                                case TextAlignment.Center:
                                    var line2 = _lines[I];
                                    var line3 = _lines[I];
                                    var textWidth = _font.GetTextWidth(Copy(line3, position + 1, line2.Length - position));
                                    var line4 = _lines[I];
                                    var line5 = _lines[I];
                                    _font.SetText(_textWidth / 2 - textWidth / 2, Y, Copy(line5, position + 1, line4.Length - position), backSurfaceDescription);
                                    break;
                                case TextAlignment.Right:
                                    var line6 = _lines[I];
                                    var line7 = _lines[I];
                                    var fontTextWidth = _font.GetTextWidth(Copy(line7, position + 1, line6.Length - position));
                                    var line8 = _lines[I];
                                    var line9 = _lines[I];
                                    _font.SetText(X - fontTextWidth, Y, Copy(line9, position + 1, line8.Length - position), backSurfaceDescription);
                                    break;
                            }
                        }

                        Y = Y + lh;
                    }

                    BackSurface.Unlock();
                    _backDrawn = true;
                }

                SetWinLeftWithoutInvalidate(oldLeft);
                SetWinTopWithoutInvalidate(oldTop);
                RaisePreDraw(this, lockedSurface);

                BackSurface.Lock(ref backSurfaceDescription);
                if (SurfaceValid)
                {
                    for (int I = 0, last = DrawCount - 1; I <= last; I++)
                    {
                        R = DrawList[I];
                        if (OwnerComponent.WinLeft + WinLeft > R.Rectangle.Left)
                        {
                            R.Rectangle.Left = OwnerComponent.WinLeft + WinLeft;
                        }

                        if (OwnerComponent.WinTop + WinTop > R.Rectangle.Top)
                        {
                            R.Rectangle.Top = OwnerComponent.WinTop + WinTop;
                        }

                        if (OwnerComponent.WinLeft + WinLeft + WinWidth < R.Rectangle.Right)
                        {
                            R.Rectangle.Right = OwnerComponent.WinLeft + WinLeft + WinWidth;
                        }

                        if (OwnerComponent.WinTop + WinTop + WinHeight < R.Rectangle.Bottom)
                        {
                            R.Rectangle.Bottom = OwnerComponent.WinTop + WinTop + WinHeight;
                        }

                        sourceR = new Rect(R.Rectangle.Left - OwnerComponent.WinLeft - WinLeft, _topItem + R.Rectangle.Top - OwnerComponent.WinTop - WinTop, R.Rectangle.Right - OwnerComponent.WinLeft - WinLeft, _topItem + R.Rectangle.Bottom - OwnerComponent.WinTop - WinTop);
                        if (Masked)
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
                    sourceR = new Rect(0, _topItem, WinWidth, _topItem + WinHeight);
                    if (Masked)
                    {
                        LockedSurface.BlitSpriteClipped16(backSurfaceDescription, sourceR, lockedSurface, _window.WinLeft + WinLeft, _window.WinTop + WinTop, _maskColor);
                    }
                    else
                    {
                        LockedSurface.BlitSurfaceDescriptionRectangleClipped(backSurfaceDescription, sourceR, lockedSurface, _window.WinLeft + WinLeft, _window.WinTop + WinTop);
                    }
                }

                BackSurface.Unlock();
                RaiseDraw(this, lockedSurface);

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
                _verticalScrollBar.Priority = Priority;
            }

            Sized = false;
        }
    }

    // AoWHTMLMemo.pas:156
    public void SetListOff(int value)
    {
        if (value != _topItem)
        {
            _topItem = value;
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
        while (I < _fonts.Count && font == null)
        {
            if ((_fonts[I]).FileName == fileName)
            {
                font = _fonts[I];
            }

            I += 1;
        }

        if (font == null && FileExists(fileName))
        {
            font = new ImageLibraryFont(this);
            font.FontColorPaletteIndex = 1;
            font.LoadFromFile(fileName);
            _fonts.Add(font);
        }

        if (font != null)
        {
            _font = font;
        }

        return font != null;
    }

    public StringList Items { get => _items; set => SetFStrings(value); }
    public int ListOffset { get => _topItem; set => SetListOff(value); }

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

    public new event NotifyEvent<HtmlMemo> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<HtmlMemo> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<HtmlMemo> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<HtmlMemo> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<HtmlMemo> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<HtmlMemo> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<HtmlMemo> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<HtmlMemo> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<HtmlMemo> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(OnUpdate), value: value);
    }

    public new event AoWOnDrawBackEvent<HtmlMemo> OnDrawBack
    {
        add => ((WindowControl)this).OnDrawBack += EventSlot.Add<WindowControl, HtmlMemo>(this, slot: nameof(OnDrawBack), value: value);
        remove => ((WindowControl)this).OnDrawBack -= EventSlot.Remove<WindowControl, HtmlMemo>(this, slot: nameof(OnDrawBack), value: value);
    }

    public new event NotifyEvent<HtmlMemo> OnEnter
    {
        add => ((WindowControl)this).OnEnter += EventSlot.Add<WindowControl, HtmlMemo>(this, slot: nameof(OnEnter), value: value);
        remove => ((WindowControl)this).OnEnter -= EventSlot.Remove<WindowControl, HtmlMemo>(this, slot: nameof(OnEnter), value: value);
    }

    public new event NotifyEvent<HtmlMemo> OnExit
    {
        add => ((WindowControl)this).OnExit += EventSlot.Add<WindowControl, HtmlMemo>(this, slot: nameof(OnExit), value: value);
        remove => ((WindowControl)this).OnExit -= EventSlot.Remove<WindowControl, HtmlMemo>(this, slot: nameof(OnExit), value: value);
    }

    public new event NotifyEvent<HtmlMemo> OnHide
    {
        add => ((BaseWindow)this).OnHide += EventSlot.Add<BaseWindow, HtmlMemo>(this, slot: nameof(OnHide), value: value);
        remove => ((BaseWindow)this).OnHide -= EventSlot.Remove<BaseWindow, HtmlMemo>(this, slot: nameof(OnHide), value: value);
    }

    public new event NotifyEvent<HtmlMemo> OnShow
    {
        add => ((BaseWindow)this).OnShow += EventSlot.Add<BaseWindow, HtmlMemo>(this, slot: nameof(OnShow), value: value);
        remove => ((BaseWindow)this).OnShow -= EventSlot.Remove<BaseWindow, HtmlMemo>(this, slot: nameof(OnShow), value: value);
    }

    // a style change in the text. FHeight (the line height), FColor, FFont (the font file, '' for
    // the current font), FAlign, FPos (the character of the line where it starts), FLine, FSpacing
    public partial class ChangeInfo
    {
        public int Height { get; set; }
        public int Color { get; set; }
        public string Font { get; set; } = "";
        public TextAlignment Align { get; set; }
        public int Position { get; set; }
        public int Line { get; set; }
        public int Spacing { get; set; }
    }
}
