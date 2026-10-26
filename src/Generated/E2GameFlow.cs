// Generated from E2GameFlow.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.AoWTutorialWin;
using static Aow.Units.AoWTutorialWin.AoWTutorialWin;
using Aow.Units.AoWCampaignJoin;
using static Aow.Units.AoWCampaignJoin.AoWCampaignJoin;
using Aow.Units.AoWScenSetup;
using static Aow.Units.AoWScenSetup.AoWScenSetup;
using Aow.Units.AOWMultiWin;
using static Aow.Units.AOWMultiWin.AOWMultiWin;
using Aow.Units.AoWSpellBook;
using static Aow.Units.AoWSpellBook.AoWSpellBook;
using Aow.Units.aowSetupEvents;
using static Aow.Units.aowSetupEvents.aowSetupEvents;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;
using Aow.Units.AOWDialogWin;
using static Aow.Units.AOWDialogWin.AOWDialogWin;
using Aow.Units.AoWCampaignSettings;
using static Aow.Units.AoWCampaignSettings.AoWCampaignSettings;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AOWGauge;
using static Aow.Units.AOWGauge.AOWGauge;
using Aow.Units.AoWTitleScene;
using static Aow.Units.AoWTitleScene.AoWTitleScene;
using Aow.Units.AOWTitleWindow;
using static Aow.Units.AOWTitleWindow.AOWTitleWindow;
using Aow.Units.AoWCampaignTransferDlg;
using static Aow.Units.AoWCampaignTransferDlg.AoWCampaignTransferDlg;
using Aow.Units.AoWTargetSelection;
using static Aow.Units.AoWTargetSelection.AoWTargetSelection;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWLeaderSetup;
using static Aow.Units.AoWLeaderSetup.AoWLeaderSetup;
using Aow.Units.AoWFileDialog;
using static Aow.Units.AoWFileDialog.AoWFileDialog;
using Aow.Units.AoWIntroScene;
using static Aow.Units.AoWIntroScene.AoWIntroScene;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.E1Globals;
using static Aow.Units.E1Globals.E1Globals;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWSWindow;
using static Aow.Units.AoWSWindow.AoWSWindow;
using Aow.Units.AOWMWindow;
using static Aow.Units.AOWMWindow.AOWMWindow;
using Aow.Units.AoWTBWindow;
using static Aow.Units.AoWTBWindow.AoWTBWindow;
using Aow.Units.AoWPInfoWin;
using static Aow.Units.AoWPInfoWin.AoWPInfoWin;
using Aow.Units.aowHelpWin;
using static Aow.Units.aowHelpWin.aowHelpWin;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWSetup;
using static Aow.Units.AoWSetup.AoWSetup;
using Aow.Units.AoWComboBox;
using static Aow.Units.AoWComboBox.AoWComboBox;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWWorldMap;
using static Aow.Units.AoWWorldMap.AoWWorldMap;
using Aow.Units.E2CampaignSupport;
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;

namespace Aow.Units.E2GameFlow
{
    public static partial class E2GameFlow
    {
        // E2GameFlow.pas:594
        // { Unnamed executable flow routines called by E2. Original addresses are in E2.json. }
        public static void HideGameWindows(Aow.Units.AoWWinMan.TGeneral Controller)
        {
            int I = default;
            if (!Controller.FWindowsHidden)
            {
                Controller.FWindowsHidden = true;
                Controller.FInfoVisible = Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Visible;
                Controller.FRealmVisible = Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible;
                Controller.FMagicVisible = Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Visible;
                Controller.FUnitVisible = Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible;
                Controller.FTargetVisible = Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Visible;
                Controller.FScannerVisible = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Visible;
                Controller.FControlVisible = Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible;
                Controller.FChatVisible = Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Visible;
                Controller.FMapVisible = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible;
                {
                    I = 0;
                    int end__1 = Aow.Units.AoWWinMan.AoWWinMan.General.Manager.FChildren.Count - 1;
                    for (; I <= end__1; I += 1)
                    {
                        var call__2 = Aow.Units.AoWWinMan.AoWWinMan.General.Manager.FChildren[I];
                        if (call__2 != Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow)
                        {
                            var call__3 = Aow.Units.AoWWinMan.AoWWinMan.General.Manager.FChildren[I];
                            ((Aow.Units.AoWWindow.TAOWWindow)call__3).Visible = false;
                        }
                    }
                }
            }
        }

        // E2GameFlow.pas:632
        public static void ConfirmDialog(Aow.Units.AOWDialogWin.TDWin Dialog, Aow.Rtl.AnsiString Title, Aow.Rtl.AnsiString Message, byte Kind)
        {
            Dialog.FAction = unchecked((byte)Kind);
            Dialog.DTitle.Text = Title;
            Dialog.DMessage.Text = Message;
            Dialog.yesPnl.Visible = true;
            Dialog.okPnl.Visible = false;
            Dialog.Dialog.Visible = true;
        }

        // E2GameFlow.pas:472
        public static void ShowTitleScene(Aow.Units.AoWTitleScene.TTitleScene Scene, bool Music)
        {
            /* TODO(T-shim) Platform or editor boundary AoWUnit.Display */
            throw new System.NotImplementedException();
        }

        // E2GameFlow.pas:354
        public static bool StartLobby(Aow.Units.aowSetupEvents.TSetupEvents Events)
        {
            /* TODO(T-shim) Platform or editor boundary aowDPlayComp.CheckLobby */
            throw new System.NotImplementedException();
        }

        // E2GameFlow.pas:380
        public static void OpenSelectedGame(Aow.Units.AoWFileDialog.TFileDlgWindow Dialog)
        {
            Dialog.LoadSelectedGame(Dialog.FLastFileName);
        }

        // E2GameFlow.pas:385
        public static void SaveWindowLayout(Aow.Units.AoWWinMan.TGeneral Controller)
        {
            {
                var w__4 = Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin;
                var call__5 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Alignment.RightOffset, Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Alignment.BottomOffset, w__4.WinWidth, w__4.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(0, call__5);
            }

            {
                var w__6 = Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow;
                var call__7 = Aow.Rtl.Rtl.Rect(Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Alignment.RightOffset, Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Alignment.TopOffset, w__6.WinWidth, w__6.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(1, call__7);
            }

            {
                var w__8 = Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin;
                if (w__8.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awCenter)
                {
                    var call__9 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.LeftOffset, Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.TopOffset, w__8.WinWidth, w__8.WinHeight);
                    Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(2, call__9);
                }
                else
                {
                    var call__10 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                    Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(2, call__10);
                }
            }

            {
                var w__11 = Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg;
                if (w__11.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
                {
                    var call__12 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.LeftOffset, Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.TopOffset, w__11.WinWidth, w__11.WinHeight);
                    Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(5, call__12);
                }
                else
                {
                    var call__13 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                    Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(5, call__13);
                }
            }

            {
                var w__14 = Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg;
                if (w__14.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
                {
                    var call__15 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Alignment.LeftOffset, Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Alignment.TopOffset, w__14.WinWidth, w__14.WinHeight);
                    Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(13, call__15);
                }
                else
                {
                    var call__16 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                    Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(13, call__16);
                }
            }

            {
                var w__17 = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow;
                var call__18 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Alignment.RightOffset, Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Alignment.TopOffset, w__17.WinWidth, w__17.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(6, call__18);
            }

            {
                var w__19 = Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin;
                var call__20 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Alignment.RightOffset, Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Alignment.TopOffset, w__19.WinWidth, w__19.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(7, call__20);
            }

            {
                var w__21 = Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin;
                if (w__21.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
                {
                    var call__22 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Alignment.LeftOffset, Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Alignment.BottomOffset, w__21.WinWidth, w__21.WinHeight);
                    Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(8, call__22);
                }
                else
                {
                    var call__23 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                    Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(8, call__23);
                }
            }

            {
                var w__24 = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow;
                if (w__24.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
                {
                    var call__25 = Aow.Rtl.Rtl.Rect(Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.LeftOffset, Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.TopOffset, w__24.WinWidth, w__24.WinHeight);
                    Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(9, call__25);
                }
                else
                {
                    var call__26 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                    Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(9, call__26);
                }
            }

            {
                var w__27 = Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin;
                var call__28 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Alignment.RightOffset, Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Alignment.TopOffset, w__27.WinWidth, w__27.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(12, call__28);
            }

            {
                var w__29 = Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin;
                var call__30 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.Alignment.RightOffset, Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.Alignment.BottomOffset, w__29.WinWidth, w__29.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(10, call__30);
            }

            if (Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__31 = Aow.Rtl.Rtl.Rect(Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Left, Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Top, Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Right, Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Bottom);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(11, call__31);
            }
            else
            {
                var call__32 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(11, call__32);
            }
        }

        // E2GameFlow.pas:424
        public static void ShowTutorialMessage(Aow.Units.AoWTutorialWin.TTutorialWindow Dialog)
        {
            var call__33 = Dialog.MessageMemo.Items.Count;
            if (call__33 > 0)
            {
                Dialog.Dlg.Visible = true;
            }
        }

        // E2GameFlow.pas:436
        public static void ApplyVideoSettings(Aow.Units.AoWVideoDlg.TVideoDialog Dialog)
        {
            /* TODO(T-shim) Platform or editor boundary AoWUnit.Display */
            throw new System.NotImplementedException();
        }

        // E2GameFlow.pas:429
        public static void SetSceneSize(Aow.Units.AoWVideoDlg.TVideoDialog Dialog, int Width, int Height)
        {
            Dialog.FSceneWidth = Width;
            Dialog.FSceneHeight = Height;
            if (Dialog.FFullScreen == Aow.Units.AoWVideoDlg.TScreenMode.scmTitle)
            {
                Dialog.FModeChanged = true;
            }
        }

        // E2GameFlow.pas:81
        public static void HideScenarioSetup(Aow.Units.AoWScenSetup.TScenSetup Setup)
        {
            Setup.ScenSetupPnl.Visible = false;
            Setup.FSetupControl = null;
        }

        // E2GameFlow.pas:180
        public static void UpdateScenarioSetup(Aow.Units.AoWScenSetup.TScenSetup Setup)
        {
            /* TODO(T-shim) Platform or editor boundary aowDPlayComp.ConnectionType */
            throw new System.NotImplementedException();
        }

        // E2GameFlow.pas:45
        public static void StartCampaignJoin(Aow.Units.AoWCampaignJoin.TCampaignJoinDlg Dialog, int Faction)
        {
            Aow.Rtl.TStringList Lines = default;
            Aow.Units.AoWVideoDlg.TWMCampaign Campaign = default;
            int Index = default;
            int I = default;
            Aow.Units.AoWE.TAoWHSMap Preview = default;
            Dialog.ResetCampaignJoin();
            Dialog.FFaction = Faction;
            Dialog.FStage = 0;
            Lines = new Aow.Rtl.TStringList();
            switch (Dialog.FFaction)
            {
                case 0:
                    Aow.Units.E2GameFlow.E2GameFlow.LoadWorldCampaign(Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap, (AnsiString)"Keepers.cam");
                    Dialog.CharGenPnl.ILIndexTexture = 10;
                    Aow.Units.AoWE.AoWE.AoWHSSet.FTextControl.RetrieveText(10, Lines);
                    break;
                case 1:
                    Aow.Units.E2GameFlow.E2GameFlow.LoadWorldCampaign(Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap, (AnsiString)"Storm.cam");
                    Dialog.CharGenPnl.ILIndexTexture = 11;
                    Aow.Units.AoWE.AoWE.AoWHSSet.FTextControl.RetrieveText(11, Lines);
                    break;
            }

            var call__34 = Lines.Text;
            Aow.Units.ImageLib.ImageLib.ParseFontText(call__34, Dialog.FScrollText, Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey, 300);
            var call__35 = Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.GetCampaign();
            Campaign = call__35;
            var call__36 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(Campaign, 0);
            var call__37 = call__36.FPaths[0];
            var call__38 = Campaign.FScenarios.IndexOfID(call__37);
            Index = call__38;
            var call__39 = Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.GetCampaign();
            var call__40 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__39, Index);
            var call__41 = Aow.Units.AoWE.AoWE.AoWEngine.CreatePreviewHSM((Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory + ((AnsiString)"Scenario\\Campaign\\")) + call__40.FFileName);
            Preview = (Aow.Units.AoWE.TAoWHSMap)call__41;
            I = 0;
            // { Original uses the preview map child count as the player-search bound. }
            while (true)
            {
                var call__42 = Preview.Count;
                bool bool__43 = (I < call__42);
                if (bool__43)
                {
                    var call__44 = Preview.FPlayers.GetPlayers(I);
                    bool__43 = (((int)call__44.FPlayerType) != 0);
                }

                if (!bool__43)
                    break;
                {
                    I += 1;
                }
            }

            var call__45 = Preview.FPlayers.GetPlayers(I);
            Dialog.FLeader.Assign(call__45.FLeaderSetup, Aow.Units.AoWE.AoWE.AoWEngine);
            Preview.Free();
            Lines.Free();
        }

        // E2GameFlow.pas:498
        public static void LoadGame(Aow.Units.AoWWinMan.TGeneral Controller, Aow.Rtl.AnsiString FileName)
        {
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                Aow.Units.AoWE.AoWE.AoWEngine.CloseHSM(null);
            }

            var call__46 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.LoadingRStr);
            var call__47 = Aow.Rtl.Rtl.ExtractFileName(FileName);
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Title.Text = ((call__46 + ((AnsiString)": ")) + call__47) + ((AnsiString)"...");
            Aow.Units.E2Globals.E2Globals.ProgressDialog.StatusLbl.Text = (AnsiString)"";
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Bar.Position = 0;
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Bar.Max = 100;
            Aow.Units.E2Globals.E2Globals.ProgressDialog.ProgDlg.Visible = true;
            if (!Aow.Units.E2Globals.E2Globals.TitleScene.Window.Visible)
            {
                Controller.Manager.OverlayPriority = 5000;
            }

            Aow.Units.Sound.Sound.SongPlayer.Stop(false);
            Aow.Units.Sound.Sound.SongPlayer.Songs.Clear();
            Aow.Units.Sound.Sound.FadeMusicVolume(0, 1);
            Aow.Units.AoWE.AoWE.AoWEngine.LoadHSM(FileName, false, Controller.MapProgress);
            if ((Aow.Units.AoWE.AoWE.AoWEngine.FDemo && (Aow.Units.AoWE.AoWE.AoWHSMap != null)) && !Aow.Units.AoWE.AoWE.AoWHSMap.FShowAllPlayers)
            {
                Aow.Units.E2GameFlow.E2GameFlow.CloseGame(Controller);
            }

            Controller.Manager.OverlayPriority = 0;
            Aow.Units.E2Globals.E2Globals.ProgressDialog.ProgDlg.Visible = false;
        }

        // E2GameFlow.pas:516
        public static void StartGame(Aow.Units.AoWWinMan.TGeneral Controller)
        {
            /* TODO(T-shim) Platform or editor boundary aowDPlayComp.Connected */
            throw new System.NotImplementedException();
        }

        // E2GameFlow.pas:616
        public static void CloseGame(Aow.Units.AoWWinMan.TGeneral Controller)
        {
            /* TODO(T-shim) Platform or editor boundary SysUtils.SetCurrentDir */
            throw new System.NotImplementedException();
        }

        // E2GameFlow.pas:642
        public static bool HasModalWindow(Aow.Units.AoWWinMan.TGeneral Controller)
        {
            bool Result = default;
            int I = default;
            Aow.Units.AoWWindow.TAOWWindow Window = default;
            Result = false;
            I = Controller.Manager.FChildren.Count - 1;
            while (I >= 0)
            {
                var call__48 = Controller.Manager.FChildren[I];
                Window = (Aow.Units.AoWWindow.TAOWWindow)call__48;
                if (((Window != Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow) && Window.Modal) && Window.Visible)
                {
                    Result = true;
                    I = -1;
                }
                else
                {
                    I -= 1;
                }
            }

            return Result;
        }

        // E2GameFlow.pas:29
        public static void InitializeCampaignJoin(Aow.Units.AoWCampaignJoin.TCampaignJoinDlg Dialog)
        {
            Dialog.CharGenPnl.Visible = true;
            Dialog.LeaderTypePnl.Visible = false;
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.Visible = true;
            Dialog.ScrollTextDB.Visible = true;
            Dialog.FScrollDelay = 0;
            Dialog.FScrollPosition = 0;
            Dialog.FScrollText.Clear();
        }

        // E2GameFlow.pas:40
        public static void LoadWorldCampaign(Aow.Units.AoWWorldMap.TWorldMap Dialog, Aow.Rtl.AnsiString Name)
        {
            Aow.Units.AoWE.AoWE.AoWEngine.LoadCampaign((Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory + ((AnsiString)"Scenario\\Campaign\\")) + Name);
        }

        // E2GameFlow.pas:87
        public static void UpdateScenarioRow(Aow.Units.AoWScenSetup.TScenSetup Setup, Aow.Units.AoWSetup.TPlayerSetupSettings Settings, Aow.Units.AoWComboBox.TAOWComboBox PlayerBox, Aow.Units.AoWComboBox.TAOWComboBox BehaviorBox, Aow.Units.AoWImage.TAOWImage Icon, Aow.Units.AoWLabel.TAOWLabel PlayerName, Aow.Units.AoWLabel.TAOWLabel Info, Aow.Units.AoWPanel.TAOWPanel Fog)
        {
            Aow.Units.AoWSetup.TSetupPlayerBehavior Behavior = default;
            byte B = default;
            int I = default;
            bool Visible = default;
            bool Found = default;
            if (Settings != null)
            {
                var call__49 = Setup.FSetupControl.GetHost();
                Visible = call__49 && !Settings.FStatus.Contains(Aow.Units.AoWSetup.TPlayerSetupStatusElement.pssFixed);
                if (Visible != PlayerBox.Visible)
                {
                    PlayerBox.Visible = Visible;
                }

                if (PlayerBox.Visible)
                {
                    PlayerBox.Items.BeginUpdate();
                    PlayerBox.Items.Clear();
                    Setup.FSetupControl.ListAvailablePlayers(PlayerBox.Items);
                    PlayerBox.Items.EndUpdate();
                    Found = false;
                    var call__50 = PlayerBox.Items.Count;
                    I = call__50;
                    while (!Found && (I != 0))
                    {
                        I -= 1;
                        var call__51 = PlayerBox.Items[I];
                        var call__52 = Setup.FSetupControl.PlayerNameToPlayerID(call__51, ref Behavior);
                        if (call__52)
                        {
                            Found = (Settings.FBehavior.PlayerType == Behavior.PlayerType) && (Settings.FBehavior.Flags == Behavior.Flags);
                        }
                    }

                    if (Found)
                    {
                        PlayerBox.Selected = I;
                    }
                    else
                    {
                        PlayerBox.Selected = -1;
                    }

                    PlayerName.Visible = false;
                }
                else
                {
                    PlayerName.Visible = true;
                    if ((Settings.FBehavior.PlayerType == 0) && (Settings.FBehavior.Flags != 0))
                    {
                        var call__53 = Setup.FSetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(Settings.FBehavior.Flags);
                        I = call__53;
                        if (I != -1)
                        {
                            var call__54 = Setup.FSetupControl.FSettings.FRemotePlayers.GetPlayers(I);
                            PlayerName.Text = call__54.FName;
                        }
                        else
                        {
                            var call__55 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HumanRStr);
                            PlayerName.Text = call__55;
                        }
                    }
                    else
                    {
                        var call__56 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayerTypeRStr[Settings.FBehavior.PlayerType]);
                        PlayerName.Text = call__56;
                    }
                }

                Info.Text = (AnsiString)"";
                var call__57 = Setup.FSetupControl.GetRemote();
                Info.Visible = call__57 || Settings.FStatus.Contains(Aow.Units.AoWSetup.TPlayerSetupStatusElement.pssFixed);
                if (Info.Visible)
                {
                    if (Settings.FStatus.Contains(Aow.Units.AoWSetup.TPlayerSetupStatusElement.pssFixed))
                    {
                        var call__58 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FixedRStr);
                        Info.Text = call__58;
                    }
                    else
                    {
                        var call__59 = Setup.FSetupControl.GetRemote();
                        if ((call__59 && Settings.FStatus.Contains(Aow.Units.AoWSetup.TPlayerSetupStatusElement.pssForced)) && (Settings.FBehavior.PlayerType == 0))
                        {
                            var call__60 = Setup.FSetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(Settings.FBehavior.Flags);
                            I = call__60;
                            if (I != -1)
                            {
                                // { Original compares race bytes, including the FF sentinel. }
                                var call__61 = Setup.FSetupControl.FSettings.FRemotePlayers.GetPlayers(I);
                                if (call__61.FPreferredRace == unchecked((byte)Settings.FRace))
                                {
                                    var call__62 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TakenRStr);
                                    Info.Text = call__62;
                                }
                                else
                                {
                                    var call__63 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ForcedRStr);
                                    Info.Text = call__63;
                                }
                            }
                        }
                        else
                        {
                            if (Settings.FBehavior.Flags != 0)
                            {
                                var call__64 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TakenRStr);
                                Info.Text = call__64;
                            }
                            else
                            {
                                if (TSet<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType))
                                {
                                    var call__65 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayerBehaviorRStr[(int)((Aow.Units.AoWE.TPlayerBehavior)Settings.FBehaviorMode)]);
                                    Info.Text = call__65;
                                }
                            }
                        }
                    }
                }

                var call__66 = Setup.FSetupControl.GetHost();
                BehaviorBox.Visible = call__66 && TSet<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType);
                if (BehaviorBox.Visible)
                {
                    BehaviorBox.Items.BeginUpdate();
                    BehaviorBox.Items.Clear();
                    {
                        int first__70 = (int)0;
                        int end__67 = (int)5;
                        for (int ordinal__69 = first__70; ordinal__69 <= end__67; ordinal__69++)
                        {
                            B = (byte)ordinal__69;
                            var call__68 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayerBehaviorRStr[(int)((Aow.Units.AoWE.TPlayerBehavior)B)]);
                            BehaviorBox.Items.AddObject(call__68, Aow.Rtl.Rtl.IntToObject((int)B));
                        }
                    }

                    BehaviorBox.Items.EndUpdate();
                    BehaviorBox.Selected = Settings.FBehaviorMode;
                }

                Icon.Visible = TSet<byte>.WithBounds(0, 255).WithRange(0, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType);
                if (Icon.Visible)
                {
                    switch (Settings.FBehavior.PlayerType)
                    {
                        case 1:
                            Icon.CurrentImage = 1;
                            break;
                        case 2:
                            Icon.CurrentImage = 2;
                            break;
                        case 3:
                            Icon.CurrentImage = 3;
                            break;
                        case 6:
                            Icon.CurrentImage = 4;
                            break;
                        case 7:
                            Icon.CurrentImage = 5;
                            break;
                        default:
                            Icon.CurrentImage = 0;
                            break;
                    }
                }

                Fog.Visible = Settings.FStatus.Contains(Aow.Units.AoWSetup.TPlayerSetupStatusElement.pssFixed);
            }
            else
            {
                Icon.Visible = false;
                PlayerBox.Visible = false;
                BehaviorBox.Visible = false;
                PlayerName.Visible = false;
                Info.Visible = true;
                var call__71 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UnavailableRStr);
                Info.Text = call__71;
                Fog.Visible = true;
            }
        }

        // E2GameFlow.pas:298
        public static void InitializeScenarioSetup(Aow.Units.AoWScenSetup.TScenSetup Setup, Aow.Units.AoWSetup.TSetupControl Control)
        {
            byte B = default;
            {
                var w__72 = Setup;
                w__72.FSetupControl = Control;
                Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.Visible = true;
                w__72.ScenSetupPnl.Visible = true;
                w__72.ChatMemo.Items.Clear();
                w__72.ChatEdit.Text = (AnsiString)"";
                w__72.FValidateDelay = 100;
                w__72.FPingDelay = 15;
                var call__73 = w__72.FSetupControl.GetRemote();
                w__72.ChatFogPnl.Visible = !call__73;
                var call__74 = w__72.FSetupControl.GetRemote();
                w__72.ChatMemo.CaptureInput = call__74;
                var call__75 = w__72.FSetupControl.GetRemote();
                w__72.ChatEdit.CaptureInput = call__75;
                var call__76 = w__72.FSetupControl.GetHost();
                if (call__76 && (w__72.FSetupControl.FSettings.FMapName == ((AnsiString)"")))
                {
                    w__72.PickMap();
                }

                var call__77 = w__72.FSetupControl.GetRemote();
                if (call__77)
                {
                    w__72.TacticalCombatCB.Visible = true;
                    w__72.TacticalCombatLbl.Visible = true;
                    w__72.TacticalCombatCB.Items.Clear();
                    {
                        int first__81 = (int)0;
                        int end__78 = (int)2;
                        for (int ordinal__80 = first__81; ordinal__80 <= end__78; ordinal__80++)
                        {
                            B = (byte)ordinal__80;
                            var call__79 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TTacticalCombatAvailabilityRStr[B]);
                            w__72.TacticalCombatCB.Items.AddObject(call__79, Aow.Rtl.Rtl.IntToObject((int)B));
                        }
                    }
                }
                else
                {
                    w__72.TacticalCombatCB.Visible = false;
                    w__72.TacticalCombatLbl.Visible = false;
                }

                if (w__72.FSetupControl.FSettings.FConnectionType == 2)
                {
                    w__72.CustomizeLeadersLbl.TextColor = 8421504;
                    w__72.CustomizeLeaders.CaptureInput = false;
                    w__72.PlayModeCB.Items.Clear();
                    var call__82 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ClassicRStr);
                    w__72.PlayModeCB.Items.AddObject(call__82, Aow.Rtl.Rtl.IntToObject((int)1));
                }
                else
                {
                    w__72.CustomizeLeadersLbl.TextColor = w__72.AlliedVictoryLbl.TextColor;
                    w__72.CustomizeLeaders.CaptureInput = true;
                    w__72.PlayModeCB.Items.Clear();
                    var call__83 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SimultaneousRStr);
                    w__72.PlayModeCB.Items.AddObject(call__83, Aow.Rtl.Rtl.IntToObject((int)2));
                    var call__84 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ClassicRStr);
                    w__72.PlayModeCB.Items.AddObject(call__84, Aow.Rtl.Rtl.IntToObject((int)1));
                }

                bool bool__85 = (Aow.Units.Sound.Sound.SongPlayer.CurrentIndex != -1);
                if (bool__85)
                {
                    var call__86 = Aow.Units.Sound.Sound.SongPlayer.Songs.Count;
                    bool__85 = (Aow.Units.Sound.Sound.SongPlayer.CurrentIndex < call__86);
                }

                bool bool__87 = bool__85;
                if (bool__87)
                {
                    var call__88 = Aow.Units.Sound.Sound.SongPlayer.Songs[Aow.Units.Sound.Sound.SongPlayer.CurrentIndex];
                    bool__87 = (call__88 == ((AnsiString)"SONGS\\TITLE.IT"));
                }

                if (!bool__87)
                {
                    Aow.Units.Sound.Sound.SongPlayer.Songs.Clear();
                    Aow.Units.Sound.Sound.SongPlayer.Songs.Add((AnsiString)"SONGS\\TITLE.IT");
                    Aow.Units.Sound.Sound.SongPlayer.Play(0);
                }

                Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioSetup(Setup);
            }
        }
    }
}
