// Translated from AoWLabel.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// { The AoWLabel unit of aowInt.dpl: TAOWLabel, a line of text in an image library font; it
//   sizes itself to its text. }
// no fields of its own
public partial class Label : Control
{
    public Label(Component owner) : base(owner)
    { }

    // AoWLabel.pas:38
    public override void Validate()
    {
        if (_font != null)
        {
            Valid = true;
        }

        _masked = true;
        base.Validate();
    }

    // AoWLabel.pas:46
    public override void SetSize()
    {
        _winWidth = _font.GetTextWidth(_text);
        _winHeight = _font.GetTextHeight(_text);
        base.SetSize();
    }

    // AoWLabel.pas:72
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && _visible)
            {
                if (_onPreDraw != null)
                {
                    _onPreDraw(this, lockedSurface);
                }

                _font.Color = Color;
                _font.SetText(_window._winLeft + _winLeft, _window._winTop + _winTop, _text, lockedSurface);
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

    // AoWLabel.pas:90
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = _captureInput;
            if (_onMouseDown != null)
            {
                _onMouseDown(this, button, shift, X, Y);
            }
        }
    }

    // AoWLabel.pas:103
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = _captureInput;
            if (_onMouseUp != null)
            {
                _onMouseUp(this, button, shift, X, Y);
            }
        }
    }

    // AoWLabel.pas:115
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = _captureInput;
            if (_onMouseMove != null)
            {
                _onMouseMove(this, shift, X, Y);
            }
        }
    }

    // AoWLabel.pas:128
    public override void CheckKeyPress(ref byte key)
    {
        Handled = false;
    }

    // AoWLabel.pas:53
    public void SetGFont(ImageLibraryFont value)
    {
        if (value != _font)
        {
            _font = value;
            Valid = false;
        }
    }

    // AoWLabel.pas:62
    public void SetGText(string value)
    {
        if (_text != value)
        {
            _text = value;
            Sized = false;
            Aligned = false;
        }
    }

    [Published]
    public ImageLibraryFont Font { get => _font; set => SetGFont(value); }

    [Published]
    public string Text { get => _text; set => SetGText(value); }

    public new NotifyEvent<Label> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<Label> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<Label> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<Label> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<Label> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<Label> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<Label> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<Label> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<Label> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
