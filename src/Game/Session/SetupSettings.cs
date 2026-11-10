// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Game.Session;

// CRCs, day, play mode, victory, independent AI, seed, limits, players, connection, tactical
// combat, email game name
public partial class SetupSettings : ENode
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
    public RemotePlayerList FRemotePlayers;
    public byte FConnectionType;
    public byte FTacticalCombatAvailability;
    public bool FAlwaysViewTC;
    public string FEmailGameName = "";

    // AoWSetup.pas:334
    public override void Dispose()
    {
        FRemotePlayers?.Dispose();
        base.Dispose();
    }

    // AoWSetup.pas:402
    public override void ReadWrite(EStorageStream Stream)
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

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.String("FileName", ref FFileName);
        persister.RawByte("PlayMode", ref FPlayMode);
        persister.Bool("ExplorationEnabled", ref FExplorationEnabled);
        persister.String("MapName", ref FMapName);
        persister.RawByte("VictoryCondition", ref FVictoryCondition);
        persister.RawByte("IndependentAIMode", ref FIndependentAIMode);
        persister.Bool("CustomizeLeaders", ref FCustomizeLeaders);
        persister.Int("RandomSeed", ref FRandomSeed);
        persister.Bool("LeadersOnMap", ref FLeadersOnMap);
        persister.Int("DayLimit", ref FDayLimit);
        persister.Int("TurnTime", ref FTurnTime);
        persister.Object("RemotePlayers", FRemotePlayers);
        persister.Int("HostRemoteID", ref FHostRemoteID);
        persister.Int("MapDay", ref FMapDay);
        persister.Bool("AlliedVictory", ref FAlliedVictory);
        persister.RawByte("ConnectionType", ref FConnectionType);
        persister.Int("MapCRC", ref FMapCRC);
        persister.Int("SetCRC", ref FSetCRC);
        persister.RawByte("TacticalCombatAvailability", ref FTacticalCombatAvailability);
        persister.Bool("AlwaysViewTC", ref FAlwaysViewTC);
    }

    // AoWSetup.pas:327
    public SetupSettings(ECustomNode owner) : base(owner)
    {
        FRemotePlayers = new RemotePlayerList(null);
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
    public PlayerSetupSettings GetSettings(int Index)
    {
        PlayerSetupSettings Result;
        if (Index >= 0 && Index < FChildren.Count)
        {
            Result = (PlayerSetupSettings)FChildren.List[Index];
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
            if (((PlayerSetupSettings)FChildren.List[I]).FPlayerID == Player)
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
            if (((PlayerSetupSettings)FChildren.List[I]).FBehavior.Flags == RemoteID)
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
            if (Race == (byte)((PlayerSetupSettings)FChildren.List[I]).FRace)
            {
                Result = I;
            }

            I += 1;
        }

        return Result;
    }

}
