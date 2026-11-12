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
    public PlayerSetupSettings this[int index] => (PlayerSetupSettings)_children.List[index];

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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwString(ref FileName, 10);
        stream.rwData(Bytes.Of(ref PlayMode, 1), 1, 11);
        stream.rwBool(ref ExplorationEnabled, 12);
        stream.rwString(ref MapName, 13);
        stream.rwData(Bytes.Of(ref VictoryCondition, 1), 1, 14);
        stream.rwData(Bytes.Of(ref IndependentAIMode, 1), 1, 15);
        stream.rwBool(ref CustomizeLeaders, 16);
        stream.rwInteger(ref RandomSeed, 17);
        stream.rwBool(ref LeadersOnMap, 18);
        stream.rwInteger(ref DayLimit, 19);
        stream.rwInteger(ref TurnTime, 20);
        stream.rwEObject(RemotePlayers, 21);
        stream.rwInteger(ref HostRemoteId, 22);
        stream.rwInteger(ref MapDay, 23);
        stream.rwBool(ref AlliedVictory, 24);
        stream.rwData(Bytes.Of(ref ConnectionType, 1), 1, 25);
        stream.rwInteger(ref MapCrc, 26);
        stream.rwInteger(ref SetCrc, 27);
        stream.rwData(Bytes.Of(ref TacticalBattleAvailability, 1), 1, 28);
        stream.rwBool(ref AlwaysViewTacticalBattle, 29);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.String("FileName", ref FileName);
        persister.RawByte("PlayMode", ref PlayMode);
        persister.Boolean("ExplorationEnabled", ref ExplorationEnabled);
        persister.String("MapName", ref MapName);
        persister.RawByte("VictoryCondition", ref VictoryCondition);
        persister.RawByte("IndependentAIMode", ref IndependentAIMode);
        persister.Boolean("CustomizeLeaders", ref CustomizeLeaders);
        persister.Int32("RandomSeed", ref RandomSeed);
        persister.Boolean("LeadersOnMap", ref LeadersOnMap);
        persister.Int32("DayLimit", ref DayLimit);
        persister.Int32("TurnTime", ref TurnTime);
        persister.Object("RemotePlayers", RemotePlayers);
        persister.Int32("HostRemoteID", ref HostRemoteId);
        persister.Int32("MapDay", ref MapDay);
        persister.Boolean("AlliedVictory", ref AlliedVictory);
        persister.RawByte("ConnectionType", ref ConnectionType);
        persister.Int32("MapCRC", ref MapCrc);
        persister.Int32("SetCRC", ref SetCrc);
        persister.RawByte("TacticalCombatAvailability", ref TacticalBattleAvailability);
        persister.Boolean("AlwaysViewTC", ref AlwaysViewTacticalBattle);
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
        if (index >= 0 && index < _children.Count)
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
        while (result == -1 && I != _children.Count)
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
        while (result == -1 && I != _children.Count)
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
        while (result == -1 && I != _children.Count)
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
