#nullable disable warnings
// Translated from AoWButton.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// Fields: OnClick
public sealed partial class Button(Component? owner) : BaseButton(owner)
{
    [Published]
    public event NotifyEvent<Button>? Click;

    internal bool HasClickHandlers => Click is not null;

    internal void RaiseClick(Button sender) => Click?.Invoke(sender);

    // AoWButton.pas:239
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        Lit = false;
        if (!Window.Handled)
        {
            if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
            {
                Handled = true;
                if (button == MouseButton.Left)
                {
                    IsMouseDown = true;
                    Checked = true;
                    Drawn = false;
                    Redraw = true;
                }

                RaiseMouseDown(this, button, shift, X, Y);
            }
        }
    }

    // AoWButton.pas:261
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        Checked = false;
        Lit = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            if (button == MouseButton.Left)
            {
                if (IsMouseDown && Click is not null)
                {
                    Click(this);
                }

                Drawn = false;
            }

            RaiseMouseUp(this, button, shift, X, Y);
        }

        if (IsMouseDown)
        {
            Drawn = false;
        }

        IsMouseDown = false;
    }

    // AoWButton.pas:284
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        bool oldChecked = Checked;
        bool oldLit = Lit;
        Redraw = false;
        if (Window.Handled || X < Window.AbsoluteLeft + WinLeft || X > Window.AbsoluteLeft + WinLeft + WinWidth - 1 || Y < Window.AbsoluteTop + WinTop || Y > Window.AbsoluteTop + WinTop + WinHeight - 1)
        {
            Checked = false;
            Lit = false;
        }
        else
        {
            Handled = true;
            Checked = IsMouseDown;
            Lit = !IsMouseDown;
            Redraw = true;
            RaiseMouseMove(this, shift, X, Y);
        }

        if (oldLit != Lit || oldChecked != Checked)
        {
            Drawn = false;
        }
    }

    public new event NotifyEvent<Button> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, Button>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<Button> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, Button>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<Button> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, Button>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<Button> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, Button>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<Button> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, Button>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<Button> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, Button>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Button> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, Button>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Button> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Button>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Button> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, Button>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(Updated), value: value);
    }
}
