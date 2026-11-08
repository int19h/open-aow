using Aow.Rtl;
using Aow.Units.AoWVScroll;

namespace Aow.Units.AoWMemo;

public partial class TAOWMemo
{
    public override bool CheckMouseWheel(int Steps, TSet<TShiftState> Shift, int X, int Y)
    {
        if (Steps == 0 || !ContainsPoint(X, Y)) return false;
        TAOWVScrollBar bar = WheelScrollBar ?? FVScrollBar;
        if (bar == null) return false;
        bar.ScrollSteps(Steps);
        return true;
    }
}
