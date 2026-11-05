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
                for (int I = 0, last = General.Manager.FChildren.Count - 1; I <= last; I++)
                {
                    if (General.Manager.FChildren[I] != TBWindow.TaskWindow)
                    {
                        ((TAOWWindow)General.Manager.FChildren[I]).Visible = false;
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
            var rect = Rect(PInfoW.PInfoWin.Alignment.RightOffset, PInfoW.PInfoWin.Alignment.BottomOffset, window.WinWidth, window.WinHeight);
            AoWEngine.FRegistry.SetRectangle(0, rect);

            var window2 = AOWControlWin.AOWControlWin.ControlWin.ControlWindow;
            var rect2 = Rect(AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Alignment.RightOffset, AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Alignment.TopOffset, window2.WinWidth, window2.WinHeight);
            AoWEngine.FRegistry.SetRectangle(1, rect2);

            var window3 = AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin;
            if (window3.Alignment.AlignWidth != TAOWAlignWidth.awCenter)
            {
                var rect3 = Rect(AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.LeftOffset, AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.TopOffset, window3.WinWidth, window3.WinHeight);
                AoWEngine.FRegistry.SetRectangle(2, rect3);
            }
            else
            {
                var rect3 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(2, rect3);
            }

            var window4 = AoWRealm.AoWRealm.RealmWin.Dlg;
            if (window4.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
            {
                var rect3 = Rect(AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.LeftOffset, AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.TopOffset, window4.WinWidth, window4.WinHeight);
                AoWEngine.FRegistry.SetRectangle(5, rect3);
            }
            else
            {
                var rect3 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(5, rect3);
            }

            var window5 = MagicWin.Dlg;
            if (window5.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
            {
                var rect3 = Rect(MagicWin.Dlg.Alignment.LeftOffset, MagicWin.Dlg.Alignment.TopOffset, window5.WinWidth, window5.WinHeight);
                AoWEngine.FRegistry.SetRectangle(13, rect3);
            }
            else
            {
                var rect3 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(13, rect3);
            }

            var window6 = SWindow.ScannerWindow;
            var rect4 = Rect(SWindow.ScannerWindow.Alignment.RightOffset, SWindow.ScannerWindow.Alignment.TopOffset, window6.WinWidth, window6.WinHeight);
            AoWEngine.FRegistry.SetRectangle(6, rect4);

            var window7 = AoWInfoWin.AoWInfoWin.IWindow.InfoWin;
            var rect5 = Rect(AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Alignment.RightOffset, AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Alignment.TopOffset, window7.WinWidth, window7.WinHeight);
            AoWEngine.FRegistry.SetRectangle(7, rect5);

            var window8 = ChatWindow.ChatWin;
            if (window8.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
            {
                var rect3 = Rect(ChatWindow.ChatWin.Alignment.LeftOffset, ChatWindow.ChatWin.Alignment.BottomOffset, window8.WinWidth, window8.WinHeight);
                AoWEngine.FRegistry.SetRectangle(8, rect3);
            }
            else
            {
                var rect3 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(8, rect3);
            }

            var window9 = MWindow.MapWindow;
            if (window9.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
            {
                var rect3 = Rect(MWindow.MapWindow.Alignment.LeftOffset, MWindow.MapWindow.Alignment.TopOffset, window9.WinWidth, window9.WinHeight);
                AoWEngine.FRegistry.SetRectangle(9, rect3);
            }
            else
            {
                var rect3 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(9, rect3);
            }

            var window10 = AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin;
            var rect6 = Rect(AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Alignment.RightOffset, AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Alignment.TopOffset, window10.WinWidth, window10.WinHeight);
            AoWEngine.FRegistry.SetRectangle(12, rect6);

            var window11 = TCScanner.TCScannerWin;
            var rect7 = Rect(TCScanner.TCScannerWin.Alignment.RightOffset, TCScanner.TCScannerWin.Alignment.BottomOffset, window11.WinWidth, window11.WinHeight);
            AoWEngine.FRegistry.SetRectangle(10, rect7);

            if (MWindow.FMapAlignWidth != TAOWAlignWidth.awBoth)
            {
                var rect3 = Rect(MWindow.FMapRect.Left, MWindow.FMapRect.Top, MWindow.FMapRect.Right, MWindow.FMapRect.Bottom);
                AoWEngine.FRegistry.SetRectangle(11, rect3);
            }
            else
            {
                var rect3 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(11, rect3);
            }
        }

        // E2GameFlow.pas:424
        public static void ShowTutorialMessage(TTutorialWindow Dialog)
        {
            if (Dialog.MessageMemo.Items.Count > 0)
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
            int HostIndex;
            AnsiString HostName;
            AnsiString Connection;
            TRemotePlayer Player;
            TSetupControl Control;
            if (Setup.FSetupControl != null)
            {
                Control = Setup.FSetupControl;
                Setup.PlayerList.Items.Clear();
                Setup.PingList.Items.Clear();
                Setup.PreferredRaceList.Items.Clear();
                for (int I = 0, last = Control.FSettings.FRemotePlayers.Count - 1; I <= last; I++)
                {
                    Player = Control.FSettings.FRemotePlayers.GetPlayers(I);
                    Setup.PlayerList.Items.AddObject(Player.FName, IntToObject(Player.FRemoteID));
                    if (Player.FPreferredRace != 255)
                    {
                        var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[(sbyte)Player.FPreferredRace]);
                        Setup.PreferredRaceList.Items.Add(raceNamesPlural);
                    }
                    else
                    {
                        var none = TranslateRStr(NoneRStr);
                        Setup.PreferredRaceList.Items.Add(none);
                    }

                    Setup.PingList.Items.AddObject((AnsiString)"", IntToObject(Player.FRemoteID));
                }

                var count = Setup.PlayerList.Items.Count;
                Setup.PlayerListSB.Max = count - 1;
                Setup.SelectPlayer(Setup.PlayerList.Selected);
                Setup.CustomizeLeaders.Checked = Setup.FSetupControl.FSettings.FCustomizeLeaders;
                var indexOfObject = Setup.PlayModeCB.Items.IndexOfObject(IntToObject(Setup.FSetupControl.FSettings.FPlayMode));
                Setup.PlayModeCB.Selected = indexOfObject;
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
                bool condition = Setup.FSetupControl.GetHost() && Setup.FSetupControl.GetPlayerSetupValid();
                Setup.StartBtn.Visible = condition;
                var host = Setup.FSetupControl.GetHost();
                Setup.PickScenarioBtn.Visible = host;
                var fSetupControlHost = Setup.FSetupControl.GetHost();
                Setup.SettingsFogPnl.Visible = !fSetupControlHost;
                Setup.RestartSettingsFogPnl.Visible = !Setup.SettingsFogPnl.Visible && Setup.FSetupControl.FSettings.FMapDay != 0;
                Setup.LeadersOnMapCB.Checked = Setup.FSetupControl.FSettings.FLeadersOnMap;
                Setup.AlliedVictoryCB.Checked = Setup.FSetupControl.FSettings.FAlliedVictory;
                var itemsIndexOfObject = Setup.TacticalCombatCB.Items.IndexOfObject(IntToObject(Setup.FSetupControl.FSettings.FTacticalCombatAvailability));
                Setup.TacticalCombatCB.Selected = itemsIndexOfObject;
                Setup.AlwaysViewTCCB.Visible = Setup.TacticalCombatCB.Visible && Setup.FSetupControl.FSettings.FTacticalCombatAvailability != 0;
                Setup.AlwaysViewTCCB.Checked = Setup.FSetupControl.FSettings.FAlwaysViewTC;
                Setup.AlwaysViewTCLbl.Visible = Setup.AlwaysViewTCCB.Visible;
                var remote = Setup.FSetupControl.GetRemote();
                Setup.PreferredRaceLbl.Visible = remote;
                var fSetupControlRemote = Setup.FSetupControl.GetRemote();
                Setup.PreferredRaceCB.Visible = fSetupControlRemote;
                if (Setup.PreferredRaceCB.Visible)
                {
                    Setup.PreferredRaceCB.Items.Clear();
                    var none = TranslateRStr(NoneRStr);
                    Setup.PreferredRaceCB.Items.AddObject(none, IntToObject(-1));
                    for (int I = 0, last = Setup.FSetupControl.FSettings.Count - 1; I <= last; I++)
                    {
                        if (!Setup.FSetupControl.FSettings.GetSettings(I).FStatus.Contains(TPlayerSetupStatusElement.pssFixed))
                        {
                            var settingRace = Setup.FSetupControl.FSettings.GetSettings(I);
                            var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[settingRace.FRace]);
                            var setting = Setup.FSetupControl.FSettings.GetSettings(I);
                            Setup.PreferredRaceCB.Items.AddObject(raceNamesPlural, IntToObject(setting.FRace));
                        }
                    }

                    var aowComboBox = Setup.PreferredRaceCB;
                    aowComboBox.NumberVisible = 5;
                    aowComboBox.FSized = false;
                    aowComboBox.Selected = aowComboBox.Items.IndexOfObject(IntToObject((sbyte)Setup.FSetupControl.FRemotePlayer.FPreferredRace));
                }

                var indexOfRace = Control.FSettings.IndexOfRace(0);
                UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace), Setup.R1PlayerCB, Setup.R1BehaviorCB, Setup.R1PlayerIcon, Setup.R1PlayerLbl, Setup.R1InfoLbl, Setup.R1FogPnl);
                var fSettingsIndexOfRace = Control.FSettings.IndexOfRace(1);
                UpdateScenarioRow(Setup, Control.FSettings.GetSettings(fSettingsIndexOfRace), Setup.R2PlayerCB, Setup.R2BehaviorCB, Setup.R2PlayerIcon, Setup.R2PlayerLbl, Setup.R2InfoLbl, Setup.R2FogPnl);
                var indexOfRace2 = Control.FSettings.IndexOfRace(2);
                UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace2), Setup.R3PlayerCB, Setup.R3BehaviorCB, Setup.R3PlayerIcon, Setup.R3PlayerLbl, Setup.R3InfoLbl, Setup.R3FogPnl);
                var indexOfRace3 = Control.FSettings.IndexOfRace(3);
                UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace3), Setup.R4PlayerCB, Setup.R4BehaviorCB, Setup.R4PlayerIcon, Setup.R4PlayerLbl, Setup.R4InfoLbl, Setup.R4FogPnl);
                var indexOfRace4 = Control.FSettings.IndexOfRace(4);
                UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace4), Setup.R5PlayerCB, Setup.R5BehaviorCB, Setup.R5PlayerIcon, Setup.R5PlayerLbl, Setup.R5InfoLbl, Setup.R5FogPnl);
                var indexOfRace5 = Control.FSettings.IndexOfRace(5);
                UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace5), Setup.R6PlayerCB, Setup.R6BehaviorCB, Setup.R6PlayerIcon, Setup.R6PlayerLbl, Setup.R6InfoLbl, Setup.R6FogPnl);
                var indexOfRace6 = Control.FSettings.IndexOfRace(6);
                UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace6), Setup.R7PlayerCB, Setup.R7BehaviorCB, Setup.R7PlayerIcon, Setup.R7PlayerLbl, Setup.R7InfoLbl, Setup.R7FogPnl);
                var indexOfRace7 = Control.FSettings.IndexOfRace(7);
                UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace7), Setup.R8PlayerCB, Setup.R8BehaviorCB, Setup.R8PlayerIcon, Setup.R8PlayerLbl, Setup.R8InfoLbl, Setup.R8FogPnl);
                var indexOfRace8 = Control.FSettings.IndexOfRace(8);
                UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace8), Setup.R9PlayerCB, Setup.R9BehaviorCB, Setup.R9PlayerIcon, Setup.R9PlayerLbl, Setup.R9InfoLbl, Setup.R9FogPnl);
                var indexOfRace9 = Control.FSettings.IndexOfRace(9);
                UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace9), Setup.R10PlayerCB, Setup.R10BehaviorCB, Setup.R10PlayerIcon, Setup.R10PlayerLbl, Setup.R10InfoLbl, Setup.R10FogPnl);
                var indexOfRace10 = Control.FSettings.IndexOfRace(10);
                UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace10), Setup.R11PlayerCB, Setup.R11BehaviorCB, Setup.R11PlayerIcon, Setup.R11PlayerLbl, Setup.R11InfoLbl, Setup.R11FogPnl);
                var indexOfRace11 = Control.FSettings.IndexOfRace(11);
                UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace11), Setup.R12PlayerCB, Setup.R12BehaviorCB, Setup.R12PlayerIcon, Setup.R12PlayerLbl, Setup.R12InfoLbl, Setup.R12FogPnl);
                if (Setup.FSetupControl.GetRemote())
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
                    HostIndex = setupSettings.FRemotePlayers.IndexOfRemoteID(setupSettings.FHostRemoteID);

                    if (HostIndex != -1)
                    {
                        HostName = Setup.FSetupControl.FSettings.FRemotePlayers.GetPlayers(HostIndex).FName;
                    }
                    else
                    {
                        HostName = AnsiString.FromBytes(63);
                    }

                    Setup.HostNameLbl.Visible = true;
                    Setup.FileNameLbl.Visible = true;
                    Setup.PlayerNameLbl.Visible = true;
                    var format = AoWE.AoWE.Format(TranslateRStr(XHostYRStr), [Connection, HostName]);
                    Setup.HostNameLbl.Text = format;
                    var aoWEFormat = AoWE.AoWE.Format(TranslateRStr(PlayerNameXRStr), [Setup.FSetupControl.FRemotePlayer.FName]);
                    Setup.PlayerNameLbl.Text = aoWEFormat;
                    var format2 = AoWE.AoWE.Format(TranslateRStr(FileNameXRStr), [Setup.FSetupControl.FSettings.FMapName]);
                    Setup.FileNameLbl.Text = format2;
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

                var format3 = AoWE.AoWE.Format(TranslateRStr(ScenarioXRStr), [Setup.FSetupControl.FSettings.FFileName]);
                Setup.ScenarioLbl.Text = format3;
                TitleWindow.TitleWin.FDrawn = false;
            }
        }

        // E2GameFlow.pas:45
        public static void StartCampaignJoin(TCampaignJoinDlg Dialog, int Faction)
        {
            TStringList Lines;
            TWMCampaign Campaign;
            int Index;
            int I;
            TAoWHSMap Preview;
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

            ParseFontText(Lines.Text, Dialog.FScrollText, FontModule.AoW15WhiteGrey, 300);
            Campaign = WorldMap.GetCampaign();
            var path = GetCampaignScenario(Campaign, 0).FPaths[0];
            Index = Campaign.FScenarios.IndexOfID(path);
            var campaignScenario = GetCampaignScenario(WorldMap.GetCampaign(), Index);
            Preview = (TAoWHSMap)AoWEngine.CreatePreviewHSM(AoWEngine.StartupDirectory + (AnsiString)"Scenario\\Campaign\\" + campaignScenario.FFileName);
            I = 0;
            // { Original uses the preview map child count as the player-search bound. }
            while (I < Preview.Count && (int)Preview.FPlayers.GetPlayers(I).FPlayerType != 0)
            {
                I += 1;
            }

            var player = Preview.FPlayers.GetPlayers(I);
            Dialog.FLeader.Assign(player.FLeaderSetup, AoWEngine);
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

            var loading = TranslateRStr(LoadingRStr);
            var fileName = ExtractFileName(FileName);
            ProgressDialog.Title.Text = loading + (AnsiString)": " + fileName + (AnsiString)"...";
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
            Controller.FWindowsHidden = false;
            SongPlayer.Songs.Clear();
            if (AoWHSMap.FGameSongs.Songs.Count != 0)
            {
                SongPlayer.Songs.AddStrings(AoWHSMap.FGameSongs.Songs);
            }
            else
            {
                if (AoWHSMap.FStartupSongs.Songs.Count != 0)
                {
                    for (int I = 0, last = AoWHSMap.FStartupSongs.Songs.Count - 1; I <= last; I++)
                    {
                        var fileNotFound = TranslateRStr(E2Globals.E2Globals.FileNotFoundRStr);
                        if (CompareText(AoWHSMap.FStartupSongs.SongNames[I], fileNotFound) != 0)
                        {
                            var song = AoWHSMap.FStartupSongs.Songs[I];
                            SongPlayer.Songs.Add(song);
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
            var background = TranslateRStr(BackgroundRStr);
            TBWindow.StartMenuBar.Items.Add(background);
            if (AoWEngine.FCampaign != null)
            {
                var campaign = TranslateRStr(CampaignRStr);
                TBWindow.StartMenuBar.Items.Add(campaign);
            }

            TitleScene.Window.Visible = false;
            TitleWindow.TitleWin.Visible = false;
            TBWindow.TaskWindow.Visible = true;
            MWindow.MapWindow.Visible = true;
            SWindow.ScannerWindow.Visible = true;
            MWindow.MapViewer.SetHSMap(AoWHSMap);
            SWindow.Scanner.SetMap(AoWHSMap);
            var mapLevel = AoWHSMap.MapContainer.MapLevels[0];
            SWindow.Scanner.SetLevel(mapLevel);
            TBWindow.TurnButton.FButtonChecked = false;
            TBWindow.ChatBtn.Visible = SetupEvents.DPlay.Connected;
            Controller.Manager.FAligned = false;
            SWindow.Scanner.FPreviousWidth = 0;
            SWindow.Scanner.FIncremental = false;
            SWindow.Scanner.FSized = false;
            SWindow.ScannerTab.Items.Clear();
            var surface = TranslateRStr(SurfaceRStr);
            SWindow.ScannerTab.Items.Add(surface);
            if (AoWHSMap.MapContainer.LevelCount > 1)
            {
                var caverns = TranslateRStr(CavernsRStr);
                SWindow.ScannerTab.Items.Add(caverns);
            }

            if (AoWHSMap.MapContainer.LevelCount > 2)
            {
                var depths = TranslateRStr(DepthsRStr);
                SWindow.ScannerTab.Items.Add(depths);
            }

            SWindow.ScannerTab.Selected = 0;
            SWindow.ScannerTab.FSized = false;
            SWindow.Scanner.SetTWidth(0);
            SWindow.Scanner.SetTHeight(0);
            SWindow.Scanner.SetTLeft(-1);
            SWindow.Scanner.SetTTop(-1);
            SWindow.Scanner.FUpdateStep = -1;
            SWindow.Scanner.SetMap(AoWHSMap);
            var mapLevel2 = AoWHSMap.MapContainer.MapLevels[0];
            SWindow.Scanner.SetLevel(mapLevel2);
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
                    aowTurnButton.FLastPlayer = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FPlayerRace;
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
            byte B;
            int I;
            bool Visible;
            bool Found;
            if (Settings != null)
            {
                Visible = Setup.FSetupControl.GetHost() && !Settings.FStatus.Contains(TPlayerSetupStatusElement.pssFixed);
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
                    I = PlayerBox.Items.Count;
                    while (!Found && I != 0)
                    {
                        I -= 1;
                        var item = PlayerBox.Items[I];
                        if (Setup.FSetupControl.PlayerNameToPlayerID(item, ref Behavior))
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
                        I = Setup.FSetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(Settings.FBehavior.Flags);
                        if (I != -1)
                        {
                            PlayerName.Text = Setup.FSetupControl.FSettings.FRemotePlayers.GetPlayers(I).FName;
                        }
                        else
                        {
                            PlayerName.Text = TranslateRStr(HumanRStr);
                        }
                    }
                    else
                    {
                        PlayerName.Text = TranslateRStr(TPlayerTypeRStr[Settings.FBehavior.PlayerType]);
                    }
                }

                Info.Text = (AnsiString)"";
                Info.Visible = Setup.FSetupControl.GetRemote() || Settings.FStatus.Contains(TPlayerSetupStatusElement.pssFixed);
                if (Info.Visible)
                {
                    if (Settings.FStatus.Contains(TPlayerSetupStatusElement.pssFixed))
                    {
                        Info.Text = TranslateRStr(FixedRStr);
                    }
                    else
                    {
                        if (Setup.FSetupControl.GetRemote() && Settings.FStatus.Contains(TPlayerSetupStatusElement.pssForced) && Settings.FBehavior.PlayerType == 0)
                        {
                            I = Setup.FSetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(Settings.FBehavior.Flags);
                            if (I != -1)
                            {
                                // { Original compares race bytes, including the FF sentinel. }
                                if (Setup.FSetupControl.FSettings.FRemotePlayers.GetPlayers(I).FPreferredRace == (byte)Settings.FRace)
                                {
                                    Info.Text = TranslateRStr(TakenRStr);
                                }
                                else
                                {
                                    Info.Text = TranslateRStr(ForcedRStr);
                                }
                            }
                        }
                        else
                        {
                            if (Settings.FBehavior.Flags != 0)
                            {
                                Info.Text = TranslateRStr(TakenRStr);
                            }
                            else
                            {
                                if (TSet<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType))
                                {
                                    Info.Text = TranslateRStr(TPlayerBehaviorRStr[(int)(TPlayerBehavior)Settings.FBehaviorMode]);
                                }
                            }
                        }
                    }
                }

                BehaviorBox.Visible = Setup.FSetupControl.GetHost() && TSet<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType);
                if (BehaviorBox.Visible)
                {
                    BehaviorBox.Items.BeginUpdate();
                    BehaviorBox.Items.Clear();
                    for (int bOrdinal = 0; bOrdinal <= 5; bOrdinal++)
                    {
                        B = (byte)bOrdinal;
                        var playerBehavior = TranslateRStr(TPlayerBehaviorRStr[(int)(TPlayerBehavior)B]);
                        BehaviorBox.Items.AddObject(playerBehavior, IntToObject(B));
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
                Info.Text = TranslateRStr(UnavailableRStr);
                Fog.Visible = true;
            }
        }

        // E2GameFlow.pas:298
        public static void InitializeScenarioSetup(TScenSetup Setup, TSetupControl Control)
        {
            byte B;
            Setup.FSetupControl = Control;
            TitleWindow.TitleWin.Visible = true;
            Setup.ScenSetupPnl.Visible = true;
            Setup.ChatMemo.Items.Clear();
            Setup.ChatEdit.Text = (AnsiString)"";
            Setup.FValidateDelay = 100;
            Setup.FPingDelay = 15;
            var remote = Setup.FSetupControl.GetRemote();
            Setup.ChatFogPnl.Visible = !remote;
            var fSetupControlRemote = Setup.FSetupControl.GetRemote();
            Setup.ChatMemo.CaptureInput = fSetupControlRemote;
            var remote2 = Setup.FSetupControl.GetRemote();
            Setup.ChatEdit.CaptureInput = remote2;
            if (Setup.FSetupControl.GetHost() && Setup.FSetupControl.FSettings.FMapName == (AnsiString)"")
            {
                Setup.PickMap();
            }

            if (Setup.FSetupControl.GetRemote())
            {
                Setup.TacticalCombatCB.Visible = true;
                Setup.TacticalCombatLbl.Visible = true;
                Setup.TacticalCombatCB.Items.Clear();
                for (int bOrdinal = 0; bOrdinal <= 2; bOrdinal++)
                {
                    B = (byte)bOrdinal;
                    var tacticalCombatAvailability = TranslateRStr(TTacticalCombatAvailabilityRStr[B]);
                    Setup.TacticalCombatCB.Items.AddObject(tacticalCombatAvailability, IntToObject(B));
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
                var classic = TranslateRStr(ClassicRStr);
                Setup.PlayModeCB.Items.AddObject(classic, IntToObject(1));
            }
            else
            {
                Setup.CustomizeLeadersLbl.TextColor = Setup.AlliedVictoryLbl.TextColor;
                Setup.CustomizeLeaders.CaptureInput = true;
                Setup.PlayModeCB.Items.Clear();
                var simultaneous = TranslateRStr(SimultaneousRStr);
                Setup.PlayModeCB.Items.AddObject(simultaneous, IntToObject(2));
                var classic = TranslateRStr(ClassicRStr);
                Setup.PlayModeCB.Items.AddObject(classic, IntToObject(1));
            }

            bool condition = SongPlayer.CurrentIndex != -1;
            if (condition)
            {
                var count = SongPlayer.Songs.Count;
                condition = SongPlayer.CurrentIndex < count;
            }

            if (!(condition && SongPlayer.Songs[SongPlayer.CurrentIndex] == (AnsiString)"SONGS\\TITLE.IT"))
            {
                SongPlayer.Songs.Clear();
                SongPlayer.Songs.Add((AnsiString)"SONGS\\TITLE.IT");
                SongPlayer.Play(0);
            }

            UpdateScenarioSetup(Setup);
        }
    }
}
