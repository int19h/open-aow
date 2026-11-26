using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// Ctrl+E and Ctrl+Enter on the strategic map. AoWHSMap.PAS:1004 called
// AoWHSMap.EndTurn from TAoWHSMapKeyboard.KeyDown. The port presses the
// end-turn button instead, the same path as a click, so the button shows its
// pressed frame and a second press cancels. A port addition the original does
// not have. Combat keeps its own Ctrl+E and Ctrl+Enter in MapWindowKeyDown.
public sealed partial class MapWindow
{
    private void PressEndTurnShortcut(int key, ShiftStates shift)
    {
        StrategicMap? strategicMap = UI.Engine.StrategicMap;
        if (strategicMap is null || UI.Engine.CombatMap is not null)
        {
            return;
        }

        HexSpriteMapKeyboard keyboard = strategicMap.Keyboard;
        if (keyboard.LockCount != 0 || keyboard.Mode != HexSpriteMapKeyboardMode.Default)
        {
            return;
        }

        if (!shift.Contains(ShiftStates.Ctrl) || (key is not 69 and not 13))
        {
            return;
        }

        _ = (UI.TaskBar?.WorldTurnButton?.Press());
    }
}
