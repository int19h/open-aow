namespace Aow.Units.aowSetupEvents;

public partial class TSetupEvents
{
    public void SetupEventsCreate() => DPlay.AppGUID = AoWReg.AoWReg.ID_AOWAPP;
}
