using Aow.Game.Ui.Controls;
using Aow.Rtl;

namespace Aow.Game.Ui.Controls;

public partial class TAOWWinManager
{
    // The topmost visible window at or above the overlay that contains the pointer gets the wheel.
    // A window under it does not, including when the upper window handles nothing.
    public override bool CheckMouseWheel(int Steps, TSet<TShiftState> Shift, int X, int Y)
    {
        if (Steps == 0) return false;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            var C = (TAoWComponent)FChildren[I];
            if (C.FPriority < FOverlayPriority || !C.FVisible) continue;
            if (!C.ContainsPoint(X, Y)) continue;
            C.CheckMouseWheel(Steps, Shift, X, Y);
            return true;
        }

        return false;
    }
}
