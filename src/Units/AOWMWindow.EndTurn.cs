using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

// Ctrl+E and Ctrl+Enter on the strategic map. AoWHSMap.PAS:1004 called
// AoWHSMap.EndTurn from TAoWHSMapKeyboard.KeyDown. The port presses the
// end-turn button instead, the same path as a click, so the button shows its
// pressed frame and a second press cancels. A port addition the original does
// not have. Combat keeps its own Ctrl+E and Ctrl+Enter in MapWindowKeyDown.
public partial class MWindow
{
    private void PressEndTurnShortcut(ushort key, Set<ShiftState> shift)
    {
        if (Aow.Game.World.Map.AoWHSMap.Current == null || Aow.Game.Combat.AoWCombatMap.Current != null)
        {
            return;
        }

        var keyboard = Aow.Game.World.Map.AoWHSMap.Current.FKeyboard;
        if (keyboard.FLockCount != 0 || keyboard.FMode != Aow.Game.World.Map.AoWHSMapKeyboard.Mode.Default)
        {
            return;
        }

        if (!shift.Contains(ShiftState.Ctrl) || (key != 69 && key != 13))
        {
            return;
        }

        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow?.TurnButton?.Press();
    }
}
