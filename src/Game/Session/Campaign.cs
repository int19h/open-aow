// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;

using Aow.Game.Magic.Abilities;

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
        TransferSettings = new CampaignTransferSettings(null);
        Scenarios = new AoWCampaignScenarioList(null);
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

    public bool ScenarioStarted;

    public CampaignTransferSettings TransferSettings { get; }

    public int CurrentScenarioId;

    public int NextScenarioId { get; private set; }
    public string FileName { get; set; } = "";

    public CampaignDifficultyLevel DifficultyLevel;
    public PlayMode PlayMode;

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
    public override void ReadWrite(EStorageStream stream)
    {
        int size = default;
        base.ReadWrite(stream);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            if (stream.StorageMode().Contains(StorageModeElement.Read))
            {
                stream.rwInteger(ref size, 5);
                Stream.SetLength(size);
                stream.rwData(Stream.GetBuffer().AsSpan(0, size), size, 6);
            }
            else
            {
                size = (int)Stream.Length;
                stream.rwInteger(ref size, 5);
                stream.rwData(Stream.GetBuffer().AsSpan(0, size), size, 6);
            }
        }

        stream.rwEObject(TransferSettings, 7);
        stream.rwEObject(Scenarios, 10);
        stream.rwString(ref _name, 12);
        stream.rwInteger(ref CurrentScenarioId, 13);
        stream.rwStreamProc(14, CompletedScenarios.LoadFromStream, CompletedScenarios.SaveToStream);
        stream.rwBool(ref ScenarioStarted, 15);
        stream.rwStreamProc(16, RaceHistory.LoadFromStream, RaceHistory.SaveToStream);
        stream.rwData(Bytes.Of(ref PlayMode, 1), 1, 17);
        stream.rwData(Bytes.Of(ref DifficultyLevel, 1), 1, 19);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly) && stream.StorageMode().Contains(StorageModeElement.Read))
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

        if (stream.StorageMode().Contains(StorageModeElement.Read) && !stream.StorageMode().Contains(StorageModeElement.TextOnly) && CompletedScenarios.Count == 0 && !ScenarioStarted)
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
    public virtual CampaignScenario CreateScenario() => new CampaignScenario(null);

    // Diplomacy.pas:2413
    // The name takes the extension the host writes for a campaign save (own-formats.md 4.2).
    public virtual string GetAutoSaveFileName(CampaignAutoSaveType auto) => AoWHSMap.Current.MapName + FileBoundary.WriteExtension(DocumentKind.CampaignSave);

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
        player = (sbyte)(AoWHSMap.Current.Players.Count - 1);
        while (player > 1 && AoWHSMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
        {
            player = (sbyte)(player - 1);
        }

        level = 1 + CompletedScenarios.Count / 4;
        for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (I != player)
            {
                for (int J = 0, lastJ = AoWHSSet.Current.SpellControl.GetCount() - 1; J <= lastJ; J++)
                {
                    spell = AoWHSSet.Current.SpellControl.GetSpell(J);
                    bool condition = spell != null && spell.SpellLevel <= level;
                    if (condition)
                    {
                        var spherePicks = AoWHSMap.Current.Players.GetPlayers(I).Leader.GetSpherePicks(spell.MagicSphere);
                        condition = spell.SpellLevel <= spherePicks;
                    }

                    if (condition)
                    {
                        if (AoWHSMap.Current.Players.GetPlayers(I).MagicControl.Spells.IndexOf(J) == -1)
                        {
                            AoWHSMap.Current.Players.GetPlayers(I).MagicControl.Spells.Add(J);
                        }
                    }
                }
            }
        }

        for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (AoWHSMap.Current.Players.GetPlayers(I).PlayerType != PlayerType.Human && AoWHSMap.Current.Players.GetDiplomaticRelation((sbyte)I, player) != DiplomaticRelation.Alliance)
            {
                if (DifficultyLevel == CampaignDifficultyLevel.Easy)
                {
                    switch (AoWHSMap.Current.Players.GetPlayers(I).PlayerType)
                    {
                        case PlayerType.Cpu2:
                            AoWHSMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu1;
                            break;
                        case PlayerType.Cpu3:
                            AoWHSMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu2;
                            break;
                        case PlayerType.Cpu4:
                            AoWHSMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu3;
                            break;
                        case PlayerType.Cpu5:
                            AoWHSMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu3;
                            break;
                    }
                }

                if (DifficultyLevel == CampaignDifficultyLevel.Hard)
                {
                    switch (AoWHSMap.Current.Players.GetPlayers(I).PlayerType)
                    {
                        case PlayerType.Cpu1:
                            AoWHSMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu3;
                            break;
                        case PlayerType.Cpu2:
                            AoWHSMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu4;
                            break;
                        case PlayerType.Cpu3:
                            AoWHSMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu5;
                            break;
                        case PlayerType.Cpu4:
                            AoWHSMap.Current.Players.GetPlayers(I).PlayerType = PlayerType.Cpu5;
                            break;
                    }
                }
            }
        }

        AoWHSMap.Current.SetupPlayerControl(PlayMode, PlayerConnectionType.Local);
        for (int R = 0, last = AoWHSMap.Current.Races.Count - 1; R <= last; R++)
        {
            if (AoWHSMap.Current.Races.GetRace(R) != null)
            {
                race = AoWHSMap.Current.Races.GetRace(R);
                if (race != null)
                {
                    for (int I = 0; I <= 11; I++)
                    {
                        if ((RaceHistory.IndexOf((byte)I) != -1 || I == AoWHSMap.Current.Players.GetPlayers(player).PlayerRace) && AoWHSMap.Current.Players.GetPlayers(player).PlayerRace == R || (RaceHistory.IndexOf((byte)R) != -1 || AoWHSMap.Current.Players.GetPlayers(player).PlayerRace == R) && I == AoWHSMap.Current.Players.GetPlayers(player).PlayerRace)
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

        AoWHSMap.Current.AlliedVictory = AoWHSMap.Current.CampaignSettings.AlliedVictory;
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
        player = (sbyte)(AoWHSMap.Current.Players.Count - 1);
        while (player > 1 && AoWHSMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
        {
            player = (sbyte)(player - 1);
        }

        var players = AoWHSMap.Current.Players.GetPlayers(player);
        if (RaceHistory.IndexOf((byte)players.PlayerRace) == -1)
        {
            var fPlayersPlayers = AoWHSMap.Current.Players.GetPlayers(player);
            RaceHistory.Add((byte)fPlayersPlayers.PlayerRace);
        }

        fileName = GetAutoSaveFileName(CampaignAutoSaveType.ScenarioFinished);
        GameTables.GameEngine.SaveCampaign(GameTables.GameEngine.StartupDirectory + "Save\\" + fileName);
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
