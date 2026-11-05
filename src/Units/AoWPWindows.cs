// Translated from AoWPWindows.pas.
using Aow.Rtl;
using Aow.Units.AOWMenu;
using Aow.Units.StartMenu;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using Aow.Units.AoWSoundDlg;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.E2Globals.E2Globals;
using static Aow.Units.E3Globals.E3Globals;
using static Aow.Units.AoWFileDialog.AoWFileDialog;
using static Aow.Units.E3EventDialogs.E3EventDialogs;
using Aow.Platform;
using Aow.Units.AoWE;

namespace Aow.Units.AoWPWindows;

// published menu components
public partial class TPopupWindows : TDataModule
{
    public TPopupWindows(TComponent owner) : base(owner)
    { }

    public TStartMenu StartMenu;
    public TAOWMenu OverViewsMenu;
    public TAOWMenu SettingsMenu;
    public TAOWMenu EndMenu;
    public TAOWMenu DiskMenu;
    public TStartMenu TCStartMenu;
    public TIvTranslator Translator;
    public TAOWMenu HelpMenu;
    public TAOWMenu TCOVMenu;
    public TAOWMenu TCDiskMenu;
    public TAOWMenu TCHelpMenu;
    public TAOWMenu TCSettingsMenu;
    public TAOWMenu TCEndMenu;
    // AoWPWindows.pas:61
    public void SettingsMenuSelect(TAOWMenu Sender)
    {
        switch (Sender.FSelectedItem)
        {
            case 0:
                VideoDialog.ShowModes();
                break;
            case 1:
                AoWPWindows.ShowSoundSettings(SoundDlg);
                break;
            case 2:
                GamePlayDialog.GamePlayDlg.Visible = true;
                break;
            case 3:
                General.ResetLayout();
                break;
            case 4:
                ShowEmailSettings(EmailSettingsDlg);
                break;
            case 5:
                DialogWin.DebugWin.Visible = true;
                break;
        }
    }

    // AoWPWindows.pas:83
    public void DiskMenuSelect(TAOWMenu Sender)
    {
        switch (Sender.FSelectedItem)
        {
            case 0:
                FileDlgWindow.FMode = 0;
                FileDlgWindow.FileDialog.Visible = true;
                break;
            case 1:
                FileDlgWindow.FMode = 1;
                FileDlgWindow.FileDialog.Visible = true;
                break;
        }
    }

    // AoWPWindows.pas:91
    public void OverViewsMenuSelect(TAOWMenu Sender)
    {
        switch (Sender.FSelectedItem)
        {
            case 0:
                // { Original opens the information book. }
                ShowInformationBook(AoWWorldMap.AoWWorldMap.WorldMap, null, null);
                break;
            case 1:
                if (AoWEngine.FCampaign != null)
                {
                    AoWWorldMap.AoWWorldMap.WorldMap.ShowCampaignHistory();
                }

                break;
        }
    }

    // AoWPWindows.pas:99
    public void TCEndMenuSelect(TAOWMenu Sender)
    {
        switch (Sender.FSelectedItem)
        {
            case 0:
                var quitCombat = TranslateRStr(QuitCombatRStr);
                var confirm = TranslateRStr(E2Globals.E2Globals.ConfirmRStr);
                DialogWin.Confirm(confirm, quitCombat, 6);
                break;
        }
    }

    // AoWPWindows.pas:107
    public void HelpMenuSelect(TAOWMenu Sender)
    {
        /* TODO: Implement Forms.Handle in the platform or editor service. */
        throw new NotImplementedException();
    }

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

public static partial class AoWPWindows
{
    // AoWPWindows.pas:47
    public static void ShowVideoSettings(TVideoDialog Dialog)
    {
        Dialog.Dlg.Visible = true;
        Dialog.UpdateModes();
    }

    // AoWPWindows.pas:53
    public static void ShowSoundSettings(TSoundDlg Dialog)
    {
        Dialog.AddBtn.Visible = false;
        Dialog.FindFiles();
        Dialog.SoundSetup();
        Dialog.Dlg.Visible = true;
    }
}
