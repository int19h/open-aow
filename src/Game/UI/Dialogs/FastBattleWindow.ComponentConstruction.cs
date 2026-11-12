// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class FastBattleWindow
{
    public event NotifyEvent<FastBattleWindow> LayoutOnCreate;
    public event NotifyEvent<FastBattleWindow> LayoutOnDestroy;

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
