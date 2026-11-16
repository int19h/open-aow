#nullable disable
// Translated from AoWSetup.pas.

using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Formats;

using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Saves;

using Aow.Game.UI.Dialogs;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

public partial class SetupControl : Component
{
    private readonly GameEngine _engine;

    public Set<SetupStatusElement> Status { get; set; }

    private SetupPhase _phase;

    public SetupSettings Settings { get; }
    public MemoryStream Stream { get; }

    private int _updateCount;

    public RemotePlayer RemotePlayer { get; }

    [Published]
    public event Event OnSettingChanged;
    [Published]
    public event LoadMapEvent OnLoadMap;
    [Published]
    public event Event OnSetupMap;
    [Published]
    public event Event OnStart;
    // Nothing in the port raises OnAbort. The bound handler is kept.
    private Event _onAbort;

    [Published]
    public event Event OnAbort { add => _onAbort += value; remove => _onAbort -= value; }

    [Published]
    public event CustomizeLeaderEvent OnCustomizeLeader;
    [Published]
    public event Event OnSynchronize;
    [Published]
    public event Event OnPlayerSetupDone;
    [Published]
    public event EjectPlayerEvent OnEjectPlayer;
    [Published]
    public event IncompatibleVersionEvent OnIncompatibleVersion;
    [Published]
    public event SendDataEvent OnSendData;
    [Published]
    public event SendMapEvent OnSendMap;
    [Published]
    public event Event OnSendingMaps;

    // AoWSetup.pas:448
    public override void Dispose()
    {
        Settings?.Dispose();
        RemotePlayer?.Dispose();
        Stream?.Dispose();
        base.Dispose();
    }

    // AoWSetup.pas:436
    public SetupControl(Component owner) : base(owner)
    {
        // The engine of the dialog module that owns the control, as the original read the engine
        // global.
        _engine = (owner as UIModule)?.UI.Engine;
        Settings = new SetupSettings(null) { Holder = _engine };
        RemotePlayer = new RemotePlayer(null) { Holder = _engine };
        RemotePlayer.RemoteId = 0;
        RemotePlayer.PreferredRace = 255;
        Stream = new MemoryStream();
    }

    // AoWSetup.pas:456
    public void SendData(object data, int size, int recipient) => OnSendData?.Invoke(data, size, recipient);

    // AoWSetup.pas:461
    public void SendPackage(SetupPackage package, int recipient)
    {
        MemoryStream stream;
        stream = new MemoryStream();
        try
        {
            _engine.StoreEngineObject(stream, package, null);
            SendData(size: (int)stream.Length, data: stream.GetBuffer(), recipient: recipient);
            int streamSize = (int)stream.Length;
            _engine.OutputDebugMessage(DebugChannel.NetworkEngine, "SetupPackage Send (ID:" + IntToStr(package.MessageId) + "/Size:" + IntToStr(streamSize) + ") to " + IntToStr(recipient));
        }
        finally
        {
            stream?.Dispose();
        }
    }

    // AoWSetup.pas:476
    public void SendMessage(int messageId, EngineObject objectValue, int param2, int param1, int recipient)
    {
        SetupPackage package;
        if (GetRemote())
        {
            package = new SetupPackage(null) { Holder = _engine };
            package.MessageId = messageId;
            if (objectValue != null)
            {
                package.Data = objectValue.Copy(_engine);
            }

            package.Param2 = param2;
            package.Param1 = param1;
            SendPackage(package, recipient);
            package?.Dispose();
        }
    }

    // AoWSetup.pas:492
    public void SendHostMessage(int messageId, EngineObject objectValue, int param2, int param1)
    {
        SetupPackage package;
        if (GetRemote())
        {
            package = new SetupPackage(null) { Holder = _engine };
            package.MessageId = messageId;
            if (objectValue != null)
            {
                package.Data = objectValue.Copy(_engine);
            }

            package.Param2 = param2;
            package.Param1 = param1;
            if (!GetHost())
            {
                SendPackage(package, 0);
            }
            else
            {
                PackageReceived(package, RemotePlayer.RemoteId);
            }

            package?.Dispose();
        }
    }

    // AoWSetup.pas:509
    public void DataReceived(object data, int size, int senderId)
    {
        EngineObject objectValue = default;
        MemoryStream stream;
        stream = new MemoryStream();
        stream.SetLength(size);
        Move(BufferBytes(data)[..size], stream.GetBuffer().AsSpan(0, size), size);
        stream.Position = 0;
        try
        {
            objectValue = _engine.RetrieveEngineObject(stream, null);
            if (objectValue is SetupPackage package)
            {
                int sizeLocal = (int)stream.Length;
                _engine.OutputDebugMessage(DebugChannel.NetworkEngine, "SetupPackage Received (ID:" + IntToStr(package.MessageId) + "/Size:" + IntToStr(sizeLocal) + ") from " + IntToStr(senderId));
                PackageReceived(package, senderId);
            }

            objectValue?.Dispose();
        }
        catch
        {
            // { Original swallows the exception and frees the stream again below. }
            stream?.Dispose();
        }

        stream?.Dispose();
    }

    // AoWSetup.pas:530
    public bool ValidateCompatibleVersion(int version)
    {
        int major;
        major = (int)(((uint)(_engine.Version & -65536)) >> 16);
        version = (int)(((uint)(version & -65536)) >> 16);
        return major == version;
    }

    // AoWSetup.pas:537
    public void ConnectPlayer(RemotePlayer player)
    {
        if (_phase == SetupPhase.SetupPlayers && (ValidateCompatibleVersion(player.Version) || RemotePlayer.RemoteId == player.RemoteId))
        {
            if (Settings.RemotePlayers.IndexOfRemoteId(player.RemoteId) == -1)
            {
                player = player.Copy<RemotePlayer>(_engine);
                player.Name = ValidatePlayerName(player.Name);
                player.MapValidation = MapValidationInfo.Unknown;
                player.SetOwner(Settings.RemotePlayers);
                UpdateSettings();
            }
        }
        else
        {
            if ((int)(((uint)player.Version) >> 24) == 1 && (((int)(((uint)player.Version) >> 16)) & 255) <= 10)
            {
                OnEjectPlayer?.Invoke(this, player.RemoteId);
            }
            else
            {
                SendMessage(param1: _engine.GetMinorVersion(), messageId: 9, objectValue: null, param2: _engine.GetMajorVersion(), recipient: player.RemoteId);
            }
        }
    }

    // AoWSetup.pas:556
    public void DisconnectPlayer(int remoteId)
    {
        int i;
        int j;
        i = Settings.RemotePlayers.IndexOfRemoteId(remoteId);
        if (i != -1)
        {
            j = Settings.IndexOfRemoteId(remoteId);
            if (j != -1)
            {
                ResetPlayerSettings(j);
            }

            Settings.RemotePlayers.GetPlayers(i)?.Dispose();
            UpdateSettings();
        }
    }

    // AoWSetup.pas:569
    public void UpdatePlayer(RemotePlayer player)
    {
        int i;
        i = Settings.RemotePlayers.IndexOfRemoteId(player.RemoteId);
        if (i != -1)
        {
            Settings.RemotePlayers.GetPlayers(i).Assign(player, _engine);
            Settings.RemotePlayers.GetPlayers(i).Name = "";
            string validatePlayerName = ValidatePlayerName(player.Name);
            Settings.RemotePlayers.GetPlayers(i).Name = validatePlayerName;
            UpdatePreferredRaces();
            UpdateSettings();
        }
    }

    // AoWSetup.pas:583
    public bool CanSetHostRemoteId(int remoteId)
    {
        bool result;
        int i;
        if (GetHost() && remoteId != Settings.HostRemoteId)
        {
            i = Settings.RemotePlayers.IndexOfRemoteId(remoteId);
            if (i != -1)
            {
                result = Settings.RemotePlayers.GetPlayers(i).MapValidation == MapValidationInfo.Valid;
            }
            else
            {
                return false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWSetup.pas:594
    public void SetHostRemoteId(int remoteId)
    {
        if (CanSetHostRemoteId(remoteId))
        {
            Settings.HostRemoteId = remoteId;
            if (GetRemote())
            {
                SendMessage(4, Settings, 0, 0, 0);
            }

            OnSettingChanged?.Invoke(this);
        }
    }

    // AoWSetup.pas:604
    public void ValidateMap()
    {
        string fileName;
        MapValidationInfo validation;
        if (!IsEmpty(Settings.MapName) && RemotePlayer.MapValidation != MapValidationInfo.Valid)
        {
            if (GamePack.Current.HssCrc == Settings.SetCrc)
            {
                // The client looks up the host's map by name, its own document first (own-formats.md 2.9).
                fileName = FileBoundary.ReadName(_engine.StartupDirectory + Settings.MapName);
                validation = FileExists(fileName)
                    ? FileBoundary.ImageCrc32(fileName, GameProfile.WorldMap) == Settings.MapCrc
                        ? MapValidationInfo.Valid
                        : MapValidationInfo.InvalidMap
                    : MapValidationInfo.InvalidMap;
            }
            else
            {
                validation = MapValidationInfo.Invalid;
            }

            bool condition = validation == MapValidationInfo.InvalidMap && Stream.Length != 0;
            if (condition)
            {
                condition = EngineServices.GetCrc32(size: (int)Stream.Length, buffer: Stream.GetBuffer()) == Settings.MapCrc;
            }

            if (condition)
            {
                validation = MapValidationInfo.Valid;
            }

            if (validation != RemotePlayer.MapValidation)
            {
                RemotePlayer.MapValidation = validation;
                SendHostMessage(3, RemotePlayer, 0, 0);
            }
        }
    }

    // AoWSetup.pas:627
    public void PackageReceived(SetupPackage package, int senderId)
    {
        int i = default;
        switch (package.MessageId)
        {
            case 1:
                // Message id 1 carries a remote player in Data.
                ConnectPlayer((RemotePlayer)package.Data);
                break;
            case 2:
                if (GetHost())
                {
                    DisconnectPlayer(package.Param2);
                }

                break;
            case 3:
                if (GetHost())
                {
                    // Message id 3 carries a remote player in Data.
                    UpdatePlayer((RemotePlayer)package.Data);
                }

                break;
            case 4:
                if (!GetHost() && _phase == SetupPhase.SetupPlayers)
                {
                    Settings.Assign(package.Data, _engine);
                    i = Settings.RemotePlayers.IndexOfRemoteId(RemotePlayer.RemoteId);
                    if (i != -1)
                    {
                        RemotePlayer playerName = Settings.RemotePlayers.GetPlayers(i);
                        RemotePlayer.Name = playerName.Name;
                        RemotePlayer playerPreferredRace = Settings.RemotePlayers.GetPlayers(i);
                        RemotePlayer.PreferredRace = playerPreferredRace.PreferredRace;
                        RemotePlayer playerMapValidation = Settings.RemotePlayers.GetPlayers(i);
                        RemotePlayer.MapValidation = playerMapValidation.MapValidation;
                        if (RemotePlayer.MapValidation == MapValidationInfo.Unknown)
                        {
                            ValidateMap();
                        }
                    }

                    OnSettingChanged?.Invoke(this);
                }

                break;
            case 5:
                if (!GetHost())
                {
                    Settings.Assign(package.Data, _engine);
                    i = Settings.RemotePlayers.IndexOfRemoteId(RemotePlayer.RemoteId);
                    if (i != -1)
                    {
                        RemotePlayer playerName = Settings.RemotePlayers.GetPlayers(i);
                        RemotePlayer.Name = playerName.Name;
                        RemotePlayer playerPreferredRace = Settings.RemotePlayers.GetPlayers(i);
                        RemotePlayer.PreferredRace = playerPreferredRace.PreferredRace;
                        RemotePlayer playerMapValidation = Settings.RemotePlayers.GetPlayers(i);
                        RemotePlayer.MapValidation = playerMapValidation.MapValidation;
                        if (RemotePlayer.MapValidation == MapValidationInfo.Unknown)
                        {
                            ValidateMap();
                        }

                        OnSettingChanged?.Invoke(this);

                        LoadMap();
                    }
                }

                break;
            case 8:
                if (GetHost())
                {
                    LeaderCustomized(package.Param2, (Leader)package.Data);
                }

                break;
            case 7:
                if (GetHost())
                {
                    MapLoaded(package.Param2);
                }

                break;
            case 6:
                if (!GetHost())
                {
                    Settings.Assign(package.Data, _engine);
                    SetupMap();
                }

                break;
            case 10:
                if (!GetHost())
                {
                    _phase = SetupPhase.SendMap;
                    OnSendingMaps?.Invoke(this);
                }

                break;
            case 11:
                if (GetHost() && ValidateSendMap())
                {
                    SendMessage(5, Settings, 0, 0, 0);
                    LoadMap();
                }

                break;
            case 9:
                if (!GetHost())
                {
                    OnIncompatibleVersion?.Invoke(this, package.Param2, package.Param1);
                }

                break;
        }
    }

    // AoWSetup.pas:683
    public void LoadMap()
    {
        _phase = SetupPhase.LoadMap;
        OnLoadMap?.Invoke(this, _engine.StartupDirectory + Settings.MapName);
    }

    // AoWSetup.pas:689
    public bool ValidateSendMap()
    {
        bool result = default;
        MemoryStream stream;
        int i;
        int remoteId;
        int j = default;
        i = Settings.Count;
        remoteId = 0;
        while (i != 0 && remoteId == 0)
        {
            i -= 1;
            if (Settings.GetSettings(i).Behavior.PlayerType == 0 && GetRemote() && Settings.GetSettings(i).Behavior.Flags != RemotePlayer.RemoteId)
            {
                PlayerSetupSettings setting = Settings.GetSettings(i);
                j = Settings.RemotePlayers.IndexOfRemoteId(setting.Behavior.Flags);
                if (j != -1 && Settings.RemotePlayers.GetPlayers(j).MapValidation == MapValidationInfo.InvalidMap)
                {
                    remoteId = Settings.GetSettings(i).Behavior.Flags;
                }
            }
        }

        if (remoteId != 0)
        {
            // The host sends the map's engine image, the bytes FMapCRC covers and LoadHSMfromStream
            // reads (own-formats.md 4.1). A document the original image cannot carry has no image.
            // LoadOrRaise refuses it through the packet profile, and this sends nothing.
            stream = GameEngine.ImageStream(FileBoundary.LoadOrRaise(_engine.StartupDirectory + Settings.MapName, GameProfile.WorldMap));
            if (OnSendMap != null)
            {
                int size = (int)stream.Length;
                OnSendMap(this, stream.GetBuffer(), size, remoteId);
            }

            stream?.Dispose();
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // AoWSetup.pas:715
    public void SendMap()
    {
        _phase = SetupPhase.SendMap;
        OnSendingMaps?.Invoke(this);

        SendMessage(10, Settings, 0, 0, 0);
        if (ValidateSendMap())
        {
            SendMessage(5, Settings, 0, 0, 0);
            LoadMap();
        }
    }

    // AoWSetup.pas:727
    public void MapReceived(object data, int size)
    {
        if (!GetHost())
        {
            Stream.SetLength(size);
            Move(BufferBytes(data)[..size], Stream.GetBuffer().AsSpan(0, size), size);
            ValidateMap();
            SendHostMessage(11, RemotePlayer, 0, 0);
        }
    }

    // AoWSetup.pas:738
    public void MapLoaded(int index)
    {
        if (GetHost())
        {
            PlayerSetupSettings setting = Settings.GetSettings(index);
            Settings.GetSettings(index).Status = setting.Status + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Loaded);
            if (!ValidateSetupMap())
            {
                UpdateSettings();
            }
        }
    }

    // AoWSetup.pas:747
    public void MapLoaded()
    {
        int i = default;
        if (Settings.MapDay != 0 || !Settings.CustomizeLeaders)
        {
            _phase = SetupPhase.Synchronize;
            OnSynchronize?.Invoke(this);
        }

        if (GetRemote())
        {
            i = Settings.IndexOfRemoteId(RemotePlayer.RemoteId);
            if (GetHost())
            {
                MapLoaded(i);
            }
            else
            {
                SendHostMessage(7, null, i, 0);
            }
        }
        else
        {
            i = 0;
            int last = Settings.Count - 1;
            for (; i <= last; i += 1)
            {
                if (Settings.GetSettings(i).Behavior.PlayerType == 0)
                {
                    PlayerSetupSettings setting = Settings.GetSettings(i);
                    Settings.GetSettings(i).Status = setting.Status + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Loaded);
                }
            }

            if (!ValidateSetupMap())
            {
                UpdateSettings();
            }
        }

        if (Settings.MapDay == 0 && Settings.CustomizeLeaders)
        {
            CustomizeLeaders();
        }
    }

    // AoWSetup.pas:768
    public void LeaderCustomized(int index, Leader leader)
    {
        if (GetHost())
        {
            Settings.GetSettings(index).Leader.Assign(leader, _engine);
            PlayerSetupSettings setting = Settings.GetSettings(index);
            Settings.GetSettings(index).Status = setting.Status + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.LeaderCustomized);
            if (!ValidateSetupMap())
            {
                UpdateSettings();
            }
        }
    }

    // AoWSetup.pas:777
    public bool CustomizeLeaders()
    {
        bool found;
        int i;
        CustomizeLeaderControl cLC = default;
        _phase = SetupPhase.CustomizeLeaders;
        bool result = false;
        found = false;
        i = 0;
        while (!found && i != Settings.Count)
        {
            found = Settings.GetSettings(i).Behavior.PlayerType == 0 && (!GetRemote() || Settings.GetSettings(i).Behavior.Flags == RemotePlayer.RemoteId) && !Settings.GetSettings(i).Status.Contains(PlayerSetupStatusElement.LeaderCustomized);
            if (!found)
            {
                i += 1;
            }
        }

        if (found)
        {
            cLC = new CustomizeLeaderControl(null) { Holder = _engine };
            cLC.SetupControl = this;
            cLC.PlayerIndex = i;
            PlayerSetupSettings settingRace = Settings.GetSettings(i);
            cLC.Items.Add(settingRace.Race);
            PlayerSetupSettings settingLeader = Settings.GetSettings(i);
            cLC.Leader.Assign(settingLeader.Leader, _engine);
            PlayerSetupSettings setting = Settings.GetSettings(i);
            cLC.OriginalLeader.Assign(setting.Leader, _engine);
            if (GetRemote())
            {
                cLC.Leader.HeroName = RemotePlayer.Name;
                cLC.OriginalLeader.HeroName = RemotePlayer.Name;
            }

            if (!Settings.LeadersOnMap)
            {
                cLC.Mode = Set<LeaderSetupModeElement>.FromMask((cLC.Mode - Set<LeaderSetupModeElement>.Of((LeaderSetupModeElement)1)).Mask);
            }

            if (OnCustomizeLeader != null)
            {
                OnCustomizeLeader(this, cLC);
                result = true;
            }

            cLC.Release();
        }
        else
        {
            _phase = SetupPhase.Synchronize;
            OnSynchronize?.Invoke(this);
        }

        return result;
    }

    // AoWSetup.pas:817
    public void LeaderCustomized(CustomizeLeaderControl control)
    {
        if (!GetHost())
        {
            SendHostMessage(8, control.OriginalLeader, control.PlayerIndex, 0);
            Settings.GetSettings(control.PlayerIndex).Leader.Assign(control.OriginalLeader, _engine);
        }
        else
        {
            LeaderCustomized(control.PlayerIndex, control.OriginalLeader);
        }

        if (Status.Contains(SetupStatusElement.Busy))
        {
            PlayerSetupSettings setting = Settings.GetSettings(control.PlayerIndex);
            Settings.GetSettings(control.PlayerIndex).Status = setting.Status + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.LeaderCustomized);
            CustomizeLeaders();
        }
    }

    // AoWSetup.pas:830
    public void ListPlayerCommunicationControlInfo(PlayerCommunicationControl playerCommunicationControl)
    {
        string s;
        for (int i = 0, last = playerCommunicationControl.Count - 1; i <= last; i++)
        {
            PlayerCommunicationSettings player = playerCommunicationControl.Players[i];
            s = "Player (" + IntToStr(i) + ") at system:" + IntToStr(player.SystemId) + " CM:";
            switch (playerCommunicationControl.Players[i].ConnectionMode)
            {
                case ConnectionMode.Global:
                    s = s + "Global";
                    break;
                case ConnectionMode.Local:
                    s = s + "Local";
                    break;
                case ConnectionMode.Remote:
                    s = s + "Remote";
                    break;
            }

            _engine.OutputDebugMessage(DebugChannel.NetworkEngine, s);
        }
    }

    // AoWSetup.pas:844
    public void RemoveIndependentPlayers()
    {
        Player player;
        for (int i = StrategicMap.Current.Players.GetCount() - 1; i >= 1; i--)
        {
            player = StrategicMap.Current.Players.GetPlayers(i);
            if (player.PlayerType == PlayerType.Independent)
            {
                for (int j = player.Structures.GetCount() - 1; j >= 0; j--)
                {
                    player.Structures.GetStructure(j).SetPlayer(0);
                }

                for (int j = player.Armies.GetCount() - 1; j >= 0; j--)
                {
                    player.Armies.GetArmy(j).SetPlayer(0);
                }

                player.Leader.SetOwner(null);
                player.Status = PlayerStatus.Terminated;
            }
        }
    }

    // AoWSetup.pas:847
    public void SetupMap()
    {
        int index = default;
        Player playerPlayer = default;
        RemotePlayerCommunicationControl remotePlayerCommunicationControl = default;
        LocalPlayerCommunicationControl localPlayerCommunicationControl = default;
        _phase = SetupPhase.SetupMap;
        for (int i = 0, last = Settings.GetCount() - 1; i <= last; i++)
        {
            PlayerSetupSettings settingPlayerId = Settings.GetSettings(i);
            playerPlayer = StrategicMap.Current.Players.GetPlayers((sbyte)settingPlayerId.PlayerId);
            playerPlayer.PlayerType = (PlayerType)Settings.GetSettings(i).Behavior.PlayerType;
            playerPlayer.Behavior = (PlayerBehavior)Settings.GetSettings(i).BehaviorMode;
            if (Settings.ConnectionType == 2)
            {
                if (CompareText(b: _engine.Registry.GetLocalEmailAddress(), a: Settings.GetSettings(i).EmailAddress) != 0)
                {
                    PlayerSetupSettings setting = Settings.GetSettings(i);
                    playerPlayer.PbemSettings.EmailGame.EmailAddress = setting.EmailAddress;
                    playerPlayer.PbemSettings.EmailGame.Name = Settings.EmailGameName;
                }
                else
                {
                    PlayerSetupSettings setting = Settings.GetSettings(i);
                    EmailGame emailGame = _engine.EmailGames.CreateEmailGame(Settings.EmailGameName, setting.EmailAddress);
                    playerPlayer.PbemSettings.SetGame(emailGame);
                }
            }

            if (Settings.MapDay == 0)
            {
                playerPlayer.Leader.UpdateFromLibraryBegin();
                PlayerSetupSettings setting = Settings.GetSettings(i);
                playerPlayer.Leader.Assign(setting.Leader, _engine);
                playerPlayer.Leader.UpdateFromLibraryEnd();
            }

            if (GetRemote())
            {
                if (Settings.GetSettings(i).Behavior.PlayerType == 0 && (Settings.MapDay != 0 || !Settings.CustomizeLeaders))
                {
                    PlayerSetupSettings setting = Settings.GetSettings(i);
                    index = Settings.RemotePlayers.IndexOfRemoteId(setting.Behavior.Flags);
                    if (index != -1)
                    {
                        RemotePlayer player = Settings.RemotePlayers.GetPlayers(index);
                        playerPlayer.Leader.HeroName = player.Name;
                    }
                }
            }
        }

        StrategicMap.Current.ConnectionType = (PlayerConnectionType)Settings.ConnectionType;
        if (GetRemote())
        {
            remotePlayerCommunicationControl = new RemotePlayerCommunicationControl(_engine);
            remotePlayerCommunicationControl.Setup(StrategicMap.Current.Players.GetCount(), RemotePlayer.RemoteId);
            remotePlayerCommunicationControl.SetConnectionMode(0, ConnectionMode.Global);
            remotePlayerCommunicationControl.SetSystemId(0, RemotePlayer.RemoteId);
            for (int i = 0, last = Settings.GetCount() - 1; i <= last; i++)
            {
                if (Settings.GetSettings(i).Behavior.PlayerType != 0)
                {
                    remotePlayerCommunicationControl.SetConnectionMode((sbyte)Settings.GetSettings(i).PlayerId, ConnectionMode.Global);
                    remotePlayerCommunicationControl.SetSystemId((sbyte)Settings.GetSettings(i).PlayerId, RemotePlayer.RemoteId);
                }
                else
                {
                    if (Settings.GetSettings(i).Behavior.Flags == RemotePlayer.RemoteId)
                    {
                        remotePlayerCommunicationControl.SetConnectionMode((sbyte)Settings.GetSettings(i).PlayerId, ConnectionMode.Local);
                    }
                    else
                    {
                        remotePlayerCommunicationControl.SetConnectionMode((sbyte)Settings.GetSettings(i).PlayerId, ConnectionMode.Remote);
                    }

                    PlayerSetupSettings setting = Settings.GetSettings(i);
                    remotePlayerCommunicationControl.SetSystemId((sbyte)Settings.GetSettings(i).PlayerId, setting.Behavior.Flags);
                }
            }

            StrategicMap.Current.PlayerCommunicator.ActivatePlayerCommunicationControl(remotePlayerCommunicationControl);
            remotePlayerCommunicationControl.OnSendData += OnSendData;
            ListPlayerCommunicationControlInfo(remotePlayerCommunicationControl);
            remotePlayerCommunicationControl.Release();
        }
        else
        {
            localPlayerCommunicationControl = new LocalPlayerCommunicationControl(_engine);
            localPlayerCommunicationControl.Setup(StrategicMap.Current.Players.GetCount());
            localPlayerCommunicationControl.SetConnectionMode(0, ConnectionMode.Global);
            for (int i = 0, last = Settings.GetCount() - 1; i <= last; i++)
            {
                if (Settings.GetSettings(i).Behavior.PlayerType == 0)
                {
                    localPlayerCommunicationControl.SetConnectionMode((sbyte)Settings.GetSettings(i).PlayerId, ConnectionMode.Local);
                }
                else
                {
                    localPlayerCommunicationControl.SetConnectionMode((sbyte)Settings.GetSettings(i).PlayerId, ConnectionMode.Global);
                }
            }

            StrategicMap.Current.PlayerCommunicator.ActivatePlayerCommunicationControl(localPlayerCommunicationControl);
            ListPlayerCommunicationControlInfo(localPlayerCommunicationControl);
            localPlayerCommunicationControl.Release();
        }

        if (Settings.MapDay == 0)
        {
            StrategicMap.Current.InitialRandomSeed = Settings.RandomSeed;
            StrategicMap.Current.FixedLeaders = Settings.LeadersOnMap;
            StrategicMap.Current.AlliedVictory = Settings.AlliedVictory;
            StrategicMap.Current.SetExplorationEnabled(Settings.ExplorationEnabled);
            StrategicMap.Current.SetFogEnabled(true);
        }

        StrategicMap.Current.DayLimit = Settings.DayLimit;
        StrategicMap.Current.TurnTime = Settings.TurnTime;
        StrategicMap.Current.VictoryMode = Settings.TacticalBattleAvailability;
        StrategicMap.Current.LeadersFixed = Settings.CustomizeLeaders;
        StrategicMap.Current.StartingGame = Settings.AlwaysViewTacticalBattle;
        RemoveIndependentPlayers();
        StrategicMap.Current.SetupPlayerControl((PlayMode)Settings.PlayMode, (PlayerConnectionType)Settings.ConnectionType);
        if (OnSetupMap != null)
        {
            try
            {
                OnSetupMap(this);
            }
            catch
            {
            }
        }

        Reset();
        if (OnStart != null)
        {
            try
            {
                OnStart(this);
            }
            catch
            {
            }
        }
    }

    // AoWSetup.pas:858
    public bool ValidateSetupMap()
    {
        bool result;
        int i;
        if (GetHost())
        {
            result = true;
            i = Settings.Count;
            while (result && i != 0)
            {
                i -= 1;
                if (Settings.GetSettings(i).Behavior.PlayerType == 0)
                {
                    result = Settings.GetSettings(i).Status.Contains(PlayerSetupStatusElement.Loaded) && (!Settings.CustomizeLeaders || Settings.GetSettings(i).Status.Contains(PlayerSetupStatusElement.LeaderCustomized));
                }
            }

            if (result)
            {
                SendMessage(6, Settings, 0, 0, 0);
                SetupMap();
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWSetup.pas:879
    public void UpdateSettings()
    {
        if (_updateCount == 0 && GetHost())
        {
            _updateCount += 1;
            if (GetRemote())
            {
                SendMessage(4, Settings, 0, 0, 0);
            }

            OnSettingChanged?.Invoke(this);

            _updateCount -= 1;
        }
    }

    // AoWSetup.pas:889
    public void BeginUpdate() => _updateCount += 1;

    // AoWSetup.pas:893
    public void EndUpdate()
    {
        _updateCount -= 1;
        if (_updateCount == 0)
        {
            UpdateSettings();
        }
    }

    // AoWSetup.pas:898
    public bool GetRemote() => Status.Contains(SetupStatusElement.Remote);

    // AoWSetup.pas:902
    public bool GetHost() => Settings.HostRemoteId == RemotePlayer.RemoteId;

    // AoWSetup.pas:906
    public void Reset()
    {
        _phase = SetupPhase.Idle;
        Status = Set<SetupStatusElement>.Of();
        RemotePlayer.Name = "";
        RemotePlayer.MapValidation = MapValidationInfo.Unknown;
        RemotePlayer.PreferredRace = 255;
        Settings.Reset();
        Stream.Clear();
    }

    // AoWSetup.pas:916
    public void Disconnect() => SendHostMessage(2, null, RemotePlayer.RemoteId, 0);

    // AoWSetup.pas:920
    public string GetPlayerName() => RemotePlayer.Name;

    // AoWSetup.pas:924
    public void SetPlayerName(string value)
    {
        if (value != RemotePlayer.Name)
        {
            RemotePlayer.Name = value;
            SendHostMessage(3, RemotePlayer, 0, 0);
        }
    }

    // AoWSetup.pas:932
    public sbyte GetPreferredRace() => (sbyte)RemotePlayer.PreferredRace;

    // AoWSetup.pas:936
    public void SetPreferredRace(byte value)
    {
        if (value != RemotePlayer.PreferredRace)
        {
            RemotePlayer.PreferredRace = value;
            SendHostMessage(3, RemotePlayer, 0, 0);
        }
    }

    // AoWSetup.pas:1276
    public void ResetPlayerSettings(int index)
    {
        Assert(GetHost(), "", "AoWSetup.pas", 1278);
        if (!GetRemote())
        {
            if (Settings.GetSettings(index).OriginalBehavior.PlayerType != 0)
            {
                PlayerSetupSettings setting = Settings.GetSettings(index);
                Settings.GetSettings(index).Behavior = setting.OriginalBehavior;
            }
            else
            {
                Settings.GetSettings(index).Behavior = s_cpuBehavior;
            }
        }
        else
        {
            Settings.GetSettings(index).Behavior = s_freeBehavior;
        }

        PlayerSetupSettings settingStatus = Settings.GetSettings(index);
        Settings.GetSettings(index).Status = settingStatus.Status - Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Forced);
    }

    // AoWSetup.pas:1289
    public bool UpdatePreferredRaces()
    {
        int target;
        int old;
        int race;
        bool again;
        Assert(GetHost(), "", "AoWSetup.pas", 1292);
        again = true;
        bool result = false;
        while (again)
        {
            again = false;
            for (int i = 0, last = Settings.RemotePlayers.Count - 1; i <= last; i++)
            {
                race = (sbyte)Settings.RemotePlayers.GetPlayers(i).PreferredRace;
                if (race != -1)
                {
                    target = Settings.IndexOfRace((byte)race);
                    if (target != -1 && Settings.GetSettings(target).Behavior.Flags == 0 && (Settings.GetSettings(target).Status & Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Fixed, PlayerSetupStatusElement.Forced)) == Set<PlayerSetupStatusElement>.Of())
                    {
                        RemotePlayer playerRemoteId = Settings.RemotePlayers.GetPlayers(i);
                        old = Settings.IndexOfRemoteId(playerRemoteId.RemoteId);
                        if (old == -1 || !Settings.GetSettings(old).Status.Contains(PlayerSetupStatusElement.Forced))
                        {
                            if (old != -1)
                            {
                                ResetPlayerSettings(old);
                                again = true;
                            }

                            PlayerSetupSettings targetSettings = Settings.GetSettings(target);
                            RemotePlayer player = Settings.RemotePlayers.GetPlayers(i);
                            targetSettings.Behavior = targetSettings.Behavior with { PlayerType = 0, Flags = player.RemoteId };
                            result = true;
                        }
                    }
                }
                else
                {
                    RemotePlayer player = Settings.RemotePlayers.GetPlayers(i);
                    old = Settings.IndexOfRemoteId(player.RemoteId);
                    if (old != -1 && !Settings.GetSettings(old).Status.Contains(PlayerSetupStatusElement.Forced))
                    {
                        ResetPlayerSettings(old);
                        again = true;
                    }
                }
            }
        }

        return result;
    }

    // AoWSetup.pas:944
    public void ListAvailablePlayers(StringCollection list)
    {
        if (GetRemote())
        {
            list.Add(_engine.TranslateResourceString(GameTables.PlayerTypeTexts[5]));
            for (int i = 0, last = Settings.RemotePlayers.Count - 1; i <= last; i++)
            {
                list.Add(Settings.RemotePlayers.GetPlayers(i).Name);
            }

            list.Add(_engine.TranslateResourceString(GameTables.PlayerTypeTexts[1]));
            list.Add(_engine.TranslateResourceString(GameTables.PlayerTypeTexts[2]));
            list.Add(_engine.TranslateResourceString(GameTables.PlayerTypeTexts[3]));
            list.Add(_engine.TranslateResourceString(GameTables.PlayerTypeTexts[6]));
            list.Add(_engine.TranslateResourceString(GameTables.PlayerTypeTexts[7]));
            list.Add(_engine.TranslateResourceString(GameTables.PlayerTypeTexts[4]));
        }
        else
        {
            list.Add(_engine.TranslateResourceString(GameTables.PlayerTypeTexts[0]));
            list.Add(_engine.TranslateResourceString(GameTables.PlayerTypeTexts[1]));
            list.Add(_engine.TranslateResourceString(GameTables.PlayerTypeTexts[2]));
            list.Add(_engine.TranslateResourceString(GameTables.PlayerTypeTexts[3]));
            list.Add(_engine.TranslateResourceString(GameTables.PlayerTypeTexts[6]));
            list.Add(_engine.TranslateResourceString(GameTables.PlayerTypeTexts[7]));
            list.Add(_engine.TranslateResourceString(GameTables.PlayerTypeTexts[4]));
        }
    }

    // AoWSetup.pas:970
    public string ValidatePlayerName(string name)
    {
        bool done;
        StringList list;
        int i;
        bool unique;
        list = new StringList();
        ListAvailablePlayers(list);
        done = false;
        string result = name;
        while (!done)
        {
            i = 0;
            unique = true;
            while (i != list.Count && unique)
            {
                unique = CompareText(result, list[i]) != 0;
                i += 1;
            }

            if (!unique)
            {
                // { Original reads the last character without guarding an empty name. }
                result = Set<byte>.WithBounds(0, 255).WithRange(49, 56).Contains(Cp1252.Byte(result[result.Length - 1]))
                    ? Copy(result, 1, result.Length - 1) + IntToStr(Cp1252.Byte(result[result.Length - 1]) + 1 - 48)
                    : result + "1";
            }
            else
            {
                done = true;
            }
        }

        list?.Dispose();
        return result;
    }

    // AoWSetup.pas:995
    public bool PlayerNameToPlayerId(string name, ref SetupPlayerBehavior behavior)
    {
        int i;
        byte playerType;
        bool result = false;
        i = Settings.RemotePlayers.Count;
        while (!result && i != 0)
        {
            i -= 1;
            if (CompareText(Settings.RemotePlayers.GetPlayers(i).Name, name) == 0)
            {
                result = true;
                RemotePlayer player = Settings.RemotePlayers.GetPlayers(i);
                behavior = behavior with { PlayerType = 0, Flags = player.RemoteId };
            }
        }

        if (!result)
        {
            playerType = 0;
            while (!result && playerType <= 7)
            {
                if (CompareText(name, _engine.TranslateResourceString(GameTables.PlayerTypeTexts[playerType])) == 0)
                {
                    result = true;
                    behavior = behavior with { PlayerType = playerType, Flags = 0 };
                }
                else
                {
                    playerType = (byte)(playerType + 1);
                }
            }
        }

        if (!result)
        {
            behavior = behavior with { PlayerType = 5, Flags = 0 };
        }

        return result;
    }

    // AoWSetup.pas:1027
    public bool GetPlayerSetupValid()
    {
        int i;
        int j;
        int playerCount;
        bool human;
        human = false;
        playerCount = 0;
        bool result = Settings.Count != 0;
        i = Settings.Count;
        while (result && i != 0)
        {
            i -= 1;
            result = Settings.GetSettings(i).Behavior.PlayerType != 5;
            PlayerSetupSettings settingBehavior = Settings.GetSettings(i);
            if (Set<byte>.WithBounds(0, 255).With(6).With(7).WithRange(0, 3).Contains(settingBehavior.Behavior.PlayerType))
            {
                playerCount += 1;
            }

            human = human || Settings.GetSettings(i).Behavior.PlayerType == 0;
            if (result && Settings.GetSettings(i).Behavior.PlayerType == 0 && GetRemote())
            {
                PlayerSetupSettings setting = Settings.GetSettings(i);
                j = Settings.RemotePlayers.IndexOfRemoteId(setting.Behavior.Flags);
                bool condition = j != -1;
                if (condition)
                {
                    RemotePlayer player = Settings.RemotePlayers.GetPlayers(j);
                    condition = Set<MapValidationInfo>.Of(MapValidationInfo.InvalidMap, MapValidationInfo.Valid).Contains(player.MapValidation);
                }

                result = condition;
            }
        }

        return result && human && playerCount > 1;
    }

    // AoWSetup.pas:1048
    public bool GetAllMapsValid()
    {
        int i;
        int j;
        i = Settings.Count;
        bool result = true;
        while (result && i != 0)
        {
            i -= 1;
            result = true;
            if (result && Settings.GetSettings(i).Behavior.PlayerType == 0 && GetRemote())
            {
                PlayerSetupSettings setting = Settings.GetSettings(i);
                j = Settings.RemotePlayers.IndexOfRemoteId(setting.Behavior.Flags);
                if (j != -1)
                {
                    result = Settings.RemotePlayers.GetPlayers(j).MapValidation == MapValidationInfo.Valid;
                }
            }
        }

        return result;
    }

    // AoWSetup.pas:1064
    public bool PlayerSetupDone()
    {
        bool result = default;
        if (GetHost() && GetPlayerSetupValid())
        {
            OnPlayerSetupDone?.Invoke(this);

            if (GetRemote())
            {
                for (int i = Settings.RemotePlayers.Count - 1; i >= 0; i--)
                {
                    RemotePlayer playerRemoteId = Settings.RemotePlayers.GetPlayers(i);
                    if (Settings.IndexOfRemoteId(playerRemoteId.RemoteId) == -1)
                    {
                        if (OnEjectPlayer != null)
                        {
                            RemotePlayer player = Settings.RemotePlayers.GetPlayers(i);
                            OnEjectPlayer(this, player.RemoteId);
                        }

                        Settings.RemotePlayers.GetPlayers(i)?.Dispose();
                    }
                }
            }

            if (GetAllMapsValid())
            {
                SendMessage(5, Settings, 0, 0, 0);
                LoadMap();
            }
            else
            {
                SendMap();
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWSetup.pas:1085
    public void SetPlayerBehavior(int index, byte value)
    {
        if (GetHost() && value != Settings.GetSettings(index).BehaviorMode)
        {
            Settings.GetSettings(index).BehaviorMode = value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1093
    public void SetExplorationEnabled(bool value)
    {
        if (GetHost() && value != Settings.ExplorationEnabled && Settings.MapDay == 0)
        {
            Settings.ExplorationEnabled = value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1101
    public void SetLeadersOnMap(bool value)
    {
        if (GetHost() && value != Settings.LeadersOnMap && Settings.MapDay == 0)
        {
            Settings.LeadersOnMap = value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1109
    public void SetTacticalBattleAvailability(byte value)
    {
        if (GetHost() && value != Settings.TacticalBattleAvailability)
        {
            Settings.TacticalBattleAvailability = value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1117
    public void SetAlwaysViewTacticalBattle(bool value)
    {
        if (GetHost() && value != Settings.AlwaysViewTacticalBattle)
        {
            Settings.AlwaysViewTacticalBattle = value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1125
    public void SetAlliedVictory(bool value)
    {
        if (GetHost() && value != Settings.AlliedVictory && Settings.MapDay == 0)
        {
            Settings.AlliedVictory = value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1133
    public void SetCustomizeLeaders(bool value)
    {
        if (GetHost() && value != Settings.CustomizeLeaders && Settings.MapDay == 0)
        {
            Settings.CustomizeLeaders = value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1141
    public void SetTurnLimitation(int value)
    {
        if (GetHost() && value != Settings.DayLimit)
        {
            Settings.DayLimit = value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1149
    public void SetTurnTimeLimitation(int value)
    {
        if (GetHost() && value != Settings.TurnTime)
        {
            Settings.TurnTime = value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1157
    public void SetPlayerId(int index, in SetupPlayerBehavior behavior, bool forced)
    {
        int i;
        int playerIndex;
        if (GetHost())
        {
            playerIndex = index;
            if (Settings.GetSettings(playerIndex).Behavior.PlayerType == behavior.PlayerType && Settings.GetSettings(playerIndex).Behavior.Flags == behavior.Flags)
            {
                return;
            }

            if (behavior.Flags != 0 && Settings.RemotePlayers.IndexOfRemoteId(behavior.Flags) == -1)
            {
                return;
            }

            if (!GetRemote() && Settings.PlayMode == 2 && behavior.PlayerType == 0)
            {
                i = 0;
                int last = Settings.Count - 1;
                for (; i <= last; i += 1)
                {
                    if (Settings.GetSettings(i).Behavior.PlayerType == 0)
                    {
                        ResetPlayerSettings(i);
                    }
                }
            }

            if (GetRemote() && behavior.PlayerType == 0 && behavior.Flags != 0)
            {
                i = Settings.IndexOfRemoteId(behavior.Flags);
                if (i != -1)
                {
                    ResetPlayerSettings(i);
                }
            }

            ResetPlayerSettings(playerIndex);
            Settings.GetSettings(playerIndex).Behavior = behavior;
            if (forced && behavior.PlayerType != 5)
            {
                PlayerSetupSettings setting = Settings.GetSettings(playerIndex);
                Settings.GetSettings(playerIndex).Status = setting.Status + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Forced);
            }
            else
            {
                PlayerSetupSettings setting = Settings.GetSettings(playerIndex);
                Settings.GetSettings(playerIndex).Status = setting.Status - Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Forced);
            }

            UpdatePreferredRaces();
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1182
    public void SetPlayMode(byte value)
    {
        bool first;
        if (GetHost() && value != Settings.PlayMode)
        {
            Settings.PlayMode = value;
            if (!GetRemote() && value == 2)
            {
                first = true;
                for (int i = 0, last = Settings.Count - 1; i <= last; i++)
                {
                    if (Settings.GetSettings(i).Behavior.PlayerType == 0)
                    {
                        if (!first)
                        {
                            ResetPlayerSettings(i);
                        }

                        first = false;
                    }
                }
            }

            UpdateSettings();
        }
    }

    // AoWSetup.pas:1201
    public void SetIndependentAIMode(byte value)
    {
        if (GetHost() && value != Settings.IndependentAIMode)
        {
            Settings.IndependentAIMode = value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1209
    public bool PickMap(string fileName)
    {
        bool result = default;
        bool firstHuman = default;
        PlayerSetupSettings settings = default;
        StrategicMap preview;
        preview = (StrategicMap)_engine.CreatePreviewHsm(_engine.StartupDirectory + fileName, GameProfile.WorldMap);
        if (GetHost() && preview != null && (!_engine.Demo || preview.ShowAllPlayers) && preview.ConnectionType != PlayerConnectionType.Email)
        {
            _engine.InitializeHexSpriteSet(_engine.StartupDirectory + preview.HssFileName, null);
            Randomize();
            Settings.Clear();
            Settings.MapName = fileName;
            Settings.MapDay = preview.Day;
            Settings.FileName = preview.MapName;
            // The map CRC covers the engine image, so an own copy and its original agree (own-formats.md 4.1).
            // A document with no image uses the canonical document bytes, as ImageCrc32 states.
            int fileCrc = FileBoundary.ImageCrc32(_engine.StartupDirectory + fileName, GameProfile.WorldMap);
            Settings.MapCrc = fileCrc;
            Settings.SetCrc = GamePack.Current.HssCrc;
            Settings.RandomSeed = RandSeed;
            Settings.VictoryCondition = (byte)(int)preview.VictoryCondition;
            if (preview.Day != 0)
            {
                Settings.LeadersOnMap = preview.FixedLeaders;
                Settings.AlliedVictory = preview.AlliedVictory;
                Settings.TurnTime = preview.TurnTime;
                Settings.DayLimit = preview.DayLimit;
                Settings.AlwaysViewTacticalBattle = preview.StartingGame;
                Settings.TacticalBattleAvailability = preview.VictoryMode;
            }

            firstHuman = true;
            RemotePlayer.PreferredRace = 255;
            RemotePlayer.MapValidation = MapValidationInfo.Valid;
            for (int i = 0, last = Settings.RemotePlayers.GetCount() - 1; i <= last; i++)
            {
                Settings.RemotePlayers.GetPlayers(i).PreferredRace = 255;
                Settings.RemotePlayers.GetPlayers(i).MapValidation = Settings.RemotePlayers.GetPlayers(i).RemoteId == RemotePlayer.RemoteId
                    ? MapValidationInfo.Valid
                    : MapValidationInfo.Unknown;
            }

            for (int i = 1, last = preview.Players.GetCount() - 1; i <= last; i++)
            {
                Player player = preview.Players.GetPlayers(i);
                if (player.Status != PlayerStatus.Terminated)
                {
                    settings = new PlayerSetupSettings(Settings);
                    settings.PlayerId = (byte)i;
                    settings.Race = player.PlayerRace;
                    settings.Leader.Assign(player.LeaderSetup, _engine);
                    settings.BehaviorMode = (byte)(int)player.Behavior;
                    if (player.Participating)
                    {
                        settings.Status = settings.Status + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Fixed);
                    }

                    if (GetRemote() && !player.Participating)
                    {
                        settings.Behavior = settings.Behavior with { PlayerType = 5 };
                        settings.OriginalBehavior = settings.OriginalBehavior with { PlayerType = 5 };
                    }
                    else
                    {
                        settings.Behavior = settings.Behavior with { PlayerType = (byte)(int)player.PlayerType };
                        settings.OriginalBehavior = settings.OriginalBehavior with { PlayerType = (byte)(int)player.PlayerType };
                        if (player.PlayerType == PlayerType.Human)
                        {
                            if (!firstHuman)
                            {
                                settings.Behavior = settings.Behavior with { PlayerType = 1 };
                            }

                            firstHuman = false;
                        }
                    }
                }
            }

            UpdateSettings();
            preview?.Dispose();
            result = true;
        }
        else
        {
            // { Original leaks a rejected preview map. }
            result = false;
        }

        return result;
    }

    // AoWSetup.pas:1216
    public void EjectPlayer(int remoteId)
    {
        int i;
        int j = default;
        if (GetHost() && GetRemote() && remoteId != 0)
        {
            i = Settings.RemotePlayers.IndexOfRemoteId(remoteId);
            if (i != -1)
            {
                Settings.RemotePlayers.GetPlayers(i)?.Dispose();
                OnEjectPlayer?.Invoke(this, remoteId);

                j = Settings.IndexOfRemoteId(remoteId);
                if (j != -1)
                {
                    ResetPlayerSettings(j);
                    UpdatePreferredRaces();
                }

                UpdateSettings();
            }
        }
    }

    // AoWSetup.pas:1236
    public bool SetupHost(string name, byte mode, int remoteId)
    {
        Reset();
        _phase = SetupPhase.SetupPlayers;
        Settings.HostRemoteId = remoteId;
        Settings.ConnectionType = mode;
        switch (mode)
        {
            case 2:
                Settings.PlayMode = 1;
                break;
            default:
                Settings.PlayMode = 2;
                break;
        }

        string validatePlayerName = ValidatePlayerName(name);
        RemotePlayer.Name = validatePlayerName;
        RemotePlayer.RemoteId = remoteId;
        RemotePlayer.PreferredRace = 255;
        RemotePlayer.Version = _engine.Version;

        Status = Status + Set<SetupStatusElement>.Of(SetupStatusElement.Busy);
        if (remoteId != 0)
        {
            ConnectPlayer(RemotePlayer);
            Status = Status + Set<SetupStatusElement>.Of(SetupStatusElement.Remote);
        }

        return true;
    }

    // AoWSetup.pas:1264
    public bool SetupUser(string name, int remoteId)
    {
        Reset();
        RemotePlayer.Name = name;
        RemotePlayer.RemoteId = remoteId;
        RemotePlayer.PreferredRace = 255;
        RemotePlayer.Version = _engine.Version;
        Status = Status + Set<SetupStatusElement>.Of(SetupStatusElement.Busy, SetupStatusElement.Remote);
        SendHostMessage(1, RemotePlayer, 0, 0);
        _phase = SetupPhase.SetupPlayers;
        return true;
    }

    private static readonly SetupPlayerBehavior s_cpuBehavior = new(PlayerType: 1, Flags: 0);

    private static readonly SetupPlayerBehavior s_freeBehavior = new(PlayerType: 5, Flags: 0);

    public enum SetupPhase : byte
    {
        Idle = 0, SetupPlayers = 1, CustomizeLeaders = 2, SendMap = 3, LoadMap = 4, Synchronize = 5, SetupMap = 6
    }

    public delegate void Event(SetupControl SetupControl);
    public delegate void CustomizeLeaderEvent(SetupControl SetupControl, CustomizeLeaderControl CLC);
    public delegate void LoadMapEvent(SetupControl SetupControl, string Filename);
    public delegate void EjectPlayerEvent(SetupControl SetupControl, int RemoteID);
    public delegate void IncompatibleVersionEvent(SetupControl SetupControl, int MajorVersion, int MinorVersion);
    public delegate void SendMapEvent(SetupControl SetupControl, object Data, int Size, int Recipient);
}
