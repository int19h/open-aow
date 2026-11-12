// Translated from AoWCheckBox.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// { The AoWCheckBox unit of aowInt.dpl: TAOWCheckBox, a button that toggles its down state
//   (Checked) on a left click; with a RadioIndex above 0 it is a radio button, which only turns
//   on, and its window turns off the other radio buttons of the group (RadioChange). A right
//   click opens its PopupMenu at the mouse, kept inside the window manager. }
// Fields: OnDoubleClick
public partial class CheckBox : BaseButton
{
    public CheckBox(Component owner) : base(owner)
    { }

    [Published]
    public event NotifyEvent<CheckBox> OnDoubleClick;

    // AoWCheckBox.pas:51
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        Lit = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight && Valid == true && Visible == true)
        {
            Handled = true;
            RaiseMouseDown(this, button, shift, X, Y);

            if (button == MouseButton.Left)
            {
                if (_radioIndex > 0)
                {
                    if (!_checked)
                    {
                        _checked = true;
                        RaiseChange(this);

                        _checked = !_checked;
                        Checked = !_checked;
                    }
                }
                else
                {
                    _checked = !_checked;
                    RaiseChange(this);

                    _checked = !_checked;
                    Checked = !_checked;
                }

                if ((shift & Set<ShiftState>.Of(ShiftState.Double)) == Set<ShiftState>.Of(ShiftState.Double))
                {
                    if (OnDoubleClick != null)
                    {
                        OnDoubleClick(this);
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
            RaiseMouseUp(this, button, shift, X, Y);
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
            RaiseMouseMove(this, shift, X, Y);
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

    public new event NotifyEvent<CheckBox> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<CheckBox> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<CheckBox> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<CheckBox> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<CheckBox> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<CheckBox> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<CheckBox> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<CheckBox> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<CheckBox> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(OnUpdate), value: value);
    }
}
