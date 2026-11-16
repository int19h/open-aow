namespace Aow.Game.UI.Dialogs;

public partial class HeroUpgradeDialog
{
    public override void Loaded()
    {
        base.Loaded();
        SelectedAbilitiesList?.WheelScrollBar = SelectedAbilitiesScrollBar;

        AvailableAbilitiesList?.WheelScrollBar = AvailableAbilitiesScrollBar;
    }
}
