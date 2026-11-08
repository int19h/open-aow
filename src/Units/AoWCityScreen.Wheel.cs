namespace Aow.Units.AoWCityScreen;

public partial class TCityScreen
{
    public override void Loaded()
    {
        base.Loaded();
        if (MigrationRaceLB != null) MigrationRaceLB.WheelScrollBar = MigrationSB;
        if (MigrationTurnsLB != null) MigrationTurnsLB.WheelScrollBar = MigrationSB;
    }
}
