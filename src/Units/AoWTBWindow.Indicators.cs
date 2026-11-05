using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AoWTBWindow;

public partial class TTBWindow
{
    // Original writes Alignment.RightOffset and FAligned
    public void UpdatePlayerIndicator()
    {
        int count = 0;
        for (int i = 0; i < AoWHSMap.FPlayers.GetCount(); i++)
            if (AoWHSMap.FPlayers.GetPlayers(i).FStatus == TPlayerStatus.psPlaying) count++;
        PInd.WinWidth = count * 5 + 4;
        PInd.FAligned = false;
        TBMessEdit.Alignment.RightOffset = PInd.WinWidth + 278;
        TBMessEdit.FAligned = false;
    }

    // The companion routine at sizes the tactical indicator and message field
    public void UpdateCombatPlayerIndicator()
    {
        int count = AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FTurnOrder.Count;
        TCPInd.WinWidth = count * 5 + 4;
        TCPInd.FAligned = false;
        TCMsg.Alignment.RightOffset = TCPInd.WinWidth + 278;
        TCMsg.FAligned = false;
    }
}
