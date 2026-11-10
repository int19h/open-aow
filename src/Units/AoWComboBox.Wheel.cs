using Aow.Rtl;

namespace Aow.Game.Ui.Controls;

public partial class TAOWComboBox
{
    // A closed list leaves the wheel for the parent. An open list scrolls with its bar and moves F12C
    // in the same call, which is what Update copies from the bar on the next frame.
    public override bool CheckMouseWheel(int Steps, TSet<TShiftState> Shift, int X, int Y)
    {
        if (Steps == 0 || !FOpen || FVScrollBar == null || !ContainsPoint(X, Y)) return false;
        FVScrollBar.ScrollSteps(Steps);
        F12C = FVScrollBar.FPosition;
        int count = FItems.Count;
        if (F12C + F124 > count) F12C = count - F124;
        if (F12C < 0) F12C = 0;
        FDrawn = false;
        return true;
    }
}
