// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

public partial class General
{
    public NotifyEvent<General> LayoutOnCreate { get; set; }
    public NotifyEvent<General> LayoutOnDestroy { get; set; }

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
