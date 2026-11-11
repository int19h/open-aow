// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;

using Aow.Game.Commands;

using Aow.Game.Draw.Images;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

// update count, events, structures, victory locations, armies, messages, legacy relations,
// diplomacy, game over, player status, busy count, morale, leader setup, statistics, income
// sources, known structures, heroes, groups, AI control, budget, diplomatic actions, attack
// players, seen days, PBEM, emerging hero, participating, initial gold, base income, hero join
// days, upkeep. end turn, view, password, turn number, base hero level, emerging hero ID,
// emerge day, highest hero level, settings/description
public partial class Player : EngineObject
{
    public new const int ClassID = 131104;

    // Player.PAS:261
    public Player(ECustomNode owner) : base(owner)
    {
        FIncomeChangedEventsStore = new();
        FGemsChangedEventsStore = new();
        FUpkeepChangedEventsStore = new();
        FStructures = new PlayerStructureList(null);
        FStructures.FPlayer = this;
        FArmies = new ArmyList(null);
        FAIControl = new AIPlayerControl(null);
        FAIControl.FPlayer = this;
        FAIBudget = new AIBudgetManager(null);
        FAIBudget.FPlayer = this;
        FAIGroups = new AIGroupList(null);
        FMagicControl = new PlayerMagicControl(null);
        FMagicControl.FPlayer = this;
        FStatistics = new PlayerStatistics(null);
        FArmies.FPlayer = this;
        FArmies.Events += ArmiesChanged;
        FHeroes = new HeroList(null);
        FInitialGold = 250;
        FBaseIncome = 0;
        FPlayerIndex = -1;
        FParticipating = false;
        FEventLogbook = new EventLogbook(null);
        FEventLogbook.SetOnChanged(LogbookChangedEvent);
        FLogbookChangedEventsStore = new();
        FDiplomaticRelations = new PlayerDiplomaticRelations(null);
        FDiplomaticRelations.FPlayer = this;
        FLegacyNewRelations = new DiplomaticRelationList();
        FProductionControls = new ProductionControlList();
        FIncomeSources = new IncomeSourceList(null);
        FLeaderSetup = new Leader(null);
        FStatus = Status.Idle;
        FGameOverStatus = GameOverStatus.None;
        FFirstDaySeen = new Int32List();
        FFirstDaySeen.Default = -1;
        FLastDaySeen = new Int32List();
        FLastDaySeen.Default = -1;
        FVictoryLocations = new PlayerStructureList(null);
        FKnownStructures = new PlayerStructureList(null);
        FMessages = new StringList();
        FDiplomaticActions = new DiplomaticActionList(null);
        FAIValidAttackPlayers = new BitList();
        FPBEMSettings = new PbemPlayerSettings(null);
        FLegacyRelations = new DiplomaticRelationList();
        FHeroEmerging = false;
        FBaseHeroJoinDay = -1;
        FHeroJoinDay = -1;
        FPlayerRace = -1;
        FViewX = 255;
        FViewY = 255;
    }

    public int FUpdateCount;

    internal EventStore<Player> FGemsChangedEventsStore;

    public event NotifyEvent<Player> FGemsChangedEvents
    {
        add => FGemsChangedEventsStore.Add(value);
        remove => FGemsChangedEventsStore.Remove(value);
    }

    internal EventStore<Player> FIncomeChangedEventsStore;

    public event NotifyEvent<Player> FIncomeChangedEvents
    {
        add => FIncomeChangedEventsStore.Add(value);
        remove => FIncomeChangedEventsStore.Remove(value);
    }

    internal EventStore<Player> FUpkeepChangedEventsStore;

    public event NotifyEvent<Player> FUpkeepChangedEvents
    {
        add => FUpkeepChangedEventsStore.Add(value);
        remove => FUpkeepChangedEventsStore.Remove(value);
    }

    internal EventStore<Player> FLogbookChangedEventsStore;

    public event NotifyEvent<Player> FLogbookChangedEvents
    {
        add => FLogbookChangedEventsStore.Add(value);
        remove => FLogbookChangedEventsStore.Remove(value);
    }

    public PlayerStructureList FStructures;
    public PlayerStructureList FVictoryLocations;
    public ProductionControlList FProductionControls;
    public ArmyList FArmies;
    public bool FSingleProcessCycle;
    public byte FViewX;
    public byte FViewY;
    public byte FViewL;
    public string FPassword = "";
    public StringList FMessages;
    public DiplomaticRelationList FLegacyRelations;
    public DiplomaticRelationList FLegacyNewRelations;
    public PlayerDiplomaticRelations FDiplomaticRelations;
    public GameOverStatus FGameOverStatus;
    public Status FStatus;
    public int FBusyCount;
    public string FName = "";
    public sbyte FMoraleValue;
    public PlayerMagicControl FMagicControl;
    public Leader FLeaderSetup;
    public int FDay;
    public PlayerStatistics FStatistics;
    public IncomeSourceList FIncomeSources;
    public PlayerStructureList FKnownStructures;
    public int FBaseHeroJoinLevel;
    public int FLastHeroJoinDay;
    public int FEmergingHeroID;
    public int FLastHeroEmergeDay;
    public int FHighestHeroLevel;
    public HeroList FHeroes;
    public AIGroupList FAIGroups;
    public AIPlayerControl FAIControl;
    public AIBudgetManager FAIBudget;
    public DiplomaticActionList FDiplomaticActions;
    public BitList FAIValidAttackPlayers;
    public Int32List FFirstDaySeen;
    public Int32List FLastDaySeen;
    public PbemPlayerSettings FPBEMSettings;
    public bool FHeroEmerging;
    public sbyte FPlayerRace;
    public sbyte FPlayerIndex;
    public PlayerType FPlayerType;
    public Behavior FBehavior;
    public bool FParticipating;
    public int FInitialGold;
    public int FBaseIncome;
    public int FBaseHeroJoinDay;
    public int FHeroJoinDay;
    public string FSettings = "";
    public string FDescription = "";
    public int FGems;
    public int FArmyUpkeep;
    public int FMagicUpkeep;
    public int FIncome;
    public Leader FLeader;
    public EventLogbook FEventLogbook;

    // Player.PAS:315
    public override void Dispose()
    {
        PlayerStructureList Structures;
        ArmyList Armies;
        try
        {
            FIncomeChangedEventsStore.Clear();
            FGemsChangedEventsStore.Clear();
            FUpkeepChangedEventsStore.Clear();
            try
            {
                if (AoWHSMap.Current != null && AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
                {
                    Assert(FVictoryLocations.Count == 0, "Victory Locations not correctly cleared (TPlayer)", "Player.PAS", 326);
                    Assert(FStructures.Count == 0, "Registered Player Structures not correctly cleared (TPlayer)", "Player.PAS", 327);
                    Assert(FArmies.Count == 0, "Registered Player Armies not correctly cleared (TPlayer)", "Player.PAS", 328);
                }
                else
                {
                    Structures = FStructures;
                    for (int I = Structures.Count - 1; I >= 0; I--)
                    {
                        FStructures.GetStructure(I).SetPlayer(0);
                    }

                    Armies = FArmies;
                    for (int I = Armies.Count - 1; I >= 0; I--)
                    {
                        FArmies.GetArmy(I).SetPlayer(0);
                    }
                }
            }
            catch
            {
            }

            FMessages?.Dispose();
            FMessages = null;
            FDiplomaticActions?.Dispose();
            FDiplomaticActions = null;
            FAIValidAttackPlayers?.Dispose();
            FAIValidAttackPlayers = null;
            FLegacyRelations?.Dispose();
            try
            {
                FLogbookChangedEventsStore.Clear();
                FEventLogbook?.Dispose();
                FLogbookChangedEventsStore.Clear();
            }
            catch
            {
            }

            try
            {
                FLegacyNewRelations?.Dispose();
                FDiplomaticRelations?.Dispose();
            }
            catch
            {
            }

            try
            {
                FProductionControls?.Dispose();
            }
            catch
            {
            }

            try
            {
                FAIControl?.Dispose();
            }
            catch
            {
            }

            try
            {
                FStatistics?.Dispose();
            }
            catch
            {
            }

            try
            {
                FMagicControl?.Dispose();
            }
            catch
            {
            }

            try
            {
                FLeaderSetup?.Dispose();
            }
            catch
            {
            }

            try
            {
                FFirstDaySeen?.Dispose();
                FLastDaySeen?.Dispose();
            }
            catch
            {
            }

            try
            {
                FHeroes?.Dispose();
            }
            catch
            {
            }

            try
            {
                FArmies?.Dispose();
                FAIGroups?.Dispose();
                FAIBudget?.Dispose();
                FStructures?.Dispose();
                FVictoryLocations?.Dispose();
                FIncomeSources?.Dispose();
                FKnownStructures?.Dispose();
            }
            catch
            {
            }

            try
            {
                base.Dispose();
            }
            catch
            {
            }

            FPBEMSettings?.Dispose();
            FPBEMSettings = null;
            FIncomeChangedEventsStore.Clear();
            FGemsChangedEventsStore.Clear();
            FUpkeepChangedEventsStore.Clear();
        }
        catch
        {
        }
    }

    // Player.PAS:1023
    public override void ReadWrite(EStorageStream Stream)
    {
        int X = default;
        int Y = default;
        int L = default;
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayerRace), 2);
        Stream.rwData(Bytes.Of(ref FPlayerType, 1), 1, 3);
        Stream.rwBool(ref FParticipating, 4);
        Stream.rwInteger(ref FInitialGold, 5);
        Stream.rwInteger(ref FBaseIncome, 6);
        Stream.rwData(Bytes.Of(ref FStatus, 1), 1, 10);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FSettings, 11);
        }

        Stream.rwString(ref FDescription, 12);
        Stream.rwInteger(ref FGems, 16);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayerIndex), 18);
        if ((GameTables.AoWEngine.Status & Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHSM, HexSpriteEngineStatusElement.SavingHSM)) != Set<HexSpriteEngineStatusElement>.Of() || Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            if (!Stream.StorageMode().Contains(StorageModeElement.Write))
            {
                Stream.rwCreateEObject(ref FLeader, null, 19);
                if (FLeader != null && !Stream.StorageMode().Contains(StorageModeElement.Preview))
                {
                    FLeader.FPlayer = FPlayerIndex;
                    AoWHSMap.Current.FHeroControl.RegisterHero(FLeader);
                }
            }
        }

        if (Stream.StorageMode().Contains(StorageModeElement.Write))
        {
            if (AoWHSMap.Current.FSeatedPlayer == FPlayerIndex && AoWHSMap.Current.FConnectionType != PlayerConnectionType.Network)
            {
                AoWHSMap.Current.GetViewCenter(ref X, ref Y, ref L);
                FViewX = (byte)X;
                FViewY = (byte)Y;
                FViewL = (byte)L;
                Stream.rwByte(ref FViewX, 20);
                Stream.rwByte(ref FViewY, 21);
                Stream.rwByte(ref FViewL, 22);
            }
        }
        else
        {
            Stream.rwByte(ref FViewX, 20);
            Stream.rwByte(ref FViewY, 21);
            Stream.rwByte(ref FViewL, 22);
        }

        if (!Stream.StorageMode().Contains(StorageModeElement.Preview))
        {
            Stream.rwEObject(FEventLogbook, 23);
        }

        Stream.rwString(ref FPassword, 24);
        Stream.rwData(Bytes.Of(ref FGameOverStatus, 1), 1, 25);
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            Stream.rwStreamProc(26, FLegacyNewRelations.LoadFromStream, FLegacyNewRelations.SaveToStream);
        }

        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FMoraleValue), 27);
        Stream.rwEObject(FAIControl, 28);
        Stream.rwData(Bytes.Of(ref FBehavior, 1), 1, 29);
        if (FPlayerIndex != 0)
        {
            if (Stream.StorageMode().Contains(StorageModeElement.Write) && AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && FLeader != null)
            {
                Stream.rwEObject(FLeader, 30);
            }
            else
            {
                Stream.rwEObject(FLeaderSetup, 30);
            }
        }

        Stream.rwEObject(FMagicControl, 31);
        Stream.rwStreamProc(32, FFirstDaySeen.LoadFromStream, FFirstDaySeen.SaveToStream);
        Stream.rwStreamProc(33, FLastDaySeen.LoadFromStream, FLastDaySeen.SaveToStream);
        Stream.rwInteger(ref FDay, 34);
        Stream.rwEObject(FStatistics, 35);
        Stream.rwInteger(ref FBaseHeroJoinLevel, 36);
        Stream.rwInteger(ref FLastHeroJoinDay, 37);
        Stream.rwInteger(ref FLastHeroEmergeDay, 38);
        Stream.rwInteger(ref FHighestHeroLevel, 39);
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            SetRace(FPlayerRace);
            FEventLogbook.FPlayer = FPlayerIndex;
            FLegacyNewRelations.Default = (int)DiplomaticRelation.Unknown;
        }

        Stream.rwStrings(FMessages, 40);
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            Stream.rwStreamProc(41, FLegacyRelations.LoadFromStream, FLegacyRelations.SaveToStream);
        }

        Stream.rwInteger(ref FEmergingHeroID, 50);
        Stream.rwEObject(FAIBudget, 51);
        Stream.rwEObject(FDiplomaticActions, 52);
        Stream.rwStreamProc(53, FAIValidAttackPlayers.LoadFromStream, FAIValidAttackPlayers.SaveToStream);
        Stream.rwEObject(FPBEMSettings, 54);
        Stream.rwBool(ref FHeroEmerging, 56);
        if (Stream.StorageMode().Contains(StorageModeElement.Read) || AoWHSMap.Current.FConnectionType != PlayerConnectionType.Network)
        {
            Stream.rwInteger(ref FBaseHeroJoinDay, 57);
        }

        Stream.rwEObject(FDiplomaticRelations, 58);
        if (FPlayerIndex == 0)
        {
            FPlayerType = PlayerType.Independent;
        }
    }

    // Player.PAS:974
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        FDiplomaticRelations.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920 && AoWHSMap.Current.FDay == 0)
        {
            FFirstDaySeen.Clear();
            FLastDaySeen.Clear();
        }

        if (Msg.MsgID == 537001989)
        {
            ValidateLeader();
        }

        if (Msg.MsgID == 537001985)
        {
            try
            {
                UpdateLogbook();
                if (AoWHSMap.Current.FDay == 1)
                {
                    FViewX = 255;
                    if (AoWHSMap.Current.FGameType != GameType.Campaign && !AoWHSMap.Current.FFixedLeaders && FLeader != null)
                    {
                        FLeader.Owner = null;
                        FMagicControl.UnregisterPowerSource(FLeader.FPowerSource);
                        // { Original removes the player, not the leader. }
                        FHeroes.RemoveChild(this);
                    }

                    UpdateAlliedPlayersSeen();
                    SetupAIValidAttackPlayers();
                }
            }
            catch
            {
            }
        }

        if (Msg.MsgID == 537002243)
        {
            if (!Validate(Msg.Payload.As<StringCollection>()))
            {
                Msg.Data[0] = 1;
            }
        }

        if (Msg.MsgID == 537001986 && Msg.Payload.Int == FPlayerIndex)
        {
            UpdateAlliedPlayersSeen();
        }

        if (Msg.MsgID == 537001987 && Msg.Payload.Int == FPlayerIndex)
        {
            UpdateTurnInfo();
            if (FPlayerIndex != 0)
            {
                UpdateHeroJoin();
            }
        }

        if (Msg.MsgID == 536870920 && AoWHSMap.Current.FDay == 0)
        {
            FViewX = 255;
            FViewY = 255;
            FViewL = 255;
        }

        FMagicControl.MsgProc(ref Msg, Sender);
        FAIControl.MsgProc(ref Msg, Sender);
        FAIBudget.MsgProc(ref Msg, Sender);
        FEventLogbook.MsgProc(ref Msg, Sender);
    }

    // Player.PAS:394
    public bool GetBusy()
    {
        bool Result = FBusyCount != 0 || !AoWHSMap.Current.FTokenManager.PlayerReady(FPlayerIndex);
        Result = Result || (AoWHSMap.Current.FCombat == null && AoWHSMap.Current.FPlayMode == PlayMode.Turn && AoWHSMap.Current.FCurrentPlayer != FPlayerIndex);
        Result = Result || AoWHSMap.Current.FCombat != null && AoWHSMap.Current.FCombat.FCombatData.FPlayers.GetPlayers((sbyte)AoWHSMap.Current.FCombat.FCombatData.FCurrentPlayer).FPlayer != FPlayerIndex;
        Result = Result || FGameOverStatus != GameOverStatus.None || !GetLocal();
        return Result || FPlayerType == PlayerType.Human && AoWHSMap.Current.GetTurnTimerEnabled() && AoWHSMap.Current.GetTurnTimeLeft() <= 0;
    }

    // Player.PAS:406
    public int GetSuperiority()
    {
        int Result;
        if (FStatistics.FTurnInfo.Count != 0)
        {
            var count = FStatistics.FTurnInfo.Count;
            Result = FStatistics.FTurnInfo.GetTurnInfo(count - 1).GetSuperiority();
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Player.PAS:412
    public int GetUnitCount()
    {
        int Result = 0;
        for (int I = FArmies.Count - 1; I >= 0; I--)
        {
            Result += FArmies.GetArmy(I).Count;
        }

        return Result;
    }

    // Player.PAS:420
    public void SetPlayerIndex(sbyte Value)
    {
        FPlayerIndex = Value;
        FEventLogbook.FPlayer = Value;
    }

    // Player.PAS:425
    public void SetGameOverStatus(GameOverStatus Value)
    {
        GameOverEventLog Event;
        PlayerStructureList Structures;
        ArmyList Armies;
        Campaign Campaign;
        if (Value != FGameOverStatus && FGameOverStatus == GameOverStatus.None)
        {
            FGameOverStatus = Value;
            AoWHSMap.Current.FAIExecuter.DeactivatePlayer(FPlayerIndex);
            if (AoWHSMap.Current.FGameType == GameType.Campaign && FPlayerType == PlayerType.Human)
            {
                if (FGameOverStatus == GameOverStatus.Lost)
                {
                    AoWHSMap.Current.FGameResult = AoWHSMap.GameResult.Failed;
                    Campaign = GameTables.AoWEngine.FCampaign;
                    if (Campaign != null)
                    {
                        Campaign.RestartScenario();
                    }
                }
                else
                {
                    if (FGameOverStatus == GameOverStatus.Won)
                    {
                        AoWHSMap.Current.FGameResult = AoWHSMap.GameResult.Succeeded;
                        Campaign = GameTables.AoWEngine.FCampaign;
                        if (Campaign != null)
                        {
                            Campaign.FinishScenario();
                        }
                    }
                }
            }

            if (FGameOverStatus == GameOverStatus.Lost)
            {
                BeginUpdate();
                FMagicControl.RemoveEnchantments();
                Structures = FStructures;
                for (int I = Structures.Count - 1; I >= 0; I--)
                {
                    FStructures.GetStructure(I).SetPlayer(0);
                }

                Armies = FArmies;
                for (int I = Armies.Count - 1; I >= 0; I--)
                {
                    FArmies.GetArmy(I).SetPlayer(0);
                }

                FMagicControl.SetMana(0);
                FGems = 0;
                EndUpdate();
                AoWHSMap.Current.FPlayers.Update();
            }

            Event = new GameOverEventLog(null);
            Event.FPlayer = FPlayerIndex;
            if (Value == GameOverStatus.Lost)
            {
                var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[FPlayerRace]);
                var name = GetName();
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameOverDefeatMessageRStr), [IntToStr(AoWHSMap.Current.FDay), raceNamesPlural, name]);
                Event.FMessage.Text = format;
            }
            else
            {
                var raceNamesPluralText = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[FPlayerRace]);
                var name = GetName();
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameOverVictoryMessageRStr), [IntToStr(AoWHSMap.Current.FDay), raceNamesPluralText, name]);
                Event.FMessage.Text = format;
            }

            AoWHSMap.Current.ExecuteGameOverEvent(Event);
            Event.Release();
            if (FGameOverStatus == GameOverStatus.Lost)
            {
                AoWHSMap.Current.FGlobalMagicControl.RemovePlayerEnchantments(FPlayerIndex);
            }
        }
    }

    // Player.PAS:484
    public bool Surrender()
    {
        bool Result;
        GameOverTokenEvent TE;
        if (!GetBusy() && FGameOverStatus == GameOverStatus.None)
        {
            TE = new GameOverTokenEvent(null);
            TE.FPlayer = FPlayerIndex;
            TE.FGameOverStatus = GameOverStatus.Lost;
            AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayerIndex).AddEvent(TE);
            TE.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Player.PAS:499
    public bool Win()
    {
        bool Result;
        GameOverTokenEvent TE;
        if (!GetBusy() && FGameOverStatus == GameOverStatus.None)
        {
            TE = new GameOverTokenEvent(null);
            TE.FPlayer = FPlayerIndex;
            TE.FGameOverStatus = GameOverStatus.Won;
            AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayerIndex).AddEvent(TE);
            TE.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Player.PAS:515
    public bool Validate(StringCollection Errors)
    {
        bool Result;
        string S;
        string LeaderName;
        if (FPlayerIndex > 0)
        {
            Result = true;
            if (FLeader.GetSphereCount() != AoWHSMap.Current.FSphereCount)
            {
                var raceNames = AbilityRules.TranslateRStr(GameTables.RaceNamesRStr[FPlayerRace]);
                LeaderName = AbilityRules.Format(XLeaderRStr, [raceNames]);
                S = LeaderName + ": " + AbilityRules.TranslateRStr(NotAllSpheresAssignedRStr);
                Errors.Add(S);
                Result = false;
            }

            if (FLeader.Owner == null)
            {
                var raceNamesText = AbilityRules.TranslateRStr(GameTables.RaceNamesRStr[FPlayerRace]);
                LeaderName = AbilityRules.Format(XLeaderRStr, [raceNamesText]);
                S = LeaderName + ": " + AbilityRules.TranslateRStr(NotPlacedOnMapRStr);
                Errors.Add(S);
                Result = false;
            }

            if (FLeader.GetSkillPoints() != 0)
            {
                var translateRStr = AbilityRules.TranslateRStr(GameTables.RaceNamesRStr[FPlayerRace]);
                LeaderName = AbilityRules.Format(XLeaderRStr, [translateRStr]);
                S = LeaderName + ": " + AbilityRules.TranslateRStr(NotAllSkillPointsAssignedRStr);
                Errors.Add(S);
            }
        }
        else
        {
            // { Original reports unassigned skill points without failing validation. }
            Result = true;
        }

        return Result;
    }

    // Player.PAS:547
    public void LockBusy()
    {
        FBusyCount += 1;
    }

    // Player.PAS:552
    public void UnlockBusy()
    {
        Assert(FBusyCount != 0, "Invalid busy counter usage", "Player.PAS", 554);
        FBusyCount -= 1;
    }

    // Player.PAS:558
    public void LogbookChangedEvent(object Sender)
    {
        if (FUpdateCount == 0)
        {
            FLogbookChangedEventsStore.Raise(this);
        }
    }

    // Player.PAS:563
    public void BeginUpdate()
    {
        FUpdateCount += 1;
        FStructures.BeginUpdate();
        FArmies.BeginUpdate();
        FMagicControl.BeginUpdate();
    }

    // Player.PAS:571
    public void EndUpdate()
    {
        FUpdateCount -= 1;
        if (FUpdateCount == 0)
        {
            FGemsChangedEventsStore.Raise(this);
            FUpkeepChangedEventsStore.Raise(this);
            UpdateIncome();
            FLogbookChangedEventsStore.Raise(this);
        }

        FStructures.EndUpdate();
        FArmies.EndUpdate();
        FMagicControl.EndUpdate();
    }

    // Player.PAS:586
    public void ArmiesChanged(object Sender)
    {
        if (FArmies.Count == 0)
        {
            AoWHSMap.Current.GameOver(FPlayerIndex, GameOverStatus.Lost);
        }
    }

    // Player.PAS:591
    public void SetGems(int Value)
    {
        FGems = Value;
        if (FUpdateCount == 0)
        {
            FGemsChangedEventsStore.Raise(this);
        }

        if (AoWHSMap.Current.FVictoryCondition == AoWHSMap.VictoryCondition.AccumulateGold && AoWHSMap.Current.FVictoryGold <= FGems)
        {
            AoWHSMap.Current.GameOver(FPlayerIndex, GameOverStatus.Won);
        }
    }

    // Player.PAS:600
    public void UpdateIncome()
    {
        FIncome = FBaseIncome;
        for (int I = FIncomeSources.Count - 1; I >= 0; I--)
        {
            var income = ((IncomeSource)FIncomeSources.FChildren.List[I]).Income();
            FIncome += income;
        }

        if (FPlayerType == PlayerType.CPU2)
        {
            FIncome = Round((double)FIncome * 1);
        }
        else
        {
            if (FPlayerType == PlayerType.CPU3)
            {
                FIncome = Round((double)FIncome * 2);
            }
            else
            {
                if (FPlayerType == PlayerType.CPU4)
                {
                    FIncome = Round((double)FIncome * 2.5) + 50;
                }
                else
                {
                    if (FPlayerType == PlayerType.CPU5)
                    {
                        FIncome = Round(FIncome * 3) + 100;
                    }
                }
            }
        }

        if (FUpdateCount == 0)
        {
            FIncomeChangedEventsStore.Raise(this);
        }
    }

    // Player.PAS:614
    public void RegisterIncomeSource(IncomeSource Source)
    {
        if (FIncomeSources.FChildren.IndexOf(Source) == -1)
        {
            FIncomeSources.FChildren.Add(Source);
            if (FUpdateCount == 0)
            {
                UpdateIncome();
            }
        }
    }

    // Player.PAS:623
    public void UnregisterIncomeSource(IncomeSource Source)
    {
        if (FIncomeSources.FChildren.Remove(Source) != -1 && FUpdateCount == 0)
        {
            UpdateIncome();
        }
    }

    // Player.PAS:629
    public int GetUpkeep() => FArmyUpkeep + FMagicUpkeep;

    // Player.PAS:634
    public void SetArmyUpkeep(int Value)
    {
        FArmyUpkeep = Value;
        if (FUpdateCount == 0)
        {
            FUpkeepChangedEventsStore.Raise(this);
        }
    }

    // Player.PAS:640
    public void SetMagicUpkeep(int Value)
    {
        FMagicUpkeep = Value;
        if (FUpdateCount == 0)
        {
            FUpkeepChangedEventsStore.Raise(this);
        }
    }

    // Player.PAS:646
    public string GetName()
    {
        string Result;
        if (FPlayerIndex != 0)
        {
            if (FLeader != null)
            {
                Result = FLeader.FHeroName;
                if (!IsEmpty(FLeader.GetNickname()))
                {
                    var nickname = FLeader.GetNickname();
                    Result = Result + " " + nickname;
                }
            }
            else
            {
                Result = FName;
            }

            if (IsEmpty(Result))
            {
                Result = AbilityRules.TranslateRStr(PlayerRStr) + " " + IntToStr(FPlayerIndex);
            }
        }
        else
        {
            Result = AbilityRules.TranslateRStr(GameTables.IndependentsRStr);
        }

        return Result;
    }

    // Player.PAS:663
    public string GetNameEx()
    {
        string Result;
        if (FPlayerIndex != 0)
        {
            var name = GetName();
            var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[FPlayerRace]);
            Result = AbilityRules.Format(AbilityRules.TranslateRStr(PlayerNameExRStr), [name, raceNamesPlural]);
        }
        else
        {
            Result = GetName();
        }

        return Result;
    }

    // Player.PAS:671
    public void SetName(string Value)
    {
        if (FPlayerIndex != 0)
        {
            if (FLeader != null)
            {
                FLeader.FHeroName = Value;
                FLeader.SetNickname("");
            }
            else
            {
                FName = Value;
            }
        }
    }

    // Player.PAS:682
    public LibraryImage GetFace()
    {
        LibraryImage Result;
        if (FLeader != null)
        {
            Result = FLeader.GetFace();
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Player.PAS:688
    public int GetHeroCountMax()
    {
        int Result;
        int Strength;
        int Maximum;
        var count = FStatistics.FTurnInfo.Count;
        Strength = FStatistics.FTurnInfo.GetTurnInfo(count - 1).FArmyStrength;
        if (Strength < 10)
        {
            Result = 1;
        }
        else
        {
            if (Strength < 40)
            {
                Result = 2;
            }
            else
            {
                if (Strength < 80)
                {
                    Result = 3;
                }
                else
                {
                    if (Strength < 120)
                    {
                        Result = 4;
                    }
                    else
                    {
                        if (Strength < 200)
                        {
                            Result = 5;
                        }
                        else
                        {
                            Result = (Strength - 200) / 100 + 5;
                        }
                    }
                }
            }
        }

        Maximum = AoWHSMap.Current.FHeroCountMax;
        if (Maximum < Result)
        {
            Result = Maximum;
        }

        return Result;
    }

    // Player.PAS:703
    public int GetIdealHeroJoinDay()
    {
        int Average;
        int Maximum;
        int Result = FLastHeroJoinDay;
        Average = AoWHSMap.Current.FPlayers.GetAverageHeroCount();
        Maximum = AoWHSMap.Current.FPlayers.GetMaxHeroCount();
        if (Maximum > Average && FHeroes.Count == Maximum)
        {
            Result += 15;
        }
        else
        {
            if (Average <= FHeroes.Count && FHeroes.Count != 0 && AoWHSMap.Current.FDay > 15)
            {
                Result += 10;
            }
            else
            {
                Result += 5;
            }
        }

        return Result;
    }

    // Player.PAS:717
    public int GetIdealHeroEmergeDay()
    {
        int Average;
        int Maximum;
        int Earliest;
        int Result = GetIdealHeroJoinDay();
        Average = AoWHSMap.Current.FPlayers.GetAverageHeroCount();
        Maximum = AoWHSMap.Current.FPlayers.GetMaxHeroCount();
        if (Maximum > Average && FHeroes.Count == Maximum)
        {
            Result += 15;
        }
        else
        {
            if (Average < FHeroes.Count && FHeroes.Count != 0 && AoWHSMap.Current.FDay > 10)
            {
                Result += 10;
            }
            else
            {
                Result += 5;
            }
        }

        Earliest = FLastHeroEmergeDay + 5;
        if (Earliest > Result)
        {
            Result = Earliest;
        }

        return Result;
    }

    // Player.PAS:733
    public void ResetBaseHeroJoinDay()
    {
        FLastHeroJoinDay = AoWHSMap.Current.FDay;
    }

    // Player.PAS:738
    public void MakeHeroEmerge(HeroEmergeMode Mode)
    {
        int Chance = default;
        DoubleItemList<Hero> Heroes;
        HeroEmergeEventLog Log;
        MapCoordinateList Locations;
        Structure Structure;
        DoubleItem<Hero> Item;
        int I;
        Hero Hero;
        Heroes = new DoubleItemList<Hero>();
        if (Mode == HeroEmergeMode.Normal)
        {
            I = AoWHSMap.Current.FHeroControl.GetCount() - 1;
            for (; I >= 0; I -= 1)
            {
                Hero = AoWHSMap.Current.FHeroControl.GetHero(I);
                if (!Hero.FHeroStatus.Contains(HeroStatusElement.Died) && Hero.FPlayer <= 0 && Hero.Owner == null && Hero.CanJoin(FPlayerIndex))
                {
                    Chance = GetHeroJoinChance(Hero.GetLevel());
                    if (Chance > 0 && Hero.JoinAmount(FPlayerIndex) < FGems)
                    {
                        Heroes.AddAB(Hero, Chance * Chance);
                    }
                }
            }
        }
        else
        {
            if (Mode == HeroEmergeMode.FirstHero)
            {
                I = AoWHSMap.Current.FHeroControl.GetCount() - 1;
                for (; I >= 0; I -= 1)
                {
                    Hero = AoWHSMap.Current.FHeroControl.GetHero(I);
                    if (!Hero.FHeroStatus.Contains(HeroStatusElement.Died) && Hero.FPlayer <= 0 && Hero.Owner == null && Hero.GetAbilityEnabled(52) && Hero.GetLevel() >= 2 && Hero.GetLevel() <= 3)
                    {
                        var playerRelation = AoWHSMap.Current.FRaces.GetRace(Hero.FRace).GetPlayerRelation(FPlayerIndex);
                        if (Set<Session.Status>.Of(Session.Status.Polite, Session.Status.Friendly).Contains(playerRelation) && Hero.JoinAmount(FPlayerIndex) < FGems)
                        {
                            var level = Hero.GetLevel();
                            Heroes.AddAB(Hero, (level - 2) * 1000 + 1);
                        }
                    }
                }
            }
        }

        if (Heroes.Count != 0)
        {
            Locations = new MapCoordinateList();
            I = FStructures.Count - 1;
            for (; I >= 0; I -= 1)
            {
                Structure = FStructures.GetStructure(I);
                if (Structure.EClass.ClassID == 131336)
                {
                    var lhx = Structure.GetLhx();
                    var yhx = Structure.GetYhx();
                    Locations.AddXYL(Structure.GetXhx(), yhx, lhx);
                }
            }

            if (Locations.Count != 0)
            {
                FLastHeroEmergeDay = AoWHSMap.Current.FDay;
                RandSeed = AoWHSMap.Current.Random(1048575);
                I = Heroes.RandomChanceItemB(0);
                Item = Heroes[I];
                Hero = Item.A;
                I = AoWHSMap.Current.Random(Locations.Count);
                FEmergingHeroID = Hero.FUnitID;
                Hero.UpdateSettings();
                var item = Locations[I];
                var locationsItem = Locations[I];
                Hero.Place(Locations[I].X, locationsItem.Y, item.L, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea, UnitPlaceModeElement.CreateNewArmy));
                var item2 = Locations[I];
                var item3 = Locations[I];
                var item4 = Locations[I];
                Structure = (Structure)AoWHSMap.Current.MapContainer.GetField(item4.X, item3.Y, item2.L).FindNoneTransparentHS(typeof(Structure));
                Log = new HeroEmergeEventLog(null);
                Log.Setup(Hero, FPlayerIndex, HeroEmergeEventLog.HeroJoinMode.Emerged);
                if (Structure != null)
                {
                    Log.FLocationName = Structure.GetName();
                }
                else
                {
                    Log.FLocationName = AbilityRules.TranslateRStr(GameTables.UnknownRStr);
                }

                FEventLogbook.AddEvent(Log);
                if (FPlayerIndex == AoWHSMap.Current.FSeatedPlayer && AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FPlayerType == PlayerType.Human)
                {
                    Log.Execute(false, ExecuteTrigger.Event, null);
                }

                Log.Release();
            }

            Locations?.Dispose();
        }

        Heroes?.Dispose();
    }

    // Player.PAS:812
    public void UpdateHeroJoin()
    {
        int Days;
        for (int I = FHeroes.Count - 1; I >= 0; I--)
        {
            if (FHeroes.GetHero(I).GetLevel() > FHighestHeroLevel)
            {
                FHighestHeroLevel = FHeroes.GetHero(I).GetLevel();
            }
        }

        FBaseHeroJoinLevel = AoWHSMap.Current.FPlayers.GetAverageHeroLevel();
        if (FBaseHeroJoinLevel == 0)
        {
            FBaseHeroJoinLevel = AoWHSMap.Current.FHeroControl.GetMinFreeHeroLevel();
        }

        if (FHighestHeroLevel != 0)
        {
            FBaseHeroJoinLevel = (FBaseHeroJoinLevel + FHighestHeroLevel + 1) / 2;
        }

        if (FHeroes.Count == GetHeroCountMax())
        {
            ResetBaseHeroJoinDay();
        }

        FEmergingHeroID = 0;
        Days = GetIdealHeroEmergeDay() - AoWHSMap.Current.FDay;
        if (AoWHSMap.Current.FDay == 1 && FHeroes.Count == 1 && !AoWHSMap.Current.FFixedLeaders)
        {
            MakeHeroEmerge(HeroEmergeMode.FirstHero);
            return;
        }

        if (Days <= 0)
        {
            if (AoWHSMap.Current.Random(4) == 1)
            {
                MakeHeroEmerge(HeroEmergeMode.Normal);
            }
        }
    }

    // Player.PAS:836
    public int GetHeroJoinChance(int Level)
    {
        int Result;
        int Maximum;
        Maximum = GetHeroJoinLevelMax();
        if (Level <= Maximum)
        {
            Result = Round((double)((double)(Maximum + 2) / (Level + 2)) * 20);
            if (Result > 40)
            {
                Result = 40;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Player.PAS:849
    public int GetHeroJoinLevelMax()
    {
        int Days;
        int Result = FBaseHeroJoinLevel;
        var idealHeroJoinDay = GetIdealHeroJoinDay();
        Days = AoWHSMap.Current.FDay - idealHeroJoinDay;
        if (Days > 0)
        {
            Result += Days / 5;
        }

        if (Days < 0)
        {
            Result += Round((double)((double)(Result * Days) + 2.5) / 5);
            if (Result < 0)
            {
                Result = 0;
            }
        }

        if (Result < 3 && !AoWHSMap.Current.FFixedLeaders && AoWHSMap.Current.FDay == 1)
        {
            Result = 3;
        }

        return Result;
    }

    // Player.PAS:865
    public void ValidateLeader()
    {
        int FaceIndex;
        int ResourceIndex;
        HeroType HeroType;
        string Name;
        if (AoWHSMap.Current != null && FPlayerIndex != 0)
        {
            HeroType = Units.HeroType.HeroLeader;
            if (FLeader != null && FLeader.GetHeroType() != HeroType)
            {
                Name = FLeader.FHeroName;
                FaceIndex = FLeader.FFaceIndex;
                FLeader.Deactivate();
                ResourceIndex = AoWHSSet.Current.FHeroResources.Find(FPlayerRace, HeroType);
                FLeader.SetHeroResourceIndex(ResourceIndex);
                FLeader.FHeroName = Name;
                FLeader.SetFaceIndex(FaceIndex);
                FLeader.Activate();
            }
            else
            {
                if (FLeader == null)
                {
                    ResourceIndex = AoWHSSet.Current.FHeroResources.Find(FPlayerRace, HeroType);
                    if (ResourceIndex != -1)
                    {
                        FLeader = new Leader(null);
                        FLeader.SetHeroResourceIndex(ResourceIndex);
                        FLeader.FPlayer = FPlayerIndex;
                        FLeader.Activate();
                    }
                }
                else
                {
                    if (FLeader != null && !AoWHSMap.Current.FFixedLeaders && AoWHSMap.Current.FGameType != GameType.Campaign && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                    {
                        FLeader.Owner = null;
                    }
                }
            }
        }
    }

    // Player.PAS:902
    public void SetLeader(Leader Value)
    {
        FLeader = Value;
    }

    // Player.PAS:907
    public void SetRace(sbyte Value)
    {
        FPlayerRace = Value;
    }

    // Player.PAS:912
    public void SetEndTurn(bool Value)
    {
        if (Value != FSingleProcessCycle)
        {
            FSingleProcessCycle = Value;
            AoWHSMap.Current.TriggerEndTurnEvent(FPlayerIndex);
        }
    }

    // Player.PAS:921
    public void UpdateLogbook()
    {
        while (true)
        {
            bool condition = FEventLogbook.GetCount() != 0;
            if (condition)
            {
                var logs = FEventLogbook.GetLogs(0);
                condition = AoWHSMap.Current.FDay - logs.FDay >= 3;
            }

            if (!condition)
                break;
            var fEventLogbookLogs = FEventLogbook.GetLogs(0);
            FEventLogbook.RemoveEvent(fEventLogbookLogs);
        }
    }

    // Player.PAS:928
    public void UpdateTurnInfo()
    {
        TurnInfoItem Info;
        Army Army;
        Troop AUnit;
        Info = new TurnInfoItem(FStatistics.FTurnInfo);
        if (FGameOverStatus != GameOverStatus.Lost)
        {
            Info.FGold = FGems;
            Info.FMana = FMagicControl.FMana;
            Info.FStructures = FStructures.Count;
            Info.FHeroes = (byte)FHeroes.Count;
            for (int I = 0, last = FArmies.Count - 1; I <= last; I++)
            {
                Army = FArmies.GetArmy(I);
                for (int J = Army.Count - 1; J >= 0; J--)
                {
                    AUnit = Army.GetUnit(J);
                    if (Info.FArmyStrength < 1073741823)
                    {
                        var strengthMax = AUnit.GetStrengthMax(AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall));
                        Info.FArmyStrength += strengthMax;
                    }
                }
            }
        }
    }

    // Player.PAS:955
    public void UpdateAlliedPlayersSeen()
    {
        for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            if (I != FPlayerIndex)
            {
                if (AoWHSMap.Current.FPlayers.GetDiplomaticRelation(FPlayerIndex, (sbyte)I) == DiplomaticRelation.Alliance)
                {
                    PlayerSeen((sbyte)I);
                }
            }
        }
    }

    // Player.PAS:965
    public void SetupAIValidAttackPlayers()
    {
        for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            if (I != FPlayerIndex)
            {
                var diplomaticRelation = FDiplomaticRelations.GetDiplomaticRelation((sbyte)I);
                FAIValidAttackPlayers[I] = diplomaticRelation == DiplomaticRelation.War;
            }
        }
    }

    // Player.PAS:1108
    public int GetFirstDaySeen(sbyte Player)
    {
        int Result;
        if (Player != FPlayerIndex)
        {
            if (Player < FFirstDaySeen.Count)
            {
                Result = FFirstDaySeen[Player];
            }
            else
            {
                Result = -1;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Player.PAS:1116
    public int GetLastDaySeen(sbyte Player)
    {
        int Result;
        if (Player != FPlayerIndex)
        {
            if (Player < FLastDaySeen.Count)
            {
                Result = FLastDaySeen[Player];
            }
            else
            {
                Result = -1;
            }
        }
        else
        {
            Result = AoWHSMap.Current.FDay;
        }

        return Result;
    }

    // Player.PAS:1124
    public void PlayerSeen(sbyte Player)
    {
        if (!AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && (Player >= FLastDaySeen.Count || FLastDaySeen[Player] != AoWHSMap.Current.FDay))
        {
            FLastDaySeen[Player] = AoWHSMap.Current.FDay;
            if (GetFirstDaySeen(Player) == -1)
            {
                FFirstDaySeen[Player] = AoWHSMap.Current.FDay;
            }
        }
    }

    // Player.PAS:1134
    public void GetLeaderLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        if (FLeader != null && FLeader.UnitListOwner != null)
        {
            FLeader.UnitListOwner.GetLocation(ref X, ref Y, ref L);
        }
        else
        {
            X = -1;
            Y = -1;
            L = -1;
        }
    }

    // Player.PAS:1146
    public bool GetLocal()
    {
        bool Result;
        PlayerCommunicationControl Control;
        Control = AoWHSMap.Current.FPlayerCommunicator.PCC;
        if (Control != null)
        {
            var players = Control.GetPlayers(FPlayerIndex);
            Result = Set<ConnectionMode>.Of(ConnectionMode.Local, ConnectionMode.Global).Contains(players.ConnectionMode);
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Player.PAS:1159
    public void RegisterVictoryLocation(PlayerStructure Structure)
    {
        FVictoryLocations.AddChild(Structure);
    }

    // Player.PAS:1164
    public void UnregisterVictoryLocation(PlayerStructure Structure)
    {
        FVictoryLocations.RemoveChild(Structure);
    }

    // Player.PAS:1169
    public void CheckCaptureLocations()
    {
        if (AoWHSMap.Current.FVictoryCondition == AoWHSMap.VictoryCondition.CaptureLocations)
        {
            if (CaptureLocationsLeft() == 0)
            {
                AoWHSMap.Current.GameOver(FPlayerIndex, GameOverStatus.Won);
            }
        }
    }

    // Player.PAS:1175
    public int CaptureLocationsLeft()
    {
        int Result = 0;
        for (int I = 0, last = FVictoryLocations.Count - 1; I <= last; I++)
        {
            if (FVictoryLocations.GetStructure(I).FPlayer != FPlayerIndex)
            {
                bool condition = FVictoryLocations.GetStructure(I).FPlayer > 0;
                if (condition)
                {
                    var structure = FVictoryLocations.GetStructure(I);
                    condition = AoWHSMap.Current.FPlayers.GetDiplomaticRelation(structure.FPlayer, FPlayerIndex) == DiplomaticRelation.Alliance;
                }

                if (!(condition && AoWHSMap.Current.FAlliedVictory))
                {
                    Result += 1;
                }
            }
        }

        return Result;
    }

    // Player.PAS:1188
    public DiplomaticAction CreateDiplomaticAction(DiplomaticProposal Proposal, sbyte Target, sbyte Other)
    {
        DiplomaticAction Result;
        if (AoWHSMap.Current.FPlayers.GetPlayers(Target).FStatus == Status.Playing)
        {
            Result = new DiplomaticAction(null);
            Result.FSource = FPlayerIndex;
            Result.FTarget = Target;
            Result.FOtherPlayer = Other;
            Result.Proposal = Proposal;
            Result.RetrieveDefaultMessage(FPlayerIndex, Target, Other, DiplomaticActionReaction.None);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Player.PAS:1203
    public bool DiplomaticActionAleadySend(byte Kind, sbyte Target, sbyte Other)
    {
        EventLogbook Logbook;
        int I;
        DiplomaticAction Item;
        Logbook = AoWHSMap.Current.FPlayers.GetPlayers(Target).FEventLogbook;
        bool Result = false;
        I = Logbook.GetCount();
        while (!Result && I != 0)
        {
            I -= 1;
            if (Logbook.GetLogs(I).FDay == AoWHSMap.Current.FDay)
            {
                if (Logbook.GetLogs(I) is DiplomaticActionEventLog)
                {
                    Item = ((DiplomaticActionEventLog)Logbook.GetLogs(I)).FAction;
                    Result = Item.Proposal.ToFields().Kind == Kind && Item.FSource == FPlayerIndex && Item.FTarget == Target && (Other == -1 || Item.FOtherPlayer == Other);
                }
            }
        }

        return Result;
    }

    // Player.PAS:1226
    public int ListDiplomaticActions(sbyte Player, ref DiplomaticActionList Actions)
    {
        int Count;
        DiplomaticRelation Relation;
        DiplomaticAction Action;
        if (Player != FPlayerIndex && GetFirstDaySeen(Player) != -1)
        {
            Count = Actions.Count;
            Relation = AoWHSMap.Current.FPlayers.GetDiplomaticRelation(Player, FPlayerIndex);
            if (FPlayerType == PlayerType.Human && AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType == PlayerType.Human)
            {
                Action = CreateDiplomaticAction(new SendMessage(), Player, -1);
                if (Action != null)
                {
                    Actions.AddChild(Action);
                    Action.Release();
                }
            }

            if (Set<DiplomaticRelation>.Of(DiplomaticRelation.War, DiplomaticRelation.Unknown).Contains(Relation) && !DiplomaticActionAleadySend(OfferPeace.Kind, Player, -1))
            {
                Action = CreateDiplomaticAction(new OfferPeace(), Player, -1);
                if (Action != null)
                {
                    Actions.AddChild(Action);
                    Action.Release();
                }
            }

            if (Relation != DiplomaticRelation.War && Relation != DiplomaticRelation.Alliance)
            {
                Action = CreateDiplomaticAction(new DeclareWar(), Player, -1);
                if (Action != null)
                {
                    Actions.AddChild(Action);
                    Action.Release();
                }
            }

            if (Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace).Contains(Relation) && !DiplomaticActionAleadySend(ProposeAlliance.Kind, Player, -1))
            {
                Action = CreateDiplomaticAction(new ProposeAlliance(), Player, -1);
                if (Action != null)
                {
                    Actions.AddChild(Action);
                    Action.Release();
                }
            }

            if (Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Unknown).Contains(Relation) && !DiplomaticActionAleadySend(ThreatenToAttack.Kind, Player, -1))
            {
                Action = CreateDiplomaticAction(new ThreatenToAttack(), Player, -1);
                if (Action != null)
                {
                    Actions.AddChild(Action);
                    Action.Release();
                }
            }

            Action = CreateDiplomaticAction(new MakeTribute(), Player, -1);
            if (Action != null)
            {
                Actions.AddChild(Action);
                Action.Release();
            }

            if (Set<DiplomaticRelation>.Of(DiplomaticRelation.Alliance).Contains(Relation))
            {
                Action = CreateDiplomaticAction(new BreakAlliance(), Player, -1);
                if (Action != null)
                {
                    Actions.AddChild(Action);
                    Action.Release();
                }
            }

            for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
            {
                if (I != Player && I != FPlayerIndex && AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus == Status.Playing)
                {
                    bool condition = !DiplomaticActionAleadySend(SuggestWarDeclaration.Kind, Player, (sbyte)I) && AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).GetFirstDaySeen((sbyte)I) != -1 && AoWHSMap.Current.FPlayers.GetPlayers(Player).GetFirstDaySeen((sbyte)I) != -1;
                    if (condition)
                    {
                        var diplomaticRelation = AoWHSMap.Current.FPlayers.GetDiplomaticRelation(Player, (sbyte)I);
                        condition = Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Unknown).Contains(diplomaticRelation);
                    }

                    if (condition)
                    {
                        Action = CreateDiplomaticAction(new SuggestWarDeclaration(), Player, (sbyte)I);
                        if (Action != null)
                        {
                            Actions.AddChild(Action);
                            Action.Release();
                        }
                    }
                }
            }

            for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
            {
                if (I != Player && I != FPlayerIndex && AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus == Status.Playing)
                {
                    if (!DiplomaticActionAleadySend(SuggestAllianceBreak.Kind, Player, (sbyte)I) && AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).GetFirstDaySeen((sbyte)I) != -1 && AoWHSMap.Current.FPlayers.GetPlayers(Player).GetFirstDaySeen((sbyte)I) != -1 && AoWHSMap.Current.FPlayers.GetDiplomaticRelation(Player, (sbyte)I) == DiplomaticRelation.Alliance)
                    {
                        Action = CreateDiplomaticAction(new SuggestAllianceBreak(), Player, (sbyte)I);
                        if (Action != null)
                        {
                            Actions.AddChild(Action);
                            Action.Release();
                        }
                    }
                }
            }

            if (FPlayerRace == 5 && FHeroEmerging && FPlayerType == PlayerType.Human && AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType == PlayerType.Human)
            {
                for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
                {
                    if (I != Player && I != FPlayerIndex && AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus == Status.Playing && AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType == PlayerType.Human)
                    {
                        if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).GetFirstDaySeen((sbyte)I) != -1 && AoWHSMap.Current.FPlayers.GetPlayers(Player).GetFirstDaySeen((sbyte)I) != -1 && AoWHSMap.Current.FPlayers.GetPlayers(I).GetFirstDaySeen(Player) != -1)
                        {
                            Action = CreateDiplomaticAction(new ImpersonatedMessage(), Player, (sbyte)I);
                            if (Action != null)
                            {
                                Actions.AddChild(Action);
                                Action.Release();
                            }
                        }
                    }
                }
            }

            Count = Actions.Count - Count;
        }
        else
        {
            Count = 0;
        }

        return Count;
    }

    // Player.PAS:1348
    public bool ValidAttackRelation(sbyte Player, sbyte IndependentRelation)
    {
        bool Result;
        Assert(FPlayerIndex != 0, "", "Player.PAS", 1390);
        if (Set<PlayerType>.Of(PlayerType.CPU1, PlayerType.CPU2, PlayerType.CPU3, PlayerType.CPU4, PlayerType.CPU5).Contains(FPlayerType))
        {
            Result = FAIValidAttackPlayers[Player];
        }
        else
        {
            var diplomaticRelation = AbilityRules.DiplomaticRelation(FPlayerIndex, 0, Player, IndependentRelation);
            Result = Set<DiplomaticRelation>.Of(DiplomaticRelation.War, DiplomaticRelation.Unknown).Contains(diplomaticRelation);
        }

        return Result;
    }

    // Player.PAS:1398
    public bool ChangePlayerSettings(string Settings, ref string Error)
    {
        bool Result;
        ChangePlayerSettingsTokenEvent Event;
        if (!GetBusy())
        {
            Event = new ChangePlayerSettingsTokenEvent(null);
            Event.FPlayer = FPlayerIndex;
            Event.FSettings = Settings;
            AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayerIndex).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static readonly string PlayerRStr = "Player";
    private static readonly string PlayerNameExRStr = "%0:s of the %1:s";
    private static readonly string GameOverDefeatMessageRStr = "On day %0:s, the empire of the %1:s, led by %2:s was defeated.";
    private static readonly string GameOverVictoryMessageRStr = "On day %0:s, the empire of the %1:s, led by %2:s was victorious.";
    private static readonly string XLeaderRStr = "%s leader";
    private static readonly string NotAllSpheresAssignedRStr = "not all spheres assigned";
    private static readonly string NotPlacedOnMapRStr = "not placed on map";
    private static readonly string NotAllSkillPointsAssignedRStr = "not all skill points assigned";

    public enum Behavior : byte
    {
        Normal = 0, Aggressor = 1, Defender = 2, Expander = 3, Scorcher = 4, Random = 5
    }

    public enum Status : byte
    {
        Idle = 0, Playing = 1, Terminated = 2
    }

    public enum HeroEmergeMode : byte
    {
        Normal = 0, FirstHero = 1
    }

    // Kind 4: the source threatens to attack the target.
    public sealed partial record ThreatenToAttack : DiplomaticProposal
    {
        public const byte Kind = 4;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Threat;

        public override int Icon => 46;

        public override string Name(DiplomaticAction Action) => AbilityRules.TranslateRStr(P5ThreatenToAttackRStr);

        public override string LogText(DiplomaticAction Action) => AbilityRules.TranslateRStr(P5ThreatRStr);

        public override void DefaultMessage(DiplomaticMessageText Text, DiplomaticActionReaction Reaction, DiplomaticRelation Relation) =>
            Text.Proposing(112);

        public override void Sent(DiplomaticAction Action) => Deliver(Action);

        internal static readonly string P5ThreatenToAttackRStr = "Threaten to attack";
        internal static readonly string P5ThreatRStr = "Threat";
    }

    // Kind 5: the source pays the target a tribute.
    public sealed partial record MakeTribute(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 5;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 48;

        public override string Name(DiplomaticAction Action) => AbilityRules.TranslateRStr(P5MakeTributeRStr);

        public override string LogText(DiplomaticAction Action)
        {
            var subject = AbilityRules.TranslateRStr(P5TributeRStr);
            return Outcome(Action, subject, subject, AcceptedLine(subject), "");
        }

        public override string LoggedSample => "SFX\\EVENTS\\MESSAGE2.WAV";

        public override void DefaultMessage(DiplomaticMessageText Text, DiplomaticActionReaction Reaction, DiplomaticRelation Relation) =>
            Text.Exchange(Reaction, 118, 133, 136);

        public override bool CanSend(DiplomaticAction Action, ref string ErrorText)
        {
            if (Gold == 0 && Mana == 0)
            {
                ErrorText = AbilityRules.TranslateRStr(P5NoGoldManaSpecifiedRStr);
                return false;
            }

            return true;
        }

        public override void Sent(DiplomaticAction Action) => Deliver(Action);

        public override bool LapsesUnanswered(DiplomaticAction Action) => true;

        public override void Accepted(DiplomaticAction Action) => ReturnToSource(Action);

        public override void AnswerAI(DiplomaticAction Action, sbyte Player, DiplomaticRelation Relation, ByteList IdealPeace, ByteList IdealWar)
        {
            string ErrorText = "";
            if (Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Alliance).Contains(Relation))
            {
                Action.Accept(ref ErrorText);
            }
            else
            {
                if (AoWHSMap.Current.Random(200) + 50 < Gold + Mana)
                {
                    Action.Accept(ref ErrorText);
                }
                else
                {
                    Action.Reject(ref ErrorText);
                }
            }
        }

        internal static readonly string P5MakeTributeRStr = "Make tribute";
        internal static readonly string P5NoGoldManaSpecifiedRStr = "No gold/mana specified";
        internal static readonly string P5TributeRStr = "Tribute";
    }

    // Kind 7: the source writes to the target in the name of the other player, offered between human
    // players to a source of race 5 whose hero emerges (Player.PAS:1226).
    public sealed partial record ImpersonatedMessage : DiplomaticProposal
    {
        public const byte Kind = 7;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Message;

        public override bool SignedAsOtherPlayer => true;

        public override string Name(DiplomaticAction Action)
        {
            var name = AoWHSMap.Current.FPlayers.GetPlayers(Action.FOtherPlayer).GetName();
            return AbilityRules.Format(AbilityRules.TranslateRStr(P5SendMessageAsXRStr), [name]);
        }

        public override string LogText(DiplomaticAction Action) => AbilityRules.TranslateRStr(GameText.P5MessageRStr);

        public override void Sent(DiplomaticAction Action) => Deliver(Action);

        private static readonly string P5SendMessageAsXRStr = "Send message as %s";
    }

    // Kind 8: the source asks the target to end its alliance with the other player.
    public sealed partial record SuggestAllianceBreak(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 8;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 47;

        public override string Name(DiplomaticAction Action)
        {
            var name = AoWHSMap.Current.FPlayers.GetPlayers(Action.FOtherPlayer).GetName();
            return AbilityRules.Format(AbilityRules.TranslateRStr(P5SuggestAllianceBreakWithYRStr), [name]);
        }

        public override string LogText(DiplomaticAction Action)
        {
            var subject = AbilityRules.TranslateRStr(GameText.P5BreakAllianceRStr);
            return Outcome(Action, AbilityRules.TranslateRStr(P5BreakAllianceSuggestionRStr), subject, AcceptedLine(subject), "");
        }

        public override void DefaultMessage(DiplomaticMessageText Text, DiplomaticActionReaction Reaction, DiplomaticRelation Relation) =>
            Text.Exchange(Reaction, 160, 139, 142, true);

        public override void Sent(DiplomaticAction Action) => Deliver(Action);

        public override bool LapsesUnanswered(DiplomaticAction Action) =>
            Relation(Action.FTarget, Action.FOtherPlayer) != DiplomaticRelation.Alliance;

        public override void Accepted(DiplomaticAction Action)
        {
            if (Relation(Action.FTarget, Action.FOtherPlayer) == DiplomaticRelation.Alliance)
            {
                AoWHSMap.Current.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FTarget, (sbyte)(byte)Action.FOtherPlayer, DiplomaticRelation.Peace);
                var broken = AoWHSMap.Current.FPlayers.GetPlayers(Action.FTarget).CreateDiplomaticAction(new BreakAlliance(), Action.FOtherPlayer, -1);
                if (broken != null)
                {
                    Broadcast(broken, Action.FSource, Action.FOtherPlayer);
                    broken.Release();
                }
            }

            ReturnToSource(Action);
        }

        public override DiplomaticActionReaction RejectReaction(DiplomaticAction Action) =>
            Relation(Action.FTarget, Action.FOtherPlayer) == DiplomaticRelation.Alliance ? DiplomaticActionReaction.Reject : DiplomaticActionReaction.Ignore;

        public override void AnswerAI(DiplomaticAction Action, sbyte Player, DiplomaticRelation Relation, ByteList IdealPeace, ByteList IdealWar)
        {
            string ErrorText = "";
            if (DiplomaticProposal.Relation(Action.FTarget, Action.FOtherPlayer) != DiplomaticRelation.Alliance)
            {
                Action.Accept(ref ErrorText);
            }
            else
            {
                Action.Reject(ref ErrorText);
            }
        }

        public override int RelationModifier(DiplomaticAction Action, sbyte Player, sbyte Race) =>
            Player == Action.FTarget && Relation(Action.FTarget, Action.FOtherPlayer) == DiplomaticRelation.Alliance
                ? RaceModifier(Action.FOtherPlayer, Race, -40)
                : 0;

        private static readonly string P5SuggestAllianceBreakWithYRStr = "Suggest alliance break with %s";
        internal static readonly string P5BreakAllianceSuggestionRStr = "Break alliance suggestion";
    }

    // Kind 9: the source asks the target to declare war on the other player.
    public sealed partial record SuggestWarDeclaration(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 9;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 47;

        public override string Name(DiplomaticAction Action)
        {
            var name = AoWHSMap.Current.FPlayers.GetPlayers(Action.FOtherPlayer).GetName();
            return AbilityRules.Format(AbilityRules.TranslateRStr(P5SuggestWarWithYRStr), [name]);
        }

        public override string LogText(DiplomaticAction Action)
        {
            var subject = AbilityRules.TranslateRStr(GameText.P5DeclareWarRStr);
            return Outcome(Action, AbilityRules.TranslateRStr(P5DeclareWarSuggestionRStr), subject, AcceptedLine(subject), "");
        }

        public override int MiddleIcon(DiplomaticAction Action) =>
            Action.FReaction == DiplomaticActionReaction.Accept ? 0x15 : 0x16;

        public override void DefaultMessage(DiplomaticMessageText Text, DiplomaticActionReaction Reaction, DiplomaticRelation Relation) =>
            Text.Exchange(Reaction, 157, 145, 148, true);

        public override void Sent(DiplomaticAction Action) => Deliver(Action);

        public override bool LapsesUnanswered(DiplomaticAction Action) =>
            Relation(Action.FTarget, Action.FOtherPlayer) != DiplomaticRelation.War;

        public override void Accepted(DiplomaticAction Action)
        {
            if (Relation(Action.FTarget, Action.FOtherPlayer) != DiplomaticRelation.War)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(Action.FOtherPlayer).PlayerSeen((sbyte)(byte)Action.FTarget);
                AoWHSMap.Current.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FTarget, (sbyte)(byte)Action.FOtherPlayer, DiplomaticRelation.War);
                var war = AoWHSMap.Current.FPlayers.GetPlayers(Action.FTarget).CreateDiplomaticAction(new DeclareWar(), Action.FOtherPlayer, -1);
                if (war != null)
                {
                    Broadcast(war, Action.FSource, Action.FOtherPlayer);
                    war.Release();
                }
            }

            ReturnToSource(Action);
        }

        public override DiplomaticActionReaction RejectReaction(DiplomaticAction Action) =>
            Relation(Action.FTarget, Action.FOtherPlayer) != DiplomaticRelation.War ? DiplomaticActionReaction.Reject : DiplomaticActionReaction.Ignore;

        public override void AnswerAI(DiplomaticAction Action, sbyte Player, DiplomaticRelation Relation, ByteList IdealPeace, ByteList IdealWar)
        {
            string ErrorText = "";
            if (DiplomaticProposal.Relation(Action.FTarget, Action.FOtherPlayer) == DiplomaticRelation.War)
            {
                Action.Accept(ref ErrorText);
            }
            else
            {
                if (IdealWar.IndexOf((byte)Action.FOtherPlayer) != -1)
                {
                    if (AoWHSMap.Current.FPlayers.GetPlayers(Action.FTarget).FAIControl.CanDeclareWar((byte)Action.FOtherPlayer))
                    {
                        if (AoWHSMap.Current.Random(5) == 1 && AoWHSMap.Current.Random(550) + 250 < Action.FEscrowGold + Action.FEscrowMana)
                        {
                            Action.Accept(ref ErrorText);
                        }
                        else
                        {
                            Action.Reject(ref ErrorText);
                        }
                    }
                    else
                    {
                        Action.Reject(ref ErrorText);
                    }
                }
                else
                {
                    Action.Reject(ref ErrorText);
                }
            }
        }

        public override int RelationModifier(DiplomaticAction Action, sbyte Player, sbyte Race)
        {
            if (Player == Action.FTarget && Relation(Action.FTarget, Action.FOtherPlayer) != DiplomaticRelation.War)
            {
                int Modifier = Relation(Action.FTarget, Action.FOtherPlayer) == DiplomaticRelation.Alliance ? -60 : -20;
                return RaceModifier(Action.FOtherPlayer, Race, Modifier);
            }

            return 0;
        }

        private static readonly string P5SuggestWarWithYRStr = "Suggest war with %s";
        internal static readonly string P5DeclareWarSuggestionRStr = "Declare war suggestion";
    }

    // intf_P1_03.inc:248

    // intf_P1_03.inc:248

}
