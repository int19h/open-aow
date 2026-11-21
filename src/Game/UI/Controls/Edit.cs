#nullable disable
// Translated from AoWEdit.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.RtlHost;

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
public sealed partial class Edit(Component owner) : Control(owner)
{

    private bool _cursorShown = true;
    private int _blinkCount;
    private int _shownChars;

    public int FirstChar { get; set; }

    private InterfaceImageLibrary _imageLibraryEdit;
    [Published]
    public event NotifyEvent<Edit> Exit;
    [Published]
    public event NotifyEvent<Edit> Return;
    private int _imageLibraryIndex;
    private int _cursorPosition;

    // AoWEdit.pas:87
    public override void Validate()
    {
        if (_imageLibraryEdit is not null)
        {
            _imageLibraryEdit = _imageLibraryEdit.CheckImageLibrary(_imageLibraryIndex, count: 3, sender: this);
        }

        if (_imageLibraryEdit is not null && Font is not null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // AoWEdit.pas:96
    public override void SetSize()
    {
        LibraryImage imageLibraryEditWidth = _imageLibraryEdit[_imageLibraryIndex];
        LibraryImage imageLibraryEdit = _imageLibraryEdit[_imageLibraryIndex + 2];
        if (imageLibraryEditWidth.Width + imageLibraryEdit.Width > WinWidth)
        {
            LibraryImage imageLibraryEdit2 = _imageLibraryEdit[_imageLibraryIndex];
            LibraryImage imageLibraryEdit3 = _imageLibraryEdit[_imageLibraryIndex + 2];
            SetWinWidthWithoutInvalidate(imageLibraryEdit2.Width + imageLibraryEdit3.Width);
        }

        SetWinHeightWithoutInvalidate(_imageLibraryEdit[_imageLibraryIndex].Height);
        FirstChar = 1;
        if (base.Text.Length < _cursorPosition)
        {
            _cursorPosition = base.Text.Length;
        }

        if (Font.GetTextWidth(Copy(base.Text, index: FirstChar, count: base.Text.Length - FirstChar)) > WinWidth - (2 * BorderWidth) - 1)
        {
            while (Font.GetTextWidth(Copy(base.Text, index: FirstChar, count: base.Text.Length)) > WinWidth - (2 * BorderWidth) - 1)
            {
                FirstChar += 1;
            }
        }

        _shownChars = base.Text.Length - FirstChar + 1;
        base.SetSize();
    }

    // AoWEdit.pas:137
    public override void Update()
    {
        base.Update();
        if (Visible && Editable && BlinkDelay > 0 && Redraw)
        {
            _blinkCount += 1;
            if (_blinkCount > BlinkDelay)
            {
                Drawn = false;
                _blinkCount = 0;
                _cursorShown = !_cursorShown;
            }
        }
    }

    // AoWEdit.pas:155
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(this, surfDesc: lockedSurface);

                SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                _imageLibraryEdit.DrawHorizontalStrip(_imageLibraryIndex, area: rectangle, ref lockedSurface);
                Rect oldClip = lockedSurface.ClipRectangle;
                LibraryImage imageLibraryEditWidth = _imageLibraryEdit[_imageLibraryIndex + 2];
                if (Window.AbsoluteLeft + WinLeft + WinWidth - imageLibraryEditWidth.Width < oldClip.Right)
                {
                    LibraryImage imageLibraryEdit = _imageLibraryEdit[_imageLibraryIndex + 2];
                    Rect clipRectangle = lockedSurface.ClipRectangle;
                    clipRectangle.Right = Window.AbsoluteLeft + WinLeft + WinWidth - imageLibraryEdit.Width;
                    lockedSurface.ClipRectangle = clipRectangle;
                }

                Font.Color = Color;
                if (PassWordChar == 0)
                {
                    Font.SetText(Window.AbsoluteLeft + WinLeft + BorderWidth, Window.AbsoluteTop + WinTop + BorderHeight, text: Copy(base.Text, index: FirstChar, count: _shownChars), lockedSurface);
                }
                else
                {
                    string s = "";
                    for (int i = 0, last = _shownChars - 1; i <= last; i++)
                    {
                        s += Cp1252.Text(PassWordChar);
                    }

                    Font.SetText(Window.AbsoluteLeft + WinLeft + BorderWidth, Window.AbsoluteTop + WinTop + BorderHeight, text: s, lockedSurface);
                }

                lockedSurface.ClipRectangle = oldClip;
                if (Editable && Redraw && _cursorShown)
                {
                    int textWidth = Font.GetTextWidth(Copy(base.Text, index: FirstChar, count: _cursorPosition));
                    Font.SetText(Window.AbsoluteLeft + WinLeft + BorderWidth + textWidth, Window.AbsoluteTop + WinTop + BorderHeight, text: Cp1252.Text(CursorChar), lockedSurface);
                }

                RaiseDraw(this, surfDesc: lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // AoWEdit.pas:197
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        IsMouseDown = false;
        bool oldRedraw = Redraw;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            if (Valid && Visible)
            {
                if (button == MouseButton.Left)
                {
                    if (Editable)
                    {
                        Redraw = true;
                    }

                    int textWidth = Font.GetTextWidth(Copy(base.Text, index: FirstChar, count: base.Text.Length));
                    if (X > Window.AbsoluteLeft + WinLeft + BorderWidth + textWidth)
                    {
                        FirstChar = 1;
                        if (Font.GetTextWidth(Copy(base.Text, index: FirstChar, count: base.Text.Length - FirstChar)) > WinWidth - (2 * BorderWidth) - 1)
                        {
                            while (Font.GetTextWidth(Copy(base.Text, index: FirstChar, count: base.Text.Length)) > WinWidth - (2 * BorderWidth) - 1)
                            {
                                FirstChar += 1;
                            }
                        }

                        _cursorPosition = base.Text.Length - FirstChar + 1;
                    }
                    else
                    {
                        int i = 0;
                        int w = Font.GetTextWidth(Copy(base.Text, index: FirstChar, count: i + 1));
                        while (X > Window.AbsoluteLeft + WinLeft + BorderWidth + w && base.Text.Length > i + FirstChar - 1)
                        {
                            i += 1;
                            w = Font.GetTextWidth(Copy(base.Text, index: FirstChar, count: i + 1));
                        }

                        _cursorPosition = i;
                    }

                    _cursorShown = Editable;
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
                Exit?.Invoke(this);
            }

            Redraw = false;
        }

        if (Redraw != oldRedraw)
        {
            Drawn = false;
        }
    }

    // AoWEdit.pas:261
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // AoWEdit.pas:273
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (Visible && X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            if (Editable)
            {
                Window.Cursor = -4;
            }

            RaiseMouseMove(this, shift, X, Y);
        }
    }

    // AoWEdit.pas:289
    public override void CheckKeyPress(ref byte key)
    {
        Handled = Redraw;
        if (Editable && Redraw && Valid && Visible)
        {
            _cursorShown = true;
            switch (key)
            {
                case 8:
                    if (_cursorPosition > 0)
                    {
                        base.Text = Copy(base.Text, index: 1, count: _cursorPosition + FirstChar - 2) + Copy(base.Text, index: _cursorPosition + FirstChar, count: base.Text.Length - _cursorPosition - FirstChar + 1);
                        _cursorPosition = _cursorPosition + FirstChar - 2;
                    }

                    FirstChar = 1;
                    break;
                case 13:
                    _cursorShown = false;
                    Redraw = false;
                    Return?.Invoke(this);

                    break;
                case >= 0 and <= 7:
                case >= 9 and <= 12:
                case >= 14 and <= 31:
                    break;
                default:
                    byte c = key;
                    if (base.Text.Length < MaxChars || MaxChars == 0)
                    {
                        base.Text = Copy(base.Text, index: 1, count: FirstChar + _cursorPosition - 1) + Cp1252.Text(c) + Copy(base.Text, index: FirstChar + _cursorPosition, count: base.Text.Length - _cursorPosition - FirstChar + 1);
                        _cursorPosition += 1;
                        _cursorPosition = _cursorPosition + FirstChar - 1;
                        FirstChar = 1;
                    }

                    break;
            }

            bool scroll = true;
            if (scroll)
            {
                while (Font.GetTextWidth(Copy(base.Text, index: FirstChar, count: base.Text.Length)) > WinWidth - (2 * BorderWidth) - 1)
                {
                    FirstChar += 1;
                    _cursorPosition -= 1;
                }

                _shownChars = base.Text.Length - FirstChar + 1;
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
    public void SetImageLibraryEdit(InterfaceImageLibrary value)
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
        base.Text = value;
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
    public override string Text { get => base.Text; set => SetGText(value); }

    [Published]
    public int MaxChars { get; set; }

    [Published]
    public byte CursorChar { get; set; } = 124;

    [Published]
    public int BlinkDelay { get; set; }

    [Published]
    public bool Editable { get; set; } = true;

    [Published]
    public InterfaceImageLibrary ImageLibraryEdit { get => _imageLibraryEdit; set => SetImageLibraryEdit(value); }

    [Published]
    public int ImageLibraryIndex { get => _imageLibraryIndex; set => SetImageLibraryIndex(value); }

    [Published]
    public byte PassWordChar { get; set; }

    // AoWEdit.pas:350. Clipboard integration has no host in this port.
    public override void CheckKeyDown(ref ushort key, ShiftStates shift)
    {
        Handled = Redraw;
        if (!Editable || !Redraw || !Valid || !Visible)
        {
            return;
        }

        switch (key)
        {
            case 37:
                if (_cursorPosition > 0)
                {
                    _cursorPosition--;
                }
                else if (FirstChar > 1)
                {
                    FirstChar--;
                }

                break;
            case 39:
                if (_shownChars > _cursorPosition)
                {
                    _cursorPosition++;
                }
                else if (FirstChar + _shownChars < base.Text.Length)
                {
                    FirstChar++;
                }

                break;
            case 46:
                if (_cursorPosition < base.Text.Length)
                {
                    base.Text = Copy(base.Text, index: 1, count: _cursorPosition + FirstChar - 1) +
                        Copy(base.Text, index: _cursorPosition + FirstChar + 1, count: base.Text.Length - _cursorPosition - FirstChar);
                    _cursorPosition += FirstChar - 1;
                    FirstChar = 1;
                }
                break;
            default:
                break;
        }
        _shownChars = base.Text.Length - FirstChar + 1;
        _cursorPosition = Math.Max(_cursorPosition, 0);
        _cursorShown = true;
        _blinkCount = 0;
        Drawn = false;
        RaiseKeyDown(this, ref key, shift);
        RaiseChange(this);
    }

    public new event NotifyEvent<Edit> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<Edit> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<Edit> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<Edit> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<Edit> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<Edit> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Edit> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Edit> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Edit> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, Edit>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, Edit>(this, slot: nameof(Updated), value: value);
    }
}
