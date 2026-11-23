#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Tree;

using Aow.Game.Magic.Spells;

using Aow.Game.Saves;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Session;

// name. map stream. scenarios. completed scenario IDs. race history. scenario started. transfer
// settings. current ID. next ID. difficulty. play mode
public class Campaign : EngineObject
{

    // Diplomacy.pas:2246
    public Campaign(CustomNode? owner) : base(owner)
    {
        Stream = new MemoryStream();
        TransferSettings = new CampaignTransferSettings(null) { Holder = CreationHolder() };
        Scenarios = new AoWCampaignScenarioList(null) { Holder = CreationHolder() };
        CompletedScenarios = [];
        RaceHistory = [];
        CurrentScenarioId = -1;
        NextScenarioId = -1;
        ScenarioStarted = false;
        PlayMode = PlayMode.Simultaneous;
        DifficultyLevel = CampaignDifficultyLevel.Normal;
    }

    private string _name = "";

    public MemoryStream Stream { get; }
    public AoWCampaignScenarioList Scenarios { get; }
    public Int32List CompletedScenarios { get; }
    public ByteList RaceHistory { get; }

    public bool ScenarioStarted { get; set; }

    public CampaignTransferSettings TransferSettings { get; }

    public int CurrentScenarioId { get; set; }

    public int NextScenarioId { get; private set; }
    public string FileName { get; set; } = "";

    public CampaignDifficultyLevel DifficultyLevel { get; set; }
    public PlayMode PlayMode { get; set; }

    // Diplomacy.pas:2261
    public override void Dispose()
    {
        Stream?.Dispose();
        TransferSettings?.Dispose();
        Scenarios?.Dispose();
        CompletedScenarios?.Dispose();
        RaceHistory?.Dispose();
        base.Dispose();
    }

    // Diplomacy.pas:2295
    // The map is the bytes the stream holds (ids 5 and 6). A representable map is the compressed
    // HSM image. A map the image cannot carry is the map's own document. Both are a nested
    // world-hsm document on the tree. An absent map loads as an empty stream, as a stored size
    // of 0 does.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        if (persister.Loading)
        {
            byte[] image = persister.NestedDocument("Map", profile: "world-hsm", image: []);
            Stream.SetLength(image.Length);
            image.CopyTo(Stream.GetBuffer().AsSpan(start: 0, length: image.Length));
        }
        else if (!persister.Translating())
        {
            // The original ReadWrite leaves out the map under text only (the translate stream).
            _ = persister.NestedDocument("Map", profile: "world-hsm", image: Stream.GetBuffer().AsSpan(start: 0, length: (int)Stream.Length).ToArray());
        }

        persister.Object("TransferSettings", objectValue: TransferSettings);
        persister.Object("Scenarios", objectValue: Scenarios);
        _name = persister.String("Name", value: _name);
        CurrentScenarioId = persister.Int32("CurrentScenarioId", value: CurrentScenarioId);
        persister.Int32List(name: "CompletedScenarios", list: CompletedScenarios);
        ScenarioStarted = persister.Boolean("ScenarioStarted", value: ScenarioStarted);
        persister.ByteList(name: "RaceHistory", list: RaceHistory);
        PlayMode = persister.Enum("PlayMode", value: PlayMode);
        DifficultyLevel = persister.Enum("DifficultyLevel", value: DifficultyLevel);
        if (persister.Loading)
        {
            foreach (CampaignScenario? scenario in Scenarios)
            {
                if (scenario.Id == 0)
                {
                    int uniqueId = Scenarios.CreateUniqueId();
                    scenario.Id = uniqueId;
                }
            }
        }

        if (persister.Loading && CompletedScenarios.Count == 0 && !ScenarioStarted)
        {
            PlayMode = PlayMode.Simultaneous;
        }
    }

    // Diplomacy.pas:2271
    public override void Clear()
    {
        base.Clear();
        Stream.Clear();
        TransferSettings.Clear();
        Scenarios.Clear();
        CurrentScenarioId = -1;
        CompletedScenarios.Clear();
        RaceHistory.Clear();
    }

    // Diplomacy.pas:2282
    public virtual CampaignScenario CreateScenario() => new(null) { Holder = CreationHolder() };

    // Diplomacy.pas:2413
    // The name takes the extension the host writes for a campaign save (own-formats.md 4.2).
    public virtual string GetAutoSaveFileName(CampaignAutoSaveType auto) => StrategicMap.Current.MapName + FileBoundary.WriteExtension(DocumentKind.CampaignSave);

    // Diplomacy.pas:2330
    public virtual void SelectNextScenario(int id) => NextScenarioId = id;

    // Diplomacy.pas:2333
    public virtual void ResetNextScenario() => NextScenarioId = -1;

    // Diplomacy.pas:2336
    public virtual void StartScenario(int id, Action<object, int, int>? progress)
    {
        Stream.Clear();
        NextScenarioId = -1;
        CurrentScenarioId = id;
    }

    // Diplomacy.pas:2343
    public virtual void SetupScenario()
    {
        sbyte player = (sbyte)(StrategicMap.Current.Players.Count - 1);
        while (player > 1 && StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
        {
            player = (sbyte)(player - 1);
        }

        int level = 1 + (CompletedScenarios.Count / 4);
        for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            if (i != player)
            {
                for (int j = 0, lastJ = GamePack.Current.SpellControl.GetCount() - 1; j <= lastJ; j++)
                {
                    Spell? spell = GamePack.Current.SpellControl[j];
                    bool condition = spell is not null && spell.SpellLevel <= level;
                    if (condition)
                    {
                        int spherePicks = StrategicMap.Current.Players.GetPlayers(i).Leader.GetSpherePicks(spell.MagicSphere);
                        condition = spell.SpellLevel <= spherePicks;
                    }

                    if (condition)
                    {
                        if (StrategicMap.Current.Players.GetPlayers(i).MagicControl.Spells.IndexOf(j) == -1)
                        {
                            _ = StrategicMap.Current.Players.GetPlayers(i).MagicControl.Spells.Add(j);
                        }
                    }
                }
            }
        }

        for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            if (StrategicMap.Current.Players.GetPlayers(i).PlayerType != PlayerType.Human && StrategicMap.Current.Players.GetDiplomaticRelation((sbyte)i, otherPlayer: player) != DiplomaticRelation.Alliance)
            {
                if (DifficultyLevel == CampaignDifficultyLevel.Easy)
                {
                    switch (StrategicMap.Current.Players.GetPlayers(i).PlayerType)
                    {
                        case PlayerType.Cpu2:
                            StrategicMap.Current.Players.GetPlayers(i).PlayerType = PlayerType.Cpu1;
                            break;
                        case PlayerType.Cpu3:
                            StrategicMap.Current.Players.GetPlayers(i).PlayerType = PlayerType.Cpu2;
                            break;
                        case PlayerType.Cpu4:
                            StrategicMap.Current.Players.GetPlayers(i).PlayerType = PlayerType.Cpu3;
                            break;
                        case PlayerType.Cpu5:
                            StrategicMap.Current.Players.GetPlayers(i).PlayerType = PlayerType.Cpu3;
                            break;
                        case PlayerType.Human:
                        case PlayerType.Cpu1:
                        case PlayerType.Independent:
                        case PlayerType.Free:
                        default:
                            break;
                    }
                }

                if (DifficultyLevel == CampaignDifficultyLevel.Hard)
                {
                    switch (StrategicMap.Current.Players.GetPlayers(i).PlayerType)
                    {
                        case PlayerType.Cpu1:
                            StrategicMap.Current.Players.GetPlayers(i).PlayerType = PlayerType.Cpu3;
                            break;
                        case PlayerType.Cpu2:
                            StrategicMap.Current.Players.GetPlayers(i).PlayerType = PlayerType.Cpu4;
                            break;
                        case PlayerType.Cpu3:
                            StrategicMap.Current.Players.GetPlayers(i).PlayerType = PlayerType.Cpu5;
                            break;
                        case PlayerType.Cpu4:
                            StrategicMap.Current.Players.GetPlayers(i).PlayerType = PlayerType.Cpu5;
                            break;
                        case PlayerType.Human:
                        case PlayerType.Independent:
                        case PlayerType.Free:
                        case PlayerType.Cpu5:
                        default:
                            break;
                    }
                }
            }
        }

        StrategicMap.Current.SetupPlayerControl(PlayMode, connection: PlayerConnectionType.Local);
        for (int r = 0, last = StrategicMap.Current.Races.Count - 1; r <= last; r++)
        {
            if (StrategicMap.Current.Races.GetRace(r) is not null)
            {
                Race? race = StrategicMap.Current.Races.GetRace(r);
                if (race is not null)
                {
                    for (int i = 0; i <= 11; i++)
                    {
                        if (((RaceHistory.IndexOf((byte)i) != -1 || i == StrategicMap.Current.Players.GetPlayers(player).PlayerRace) && StrategicMap.Current.Players.GetPlayers(player).PlayerRace == r) || ((RaceHistory.IndexOf((byte)r) != -1 || StrategicMap.Current.Players.GetPlayers(player).PlayerRace == r) && i == StrategicMap.Current.Players.GetPlayers(player).PlayerRace))
                        {
                            int relation = race.DefaultRelations[i];
                            switch (relation)
                            {
                                case 0:
                                    relation = 3;
                                    break;
                                case 1:
                                    relation = 3;
                                    break;
                                case 2:
                                    relation = 4;
                                    break;
                                case 3:
                                    relation = 4;
                                    break;
                                default:
                                    break;
                            }

                            race.DefaultRelations[i] = (byte)relation;
                        }
                    }
                }
            }
        }

        StrategicMap.Current.AlliedVictory = StrategicMap.Current.CampaignSettings.AlliedVictory;
        TransferSettings.ExecuteSelection();
        ScenarioStarted = true;
    }

    // Diplomacy.pas:2416
    public virtual void FinishScenario()
    {
        ScenarioStarted = false;
        _ = CompletedScenarios.Add(CurrentScenarioId);
        TransferSettings.Retrieve();
        Stream.Clear();
        sbyte player = (sbyte)(StrategicMap.Current.Players.Count - 1);
        while (player > 1 && StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
        {
            player = (sbyte)(player - 1);
        }

        Player? players = StrategicMap.Current.Players.GetPlayers(player);
        if (RaceHistory.IndexOf((byte)players.PlayerRace) == -1)
        {
            Player? fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(player);
            _ = RaceHistory.Add(fPlayersPlayers.PlayerRace.TruncateToByte());
        }

        string fileName = GetAutoSaveFileName(CampaignAutoSaveType.ScenarioFinished);
        GetEngine().SaveCampaign($"{GetEngine().StartupDirectory}Save\\{fileName}");
    }

    // Diplomacy.pas:2432
    public virtual void RestartScenario()
    {
        CurrentScenarioId = CompletedScenarios.Count != 0 ? CompletedScenarios[^1] : 0;

        ScenarioStarted = false;
        Stream.Clear();
    }

    // Diplomacy.pas:2285
    public virtual CampaignScenario AddScenario()
    {
        CampaignScenario result = CreateScenario();
        result.Id = Scenarios.CreateUniqueId();
        result.SetOwner(Scenarios);
        return result;
    }

    // Diplomacy.pas:2292
    public bool ContainsMap => Stream.Length != 0;
}

public enum CampaignDifficultyLevel : byte
{
    Easy = 0, Normal = 1, Hard = 2
}
