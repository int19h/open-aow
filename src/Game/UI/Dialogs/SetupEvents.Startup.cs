
namespace Aow.Game.UI.Dialogs;

public partial class SetupEvents
{
    public void SetupEventsCreate() => DPlay.AppGUID = s_applicationGuid;

    public void SetupControlPlayerSetupDone() { }
}
