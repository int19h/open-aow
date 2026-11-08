using Aow.Rtl;

namespace Aow.Units.AoWVScroll;

public partial class TAOWScrollBar
{
    // The highest position is above FMin when a step can move the bar.
    internal bool HasWheelRange() => FMax + 1 - FNumberVisible > FMin;

    // One arrow step. The wheel, the arrow buttons and a held arrow all come through here.
    protected void ApplySmallStep(bool up)
    {
        if (up)
        {
            FPosition = FPosition - FSmallChange;
            if (FMin > FPosition) FPosition = FMin;
        }
        else
        {
            FPosition = FPosition + FSmallChange;
            if (FMax + 1 - FNumberVisible < FPosition) FPosition = FMax + 1 - FNumberVisible;
        }

        FDrawn = false;
        if (FOnChange != null) FOnChange(this);
    }

    // Positive steps move toward FMin, one SmallChange and one OnChange per step.
    internal void ScrollSteps(int steps)
    {
        int count = steps < 0 ? -steps : steps;
        for (int i = 0; i < count; i++) ApplySmallStep(steps > 0);
    }
}

public partial class TAOWVScrollBar
{
    public override bool CheckMouseWheel(int Steps, TSet<TShiftState> Shift, int X, int Y)
    {
        if (Steps == 0 || !ContainsPoint(X, Y)) return false;
        ScrollSteps(Steps);
        return true;
    }
}
