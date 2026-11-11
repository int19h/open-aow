// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class HeroJoinDialog
{
    public NotifyEvent<HeroJoinDialog> LayoutOnCreate { get; set; }
    public NotifyEvent<HeroJoinDialog> LayoutOnDestroy { get; set; }

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
