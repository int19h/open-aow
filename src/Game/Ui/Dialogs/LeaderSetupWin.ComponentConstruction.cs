// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

public partial class LeaderSetupWin
{
    public NotifyEvent<LeaderSetupWin> LayoutOnCreate { get; set; }
    public NotifyEvent<LeaderSetupWin> LayoutOnDestroy { get; set; }

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
