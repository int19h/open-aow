#nullable disable
// Translated from AoWMemo.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// Fields: F114 (cleared by CheckMouseUp, not read), FItems (the text, one paragraph a line),
// FColorList (TColorChange of every wrapped line), FVScrollBar, FNumVisible (lines that fit),
// FTopItem (the first line shown), FScrollSet, FOnDrawItem, FScrollWidth (the width taken at
// the right by the frame or the scroll bar), FScrollVisible, FCursorChar, FBlink (the cursor is
// shown), FBlinkDelay, FBlinkCount, FCurCol, FCurLine (the cursor in FLines), FRealCol,
// FRealLine (the cursor in FItems), FEditable, FILFrame, FILTexture, FItemHeight,
// FColorChangeChar, FTextAlign, FNoFrame (Validate found no frame or texture: drawn
// transparent), FILIndexFrame, FILIndexTexture, FDrawIndex, FItemRect (the line being drawn,
// for OnDrawItem), FLines (the wrapped lines). FRedraw is the focus: the memo takes the keys
// while it is set
public partial class Memo : Control
{

    private readonly StringList _items;

    public List<ColorChange> ColorList { get; }

    private VerticalScrollBar _verticalScrollBar;
    private int _numberVisible;
    private int _topItem;
    private bool _scrollSet;
    [Published]
    public event NotifyEvent<Memo> OnDrawItem;
    private int _scrollWidth;

    private byte _cursorChar;
    private bool _blink;
    private byte _blinkDelay;
    private byte _blinkCount;
    private int _currentColumn;
    private int _currentLine;
    private int _realColumn;
    private int _realLine;
    private bool _editable;
    private InterfaceImageLibrary _imageLibraryFrame;
    private InterfaceImageLibrary _imageLibraryTexture;
    private int _itemHeight;
    private byte _colorChangeChar;
    private TextAlignment _textAlign;

    public bool NoFrame { get; set; }

    private int _imageLibraryIndexFrame;
    private int _imageLibraryIndexTexture;

    private Rect _itemRectangle;

    public StringList Lines { get; }

    // AoWMemo.pas:126
    public Memo(Component owner) : base(owner)
    {
        Lines = new StringList();
        _items = new StringList();
        _items.OnChange = OnChangeText;
        ColorList = [];
        _numberVisible = 3;
        _cursorChar = 124;
        _editable = true;
    }

    // AoWMemo.pas:138
    public override void Dispose()
    {
        Lines?.Dispose();
        _items?.Dispose();
        for (int i = 0, last = ColorList.Count - 1; i <= last; i++)
        {
        }

        ColorList?.Clear();
        base.Dispose();
    }

    // AoWMemo.pas:150
    public override void Validate()
    {
        if (base.Font != null)
        {
            if (_imageLibraryFrame != null)
            {
                _imageLibraryFrame = _imageLibraryFrame.CheckImageLibrary(_imageLibraryIndexFrame, 8, this);
            }

            if (_imageLibraryTexture != null)
            {
                _imageLibraryTexture = _imageLibraryTexture.CheckImageLibrary(_imageLibraryIndexTexture, 1, this);
            }

            NoFrame = _imageLibraryFrame == null || _imageLibraryTexture == null;

            Valid = true;
        }

        base.Validate();
    }

    // AoWMemo.pas:169
    // { SetSize wraps every item at the width of the control into FLines (at spaces; a word that does
    //   not fit starts a new line) and sets up the scroll bar }
    public override void SetSize()
    {
        int length;
        int w;
        ReadOnlySpan<byte> text;
        string s;
        int j;
        int start;
        int tw;
        ColorChange cc;
        if (_itemHeight < 1)
        {
            _itemHeight = 10;
        }

        switch (NoFrame)
        {
            case false:
                LibraryImage imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
                LibraryImage imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > WinHeight)
                {
                    LibraryImage imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    LibraryImage imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                    SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
                }

                if (_imageLibraryFrame[_imageLibraryIndexFrame].Width > WinWidth)
                {
                    SetWinWidthWithoutInvalidate(_imageLibraryFrame[_imageLibraryIndexFrame].Width);
                }

                LibraryImage imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                LibraryImage imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                _numberVisible = (WinHeight - imageLibraryFrame4.Height - imageLibraryFrame5.Height) / _itemHeight;
                _scrollWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 4].Width;
                Masked = false;
                break;
            case true:
                _numberVisible = WinHeight / _itemHeight;
                _scrollWidth = 0;
                Masked = true;
                break;
        }

        if (_verticalScrollBar != null)
        {
            _scrollWidth = _verticalScrollBar.WinWidth;
            _topItem = _verticalScrollBar.Position;
        }

        for (int i = 0, last = ColorList.Count - 1; i <= last; i++)
        {
            _ = ColorList[i];
        }

        ColorList.Clear();
        Lines.Clear();
        for (int i = 0, last = _items.Count - 1; i <= last; i++)
        {
            _ = Lines.Add("");
            ColorChange colorChange = new();
            ColorList.Add(colorChange);
            j = 0;
            start = 0;
            w = !NoFrame ? _imageLibraryFrame[_imageLibraryIndexFrame + 3].Width + _scrollWidth + (BorderWidth * 2) : BorderWidth * 2;

            string item = NullTerminatedText(_items[i]);
            text = Cp1252.Encode(item);
            length = text.Length;
            if (length > 6 && text[0] == _colorChangeChar)
            {
                cc = ColorList[^1];
                cc.HasColor = true;
                try
                {
                    cc.Color = StrToInt("$" + Copy(_items[i], index: 2, count: 6));
                }
                catch
                {
                }

                j = 8;
                start = 7;
            }

            while (j < length)
            {
                if (text[j] == 32 || j == length - 1)
                {
                    s = Copy(item, start + 1, j + 1 - start);
                    tw = base.Font.GetTextWidth(s);
                    if (w + tw > WinWidth)
                    {
                        _ = Lines.Add(s);
                        ColorChange colorChange2 = new();
                        ColorList.Add(colorChange2);
                        w = !NoFrame
                            ? tw + _imageLibraryFrame[_imageLibraryIndexFrame + 3].Width + _scrollWidth + (BorderWidth * 2)
                            : tw + (BorderWidth * 2);
                    }
                    else
                    {
                        int count = Lines.Count;
                        string line = Lines[count - 1];
                        int fLinesCount = Lines.Count;
                        Lines[fLinesCount - 1] = line + s;
                        w += tw;
                    }

                    start = j + 1;
                }

                j += 1;
            }
        }

        if (Lines.Count < _topItem + _numberVisible)
        {
            _topItem = Lines.Count - _numberVisible;
        }

        if (_topItem < 0)
        {
            _topItem = 0;
        }

        _scrollSet = false;
        if (!_scrollSet)
        {
            _scrollWidth = !NoFrame ? _imageLibraryFrame[_imageLibraryIndexFrame + 4].Width : 0;
            if (_verticalScrollBar != null)
            {
                _scrollSet = true;
                if (Window != _verticalScrollBar.Window)
                {
                    _verticalScrollBar.Window = Window;
                }

                _verticalScrollBar.Min = 0;
                int count = Lines.Count;
                _verticalScrollBar.Max = count - 2;
                _verticalScrollBar.Priority = Priority + 1;
                _verticalScrollBar.Position = _topItem;
                _verticalScrollBar.NumberVisible = _numberVisible - 1;
                _verticalScrollBar.WinHeight = WinHeight;
                _verticalScrollBar.LargeChange = _numberVisible - 1;
                _verticalScrollBar.Visible = Visible;
                if (_verticalScrollBar.VanishWhenFull && Lines.Count - 1 < _numberVisible)
                {
                    _verticalScrollBar.Visible = false;
                }
                else
                {
                    _scrollWidth = _verticalScrollBar.WinWidth;
                }

                _verticalScrollBar.Sized = false;
                _verticalScrollBar.WinLeft = WinLeft + WinWidth - _verticalScrollBar.WinWidth;
                _verticalScrollBar.WinTop = WinTop;
            }
        }

        base.SetSize();
    }

    // AoWMemo.pas:457
    // { Update follows the scroll bar and blinks the cursor }
    public override void Update()
    {
        int p;
        base.Update();
        if (Visible)
        {
            if (_verticalScrollBar != null && Lines.Count > 0)
            {
                if (_verticalScrollBar.Position != _topItem)
                {
                    p = _verticalScrollBar.Position;
                    _topItem = p;
                    if (Lines.Count < p + _numberVisible)
                    {
                        _topItem = Lines.Count - _numberVisible;
                    }

                    if (_topItem < 0)
                    {
                        _topItem = 0;
                    }

                    Drawn = false;
                    if (NoFrame)
                    {
                        Window?.Drawn = false;
                    }
                }
            }

            if (_editable && _blinkDelay > 0 && Redraw)
            {
                _blinkCount = (byte)(_blinkCount + 1);
                if (_blinkCount > _blinkDelay)
                {
                    Drawn = false;
                    if (NoFrame)
                    {
                        Window?.Drawn = false;
                    }

                    _blinkCount = 0;
                    _blink = !_blink;
                }
            }
        }
    }

    // AoWMemo.pas:333
    public override void SetVisible(bool value)
    {
        base.SetVisible(value);
        _verticalScrollBar?.Visible = Visible;
    }

    // AoWMemo.pas:378
    // { Draw draws the frame and the texture, the visible lines (clipped to the text area) and the
    //   cursor }
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(this, lockedSurface);

                base.Font.Color = Color;
                Rect oldClip = lockedSurface.ClipRectangle;
                int x;
                int y;
                int w;
                int h;
                switch (NoFrame)
                {
                    case false:
                        SizedRectangle rectangle = new(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle, ref lockedSurface);
                        LibraryImage imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        LibraryImage imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        LibraryImage imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        LibraryImage imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        LibraryImage imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        SizedRectangle rectangle2 = new(Window.WinLeft + WinLeft + imageLibraryFrame3.Width, Window.WinTop + WinTop + imageLibraryFrame2.Height, WinWidth - imageLibraryFrameWidth.Width - _scrollWidth, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle2, ref lockedSurface);
                        LibraryImage imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        x = Window.WinLeft + WinLeft + imageLibraryFrame4.Width;
                        LibraryImage imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        y = Window.WinTop + WinTop + imageLibraryFrame5.Height;
                        LibraryImage imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        LibraryImage imageLibraryFrame7 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        h = WinHeight - imageLibraryFrame6.Height - imageLibraryFrame7.Height;
                        LibraryImage imageLibraryFrame8 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        w = WinWidth - imageLibraryFrame8.Width - _scrollWidth;
                        break;
                    default:
                        x = Window.WinLeft + WinLeft;
                        y = Window.WinTop + WinTop;
                        h = WinHeight;
                        w = WinWidth - _scrollWidth;
                        break;
                }

                if (x + w < oldClip.Right)
                {
                    Rect clipRectangle = lockedSurface.ClipRectangle;
                    clipRectangle.Right = x + w;
                    lockedSurface.ClipRectangle = clipRectangle;
                }

                if (y + h < oldClip.Bottom)
                {
                    Rect clipRectangle2 = lockedSurface.ClipRectangle;
                    clipRectangle2.Bottom = y + h;
                    lockedSurface.ClipRectangle = clipRectangle2;
                }

                for (int i = 0, last = _numberVisible; i <= last; i++)
                {
                    int count = Lines.Count;
                    if (_topItem + i < count)
                    {
                        ColorChange cc = ColorList[_topItem + i];
                        if (cc.HasColor)
                        {
                            base.Font.Color = cc.Color;
                        }

                        switch (_textAlign)
                        {
                            case TextAlignment.Left:
                                string line = Lines[_topItem + i];
                                base.Font.SetText(BorderWidth + x, BorderHeight + y + (_itemHeight * i), line, lockedSurface);
                                break;
                            case TextAlignment.Right:
                                string line2 = Lines[_topItem + i];
                                int textWidth = base.Font.GetTextWidth(line2);
                                string line3 = Lines[_topItem + i];
                                base.Font.SetText(x + w - BorderWidth - textWidth, BorderHeight + y + (_itemHeight * i), line3, lockedSurface);
                                break;
                            default:
                                string line4 = Lines[_topItem + i];
                                int fontTextWidth = base.Font.GetTextWidth(line4);
                                string line5 = Lines[_topItem + i];
                                base.Font.SetText(x + (w / 2) - (fontTextWidth / 2), BorderHeight + y + (_itemHeight * i), line5, lockedSurface);
                                break;
                        }
                        _itemRectangle = new Rect(x, y + (_itemHeight * i), x + w, y + ((i + 1) * _itemHeight));
                        if (_itemRectangle.Bottom > lockedSurface.ClipRectangle.Bottom)
                        {
                            _itemRectangle.Bottom = lockedSurface.ClipRectangle.Bottom;
                        }

                        OnDrawItem?.Invoke(this);
                    }
                }

                lockedSurface.ClipRectangle = oldClip;
                bool condition = _currentLine - _topItem > -1 && _currentLine - _topItem < _numberVisible;
                if (condition)
                {
                    int count = Lines.Count;
                    condition = _currentLine - _topItem < count;
                }

                if (condition && Redraw && _editable && _blink)
                {
                    string line = Lines[_currentLine];
                    int textWidth = base.Font.GetTextWidth(Copy(line, 1, _currentColumn));
                    base.Font.SetText(BorderWidth + x + textWidth, BorderHeight + y + ((_currentLine - _topItem) * _itemHeight), Cp1252.Text(_cursorChar), lockedSurface);
                }

                RaiseDraw(this, lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AoWMemo.pas:497
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseDown(this, button, shift, X, Y);
        }
    }

    // AoWMemo.pas:610
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (Visible && X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth - _scrollWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            if (_editable)
            {
                Window.Cursor = -4;
            }

            RaiseMouseMove(this, shift, X, Y);
        }
    }

    // AoWMemo.pas:715
    // { CheckKeyPress edits the text: Backspace, Enter (splits the paragraph) and the printable
    //   characters }
    public override void CheckKeyPress(ref byte key)
    {
        bool changed;
        bool moved;
        Handled = Redraw;
        changed = false;
        moved = false;
        if (_editable && Redraw && Valid && Visible)
        {
            switch (key)
            {
                case 8:
                    if (_realColumn > 0)
                    {
                        string itemLength = _items[_realLine];
                        string item = _items[_realLine];
                        string item2 = _items[_realLine];
                        _items[_realLine] = Copy(item2, 1, _realColumn - 1) + Copy(item, _realColumn + 1, itemLength.Length - _realColumn + 1);
                        _realColumn -= 1;
                        changed = true;
                    }
                    else
                    {
                        if (_realLine > 0)
                        {
                            _realColumn = _items[_realLine - 1].Length;
                            string item = _items[_realLine - 1];
                            string item2 = _items[_realLine];
                            _items[_realLine - 1] = item + item2;
                            _items.Delete(_realLine);
                            _realLine -= 1;
                            changed = true;
                        }
                    }

                    break;
                case 13:
                    if (Lines[_currentLine].Length == _currentColumn)
                    {
                        _items.Insert(_realLine + 1, "");
                        string item = _items[_realLine];
                        _items[_realLine] = Copy(item, 1, _realColumn + 1);
                    }
                    else
                    {
                        string itemLength = _items[_realLine];
                        string item = _items[_realLine];
                        _items.Insert(_realLine + 1, Copy(item, _realColumn + 1, itemLength.Length));
                        string item2 = _items[_realLine];
                        _items[_realLine] = Copy(item2, 1, _realColumn);
                    }

                    _realColumn = 0;
                    _realLine += 1;
                    changed = true;
                    break;
                case >= 0 and <= 7:
                case >= 9 and <= 12:
                case >= 14 and <= 31:
                    break;
                default:
                    {
                        byte c = key;
                        if (Lines[_currentLine].Length == _currentColumn)
                        {
                            string itemLength = _items[_realLine];
                            string item = _items[_realLine];
                            string item2 = _items[_realLine];
                            _items[_realLine] = Copy(item2, 1, _realColumn + 1) + Cp1252.Text(c) + Copy(item, _realColumn + 2, itemLength.Length - _realColumn);
                        }
                        else
                        {
                            string itemLength = _items[_realLine];
                            string item = _items[_realLine];
                            string item2 = _items[_realLine];
                            _items[_realLine] = Copy(item2, 1, _realColumn) + Cp1252.Text(c) + Copy(item, _realColumn + 1, itemLength.Length - _realColumn);
                        }

                        _realColumn += 1;
                        changed = true;
                    }

                    break;
            }

            if (moved)
            {
                CurrentToReal();
            }

            if (changed)
            {
                RealToCurrent();
            }

            KeyStuff();
            // { tests OnKeyDown and calls OnKeyPress (original bug) }
            if (HasKeyDownHandlers)
            {
                RaiseKeyPressUnchecked(this, ref key);
            }

            RaiseChange(this);
        }
    }

    // AoWMemo.pas:627
    // { CheckKeyDown moves the cursor with the arrow keys and deletes with Delete }
    public override void CheckKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        bool changed;
        bool moved;
        Handled = Redraw;
        changed = false;
        moved = false;
        if (_editable && Redraw && Valid && Visible)
        {
            switch (key)
            {
                case 37:
                    if (_realColumn > 0)
                    {
                        _realColumn -= 1;
                    }
                    else
                    {
                        if (_realLine > 0)
                        {
                            _realLine -= 1;
                            _realColumn = _items[_realLine].Length;
                        }
                    }

                    if (_currentColumn > 0)
                    {
                        _currentColumn -= 1;
                    }
                    else
                    {
                        if (_currentLine > 0)
                        {
                            _currentLine -= 1;
                            _currentColumn = Lines[_currentLine].Length;
                        }
                    }

                    break;
                case 38:
                    if (_currentLine > 0)
                    {
                        _currentLine -= 1;
                        if (Lines[_currentLine].Length < _currentColumn)
                        {
                            _currentColumn = Lines[_currentLine].Length;
                        }

                        moved = true;
                    }

                    break;
                case 39:
                    if (Lines[_currentLine].Length > _currentColumn)
                    {
                        _currentColumn += 1;
                    }
                    else
                    {
                        if (Lines.Count - 1 > _currentLine)
                        {
                            _currentColumn = 0;
                            _currentLine += 1;
                        }
                    }

                    if (_items[_realLine].Length > _realColumn)
                    {
                        _realColumn += 1;
                    }
                    else
                    {
                        if (_items.Count - 1 > _realLine)
                        {
                            _realColumn = 0;
                            _realLine += 1;
                        }
                    }

                    break;
                case 40:
                    if (Lines.Count - 1 > _currentLine)
                    {
                        _currentLine += 1;
                        if (Lines[_currentLine].Length < _currentColumn)
                        {
                            _currentColumn = Lines[_currentLine].Length;
                        }

                        moved = true;
                    }

                    break;
                case 46:
                    if (_items[_realLine].Length > _realColumn)
                    {
                        string itemLength = _items[_realLine];
                        string item = _items[_realLine];
                        string item2 = _items[_realLine];
                        _items[_realLine] = Copy(item2, 1, _realColumn) + Copy(item, _realColumn + 2, itemLength.Length - _realColumn);
                        changed = true;
                    }
                    else
                    {
                        if (_items.Count - 1 > _realLine)
                        {
                            string item = _items[_realLine];
                            string item2 = _items[_realLine + 1];
                            _items[_realLine] = item + item2;
                            _items.Delete(_realLine + 1);
                            changed = true;
                        }
                    }

                    break;
            }

            if (changed)
            {
                RealToCurrent();
            }

            if (moved)
            {
                CurrentToReal();
            }

            KeyStuff();
            RaiseKeyDown(this, ref key, shift);

            RaiseChange(this);
        }
    }

    // AoWMemo.pas:340
    public virtual void SetImageLibraryFrame(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryFrame)
        {
            Valid = false;
            _imageLibraryFrame = value;
        }
    }

    // AoWMemo.pas:349
    public virtual void SetImageLibraryTexture(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryTexture)
        {
            Valid = false;
            _imageLibraryTexture = value;
        }
    }

    // AoWMemo.pas:296
    public void SetFStrings(StringList value)
    {
        Lines.Clear();
        _items.Clear();
        for (int i = 0, last = value.Count - 1; i <= last; i++)
        {
            string valueValue = value[i];
            _ = Lines.Add(valueValue);
            string valueValue2 = value[i];
            _ = _items.Add(valueValue2);
        }

        if (Lines.Count < _numberVisible)
        {
            _numberVisible = Lines.Count;
        }

        if (Lines.Count < _topItem + _numberVisible)
        {
            _topItem = Lines.Count - _numberVisible;
        }

        Sized = false;
    }

    // AoWMemo.pas:314
    public void SetListOff(int value)
    {
        if (value >= Lines.Count)
        {
            value = Lines.Count - 1;
        }

        if (value != _topItem)
        {
            _topItem = value;
            Drawn = false;
        }
    }

    // AoWMemo.pas:325
    public void SetVScroll(VerticalScrollBar value)
    {
        _verticalScrollBar = value;
        value?.Priority = Priority;

        Sized = false;
    }

    // AoWMemo.pas:358
    public void SetILFIndex(int value)
    {
        if (value != _imageLibraryIndexFrame)
        {
            _imageLibraryIndexFrame = value;
            Valid = false;
        }
    }

    // AoWMemo.pas:367
    public void SetImageLibraryTileIndex(int value)
    {
        if (value != _imageLibraryIndexTexture)
        {
            _imageLibraryIndexTexture = value;
            Valid = false;
        }
    }

    // AoWMemo.pas:789
    // { CurToReal finds the cursor in FItems from the cursor in FLines; it adds the length of the
    //   cursor's line for every line up to it, not of each line (original bug) }
    public void CurrentToReal()
    {
        int length;
        int i;
        length = _items[0].Length;
        _realColumn = 0;
        _realLine = 0;
        i = 0;
        while (i <= _currentLine)
        {
            string line = Lines[_currentLine];
            _realColumn += line.Length;
            if (_realColumn >= length)
            {
                _realColumn = 0;
                _realLine += 1;
                length = _items[_realLine].Length;
            }

            i += 1;
        }

        _realColumn += _currentColumn;
    }

    // AoWMemo.pas:812
    // { RealToCur wraps the text again and finds the cursor in FLines from the cursor in FItems }
    public void RealToCurrent()
    {
        int length;
        int i;
        SetSize();
        length = _items[0].Length;
        _currentColumn = 0;
        _currentLine = 0;
        i = 0;
        while (i < _realLine)
        {
            string line = Lines[_currentLine];
            _currentColumn += line.Length;
            if (_currentColumn >= length)
            {
                _currentColumn = 0;
                i += 1;
                length = _items[i].Length;
            }

            _currentLine += 1;
        }

        i = Lines[_currentLine].Length;
        while (i < _realColumn && Lines.Count > _currentLine)
        {
            _currentLine += 1;
            i += Lines[_currentLine].Length;
        }

        _currentColumn = Lines[_currentLine].Length + _realColumn - i;
    }

    // AoWMemo.pas:842
    // { KeyStuff scrolls the cursor into view and shows it }
    public void KeyStuff()
    {
        if (_numberVisible + _topItem - 1 < _currentLine)
        {
            _verticalScrollBar?.Position = _verticalScrollBar.Position + 1;

            _topItem += 1;
        }

        if (_currentLine < _topItem)
        {
            _verticalScrollBar?.Position = _verticalScrollBar.Position - 1;

            _topItem -= 1;
        }

        _blink = true;
        _blinkCount = 0;
        Drawn = false;
        if (NoFrame)
        {
            Window?.Drawn = false;
        }
    }

    public int ListOffset { get => _topItem; set => SetListOff(value); }

    [Published]
    public StringList Items { get => _items; set => SetFStrings(value); }

    [Published]
    public VerticalScrollBar VerticalScrollBar { get => _verticalScrollBar; set => SetVScroll(value); }

    internal void SetVerticalScrollBarWithoutLayout(VerticalScrollBar value) => _verticalScrollBar = value;

    [Published]
    public byte CursorChar { get => _cursorChar; set => _cursorChar = value; }

    [Published]
    public byte BlinkDelay { get => _blinkDelay; set => _blinkDelay = value; }

    [Published]
    public bool Editable { get => _editable; set => _editable = value; }

    [Published]
    public InterfaceImageLibrary ImageLibraryFrame { get => _imageLibraryFrame; set => SetImageLibraryFrame(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryTexture { get => _imageLibraryTexture; set => SetImageLibraryTexture(value); }

    [Published]
    public int ImageLibraryIndexFrame { get => _imageLibraryIndexFrame; set => SetILFIndex(value); }

    [Published]
    public int ImageLibraryIndexTexture { get => _imageLibraryIndexTexture; set => SetImageLibraryTileIndex(value); }

    [Published]
    public int ItemHeight { get => _itemHeight; set => _itemHeight = value; }

    [Published]
    public byte ColorChangeChar { get => _colorChangeChar; set => _colorChangeChar = value; }

    [Published]
    public TextAlignment TextAlign { get => _textAlign; set => _textAlign = value; }

    public new event NotifyEvent<Memo> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, Memo>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, Memo>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<Memo> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, Memo>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, Memo>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<Memo> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, Memo>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, Memo>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<Memo> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, Memo>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, Memo>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<Memo> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, Memo>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, Memo>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<Memo> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, Memo>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, Memo>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Memo> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, Memo>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, Memo>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Memo> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Memo>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Memo>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Memo> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, Memo>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, Memo>(this, slot: nameof(OnUpdate), value: value);
    }
}
