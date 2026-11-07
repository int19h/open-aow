// Translated from AoWSetup.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.NetworkE;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.Engine.EngineUnit;
using static Aow.Units.NetworkE.NetworkEUnit;

namespace Aow.Units.AoWSetup;

public enum TSetupStatusElement : byte
{
    ssBusy = 0,
    ssAborted = 1,
    ssRemote = 2
}

public enum TPlayerSetupStatusElement : byte
{
    pssFixed = 0,
    pssForced = 1,
    pssLeaderCustomized = 2,
    pssLoaded = 3
}

public partial class TSetupPackage : TEObject
{
    // AoWSetup.pas:254
    public new const int ClassID = 131681;

    public int FMessageID;
    public int FParam2;
    public int FParam1;
    public TEObject FData;

    // AoWSetup.pas:245
    public override void Destroy()
    {
        TEObject Data;
        Data = FData;
        if (Data != null)
        {
            Data.Free();
        }

        base.Destroy();
    }

    // AoWSetup.pas:257
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FMessageID, 20);
        Stream.rwInteger(ref FParam2, 21);
        Stream.rwInteger(ref FParam1, 22);
        Stream.rwCreateEObject(ref FData, null, 23);
    }

    // AoWSetup.pas:242
    public TSetupPackage(TECustomNode owner) : base(owner)
    {
    }

}

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct TSetupPlayerBehavior
{
    [FieldOffset(0)]
    public byte PlayerType;
    [FieldOffset(1)]
    public FixedArray_byte_3 Reserved;
    [FieldOffset(4)]
    public int Flags;
}

public partial class TRemotePlayer : TEObject
{
    // AoWSetup.pas:266
    public new const int ClassID = 131682;

    public TRemotePlayer(TECustomNode owner) : base(owner)
    { }

    public string FName = "";
    public int FRemoteID;
    public byte FPreferredRace;
    public TMapValidationInfo FMapValidation;
    public int FVersion;

    // AoWSetup.pas:269
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwString(ref FName, 20);
        Stream.rwData(Bytes.Of(ref FRemoteID, 4), 4, 21);
        Stream.rwByte(ref FPreferredRace, 22);
        Stream.rwData(Bytes.Of(ref FMapValidation, 1), 1, 23);
        Stream.rwInteger(ref FVersion, 24);
    }

}

public partial class TRemotePlayerList : TENode
{
    public TRemotePlayerList(TECustomNode owner) : base(owner)
    { }

    // AoWSetup.pas:279
    public TRemotePlayer GetPlayers(int Index) => (TRemotePlayer)FChildren.List[Index];

    // AoWSetup.pas:284
    public int IndexOfRemoteID(int RemoteID)
    {
        int I;
        int Result = -1;
        I = 0;
        while (Result == -1 && I != FChildren.Count)
        {
            if (((TRemotePlayer)FChildren.List[I]).FRemoteID == RemoteID)
            {
                Result = I;
            }

            I += 1;
        }

        return Result;
    }

}

// behaviors, race, status, leader, player, behavior, email
public partial class TPlayerSetupSettings : TEObject
{
    // AoWSetup.pas:312
    public new const int ClassID = 131683;

    public TSetupPlayerBehavior FBehavior;
    public TSetupPlayerBehavior FOriginalBehavior;
    public sbyte FRace;
    // AoW.exe
    public TSet<TPlayerSetupStatusElement> FStatus;
    public TLeader FLeader;
    public byte FPlayerID;
    public byte FBehaviorMode;
    public string FEmailAddress = "";

    // AoWSetup.pas:305
    public override void Destroy()
    {
        FLeader.Free();
        FLeader = null;
        base.Destroy();
    }

    // AoWSetup.pas:315
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FPlayerID, 10);
        Stream.rwData(Bytes.Of(ref FBehavior, 8), 8, 11);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FRace), 12);
        Stream.rwData(Bytes.Of(ref FBehaviorMode, 1), 1, 13);
        Stream.rwEObject(FLeader, 14);
        RwSet(ref FStatus, 1, 15, Stream.rwData, 0);
        Stream.rwData(Bytes.Of(ref FOriginalBehavior, 8), 8, 16);
    }

    // AoWSetup.pas:296
    public TPlayerSetupSettings(TECustomNode owner) : base(owner)
    {
        FStatus = TSet<TPlayerSetupStatusElement>.Of();
        FBehavior = AoWSetupUnit.DefaultBehavior;
        FOriginalBehavior = AoWSetupUnit.DefaultOriginalBehavior;
        FLeader = new TLeader(null);
    }

}

// CRCs, day, play mode, victory, independent AI, seed, limits, players, connection, tactical
// combat, email game name
public partial class TSetupSettings : TENode
{
    // AoWSetup.pas:356
    public new const int ClassID = 131680;

    public int FHostRemoteID;
    public string FFileName = "";
    public string FMapName = "";
    public int FMapCRC;
    public int FSetCRC;
    public int FMapDay;
    public byte FPlayMode;
    public bool FExplorationEnabled;
    public bool FLeadersOnMap;
    public bool FAlliedVictory;
    public byte FVictoryCondition;
    public byte FIndependentAIMode;
    public bool FCustomizeLeaders;
    public int FRandomSeed;
    public int FDayLimit;
    public int FTurnTime;
    public TRemotePlayerList FRemotePlayers;
    public byte FConnectionType;
    public byte FTacticalCombatAvailability;
    public bool FAlwaysViewTC;
    public string FEmailGameName = "";

    // AoWSetup.pas:334
    public override void Destroy()
    {
        FRemotePlayers.Free();
        base.Destroy();
    }

    // AoWSetup.pas:402
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwString(ref FFileName, 10);
        Stream.rwData(Bytes.Of(ref FPlayMode, 1), 1, 11);
        Stream.rwBool(ref FExplorationEnabled, 12);
        Stream.rwString(ref FMapName, 13);
        Stream.rwData(Bytes.Of(ref FVictoryCondition, 1), 1, 14);
        Stream.rwData(Bytes.Of(ref FIndependentAIMode, 1), 1, 15);
        Stream.rwBool(ref FCustomizeLeaders, 16);
        Stream.rwInteger(ref FRandomSeed, 17);
        Stream.rwBool(ref FLeadersOnMap, 18);
        Stream.rwInteger(ref FDayLimit, 19);
        Stream.rwInteger(ref FTurnTime, 20);
        Stream.rwEObject(FRemotePlayers, 21);
        Stream.rwInteger(ref FHostRemoteID, 22);
        Stream.rwInteger(ref FMapDay, 23);
        Stream.rwBool(ref FAlliedVictory, 24);
        Stream.rwData(Bytes.Of(ref FConnectionType, 1), 1, 25);
        Stream.rwInteger(ref FMapCRC, 26);
        Stream.rwInteger(ref FSetCRC, 27);
        Stream.rwData(Bytes.Of(ref FTacticalCombatAvailability, 1), 1, 28);
        Stream.rwBool(ref FAlwaysViewTC, 29);
    }

    // AoWSetup.pas:327
    public TSetupSettings(TECustomNode owner) : base(owner)
    {
        FRemotePlayers = new TRemotePlayerList(null);
        Reset();
    }

    // AoWSetup.pas:340
    public void Reset()
    {
        Clear();
        FTacticalCombatAvailability = 1;
        FCustomizeLeaders = false;
        FTurnTime = 0;
        FDayLimit = 0;
        FPlayMode = 2;
        FExplorationEnabled = true;
        FLeadersOnMap = true;
        FRemotePlayers.Clear();
        FMapName = "";
        FFileName = "";
        FAlwaysViewTC = false;
    }

    // AoWSetup.pas:359
    public TPlayerSetupSettings GetSettings(int Index)
    {
        TPlayerSetupSettings Result;
        if (Index >= 0 && Index < FChildren.Count)
        {
            Result = (TPlayerSetupSettings)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // AoWSetup.pas:366
    public int IndexOfPlayer(byte Player)
    {
        int I;
        int Result = -1;
        I = 0;
        while (Result == -1 && I != FChildren.Count)
        {
            if (((TPlayerSetupSettings)FChildren.List[I]).FPlayerID == Player)
            {
                Result = I;
            }

            I += 1;
        }

        return Result;
    }

    // AoWSetup.pas:378
    public int IndexOfRemoteID(int RemoteID)
    {
        int I;
        int Result = -1;
        I = 0;
        while (Result == -1 && I != FChildren.Count)
        {
            if (((TPlayerSetupSettings)FChildren.List[I]).FBehavior.Flags == RemoteID)
            {
                Result = I;
            }

            I += 1;
        }

        return Result;
    }

    // AoWSetup.pas:390
    public int IndexOfRace(byte Race)
    {
        int I;
        int Result = -1;
        I = 0;
        while (Result == -1 && I != FChildren.Count)
        {
            if (Race == (byte)((TPlayerSetupSettings)FChildren.List[I]).FRace)
            {
                Result = I;
            }

            I += 1;
        }

        return Result;
    }

}

public partial class TCustomizeLeaderControl : TLeaderSetupControl
{
    public TCustomizeLeaderControl(TECustomNode owner) : base(owner)
    { }

    public TSetupControl FSetupControl;
    public int FPlayerIndex;

    // intf_P1_03.inc:99
    public override bool Done() { FSetupControl.LeaderCustomized(this); return true; }
}

public partial class TSetupControl : TComponent
{
    public TSet<TSetupStatusElement> FStatus;
    public TSetupPhase FPhase;
    public TSetupSettings FSettings;
    public TMemoryStream FStream;
    public int FUpdateCount;
    public TRemotePlayer FRemotePlayer;
    public TSetupControlEvent FOnSettingChanged;
    public TLoadMapEvent FOnLoadMap;
    public TSetupControlEvent FOnSetupMap;
    public TSetupControlEvent FOnStart;
    public TSetupControlEvent FOnAbort;
    public TCustomizeLeaderEvent FOnCustomizeLeader;
    public TSetupControlEvent FOnSynchronize;
    public TSetupControlEvent FOnPlayerSetupDone;
    public TEjectPlayerEvent FOnEjectPlayer;
    public TIncompatibleVersionEvent FOnIncompatibleVersion;
    public TSendDataEvent FOnSendData;
    public TSendMapEvent FOnSendMap;
    public TSetupControlEvent FOnSendingMaps;

    // AoWSetup.pas:448
    public override void Destroy()
    {
        FSettings.Free();
        FRemotePlayer.Free();
        FStream.Free();
        base.Destroy();
    }

    // AoWSetup.pas:436
    public TSetupControl(TComponent owner) : base(owner)
    {
        FSettings = new TSetupSettings(null);
        FRemotePlayer = new TRemotePlayer(null);
        FRemotePlayer.FRemoteID = 0;
        FRemotePlayer.FPreferredRace = 255;
        FStream = new TMemoryStream();
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
    public void SendPackage(TSetupPackage Package, int Recipient)
    {
        TMemoryStream Stream;
        Stream = new TMemoryStream();
        try
        {
            AoWEngine.StoreEObject(Stream, Package, null);
            var size = Stream.Size;
            SendData(Stream.Memory, size, Recipient);
            var streamSize = Stream.Size;
            AoWEngine.OutputDebugMessage(NetworkEngineDID, "SetupPackage Send (ID:" + IntToStr(Package.FMessageID) + "/Size:" + IntToStr(streamSize) + ") to " + IntToStr(Recipient));
        }
        finally
        {
            Stream.Free();
        }
    }

    // AoWSetup.pas:476
    public void SendMessage(int MessageID, TEObject Obj, int Param2, int Param1, int Recipient)
    {
        TSetupPackage Package;
        if (GetRemote())
        {
            Package = new TSetupPackage(null);
            Package.FMessageID = MessageID;
            if (Obj != null)
            {
                Package.FData = Obj.Copy(AoWEngine);
            }

            Package.FParam2 = Param2;
            Package.FParam1 = Param1;
            SendPackage(Package, Recipient);
            Package.Free();
        }
    }

    // AoWSetup.pas:492
    public void SendHostMessage(int MessageID, TEObject Obj, int Param2, int Param1)
    {
        TSetupPackage Package;
        if (GetRemote())
        {
            Package = new TSetupPackage(null);
            Package.FMessageID = MessageID;
            if (Obj != null)
            {
                Package.FData = Obj.Copy(AoWEngine);
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

            Package.Free();
        }
    }

    // AoWSetup.pas:509
    public void DataReceived(object Data, int Size, int SenderID)
    {
        TEObject Obj = default;
        TMemoryStream Stream;
        Stream = new TMemoryStream();
        Stream.Size = Size;
        Move(BufferBytes(Data)[..Size], Stream.Memory.AsSpan(0, Size), Size);
        Stream.Position = 0;
        try
        {
            Obj = AoWEngine.RetrieveEObject(Stream, null);
            if (Obj is TSetupPackage)
            {
                var size = Stream.Size;
                AoWEngine.OutputDebugMessage(NetworkEngineDID, "SetupPackage Received (ID:" + IntToStr(((TSetupPackage)Obj).FMessageID) + "/Size:" + IntToStr(size) + ") from " + IntToStr(SenderID));
                PackageReceived((TSetupPackage)Obj, SenderID);
            }

            Obj.Free();
        }
        catch
        {
            // { Original swallows the exception and frees the stream again below. }
            Stream.Free();
        }

        Stream.Free();
    }

    // AoWSetup.pas:530
    public bool ValidateCompatibleVersion(int Version)
    {
        int Major;
        Major = (int)(((uint)(AoWEngine.FVersion & -65536)) >> 16);
        Version = (int)(((uint)(Version & -65536)) >> 16);
        return Major == Version;
    }

    // AoWSetup.pas:537
    public void hConnectPlayer(TRemotePlayer Player)
    {
        if (FPhase == TSetupPhase.spSetupPlayers && (ValidateCompatibleVersion(Player.FVersion) || FRemotePlayer.FRemoteID == Player.FRemoteID))
        {
            if (FSettings.FRemotePlayers.IndexOfRemoteID(Player.FRemoteID) == -1)
            {
                Player = (TRemotePlayer)Player.Copy(AoWEngine);
                Player.FName = ValidatePlayerName(Player.FName);
                Player.FMapValidation = TMapValidationInfo.mviUnknown;
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
                var minorVersion = AoWEngine.GetMinorVersion();
                SendMessage(9, null, AoWEngine.GetMajorVersion(), minorVersion, Player.FRemoteID);
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

            FSettings.FRemotePlayers.GetPlayers(I).Free();
            UpdateSettings();
        }
    }

    // AoWSetup.pas:569
    public void hUpdatePlayer(TRemotePlayer Player)
    {
        int I;
        I = FSettings.FRemotePlayers.IndexOfRemoteID(Player.FRemoteID);
        if (I != -1)
        {
            FSettings.FRemotePlayers.GetPlayers(I).Assign(Player, AoWEngine);
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
                Result = FSettings.FRemotePlayers.GetPlayers(I).FMapValidation == TMapValidationInfo.mviValid;
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
        TMapValidationInfo Validation;
        if (!IsEmpty(FSettings.FMapName) && FRemotePlayer.FMapValidation != TMapValidationInfo.mviValid)
        {
            if (AoWHSSet.FHSSCRC == FSettings.FSetCRC)
            {
                // The client looks up the host's map by name, its own document first (own-formats.md 2.9).
                FileName = FileBoundary.ReadName(AoWEngine.StartupDirectory + FSettings.FMapName);
                if (FileExists(FileName))
                {
                    if (FileBoundary.ImageCrc32(FileName, Aow.Formats.GameProfile.WorldMap) == FSettings.FMapCRC)
                    {
                        Validation = TMapValidationInfo.mviValid;
                    }
                    else
                    {
                        Validation = TMapValidationInfo.mviInvalidMap;
                    }
                }
                else
                {
                    Validation = TMapValidationInfo.mviInvalidMap;
                }
            }
            else
            {
                Validation = TMapValidationInfo.mviInvalid;
            }

            bool condition = Validation == TMapValidationInfo.mviInvalidMap && FStream.Size != 0;
            if (condition)
            {
                var size = FStream.Size;
                condition = GetCRC32(FStream.Memory, size) == FSettings.FMapCRC;
            }

            if (condition)
            {
                Validation = TMapValidationInfo.mviValid;
            }

            if (Validation != FRemotePlayer.FMapValidation)
            {
                FRemotePlayer.FMapValidation = Validation;
                SendHostMessage(3, FRemotePlayer, 0, 0);
            }
        }
    }

    // AoWSetup.pas:627
    public void PackageReceived(TSetupPackage Package, int SenderID)
    {
        int I = default;
        switch (Package.FMessageID)
        {
            case 1:
                hConnectPlayer((TRemotePlayer)Package.FData);
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
                    hUpdatePlayer((TRemotePlayer)Package.FData);
                }

                break;
            case 4:
                if (!GetHost() && FPhase == TSetupPhase.spSetupPlayers)
                {
                    FSettings.Assign(Package.FData, AoWEngine);
                    I = FSettings.FRemotePlayers.IndexOfRemoteID(FRemotePlayer.FRemoteID);
                    if (I != -1)
                    {
                        var playerName = FSettings.FRemotePlayers.GetPlayers(I);
                        FRemotePlayer.FName = playerName.FName;
                        var playerPreferredRace = FSettings.FRemotePlayers.GetPlayers(I);
                        FRemotePlayer.FPreferredRace = playerPreferredRace.FPreferredRace;
                        var playerMapValidation = FSettings.FRemotePlayers.GetPlayers(I);
                        FRemotePlayer.FMapValidation = playerMapValidation.FMapValidation;
                        if (FRemotePlayer.FMapValidation == TMapValidationInfo.mviUnknown)
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
                    FSettings.Assign(Package.FData, AoWEngine);
                    I = FSettings.FRemotePlayers.IndexOfRemoteID(FRemotePlayer.FRemoteID);
                    if (I != -1)
                    {
                        var playerName = FSettings.FRemotePlayers.GetPlayers(I);
                        FRemotePlayer.FName = playerName.FName;
                        var playerPreferredRace = FSettings.FRemotePlayers.GetPlayers(I);
                        FRemotePlayer.FPreferredRace = playerPreferredRace.FPreferredRace;
                        var playerMapValidation = FSettings.FRemotePlayers.GetPlayers(I);
                        FRemotePlayer.FMapValidation = playerMapValidation.FMapValidation;
                        if (FRemotePlayer.FMapValidation == TMapValidationInfo.mviUnknown)
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
                    hLeaderCustomized(Package.FParam2, (TLeader)Package.FData);
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
                    FSettings.Assign(Package.FData, AoWEngine);
                    SetupMap();
                }

                break;
            case 10:
                if (!GetHost())
                {
                    FPhase = TSetupPhase.spSendMap;
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
        FPhase = TSetupPhase.spLoadMap;
        if (FOnLoadMap != null)
        {
            FOnLoadMap(this, AoWEngine.StartupDirectory + FSettings.FMapName);
        }
    }

    // AoWSetup.pas:689
    public bool hValidateSendMap()
    {
        bool Result = default;
        TMemoryStream Stream;
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
                if (J != -1 && FSettings.FRemotePlayers.GetPlayers(J).FMapValidation == TMapValidationInfo.mviInvalidMap)
                {
                    RemoteID = FSettings.GetSettings(I).FBehavior.Flags;
                }
            }
        }

        if (RemoteID != 0)
        {
            // The host sends the map's engine image, the bytes FMapCRC covers and LoadHSMfromStream
            // reads, for an own document as for an original (own-formats.md 4.1).
            Stream = Aow.Units.Engine.TEngine.ImageStream(FileBoundary.LoadOrRaise(AoWEngine.StartupDirectory + FSettings.FMapName, Aow.Formats.GameProfile.WorldMap));
            if (FOnSendMap != null)
            {
                var size = Stream.Size;
                FOnSendMap(this, Stream.Memory, size, RemoteID);
            }

            Stream.Free();
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
        FPhase = TSetupPhase.spSendMap;
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
            FStream.Size = Size;
            Move(BufferBytes(Data)[..Size], FStream.Memory.AsSpan(0, Size), Size);
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
            FSettings.GetSettings(Index).FStatus = setting.FStatus + TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssLoaded);
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
            FPhase = TSetupPhase.spSynchronize;
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
                    FSettings.GetSettings(I).FStatus = setting.FStatus + TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssLoaded);
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
    public void hLeaderCustomized(int Index, TLeader Leader)
    {
        if (GetHost())
        {
            FSettings.GetSettings(Index).FLeader.Assign(Leader, AoWEngine);
            var setting = FSettings.GetSettings(Index);
            FSettings.GetSettings(Index).FStatus = setting.FStatus + TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssLeaderCustomized);
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
        TCustomizeLeaderControl CLC = default;
        FPhase = TSetupPhase.spCustomizeLeaders;
        bool Result = false;
        Found = false;
        I = 0;
        while (!Found && I != FSettings.Count)
        {
            Found = FSettings.GetSettings(I).FBehavior.PlayerType == 0 && (!GetRemote() || FSettings.GetSettings(I).FBehavior.Flags == FRemotePlayer.FRemoteID) && !FSettings.GetSettings(I).FStatus.Contains(TPlayerSetupStatusElement.pssLeaderCustomized);
            if (!Found)
            {
                I += 1;
            }
        }

        if (Found)
        {
            CLC = new TCustomizeLeaderControl(null);
            CLC.FSetupControl = this;
            CLC.FPlayerIndex = I;
            var settingRace = FSettings.GetSettings(I);
            CLC.FItems.Add(settingRace.FRace);
            var settingLeader = FSettings.GetSettings(I);
            CLC.FLeader.Assign(settingLeader.FLeader, AoWEngine);
            var setting = FSettings.GetSettings(I);
            CLC.FOriginalLeader.Assign(setting.FLeader, AoWEngine);
            if (GetRemote())
            {
                CLC.FLeader.FHeroName = FRemotePlayer.FName;
                CLC.FOriginalLeader.FHeroName = FRemotePlayer.FName;
            }

            if (!FSettings.FLeadersOnMap)
            {
                CLC.FMode = TSet<TLeaderSetupModeElement>.FromMask((CLC.FMode - TSet<TLeaderSetupModeElement>.Of((TLeaderSetupModeElement)1)).Mask);
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
            FPhase = TSetupPhase.spSynchronize;
            if (FOnSynchronize != null)
            {
                FOnSynchronize(this);
            }
        }

        return Result;
    }

    // AoWSetup.pas:817
    public void LeaderCustomized(TCustomizeLeaderControl Control)
    {
        if (!GetHost())
        {
            SendHostMessage(8, Control.FOriginalLeader, Control.FPlayerIndex, 0);
            FSettings.GetSettings(Control.FPlayerIndex).FLeader.Assign(Control.FOriginalLeader, AoWEngine);
        }
        else
        {
            hLeaderCustomized(Control.FPlayerIndex, Control.FOriginalLeader);
        }

        if (FStatus.Contains(TSetupStatusElement.ssBusy))
        {
            var setting = FSettings.GetSettings(Control.FPlayerIndex);
            FSettings.GetSettings(Control.FPlayerIndex).FStatus = setting.FStatus + TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssLeaderCustomized);
            CustomizeLeaders();
        }
    }

    // AoWSetup.pas:830
    public void ListPCCInfo(TPlayerCommunicationControl PCC)
    {
        string S;
        for (int I = 0, last = PCC.Count - 1; I <= last; I++)
        {
            var player = PCC.Players[I];
            S = "Player (" + IntToStr(I) + ") at system:" + IntToStr(player.SystemID) + " CM:";
            switch (PCC.Players[I].ConnectionMode)
            {
                case TConnectionMode.cmGlobal:
                    S = S + "Global";
                    break;
                case TConnectionMode.cmLocal:
                    S = S + "Local";
                    break;
                case TConnectionMode.cmRemote:
                    S = S + "Remote";
                    break;
            }

            AoWEngine.OutputDebugMessage(NetworkEngineDID, S);
        }
    }

    // AoWSetup.pas:844
    public void RemoveIndependentPlayers()
    {
        TPlayer Player;
        for (int I = AoWHSMap.FPlayers.GetCount() - 1; I >= 1; I--)
        {
            Player = AoWHSMap.FPlayers.GetPlayers(I);
            if (Player.FPlayerType == TPlayerType.ptIndependent)
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
                Player.FStatus = TPlayerStatus.psTerminated;
            }
        }
    }

    // AoWSetup.pas:847
    public void SetupMap()
    {
        int Index = default;
        TPlayer Player = default;
        TRemotePCC RemotePCC = default;
        TLocalPCC LocalPCC = default;
        FPhase = TSetupPhase.spSetupMap;
        for (int I = 0, last = FSettings.GetCount() - 1; I <= last; I++)
        {
            var settingPlayerID = FSettings.GetSettings(I);
            Player = AoWHSMap.FPlayers.GetPlayers((sbyte)settingPlayerID.FPlayerID);
            Player.FPlayerType = (TPlayerType)FSettings.GetSettings(I).FBehavior.PlayerType;
            Player.FBehavior = (TPlayerBehavior)FSettings.GetSettings(I).FBehaviorMode;
            if (FSettings.FConnectionType == 2)
            {
                var localEmailAddress = AoWEngine.FRegistry.GetLocalEmailAddress();
                if (CompareText(FSettings.GetSettings(I).FEmailAddress, localEmailAddress) != 0)
                {
                    var setting = FSettings.GetSettings(I);
                    Player.FPBEMSettings.FEmailGame.FEmailAddress = setting.FEmailAddress;
                    Player.FPBEMSettings.FEmailGame.FName = FSettings.FEmailGameName;
                }
                else
                {
                    var setting = FSettings.GetSettings(I);
                    var emailGame = AoWEngine.FEmailGames.CreateEmailGame(FSettings.FEmailGameName, setting.FEmailAddress);
                    Player.FPBEMSettings.SetGame(emailGame);
                }
            }

            if (FSettings.FMapDay == 0)
            {
                Player.FLeader.UpdateFromLibraryBegin();
                var setting = FSettings.GetSettings(I);
                Player.FLeader.Assign(setting.FLeader, AoWEngine);
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

        AoWHSMap.FConnectionType = (TPlayerConnectionType)FSettings.FConnectionType;
        if (GetRemote())
        {
            RemotePCC = new TRemotePCC();
            RemotePCC.Setup(AoWHSMap.FPlayers.GetCount(), FRemotePlayer.FRemoteID);
            RemotePCC.SetConnectionMode(0, TConnectionMode.cmGlobal);
            RemotePCC.SetSystemID(0, FRemotePlayer.FRemoteID);
            for (int I = 0, last = FSettings.GetCount() - 1; I <= last; I++)
            {
                if (FSettings.GetSettings(I).FBehavior.PlayerType != 0)
                {
                    RemotePCC.SetConnectionMode((sbyte)FSettings.GetSettings(I).FPlayerID, TConnectionMode.cmGlobal);
                    RemotePCC.SetSystemID((sbyte)FSettings.GetSettings(I).FPlayerID, FRemotePlayer.FRemoteID);
                }
                else
                {
                    if (FSettings.GetSettings(I).FBehavior.Flags == FRemotePlayer.FRemoteID)
                    {
                        RemotePCC.SetConnectionMode((sbyte)FSettings.GetSettings(I).FPlayerID, TConnectionMode.cmLocal);
                    }
                    else
                    {
                        RemotePCC.SetConnectionMode((sbyte)FSettings.GetSettings(I).FPlayerID, TConnectionMode.cmRemote);
                    }

                    var setting = FSettings.GetSettings(I);
                    RemotePCC.SetSystemID((sbyte)FSettings.GetSettings(I).FPlayerID, setting.FBehavior.Flags);
                }
            }

            AoWHSMap.FPlayerCommunicator.ActivatePCC(RemotePCC);
            RemotePCC.OnSendData = FOnSendData;
            ListPCCInfo(RemotePCC);
            RemotePCC.Release();
        }
        else
        {
            LocalPCC = new TLocalPCC();
            LocalPCC.Setup(AoWHSMap.FPlayers.GetCount());
            LocalPCC.SetConnectionMode(0, TConnectionMode.cmGlobal);
            for (int I = 0, last = FSettings.GetCount() - 1; I <= last; I++)
            {
                if (FSettings.GetSettings(I).FBehavior.PlayerType == 0)
                {
                    LocalPCC.SetConnectionMode((sbyte)FSettings.GetSettings(I).FPlayerID, TConnectionMode.cmLocal);
                }
                else
                {
                    LocalPCC.SetConnectionMode((sbyte)FSettings.GetSettings(I).FPlayerID, TConnectionMode.cmGlobal);
                }
            }

            AoWHSMap.FPlayerCommunicator.ActivatePCC(LocalPCC);
            ListPCCInfo(LocalPCC);
            LocalPCC.Release();
        }

        if (FSettings.FMapDay == 0)
        {
            AoWHSMap.FInitialRandomSeed = FSettings.FRandomSeed;
            AoWHSMap.FFixedLeaders = FSettings.FLeadersOnMap;
            AoWHSMap.FAlliedVictory = FSettings.FAlliedVictory;
            AoWHSMap.SetExplorationEnabled(FSettings.FExplorationEnabled);
            AoWHSMap.SetFogEnabled(true);
        }

        AoWHSMap.FDayLimit = FSettings.FDayLimit;
        AoWHSMap.FTurnTime = FSettings.FTurnTime;
        AoWHSMap.FVictoryMode = FSettings.FTacticalCombatAvailability;
        AoWHSMap.FLeadersFixed = FSettings.FCustomizeLeaders;
        AoWHSMap.FStartingGame = FSettings.FAlwaysViewTC;
        RemoveIndependentPlayers();
        AoWHSMap.SetupPlayerControl((TPlayMode)FSettings.FPlayMode, (TPlayerConnectionType)FSettings.FConnectionType);
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
                    Result = FSettings.GetSettings(I).FStatus.Contains(TPlayerSetupStatusElement.pssLoaded) && (!FSettings.FCustomizeLeaders || FSettings.GetSettings(I).FStatus.Contains(TPlayerSetupStatusElement.pssLeaderCustomized));
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
    public bool GetRemote() => FStatus.Contains(TSetupStatusElement.ssRemote);

    // AoWSetup.pas:902
    public bool GetHost() => FSettings.FHostRemoteID == FRemotePlayer.FRemoteID;

    // AoWSetup.pas:906
    public void Reset()
    {
        FPhase = TSetupPhase.spIdle;
        FStatus = TSet<TSetupStatusElement>.Of();
        FRemotePlayer.FName = "";
        FRemotePlayer.FMapValidation = TMapValidationInfo.mviUnknown;
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
                FSettings.GetSettings(Index).FBehavior = AoWSetupUnit.CPUBehavior;
            }
        }
        else
        {
            FSettings.GetSettings(Index).FBehavior = AoWSetupUnit.FreeBehavior;
        }

        var settingStatus = FSettings.GetSettings(Index);
        FSettings.GetSettings(Index).FStatus = settingStatus.FStatus - TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssForced);
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
                    if (Target != -1 && FSettings.GetSettings(Target).FBehavior.Flags == 0 && (FSettings.GetSettings(Target).FStatus & TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssFixed, TPlayerSetupStatusElement.pssForced)) == TSet<TPlayerSetupStatusElement>.Of())
                    {
                        var playerRemoteID = FSettings.FRemotePlayers.GetPlayers(I);
                        Old = FSettings.IndexOfRemoteID(playerRemoteID.FRemoteID);
                        if (Old == -1 || !FSettings.GetSettings(Old).FStatus.Contains(TPlayerSetupStatusElement.pssForced))
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
                    if (Old != -1 && !FSettings.GetSettings(Old).FStatus.Contains(TPlayerSetupStatusElement.pssForced))
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
    public void ListAvailablePlayers(TStrings List)
    {
        if (GetRemote())
        {
            List.Add(TranslateRStr(TPlayerTypeRStr[5]));
            for (int I = 0, last = FSettings.FRemotePlayers.Count - 1; I <= last; I++)
            {
                List.Add(FSettings.FRemotePlayers.GetPlayers(I).FName);
            }

            List.Add(TranslateRStr(TPlayerTypeRStr[1]));
            List.Add(TranslateRStr(TPlayerTypeRStr[2]));
            List.Add(TranslateRStr(TPlayerTypeRStr[3]));
            List.Add(TranslateRStr(TPlayerTypeRStr[6]));
            List.Add(TranslateRStr(TPlayerTypeRStr[7]));
            List.Add(TranslateRStr(TPlayerTypeRStr[4]));
        }
        else
        {
            List.Add(TranslateRStr(TPlayerTypeRStr[0]));
            List.Add(TranslateRStr(TPlayerTypeRStr[1]));
            List.Add(TranslateRStr(TPlayerTypeRStr[2]));
            List.Add(TranslateRStr(TPlayerTypeRStr[3]));
            List.Add(TranslateRStr(TPlayerTypeRStr[6]));
            List.Add(TranslateRStr(TPlayerTypeRStr[7]));
            List.Add(TranslateRStr(TPlayerTypeRStr[4]));
        }
    }

    // AoWSetup.pas:970
    public string ValidatePlayerName(string Name)
    {
        bool Done;
        TStringList List;
        int I;
        bool Unique;
        List = new TStringList();
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
                if (TSet<byte>.WithBounds(0, 255).WithRange(49, 56).Contains(Cp1252.Byte(Result[Result.Length - 1])))
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

        List.Free();
        return Result;
    }

    // AoWSetup.pas:995
    public bool PlayerNameToPlayerID(string Name, ref TSetupPlayerBehavior Behavior)
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
                if (CompareText(Name, TranslateRStr(TPlayerTypeRStr[PlayerType])) == 0)
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
            if (TSet<byte>.WithBounds(0, 255).With(6).With(7).WithRange(0, 3).Contains(settingBehavior.FBehavior.PlayerType))
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
                    condition = TSet<TMapValidationInfo>.Of(TMapValidationInfo.mviInvalidMap, TMapValidationInfo.mviValid).Contains(player.FMapValidation);
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
                    Result = FSettings.FRemotePlayers.GetPlayers(J).FMapValidation == TMapValidationInfo.mviValid;
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

                        FSettings.FRemotePlayers.GetPlayers(I).Free();
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
    public void hSetPlayerID(int Index, in TSetupPlayerBehavior Behavior, bool Forced)
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
                FSettings.GetSettings(PlayerIndex).FStatus = setting.FStatus + TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssForced);
            }
            else
            {
                var setting = FSettings.GetSettings(PlayerIndex);
                FSettings.GetSettings(PlayerIndex).FStatus = setting.FStatus - TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssForced);
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
        TPlayerSetupSettings Settings = default;
        TAoWHSMap Preview;
        Preview = (TAoWHSMap)AoWEngine.CreatePreviewHSM(AoWEngine.StartupDirectory + FileName, Aow.Formats.GameProfile.WorldMap);
        if (GetHost() && Preview != null && (!AoWEngine.FDemo || Preview.FShowAllPlayers) && Preview.FConnectionType != TPlayerConnectionType.pcEmail)
        {
            AoWEngine.InitializeHSSet(AoWEngine.StartupDirectory + Preview.HSSFileName, null);
            Randomize();
            FSettings.Clear();
            FSettings.FMapName = FileName;
            FSettings.FMapDay = Preview.FDay;
            FSettings.FFileName = Preview.FMapName;
            // The map CRC is taken over the engine image, so an own copy and its original agree (own-formats.md 4.1).
            var fileCRC = FileBoundary.ImageCrc32(AoWEngine.StartupDirectory + FileName, Aow.Formats.GameProfile.WorldMap);
            FSettings.FMapCRC = fileCRC;
            FSettings.FSetCRC = AoWHSSet.FHSSCRC;
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
            FRemotePlayer.FMapValidation = TMapValidationInfo.mviValid;
            for (int I = 0, last = FSettings.FRemotePlayers.GetCount() - 1; I <= last; I++)
            {
                FSettings.FRemotePlayers.GetPlayers(I).FPreferredRace = 255;
                if (FSettings.FRemotePlayers.GetPlayers(I).FRemoteID == FRemotePlayer.FRemoteID)
                {
                    FSettings.FRemotePlayers.GetPlayers(I).FMapValidation = TMapValidationInfo.mviValid;
                }
                else
                {
                    FSettings.FRemotePlayers.GetPlayers(I).FMapValidation = TMapValidationInfo.mviUnknown;
                }
            }

            for (int I = 1, last = Preview.FPlayers.GetCount() - 1; I <= last; I++)
            {
                var player = Preview.FPlayers.GetPlayers(I);
                if (player.FStatus != TPlayerStatus.psTerminated)
                {
                    Settings = new TPlayerSetupSettings(this.FSettings);
                    Settings.FPlayerID = (byte)I;
                    Settings.FRace = player.FPlayerRace;
                    Settings.FLeader.Assign(player.FLeaderSetup, AoWEngine);
                    Settings.FBehaviorMode = (byte)(int)player.FBehavior;
                    if (player.FParticipating)
                    {
                        Settings.FStatus = Settings.FStatus + TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssFixed);
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
                        if (player.FPlayerType == TPlayerType.ptHuman)
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
            Preview.Free();
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
                FSettings.FRemotePlayers.GetPlayers(I).Free();
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
        FPhase = TSetupPhase.spSetupPlayers;
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
        this.FRemotePlayer.FVersion = AoWEngine.FVersion;

        FStatus = FStatus + TSet<TSetupStatusElement>.Of(TSetupStatusElement.ssBusy);
        if (RemoteID != 0)
        {
            hConnectPlayer(FRemotePlayer);
            FStatus = FStatus + TSet<TSetupStatusElement>.Of(TSetupStatusElement.ssRemote);
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
        FRemotePlayer.FVersion = AoWEngine.FVersion;
        FStatus = FStatus + TSet<TSetupStatusElement>.Of(TSetupStatusElement.ssBusy, TSetupStatusElement.ssRemote);
        SendHostMessage(1, FRemotePlayer, 0, 0);
        FPhase = TSetupPhase.spSetupPlayers;
        return true;
    }

    [Published]
    public TSendMapEvent OnSendMap { get => FOnSendMap; set => FOnSendMap = value; }

    [Published]
    public TSendDataEvent OnSendData { get => FOnSendData; set => FOnSendData = value; }

    [Published]
    public TSetupControlEvent OnPlayerSetupDone { get => FOnPlayerSetupDone; set => FOnPlayerSetupDone = value; }

    [Published]
    public TSetupControlEvent OnSettingChanged { get => FOnSettingChanged; set => FOnSettingChanged = value; }

    [Published]
    public TCustomizeLeaderEvent OnCustomizeLeader { get => FOnCustomizeLeader; set => FOnCustomizeLeader = value; }

    [Published]
    public TLoadMapEvent OnLoadMap { get => FOnLoadMap; set => FOnLoadMap = value; }

    [Published]
    public TSetupControlEvent OnSetupMap { get => FOnSetupMap; set => FOnSetupMap = value; }

    [Published]
    public TSetupControlEvent OnStart { get => FOnStart; set => FOnStart = value; }

    [Published]
    public TSetupControlEvent OnAbort { get => FOnAbort; set => FOnAbort = value; }

    [Published]
    public TSetupControlEvent OnSynchronize { get => FOnSynchronize; set => FOnSynchronize = value; }

    [Published]
    public TEjectPlayerEvent OnEjectPlayer { get => FOnEjectPlayer; set => FOnEjectPlayer = value; }

    [Published]
    public TIncompatibleVersionEvent OnIncompatibleVersion { get => FOnIncompatibleVersion; set => FOnIncompatibleVersion = value; }

    [Published]
    public TSetupControlEvent OnSendingMaps { get => FOnSendingMaps; set => FOnSendingMaps = value; }
}

public enum TSetupPhase : byte
{
    spIdle = 0,
    spSetupPlayers = 1,
    spCustomizeLeaders = 2,
    spSendMap = 3,
    spLoadMap = 4,
    spSynchronize = 5,
    spSetupMap = 6
}

public enum TMapValidationInfo : byte
{
    mviUnknown = 0,
    mviInvalid = 1,
    mviInvalidMap = 2,
    mviValid = 3
}

public delegate void TSetupControlEvent(TSetupControl SetupControl);
public delegate void TCustomizeLeaderEvent(TSetupControl SetupControl, TCustomizeLeaderControl CLC);
public delegate void TLoadMapEvent(TSetupControl SetupControl, string Filename);
public delegate void TEjectPlayerEvent(TSetupControl SetupControl, int RemoteID);
public delegate void TIncompatibleVersionEvent(TSetupControl SetupControl, int MajorVersion, int MinorVersion);
public delegate void TSendMapEvent(TSetupControl SetupControl, object Data, int Size, int Recipient);

public partial class TLeaderSetupView : TObject
{
    public TLeader OriginalLeader;
    public TSet<byte> Options;
    public TLeader Leader;
    public TIntegerList Races;
}

public partial class TSetupHeroView : TObject
{
    public string Name = "";
}

public static partial class AoWSetupUnit
{
    // AoWSetup.pas:1334
    public static void RegisterSetupClasses(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TSetupSettings>(), EClass.Of<TPlayerSetupSettings>(), EClass.Of<TSetupPackage>(), EClass.Of<TRemotePlayer>()]);
    }

    public static TSetupPlayerBehavior CPUBehavior = new TSetupPlayerBehavior
    {
        PlayerType = 1,
        Reserved = FixedArray_byte_3.From([0, 0, 0]),
        Flags = 0
    };

    public static TSetupPlayerBehavior FreeBehavior = new TSetupPlayerBehavior
    {
        PlayerType = 5,
        Reserved = FixedArray_byte_3.From([0, 0, 0]),
        Flags = 0
    };

    public static TSetupPlayerBehavior DefaultBehavior = new TSetupPlayerBehavior
    {
        PlayerType = 0,
        Reserved = FixedArray_byte_3.From([0, 0, 0]),
        Flags = 0
    };

    public static TSetupPlayerBehavior DefaultOriginalBehavior = new TSetupPlayerBehavior
    {
        PlayerType = 5,
        Reserved = FixedArray_byte_3.From([0, 0, 0]),
        Flags = 0
    };

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
