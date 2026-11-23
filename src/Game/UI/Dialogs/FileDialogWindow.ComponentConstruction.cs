// Layout translated from recovered Delphi forms.
#nullable disable warnings

namespace Aow.Game.UI.Dialogs;

public sealed partial class FileDialogWindow
{
    private bool _hasLayout;

    public override void Dispose()
    {
        Destroying();
        try
        {
            if (_hasLayout)
            {
                FileDialogWindowDestroy();
            }
        }
        finally
        {
            base.Dispose();
        }
    }
}
