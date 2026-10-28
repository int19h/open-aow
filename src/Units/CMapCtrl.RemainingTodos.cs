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
            var info = sender is Aow.Units.AoWE.ExecuteEventLogNotification notification
                ? notification.Info : Aow.Rtl.Rtl.ReadLegacyRecord<TExecuteEventLogInfo>(sender);
            FOnExecuteEventLog(info.Log, info.Done);
        }
        catch (System.Exception) { Aow.Rtl.Rtl.ShowMessage("Exception occurred during OnExecuteEventLog event (TAoWCombatMapControl)"); }
    }
}
