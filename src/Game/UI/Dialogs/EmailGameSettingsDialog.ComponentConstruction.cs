// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class EmailGameSettingsDialog
{
    public NotifyEvent<EmailGameSettingsDialog> LayoutOnCreate { get; set; }
    public NotifyEvent<EmailGameSettingsDialog> LayoutOnDestroy { get; set; }

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
