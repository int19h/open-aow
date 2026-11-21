// Layout translated from recovered Delphi forms.
#nullable disable warnings
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public sealed partial class TitleScene
{
    public event NotifyEvent<TitleScene>? LayoutOnCreate;
    public event NotifyEvent<TitleScene>? LayoutOnDestroy;

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
