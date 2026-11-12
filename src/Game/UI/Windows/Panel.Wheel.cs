
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

public partial class Panel
{
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

    // The panel moves by its window, as CheckMouseDown does, so a child hit test sees screen pixels.
    // A wheel that nobody handles is not consumed, so a book page can still turn.
    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0 || !Visible) return false;
        _savedLeft = WinLeft;
        _savedTop = WinTop;
        try
        {
            if (_window != null)
            {
                SetWinLeftWithoutInvalidate(WinLeft + (_window.WinLeft));
                SetWinTopWithoutInvalidate(WinTop + (_window.WinTop));
            }

            var child = TopWheelChild(X, Y);
            if (child != null && child.CheckMouseWheel(steps, shift, X, Y)) return true;
            if (!PointInside(X, Y, WinLeft, WinTop, WinWidth, WinHeight)) return false;
            return ScrollOwnedBar(steps);
        }
        finally
        {
            SetWinLeftWithoutInvalidate(_savedLeft);
            SetWinTopWithoutInvalidate(_savedTop);
        }
    }
}
