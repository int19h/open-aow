using Aow.Game.Session;

namespace Aow.Game.UI.Dialogs;

public partial class TaskBar
{
    // Original writes Alignment.RightOffset and FAligned
    public void UpdatePlayerIndicator()
    {
        int count = 0;
        for (int i = 0; i < World.Map.AoWHSMap.Current.Players.GetCount(); i++)
            if (World.Map.AoWHSMap.Current.Players.GetPlayers(i).Status == PlayerStatus.Playing) count++;
        PInd.WinWidth = count * 5 + 4;
        PInd.Aligned = false;
        TBMessEdit.Alignment.RightOffset = PInd.WinWidth + 278;
        TBMessEdit.Aligned = false;
    }

    // The companion routine at sizes the tactical indicator and message field
    public void UpdateCombatPlayerIndicator()
    {
        int count = Combat.CombatMap.Current.Combat.CombatData.TurnOrder.Count;
        TCPInd.WinWidth = count * 5 + 4;
        TCPInd.Aligned = false;
        TCMsg.Alignment.RightOffset = TCPInd.WinWidth + 278;
        TCMsg.Aligned = false;
    }
}
