namespace Aow.Game.UI.Controls;

public partial class Control
{
    public override bool ContainsPoint(int X, int Y) => _window != null && PointInside(X, Y, _window.AbsoluteLeft + WinLeft, _window.AbsoluteTop + WinTop, WinWidth, WinHeight);
}
