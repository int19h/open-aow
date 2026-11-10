using Aow.Game.Ui.Controls;
using Aow.Rtl;

namespace Aow.Game.Ui.Windows;

public partial class AOWWindow
{
    public override bool ContainsPoint(int X, int Y) => PointInside(X, Y, FWinLeft, FWinTop, FWinWidth, FWinHeight);

    // A book starts a page animation from the button handler. One wheel event turns one page, so a
    // second step in the same event does not restart that animation.
    protected virtual bool OneWheelTurnPerEvent => false;

    // Children first. A child that handles the wheel stops it. Otherwise the page buttons, then
    // one vertical scroll bar. Containing the pointer does not by itself consume the wheel.
    public override bool CheckMouseWheel(int Steps, Set<ShiftState> Shift, int X, int Y)
    {
        if (Steps == 0 || FWinManager == null || !FVisible || !ContainsPoint(X, Y)) return false;
        var child = TopWheelChild(X, Y);
        if (child != null && child.CheckMouseWheel(Steps, Shift, X, Y)) return true;
        if (TurnPages(Steps)) return true;
        return ScrollOwnedBar(Steps);
    }

    private bool TurnPages(int Steps)
    {
        AOWButton button = Steps > 0 ? PageBack : PageForward;
        if (button == null) return false;
        int count = Steps < 0 ? -Steps : Steps;
        bool turned = false;
        for (int i = 0; i < count; i++)
        {
            if (!button.FVisible || !button.FCaptureInput || button.FOnClick == null) break;
            button.FOnClick(button);
            turned = true;
            if (OneWheelTurnPerEvent) break;
        }

        return turned;
    }
}
