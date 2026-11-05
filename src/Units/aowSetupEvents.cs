// Translated from aowSetupEvents.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using Aow.Units.HSEngine;
using Aow.Units.AoWSetup;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.E2Globals.E2Globals;
using static Aow.Units.E2GameFlow.E2GameFlow;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.AoWMessageDlg.AoWMessageDlg;

namespace Aow.Units.aowSetupEvents
{
    public partial class TSetupEvents : TDataModule
    {
        public TSetupEvents(TComponent owner) : base(owner)
        { }

        public TGDPlay DPlay;
        public TSetupControl SetupControl;
        public bool FLobbyGame;
        // aowSetupEvents.pas:61
        public void ClearChat()
        {
            ChatWindow.ChatMemo.Items.Clear();
            ChatWindow.ChatEdit.Text = (AnsiString)"";
        }

        // aowSetupEvents.pas:67
        public void UpdateChatPlayers()
        {
            int I = default;
            ChatWindow.PlayerList.Items.Clear();
            var call__1 = TranslateRStr(AllPlayersRStr);
            ChatWindow.PlayerList.Items.AddObject(call__1, null);
            if (AoWHSMap != null)
            {
                {
                    I = 1;
                    int end__2 = AoWHSMap.FPlayers.Count - 1;
                    for (; I <= end__2; I += 1)
                    {
                        bool bool__3 = I != AoWHSMap.FSeatedPlayer;
                        if (bool__3)
                        {
                            var call__4 = AoWHSMap.FPlayers.GetPlayers(I);
                            bool__3 = call__4.FStatus != TPlayerStatus.psTerminated;
                        }

                        bool bool__5 = bool__3;
                        if (bool__5)
                        {
                            var call__6 = AoWHSMap.FPlayerCommunicator.PCC.Players[I];
                            bool__5 = call__6.ConnectionMode == TConnectionMode.cmRemote;
                        }

                        if (bool__5)
                        {
                            var call__7 = AoWHSMap.FPlayerCommunicator.PCC.Players[I];
                            var call__8 = AoWHSMap.FPlayers.GetPlayers(I);
                            ChatWindow.PlayerList.Items.AddObject(call__8.FLeader.FHeroName, IntToObject(call__7.SystemID));
                            ChatWindow.PlayerList.Selected = 0;
                        }
                    }
                }
            }
        }

        // aowSetupEvents.pas:84
        public void UpdatePlayerName(AnsiString Name)
        {
            /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
            throw new NotImplementedException();
        }

        // aowSetupEvents.pas:90
        public void SetupControlStart()
        {
            SynchronizeDlg.Dlg.Visible = false;
            ScenSetup.CloseSetup();
            AoWWinMan.AoWWinMan.StartGame(General);
            if (DPlay.Connected)
            {
                ClearChat();
                UpdateChatPlayers();
            }
        }

        // aowSetupEvents.pas:102
        public void SetupControlSendMap(TSetupControl Control, object Data, int Size, int Recipient)
        {
            /* TODO: Implement aowDPlayComp.Players in the platform or editor service. */
            throw new NotImplementedException();
        }

        // aowSetupEvents.pas:111
        public void DPlayFileStartMessage(int ID, ref AnsiString Name, int Size, ref byte Response)
        {
            ProgressDialog.StatusLbl.Text = (AnsiString)"0k of " + IntToStr(Size / 1000) + AnsiString.FromBytes(107);
            ProgressDialog.Title.Text = (AnsiString)"Receiving File";
            ProgressDialog.Bar.Max = Size;
            ProgressDialog.Bar.Position = 0;
        }

        // aowSetupEvents.pas:119
        public void DPlayFileDoneMessage(int ID, TGDFile FileData)
        {
            /* TODO: Implement aowDPlayComp.FData in the platform or editor service. */
            throw new NotImplementedException();
        }

        // aowSetupEvents.pas:125
        public void DPlayFileProgressMessage(int ID, int Position, int Size)
        {
            if (Position > ProgressDialog.Bar.Position)
            {
                ProgressDialog.StatusLbl.Text = IntToStr(Position / 1000) + (AnsiString)"k of " + IntToStr(Size / 1000) + AnsiString.FromBytes(107);
                ProgressDialog.Bar.Position = Position;
            }
        }

        // aowSetupEvents.pas:134
        public void DPlayChatMessage(ref AnsiString Message, ref int ToID, ref int FromID)
        {
            if (ScenSetup.ScenSetupPnl.Visible)
            {
                ScenSetup.ChatMemo.Items.Add(Message);
                ScenSetup.FChatScrollPending = true;
            }
            else
            {
                ChatWindow.ChatMemo.Items.Add(Message);
                TBWindow.FChatFlash = 136;
                ChatWindow.FScrollPending = true;
                PlaySample((AnsiString)"SFX\\EVENTS\\CHAT.WAV");
            }
        }

        // aowSetupEvents.pas:150
        public void DPlayMessageTimeout(ref TGDMessage Message)
        {
            /* TODO: Implement aowDPlayComp.FToID in the platform or editor service. */
            throw new NotImplementedException();
        }

        // aowSetupEvents.pas:170
        public void DPlayEjected()
        {
            if (ScenSetup.ScenSetupPnl.Visible)
            {
                ScenSetup.ExitGameClick(null);
                ResetMessageDialog(MessageDlg);
                var call__9 = TranslateRStr(E2Globals.E2Globals.WarningRStr);
                MessageDlg.TitleLbl.Text = call__9;
                MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                var call__10 = TranslateRStr(YouWereEjectedByTheHostRStr);
                MessageDlg.MessageMemo.Items.Add(call__10);
                MessageDlg.OKPnl.Visible = true;
                MessageDlg.Dlg.Visible = true;
            }
        }

        // aowSetupEvents.pas:184
        public void DPlayPingMessage(int PingMS, int Player)
        {
            if (ScenSetup.ScenSetupPnl.Visible)
            {
                ScenSetup.PingList.FDrawn = false;
            }
        }

        // aowSetupEvents.pas:189
        public void SetupControlCustomizeLeader(TSetupControl Control, TCustomizeLeaderControl Leader)
        {
            ScenSetup.ScenSetupPnl.Visible = false;
            AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.Execute(Leader);
        }

        // aowSetupEvents.pas:195
        public void SetupControlLoadMap(TSetupControl Control, AnsiString FileName)
        {
            var call__11 = ExtractFileName(FileName);
            var call__12 = TranslateRStr(LoadingXRStr);
            var call__13 = AoWE.AoWE.Format(call__12, [call__11]);
            ProgressDialog.Title.Text = call__13;
            ProgressDialog.StatusLbl.Text = (AnsiString)"";
            ProgressDialog.Bar.Position = 0;
            ProgressDialog.Bar.Max = 100;
            ProgressDialog.ProgDlg.Visible = true;
            General.Manager.OverlayPriority = 5000;
            if (!Control.FSettings.FCustomizeLeaders)
            {
                SongPlayer.Stop(false);
                SongPlayer.Songs.Clear();
                FadeMusicVolume(0, 1);
            }

            var call__14 = Control.FStream.Size;
            if (call__14 != 0)
            {
                AoWEngine.LoadHSMfromStream(Control.FStream, false, General.MapProgress);
                var call__15 = ExtractFileName(FileName);
                AoWHSMap.FileName = AoWEngine.StartupDirectory + (AnsiString)"Scenario\\" + call__15;
            }
            else
            {
                AoWEngine.LoadHSM(FileName, false, General.MapProgress);
            }

            General.Manager.OverlayPriority = 0;
            ProgressDialog.ProgDlg.Visible = false;
            Control.MapLoaded();
        }

        // aowSetupEvents.pas:220
        public bool SetupControlSendData(object Data, int Size, int Recipient)
        {
            /* TODO: Implement aowDPlayComp.SendMessage in the platform or editor service. */
            throw new NotImplementedException();
        }

        // aowSetupEvents.pas:225
        public void SetupControlSettingChanged()
        {
            UpdateScenarioSetup(ScenSetup);
        }

        // aowSetupEvents.pas:228
        public void SetupControlEjectPlayer(TSetupControl Control, int RemoteID)
        {
            /* TODO: Implement aowDPlayComp.EjectPlayer in the platform or editor service. */
            throw new NotImplementedException();
        }

        // aowSetupEvents.pas:231
        public void DPlayMessage(ref object Data, ref int Size, ref int ToID, ref int FromID)
        {
            if (SetupControl.FStatus.Contains(TSetupStatusElement.ssBusy))
            {
                SetupControl.DataReceived(Data, Size, FromID);
            }

            if (AoWHSMap != null && AoWHSMap.Status.Contains(THSMapStatusElement.msActivated))
            {
                if (AoWHSMap.FPlayerCommunicator.PCC is TRemotePCC)
                {
                    ((TRemotePCC)AoWHSMap.FPlayerCommunicator.PCC).DataReceived(Data, Size, FromID);
                }
            }
        }

        // aowSetupEvents.pas:239
        public void SetupControlSynchronize(TSetupControl Control)
        {
            General.Manager.FAligned = false;
            ScenSetup.ScenSetupPnl.Visible = false;
            if (Control.GetRemote())
            {
                SynchronizeDlg.Dlg.Visible = true;
            }
        }

        // aowSetupEvents.pas:249
        public void SetupControlIncompatibleVersion(TSetupControl Control, int Major, int Minor)
        {
            /* TODO: Implement aowDPlayComp.CloseSession in the platform or editor service. */
            throw new NotImplementedException();
        }

        // aowSetupEvents.pas:263
        public void DPlaySessionLost()
        {
            /* TODO: Implement aowDPlayComp.DisConnect in the platform or editor service. */
            throw new NotImplementedException();
        }

        // aowSetupEvents.pas:282
        public void SetupControlSendingMaps()
        {
            General.Manager.OverlayPriority = 5000;
            ProgressDialog.ProgDlg.Visible = true;
            ProgressDialog.Title.Text = (AnsiString)"Transferring Files...";
            ProgressDialog.StatusLbl.Text = (AnsiString)"Please Wait";
            ProgressDialog.Bar.Position = 0;
        }

        // aowSetupEvents.pas:291
        public void DPlayMessageError()
        {
            if (AoWHSMap != null)
            {
                AoWHSMap.PlayerConnectionLost();
            }
        }

        // aowSetupEvents.pas:299
        public void DPlayDestroyPlayer(int ID, int NumPlayers)
        {
            sbyte Player = default;
            if (ScenSetup.FSetupControl != null)
            {
                var call__16 = ScenSetup.FSetupControl.GetHost();
                if (call__16)
                {
                    ScenSetup.FSetupControl.hDisconnectPlayer(ID);
                }
            }

            if (AoWHSMap != null && AoWHSMap.FPlayerCommunicator.PCC != null)
            {
                var call__17 = AoWHSMap.FPlayerCommunicator.PCC.IndexOfSystemID(ID);
                Player = (sbyte)call__17;
                if (Player != -1)
                {
                    var call__18 = AoWHSMap.FPlayers.GetPlayers(Player);
                    if (call__18 != null)
                    {
                        var call__19 = AoWHSMap.FPlayers.GetPlayers(Player);
                        if (call__19.FStatus == TPlayerStatus.psPlaying)
                        {
                            AoWHSMap.PlayerDisconnected(Player);
                        }

                        UpdateChatPlayers();
                    }
                }
            }
        }
    }

    public static partial class aowSetupEvents
    {
        public static TSetupEvents SetupEvents;
    }
}
