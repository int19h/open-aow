#nullable disable warnings
// Translated from AoWButton.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// Fields: OnClick
public sealed partial class Button(Component? owner) : BaseButton(owner)
{
    [Published]
    public event EventHandler? Clicked;

    internal bool HasClickHandlers => Clicked is not null;

    internal void RaiseClick() => Clicked?.Invoke(this, e: EventArgs.Empty);

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

                RaiseMouseDown(button, shift, X, Y);
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
                    Clicked(this, e: EventArgs.Empty);
                }

                IsDrawn = false;
            }

            RaiseMouseUp(button, shift, X, Y);
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
            RaiseMouseMove(shift, X, Y);
        }

        if (oldLit != Lit || oldChecked != Checked)
        {
            IsDrawn = false;
        }
    }
}
