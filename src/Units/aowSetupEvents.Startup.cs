using Aow.Game.Engine;

namespace Aow.Game.Ui.Dialogs;

public partial class SetupEvents
{
    public void SetupEventsCreate() => DPlay.AppGUID = Aow.Game.Ui.Dialogs.SetupEvents.ID_AOWAPP;

    public void SetupControlPlayerSetupDone() { }
}
