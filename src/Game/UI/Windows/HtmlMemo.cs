#nullable disable
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

    private readonly StringList _lines;
    private readonly StringList _items;
    private VerticalScrollBar _verticalScrollBar;
    private string _fileName = "";
    private int _scrollWidth;
    private bool _backDrawn;
    private readonly InterfaceImageLibrary _images;
    private int _topItem;
    private readonly List<ImageLibraryFont> _fonts;
    private BaseWindow _window;
    private int _textWidth;
    private int _textHeight;

    // AoWHTMLMemo.pas:93
    public HtmlMemo(Component owner) : base(owner)
    {
        _lines = new StringList();
        _items = new StringList
        {
            OnChange = OnChangeText
        };
        Changes = [];
        _fonts = [];
        _images = new InterfaceImageLibrary(this);
    }

    // AoWHTMLMemo.pas:105
    public override void Dispose()
    {
        _lines.Dispose();
        _items.Dispose();
        for (int i = 0, last = Changes.Count - 1; i <= last; i++)
        {
        }

        for (int i = 0, last = _fonts.Count - 1; i <= last; i++)
        {
            _fonts[i]?.Dispose();
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
        int h;
        int lineIndex;
        IntegerParseMode state;
        int last2;
        int textStart;
        int textEnd;
        int tagStart;
        byte c;
        ReadOnlySpan<byte> text;
        string s;
        int v;
        void NewLine()
        {
            _ = _lines.Add("");
            h += ci.Height;
            lineIndex += 1;
        }

        void AddText(string s)
        {
            int tw;
            int w;
            int start;
            string word;
            int i;
            int length;
            if (Font != null)
            {
                i = 0;
                start = 0;
                length = s.Length;
                string line = _lines[lineIndex];
                w = Font.GetTextWidth(line) + (BorderWidth * 2) + _scrollWidth;
                while (i < length)
                {
                    if (s[i - 1] == 32 || i == length - 1)
                    {
                        if (i == length - 1)
                        {
                            i += 1;
                        }

                        word = Copy(s, start + 1, i - start);
                        tw = Font.GetTextWidth(word);
                        if (w + tw > WinWidth)
                        {
                            NewLine();
                            string line2 = _lines[lineIndex];
                            _lines[lineIndex] = line2 + word;
                            w = (BorderWidth * 2) + _scrollWidth + tw;
                        }
                        else
                        {
                            string line2 = _lines[lineIndex];
                            _lines[lineIndex] = line2 + word;
                            w += tw;
                        }

                        start = i;
                    }

                    i += 1;
                }
            }
        }

        void NewChange()
        {
            ChangeInfo n;
            n = new ChangeInfo
            {
                Height = ci.Height,
                Color = ci.Color,
                Font = ci.Font,
                Align = ci.Align,
                Spacing = ci.Spacing,
                Position = ci.Position,
                Line = ci.Line
            };
            Changes.Add(n);
            ci = n;
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
                case IntegerParseMode.None:
                case IntegerParseMode.Start:
                case IntegerParseMode.Font:
                case IntegerParseMode.ItemHeight:
                case IntegerParseMode.Color:
                case IntegerParseMode.Background:
                case IntegerParseMode.BackgroundIndex:
                case IntegerParseMode.Space:
                case IntegerParseMode.Align:
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

        h = _textHeight;
        _lines.Clear();
        state = IntegerParseMode.None;
        if (_items.Count > 0 && UpperCase(Copy(_items[0], 1, 5)) == "<INT>")
        {
            _ = _lines.Add("");
            h = BorderHeight * 2;
            _images.Clear();
            for (int i = 0, last = Changes.Count - 1; i <= last; i++)
            {
            }

            Changes.Clear();
            ci = new ChangeInfo
            {
                Height = 10,
                Color = 16777215,
                Font = "",
                Align = TextAlignment.Left,
                Position = 0,
                Line = 0,
                Spacing = 0
            };
            Changes.Add(ci);
            lineIndex = 0;
            for (int i = 0, last = _items.Count - 1; i <= last; i++)
            {
                string item = NullTerminatedText(_items[i]);
                text = Cp1252.Encode(item);
                last2 = text.Length - 1;
                textStart = 0;
                textEnd = -1;
                tagStart = 0;
                for (int j = 0; j <= last2; j++)
                {
                    c = text[j];
                    switch (state)
                    {
                        case IntegerParseMode.None:
                            switch (c)
                            {
                                case 60:
                                    state = IntegerParseMode.Start;
                                    textEnd = j - 1;
                                    tagStart = j + 1;
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case IntegerParseMode.Start:
                        case IntegerParseMode.InBackground:
                            switch (c)
                            {
                                case 62:
                                    ParseTag(Copy(item, tagStart + 1, j - tagStart));
                                    state = IntegerParseMode.None;
                                    textStart = j + 1;
                                    break;
                                case 32:
                                case 61:
                                    ParseTag(Copy(item, tagStart + 1, j - tagStart));
                                    tagStart = j + 1;
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case IntegerParseMode.Font:
                            if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(c))
                            {
                                string line = _lines[lineIndex];
                                ci.Position = line.Length;
                                ci.Line = lineIndex;
                                ci.Font = Copy(item, tagStart + 1, j - tagStart);
                                if (!IsEmpty(ci.Font))
                                {
                                    if (!LoadFont(ci.Font))
                                    {
                                        ci.Font = "";
                                    }
                                }

                                NewChange();
                                if (c == 62)
                                {
                                    state = IntegerParseMode.None;
                                    textStart = j + 1;
                                }
                                else
                                {
                                    state = IntegerParseMode.Start;
                                    tagStart = j + 1;
                                }
                            }

                            break;
                        case IntegerParseMode.ItemHeight:
                            if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(c))
                            {
                                string line = _lines[lineIndex];
                                ci.Position = line.Length;
                                ci.Line = lineIndex;
                                try
                                {
                                    ci.Height = StrToInt(Copy(item, tagStart + 1, j - tagStart));
                                }
                                catch
                                {
                                }

                                NewChange();
                                if (c == 62)
                                {
                                    state = IntegerParseMode.None;
                                    textStart = j + 1;
                                }
                                else
                                {
                                    state = IntegerParseMode.Start;
                                    tagStart = j + 1;
                                }
                            }

                            break;
                        case IntegerParseMode.Color:
                            if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(c))
                            {
                                string line = _lines[lineIndex];
                                ci.Position = line.Length;
                                ci.Line = lineIndex;
                                if (j - tagStart == 6)
                                {
                                    ci.Color = 0;
                                    for (int k = 0; k <= 5; k++)
                                    {
                                        v = text[tagStart + k];
                                        v = v < 58 ? v - 48 : v - 55;

                                        ci.Color = (ci.Color << 4) + v;
                                    }

                                    NewChange();
                                }

                                if (c == 62)
                                {
                                    state = IntegerParseMode.None;
                                    textStart = j + 1;
                                }
                                else
                                {
                                    state = IntegerParseMode.Start;
                                    tagStart = j + 1;
                                }
                            }

                            break;
                        case IntegerParseMode.Space:
                            if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(c))
                            {
                                string line = _lines[lineIndex];
                                ci.Position = line.Length;
                                ci.Line = lineIndex;
                                try
                                {
                                    ci.Spacing = StrToInt(Copy(item, tagStart + 1, j - tagStart));
                                    Font?.CharacterDistance = ci.Spacing;
                                }
                                catch
                                {
                                }

                                NewChange();
                                if (c == 62)
                                {
                                    state = IntegerParseMode.None;
                                    textStart = j + 1;
                                }
                                else
                                {
                                    state = IntegerParseMode.Start;
                                    tagStart = j + 1;
                                }
                            }

                            break;
                        case IntegerParseMode.Align:
                            if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(c))
                            {
                                s = UpperCase(Copy(item, tagStart + 1, j - tagStart));
                                string line = _lines[lineIndex];
                                ci.Position = line.Length;
                                ci.Line = lineIndex;
                                ci.Align = s == "RIGHT" ? TextAlignment.Right : s == "CENTER" ? TextAlignment.Center : TextAlignment.Left;

                                NewChange();
                                if (c == 62)
                                {
                                    state = IntegerParseMode.None;
                                    textStart = j + 1;
                                }
                                else
                                {
                                    state = IntegerParseMode.Start;
                                    tagStart = j + 1;
                                }
                            }

                            break;
                        case IntegerParseMode.Background:
                            if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(c))
                            {
                                if (FileExists(ExtractFilePath(RtlUnit.Application.ExeName) + Copy(item, tagStart + 1, j - tagStart)))
                                {
                                    string filePath = ExtractFilePath(RtlUnit.Application.ExeName);
                                    _images.LoadFromFile(filePath + Copy(item, tagStart + 1, j - tagStart));
                                }

                                ImageLibraryIndexTexture = 0;
                                if (c == 62)
                                {
                                    state = IntegerParseMode.None;
                                    textStart = j + 1;
                                }
                                else
                                {
                                    state = IntegerParseMode.InBackground;
                                    tagStart = j + 1;
                                }
                            }

                            break;
                        case IntegerParseMode.BackgroundIndex:
                            if (Set<byte>.WithBounds(0, 255).With(32).With(62).Contains(c))
                            {
                                try
                                {
                                    ImageLibraryIndexTexture = StrToInt(Copy(item, tagStart + 1, j - tagStart));
                                }
                                catch
                                {
                                }

                                if (c == 62)
                                {
                                    state = IntegerParseMode.None;
                                    textStart = j + 1;
                                }
                                else
                                {
                                    state = IntegerParseMode.InBackground;
                                    tagStart = j + 1;
                                }
                            }

                            break;
                        default:
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
                        if (textStart <= last2)
                        {
                            AddText(Copy(item, textStart + 1, last2 - textStart + 1));
                        }

                        break;
                    case IntegerParseMode.Start:
                        if (tagStart <= last2)
                        {
                            ParseTag(Copy(item, tagStart + 1, last2 - tagStart));
                        }

                        break;
                    case IntegerParseMode.Font:
                    case IntegerParseMode.ItemHeight:
                    case IntegerParseMode.Color:
                    case IntegerParseMode.Background:
                    case IntegerParseMode.InBackground:
                    case IntegerParseMode.BackgroundIndex:
                    case IntegerParseMode.Space:
                    case IntegerParseMode.Align:
                    default:
                        break;
                }
            }

            h += ci.Height;
        }

        if (WinWidth - _scrollWidth != _textWidth || _textHeight != h)
        {
            _textWidth = WinWidth - _scrollWidth;
            _textHeight = h;
            if (_textHeight < WinHeight)
            {
                _textHeight = WinHeight;
            }

            _ = (BackSurface?.Resize(_textWidth, _textHeight));
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
        Rect r;
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
            r = OwnerComponent.OwnerComponent.OwnerComponent == null
                ? new Rect(WinLeft, WinTop, WinLeft + WinWidth - 1, WinTop + WinHeight - 1)
                : new Rect(OwnerComponent.WindowRelativeLeft + WinLeft, OwnerComponent.WindowRelativeTop + WinTop, OwnerComponent.WindowRelativeLeft + WinLeft + WinWidth - 1, OwnerComponent.WindowRelativeTop + WinTop + WinHeight - 1);

            OwnerComponent.AddDrawRectangle(ref r, this);
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
        // The owner is a UIComponent. This memo sits in a window, and the cast stays.
        _window = (BaseWindow)OwnerComponent;
    }

    // AoWHTMLMemo.pas:584
    // { Draw renders the text into the back surface when it changed, then blits the dirty rectangles
    //   of the window (or the whole visible part) from it }
    public override void Draw(LockedSurface lockedSurface)
    {
        int oldLeft;
        int oldTop;
        LockedSurface backSurfaceDescription = default;
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
                    if (_images.Count <= ImageLibraryIndexTexture)
                    {
                        _ = Masked ? BackSurface.Clear(MaskColor) : BackSurface.Clear(0);

                        _ = BackSurface.Lock(ref backSurfaceDescription);
                    }
                    else
                    {
                        _ = BackSurface.Lock(ref backSurfaceDescription);
                        SizedRectangle rectangle = new(0, 0, _textWidth, _textHeight);
                        _images.DrawTiled(ImageLibraryIndexTexture, rectangle, ref backSurfaceDescription);
                    }

                    int line10 = 1;
                    ChangeInfo ci = Changes[0];
                    TextAlignment align = TextAlignment.Left;
                    string fontName = "";
                    int x = BorderWidth;
                    int y = BorderHeight;
                    int lh = 10;
                    for (int i = 0, last = _lines.Count - 1; i <= last; i++)
                    {
                        int position = 0;
                        switch (align)
                        {
                            case TextAlignment.Left:
                                x = BorderWidth;
                                break;
                            case TextAlignment.Center:
                                x = 0;
                                break;
                            case TextAlignment.Right:
                                x = _textWidth - BorderWidth;
                                break;
                            default:
                                break;
                        }

                        while (ci.Line == i && line10 <= Changes.Count)
                        {
                            if (!IsEmpty(fontName) && ci.Position > position)
                            {
                                switch (align)
                                {
                                    case TextAlignment.Left:
                                        string line = _lines[i];
                                        Font.SetText(x, y, Copy(line, position + 1, ci.Position - position), backSurfaceDescription);
                                        string line2 = _lines[i];
                                        x += Font.GetTextWidth(Copy(line2, position + 1, ci.Position - position));
                                        break;
                                    case TextAlignment.Center:
                                        string line3 = _lines[i];
                                        int textWidth = Font.GetTextWidth(Copy(line3, position + 1, ci.Position - position));
                                        string line4 = _lines[i];
                                        Font.SetText((_textWidth / 2) - (textWidth / 2), y, Copy(line4, position + 1, ci.Position - position), backSurfaceDescription);
                                        break;
                                    case TextAlignment.Right:
                                        string line5 = _lines[i];
                                        int fontTextWidth = Font.GetTextWidth(Copy(line5, position + 1, ci.Position - position));
                                        string line6 = _lines[i];
                                        Font.SetText(x - fontTextWidth, y, Copy(line6, position + 1, ci.Position - position), backSurfaceDescription);
                                        string line7 = _lines[i];
                                        x -= Font.GetTextWidth(Copy(line7, position + 1, ci.Position - position));
                                        break;
                                    default:
                                        break;
                                }
                            }

                            position = ci.Position;
                            if (!IsEmpty(ci.Font))
                            {
                                _ = LoadFont(ci.Font);
                            }

                            Font.FontColorPaletteIndex = 1;
                            Font.Color = ci.Color;
                            Font.CharacterDistance = ci.Spacing;
                            fontName = ci.Font;
                            lh = ci.Height;
                            if (ci.Align != align)
                            {
                                switch (align)
                                {
                                    case TextAlignment.Left:
                                        x = BorderWidth;
                                        break;
                                    case TextAlignment.Center:
                                        x = 0;
                                        break;
                                    case TextAlignment.Right:
                                        x = _textWidth - BorderWidth;
                                        break;
                                    default:
                                        break;
                                }
                            }

                            align = ci.Align;
                            if (line10 < Changes.Count)
                            {
                                ci = Changes[line10];
                            }

                            line10 += 1;
                        }

                        if (!IsEmpty(fontName))
                        {
                            switch (align)
                            {
                                case TextAlignment.Left:
                                    string lineLength = _lines[i];
                                    string line = _lines[i];
                                    Font.SetText(x, y, Copy(line, position + 1, lineLength.Length - position), backSurfaceDescription);
                                    break;
                                case TextAlignment.Center:
                                    string line2 = _lines[i];
                                    string line3 = _lines[i];
                                    int textWidth = Font.GetTextWidth(Copy(line3, position + 1, line2.Length - position));
                                    string line4 = _lines[i];
                                    string line5 = _lines[i];
                                    Font.SetText((_textWidth / 2) - (textWidth / 2), y, Copy(line5, position + 1, line4.Length - position), backSurfaceDescription);
                                    break;
                                case TextAlignment.Right:
                                    string line6 = _lines[i];
                                    string line7 = _lines[i];
                                    int fontTextWidth = Font.GetTextWidth(Copy(line7, position + 1, line6.Length - position));
                                    string line8 = _lines[i];
                                    string line9 = _lines[i];
                                    Font.SetText(x - fontTextWidth, y, Copy(line9, position + 1, line8.Length - position), backSurfaceDescription);
                                    break;
                                default:
                                    break;
                            }
                        }

                        y += lh;
                    }

                    _ = BackSurface.Unlock();
                    _backDrawn = true;
                }

                SetWinLeftWithoutInvalidate(oldLeft);
                SetWinTopWithoutInvalidate(oldTop);
                RaisePreDraw(this, lockedSurface);

                _ = BackSurface.Lock(ref backSurfaceDescription);
                Rect sourceR;
                if (SurfaceValid)
                {
                    for (int i = 0, last = DrawCount - 1; i <= last; i++)
                    {
                        DirtyRectangle r = DrawList[i];
                        if (OwnerComponent.AbsoluteLeft + WinLeft > r.Rectangle.Left)
                        {
                            Rect rectangle2 = r.Rectangle;
                            rectangle2.Left = OwnerComponent.AbsoluteLeft + WinLeft;
                            r.Rectangle = rectangle2;
                        }

                        if (OwnerComponent.AbsoluteTop + WinTop > r.Rectangle.Top)
                        {
                            Rect rectangle3 = r.Rectangle;
                            rectangle3.Top = OwnerComponent.AbsoluteTop + WinTop;
                            r.Rectangle = rectangle3;
                        }

                        if (OwnerComponent.AbsoluteLeft + WinLeft + WinWidth < r.Rectangle.Right)
                        {
                            Rect rectangle4 = r.Rectangle;
                            rectangle4.Right = OwnerComponent.AbsoluteLeft + WinLeft + WinWidth;
                            r.Rectangle = rectangle4;
                        }

                        if (OwnerComponent.AbsoluteTop + WinTop + WinHeight < r.Rectangle.Bottom)
                        {
                            Rect rectangle5 = r.Rectangle;
                            rectangle5.Bottom = OwnerComponent.AbsoluteTop + WinTop + WinHeight;
                            r.Rectangle = rectangle5;
                        }

                        sourceR = new Rect(r.Rectangle.Left - OwnerComponent.AbsoluteLeft - WinLeft, _topItem + r.Rectangle.Top - OwnerComponent.AbsoluteTop - WinTop, r.Rectangle.Right - OwnerComponent.AbsoluteLeft - WinLeft, _topItem + r.Rectangle.Bottom - OwnerComponent.AbsoluteTop - WinTop);
                        _ = Masked
                            ? LockedSurface.BlitSpriteClipped16(backSurfaceDescription, sourceR, lockedSurface, r.Rectangle.Left, r.Rectangle.Top, MaskColor)
                            : LockedSurface.BlitSurfaceDescriptionRectangleClipped(backSurfaceDescription, sourceR, lockedSurface, r.Rectangle.Left, r.Rectangle.Top);
                    }
                }
                else
                {
                    sourceR = new Rect(0, _topItem, WinWidth, _topItem + WinHeight);
                    _ = Masked
                        ? LockedSurface.BlitSpriteClipped16(backSurfaceDescription, sourceR, lockedSurface, _window.AbsoluteLeft + WinLeft, _window.AbsoluteTop + WinTop, MaskColor)
                        : LockedSurface.BlitSurfaceDescriptionRectangleClipped(backSurfaceDescription, sourceR, lockedSurface, _window.AbsoluteLeft + WinLeft, _window.AbsoluteTop + WinTop);
                }

                _ = BackSurface.Unlock();
                RaiseDraw(this, lockedSurface);

                SurfaceValid = true;
            }
        }
        catch
        {
            _ = MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AoWHTMLMemo.pas:134
    public void SetWindow(BaseWindow value) => SetOwner(value);

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
            _verticalScrollBar?.Priority = Priority;

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
        int i;
        fileName = ExtractFilePath(RtlUnit.Application.ExeName) + fileName;
        font = null;
        i = 0;
        while (i < _fonts.Count && font == null)
        {
            if (_fonts[i].FileName == fileName)
            {
                font = _fonts[i];
            }

            i += 1;
        }

        if (font == null && FileExists(fileName))
        {
            font = new ImageLibraryFont(this)
            {
                FontColorPaletteIndex = 1
            };
            font.LoadFromFile(fileName);
            _fonts.Add(font);
        }

        if (font != null)
        {
            Font = font;
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
