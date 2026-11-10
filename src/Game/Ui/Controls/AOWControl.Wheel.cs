namespace Aow.Game.Ui.Controls;

public partial class AOWControl
{
    public override bool ContainsPoint(int X, int Y)
    {
        if (FAOWWindow == null) return false;
        return PointInside(X, Y, FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
    }
}
