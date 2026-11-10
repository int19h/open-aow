namespace Aow.Game.Ui.Dialogs;

public partial class TLeaderSetupWin
{
    public override void Loaded()
    {
        base.Loaded();
        if (SelectedAbilities != null) SelectedAbilities.WheelScrollBar = SelectedAbilitiesLB;
        if (AvailableAbilities != null) AvailableAbilities.WheelScrollBar = AvailableAbilitiesLB;
    }
}
