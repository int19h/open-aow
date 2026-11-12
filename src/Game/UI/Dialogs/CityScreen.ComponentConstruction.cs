// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class CityScreen
{
    public event NotifyEvent<CityScreen> LayoutOnCreate;
    public event NotifyEvent<CityScreen> LayoutOnDestroy;

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
