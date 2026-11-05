// Translated from E1GameFlow.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWScenSetup;
using Aow.Units.AoWWorldMap;
using static Aow.Units.AoWWorldMap.AoWWorldMap;
using Aow.Units.AOWMultiWin;
using Aow.Units.AOWTitleWindow;
using Aow.Units.AOWPyreScrn;
using Aow.Units.AoWMagic;
using Aow.Units.EventLog;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWWindow;
using static Aow.Units.E3Globals.E3Globals;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.AoWFileDialog.AoWFileDialog;
using static Aow.Units.Sound.Sound;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;

namespace Aow.Units.E1GameFlow
{
    public static partial class E1GameFlow
    {
        // E1GameFlow.pas:28
        // { Unnamed executable game-flow routines used by E1. }
        public static void ShowMagic(TMagicWin Window)
        {
            if (!Window.Dlg.Visible)
            {
                RefreshMagicWindow(Window);
                RefreshGlobalMagic(Window);
            }

            Window.Dlg.Visible = true;
        }

        // E1GameFlow.pas:38
        public static void HideMagic(TMagicWin Window)
        {
            if (Window.Dlg.Visible)
            {
                Window.Dlg.Visible = false;
            }
        }

        // E1GameFlow.pas:211
        public static void FinishPyre(TPyreScreen Dialog)
        {
            TExecuteDoneEvent Done;
            TGameOverEventLog Event;
            SetSFXVolume(SoundDlg.SFXVolume.Position);
            Dialog.ClearFire();
            if (Dialog.FSavedSongStatus != null)
            {
                RestoreSongStatus(Dialog.FSavedSongStatus);
                Dialog.FSavedSongStatus.Free();
                Dialog.FSavedSongStatus = null;
            }

            Dialog.FGameOverEvent.Handled();
            General.Manager.OverlayPriority = 0;
            Done = Dialog.FDone;
            Event = Dialog.FGameOverEvent;
            Dialog.FDone = null;
            Dialog.FGameOverEvent = null;
            if (Done != null)
            {
                Done(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }
        }

        // E1GameFlow.pas:193
        public static void ShowExitSplash(TTitleWindow Dialog)
        {
            ShowSplash(Dialog);
            Dialog.SplashPnl.ILIndexTexture = 2;
            Dialog.FSplashStage = 10;
            Dialog.FSplashTimer = 300;
        }

        // E1GameFlow.pas:43
        public static void HideGameWindows(TGeneral Controller)
        {
            if (!Controller.FWindowsHidden)
            {
                Controller.FWindowsHidden = true;
                Controller.FInfoVisible = AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Visible;
                Controller.FRealmVisible = AoWRealm.AoWRealm.RealmWin.Dlg.Visible;
                Controller.FMagicVisible = MagicWin.Dlg.Visible;
                Controller.FUnitVisible = AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible;
                Controller.FTargetVisible = TargetSelectionDlg.Dlg.Visible;
                Controller.FScannerVisible = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Visible;
                Controller.FControlVisible = AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible;
                Controller.FChatVisible = ChatWindow.ChatWin.Visible;
                Controller.FMapVisible = AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible;
                for (int I = 0, last = General.Manager.FChildren.Count - 1; I <= last; I++)
                {
                    if (General.Manager.FChildren[I] != TBWindow.TaskWindow)
                    {
                        ((TAOWWindow)General.Manager.FChildren[I]).Visible = false;
                    }
                }
            }
        }

        // E1GameFlow.pas:66
        public static void RestoreGameWindows(TGeneral Controller)
        {
            if (Controller.FWindowsHidden)
            {
                Controller.FWindowsHidden = false;
                AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Visible = Controller.FInfoVisible;
                AoWRealm.AoWRealm.RealmWin.Dlg.Visible = Controller.FRealmVisible;
                MagicWin.Dlg.Visible = Controller.FMagicVisible;
                AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Visible = Controller.FScannerVisible;
                AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible = Controller.FControlVisible;
                AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible = Controller.FMapVisible;
                AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = Controller.FUnitVisible;
                TargetSelectionDlg.Dlg.Visible = Controller.FTargetVisible;
                ChatWindow.ChatWin.Visible = Controller.FChatVisible;
            }
        }

        // E1GameFlow.pas:84
        public static void ExitToTitle(TGeneral Controller)
        {
            AoWEngine.CloseHSM(null);
            AoWEngine.CloseCampaign();
            AoWSWindow.AoWSWindow.SWindow.Scanner.SetMap(null);
            TCScanner.Scanner.SetMap(null);
            SongPlayer.Stop(false);
            SongPlayer.Songs.Clear();
            SetCurrentDir(AoWEngine.StartupDirectory);
            HideGameWindows(General);
            TBWindow.TaskWindow.Visible = false;
            E2Globals.E2Globals.TitleWindow.TitleWin.Visible = false;
            E2Globals.E2Globals.TitleScene.ResetScene();
            E2Globals.E2Globals.TitleScene.ShowScene(true);
        }

        // E1GameFlow.pas:100
        public static void SaveGame(TGeneral Controller, AnsiString Name)
        {
            AoWHSMap.SaveGame(Name);
        }

        // E1GameFlow.pas:105
        public static void SaveCampaign(TGeneral Controller, AnsiString Name)
        {
            // { Original campaign-save command uses the map save routine. }
            AoWHSMap.SaveGame(Name);
        }

        // E1GameFlow.pas:110
        public static void PickScenarioMap(TScenSetup Dialog, AnsiString Name)
        {
            var currentDir = GetCurrentDir();
            Name = ExtractRelativePath(AoWEngine.StartupDirectory, currentDir + AnsiString.FromBytes(92) + Name);
            FileDlgWindow.FileDialog.Visible = false;
            Dialog.FSetupControl.hPickMap(Name);
        }

        // E1GameFlow.pas:118
        public static void ShowMultiplayer(TMultiWindow Dialog, AnsiString Name)
        {
            /* TODO: Implement aowDPlayComp.DisConnect in the platform or editor service. */
            throw new NotImplementedException();
        }

        // E1GameFlow.pas:154
        public static void ShowCampaignMap(TWorldMap Dialog)
        {
            AnsiString Song = default;
            Dialog.ResetCampaign();
            AoWEngine.CloseHSM(null);
            E2Globals.E2Globals.TitleWindow.TitleWin.Visible = true;
            E2Globals.E2Globals.TitleWindow.TitlePnl.Visible = false;
            Dialog.WorldPnl.Visible = true;
            Dialog.FPathFrame = 0;
            Dialog.FPathDelay = 5;
            Dialog.DescLbl.Text = (AnsiString)"";
            General.Manager.OverlayPriority = 249;
            SongPlayer.Stop(false);
            SetCurrentDir(AoWEngine.StartupDirectory);
            var campaign = Dialog.GetCampaign();
            if (GetCampaignScenario(Dialog.GetCampaign(), campaign.FCurrentScenarioID).FImageLibrary == (AnsiString)"")
            {
                Song = (AnsiString)"Songs\\Title.IT";
            }
            else
            {
                var dialogCampaign = Dialog.GetCampaign();
                Song = (AnsiString)"Songs\\" + GetCampaignScenario(Dialog.GetCampaign(), dialogCampaign.FCurrentScenarioID).FImageLibrary;
            }

            PlaySongEx(Song, 1, 0, true);
            var campaign2 = Dialog.GetCampaign();
            Dialog.PPawn.CurrentImage = campaign2.FFaction + 7;
            var campaign3 = Dialog.GetCampaign();
            Dialog.EPawn.CurrentImage = 8 - campaign3.FFaction;
            Dialog.UpdateCampaignState();
        }

        // E1GameFlow.pas:179
        public static void ShowSplash(TTitleWindow Dialog)
        {
            SetVideoSize(VideoDialog, 640, 480);
            SetVideoMode(VideoDialog, TScreenMode.scmTitle);
            VideoDialog.ApplyBtnClick();
            Dialog.TitleWin.Visible = true;
            Dialog.TitlePnl.Visible = false;
            Dialog.SplashPnl.ILTexture = Dialog.DemoIL;
            Dialog.SplashPnl.ILIndexTexture = 0;
            Dialog.SplashPnl.Visible = true;
            Dialog.FSplashTimer = 450;
            Dialog.FSplashStage = 0;
        }

        // E1GameFlow.pas:202
        public static void ClearPyre(TPyreScreen Dialog)
        {
            if (Dialog.FAnimation != null)
            {
                Dialog.FAnimation.Free();
                Dialog.FAnimation = null;
            }
        }
    }
}
