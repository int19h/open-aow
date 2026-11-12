// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Game.Session;

// CRCs, day, play mode, victory, independent AI, seed, limits, players, connection, tactical
// combat, email game name
public partial class SetupSettings : ENode, IChildList<PlayerSetupSettings>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a PlayerSetupSettings, so the cast never fails.
    public PlayerSetupSettings this[int index] => (PlayerSetupSettings)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is PlayerSetupSettings;

    int IChildSink<PlayerSetupSettings>.AddChild(PlayerSetupSettings child) => AddChild(child);

    // AoWSetup.pas:356
    public new const int ClassId = 131680;

    public int HostRemoteId;
    public string FileName = "";
    public string MapName = "";
    public int MapCrc;
    public int SetCrc;
    public int MapDay;
    public byte PlayMode;
    public bool ExplorationEnabled;
    public bool LeadersOnMap;
    public bool AlliedVictory;
    public byte VictoryCondition;
    public byte IndependentAIMode;
    public bool CustomizeLeaders;
    public int RandomSeed;
    public int DayLimit;
    public int TurnTime;

    public RemotePlayerList RemotePlayers { get; }

    public byte ConnectionType;
    public byte TacticalBattleAvailability;
    public bool AlwaysViewTacticalBattle;

    public string EmailGameName { get; set; } = "";

    // AoWSetup.pas:334
    public override void Dispose()
    {
        RemotePlayers?.Dispose();
        base.Dispose();
    }

    // AoWSetup.pas:402
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.String(ref FileName, 10);
        stream.Bytes(Bytes.Of(ref PlayMode, 1), 1, 11);
        stream.Boolean(ref ExplorationEnabled, 12);
        stream.String(ref MapName, 13);
        stream.Bytes(Bytes.Of(ref VictoryCondition, 1), 1, 14);
        stream.Bytes(Bytes.Of(ref IndependentAIMode, 1), 1, 15);
        stream.Boolean(ref CustomizeLeaders, 16);
        stream.Int32(ref RandomSeed, 17);
        stream.Boolean(ref LeadersOnMap, 18);
        stream.Int32(ref DayLimit, 19);
        stream.Int32(ref TurnTime, 20);
        stream.Object(RemotePlayers, 21);
        stream.Int32(ref HostRemoteId, 22);
        stream.Int32(ref MapDay, 23);
        stream.Boolean(ref AlliedVictory, 24);
        stream.Bytes(Bytes.Of(ref ConnectionType, 1), 1, 25);
        stream.Int32(ref MapCrc, 26);
        stream.Int32(ref SetCrc, 27);
        stream.Bytes(Bytes.Of(ref TacticalBattleAvailability, 1), 1, 28);
        stream.Boolean(ref AlwaysViewTacticalBattle, 29);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        FileName = persister.String("FileName", FileName);
        PlayMode = persister.RawByte("PlayMode", PlayMode);
        ExplorationEnabled = persister.Boolean("ExplorationEnabled", ExplorationEnabled);
        MapName = persister.String("MapName", MapName);
        VictoryCondition = persister.RawByte("VictoryCondition", VictoryCondition);
        IndependentAIMode = persister.RawByte("IndependentAIMode", IndependentAIMode);
        CustomizeLeaders = persister.Boolean("CustomizeLeaders", CustomizeLeaders);
        RandomSeed = persister.Int32("RandomSeed", RandomSeed);
        LeadersOnMap = persister.Boolean("LeadersOnMap", LeadersOnMap);
        DayLimit = persister.Int32("DayLimit", DayLimit);
        TurnTime = persister.Int32("TurnTime", TurnTime);
        persister.Object("RemotePlayers", RemotePlayers);
        HostRemoteId = persister.Int32("HostRemoteID", HostRemoteId);
        MapDay = persister.Int32("MapDay", MapDay);
        AlliedVictory = persister.Boolean("AlliedVictory", AlliedVictory);
        ConnectionType = persister.RawByte("ConnectionType", ConnectionType);
        MapCrc = persister.Int32("MapCRC", MapCrc);
        SetCrc = persister.Int32("SetCRC", SetCrc);
        TacticalBattleAvailability = persister.RawByte("TacticalCombatAvailability", TacticalBattleAvailability);
        AlwaysViewTacticalBattle = persister.Boolean("AlwaysViewTC", AlwaysViewTacticalBattle);
    }

    // AoWSetup.pas:327
    public SetupSettings(ECustomNode owner) : base(owner)
    {
        RemotePlayers = new RemotePlayerList(null);
        Reset();
    }

    // AoWSetup.pas:340
    public void Reset()
    {
        Clear();
        TacticalBattleAvailability = 1;
        CustomizeLeaders = false;
        TurnTime = 0;
        DayLimit = 0;
        PlayMode = 2;
        ExplorationEnabled = true;
        LeadersOnMap = true;
        RemotePlayers.Clear();
        MapName = "";
        FileName = "";
        AlwaysViewTacticalBattle = false;
    }

    // AoWSetup.pas:359
    public PlayerSetupSettings GetSettings(int index)
    {
        PlayerSetupSettings result;
        if (index >= 0 && index < Count)
        {
            result = this[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // AoWSetup.pas:366
    public int IndexOfPlayer(byte player)
    {
        int I;
        int result = -1;
        I = 0;
        while (result == -1 && I != Count)
        {
            if (this[I].PlayerId == player)
            {
                result = I;
            }

            I += 1;
        }

        return result;
    }

    // AoWSetup.pas:378
    public int IndexOfRemoteId(int remoteId)
    {
        int I;
        int result = -1;
        I = 0;
        while (result == -1 && I != Count)
        {
            if (this[I].Behavior.Flags == remoteId)
            {
                result = I;
            }

            I += 1;
        }

        return result;
    }

    // AoWSetup.pas:390
    public int IndexOfRace(byte race)
    {
        int I;
        int result = -1;
        I = 0;
        while (result == -1 && I != Count)
        {
            if (race == (byte)this[I].Race)
            {
                result = I;
            }

            I += 1;
        }

        return result;
    }

}
