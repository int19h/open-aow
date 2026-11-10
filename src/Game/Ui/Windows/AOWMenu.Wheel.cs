
using Aow.Rtl;

namespace Aow.Game.Ui.Windows;

public partial class AOWMenu
{
    public override bool ContainsPoint(int X, int Y) => PointInside(X, Y, FWinLeft, FWinTop, FWinWidth, FWinHeight);

    public override bool CheckMouseWheel(int Steps, Set<ShiftState> Shift, int X, int Y)
    {
        if (Steps == 0 || !FVisible || !ContainsPoint(X, Y)) return false;
        var child = TopWheelChild(X, Y);
        if (child != null && child.CheckMouseWheel(Steps, Shift, X, Y)) return true;
        return ScrollOwnedBar(Steps);
    }
}
