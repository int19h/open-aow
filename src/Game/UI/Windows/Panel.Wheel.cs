
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

public partial class Panel
{
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

    // The panel moves by its window, as CheckMouseDown does, so a child hit test sees screen pixels.
    // A wheel that nobody handles is not consumed, so a book page can still turn.
    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0 || !_visible) return false;
        SavedLeft = _winLeft;
        SavedTop = _winTop;
        try
        {
            if (_window != null)
            {
                _winLeft += _window._winLeft;
                _winTop += _window._winTop;
            }

            var child = TopWheelChild(X, Y);
            if (child != null && child.CheckMouseWheel(steps, shift, X, Y)) return true;
            if (!PointInside(X, Y, _winLeft, _winTop, _winWidth, _winHeight)) return false;
            return ScrollOwnedBar(steps);
        }
        finally
        {
            _winLeft = SavedLeft;
            _winTop = SavedTop;
        }
    }
}
