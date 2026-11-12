
namespace Aow.Game.UI.Dialogs;

public partial class SetupEvents
{
    public void SetupEventsCreate() => DirectPlay.AppGUID = s_applicationGuid;

    public void SetupControlPlayerSetupDone() { }
}
