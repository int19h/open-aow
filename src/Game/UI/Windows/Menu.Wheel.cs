
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

public partial class Menu
{
    public override bool ContainsPoint(int X, int Y) => PointInside(X, Y, _winLeft, _winTop, _winWidth, _winHeight);

    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0 || !_visible || !ContainsPoint(X, Y)) return false;
        var child = TopWheelChild(X, Y);
        if (child != null && child.CheckMouseWheel(steps, shift, X, Y)) return true;
        return ScrollOwnedBar(steps);
    }
}
