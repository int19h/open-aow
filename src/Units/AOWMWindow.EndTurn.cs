using Aow.Rtl;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.AoWTC.AoWTCUnit;

namespace Aow.Units.AOWMWindow;

// Ctrl+E and Ctrl+Enter on the strategic map. AoWHSMap.PAS:1004 called
// AoWHSMap.EndTurn from TAoWHSMapKeyboard.KeyDown. The port presses the
// end-turn button instead, the same path as a click, so the button shows its
// pressed frame and a second press cancels. A port addition the original does
// not have. Combat keeps its own Ctrl+E and Ctrl+Enter in MapWindowKeyDown.
public partial class TMWindow
{
    private void PressEndTurnShortcut(ushort key, TSet<TShiftState> shift)
    {
        if (AoWHSMap == null || AoWCombatMap != null)
        {
            return;
        }

        var keyboard = AoWHSMap.FKeyboard;
        if (keyboard.FLockCount != 0 || keyboard.FMode != TAoWHSMapKeyboardMode.kmDefault)
        {
            return;
        }

        if (!shift.Contains(TShiftState.ssCtrl) || (key != 69 && key != 13))
        {
            return;
        }

        TBWindow?.TurnButton?.Press();
    }
}
