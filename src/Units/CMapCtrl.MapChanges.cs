using Aow.Rtl;
using Aow.Units.AoWE;

namespace Aow.Units.CMapCtrl;

public partial class TAoWCombatMapControl
{
    // CMapCtrl.pas:198. The notification carries a pointer to TMapChangedInfo.
    public void MapChanged(TObject sender)
    {
        try
        {
            if (FOnMapChanged is not null) FOnMapChanged(Ptr<TMapChangedInfo>.Unbox(sender).Value);
        }
        catch (System.Exception)
        {
            Aow.Rtl.Rtl.ShowMessage("Exception occurred during MapChanged event (TAoWCombatMapControl)");
        }
    }
}
