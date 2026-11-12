using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class VerticalScrollBar
{
    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0 || !ContainsPoint(X, Y)) return false;
        ScrollSteps(steps);
        return true;
    }
}
