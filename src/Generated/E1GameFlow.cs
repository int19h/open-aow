// Generated from E1GameFlow.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWScenSetup;
using static Aow.Units.AoWScenSetup.AoWScenSetup;
using Aow.Units.AoWSelFactionWin;
using static Aow.Units.AoWSelFactionWin.AoWSelFactionWin;
using Aow.Units.AoWWorldMap;
using static Aow.Units.AoWWorldMap.AoWWorldMap;
using Aow.Units.AOWMultiWin;
using static Aow.Units.AOWMultiWin.AOWMultiWin;
using Aow.Units.AOWTitleWindow;
using static Aow.Units.AOWTitleWindow.AOWTitleWindow;
using Aow.Units.AOWPyreScrn;
using static Aow.Units.AOWPyreScrn.AOWPyreScrn;
using Aow.Units.AoWMagic;
using static Aow.Units.AoWMagic.AoWMagic;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWFileDialog;
using static Aow.Units.AoWFileDialog.AoWFileDialog;
using Aow.Units.AoWTitleScene;
using static Aow.Units.AoWTitleScene.AoWTitleScene;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.E1Globals;
using static Aow.Units.E1Globals.E1Globals;
using Aow.Units.E1Resources;
using static Aow.Units.E1Resources.E1Resources;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using Aow.Units.E2CampaignSupport;
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;
using Aow.Units.AoWReg;
using static Aow.Units.AoWReg.AoWReg;
using Aow.Units.aowSetupEvents;
using static Aow.Units.aowSetupEvents.aowSetupEvents;

namespace Aow.Units.E1GameFlow
{
    public static partial class E1GameFlow
    {
        // E1GameFlow.pas:28
        // { Unnamed executable game-flow routines used by E1. }
        public static void ShowMagic(Aow.Units.AoWMagic.TMagicWin Window)
        {
            if (!Window.Dlg.Visible)
            {
                Aow.Units.E3Globals.E3Globals.RefreshMagicWindow(Window);
                Aow.Units.E3Globals.E3Globals.RefreshGlobalMagic(Window);
            }

            Window.Dlg.Visible = true;
        }

        // E1GameFlow.pas:38
        public static void HideMagic(Aow.Units.AoWMagic.TMagicWin Window)
        {
            if (Window.Dlg.Visible)
            {
                Window.Dlg.Visible = false;
            }
        }

        // E1GameFlow.pas:211
        public static void FinishPyre(Aow.Units.AOWPyreScrn.TPyreScreen Dialog)
        {
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            Aow.Units.EventLog.TGameOverEventLog Event = default;
            Aow.Units.Sound.Sound.SetSFXVolume(Aow.Units.AoWExeForms.AoWExeForms.SoundDlg.SFXVolume.Position);
            Dialog.ClearFire();
            if (Dialog.FSavedSongStatus != null)
            {
                Aow.Units.Sound.Sound.RestoreSongStatus(Dialog.FSavedSongStatus);
                Dialog.FSavedSongStatus.Free();
                Dialog.FSavedSongStatus = null;
            }

            Dialog.FGameOverEvent.Handled();
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
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
        public static void ShowExitSplash(Aow.Units.AOWTitleWindow.TTitleWindow Dialog)
        {
            Aow.Units.E1GameFlow.E1GameFlow.ShowSplash(Dialog);
            Dialog.SplashPnl.ILIndexTexture = 2;
            Dialog.FSplashStage = 10;
            Dialog.FSplashTimer = 300;
        }

        // E1GameFlow.pas:43
        public static void HideGameWindows(Aow.Units.AoWWinMan.TGeneral Controller)
        {
            int I = default;
            {
                var w__1 = Controller;
                if (!w__1.FWindowsHidden)
                {
                    w__1.FWindowsHidden = true;
                    w__1.FInfoVisible = Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Visible;
                    w__1.FRealmVisible = Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible;
                    w__1.FMagicVisible = Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Visible;
                    w__1.FUnitVisible = Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible;
                    w__1.FTargetVisible = Aow.Units.AoWWorldMap.AoWWorldMap.TargetSelectionDlg.Dlg.Visible;
                    w__1.FScannerVisible = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Visible;
                    w__1.FControlVisible = Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible;
                    w__1.FChatVisible = Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Visible;
                    w__1.FMapVisible = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible;
                    {
                        I = 0;
                        int end__2 = Aow.Units.AoWWinMan.AoWWinMan.General.Manager.FChildren.Count - 1;
                        for (; I <= end__2; I += 1)
                        {
                            var call__3 = Aow.Units.AoWWinMan.AoWWinMan.General.Manager.FChildren[I];
                            if (call__3 != Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow)
                            {
                                var call__4 = Aow.Units.AoWWinMan.AoWWinMan.General.Manager.FChildren[I];
                                ((Aow.Units.AoWWindow.TAOWWindow)call__4).Visible = false;
                            }
                        }
                    }
                }
            }
        }

        // E1GameFlow.pas:66
        public static void RestoreGameWindows(Aow.Units.AoWWinMan.TGeneral Controller)
        {
            {
                var w__5 = Controller;
                if (w__5.FWindowsHidden)
                {
                    w__5.FWindowsHidden = false;
                    Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Visible = w__5.FInfoVisible;
                    Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible = w__5.FRealmVisible;
                    Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Visible = w__5.FMagicVisible;
                    Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Visible = w__5.FScannerVisible;
                    Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible = w__5.FControlVisible;
                    Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible = w__5.FMapVisible;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = w__5.FUnitVisible;
                    Aow.Units.AoWWorldMap.AoWWorldMap.TargetSelectionDlg.Dlg.Visible = w__5.FTargetVisible;
                    Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Visible = w__5.FChatVisible;
                }
            }
        }

        // E1GameFlow.pas:84
        public static void ExitToTitle(Aow.Units.AoWWinMan.TGeneral Controller)
        {
            /* TODO(T-shim) Platform or editor boundary SysUtils.SetCurrentDir */
            throw new System.NotImplementedException();
        }

        // E1GameFlow.pas:100
        public static void SaveGame(Aow.Units.AoWWinMan.TGeneral Controller, Aow.Rtl.AnsiString Name)
        {
            Aow.Units.AoWE.AoWE.AoWHSMap.SaveGame(Name);
        }

        // E1GameFlow.pas:105
        public static void SaveCampaign(Aow.Units.AoWWinMan.TGeneral Controller, Aow.Rtl.AnsiString Name)
        {
            // { Original campaign-save command uses the map save routine. }
            Aow.Units.AoWE.AoWE.AoWHSMap.SaveGame(Name);
        }

        // E1GameFlow.pas:110
        public static void PickScenarioMap(Aow.Units.AoWScenSetup.TScenSetup Dialog, Aow.Rtl.AnsiString Name)
        {
            var call__6 = Aow.Rtl.Rtl.GetCurrentDir();
            var call__7 = Aow.Rtl.Rtl.ExtractRelativePath(Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory, (call__6 + AnsiString.FromBytes(92)) + Name);
            Name = call__7;
            Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FileDialog.Visible = false;
            Dialog.FSetupControl.hPickMap(Name);
        }

        // E1GameFlow.pas:118
        public static void ShowMultiplayer(Aow.Units.AOWMultiWin.TMultiWindow Dialog, Aow.Rtl.AnsiString Name)
        {
            /* TODO(T-shim) Platform or editor boundary aowDPlayComp.Connected */
            throw new System.NotImplementedException();
        }

        // E1GameFlow.pas:154
        public static void ShowCampaignMap(Aow.Units.AoWWorldMap.TWorldMap Dialog)
        {
            /* TODO(T-shim) Platform or editor boundary SysUtils.SetCurrentDir */
            throw new System.NotImplementedException();
        }

        // E1GameFlow.pas:179
        public static void ShowSplash(Aow.Units.AOWTitleWindow.TTitleWindow Dialog)
        {
            Aow.Units.AoWExeForms.AoWExeForms.SetVideoSize(Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog, 640, 480);
            Aow.Units.AoWExeForms.AoWExeForms.SetVideoMode(Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog, Aow.Units.AoWVideoDlg.TScreenMode.scmTitle);
            Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog.ApplyBtnClick();
            Dialog.TitleWin.Visible = true;
            Dialog.TitlePnl.Visible = false;
            Dialog.SplashPnl.ILTexture = Dialog.DemoIL;
            Dialog.SplashPnl.ILIndexTexture = 0;
            Dialog.SplashPnl.Visible = true;
            Dialog.FSplashTimer = 450;
            Dialog.FSplashStage = 0;
        }

        // E1GameFlow.pas:202
        public static void ClearPyre(Aow.Units.AOWPyreScrn.TPyreScreen Dialog)
        {
            if (Dialog.FAnimation != null)
            {
                Dialog.FAnimation.Free();
                Dialog.FAnimation = null;
            }
        }
    }
}
