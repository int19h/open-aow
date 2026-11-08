using Aow.Rtl;
using Aow.Units.AoWVScroll;

namespace Aow.Units.AoWMemo;

public partial class TAOWMemo
{
    // A memo with CaptureInput clear still takes the wheel. The world map intro text is one of those.
    public override bool WantsMouseWheel => FVisible;

    public override bool CheckMouseWheel(int Steps, TSet<TShiftState> Shift, int X, int Y)
    {
        if (Steps == 0 || !ContainsPoint(X, Y)) return false;
        TAOWVScrollBar bar = WheelScrollBar ?? FVScrollBar;
        if (bar == null || !bar.HasWheelRange()) return false;
        bar.ScrollSteps(Steps);
        return true;
    }
}
