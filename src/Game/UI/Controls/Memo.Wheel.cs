
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class Memo
{
    // A memo with CaptureInput clear still takes the wheel. The world map intro text is one of those.
    public override bool WantsMouseWheel => FVisible;

    public override bool CheckMouseWheel(int Steps, Set<ShiftState> Shift, int X, int Y)
    {
        if (Steps == 0 || !ContainsPoint(X, Y)) return false;
        VerticalScrollBar bar = WheelScrollBar ?? FVScrollBar;
        if (bar == null || !bar.HasWheelRange()) return false;
        bar.ScrollSteps(Steps);
        return true;
    }
}
