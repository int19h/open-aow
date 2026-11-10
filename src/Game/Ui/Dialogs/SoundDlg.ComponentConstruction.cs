// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

public partial class SoundDlg
{
    public NotifyEvent<SoundDlg> LayoutOnCreate { get; set; }
    public NotifyEvent<SoundDlg> LayoutOnDestroy { get; set; }

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
