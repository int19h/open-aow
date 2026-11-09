namespace Aow.Units.TurnButton;

public partial class TAOWTurnButton
{
    // A left click and the strategic-map end-turn shortcut share this press.
    // While the button animates a click does nothing, and so does a press.
    // A port addition the original does not have.
    internal bool Press()
    {
        if (FAnimating)
        {
            return false;
        }

        if (FButtonChecked)
        {
            FButtonChecked = false;
        }
        else
        {
            FButtonChecked = true;
        }

        FDrawn = false;
        if (FOnPlayerChange != null)
        {
            FOnPlayerChange(this);
        }

        return true;
    }
}
