using Aow.Game.UI.Controls;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

public partial class HtmlMemo
{
    public override bool WantsMouseWheel => Visible;

    public override bool ContainsPoint(int X, int Y)
    {
        int left = WinLeft;
        int top = WinTop;
        if (_window != null)
        {
            left += _window.WinLeft;
            top += _window.WinTop;
        }

        return PointInside(X, Y, left, top, WinWidth, WinHeight);
    }

    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0 || !ContainsPoint(X, Y)) return false;
        VerticalScrollBar bar = WheelScrollBar ?? _verticalScrollBar;
        if (bar == null || !bar.HasWheelRange()) return false;
        bar.ScrollSteps(steps);
        _topItem = bar.Position;
        return true;
    }
}
