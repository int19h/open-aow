namespace Aow.Game.Ui.Dialogs;

public partial class HeroUpgradeDlg
{
    public override void Loaded()
    {
        base.Loaded();
        if (SelectedAbilities != null) SelectedAbilities.WheelScrollBar = SelectedAbilitiesSB;
        if (AvailableAbilities != null) AvailableAbilities.WheelScrollBar = AvailableAbilitiesSB;
    }
}
