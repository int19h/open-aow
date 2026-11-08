using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWVScroll;

namespace Aow.Units.AoWListBox;

public partial class TAOWListBox
{
    public override bool ContainsPoint(int X, int Y)
    {
        if (FAOWWindow == null) return false;
        return PointInside(X, Y, FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth - F138, FWinHeight);
    }

    // The linked bar scrolls even when it is hidden, so a shared column does not drift. With no bar
    // the list moves its own offset and does not fire OnChange.
    public override bool CheckMouseWheel(int Steps, TSet<TShiftState> Shift, int X, int Y)
    {
        if (Steps == 0 || !ContainsPoint(X, Y)) return false;
        TAOWVScrollBar bar = WheelScrollBar ?? FVScrollBar;
        if (bar != null)
        {
            bar.ScrollSteps(Steps);
            return true;
        }

        int limit = FItems.Count - F11C;
        if (limit < 0) limit = 0;
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
