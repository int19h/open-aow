// Translated from aowSetupEvents.pas.
using Aow.Formats;
using Aow.Game.Audio;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public partial class SetupEvents : DataModule
{
    public SetupEvents(Component owner) : base(owner)
    { }

    public GDPlay DirectPlay;
    public SetupControl SetupControl;

    public bool LobbyGame { get; }

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
        var allPlayers = AbilityRules.TranslateResourceString(s_allPlayersText);
        DialogHosts.ChatWindow.PlayerList.Items.AddObject(allPlayers, null);
        if (StrategicMap.Current != null)
        {
            for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
            {
                if (I != StrategicMap.Current.SeatedPlayer && StrategicMap.Current.Players.GetPlayers(I).Status != PlayerStatus.Terminated && StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl.Players[I].ConnectionMode == ConnectionMode.Remote)
                {
                    var player = StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl.Players[I];
                    var playerLeader = StrategicMap.Current.Players.GetPlayers(I);
                    DialogHosts.ChatWindow.PlayerList.Items.AddObject(playerLeader.Leader.HeroName, IntToObject(player.SystemId));
                    DialogHosts.ChatWindow.PlayerList.Selected = 0;
                }
            }
        }
    }

    // aowSetupEvents.pas:84
    public void UpdatePlayerName(string name)
    {
        if (SetupControl.GetRemote() && DirectPlay.Connected)
        {
            DirectPlay.PlayerName = name;
        }
    }

    // aowSetupEvents.pas:90
    public void SetupControlStart()
    {
        DialogHosts.SynchronizeDialog.Window.Visible = false;
        DialogHosts.ScenarioSetup.CloseSetup();
        General.StartGame(DialogHosts.General);
        if (DirectPlay.Connected)
        {
            ClearChat();
            UpdateChatPlayers();
        }
    }

    // aowSetupEvents.pas:102
    public void SetupControlSendMap(SetupControl control, object data, int size, int recipient)
    {
        /* TODO: Implement aowDPlayComp.Players in the platform or editor service. */
        throw new NotImplementedException();
    }

    // aowSetupEvents.pas:111
    public void DPlayFileStartMessage(int id, ref string name, int size, ref byte response)
    {
        DialogHosts.ProgressDialog.StatusLabel.Text = "0k of " + IntToStr(size / 1000) + "k";
        DialogHosts.ProgressDialog.TitleLabel.Text = "Receiving File";
        DialogHosts.ProgressDialog.ProgressBar.Max = size;
        DialogHosts.ProgressDialog.ProgressBar.Position = 0;
    }

    // aowSetupEvents.pas:119
    public void DPlayFileDoneMessage(int id, GDFile fileData)
    {
        /* TODO: Implement aowDPlayComp.FData in the platform or editor service. */
        throw new NotImplementedException();
    }

    // aowSetupEvents.pas:125
    public void DPlayFileProgressMessage(int id, int position, int size)
    {
        if (position > DialogHosts.ProgressDialog.ProgressBar.Position)
        {
            DialogHosts.ProgressDialog.StatusLabel.Text = IntToStr(position / 1000) + "k of " + IntToStr(size / 1000) + "k";
            DialogHosts.ProgressDialog.ProgressBar.Position = position;
        }
    }

    // aowSetupEvents.pas:134
    public void DPlayChatMessage(ref string message, ref int toId, ref int fromId)
    {
        if (DialogHosts.ScenarioSetup.ScenarioSetupPanel.Visible)
        {
            DialogHosts.ScenarioSetup.ChatMemo.Items.Add(message);
            DialogHosts.ScenarioSetup.ChatScrollPending = true;
        }
        else
        {
            DialogHosts.ChatWindow.ChatMemo.Items.Add(message);
            DialogHosts.TaskBar.ChatFlash = 136;
            DialogHosts.ChatWindow.ScrollPending = true;
            AudioPlayback.PlaySample("SFX\\EVENTS\\CHAT.WAV");
        }
    }

    // aowSetupEvents.pas:150
    public void DPlayMessageTimeout(ref GDMessage message)
    {
        /* TODO: Implement aowDPlayComp.FToID in the platform or editor service. */
        throw new NotImplementedException();
    }

    // aowSetupEvents.pas:170
    public void DPlayEjected()
    {
        if (DialogHosts.ScenarioSetup.ScenarioSetupPanel.Visible)
        {
            DialogHosts.ScenarioSetup.ExitGameClick();
            MessageDialog.ResetMessageDialog(DialogHosts.MessageDialog);
            var warning = AbilityRules.TranslateResourceString(s_warningText);
            DialogHosts.MessageDialog.TitleLabel.Text = warning;
            DialogHosts.MessageDialog.MessageMemo.Items.Add("");
            var youWereEjectedByTheHost = AbilityRules.TranslateResourceString(s_youWereEjectedByHostText);
            DialogHosts.MessageDialog.MessageMemo.Items.Add(youWereEjectedByTheHost);
            DialogHosts.MessageDialog.OKPanel.Visible = true;
            DialogHosts.MessageDialog.Window.Visible = true;
        }
    }

    // aowSetupEvents.pas:184
    public void DPlayPingMessage(int pingMilliseconds, int player)
    {
        if (DialogHosts.ScenarioSetup.ScenarioSetupPanel.Visible)
        {
            DialogHosts.ScenarioSetup.PingList.Drawn = false;
        }
    }

    // aowSetupEvents.pas:189
    public void SetupControlCustomizeLeader(SetupControl control, CustomizeLeaderControl leader)
    {
        DialogHosts.ScenarioSetup.ScenarioSetupPanel.Visible = false;
        DialogHosts.LeaderSetupWindow.Execute(leader);
    }

    // aowSetupEvents.pas:195
    public void SetupControlLoadMap(SetupControl control, string fileName)
    {
        var fileNameLocal = ExtractFileName(fileName);
        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_loadingFormat), [fileNameLocal]);
        DialogHosts.ProgressDialog.TitleLabel.Text = format;
        DialogHosts.ProgressDialog.StatusLabel.Text = "";
        DialogHosts.ProgressDialog.ProgressBar.Position = 0;
        DialogHosts.ProgressDialog.ProgressBar.Max = 100;
        DialogHosts.ProgressDialog.Window.Visible = true;
        DialogHosts.General.Manager.OverlayPriority = 5000;
        if (!control.Settings.CustomizeLeaders)
        {
            AudioHost.SongPlayer.Stop(false);
            AudioHost.SongPlayer.Songs.Clear();
            AudioPlayback.FadeMusicVolume(0, 1);
        }

        if (control.Stream.Length != 0)
        {
            GameTables.GameEngine.LoadHsmFromStream(control.Stream, false, (_, message, progression) => DialogHosts.General.MapProgress(message, progression));
            var fileName2 = ExtractFileName(fileName);
            StrategicMap.Current.FileName = GameTables.GameEngine.StartupDirectory + "Scenario\\" + fileName2;
        }
        else
        {
            GameTables.GameEngine.LoadHsm(fileName, false, (_, message, progression) => DialogHosts.General.MapProgress(message, progression), GameProfile.WorldMap);
        }

        DialogHosts.General.Manager.OverlayPriority = 0;
        DialogHosts.ProgressDialog.Window.Visible = false;
        control.MapLoaded();
    }

    // aowSetupEvents.pas:220
    public bool SetupControlSendData(object data, int size, int recipient)
    {
        /* TODO: Implement aowDPlayComp.SendMessage in the platform or editor service. */
        throw new NotImplementedException();
    }

    // aowSetupEvents.pas:225
    public void SetupControlSettingChanged()
    {
        ScenarioSetup.UpdateScenarioSetup(DialogHosts.ScenarioSetup);
    }

    // aowSetupEvents.pas:228
    public void SetupControlEjectPlayer(SetupControl control, int remoteId)
    {
        /* TODO: Implement aowDPlayComp.EjectPlayer in the platform or editor service. */
        throw new NotImplementedException();
    }

    // aowSetupEvents.pas:231
    public void DPlayMessage(ref object data, ref int size, ref int toId, ref int fromId)
    {
        if (SetupControl.Status.Contains(SetupStatusElement.Busy))
        {
            SetupControl.DataReceived(data, size, fromId);
        }

        if (StrategicMap.Current != null && StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            if (StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl is RemotePlayerCommunicationControl)
            {
                ((RemotePlayerCommunicationControl)StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl).DataReceived(data, size, fromId);
            }
        }
    }

    // aowSetupEvents.pas:239
    public void SetupControlSynchronize(SetupControl control)
    {
        DialogHosts.General.Manager.Aligned = false;
        DialogHosts.ScenarioSetup.ScenarioSetupPanel.Visible = false;
        if (control.GetRemote())
        {
            DialogHosts.SynchronizeDialog.Window.Visible = true;
        }
    }

    // aowSetupEvents.pas:249
    public void SetupControlIncompatibleVersion(SetupControl control, int major, int minor)
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
        DialogHosts.ProgressDialog.Window.Visible = true;
        DialogHosts.ProgressDialog.TitleLabel.Text = "Transferring Files...";
        DialogHosts.ProgressDialog.StatusLabel.Text = "Please Wait";
        DialogHosts.ProgressDialog.ProgressBar.Position = 0;
    }

    // aowSetupEvents.pas:291
    public void DPlayMessageError()
    {
        if (StrategicMap.Current != null)
        {
            StrategicMap.Current.PlayerConnectionLost();
        }
    }

    // aowSetupEvents.pas:299
    public void DPlayDestroyPlayer(int id, int numberPlayers)
    {
        sbyte player;
        if (DialogHosts.ScenarioSetup.SetupControl != null)
        {
            if (DialogHosts.ScenarioSetup.SetupControl.GetHost())
            {
                DialogHosts.ScenarioSetup.SetupControl.DisconnectPlayer(id);
            }
        }

        if (StrategicMap.Current != null && StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl != null)
        {
            player = (sbyte)StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl.IndexOfSystemId(id);
            if (player != -1)
            {
                if (StrategicMap.Current.Players.GetPlayers(player) != null)
                {
                    if (StrategicMap.Current.Players.GetPlayers(player).Status == PlayerStatus.Playing)
                    {
                        StrategicMap.Current.PlayerDisconnected(player);
                    }

                    UpdateChatPlayers();
                }
            }
        }
    }

    private static readonly string s_allPlayersText = "All players";

    private static readonly string s_loadingFormat = "Loading: %s...";

    private static readonly string s_youWereEjectedByHostText = "You were ejected by the host.";

    private static readonly GUID s_applicationGuid = "{3205D060-4FF5-11D3-811F-0050042E6D24}";

    private static readonly string s_warningText = "Warning";

    internal static bool StartLobby(SetupEvents events)
    {
        /* TODO: Implement aowDPlayComp.CheckLobby in the platform or editor service. */
        throw new NotImplementedException();
    }

}
