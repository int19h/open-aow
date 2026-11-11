// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class ArmyEventDialog
{
    public NotifyEvent<ArmyEventDialog> LayoutOnCreate { get; set; }
    public NotifyEvent<ArmyEventDialog> LayoutOnDestroy { get; set; }

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
