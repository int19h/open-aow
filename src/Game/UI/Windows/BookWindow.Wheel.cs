using Aow.Rtl;

namespace Aow.Game.UI.Windows;

public partial class BookWindow
{
    protected override bool OneWheelTurnPerEvent => true;

    // A page turn in progress keeps the wheel. It does not cancel the animation and it does not turn again.
    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (Animating && PointInside(X, Y, _winLeft, _winTop, _winWidth, _winHeight)) return true;
        return base.CheckMouseWheel(steps, shift, X, Y);
    }
}
