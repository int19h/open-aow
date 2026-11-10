using Aow.Game.Ui.Controls;
using Aow.Rtl;

namespace Aow.Game.Ui.Controls;

public partial class AOWListBox
{
    public override bool ContainsPoint(int X, int Y)
    {
        if (FAOWWindow == null) return false;
        return PointInside(X, Y, FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth - F138, FWinHeight);
    }

    // A linked bar with room to move scrolls even when it is hidden, so shared columns stay together.
    // A bar with no room, or no bar when every item fits, leaves the wheel for the parent.
    public override bool CheckMouseWheel(int Steps, Set<ShiftState> Shift, int X, int Y)
    {
        if (Steps == 0 || !ContainsPoint(X, Y)) return false;
        AOWVScrollBar bar = WheelScrollBar ?? FVScrollBar;
        if (bar != null)
        {
            if (!bar.HasWheelRange()) return false;
            bar.ScrollSteps(Steps);
            return true;
        }

        int limit = FItems.Count - F11C;
        if (limit < 0) limit = 0;
        if (limit == 0) return false;
        int next = F15C - Steps;
        if (next < 0) next = 0;
        if (next > limit) next = limit;
        if (next != F15C)
        {
            F15C = next;
            FDrawn = false;
        }

        return true;
    }
}
