// Translated from aowSetupEvents.pas.
using Aow.Game.Audio;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

public partial class SetupEvents : DataModule
{
    public SetupEvents(Component owner) : base(owner)
    { }

    public GDPlay DPlay;
    public SetupControl SetupControl;
    public bool FLobbyGame;

    // aowSetupEvents.pas:61
    public void ClearChat()
    {
        DialogHosts.ChatWindow.ChatMemo.Items.Clear();
        DialogHosts.ChatWindow.ChatEdit.Text = "";
    }

    // aowSetupEvents.pas:67
    public void UpdateChatPlayers()
    {
        DialogHosts.ChatWindow.PlayerList.Items.Clear();
        var allPlayers = AbilityRules.TranslateRStr(AllPlayersRStr);
        DialogHosts.ChatWindow.PlayerList.Items.AddObject(allPlayers, null);
        if (AoWHSMap.Current != null)
        {
            for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
            {
                if (I != AoWHSMap.Current.FSeatedPlayer && AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus != Player.Status.Terminated && AoWHSMap.Current.FPlayerCommunicator.PCC.Players[I].ConnectionMode == ConnectionMode.Remote)
                {
                    var player = AoWHSMap.Current.FPlayerCommunicator.PCC.Players[I];
                    var playerLeader = AoWHSMap.Current.FPlayers.GetPlayers(I);
                    DialogHosts.ChatWindow.PlayerList.Items.AddObject(playerLeader.FLeader.FHeroName, IntToObject(player.SystemID));
                    DialogHosts.ChatWindow.PlayerList.Selected = 0;
                }
            }
        }
    }

    // aowSetupEvents.pas:84
    public void UpdatePlayerName(string Name)
    {
        if (SetupControl.GetRemote() && DPlay.Connected)
        {
            DPlay.PlayerName = Name;
        }
    }

    // aowSetupEvents.pas:90
    public void SetupControlStart()
    {
        DialogHosts.SynchronizeDlg.Dlg.Visible = false;
        DialogHosts.ScenSetup.CloseSetup();
        General.StartGame(DialogHosts.General);
        if (DPlay.Connected)
        {
            ClearChat();
            UpdateChatPlayers();
        }
    }

    // aowSetupEvents.pas:102
    public void SetupControlSendMap(SetupControl Control, object Data, int Size, int Recipient)
    {
        /* TODO: Implement aowDPlayComp.Players in the platform or editor service. */
        throw new NotImplementedException();
    }

    // aowSetupEvents.pas:111
    public void DPlayFileStartMessage(int ID, ref string Name, int Size, ref byte Response)
    {
        DialogHosts.ProgressDialog.StatusLbl.Text = "0k of " + IntToStr(Size / 1000) + "k";
        DialogHosts.ProgressDialog.Title.Text = "Receiving File";
        DialogHosts.ProgressDialog.Bar.Max = Size;
        DialogHosts.ProgressDialog.Bar.Position = 0;
    }

    // aowSetupEvents.pas:119
    public void DPlayFileDoneMessage(int ID, GDFile FileData)
    {
        /* TODO: Implement aowDPlayComp.FData in the platform or editor service. */
        throw new NotImplementedException();
    }

    // aowSetupEvents.pas:125
    public void DPlayFileProgressMessage(int ID, int Position, int Size)
    {
        if (Position > DialogHosts.ProgressDialog.Bar.Position)
        {
            DialogHosts.ProgressDialog.StatusLbl.Text = IntToStr(Position / 1000) + "k of " + IntToStr(Size / 1000) + "k";
            DialogHosts.ProgressDialog.Bar.Position = Position;
        }
    }

    // aowSetupEvents.pas:134
    public void DPlayChatMessage(ref string Message, ref int ToID, ref int FromID)
    {
        if (DialogHosts.ScenSetup.ScenSetupPnl.Visible)
        {
            DialogHosts.ScenSetup.ChatMemo.Items.Add(Message);
            DialogHosts.ScenSetup.FChatScrollPending = true;
        }
        else
        {
            DialogHosts.ChatWindow.ChatMemo.Items.Add(Message);
            DialogHosts.TBWindow.FChatFlash = 136;
            DialogHosts.ChatWindow.FScrollPending = true;
            AudioPlayback.PlaySample("SFX\\EVENTS\\CHAT.WAV");
        }
    }

    // aowSetupEvents.pas:150
    public void DPlayMessageTimeout(ref GDMessage Message)
    {
        /* TODO: Implement aowDPlayComp.FToID in the platform or editor service. */
        throw new NotImplementedException();
    }

    // aowSetupEvents.pas:170
    public void DPlayEjected()
    {
        if (DialogHosts.ScenSetup.ScenSetupPnl.Visible)
        {
            DialogHosts.ScenSetup.ExitGameClick();
            MessageDlg.ResetMessageDialog(DialogHosts.MessageDlg);
            var warning = AbilityRules.TranslateRStr(WarningRStr);
            DialogHosts.MessageDlg.TitleLbl.Text = warning;
            DialogHosts.MessageDlg.MessageMemo.Items.Add("");
            var youWereEjectedByTheHost = AbilityRules.TranslateRStr(YouWereEjectedByTheHostRStr);
            DialogHosts.MessageDlg.MessageMemo.Items.Add(youWereEjectedByTheHost);
            DialogHosts.MessageDlg.OKPnl.Visible = true;
            DialogHosts.MessageDlg.Dlg.Visible = true;
        }
    }

    // aowSetupEvents.pas:184
    public void DPlayPingMessage(int PingMS, int Player)
    {
        if (DialogHosts.ScenSetup.ScenSetupPnl.Visible)
        {
            DialogHosts.ScenSetup.PingList.FDrawn = false;
        }
    }

    // aowSetupEvents.pas:189
    public void SetupControlCustomizeLeader(SetupControl Control, CustomizeLeaderControl Leader)
    {
        DialogHosts.ScenSetup.ScenSetupPnl.Visible = false;
        DialogHosts.LeaderSetupWin.Execute(Leader);
    }

    // aowSetupEvents.pas:195
    public void SetupControlLoadMap(SetupControl Control, string FileName)
    {
        var fileName = ExtractFileName(FileName);
        var format = AbilityRules.Format(AbilityRules.TranslateRStr(LoadingXRStr), [fileName]);
        DialogHosts.ProgressDialog.Title.Text = format;
        DialogHosts.ProgressDialog.StatusLbl.Text = "";
        DialogHosts.ProgressDialog.Bar.Position = 0;
        DialogHosts.ProgressDialog.Bar.Max = 100;
        DialogHosts.ProgressDialog.ProgDlg.Visible = true;
        DialogHosts.General.Manager.OverlayPriority = 5000;
        if (!Control.FSettings.FCustomizeLeaders)
        {
            AudioHost.SongPlayer.Stop(false);
            AudioHost.SongPlayer.Songs.Clear();
            AudioPlayback.FadeMusicVolume(0, 1);
        }

        if (Control.FStream.Length != 0)
        {
            GameTables.AoWEngine.LoadHSMfromStream(Control.FStream, false, (_, Message, Progression) => DialogHosts.General.MapProgress(Message, Progression));
            var fileName2 = ExtractFileName(FileName);
            AoWHSMap.Current.FileName = GameTables.AoWEngine.StartupDirectory + "Scenario\\" + fileName2;
        }
        else
        {
            GameTables.AoWEngine.LoadHSM(FileName, false, (_, Message, Progression) => DialogHosts.General.MapProgress(Message, Progression), Aow.Formats.GameProfile.WorldMap);
        }

        DialogHosts.General.Manager.OverlayPriority = 0;
        DialogHosts.ProgressDialog.ProgDlg.Visible = false;
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
        ScenSetup.UpdateScenarioSetup(DialogHosts.ScenSetup);
    }

    // aowSetupEvents.pas:228
    public void SetupControlEjectPlayer(SetupControl Control, int RemoteID)
    {
        /* TODO: Implement aowDPlayComp.EjectPlayer in the platform or editor service. */
        throw new NotImplementedException();
    }

    // aowSetupEvents.pas:231
    public void DPlayMessage(ref object Data, ref int Size, ref int ToID, ref int FromID)
    {
        if (SetupControl.FStatus.Contains(SetupStatusElement.Busy))
        {
            SetupControl.DataReceived(Data, Size, FromID);
        }

        if (AoWHSMap.Current != null && AoWHSMap.Current.Status.Contains(HSMapStatusElement.Activated))
        {
            if (AoWHSMap.Current.FPlayerCommunicator.PCC is RemotePCC)
            {
                ((RemotePCC)AoWHSMap.Current.FPlayerCommunicator.PCC).DataReceived(Data, Size, FromID);
            }
        }
    }

    // aowSetupEvents.pas:239
    public void SetupControlSynchronize(SetupControl Control)
    {
        DialogHosts.General.Manager.FAligned = false;
        DialogHosts.ScenSetup.ScenSetupPnl.Visible = false;
        if (Control.GetRemote())
        {
            DialogHosts.SynchronizeDlg.Dlg.Visible = true;
        }
    }

    // aowSetupEvents.pas:249
    public void SetupControlIncompatibleVersion(SetupControl Control, int Major, int Minor)
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
        DialogHosts.General.Manager.OverlayPriority = 5000;
        DialogHosts.ProgressDialog.ProgDlg.Visible = true;
        DialogHosts.ProgressDialog.Title.Text = "Transferring Files...";
        DialogHosts.ProgressDialog.StatusLbl.Text = "Please Wait";
        DialogHosts.ProgressDialog.Bar.Position = 0;
    }

    // aowSetupEvents.pas:291
    public void DPlayMessageError()
    {
        if (AoWHSMap.Current != null)
        {
            AoWHSMap.Current.PlayerConnectionLost();
        }
    }

    // aowSetupEvents.pas:299
    public void DPlayDestroyPlayer(int ID, int NumPlayers)
    {
        sbyte Player;
        if (DialogHosts.ScenSetup.FSetupControl != null)
        {
            if (DialogHosts.ScenSetup.FSetupControl.GetHost())
            {
                DialogHosts.ScenSetup.FSetupControl.hDisconnectPlayer(ID);
            }
        }

        if (AoWHSMap.Current != null && AoWHSMap.Current.FPlayerCommunicator.PCC != null)
        {
            Player = (sbyte)AoWHSMap.Current.FPlayerCommunicator.PCC.IndexOfSystemID(ID);
            if (Player != -1)
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(Player) != null)
                {
                    if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatus == Session.Player.Status.Playing)
                    {
                        AoWHSMap.Current.PlayerDisconnected(Player);
                    }

                    UpdateChatPlayers();
                }
            }
        }
    }

    private static readonly string AllPlayersRStr = "All players";

    private static readonly string LoadingXRStr = "Loading: %s...";

    private static readonly string YouWereEjectedByTheHostRStr = "You were ejected by the host.";

    private static GUID ID_AOWAPP = "{3205D060-4FF5-11D3-811F-0050042E6D24}";

    private static readonly string WarningRStr = "Warning";

    internal static bool StartLobby(SetupEvents Events)
    {
        /* TODO: Implement aowDPlayComp.CheckLobby in the platform or editor service. */
        throw new NotImplementedException();
    }

}
