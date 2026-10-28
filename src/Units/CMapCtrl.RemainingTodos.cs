using Aow.Rtl;
using Aow.Units.EventLog;

namespace Aow.Units.CMapCtrl;

public partial class TAoWCombatMapControl
{
    public void ExecuteEventLog(TObject sender)
    {
        try
        {
            if (FOnExecuteEventLog is null) return;
            ref var info = ref Ptr<TExecuteEventLogInfo>.Unbox(sender).Value;
            FOnExecuteEventLog(info.Log, info.Done);
        }
        catch (System.Exception) { Aow.Rtl.Rtl.ShowMessage("Exception occurred during OnExecuteEventLog event (TAoWCombatMapControl)"); }
    }
}
