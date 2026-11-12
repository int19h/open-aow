// Translated from AoWButton.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// Fields: _onClick
public partial class Button : BaseButton
{
    public Button(Component owner) : base(owner)
    { }

    private NotifyEvent<Button> _onClick;

    // AoWButton.pas:239
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        Lit = false;
        if (!Window.Handled)
        {
            if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
            {
                Handled = true;
                if (button == MouseButton.Left)
                {
                    MouseDown = true;
                    _checked = true;
                    Drawn = false;
                    Redraw = true;
                }

                if (_onMouseDown != null)
                {
                    _onMouseDown(this, button, shift, X, Y);
                }
            }
        }
    }

    // AoWButton.pas:261
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        _checked = false;
        Lit = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            if (button == MouseButton.Left)
            {
                if (MouseDown && _onClick != null)
                {
                    _onClick(this);
                }

                Drawn = false;
            }

            if (_onMouseUp != null)
            {
                _onMouseUp(this, button, shift, X, Y);
            }
        }

        if (MouseDown)
        {
            Drawn = false;
        }

        MouseDown = false;
    }

    // AoWButton.pas:284
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        bool oldChecked;
        bool oldLit;
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        oldChecked = _checked;
        oldLit = Lit;
        Redraw = false;
        if (Window.Handled || X < Window.WinLeft + WinLeft || X > Window.WinLeft + WinLeft + WinWidth - 1 || Y < Window.WinTop + WinTop || Y > Window.WinTop + WinTop + WinHeight - 1)
        {
            _checked = false;
            Lit = false;
        }
        else
        {
            Handled = true;
            _checked = MouseDown;
            Lit = !MouseDown;
            Redraw = true;
            if (_onMouseMove != null)
            {
                _onMouseMove(this, shift, X, Y);
            }
        }

        if (oldLit != Lit || oldChecked != _checked)
        {
            Drawn = false;
        }
    }

    [Published]
    public NotifyEvent<Button> OnClick { get => _onClick; set => _onClick = value; }

    public new NotifyEvent<Button> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<Button> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<Button> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<Button> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<Button> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<Button> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<Button> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<Button> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<Button> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
