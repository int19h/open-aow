namespace Aow.Game.UI.Grids;

public partial class TurnButton
{
    // A left click and the strategic-map end-turn shortcut share this press.
    // While the button animates a click does nothing, and so does a press.
    // A port addition the original does not have.
    internal bool Press()
    {
        if (_animating)
        {
            return false;
        }

        if (ButtonChecked)
        {
            ButtonChecked = false;
        }
        else
        {
            ButtonChecked = true;
        }

        Drawn = false;
        if (OnChange != null)
        {
            OnChange(this);
        }

        return true;
    }
}
