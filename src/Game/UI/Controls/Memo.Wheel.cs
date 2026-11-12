
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public partial class Memo
{
    // A memo with CaptureInput clear still takes the wheel. The world map intro text is one of those.
    public override bool WantsMouseWheel => _visible;

    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0 || !ContainsPoint(X, Y)) return false;
        VerticalScrollBar bar = WheelScrollBar ?? _verticalScrollBar;
        if (bar == null || !bar.HasWheelRange()) return false;
        bar.ScrollSteps(steps);
        return true;
    }
}
