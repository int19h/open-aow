using Aow.Rtl;
using Aow.Units.AoWE;

namespace Aow.Units.CMapCtrl;

public partial class TAoWCombatMapControl
{
    public void ShowMessage(TObject sender)
    {
        try { FOnShowMessage?.Invoke(((Aow.Units.AoWE.ShowMessageNotification)sender).Info); }
        catch (System.Exception) { Aow.Rtl.Rtl.ShowMessage("Exception occurred during ShowMessage event(TAoWCombatMapControl)"); }
    }
}
