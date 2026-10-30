// Translated from E2GameFlow.pas.
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
            /* TODO: Implement aowDPlayComp.CheckLobby in the platform or editor service. */
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
            var window = Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin;
            var call__5 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Alignment.RightOffset, Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Alignment.BottomOffset, window.WinWidth, window.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(0, call__5);

            var window2 = Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow;
            var call__7 = Aow.Rtl.Rtl.Rect(Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Alignment.RightOffset, Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Alignment.TopOffset, window2.WinWidth, window2.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(1, call__7);

            var window3 = Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin;
            if (window3.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awCenter)
            {
                var call__9 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.LeftOffset, Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.TopOffset, window3.WinWidth, window3.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(2, call__9);
            }
            else
            {
                var call__10 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(2, call__10);
            }

            var window4 = Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg;
            if (window4.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__12 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.LeftOffset, Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.TopOffset, window4.WinWidth, window4.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(5, call__12);
            }
            else
            {
                var call__13 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(5, call__13);
            }

            var window5 = Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg;
            if (window5.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__15 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Alignment.LeftOffset, Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Alignment.TopOffset, window5.WinWidth, window5.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(13, call__15);
            }
            else
            {
                var call__16 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(13, call__16);
            }

            var window6 = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow;
            var call__18 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Alignment.RightOffset, Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Alignment.TopOffset, window6.WinWidth, window6.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(6, call__18);

            var window7 = Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin;
            var call__20 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Alignment.RightOffset, Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Alignment.TopOffset, window7.WinWidth, window7.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(7, call__20);

            var window8 = Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin;
            if (window8.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__22 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Alignment.LeftOffset, Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Alignment.BottomOffset, window8.WinWidth, window8.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(8, call__22);
            }
            else
            {
                var call__23 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(8, call__23);
            }

            var window9 = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow;
            if (window9.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__25 = Aow.Rtl.Rtl.Rect(Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.LeftOffset, Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.TopOffset, window9.WinWidth, window9.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(9, call__25);
            }
            else
            {
                var call__26 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(9, call__26);
            }

            var window10 = Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin;
            var call__28 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Alignment.RightOffset, Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Alignment.TopOffset, window10.WinWidth, window10.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(12, call__28);

            var window11 = Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin;
            var call__30 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.Alignment.RightOffset, Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.Alignment.BottomOffset, window11.WinWidth, window11.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(10, call__30);

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
            if (Setup.FSetupControl != null)
            {
                Control = Setup.FSetupControl;
                Setup.PlayerList.Items.Clear();
                Setup.PingList.Items.Clear();
                Setup.PreferredRaceList.Items.Clear();
                {
                    I = 0;
                    int end__35 = Control.FSettings.FRemotePlayers.Count - 1;
                    for (; I <= end__35; I += 1)
                    {
                        var call__36 = Control.FSettings.FRemotePlayers.GetPlayers(I);
                        Player = call__36;
                        Setup.PlayerList.Items.AddObject(Player.FName, Aow.Rtl.Rtl.IntToObject((int)Player.FRemoteID));
                        if (Player.FPreferredRace != 255)
                        {
                            var call__37 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[unchecked((sbyte)Player.FPreferredRace)]);
                            Setup.PreferredRaceList.Items.Add(call__37);
                        }
                        else
                        {
                            var call__38 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoneRStr);
                            Setup.PreferredRaceList.Items.Add(call__38);
                        }

                        Setup.PingList.Items.AddObject((AnsiString)"", Aow.Rtl.Rtl.IntToObject((int)Player.FRemoteID));
                    }
                }

                var call__39 = Setup.PlayerList.Items.Count;
                Setup.PlayerListSB.Max = call__39 - 1;
                Setup.SelectPlayer(Setup.PlayerList.Selected);
                Setup.CustomizeLeaders.Checked = Setup.FSetupControl.FSettings.FCustomizeLeaders;
                var call__40 = Setup.PlayModeCB.Items.IndexOfObject(Aow.Rtl.Rtl.IntToObject((int)Setup.FSetupControl.FSettings.FPlayMode));
                Setup.PlayModeCB.Selected = call__40;
                Setup.Exploration.Checked = Setup.FSetupControl.FSettings.FExplorationEnabled;
                Setup.TurnTimeSl.Position = Setup.FSetupControl.FSettings.FTurnTime;
                if (Setup.FSetupControl.FSettings.FDayLimit != 0)
                {
                    Setup.TurnsSl.Position = Setup.FSetupControl.FSettings.FDayLimit / 10;
                }
                else
                {
                    Setup.TurnsSl.Position = 0;
                }

                Setup.UpdateTurnLimit();
                Setup.UpdateTurnTime();
                var call__41 = Setup.FSetupControl.GetHost();
                bool bool__42 = call__41;
                if (bool__42)
                {
                    var call__43 = Setup.FSetupControl.GetPlayerSetupValid();
                    bool__42 = call__43;
                }

                Setup.StartBtn.Visible = bool__42;
                var call__44 = Setup.FSetupControl.GetHost();
                Setup.PickScenarioBtn.Visible = call__44;
                var call__45 = Setup.FSetupControl.GetHost();
                Setup.SettingsFogPnl.Visible = !call__45;
                Setup.RestartSettingsFogPnl.Visible = !Setup.SettingsFogPnl.Visible && (Setup.FSetupControl.FSettings.FMapDay != 0);
                Setup.LeadersOnMapCB.Checked = Setup.FSetupControl.FSettings.FLeadersOnMap;
                Setup.AlliedVictoryCB.Checked = Setup.FSetupControl.FSettings.FAlliedVictory;
                var call__46 = Setup.TacticalCombatCB.Items.IndexOfObject(Aow.Rtl.Rtl.IntToObject((int)Setup.FSetupControl.FSettings.FTacticalCombatAvailability));
                Setup.TacticalCombatCB.Selected = call__46;
                Setup.AlwaysViewTCCB.Visible = Setup.TacticalCombatCB.Visible && (Setup.FSetupControl.FSettings.FTacticalCombatAvailability != 0);
                Setup.AlwaysViewTCCB.Checked = Setup.FSetupControl.FSettings.FAlwaysViewTC;
                Setup.AlwaysViewTCLbl.Visible = Setup.AlwaysViewTCCB.Visible;
                var call__47 = Setup.FSetupControl.GetRemote();
                Setup.PreferredRaceLbl.Visible = call__47;
                var call__48 = Setup.FSetupControl.GetRemote();
                Setup.PreferredRaceCB.Visible = call__48;
                if (Setup.PreferredRaceCB.Visible)
                {
                    Setup.PreferredRaceCB.Items.Clear();
                    var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoneRStr);
                    Setup.PreferredRaceCB.Items.AddObject(call__49, Aow.Rtl.Rtl.IntToObject((int)-1));
                    {
                        I = 0;
                        int end__50 = Setup.FSetupControl.FSettings.Count - 1;
                        for (; I <= end__50; I += 1)
                        {
                            var call__51 = Setup.FSetupControl.FSettings.GetSettings(I);
                            if (!call__51.FStatus.Contains(Aow.Units.AoWSetup.TPlayerSetupStatusElement.pssFixed))
                            {
                                var call__52 = Setup.FSetupControl.FSettings.GetSettings(I);
                                var call__53 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[call__52.FRace]);
                                var call__54 = Setup.FSetupControl.FSettings.GetSettings(I);
                                Setup.PreferredRaceCB.Items.AddObject(call__53, Aow.Rtl.Rtl.IntToObject((int)call__54.FRace));
                            }
                        }
                    }

                    var aowComboBox = Setup.PreferredRaceCB;
                    aowComboBox.NumberVisible = 5;
                    aowComboBox.FSized = false;
                    var call__56 = aowComboBox.Items.IndexOfObject(Aow.Rtl.Rtl.IntToObject((int)unchecked((sbyte)Setup.FSetupControl.FRemotePlayer.FPreferredRace)));
                    aowComboBox.Selected = call__56;
                }

                var call__57 = Control.FSettings.IndexOfRace(0);
                var call__58 = Control.FSettings.GetSettings(call__57);
                Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__58, Setup.R1PlayerCB, Setup.R1BehaviorCB, Setup.R1PlayerIcon, Setup.R1PlayerLbl, Setup.R1InfoLbl, Setup.R1FogPnl);
                var call__59 = Control.FSettings.IndexOfRace(1);
                var call__60 = Control.FSettings.GetSettings(call__59);
                Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__60, Setup.R2PlayerCB, Setup.R2BehaviorCB, Setup.R2PlayerIcon, Setup.R2PlayerLbl, Setup.R2InfoLbl, Setup.R2FogPnl);
                var call__61 = Control.FSettings.IndexOfRace(2);
                var call__62 = Control.FSettings.GetSettings(call__61);
                Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__62, Setup.R3PlayerCB, Setup.R3BehaviorCB, Setup.R3PlayerIcon, Setup.R3PlayerLbl, Setup.R3InfoLbl, Setup.R3FogPnl);
                var call__63 = Control.FSettings.IndexOfRace(3);
                var call__64 = Control.FSettings.GetSettings(call__63);
                Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__64, Setup.R4PlayerCB, Setup.R4BehaviorCB, Setup.R4PlayerIcon, Setup.R4PlayerLbl, Setup.R4InfoLbl, Setup.R4FogPnl);
                var call__65 = Control.FSettings.IndexOfRace(4);
                var call__66 = Control.FSettings.GetSettings(call__65);
                Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__66, Setup.R5PlayerCB, Setup.R5BehaviorCB, Setup.R5PlayerIcon, Setup.R5PlayerLbl, Setup.R5InfoLbl, Setup.R5FogPnl);
                var call__67 = Control.FSettings.IndexOfRace(5);
                var call__68 = Control.FSettings.GetSettings(call__67);
                Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__68, Setup.R6PlayerCB, Setup.R6BehaviorCB, Setup.R6PlayerIcon, Setup.R6PlayerLbl, Setup.R6InfoLbl, Setup.R6FogPnl);
                var call__69 = Control.FSettings.IndexOfRace(6);
                var call__70 = Control.FSettings.GetSettings(call__69);
                Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__70, Setup.R7PlayerCB, Setup.R7BehaviorCB, Setup.R7PlayerIcon, Setup.R7PlayerLbl, Setup.R7InfoLbl, Setup.R7FogPnl);
                var call__71 = Control.FSettings.IndexOfRace(7);
                var call__72 = Control.FSettings.GetSettings(call__71);
                Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__72, Setup.R8PlayerCB, Setup.R8BehaviorCB, Setup.R8PlayerIcon, Setup.R8PlayerLbl, Setup.R8InfoLbl, Setup.R8FogPnl);
                var call__73 = Control.FSettings.IndexOfRace(8);
                var call__74 = Control.FSettings.GetSettings(call__73);
                Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__74, Setup.R9PlayerCB, Setup.R9BehaviorCB, Setup.R9PlayerIcon, Setup.R9PlayerLbl, Setup.R9InfoLbl, Setup.R9FogPnl);
                var call__75 = Control.FSettings.IndexOfRace(9);
                var call__76 = Control.FSettings.GetSettings(call__75);
                Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__76, Setup.R10PlayerCB, Setup.R10BehaviorCB, Setup.R10PlayerIcon, Setup.R10PlayerLbl, Setup.R10InfoLbl, Setup.R10FogPnl);
                var call__77 = Control.FSettings.IndexOfRace(10);
                var call__78 = Control.FSettings.GetSettings(call__77);
                Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__78, Setup.R11PlayerCB, Setup.R11BehaviorCB, Setup.R11PlayerIcon, Setup.R11PlayerLbl, Setup.R11InfoLbl, Setup.R11FogPnl);
                var call__79 = Control.FSettings.IndexOfRace(11);
                var call__80 = Control.FSettings.GetSettings(call__79);
                Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioRow(Setup, call__80, Setup.R12PlayerCB, Setup.R12BehaviorCB, Setup.R12PlayerIcon, Setup.R12PlayerLbl, Setup.R12InfoLbl, Setup.R12FogPnl);
                var call__81 = Setup.FSetupControl.GetRemote();
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

                    var setupSettings = Setup.FSetupControl.FSettings;
                    var call__83 = setupSettings.FRemotePlayers.IndexOfRemoteID(setupSettings.FHostRemoteID);
                    HostIndex = call__83;

                    if (HostIndex != -1)
                    {
                        var call__84 = Setup.FSetupControl.FSettings.FRemotePlayers.GetPlayers(HostIndex);
                        HostName = call__84.FName;
                    }
                    else
                    {
                        HostName = AnsiString.FromBytes(63);
                    }

                    Setup.HostNameLbl.Visible = true;
                    Setup.FileNameLbl.Visible = true;
                    Setup.PlayerNameLbl.Visible = true;
                    var call__85 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XHostYRStr);
                    var call__86 = Aow.Units.AoWE.AoWE.Format(call__85, [Connection, HostName]);
                    Setup.HostNameLbl.Text = call__86;
                    var call__87 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PlayerNameXRStr);
                    var call__88 = Aow.Units.AoWE.AoWE.Format(call__87, [Setup.FSetupControl.FRemotePlayer.FName]);
                    Setup.PlayerNameLbl.Text = call__88;
                    var call__89 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FileNameXRStr);
                    var call__90 = Aow.Units.AoWE.AoWE.Format(call__89, [Setup.FSetupControl.FSettings.FMapName]);
                    Setup.FileNameLbl.Text = call__90;
                    if (Setup.FSetupControl.FRemotePlayer.FMapValidation == Aow.Units.AoWSetup.TMapValidationInfo.mviInvalid)
                    {
                        Setup.FileNameLbl.TextColor = 255;
                    }
                    else
                    {
                        Setup.FileNameLbl.TextColor = 10329501;
                    }
                }
                else
                {
                    Setup.HostNameLbl.Visible = false;
                    Setup.FileNameLbl.Visible = false;
                    Setup.PlayerNameLbl.Visible = false;
                }

                var call__91 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ScenarioXRStr);
                var call__92 = Aow.Units.AoWE.AoWE.Format(call__91, [Setup.FSetupControl.FSettings.FFileName]);
                Setup.ScenarioLbl.Text = call__92;
                Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
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
                var aowTurnButton = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TurnButton;
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == -1)
                {
                    aowTurnButton.FCurPlayer = 12;
                    aowTurnButton.FLastPlayer = 12;
                    aowTurnButton.FAnimationFrame = aowTurnButton.FCurPlayer * 7;
                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.AutoBtn.Visible = false;
                }
                else
                {
                    var call__121 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    aowTurnButton.FLastPlayer = call__121.FPlayerRace;
                    if (aowTurnButton.FLastPlayer == -1)
                    {
                        aowTurnButton.FLastPlayer = 12;
                    }

                    aowTurnButton.FCurPlayer = aowTurnButton.FLastPlayer;
                    aowTurnButton.FAnimationFrame = aowTurnButton.FLastPlayer * 7;
                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.AutoBtn.Visible = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode == Aow.Units.AoWE.TPlayMode.pmSimultaneous;
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
            Setup.FSetupControl = Control;
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.Visible = true;
            Setup.ScenSetupPnl.Visible = true;
            Setup.ChatMemo.Items.Clear();
            Setup.ChatEdit.Text = (AnsiString)"";
            Setup.FValidateDelay = 100;
            Setup.FPingDelay = 15;
            var call__146 = Setup.FSetupControl.GetRemote();
            Setup.ChatFogPnl.Visible = !call__146;
            var call__147 = Setup.FSetupControl.GetRemote();
            Setup.ChatMemo.CaptureInput = call__147;
            var call__148 = Setup.FSetupControl.GetRemote();
            Setup.ChatEdit.CaptureInput = call__148;
            var call__149 = Setup.FSetupControl.GetHost();
            if (call__149 && (Setup.FSetupControl.FSettings.FMapName == ((AnsiString)"")))
            {
                Setup.PickMap();
            }

            var call__150 = Setup.FSetupControl.GetRemote();
            if (call__150)
            {
                Setup.TacticalCombatCB.Visible = true;
                Setup.TacticalCombatLbl.Visible = true;
                Setup.TacticalCombatCB.Items.Clear();
                {
                    int first__154 = (int)0;
                    int end__151 = (int)2;
                    for (int ordinal__153 = first__154; ordinal__153 <= end__151; ordinal__153++)
                    {
                        B = (byte)ordinal__153;
                        var call__152 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TTacticalCombatAvailabilityRStr[B]);
                        Setup.TacticalCombatCB.Items.AddObject(call__152, Aow.Rtl.Rtl.IntToObject((int)B));
                    }
                }
            }
            else
            {
                Setup.TacticalCombatCB.Visible = false;
                Setup.TacticalCombatLbl.Visible = false;
            }

            if (Setup.FSetupControl.FSettings.FConnectionType == 2)
            {
                Setup.CustomizeLeadersLbl.TextColor = 8421504;
                Setup.CustomizeLeaders.CaptureInput = false;
                Setup.PlayModeCB.Items.Clear();
                var call__155 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ClassicRStr);
                Setup.PlayModeCB.Items.AddObject(call__155, Aow.Rtl.Rtl.IntToObject((int)1));
            }
            else
            {
                Setup.CustomizeLeadersLbl.TextColor = Setup.AlliedVictoryLbl.TextColor;
                Setup.CustomizeLeaders.CaptureInput = true;
                Setup.PlayModeCB.Items.Clear();
                var call__156 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SimultaneousRStr);
                Setup.PlayModeCB.Items.AddObject(call__156, Aow.Rtl.Rtl.IntToObject((int)2));
                var call__157 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ClassicRStr);
                Setup.PlayModeCB.Items.AddObject(call__157, Aow.Rtl.Rtl.IntToObject((int)1));
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
