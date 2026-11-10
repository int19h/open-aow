
using Aow.Rtl;

namespace Aow.Game.Ui.Windows;

public partial class AOWPanel
{
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

    // The panel moves by its window, as CheckMouseDown does, so a child hit test sees screen pixels.
    // A wheel that nobody handles is not consumed, so a book page can still turn.
    public override bool CheckMouseWheel(int Steps, Set<ShiftState> Shift, int X, int Y)
    {
        if (Steps == 0 || !FVisible) return false;
        FOrgLeft = FWinLeft;
        FOrgTop = FWinTop;
        try
        {
            if (FAOWWindow != null)
            {
                FWinLeft += FAOWWindow.FWinLeft;
                FWinTop += FAOWWindow.FWinTop;
            }

            var child = TopWheelChild(X, Y);
            if (child != null && child.CheckMouseWheel(Steps, Shift, X, Y)) return true;
            if (!PointInside(X, Y, FWinLeft, FWinTop, FWinWidth, FWinHeight)) return false;
            return ScrollOwnedBar(Steps);
        }
        finally
        {
            FWinLeft = FOrgLeft;
            FWinTop = FOrgTop;
        }
    }
}
