// Layout translated from recovered Delphi forms.
#nullable disable warnings
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public sealed partial class PartyJoinDialog
{
    public event NotifyEvent<PartyJoinDialog>? LayoutOnCreate;
    public event NotifyEvent<PartyJoinDialog>? LayoutOnDestroy;

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
