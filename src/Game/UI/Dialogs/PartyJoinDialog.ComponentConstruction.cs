// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class PartyJoinDialog
{
    public event NotifyEvent<PartyJoinDialog> LayoutOnCreate;
    public event NotifyEvent<PartyJoinDialog> LayoutOnDestroy;

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
