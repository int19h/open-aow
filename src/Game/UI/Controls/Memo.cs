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
        ColorList = new List<ColorChange>();
        _numberVisible = 3;
        _cursorChar = 124;
        _editable = true;
    }

    // AoWMemo.pas:138
    public override void Dispose()
    {
        Lines?.Dispose();
        _items?.Dispose();
        for (int I = 0, last = ColorList.Count - 1; I <= last; I++)
        {
        }

        ColorList?.Clear();
        base.Dispose();
    }

    // AoWMemo.pas:150
    public override void Validate()
    {
        if (_font != null)
        {
            if (_imageLibraryFrame != null)
            {
                _imageLibraryFrame = _imageLibraryFrame.CheckImageLibrary(_imageLibraryIndexFrame, 8, this);
            }

            if (_imageLibraryTexture != null)
            {
                _imageLibraryTexture = _imageLibraryTexture.CheckImageLibrary(_imageLibraryIndexTexture, 1, this);
            }

            if (_imageLibraryFrame != null && _imageLibraryTexture != null)
            {
                NoFrame = false;
            }
            else
            {
                NoFrame = true;
            }

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
        int W;
        ReadOnlySpan<byte> text;
        string S;
        int J;
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
                var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > WinHeight)
                {
                    var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                    SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
                }

                if (_imageLibraryFrame[_imageLibraryIndexFrame].Width > WinWidth)
                {
                    SetWinWidthWithoutInvalidate(_imageLibraryFrame[_imageLibraryIndexFrame].Width);
                }

                var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
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

        for (int I = 0, last = ColorList.Count - 1; I <= last; I++)
        {
            cc = ColorList[I];
        }

        ColorList.Clear();
        Lines.Clear();
        for (int I = 0, last = _items.Count - 1; I <= last; I++)
        {
            Lines.Add("");
            var colorChange = new ColorChange();
            ColorList.Add(colorChange);
            J = 0;
            start = 0;
            if (!NoFrame)
            {
                W = _imageLibraryFrame[_imageLibraryIndexFrame + 3].Width + _scrollWidth + _borderWidth * 2;
            }
            else
            {
                W = _borderWidth * 2;
            }

            string item = NullTerminatedText(_items[I]);
            text = Cp1252.Encode(item);
            length = text.Length;
            if (length > 6 && text[0] == _colorChangeChar)
            {
                cc = ColorList[ColorList.Count - 1];
                cc.HasColor = true;
                try
                {
                    var item2 = _items[I];
                    cc.Color = StrToInt("$" + Copy(item2, 2, 6));
                }
                catch
                {
                }

                J = 8;
                start = 7;
            }

            while (J < length)
            {
                if (text[J] == 32 || J == length - 1)
                {
                    S = Copy(item, start + 1, J + 1 - start);
                    tw = _font.GetTextWidth(S);
                    if (W + tw > WinWidth)
                    {
                        Lines.Add(S);
                        var colorChange2 = new ColorChange();
                        ColorList.Add(colorChange2);
                        if (!NoFrame)
                        {
                            W = tw + (_imageLibraryFrame[_imageLibraryIndexFrame + 3].Width + _scrollWidth + _borderWidth * 2);
                        }
                        else
                        {
                            W = tw + _borderWidth * 2;
                        }
                    }
                    else
                    {
                        var count = Lines.Count;
                        var line = Lines[count - 1];
                        var fLinesCount = Lines.Count;
                        Lines[fLinesCount - 1] = line + S;
                        W = W + tw;
                    }

                    start = J + 1;
                }

                J += 1;
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
            if (!NoFrame)
            {
                _scrollWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 4].Width;
            }
            else
            {
                _scrollWidth = 0;
            }
            if (_verticalScrollBar != null)
            {
                _scrollSet = true;
                if (Window != _verticalScrollBar.Window)
                {
                    _verticalScrollBar.Window = Window;
                }

                _verticalScrollBar.Min = 0;
                var count = Lines.Count;
                _verticalScrollBar.Max = count - 2;
                _verticalScrollBar.Priority = Priority + 1;
                _verticalScrollBar.Position = _topItem;
                _verticalScrollBar.NumberVisible = _numberVisible - 1;
                _verticalScrollBar.WinHeight = WinHeight;
                _verticalScrollBar.LargeChange = _numberVisible - 1;
                _verticalScrollBar.Visible = Visible;
                if (_verticalScrollBar.VanishWhenFull == true && Lines.Count - 1 < _numberVisible)
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
        int P;
        base.Update();
        if (Visible)
        {
            if (_verticalScrollBar != null && Lines.Count > 0)
            {
                if (_verticalScrollBar.Position != _topItem)
                {
                    P = _verticalScrollBar.Position;
                    _topItem = P;
                    if (Lines.Count < P + _numberVisible)
                    {
                        _topItem = Lines.Count - _numberVisible;
                    }

                    if (_topItem < 0)
                    {
                        _topItem = 0;
                    }

                    Drawn = false;
                    if (NoFrame == true)
                    {
                        if (Window != null)
                        {
                            Window.Drawn = false;
                        }
                    }
                }
            }

            if (_editable && _blinkDelay > 0 && Redraw == true)
            {
                _blinkCount = (byte)(_blinkCount + 1);
                if (_blinkCount > _blinkDelay)
                {
                    Drawn = false;
                    if (NoFrame == true)
                    {
                        if (Window != null)
                        {
                            Window.Drawn = false;
                        }
                    }

                    _blinkCount = 0;
                    if (_blink == true)
                    {
                        _blink = false;
                    }
                    else
                    {
                        _blink = true;
                    }
                }
            }
        }
    }

    // AoWMemo.pas:333
    public override void SetVisible(bool value)
    {
        base.SetVisible(value);
        if (_verticalScrollBar != null)
        {
            _verticalScrollBar.Visible = Visible;
        }
    }

    // AoWMemo.pas:378
    // { Draw draws the frame and the texture, the visible lines (clipped to the text area) and the
    //   cursor }
    public override void Draw(LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        int W = default;
        Rect oldClip = default;
        int H = default;
        ColorChange cc = default;
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(this, lockedSurface);

                _font.Color = Color;
                oldClip = lockedSurface.ClipRectangle;
                switch (NoFrame)
                {
                    case false:
                        var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle, ref lockedSurface);
                        var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var rectangle2 = new SizedRectangle(Window.WinLeft + WinLeft + imageLibraryFrame3.Width, Window.WinTop + WinTop + imageLibraryFrame2.Height, WinWidth - imageLibraryFrameWidth.Width - _scrollWidth, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle2, ref lockedSurface);
                        var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        X = Window.WinLeft + WinLeft + imageLibraryFrame4.Width;
                        var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        Y = Window.WinTop + WinTop + imageLibraryFrame5.Height;
                        var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame7 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        H = WinHeight - imageLibraryFrame6.Height - imageLibraryFrame7.Height;
                        var imageLibraryFrame8 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        W = WinWidth - imageLibraryFrame8.Width - _scrollWidth;
                        break;
                    default:
                        X = Window.WinLeft + WinLeft;
                        Y = Window.WinTop + WinTop;
                        H = WinHeight;
                        W = WinWidth - _scrollWidth;
                        break;
                }

                if (X + W < oldClip.Right)
                {
                    lockedSurface.ClipRectangle.Right = X + W;
                }

                if (Y + H < oldClip.Bottom)
                {
                    lockedSurface.ClipRectangle.Bottom = Y + H;
                }

                for (int I = 0, last = _numberVisible; I <= last; I++)
                {
                    var count = Lines.Count;
                    if (_topItem + I < count)
                    {
                        cc = ColorList[_topItem + I];
                        if (cc.HasColor)
                        {
                            _font.Color = cc.Color;
                        }

                        switch (_textAlign)
                        {
                            case TextAlignment.Left:
                                var line = Lines[_topItem + I];
                                _font.SetText(_borderWidth + X, _borderHeight + Y + _itemHeight * I, line, lockedSurface);
                                break;
                            case TextAlignment.Right:
                                var line2 = Lines[_topItem + I];
                                var textWidth = _font.GetTextWidth(line2);
                                var line3 = Lines[_topItem + I];
                                _font.SetText(X + W - _borderWidth - textWidth, _borderHeight + Y + _itemHeight * I, line3, lockedSurface);
                                break;
                            default:
                                var line4 = Lines[_topItem + I];
                                var fontTextWidth = _font.GetTextWidth(line4);
                                var line5 = Lines[_topItem + I];
                                _font.SetText(X + W / 2 - fontTextWidth / 2, _borderHeight + Y + _itemHeight * I, line5, lockedSurface);
                                break;
                        }
                        _itemRectangle = new Rect(X, Y + _itemHeight * I, X + W, Y + (I + 1) * _itemHeight);
                        if (_itemRectangle.Bottom > lockedSurface.ClipRectangle.Bottom)
                        {
                            _itemRectangle.Bottom = lockedSurface.ClipRectangle.Bottom;
                        }

                        if (OnDrawItem != null)
                        {
                            OnDrawItem(this);
                        }
                    }
                }

                lockedSurface.ClipRectangle = oldClip;
                bool condition = _currentLine - _topItem > -1 && _currentLine - _topItem < _numberVisible;
                if (condition)
                {
                    var count = Lines.Count;
                    condition = _currentLine - _topItem < count;
                }

                if (condition && Redraw == true && _editable && _blink == true)
                {
                    var line = Lines[_currentLine];
                    var textWidth = _font.GetTextWidth(Copy(line, 1, _currentColumn));
                    _font.SetText(_borderWidth + X + textWidth, _borderHeight + Y + (_currentLine - _topItem) * _itemHeight, Cp1252.Text(_cursorChar), lockedSurface);
                }

                RaiseDraw(this, lockedSurface);
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
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
            if (_editable == true)
            {
                Window._cursor = -4;
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
        byte C = default;
        Handled = Redraw;
        changed = false;
        moved = false;
        if (_editable && Redraw == true && Valid == true && Visible == true)
        {
            switch (key)
            {
                case 8:
                    if (_realColumn > 0)
                    {
                        var itemLength = _items[_realLine];
                        var item = _items[_realLine];
                        var item2 = _items[_realLine];
                        _items[_realLine] = Copy(item2, 1, _realColumn - 1) + Copy(item, _realColumn + 1, itemLength.Length - _realColumn + 1);
                        _realColumn -= 1;
                        changed = true;
                    }
                    else
                    {
                        if (_realLine > 0)
                        {
                            _realColumn = _items[_realLine - 1].Length;
                            var item = _items[_realLine - 1];
                            var item2 = _items[_realLine];
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
                        var item = _items[_realLine];
                        _items[_realLine] = Copy(item, 1, _realColumn + 1);
                    }
                    else
                    {
                        var itemLength = _items[_realLine];
                        var item = _items[_realLine];
                        _items.Insert(_realLine + 1, Copy(item, _realColumn + 1, itemLength.Length));
                        var item2 = _items[_realLine];
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
                        C = key;
                        if (Lines[_currentLine].Length == _currentColumn)
                        {
                            var itemLength = _items[_realLine];
                            var item = _items[_realLine];
                            var item2 = _items[_realLine];
                            _items[_realLine] = Copy(item2, 1, _realColumn + 1) + Cp1252.Text(C) + Copy(item, _realColumn + 2, itemLength.Length - _realColumn);
                        }
                        else
                        {
                            var itemLength = _items[_realLine];
                            var item = _items[_realLine];
                            var item2 = _items[_realLine];
                            _items[_realLine] = Copy(item2, 1, _realColumn) + Cp1252.Text(C) + Copy(item, _realColumn + 1, itemLength.Length - _realColumn);
                        }

                        _realColumn += 1;
                        changed = true;
                    }

                    break;
            }

            if (moved == true)
            {
                CurrentToReal();
            }

            if (changed == true)
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
        if (_editable && Redraw == true && Valid == true && Visible == true)
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
                        var itemLength = _items[_realLine];
                        var item = _items[_realLine];
                        var item2 = _items[_realLine];
                        _items[_realLine] = Copy(item2, 1, _realColumn) + Copy(item, _realColumn + 2, itemLength.Length - _realColumn);
                        changed = true;
                    }
                    else
                    {
                        if (_items.Count - 1 > _realLine)
                        {
                            var item = _items[_realLine];
                            var item2 = _items[_realLine + 1];
                            _items[_realLine] = item + item2;
                            _items.Delete(_realLine + 1);
                            changed = true;
                        }
                    }

                    break;
            }

            if (changed == true)
            {
                RealToCurrent();
            }

            if (moved == true)
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
        for (int I = 0, last = value.Count - 1; I <= last; I++)
        {
            var valueValue = value[I];
            Lines.Add(valueValue);
            var valueValue2 = value[I];
            _items.Add(valueValue2);
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
        if (value != null)
        {
            value.Priority = Priority;
        }

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
        int I;
        length = _items[0].Length;
        _realColumn = 0;
        _realLine = 0;
        I = 0;
        while (I <= _currentLine)
        {
            var line = Lines[_currentLine];
            _realColumn = _realColumn + line.Length;
            if (_realColumn >= length)
            {
                _realColumn = 0;
                _realLine += 1;
                length = _items[_realLine].Length;
            }

            I += 1;
        }

        _realColumn = _realColumn + _currentColumn;
    }

    // AoWMemo.pas:812
    // { RealToCur wraps the text again and finds the cursor in FLines from the cursor in FItems }
    public void RealToCurrent()
    {
        int length;
        int I;
        SetSize();
        length = _items[0].Length;
        _currentColumn = 0;
        _currentLine = 0;
        I = 0;
        while (I < _realLine)
        {
            var line = Lines[_currentLine];
            _currentColumn = _currentColumn + line.Length;
            if (_currentColumn >= length)
            {
                _currentColumn = 0;
                I += 1;
                length = _items[I].Length;
            }

            _currentLine += 1;
        }

        I = Lines[_currentLine].Length;
        while (I < _realColumn && Lines.Count > _currentLine)
        {
            _currentLine += 1;
            I = I + Lines[_currentLine].Length;
        }

        _currentColumn = Lines[_currentLine].Length + _realColumn - I;
    }

    // AoWMemo.pas:842
    // { KeyStuff scrolls the cursor into view and shows it }
    public void KeyStuff()
    {
        if (_numberVisible + _topItem - 1 < _currentLine)
        {
            if (_verticalScrollBar != null)
            {
                _verticalScrollBar.Position = _verticalScrollBar.Position + 1;
            }

            _topItem += 1;
        }

        if (_currentLine < _topItem)
        {
            if (_verticalScrollBar != null)
            {
                _verticalScrollBar.Position = _verticalScrollBar.Position - 1;
            }

            _topItem -= 1;
        }

        _blink = true;
        _blinkCount = 0;
        Drawn = false;
        if (NoFrame == true)
        {
            if (Window != null)
            {
                Window.Drawn = false;
            }
        }
    }

    public int ListOffset { get => _topItem; set => SetListOff(value); }

    [Published]
    public ImageLibraryFont Font { get => _font; set => _font = value; }

    [Published]
    public StringList Items { get => _items; set => SetFStrings(value); }

    [Published]
    public int BorderWidth { get => _borderWidth; set => _borderWidth = value; }

    [Published]
    public int BorderHeight { get => _borderHeight; set => _borderHeight = value; }

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
