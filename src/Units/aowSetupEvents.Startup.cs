using Aow.Game.Engine;

namespace Aow.Game.Ui.Dialogs;

public partial class SetupEvents
{
    public void SetupEventsCreate() => DPlay.AppGUID = ID_AOWAPP;

    public void SetupControlPlayerSetupDone() { }
}
