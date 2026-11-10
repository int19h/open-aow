using Aow.Game.Engine;

namespace Aow.Game.Ui.Dialogs;

public partial class TSetupEvents
{
    public void SetupEventsCreate() => DPlay.AppGUID = Aow.Game.Ui.Dialogs.TSetupEvents.ID_AOWAPP;

    public void SetupControlPlayerSetupDone() { }
}
