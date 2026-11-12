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
public partial class Player : EngineObject, IOwned<PlayerList>, IOwnedSink<PlayerList>
{
    // The list that holds the player. AcceptsOwner admits only that list, so the cast never fails.
    public new PlayerList Owner { get => (PlayerList)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is PlayerList;

    void IOwnedSink<PlayerList>.SetOwner(PlayerList owner) => SetOwner(owner);

    public new const int ClassId = 131104;

    // Player.PAS:261
    public Player(ECustomNode owner) : base(owner)
    {
        _incomeChangedEventsStore = new();
        _gemsChangedEventsStore = new();
        _upkeepChangedEventsStore = new();
        Structures = new PlayerStructureList(null);
        Structures.Player = this;
        Armies = new ArmyList(null);
        AIControl = new AIPlayerControl(null);
        AIControl.Player = this;
        AIBudget = new AIBudgetManager(null);
        AIBudget.Player = this;
        AIGroups = new AIGroupList(null);
        MagicControl = new PlayerMagicControl(null);
        MagicControl.Player = this;
        Statistics = new PlayerStatistics(null);
        Armies.Player = this;
        Armies.Events += ArmiesChanged;
        Heroes = new HeroList(null);
        InitialGold = 250;
        BaseIncome = 0;
        PlayerIndex = -1;
        Participating = false;
        EventLogbook = new EventLogbook(null);
        EventLogbook.SetOnChanged(LogbookChangedEvent);
        _logbookChangedEventsStore = new();
        DiplomaticRelations = new PlayerDiplomaticRelations(null);
        DiplomaticRelations.Player = this;
        LegacyNewRelations = new DiplomaticRelationList();
        ProductionControls = new ProductionControlList();
        _incomeSources = new IncomeSourceList(null);
        LeaderSetup = new Leader(null);
        Status = PlayerStatus.Idle;
        GameOverStatus = GameOverStatus.None;
        FirstDaySeen = new Int32List();
        FirstDaySeen.Default = -1;
        _lastDaySeen = new Int32List();
        _lastDaySeen.Default = -1;
        _victoryLocations = new PlayerStructureList(null);
        KnownStructures = new PlayerStructureList(null);
        _messages = new StringList();
        DiplomaticActions = new DiplomaticActionList(null);
        AIValidAttackPlayers = new BitList();
        PbemSettings = new PbemPlayerSettings(null);
        LegacyRelations = new DiplomaticRelationList();
        HeroEmerging = false;
        BaseHeroJoinDay = -1;
        HeroJoinDay = -1;
        PlayerRace = -1;
        ViewX = 255;
        ViewY = 255;
    }

    private int _updateCount;

    private EventStore<Player> _gemsChangedEventsStore;

    public event NotifyEvent<Player> FGemsChangedEvents
    {
        add => _gemsChangedEventsStore.Add(value);
        remove => _gemsChangedEventsStore.Remove(value);
    }

    private EventStore<Player> _incomeChangedEventsStore;

    public event NotifyEvent<Player> FIncomeChangedEvents
    {
        add => _incomeChangedEventsStore.Add(value);
        remove => _incomeChangedEventsStore.Remove(value);
    }

    private EventStore<Player> _upkeepChangedEventsStore;

    public event NotifyEvent<Player> FUpkeepChangedEvents
    {
        add => _upkeepChangedEventsStore.Add(value);
        remove => _upkeepChangedEventsStore.Remove(value);
    }

    private EventStore<Player> _logbookChangedEventsStore;

    public event NotifyEvent<Player> FLogbookChangedEvents
    {
        add => _logbookChangedEventsStore.Add(value);
        remove => _logbookChangedEventsStore.Remove(value);
    }

    public PlayerStructureList Structures { get; }

    private PlayerStructureList _victoryLocations;

    public ProductionControlList ProductionControls { get; }
    public ArmyList Armies { get; }
    public bool SingleProcessCycle { get; private set; }

    public byte ViewX;
    public byte ViewY;
    public byte ViewL;
    private string _password = "";
    private StringList _messages;

    public DiplomaticRelationList LegacyRelations { get; }
    public DiplomaticRelationList LegacyNewRelations { get; }
    public PlayerDiplomaticRelations DiplomaticRelations { get; }

    public GameOverStatus GameOverStatus;
    public PlayerStatus Status;
    private int _busyCount;

    public string Name { get; set; } = "";

    public sbyte MoraleValue;

    public PlayerMagicControl MagicControl { get; }
    public Leader LeaderSetup { get; }

    public int Day;

    public PlayerStatistics Statistics { get; }

    private IncomeSourceList _incomeSources;

    public PlayerStructureList KnownStructures { get; }

    public int BaseHeroJoinLevel;
    private int _lastHeroJoinDay;
    public int EmergingHeroId;
    private int _lastHeroEmergeDay;
    private int _highestHeroLevel;

    public HeroList Heroes { get; }
    public AIGroupList AIGroups { get; }
    public AIPlayerControl AIControl { get; }
    public AIBudgetManager AIBudget { get; }
    public DiplomaticActionList DiplomaticActions { get; private set; }
    public BitList AIValidAttackPlayers { get; private set; }
    public Int32List FirstDaySeen { get; }

    private Int32List _lastDaySeen;

    public PbemPlayerSettings PbemSettings { get; private set; }

    public bool HeroEmerging;
    public sbyte PlayerRace;
    public sbyte PlayerIndex;
    public PlayerType PlayerType;
    public PlayerBehavior Behavior;
    public bool Participating;
    public int InitialGold;
    public int BaseIncome;
    public int BaseHeroJoinDay;

    public int HeroJoinDay { get; set; }

    public string Settings = "";
    private string _description = "";
    public int Gems;

    public int ArmyUpkeep { get; private set; }

    private int _magicUpkeep;

    public int Income { get; private set; }

    public Leader Leader;

    public EventLogbook EventLogbook { get; }

    // Player.PAS:315
    public override void Dispose()
    {
        PlayerStructureList structures;
        ArmyList armies;
        try
        {
            _incomeChangedEventsStore.Clear();
            _gemsChangedEventsStore.Clear();
            _upkeepChangedEventsStore.Clear();
            try
            {
                if (StrategicMap.Current != null && StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
                {
                    Assert(_victoryLocations.Count == 0, "Victory Locations not correctly cleared (TPlayer)", "Player.PAS", 326);
                    Assert(Structures.Count == 0, "Registered Player Structures not correctly cleared (TPlayer)", "Player.PAS", 327);
                    Assert(Armies.Count == 0, "Registered Player Armies not correctly cleared (TPlayer)", "Player.PAS", 328);
                }
                else
                {
                    structures = Structures;
                    for (int I = structures.Count - 1; I >= 0; I--)
                    {
                        Structures.GetStructure(I).SetPlayer(0);
                    }

                    armies = Armies;
                    for (int I = armies.Count - 1; I >= 0; I--)
                    {
                        Armies.GetArmy(I).SetPlayer(0);
                    }
                }
            }
            catch
            {
            }

            _messages?.Dispose();
            _messages = null;
            DiplomaticActions?.Dispose();
            DiplomaticActions = null;
            AIValidAttackPlayers?.Dispose();
            AIValidAttackPlayers = null;
            LegacyRelations?.Dispose();
            try
            {
                _logbookChangedEventsStore.Clear();
                EventLogbook?.Dispose();
                _logbookChangedEventsStore.Clear();
            }
            catch
            {
            }

            try
            {
                LegacyNewRelations?.Dispose();
                DiplomaticRelations?.Dispose();
            }
            catch
            {
            }

            try
            {
                ProductionControls?.Dispose();
            }
            catch
            {
            }

            try
            {
                AIControl?.Dispose();
            }
            catch
            {
            }

            try
            {
                Statistics?.Dispose();
            }
            catch
            {
            }

            try
            {
                MagicControl?.Dispose();
            }
            catch
            {
            }

            try
            {
                LeaderSetup?.Dispose();
            }
            catch
            {
            }

            try
            {
                FirstDaySeen?.Dispose();
                _lastDaySeen?.Dispose();
            }
            catch
            {
            }

            try
            {
                Heroes?.Dispose();
            }
            catch
            {
            }

            try
            {
                Armies?.Dispose();
                AIGroups?.Dispose();
                AIBudget?.Dispose();
                Structures?.Dispose();
                _victoryLocations?.Dispose();
                _incomeSources?.Dispose();
                KnownStructures?.Dispose();
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

            PbemSettings?.Dispose();
            PbemSettings = null;
            _incomeChangedEventsStore.Clear();
            _gemsChangedEventsStore.Clear();
            _upkeepChangedEventsStore.Clear();
        }
        catch
        {
        }
    }

    // Player.PAS:1023
    public override void ReadWrite(EStorageStream stream)
    {
        int X = default;
        int Y = default;
        int L = default;
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref PlayerRace), 2);
        stream.rwData(Bytes.Of(ref PlayerType, 1), 1, 3);
        stream.rwBool(ref Participating, 4);
        stream.rwInteger(ref InitialGold, 5);
        stream.rwInteger(ref BaseIncome, 6);
        stream.rwData(Bytes.Of(ref Status, 1), 1, 10);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.rwString(ref Settings, 11);
        }

        stream.rwString(ref _description, 12);
        stream.rwInteger(ref Gems, 16);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref PlayerIndex), 18);
        if ((GameTables.GameEngine.Status & Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm, HexSpriteEngineStatusElement.SavingHsm)) != Set<HexSpriteEngineStatusElement>.Of() || stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            if (!stream.StorageMode().Contains(StorageModeElement.Write))
            {
                stream.rwCreateEObject(ref Leader, null, 19);
                if (Leader != null && !stream.StorageMode().Contains(StorageModeElement.Preview))
                {
                    Leader.Player = PlayerIndex;
                    StrategicMap.Current.HeroControl.RegisterHero(Leader);
                }
            }
        }

        if (stream.StorageMode().Contains(StorageModeElement.Write))
        {
            if (StrategicMap.Current.SeatedPlayer == PlayerIndex && StrategicMap.Current.ConnectionType != PlayerConnectionType.Network)
            {
                StrategicMap.Current.GetViewCenter(ref X, ref Y, ref L);
                ViewX = (byte)X;
                ViewY = (byte)Y;
                ViewL = (byte)L;
                stream.rwByte(ref ViewX, 20);
                stream.rwByte(ref ViewY, 21);
                stream.rwByte(ref ViewL, 22);
            }
        }
        else
        {
            stream.rwByte(ref ViewX, 20);
            stream.rwByte(ref ViewY, 21);
            stream.rwByte(ref ViewL, 22);
        }

        if (!stream.StorageMode().Contains(StorageModeElement.Preview))
        {
            stream.rwEObject(EventLogbook, 23);
        }

        stream.rwString(ref _password, 24);
        stream.rwData(Bytes.Of(ref GameOverStatus, 1), 1, 25);
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            stream.rwStreamProc(26, LegacyNewRelations.LoadFromStream, LegacyNewRelations.SaveToStream);
        }

        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref MoraleValue), 27);
        stream.rwEObject(AIControl, 28);
        stream.rwData(Bytes.Of(ref Behavior, 1), 1, 29);
        if (PlayerIndex != 0)
        {
            if (stream.StorageMode().Contains(StorageModeElement.Write) && StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && Leader != null)
            {
                stream.rwEObject(Leader, 30);
            }
            else
            {
                stream.rwEObject(LeaderSetup, 30);
            }
        }

        stream.rwEObject(MagicControl, 31);
        stream.rwStreamProc(32, FirstDaySeen.LoadFromStream, FirstDaySeen.SaveToStream);
        stream.rwStreamProc(33, _lastDaySeen.LoadFromStream, _lastDaySeen.SaveToStream);
        stream.rwInteger(ref Day, 34);
        stream.rwEObject(Statistics, 35);
        stream.rwInteger(ref BaseHeroJoinLevel, 36);
        stream.rwInteger(ref _lastHeroJoinDay, 37);
        stream.rwInteger(ref _lastHeroEmergeDay, 38);
        stream.rwInteger(ref _highestHeroLevel, 39);
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            SetRace(PlayerRace);
            EventLogbook.Player = PlayerIndex;
            LegacyNewRelations.Default = (int)DiplomaticRelation.Unknown;
        }

        stream.rwStrings(_messages, 40);
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            stream.rwStreamProc(41, LegacyRelations.LoadFromStream, LegacyRelations.SaveToStream);
        }

        stream.rwInteger(ref EmergingHeroId, 50);
        stream.rwEObject(AIBudget, 51);
        stream.rwEObject(DiplomaticActions, 52);
        stream.rwStreamProc(53, AIValidAttackPlayers.LoadFromStream, AIValidAttackPlayers.SaveToStream);
        stream.rwEObject(PbemSettings, 54);
        stream.rwBool(ref HeroEmerging, 56);
        if (stream.StorageMode().Contains(StorageModeElement.Read) || StrategicMap.Current.ConnectionType != PlayerConnectionType.Network)
        {
            stream.rwInteger(ref BaseHeroJoinDay, 57);
        }

        stream.rwEObject(DiplomaticRelations, 58);
        if (PlayerIndex == 0)
        {
            PlayerType = PlayerType.Independent;
        }
    }

    // Player.PAS:974
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        DiplomaticRelations.MessageProc(ref message, sender);
        if (message.MessageId == 536870920 && StrategicMap.Current.Day == 0)
        {
            FirstDaySeen.Clear();
            _lastDaySeen.Clear();
        }

        if (message.MessageId == 537001989)
        {
            ValidateLeader();
        }

        if (message.MessageId == 537001985)
        {
            try
            {
                UpdateLogbook();
                if (StrategicMap.Current.Day == 1)
                {
                    ViewX = 255;
                    if (StrategicMap.Current.GameType != GameType.Campaign && !StrategicMap.Current.FixedLeaders && Leader != null)
                    {
                        Leader.Owner = null;
                        MagicControl.UnregisterPowerSource(Leader.PowerSource);
                        // { Original removes the player, not the leader. }
                        Heroes.RemoveChild(this);
                    }

                    UpdateAlliedPlayersSeen();
                    SetupAIValidAttackPlayers();
                }
            }
            catch
            {
            }
        }

        if (message.MessageId == 537002243)
        {
            if (!Validate(message.Payload.As<StringCollection>()))
            {
                message.Data[0] = 1;
            }
        }

        if (message.MessageId == 537001986 && message.Payload.Int == PlayerIndex)
        {
            UpdateAlliedPlayersSeen();
        }

        if (message.MessageId == 537001987 && message.Payload.Int == PlayerIndex)
        {
            UpdateTurnInfo();
            if (PlayerIndex != 0)
            {
                UpdateHeroJoin();
            }
        }

        if (message.MessageId == 536870920 && StrategicMap.Current.Day == 0)
        {
            ViewX = 255;
            ViewY = 255;
            ViewL = 255;
        }

        MagicControl.MessageProc(ref message, sender);
        AIControl.MessageProc(ref message, sender);
        AIBudget.MessageProc(ref message, sender);
        EventLogbook.MessageProc(ref message, sender);
    }

    // Player.PAS:394
    public bool GetBusy()
    {
        bool result = _busyCount != 0 || !StrategicMap.Current.TokenManager.PlayerReady(PlayerIndex);
        result = result || (StrategicMap.Current.Combat == null && StrategicMap.Current.PlayMode == PlayMode.Turn && StrategicMap.Current.CurrentPlayer != PlayerIndex);
        result = result || StrategicMap.Current.Combat != null && StrategicMap.Current.Combat.CombatData.Players.GetPlayers((sbyte)StrategicMap.Current.Combat.CombatData.CurrentPlayer).Player != PlayerIndex;
        result = result || GameOverStatus != GameOverStatus.None || !GetLocal();
        return result || PlayerType == PlayerType.Human && StrategicMap.Current.GetTurnTimerEnabled() && StrategicMap.Current.GetTurnTimeLeft() <= 0;
    }

    // Player.PAS:406
    public int GetSuperiority()
    {
        int result;
        if (Statistics.TurnInfo.Count != 0)
        {
            var count = Statistics.TurnInfo.Count;
            result = Statistics.TurnInfo.GetTurnInfo(count - 1).GetSuperiority();
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Player.PAS:412
    public int GetUnitCount()
    {
        int result = 0;
        for (int I = Armies.Count - 1; I >= 0; I--)
        {
            result += Armies.GetArmy(I).Count;
        }

        return result;
    }

    // Player.PAS:420
    public void SetPlayerIndex(sbyte value)
    {
        PlayerIndex = value;
        EventLogbook.Player = value;
    }

    // Player.PAS:425
    public void SetGameOverStatus(GameOverStatus value)
    {
        GameOverEventLog eventValue;
        PlayerStructureList structures;
        ArmyList armies;
        Campaign campaign;
        if (value != GameOverStatus && GameOverStatus == GameOverStatus.None)
        {
            GameOverStatus = value;
            StrategicMap.Current.AIExecutor.DeactivatePlayer(PlayerIndex);
            if (StrategicMap.Current.GameType == GameType.Campaign && PlayerType == PlayerType.Human)
            {
                if (GameOverStatus == GameOverStatus.Lost)
                {
                    StrategicMap.Current.GameResult = StrategicMapGameResult.Failed;
                    campaign = GameTables.GameEngine.Campaign;
                    if (campaign != null)
                    {
                        campaign.RestartScenario();
                    }
                }
                else
                {
                    if (GameOverStatus == GameOverStatus.Won)
                    {
                        StrategicMap.Current.GameResult = StrategicMapGameResult.Succeeded;
                        campaign = GameTables.GameEngine.Campaign;
                        if (campaign != null)
                        {
                            campaign.FinishScenario();
                        }
                    }
                }
            }

            if (GameOverStatus == GameOverStatus.Lost)
            {
                BeginUpdate();
                MagicControl.RemoveEnchantments();
                structures = Structures;
                for (int I = structures.Count - 1; I >= 0; I--)
                {
                    Structures.GetStructure(I).SetPlayer(0);
                }

                armies = Armies;
                for (int I = armies.Count - 1; I >= 0; I--)
                {
                    Armies.GetArmy(I).SetPlayer(0);
                }

                MagicControl.SetMana(0);
                Gems = 0;
                EndUpdate();
                StrategicMap.Current.Players.Update();
            }

            eventValue = new GameOverEventLog(null);
            eventValue._player = PlayerIndex;
            if (value == GameOverStatus.Lost)
            {
                var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[PlayerRace]);
                var name = GetName();
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_gameOverDefeatMessageFormat), [IntToStr(StrategicMap.Current.Day), raceNamesPlural, name]);
                eventValue.Message.Text = format;
            }
            else
            {
                var raceNamesPluralText = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[PlayerRace]);
                var name = GetName();
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_gameOverVictoryMessageFormat), [IntToStr(StrategicMap.Current.Day), raceNamesPluralText, name]);
                eventValue.Message.Text = format;
            }

            StrategicMap.Current.ExecuteGameOverEvent(eventValue);
            eventValue.Release();
            if (GameOverStatus == GameOverStatus.Lost)
            {
                StrategicMap.Current.GlobalMagicControl.RemovePlayerEnchantments(PlayerIndex);
            }
        }
    }

    // Player.PAS:484
    public bool Surrender()
    {
        bool result;
        GameOverTokenEvent tokenEvent;
        if (!GetBusy() && GameOverStatus == GameOverStatus.None)
        {
            tokenEvent = new GameOverTokenEvent(null);
            tokenEvent.Player = PlayerIndex;
            tokenEvent.GameOverStatus = GameOverStatus.Lost;
            StrategicMap.Current.TokenManager.GetTokenControl(PlayerIndex).AddEvent(tokenEvent);
            tokenEvent.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Player.PAS:499
    public bool Win()
    {
        bool result;
        GameOverTokenEvent tokenEvent;
        if (!GetBusy() && GameOverStatus == GameOverStatus.None)
        {
            tokenEvent = new GameOverTokenEvent(null);
            tokenEvent.Player = PlayerIndex;
            tokenEvent.GameOverStatus = GameOverStatus.Won;
            StrategicMap.Current.TokenManager.GetTokenControl(PlayerIndex).AddEvent(tokenEvent);
            tokenEvent.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Player.PAS:515
    public bool Validate(StringCollection errors)
    {
        bool result;
        string S;
        string leaderName;
        if (PlayerIndex > 0)
        {
            result = true;
            if (Leader.GetSphereCount() != StrategicMap.Current.SphereCount)
            {
                var raceNames = AbilityRules.TranslateResourceString(GameTables.RaceNames[PlayerRace]);
                leaderName = AbilityRules.Format(s_leaderFormat, [raceNames]);
                S = leaderName + ": " + AbilityRules.TranslateResourceString(s_notAllSpheresAssignedText);
                errors.Add(S);
                result = false;
            }

            if (Leader.Owner == null)
            {
                var raceNamesText = AbilityRules.TranslateResourceString(GameTables.RaceNames[PlayerRace]);
                leaderName = AbilityRules.Format(s_leaderFormat, [raceNamesText]);
                S = leaderName + ": " + AbilityRules.TranslateResourceString(s_notPlacedOnMapText);
                errors.Add(S);
                result = false;
            }

            if (Leader.GetSkillPoints() != 0)
            {
                var translatedText = AbilityRules.TranslateResourceString(GameTables.RaceNames[PlayerRace]);
                leaderName = AbilityRules.Format(s_leaderFormat, [translatedText]);
                S = leaderName + ": " + AbilityRules.TranslateResourceString(s_notAllSkillPointsAssignedText);
                errors.Add(S);
            }
        }
        else
        {
            // { Original reports unassigned skill points without failing validation. }
            result = true;
        }

        return result;
    }

    // Player.PAS:547
    public void LockBusy()
    {
        _busyCount += 1;
    }

    // Player.PAS:552
    public void UnlockBusy()
    {
        Assert(_busyCount != 0, "Invalid busy counter usage", "Player.PAS", 554);
        _busyCount -= 1;
    }

    // Player.PAS:558
    public void LogbookChangedEvent(object sender)
    {
        if (_updateCount == 0)
        {
            _logbookChangedEventsStore.Raise(this);
        }
    }

    // Player.PAS:563
    public void BeginUpdate()
    {
        _updateCount += 1;
        Structures.BeginUpdate();
        Armies.BeginUpdate();
        MagicControl.BeginUpdate();
    }

    // Player.PAS:571
    public void EndUpdate()
    {
        _updateCount -= 1;
        if (_updateCount == 0)
        {
            _gemsChangedEventsStore.Raise(this);
            _upkeepChangedEventsStore.Raise(this);
            UpdateIncome();
            _logbookChangedEventsStore.Raise(this);
        }

        Structures.EndUpdate();
        Armies.EndUpdate();
        MagicControl.EndUpdate();
    }

    // Player.PAS:586
    public void ArmiesChanged(object sender)
    {
        if (Armies.Count == 0)
        {
            StrategicMap.Current.GameOver(PlayerIndex, GameOverStatus.Lost);
        }
    }

    // Player.PAS:591
    public void SetGems(int value)
    {
        Gems = value;
        if (_updateCount == 0)
        {
            _gemsChangedEventsStore.Raise(this);
        }

        if (StrategicMap.Current.VictoryCondition == StrategicMapVictoryCondition.AccumulateGold && StrategicMap.Current.VictoryGold <= Gems)
        {
            StrategicMap.Current.GameOver(PlayerIndex, GameOverStatus.Won);
        }
    }

    // Player.PAS:600
    public void UpdateIncome()
    {
        Income = BaseIncome;
        for (int I = _incomeSources.Count - 1; I >= 0; I--)
        {
            var income = _incomeSources[I].Income();
            Income += income;
        }

        if (PlayerType == PlayerType.Cpu2)
        {
            Income = Round((double)Income * 1);
        }
        else
        {
            if (PlayerType == PlayerType.Cpu3)
            {
                Income = Round((double)Income * 2);
            }
            else
            {
                if (PlayerType == PlayerType.Cpu4)
                {
                    Income = Round((double)Income * 2.5) + 50;
                }
                else
                {
                    if (PlayerType == PlayerType.Cpu5)
                    {
                        Income = Round(Income * 3) + 100;
                    }
                }
            }
        }

        if (_updateCount == 0)
        {
            _incomeChangedEventsStore.Raise(this);
        }
    }

    // Player.PAS:614
    public void RegisterIncomeSource(IncomeSource source)
    {
        if (_incomeSources.IndexOfChild(source) == -1)
        {
            _incomeSources.AddChild(source);
            if (_updateCount == 0)
            {
                UpdateIncome();
            }
        }
    }

    // Player.PAS:623
    public void UnregisterIncomeSource(IncomeSource source)
    {
        if (_incomeSources.DropChild(source) != -1 && _updateCount == 0)
        {
            UpdateIncome();
        }
    }

    // Player.PAS:629
    public int GetUpkeep() => ArmyUpkeep + _magicUpkeep;

    // Player.PAS:634
    public void SetArmyUpkeep(int value)
    {
        ArmyUpkeep = value;
        if (_updateCount == 0)
        {
            _upkeepChangedEventsStore.Raise(this);
        }
    }

    // Player.PAS:640
    public void SetMagicUpkeep(int value)
    {
        _magicUpkeep = value;
        if (_updateCount == 0)
        {
            _upkeepChangedEventsStore.Raise(this);
        }
    }

    // Player.PAS:646
    public string GetName()
    {
        string result;
        if (PlayerIndex != 0)
        {
            if (Leader != null)
            {
                result = Leader.HeroName;
                if (!IsEmpty(Leader.GetNickname()))
                {
                    var nickname = Leader.GetNickname();
                    result = result + " " + nickname;
                }
            }
            else
            {
                result = Name;
            }

            if (IsEmpty(result))
            {
                result = AbilityRules.TranslateResourceString(s_playerText) + " " + IntToStr(PlayerIndex);
            }
        }
        else
        {
            result = AbilityRules.TranslateResourceString(GameTables.IndependentsText);
        }

        return result;
    }

    // Player.PAS:663
    public string GetNameWithRace()
    {
        string result;
        if (PlayerIndex != 0)
        {
            var name = GetName();
            var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[PlayerRace]);
            result = AbilityRules.Format(AbilityRules.TranslateResourceString(s_playerNameWithRaceFormat), [name, raceNamesPlural]);
        }
        else
        {
            result = GetName();
        }

        return result;
    }

    // Player.PAS:671
    public void SetName(string value)
    {
        if (PlayerIndex != 0)
        {
            if (Leader != null)
            {
                Leader.HeroName = value;
                Leader.SetNickname("");
            }
            else
            {
                Name = value;
            }
        }
    }

    // Player.PAS:682
    public LibraryImage GetFace()
    {
        LibraryImage result;
        if (Leader != null)
        {
            result = Leader.GetFace();
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Player.PAS:688
    public int GetHeroCountMax()
    {
        int result;
        int strength;
        int maximum;
        var count = Statistics.TurnInfo.Count;
        strength = Statistics.TurnInfo.GetTurnInfo(count - 1).ArmyStrength;
        if (strength < 10)
        {
            result = 1;
        }
        else
        {
            if (strength < 40)
            {
                result = 2;
            }
            else
            {
                if (strength < 80)
                {
                    result = 3;
                }
                else
                {
                    if (strength < 120)
                    {
                        result = 4;
                    }
                    else
                    {
                        if (strength < 200)
                        {
                            result = 5;
                        }
                        else
                        {
                            result = (strength - 200) / 100 + 5;
                        }
                    }
                }
            }
        }

        maximum = StrategicMap.Current.HeroCountMax;
        if (maximum < result)
        {
            result = maximum;
        }

        return result;
    }

    // Player.PAS:703
    public int GetIdealHeroJoinDay()
    {
        int average;
        int maximum;
        int result = _lastHeroJoinDay;
        average = StrategicMap.Current.Players.GetAverageHeroCount();
        maximum = StrategicMap.Current.Players.GetMaxHeroCount();
        if (maximum > average && Heroes.Count == maximum)
        {
            result += 15;
        }
        else
        {
            if (average <= Heroes.Count && Heroes.Count != 0 && StrategicMap.Current.Day > 15)
            {
                result += 10;
            }
            else
            {
                result += 5;
            }
        }

        return result;
    }

    // Player.PAS:717
    public int GetIdealHeroEmergeDay()
    {
        int average;
        int maximum;
        int earliest;
        int result = GetIdealHeroJoinDay();
        average = StrategicMap.Current.Players.GetAverageHeroCount();
        maximum = StrategicMap.Current.Players.GetMaxHeroCount();
        if (maximum > average && Heroes.Count == maximum)
        {
            result += 15;
        }
        else
        {
            if (average < Heroes.Count && Heroes.Count != 0 && StrategicMap.Current.Day > 10)
            {
                result += 10;
            }
            else
            {
                result += 5;
            }
        }

        earliest = _lastHeroEmergeDay + 5;
        if (earliest > result)
        {
            result = earliest;
        }

        return result;
    }

    // Player.PAS:733
    public void ResetBaseHeroJoinDay()
    {
        _lastHeroJoinDay = StrategicMap.Current.Day;
    }

    // Player.PAS:738
    public void MakeHeroEmerge(HeroEmergeMode mode)
    {
        int chance = default;
        DoubleItemList<Hero> heroes;
        HeroEmergeEventLog log;
        MapCoordinateList locations;
        Structure structure;
        DoubleItem<Hero> Item;
        int I;
        Hero hero;
        heroes = new DoubleItemList<Hero>();
        if (mode == HeroEmergeMode.Normal)
        {
            I = StrategicMap.Current.HeroControl.GetCount() - 1;
            for (; I >= 0; I -= 1)
            {
                hero = StrategicMap.Current.HeroControl.GetHero(I);
                if (!hero.HeroStatus.Contains(HeroStatusElement.Died) && hero.Player <= 0 && hero.Owner == null && hero.CanJoin(PlayerIndex))
                {
                    chance = GetHeroJoinChance(hero.GetLevel());
                    if (chance > 0 && hero.JoinAmount(PlayerIndex) < Gems)
                    {
                        heroes.AddAbility(hero, chance * chance);
                    }
                }
            }
        }
        else
        {
            if (mode == HeroEmergeMode.FirstHero)
            {
                I = StrategicMap.Current.HeroControl.GetCount() - 1;
                for (; I >= 0; I -= 1)
                {
                    hero = StrategicMap.Current.HeroControl.GetHero(I);
                    if (!hero.HeroStatus.Contains(HeroStatusElement.Died) && hero.Player <= 0 && hero.Owner == null && hero.GetAbilityEnabled(52) && hero.GetLevel() >= 2 && hero.GetLevel() <= 3)
                    {
                        var playerRelation = StrategicMap.Current.Races.GetRace(hero.Race).GetPlayerRelation(PlayerIndex);
                        if (Set<Session.Status>.Of(Session.Status.Polite, Session.Status.Friendly).Contains(playerRelation) && hero.JoinAmount(PlayerIndex) < Gems)
                        {
                            var level = hero.GetLevel();
                            heroes.AddAbility(hero, (level - 2) * 1000 + 1);
                        }
                    }
                }
            }
        }

        if (heroes.Count != 0)
        {
            locations = new MapCoordinateList();
            I = Structures.Count - 1;
            for (; I >= 0; I -= 1)
            {
                structure = Structures.GetStructure(I);
                if (structure.EngineClass.ClassId == 131336)
                {
                    var hexLevel = structure.GetHexLevel();
                    var hexY = structure.GetHexY();
                    locations.AddMapCoordinate(structure.GetHexX(), hexY, hexLevel);
                }
            }

            if (locations.Count != 0)
            {
                _lastHeroEmergeDay = StrategicMap.Current.Day;
                RandSeed = StrategicMap.Current.Random(1048575);
                I = heroes.RandomChanceItemB(0);
                Item = heroes[I];
                hero = Item.First;
                I = StrategicMap.Current.Random(locations.Count);
                EmergingHeroId = hero.UnitId;
                hero.UpdateSettings();
                var item = locations[I];
                var locationsItem = locations[I];
                hero.Place(locations[I].X, locationsItem.Y, item.MapLevel, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea, UnitPlaceModeElement.CreateNewArmy));
                var item2 = locations[I];
                var item3 = locations[I];
                var item4 = locations[I];
                structure = (Structure)StrategicMap.Current.MapContainer.GetField(item4.X, item3.Y, item2.MapLevel).FindNoneTransparentHexSprite(typeof(Structure));
                log = new HeroEmergeEventLog(null);
                log.Setup(hero, PlayerIndex, HeroEmergeEventLog.HeroJoinMode.Emerged);
                if (structure != null)
                {
                    log.LocationName = structure.GetName();
                }
                else
                {
                    log.LocationName = AbilityRules.TranslateResourceString(GameTables.UnknownText);
                }

                EventLogbook.AddEvent(log);
                if (PlayerIndex == StrategicMap.Current.SeatedPlayer && StrategicMap.Current.Players.GetPlayers(PlayerIndex).PlayerType == PlayerType.Human)
                {
                    log.Execute(false, ExecuteTrigger.Event, null);
                }

                log.Release();
            }

            locations?.Dispose();
        }

        heroes?.Dispose();
    }

    // Player.PAS:812
    public void UpdateHeroJoin()
    {
        int days;
        for (int I = Heroes.Count - 1; I >= 0; I--)
        {
            if (Heroes.GetHero(I).GetLevel() > _highestHeroLevel)
            {
                _highestHeroLevel = Heroes.GetHero(I).GetLevel();
            }
        }

        BaseHeroJoinLevel = StrategicMap.Current.Players.GetAverageHeroLevel();
        if (BaseHeroJoinLevel == 0)
        {
            BaseHeroJoinLevel = StrategicMap.Current.HeroControl.GetMinFreeHeroLevel();
        }

        if (_highestHeroLevel != 0)
        {
            BaseHeroJoinLevel = (BaseHeroJoinLevel + _highestHeroLevel + 1) / 2;
        }

        if (Heroes.Count == GetHeroCountMax())
        {
            ResetBaseHeroJoinDay();
        }

        EmergingHeroId = 0;
        days = GetIdealHeroEmergeDay() - StrategicMap.Current.Day;
        if (StrategicMap.Current.Day == 1 && Heroes.Count == 1 && !StrategicMap.Current.FixedLeaders)
        {
            MakeHeroEmerge(HeroEmergeMode.FirstHero);
            return;
        }

        if (days <= 0)
        {
            if (StrategicMap.Current.Random(4) == 1)
            {
                MakeHeroEmerge(HeroEmergeMode.Normal);
            }
        }
    }

    // Player.PAS:836
    public int GetHeroJoinChance(int level)
    {
        int result;
        int maximum;
        maximum = GetHeroJoinLevelMax();
        if (level <= maximum)
        {
            result = Round((double)((double)(maximum + 2) / (level + 2)) * 20);
            if (result > 40)
            {
                result = 40;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Player.PAS:849
    public int GetHeroJoinLevelMax()
    {
        int days;
        int result = BaseHeroJoinLevel;
        var idealHeroJoinDay = GetIdealHeroJoinDay();
        days = StrategicMap.Current.Day - idealHeroJoinDay;
        if (days > 0)
        {
            result += days / 5;
        }

        if (days < 0)
        {
            result += Round((double)((double)(result * days) + 2.5) / 5);
            if (result < 0)
            {
                result = 0;
            }
        }

        if (result < 3 && !StrategicMap.Current.FixedLeaders && StrategicMap.Current.Day == 1)
        {
            result = 3;
        }

        return result;
    }

    // Player.PAS:865
    public void ValidateLeader()
    {
        int faceIndex;
        int resourceIndex;
        HeroType heroType;
        string name;
        if (StrategicMap.Current != null && PlayerIndex != 0)
        {
            heroType = Units.HeroType.HeroLeader;
            if (Leader != null && Leader.GetHeroType() != heroType)
            {
                name = Leader.HeroName;
                faceIndex = Leader.FaceIndex;
                Leader.Deactivate();
                resourceIndex = GamePack.Current.HeroResources.Find(PlayerRace, heroType);
                Leader.SetHeroResourceIndex(resourceIndex);
                Leader.HeroName = name;
                Leader.SetFaceIndex(faceIndex);
                Leader.Activate();
            }
            else
            {
                if (Leader == null)
                {
                    resourceIndex = GamePack.Current.HeroResources.Find(PlayerRace, heroType);
                    if (resourceIndex != -1)
                    {
                        Leader = new Leader(null);
                        Leader.SetHeroResourceIndex(resourceIndex);
                        Leader.Player = PlayerIndex;
                        Leader.Activate();
                    }
                }
                else
                {
                    if (Leader != null && !StrategicMap.Current.FixedLeaders && StrategicMap.Current.GameType != GameType.Campaign && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                    {
                        Leader.Owner = null;
                    }
                }
            }
        }
    }

    // Player.PAS:902
    public void SetLeader(Leader value)
    {
        Leader = value;
    }

    // Player.PAS:907
    public void SetRace(sbyte value)
    {
        PlayerRace = value;
    }

    // Player.PAS:912
    public void SetEndTurn(bool value)
    {
        if (value != SingleProcessCycle)
        {
            SingleProcessCycle = value;
            StrategicMap.Current.TriggerEndTurnEvent(PlayerIndex);
        }
    }

    // Player.PAS:921
    public void UpdateLogbook()
    {
        while (true)
        {
            bool condition = EventLogbook.GetCount() != 0;
            if (condition)
            {
                var logs = EventLogbook.GetLogs(0);
                condition = StrategicMap.Current.Day - logs._day >= 3;
            }

            if (!condition)
                break;
            var fEventLogbookLogs = EventLogbook.GetLogs(0);
            EventLogbook.RemoveEvent(fEventLogbookLogs);
        }
    }

    // Player.PAS:928
    public void UpdateTurnInfo()
    {
        TurnInfoItem info;
        Army army;
        Troop troop;
        info = new TurnInfoItem(Statistics.TurnInfo);
        if (GameOverStatus != GameOverStatus.Lost)
        {
            info.Gold = Gems;
            info.Mana = MagicControl.Mana;
            info.Structures = Structures.Count;
            info.Heroes = (byte)Heroes.Count;
            for (int I = 0, last = Armies.Count - 1; I <= last; I++)
            {
                army = Armies.GetArmy(I);
                for (int J = army.Count - 1; J >= 0; J--)
                {
                    troop = army.GetUnit(J);
                    if (info.ArmyStrength < 1073741823)
                    {
                        var strengthMax = troop.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                        info.ArmyStrength += strengthMax;
                    }
                }
            }
        }
    }

    // Player.PAS:955
    public void UpdateAlliedPlayersSeen()
    {
        for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (I != PlayerIndex)
            {
                if (StrategicMap.Current.Players.GetDiplomaticRelation(PlayerIndex, (sbyte)I) == DiplomaticRelation.Alliance)
                {
                    PlayerSeen((sbyte)I);
                }
            }
        }
    }

    // Player.PAS:965
    public void SetupAIValidAttackPlayers()
    {
        for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (I != PlayerIndex)
            {
                var diplomaticRelation = DiplomaticRelations.GetDiplomaticRelation((sbyte)I);
                AIValidAttackPlayers[I] = diplomaticRelation == DiplomaticRelation.War;
            }
        }
    }

    // Player.PAS:1108
    public int GetFirstDaySeen(sbyte player)
    {
        int result;
        if (player != PlayerIndex)
        {
            if (player < FirstDaySeen.Count)
            {
                result = FirstDaySeen[player];
            }
            else
            {
                result = -1;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Player.PAS:1116
    public int GetLastDaySeen(sbyte player)
    {
        int result;
        if (player != PlayerIndex)
        {
            if (player < _lastDaySeen.Count)
            {
                result = _lastDaySeen[player];
            }
            else
            {
                result = -1;
            }
        }
        else
        {
            result = StrategicMap.Current.Day;
        }

        return result;
    }

    // Player.PAS:1124
    public void PlayerSeen(sbyte player)
    {
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && (player >= _lastDaySeen.Count || _lastDaySeen[player] != StrategicMap.Current.Day))
        {
            _lastDaySeen[player] = StrategicMap.Current.Day;
            if (GetFirstDaySeen(player) == -1)
            {
                FirstDaySeen[player] = StrategicMap.Current.Day;
            }
        }
    }

    // Player.PAS:1134
    public void GetLeaderLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        if (Leader != null && Leader.UnitListOwner != null)
        {
            Leader.UnitListOwner.GetLocation(ref X, ref Y, ref L);
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
        bool result;
        PlayerCommunicationControl control;
        control = StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl;
        if (control != null)
        {
            var players = control.GetPlayers(PlayerIndex);
            result = Set<ConnectionMode>.Of(ConnectionMode.Local, ConnectionMode.Global).Contains(players.ConnectionMode);
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Player.PAS:1159
    public void RegisterVictoryLocation(PlayerStructure structure)
    {
        _victoryLocations.AddChild(structure);
    }

    // Player.PAS:1164
    public void UnregisterVictoryLocation(PlayerStructure structure)
    {
        _victoryLocations.RemoveChild(structure);
    }

    // Player.PAS:1169
    public void CheckCaptureLocations()
    {
        if (StrategicMap.Current.VictoryCondition == StrategicMapVictoryCondition.CaptureLocations)
        {
            if (CaptureLocationsLeft() == 0)
            {
                StrategicMap.Current.GameOver(PlayerIndex, GameOverStatus.Won);
            }
        }
    }

    // Player.PAS:1175
    public int CaptureLocationsLeft()
    {
        int result = 0;
        for (int I = 0, last = _victoryLocations.Count - 1; I <= last; I++)
        {
            if (_victoryLocations.GetStructure(I).Player != PlayerIndex)
            {
                bool condition = _victoryLocations.GetStructure(I).Player > 0;
                if (condition)
                {
                    var structure = _victoryLocations.GetStructure(I);
                    condition = StrategicMap.Current.Players.GetDiplomaticRelation(structure.Player, PlayerIndex) == DiplomaticRelation.Alliance;
                }

                if (!(condition && StrategicMap.Current.AlliedVictory))
                {
                    result += 1;
                }
            }
        }

        return result;
    }

    // Player.PAS:1188
    public DiplomaticAction CreateDiplomaticAction(DiplomaticProposal proposal, sbyte target, sbyte other)
    {
        DiplomaticAction result;
        if (StrategicMap.Current.Players.GetPlayers(target).Status == PlayerStatus.Playing)
        {
            result = new DiplomaticAction(null);
            result.Source = PlayerIndex;
            result.Target = target;
            result.OtherPlayer = other;
            result.Proposal = proposal;
            result.RetrieveDefaultMessage(PlayerIndex, target, other, DiplomaticActionReaction.None);
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Player.PAS:1203
    public bool DiplomaticActionAlreadySend(byte kind, sbyte target, sbyte other)
    {
        EventLogbook logbook;
        int I;
        DiplomaticAction item;
        logbook = StrategicMap.Current.Players.GetPlayers(target).EventLogbook;
        bool result = false;
        I = logbook.GetCount();
        while (!result && I != 0)
        {
            I -= 1;
            if (logbook.GetLogs(I)._day == StrategicMap.Current.Day)
            {
                if (logbook.GetLogs(I) is DiplomaticActionEventLog)
                {
                    item = ((DiplomaticActionEventLog)logbook.GetLogs(I)).Action;
                    result = item.Proposal.ToFields().Kind == kind && item.Source == PlayerIndex && item.Target == target && (other == -1 || item.OtherPlayer == other);
                }
            }
        }

        return result;
    }

    // Player.PAS:1226
    public int ListDiplomaticActions(sbyte player, ref DiplomaticActionList actions)
    {
        int count;
        DiplomaticRelation relation;
        DiplomaticAction action;
        if (player != PlayerIndex && GetFirstDaySeen(player) != -1)
        {
            count = actions.Count;
            relation = StrategicMap.Current.Players.GetDiplomaticRelation(player, PlayerIndex);
            if (PlayerType == PlayerType.Human && StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                action = CreateDiplomaticAction(new SendMessage(), player, -1);
                if (action != null)
                {
                    actions.AddChild(action);
                    action.Release();
                }
            }

            if (Set<DiplomaticRelation>.Of(DiplomaticRelation.War, DiplomaticRelation.Unknown).Contains(relation) && !DiplomaticActionAlreadySend(OfferPeace.Kind, player, -1))
            {
                action = CreateDiplomaticAction(new OfferPeace(), player, -1);
                if (action != null)
                {
                    actions.AddChild(action);
                    action.Release();
                }
            }

            if (relation != DiplomaticRelation.War && relation != DiplomaticRelation.Alliance)
            {
                action = CreateDiplomaticAction(new DeclareWar(), player, -1);
                if (action != null)
                {
                    actions.AddChild(action);
                    action.Release();
                }
            }

            if (Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace).Contains(relation) && !DiplomaticActionAlreadySend(ProposeAlliance.Kind, player, -1))
            {
                action = CreateDiplomaticAction(new ProposeAlliance(), player, -1);
                if (action != null)
                {
                    actions.AddChild(action);
                    action.Release();
                }
            }

            if (Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Unknown).Contains(relation) && !DiplomaticActionAlreadySend(ThreatenToAttack.Kind, player, -1))
            {
                action = CreateDiplomaticAction(new ThreatenToAttack(), player, -1);
                if (action != null)
                {
                    actions.AddChild(action);
                    action.Release();
                }
            }

            action = CreateDiplomaticAction(new MakeTribute(), player, -1);
            if (action != null)
            {
                actions.AddChild(action);
                action.Release();
            }

            if (Set<DiplomaticRelation>.Of(DiplomaticRelation.Alliance).Contains(relation))
            {
                action = CreateDiplomaticAction(new BreakAlliance(), player, -1);
                if (action != null)
                {
                    actions.AddChild(action);
                    action.Release();
                }
            }

            for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
            {
                if (I != player && I != PlayerIndex && StrategicMap.Current.Players.GetPlayers(I).Status == PlayerStatus.Playing)
                {
                    bool condition = !DiplomaticActionAlreadySend(SuggestWarDeclaration.Kind, player, (sbyte)I) && StrategicMap.Current.Players.GetPlayers(PlayerIndex).GetFirstDaySeen((sbyte)I) != -1 && StrategicMap.Current.Players.GetPlayers(player).GetFirstDaySeen((sbyte)I) != -1;
                    if (condition)
                    {
                        var diplomaticRelation = StrategicMap.Current.Players.GetDiplomaticRelation(player, (sbyte)I);
                        condition = Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Unknown).Contains(diplomaticRelation);
                    }

                    if (condition)
                    {
                        action = CreateDiplomaticAction(new SuggestWarDeclaration(), player, (sbyte)I);
                        if (action != null)
                        {
                            actions.AddChild(action);
                            action.Release();
                        }
                    }
                }
            }

            for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
            {
                if (I != player && I != PlayerIndex && StrategicMap.Current.Players.GetPlayers(I).Status == PlayerStatus.Playing)
                {
                    if (!DiplomaticActionAlreadySend(SuggestAllianceBreak.Kind, player, (sbyte)I) && StrategicMap.Current.Players.GetPlayers(PlayerIndex).GetFirstDaySeen((sbyte)I) != -1 && StrategicMap.Current.Players.GetPlayers(player).GetFirstDaySeen((sbyte)I) != -1 && StrategicMap.Current.Players.GetDiplomaticRelation(player, (sbyte)I) == DiplomaticRelation.Alliance)
                    {
                        action = CreateDiplomaticAction(new SuggestAllianceBreak(), player, (sbyte)I);
                        if (action != null)
                        {
                            actions.AddChild(action);
                            action.Release();
                        }
                    }
                }
            }

            if (PlayerRace == 5 && HeroEmerging && PlayerType == PlayerType.Human && StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
                {
                    if (I != player && I != PlayerIndex && StrategicMap.Current.Players.GetPlayers(I).Status == PlayerStatus.Playing && StrategicMap.Current.Players.GetPlayers(I).PlayerType == PlayerType.Human)
                    {
                        if (StrategicMap.Current.Players.GetPlayers(PlayerIndex).GetFirstDaySeen((sbyte)I) != -1 && StrategicMap.Current.Players.GetPlayers(player).GetFirstDaySeen((sbyte)I) != -1 && StrategicMap.Current.Players.GetPlayers(I).GetFirstDaySeen(player) != -1)
                        {
                            action = CreateDiplomaticAction(new ImpersonatedMessage(), player, (sbyte)I);
                            if (action != null)
                            {
                                actions.AddChild(action);
                                action.Release();
                            }
                        }
                    }
                }
            }

            count = actions.Count - count;
        }
        else
        {
            count = 0;
        }

        return count;
    }

    // Player.PAS:1348
    public bool ValidAttackRelation(sbyte player, sbyte independentRelation)
    {
        bool result;
        Assert(PlayerIndex != 0, "", "Player.PAS", 1390);
        if (Set<PlayerType>.Of(PlayerType.Cpu1, PlayerType.Cpu2, PlayerType.Cpu3, PlayerType.Cpu4, PlayerType.Cpu5).Contains(PlayerType))
        {
            result = AIValidAttackPlayers[player];
        }
        else
        {
            var diplomaticRelation = AbilityRules.DiplomaticRelation(PlayerIndex, 0, player, independentRelation);
            result = Set<DiplomaticRelation>.Of(DiplomaticRelation.War, DiplomaticRelation.Unknown).Contains(diplomaticRelation);
        }

        return result;
    }

    // Player.PAS:1398
    public bool ChangePlayerSettings(string settings, ref string error)
    {
        bool result;
        ChangePlayerSettingsTokenEvent eventValue;
        if (!GetBusy())
        {
            eventValue = new ChangePlayerSettingsTokenEvent(null);
            eventValue.Player = PlayerIndex;
            eventValue.Settings = settings;
            StrategicMap.Current.TokenManager.GetTokenControl(PlayerIndex).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_playerText = "Player";
    private static readonly string s_playerNameWithRaceFormat = "%0:s of the %1:s";
    private static readonly string s_gameOverDefeatMessageFormat = "On day %0:s, the empire of the %1:s, led by %2:s was defeated.";
    private static readonly string s_gameOverVictoryMessageFormat = "On day %0:s, the empire of the %1:s, led by %2:s was victorious.";
    private static readonly string s_leaderFormat = "%s leader";
    private static readonly string s_notAllSpheresAssignedText = "not all spheres assigned";
    private static readonly string s_notPlacedOnMapText = "not placed on map";
    private static readonly string s_notAllSkillPointsAssignedText = "not all skill points assigned";

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

        public override string Name(DiplomaticAction action) => AbilityRules.TranslateResourceString(ThreatenToAttackText);

        public override string LogText(DiplomaticAction action) => AbilityRules.TranslateResourceString(ThreatText);

        public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation) =>
            text.Proposing(112);

        public override void Sent(DiplomaticAction action) => Deliver(action);

        internal static string ThreatenToAttackText { get; } = "Threaten to attack";
        internal static string ThreatText { get; } = "Threat";
    }

    // Kind 5: the source pays the target a tribute.
    public sealed partial record MakeTribute(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 5;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 48;

        public override string Name(DiplomaticAction action) => AbilityRules.TranslateResourceString(MakeTributeText);

        public override string LogText(DiplomaticAction action)
        {
            var subject = AbilityRules.TranslateResourceString(TributeText);
            return Outcome(action, subject, subject, AcceptedLine(subject), "");
        }

        public override string LoggedSample => "SFX\\EVENTS\\MESSAGE2.WAV";

        public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation) =>
            text.Exchange(reaction, 118, 133, 136);

        public override bool CanSend(DiplomaticAction action, ref string errorText)
        {
            if (Gold == 0 && Mana == 0)
            {
                errorText = AbilityRules.TranslateResourceString(NoGoldManaSpecifiedText);
                return false;
            }

            return true;
        }

        public override void Sent(DiplomaticAction action) => Deliver(action);

        public override bool LapsesUnanswered(DiplomaticAction action) => true;

        public override void Accepted(DiplomaticAction action) => ReturnToSource(action);

        public override void AnswerAI(DiplomaticAction action, sbyte player, DiplomaticRelation relation, ByteList idealPeace, ByteList idealWar)
        {
            string errorText = "";
            if (Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Alliance).Contains(relation))
            {
                action.Accept(ref errorText);
            }
            else
            {
                if (StrategicMap.Current.Random(200) + 50 < Gold + Mana)
                {
                    action.Accept(ref errorText);
                }
                else
                {
                    action.Reject(ref errorText);
                }
            }
        }

        internal static string MakeTributeText { get; } = "Make tribute";
        internal static string NoGoldManaSpecifiedText { get; } = "No gold/mana specified";
        internal static string TributeText { get; } = "Tribute";
    }

    // Kind 7: the source writes to the target in the name of the other player, offered between human
    // players to a source of race 5 whose hero emerges (Player.PAS:1226).
    public sealed partial record ImpersonatedMessage : DiplomaticProposal
    {
        public const byte Kind = 7;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Message;

        public override bool SignedAsOtherPlayer => true;

        public override string Name(DiplomaticAction action)
        {
            var name = StrategicMap.Current.Players.GetPlayers(action.OtherPlayer).GetName();
            return AbilityRules.Format(AbilityRules.TranslateResourceString(s_sendMessageFormat), [name]);
        }

        public override string LogText(DiplomaticAction action) => AbilityRules.TranslateResourceString(GameText.DiplomaticMessageText);

        public override void Sent(DiplomaticAction action) => Deliver(action);

        private static readonly string s_sendMessageFormat = "Send message as %s";
    }

    // Kind 8: the source asks the target to end its alliance with the other player.
    public sealed partial record SuggestAllianceBreak(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 8;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 47;

        public override string Name(DiplomaticAction action)
        {
            var name = StrategicMap.Current.Players.GetPlayers(action.OtherPlayer).GetName();
            return AbilityRules.Format(AbilityRules.TranslateResourceString(s_suggestAllianceBreakFormat), [name]);
        }

        public override string LogText(DiplomaticAction action)
        {
            var subject = AbilityRules.TranslateResourceString(GameText.BreakAllianceText);
            return Outcome(action, AbilityRules.TranslateResourceString(BreakAllianceSuggestionText), subject, AcceptedLine(subject), "");
        }

        public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation) =>
            text.Exchange(reaction, 160, 139, 142, true);

        public override void Sent(DiplomaticAction action) => Deliver(action);

        public override bool LapsesUnanswered(DiplomaticAction action) =>
            Relation(action.Target, action.OtherPlayer) != DiplomaticRelation.Alliance;

        public override void Accepted(DiplomaticAction action)
        {
            if (Relation(action.Target, action.OtherPlayer) == DiplomaticRelation.Alliance)
            {
                StrategicMap.Current.Players.SetDiplomaticRelation((sbyte)(byte)action.Target, (sbyte)(byte)action.OtherPlayer, DiplomaticRelation.Peace);
                var broken = StrategicMap.Current.Players.GetPlayers(action.Target).CreateDiplomaticAction(new BreakAlliance(), action.OtherPlayer, -1);
                if (broken != null)
                {
                    Broadcast(broken, action.Source, action.OtherPlayer);
                    broken.Release();
                }
            }

            ReturnToSource(action);
        }

        public override DiplomaticActionReaction RejectReaction(DiplomaticAction action) =>
            Relation(action.Target, action.OtherPlayer) == DiplomaticRelation.Alliance ? DiplomaticActionReaction.Reject : DiplomaticActionReaction.Ignore;

        public override void AnswerAI(DiplomaticAction action, sbyte player, DiplomaticRelation relation, ByteList idealPeace, ByteList idealWar)
        {
            string errorText = "";
            if (DiplomaticProposal.Relation(action.Target, action.OtherPlayer) != DiplomaticRelation.Alliance)
            {
                action.Accept(ref errorText);
            }
            else
            {
                action.Reject(ref errorText);
            }
        }

        public override int RelationModifier(DiplomaticAction action, sbyte player, sbyte race) =>
            player == action.Target && Relation(action.Target, action.OtherPlayer) == DiplomaticRelation.Alliance
                ? RaceModifier(action.OtherPlayer, race, -40)
                : 0;

        private static readonly string s_suggestAllianceBreakFormat = "Suggest alliance break with %s";

        internal static string BreakAllianceSuggestionText { get; } = "Break alliance suggestion";
    }

    // Kind 9: the source asks the target to declare war on the other player.
    public sealed partial record SuggestWarDeclaration(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 9;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 47;

        public override string Name(DiplomaticAction action)
        {
            var name = StrategicMap.Current.Players.GetPlayers(action.OtherPlayer).GetName();
            return AbilityRules.Format(AbilityRules.TranslateResourceString(s_suggestWarFormat), [name]);
        }

        public override string LogText(DiplomaticAction action)
        {
            var subject = AbilityRules.TranslateResourceString(GameText.DeclareWarText);
            return Outcome(action, AbilityRules.TranslateResourceString(DeclareWarSuggestionText), subject, AcceptedLine(subject), "");
        }

        public override int MiddleIcon(DiplomaticAction action) =>
            action.Reaction == DiplomaticActionReaction.Accept ? 0x15 : 0x16;

        public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation) =>
            text.Exchange(reaction, 157, 145, 148, true);

        public override void Sent(DiplomaticAction action) => Deliver(action);

        public override bool LapsesUnanswered(DiplomaticAction action) =>
            Relation(action.Target, action.OtherPlayer) != DiplomaticRelation.War;

        public override void Accepted(DiplomaticAction action)
        {
            if (Relation(action.Target, action.OtherPlayer) != DiplomaticRelation.War)
            {
                StrategicMap.Current.Players.GetPlayers(action.OtherPlayer).PlayerSeen((sbyte)(byte)action.Target);
                StrategicMap.Current.Players.SetDiplomaticRelation((sbyte)(byte)action.Target, (sbyte)(byte)action.OtherPlayer, DiplomaticRelation.War);
                var war = StrategicMap.Current.Players.GetPlayers(action.Target).CreateDiplomaticAction(new DeclareWar(), action.OtherPlayer, -1);
                if (war != null)
                {
                    Broadcast(war, action.Source, action.OtherPlayer);
                    war.Release();
                }
            }

            ReturnToSource(action);
        }

        public override DiplomaticActionReaction RejectReaction(DiplomaticAction action) =>
            Relation(action.Target, action.OtherPlayer) != DiplomaticRelation.War ? DiplomaticActionReaction.Reject : DiplomaticActionReaction.Ignore;

        public override void AnswerAI(DiplomaticAction action, sbyte player, DiplomaticRelation relation, ByteList idealPeace, ByteList idealWar)
        {
            string errorText = "";
            if (DiplomaticProposal.Relation(action.Target, action.OtherPlayer) == DiplomaticRelation.War)
            {
                action.Accept(ref errorText);
            }
            else
            {
                if (idealWar.IndexOf((byte)action.OtherPlayer) != -1)
                {
                    if (StrategicMap.Current.Players.GetPlayers(action.Target).AIControl.CanDeclareWar((byte)action.OtherPlayer))
                    {
                        if (StrategicMap.Current.Random(5) == 1 && StrategicMap.Current.Random(550) + 250 < action.EscrowGold + action.EscrowMana)
                        {
                            action.Accept(ref errorText);
                        }
                        else
                        {
                            action.Reject(ref errorText);
                        }
                    }
                    else
                    {
                        action.Reject(ref errorText);
                    }
                }
                else
                {
                    action.Reject(ref errorText);
                }
            }
        }

        public override int RelationModifier(DiplomaticAction action, sbyte player, sbyte race)
        {
            if (player == action.Target && Relation(action.Target, action.OtherPlayer) != DiplomaticRelation.War)
            {
                int modifier = Relation(action.Target, action.OtherPlayer) == DiplomaticRelation.Alliance ? -60 : -20;
                return RaceModifier(action.OtherPlayer, race, modifier);
            }

            return 0;
        }

        private static readonly string s_suggestWarFormat = "Suggest war with %s";

        internal static string DeclareWarSuggestionText { get; } = "Declare war suggestion";
    }

    // intf_P1_03.inc:248

    // intf_P1_03.inc:248

}

public enum PlayerBehavior : byte
{
    Normal = 0, Aggressor = 1, Defender = 2, Expander = 3, Scorcher = 4, Random = 5
}

public enum PlayerStatus : byte
{
    Idle = 0, Playing = 1, Terminated = 2
}
