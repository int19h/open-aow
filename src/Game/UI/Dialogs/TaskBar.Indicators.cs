using Aow.Game.Session;

namespace Aow.Game.UI.Dialogs;

public partial class TaskBar
{
    // Original writes Alignment.RightOffset and FAligned
    public void UpdatePlayerIndicator()
    {
        int count = 0;
        for (int i = 0; i < World.Map.AoWHSMap.Current.FPlayers.GetCount(); i++)
            if (World.Map.AoWHSMap.Current.FPlayers.GetPlayers(i).FStatus == Player.Status.Playing) count++;
        PInd.WinWidth = count * 5 + 4;
        PInd.FAligned = false;
        TBMessEdit.Alignment.RightOffset = PInd.WinWidth + 278;
        TBMessEdit.FAligned = false;
    }

    // The companion routine at sizes the tactical indicator and message field
    public void UpdateCombatPlayerIndicator()
    {
        int count = Combat.CombatMap.Current.FCombat.FCombatData.FTurnOrder.Count;
        TCPInd.WinWidth = count * 5 + 4;
        TCPInd.FAligned = false;
        TCMsg.Alignment.RightOffset = TCPInd.WinWidth + 278;
        TCMsg.FAligned = false;
    }
}
