using Aow.Game.UI.Controls;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

public partial class Window
{
    public override bool ContainsPoint(int X, int Y) => PointInside(X, Y, WinLeft, WinTop, WinWidth, WinHeight);

    // A book starts a page animation from the button handler. One wheel event turns one page, so a
    // second step in the same event does not restart that animation.
    protected virtual bool OneWheelTurnPerEvent => false;

    // Children first. A child that handles the wheel stops it. Otherwise the page buttons, then
    // one vertical scroll bar. Containing the pointer does not by itself consume the wheel.
    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0 || _windowManager == null || !Visible || !ContainsPoint(X, Y)) return false;
        var child = TopWheelChild(X, Y);
        if (child != null && child.CheckMouseWheel(steps, shift, X, Y)) return true;
        if (TurnPages(steps)) return true;
        return ScrollOwnedBar(steps);
    }

    private bool TurnPages(int steps)
    {
        Button button = steps > 0 ? PageBack : PageForward;
        if (button == null) return false;
        int count = steps < 0 ? -steps : steps;
        bool turned = false;
        for (int i = 0; i < count; i++)
        {
            if (!button.Visible || !button.CaptureInput || !button.HasClickHandlers) break;
            button.RaiseClick(button);
            turned = true;
            if (OneWheelTurnPerEvent) break;
        }

        return turned;
    }
}
