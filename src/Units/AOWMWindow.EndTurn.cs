using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

// Ctrl+E and Ctrl+Enter on the strategic map. AoWHSMap.PAS:1004 called
// AoWHSMap.EndTurn from TAoWHSMapKeyboard.KeyDown. The port presses the
// end-turn button instead, the same path as a click, so the button shows its
// pressed frame and a second press cancels. A port addition the original does
// not have. Combat keeps its own Ctrl+E and Ctrl+Enter in MapWindowKeyDown.
public partial class TMWindow
{
    private void PressEndTurnShortcut(ushort key, TSet<TShiftState> shift)
    {
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap == null || Aow.Game.Combat.TAoWCombatMap.AoWCombatMap != null)
        {
            return;
        }

        var keyboard = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FKeyboard;
        if (keyboard.FLockCount != 0 || keyboard.FMode != TAoWHSMapKeyboardMode.kmDefault)
        {
            return;
        }

        if (!shift.Contains(TShiftState.ssCtrl) || (key != 69 && key != 13))
        {
            return;
        }

        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow?.TurnButton?.Press();
    }
}
