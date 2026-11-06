// Translated from aowSetupEvents.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.Sound.SoundUnit;
using Aow.Units.NetworkE;
using Aow.Units.HSEngine;
using Aow.Units.AoWSetup;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.E2Globals.E2GlobalsUnit;
using static Aow.Units.E2GameFlow.E2GameFlowUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.AoWMessageDlg.AoWMessageDlgUnit;
using Aow.Units.AoWWinMan;
using Aow.Units.E2Globals;
using Aow.Units.AoWLeaderSetup;

namespace Aow.Units.aowSetupEvents;

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
        ChatWindow.ChatEdit.Text = "";
    }

    // aowSetupEvents.pas:67
    public void UpdateChatPlayers()
    {
        ChatWindow.PlayerList.Items.Clear();
        var allPlayers = TranslateRStr(AllPlayersRStr);
        ChatWindow.PlayerList.Items.AddObject(allPlayers, null);
        if (AoWHSMap != null)
        {
            for (int I = 1, last = AoWHSMap.FPlayers.Count - 1; I <= last; I++)
            {
                if (I != AoWHSMap.FSeatedPlayer && AoWHSMap.FPlayers.GetPlayers(I).FStatus != TPlayerStatus.psTerminated && AoWHSMap.FPlayerCommunicator.PCC.Players[I].ConnectionMode == TConnectionMode.cmRemote)
                {
                    var player = AoWHSMap.FPlayerCommunicator.PCC.Players[I];
                    var playerLeader = AoWHSMap.FPlayers.GetPlayers(I);
                    ChatWindow.PlayerList.Items.AddObject(playerLeader.FLeader.FHeroName, IntToObject(player.SystemID));
                    ChatWindow.PlayerList.Selected = 0;
                }
            }
        }
    }

    // aowSetupEvents.pas:84
    public void UpdatePlayerName(string Name)
    {
        /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
        throw new NotImplementedException();
    }

    // aowSetupEvents.pas:90
    public void SetupControlStart()
    {
        SynchronizeDlg.Dlg.Visible = false;
        ScenSetup.CloseSetup();
        AoWWinManUnit.StartGame(General);
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
    public void DPlayFileStartMessage(int ID, ref string Name, int Size, ref byte Response)
    {
        ProgressDialog.StatusLbl.Text = "0k of " + IntToStr(Size / 1000) + "k";
        ProgressDialog.Title.Text = "Receiving File";
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
            ProgressDialog.StatusLbl.Text = IntToStr(Position / 1000) + "k of " + IntToStr(Size / 1000) + "k";
            ProgressDialog.Bar.Position = Position;
        }
    }

    // aowSetupEvents.pas:134
    public void DPlayChatMessage(ref string Message, ref int ToID, ref int FromID)
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
            PlaySample("SFX\\EVENTS\\CHAT.WAV");
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
            var warning = TranslateRStr(E2GlobalsUnit.WarningRStr);
            MessageDlg.TitleLbl.Text = warning;
            MessageDlg.MessageMemo.Items.Add("");
            var youWereEjectedByTheHost = TranslateRStr(YouWereEjectedByTheHostRStr);
            MessageDlg.MessageMemo.Items.Add(youWereEjectedByTheHost);
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
        AoWLeaderSetupUnit.LeaderSetupWin.Execute(Leader);
    }

    // aowSetupEvents.pas:195
    public void SetupControlLoadMap(TSetupControl Control, string FileName)
    {
        var fileName = ExtractFileName(FileName);
        var format = AoWEUnit.Format(TranslateRStr(LoadingXRStr), [fileName]);
        ProgressDialog.Title.Text = format;
        ProgressDialog.StatusLbl.Text = "";
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

        if (Control.FStream.Size != 0)
        {
            AoWEngine.LoadHSMfromStream(Control.FStream, false, General.MapProgress);
            var fileName2 = ExtractFileName(FileName);
            AoWHSMap.FileName = AoWEngine.StartupDirectory + "Scenario\\" + fileName2;
        }
        else
        {
            AoWEngine.LoadHSM(FileName, false, General.MapProgress, Aow.Formats.GameProfile.WorldMap);
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
        ProgressDialog.Title.Text = "Transferring Files...";
        ProgressDialog.StatusLbl.Text = "Please Wait";
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
        sbyte Player;
        if (ScenSetup.FSetupControl != null)
        {
            if (ScenSetup.FSetupControl.GetHost())
            {
                ScenSetup.FSetupControl.hDisconnectPlayer(ID);
            }
        }

        if (AoWHSMap != null && AoWHSMap.FPlayerCommunicator.PCC != null)
        {
            Player = (sbyte)AoWHSMap.FPlayerCommunicator.PCC.IndexOfSystemID(ID);
            if (Player != -1)
            {
                if (AoWHSMap.FPlayers.GetPlayers(Player) != null)
                {
                    if (AoWHSMap.FPlayers.GetPlayers(Player).FStatus == TPlayerStatus.psPlaying)
                    {
                        AoWHSMap.PlayerDisconnected(Player);
                    }

                    UpdateChatPlayers();
                }
            }
        }
    }
}

public static partial class aowSetupEventsUnit
{
    public static TSetupEvents SetupEvents;
}
