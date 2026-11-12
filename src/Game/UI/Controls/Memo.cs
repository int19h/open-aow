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
    public int UnusedMouseUpValue;
    public StringList _items;
    public List<ColorChange> ColorList;
    public VerticalScrollBar _verticalScrollBar;
    public int NumberVisible;
    public int TopItem;
    public bool ScrollSet;
    public NotifyEvent<Memo> _onDrawItem;
    public int ScrollWidth;
    public bool ScrollVisible;
    public byte _cursorChar;
    public bool Blink;
    public byte _blinkDelay;
    public byte BlinkCount;
    public int CurrentColumn;
    public int CurrentLine;
    public int RealColumn;
    public int RealLine;
    public bool _editable;
    public InterfaceImageLibrary _imageLibraryFrame;
    public InterfaceImageLibrary _imageLibraryTexture;
    public int _itemHeight;
    public byte _colorChangeChar;
    public TextAlignment _textAlign;
    public bool NoFrame;
    public int _imageLibraryIndexFrame;
    public int _imageLibraryIndexTexture;
    public int DrawIndex;
    public Rect ItemRectangle;
    public StringList Lines;

    // AoWMemo.pas:126
    public Memo(Component owner) : base(owner)
    {
        Lines = new StringList();
        _items = new StringList();
        _items.OnChange = OnChangeText;
        ColorList = new List<ColorChange>();
        NumberVisible = 3;
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
                if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > _winHeight)
                {
                    var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                    _winHeight = imageLibraryFrame2.Height + imageLibraryFrame3.Height;
                }

                if (_imageLibraryFrame[_imageLibraryIndexFrame].Width > _winWidth)
                {
                    _winWidth = _imageLibraryFrame[_imageLibraryIndexFrame].Width;
                }

                var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                NumberVisible = (_winHeight - imageLibraryFrame4.Height - imageLibraryFrame5.Height) / _itemHeight;
                ScrollWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 4].Width;
                _masked = false;
                break;
            case true:
                NumberVisible = _winHeight / _itemHeight;
                ScrollWidth = 0;
                _masked = true;
                break;
        }

        if (_verticalScrollBar != null)
        {
            ScrollWidth = _verticalScrollBar.WinWidth;
            TopItem = _verticalScrollBar.Position;
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
                W = _imageLibraryFrame[_imageLibraryIndexFrame + 3].Width + ScrollWidth + _borderWidth * 2;
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
                    if (W + tw > _winWidth)
                    {
                        Lines.Add(S);
                        var colorChange2 = new ColorChange();
                        ColorList.Add(colorChange2);
                        if (!NoFrame)
                        {
                            W = tw + (_imageLibraryFrame[_imageLibraryIndexFrame + 3].Width + ScrollWidth + _borderWidth * 2);
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

        if (Lines.Count < TopItem + NumberVisible)
        {
            TopItem = Lines.Count - NumberVisible;
        }

        if (TopItem < 0)
        {
            TopItem = 0;
        }

        ScrollSet = false;
        if (!ScrollSet)
        {
            if (!NoFrame)
            {
                ScrollWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 4].Width;
            }
            else
            {
                ScrollWidth = 0;
            }

            ScrollVisible = false;
            if (_verticalScrollBar != null)
            {
                ScrollSet = true;
                if (_window != _verticalScrollBar.Window)
                {
                    _verticalScrollBar.Window = _window;
                }

                _verticalScrollBar.Min = 0;
                var count = Lines.Count;
                _verticalScrollBar.Max = count - 2;
                _verticalScrollBar.Priority = _priority + 1;
                _verticalScrollBar.Position = TopItem;
                _verticalScrollBar.NumberVisible = NumberVisible - 1;
                _verticalScrollBar.WinHeight = _winHeight;
                _verticalScrollBar.LargeChange = NumberVisible - 1;
                _verticalScrollBar.Visible = _visible;
                if (_verticalScrollBar.VanishWhenFull == true && Lines.Count - 1 < NumberVisible)
                {
                    _verticalScrollBar.Visible = false;
                }
                else
                {
                    ScrollWidth = _verticalScrollBar.WinWidth;
                }

                _verticalScrollBar.Sized = false;
                _verticalScrollBar.WinLeft = _winLeft + _winWidth - _verticalScrollBar.WinWidth;
                _verticalScrollBar.WinTop = _winTop;
                ScrollVisible = _verticalScrollBar.Visible;
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
        if (_visible)
        {
            if (_verticalScrollBar != null && Lines.Count > 0)
            {
                if (_verticalScrollBar.Position != TopItem)
                {
                    P = _verticalScrollBar.Position;
                    TopItem = P;
                    if (Lines.Count < P + NumberVisible)
                    {
                        TopItem = Lines.Count - NumberVisible;
                    }

                    if (TopItem < 0)
                    {
                        TopItem = 0;
                    }

                    Drawn = false;
                    if (NoFrame == true)
                    {
                        if (_window != null)
                        {
                            _window.Drawn = false;
                        }
                    }
                }
            }

            if (_editable && _blinkDelay > 0 && Redraw == true)
            {
                BlinkCount = (byte)(BlinkCount + 1);
                if (BlinkCount > _blinkDelay)
                {
                    Drawn = false;
                    if (NoFrame == true)
                    {
                        if (_window != null)
                        {
                            _window.Drawn = false;
                        }
                    }

                    BlinkCount = 0;
                    if (Blink == true)
                    {
                        Blink = false;
                    }
                    else
                    {
                        Blink = true;
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
            _verticalScrollBar.Visible = _visible;
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
            if (Valid && _visible)
            {
                if (_onPreDraw != null)
                {
                    _onPreDraw(this, lockedSurface);
                }

                _font.Color = Color;
                oldClip = lockedSurface.ClipRectangle;
                switch (NoFrame)
                {
                    case false:
                        var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _winWidth, _winHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle, ref lockedSurface);
                        var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var rectangle2 = new Rect(_window._winLeft + _winLeft + imageLibraryFrame3.Width, _window._winTop + _winTop + imageLibraryFrame2.Height, _winWidth - imageLibraryFrameWidth.Width - ScrollWidth, _winHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle2, ref lockedSurface);
                        var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        X = _window._winLeft + _winLeft + imageLibraryFrame4.Width;
                        var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        Y = _window._winTop + _winTop + imageLibraryFrame5.Height;
                        var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame7 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        H = _winHeight - imageLibraryFrame6.Height - imageLibraryFrame7.Height;
                        var imageLibraryFrame8 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        W = _winWidth - imageLibraryFrame8.Width - ScrollWidth;
                        break;
                    default:
                        X = _window._winLeft + _winLeft;
                        Y = _window._winTop + _winTop;
                        H = _winHeight;
                        W = _winWidth - ScrollWidth;
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

                for (int I = 0, last = NumberVisible; I <= last; I++)
                {
                    var count = Lines.Count;
                    if (TopItem + I < count)
                    {
                        cc = ColorList[TopItem + I];
                        if (cc.HasColor)
                        {
                            _font.Color = cc.Color;
                        }

                        switch (_textAlign)
                        {
                            case TextAlignment.Left:
                                var line = Lines[TopItem + I];
                                _font.SetText(_borderWidth + X, _borderHeight + Y + _itemHeight * I, line, lockedSurface);
                                break;
                            case TextAlignment.Right:
                                var line2 = Lines[TopItem + I];
                                var textWidth = _font.GetTextWidth(line2);
                                var line3 = Lines[TopItem + I];
                                _font.SetText(X + W - _borderWidth - textWidth, _borderHeight + Y + _itemHeight * I, line3, lockedSurface);
                                break;
                            default:
                                var line4 = Lines[TopItem + I];
                                var fontTextWidth = _font.GetTextWidth(line4);
                                var line5 = Lines[TopItem + I];
                                _font.SetText(X + W / 2 - fontTextWidth / 2, _borderHeight + Y + _itemHeight * I, line5, lockedSurface);
                                break;
                        }

                        DrawIndex = TopItem + I;
                        ItemRectangle = new Rect(X, Y + _itemHeight * I, X + W, Y + (I + 1) * _itemHeight);
                        if (ItemRectangle.Bottom > lockedSurface.ClipRectangle.Bottom)
                        {
                            ItemRectangle.Bottom = lockedSurface.ClipRectangle.Bottom;
                        }

                        if (_onDrawItem != null)
                        {
                            _onDrawItem(this);
                        }
                    }
                }

                lockedSurface.ClipRectangle = oldClip;
                bool condition = CurrentLine - TopItem > -1 && CurrentLine - TopItem < NumberVisible;
                if (condition)
                {
                    var count = Lines.Count;
                    condition = CurrentLine - TopItem < count;
                }

                if (condition && Redraw == true && _editable && Blink == true)
                {
                    var line = Lines[CurrentLine];
                    var textWidth = _font.GetTextWidth(Copy(line, 1, CurrentColumn));
                    _font.SetText(_borderWidth + X + textWidth, _borderHeight + Y + (CurrentLine - TopItem) * _itemHeight, Cp1252.Text(_cursorChar), lockedSurface);
                }

                if (_onDraw != null)
                {
                    _onDraw(this, lockedSurface);
                }
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
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = true;
            if (_onMouseDown != null)
            {
                _onMouseDown(this, button, shift, X, Y);
            }
        }
    }

    // AoWMemo.pas:610
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (_visible && X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth - ScrollWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = true;
            if (_editable == true)
            {
                _window._cursor = -4;
            }

            if (_onMouseMove != null)
            {
                _onMouseMove(this, shift, X, Y);
            }
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
        if (_editable && Redraw == true && Valid == true && _visible == true)
        {
            switch (key)
            {
                case 8:
                    if (RealColumn > 0)
                    {
                        var itemLength = _items[RealLine];
                        var item = _items[RealLine];
                        var item2 = _items[RealLine];
                        _items[RealLine] = Copy(item2, 1, RealColumn - 1) + Copy(item, RealColumn + 1, itemLength.Length - RealColumn + 1);
                        RealColumn -= 1;
                        changed = true;
                    }
                    else
                    {
                        if (RealLine > 0)
                        {
                            RealColumn = _items[RealLine - 1].Length;
                            var item = _items[RealLine - 1];
                            var item2 = _items[RealLine];
                            _items[RealLine - 1] = item + item2;
                            _items.Delete(RealLine);
                            RealLine -= 1;
                            changed = true;
                        }
                    }

                    break;
                case 13:
                    if (Lines[CurrentLine].Length == CurrentColumn)
                    {
                        _items.Insert(RealLine + 1, "");
                        var item = _items[RealLine];
                        _items[RealLine] = Copy(item, 1, RealColumn + 1);
                    }
                    else
                    {
                        var itemLength = _items[RealLine];
                        var item = _items[RealLine];
                        _items.Insert(RealLine + 1, Copy(item, RealColumn + 1, itemLength.Length));
                        var item2 = _items[RealLine];
                        _items[RealLine] = Copy(item2, 1, RealColumn);
                    }

                    RealColumn = 0;
                    RealLine += 1;
                    changed = true;
                    break;
                case >= 0 and <= 7:
                case >= 9 and <= 12:
                case >= 14 and <= 31:
                    break;
                default:
                    {
                        C = key;
                        if (Lines[CurrentLine].Length == CurrentColumn)
                        {
                            var itemLength = _items[RealLine];
                            var item = _items[RealLine];
                            var item2 = _items[RealLine];
                            _items[RealLine] = Copy(item2, 1, RealColumn + 1) + Cp1252.Text(C) + Copy(item, RealColumn + 2, itemLength.Length - RealColumn);
                        }
                        else
                        {
                            var itemLength = _items[RealLine];
                            var item = _items[RealLine];
                            var item2 = _items[RealLine];
                            _items[RealLine] = Copy(item2, 1, RealColumn) + Cp1252.Text(C) + Copy(item, RealColumn + 1, itemLength.Length - RealColumn);
                        }

                        RealColumn += 1;
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
            if (_onKeyDown != null)
            {
                _onKeyPress(this, ref key);
            }

            if (_onChange != null)
            {
                _onChange(this);
            }
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
        if (_editable && Redraw == true && Valid == true && _visible == true)
        {
            switch (key)
            {
                case 37:
                    if (RealColumn > 0)
                    {
                        RealColumn -= 1;
                    }
                    else
                    {
                        if (RealLine > 0)
                        {
                            RealLine -= 1;
                            RealColumn = _items[RealLine].Length;
                        }
                    }

                    if (CurrentColumn > 0)
                    {
                        CurrentColumn -= 1;
                    }
                    else
                    {
                        if (CurrentLine > 0)
                        {
                            CurrentLine -= 1;
                            CurrentColumn = Lines[CurrentLine].Length;
                        }
                    }

                    break;
                case 38:
                    if (CurrentLine > 0)
                    {
                        CurrentLine -= 1;
                        if (Lines[CurrentLine].Length < CurrentColumn)
                        {
                            CurrentColumn = Lines[CurrentLine].Length;
                        }

                        moved = true;
                    }

                    break;
                case 39:
                    if (Lines[CurrentLine].Length > CurrentColumn)
                    {
                        CurrentColumn += 1;
                    }
                    else
                    {
                        if (Lines.Count - 1 > CurrentLine)
                        {
                            CurrentColumn = 0;
                            CurrentLine += 1;
                        }
                    }

                    if (_items[RealLine].Length > RealColumn)
                    {
                        RealColumn += 1;
                    }
                    else
                    {
                        if (_items.Count - 1 > RealLine)
                        {
                            RealColumn = 0;
                            RealLine += 1;
                        }
                    }

                    break;
                case 40:
                    if (Lines.Count - 1 > CurrentLine)
                    {
                        CurrentLine += 1;
                        if (Lines[CurrentLine].Length < CurrentColumn)
                        {
                            CurrentColumn = Lines[CurrentLine].Length;
                        }

                        moved = true;
                    }

                    break;
                case 46:
                    if (_items[RealLine].Length > RealColumn)
                    {
                        var itemLength = _items[RealLine];
                        var item = _items[RealLine];
                        var item2 = _items[RealLine];
                        _items[RealLine] = Copy(item2, 1, RealColumn) + Copy(item, RealColumn + 2, itemLength.Length - RealColumn);
                        changed = true;
                    }
                    else
                    {
                        if (_items.Count - 1 > RealLine)
                        {
                            var item = _items[RealLine];
                            var item2 = _items[RealLine + 1];
                            _items[RealLine] = item + item2;
                            _items.Delete(RealLine + 1);
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
            if (_onKeyDown != null)
            {
                _onKeyDown(this, ref key, shift);
            }

            if (_onChange != null)
            {
                _onChange(this);
            }
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

        if (Lines.Count < NumberVisible)
        {
            NumberVisible = Lines.Count;
        }

        if (Lines.Count < TopItem + NumberVisible)
        {
            TopItem = Lines.Count - NumberVisible;
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

        if (value != TopItem)
        {
            TopItem = value;
            Drawn = false;
        }
    }

    // AoWMemo.pas:325
    public void SetVScroll(VerticalScrollBar value)
    {
        _verticalScrollBar = value;
        if (value != null)
        {
            value.Priority = _priority;
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
        RealColumn = 0;
        RealLine = 0;
        I = 0;
        while (I <= CurrentLine)
        {
            var line = Lines[CurrentLine];
            RealColumn = RealColumn + line.Length;
            if (RealColumn >= length)
            {
                RealColumn = 0;
                RealLine += 1;
                length = _items[RealLine].Length;
            }

            I += 1;
        }

        RealColumn = RealColumn + CurrentColumn;
    }

    // AoWMemo.pas:812
    // { RealToCur wraps the text again and finds the cursor in FLines from the cursor in FItems }
    public void RealToCurrent()
    {
        int length;
        int I;
        SetSize();
        length = _items[0].Length;
        CurrentColumn = 0;
        CurrentLine = 0;
        I = 0;
        while (I < RealLine)
        {
            var line = Lines[CurrentLine];
            CurrentColumn = CurrentColumn + line.Length;
            if (CurrentColumn >= length)
            {
                CurrentColumn = 0;
                I += 1;
                length = _items[I].Length;
            }

            CurrentLine += 1;
        }

        I = Lines[CurrentLine].Length;
        while (I < RealColumn && Lines.Count > CurrentLine)
        {
            CurrentLine += 1;
            I = I + Lines[CurrentLine].Length;
        }

        CurrentColumn = Lines[CurrentLine].Length + RealColumn - I;
    }

    // AoWMemo.pas:842
    // { KeyStuff scrolls the cursor into view and shows it }
    public void KeyStuff()
    {
        if (NumberVisible + TopItem - 1 < CurrentLine)
        {
            if (_verticalScrollBar != null)
            {
                _verticalScrollBar.Position = _verticalScrollBar.Position + 1;
            }

            TopItem += 1;
        }

        if (CurrentLine < TopItem)
        {
            if (_verticalScrollBar != null)
            {
                _verticalScrollBar.Position = _verticalScrollBar.Position - 1;
            }

            TopItem -= 1;
        }

        Blink = true;
        BlinkCount = 0;
        Drawn = false;
        if (NoFrame == true)
        {
            if (_window != null)
            {
                _window.Drawn = false;
            }
        }
    }

    public int ListOffset { get => TopItem; set => SetListOff(value); }

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

    [Published]
    public NotifyEvent<Memo> OnDrawItem { get => _onDrawItem; set => _onDrawItem = value; }

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

    public new NotifyEvent<Memo> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<Memo> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<Memo> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<Memo> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<Memo> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<Memo> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<Memo> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<Memo> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<Memo> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
