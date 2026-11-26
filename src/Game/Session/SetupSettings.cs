#nullable disable warnings
// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Game.Session;

// CRCs, day, play mode, victory, independent AI, seed, limits, players, connection, tactical
// combat, email game name
public sealed class SetupSettings : OwningNode, IChildList<PlayerSetupSettings>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a PlayerSetupSettings, so the cast never fails.
    public PlayerSetupSettings this[int index] => GetChild<PlayerSetupSettings>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is PlayerSetupSettings;

    int IChildSink<PlayerSetupSettings>.AddChild(PlayerSetupSettings child) => AddChild(child);

    // AoWSetup.pas:356
    public new const int ClassId = 131_680;

    public int HostRemoteId { get; set; }
    public string FileName { get; set; } = "";
    public string MapName { get; set; } = "";
    public int MapCrc { get; set; }
    public int SetCrc { get; set; }
    public int MapDay { get; set; }
    public int PlayMode { get; set; }
    public bool ExplorationEnabled { get; set; }
    public bool LeadersOnMap { get; set; }
    public bool AlliedVictory { get; set; }
    public int VictoryCondition { get; set; }
    public int IndependentAIMode { get; set; }
    public bool CustomizeLeaders { get; set; }
    public int RandomSeed { get; set; }
    public int DayLimit { get; set; }
    public int TurnTime { get; set; }

    public RemotePlayerList RemotePlayers { get; }

    public int ConnectionType { get; set; }
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
        PlayMode = persister.RawByte("PlayMode", value: PlayMode.TruncateToByte());
        ExplorationEnabled = persister.Boolean("ExplorationEnabled", value: ExplorationEnabled);
        MapName = persister.String("MapName", value: MapName);
        VictoryCondition = persister.RawByte("VictoryCondition", value: VictoryCondition.TruncateToByte());
        IndependentAIMode = persister.RawByte("IndependentAIMode", value: IndependentAIMode.TruncateToByte());
        CustomizeLeaders = persister.Boolean("CustomizeLeaders", value: CustomizeLeaders);
        RandomSeed = persister.Int32("RandomSeed", value: RandomSeed);
        LeadersOnMap = persister.Boolean("LeadersOnMap", value: LeadersOnMap);
        DayLimit = persister.Int32("DayLimit", value: DayLimit);
        TurnTime = persister.Int32("TurnTime", value: TurnTime);
        persister.Object("RemotePlayers", objectValue: RemotePlayers);
        HostRemoteId = persister.Int32("HostRemoteId", value: HostRemoteId);
        MapDay = persister.Int32("MapDay", value: MapDay);
        AlliedVictory = persister.Boolean("AlliedVictory", value: AlliedVictory);
        ConnectionType = persister.RawByte("ConnectionType", value: ConnectionType.TruncateToByte());
        MapCrc = persister.Int32("MapCrc", value: MapCrc);
        SetCrc = persister.Int32("SetCrc", value: SetCrc);
        TacticalBattleAvailability = persister.RawByte("TacticalBattleAvailability", value: TacticalBattleAvailability);
        AlwaysViewTacticalBattle = persister.Boolean("AlwaysViewTacticalBattle", value: AlwaysViewTacticalBattle);
    }

    // AoWSetup.pas:327
    public SetupSettings(CustomNode? owner) : base(owner)
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
    public PlayerSetupSettings? GetSettings(int index) => index >= 0 && index < Count ? this[index] : null;

    // AoWSetup.pas:366
    public int IndexOfPlayer(PlayerId player) => this.FindIndex(player, match: static (sought, setting) => setting.PlayerId == sought);

    // AoWSetup.pas:378
    public int IndexOfRemoteId(int remoteId) => this.FindIndex(remoteId, match: static (sought, setting) => setting.Behavior.Flags == sought);

    // AoWSetup.pas:390
    public int IndexOfRace(int race) => this.FindIndex(race, match: static (sought, setting) => sought == (byte)setting.Race);
}
