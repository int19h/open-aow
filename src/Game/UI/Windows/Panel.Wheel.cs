
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

public partial class Panel
{
    public override bool ContainsPoint(int X, int Y)
    {
        var absolute = AbsoluteArea();
        return PointInside(X, Y, absolute.Left, absolute.Top, absolute.Width, absolute.Height);
    }

    // The panel moves by its window, as CheckMouseDown does, so a child hit test sees screen pixels.
    // The panel's own hit test reads the absolute rectangle from a local.
    // A wheel that nobody handles is not consumed, so a book page can still turn.
    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0 || !Visible) return false;
        int ownLeft = WinLeft;
        int ownTop = WinTop;
        var absolute = AbsoluteArea();
        try
        {
            if (_window != null)
            {
                SetWinLeftWithoutInvalidate(absolute.Left);
                SetWinTopWithoutInvalidate(absolute.Top);
            }

            var child = TopWheelChild(X, Y);
            if (child != null && child.CheckMouseWheel(steps, shift, X, Y)) return true;
            if (!PointInside(X, Y, absolute.Left, absolute.Top, WinWidth, WinHeight)) return false;
            return ScrollOwnedBar(steps);
        }
        finally
        {
            SetWinLeftWithoutInvalidate(ownLeft);
            SetWinTopWithoutInvalidate(ownTop);
        }
    }
}
