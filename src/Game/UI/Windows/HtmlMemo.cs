#nullable disable
// Translated from AoWHTMLMemo.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.PCharStrings;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Windows;

// Fields: F1EC (a list that is created and freed only), FChanges (TChangeInfo), FLines (the
// wrapped lines), FItems (the text), FVScrollBar, FFilename, FScrollWidth, FBackDrawn (the back
// surface holds the text), FImages (the background textures), FTopItem (the first pixel row
// shown), FFonts (the fonts loaded, TImageLibraryFont), FAOWWindow, FTextWidth, FTextHeight
// (the size of the back surface)
public sealed partial class HtmlMemo : BaseWindow
{
    public List<ChangeInfo> Changes { get; }

    private readonly DelphiStringList _lines;
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
        _lines = new DelphiStringList();
        Items = new DelphiStringList
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
        Items.Dispose();
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
        void NewLine()
        {
            _ = _lines.Add("");
            h += ci.Height;
            lineIndex += 1;
        }

        void AddText(string s)
        {
            if (Font is not null)
            {
                int i = 0;
                int start = 0;
                int length = s.Length;
                string line = _lines[lineIndex];
                int w = Font.GetTextWidth(line) + (BorderWidth * 2) + _scrollWidth;
                while (i < length)
                {
                    if (s[i - 1] == 32 || i == length - 1)
                    {
                        if (i == length - 1)
                        {
                            i += 1;
                        }

                        string word = Copy(s, index: start + 1, count: i - start);
                        int tw = Font.GetTextWidth(word);
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
            ChangeInfo n = new()
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

        IntegerParseMode state;
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
        if (_verticalScrollBar is not null)
        {
            _scrollWidth = _verticalScrollBar.WinWidth;
            _verticalScrollBar.Position = _topItem;
        }

        h = _textHeight;
        _lines.Clear();
        state = IntegerParseMode.None;
        if (Items.Count > 0 && UpperCase(Copy(Items[0], index: 1, count: 5)) == "<INT>")
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
                Color = 0xFF_FF_FF,
                Font = "",
                Align = TextAlignment.Left,
                Position = 0,
                Line = 0,
                Spacing = 0
            };
            Changes.Add(ci);
            lineIndex = 0;
            for (int i = 0, last = Items.Count - 1; i <= last; i++)
            {
                string item = NullTerminatedText(Items[i]);
                ReadOnlySpan<byte> text = Cp1252.Encode(item);
                int last2 = text.Length - 1;
                int textStart = 0;
                int textEnd = -1;
                int tagStart = 0;
                for (int j = 0; j <= last2; j++)
                {
                    byte c = text[j];
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
                                    ParseTag(Copy(item, index: tagStart + 1, count: j - tagStart));
                                    state = IntegerParseMode.None;
                                    textStart = j + 1;
                                    break;
                                case 32:
                                case 61:
                                    ParseTag(Copy(item, index: tagStart + 1, count: j - tagStart));
                                    tagStart = j + 1;
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case IntegerParseMode.Font:
                            if (c is 32 or 62)
                            {
                                string line = _lines[lineIndex];
                                ci.Position = line.Length;
                                ci.Line = lineIndex;
                                ci.Font = Copy(item, index: tagStart + 1, count: j - tagStart);
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
                            if (c is 32 or 62)
                            {
                                string line = _lines[lineIndex];
                                ci.Position = line.Length;
                                ci.Line = lineIndex;
                                try
                                {
                                    ci.Height = StrToInt(Copy(item, index: tagStart + 1, count: j - tagStart));
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
                            if (c is 32 or 62)
                            {
                                string line = _lines[lineIndex];
                                ci.Position = line.Length;
                                ci.Line = lineIndex;
                                if (j - tagStart == 6)
                                {
                                    ci.Color = 0;
                                    for (int k = 0; k <= 5; k++)
                                    {
                                        int v = text[tagStart + k];
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
                            if (c is 32 or 62)
                            {
                                string line = _lines[lineIndex];
                                ci.Position = line.Length;
                                ci.Line = lineIndex;
                                try
                                {
                                    ci.Spacing = StrToInt(Copy(item, index: tagStart + 1, count: j - tagStart));
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
                            if (c is 32 or 62)
                            {
                                string s = UpperCase(Copy(item, index: tagStart + 1, count: j - tagStart));
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
                            if (c is 32 or 62)
                            {
                                if (Files.FileExists(ExtractFilePath(LegacyApplication.Application.ExeName) + Copy(item, index: tagStart + 1, count: j - tagStart)))
                                {
                                    string filePath = ExtractFilePath(LegacyApplication.Application.ExeName);
                                    _images.LoadFromFile(filePath + Copy(item, index: tagStart + 1, count: j - tagStart));
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
                            if (c is 32 or 62)
                            {
                                try
                                {
                                    ImageLibraryIndexTexture = StrToInt(Copy(item, index: tagStart + 1, count: j - tagStart));
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
                        AddText(Copy(item, index: textStart + 1, count: textEnd - textStart + 1));
                        textStart = textEnd;
                    }
                }

                switch (state)
                {
                    case IntegerParseMode.None:
                        if (textStart <= last2)
                        {
                            AddText(Copy(item, index: textStart + 1, count: last2 - textStart + 1));
                        }

                        break;
                    case IntegerParseMode.Start:
                        if (tagStart <= last2)
                        {
                            ParseTag(Copy(item, index: tagStart + 1, count: last2 - tagStart));
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
        if (_verticalScrollBar is not null)
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
        if (Visible)
        {
            if (_verticalScrollBar is not null && _lines.Count > 0)
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
            Rect r = OwnerComponent.OwnerComponent.OwnerComponent is null
                ? new Rect(WinLeft, WinTop, WinLeft + WinWidth - 1, WinTop + WinHeight - 1)
                : new Rect(OwnerComponent.WindowRelativeLeft + WinLeft, OwnerComponent.WindowRelativeTop + WinTop, OwnerComponent.WindowRelativeLeft + WinLeft + WinWidth - 1, OwnerComponent.WindowRelativeTop + WinTop + WinHeight - 1);

            OwnerComponent.AddDrawRectangle(ref r, sender: this);
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
        try
        {
            if (Valid && Visible)
            {
                int oldLeft = WinLeft;
                int oldTop = WinTop;
                SetWinLeftWithoutInvalidate(0);
                SetWinTopWithoutInvalidate(0);
                if (BackSurface is not null)
                {
                    if (BackSurface.PixelFormatId != lockedSurface.PixelFormatId)
                    {
                        BackSurface?.Dispose();
                        BackSurface = null;
                    }
                }

                if (BackSurface is null)
                {
                    BackSurface = new DataSurface(_textWidth, _textHeight, lockedSurface.PixelFormatId, palette: null);
                    _backDrawn = false;
                }

                LockedSurface backSurfaceDescription = default;
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
                        _images.DrawTiled(ImageLibraryIndexTexture, area: rectangle, lockedSurface: ref backSurfaceDescription);
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
                                        Font.SetText(x, y, text: Copy(line, index: position + 1, count: ci.Position - position), lockedSurface: backSurfaceDescription);
                                        string line2 = _lines[i];
                                        x += Font.GetTextWidth(Copy(line2, index: position + 1, count: ci.Position - position));
                                        break;
                                    case TextAlignment.Center:
                                        string line3 = _lines[i];
                                        int textWidth = Font.GetTextWidth(Copy(line3, index: position + 1, count: ci.Position - position));
                                        string line4 = _lines[i];
                                        Font.SetText((_textWidth / 2) - (textWidth / 2), y, text: Copy(line4, index: position + 1, count: ci.Position - position), lockedSurface: backSurfaceDescription);
                                        break;
                                    case TextAlignment.Right:
                                        string line5 = _lines[i];
                                        int fontTextWidth = Font.GetTextWidth(Copy(line5, index: position + 1, count: ci.Position - position));
                                        string line6 = _lines[i];
                                        Font.SetText(x - fontTextWidth, y, text: Copy(line6, index: position + 1, count: ci.Position - position), lockedSurface: backSurfaceDescription);
                                        string line7 = _lines[i];
                                        x -= Font.GetTextWidth(Copy(line7, index: position + 1, count: ci.Position - position));
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
                                    Font.SetText(x, y, text: Copy(line, index: position + 1, count: lineLength.Length - position), lockedSurface: backSurfaceDescription);
                                    break;
                                case TextAlignment.Center:
                                    string line2 = _lines[i];
                                    string line3 = _lines[i];
                                    int textWidth = Font.GetTextWidth(Copy(line3, index: position + 1, count: line2.Length - position));
                                    string line4 = _lines[i];
                                    string line5 = _lines[i];
                                    Font.SetText((_textWidth / 2) - (textWidth / 2), y, text: Copy(line5, index: position + 1, count: line4.Length - position), lockedSurface: backSurfaceDescription);
                                    break;
                                case TextAlignment.Right:
                                    string line6 = _lines[i];
                                    string line7 = _lines[i];
                                    int fontTextWidth = Font.GetTextWidth(Copy(line7, index: position + 1, count: line6.Length - position));
                                    string line8 = _lines[i];
                                    string line9 = _lines[i];
                                    Font.SetText(x - fontTextWidth, y, text: Copy(line9, index: position + 1, count: line8.Length - position), lockedSurface: backSurfaceDescription);
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
                RaisePreDraw(this, surfDesc: lockedSurface);

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
                            ? LockedSurface.BlitSpriteClipped16(backSurfaceDescription, sourceRectangle: sourceR, destination: lockedSurface, r.Rectangle.Left, r.Rectangle.Top, colorKey: MaskColor)
                            : LockedSurface.BlitSurfaceDescriptionRectangleClipped(backSurfaceDescription, sourceRectangle: sourceR, destination: lockedSurface, r.Rectangle.Left, r.Rectangle.Top);
                    }
                }
                else
                {
                    sourceR = new Rect(0, _topItem, WinWidth, _topItem + WinHeight);
                    _ = Masked
                        ? LockedSurface.BlitSpriteClipped16(backSurfaceDescription, sourceRectangle: sourceR, destination: lockedSurface, _window.AbsoluteLeft + WinLeft, _window.AbsoluteTop + WinTop, colorKey: MaskColor)
                        : LockedSurface.BlitSurfaceDescriptionRectangleClipped(backSurfaceDescription, sourceRectangle: sourceR, destination: lockedSurface, _window.AbsoluteLeft + WinLeft, _window.AbsoluteTop + WinTop);
                }

                _ = BackSurface.Unlock();
                RaiseDraw(this, surfDesc: lockedSurface);

                SurfaceValid = true;
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // AoWHTMLMemo.pas:134
    public void SetWindow(BaseWindow value) => SetOwner(value);

    // AoWHTMLMemo.pas:139
    public void SetFStrings(DelphiStringList value)
    {
        Items.Clear();
        Items.AddStrings(value);
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
        if (value != _fileName && Files.FileExists(value))
        {
            _fileName = value;
            Items.LoadFromFile(_fileName);
        }
    }

    // AoWHTMLMemo.pas:182
    // { LoadFont makes the font in FileName (relative to the program) the current font, loading it
    //   the first time }
    public bool LoadFont(string fileName)
    {
        fileName = ExtractFilePath(LegacyApplication.Application.ExeName) + fileName;
        ImageLibraryFont font = null;
        int i = 0;
        while (i < _fonts.Count && font is null)
        {
            if (_fonts[i].FileName == fileName)
            {
                font = _fonts[i];
            }

            i += 1;
        }

        if (font is null && Files.FileExists(fileName))
        {
            font = new ImageLibraryFont(this)
            {
                FontColorPaletteIndex = 1
            };
            font.LoadFromFile(fileName);
            _fonts.Add(font);
        }

        if (font is not null)
        {
            Font = font;
        }

        return font is not null;
    }

    public DelphiStringList Items { get; }
    public int ListOffset { get => _topItem; set => SetListOff(value); }

    [Published]
    public BaseWindow Window { get => _window; set => SetWindow(value); }

    [Published]
    public VerticalScrollBar VerticalScrollBar { get => _verticalScrollBar; set => SetVScroll(value); }

    [Published]
    public string FileName { get => _fileName; set => SetFileName(value); }

    public new event NotifyEvent<HtmlMemo> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<HtmlMemo> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<HtmlMemo> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<HtmlMemo> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<HtmlMemo> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<HtmlMemo> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<HtmlMemo> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<HtmlMemo> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<HtmlMemo> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, HtmlMemo>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, HtmlMemo>(this, slot: nameof(Updated), value: value);
    }

    public new event AoWOnDrawBackEvent<HtmlMemo> DrawBack
    {
        add => ((WindowControl)this).DrawBack += EventSlot.Add<WindowControl, HtmlMemo>(this, slot: nameof(DrawBack), value: value);
        remove => ((WindowControl)this).DrawBack -= EventSlot.Remove<WindowControl, HtmlMemo>(this, slot: nameof(DrawBack), value: value);
    }

    public new event NotifyEvent<HtmlMemo> Enter
    {
        add => ((WindowControl)this).Enter += EventSlot.Add<WindowControl, HtmlMemo>(this, slot: nameof(Enter), value: value);
        remove => ((WindowControl)this).Enter -= EventSlot.Remove<WindowControl, HtmlMemo>(this, slot: nameof(Enter), value: value);
    }

    public new event NotifyEvent<HtmlMemo> Exit
    {
        add => ((WindowControl)this).Exit += EventSlot.Add<WindowControl, HtmlMemo>(this, slot: nameof(Exit), value: value);
        remove => ((WindowControl)this).Exit -= EventSlot.Remove<WindowControl, HtmlMemo>(this, slot: nameof(Exit), value: value);
    }

    public new event NotifyEvent<HtmlMemo> Hide
    {
        add => ((BaseWindow)this).Hide += EventSlot.Add<BaseWindow, HtmlMemo>(this, slot: nameof(Hide), value: value);
        remove => ((BaseWindow)this).Hide -= EventSlot.Remove<BaseWindow, HtmlMemo>(this, slot: nameof(Hide), value: value);
    }

    public new event NotifyEvent<HtmlMemo> Show
    {
        add => ((BaseWindow)this).Show += EventSlot.Add<BaseWindow, HtmlMemo>(this, slot: nameof(Show), value: value);
        remove => ((BaseWindow)this).Show -= EventSlot.Remove<BaseWindow, HtmlMemo>(this, slot: nameof(Show), value: value);
    }

    // a style change in the text. FHeight (the line height), FColor, FFont (the font file, '' for
    // the current font), FAlign, FPos (the character of the line where it starts), FLine, FSpacing
    public sealed class ChangeInfo
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
