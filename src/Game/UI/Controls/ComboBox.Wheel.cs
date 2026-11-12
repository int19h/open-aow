using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class ComboBox
{
    // A closed list leaves the wheel for the parent. An open list scrolls with its bar and moves F12C
    // in the same call, which is what Update copies from the bar on the next frame.
    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0 || !_open || _verticalScrollBar == null || !ContainsPoint(X, Y)) return false;
        _verticalScrollBar.ScrollSteps(steps);
        _firstShownItem = _verticalScrollBar.Position;
        int count = _items.Count;
        if (_firstShownItem + _openListRows > count) _firstShownItem = count - _openListRows;
        if (_firstShownItem < 0) _firstShownItem = 0;
        Drawn = false;
        return true;
    }
}
