// Layout translated from recovered Delphi forms.
#nullable disable warnings
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public sealed partial class ResultsWindow
{
    public event NotifyEvent<ResultsWindow>? LayoutOnCreate;
    public event NotifyEvent<ResultsWindow>? LayoutOnDestroy;

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
