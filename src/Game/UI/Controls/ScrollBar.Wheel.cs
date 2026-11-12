
namespace Aow.Game.UI.Controls;

public partial class ScrollBar
{
    // The highest position is above FMin when a step can move the bar.
    internal bool HasWheelRange() => _max + 1 - _numberVisible > _min;

    // One arrow step. The wheel, the arrow buttons and a held arrow all come through here.
    protected void ApplySmallStep(bool up)
    {
        if (up)
        {
            _position = _position - _smallChange;
            if (_min > _position) _position = _min;
        }
        else
        {
            _position = _position + _smallChange;
            if (_max + 1 - _numberVisible < _position) _position = _max + 1 - _numberVisible;
        }

        Drawn = false;
        RaiseChange(this);
    }

    // Positive steps move toward FMin, one SmallChange and one OnChange per step.
    internal void ScrollSteps(int steps)
    {
        int count = steps < 0 ? -steps : steps;
        for (int i = 0; i < count; i++) ApplySmallStep(steps > 0);
    }
}
