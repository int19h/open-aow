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

    public new event NotifyEvent<UserDrawBox> Changed
    {
        add => ((UIComponent)this).Changed += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(Changed), value: value);
        remove => ((UIComponent)this).Changed -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(Changed), value: value);
    }

    public new event MouseEvent<UserDrawBox> MousePressed
    {
        add => ((UIComponent)this).MousePressed += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(MousePressed), value: value);
        remove => ((UIComponent)this).MousePressed -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(MousePressed), value: value);
    }

    public new event MouseEvent<UserDrawBox> MouseReleased
    {
        add => ((UIComponent)this).MouseReleased += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(MouseReleased), value: value);
        remove => ((UIComponent)this).MouseReleased -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(MouseReleased), value: value);
    }

    public new event MouseMoveEvent<UserDrawBox> MouseMoved
    {
        add => ((UIComponent)this).MouseMoved += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(MouseMoved), value: value);
        remove => ((UIComponent)this).MouseMoved -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(MouseMoved), value: value);
    }

    public new event KeyPressEvent<UserDrawBox> CharacterTyped
    {
        add => ((UIComponent)this).CharacterTyped += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(CharacterTyped), value: value);
        remove => ((UIComponent)this).CharacterTyped -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(CharacterTyped), value: value);
    }

    public new event KeyEvent<UserDrawBox> KeyPressed
    {
        add => ((UIComponent)this).KeyPressed += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(KeyPressed), value: value);
        remove => ((UIComponent)this).KeyPressed -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(KeyPressed), value: value);
    }

    public new event AoWOnDrawEvent<UserDrawBox> Drawing
    {
        add => ((UIComponent)this).Drawing += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(Drawing), value: value);
        remove => ((UIComponent)this).Drawing -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(Drawing), value: value);
    }

    public new event AoWOnDrawEvent<UserDrawBox> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(Drawn), value: value);
    }

    public new event NotifyEvent<UserDrawBox> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(Updated), value: value);
    }
}
