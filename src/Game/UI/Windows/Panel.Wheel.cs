
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

public partial class Panel
{
    public override bool ContainsPoint(int X, int Y)
    {
        SizedRectangle absolute = AbsoluteArea();
        return PointInside(X, Y, absolute.Left, absolute.Top, absolute.Width, absolute.Height);
    }

    // Child hit tests read AbsoluteLeft and AbsoluteTop so the panel's coordinates stay fixed.
    // A wheel that nobody handles is not consumed, so a book page can still turn.
    public override bool CheckMouseWheel(int steps, Set<ShiftState> shift, int X, int Y)
    {
        if (steps == 0 || !Visible)
        {
            return false;
        }

        SizedRectangle absolute = AbsoluteArea();
        UIComponent child = TopWheelChild(X, Y);
        return (child is not null && child.CheckMouseWheel(steps, shift, X, Y)) || (PointInside(X, Y, absolute.Left, absolute.Top, WinWidth, WinHeight) && ScrollOwnedBar(steps));
    }
}
