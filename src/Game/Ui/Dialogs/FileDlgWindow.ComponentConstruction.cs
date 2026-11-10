// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

public partial class FileDlgWindow
{
    public NotifyEvent<FileDlgWindow> LayoutOnCreate { get; set; }
    public NotifyEvent<FileDlgWindow> LayoutOnDestroy { get; set; }

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
