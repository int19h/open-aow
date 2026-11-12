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
    private int _maxChars;
    private byte _cursorChar;
    private byte _passWordChar;
    private bool _cursorShown;
    private int _blinkDelay;
    private int _blinkCount;
    private int _shownChars;

    public int FirstChar { get; set; }

    private bool _editable;
    private InterfaceImageLibrary _imageLibraryEdit;
    [Published]
    public event NotifyEvent<Edit> OnExit;
    [Published]
    public event NotifyEvent<Edit> OnReturn;
    private int _imageLibraryIndex;
    private int _cursorPosition;

    // AoWEdit.pas:79
    public Edit(Component owner) : base(owner)
    {
        _cursorChar = 124;
        _cursorShown = true;
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
        if (imageLibraryEditWidth.Width + imageLibraryEdit.Width > WinWidth)
        {
            var imageLibraryEdit2 = _imageLibraryEdit[_imageLibraryIndex];
            var imageLibraryEdit3 = _imageLibraryEdit[_imageLibraryIndex + 2];
            SetWinWidthWithoutInvalidate(imageLibraryEdit2.Width + imageLibraryEdit3.Width);
        }

        SetWinHeightWithoutInvalidate(_imageLibraryEdit[_imageLibraryIndex].Height);
        FirstChar = 1;
        if (_text.Length < _cursorPosition)
        {
            _cursorPosition = _text.Length;
        }

        if (_font.GetTextWidth(Copy(_text, FirstChar, _text.Length - FirstChar)) > WinWidth - 2 * _borderWidth - 1)
        {
            while (_font.GetTextWidth(Copy(_text, FirstChar, _text.Length)) > WinWidth - 2 * _borderWidth - 1)
            {
                FirstChar += 1;
            }
        }

        _shownChars = _text.Length - FirstChar + 1;
        base.SetSize();
    }

    // AoWEdit.pas:137
    public override void Update()
    {
        base.Update();
        if (Visible && _editable && _blinkDelay > 0 && Redraw == true)
        {
            _blinkCount += 1;
            if (_blinkCount > _blinkDelay)
            {
                Drawn = false;
                _blinkCount = 0;
                if (_cursorShown == true)
                {
                    _cursorShown = false;
                }
                else
                {
                    _cursorShown = true;
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
            if (Valid && Visible)
            {
                RaisePreDraw(this, lockedSurface);

                var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                _imageLibraryEdit.DrawHorizontalStrip(_imageLibraryIndex, rectangle, ref lockedSurface);
                oldClip = lockedSurface.ClipRectangle;
                var imageLibraryEditWidth = _imageLibraryEdit[_imageLibraryIndex + 2];
                if (Window.WinLeft + WinLeft + WinWidth - imageLibraryEditWidth.Width < oldClip.Right)
                {
                    var imageLibraryEdit = _imageLibraryEdit[_imageLibraryIndex + 2];
                    lockedSurface.ClipRectangle.Right = Window.WinLeft + WinLeft + WinWidth - imageLibraryEdit.Width;
                }

                _font.Color = Color;
                if (_passWordChar == 0)
                {
                    _font.SetText(Window.WinLeft + WinLeft + _borderWidth, Window.WinTop + WinTop + _borderHeight, Copy(_text, FirstChar, _shownChars), lockedSurface);
                }
                else
                {
                    S = "";
                    for (int I = 0, last = _shownChars - 1; I <= last; I++)
                    {
                        S = S + Cp1252.Text(_passWordChar);
                    }

                    _font.SetText(Window.WinLeft + WinLeft + _borderWidth, Window.WinTop + WinTop + _borderHeight, S, lockedSurface);
                }

                lockedSurface.ClipRectangle = oldClip;
                if (_editable && Redraw == true && _cursorShown == true)
                {
                    var textWidth = _font.GetTextWidth(Copy(_text, FirstChar, _cursorPosition));
                    _font.SetText(Window.WinLeft + WinLeft + _borderWidth + textWidth, Window.WinTop + WinTop + _borderHeight, Cp1252.Text(_cursorChar), lockedSurface);
                }

                RaiseDraw(this, lockedSurface);
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
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            if (Valid == true && Visible == true)
            {
                if (button == MouseButton.Left)
                {
                    if (_editable == true)
                    {
                        Redraw = true;
                    }

                    var textWidth = _font.GetTextWidth(Copy(_text, FirstChar, _text.Length));
                    if (X > Window.WinLeft + WinLeft + _borderWidth + textWidth)
                    {
                        FirstChar = 1;
                        if (_font.GetTextWidth(Copy(_text, FirstChar, _text.Length - FirstChar)) > WinWidth - 2 * _borderWidth - 1)
                        {
                            while (_font.GetTextWidth(Copy(_text, FirstChar, _text.Length)) > WinWidth - 2 * _borderWidth - 1)
                            {
                                FirstChar += 1;
                            }
                        }

                        _cursorPosition = _text.Length - FirstChar + 1;
                    }
                    else
                    {
                        I = 0;
                        W = _font.GetTextWidth(Copy(_text, FirstChar, I + 1));
                        while (X > Window.WinLeft + WinLeft + _borderWidth + W && _text.Length > I + FirstChar - 1)
                        {
                            I += 1;
                            W = _font.GetTextWidth(Copy(_text, FirstChar, I + 1));
                        }

                        _cursorPosition = I;
                    }

                    _cursorShown = _editable;
                    _blinkCount = 0;
                    Drawn = false;
                }

                RaiseMouseDown(this, button, shift, X, Y);
            }
        }
        else
        {
            if (_cursorShown)
            {
                _cursorShown = false;
                if (OnExit != null)
                {
                    OnExit(this);
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
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // AoWEdit.pas:273
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (Visible && X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            if (_editable == true)
            {
                Window._cursor = -4;
            }

            RaiseMouseMove(this, shift, X, Y);
        }
    }

    // AoWEdit.pas:289
    public override void CheckKeyPress(ref byte key)
    {
        byte C = default;
        bool scroll;
        Handled = Redraw;
        scroll = true;
        if (_editable && Redraw == true && Valid == true && Visible == true)
        {
            _cursorShown = true;
            switch (key)
            {
                case 8:
                    if (_cursorPosition > 0)
                    {
                        _text = Copy(_text, 1, _cursorPosition + FirstChar - 2) + Copy(_text, _cursorPosition + FirstChar, _text.Length - _cursorPosition - FirstChar + 1);
                        _cursorPosition = _cursorPosition + FirstChar - 2;
                    }

                    FirstChar = 1;
                    break;
                case 13:
                    _cursorShown = false;
                    Redraw = false;
                    if (OnReturn != null)
                    {
                        OnReturn(this);
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
                        _text = Copy(_text, 1, FirstChar + _cursorPosition - 1) + Cp1252.Text(C) + Copy(_text, FirstChar + _cursorPosition, _text.Length - _cursorPosition - FirstChar + 1);
                        _cursorPosition += 1;
                        _cursorPosition = _cursorPosition + FirstChar - 1;
                        FirstChar = 1;
                    }

                    break;
            }

            if (scroll == true)
            {
                while (_font.GetTextWidth(Copy(_text, FirstChar, _text.Length)) > WinWidth - 2 * _borderWidth - 1)
                {
                    FirstChar += 1;
                    _cursorPosition -= 1;
                }

                _shownChars = _text.Length - FirstChar + 1;
                if (_cursorPosition < 0)
                {
                    _cursorPosition = 0;
                }
            }

            _blinkCount = 0;
            Drawn = false;
            RaiseKeyPress(this, ref key);

            RaiseChange(this);
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
    public byte PassWordChar { get => _passWordChar; set => _passWordChar = value; }

    // AoWEdit.pas:350. Clipboard integration has no host in this port.
    public override void CheckKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        Handled = Redraw;
        if (!_editable || !Redraw || !Valid || !Visible) return;
        switch (key)
        {
            case 37:
                if (_cursorPosition > 0) _cursorPosition--;
                else if (FirstChar > 1) FirstChar--;
                break;
            case 39:
                if (_shownChars > _cursorPosition) _cursorPosition++;
                else if (FirstChar + _shownChars < _text.Length) FirstChar++;
                break;
            case 46:
                if (_cursorPosition < _text.Length)
                {
                    _text = Copy(_text, 1, _cursorPosition + FirstChar - 1) +
                        Copy(_text, _cursorPosition + FirstChar + 1, _text.Length - _cursorPosition - FirstChar);
                    _cursorPosition += FirstChar - 1;
                    FirstChar = 1;
                }
                break;
        }
        _shownChars = _text.Length - FirstChar + 1;
        _cursorPosition = Math.Max(_cursorPosition, 0);
        _cursorShown = true;
        _blinkCount = 0;
        Drawn = false;
        RaiseKeyDown(this, ref key, shift);
        RaiseChange(this);
    }

    public new event NotifyEvent<Edit> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<Edit> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<Edit> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<Edit> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<Edit> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<Edit> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Edit> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Edit> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Edit> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(OnUpdate), value: value);
    }
}
