// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class EmailGameSettingsDialog
{
    public event NotifyEvent<EmailGameSettingsDialog> LayoutOnCreate;
    public event NotifyEvent<EmailGameSettingsDialog> LayoutOnDestroy;

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
