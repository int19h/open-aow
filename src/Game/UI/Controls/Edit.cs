// Translated from AoWEdit.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// { The AoWEdit unit of aowInt.dpl: TAOWEdit, a one-line edit box drawn from an edit library (a
//   horizontal line of three images) with its text in AOWFont. It scrolls the text so that the
//   part after FFirstChar fits, shows a blinking cursor character while it has the input focus
//   (FRedraw), takes the focus on a click and loses it on a click outside, and handles the
//   cursor keys, Delete, Backspace, Return and Ctrl+V (paste the clipboard text). }
// Fields: FMaxChars (0: no limit), FCursorChar, FPassWordChar (#0: show the text), FCursorShown
// (the blink state of the cursor), FBlinkDelay (updates between two blinks), FBlinkCount,
// FShownChars (the characters from FFirstChar to the end), FFirstChar (the first character
// shown), FEditable, FILEdit, FOnExit, FOnReturn, FILIndex, FCursorPos (the cursor position
// counted from FFirstChar). FRedraw is the input focus
public partial class Edit : Control
{
    public int _maxChars;
    public byte _cursorChar;
    public byte _passWordChar;
    public bool CursorShown;
    public int _blinkDelay;
    public int BlinkCount;
    public int ShownChars;
    public int FirstChar;
    public bool _editable;
    public InterfaceImageLibrary _imageLibraryEdit;
    public NotifyEvent<Edit> _onExit;
    public NotifyEvent<Edit> _onReturn;
    public int _imageLibraryIndex;
    public int CursorPosition;

    // AoWEdit.pas:79
    public Edit(Component owner) : base(owner)
    {
        _cursorChar = 124;
        CursorShown = true;
        _editable = true;
    }

    // AoWEdit.pas:87
    public override void Validate()
    {
        if (_imageLibraryEdit != null)
        {
            _imageLibraryEdit = _imageLibraryEdit.CheckImageLibrary(_imageLibraryIndex, 3, this);
        }

        if (_imageLibraryEdit != null && _font != null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // AoWEdit.pas:96
    public override void SetSize()
    {
        var imageLibraryEditWidth = _imageLibraryEdit[_imageLibraryIndex];
        var imageLibraryEdit = _imageLibraryEdit[_imageLibraryIndex + 2];
        if (imageLibraryEditWidth.Width + imageLibraryEdit.Width > _winWidth)
        {
            var imageLibraryEdit2 = _imageLibraryEdit[_imageLibraryIndex];
            var imageLibraryEdit3 = _imageLibraryEdit[_imageLibraryIndex + 2];
            _winWidth = imageLibraryEdit2.Width + imageLibraryEdit3.Width;
        }

        _winHeight = _imageLibraryEdit[_imageLibraryIndex].Height;
        FirstChar = 1;
        if (_text.Length < CursorPosition)
        {
            CursorPosition = _text.Length;
        }

        if (_font.GetTextWidth(Copy(_text, FirstChar, _text.Length - FirstChar)) > _winWidth - 2 * _borderWidth - 1)
        {
            while (_font.GetTextWidth(Copy(_text, FirstChar, _text.Length)) > _winWidth - 2 * _borderWidth - 1)
            {
                FirstChar += 1;
            }
        }

        ShownChars = _text.Length - FirstChar + 1;
        base.SetSize();
    }

    // AoWEdit.pas:137
    public override void Update()
    {
        base.Update();
        if (_visible && _editable && _blinkDelay > 0 && Redraw == true)
        {
            BlinkCount += 1;
            if (BlinkCount > _blinkDelay)
            {
                Drawn = false;
                BlinkCount = 0;
                if (CursorShown == true)
                {
                    CursorShown = false;
                }
                else
                {
                    CursorShown = true;
                }
            }
        }
    }

    // AoWEdit.pas:155
    public override void Draw(LockedSurface lockedSurface)
    {
        string S = "";
        Rect oldClip = default;
        try
        {
            if (Valid && _visible)
            {
                if (_onPreDraw != null)
                {
                    _onPreDraw(this, lockedSurface);
                }

                var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _winWidth, _winHeight);
                _imageLibraryEdit.DrawHorizontalStrip(_imageLibraryIndex, rectangle, ref lockedSurface);
                oldClip = lockedSurface.ClipRectangle;
                var imageLibraryEditWidth = _imageLibraryEdit[_imageLibraryIndex + 2];
                if (_window._winLeft + _winLeft + _winWidth - imageLibraryEditWidth.Width < oldClip.Right)
                {
                    var imageLibraryEdit = _imageLibraryEdit[_imageLibraryIndex + 2];
                    lockedSurface.ClipRectangle.Right = _window._winLeft + _winLeft + _winWidth - imageLibraryEdit.Width;
                }

                _font.Color = Color;
                if (_passWordChar == 0)
                {
                    _font.SetText(_window._winLeft + _winLeft + _borderWidth, _window._winTop + _winTop + _borderHeight, Copy(_text, FirstChar, ShownChars), lockedSurface);
                }
                else
                {
                    S = "";
                    for (int I = 0, last = ShownChars - 1; I <= last; I++)
                    {
                        S = S + Cp1252.Text(_passWordChar);
                    }

                    _font.SetText(_window._winLeft + _winLeft + _borderWidth, _window._winTop + _winTop + _borderHeight, S, lockedSurface);
                }

                lockedSurface.ClipRectangle = oldClip;
                if (_editable && Redraw == true && CursorShown == true)
                {
                    var textWidth = _font.GetTextWidth(Copy(_text, FirstChar, CursorPosition));
                    _font.SetText(_window._winLeft + _winLeft + _borderWidth + textWidth, _window._winTop + _winTop + _borderHeight, Cp1252.Text(_cursorChar), lockedSurface);
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

    // AoWEdit.pas:197
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        int W = default;
        int I = default;
        bool oldRedraw;
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        MouseDown = false;
        oldRedraw = Redraw;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = true;
            if (Valid == true && _visible == true)
            {
                if (button == MouseButton.Left)
                {
                    if (_editable == true)
                    {
                        Redraw = true;
                    }

                    var textWidth = _font.GetTextWidth(Copy(_text, FirstChar, _text.Length));
                    if (X > _window._winLeft + _winLeft + _borderWidth + textWidth)
                    {
                        FirstChar = 1;
                        if (_font.GetTextWidth(Copy(_text, FirstChar, _text.Length - FirstChar)) > _winWidth - 2 * _borderWidth - 1)
                        {
                            while (_font.GetTextWidth(Copy(_text, FirstChar, _text.Length)) > _winWidth - 2 * _borderWidth - 1)
                            {
                                FirstChar += 1;
                            }
                        }

                        CursorPosition = _text.Length - FirstChar + 1;
                    }
                    else
                    {
                        I = 0;
                        W = _font.GetTextWidth(Copy(_text, FirstChar, I + 1));
                        while (X > _window._winLeft + _winLeft + _borderWidth + W && _text.Length > I + FirstChar - 1)
                        {
                            I += 1;
                            W = _font.GetTextWidth(Copy(_text, FirstChar, I + 1));
                        }

                        CursorPosition = I;
                    }

                    CursorShown = _editable;
                    BlinkCount = 0;
                    Drawn = false;
                }

                if (_onMouseDown != null)
                {
                    _onMouseDown(this, button, shift, X, Y);
                }
            }
        }
        else
        {
            if (CursorShown)
            {
                CursorShown = false;
                if (_onExit != null)
                {
                    _onExit(this);
                }
            }

            Redraw = false;
        }

        if (Redraw != oldRedraw)
        {
            Drawn = false;
        }
    }

    // AoWEdit.pas:261
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = true;
            if (_onMouseUp != null)
            {
                _onMouseUp(this, button, shift, X, Y);
            }
        }
    }

    // AoWEdit.pas:273
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (_visible && X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
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

    // AoWEdit.pas:289
    public override void CheckKeyPress(ref byte key)
    {
        byte C = default;
        bool scroll;
        Handled = Redraw;
        scroll = true;
        if (_editable && Redraw == true && Valid == true && _visible == true)
        {
            CursorShown = true;
            switch (key)
            {
                case 8:
                    if (CursorPosition > 0)
                    {
                        _text = Copy(_text, 1, CursorPosition + FirstChar - 2) + Copy(_text, CursorPosition + FirstChar, _text.Length - CursorPosition - FirstChar + 1);
                        CursorPosition = CursorPosition + FirstChar - 2;
                    }

                    FirstChar = 1;
                    break;
                case 13:
                    CursorShown = false;
                    Redraw = false;
                    if (_onReturn != null)
                    {
                        _onReturn(this);
                    }

                    break;
                case >= 0 and <= 7:
                case >= 9 and <= 12:
                case >= 14 and <= 31:
                    break;
                default:
                    C = key;
                    if (_text.Length < _maxChars || _maxChars == 0)
                    {
                        _text = Copy(_text, 1, FirstChar + CursorPosition - 1) + Cp1252.Text(C) + Copy(_text, FirstChar + CursorPosition, _text.Length - CursorPosition - FirstChar + 1);
                        CursorPosition += 1;
                        CursorPosition = CursorPosition + FirstChar - 1;
                        FirstChar = 1;
                    }

                    break;
            }

            if (scroll == true)
            {
                while (_font.GetTextWidth(Copy(_text, FirstChar, _text.Length)) > _winWidth - 2 * _borderWidth - 1)
                {
                    FirstChar += 1;
                    CursorPosition -= 1;
                }

                ShownChars = _text.Length - FirstChar + 1;
                if (CursorPosition < 0)
                {
                    CursorPosition = 0;
                }
            }

            BlinkCount = 0;
            Drawn = false;
            if (_onKeyPress != null)
            {
                _onKeyPress(this, ref key);
            }

            if (_onChange != null)
            {
                _onChange(this);
            }
        }
    }

    // AoWEdit.pas:119
    public virtual void SetImageLibraryEdit(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryEdit)
        {
            Valid = false;
            _imageLibraryEdit = value;
        }
    }

    // AoWEdit.pas:113
    public void SetGText(string value)
    {
        _text = value;
        Sized = false;
    }

    // AoWEdit.pas:128
    public void SetImageLibraryIndex(int value)
    {
        if (value != _imageLibraryIndex)
        {
            _imageLibraryIndex = value;
            Valid = false;
        }
    }

    [Published]
    public ImageLibraryFont Font { get => _font; set => _font = value; }

    [Published]
    public string Text { get => _text; set => SetGText(value); }

    [Published]
    public int BorderWidth { get => _borderWidth; set => _borderWidth = value; }

    [Published]
    public int BorderHeight { get => _borderHeight; set => _borderHeight = value; }

    [Published]
    public int MaxChars { get => _maxChars; set => _maxChars = value; }

    [Published]
    public byte CursorChar { get => _cursorChar; set => _cursorChar = value; }

    [Published]
    public int BlinkDelay { get => _blinkDelay; set => _blinkDelay = value; }

    [Published]
    public bool Editable { get => _editable; set => _editable = value; }

    [Published]
    public InterfaceImageLibrary ImageLibraryEdit { get => _imageLibraryEdit; set => SetImageLibraryEdit(value); }

    [Published]
    public int ImageLibraryIndex { get => _imageLibraryIndex; set => SetImageLibraryIndex(value); }

    [Published]
    public NotifyEvent<Edit> OnExit { get => _onExit; set => _onExit = value; }

    [Published]
    public NotifyEvent<Edit> OnReturn { get => _onReturn; set => _onReturn = value; }

    [Published]
    public byte PassWordChar { get => _passWordChar; set => _passWordChar = value; }

    // AoWEdit.pas:350. Clipboard integration has no host in this port.
    public override void CheckKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        Handled = Redraw;
        if (!_editable || !Redraw || !Valid || !_visible) return;
        switch (key)
        {
            case 37:
                if (CursorPosition > 0) CursorPosition--;
                else if (FirstChar > 1) FirstChar--;
                break;
            case 39:
                if (ShownChars > CursorPosition) CursorPosition++;
                else if (FirstChar + ShownChars < _text.Length) FirstChar++;
                break;
            case 46:
                if (CursorPosition < _text.Length)
                {
                    _text = Copy(_text, 1, CursorPosition + FirstChar - 1) +
                        Copy(_text, CursorPosition + FirstChar + 1, _text.Length - CursorPosition - FirstChar);
                    CursorPosition += FirstChar - 1;
                    FirstChar = 1;
                }
                break;
        }
        ShownChars = _text.Length - FirstChar + 1;
        CursorPosition = Math.Max(CursorPosition, 0);
        CursorShown = true;
        BlinkCount = 0;
        Drawn = false;
        _onKeyDown?.Invoke(this, ref key, shift);
        _onChange?.Invoke(this);
    }

    public new NotifyEvent<Edit> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<Edit> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<Edit> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<Edit> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<Edit> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<Edit> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<Edit> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<Edit> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<Edit> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
