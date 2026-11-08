using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWVScroll;

namespace Aow.Units.AoWHTMLMemo;

public partial class TaowHTMLMemo
{
    public override bool WantsMouseWheel => FVisible;

    public override bool ContainsPoint(int X, int Y)
    {
        int left = FWinLeft;
        int top = FWinTop;
        if (FAOWWindow != null)
        {
            left += FAOWWindow.FWinLeft;
            top += FAOWWindow.FWinTop;
        }

        return PointInside(X, Y, left, top, FWinWidth, FWinHeight);
    }

    public override bool CheckMouseWheel(int Steps, TSet<TShiftState> Shift, int X, int Y)
    {
        if (Steps == 0 || !ContainsPoint(X, Y)) return false;
        TAOWVScrollBar bar = WheelScrollBar ?? FVScrollBar;
        if (bar == null || !bar.HasWheelRange()) return false;
        bar.ScrollSteps(Steps);
        FTopItem = bar.FPosition;
        return true;
    }
}
