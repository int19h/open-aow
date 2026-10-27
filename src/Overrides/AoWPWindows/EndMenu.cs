using Aow.Rtl;

namespace Aow.Units.AoWPWindows;

public partial class TPopupWindows
{
    // AoWPWindows.pas:73. Keep all three branches across the host boundary.
    public void EndMenuSelect(Aow.Units.AOWMenu.TAOWMenu sender)
    {
        switch (sender.FSelectedItem)
        {
            case 0:
                Aow.Units.E2Globals.E2Globals.DialogWin.Confirm(
                    Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.ConfirmRStr),
                    Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SurrenderYourEmpireRStr), 4);
                break;
            case 1:
                Aow.Units.AoWE.AoWE.AoWHSMap.ExitGame(Aow.Units.AoWE.TExitGameMode.egmTerminate, null);
                break;
            case 2:
                if (Aow.Units.AoWE.AoWE.AoWEngine.FDemo) Aow.Units.E2Globals.E2Globals.TitleWindow.ShowExitSplash();
                else Aow.Units.AoWUnit.AoWUnit.MainForm.CloseRequested();
                break;
        }
    }
}
