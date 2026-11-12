namespace Aow.Game.UI.Controls;

public partial class Control
{
    public override bool ContainsPoint(int X, int Y)
    {
        if (_window == null) return false;
        return PointInside(X, Y, _window.WinLeft + WinLeft, _window.WinTop + WinTop, WinWidth, WinHeight);
    }
}
