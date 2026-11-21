// Layout translated from recovered Delphi forms.
#nullable disable warnings
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public sealed partial class FileDialogWindow
{
    public event NotifyEvent<FileDialogWindow>? LayoutOnCreate;
    public event NotifyEvent<FileDialogWindow>? LayoutOnDestroy;

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
