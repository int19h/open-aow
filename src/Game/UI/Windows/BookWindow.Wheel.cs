using Aow.Rtl;

namespace Aow.Game.UI.Windows;

public sealed partial class BookWindow
{
    protected override bool OneWheelTurnPerEvent => true;

    // A page turn in progress keeps the wheel. It does not cancel the animation and it does not turn again.
    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y) => (Animating && PointInside(X, Y, WinLeft, WinTop, WinWidth, WinHeight)) || base.CheckMouseWheel(steps, shift, X, Y);
}
