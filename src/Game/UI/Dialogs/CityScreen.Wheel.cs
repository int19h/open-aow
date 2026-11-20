namespace Aow.Game.UI.Dialogs;

public sealed partial class CityScreen
{
    public override void Loaded()
    {
        base.Loaded();
        MigrationRaceList?.WheelScrollBar = MigrationScrollBar;

        MigrationTurnsList?.WheelScrollBar = MigrationScrollBar;
    }
}
