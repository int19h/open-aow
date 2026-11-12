
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class WindowManager
{
    // The topmost visible window at or above the overlay that contains the pointer gets the wheel.
    // A window under it does not, including when the upper window handles nothing.
    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0) return false;
        for (int I = Children.Count - 1; I >= 0; I--)
        {
            var C = Children[I];
            if (C.Priority < _overlayPriority || !C.Visible) continue;
            if (!C.ContainsPoint(X, Y)) continue;
            C.CheckMouseWheel(steps, shift, X, Y);
            return true;
        }

        return false;
    }
}
