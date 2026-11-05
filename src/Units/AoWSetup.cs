// Translated from AoWSetup.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AoWSetup
{
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
            TEObject Data = default;
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

        public AnsiString FName;
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
        public TRemotePlayer GetPlayers(int Index)
        {
            TRemotePlayer Result = default;
            Result = (TRemotePlayer)FChildren.List[Index];
            return Result;
        }

        // AoWSetup.pas:284
        public int IndexOfRemoteID(int RemoteID)
        {
            int Result = default;
            int I = default;
            Result = -1;
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
        public AnsiString FEmailAddress;
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
            FBehavior = AoWSetup.DefaultBehavior;
            FOriginalBehavior = AoWSetup.DefaultOriginalBehavior;
            var call__1 = new TLeader(null);
            FLeader = call__1;
        }

    }

    // CRCs, day, play mode, victory, independent AI, seed, limits, players, connection, tactical
    // combat, email game name
    public partial class TSetupSettings : TENode
    {
        // AoWSetup.pas:356
        public new const int ClassID = 131680;

        public int FHostRemoteID;
        public AnsiString FFileName;
        public AnsiString FMapName;
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
        public AnsiString FEmailGameName;
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
            var call__2 = new TRemotePlayerList(null);
            FRemotePlayers = call__2;
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
            FMapName = (AnsiString)"";
            FFileName = (AnsiString)"";
            FAlwaysViewTC = false;
        }

        // AoWSetup.pas:359
        public TPlayerSetupSettings GetSettings(int Index)
        {
            TPlayerSetupSettings Result = default;
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
            int Result = default;
            int I = default;
            Result = -1;
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
            int Result = default;
            int I = default;
            Result = -1;
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
            int Result = default;
            int I = default;
            Result = -1;
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
            var call__3 = new TSetupSettings(null);
            FSettings = call__3;
            var call__4 = new TRemotePlayer(null);
            FRemotePlayer = call__4;
            FRemotePlayer.FRemoteID = 0;
            FRemotePlayer.FPreferredRace = 255;
            var call__5 = new TMemoryStream();
            FStream = call__5;
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
            TMemoryStream Stream = default;
            Stream = new TMemoryStream();
            try
            {
                AoWEngine.StoreEObject(Stream, Package, null);
                var call__6 = Stream.Size;
                SendData(Stream.Memory, call__6, Recipient);
                var call__7 = Stream.Size;
                AoWEngine.OutputDebugMessage(NetworkEngineDID, (AnsiString)"SetupPackage Send (ID:" + IntToStr(Package.FMessageID) + (AnsiString)"/Size:" + IntToStr(call__7) + (AnsiString)") to " + IntToStr(Recipient));
            }
            finally
            {
                Stream.Free();
            }
        }

        // AoWSetup.pas:476
        public void SendMessage(int MessageID, TEObject Obj, int Param2, int Param1, int Recipient)
        {
            TSetupPackage Package = default;
            if (GetRemote())
            {
                Package = new TSetupPackage(null);
                Package.FMessageID = MessageID;
                if (Obj != null)
                {
                    var call__8 = Obj.Copy(AoWEngine);
                    Package.FData = call__8;
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
            TSetupPackage Package = default;
            if (GetRemote())
            {
                Package = new TSetupPackage(null);
                Package.FMessageID = MessageID;
                if (Obj != null)
                {
                    var call__9 = Obj.Copy(AoWEngine);
                    Package.FData = call__9;
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
            TMemoryStream Stream = default;
            Stream = new TMemoryStream();
            Stream.Size = Size;
            Move(BufferBytes(Data)[..Size], Stream.Memory.AsSpan(0, Size), Size);
            Stream.Position = 0;
            try
            {
                var call__10 = AoWEngine.RetrieveEObject(Stream, null);
                Obj = call__10;
                if (Obj is TSetupPackage)
                {
                    var call__11 = Stream.Size;
                    AoWEngine.OutputDebugMessage(NetworkEngineDID, (AnsiString)"SetupPackage Received (ID:" + IntToStr(((TSetupPackage)Obj).FMessageID) + (AnsiString)"/Size:" + IntToStr(call__11) + (AnsiString)") from " + IntToStr(SenderID));
                    PackageReceived((TSetupPackage)Obj, SenderID);
                }

                Obj.Free();
            }
            catch (System.Exception)
            {
                // { Original swallows the exception and frees the stream again below. }
                Stream.Free();
            }

            Stream.Free();
        }

        // AoWSetup.pas:530
        public bool ValidateCompatibleVersion(int Version)
        {
            bool Result = default;
            int Major = default;
            Major = (int)(((uint)(AoWEngine.FVersion & -65536)) >> 16);
            Version = (int)(((uint)(Version & -65536)) >> 16);
            Result = Major == Version;
            return Result;
        }

        // AoWSetup.pas:537
        public void hConnectPlayer(TRemotePlayer Player)
        {
            bool bool__12 = FPhase == TSetupPhase.spSetupPlayers;
            if (bool__12)
            {
                var call__13 = ValidateCompatibleVersion(Player.FVersion);
                bool__12 = call__13 || FRemotePlayer.FRemoteID == Player.FRemoteID;
            }

            if (bool__12)
            {
                var call__14 = FSettings.FRemotePlayers.IndexOfRemoteID(Player.FRemoteID);
                if (call__14 == -1)
                {
                    var call__15 = Player.Copy(AoWEngine);
                    Player = (TRemotePlayer)call__15;
                    var call__16 = ValidatePlayerName(Player.FName);
                    Player.FName = call__16;
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
                    var call__17 = AoWEngine.GetMinorVersion();
                    var call__18 = AoWEngine.GetMajorVersion();
                    SendMessage(9, null, call__18, call__17, Player.FRemoteID);
                }
            }
        }

        // AoWSetup.pas:556
        public void hDisconnectPlayer(int RemoteID)
        {
            int I = default;
            int J = default;
            var call__19 = FSettings.FRemotePlayers.IndexOfRemoteID(RemoteID);
            I = call__19;
            if (I != -1)
            {
                var call__20 = FSettings.IndexOfRemoteID(RemoteID);
                J = call__20;
                if (J != -1)
                {
                    ResetPlayerSettings(J);
                }

                var call__21 = FSettings.FRemotePlayers.GetPlayers(I);
                call__21.Free();
                UpdateSettings();
            }
        }

        // AoWSetup.pas:569
        public void hUpdatePlayer(TRemotePlayer Player)
        {
            int I = default;
            var call__22 = FSettings.FRemotePlayers.IndexOfRemoteID(Player.FRemoteID);
            I = call__22;
            if (I != -1)
            {
                var call__23 = FSettings.FRemotePlayers.GetPlayers(I);
                call__23.Assign(Player, AoWEngine);
                var call__24 = FSettings.FRemotePlayers.GetPlayers(I);
                call__24.FName = (AnsiString)"";
                var call__25 = ValidatePlayerName(Player.FName);
                var call__26 = FSettings.FRemotePlayers.GetPlayers(I);
                call__26.FName = call__25;
                UpdatePreferredRaces();
                UpdateSettings();
            }
        }

        // AoWSetup.pas:583
        public bool CanSetHostRemoteID(int RemoteID)
        {
            bool Result = default;
            int I = default;
            var call__27 = GetHost();
            if (call__27 && RemoteID != FSettings.FHostRemoteID)
            {
                var call__28 = FSettings.FRemotePlayers.IndexOfRemoteID(RemoteID);
                I = call__28;
                if (I != -1)
                {
                    var call__29 = FSettings.FRemotePlayers.GetPlayers(I);
                    Result = call__29.FMapValidation == TMapValidationInfo.mviValid;
                }
                else
                {
                    Result = false;
                    return Result;
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
            AnsiString FileName = default;
            TMapValidationInfo Validation = default;
            if (FSettings.FMapName != (AnsiString)"" && FRemotePlayer.FMapValidation != TMapValidationInfo.mviValid)
            {
                if (AoWHSSet.FHSSCRC == FSettings.FSetCRC)
                {
                    FileName = AoWEngine.StartupDirectory + FSettings.FMapName;
                    if (FileExists(FileName))
                    {
                        var call__30 = GetFileCRC32(FileName);
                        if (call__30 == FSettings.FMapCRC)
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

                bool bool__31 = Validation == TMapValidationInfo.mviInvalidMap;
                if (bool__31)
                {
                    var call__32 = FStream.Size;
                    bool__31 = call__32 != 0;
                }

                bool bool__33 = bool__31;
                if (bool__33)
                {
                    var call__34 = FStream.Size;
                    var call__35 = GetCRC32(FStream.Memory, call__34);
                    bool__33 = call__35 == FSettings.FMapCRC;
                }

                if (bool__33)
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
                    var call__36 = GetHost();
                    if (!call__36 && FPhase == TSetupPhase.spSetupPlayers)
                    {
                        FSettings.Assign(Package.FData, AoWEngine);
                        var call__37 = FSettings.FRemotePlayers.IndexOfRemoteID(FRemotePlayer.FRemoteID);
                        I = call__37;
                        if (I != -1)
                        {
                            var call__38 = FSettings.FRemotePlayers.GetPlayers(I);
                            FRemotePlayer.FName = call__38.FName;
                            var call__39 = FSettings.FRemotePlayers.GetPlayers(I);
                            FRemotePlayer.FPreferredRace = call__39.FPreferredRace;
                            var call__40 = FSettings.FRemotePlayers.GetPlayers(I);
                            FRemotePlayer.FMapValidation = call__40.FMapValidation;
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
                        var call__41 = FSettings.FRemotePlayers.IndexOfRemoteID(FRemotePlayer.FRemoteID);
                        I = call__41;
                        if (I != -1)
                        {
                            var call__42 = FSettings.FRemotePlayers.GetPlayers(I);
                            FRemotePlayer.FName = call__42.FName;
                            var call__43 = FSettings.FRemotePlayers.GetPlayers(I);
                            FRemotePlayer.FPreferredRace = call__43.FPreferredRace;
                            var call__44 = FSettings.FRemotePlayers.GetPlayers(I);
                            FRemotePlayer.FMapValidation = call__44.FMapValidation;
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
                    var call__45 = GetHost();
                    bool bool__46 = call__45;
                    if (bool__46)
                    {
                        var call__47 = hValidateSendMap();
                        bool__46 = call__47;
                    }

                    if (bool__46)
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
            TMemoryStream Stream = default;
            int I = default;
            int RemoteID = default;
            int J = default;
            var call__48 = FSettings.Count;
            I = call__48;
            RemoteID = 0;
            while (I != 0 && RemoteID == 0)
            {
                I -= 1;
                var call__49 = FSettings.GetSettings(I);
                bool bool__50 = call__49.FBehavior.PlayerType == 0;
                if (bool__50)
                {
                    var call__51 = GetRemote();
                    bool__50 = call__51;
                }

                bool bool__52 = bool__50;
                if (bool__52)
                {
                    var call__53 = FSettings.GetSettings(I);
                    bool__52 = call__53.FBehavior.Flags != FRemotePlayer.FRemoteID;
                }

                if (bool__52)
                {
                    var call__54 = FSettings.GetSettings(I);
                    var call__55 = FSettings.FRemotePlayers.IndexOfRemoteID(call__54.FBehavior.Flags);
                    J = call__55;
                    bool bool__56 = J != -1;
                    if (bool__56)
                    {
                        var call__57 = FSettings.FRemotePlayers.GetPlayers(J);
                        bool__56 = call__57.FMapValidation == TMapValidationInfo.mviInvalidMap;
                    }

                    if (bool__56)
                    {
                        var call__58 = FSettings.GetSettings(I);
                        RemoteID = call__58.FBehavior.Flags;
                    }
                }
            }

            if (RemoteID != 0)
            {
                Stream = new TMemoryStream();
                Stream.LoadFromFile(AoWEngine.StartupDirectory + FSettings.FMapName);
                if (FOnSendMap != null)
                {
                    var call__59 = Stream.Size;
                    FOnSendMap(this, Stream.Memory, call__59, RemoteID);
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
                var call__60 = FSettings.GetSettings(Index);
                var call__61 = FSettings.GetSettings(Index);
                call__61.FStatus = call__60.FStatus + TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssLoaded);
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
                var call__62 = FSettings.IndexOfRemoteID(FRemotePlayer.FRemoteID);
                I = call__62;
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
                {
                    I = 0;
                    int end__63 = FSettings.Count - 1;
                    for (; I <= end__63; I += 1)
                    {
                        var call__64 = FSettings.GetSettings(I);
                        if (call__64.FBehavior.PlayerType == 0)
                        {
                            var call__65 = FSettings.GetSettings(I);
                            var call__66 = FSettings.GetSettings(I);
                            call__66.FStatus = call__65.FStatus + TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssLoaded);
                        }
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
                var call__67 = FSettings.GetSettings(Index);
                call__67.FLeader.Assign(Leader, AoWEngine);
                var call__68 = FSettings.GetSettings(Index);
                var call__69 = FSettings.GetSettings(Index);
                call__69.FStatus = call__68.FStatus + TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssLeaderCustomized);
                if (!hValidateSetupMap())
                {
                    UpdateSettings();
                }
            }
        }

        // AoWSetup.pas:777
        public bool CustomizeLeaders()
        {
            bool Result = default;
            bool Found = default;
            int I = default;
            TCustomizeLeaderControl CLC = default;
            FPhase = TSetupPhase.spCustomizeLeaders;
            Result = false;
            Found = false;
            I = 0;
            while (true)
            {
                bool bool__70 = !Found;
                if (bool__70)
                {
                    var call__71 = FSettings.Count;
                    bool__70 = I != call__71;
                }

                if (!bool__70)
                    break;
                {
                    var call__72 = FSettings.GetSettings(I);
                    bool bool__73 = call__72.FBehavior.PlayerType == 0;
                    if (bool__73)
                    {
                        var call__74 = GetRemote();
                        bool bool__75 = !call__74;
                        if (!bool__75)
                        {
                            var call__76 = FSettings.GetSettings(I);
                            bool__75 = call__76.FBehavior.Flags == FRemotePlayer.FRemoteID;
                        }

                        bool__73 = bool__75;
                    }

                    bool bool__77 = bool__73;
                    if (bool__77)
                    {
                        var call__78 = FSettings.GetSettings(I);
                        bool__77 = !call__78.FStatus.Contains(TPlayerSetupStatusElement.pssLeaderCustomized);
                    }

                    Found = bool__77;
                    if (!Found)
                    {
                        I += 1;
                    }
                }
            }

            if (Found)
            {
                CLC = new TCustomizeLeaderControl(null);
                CLC.FSetupControl = this;
                CLC.FPlayerIndex = I;
                var call__79 = FSettings.GetSettings(I);
                CLC.FItems.Add(call__79.FRace);
                var call__80 = FSettings.GetSettings(I);
                CLC.FLeader.Assign(call__80.FLeader, AoWEngine);
                var call__81 = FSettings.GetSettings(I);
                CLC.FOriginalLeader.Assign(call__81.FLeader, AoWEngine);
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
                var call__82 = FSettings.GetSettings(Control.FPlayerIndex);
                call__82.FLeader.Assign(Control.FOriginalLeader, AoWEngine);
            }
            else
            {
                hLeaderCustomized(Control.FPlayerIndex, Control.FOriginalLeader);
            }

            if (FStatus.Contains(TSetupStatusElement.ssBusy))
            {
                var call__83 = FSettings.GetSettings(Control.FPlayerIndex);
                var call__84 = FSettings.GetSettings(Control.FPlayerIndex);
                call__84.FStatus = call__83.FStatus + TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssLeaderCustomized);
                CustomizeLeaders();
            }
        }

        // AoWSetup.pas:830
        public void ListPCCInfo(TPlayerCommunicationControl PCC)
        {
            int I = default;
            AnsiString S = default;
            {
                I = 0;
                int end__85 = PCC.Count - 1;
                for (; I <= end__85; I += 1)
                {
                    var call__86 = PCC.Players[I];
                    S = (AnsiString)"Player (" + IntToStr(I) + (AnsiString)") at system:" + IntToStr(call__86.SystemID) + (AnsiString)" CM:";
                    var call__87 = PCC.Players[I];
                    switch (call__87.ConnectionMode)
                    {
                        case TConnectionMode.cmGlobal:
                            S = S + (AnsiString)"Global";
                            break;
                        case TConnectionMode.cmLocal:
                            S = S + (AnsiString)"Local";
                            break;
                        case TConnectionMode.cmRemote:
                            S = S + (AnsiString)"Remote";
                            break;
                    }

                    AoWEngine.OutputDebugMessage(NetworkEngineDID, S);
                }
            }
        }

        // AoWSetup.pas:844
        public void RemoveIndependentPlayers()
        {
            int I = default;
            int J = default;
            TPlayer Player = default;
            {
                I = AoWHSMap.FPlayers.GetCount() - 1;
                byte end__88 = 1;
                for (; I >= end__88; I -= 1)
                {
                    var call__89 = AoWHSMap.FPlayers.GetPlayers(I);
                    Player = call__89;
                    if (Player.FPlayerType == TPlayerType.ptIndependent)
                    {
                        {
                            J = Player.FStructures.GetCount() - 1;
                            byte end__90 = 0;
                            for (; J >= end__90; J -= 1)
                            {
                                var call__91 = Player.FStructures.GetStructure(J);
                                call__91.SetPlayer(0);
                            }
                        }

                        {
                            J = Player.FArmies.GetCount() - 1;
                            byte end__92 = 0;
                            for (; J >= end__92; J -= 1)
                            {
                                var call__93 = Player.FArmies.GetArmy(J);
                                call__93.SetPlayer(0);
                            }
                        }

                        Player.FLeader.SetOwner(null);
                        Player.FStatus = TPlayerStatus.psTerminated;
                    }
                }
            }
        }

        // AoWSetup.pas:847
        public void SetupMap()
        {
            int I = default;
            int Index = default;
            TPlayer Player = default;
            TRemotePCC RemotePCC = default;
            TLocalPCC LocalPCC = default;
            FPhase = TSetupPhase.spSetupMap;
            {
                I = 0;
                int end__94 = FSettings.GetCount() - 1;
                for (; I <= end__94; I += 1)
                {
                    var call__95 = FSettings.GetSettings(I);
                    var call__96 = AoWHSMap.FPlayers.GetPlayers((sbyte)call__95.FPlayerID);
                    Player = call__96;
                    var call__97 = FSettings.GetSettings(I);
                    Player.FPlayerType = (TPlayerType)call__97.FBehavior.PlayerType;
                    var call__98 = FSettings.GetSettings(I);
                    Player.FBehavior = (TPlayerBehavior)call__98.FBehaviorMode;
                    if (FSettings.FConnectionType == 2)
                    {
                        var call__99 = AoWEngine.FRegistry.GetLocalEmailAddress();
                        var call__100 = FSettings.GetSettings(I);
                        if (CompareText(call__100.FEmailAddress, call__99) != 0)
                        {
                            var call__101 = FSettings.GetSettings(I);
                            Player.FPBEMSettings.FEmailGame.FEmailAddress = call__101.FEmailAddress;
                            Player.FPBEMSettings.FEmailGame.FName = FSettings.FEmailGameName;
                        }
                        else
                        {
                            var call__102 = FSettings.GetSettings(I);
                            var call__103 = AoWEngine.FEmailGames.CreateEmailGame(FSettings.FEmailGameName, call__102.FEmailAddress);
                            Player.FPBEMSettings.SetGame(call__103);
                        }
                    }

                    if (FSettings.FMapDay == 0)
                    {
                        Player.FLeader.UpdateFromLibraryBegin();
                        var call__104 = FSettings.GetSettings(I);
                        Player.FLeader.Assign(call__104.FLeader, AoWEngine);
                        Player.FLeader.UpdateFromLibraryEnd();
                    }

                    if (GetRemote())
                    {
                        var call__105 = FSettings.GetSettings(I);
                        if (call__105.FBehavior.PlayerType == 0 && (FSettings.FMapDay != 0 || !FSettings.FCustomizeLeaders))
                        {
                            var call__106 = FSettings.GetSettings(I);
                            var call__107 = FSettings.FRemotePlayers.IndexOfRemoteID(call__106.FBehavior.Flags);
                            Index = call__107;
                            if (Index != -1)
                            {
                                var call__108 = FSettings.FRemotePlayers.GetPlayers(Index);
                                Player.FLeader.FHeroName = call__108.FName;
                            }
                        }
                    }
                }
            }

            AoWHSMap.FConnectionType = (TPlayerConnectionType)FSettings.FConnectionType;
            if (GetRemote())
            {
                RemotePCC = new TRemotePCC();
                var call__109 = AoWHSMap.FPlayers.GetCount();
                RemotePCC.Setup(call__109, FRemotePlayer.FRemoteID);
                RemotePCC.SetConnectionMode(0, TConnectionMode.cmGlobal);
                RemotePCC.SetSystemID(0, FRemotePlayer.FRemoteID);
                {
                    I = 0;
                    int end__110 = FSettings.GetCount() - 1;
                    for (; I <= end__110; I += 1)
                    {
                        var call__111 = FSettings.GetSettings(I);
                        if (call__111.FBehavior.PlayerType != 0)
                        {
                            var call__112 = FSettings.GetSettings(I);
                            RemotePCC.SetConnectionMode((sbyte)call__112.FPlayerID, TConnectionMode.cmGlobal);
                            var call__113 = FSettings.GetSettings(I);
                            RemotePCC.SetSystemID((sbyte)call__113.FPlayerID, FRemotePlayer.FRemoteID);
                        }
                        else
                        {
                            var call__114 = FSettings.GetSettings(I);
                            if (call__114.FBehavior.Flags == FRemotePlayer.FRemoteID)
                            {
                                var call__115 = FSettings.GetSettings(I);
                                RemotePCC.SetConnectionMode((sbyte)call__115.FPlayerID, TConnectionMode.cmLocal);
                            }
                            else
                            {
                                var call__116 = FSettings.GetSettings(I);
                                RemotePCC.SetConnectionMode((sbyte)call__116.FPlayerID, TConnectionMode.cmRemote);
                            }

                            var call__117 = FSettings.GetSettings(I);
                            var call__118 = FSettings.GetSettings(I);
                            RemotePCC.SetSystemID((sbyte)call__118.FPlayerID, call__117.FBehavior.Flags);
                        }
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
                var call__119 = AoWHSMap.FPlayers.GetCount();
                LocalPCC.Setup(call__119);
                LocalPCC.SetConnectionMode(0, TConnectionMode.cmGlobal);
                {
                    I = 0;
                    int end__120 = FSettings.GetCount() - 1;
                    for (; I <= end__120; I += 1)
                    {
                        var call__121 = FSettings.GetSettings(I);
                        if (call__121.FBehavior.PlayerType == 0)
                        {
                            var call__122 = FSettings.GetSettings(I);
                            LocalPCC.SetConnectionMode((sbyte)call__122.FPlayerID, TConnectionMode.cmLocal);
                        }
                        else
                        {
                            var call__123 = FSettings.GetSettings(I);
                            LocalPCC.SetConnectionMode((sbyte)call__123.FPlayerID, TConnectionMode.cmGlobal);
                        }
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
                catch (System.Exception)
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
                catch (System.Exception)
                {
                }
            }
        }

        // AoWSetup.pas:858
        public bool hValidateSetupMap()
        {
            bool Result = default;
            int I = default;
            if (GetHost())
            {
                Result = true;
                var call__124 = FSettings.Count;
                I = call__124;
                while (Result && I != 0)
                {
                    I -= 1;
                    var call__125 = FSettings.GetSettings(I);
                    if (call__125.FBehavior.PlayerType == 0)
                    {
                        var call__126 = FSettings.GetSettings(I);
                        bool bool__127 = call__126.FStatus.Contains(TPlayerSetupStatusElement.pssLoaded);
                        if (bool__127)
                        {
                            bool bool__128 = !FSettings.FCustomizeLeaders;
                            if (!bool__128)
                            {
                                var call__129 = FSettings.GetSettings(I);
                                bool__128 = call__129.FStatus.Contains(TPlayerSetupStatusElement.pssLeaderCustomized);
                            }

                            bool__127 = bool__128;
                        }

                        Result = bool__127;
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
            bool bool__130 = FUpdateCount == 0;
            if (bool__130)
            {
                var call__131 = GetHost();
                bool__130 = call__131;
            }

            if (bool__130)
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
        public bool GetRemote()
        {
            bool Result = default;
            Result = FStatus.Contains(TSetupStatusElement.ssRemote);
            return Result;
        }

        // AoWSetup.pas:902
        public bool GetHost()
        {
            bool Result = default;
            Result = FSettings.FHostRemoteID == FRemotePlayer.FRemoteID;
            return Result;
        }

        // AoWSetup.pas:906
        public void Reset()
        {
            FPhase = TSetupPhase.spIdle;
            FStatus = TSet<TSetupStatusElement>.Of();
            FRemotePlayer.FName = (AnsiString)"";
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
        public AnsiString GetPlayerName()
        {
            AnsiString Result = default;
            Result = FRemotePlayer.FName;
            return Result;
        }

        // AoWSetup.pas:924
        public void SetPlayerName(AnsiString Value)
        {
            if (Value != FRemotePlayer.FName)
            {
                FRemotePlayer.FName = Value;
                SendHostMessage(3, FRemotePlayer, 0, 0);
            }
        }

        // AoWSetup.pas:932
        public sbyte GetPreferredRace()
        {
            sbyte Result = default;
            Result = (sbyte)FRemotePlayer.FPreferredRace;
            return Result;
        }

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
            var call__132 = GetHost();
            Assert(call__132, (AnsiString)"", "AoWSetup.pas", 1278);
            if (!GetRemote())
            {
                var call__133 = FSettings.GetSettings(Index);
                if (call__133.FOriginalBehavior.PlayerType != 0)
                {
                    var call__134 = FSettings.GetSettings(Index);
                    var call__135 = FSettings.GetSettings(Index);
                    call__135.FBehavior = call__134.FOriginalBehavior;
                }
                else
                {
                    var call__136 = FSettings.GetSettings(Index);
                    call__136.FBehavior = AoWSetup.CPUBehavior;
                }
            }
            else
            {
                var call__137 = FSettings.GetSettings(Index);
                call__137.FBehavior = AoWSetup.FreeBehavior;
            }

            var call__138 = FSettings.GetSettings(Index);
            var call__139 = FSettings.GetSettings(Index);
            call__139.FStatus = call__138.FStatus - TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssForced);
        }

        // AoWSetup.pas:1289
        public bool UpdatePreferredRaces()
        {
            bool Result = default;
            int I = default;
            int Target = default;
            int Old = default;
            int Race = default;
            bool Again = default;
            var call__140 = GetHost();
            Assert(call__140, (AnsiString)"", "AoWSetup.pas", 1292);
            Again = true;
            Result = false;
            while (Again)
            {
                Again = false;
                {
                    I = 0;
                    int end__141 = FSettings.FRemotePlayers.Count - 1;
                    for (; I <= end__141; I += 1)
                    {
                        var call__142 = FSettings.FRemotePlayers.GetPlayers(I);
                        Race = (sbyte)call__142.FPreferredRace;
                        if (Race != -1)
                        {
                            var call__143 = FSettings.IndexOfRace((byte)Race);
                            Target = call__143;
                            bool bool__144 = Target != -1;
                            if (bool__144)
                            {
                                var call__145 = FSettings.GetSettings(Target);
                                bool__144 = call__145.FBehavior.Flags == 0;
                            }

                            bool bool__146 = bool__144;
                            if (bool__146)
                            {
                                var call__147 = FSettings.GetSettings(Target);
                                bool__146 = (call__147.FStatus & TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssFixed, TPlayerSetupStatusElement.pssForced)) == TSet<TPlayerSetupStatusElement>.Of();
                            }

                            if (bool__146)
                            {
                                var call__148 = FSettings.FRemotePlayers.GetPlayers(I);
                                var call__149 = FSettings.IndexOfRemoteID(call__148.FRemoteID);
                                Old = call__149;
                                bool bool__150 = Old == -1;
                                if (!bool__150)
                                {
                                    var call__151 = FSettings.GetSettings(Old);
                                    bool__150 = !call__151.FStatus.Contains(TPlayerSetupStatusElement.pssForced);
                                }

                                if (bool__150)
                                {
                                    if (Old != -1)
                                    {
                                        ResetPlayerSettings(Old);
                                        Again = true;
                                    }

                                    var call__152 = FSettings.GetSettings(Target);
                                    call__152.FBehavior.PlayerType = 0;
                                    var call__153 = FSettings.FRemotePlayers.GetPlayers(I);
                                    var call__154 = FSettings.GetSettings(Target);
                                    call__154.FBehavior.Flags = call__153.FRemoteID;
                                    Result = true;
                                }
                            }
                        }
                        else
                        {
                            var call__155 = FSettings.FRemotePlayers.GetPlayers(I);
                            var call__156 = FSettings.IndexOfRemoteID(call__155.FRemoteID);
                            Old = call__156;
                            bool bool__157 = Old != -1;
                            if (bool__157)
                            {
                                var call__158 = FSettings.GetSettings(Old);
                                bool__157 = !call__158.FStatus.Contains(TPlayerSetupStatusElement.pssForced);
                            }

                            if (bool__157)
                            {
                                ResetPlayerSettings(Old);
                                Again = true;
                            }
                        }
                    }
                }
            }

            return Result;
        }

        // AoWSetup.pas:944
        public void ListAvailablePlayers(TStrings List)
        {
            int I = default;
            if (GetRemote())
            {
                var call__159 = TranslateRStr(TPlayerTypeRStr[5]);
                List.Add(call__159);
                {
                    I = 0;
                    int end__160 = FSettings.FRemotePlayers.Count - 1;
                    for (; I <= end__160; I += 1)
                    {
                        var call__161 = FSettings.FRemotePlayers.GetPlayers(I);
                        List.Add(call__161.FName);
                    }
                }

                var call__162 = TranslateRStr(TPlayerTypeRStr[1]);
                List.Add(call__162);
                var call__163 = TranslateRStr(TPlayerTypeRStr[2]);
                List.Add(call__163);
                var call__164 = TranslateRStr(TPlayerTypeRStr[3]);
                List.Add(call__164);
                var call__165 = TranslateRStr(TPlayerTypeRStr[6]);
                List.Add(call__165);
                var call__166 = TranslateRStr(TPlayerTypeRStr[7]);
                List.Add(call__166);
                var call__167 = TranslateRStr(TPlayerTypeRStr[4]);
                List.Add(call__167);
            }
            else
            {
                var call__168 = TranslateRStr(TPlayerTypeRStr[0]);
                List.Add(call__168);
                var call__169 = TranslateRStr(TPlayerTypeRStr[1]);
                List.Add(call__169);
                var call__170 = TranslateRStr(TPlayerTypeRStr[2]);
                List.Add(call__170);
                var call__171 = TranslateRStr(TPlayerTypeRStr[3]);
                List.Add(call__171);
                var call__172 = TranslateRStr(TPlayerTypeRStr[6]);
                List.Add(call__172);
                var call__173 = TranslateRStr(TPlayerTypeRStr[7]);
                List.Add(call__173);
                var call__174 = TranslateRStr(TPlayerTypeRStr[4]);
                List.Add(call__174);
            }
        }

        // AoWSetup.pas:970
        public AnsiString ValidatePlayerName(AnsiString Name)
        {
            AnsiString Result = default;
            bool Done = default;
            TStringList List = default;
            int I = default;
            bool Unique = default;
            List = new TStringList();
            ListAvailablePlayers(List);
            Done = false;
            Result = Name;
            while (!Done)
            {
                I = 0;
                Unique = true;
                while (true)
                {
                    var call__175 = List.Count;
                    if (!(I != call__175 && Unique))
                        break;
                    {
                        var call__176 = List[I];
                        Unique = CompareText(Result, call__176) != 0;
                        I += 1;
                    }
                }

                if (!Unique)
                {
                    // { Original reads the last character without guarding an empty name. }
                    if (TSet<byte>.WithBounds(0, 255).WithRange(49, 56).Contains(Result[Result.Length]))
                    {
                        Result = Copy(Result, 1, Result.Length - 1) + IntToStr(Result[Result.Length] + 1 - 48);
                    }
                    else
                    {
                        Result = Result + AnsiString.FromBytes(49);
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
        public bool PlayerNameToPlayerID(AnsiString Name, ref TSetupPlayerBehavior Behavior)
        {
            bool Result = default;
            int I = default;
            byte PlayerType = default;
            Result = false;
            var call__177 = FSettings.FRemotePlayers.Count;
            I = call__177;
            while (!Result && I != 0)
            {
                I -= 1;
                var call__178 = FSettings.FRemotePlayers.GetPlayers(I);
                if (CompareText(call__178.FName, Name) == 0)
                {
                    Result = true;
                    Behavior.PlayerType = 0;
                    var call__179 = FSettings.FRemotePlayers.GetPlayers(I);
                    Behavior.Flags = call__179.FRemoteID;
                }
            }

            if (!Result)
            {
                PlayerType = 0;
                while (!Result && PlayerType <= 7)
                {
                    var call__180 = TranslateRStr(TPlayerTypeRStr[PlayerType]);
                    if (CompareText(Name, call__180) == 0)
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
            bool Result = default;
            int I = default;
            int J = default;
            int PlayerCount = default;
            bool Human = default;
            Human = false;
            PlayerCount = 0;
            var call__181 = FSettings.Count;
            Result = call__181 != 0;
            var call__182 = FSettings.Count;
            I = call__182;
            while (Result && I != 0)
            {
                I -= 1;
                var call__183 = FSettings.GetSettings(I);
                Result = call__183.FBehavior.PlayerType != 5;
                var call__184 = FSettings.GetSettings(I);
                if (TSet<byte>.WithBounds(0, 255).With(6).With(7).WithRange(0, 3).Contains(call__184.FBehavior.PlayerType))
                {
                    PlayerCount += 1;
                }

                bool bool__185 = Human;
                if (!bool__185)
                {
                    var call__186 = FSettings.GetSettings(I);
                    bool__185 = call__186.FBehavior.PlayerType == 0;
                }

                Human = bool__185;
                bool bool__187 = Result;
                if (bool__187)
                {
                    var call__188 = FSettings.GetSettings(I);
                    bool__187 = call__188.FBehavior.PlayerType == 0;
                }

                bool bool__189 = bool__187;
                if (bool__189)
                {
                    var call__190 = GetRemote();
                    bool__189 = call__190;
                }

                if (bool__189)
                {
                    var call__191 = FSettings.GetSettings(I);
                    var call__192 = FSettings.FRemotePlayers.IndexOfRemoteID(call__191.FBehavior.Flags);
                    J = call__192;
                    bool bool__193 = J != -1;
                    if (bool__193)
                    {
                        var call__194 = FSettings.FRemotePlayers.GetPlayers(J);
                        bool__193 = TSet<TMapValidationInfo>.Of(TMapValidationInfo.mviInvalidMap, TMapValidationInfo.mviValid).Contains(call__194.FMapValidation);
                    }

                    Result = bool__193;
                }
            }

            Result = Result && Human && PlayerCount > 1;
            return Result;
        }

        // AoWSetup.pas:1048
        public bool GetAllMapsValid()
        {
            bool Result = default;
            int I = default;
            int J = default;
            var call__195 = FSettings.Count;
            I = call__195;
            Result = true;
            while (Result && I != 0)
            {
                I -= 1;
                Result = true;
                bool bool__196 = Result;
                if (bool__196)
                {
                    var call__197 = FSettings.GetSettings(I);
                    bool__196 = call__197.FBehavior.PlayerType == 0;
                }

                bool bool__198 = bool__196;
                if (bool__198)
                {
                    var call__199 = GetRemote();
                    bool__198 = call__199;
                }

                if (bool__198)
                {
                    var call__200 = FSettings.GetSettings(I);
                    var call__201 = FSettings.FRemotePlayers.IndexOfRemoteID(call__200.FBehavior.Flags);
                    J = call__201;
                    if (J != -1)
                    {
                        var call__202 = FSettings.FRemotePlayers.GetPlayers(J);
                        Result = call__202.FMapValidation == TMapValidationInfo.mviValid;
                    }
                }
            }

            return Result;
        }

        // AoWSetup.pas:1064
        public bool hPlayerSetupDone()
        {
            bool Result = default;
            int I = default;
            var call__203 = GetHost();
            bool bool__204 = call__203;
            if (bool__204)
            {
                var call__205 = GetPlayerSetupValid();
                bool__204 = call__205;
            }

            if (bool__204)
            {
                if (FOnPlayerSetupDone != null)
                {
                    FOnPlayerSetupDone(this);
                }

                if (GetRemote())
                {
                    {
                        I = FSettings.FRemotePlayers.Count - 1;
                        byte end__206 = 0;
                        for (; I >= end__206; I -= 1)
                        {
                            var call__207 = FSettings.FRemotePlayers.GetPlayers(I);
                            var call__208 = FSettings.IndexOfRemoteID(call__207.FRemoteID);
                            if (call__208 == -1)
                            {
                                if (FOnEjectPlayer != null)
                                {
                                    var call__209 = FSettings.FRemotePlayers.GetPlayers(I);
                                    FOnEjectPlayer(this, call__209.FRemoteID);
                                }

                                var call__210 = FSettings.FRemotePlayers.GetPlayers(I);
                                call__210.Free();
                            }
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
            var call__211 = GetHost();
            bool bool__212 = call__211;
            if (bool__212)
            {
                var call__213 = FSettings.GetSettings(Index);
                bool__212 = Value != call__213.FBehaviorMode;
            }

            if (bool__212)
            {
                var call__214 = FSettings.GetSettings(Index);
                call__214.FBehaviorMode = Value;
                UpdateSettings();
            }
        }

        // AoWSetup.pas:1093
        public void hSetExplorationEnabled(bool Value)
        {
            var call__215 = GetHost();
            if (call__215 && Value != FSettings.FExplorationEnabled && FSettings.FMapDay == 0)
            {
                FSettings.FExplorationEnabled = Value;
                UpdateSettings();
            }
        }

        // AoWSetup.pas:1101
        public void hSetLeadersOnMap(bool Value)
        {
            var call__216 = GetHost();
            if (call__216 && Value != FSettings.FLeadersOnMap && FSettings.FMapDay == 0)
            {
                FSettings.FLeadersOnMap = Value;
                UpdateSettings();
            }
        }

        // AoWSetup.pas:1109
        public void hSetTacticalCombatAvailability(byte Value)
        {
            var call__217 = GetHost();
            if (call__217 && Value != FSettings.FTacticalCombatAvailability)
            {
                FSettings.FTacticalCombatAvailability = Value;
                UpdateSettings();
            }
        }

        // AoWSetup.pas:1117
        public void hSetAlwaysViewTC(bool Value)
        {
            var call__218 = GetHost();
            if (call__218 && Value != FSettings.FAlwaysViewTC)
            {
                FSettings.FAlwaysViewTC = Value;
                UpdateSettings();
            }
        }

        // AoWSetup.pas:1125
        public void hSetAlliedVictory(bool Value)
        {
            var call__219 = GetHost();
            if (call__219 && Value != FSettings.FAlliedVictory && FSettings.FMapDay == 0)
            {
                FSettings.FAlliedVictory = Value;
                UpdateSettings();
            }
        }

        // AoWSetup.pas:1133
        public void hSetCustomizeLeaders(bool Value)
        {
            var call__220 = GetHost();
            if (call__220 && Value != FSettings.FCustomizeLeaders && FSettings.FMapDay == 0)
            {
                FSettings.FCustomizeLeaders = Value;
                UpdateSettings();
            }
        }

        // AoWSetup.pas:1141
        public void hSetTurnLimitation(int Value)
        {
            var call__221 = GetHost();
            if (call__221 && Value != FSettings.FDayLimit)
            {
                FSettings.FDayLimit = Value;
                UpdateSettings();
            }
        }

        // AoWSetup.pas:1149
        public void hSetTurnTimeLimitation(int Value)
        {
            var call__222 = GetHost();
            if (call__222 && Value != FSettings.FTurnTime)
            {
                FSettings.FTurnTime = Value;
                UpdateSettings();
            }
        }

        // AoWSetup.pas:1157
        public void hSetPlayerID(int Index, in TSetupPlayerBehavior Behavior, bool Forced)
        {
            int I = default;
            int PlayerIndex = default;
            if (GetHost())
            {
                PlayerIndex = Index;
                var call__223 = FSettings.GetSettings(PlayerIndex);
                bool bool__224 = call__223.FBehavior.PlayerType == Behavior.PlayerType;
                if (bool__224)
                {
                    var call__225 = FSettings.GetSettings(PlayerIndex);
                    bool__224 = call__225.FBehavior.Flags == Behavior.Flags;
                }

                if (bool__224)
                {
                    return;
                }

                bool bool__226 = Behavior.Flags != 0;
                if (bool__226)
                {
                    var call__227 = FSettings.FRemotePlayers.IndexOfRemoteID(Behavior.Flags);
                    bool__226 = call__227 == -1;
                }

                if (bool__226)
                {
                    return;
                }

                var call__228 = GetRemote();
                if (!call__228 && FSettings.FPlayMode == 2 && Behavior.PlayerType == 0)
                {
                    {
                        I = 0;
                        int end__229 = FSettings.Count - 1;
                        for (; I <= end__229; I += 1)
                        {
                            var call__230 = FSettings.GetSettings(I);
                            if (call__230.FBehavior.PlayerType == 0)
                            {
                                ResetPlayerSettings(I);
                            }
                        }
                    }
                }

                var call__231 = GetRemote();
                if (call__231 && Behavior.PlayerType == 0 && Behavior.Flags != 0)
                {
                    var call__232 = FSettings.IndexOfRemoteID(Behavior.Flags);
                    I = call__232;
                    if (I != -1)
                    {
                        ResetPlayerSettings(I);
                    }
                }

                ResetPlayerSettings(PlayerIndex);
                var call__233 = FSettings.GetSettings(PlayerIndex);
                call__233.FBehavior = Behavior;
                if (Forced && Behavior.PlayerType != 5)
                {
                    var call__234 = FSettings.GetSettings(PlayerIndex);
                    var call__235 = FSettings.GetSettings(PlayerIndex);
                    call__235.FStatus = call__234.FStatus + TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssForced);
                }
                else
                {
                    var call__236 = FSettings.GetSettings(PlayerIndex);
                    var call__237 = FSettings.GetSettings(PlayerIndex);
                    call__237.FStatus = call__236.FStatus - TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssForced);
                }

                UpdatePreferredRaces();
                UpdateSettings();
            }
        }

        // AoWSetup.pas:1182
        public void hSetPlayMode(byte Value)
        {
            int I = default;
            bool First = default;
            var call__238 = GetHost();
            if (call__238 && Value != FSettings.FPlayMode)
            {
                FSettings.FPlayMode = Value;
                if (!GetRemote() && Value == 2)
                {
                    First = true;
                    {
                        I = 0;
                        int end__239 = FSettings.Count - 1;
                        for (; I <= end__239; I += 1)
                        {
                            var call__240 = FSettings.GetSettings(I);
                            if (call__240.FBehavior.PlayerType == 0)
                            {
                                if (!First)
                                {
                                    ResetPlayerSettings(I);
                                }

                                First = false;
                            }
                        }
                    }
                }

                UpdateSettings();
            }
        }

        // AoWSetup.pas:1201
        public void hSetIndependentAIMode(byte Value)
        {
            var call__241 = GetHost();
            if (call__241 && Value != FSettings.FIndependentAIMode)
            {
                FSettings.FIndependentAIMode = Value;
                UpdateSettings();
            }
        }

        // AoWSetup.pas:1209
        public bool hPickMap(AnsiString FileName)
        {
            bool Result = default;
            bool FirstHuman = default;
            TPlayerSetupSettings Settings = default;
            TAoWHSMap Preview = default;
            int I = default;
            var call__242 = AoWEngine.CreatePreviewHSM(AoWEngine.StartupDirectory + FileName);
            Preview = (TAoWHSMap)call__242;
            var call__243 = GetHost();
            if (call__243 && Preview != null && (!AoWEngine.FDemo || Preview.FShowAllPlayers) && Preview.FConnectionType != TPlayerConnectionType.pcEmail)
            {
                AoWEngine.InitializeHSSet(AoWEngine.StartupDirectory + Preview.HSSFileName, null);
                Randomize();
                FSettings.Clear();
                FSettings.FMapName = FileName;
                FSettings.FMapDay = Preview.FDay;
                FSettings.FFileName = Preview.FMapName;
                var call__244 = GetFileCRC32(AoWEngine.StartupDirectory + FileName);
                FSettings.FMapCRC = call__244;
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
                {
                    I = 0;
                    int end__245 = FSettings.FRemotePlayers.GetCount() - 1;
                    for (; I <= end__245; I += 1)
                    {
                        var call__246 = FSettings.FRemotePlayers.GetPlayers(I);
                        call__246.FPreferredRace = 255;
                        var call__247 = FSettings.FRemotePlayers.GetPlayers(I);
                        if (call__247.FRemoteID == FRemotePlayer.FRemoteID)
                        {
                            var call__248 = FSettings.FRemotePlayers.GetPlayers(I);
                            call__248.FMapValidation = TMapValidationInfo.mviValid;
                        }
                        else
                        {
                            var call__249 = FSettings.FRemotePlayers.GetPlayers(I);
                            call__249.FMapValidation = TMapValidationInfo.mviUnknown;
                        }
                    }
                }

                {
                    I = 1;
                    int end__250 = Preview.FPlayers.GetCount() - 1;
                    for (; I <= end__250; I += 1)
                    {
                        var player = Preview.FPlayers.GetPlayers(I);
                        if (player.FStatus != TPlayerStatus.psTerminated)
                        {
                            var call__252 = new TPlayerSetupSettings(this.FSettings);
                            Settings = call__252;
                            Settings.FPlayerID = (byte)I;
                            Settings.FRace = player.FPlayerRace;
                            Settings.FLeader.Assign(player.FLeaderSetup, AoWEngine);
                            Settings.FBehaviorMode = (byte)(int)player.FBehavior;
                            if (player.FParticipating)
                            {
                                Settings.FStatus = Settings.FStatus + TSet<TPlayerSetupStatusElement>.Of(TPlayerSetupStatusElement.pssFixed);
                            }

                            var call__253 = GetRemote();
                            if (call__253 && !player.FParticipating)
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
            int I = default;
            int J = default;
            var call__254 = GetHost();
            bool bool__255 = call__254;
            if (bool__255)
            {
                var call__256 = GetRemote();
                bool__255 = call__256;
            }

            if (bool__255 && RemoteID != 0)
            {
                var call__257 = FSettings.FRemotePlayers.IndexOfRemoteID(RemoteID);
                I = call__257;
                if (I != -1)
                {
                    var call__258 = FSettings.FRemotePlayers.GetPlayers(I);
                    call__258.Free();
                    if (FOnEjectPlayer != null)
                    {
                        FOnEjectPlayer(this, RemoteID);
                    }

                    var call__259 = FSettings.IndexOfRemoteID(RemoteID);
                    J = call__259;
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
        public bool SetupHost(AnsiString Name, byte Mode, int RemoteID)
        {
            bool Result = default;
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

            var call__261 = ValidatePlayerName(Name);
            FRemotePlayer.FName = call__261;
            this.FRemotePlayer.FRemoteID = RemoteID;
            this.FRemotePlayer.FPreferredRace = 255;
            this.FRemotePlayer.FVersion = AoWEngine.FVersion;

            FStatus = FStatus + TSet<TSetupStatusElement>.Of(TSetupStatusElement.ssBusy);
            if (RemoteID != 0)
            {
                hConnectPlayer(FRemotePlayer);
                FStatus = FStatus + TSet<TSetupStatusElement>.Of(TSetupStatusElement.ssRemote);
            }

            Result = true;
            return Result;
        }

        // AoWSetup.pas:1264
        public bool SetupUser(AnsiString Name, int RemoteID)
        {
            bool Result = default;
            Reset();
            FRemotePlayer.FName = Name;
            FRemotePlayer.FRemoteID = RemoteID;
            FRemotePlayer.FPreferredRace = 255;
            FRemotePlayer.FVersion = AoWEngine.FVersion;
            FStatus = FStatus + TSet<TSetupStatusElement>.Of(TSetupStatusElement.ssBusy, TSetupStatusElement.ssRemote);
            SendHostMessage(1, FRemotePlayer, 0, 0);
            FPhase = TSetupPhase.spSetupPlayers;
            Result = true;
            return Result;
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
    public delegate void TLoadMapEvent(TSetupControl SetupControl, AnsiString Filename);
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
        public AnsiString Name;
    }

    public static partial class AoWSetup
    {
        // AoWSetup.pas:1334
        public static void RegisterSetupClasses(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TSetupSettings>(), EClass.Of<TPlayerSetupSettings>(), EClass.Of<TSetupPackage>(), EClass.Of<TRemotePlayer>()]);
        }

        // AoWSetup.pas:1339
        public static void Register()
        {
            RegisterComponents((AnsiString)"AoW", [typeof(TSetupControl)]);
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
}
