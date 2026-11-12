// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class LeaderSetupWindow
{
    public event NotifyEvent<LeaderSetupWindow> LayoutOnCreate;
    public event NotifyEvent<LeaderSetupWindow> LayoutOnDestroy;

    public override void Dispose()
    {
        Destroying();
        try
        {
            LayoutOnDestroy?.Invoke(this);
        }
        finally
        {
            base.Dispose();
        }
    }
}
