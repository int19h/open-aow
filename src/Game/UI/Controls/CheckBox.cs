#nullable disable warnings
// Translated from AoWCheckBox.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// { The AoWCheckBox unit of aowInt.dpl: TAOWCheckBox, a button that toggles its down state
//   (Checked) on a left click; with a RadioIndex above 0 it is a radio button, which only turns
//   on, and its window turns off the other radio buttons of the group (RadioChange). A right
//   click opens its PopupMenu at the mouse, kept inside the window manager. }
// Fields: OnDoubleClick
public sealed partial class CheckBox(Component? owner) : BaseButton(owner)
{
    [Published]
    public event NotifyEvent<CheckBox>? DoubleClicked;

    // AoWCheckBox.pas:51
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        Lit = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight && Valid && Visible)
        {
            Handled = true;
            RaiseMouseDown(this, button, shift, X, Y);

            if (button == MouseButton.Left)
            {
                if (RadioIndex > 0)
                {
                    if (!base.Checked)
                    {
                        base.Checked = true;
                        RaiseChange(this);

                        base.Checked = !base.Checked;
                        Checked = !base.Checked;
                    }
                }
                else
                {
                    base.Checked = !base.Checked;
                    RaiseChange(this);

                    base.Checked = !base.Checked;
                    Checked = !base.Checked;
                }

                if ((shift & ShiftStates.Double) == ShiftStates.Double)
                {
                    DoubleClicked?.Invoke(this);
                }
            }
            else
            {
                if (button == MouseButton.Right && PopupMenu is not null)
                {
                    PopupMenu.WinLeft = X;
                    PopupMenu.WinTop = Y;
                    PopupMenu.SetPopupOwner(this);
                    if (PopupMenu.WindowManager is not null)
                    {
                        if (PopupMenu.WinLeft + PopupMenu.WinWidth > PopupMenu.WindowManager.WinWidth)
                        {
                            PopupMenu.SetWinLeftWithoutInvalidate(X - PopupMenu.WinWidth);
                        }

                        if (PopupMenu.WinTop + PopupMenu.WinHeight > PopupMenu.WindowManager.WinHeight)
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
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        Lit = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight && Valid && Visible)
        {
            Handled = true;
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // AoWCheckBox.pas:119
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        bool oldLit = Lit;
        if (X < Window.AbsoluteLeft + WinLeft || X > Window.AbsoluteLeft + WinLeft + WinWidth - 1 || Y < Window.AbsoluteTop + WinTop || Y > Window.AbsoluteTop + WinTop + WinHeight - 1)
        {
            Lit = false;
        }
        else
        {
            Handled = true;
            Lit = !base.Checked && !OwnerComponent.Handled;
            RaiseMouseMove(this, shift, X, Y);
        }

        Redraw = Lit;
        if (oldLit != Lit)
        {
            IsDrawn = false;
        }
    }

    // AoWCheckBox.pas:38
    public void SetChecked(bool value)
    {
        if (value != base.Checked)
        {
            base.Checked = value;
            IsDrawn = false;
            if (RadioIndex > 0)
            {
                if (Window is not null)
                {
                    if (base.Checked)
                    {
                        Window.RadioChange(this);
                    }
                }
            }
        }
    }

    [Published]
    public override bool Checked { get => base.Checked; set => SetChecked(value); }

    public new event NotifyEvent<CheckBox> Changed
    {
        add => ((UIComponent)this).Changed += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(Changed), value: value);
        remove => ((UIComponent)this).Changed -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(Changed), value: value);
    }

    public new event MouseEvent<CheckBox> MousePressed
    {
        add => ((UIComponent)this).MousePressed += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(MousePressed), value: value);
        remove => ((UIComponent)this).MousePressed -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(MousePressed), value: value);
    }

    public new event MouseEvent<CheckBox> MouseReleased
    {
        add => ((UIComponent)this).MouseReleased += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(MouseReleased), value: value);
        remove => ((UIComponent)this).MouseReleased -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(MouseReleased), value: value);
    }

    public new event MouseMoveEvent<CheckBox> MouseMoved
    {
        add => ((UIComponent)this).MouseMoved += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(MouseMoved), value: value);
        remove => ((UIComponent)this).MouseMoved -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(MouseMoved), value: value);
    }

    public new event KeyPressEvent<CheckBox> CharacterTyped
    {
        add => ((UIComponent)this).CharacterTyped += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(CharacterTyped), value: value);
        remove => ((UIComponent)this).CharacterTyped -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(CharacterTyped), value: value);
    }

    public new event KeyEvent<CheckBox> KeyPressed
    {
        add => ((UIComponent)this).KeyPressed += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(KeyPressed), value: value);
        remove => ((UIComponent)this).KeyPressed -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(KeyPressed), value: value);
    }

    public new event AoWOnDrawEvent<CheckBox> Drawing
    {
        add => ((UIComponent)this).Drawing += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(Drawing), value: value);
        remove => ((UIComponent)this).Drawing -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(Drawing), value: value);
    }

    public new event AoWOnDrawEvent<CheckBox> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(Drawn), value: value);
    }

    public new event NotifyEvent<CheckBox> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, CheckBox>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, CheckBox>(this, slot: nameof(Updated), value: value);
    }
}
