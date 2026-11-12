// Translated from AoWCheckBox.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// { The AoWCheckBox unit of aowInt.dpl: TAOWCheckBox, a button that toggles its down state
//   (Checked) on a left click; with a RadioIndex above 0 it is a radio button, which only turns
//   on, and its window turns off the other radio buttons of the group (RadioChange). A right
//   click opens its PopupMenu at the mouse, kept inside the window manager. }
// Fields: _onDoubleClick
public partial class CheckBox : BaseButton
{
    public CheckBox(Component owner) : base(owner)
    { }

    private NotifyEvent<CheckBox> _onDoubleClick;

    // AoWCheckBox.pas:51
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        Lit = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight && Valid == true && Visible == true)
        {
            Handled = true;
            if (_onMouseDown != null)
            {
                _onMouseDown(this, button, shift, X, Y);
            }

            if (button == MouseButton.Left)
            {
                if (_radioIndex > 0)
                {
                    if (!_checked)
                    {
                        _checked = true;
                        if (_onChange != null)
                        {
                            _onChange(this);
                        }

                        _checked = !_checked;
                        Checked = !_checked;
                    }
                }
                else
                {
                    _checked = !_checked;
                    if (_onChange != null)
                    {
                        _onChange(this);
                    }

                    _checked = !_checked;
                    Checked = !_checked;
                }

                if ((shift & Set<ShiftState>.Of(ShiftState.Double)) == Set<ShiftState>.Of(ShiftState.Double))
                {
                    if (_onDoubleClick != null)
                    {
                        _onDoubleClick(this);
                    }
                }
            }
            else
            {
                if (button == MouseButton.Right && PopupMenu != null)
                {
                    PopupMenu.WinLeft = X;
                    PopupMenu.WinTop = Y;
                    PopupMenu.SetPopupOwner(this);
                    if (PopupMenu._windowManager != null)
                    {
                        if (PopupMenu.WinLeft + PopupMenu.WinWidth > PopupMenu._windowManager.WinWidth)
                        {
                            PopupMenu.SetWinLeftWithoutInvalidate(X - PopupMenu.WinWidth);
                        }

                        if (PopupMenu.WinTop + PopupMenu.WinHeight > PopupMenu._windowManager.WinHeight)
                        {
                            PopupMenu.SetWinTopWithoutInvalidate(Y - PopupMenu.WinHeight);
                        }
                    }

                    PopupMenu.Visible = true;
                }
            }
        }
    }

    // AoWCheckBox.pas:105
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        Lit = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight && Valid == true && Visible == true)
        {
            Handled = true;
            if (_onMouseUp != null)
            {
                _onMouseUp(this, button, shift, X, Y);
            }
        }
    }

    // AoWCheckBox.pas:119
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        bool oldLit;
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        oldLit = Lit;
        if (X < Window.WinLeft + WinLeft || X > Window.WinLeft + WinLeft + WinWidth - 1 || Y < Window.WinTop + WinTop || Y > Window.WinTop + WinTop + WinHeight - 1)
        {
            Lit = false;
        }
        else
        {
            Handled = true;
            Lit = !_checked && !OwnerComponent.Handled;
            if (_onMouseMove != null)
            {
                _onMouseMove(this, shift, X, Y);
            }
        }

        Redraw = Lit;
        if (oldLit != Lit)
        {
            Drawn = false;
        }
    }

    // AoWCheckBox.pas:38
    public virtual void SetChecked(bool value)
    {
        if (value != _checked)
        {
            _checked = value;
            Drawn = false;
            if (_radioIndex > 0)
            {
                if (Window != null)
                {
                    if (_checked)
                    {
                        Window.RadioChange(this);
                    }
                }
            }
        }
    }

    [Published]
    public bool Checked { get => _checked; set => SetChecked(value); }

    [Published]
    public int RadioIndex { get => _radioIndex; set => _radioIndex = value; }

    [Published]
    public NotifyEvent<CheckBox> OnDoubleClick { get => _onDoubleClick; set => _onDoubleClick = value; }

    public new NotifyEvent<CheckBox> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<CheckBox> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<CheckBox> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<CheckBox> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<CheckBox> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<CheckBox> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<CheckBox> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<CheckBox> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<CheckBox> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
