namespace Aow.Game.UI.Dialogs;

public partial class CityScreen
{
    public override void Loaded()
    {
        base.Loaded();
        if (MigrationRaceLB != null) MigrationRaceLB.WheelScrollBar = MigrationSB;
        if (MigrationTurnsLB != null) MigrationTurnsLB.WheelScrollBar = MigrationSB;
    }
}
