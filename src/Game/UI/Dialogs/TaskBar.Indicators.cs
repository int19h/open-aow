#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Game.Session;

namespace Aow.Game.UI.Dialogs;

public sealed partial class TaskBar
{
    // Original writes Alignment.RightOffset and FAligned
    public void UpdatePlayerIndicator()
    {
        int count = 0;
        World.Map.StrategicMap strategicMap = UI.Engine.StrategicMap;
        for (int i = 0; i < strategicMap.Players.GetCount(); i++)
        {
            if (strategicMap.Players.GetPlayers(i).Status == PlayerStatus.Playing)
            {
                count++;
            }
        }

        PlayerIndicatorFrame.WinWidth = (count * 5) + 4;
        PlayerIndicatorFrame.Aligned = false;
        WorldMessageEdit.Alignment.RightOffset = PlayerIndicatorFrame.WinWidth + 278;
        WorldMessageEdit.Aligned = false;
    }

    // The companion routine at sizes the tactical indicator and message field
    public void UpdateCombatPlayerIndicator()
    {
        int count = UI.Engine.CombatMap.Combat.CombatData.TurnOrder.Count;
        CombatPlayerIndicatorFrame.WinWidth = (count * 5) + 4;
        CombatPlayerIndicatorFrame.Aligned = false;
        CombatMessageEdit.Alignment.RightOffset = CombatPlayerIndicatorFrame.WinWidth + 278;
        CombatMessageEdit.Aligned = false;
    }
}
