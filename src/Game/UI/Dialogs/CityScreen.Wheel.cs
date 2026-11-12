namespace Aow.Game.UI.Dialogs;

public partial class CityScreen
{
    public override void Loaded()
    {
        base.Loaded();
        if (MigrationRaceList != null) MigrationRaceList.WheelScrollBar = MigrationScrollBar;
        if (MigrationTurnsList != null) MigrationTurnsList.WheelScrollBar = MigrationScrollBar;
    }
}
