
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class ListBox
{
    public override bool ContainsPoint(int X, int Y)
    {
        if (Window == null) return false;
        return PointInside(X, Y, Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth - _scrollWidth, WinHeight);
    }

    // A linked bar with room to move scrolls even when it is hidden, so shared columns stay together.
    // A bar with no room, or no bar when every item fits, leaves the wheel for the parent.
    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0 || !ContainsPoint(X, Y)) return false;
        VerticalScrollBar bar = WheelScrollBar ?? _verticalScrollBar;
        if (bar != null)
        {
            if (!bar.HasWheelRange()) return false;
            bar.ScrollSteps(steps);
            return true;
        }

        int limit = _items.Count - VisibleRows;
        if (limit < 0) limit = 0;
        if (limit == 0) return false;
        int next = FirstShownItem - steps;
        if (next < 0) next = 0;
        if (next > limit) next = limit;
        if (next != FirstShownItem)
        {
            FirstShownItem = next;
            Drawn = false;
        }

        return true;
    }
}
