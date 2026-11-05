using Aow.Rtl;
using Aow.Units.AoWE;

namespace Aow.Units.CMapCtrl;

public partial class TAoWCombatMapControl
{
    // CMapCtrl.pas:198. The notification carries the changed map area.
    public void MapChanged(TObject sender)
    {
        try
        {
            if (FOnMapChanged is not null) FOnMapChanged(((MapChangedNotification)sender).Info);
        }
        catch (System.Exception)
        {
            Rtl.Rtl.ShowMessage("Exception occurred during MapChanged event (TAoWCombatMapControl)");
        }
    }
}
