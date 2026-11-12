namespace Aow.Game.UI.Dialogs;

public partial class HeroUpgradeDialog
{
    public override void Loaded()
    {
        base.Loaded();
        if (SelectedAbilitiesList != null) SelectedAbilitiesList.WheelScrollBar = SelectedAbilitiesScrollBar;
        if (AvailableAbilitiesList != null) AvailableAbilitiesList.WheelScrollBar = AvailableAbilitiesScrollBar;
    }
}
