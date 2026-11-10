using Aow.Rtl;

namespace Aow.Game.Ui.Windows;

public partial class AoWBookWindow
{
    protected override bool OneWheelTurnPerEvent => true;

    // A page turn in progress keeps the wheel. It does not cancel the animation and it does not turn again.
    public override bool CheckMouseWheel(int Steps, Set<ShiftState> Shift, int X, int Y)
    {
        if (FAnimating && PointInside(X, Y, FWinLeft, FWinTop, FWinWidth, FWinHeight)) return true;
        return base.CheckMouseWheel(Steps, Shift, X, Y);
    }
}
