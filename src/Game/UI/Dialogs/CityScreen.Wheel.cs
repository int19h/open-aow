namespace Aow.Game.UI.Dialogs;

public partial class CityScreen
{
    public override void Loaded()
    {
        base.Loaded();
        MigrationRaceList?.WheelScrollBar = MigrationScrollBar;

        MigrationTurnsList?.WheelScrollBar = MigrationScrollBar;
    }
}
