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
                RaisePreDraw(surfDesc: lockedSurface);

                RaiseDraw(surfDesc: lockedSurface);
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
            RaiseMouseDown(button, shift, X - Window.AbsoluteLeft - WinLeft, Y - Window.AbsoluteTop - WinTop);
        }
    }

    // aowUserDrawBox.pas:69
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseUp(button, shift, X, Y);
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
            RaiseMouseMove(shift, X - Window.AbsoluteLeft - WinLeft, Y - Window.AbsoluteTop - WinTop);
        }
    }

    // aowUserDrawBox.pas:94
    public override void CheckKeyPress(ref byte key) => Handled = false;
}
