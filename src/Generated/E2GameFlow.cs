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
            int I = default;
            int HostIndex = default;
            Aow.Rtl.AnsiString HostName = default;
            Aow.Rtl.AnsiString Connection = default;
            Aow.Units.AoWSetup.TRemotePlayer Player = default;
            Aow.Units.AoWSetup.TSetupControl Control = default;
            {
                var w__34 = Setup;
                if (w__34.FSetupControl != null)
                {
                    Control = w__34.FSetupControl;
                    w__34.PlayerList.Items.Clear();
                    w__34.PingList.Items.Clear();
                    w__34.PreferredRaceList.Items.Clear();
                    {
                        I = 0;
                        int end__35 = Control.FSettings.FRemotePlayers.Count - 1;
                        for (; I <= end__35; I += 1)
                        {
                            var call__36 = Control.FSettings.FRemotePlayers.GetPlayers(I);
                            Player = call__36;
                            w__34.PlayerList.Items.AddObject(Player.FName, Aow.Rtl.Rtl.IntToObject((int)Player.FRemoteID));
                            if (Player.FPreferredRace != 255)
                            {
                                var call__37 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[unchecked((sbyte)Player.FPreferredRace)]);
                                w__34.PreferredRaceList.Items.Add(call__37);
                            }
                            else
                            {
                                var call__38 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoneRStr);
                                w__34.PreferredRaceList.Items.Add(call__38);
                            }

                            w__34.PingList.Items.AddObject((AnsiString)"", Aow.Rtl.Rtl.IntToObject((int)Player.FRemoteID));
                        }
                    }

                    var call__39 = w__34.PlayerList.Items.Count;
                    w__34.PlayerListSB.Max = call__39 - 1;
                    w__34.SelectPlayer(w__34.PlayerList.Selected);
                    w__34.CustomizeLeaders.Checked = w__34.FSetupControl.FSettings.FCustomizeLeaders;
                    var call__40 = w__34.PlayModeCB.Items.IndexOfObject(Aow.Rtl.Rtl.IntToObject((int)w__34.FSetupControl.FSettings.FPlayMode));
                    w__34.PlayModeCB.Selected = call__40;
                    w__34.Exploration.Checked = w__34.FSetupControl.FSettings.FExplorationEnabled;
                    w__34.TurnTimeSl.Position = w__34.FSetupControl.FSettings.FTurnTime;
                    if (w__34.FSetupControl.FSettings.FDayLimit != 0)
                    {
                        w__34.TurnsSl.Position = w__34.FSetupControl.FSettings.FDayLimit / 10;
                    }
                    else
                    {
                        w__34.TurnsSl.Position = 0;
                    }

                    w__34.UpdateTurnLimit();
                    w__34.UpdateTurnTime();
                    var call__41 = w__34.FSetupControl.GetHost();
                    bool bool__42 = call__41;
                    if (bool__42)
                    {
                        var call__43 = w__34.FSetupControl.GetPlayerSetupValid();
                        bool__42 = call__43;
                    }

                    w__34.StartBtn.Visible = bool__42;
                    var call__44 = w__34.FSetupControl.GetHost();
                    w__34.PickScenarioBtn.Visible = call__44;
                    var call__45 = w__34.FSetupControl.GetHost();
                    w__34.SettingsFogPnl.Visible = !call__45;
                    w__34.RestartSettingsFogPnl.Visible = !w__34.SettingsFogPnl.Visible && (w__34.FSetupControl.FSettings.FMapDay != 0);
                    w__34.LeadersOnMapCB.Checked = w__34.FSetupControl.FSettings.FLeadersOnMap;
                    w__34.AlliedVictoryCB.Checked = w__34.FSetupControl.FSettings.FAlliedVictory;
                    var call__46 = w__34.TacticalCombatCB.Items.IndexOfObject(Aow.Rtl.Rtl.IntToObject((int)w__34.FSetupControl.FSettings.FTacticalCombatAvailability));
                    w__34.TacticalCombatCB.Selected = call__46;
                    w__34.AlwaysViewTCCB.Visible = w__34.TacticalCombatCB.Visible && (w__34.FSetupControl.FSettings.FTacticalCombatAvailability != 0);
                    w__34.AlwaysViewTCCB.Checked = w__34.FSetupControl.FSettings.FAlwaysViewTC;
                    w__34.AlwaysViewTCLbl.Visible = w__34.AlwaysViewTCCB.Visible;
                    var call__47 = w__34.FSetupControl.GetRemote();
                    w__34.PreferredRaceLbl.Visible = call__47;
                    var call__48 = w__34.FSetupControl.GetRemote();
                    w__34.PreferredRaceCB.Visible = call__48;
                    if (w__34.PreferredRaceCB.Visible)
                    {
                        w__34.PreferredRaceCB.Items.Clear();
                        var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoneRStr);
                        w__34.PreferredRaceCB.Items.AddObject(call__49, Aow.Rtl.Rtl.IntToObject((int)-1));
                        {
                            I = 0;
                            int end__50 = w__34.FSetupControl.FSettings.Count - 1;
                            for (; I <= end__50; I += 1)
                            {
                                var call__51 = w__34.FSetupControl.FSettings.GetSettings(I);
                                if (!call__51.FStatus.Contains(Aow.Units.AoWSetup.TPlayerSetupStatusElement.pssFixed))
                                {
                                    var call__52 = w__34.FSetupControl.FSettings.GetSettings(I);
                                    var call__53 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[call__52.FRace]);
                                    var call__54 = w__34.FSetupControl.FSettings.GetSettings(I);
                                    w__34.PreferredRaceCB.Items.AddObject(call__53, Aow.Rtl.Rtl.IntToObject((int)call__54.FRace));
                                }
                            }
                        }

                        {
                            var w__55 = w__34.PreferredRaceCB;
                            w__55.NumberVisible = 5;
                            w__55.FSized = false;
                            var call__56 = w__55.Items.IndexOfObject(Aow.Rtl.Rtl.IntToObject((int)unchecked((sbyte)w__34.FSetupControl.FRemotePlayer.FPreferredRace)));
                            w__55.Selected = call__56;
                        }
                    }

                    var call__57 = Control.FSettings.IndexOfRace(0);
                    var call__58 = Control.FSettings.GetSettings(call__57);
                    Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__58, w__34.R1PlayerCB, w__34.R1BehaviorCB, w__34.R1PlayerIcon, w__34.R1PlayerLbl, w__34.R1InfoLbl, w__34.R1FogPnl);
                    var call__59 = Control.FSettings.IndexOfRace(1);
                    var call__60 = Control.FSettings.GetSettings(call__59);
                    Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__60, w__34.R2PlayerCB, w__34.R2BehaviorCB, w__34.R2PlayerIcon, w__34.R2PlayerLbl, w__34.R2InfoLbl, w__34.R2FogPnl);
                    var call__61 = Control.FSettings.IndexOfRace(2);
                    var call__62 = Control.FSettings.GetSettings(call__61);
                    Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__62, w__34.R3PlayerCB, w__34.R3BehaviorCB, w__34.R3PlayerIcon, w__34.R3PlayerLbl, w__34.R3InfoLbl, w__34.R3FogPnl);
                    var call__63 = Control.FSettings.IndexOfRace(3);
                    var call__64 = Control.FSettings.GetSettings(call__63);
                    Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__64, w__34.R4PlayerCB, w__34.R4BehaviorCB, w__34.R4PlayerIcon, w__34.R4PlayerLbl, w__34.R4InfoLbl, w__34.R4FogPnl);
                    var call__65 = Control.FSettings.IndexOfRace(4);
                    var call__66 = Control.FSettings.GetSettings(call__65);
                    Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__66, w__34.R5PlayerCB, w__34.R5BehaviorCB, w__34.R5PlayerIcon, w__34.R5PlayerLbl, w__34.R5InfoLbl, w__34.R5FogPnl);
                    var call__67 = Control.FSettings.IndexOfRace(5);
                    var call__68 = Control.FSettings.GetSettings(call__67);
                    Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__68, w__34.R6PlayerCB, w__34.R6BehaviorCB, w__34.R6PlayerIcon, w__34.R6PlayerLbl, w__34.R6InfoLbl, w__34.R6FogPnl);
                    var call__69 = Control.FSettings.IndexOfRace(6);
                    var call__70 = Control.FSettings.GetSettings(call__69);
                    Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__70, w__34.R7PlayerCB, w__34.R7BehaviorCB, w__34.R7PlayerIcon, w__34.R7PlayerLbl, w__34.R7InfoLbl, w__34.R7FogPnl);
                    var call__71 = Control.FSettings.IndexOfRace(7);
                    var call__72 = Control.FSettings.GetSettings(call__71);
                    Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__72, w__34.R8PlayerCB, w__34.R8BehaviorCB, w__34.R8PlayerIcon, w__34.R8PlayerLbl, w__34.R8InfoLbl, w__34.R8FogPnl);
                    var call__73 = Control.FSettings.IndexOfRace(8);
                    var call__74 = Control.FSettings.GetSettings(call__73);
                    Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__74, w__34.R9PlayerCB, w__34.R9BehaviorCB, w__34.R9PlayerIcon, w__34.R9PlayerLbl, w__34.R9InfoLbl, w__34.R9FogPnl);
                    var call__75 = Control.FSettings.IndexOfRace(9);
                    var call__76 = Control.FSettings.GetSettings(call__75);
                    Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__76, w__34.R10PlayerCB, w__34.R10BehaviorCB, w__34.R10PlayerIcon, w__34.R10PlayerLbl, w__34.R10InfoLbl, w__34.R10FogPnl);
                    var call__77 = Control.FSettings.IndexOfRace(10);
                    var call__78 = Control.FSettings.GetSettings(call__77);
                    Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__78, w__34.R11PlayerCB, w__34.R11BehaviorCB, w__34.R11PlayerIcon, w__34.R11PlayerLbl, w__34.R11InfoLbl, w__34.R11FogPnl);
                    var call__79 = Control.FSettings.IndexOfRace(11);
                    var call__80 = Control.FSettings.GetSettings(call__79);
                    Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__80, w__34.R12PlayerCB, w__34.R12BehaviorCB, w__34.R12PlayerIcon, w__34.R12PlayerLbl, w__34.R12InfoLbl, w__34.R12FogPnl);
                    var call__81 = w__34.FSetupControl.GetRemote();
                    if (call__81)
                    {
                        switch (Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.DPlay.ConnectionType)
                        {
                            case Aow.Rtl.TConnectionType.conTCP:
                                Connection = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TCPIPRStr);
                                break;
                            case Aow.Rtl.TConnectionType.conIPX:
                                Connection = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.IPXRStr);
                                break;
                            case Aow.Rtl.TConnectionType.conModem:
                                Connection = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ModemRStr);
                                break;
                            case Aow.Rtl.TConnectionType.conSerial:
                                Connection = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SerialRStr);
                                break;
                            default:
                                Connection = (AnsiString)"";
                                break;
                        }

                        {
                            var w__82 = w__34.FSetupControl.FSettings;
                            var call__83 = w__82.FRemotePlayers.IndexOfRemoteID(w__82.FHostRemoteID);
                            HostIndex = call__83;
                        }

                        if (HostIndex != -1)
                        {
                            var call__84 = w__34.FSetupControl.FSettings.FRemotePlayers.GetPlayers(HostIndex);
                            HostName = call__84.FName;
                        }
                        else
                        {
                            HostName = AnsiString.FromBytes(63);
                        }

                        w__34.HostNameLbl.Visible = true;
                        w__34.FileNameLbl.Visible = true;
                        w__34.PlayerNameLbl.Visible = true;
                        var call__85 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XHostYRStr);
                        var call__86 = Aow.Units.AoWE.AoWE.Format(call__85, [Connection, HostName]);
                        w__34.HostNameLbl.Text = call__86;
                        var call__87 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PlayerNameXRStr);
                        var call__88 = Aow.Units.AoWE.AoWE.Format(call__87, [w__34.FSetupControl.FRemotePlayer.FName]);
                        w__34.PlayerNameLbl.Text = call__88;
                        var call__89 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FileNameXRStr);
                        var call__90 = Aow.Units.AoWE.AoWE.Format(call__89, [w__34.FSetupControl.FSettings.FMapName]);
                        w__34.FileNameLbl.Text = call__90;
                        if (w__34.FSetupControl.FRemotePlayer.FMapValidation == Aow.Units.AoWSetup.TMapValidationInfo.mviInvalid)
                        {
                            w__34.FileNameLbl.TextColor = 255;
                        }
                        else
                        {
                            w__34.FileNameLbl.TextColor = 10329501;
                        }
                    }
                    else
                    {
                        w__34.HostNameLbl.Visible = false;
                        w__34.FileNameLbl.Visible = false;
                        w__34.PlayerNameLbl.Visible = false;
                    }

                    var call__91 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ScenarioXRStr);
                    var call__92 = Aow.Units.AoWE.AoWE.Format(call__91, [w__34.FSetupControl.FSettings.FFileName]);
                    w__34.ScenarioLbl.Text = call__92;
                    Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
                }
            }
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

            var call__93 = Lines.Text;
            Aow.Units.ImageLib.ImageLib.ParseFontText(call__93, Dialog.FScrollText, Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey, 300);
            var call__94 = Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.GetCampaign();
            Campaign = call__94;
            var call__95 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(Campaign, 0);
            var call__96 = call__95.FPaths[0];
            var call__97 = Campaign.FScenarios.IndexOfID(call__96);
            Index = call__97;
            var call__98 = Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.GetCampaign();
            var call__99 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__98, Index);
            var call__100 = Aow.Units.AoWE.AoWE.AoWEngine.CreatePreviewHSM((Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory + ((AnsiString)"Scenario\\Campaign\\")) + call__99.FFileName);
            Preview = (Aow.Units.AoWE.TAoWHSMap)call__100;
            I = 0;
            // { Original uses the preview map child count as the player-search bound. }
            while (true)
            {
                var call__101 = Preview.Count;
                bool bool__102 = (I < call__101);
                if (bool__102)
                {
                    var call__103 = Preview.FPlayers.GetPlayers(I);
                    bool__102 = (((int)call__103.FPlayerType) != 0);
                }

                if (!bool__102)
                    break;
                {
                    I += 1;
                }
            }

            var call__104 = Preview.FPlayers.GetPlayers(I);
            Dialog.FLeader.Assign(call__104.FLeaderSetup, Aow.Units.AoWE.AoWE.AoWEngine);
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

            var call__105 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.LoadingRStr);
            var call__106 = Aow.Rtl.Rtl.ExtractFileName(FileName);
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Title.Text = ((call__105 + ((AnsiString)": ")) + call__106) + ((AnsiString)"...");
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
            int I = default;
            Controller.FWindowsHidden = false;
            Aow.Units.Sound.Sound.SongPlayer.Songs.Clear();
            var call__107 = Aow.Units.AoWE.AoWE.AoWHSMap.FGameSongs.Songs.Count;
            if (call__107 != 0)
            {
                Aow.Units.Sound.Sound.SongPlayer.Songs.AddStrings(Aow.Units.AoWE.AoWE.AoWHSMap.FGameSongs.Songs);
            }
            else
            {
                var call__108 = Aow.Units.AoWE.AoWE.AoWHSMap.FStartupSongs.Songs.Count;
                if (call__108 != 0)
                {
                    {
                        I = 0;
                        int end__109 = Aow.Units.AoWE.AoWE.AoWHSMap.FStartupSongs.Songs.Count - 1;
                        for (; I <= end__109; I += 1)
                        {
                            var call__110 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.FileNotFoundRStr);
                            var call__111 = Aow.Units.AoWE.AoWE.AoWHSMap.FStartupSongs.SongNames[I];
                            if (Aow.Rtl.Rtl.CompareText(call__111, call__110) != 0)
                            {
                                var call__112 = Aow.Units.AoWE.AoWE.AoWHSMap.FStartupSongs.Songs[I];
                                Aow.Units.Sound.Sound.SongPlayer.Songs.Add(call__112);
                            }
                        }
                    }
                }
                else
                {
                    Aow.Units.Sound.Sound.SongPlayer.Songs.Add((AnsiString)"Songs\\Heartwoo.it");
                }
            }

            Aow.Units.Sound.Sound.SongPlayer.Play(0);
            Aow.Units.Sound.Sound.SetMusicVolume(Aow.Units.AoWExeForms.AoWExeForms.SoundDlg.MusicVolume.Position);
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.StartMenuBar.Items.Clear();
            var call__113 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.BackgroundRStr);
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.StartMenuBar.Items.Add(call__113);
            if (Aow.Units.AoWE.AoWE.AoWEngine.FCampaign != null)
            {
                var call__114 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CampaignRStr);
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.StartMenuBar.Items.Add(call__114);
            }

            Aow.Units.E2Globals.E2Globals.TitleScene.Window.Visible = false;
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.Visible = false;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.Visible = true;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible = true;
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Visible = true;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SetHSMap(Aow.Units.AoWE.AoWE.AoWHSMap);
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetMap(Aow.Units.AoWE.AoWE.AoWHSMap);
            var call__115 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[0];
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetLevel(call__115);
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TurnButton.FButtonChecked = false;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.ChatBtn.Visible = Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.DPlay.Connected;
            Controller.Manager.FAligned = false;
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.FPreviousWidth = 0;
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.FIncremental = false;
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.FSized = false;
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerTab.Items.Clear();
            var call__116 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.SurfaceRStr);
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerTab.Items.Add(call__116);
            if (Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.LevelCount > 1)
            {
                var call__117 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.CavernsRStr);
                Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerTab.Items.Add(call__117);
            }

            if (Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.LevelCount > 2)
            {
                var call__118 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.DepthsRStr);
                Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerTab.Items.Add(call__118);
            }

            Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerTab.Selected = 0;
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerTab.FSized = false;
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetTWidth(0);
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetTHeight(0);
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetTLeft(-1);
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetTTop(-1);
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.FUpdateStep = -1;
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetMap(Aow.Units.AoWE.AoWE.AoWHSMap);
            var call__119 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[0];
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetLevel(call__119);
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SetHSMap(Aow.Units.AoWE.AoWE.AoWHSMap);
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneWidth = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.WinWidth - 24;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneHeight = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.WinHeight - 24;
            Aow.Units.AoWE.AoWE.AoWHSMap.Activate();
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible = false;
            Aow.Units.aowMapEvents.aowMapEvents.MapEvents.TheMapArmyDeselected(null);
            Aow.Units.aowMapEvents.aowMapEvents.MapEvents.TheMapStructureDeselected(null);
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TBWorldUpdate(null);
            Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.UpdatePlayer();
            if (Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msActivated))
            {
                {
                    var w__120 = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TurnButton;
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == -1)
                    {
                        w__120.FCurPlayer = 12;
                        w__120.FLastPlayer = 12;
                        w__120.FAnimationFrame = w__120.FCurPlayer * 7;
                        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.AutoBtn.Visible = false;
                    }
                    else
                    {
                        var call__121 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        w__120.FLastPlayer = call__121.FPlayerRace;
                        if (w__120.FLastPlayer == -1)
                        {
                            w__120.FLastPlayer = 12;
                        }

                        w__120.FCurPlayer = w__120.FLastPlayer;
                        w__120.FAnimationFrame = w__120.FLastPlayer * 7;
                        Aow.Units.AoWExeForms.AoWExeForms.TBWindow.AutoBtn.Visible = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode == Aow.Units.AoWE.TPlayMode.pmSimultaneous;
                    }
                }
            }

            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.FWorldClockDelay = 0;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.ChatBtn.TextColor = 14;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.FChatFlash = 0;
            Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog.ChangeResolution(Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog.FDisplayWidth, Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog.FDisplayHeight);
        }

        // E2GameFlow.pas:616
        public static void CloseGame(Aow.Units.AoWWinMan.TGeneral Controller)
        {
            Aow.Units.AoWE.AoWE.AoWEngine.CloseHSM(null);
            Aow.Units.AoWE.AoWE.AoWEngine.CloseCampaign();
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetMap(null);
            Aow.Units.AoWExeForms.AoWExeForms.TCScanner.Scanner.SetMap(null);
            Aow.Units.Sound.Sound.SongPlayer.Stop(false);
            Aow.Units.Sound.Sound.SongPlayer.Songs.Clear();
            Aow.Rtl.Rtl.SetCurrentDir(Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory);
            Aow.Units.E2GameFlow.E2GameFlow.HideGameWindows(Aow.Units.AoWWinMan.AoWWinMan.General);
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.Visible = false;
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.Visible = false;
            Aow.Units.E2Globals.E2Globals.TitleScene.ResetScene();
            Aow.Units.E2GameFlow.E2GameFlow.ShowTitleScene(Aow.Units.E2Globals.E2Globals.TitleScene, true);
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
                var call__122 = Setup.FSetupControl.GetHost();
                Visible = call__122 && !Settings.FStatus.Contains(Aow.Units.AoWSetup.TPlayerSetupStatusElement.pssFixed);
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
                    var call__123 = PlayerBox.Items.Count;
                    I = call__123;
                    while (!Found && (I != 0))
                    {
                        I -= 1;
                        var call__124 = PlayerBox.Items[I];
                        var call__125 = Setup.FSetupControl.PlayerNameToPlayerID(call__124, ref Behavior);
                        if (call__125)
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
                        var call__126 = Setup.FSetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(Settings.FBehavior.Flags);
                        I = call__126;
                        if (I != -1)
                        {
                            var call__127 = Setup.FSetupControl.FSettings.FRemotePlayers.GetPlayers(I);
                            PlayerName.Text = call__127.FName;
                        }
                        else
                        {
                            var call__128 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HumanRStr);
                            PlayerName.Text = call__128;
                        }
                    }
                    else
                    {
                        var call__129 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayerTypeRStr[Settings.FBehavior.PlayerType]);
                        PlayerName.Text = call__129;
                    }
                }

                Info.Text = (AnsiString)"";
                var call__130 = Setup.FSetupControl.GetRemote();
                Info.Visible = call__130 || Settings.FStatus.Contains(Aow.Units.AoWSetup.TPlayerSetupStatusElement.pssFixed);
                if (Info.Visible)
                {
                    if (Settings.FStatus.Contains(Aow.Units.AoWSetup.TPlayerSetupStatusElement.pssFixed))
                    {
                        var call__131 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FixedRStr);
                        Info.Text = call__131;
                    }
                    else
                    {
                        var call__132 = Setup.FSetupControl.GetRemote();
                        if ((call__132 && Settings.FStatus.Contains(Aow.Units.AoWSetup.TPlayerSetupStatusElement.pssForced)) && (Settings.FBehavior.PlayerType == 0))
                        {
                            var call__133 = Setup.FSetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(Settings.FBehavior.Flags);
                            I = call__133;
                            if (I != -1)
                            {
                                // { Original compares race bytes, including the FF sentinel. }
                                var call__134 = Setup.FSetupControl.FSettings.FRemotePlayers.GetPlayers(I);
                                if (call__134.FPreferredRace == unchecked((byte)Settings.FRace))
                                {
                                    var call__135 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TakenRStr);
                                    Info.Text = call__135;
                                }
                                else
                                {
                                    var call__136 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ForcedRStr);
                                    Info.Text = call__136;
                                }
                            }
                        }
                        else
                        {
                            if (Settings.FBehavior.Flags != 0)
                            {
                                var call__137 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TakenRStr);
                                Info.Text = call__137;
                            }
                            else
                            {
                                if (TSet<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType))
                                {
                                    var call__138 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayerBehaviorRStr[(int)((Aow.Units.AoWE.TPlayerBehavior)Settings.FBehaviorMode)]);
                                    Info.Text = call__138;
                                }
                            }
                        }
                    }
                }

                var call__139 = Setup.FSetupControl.GetHost();
                BehaviorBox.Visible = call__139 && TSet<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType);
                if (BehaviorBox.Visible)
                {
                    BehaviorBox.Items.BeginUpdate();
                    BehaviorBox.Items.Clear();
                    {
                        int first__143 = (int)0;
                        int end__140 = (int)5;
                        for (int ordinal__142 = first__143; ordinal__142 <= end__140; ordinal__142++)
                        {
                            B = (byte)ordinal__142;
                            var call__141 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayerBehaviorRStr[(int)((Aow.Units.AoWE.TPlayerBehavior)B)]);
                            BehaviorBox.Items.AddObject(call__141, Aow.Rtl.Rtl.IntToObject((int)B));
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
                var call__144 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UnavailableRStr);
                Info.Text = call__144;
                Fog.Visible = true;
            }
        }

        // E2GameFlow.pas:298
        public static void InitializeScenarioSetup(Aow.Units.AoWScenSetup.TScenSetup Setup, Aow.Units.AoWSetup.TSetupControl Control)
        {
            byte B = default;
            {
                var w__145 = Setup;
                w__145.FSetupControl = Control;
                Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.Visible = true;
                w__145.ScenSetupPnl.Visible = true;
                w__145.ChatMemo.Items.Clear();
                w__145.ChatEdit.Text = (AnsiString)"";
                w__145.FValidateDelay = 100;
                w__145.FPingDelay = 15;
                var call__146 = w__145.FSetupControl.GetRemote();
                w__145.ChatFogPnl.Visible = !call__146;
                var call__147 = w__145.FSetupControl.GetRemote();
                w__145.ChatMemo.CaptureInput = call__147;
                var call__148 = w__145.FSetupControl.GetRemote();
                w__145.ChatEdit.CaptureInput = call__148;
                var call__149 = w__145.FSetupControl.GetHost();
                if (call__149 && (w__145.FSetupControl.FSettings.FMapName == ((AnsiString)"")))
                {
                    w__145.PickMap();
                }

                var call__150 = w__145.FSetupControl.GetRemote();
                if (call__150)
                {
                    w__145.TacticalCombatCB.Visible = true;
                    w__145.TacticalCombatLbl.Visible = true;
                    w__145.TacticalCombatCB.Items.Clear();
                    {
                        int first__154 = (int)0;
                        int end__151 = (int)2;
                        for (int ordinal__153 = first__154; ordinal__153 <= end__151; ordinal__153++)
                        {
                            B = (byte)ordinal__153;
                            var call__152 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TTacticalCombatAvailabilityRStr[B]);
                            w__145.TacticalCombatCB.Items.AddObject(call__152, Aow.Rtl.Rtl.IntToObject((int)B));
                        }
                    }
                }
                else
                {
                    w__145.TacticalCombatCB.Visible = false;
                    w__145.TacticalCombatLbl.Visible = false;
                }

                if (w__145.FSetupControl.FSettings.FConnectionType == 2)
                {
                    w__145.CustomizeLeadersLbl.TextColor = 8421504;
                    w__145.CustomizeLeaders.CaptureInput = false;
                    w__145.PlayModeCB.Items.Clear();
                    var call__155 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ClassicRStr);
                    w__145.PlayModeCB.Items.AddObject(call__155, Aow.Rtl.Rtl.IntToObject((int)1));
                }
                else
                {
                    w__145.CustomizeLeadersLbl.TextColor = w__145.AlliedVictoryLbl.TextColor;
                    w__145.CustomizeLeaders.CaptureInput = true;
                    w__145.PlayModeCB.Items.Clear();
                    var call__156 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SimultaneousRStr);
                    w__145.PlayModeCB.Items.AddObject(call__156, Aow.Rtl.Rtl.IntToObject((int)2));
                    var call__157 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ClassicRStr);
                    w__145.PlayModeCB.Items.AddObject(call__157, Aow.Rtl.Rtl.IntToObject((int)1));
                }

                bool bool__158 = (Aow.Units.Sound.Sound.SongPlayer.CurrentIndex != -1);
                if (bool__158)
                {
                    var call__159 = Aow.Units.Sound.Sound.SongPlayer.Songs.Count;
                    bool__158 = (Aow.Units.Sound.Sound.SongPlayer.CurrentIndex < call__159);
                }

                bool bool__160 = bool__158;
                if (bool__160)
                {
                    var call__161 = Aow.Units.Sound.Sound.SongPlayer.Songs[Aow.Units.Sound.Sound.SongPlayer.CurrentIndex];
                    bool__160 = (call__161 == ((AnsiString)"SONGS\\TITLE.IT"));
                }

                if (!bool__160)
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
