// Translated from aowUserDrawBox.pas.
using Aow.Game.Draw;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// { The aowUserDrawBox unit of aowInt.dpl: UserDrawBox, an empty control that its owner draws
//   in OnPreDraw and OnDraw; its mouse events get coordinates relative to the control. }
// no fields of its own
public partial class UserDrawBox : Control
{
    public UserDrawBox(Component owner) : base(owner)
    { }

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
                RaisePreDraw(this, lockedSurface);

                RaiseDraw(this, lockedSurface);
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // aowUserDrawBox.pas:55
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseDown(this, button, shift, X - Window.WinLeft - WinLeft, Y - Window.WinTop - WinTop);
        }
    }

    // aowUserDrawBox.pas:69
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // aowUserDrawBox.pas:81
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseMove(this, shift, X - Window.WinLeft - WinLeft, Y - Window.WinTop - WinTop);
        }
    }

    // aowUserDrawBox.pas:94
    public override void CheckKeyPress(ref byte key)
    {
        Handled = false;
    }

    public new event NotifyEvent<UserDrawBox> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<UserDrawBox> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<UserDrawBox> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<UserDrawBox> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<UserDrawBox> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<UserDrawBox> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<UserDrawBox> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<UserDrawBox> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<UserDrawBox> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, UserDrawBox>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, UserDrawBox>(this, slot: nameof(OnUpdate), value: value);
    }
}
