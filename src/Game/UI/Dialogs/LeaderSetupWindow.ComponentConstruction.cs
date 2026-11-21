// Layout translated from recovered Delphi forms.
#nullable disable warnings
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public sealed partial class LeaderSetupWindow
{
    public event NotifyEvent<LeaderSetupWindow>? LayoutOnCreate;
    public event NotifyEvent<LeaderSetupWindow>? LayoutOnDestroy;

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
