using Aow.Game.Session;

namespace Aow.Game.Ui.Dialogs;

public partial class TBWindow
{
    // Original writes Alignment.RightOffset and FAligned
    public void UpdatePlayerIndicator()
    {
        int count = 0;
        for (int i = 0; i < Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetCount(); i++)
            if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(i).FStatus == Aow.Game.Session.Player.Status.Playing) count++;
        PInd.WinWidth = count * 5 + 4;
        PInd.FAligned = false;
        TBMessEdit.Alignment.RightOffset = PInd.WinWidth + 278;
        TBMessEdit.FAligned = false;
    }

    // The companion routine at sizes the tactical indicator and message field
    public void UpdateCombatPlayerIndicator()
    {
        int count = Aow.Game.Combat.AoWCombatMap.Current.FCombat.FCombatData.FTurnOrder.Count;
        TCPInd.WinWidth = count * 5 + 4;
        TCPInd.FAligned = false;
        TCMsg.Alignment.RightOffset = TCPInd.WinWidth + 278;
        TCMsg.FAligned = false;
    }
}
