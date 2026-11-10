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
public partial class AoWCampaign : EObject
{

    // Diplomacy.pas:2246
    public AoWCampaign(ECustomNode owner) : base(owner)
    {
        FStream = new MemoryStream();
        FTransferSettings = new AoWCampaignTransferSettings(null);
        FScenarios = new AoWCampaignScenarioList(null);
        FCompletedScenarios = new IntegerList();
        FRaceHistory = new ByteList();
        FCurrentScenarioID = -1;
        FNextScenarioID = -1;
        FScenarioStarted = false;
        FPlayMode = PlayMode.Simultaneous;
        FDifficultyLevel = CampaignDifficultyLevel.Normal;
    }

    public string FName = "";
    public MemoryStream FStream;
    public AoWCampaignScenarioList FScenarios;
    public IntegerList FCompletedScenarios;
    public ByteList FRaceHistory;
    public bool FScenarioStarted;
    public AoWCampaignTransferSettings FTransferSettings;
    public int FCurrentScenarioID;
    public int FNextScenarioID;
    public string FFileName = "";
    public CampaignDifficultyLevel FDifficultyLevel;
    public PlayMode FPlayMode;

    // Diplomacy.pas:2261
    public override void Dispose()
    {
        FStream?.Dispose();
        FTransferSettings?.Dispose();
        FScenarios?.Dispose();
        FCompletedScenarios?.Dispose();
        FRaceHistory?.Dispose();
        base.Dispose();
    }

    // Diplomacy.pas:2295
    public override void ReadWrite(EStorageStream Stream)
    {
        int Size = default;
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            if (Stream.StorageMode().Contains(StorageModeElement.Read))
            {
                Stream.rwInteger(ref Size, 5);
                FStream.SetLength(Size);
                Stream.rwData(FStream.GetBuffer().AsSpan(0, Size), Size, 6);
            }
            else
            {
                Size = (int)FStream.Length;
                Stream.rwInteger(ref Size, 5);
                Stream.rwData(FStream.GetBuffer().AsSpan(0, Size), Size, 6);
            }
        }

        Stream.rwEObject(FTransferSettings, 7);
        Stream.rwEObject(FScenarios, 10);
        Stream.rwString(ref FName, 12);
        Stream.rwInteger(ref FCurrentScenarioID, 13);
        Stream.rwStreamProc(14, FCompletedScenarios.LoadFromStream, FCompletedScenarios.SaveToStream);
        Stream.rwBool(ref FScenarioStarted, 15);
        Stream.rwStreamProc(16, FRaceHistory.LoadFromStream, FRaceHistory.SaveToStream);
        Stream.rwData(Bytes.Of(ref FPlayMode, 1), 1, 17);
        Stream.rwData(Bytes.Of(ref FDifficultyLevel, 1), 1, 19);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly) && Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            for (int I = 0, last = FScenarios.Count - 1; I <= last; I++)
            {
                if (FScenarios.GetScenario(I).FID == 0)
                {
                    var uniqueID = FScenarios.CreateUniqueID();
                    FScenarios.GetScenario(I).FID = uniqueID;
                }
            }
        }

        if (Stream.StorageMode().Contains(StorageModeElement.Read) && !Stream.StorageMode().Contains(StorageModeElement.TextOnly) && FCompletedScenarios.Count == 0 && !FScenarioStarted)
        {
            FPlayMode = PlayMode.Simultaneous;
        }
    }

    // Diplomacy.pas:2271
    public override void Clear()
    {
        base.Clear();
        FStream.Clear();
        FTransferSettings.Clear();
        FScenarios.Clear();
        FCurrentScenarioID = -1;
        FCompletedScenarios.Clear();
        FRaceHistory.Clear();
    }

    // Diplomacy.pas:2282
    public virtual AoWCampaignScenario CreateScenario() => new AoWCampaignScenario(null);

    // Diplomacy.pas:2413
    // The name takes the extension the host writes for a campaign save (own-formats.md 4.2).
    public virtual string GetAutoSaveFileName(CampaignAutoSaveType Auto) => AoWHSMap.Current.FMapName + FileBoundary.WriteExtension(DocumentKind.CampaignSave);

    // Diplomacy.pas:2330
    public virtual void SelectNextScenario(int ID)
    {
        FNextScenarioID = ID;
    }

    // Diplomacy.pas:2333
    public virtual void ResetNextScenario()
    {
        FNextScenarioID = -1;
    }

    // Diplomacy.pas:2336
    public virtual void StartScenario(int ID, Draw.ProgressEvent Progress)
    {
        FStream.Clear();
        FNextScenarioID = -1;
        FCurrentScenarioID = ID;
    }

    // Diplomacy.pas:2343
    public virtual void SetupScenario()
    {
        sbyte Player;
        int Level;
        Spell Spell;
        Race Race;
        byte Relation;
        Player = (sbyte)(AoWHSMap.Current.FPlayers.Count - 1);
        while (Player > 1 && AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
        {
            Player = (sbyte)(Player - 1);
        }

        Level = 1 + FCompletedScenarios.Count / 4;
        for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            if (I != Player)
            {
                for (int J = 0, lastJ = AoWHSSet.Current.FSpellControl.GetCount() - 1; J <= lastJ; J++)
                {
                    Spell = AoWHSSet.Current.FSpellControl.GetSpell(J);
                    bool condition = Spell != null && Spell.FSpellLevel <= Level;
                    if (condition)
                    {
                        var spherePicks = AoWHSMap.Current.FPlayers.GetPlayers(I).FLeader.GetSpherePicks(Spell.FMagicSphere);
                        condition = Spell.FSpellLevel <= spherePicks;
                    }

                    if (condition)
                    {
                        if (AoWHSMap.Current.FPlayers.GetPlayers(I).FMagicControl.FSpells.IndexOf(J) == -1)
                        {
                            AoWHSMap.Current.FPlayers.GetPlayers(I).FMagicControl.FSpells.Add(J);
                        }
                    }
                }
            }
        }

        for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType != PlayerType.Human && AoWHSMap.Current.FPlayers.GetDiplomaticRelation((sbyte)I, Player) != DiplomaticRelation.Alliance)
            {
                if (FDifficultyLevel == CampaignDifficultyLevel.Easy)
                {
                    switch (AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType)
                    {
                        case PlayerType.CPU2:
                            AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType = PlayerType.CPU1;
                            break;
                        case PlayerType.CPU3:
                            AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType = PlayerType.CPU2;
                            break;
                        case PlayerType.CPU4:
                            AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType = PlayerType.CPU3;
                            break;
                        case PlayerType.CPU5:
                            AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType = PlayerType.CPU3;
                            break;
                    }
                }

                if (FDifficultyLevel == CampaignDifficultyLevel.Hard)
                {
                    switch (AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType)
                    {
                        case PlayerType.CPU1:
                            AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType = PlayerType.CPU3;
                            break;
                        case PlayerType.CPU2:
                            AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType = PlayerType.CPU4;
                            break;
                        case PlayerType.CPU3:
                            AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType = PlayerType.CPU5;
                            break;
                        case PlayerType.CPU4:
                            AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType = PlayerType.CPU5;
                            break;
                    }
                }
            }
        }

        AoWHSMap.Current.SetupPlayerControl(FPlayMode, PlayerConnectionType.Local);
        for (int R = 0, last = AoWHSMap.Current.FRaces.Count - 1; R <= last; R++)
        {
            if (AoWHSMap.Current.FRaces.GetRace(R) != null)
            {
                Race = AoWHSMap.Current.FRaces.GetRace(R);
                if (Race != null)
                {
                    for (int I = 0; I <= 11; I++)
                    {
                        if ((FRaceHistory.IndexOf((byte)I) != -1 || I == AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerRace) && AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerRace == R || (FRaceHistory.IndexOf((byte)R) != -1 || AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerRace == R) && I == AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerRace)
                        {
                            Relation = Race.FDefaultRelations[I];
                            switch (Relation)
                            {
                                case 0:
                                    Relation = 3;
                                    break;
                                case 1:
                                    Relation = 3;
                                    break;
                                case 2:
                                    Relation = 4;
                                    break;
                                case 3:
                                    Relation = 4;
                                    break;
                            }

                            Race.FDefaultRelations[I] = Relation;
                        }
                    }
                }
            }
        }

        AoWHSMap.Current.FAlliedVictory = AoWHSMap.Current.FCampaignSettings.FAlliedVictory;
        FTransferSettings.ExecuteSelection();
        FScenarioStarted = true;
    }

    // Diplomacy.pas:2416
    public virtual void FinishScenario()
    {
        sbyte Player;
        string FileName;
        FScenarioStarted = false;
        FCompletedScenarios.Add(FCurrentScenarioID);
        FTransferSettings.Retrieve();
        FStream.Clear();
        Player = (sbyte)(AoWHSMap.Current.FPlayers.Count - 1);
        while (Player > 1 && AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
        {
            Player = (sbyte)(Player - 1);
        }

        var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        if (FRaceHistory.IndexOf((byte)players.FPlayerRace) == -1)
        {
            var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(Player);
            FRaceHistory.Add((byte)fPlayersPlayers.FPlayerRace);
        }

        FileName = GetAutoSaveFileName(CampaignAutoSaveType.ScenarioFinished);
        GameTables.AoWEngine.SaveCampaign(GameTables.AoWEngine.StartupDirectory + "Save\\" + FileName);
    }

    // Diplomacy.pas:2432
    public virtual void RestartScenario()
    {
        if (FCompletedScenarios.Count != 0)
        {
            FCurrentScenarioID = FCompletedScenarios[FCompletedScenarios.Count - 1];
        }
        else
        {
            FCurrentScenarioID = 0;
        }

        FScenarioStarted = false;
        FStream.Clear();
    }

    // Diplomacy.pas:2285
    public virtual AoWCampaignScenario AddScenario()
    {
        AoWCampaignScenario Result = CreateScenario();
        Result.FID = FScenarios.CreateUniqueID();
        Result.SetOwner(FScenarios);
        return Result;
    }

    // Diplomacy.pas:2292
    public bool GetContainsMap() => FStream.Length != 0;

    public enum CampaignDifficultyLevel : byte
    {
        Easy = 0, Normal = 1, Hard = 2
    }

    // intf_P5_08.inc:107

}
