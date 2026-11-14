
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class WindowManager
{
    // The topmost visible window at or above the overlay that contains the pointer gets the wheel.
    // A window under it does not, including when the upper window handles nothing.
    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0) return false;
        for (int i = Children.Count - 1; i >= 0; i--)
        {
            var c = Children[i];
            if (c.Priority < _overlayPriority || !c.Visible) continue;
            if (!c.ContainsPoint(X, Y)) continue;
            c.CheckMouseWheel(steps, shift, X, Y);
            return true;
        }

        return false;
    }
}
