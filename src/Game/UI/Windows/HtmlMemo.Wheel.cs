using Aow.Game.UI.Controls;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

public partial class HtmlMemo
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

    public override bool CheckMouseWheel(int Steps, Set<ShiftState> Shift, int X, int Y)
    {
        if (Steps == 0 || !ContainsPoint(X, Y)) return false;
        VerticalScrollBar bar = WheelScrollBar ?? FVScrollBar;
        if (bar == null || !bar.HasWheelRange()) return false;
        bar.ScrollSteps(Steps);
        FTopItem = bar.FPosition;
        return true;
    }
}
