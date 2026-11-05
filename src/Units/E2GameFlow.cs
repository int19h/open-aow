// Translated from E2GameFlow.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.AoWTutorialWin;
using Aow.Units.AoWCampaignJoin;
using Aow.Units.AoWScenSetup;
using Aow.Units.aowSetupEvents;
using static Aow.Units.aowSetupEvents.aowSetupEvents;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using static Aow.Units.E2Globals.E2Globals;
using Aow.Units.AOWDialogWin;
using Aow.Units.AoWWindow;
using Aow.Units.AoWLabel;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWFileDialog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWComp;
using static Aow.Units.AoWSWindow.AoWSWindow;
using static Aow.Units.AOWMWindow.AOWMWindow;
using static Aow.Units.AoWPInfoWin.AoWPInfoWin;
using static Aow.Units.Sound.Sound;
using Aow.Units.HSEngine;
using Aow.Units.AoWSetup;
using Aow.Units.AoWComboBox;
using Aow.Units.AoWPanel;
using Aow.Units.AoWImage;
using Aow.Units.AoWWorldMap;
using static Aow.Units.AoWWorldMap.AoWWorldMap;
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;
using static Aow.Units.ImageLib.ImageLib;

namespace Aow.Units.E2GameFlow
{
    public static partial class E2GameFlow
    {
        // E2GameFlow.pas:594
        // { Unnamed executable flow routines called by E2. Original addresses are in E2.json. }
        public static void HideGameWindows(TGeneral Controller)
        {
            int I = default;
            if (!Controller.FWindowsHidden)
            {
                Controller.FWindowsHidden = true;
                Controller.FInfoVisible = PInfoW.PInfoWin.Visible;
                Controller.FRealmVisible = AoWRealm.AoWRealm.RealmWin.Dlg.Visible;
                Controller.FMagicVisible = MagicWin.Dlg.Visible;
                Controller.FUnitVisible = AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible;
                Controller.FTargetVisible = ChatWindow.ChatWin.Visible;
                Controller.FScannerVisible = SWindow.ScannerWindow.Visible;
                Controller.FControlVisible = AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible;
                Controller.FChatVisible = AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Visible;
                Controller.FMapVisible = MWindow.MapWindow.Visible;
                {
                    I = 0;
                    int end__1 = General.Manager.FChildren.Count - 1;
                    for (; I <= end__1; I += 1)
                    {
                        var call__2 = General.Manager.FChildren[I];
                        if (call__2 != TBWindow.TaskWindow)
                        {
                            var call__3 = General.Manager.FChildren[I];
                            ((TAOWWindow)call__3).Visible = false;
                        }
                    }
                }
            }
        }

        // E2GameFlow.pas:632
        public static void ConfirmDialog(TDWin Dialog, AnsiString Title, AnsiString Message, byte Kind)
        {
            Dialog.FAction = Kind;
            Dialog.DTitle.Text = Title;
            Dialog.DMessage.Text = Message;
            Dialog.yesPnl.Visible = true;
            Dialog.okPnl.Visible = false;
            Dialog.Dialog.Visible = true;
        }

        // E2GameFlow.pas:354
        public static bool StartLobby(TSetupEvents Events)
        {
            /* TODO: Implement aowDPlayComp.CheckLobby in the platform or editor service. */
            throw new NotImplementedException();
        }

        // E2GameFlow.pas:380
        public static void OpenSelectedGame(TFileDlgWindow Dialog)
        {
            Dialog.LoadSelectedGame(Dialog.FLastFileName);
        }

        // E2GameFlow.pas:385
        public static void SaveWindowLayout(TGeneral Controller)
        {
            var window = PInfoW.PInfoWin;
            var call__5 = Rect(PInfoW.PInfoWin.Alignment.RightOffset, PInfoW.PInfoWin.Alignment.BottomOffset, window.WinWidth, window.WinHeight);
            AoWEngine.FRegistry.SetRectangle(0, call__5);

            var window2 = AOWControlWin.AOWControlWin.ControlWin.ControlWindow;
            var call__7 = Rect(AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Alignment.RightOffset, AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Alignment.TopOffset, window2.WinWidth, window2.WinHeight);
            AoWEngine.FRegistry.SetRectangle(1, call__7);

            var window3 = AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin;
            if (window3.Alignment.AlignWidth != TAOWAlignWidth.awCenter)
            {
                var call__9 = Rect(AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.LeftOffset, AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.TopOffset, window3.WinWidth, window3.WinHeight);
                AoWEngine.FRegistry.SetRectangle(2, call__9);
            }
            else
            {
                var call__10 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(2, call__10);
            }

            var window4 = AoWRealm.AoWRealm.RealmWin.Dlg;
            if (window4.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
            {
                var call__12 = Rect(AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.LeftOffset, AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.TopOffset, window4.WinWidth, window4.WinHeight);
                AoWEngine.FRegistry.SetRectangle(5, call__12);
            }
            else
            {
                var call__13 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(5, call__13);
            }

            var window5 = MagicWin.Dlg;
            if (window5.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
            {
                var call__15 = Rect(MagicWin.Dlg.Alignment.LeftOffset, MagicWin.Dlg.Alignment.TopOffset, window5.WinWidth, window5.WinHeight);
                AoWEngine.FRegistry.SetRectangle(13, call__15);
            }
            else
            {
                var call__16 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(13, call__16);
            }

            var window6 = SWindow.ScannerWindow;
            var call__18 = Rect(SWindow.ScannerWindow.Alignment.RightOffset, SWindow.ScannerWindow.Alignment.TopOffset, window6.WinWidth, window6.WinHeight);
            AoWEngine.FRegistry.SetRectangle(6, call__18);

            var window7 = AoWInfoWin.AoWInfoWin.IWindow.InfoWin;
            var call__20 = Rect(AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Alignment.RightOffset, AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Alignment.TopOffset, window7.WinWidth, window7.WinHeight);
            AoWEngine.FRegistry.SetRectangle(7, call__20);

            var window8 = ChatWindow.ChatWin;
            if (window8.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
            {
                var call__22 = Rect(ChatWindow.ChatWin.Alignment.LeftOffset, ChatWindow.ChatWin.Alignment.BottomOffset, window8.WinWidth, window8.WinHeight);
                AoWEngine.FRegistry.SetRectangle(8, call__22);
            }
            else
            {
                var call__23 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(8, call__23);
            }

            var window9 = MWindow.MapWindow;
            if (window9.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
            {
                var call__25 = Rect(MWindow.MapWindow.Alignment.LeftOffset, MWindow.MapWindow.Alignment.TopOffset, window9.WinWidth, window9.WinHeight);
                AoWEngine.FRegistry.SetRectangle(9, call__25);
            }
            else
            {
                var call__26 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(9, call__26);
            }

            var window10 = AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin;
            var call__28 = Rect(AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Alignment.RightOffset, AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Alignment.TopOffset, window10.WinWidth, window10.WinHeight);
            AoWEngine.FRegistry.SetRectangle(12, call__28);

            var window11 = TCScanner.TCScannerWin;
            var call__30 = Rect(TCScanner.TCScannerWin.Alignment.RightOffset, TCScanner.TCScannerWin.Alignment.BottomOffset, window11.WinWidth, window11.WinHeight);
            AoWEngine.FRegistry.SetRectangle(10, call__30);

            if (MWindow.FMapAlignWidth != TAOWAlignWidth.awBoth)
            {
                var call__31 = Rect(MWindow.FMapRect.Left, MWindow.FMapRect.Top, MWindow.FMapRect.Right, MWindow.FMapRect.Bottom);
                AoWEngine.FRegistry.SetRectangle(11, call__31);
            }
            else
            {
                var call__32 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(11, call__32);
            }
        }

        // E2GameFlow.pas:424
        public static void ShowTutorialMessage(TTutorialWindow Dialog)
        {
            var call__33 = Dialog.MessageMemo.Items.Count;
            if (call__33 > 0)
            {
                Dialog.Dlg.Visible = true;
            }
        }

        // E2GameFlow.pas:429
        public static void SetSceneSize(TVideoDialog Dialog, int Width, int Height)
        {
            Dialog.FSceneWidth = Width;
            Dialog.FSceneHeight = Height;
            if (Dialog.FFullScreen == TScreenMode.scmTitle)
            {
                Dialog.FModeChanged = true;
            }
        }

        // E2GameFlow.pas:81
        public static void HideScenarioSetup(TScenSetup Setup)
        {
            Setup.ScenSetupPnl.Visible = false;
            Setup.FSetupControl = null;
        }

        // E2GameFlow.pas:180
        public static void UpdateScenarioSetup(TScenSetup Setup)
        {
            int I = default;
            int HostIndex = default;
            AnsiString HostName = default;
            AnsiString Connection = default;
            TRemotePlayer Player = default;
            TSetupControl Control = default;
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
                        Setup.PlayerList.Items.AddObject(Player.FName, IntToObject(Player.FRemoteID));
                        if (Player.FPreferredRace != 255)
                        {
                            var call__37 = TranslateRStr(RaceNamesPluralRStr[(sbyte)Player.FPreferredRace]);
                            Setup.PreferredRaceList.Items.Add(call__37);
                        }
                        else
                        {
                            var call__38 = TranslateRStr(NoneRStr);
                            Setup.PreferredRaceList.Items.Add(call__38);
                        }

                        Setup.PingList.Items.AddObject((AnsiString)"", IntToObject(Player.FRemoteID));
                    }
                }

                var call__39 = Setup.PlayerList.Items.Count;
                Setup.PlayerListSB.Max = call__39 - 1;
                Setup.SelectPlayer(Setup.PlayerList.Selected);
                Setup.CustomizeLeaders.Checked = Setup.FSetupControl.FSettings.FCustomizeLeaders;
                var call__40 = Setup.PlayModeCB.Items.IndexOfObject(IntToObject(Setup.FSetupControl.FSettings.FPlayMode));
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
                Setup.RestartSettingsFogPnl.Visible = !Setup.SettingsFogPnl.Visible && Setup.FSetupControl.FSettings.FMapDay != 0;
                Setup.LeadersOnMapCB.Checked = Setup.FSetupControl.FSettings.FLeadersOnMap;
                Setup.AlliedVictoryCB.Checked = Setup.FSetupControl.FSettings.FAlliedVictory;
                var call__46 = Setup.TacticalCombatCB.Items.IndexOfObject(IntToObject(Setup.FSetupControl.FSettings.FTacticalCombatAvailability));
                Setup.TacticalCombatCB.Selected = call__46;
                Setup.AlwaysViewTCCB.Visible = Setup.TacticalCombatCB.Visible && Setup.FSetupControl.FSettings.FTacticalCombatAvailability != 0;
                Setup.AlwaysViewTCCB.Checked = Setup.FSetupControl.FSettings.FAlwaysViewTC;
                Setup.AlwaysViewTCLbl.Visible = Setup.AlwaysViewTCCB.Visible;
                var call__47 = Setup.FSetupControl.GetRemote();
                Setup.PreferredRaceLbl.Visible = call__47;
                var call__48 = Setup.FSetupControl.GetRemote();
                Setup.PreferredRaceCB.Visible = call__48;
                if (Setup.PreferredRaceCB.Visible)
                {
                    Setup.PreferredRaceCB.Items.Clear();
                    var call__49 = TranslateRStr(NoneRStr);
                    Setup.PreferredRaceCB.Items.AddObject(call__49, IntToObject(-1));
                    {
                        I = 0;
                        int end__50 = Setup.FSetupControl.FSettings.Count - 1;
                        for (; I <= end__50; I += 1)
                        {
                            var call__51 = Setup.FSetupControl.FSettings.GetSettings(I);
                            if (!call__51.FStatus.Contains(TPlayerSetupStatusElement.pssFixed))
                            {
                                var call__52 = Setup.FSetupControl.FSettings.GetSettings(I);
                                var call__53 = TranslateRStr(RaceNamesPluralRStr[call__52.FRace]);
                                var call__54 = Setup.FSetupControl.FSettings.GetSettings(I);
                                Setup.PreferredRaceCB.Items.AddObject(call__53, IntToObject(call__54.FRace));
                            }
                        }
                    }

                    var aowComboBox = Setup.PreferredRaceCB;
                    aowComboBox.NumberVisible = 5;
                    aowComboBox.FSized = false;
                    var call__56 = aowComboBox.Items.IndexOfObject(IntToObject((sbyte)Setup.FSetupControl.FRemotePlayer.FPreferredRace));
                    aowComboBox.Selected = call__56;
                }

                var call__57 = Control.FSettings.IndexOfRace(0);
                var call__58 = Control.FSettings.GetSettings(call__57);
                UpdateScenarioRow(Setup, call__58, Setup.R1PlayerCB, Setup.R1BehaviorCB, Setup.R1PlayerIcon, Setup.R1PlayerLbl, Setup.R1InfoLbl, Setup.R1FogPnl);
                var call__59 = Control.FSettings.IndexOfRace(1);
                var call__60 = Control.FSettings.GetSettings(call__59);
                UpdateScenarioRow(Setup, call__60, Setup.R2PlayerCB, Setup.R2BehaviorCB, Setup.R2PlayerIcon, Setup.R2PlayerLbl, Setup.R2InfoLbl, Setup.R2FogPnl);
                var call__61 = Control.FSettings.IndexOfRace(2);
                var call__62 = Control.FSettings.GetSettings(call__61);
                UpdateScenarioRow(Setup, call__62, Setup.R3PlayerCB, Setup.R3BehaviorCB, Setup.R3PlayerIcon, Setup.R3PlayerLbl, Setup.R3InfoLbl, Setup.R3FogPnl);
                var call__63 = Control.FSettings.IndexOfRace(3);
                var call__64 = Control.FSettings.GetSettings(call__63);
                UpdateScenarioRow(Setup, call__64, Setup.R4PlayerCB, Setup.R4BehaviorCB, Setup.R4PlayerIcon, Setup.R4PlayerLbl, Setup.R4InfoLbl, Setup.R4FogPnl);
                var call__65 = Control.FSettings.IndexOfRace(4);
                var call__66 = Control.FSettings.GetSettings(call__65);
                UpdateScenarioRow(Setup, call__66, Setup.R5PlayerCB, Setup.R5BehaviorCB, Setup.R5PlayerIcon, Setup.R5PlayerLbl, Setup.R5InfoLbl, Setup.R5FogPnl);
                var call__67 = Control.FSettings.IndexOfRace(5);
                var call__68 = Control.FSettings.GetSettings(call__67);
                UpdateScenarioRow(Setup, call__68, Setup.R6PlayerCB, Setup.R6BehaviorCB, Setup.R6PlayerIcon, Setup.R6PlayerLbl, Setup.R6InfoLbl, Setup.R6FogPnl);
                var call__69 = Control.FSettings.IndexOfRace(6);
                var call__70 = Control.FSettings.GetSettings(call__69);
                UpdateScenarioRow(Setup, call__70, Setup.R7PlayerCB, Setup.R7BehaviorCB, Setup.R7PlayerIcon, Setup.R7PlayerLbl, Setup.R7InfoLbl, Setup.R7FogPnl);
                var call__71 = Control.FSettings.IndexOfRace(7);
                var call__72 = Control.FSettings.GetSettings(call__71);
                UpdateScenarioRow(Setup, call__72, Setup.R8PlayerCB, Setup.R8BehaviorCB, Setup.R8PlayerIcon, Setup.R8PlayerLbl, Setup.R8InfoLbl, Setup.R8FogPnl);
                var call__73 = Control.FSettings.IndexOfRace(8);
                var call__74 = Control.FSettings.GetSettings(call__73);
                UpdateScenarioRow(Setup, call__74, Setup.R9PlayerCB, Setup.R9BehaviorCB, Setup.R9PlayerIcon, Setup.R9PlayerLbl, Setup.R9InfoLbl, Setup.R9FogPnl);
                var call__75 = Control.FSettings.IndexOfRace(9);
                var call__76 = Control.FSettings.GetSettings(call__75);
                UpdateScenarioRow(Setup, call__76, Setup.R10PlayerCB, Setup.R10BehaviorCB, Setup.R10PlayerIcon, Setup.R10PlayerLbl, Setup.R10InfoLbl, Setup.R10FogPnl);
                var call__77 = Control.FSettings.IndexOfRace(10);
                var call__78 = Control.FSettings.GetSettings(call__77);
                UpdateScenarioRow(Setup, call__78, Setup.R11PlayerCB, Setup.R11BehaviorCB, Setup.R11PlayerIcon, Setup.R11PlayerLbl, Setup.R11InfoLbl, Setup.R11FogPnl);
                var call__79 = Control.FSettings.IndexOfRace(11);
                var call__80 = Control.FSettings.GetSettings(call__79);
                UpdateScenarioRow(Setup, call__80, Setup.R12PlayerCB, Setup.R12BehaviorCB, Setup.R12PlayerIcon, Setup.R12PlayerLbl, Setup.R12InfoLbl, Setup.R12FogPnl);
                var call__81 = Setup.FSetupControl.GetRemote();
                if (call__81)
                {
                    switch (SetupEvents.DPlay.ConnectionType)
                    {
                        case TConnectionType.conTCP:
                            Connection = TranslateRStr(TCPIPRStr);
                            break;
                        case TConnectionType.conIPX:
                            Connection = TranslateRStr(IPXRStr);
                            break;
                        case TConnectionType.conModem:
                            Connection = TranslateRStr(ModemRStr);
                            break;
                        case TConnectionType.conSerial:
                            Connection = TranslateRStr(SerialRStr);
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
                    var call__85 = TranslateRStr(XHostYRStr);
                    var call__86 = AoWE.AoWE.Format(call__85, [Connection, HostName]);
                    Setup.HostNameLbl.Text = call__86;
                    var call__87 = TranslateRStr(PlayerNameXRStr);
                    var call__88 = AoWE.AoWE.Format(call__87, [Setup.FSetupControl.FRemotePlayer.FName]);
                    Setup.PlayerNameLbl.Text = call__88;
                    var call__89 = TranslateRStr(FileNameXRStr);
                    var call__90 = AoWE.AoWE.Format(call__89, [Setup.FSetupControl.FSettings.FMapName]);
                    Setup.FileNameLbl.Text = call__90;
                    if (Setup.FSetupControl.FRemotePlayer.FMapValidation == TMapValidationInfo.mviInvalid)
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

                var call__91 = TranslateRStr(ScenarioXRStr);
                var call__92 = AoWE.AoWE.Format(call__91, [Setup.FSetupControl.FSettings.FFileName]);
                Setup.ScenarioLbl.Text = call__92;
                TitleWindow.TitleWin.FDrawn = false;
            }
        }

        // E2GameFlow.pas:45
        public static void StartCampaignJoin(TCampaignJoinDlg Dialog, int Faction)
        {
            TStringList Lines = default;
            TWMCampaign Campaign = default;
            int Index = default;
            int I = default;
            TAoWHSMap Preview = default;
            Dialog.ResetCampaignJoin();
            Dialog.FFaction = Faction;
            Dialog.FStage = 0;
            Lines = new TStringList();
            switch (Dialog.FFaction)
            {
                case 0:
                    LoadWorldCampaign(WorldMap, (AnsiString)"Keepers.cam");
                    Dialog.CharGenPnl.ILIndexTexture = 10;
                    AoWHSSet.FTextControl.RetrieveText(10, Lines);
                    break;
                case 1:
                    LoadWorldCampaign(WorldMap, (AnsiString)"Storm.cam");
                    Dialog.CharGenPnl.ILIndexTexture = 11;
                    AoWHSSet.FTextControl.RetrieveText(11, Lines);
                    break;
            }

            var call__93 = Lines.Text;
            ParseFontText(call__93, Dialog.FScrollText, FontModule.AoW15WhiteGrey, 300);
            var call__94 = WorldMap.GetCampaign();
            Campaign = call__94;
            var call__95 = GetCampaignScenario(Campaign, 0);
            var call__96 = call__95.FPaths[0];
            var call__97 = Campaign.FScenarios.IndexOfID(call__96);
            Index = call__97;
            var call__98 = WorldMap.GetCampaign();
            var call__99 = GetCampaignScenario(call__98, Index);
            var call__100 = AoWEngine.CreatePreviewHSM(AoWEngine.StartupDirectory + (AnsiString)"Scenario\\Campaign\\" + call__99.FFileName);
            Preview = (TAoWHSMap)call__100;
            I = 0;
            // { Original uses the preview map child count as the player-search bound. }
            while (true)
            {
                var call__101 = Preview.Count;
                bool bool__102 = I < call__101;
                if (bool__102)
                {
                    var call__103 = Preview.FPlayers.GetPlayers(I);
                    bool__102 = (int)call__103.FPlayerType != 0;
                }

                if (!bool__102)
                    break;
                {
                    I += 1;
                }
            }

            var call__104 = Preview.FPlayers.GetPlayers(I);
            Dialog.FLeader.Assign(call__104.FLeaderSetup, AoWEngine);
            Preview.Free();
            Lines.Free();
        }

        // E2GameFlow.pas:498
        public static void LoadGame(TGeneral Controller, AnsiString FileName)
        {
            if (AoWHSMap != null)
            {
                AoWEngine.CloseHSM(null);
            }

            var call__105 = TranslateRStr(LoadingRStr);
            var call__106 = ExtractFileName(FileName);
            ProgressDialog.Title.Text = call__105 + (AnsiString)": " + call__106 + (AnsiString)"...";
            ProgressDialog.StatusLbl.Text = (AnsiString)"";
            ProgressDialog.Bar.Position = 0;
            ProgressDialog.Bar.Max = 100;
            ProgressDialog.ProgDlg.Visible = true;
            if (!TitleScene.Window.Visible)
            {
                Controller.Manager.OverlayPriority = 5000;
            }

            SongPlayer.Stop(false);
            SongPlayer.Songs.Clear();
            FadeMusicVolume(0, 1);
            AoWEngine.LoadHSM(FileName, false, Controller.MapProgress);
            if (AoWEngine.FDemo && AoWHSMap != null && !AoWHSMap.FShowAllPlayers)
            {
                CloseGame(Controller);
            }

            Controller.Manager.OverlayPriority = 0;
            ProgressDialog.ProgDlg.Visible = false;
        }

        // E2GameFlow.pas:516
        public static void StartGame(TGeneral Controller)
        {
            int I = default;
            Controller.FWindowsHidden = false;
            SongPlayer.Songs.Clear();
            var call__107 = AoWHSMap.FGameSongs.Songs.Count;
            if (call__107 != 0)
            {
                SongPlayer.Songs.AddStrings(AoWHSMap.FGameSongs.Songs);
            }
            else
            {
                var call__108 = AoWHSMap.FStartupSongs.Songs.Count;
                if (call__108 != 0)
                {
                    {
                        I = 0;
                        int end__109 = AoWHSMap.FStartupSongs.Songs.Count - 1;
                        for (; I <= end__109; I += 1)
                        {
                            var call__110 = TranslateRStr(E2Globals.E2Globals.FileNotFoundRStr);
                            var call__111 = AoWHSMap.FStartupSongs.SongNames[I];
                            if (CompareText(call__111, call__110) != 0)
                            {
                                var call__112 = AoWHSMap.FStartupSongs.Songs[I];
                                SongPlayer.Songs.Add(call__112);
                            }
                        }
                    }
                }
                else
                {
                    SongPlayer.Songs.Add((AnsiString)"Songs\\Heartwoo.it");
                }
            }

            SongPlayer.Play(0);
            SetMusicVolume(SoundDlg.MusicVolume.Position);
            TBWindow.StartMenuBar.Items.Clear();
            var call__113 = TranslateRStr(BackgroundRStr);
            TBWindow.StartMenuBar.Items.Add(call__113);
            if (AoWEngine.FCampaign != null)
            {
                var call__114 = TranslateRStr(CampaignRStr);
                TBWindow.StartMenuBar.Items.Add(call__114);
            }

            TitleScene.Window.Visible = false;
            TitleWindow.TitleWin.Visible = false;
            TBWindow.TaskWindow.Visible = true;
            MWindow.MapWindow.Visible = true;
            SWindow.ScannerWindow.Visible = true;
            MWindow.MapViewer.SetHSMap(AoWHSMap);
            SWindow.Scanner.SetMap(AoWHSMap);
            var call__115 = AoWHSMap.MapContainer.MapLevels[0];
            SWindow.Scanner.SetLevel(call__115);
            TBWindow.TurnButton.FButtonChecked = false;
            TBWindow.ChatBtn.Visible = SetupEvents.DPlay.Connected;
            Controller.Manager.FAligned = false;
            SWindow.Scanner.FPreviousWidth = 0;
            SWindow.Scanner.FIncremental = false;
            SWindow.Scanner.FSized = false;
            SWindow.ScannerTab.Items.Clear();
            var call__116 = TranslateRStr(SurfaceRStr);
            SWindow.ScannerTab.Items.Add(call__116);
            if (AoWHSMap.MapContainer.LevelCount > 1)
            {
                var call__117 = TranslateRStr(CavernsRStr);
                SWindow.ScannerTab.Items.Add(call__117);
            }

            if (AoWHSMap.MapContainer.LevelCount > 2)
            {
                var call__118 = TranslateRStr(DepthsRStr);
                SWindow.ScannerTab.Items.Add(call__118);
            }

            SWindow.ScannerTab.Selected = 0;
            SWindow.ScannerTab.FSized = false;
            SWindow.Scanner.SetTWidth(0);
            SWindow.Scanner.SetTHeight(0);
            SWindow.Scanner.SetTLeft(-1);
            SWindow.Scanner.SetTTop(-1);
            SWindow.Scanner.FUpdateStep = -1;
            SWindow.Scanner.SetMap(AoWHSMap);
            var call__119 = AoWHSMap.MapContainer.MapLevels[0];
            SWindow.Scanner.SetLevel(call__119);
            MWindow.MapViewer.SetHSMap(AoWHSMap);
            MWindow.MapViewer.SceneWidth = MWindow.MapWindow.WinWidth - 24;
            MWindow.MapViewer.SceneHeight = MWindow.MapWindow.WinHeight - 24;
            AoWHSMap.Activate();
            AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible = false;
            aowMapEvents.aowMapEvents.MapEvents.TheMapArmyDeselected(null);
            aowMapEvents.aowMapEvents.MapEvents.TheMapStructureDeselected(null);
            TBWindow.TBWorldUpdate(null);
            PInfoW.UpdatePlayer();
            if (AoWHSMap.Status.Contains(THSMapStatusElement.msActivated))
            {
                var aowTurnButton = TBWindow.TurnButton;
                if (AoWHSMap.FSeatedPlayer == -1)
                {
                    aowTurnButton.FCurPlayer = 12;
                    aowTurnButton.FLastPlayer = 12;
                    aowTurnButton.FAnimationFrame = aowTurnButton.FCurPlayer * 7;
                    TBWindow.AutoBtn.Visible = false;
                }
                else
                {
                    var call__121 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    aowTurnButton.FLastPlayer = call__121.FPlayerRace;
                    if (aowTurnButton.FLastPlayer == -1)
                    {
                        aowTurnButton.FLastPlayer = 12;
                    }

                    aowTurnButton.FCurPlayer = aowTurnButton.FLastPlayer;
                    aowTurnButton.FAnimationFrame = aowTurnButton.FLastPlayer * 7;
                    TBWindow.AutoBtn.Visible = AoWHSMap.FPlayMode == TPlayMode.pmSimultaneous;
                }
            }

            TBWindow.FWorldClockDelay = 0;
            TBWindow.ChatBtn.TextColor = 14;
            TBWindow.FChatFlash = 0;
            VideoDialog.ChangeResolution(VideoDialog.FDisplayWidth, VideoDialog.FDisplayHeight);
        }

        // E2GameFlow.pas:616
        public static void CloseGame(TGeneral Controller)
        {
            AoWEngine.CloseHSM(null);
            AoWEngine.CloseCampaign();
            SWindow.Scanner.SetMap(null);
            TCScanner.Scanner.SetMap(null);
            SongPlayer.Stop(false);
            SongPlayer.Songs.Clear();
            SetCurrentDir(AoWEngine.StartupDirectory);
            HideGameWindows(General);
            TBWindow.TaskWindow.Visible = false;
            TitleWindow.TitleWin.Visible = false;
            TitleScene.ResetScene();
            ShowTitleScene(TitleScene, true);
        }

        // E2GameFlow.pas:29
        public static void InitializeCampaignJoin(TCampaignJoinDlg Dialog)
        {
            Dialog.CharGenPnl.Visible = true;
            Dialog.LeaderTypePnl.Visible = false;
            TitleWindow.TitleWin.Visible = true;
            Dialog.ScrollTextDB.Visible = true;
            Dialog.FScrollDelay = 0;
            Dialog.FScrollPosition = 0;
            Dialog.FScrollText.Clear();
        }

        // E2GameFlow.pas:40
        public static void LoadWorldCampaign(TWorldMap Dialog, AnsiString Name)
        {
            AoWEngine.LoadCampaign(AoWEngine.StartupDirectory + (AnsiString)"Scenario\\Campaign\\" + Name);
        }

        // E2GameFlow.pas:87
        public static void UpdateScenarioRow(TScenSetup Setup, TPlayerSetupSettings Settings, TAOWComboBox PlayerBox, TAOWComboBox BehaviorBox, TAOWImage Icon, TAOWLabel PlayerName, TAOWLabel Info, TAOWPanel Fog)
        {
            TSetupPlayerBehavior Behavior = default;
            byte B = default;
            int I = default;
            bool Visible = default;
            bool Found = default;
            if (Settings != null)
            {
                var call__122 = Setup.FSetupControl.GetHost();
                Visible = call__122 && !Settings.FStatus.Contains(TPlayerSetupStatusElement.pssFixed);
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
                    while (!Found && I != 0)
                    {
                        I -= 1;
                        var call__124 = PlayerBox.Items[I];
                        var call__125 = Setup.FSetupControl.PlayerNameToPlayerID(call__124, ref Behavior);
                        if (call__125)
                        {
                            Found = Settings.FBehavior.PlayerType == Behavior.PlayerType && Settings.FBehavior.Flags == Behavior.Flags;
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
                    if (Settings.FBehavior.PlayerType == 0 && Settings.FBehavior.Flags != 0)
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
                            var call__128 = TranslateRStr(HumanRStr);
                            PlayerName.Text = call__128;
                        }
                    }
                    else
                    {
                        var call__129 = TranslateRStr(TPlayerTypeRStr[Settings.FBehavior.PlayerType]);
                        PlayerName.Text = call__129;
                    }
                }

                Info.Text = (AnsiString)"";
                var call__130 = Setup.FSetupControl.GetRemote();
                Info.Visible = call__130 || Settings.FStatus.Contains(TPlayerSetupStatusElement.pssFixed);
                if (Info.Visible)
                {
                    if (Settings.FStatus.Contains(TPlayerSetupStatusElement.pssFixed))
                    {
                        var call__131 = TranslateRStr(FixedRStr);
                        Info.Text = call__131;
                    }
                    else
                    {
                        var call__132 = Setup.FSetupControl.GetRemote();
                        if (call__132 && Settings.FStatus.Contains(TPlayerSetupStatusElement.pssForced) && Settings.FBehavior.PlayerType == 0)
                        {
                            var call__133 = Setup.FSetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(Settings.FBehavior.Flags);
                            I = call__133;
                            if (I != -1)
                            {
                                // { Original compares race bytes, including the FF sentinel. }
                                var call__134 = Setup.FSetupControl.FSettings.FRemotePlayers.GetPlayers(I);
                                if (call__134.FPreferredRace == (byte)Settings.FRace)
                                {
                                    var call__135 = TranslateRStr(TakenRStr);
                                    Info.Text = call__135;
                                }
                                else
                                {
                                    var call__136 = TranslateRStr(ForcedRStr);
                                    Info.Text = call__136;
                                }
                            }
                        }
                        else
                        {
                            if (Settings.FBehavior.Flags != 0)
                            {
                                var call__137 = TranslateRStr(TakenRStr);
                                Info.Text = call__137;
                            }
                            else
                            {
                                if (TSet<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType))
                                {
                                    var call__138 = TranslateRStr(TPlayerBehaviorRStr[(int)(TPlayerBehavior)Settings.FBehaviorMode]);
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
                        int first__143 = 0;
                        int end__140 = 5;
                        for (int ordinal__142 = first__143; ordinal__142 <= end__140; ordinal__142++)
                        {
                            B = (byte)ordinal__142;
                            var call__141 = TranslateRStr(TPlayerBehaviorRStr[(int)(TPlayerBehavior)B]);
                            BehaviorBox.Items.AddObject(call__141, IntToObject(B));
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

                Fog.Visible = Settings.FStatus.Contains(TPlayerSetupStatusElement.pssFixed);
            }
            else
            {
                Icon.Visible = false;
                PlayerBox.Visible = false;
                BehaviorBox.Visible = false;
                PlayerName.Visible = false;
                Info.Visible = true;
                var call__144 = TranslateRStr(UnavailableRStr);
                Info.Text = call__144;
                Fog.Visible = true;
            }
        }

        // E2GameFlow.pas:298
        public static void InitializeScenarioSetup(TScenSetup Setup, TSetupControl Control)
        {
            byte B = default;
            Setup.FSetupControl = Control;
            TitleWindow.TitleWin.Visible = true;
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
            if (call__149 && Setup.FSetupControl.FSettings.FMapName == (AnsiString)"")
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
                    int first__154 = 0;
                    int end__151 = 2;
                    for (int ordinal__153 = first__154; ordinal__153 <= end__151; ordinal__153++)
                    {
                        B = (byte)ordinal__153;
                        var call__152 = TranslateRStr(TTacticalCombatAvailabilityRStr[B]);
                        Setup.TacticalCombatCB.Items.AddObject(call__152, IntToObject(B));
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
                var call__155 = TranslateRStr(ClassicRStr);
                Setup.PlayModeCB.Items.AddObject(call__155, IntToObject(1));
            }
            else
            {
                Setup.CustomizeLeadersLbl.TextColor = Setup.AlliedVictoryLbl.TextColor;
                Setup.CustomizeLeaders.CaptureInput = true;
                Setup.PlayModeCB.Items.Clear();
                var call__156 = TranslateRStr(SimultaneousRStr);
                Setup.PlayModeCB.Items.AddObject(call__156, IntToObject(2));
                var call__157 = TranslateRStr(ClassicRStr);
                Setup.PlayModeCB.Items.AddObject(call__157, IntToObject(1));
            }

            bool bool__158 = SongPlayer.CurrentIndex != -1;
            if (bool__158)
            {
                var call__159 = SongPlayer.Songs.Count;
                bool__158 = SongPlayer.CurrentIndex < call__159;
            }

            bool bool__160 = bool__158;
            if (bool__160)
            {
                var call__161 = SongPlayer.Songs[SongPlayer.CurrentIndex];
                bool__160 = call__161 == (AnsiString)"SONGS\\TITLE.IT";
            }

            if (!bool__160)
            {
                SongPlayer.Songs.Clear();
                SongPlayer.Songs.Add((AnsiString)"SONGS\\TITLE.IT");
                SongPlayer.Play(0);
            }

            UpdateScenarioSetup(Setup);
        }
    }
}
