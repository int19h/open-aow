using Aow.Units.AoWReg;

namespace Aow.Units.aowSetupEvents;

public partial class TSetupEvents
{
    public void SetupEventsCreate() => DPlay.AppGUID = AoWRegUnit.ID_AOWAPP;

    public void SetupControlPlayerSetupDone() { }
}
