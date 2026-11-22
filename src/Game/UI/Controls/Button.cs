#nullable disable warnings
// Translated from AoWButton.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// Fields: OnClick
public sealed partial class Button(Component? owner) : BaseButton(owner)
{
    [Published]
    public event NotifyEvent<Button>? Clicked;

    internal bool HasClickHandlers => Clicked is not null;

    internal void RaiseClick(Button sender) => Clicked?.Invoke(sender);

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
                    IsDrawn = false;
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
                if (IsMouseDown && Clicked is not null)
                {
                    Clicked(this);
                }

                IsDrawn = false;
            }

            RaiseMouseUp(this, button, shift, X, Y);
        }

        if (IsMouseDown)
        {
            IsDrawn = false;
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
            IsDrawn = false;
        }
    }

    public new event NotifyEvent<Button> Changed
    {
        add => ((UIComponent)this).Changed += EventSlot.Add<UIComponent, Button>(this, slot: nameof(Changed), value: value);
        remove => ((UIComponent)this).Changed -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(Changed), value: value);
    }

    public new event MouseEvent<Button> MousePressed
    {
        add => ((UIComponent)this).MousePressed += EventSlot.Add<UIComponent, Button>(this, slot: nameof(MousePressed), value: value);
        remove => ((UIComponent)this).MousePressed -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(MousePressed), value: value);
    }

    public new event MouseEvent<Button> MouseReleased
    {
        add => ((UIComponent)this).MouseReleased += EventSlot.Add<UIComponent, Button>(this, slot: nameof(MouseReleased), value: value);
        remove => ((UIComponent)this).MouseReleased -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(MouseReleased), value: value);
    }

    public new event MouseMoveEvent<Button> MouseMoved
    {
        add => ((UIComponent)this).MouseMoved += EventSlot.Add<UIComponent, Button>(this, slot: nameof(MouseMoved), value: value);
        remove => ((UIComponent)this).MouseMoved -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(MouseMoved), value: value);
    }

    public new event KeyPressEvent<Button> CharacterTyped
    {
        add => ((UIComponent)this).CharacterTyped += EventSlot.Add<UIComponent, Button>(this, slot: nameof(CharacterTyped), value: value);
        remove => ((UIComponent)this).CharacterTyped -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(CharacterTyped), value: value);
    }

    public new event KeyEvent<Button> KeyPressed
    {
        add => ((UIComponent)this).KeyPressed += EventSlot.Add<UIComponent, Button>(this, slot: nameof(KeyPressed), value: value);
        remove => ((UIComponent)this).KeyPressed -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(KeyPressed), value: value);
    }

    public new event AoWOnDrawEvent<Button> Drawing
    {
        add => ((UIComponent)this).Drawing += EventSlot.Add<UIComponent, Button>(this, slot: nameof(Drawing), value: value);
        remove => ((UIComponent)this).Drawing -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(Drawing), value: value);
    }

    public new event AoWOnDrawEvent<Button> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, Button>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(Drawn), value: value);
    }

    public new event NotifyEvent<Button> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, Button>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, Button>(this, slot: nameof(Updated), value: value);
    }
}
