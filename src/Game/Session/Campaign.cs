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
public partial class Campaign : EngineObject
{

    // Diplomacy.pas:2246
    public Campaign(ECustomNode owner) : base(owner)
    {
        Stream = new MemoryStream();
        TransferSettings = new CampaignTransferSettings(null) { Holder = CreationHolder() };
        Scenarios = new AoWCampaignScenarioList(null) { Holder = CreationHolder() };
        CompletedScenarios = new Int32List();
        RaceHistory = new ByteList();
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
    // The map is the whole HSM file the stream holds (ids 5 and 6), a nested world-hsm document
    // on the tree. An absent map loads as an empty stream, as a stored size of 0 does.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        if (persister.Loading)
        {
            var image = persister.NestedDocument("Map", profile: "world-hsm", image: []);
            Stream.SetLength(image.Length);
            image.CopyTo(Stream.GetBuffer().AsSpan(start: 0, length: image.Length));
        }
        else if (!PersistSupport.Translating(persister))
        {
            // The original ReadWrite leaves out the map under text only (the translate stream).
            persister.NestedDocument("Map", profile: "world-hsm", image: Stream.GetBuffer().AsSpan(start: 0, length: (int)Stream.Length).ToArray());
        }

        persister.Object("TransferSettings", objectValue: TransferSettings);
        persister.Object("Scenarios", objectValue: Scenarios);
        _name = persister.String("Name", value: _name);
        CurrentScenarioId = persister.Int32("CurrentScenarioId", value: CurrentScenarioId);
        PersistSupport.Int32List(persister, name: "CompletedScenarios", list: CompletedScenarios);
        ScenarioStarted = persister.Boolean("ScenarioStarted", value: ScenarioStarted);
        PersistSupport.ByteList(persister, name: "RaceHistory", list: RaceHistory);
        PlayMode = persister.Enum("PlayMode", value: PlayMode);
        DifficultyLevel = persister.Enum("DifficultyLevel", value: DifficultyLevel);
        if (persister.Loading)
        {
            for (int I = 0, last = Scenarios.Count - 1; I <= last; I++)
            {
                if (Scenarios.GetScenario(I).Id == 0)
                {
                    var uniqueId = Scenarios.CreateUniqueId();
                    Scenarios.GetScenario(I).Id = uniqueId;
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
    public virtual CampaignScenario CreateScenario() => new CampaignScenario(null) { Holder = CreationHolder() };

    // Diplomacy.pas:2413
    // The name takes the extension the host writes for a campaign save (own-formats.md 4.2).
    public virtual string GetAutoSaveFileName(CampaignAutoSaveType auto) => StrategicMap.Current.MapName + FileBoundary.WriteExtension(DocumentKind.CampaignSave);

    // Diplomacy.pas:2330
    public virtual void SelectNextScenario(int id)
    {
        NextScenarioId = id;
    }

    // Diplomacy.pas:2333
    public virtual void ResetNextScenario()
    {
        NextScenarioId = -1;
    }

    // Diplomacy.pas:2336
    public virtual void StartScenario(int id, Draw.ProgressEvent progress)
    {
        Stream.Clear();
        NextScenarioId = -1;
        CurrentScenarioId = id;
    }

    // Diplomacy.pas:2343
    public virtual void SetupScenario()
    {
        sbyte player;
        int level;
        Spell spell;
        Race race;
        byte relation;
        player = (sbyte)(StrategicMap.Current.Players.Count - 1);
        while (player > 1 && StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
        {
            player = (sbyte)(player - 1);
        }

        level = 1 + CompletedScenarios.Count / 4;
        for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (I != player)
            {
                for (int J = 0, lastJ = GamePack.Current.SpellControl.GetCount() - 1; J <= lastJ; J++)
                {
                    spell = GamePack.Current.SpellControl.GetSpell(J);
                    bool condition = spell != null && spell.SpellLevel <= level;
                    if (condition)
                    {
                        var spherePicks = StrategicMap.Current.Players.GetPlayers(I).Leader.GetSpherePicks(spell.MagicSphere);
                        condition = spell.SpellLevel <= spherePicks;
                    }

                    if (condition)
                    {
                        if (StrategicMap.Current.Players.GetPlayers(I).MagicControl.Spells.IndexOf(J) == -1)
                        {
                            StrategicMap.Current.Players.GetPlayers(I).MagicControl.Spells.Add(J);
                        }
                    }
                }
            }
        }

        for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (StrategicMap.Current.Players.GetPlayers(I).PlayerType != PlayerType.Human && StrategicMap.Current.Players.GetDiplomaticRelation((sbyte)I, player) != DiplomaticRelation.Alliance)
            {
                if (DifficultyLevel == CampaignDifficultyLevel.Easy)
                {
                    switch (StrategicMap.Current.Players.GetPlayers(I).PlayerType)
                    {
                        case PlayerType.Cpu2:
                            StrategicMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu1;
                            break;
                        case PlayerType.Cpu3:
                            StrategicMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu2;
                            break;
                        case PlayerType.Cpu4:
                            StrategicMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu3;
                            break;
                        case PlayerType.Cpu5:
                            StrategicMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu3;
                            break;
                    }
                }

                if (DifficultyLevel == CampaignDifficultyLevel.Hard)
                {
                    switch (StrategicMap.Current.Players.GetPlayers(I).PlayerType)
                    {
                        case PlayerType.Cpu1:
                            StrategicMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu3;
                            break;
                        case PlayerType.Cpu2:
                            StrategicMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu4;
                            break;
                        case PlayerType.Cpu3:
                            StrategicMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu5;
                            break;
                        case PlayerType.Cpu4:
                            StrategicMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu5;
                            break;
                    }
                }
            }
        }

        StrategicMap.Current.SetupPlayerControl(PlayMode, PlayerConnectionType.Local);
        for (int R = 0, last = StrategicMap.Current.Races.Count - 1; R <= last; R++)
        {
            if (StrategicMap.Current.Races.GetRace(R) != null)
            {
                race = StrategicMap.Current.Races.GetRace(R);
                if (race != null)
                {
                    for (int I = 0; I <= 11; I++)
                    {
                        if ((RaceHistory.IndexOf((byte)I) != -1 || I == StrategicMap.Current.Players.GetPlayers(player).PlayerRace) && StrategicMap.Current.Players.GetPlayers(player).PlayerRace == R || (RaceHistory.IndexOf((byte)R) != -1 || StrategicMap.Current.Players.GetPlayers(player).PlayerRace == R) && I == StrategicMap.Current.Players.GetPlayers(player).PlayerRace)
                        {
                            relation = race.DefaultRelations[I];
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
                            }

                            race.DefaultRelations[I] = relation;
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
        sbyte player;
        string fileName;
        ScenarioStarted = false;
        CompletedScenarios.Add(CurrentScenarioId);
        TransferSettings.Retrieve();
        Stream.Clear();
        player = (sbyte)(StrategicMap.Current.Players.Count - 1);
        while (player > 1 && StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
        {
            player = (sbyte)(player - 1);
        }

        var players = StrategicMap.Current.Players.GetPlayers(player);
        if (RaceHistory.IndexOf((byte)players.PlayerRace) == -1)
        {
            var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(player);
            RaceHistory.Add((byte)fPlayersPlayers.PlayerRace);
        }

        fileName = GetAutoSaveFileName(CampaignAutoSaveType.ScenarioFinished);
        GetEngine().SaveCampaign(GetEngine().StartupDirectory + "Save\\" + fileName);
    }

    // Diplomacy.pas:2432
    public virtual void RestartScenario()
    {
        if (CompletedScenarios.Count != 0)
        {
            CurrentScenarioId = CompletedScenarios[CompletedScenarios.Count - 1];
        }
        else
        {
            CurrentScenarioId = 0;
        }

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
    public bool GetContainsMap() => Stream.Length != 0;

    // intf_P5_08.inc:107

}

public enum CampaignDifficultyLevel : byte
{
    Easy = 0, Normal = 1, Hard = 2
}
