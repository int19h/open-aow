using Aow.Platform;
using Aow.Units.AoWE;
using Aow.Units.AOWMenu;

namespace Aow.Units.AoWPWindows;

public partial class TPopupWindows
{
    // AoWPWindows.pas:73. Keep all three branches across the host boundary.
    public void EndMenuSelect(TAOWMenu sender)
    {
        switch (sender.FSelectedItem)
        {
            case 0:
                E2Globals.E2Globals.DialogWin.Confirm(
                    AoWE.AoWE.TranslateRStr(E2Globals.E2Globals.ConfirmRStr),
                    AoWE.AoWE.TranslateRStr(AoWE.AoWE.SurrenderYourEmpireRStr), 4);
                break;
            case 1:
                AoWE.AoWE.AoWHSMap.ExitGame(TExitGameMode.egmTerminate, null);
                break;
            case 2:
                if (AoWE.AoWE.AoWEngine.FDemo) E2Globals.E2Globals.TitleWindow.ShowExitSplash();
                else GameHost.MainForm.CloseRequested();
                break;
        }
    }
}
