// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class HeroJoinDialog
{
    public event NotifyEvent<HeroJoinDialog> LayoutOnCreate;
    public event NotifyEvent<HeroJoinDialog> LayoutOnDestroy;

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
