using Aow.Game.UI.Controls;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

public partial class HtmlMemo
{
    public override bool WantsMouseWheel => _visible;

    public override bool ContainsPoint(int X, int Y)
    {
        int left = _winLeft;
        int top = _winTop;
        if (_window != null)
        {
            left += _window._winLeft;
            top += _window._winTop;
        }

        return PointInside(X, Y, left, top, _winWidth, _winHeight);
    }

    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0 || !ContainsPoint(X, Y)) return false;
        VerticalScrollBar bar = WheelScrollBar ?? _verticalScrollBar;
        if (bar == null || !bar.HasWheelRange()) return false;
        bar.ScrollSteps(steps);
        TopItem = bar._position;
        return true;
    }
}
