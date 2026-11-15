#nullable disable

using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class WindowManager
{
    // The topmost visible window at or above the overlay that contains the pointer gets the wheel.
    // A window under it does not, including when the upper window handles nothing.
    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int x, int y)
    {
        if (steps == 0)
        {
            return false;
        }

        for (int i = Children.Count - 1; i >= 0; i--)
        {
            UIComponent c = Children[i];
            if (c.Priority < _overlayPriority || !c.Visible)
            {
                continue;
            }

            if (!c.ContainsPoint(x, y))
            {
                continue;
            }

            c.CheckMouseWheel(steps, shift, x, y);
            return true;
        }

        return false;
    }
}
