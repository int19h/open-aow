namespace Aow.Game.UI.Dialogs;

public sealed partial class LeaderSetupWindow
{
    public override void Loaded()
    {
        base.Loaded();
        SelectedAbilitiesList?.WheelScrollBar = SelectedAbilitiesScrollBar;

        AvailableAbilitiesList?.WheelScrollBar = AvailableAbilitiesScrollBar;
    }
}
