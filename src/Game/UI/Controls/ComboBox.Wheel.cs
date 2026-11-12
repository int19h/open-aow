using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class ComboBox
{
    // A closed list leaves the wheel for the parent. An open list scrolls with its bar and moves F12C
    // in the same call, which is what Update copies from the bar on the next frame.
    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0 || !Open || _verticalScrollBar == null || !ContainsPoint(X, Y)) return false;
        _verticalScrollBar.ScrollSteps(steps);
        FirstShownItem = _verticalScrollBar._position;
        int count = _items.Count;
        if (FirstShownItem + OpenListRows > count) FirstShownItem = count - OpenListRows;
        if (FirstShownItem < 0) FirstShownItem = 0;
        Drawn = false;
        return true;
    }
}
