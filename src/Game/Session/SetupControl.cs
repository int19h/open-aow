// Translated from AoWSetup.pas.

using Aow.Engine.Nodes;

using Aow.Formats;

using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Saves;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

public partial class SetupControl : Component
{
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
        Settings = new SetupSettings(null);
        RemotePlayer = new RemotePlayer(null);
        RemotePlayer.RemoteId = 0;
        RemotePlayer.PreferredRace = 255;
        Stream = new MemoryStream();
    }

    // AoWSetup.pas:456
    public void SendData(object data, int size, int recipient)
    {
        if (OnSendData != null)
        {
            OnSendData(data, size, recipient);
        }
    }

    // AoWSetup.pas:461
    public void SendPackage(SetupPackage package, int recipient)
    {
        MemoryStream stream;
        stream = new MemoryStream();
        try
        {
            GameTables.GameEngine.StoreEngineObject(stream, package, null);
            var size = (int)stream.Length;
            SendData(stream.GetBuffer(), size, recipient);
            var streamSize = (int)stream.Length;
            GameTables.GameEngine.OutputDebugMessage(NetworkSession.NetworkEngineDebugChannel, "SetupPackage Send (ID:" + IntToStr(package.MessageId) + "/Size:" + IntToStr(streamSize) + ") to " + IntToStr(recipient));
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
            package = new SetupPackage(null);
            package.MessageId = messageId;
            if (objectValue != null)
            {
                package.Data = objectValue.Copy(GameTables.GameEngine);
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
            package = new SetupPackage(null);
            package.MessageId = messageId;
            if (objectValue != null)
            {
                package.Data = objectValue.Copy(GameTables.GameEngine);
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
            objectValue = GameTables.GameEngine.RetrieveEngineObject(stream, null);
            if (objectValue is SetupPackage)
            {
                var sizeLocal = (int)stream.Length;
                GameTables.GameEngine.OutputDebugMessage(NetworkSession.NetworkEngineDebugChannel, "SetupPackage Received (ID:" + IntToStr(((SetupPackage)objectValue).MessageId) + "/Size:" + IntToStr(sizeLocal) + ") from " + IntToStr(senderId));
                PackageReceived((SetupPackage)objectValue, senderId);
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
        major = (int)(((uint)(GameTables.GameEngine.Version & -65536)) >> 16);
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
                player = (RemotePlayer)player.Copy(GameTables.GameEngine);
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
                if (OnEjectPlayer != null)
                {
                    OnEjectPlayer(this, player.RemoteId);
                }
            }
            else
            {
                var minorVersion = GameTables.GameEngine.GetMinorVersion();
                SendMessage(9, null, GameTables.GameEngine.GetMajorVersion(), minorVersion, player.RemoteId);
            }
        }
    }

    // AoWSetup.pas:556
    public void DisconnectPlayer(int remoteId)
    {
        int I;
        int J;
        I = Settings.RemotePlayers.IndexOfRemoteId(remoteId);
        if (I != -1)
        {
            J = Settings.IndexOfRemoteId(remoteId);
            if (J != -1)
            {
                ResetPlayerSettings(J);
            }

            Settings.RemotePlayers.GetPlayers(I)?.Dispose();
            UpdateSettings();
        }
    }

    // AoWSetup.pas:569
    public void UpdatePlayer(RemotePlayer player)
    {
        int I;
        I = Settings.RemotePlayers.IndexOfRemoteId(player.RemoteId);
        if (I != -1)
        {
            Settings.RemotePlayers.GetPlayers(I).Assign(player, GameTables.GameEngine);
            Settings.RemotePlayers.GetPlayers(I).Name = "";
            var validatePlayerName = ValidatePlayerName(player.Name);
            Settings.RemotePlayers.GetPlayers(I).Name = validatePlayerName;
            UpdatePreferredRaces();
            UpdateSettings();
        }
    }

    // AoWSetup.pas:583
    public bool CanSetHostRemoteId(int remoteId)
    {
        bool result;
        int I;
        if (GetHost() && remoteId != Settings.HostRemoteId)
        {
            I = Settings.RemotePlayers.IndexOfRemoteId(remoteId);
            if (I != -1)
            {
                result = Settings.RemotePlayers.GetPlayers(I).MapValidation == MapValidationInfo.Valid;
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

            if (OnSettingChanged != null)
            {
                OnSettingChanged(this);
            }
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
                fileName = FileBoundary.ReadName(GameTables.GameEngine.StartupDirectory + Settings.MapName);
                if (FileExists(fileName))
                {
                    if (FileBoundary.ImageCrc32(fileName, GameProfile.WorldMap) == Settings.MapCrc)
                    {
                        validation = MapValidationInfo.Valid;
                    }
                    else
                    {
                        validation = MapValidationInfo.InvalidMap;
                    }
                }
                else
                {
                    validation = MapValidationInfo.InvalidMap;
                }
            }
            else
            {
                validation = MapValidationInfo.Invalid;
            }

            bool condition = validation == MapValidationInfo.InvalidMap && Stream.Length != 0;
            if (condition)
            {
                var size = (int)Stream.Length;
                condition = EngineServices.GetCrc32(Stream.GetBuffer(), size) == Settings.MapCrc;
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
        int I = default;
        switch (package.MessageId)
        {
            case 1:
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
                    UpdatePlayer((RemotePlayer)package.Data);
                }

                break;
            case 4:
                if (!GetHost() && _phase == SetupPhase.SetupPlayers)
                {
                    Settings.Assign(package.Data, GameTables.GameEngine);
                    I = Settings.RemotePlayers.IndexOfRemoteId(RemotePlayer.RemoteId);
                    if (I != -1)
                    {
                        var playerName = Settings.RemotePlayers.GetPlayers(I);
                        RemotePlayer.Name = playerName.Name;
                        var playerPreferredRace = Settings.RemotePlayers.GetPlayers(I);
                        RemotePlayer.PreferredRace = playerPreferredRace.PreferredRace;
                        var playerMapValidation = Settings.RemotePlayers.GetPlayers(I);
                        RemotePlayer.MapValidation = playerMapValidation.MapValidation;
                        if (RemotePlayer.MapValidation == MapValidationInfo.Unknown)
                        {
                            ValidateMap();
                        }
                    }

                    if (OnSettingChanged != null)
                    {
                        OnSettingChanged(this);
                    }
                }

                break;
            case 5:
                if (!GetHost())
                {
                    Settings.Assign(package.Data, GameTables.GameEngine);
                    I = Settings.RemotePlayers.IndexOfRemoteId(RemotePlayer.RemoteId);
                    if (I != -1)
                    {
                        var playerName = Settings.RemotePlayers.GetPlayers(I);
                        RemotePlayer.Name = playerName.Name;
                        var playerPreferredRace = Settings.RemotePlayers.GetPlayers(I);
                        RemotePlayer.PreferredRace = playerPreferredRace.PreferredRace;
                        var playerMapValidation = Settings.RemotePlayers.GetPlayers(I);
                        RemotePlayer.MapValidation = playerMapValidation.MapValidation;
                        if (RemotePlayer.MapValidation == MapValidationInfo.Unknown)
                        {
                            ValidateMap();
                        }

                        if (OnSettingChanged != null)
                        {
                            OnSettingChanged(this);
                        }

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
                    Settings.Assign(package.Data, GameTables.GameEngine);
                    SetupMap();
                }

                break;
            case 10:
                if (!GetHost())
                {
                    _phase = SetupPhase.SendMap;
                    if (OnSendingMaps != null)
                    {
                        OnSendingMaps(this);
                    }
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
                    if (OnIncompatibleVersion != null)
                    {
                        OnIncompatibleVersion(this, package.Param2, package.Param1);
                    }
                }

                break;
        }
    }

    // AoWSetup.pas:683
    public void LoadMap()
    {
        _phase = SetupPhase.LoadMap;
        if (OnLoadMap != null)
        {
            OnLoadMap(this, GameTables.GameEngine.StartupDirectory + Settings.MapName);
        }
    }

    // AoWSetup.pas:689
    public bool ValidateSendMap()
    {
        bool result = default;
        MemoryStream stream;
        int I;
        int remoteId;
        int J = default;
        I = Settings.Count;
        remoteId = 0;
        while (I != 0 && remoteId == 0)
        {
            I -= 1;
            if (Settings.GetSettings(I).Behavior.PlayerType == 0 && GetRemote() && Settings.GetSettings(I).Behavior.Flags != RemotePlayer.RemoteId)
            {
                var setting = Settings.GetSettings(I);
                J = Settings.RemotePlayers.IndexOfRemoteId(setting.Behavior.Flags);
                if (J != -1 && Settings.RemotePlayers.GetPlayers(J).MapValidation == MapValidationInfo.InvalidMap)
                {
                    remoteId = Settings.GetSettings(I).Behavior.Flags;
                }
            }
        }

        if (remoteId != 0)
        {
            // The host sends the map's engine image, the bytes FMapCRC covers and LoadHSMfromStream
            // reads, for an own document as for an original (own-formats.md 4.1).
            stream = GameEngine.ImageStream(FileBoundary.LoadOrRaise(GameTables.GameEngine.StartupDirectory + Settings.MapName, GameProfile.WorldMap));
            if (OnSendMap != null)
            {
                var size = (int)stream.Length;
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
        if (OnSendingMaps != null)
        {
            OnSendingMaps(this);
        }

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
            var setting = Settings.GetSettings(index);
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
        int I = default;
        if (Settings.MapDay != 0 || !Settings.CustomizeLeaders)
        {
            _phase = SetupPhase.Synchronize;
            if (OnSynchronize != null)
            {
                OnSynchronize(this);
            }
        }

        if (GetRemote())
        {
            I = Settings.IndexOfRemoteId(RemotePlayer.RemoteId);
            if (GetHost())
            {
                MapLoaded(I);
            }
            else
            {
                SendHostMessage(7, null, I, 0);
            }
        }
        else
        {
            I = 0;
            int last = Settings.Count - 1;
            for (; I <= last; I += 1)
            {
                if (Settings.GetSettings(I).Behavior.PlayerType == 0)
                {
                    var setting = Settings.GetSettings(I);
                    Settings.GetSettings(I).Status = setting.Status + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Loaded);
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
            Settings.GetSettings(index).Leader.Assign(leader, GameTables.GameEngine);
            var setting = Settings.GetSettings(index);
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
        int I;
        CustomizeLeaderControl cLC = default;
        _phase = SetupPhase.CustomizeLeaders;
        bool result = false;
        found = false;
        I = 0;
        while (!found && I != Settings.Count)
        {
            found = Settings.GetSettings(I).Behavior.PlayerType == 0 && (!GetRemote() || Settings.GetSettings(I).Behavior.Flags == RemotePlayer.RemoteId) && !Settings.GetSettings(I).Status.Contains(PlayerSetupStatusElement.LeaderCustomized);
            if (!found)
            {
                I += 1;
            }
        }

        if (found)
        {
            cLC = new CustomizeLeaderControl(null);
            cLC.SetupControl = this;
            cLC.PlayerIndex = I;
            var settingRace = Settings.GetSettings(I);
            cLC.Items.Add(settingRace.Race);
            var settingLeader = Settings.GetSettings(I);
            cLC.Leader.Assign(settingLeader.Leader, GameTables.GameEngine);
            var setting = Settings.GetSettings(I);
            cLC.OriginalLeader.Assign(setting.Leader, GameTables.GameEngine);
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
            if (OnSynchronize != null)
            {
                OnSynchronize(this);
            }
        }

        return result;
    }

    // AoWSetup.pas:817
    public void LeaderCustomized(CustomizeLeaderControl control)
    {
        if (!GetHost())
        {
            SendHostMessage(8, control.OriginalLeader, control.PlayerIndex, 0);
            Settings.GetSettings(control.PlayerIndex).Leader.Assign(control.OriginalLeader, GameTables.GameEngine);
        }
        else
        {
            LeaderCustomized(control.PlayerIndex, control.OriginalLeader);
        }

        if (Status.Contains(SetupStatusElement.Busy))
        {
            var setting = Settings.GetSettings(control.PlayerIndex);
            Settings.GetSettings(control.PlayerIndex).Status = setting.Status + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.LeaderCustomized);
            CustomizeLeaders();
        }
    }

    // AoWSetup.pas:830
    public void ListPlayerCommunicationControlInfo(PlayerCommunicationControl playerCommunicationControl)
    {
        string S;
        for (int I = 0, last = playerCommunicationControl.Count - 1; I <= last; I++)
        {
            var player = playerCommunicationControl.Players[I];
            S = "Player (" + IntToStr(I) + ") at system:" + IntToStr(player.SystemId) + " CM:";
            switch (playerCommunicationControl.Players[I].ConnectionMode)
            {
                case ConnectionMode.Global:
                    S = S + "Global";
                    break;
                case ConnectionMode.Local:
                    S = S + "Local";
                    break;
                case ConnectionMode.Remote:
                    S = S + "Remote";
                    break;
            }

            GameTables.GameEngine.OutputDebugMessage(NetworkSession.NetworkEngineDebugChannel, S);
        }
    }

    // AoWSetup.pas:844
    public void RemoveIndependentPlayers()
    {
        Player player;
        for (int I = StrategicMap.Current.Players.GetCount() - 1; I >= 1; I--)
        {
            player = StrategicMap.Current.Players.GetPlayers(I);
            if (player.PlayerType == PlayerType.Independent)
            {
                for (int J = player.Structures.GetCount() - 1; J >= 0; J--)
                {
                    player.Structures.GetStructure(J).SetPlayer(0);
                }

                for (int J = player.Armies.GetCount() - 1; J >= 0; J--)
                {
                    player.Armies.GetArmy(J).SetPlayer(0);
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
        Player Player = default;
        RemotePlayerCommunicationControl remotePlayerCommunicationControl = default;
        LocalPlayerCommunicationControl localPlayerCommunicationControl = default;
        _phase = SetupPhase.SetupMap;
        for (int I = 0, last = Settings.GetCount() - 1; I <= last; I++)
        {
            var settingPlayerId = Settings.GetSettings(I);
            Player = StrategicMap.Current.Players.GetPlayers((sbyte)settingPlayerId.PlayerId);
            Player.PlayerType = (PlayerType)Settings.GetSettings(I).Behavior.PlayerType;
            Player.Behavior = (PlayerBehavior)Settings.GetSettings(I).BehaviorMode;
            if (Settings.ConnectionType == 2)
            {
                var localEmailAddress = GameTables.GameEngine.Registry.GetLocalEmailAddress();
                if (CompareText(Settings.GetSettings(I).EmailAddress, localEmailAddress) != 0)
                {
                    var setting = Settings.GetSettings(I);
                    Player.PbemSettings.EmailGame.EmailAddress = setting.EmailAddress;
                    Player.PbemSettings.EmailGame.Name = Settings.EmailGameName;
                }
                else
                {
                    var setting = Settings.GetSettings(I);
                    var emailGame = GameTables.GameEngine.EmailGames.CreateEmailGame(Settings.EmailGameName, setting.EmailAddress);
                    Player.PbemSettings.SetGame(emailGame);
                }
            }

            if (Settings.MapDay == 0)
            {
                Player.Leader.UpdateFromLibraryBegin();
                var setting = Settings.GetSettings(I);
                Player.Leader.Assign(setting.Leader, GameTables.GameEngine);
                Player.Leader.UpdateFromLibraryEnd();
            }

            if (GetRemote())
            {
                if (Settings.GetSettings(I).Behavior.PlayerType == 0 && (Settings.MapDay != 0 || !Settings.CustomizeLeaders))
                {
                    var setting = Settings.GetSettings(I);
                    index = Settings.RemotePlayers.IndexOfRemoteId(setting.Behavior.Flags);
                    if (index != -1)
                    {
                        var player = Settings.RemotePlayers.GetPlayers(index);
                        Player.Leader.HeroName = player.Name;
                    }
                }
            }
        }

        StrategicMap.Current.ConnectionType = (PlayerConnectionType)Settings.ConnectionType;
        if (GetRemote())
        {
            remotePlayerCommunicationControl = new RemotePlayerCommunicationControl();
            remotePlayerCommunicationControl.Setup(StrategicMap.Current.Players.GetCount(), RemotePlayer.RemoteId);
            remotePlayerCommunicationControl.SetConnectionMode(0, ConnectionMode.Global);
            remotePlayerCommunicationControl.SetSystemId(0, RemotePlayer.RemoteId);
            for (int I = 0, last = Settings.GetCount() - 1; I <= last; I++)
            {
                if (Settings.GetSettings(I).Behavior.PlayerType != 0)
                {
                    remotePlayerCommunicationControl.SetConnectionMode((sbyte)Settings.GetSettings(I).PlayerId, ConnectionMode.Global);
                    remotePlayerCommunicationControl.SetSystemId((sbyte)Settings.GetSettings(I).PlayerId, RemotePlayer.RemoteId);
                }
                else
                {
                    if (Settings.GetSettings(I).Behavior.Flags == RemotePlayer.RemoteId)
                    {
                        remotePlayerCommunicationControl.SetConnectionMode((sbyte)Settings.GetSettings(I).PlayerId, ConnectionMode.Local);
                    }
                    else
                    {
                        remotePlayerCommunicationControl.SetConnectionMode((sbyte)Settings.GetSettings(I).PlayerId, ConnectionMode.Remote);
                    }

                    var setting = Settings.GetSettings(I);
                    remotePlayerCommunicationControl.SetSystemId((sbyte)Settings.GetSettings(I).PlayerId, setting.Behavior.Flags);
                }
            }

            StrategicMap.Current.PlayerCommunicator.ActivatePlayerCommunicationControl(remotePlayerCommunicationControl);
            remotePlayerCommunicationControl.OnSendData += OnSendData;
            ListPlayerCommunicationControlInfo(remotePlayerCommunicationControl);
            remotePlayerCommunicationControl.Release();
        }
        else
        {
            localPlayerCommunicationControl = new LocalPlayerCommunicationControl();
            localPlayerCommunicationControl.Setup(StrategicMap.Current.Players.GetCount());
            localPlayerCommunicationControl.SetConnectionMode(0, ConnectionMode.Global);
            for (int I = 0, last = Settings.GetCount() - 1; I <= last; I++)
            {
                if (Settings.GetSettings(I).Behavior.PlayerType == 0)
                {
                    localPlayerCommunicationControl.SetConnectionMode((sbyte)Settings.GetSettings(I).PlayerId, ConnectionMode.Local);
                }
                else
                {
                    localPlayerCommunicationControl.SetConnectionMode((sbyte)Settings.GetSettings(I).PlayerId, ConnectionMode.Global);
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
        int I;
        if (GetHost())
        {
            result = true;
            I = Settings.Count;
            while (result && I != 0)
            {
                I -= 1;
                if (Settings.GetSettings(I).Behavior.PlayerType == 0)
                {
                    result = Settings.GetSettings(I).Status.Contains(PlayerSetupStatusElement.Loaded) && (!Settings.CustomizeLeaders || Settings.GetSettings(I).Status.Contains(PlayerSetupStatusElement.LeaderCustomized));
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

            if (OnSettingChanged != null)
            {
                OnSettingChanged(this);
            }

            _updateCount -= 1;
        }
    }

    // AoWSetup.pas:889
    public void BeginUpdate()
    {
        _updateCount += 1;
    }

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
    public void Disconnect()
    {
        SendHostMessage(2, null, RemotePlayer.RemoteId, 0);
    }

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
                var setting = Settings.GetSettings(index);
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

        var settingStatus = Settings.GetSettings(index);
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
            for (int I = 0, last = Settings.RemotePlayers.Count - 1; I <= last; I++)
            {
                race = (sbyte)Settings.RemotePlayers.GetPlayers(I).PreferredRace;
                if (race != -1)
                {
                    target = Settings.IndexOfRace((byte)race);
                    if (target != -1 && Settings.GetSettings(target).Behavior.Flags == 0 && (Settings.GetSettings(target).Status & Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Fixed, PlayerSetupStatusElement.Forced)) == Set<PlayerSetupStatusElement>.Of())
                    {
                        var playerRemoteId = Settings.RemotePlayers.GetPlayers(I);
                        old = Settings.IndexOfRemoteId(playerRemoteId.RemoteId);
                        if (old == -1 || !Settings.GetSettings(old).Status.Contains(PlayerSetupStatusElement.Forced))
                        {
                            if (old != -1)
                            {
                                ResetPlayerSettings(old);
                                again = true;
                            }

                            Settings.GetSettings(target).Behavior.PlayerType = 0;
                            var player = Settings.RemotePlayers.GetPlayers(I);
                            Settings.GetSettings(target).Behavior.Flags = player.RemoteId;
                            result = true;
                        }
                    }
                }
                else
                {
                    var player = Settings.RemotePlayers.GetPlayers(I);
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
            list.Add(AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[5]));
            for (int I = 0, last = Settings.RemotePlayers.Count - 1; I <= last; I++)
            {
                list.Add(Settings.RemotePlayers.GetPlayers(I).Name);
            }

            list.Add(AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[1]));
            list.Add(AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[2]));
            list.Add(AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[3]));
            list.Add(AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[6]));
            list.Add(AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[7]));
            list.Add(AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[4]));
        }
        else
        {
            list.Add(AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[0]));
            list.Add(AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[1]));
            list.Add(AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[2]));
            list.Add(AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[3]));
            list.Add(AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[6]));
            list.Add(AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[7]));
            list.Add(AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[4]));
        }
    }

    // AoWSetup.pas:970
    public string ValidatePlayerName(string name)
    {
        bool done;
        StringList list;
        int I;
        bool unique;
        list = new StringList();
        ListAvailablePlayers(list);
        done = false;
        string result = name;
        while (!done)
        {
            I = 0;
            unique = true;
            while (I != list.Count && unique)
            {
                unique = CompareText(result, list[I]) != 0;
                I += 1;
            }

            if (!unique)
            {
                // { Original reads the last character without guarding an empty name. }
                if (Set<byte>.WithBounds(0, 255).WithRange(49, 56).Contains(Cp1252.Byte(result[result.Length - 1])))
                {
                    result = Copy(result, 1, result.Length - 1) + IntToStr(Cp1252.Byte(result[result.Length - 1]) + 1 - 48);
                }
                else
                {
                    result = result + "1";
                }
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
        int I;
        byte playerType;
        bool result = false;
        I = Settings.RemotePlayers.Count;
        while (!result && I != 0)
        {
            I -= 1;
            if (CompareText(Settings.RemotePlayers.GetPlayers(I).Name, name) == 0)
            {
                result = true;
                behavior.PlayerType = 0;
                var player = Settings.RemotePlayers.GetPlayers(I);
                behavior.Flags = player.RemoteId;
            }
        }

        if (!result)
        {
            playerType = 0;
            while (!result && playerType <= 7)
            {
                if (CompareText(name, AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[playerType])) == 0)
                {
                    result = true;
                    behavior.PlayerType = playerType;
                    behavior.Flags = 0;
                }
                else
                {
                    playerType = (byte)(playerType + 1);
                }
            }
        }

        if (!result)
        {
            behavior.PlayerType = 5;
            behavior.Flags = 0;
        }

        return result;
    }

    // AoWSetup.pas:1027
    public bool GetPlayerSetupValid()
    {
        int I;
        int J;
        int playerCount;
        bool human;
        human = false;
        playerCount = 0;
        bool result = Settings.Count != 0;
        I = Settings.Count;
        while (result && I != 0)
        {
            I -= 1;
            result = Settings.GetSettings(I).Behavior.PlayerType != 5;
            var settingBehavior = Settings.GetSettings(I);
            if (Set<byte>.WithBounds(0, 255).With(6).With(7).WithRange(0, 3).Contains(settingBehavior.Behavior.PlayerType))
            {
                playerCount += 1;
            }

            human = human || Settings.GetSettings(I).Behavior.PlayerType == 0;
            if (result && Settings.GetSettings(I).Behavior.PlayerType == 0 && GetRemote())
            {
                var setting = Settings.GetSettings(I);
                J = Settings.RemotePlayers.IndexOfRemoteId(setting.Behavior.Flags);
                bool condition = J != -1;
                if (condition)
                {
                    var player = Settings.RemotePlayers.GetPlayers(J);
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
        int I;
        int J;
        I = Settings.Count;
        bool result = true;
        while (result && I != 0)
        {
            I -= 1;
            result = true;
            if (result && Settings.GetSettings(I).Behavior.PlayerType == 0 && GetRemote())
            {
                var setting = Settings.GetSettings(I);
                J = Settings.RemotePlayers.IndexOfRemoteId(setting.Behavior.Flags);
                if (J != -1)
                {
                    result = Settings.RemotePlayers.GetPlayers(J).MapValidation == MapValidationInfo.Valid;
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
            if (OnPlayerSetupDone != null)
            {
                OnPlayerSetupDone(this);
            }

            if (GetRemote())
            {
                for (int I = Settings.RemotePlayers.Count - 1; I >= 0; I--)
                {
                    var playerRemoteId = Settings.RemotePlayers.GetPlayers(I);
                    if (Settings.IndexOfRemoteId(playerRemoteId.RemoteId) == -1)
                    {
                        if (OnEjectPlayer != null)
                        {
                            var player = Settings.RemotePlayers.GetPlayers(I);
                            OnEjectPlayer(this, player.RemoteId);
                        }

                        Settings.RemotePlayers.GetPlayers(I)?.Dispose();
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
        int I;
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
                I = 0;
                int last = Settings.Count - 1;
                for (; I <= last; I += 1)
                {
                    if (Settings.GetSettings(I).Behavior.PlayerType == 0)
                    {
                        ResetPlayerSettings(I);
                    }
                }
            }

            if (GetRemote() && behavior.PlayerType == 0 && behavior.Flags != 0)
            {
                I = Settings.IndexOfRemoteId(behavior.Flags);
                if (I != -1)
                {
                    ResetPlayerSettings(I);
                }
            }

            ResetPlayerSettings(playerIndex);
            Settings.GetSettings(playerIndex).Behavior = behavior;
            if (forced && behavior.PlayerType != 5)
            {
                var setting = Settings.GetSettings(playerIndex);
                Settings.GetSettings(playerIndex).Status = setting.Status + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Forced);
            }
            else
            {
                var setting = Settings.GetSettings(playerIndex);
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
                for (int I = 0, last = Settings.Count - 1; I <= last; I++)
                {
                    if (Settings.GetSettings(I).Behavior.PlayerType == 0)
                    {
                        if (!first)
                        {
                            ResetPlayerSettings(I);
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
        preview = (StrategicMap)GameTables.GameEngine.CreatePreviewHsm(GameTables.GameEngine.StartupDirectory + fileName, GameProfile.WorldMap);
        if (GetHost() && preview != null && (!GameTables.GameEngine.Demo || preview.ShowAllPlayers) && preview.ConnectionType != PlayerConnectionType.Email)
        {
            GameTables.GameEngine.InitializeHexSpriteSet(GameTables.GameEngine.StartupDirectory + preview.HssFileName, null);
            Randomize();
            Settings.Clear();
            Settings.MapName = fileName;
            Settings.MapDay = preview.Day;
            Settings.FileName = preview.MapName;
            // The map CRC is taken over the engine image, so an own copy and its original agree (own-formats.md 4.1).
            var fileCrc = FileBoundary.ImageCrc32(GameTables.GameEngine.StartupDirectory + fileName, GameProfile.WorldMap);
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
            for (int I = 0, last = Settings.RemotePlayers.GetCount() - 1; I <= last; I++)
            {
                Settings.RemotePlayers.GetPlayers(I).PreferredRace = 255;
                if (Settings.RemotePlayers.GetPlayers(I).RemoteId == RemotePlayer.RemoteId)
                {
                    Settings.RemotePlayers.GetPlayers(I).MapValidation = MapValidationInfo.Valid;
                }
                else
                {
                    Settings.RemotePlayers.GetPlayers(I).MapValidation = MapValidationInfo.Unknown;
                }
            }

            for (int I = 1, last = preview.Players.GetCount() - 1; I <= last; I++)
            {
                var player = preview.Players.GetPlayers(I);
                if (player.Status != PlayerStatus.Terminated)
                {
                    settings = new PlayerSetupSettings(this.Settings);
                    settings.PlayerId = (byte)I;
                    settings.Race = player.PlayerRace;
                    settings.Leader.Assign(player.LeaderSetup, GameTables.GameEngine);
                    settings.BehaviorMode = (byte)(int)player.Behavior;
                    if (player.Participating)
                    {
                        settings.Status = settings.Status + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Fixed);
                    }

                    if (GetRemote() && !player.Participating)
                    {
                        settings.Behavior.PlayerType = 5;
                        settings.OriginalBehavior.PlayerType = 5;
                    }
                    else
                    {
                        settings.Behavior.PlayerType = (byte)(int)player.PlayerType;
                        settings.OriginalBehavior.PlayerType = (byte)(int)player.PlayerType;
                        if (player.PlayerType == PlayerType.Human)
                        {
                            if (!firstHuman)
                            {
                                settings.Behavior.PlayerType = 1;
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
        int I;
        int J = default;
        if (GetHost() && GetRemote() && remoteId != 0)
        {
            I = Settings.RemotePlayers.IndexOfRemoteId(remoteId);
            if (I != -1)
            {
                Settings.RemotePlayers.GetPlayers(I)?.Dispose();
                if (OnEjectPlayer != null)
                {
                    OnEjectPlayer(this, remoteId);
                }

                J = Settings.IndexOfRemoteId(remoteId);
                if (J != -1)
                {
                    ResetPlayerSettings(J);
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
        this.Settings.HostRemoteId = remoteId;
        this.Settings.ConnectionType = mode;
        switch (mode)
        {
            case 2:
                this.Settings.PlayMode = 1;
                break;
            default:
                this.Settings.PlayMode = 2;
                break;
        }

        var validatePlayerName = ValidatePlayerName(name);
        RemotePlayer.Name = validatePlayerName;
        this.RemotePlayer.RemoteId = remoteId;
        this.RemotePlayer.PreferredRace = 255;
        this.RemotePlayer.Version = GameTables.GameEngine.Version;

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
        RemotePlayer.Version = GameTables.GameEngine.Version;
        Status = Status + Set<SetupStatusElement>.Of(SetupStatusElement.Busy, SetupStatusElement.Remote);
        SendHostMessage(1, RemotePlayer, 0, 0);
        _phase = SetupPhase.SetupPlayers;
        return true;
    }

    private static SetupPlayerBehavior s_cpuBehavior = new SetupPlayerBehavior
    {
        PlayerType = 1,
        Reserved = default,
        Flags = 0
    };

    private static SetupPlayerBehavior s_freeBehavior = new SetupPlayerBehavior
    {
        PlayerType = 5,
        Reserved = default,
        Flags = 0
    };

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
