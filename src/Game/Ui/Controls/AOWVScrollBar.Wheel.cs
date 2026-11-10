using Aow.Rtl;

namespace Aow.Game.Ui.Controls;

public partial class AOWVScrollBar
{
    public override bool CheckMouseWheel(int Steps, Set<ShiftState> Shift, int X, int Y)
    {
        if (Steps == 0 || !ContainsPoint(X, Y)) return false;
        ScrollSteps(Steps);
        return true;
    }
}
