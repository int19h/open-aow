using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class VerticalScrollBar
{
    public override bool CheckMouseWheel(int Steps, Set<ShiftState> Shift, int X, int Y)
    {
        if (Steps == 0 || !ContainsPoint(X, Y)) return false;
        ScrollSteps(Steps);
        return true;
    }
}
