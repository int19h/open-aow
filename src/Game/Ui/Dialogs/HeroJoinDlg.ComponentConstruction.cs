// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

public partial class HeroJoinDlg
{
    public NotifyEvent<HeroJoinDlg> LayoutOnCreate { get; set; }
    public NotifyEvent<HeroJoinDlg> LayoutOnDestroy { get; set; }

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
