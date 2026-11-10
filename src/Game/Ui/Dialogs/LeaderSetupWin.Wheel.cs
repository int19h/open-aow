namespace Aow.Game.Ui.Dialogs;

public partial class LeaderSetupWin
{
    public override void Loaded()
    {
        base.Loaded();
        if (SelectedAbilities != null) SelectedAbilities.WheelScrollBar = SelectedAbilitiesLB;
        if (AvailableAbilities != null) AvailableAbilities.WheelScrollBar = AvailableAbilitiesLB;
    }
}
