using Aow.Rtl;
using Aow.Units.AoWE;

namespace Aow.Units.CMapCtrl;

public partial class TAoWCombatMapControl
{
    public void ShowMessage(TObject sender)
    {
        try { FOnShowMessage?.Invoke(((ShowMessageNotification)sender).Info); }
        catch (System.Exception) { Rtl.Rtl.ShowMessage("Exception occurred during ShowMessage event(TAoWCombatMapControl)"); }
    }
}
