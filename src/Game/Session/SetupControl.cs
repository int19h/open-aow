// Translated from AoWSetup.pas.
using Aow.Engine.Buffers;

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
    public Set<SetupStatusElement> FStatus;
    public SetupPhase FPhase;
    public SetupSettings FSettings;
    public MemoryStream FStream;
    public int FUpdateCount;
    public RemotePlayer FRemotePlayer;
    public Event FOnSettingChanged;
    public LoadMapEvent FOnLoadMap;
    public Event FOnSetupMap;
    public Event FOnStart;
    public Event FOnAbort;
    public CustomizeLeaderEvent FOnCustomizeLeader;
    public Event FOnSynchronize;
    public Event FOnPlayerSetupDone;
    public EjectPlayerEvent FOnEjectPlayer;
    public IncompatibleVersionEvent FOnIncompatibleVersion;
    public SendDataEvent FOnSendData;
    public SendMapEvent FOnSendMap;
    public Event FOnSendingMaps;

    // AoWSetup.pas:448
    public override void Dispose()
    {
        FSettings?.Dispose();
        FRemotePlayer?.Dispose();
        FStream?.Dispose();
        base.Dispose();
    }

    // AoWSetup.pas:436
    public SetupControl(Component owner) : base(owner)
    {
        FSettings = new SetupSettings(null);
        FRemotePlayer = new RemotePlayer(null);
        FRemotePlayer.FRemoteID = 0;
        FRemotePlayer.FPreferredRace = 255;
        FStream = new MemoryStream();
    }

    // AoWSetup.pas:456
    public void SendData(object Data, int Size, int Recipient)
    {
        if (FOnSendData != null)
        {
            FOnSendData(Data, Size, Recipient);
        }
    }

    // AoWSetup.pas:461
    public void SendPackage(SetupPackage Package, int Recipient)
    {
        MemoryStream Stream;
        Stream = new MemoryStream();
        try
        {
            GameTables.AoWEngine.StoreEObject(Stream, Package, null);
            var size = (int)Stream.Length;
            SendData(Stream.GetBuffer(), size, Recipient);
            var streamSize = (int)Stream.Length;
            GameTables.AoWEngine.OutputDebugMessage(NetworkSession.NetworkEngineDID, "SetupPackage Send (ID:" + IntToStr(Package.FMessageID) + "/Size:" + IntToStr(streamSize) + ") to " + IntToStr(Recipient));
        }
        finally
        {
            Stream?.Dispose();
        }
    }

    // AoWSetup.pas:476
    public void SendMessage(int MessageID, EngineObject Obj, int Param2, int Param1, int Recipient)
    {
        SetupPackage Package;
        if (GetRemote())
        {
            Package = new SetupPackage(null);
            Package.FMessageID = MessageID;
            if (Obj != null)
            {
                Package.FData = Obj.Copy(GameTables.AoWEngine);
            }

            Package.FParam2 = Param2;
            Package.FParam1 = Param1;
            SendPackage(Package, Recipient);
            Package?.Dispose();
        }
    }

    // AoWSetup.pas:492
    public void SendHostMessage(int MessageID, EngineObject Obj, int Param2, int Param1)
    {
        SetupPackage Package;
        if (GetRemote())
        {
            Package = new SetupPackage(null);
            Package.FMessageID = MessageID;
            if (Obj != null)
            {
                Package.FData = Obj.Copy(GameTables.AoWEngine);
            }

            Package.FParam2 = Param2;
            Package.FParam1 = Param1;
            if (!GetHost())
            {
                SendPackage(Package, 0);
            }
            else
            {
                PackageReceived(Package, FRemotePlayer.FRemoteID);
            }

            Package?.Dispose();
        }
    }

    // AoWSetup.pas:509
    public void DataReceived(object Data, int Size, int SenderID)
    {
        EngineObject Obj = default;
        MemoryStream Stream;
        Stream = new MemoryStream();
        Stream.SetLength(Size);
        Move(BufferBytes(Data)[..Size], Stream.GetBuffer().AsSpan(0, Size), Size);
        Stream.Position = 0;
        try
        {
            Obj = GameTables.AoWEngine.RetrieveEObject(Stream, null);
            if (Obj is SetupPackage)
            {
                var size = (int)Stream.Length;
                GameTables.AoWEngine.OutputDebugMessage(NetworkSession.NetworkEngineDID, "SetupPackage Received (ID:" + IntToStr(((SetupPackage)Obj).FMessageID) + "/Size:" + IntToStr(size) + ") from " + IntToStr(SenderID));
                PackageReceived((SetupPackage)Obj, SenderID);
            }

            Obj?.Dispose();
        }
        catch
        {
            // { Original swallows the exception and frees the stream again below. }
            Stream?.Dispose();
        }

        Stream?.Dispose();
    }

    // AoWSetup.pas:530
    public bool ValidateCompatibleVersion(int Version)
    {
        int Major;
        Major = (int)(((uint)(GameTables.AoWEngine.FVersion & -65536)) >> 16);
        Version = (int)(((uint)(Version & -65536)) >> 16);
        return Major == Version;
    }

    // AoWSetup.pas:537
    public void hConnectPlayer(RemotePlayer Player)
    {
        if (FPhase == SetupPhase.SetupPlayers && (ValidateCompatibleVersion(Player.FVersion) || FRemotePlayer.FRemoteID == Player.FRemoteID))
        {
            if (FSettings.FRemotePlayers.IndexOfRemoteID(Player.FRemoteID) == -1)
            {
                Player = (RemotePlayer)Player.Copy(GameTables.AoWEngine);
                Player.FName = ValidatePlayerName(Player.FName);
                Player.FMapValidation = MapValidationInfo.Unknown;
                Player.SetOwner(FSettings.FRemotePlayers);
                UpdateSettings();
            }
        }
        else
        {
            if ((int)(((uint)Player.FVersion) >> 24) == 1 && (((int)(((uint)Player.FVersion) >> 16)) & 255) <= 10)
            {
                if (FOnEjectPlayer != null)
                {
                    FOnEjectPlayer(this, Player.FRemoteID);
                }
            }
            else
            {
                var minorVersion = GameTables.AoWEngine.GetMinorVersion();
                SendMessage(9, null, GameTables.AoWEngine.GetMajorVersion(), minorVersion, Player.FRemoteID);
            }
        }
    }

    // AoWSetup.pas:556
    public void hDisconnectPlayer(int RemoteID)
    {
        int I;
        int J;
        I = FSettings.FRemotePlayers.IndexOfRemoteID(RemoteID);
        if (I != -1)
        {
            J = FSettings.IndexOfRemoteID(RemoteID);
            if (J != -1)
            {
                ResetPlayerSettings(J);
            }

            FSettings.FRemotePlayers.GetPlayers(I)?.Dispose();
            UpdateSettings();
        }
    }

    // AoWSetup.pas:569
    public void hUpdatePlayer(RemotePlayer Player)
    {
        int I;
        I = FSettings.FRemotePlayers.IndexOfRemoteID(Player.FRemoteID);
        if (I != -1)
        {
            FSettings.FRemotePlayers.GetPlayers(I).Assign(Player, GameTables.AoWEngine);
            FSettings.FRemotePlayers.GetPlayers(I).FName = "";
            var validatePlayerName = ValidatePlayerName(Player.FName);
            FSettings.FRemotePlayers.GetPlayers(I).FName = validatePlayerName;
            UpdatePreferredRaces();
            UpdateSettings();
        }
    }

    // AoWSetup.pas:583
    public bool CanSetHostRemoteID(int RemoteID)
    {
        bool Result;
        int I;
        if (GetHost() && RemoteID != FSettings.FHostRemoteID)
        {
            I = FSettings.FRemotePlayers.IndexOfRemoteID(RemoteID);
            if (I != -1)
            {
                Result = FSettings.FRemotePlayers.GetPlayers(I).FMapValidation == MapValidationInfo.Valid;
            }
            else
            {
                return false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWSetup.pas:594
    public void hSetHostRemoteID(int RemoteID)
    {
        if (CanSetHostRemoteID(RemoteID))
        {
            FSettings.FHostRemoteID = RemoteID;
            if (GetRemote())
            {
                SendMessage(4, FSettings, 0, 0, 0);
            }

            if (FOnSettingChanged != null)
            {
                FOnSettingChanged(this);
            }
        }
    }

    // AoWSetup.pas:604
    public void ValidateMap()
    {
        string FileName;
        MapValidationInfo Validation;
        if (!IsEmpty(FSettings.FMapName) && FRemotePlayer.FMapValidation != MapValidationInfo.Valid)
        {
            if (AoWHSSet.Current.FHSSCRC == FSettings.FSetCRC)
            {
                // The client looks up the host's map by name, its own document first (own-formats.md 2.9).
                FileName = FileBoundary.ReadName(GameTables.AoWEngine.StartupDirectory + FSettings.FMapName);
                if (FileExists(FileName))
                {
                    if (FileBoundary.ImageCrc32(FileName, GameProfile.WorldMap) == FSettings.FMapCRC)
                    {
                        Validation = MapValidationInfo.Valid;
                    }
                    else
                    {
                        Validation = MapValidationInfo.InvalidMap;
                    }
                }
                else
                {
                    Validation = MapValidationInfo.InvalidMap;
                }
            }
            else
            {
                Validation = MapValidationInfo.Invalid;
            }

            bool condition = Validation == MapValidationInfo.InvalidMap && FStream.Length != 0;
            if (condition)
            {
                var size = (int)FStream.Length;
                condition = EngineServices.GetCRC32(FStream.GetBuffer(), size) == FSettings.FMapCRC;
            }

            if (condition)
            {
                Validation = MapValidationInfo.Valid;
            }

            if (Validation != FRemotePlayer.FMapValidation)
            {
                FRemotePlayer.FMapValidation = Validation;
                SendHostMessage(3, FRemotePlayer, 0, 0);
            }
        }
    }

    // AoWSetup.pas:627
    public void PackageReceived(SetupPackage Package, int SenderID)
    {
        int I = default;
        switch (Package.FMessageID)
        {
            case 1:
                hConnectPlayer((RemotePlayer)Package.FData);
                break;
            case 2:
                if (GetHost())
                {
                    hDisconnectPlayer(Package.FParam2);
                }

                break;
            case 3:
                if (GetHost())
                {
                    hUpdatePlayer((RemotePlayer)Package.FData);
                }

                break;
            case 4:
                if (!GetHost() && FPhase == SetupPhase.SetupPlayers)
                {
                    FSettings.Assign(Package.FData, GameTables.AoWEngine);
                    I = FSettings.FRemotePlayers.IndexOfRemoteID(FRemotePlayer.FRemoteID);
                    if (I != -1)
                    {
                        var playerName = FSettings.FRemotePlayers.GetPlayers(I);
                        FRemotePlayer.FName = playerName.FName;
                        var playerPreferredRace = FSettings.FRemotePlayers.GetPlayers(I);
                        FRemotePlayer.FPreferredRace = playerPreferredRace.FPreferredRace;
                        var playerMapValidation = FSettings.FRemotePlayers.GetPlayers(I);
                        FRemotePlayer.FMapValidation = playerMapValidation.FMapValidation;
                        if (FRemotePlayer.FMapValidation == MapValidationInfo.Unknown)
                        {
                            ValidateMap();
                        }
                    }

                    if (FOnSettingChanged != null)
                    {
                        FOnSettingChanged(this);
                    }
                }

                break;
            case 5:
                if (!GetHost())
                {
                    FSettings.Assign(Package.FData, GameTables.AoWEngine);
                    I = FSettings.FRemotePlayers.IndexOfRemoteID(FRemotePlayer.FRemoteID);
                    if (I != -1)
                    {
                        var playerName = FSettings.FRemotePlayers.GetPlayers(I);
                        FRemotePlayer.FName = playerName.FName;
                        var playerPreferredRace = FSettings.FRemotePlayers.GetPlayers(I);
                        FRemotePlayer.FPreferredRace = playerPreferredRace.FPreferredRace;
                        var playerMapValidation = FSettings.FRemotePlayers.GetPlayers(I);
                        FRemotePlayer.FMapValidation = playerMapValidation.FMapValidation;
                        if (FRemotePlayer.FMapValidation == MapValidationInfo.Unknown)
                        {
                            ValidateMap();
                        }

                        if (FOnSettingChanged != null)
                        {
                            FOnSettingChanged(this);
                        }

                        LoadMap();
                    }
                }

                break;
            case 8:
                if (GetHost())
                {
                    hLeaderCustomized(Package.FParam2, (Leader)Package.FData);
                }

                break;
            case 7:
                if (GetHost())
                {
                    hMapLoaded(Package.FParam2);
                }

                break;
            case 6:
                if (!GetHost())
                {
                    FSettings.Assign(Package.FData, GameTables.AoWEngine);
                    SetupMap();
                }

                break;
            case 10:
                if (!GetHost())
                {
                    FPhase = SetupPhase.SendMap;
                    if (FOnSendingMaps != null)
                    {
                        FOnSendingMaps(this);
                    }
                }

                break;
            case 11:
                if (GetHost() && hValidateSendMap())
                {
                    SendMessage(5, FSettings, 0, 0, 0);
                    LoadMap();
                }

                break;
            case 9:
                if (!GetHost())
                {
                    if (FOnIncompatibleVersion != null)
                    {
                        FOnIncompatibleVersion(this, Package.FParam2, Package.FParam1);
                    }
                }

                break;
        }
    }

    // AoWSetup.pas:683
    public void LoadMap()
    {
        FPhase = SetupPhase.LoadMap;
        if (FOnLoadMap != null)
        {
            FOnLoadMap(this, GameTables.AoWEngine.StartupDirectory + FSettings.FMapName);
        }
    }

    // AoWSetup.pas:689
    public bool hValidateSendMap()
    {
        bool Result = default;
        MemoryStream Stream;
        int I;
        int RemoteID;
        int J = default;
        I = FSettings.Count;
        RemoteID = 0;
        while (I != 0 && RemoteID == 0)
        {
            I -= 1;
            if (FSettings.GetSettings(I).FBehavior.PlayerType == 0 && GetRemote() && FSettings.GetSettings(I).FBehavior.Flags != FRemotePlayer.FRemoteID)
            {
                var setting = FSettings.GetSettings(I);
                J = FSettings.FRemotePlayers.IndexOfRemoteID(setting.FBehavior.Flags);
                if (J != -1 && FSettings.FRemotePlayers.GetPlayers(J).FMapValidation == MapValidationInfo.InvalidMap)
                {
                    RemoteID = FSettings.GetSettings(I).FBehavior.Flags;
                }
            }
        }

        if (RemoteID != 0)
        {
            // The host sends the map's engine image, the bytes FMapCRC covers and LoadHSMfromStream
            // reads, for an own document as for an original (own-formats.md 4.1).
            Stream = GameEngine.ImageStream(FileBoundary.LoadOrRaise(GameTables.AoWEngine.StartupDirectory + FSettings.FMapName, GameProfile.WorldMap));
            if (FOnSendMap != null)
            {
                var size = (int)Stream.Length;
                FOnSendMap(this, Stream.GetBuffer(), size, RemoteID);
            }

            Stream?.Dispose();
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // AoWSetup.pas:715
    public void hSendMap()
    {
        FPhase = SetupPhase.SendMap;
        if (FOnSendingMaps != null)
        {
            FOnSendingMaps(this);
        }

        SendMessage(10, FSettings, 0, 0, 0);
        if (hValidateSendMap())
        {
            SendMessage(5, FSettings, 0, 0, 0);
            LoadMap();
        }
    }

    // AoWSetup.pas:727
    public void MapReceived(object Data, int Size)
    {
        if (!GetHost())
        {
            FStream.SetLength(Size);
            Move(BufferBytes(Data)[..Size], FStream.GetBuffer().AsSpan(0, Size), Size);
            ValidateMap();
            SendHostMessage(11, FRemotePlayer, 0, 0);
        }
    }

    // AoWSetup.pas:738
    public void hMapLoaded(int Index)
    {
        if (GetHost())
        {
            var setting = FSettings.GetSettings(Index);
            FSettings.GetSettings(Index).FStatus = setting.FStatus + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Loaded);
            if (!hValidateSetupMap())
            {
                UpdateSettings();
            }
        }
    }

    // AoWSetup.pas:747
    public void MapLoaded()
    {
        int I = default;
        if (FSettings.FMapDay != 0 || !FSettings.FCustomizeLeaders)
        {
            FPhase = SetupPhase.Synchronize;
            if (FOnSynchronize != null)
            {
                FOnSynchronize(this);
            }
        }

        if (GetRemote())
        {
            I = FSettings.IndexOfRemoteID(FRemotePlayer.FRemoteID);
            if (GetHost())
            {
                hMapLoaded(I);
            }
            else
            {
                SendHostMessage(7, null, I, 0);
            }
        }
        else
        {
            I = 0;
            int last = FSettings.Count - 1;
            for (; I <= last; I += 1)
            {
                if (FSettings.GetSettings(I).FBehavior.PlayerType == 0)
                {
                    var setting = FSettings.GetSettings(I);
                    FSettings.GetSettings(I).FStatus = setting.FStatus + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Loaded);
                }
            }

            if (!hValidateSetupMap())
            {
                UpdateSettings();
            }
        }

        if (FSettings.FMapDay == 0 && FSettings.FCustomizeLeaders)
        {
            CustomizeLeaders();
        }
    }

    // AoWSetup.pas:768
    public void hLeaderCustomized(int Index, Leader Leader)
    {
        if (GetHost())
        {
            FSettings.GetSettings(Index).FLeader.Assign(Leader, GameTables.AoWEngine);
            var setting = FSettings.GetSettings(Index);
            FSettings.GetSettings(Index).FStatus = setting.FStatus + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.LeaderCustomized);
            if (!hValidateSetupMap())
            {
                UpdateSettings();
            }
        }
    }

    // AoWSetup.pas:777
    public bool CustomizeLeaders()
    {
        bool Found;
        int I;
        CustomizeLeaderControl CLC = default;
        FPhase = SetupPhase.CustomizeLeaders;
        bool Result = false;
        Found = false;
        I = 0;
        while (!Found && I != FSettings.Count)
        {
            Found = FSettings.GetSettings(I).FBehavior.PlayerType == 0 && (!GetRemote() || FSettings.GetSettings(I).FBehavior.Flags == FRemotePlayer.FRemoteID) && !FSettings.GetSettings(I).FStatus.Contains(PlayerSetupStatusElement.LeaderCustomized);
            if (!Found)
            {
                I += 1;
            }
        }

        if (Found)
        {
            CLC = new CustomizeLeaderControl(null);
            CLC.FSetupControl = this;
            CLC.FPlayerIndex = I;
            var settingRace = FSettings.GetSettings(I);
            CLC.FItems.Add(settingRace.FRace);
            var settingLeader = FSettings.GetSettings(I);
            CLC.FLeader.Assign(settingLeader.FLeader, GameTables.AoWEngine);
            var setting = FSettings.GetSettings(I);
            CLC.FOriginalLeader.Assign(setting.FLeader, GameTables.AoWEngine);
            if (GetRemote())
            {
                CLC.FLeader.FHeroName = FRemotePlayer.FName;
                CLC.FOriginalLeader.FHeroName = FRemotePlayer.FName;
            }

            if (!FSettings.FLeadersOnMap)
            {
                CLC.FMode = Set<LeaderSetupModeElement>.FromMask((CLC.FMode - Set<LeaderSetupModeElement>.Of((LeaderSetupModeElement)1)).Mask);
            }

            if (FOnCustomizeLeader != null)
            {
                FOnCustomizeLeader(this, CLC);
                Result = true;
            }

            CLC.Release();
        }
        else
        {
            FPhase = SetupPhase.Synchronize;
            if (FOnSynchronize != null)
            {
                FOnSynchronize(this);
            }
        }

        return Result;
    }

    // AoWSetup.pas:817
    public void LeaderCustomized(CustomizeLeaderControl Control)
    {
        if (!GetHost())
        {
            SendHostMessage(8, Control.FOriginalLeader, Control.FPlayerIndex, 0);
            FSettings.GetSettings(Control.FPlayerIndex).FLeader.Assign(Control.FOriginalLeader, GameTables.AoWEngine);
        }
        else
        {
            hLeaderCustomized(Control.FPlayerIndex, Control.FOriginalLeader);
        }

        if (FStatus.Contains(SetupStatusElement.Busy))
        {
            var setting = FSettings.GetSettings(Control.FPlayerIndex);
            FSettings.GetSettings(Control.FPlayerIndex).FStatus = setting.FStatus + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.LeaderCustomized);
            CustomizeLeaders();
        }
    }

    // AoWSetup.pas:830
    public void ListPCCInfo(PlayerCommunicationControl PCC)
    {
        string S;
        for (int I = 0, last = PCC.Count - 1; I <= last; I++)
        {
            var player = PCC.Players[I];
            S = "Player (" + IntToStr(I) + ") at system:" + IntToStr(player.SystemID) + " CM:";
            switch (PCC.Players[I].ConnectionMode)
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

            GameTables.AoWEngine.OutputDebugMessage(NetworkSession.NetworkEngineDID, S);
        }
    }

    // AoWSetup.pas:844
    public void RemoveIndependentPlayers()
    {
        Player Player;
        for (int I = AoWHSMap.Current.FPlayers.GetCount() - 1; I >= 1; I--)
        {
            Player = AoWHSMap.Current.FPlayers.GetPlayers(I);
            if (Player.FPlayerType == PlayerType.Independent)
            {
                for (int J = Player.FStructures.GetCount() - 1; J >= 0; J--)
                {
                    Player.FStructures.GetStructure(J).SetPlayer(0);
                }

                for (int J = Player.FArmies.GetCount() - 1; J >= 0; J--)
                {
                    Player.FArmies.GetArmy(J).SetPlayer(0);
                }

                Player.FLeader.SetOwner(null);
                Player.FStatus = Session.Player.Status.Terminated;
            }
        }
    }

    // AoWSetup.pas:847
    public void SetupMap()
    {
        int Index = default;
        Player Player = default;
        RemotePlayerCommunicationControl RemotePCC = default;
        LocalPlayerCommunicationControl LocalPCC = default;
        FPhase = SetupPhase.SetupMap;
        for (int I = 0, last = FSettings.GetCount() - 1; I <= last; I++)
        {
            var settingPlayerID = FSettings.GetSettings(I);
            Player = AoWHSMap.Current.FPlayers.GetPlayers((sbyte)settingPlayerID.FPlayerID);
            Player.FPlayerType = (PlayerType)FSettings.GetSettings(I).FBehavior.PlayerType;
            Player.FBehavior = (Player.Behavior)FSettings.GetSettings(I).FBehaviorMode;
            if (FSettings.FConnectionType == 2)
            {
                var localEmailAddress = GameTables.AoWEngine.FRegistry.GetLocalEmailAddress();
                if (CompareText(FSettings.GetSettings(I).FEmailAddress, localEmailAddress) != 0)
                {
                    var setting = FSettings.GetSettings(I);
                    Player.FPBEMSettings.FEmailGame.FEmailAddress = setting.FEmailAddress;
                    Player.FPBEMSettings.FEmailGame.FName = FSettings.FEmailGameName;
                }
                else
                {
                    var setting = FSettings.GetSettings(I);
                    var emailGame = GameTables.AoWEngine.FEmailGames.CreateEmailGame(FSettings.FEmailGameName, setting.FEmailAddress);
                    Player.FPBEMSettings.SetGame(emailGame);
                }
            }

            if (FSettings.FMapDay == 0)
            {
                Player.FLeader.UpdateFromLibraryBegin();
                var setting = FSettings.GetSettings(I);
                Player.FLeader.Assign(setting.FLeader, GameTables.AoWEngine);
                Player.FLeader.UpdateFromLibraryEnd();
            }

            if (GetRemote())
            {
                if (FSettings.GetSettings(I).FBehavior.PlayerType == 0 && (FSettings.FMapDay != 0 || !FSettings.FCustomizeLeaders))
                {
                    var setting = FSettings.GetSettings(I);
                    Index = FSettings.FRemotePlayers.IndexOfRemoteID(setting.FBehavior.Flags);
                    if (Index != -1)
                    {
                        var player = FSettings.FRemotePlayers.GetPlayers(Index);
                        Player.FLeader.FHeroName = player.FName;
                    }
                }
            }
        }

        AoWHSMap.Current.FConnectionType = (PlayerConnectionType)FSettings.FConnectionType;
        if (GetRemote())
        {
            RemotePCC = new RemotePlayerCommunicationControl();
            RemotePCC.Setup(AoWHSMap.Current.FPlayers.GetCount(), FRemotePlayer.FRemoteID);
            RemotePCC.SetConnectionMode(0, ConnectionMode.Global);
            RemotePCC.SetSystemID(0, FRemotePlayer.FRemoteID);
            for (int I = 0, last = FSettings.GetCount() - 1; I <= last; I++)
            {
                if (FSettings.GetSettings(I).FBehavior.PlayerType != 0)
                {
                    RemotePCC.SetConnectionMode((sbyte)FSettings.GetSettings(I).FPlayerID, ConnectionMode.Global);
                    RemotePCC.SetSystemID((sbyte)FSettings.GetSettings(I).FPlayerID, FRemotePlayer.FRemoteID);
                }
                else
                {
                    if (FSettings.GetSettings(I).FBehavior.Flags == FRemotePlayer.FRemoteID)
                    {
                        RemotePCC.SetConnectionMode((sbyte)FSettings.GetSettings(I).FPlayerID, ConnectionMode.Local);
                    }
                    else
                    {
                        RemotePCC.SetConnectionMode((sbyte)FSettings.GetSettings(I).FPlayerID, ConnectionMode.Remote);
                    }

                    var setting = FSettings.GetSettings(I);
                    RemotePCC.SetSystemID((sbyte)FSettings.GetSettings(I).FPlayerID, setting.FBehavior.Flags);
                }
            }

            AoWHSMap.Current.FPlayerCommunicator.ActivatePCC(RemotePCC);
            RemotePCC.OnSendData = FOnSendData;
            ListPCCInfo(RemotePCC);
            RemotePCC.Release();
        }
        else
        {
            LocalPCC = new LocalPlayerCommunicationControl();
            LocalPCC.Setup(AoWHSMap.Current.FPlayers.GetCount());
            LocalPCC.SetConnectionMode(0, ConnectionMode.Global);
            for (int I = 0, last = FSettings.GetCount() - 1; I <= last; I++)
            {
                if (FSettings.GetSettings(I).FBehavior.PlayerType == 0)
                {
                    LocalPCC.SetConnectionMode((sbyte)FSettings.GetSettings(I).FPlayerID, ConnectionMode.Local);
                }
                else
                {
                    LocalPCC.SetConnectionMode((sbyte)FSettings.GetSettings(I).FPlayerID, ConnectionMode.Global);
                }
            }

            AoWHSMap.Current.FPlayerCommunicator.ActivatePCC(LocalPCC);
            ListPCCInfo(LocalPCC);
            LocalPCC.Release();
        }

        if (FSettings.FMapDay == 0)
        {
            AoWHSMap.Current.FInitialRandomSeed = FSettings.FRandomSeed;
            AoWHSMap.Current.FFixedLeaders = FSettings.FLeadersOnMap;
            AoWHSMap.Current.FAlliedVictory = FSettings.FAlliedVictory;
            AoWHSMap.Current.SetExplorationEnabled(FSettings.FExplorationEnabled);
            AoWHSMap.Current.SetFogEnabled(true);
        }

        AoWHSMap.Current.FDayLimit = FSettings.FDayLimit;
        AoWHSMap.Current.FTurnTime = FSettings.FTurnTime;
        AoWHSMap.Current.FVictoryMode = FSettings.FTacticalCombatAvailability;
        AoWHSMap.Current.FLeadersFixed = FSettings.FCustomizeLeaders;
        AoWHSMap.Current.FStartingGame = FSettings.FAlwaysViewTC;
        RemoveIndependentPlayers();
        AoWHSMap.Current.SetupPlayerControl((PlayMode)FSettings.FPlayMode, (PlayerConnectionType)FSettings.FConnectionType);
        if (FOnSetupMap != null)
        {
            try
            {
                FOnSetupMap(this);
            }
            catch
            {
            }
        }

        Reset();
        if (FOnStart != null)
        {
            try
            {
                FOnStart(this);
            }
            catch
            {
            }
        }
    }

    // AoWSetup.pas:858
    public bool hValidateSetupMap()
    {
        bool Result;
        int I;
        if (GetHost())
        {
            Result = true;
            I = FSettings.Count;
            while (Result && I != 0)
            {
                I -= 1;
                if (FSettings.GetSettings(I).FBehavior.PlayerType == 0)
                {
                    Result = FSettings.GetSettings(I).FStatus.Contains(PlayerSetupStatusElement.Loaded) && (!FSettings.FCustomizeLeaders || FSettings.GetSettings(I).FStatus.Contains(PlayerSetupStatusElement.LeaderCustomized));
                }
            }

            if (Result)
            {
                SendMessage(6, FSettings, 0, 0, 0);
                SetupMap();
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWSetup.pas:879
    public void UpdateSettings()
    {
        if (FUpdateCount == 0 && GetHost())
        {
            FUpdateCount += 1;
            if (GetRemote())
            {
                SendMessage(4, FSettings, 0, 0, 0);
            }

            if (FOnSettingChanged != null)
            {
                FOnSettingChanged(this);
            }

            FUpdateCount -= 1;
        }
    }

    // AoWSetup.pas:889
    public void BeginUpdate()
    {
        FUpdateCount += 1;
    }

    // AoWSetup.pas:893
    public void EndUpdate()
    {
        FUpdateCount -= 1;
        if (FUpdateCount == 0)
        {
            UpdateSettings();
        }
    }

    // AoWSetup.pas:898
    public bool GetRemote() => FStatus.Contains(SetupStatusElement.Remote);

    // AoWSetup.pas:902
    public bool GetHost() => FSettings.FHostRemoteID == FRemotePlayer.FRemoteID;

    // AoWSetup.pas:906
    public void Reset()
    {
        FPhase = SetupPhase.Idle;
        FStatus = Set<SetupStatusElement>.Of();
        FRemotePlayer.FName = "";
        FRemotePlayer.FMapValidation = MapValidationInfo.Unknown;
        FRemotePlayer.FPreferredRace = 255;
        FSettings.Reset();
        FStream.Clear();
    }

    // AoWSetup.pas:916
    public void Disconnect()
    {
        SendHostMessage(2, null, FRemotePlayer.FRemoteID, 0);
    }

    // AoWSetup.pas:920
    public string GetPlayerName() => FRemotePlayer.FName;

    // AoWSetup.pas:924
    public void SetPlayerName(string Value)
    {
        if (Value != FRemotePlayer.FName)
        {
            FRemotePlayer.FName = Value;
            SendHostMessage(3, FRemotePlayer, 0, 0);
        }
    }

    // AoWSetup.pas:932
    public sbyte GetPreferredRace() => (sbyte)FRemotePlayer.FPreferredRace;

    // AoWSetup.pas:936
    public void SetPreferredRace(byte Value)
    {
        if (Value != FRemotePlayer.FPreferredRace)
        {
            FRemotePlayer.FPreferredRace = Value;
            SendHostMessage(3, FRemotePlayer, 0, 0);
        }
    }

    // AoWSetup.pas:1276
    public void ResetPlayerSettings(int Index)
    {
        Assert(GetHost(), "", "AoWSetup.pas", 1278);
        if (!GetRemote())
        {
            if (FSettings.GetSettings(Index).FOriginalBehavior.PlayerType != 0)
            {
                var setting = FSettings.GetSettings(Index);
                FSettings.GetSettings(Index).FBehavior = setting.FOriginalBehavior;
            }
            else
            {
                FSettings.GetSettings(Index).FBehavior = CPUBehavior;
            }
        }
        else
        {
            FSettings.GetSettings(Index).FBehavior = FreeBehavior;
        }

        var settingStatus = FSettings.GetSettings(Index);
        FSettings.GetSettings(Index).FStatus = settingStatus.FStatus - Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Forced);
    }

    // AoWSetup.pas:1289
    public bool UpdatePreferredRaces()
    {
        int Target;
        int Old;
        int Race;
        bool Again;
        Assert(GetHost(), "", "AoWSetup.pas", 1292);
        Again = true;
        bool Result = false;
        while (Again)
        {
            Again = false;
            for (int I = 0, last = FSettings.FRemotePlayers.Count - 1; I <= last; I++)
            {
                Race = (sbyte)FSettings.FRemotePlayers.GetPlayers(I).FPreferredRace;
                if (Race != -1)
                {
                    Target = FSettings.IndexOfRace((byte)Race);
                    if (Target != -1 && FSettings.GetSettings(Target).FBehavior.Flags == 0 && (FSettings.GetSettings(Target).FStatus & Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Fixed, PlayerSetupStatusElement.Forced)) == Set<PlayerSetupStatusElement>.Of())
                    {
                        var playerRemoteID = FSettings.FRemotePlayers.GetPlayers(I);
                        Old = FSettings.IndexOfRemoteID(playerRemoteID.FRemoteID);
                        if (Old == -1 || !FSettings.GetSettings(Old).FStatus.Contains(PlayerSetupStatusElement.Forced))
                        {
                            if (Old != -1)
                            {
                                ResetPlayerSettings(Old);
                                Again = true;
                            }

                            FSettings.GetSettings(Target).FBehavior.PlayerType = 0;
                            var player = FSettings.FRemotePlayers.GetPlayers(I);
                            FSettings.GetSettings(Target).FBehavior.Flags = player.FRemoteID;
                            Result = true;
                        }
                    }
                }
                else
                {
                    var player = FSettings.FRemotePlayers.GetPlayers(I);
                    Old = FSettings.IndexOfRemoteID(player.FRemoteID);
                    if (Old != -1 && !FSettings.GetSettings(Old).FStatus.Contains(PlayerSetupStatusElement.Forced))
                    {
                        ResetPlayerSettings(Old);
                        Again = true;
                    }
                }
            }
        }

        return Result;
    }

    // AoWSetup.pas:944
    public void ListAvailablePlayers(StringCollection List)
    {
        if (GetRemote())
        {
            List.Add(AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[5]));
            for (int I = 0, last = FSettings.FRemotePlayers.Count - 1; I <= last; I++)
            {
                List.Add(FSettings.FRemotePlayers.GetPlayers(I).FName);
            }

            List.Add(AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[1]));
            List.Add(AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[2]));
            List.Add(AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[3]));
            List.Add(AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[6]));
            List.Add(AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[7]));
            List.Add(AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[4]));
        }
        else
        {
            List.Add(AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[0]));
            List.Add(AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[1]));
            List.Add(AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[2]));
            List.Add(AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[3]));
            List.Add(AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[6]));
            List.Add(AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[7]));
            List.Add(AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[4]));
        }
    }

    // AoWSetup.pas:970
    public string ValidatePlayerName(string Name)
    {
        bool Done;
        StringList List;
        int I;
        bool Unique;
        List = new StringList();
        ListAvailablePlayers(List);
        Done = false;
        string Result = Name;
        while (!Done)
        {
            I = 0;
            Unique = true;
            while (I != List.Count && Unique)
            {
                Unique = CompareText(Result, List[I]) != 0;
                I += 1;
            }

            if (!Unique)
            {
                // { Original reads the last character without guarding an empty name. }
                if (Set<byte>.WithBounds(0, 255).WithRange(49, 56).Contains(Cp1252.Byte(Result[Result.Length - 1])))
                {
                    Result = Copy(Result, 1, Result.Length - 1) + IntToStr(Cp1252.Byte(Result[Result.Length - 1]) + 1 - 48);
                }
                else
                {
                    Result = Result + "1";
                }
            }
            else
            {
                Done = true;
            }
        }

        List?.Dispose();
        return Result;
    }

    // AoWSetup.pas:995
    public bool PlayerNameToPlayerID(string Name, ref SetupPlayerBehavior Behavior)
    {
        int I;
        byte PlayerType;
        bool Result = false;
        I = FSettings.FRemotePlayers.Count;
        while (!Result && I != 0)
        {
            I -= 1;
            if (CompareText(FSettings.FRemotePlayers.GetPlayers(I).FName, Name) == 0)
            {
                Result = true;
                Behavior.PlayerType = 0;
                var player = FSettings.FRemotePlayers.GetPlayers(I);
                Behavior.Flags = player.FRemoteID;
            }
        }

        if (!Result)
        {
            PlayerType = 0;
            while (!Result && PlayerType <= 7)
            {
                if (CompareText(Name, AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[PlayerType])) == 0)
                {
                    Result = true;
                    Behavior.PlayerType = PlayerType;
                    Behavior.Flags = 0;
                }
                else
                {
                    PlayerType = (byte)(PlayerType + 1);
                }
            }
        }

        if (!Result)
        {
            Behavior.PlayerType = 5;
            Behavior.Flags = 0;
        }

        return Result;
    }

    // AoWSetup.pas:1027
    public bool GetPlayerSetupValid()
    {
        int I;
        int J;
        int PlayerCount;
        bool Human;
        Human = false;
        PlayerCount = 0;
        bool Result = FSettings.Count != 0;
        I = FSettings.Count;
        while (Result && I != 0)
        {
            I -= 1;
            Result = FSettings.GetSettings(I).FBehavior.PlayerType != 5;
            var settingBehavior = FSettings.GetSettings(I);
            if (Set<byte>.WithBounds(0, 255).With(6).With(7).WithRange(0, 3).Contains(settingBehavior.FBehavior.PlayerType))
            {
                PlayerCount += 1;
            }

            Human = Human || FSettings.GetSettings(I).FBehavior.PlayerType == 0;
            if (Result && FSettings.GetSettings(I).FBehavior.PlayerType == 0 && GetRemote())
            {
                var setting = FSettings.GetSettings(I);
                J = FSettings.FRemotePlayers.IndexOfRemoteID(setting.FBehavior.Flags);
                bool condition = J != -1;
                if (condition)
                {
                    var player = FSettings.FRemotePlayers.GetPlayers(J);
                    condition = Set<MapValidationInfo>.Of(MapValidationInfo.InvalidMap, MapValidationInfo.Valid).Contains(player.FMapValidation);
                }

                Result = condition;
            }
        }

        return Result && Human && PlayerCount > 1;
    }

    // AoWSetup.pas:1048
    public bool GetAllMapsValid()
    {
        int I;
        int J;
        I = FSettings.Count;
        bool Result = true;
        while (Result && I != 0)
        {
            I -= 1;
            Result = true;
            if (Result && FSettings.GetSettings(I).FBehavior.PlayerType == 0 && GetRemote())
            {
                var setting = FSettings.GetSettings(I);
                J = FSettings.FRemotePlayers.IndexOfRemoteID(setting.FBehavior.Flags);
                if (J != -1)
                {
                    Result = FSettings.FRemotePlayers.GetPlayers(J).FMapValidation == MapValidationInfo.Valid;
                }
            }
        }

        return Result;
    }

    // AoWSetup.pas:1064
    public bool hPlayerSetupDone()
    {
        bool Result = default;
        if (GetHost() && GetPlayerSetupValid())
        {
            if (FOnPlayerSetupDone != null)
            {
                FOnPlayerSetupDone(this);
            }

            if (GetRemote())
            {
                for (int I = FSettings.FRemotePlayers.Count - 1; I >= 0; I--)
                {
                    var playerRemoteID = FSettings.FRemotePlayers.GetPlayers(I);
                    if (FSettings.IndexOfRemoteID(playerRemoteID.FRemoteID) == -1)
                    {
                        if (FOnEjectPlayer != null)
                        {
                            var player = FSettings.FRemotePlayers.GetPlayers(I);
                            FOnEjectPlayer(this, player.FRemoteID);
                        }

                        FSettings.FRemotePlayers.GetPlayers(I)?.Dispose();
                    }
                }
            }

            if (GetAllMapsValid())
            {
                SendMessage(5, FSettings, 0, 0, 0);
                LoadMap();
            }
            else
            {
                hSendMap();
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWSetup.pas:1085
    public void hSetPlayerBehavior(int Index, byte Value)
    {
        if (GetHost() && Value != FSettings.GetSettings(Index).FBehaviorMode)
        {
            FSettings.GetSettings(Index).FBehaviorMode = Value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1093
    public void hSetExplorationEnabled(bool Value)
    {
        if (GetHost() && Value != FSettings.FExplorationEnabled && FSettings.FMapDay == 0)
        {
            FSettings.FExplorationEnabled = Value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1101
    public void hSetLeadersOnMap(bool Value)
    {
        if (GetHost() && Value != FSettings.FLeadersOnMap && FSettings.FMapDay == 0)
        {
            FSettings.FLeadersOnMap = Value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1109
    public void hSetTacticalCombatAvailability(byte Value)
    {
        if (GetHost() && Value != FSettings.FTacticalCombatAvailability)
        {
            FSettings.FTacticalCombatAvailability = Value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1117
    public void hSetAlwaysViewTC(bool Value)
    {
        if (GetHost() && Value != FSettings.FAlwaysViewTC)
        {
            FSettings.FAlwaysViewTC = Value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1125
    public void hSetAlliedVictory(bool Value)
    {
        if (GetHost() && Value != FSettings.FAlliedVictory && FSettings.FMapDay == 0)
        {
            FSettings.FAlliedVictory = Value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1133
    public void hSetCustomizeLeaders(bool Value)
    {
        if (GetHost() && Value != FSettings.FCustomizeLeaders && FSettings.FMapDay == 0)
        {
            FSettings.FCustomizeLeaders = Value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1141
    public void hSetTurnLimitation(int Value)
    {
        if (GetHost() && Value != FSettings.FDayLimit)
        {
            FSettings.FDayLimit = Value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1149
    public void hSetTurnTimeLimitation(int Value)
    {
        if (GetHost() && Value != FSettings.FTurnTime)
        {
            FSettings.FTurnTime = Value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1157
    public void hSetPlayerID(int Index, in SetupPlayerBehavior Behavior, bool Forced)
    {
        int I;
        int PlayerIndex;
        if (GetHost())
        {
            PlayerIndex = Index;
            if (FSettings.GetSettings(PlayerIndex).FBehavior.PlayerType == Behavior.PlayerType && FSettings.GetSettings(PlayerIndex).FBehavior.Flags == Behavior.Flags)
            {
                return;
            }

            if (Behavior.Flags != 0 && FSettings.FRemotePlayers.IndexOfRemoteID(Behavior.Flags) == -1)
            {
                return;
            }

            if (!GetRemote() && FSettings.FPlayMode == 2 && Behavior.PlayerType == 0)
            {
                I = 0;
                int last = FSettings.Count - 1;
                for (; I <= last; I += 1)
                {
                    if (FSettings.GetSettings(I).FBehavior.PlayerType == 0)
                    {
                        ResetPlayerSettings(I);
                    }
                }
            }

            if (GetRemote() && Behavior.PlayerType == 0 && Behavior.Flags != 0)
            {
                I = FSettings.IndexOfRemoteID(Behavior.Flags);
                if (I != -1)
                {
                    ResetPlayerSettings(I);
                }
            }

            ResetPlayerSettings(PlayerIndex);
            FSettings.GetSettings(PlayerIndex).FBehavior = Behavior;
            if (Forced && Behavior.PlayerType != 5)
            {
                var setting = FSettings.GetSettings(PlayerIndex);
                FSettings.GetSettings(PlayerIndex).FStatus = setting.FStatus + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Forced);
            }
            else
            {
                var setting = FSettings.GetSettings(PlayerIndex);
                FSettings.GetSettings(PlayerIndex).FStatus = setting.FStatus - Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Forced);
            }

            UpdatePreferredRaces();
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1182
    public void hSetPlayMode(byte Value)
    {
        bool First;
        if (GetHost() && Value != FSettings.FPlayMode)
        {
            FSettings.FPlayMode = Value;
            if (!GetRemote() && Value == 2)
            {
                First = true;
                for (int I = 0, last = FSettings.Count - 1; I <= last; I++)
                {
                    if (FSettings.GetSettings(I).FBehavior.PlayerType == 0)
                    {
                        if (!First)
                        {
                            ResetPlayerSettings(I);
                        }

                        First = false;
                    }
                }
            }

            UpdateSettings();
        }
    }

    // AoWSetup.pas:1201
    public void hSetIndependentAIMode(byte Value)
    {
        if (GetHost() && Value != FSettings.FIndependentAIMode)
        {
            FSettings.FIndependentAIMode = Value;
            UpdateSettings();
        }
    }

    // AoWSetup.pas:1209
    public bool hPickMap(string FileName)
    {
        bool Result = default;
        bool FirstHuman = default;
        PlayerSetupSettings Settings = default;
        AoWHSMap Preview;
        Preview = (AoWHSMap)GameTables.AoWEngine.CreatePreviewHSM(GameTables.AoWEngine.StartupDirectory + FileName, GameProfile.WorldMap);
        if (GetHost() && Preview != null && (!GameTables.AoWEngine.FDemo || Preview.FShowAllPlayers) && Preview.FConnectionType != PlayerConnectionType.Email)
        {
            GameTables.AoWEngine.InitializeHSSet(GameTables.AoWEngine.StartupDirectory + Preview.HSSFileName, null);
            Randomize();
            FSettings.Clear();
            FSettings.FMapName = FileName;
            FSettings.FMapDay = Preview.FDay;
            FSettings.FFileName = Preview.FMapName;
            // The map CRC is taken over the engine image, so an own copy and its original agree (own-formats.md 4.1).
            var fileCRC = FileBoundary.ImageCrc32(GameTables.AoWEngine.StartupDirectory + FileName, GameProfile.WorldMap);
            FSettings.FMapCRC = fileCRC;
            FSettings.FSetCRC = AoWHSSet.Current.FHSSCRC;
            FSettings.FRandomSeed = RandSeed;
            FSettings.FVictoryCondition = (byte)(int)Preview.FVictoryCondition;
            if (Preview.FDay != 0)
            {
                FSettings.FLeadersOnMap = Preview.FFixedLeaders;
                FSettings.FAlliedVictory = Preview.FAlliedVictory;
                FSettings.FTurnTime = Preview.FTurnTime;
                FSettings.FDayLimit = Preview.FDayLimit;
                FSettings.FAlwaysViewTC = Preview.FStartingGame;
                FSettings.FTacticalCombatAvailability = Preview.FVictoryMode;
            }

            FirstHuman = true;
            FRemotePlayer.FPreferredRace = 255;
            FRemotePlayer.FMapValidation = MapValidationInfo.Valid;
            for (int I = 0, last = FSettings.FRemotePlayers.GetCount() - 1; I <= last; I++)
            {
                FSettings.FRemotePlayers.GetPlayers(I).FPreferredRace = 255;
                if (FSettings.FRemotePlayers.GetPlayers(I).FRemoteID == FRemotePlayer.FRemoteID)
                {
                    FSettings.FRemotePlayers.GetPlayers(I).FMapValidation = MapValidationInfo.Valid;
                }
                else
                {
                    FSettings.FRemotePlayers.GetPlayers(I).FMapValidation = MapValidationInfo.Unknown;
                }
            }

            for (int I = 1, last = Preview.FPlayers.GetCount() - 1; I <= last; I++)
            {
                var player = Preview.FPlayers.GetPlayers(I);
                if (player.FStatus != Player.Status.Terminated)
                {
                    Settings = new PlayerSetupSettings(this.FSettings);
                    Settings.FPlayerID = (byte)I;
                    Settings.FRace = player.FPlayerRace;
                    Settings.FLeader.Assign(player.FLeaderSetup, GameTables.AoWEngine);
                    Settings.FBehaviorMode = (byte)(int)player.FBehavior;
                    if (player.FParticipating)
                    {
                        Settings.FStatus = Settings.FStatus + Set<PlayerSetupStatusElement>.Of(PlayerSetupStatusElement.Fixed);
                    }

                    if (GetRemote() && !player.FParticipating)
                    {
                        Settings.FBehavior.PlayerType = 5;
                        Settings.FOriginalBehavior.PlayerType = 5;
                    }
                    else
                    {
                        Settings.FBehavior.PlayerType = (byte)(int)player.FPlayerType;
                        Settings.FOriginalBehavior.PlayerType = (byte)(int)player.FPlayerType;
                        if (player.FPlayerType == PlayerType.Human)
                        {
                            if (!FirstHuman)
                            {
                                Settings.FBehavior.PlayerType = 1;
                            }

                            FirstHuman = false;
                        }
                    }
                }
            }

            UpdateSettings();
            Preview?.Dispose();
            Result = true;
        }
        else
        {
            // { Original leaks a rejected preview map. }
            Result = false;
        }

        return Result;
    }

    // AoWSetup.pas:1216
    public void hEjectPlayer(int RemoteID)
    {
        int I;
        int J = default;
        if (GetHost() && GetRemote() && RemoteID != 0)
        {
            I = FSettings.FRemotePlayers.IndexOfRemoteID(RemoteID);
            if (I != -1)
            {
                FSettings.FRemotePlayers.GetPlayers(I)?.Dispose();
                if (FOnEjectPlayer != null)
                {
                    FOnEjectPlayer(this, RemoteID);
                }

                J = FSettings.IndexOfRemoteID(RemoteID);
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
    public bool SetupHost(string Name, byte Mode, int RemoteID)
    {
        Reset();
        FPhase = SetupPhase.SetupPlayers;
        this.FSettings.FHostRemoteID = RemoteID;
        this.FSettings.FConnectionType = Mode;
        switch (Mode)
        {
            case 2:
                this.FSettings.FPlayMode = 1;
                break;
            default:
                this.FSettings.FPlayMode = 2;
                break;
        }

        var validatePlayerName = ValidatePlayerName(Name);
        FRemotePlayer.FName = validatePlayerName;
        this.FRemotePlayer.FRemoteID = RemoteID;
        this.FRemotePlayer.FPreferredRace = 255;
        this.FRemotePlayer.FVersion = GameTables.AoWEngine.FVersion;

        FStatus = FStatus + Set<SetupStatusElement>.Of(SetupStatusElement.Busy);
        if (RemoteID != 0)
        {
            hConnectPlayer(FRemotePlayer);
            FStatus = FStatus + Set<SetupStatusElement>.Of(SetupStatusElement.Remote);
        }

        return true;
    }

    // AoWSetup.pas:1264
    public bool SetupUser(string Name, int RemoteID)
    {
        Reset();
        FRemotePlayer.FName = Name;
        FRemotePlayer.FRemoteID = RemoteID;
        FRemotePlayer.FPreferredRace = 255;
        FRemotePlayer.FVersion = GameTables.AoWEngine.FVersion;
        FStatus = FStatus + Set<SetupStatusElement>.Of(SetupStatusElement.Busy, SetupStatusElement.Remote);
        SendHostMessage(1, FRemotePlayer, 0, 0);
        FPhase = SetupPhase.SetupPlayers;
        return true;
    }

    [Published]
    public SendMapEvent OnSendMap { get => FOnSendMap; set => FOnSendMap = value; }

    [Published]
    public SendDataEvent OnSendData { get => FOnSendData; set => FOnSendData = value; }

    [Published]
    public Event OnPlayerSetupDone { get => FOnPlayerSetupDone; set => FOnPlayerSetupDone = value; }

    [Published]
    public Event OnSettingChanged { get => FOnSettingChanged; set => FOnSettingChanged = value; }

    [Published]
    public CustomizeLeaderEvent OnCustomizeLeader { get => FOnCustomizeLeader; set => FOnCustomizeLeader = value; }

    [Published]
    public LoadMapEvent OnLoadMap { get => FOnLoadMap; set => FOnLoadMap = value; }

    [Published]
    public Event OnSetupMap { get => FOnSetupMap; set => FOnSetupMap = value; }

    [Published]
    public Event OnStart { get => FOnStart; set => FOnStart = value; }

    [Published]
    public Event OnAbort { get => FOnAbort; set => FOnAbort = value; }

    [Published]
    public Event OnSynchronize { get => FOnSynchronize; set => FOnSynchronize = value; }

    [Published]
    public EjectPlayerEvent OnEjectPlayer { get => FOnEjectPlayer; set => FOnEjectPlayer = value; }

    [Published]
    public IncompatibleVersionEvent OnIncompatibleVersion { get => FOnIncompatibleVersion; set => FOnIncompatibleVersion = value; }

    [Published]
    public Event OnSendingMaps { get => FOnSendingMaps; set => FOnSendingMaps = value; }

    private static SetupPlayerBehavior CPUBehavior = new SetupPlayerBehavior
    {
        PlayerType = 1,
        Reserved = ByteArray3.From((ReadOnlySpan<byte>)new byte[3] { 0, 0, 0 }),
        Flags = 0
    };

    private static SetupPlayerBehavior FreeBehavior = new SetupPlayerBehavior
    {
        PlayerType = 5,
        Reserved = ByteArray3.From((ReadOnlySpan<byte>)new byte[3] { 0, 0, 0 }),
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
