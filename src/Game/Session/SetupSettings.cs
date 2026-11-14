// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

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

    public int HostRemoteId { get; set; }
    public string FileName { get; set; } = "";
    public string MapName { get; set; } = "";
    public int MapCrc { get; set; }
    public int SetCrc { get; set; }
    public int MapDay { get; set; }
    public byte PlayMode { get; set; }
    public bool ExplorationEnabled { get; set; }
    public bool LeadersOnMap { get; set; }
    public bool AlliedVictory { get; set; }
    public byte VictoryCondition { get; set; }
    public byte IndependentAIMode { get; set; }
    public bool CustomizeLeaders { get; set; }
    public int RandomSeed { get; set; }
    public int DayLimit { get; set; }
    public int TurnTime { get; set; }

    public RemotePlayerList RemotePlayers { get; }

    public byte ConnectionType { get; set; }
    public byte TacticalBattleAvailability { get; set; }
    public bool AlwaysViewTacticalBattle { get; set; }

    public string EmailGameName { get; set; } = "";

    // AoWSetup.pas:334
    public override void Dispose()
    {
        RemotePlayers?.Dispose();
        base.Dispose();
    }

    // AoWSetup.pas:402
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        FileName = persister.String("FileName", value: FileName);
        PlayMode = persister.RawByte("PlayMode", value: PlayMode);
        ExplorationEnabled = persister.Boolean("ExplorationEnabled", value: ExplorationEnabled);
        MapName = persister.String("MapName", value: MapName);
        VictoryCondition = persister.RawByte("VictoryCondition", value: VictoryCondition);
        IndependentAIMode = persister.RawByte("IndependentAIMode", value: IndependentAIMode);
        CustomizeLeaders = persister.Boolean("CustomizeLeaders", value: CustomizeLeaders);
        RandomSeed = persister.Int32("RandomSeed", value: RandomSeed);
        LeadersOnMap = persister.Boolean("LeadersOnMap", value: LeadersOnMap);
        DayLimit = persister.Int32("DayLimit", value: DayLimit);
        TurnTime = persister.Int32("TurnTime", value: TurnTime);
        persister.Object("RemotePlayers", objectValue: RemotePlayers);
        HostRemoteId = persister.Int32("HostRemoteId", value: HostRemoteId);
        MapDay = persister.Int32("MapDay", value: MapDay);
        AlliedVictory = persister.Boolean("AlliedVictory", value: AlliedVictory);
        ConnectionType = persister.RawByte("ConnectionType", value: ConnectionType);
        MapCrc = persister.Int32("MapCrc", value: MapCrc);
        SetCrc = persister.Int32("SetCrc", value: SetCrc);
        TacticalBattleAvailability = persister.RawByte("TacticalBattleAvailability", value: TacticalBattleAvailability);
        AlwaysViewTacticalBattle = persister.Boolean("AlwaysViewTacticalBattle", value: AlwaysViewTacticalBattle);
    }

    // AoWSetup.pas:327
    public SetupSettings(ECustomNode owner) : base(owner)
    {
        RemotePlayers = new RemotePlayerList(null) { Holder = CreationHolder() };
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
