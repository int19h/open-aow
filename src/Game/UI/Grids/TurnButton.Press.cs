namespace Aow.Game.UI.Grids;

public sealed partial class TurnButton
{
    // A left click and the strategic-map end-turn shortcut share this press.
    // While the button animates a click does nothing, and so does a press.
    // A port addition the original does not have.
    internal bool Press()
    {
        if (Animating)
        {
            return false;
        }

        ButtonChecked = !ButtonChecked;

        IsDrawn = false;
        Changed?.Invoke(this);

        return true;
    }
}
