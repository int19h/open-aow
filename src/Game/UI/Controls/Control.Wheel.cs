namespace Aow.Game.UI.Controls;

public partial class Control
{
    public override bool ContainsPoint(int X, int Y) => _window != null && PointInside(X, Y, _window.WinLeft + WinLeft, _window.WinTop + WinTop, WinWidth, WinHeight);
}
