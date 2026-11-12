using Aow.Game.Session;

namespace Aow.Game.UI.Dialogs;

public partial class TaskBar
{
    // Original writes Alignment.RightOffset and FAligned
    public void UpdatePlayerIndicator()
    {
        int count = 0;
        for (int i = 0; i < World.Map.StrategicMap.Current.Players.GetCount(); i++)
            if (World.Map.StrategicMap.Current.Players.GetPlayers(i).Status == PlayerStatus.Playing) count++;
        PlayerIndicatorFrame.WinWidth = count * 5 + 4;
        PlayerIndicatorFrame.Aligned = false;
        WorldMessageEdit.Alignment.RightOffset = PlayerIndicatorFrame.WinWidth + 278;
        WorldMessageEdit.Aligned = false;
    }

    // The companion routine at sizes the tactical indicator and message field
    public void UpdateCombatPlayerIndicator()
    {
        int count = Combat.CombatMap.Current.Combat.CombatData.TurnOrder.Count;
        CombatPlayerIndicatorFrame.WinWidth = count * 5 + 4;
        CombatPlayerIndicatorFrame.Aligned = false;
        CombatMessageEdit.Alignment.RightOffset = CombatPlayerIndicatorFrame.WinWidth + 278;
        CombatMessageEdit.Aligned = false;
    }
}
