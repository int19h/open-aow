#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from aowUserDrawBox.pas.
using Aow.Game.Draw;

using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Controls;

// { The aowUserDrawBox unit of aowInt.dpl: UserDrawBox, an empty control that its owner draws
//   in OnPreDraw and OnDraw; its mouse events get coordinates relative to the control. }
// no fields of its own
public sealed class UserDrawBox(Component owner) : Control(owner)
{

    // aowUserDrawBox.pas:34
    public override void Validate()
    {
        Valid = true;
        base.Validate();
    }

    // aowUserDrawBox.pas:40
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(this, surfDesc: lockedSurface);

                RaiseDraw(this, surfDesc: lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // aowUserDrawBox.pas:55
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseDown(this, button, shift, X - Window.AbsoluteLeft - WinLeft, Y - Window.AbsoluteTop - WinTop);
        }
    }

    // aowUserDrawBox.pas:69
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // aowUserDrawBox.pas:81
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseMove(this, shift, X - Window.AbsoluteLeft - WinLeft, Y - Window.AbsoluteTop - WinTop);
        }
    }

    // aowUserDrawBox.pas:94
    public override void CheckKeyPress(ref byte key) => Handled = false;

    public new event NotifyEvent<UserDrawBox> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<UserDrawBox> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<UserDrawBox> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<UserDrawBox> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<UserDrawBox> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<UserDrawBox> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<UserDrawBox> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<UserDrawBox> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<UserDrawBox> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(Updated), value: value);
    }
}
