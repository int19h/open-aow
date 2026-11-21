#nullable disable
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine;
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
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

using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Session;

// update count, events, structures, victory locations, armies, messages, legacy relations,
// diplomacy, game over, player status, busy count, morale, leader setup, statistics, income
// sources, known structures, heroes, groups, AI control, budget, diplomatic actions, attack
// players, seen days, PBEM, emerging hero, participating, initial gold, base income, hero join
// days, upkeep. end turn, view, password, turn number, base hero level, emerging hero ID,
// emerge day, highest hero level, settings/description
public sealed class Player : EngineObject, IOwned<PlayerCollection>, IOwnedSink<PlayerCollection>
{
    // The list that holds the player. AcceptsOwner admits only that list, so the cast never fails.
    public new PlayerCollection Owner => GetOwner<PlayerCollection>();

    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is PlayerCollection;

    void IOwnedSink<PlayerCollection>.SetOwner(PlayerCollection owner) => SetOwner(owner);

    public new const int ClassId = 131_104;

    // Player.PAS:261
    public Player(CustomNode owner) : base(owner)
    {
        _incomeChangedEventsStore = new();
        _gemsChangedEventsStore = new();
        _upkeepChangedEventsStore = new();
        Structures = new PlayerStructureList(null)
        {
            Holder = CreationHolder(),
            Player = this
        };
        _armies = new ArmyList(null) { Holder = CreationHolder() };
        AIControl = new AIPlayerControl(null)
        {
            Holder = CreationHolder(),
            Player = this
        };
        AIBudget = new AIBudgetManager(null)
        {
            Holder = CreationHolder(),
            Player = this
        };
        AIGroups = new AIGroupList(null) { Holder = CreationHolder() };
        MagicControl = new PlayerMagicControl(null)
        {
            Holder = CreationHolder(),
            Player = this
        };
        Statistics = new PlayerStatistics(null) { Holder = CreationHolder() };
        _armies.Player = this;
        _armies.Events += ArmiesChanged;
        Heroes = new HeroList(null) { Holder = CreationHolder() };
        InitialGold = 250;
        BaseIncome = 0;
        PlayerIndex = -1;
        Participating = false;
        EventLogbook = new EventLogbook(null) { Holder = CreationHolder() };
        EventLogbook.OnChanged += LogbookChangedEvent;
        _logbookChangedEventsStore = new();
        DiplomaticRelations = new PlayerDiplomaticRelations(null)
        {
            Holder = CreationHolder(),
            Player = this
        };
        LegacyNewRelations = new DiplomaticRelationList();
        ProductionControls = new ProductionControlList();
        _incomeSources = new IncomeSourceList(null) { Holder = CreationHolder() };
        LeaderSetup = new Leader(null) { Holder = CreationHolder() };
        Status = PlayerStatus.Idle;
        GameOverStatus = GameOverStatus.None;
        FirstDaySeen = new Int32List
        {
            Default = -1
        };
        _lastDaySeen = new Int32List
        {
            Default = -1
        };
        _victoryLocations = new PlayerStructureList(null) { Holder = CreationHolder() };
        KnownStructures = new PlayerStructureList(null) { Holder = CreationHolder() };
        _messages = new DelphiStringList();
        DiplomaticActions = new DiplomaticActionList(null) { Holder = CreationHolder() };
        AIValidAttackPlayers = new BitList();
        PbemSettings = new PbemPlayerSettings(null) { Holder = CreationHolder() };
        LegacyRelations = new DiplomaticRelationList();
        HeroEmerging = false;
        BaseHeroJoinDay = -1;
        HeroJoinDay = -1;
        PlayerRace = -1;
        {
            View = View with { X = -1 };
            View = View with { Y = -1 };
        }
    }

    private int _updateCount;

    private readonly EventStore<Player> _gemsChangedEventsStore;

    public event NotifyEvent<Player> GemsChanged
    {
        add => _gemsChangedEventsStore.Add(value);
        remove => _gemsChangedEventsStore.Remove(value);
    }

    private readonly EventStore<Player> _incomeChangedEventsStore;

    public event NotifyEvent<Player> IncomeChanged
    {
        add => _incomeChangedEventsStore.Add(value);
        remove => _incomeChangedEventsStore.Remove(value);
    }

    private readonly EventStore<Player> _upkeepChangedEventsStore;

    public event NotifyEvent<Player> UpkeepChanged
    {
        add => _upkeepChangedEventsStore.Add(value);
        remove => _upkeepChangedEventsStore.Remove(value);
    }

    private readonly EventStore<Player> _logbookChangedEventsStore;

    public event NotifyEvent<Player> LogbookChanged
    {
        add => _logbookChangedEventsStore.Add(value);
        remove => _logbookChangedEventsStore.Remove(value);
    }

    public PlayerStructureList Structures { get; }

    private readonly PlayerStructureList _victoryLocations;

    public ProductionControlList ProductionControls { get; }

    private readonly ArmyList _armies;

    /// <summary>The armies registered with this player, in the list's child slots.</summary>
    public ArmyCollection Armies => new(_armies);
    public bool SingleProcessCycle { get; private set; }

    public MapCoordinate View { get; set; }

    private string _password = "";
    private DelphiStringList _messages;

    public DiplomaticRelationList LegacyRelations { get; }
    public DiplomaticRelationList LegacyNewRelations { get; }
    public PlayerDiplomaticRelations DiplomaticRelations { get; }

    public GameOverStatus GameOverStatus { get; private set; }
    public PlayerStatus Status { get; set; }

    private int _busyCount;

    public string Name { get; set; } = "";

    public sbyte MoraleValue { get; set; }

    public PlayerMagicControl MagicControl { get; }
    public Leader LeaderSetup { get; }

    public int Day { get; set; }

    public PlayerStatistics Statistics { get; }

    private readonly IncomeSourceList _incomeSources;

    public PlayerStructureList KnownStructures { get; }

    public int BaseHeroJoinLevel { get; private set; }

    private int _lastHeroJoinDay;

    public int EmergingHeroId { get; private set; }

    private int _lastHeroEmergeDay;
    private int _highestHeroLevel;

    public HeroList Heroes { get; }
    public AIGroupList AIGroups { get; }
    public AIPlayerControl AIControl { get; }
    public AIBudgetManager AIBudget { get; }
    public DiplomaticActionList DiplomaticActions { get; private set; }
    public BitList AIValidAttackPlayers { get; private set; }
    public Int32List FirstDaySeen { get; }

    private readonly Int32List _lastDaySeen;

    public PbemPlayerSettings PbemSettings { get; private set; }

    public bool HeroEmerging { get; set; }
    public sbyte PlayerRace { get; set; }
    public sbyte PlayerIndex { get; set; }
    public PlayerType PlayerType { get; set; }
    public PlayerBehavior Behavior { get; set; }
    public bool Participating { get; private set; }
    public int InitialGold { get; set; }
    public int BaseIncome { get; private set; }
    public int BaseHeroJoinDay { get; set; }

    public int HeroJoinDay { get; set; }

    public string Settings { get; set; } = "";

    private string _description = "";

    public int Gems { get; private set; }

    public int ArmyUpkeep { get; private set; }

    private int _magicUpkeep;

    public int Income { get; private set; }

    public Leader Leader { get; set; }

    public EventLogbook EventLogbook { get; }

    // Player.PAS:315
    public override void Dispose()
    {
        try
        {
            _incomeChangedEventsStore.Clear();
            _gemsChangedEventsStore.Clear();
            _upkeepChangedEventsStore.Clear();
            try
            {
                if (StrategicMap.Current is not null && StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Destroying))
                {
                    Assert(_victoryLocations.Count == 0, message: "Victory Locations not correctly cleared (TPlayer)", file: "Player.PAS", line: 326);
                    Assert(Structures.Count == 0, message: "Registered Player Structures not correctly cleared (TPlayer)", file: "Player.PAS", line: 327);
                    Assert(Armies.Count == 0, message: "Registered Player Armies not correctly cleared (TPlayer)", file: "Player.PAS", line: 328);
                }
                else
                {
                    PlayerStructureList structures = Structures;
                    for (int i = structures.Count - 1; i >= 0; i--)
                    {
                        Structures.GetStructure(i).SetPlayer(0);
                    }

                    ArmyCollection armies = Armies;
                    for (int i = armies.Count - 1; i >= 0; i--)
                    {
                        Armies.GetArmy(i).SetPlayer(0);
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
                _armies.Dispose();
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
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        // The translate stream's mode is read and text only. The original ReadWrite leaves out the
        // settings there, visits and registers the leader, keeps the view, visits the leader setup
        // and does the work of the read branch.
        bool translating = persister.Translating();
        PlayerRace = persister.UnsignedByte(name: "PlayerRace", value: PlayerRace);
        PlayerType = persister.Enum("PlayerType", value: PlayerType);
        Participating = persister.Boolean("Participating", value: Participating);
        InitialGold = persister.Int32("InitialGold", value: InitialGold);
        BaseIncome = persister.Int32("BaseIncome", value: BaseIncome);
        Status = persister.Enum("Status", value: Status);
        if (!translating)
        {
            Settings = persister.String("Settings", value: Settings);
        }

        _description = persister.String("Description", value: _description);
        Gems = persister.Int32("Gems", value: Gems);
        PlayerIndex = persister.UnsignedByte(name: "PlayerIndex", value: PlayerIndex);
        if ((GetEngine().Status & (HexSpriteEngineStatuses.LoadingHsm | HexSpriteEngineStatuses.SavingHsm)) != HexSpriteEngineStatuses.None || translating)
        {
            if (persister.Loading || translating)
            {
                Leader = persister.Created("Leader", objectValue: Leader);
                if (Leader is not null && !persister.Preview)
                {
                    Leader.Player = PlayerIndex;
                    _ = StrategicMap.Current.HeroControl.RegisterHero(Leader);
                }
            }
        }
        else
        {
            persister.Skip("Leader");
        }

        if (!persister.Loading && !translating)
        {
            if (StrategicMap.Current.SeatedPlayer == PlayerIndex && StrategicMap.Current.ConnectionType != PlayerConnectionType.Network)
            {
                StrategicMap.Current.GetViewCenter(out int x, out int y, out int l);
                View = new MapCoordinate(unchecked((sbyte)(byte)x), unchecked((sbyte)(byte)y), unchecked((sbyte)(byte)l));
                View = persister.Coordinate(name: "View", value: View, width: CoordinateWidth.Byte);
            }
        }
        else
        {
            View = persister.Coordinate(name: "View", value: View, width: CoordinateWidth.Byte);
        }

        if (!persister.Preview)
        {
            persister.Object("EventLogbook", objectValue: EventLogbook);
        }
        else
        {
            persister.Skip("EventLogbook");
        }

        _password = persister.String("Password", value: _password);
        GameOverStatus = persister.Enum("GameOverStatus", value: GameOverStatus);
        if (persister.Loading)
        {
            persister.ByteList(name: "LegacyNewRelations", list: LegacyNewRelations);
        }

        MoraleValue = persister.UnsignedByte(name: "MoraleValue", value: MoraleValue);
        persister.Object("AIControl", objectValue: AIControl);
        Behavior = persister.Enum("Behavior", value: Behavior);
        if (PlayerIndex != 0)
        {
            if (!persister.Loading && !translating && StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting) && Leader is not null)
            {
                persister.Object("LeaderSetup", objectValue: Leader);
            }
            else
            {
                persister.Object("LeaderSetup", objectValue: LeaderSetup);
            }
        }
        else
        {
            persister.Skip("LeaderSetup");
        }

        persister.Object("MagicControl", objectValue: MagicControl);
        persister.Int32List(name: "FirstDaySeen", list: FirstDaySeen);
        persister.Int32List(name: "LastDaySeen", list: _lastDaySeen);
        Day = persister.Int32("Day", value: Day);
        persister.Object("Statistics", objectValue: Statistics);
        BaseHeroJoinLevel = persister.Int32("BaseHeroJoinLevel", value: BaseHeroJoinLevel);
        _lastHeroJoinDay = persister.Int32("LastHeroJoinDay", value: _lastHeroJoinDay);
        _lastHeroEmergeDay = persister.Int32("LastHeroEmergeDay", value: _lastHeroEmergeDay);
        _highestHeroLevel = persister.Int32("HighestHeroLevel", value: _highestHeroLevel);
        if (persister.Loading || translating)
        {
            SetRace(PlayerRace);
            EventLogbook.Player = PlayerIndex;
            LegacyNewRelations.Default = (int)DiplomaticRelation.Unknown;
        }

        persister.Lines(name: "Messages", lines: _messages);
        if (persister.Loading)
        {
            persister.ByteList(name: "LegacyRelations", list: LegacyRelations);
        }

        EmergingHeroId = persister.Int32("EmergingHeroId", value: EmergingHeroId);
        persister.Object("AIBudget", objectValue: AIBudget);
        persister.Object("DiplomaticActions", objectValue: DiplomaticActions);
        BitListImage attackPlayers = persister.BitList(name: "AIValidAttackPlayers", list: AIValidAttackPlayers);
        persister.Object("PbemSettings", objectValue: PbemSettings);
        HeroEmerging = persister.Boolean("HeroEmerging", value: HeroEmerging);
        if (persister.Loading || translating || StrategicMap.Current.ConnectionType != PlayerConnectionType.Network)
        {
            BaseHeroJoinDay = persister.Int32("BaseHeroJoinDay", value: BaseHeroJoinDay);
        }

        persister.Object("DiplomaticRelations", objectValue: DiplomaticRelations);
        // The stream load of the list reads the start of the data stored after it, the members
        // from PbemSettings on, into the bits past a stored count that does not fill 32 bits.
        if (attackPlayers != null)
        {
            persister.BitListOverRead(list: AIValidAttackPlayers, stored: attackPlayers, owner: this, following: tail =>
            {
                tail.Object("PbemSettings", objectValue: PbemSettings);
                _ = tail.Boolean("HeroEmerging", value: HeroEmerging);
                _ = tail.Int32("BaseHeroJoinDay", value: BaseHeroJoinDay);
                tail.Object("DiplomaticRelations", objectValue: DiplomaticRelations);
            });
        }

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
                    View = View with { X = -1 };
                    if (StrategicMap.Current.GameType != GameType.Campaign && !StrategicMap.Current.FixedLeaders && Leader is not null)
                    {
                        Leader.Owner = null;
                        _ = MagicControl.UnregisterPowerSource(Leader.PowerSource);
                        // { Original removes the player, not the leader. }
                        _ = Heroes.RemoveChild(this);
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
            if (!Validate(message.Payload.As<DelphiStringCollection>()))
            {
                Array4<int> data = message.Data;
                data[0] = 1;
                message.Data = data;
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
            View = new MapCoordinate(-1, -1, -1);
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
        result = result || (StrategicMap.Current.Combat is null && StrategicMap.Current.PlayMode == PlayMode.Turn && StrategicMap.Current.CurrentPlayer != PlayerIndex);
        result = result || (StrategicMap.Current.Combat is not null && StrategicMap.Current.Combat.CombatData.Players.GetPlayers((sbyte)StrategicMap.Current.Combat.CombatData.CurrentPlayer).Player != PlayerIndex);
        result = result || GameOverStatus != GameOverStatus.None || !GetLocal();
        return result || (PlayerType == PlayerType.Human && StrategicMap.Current.TurnTimerEnabled && StrategicMap.Current.TurnTimeLeft <= 0);
    }

    // Player.PAS:406
    public int Superiority
    {
        get
        {
            if (Statistics.TurnInfo.Count != 0)
            {
                int count = Statistics.TurnInfo.Count;
                return Statistics.TurnInfo.GetTurnInfo(count - 1).Superiority;
            }
            else
            {
                return 0;
            }
        }
    }

    // Player.PAS:412
    public int GetUnitCount()
    {
        int result = 0;
        for (int i = Armies.Count - 1; i >= 0; i--)
        {
            result += Armies.GetArmy(i).Units.Count;
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
        if (value != GameOverStatus && GameOverStatus == GameOverStatus.None)
        {
            GameOverStatus = value;
            StrategicMap.Current.AIExecutor.DeactivatePlayer(PlayerIndex);
            if (StrategicMap.Current.GameType == GameType.Campaign && PlayerType == PlayerType.Human)
            {
                Campaign campaign;
                if (GameOverStatus == GameOverStatus.Lost)
                {
                    StrategicMap.Current.GameResult = StrategicMapGameResult.Failed;
                    campaign = GetEngine().Campaign;
                    campaign?.RestartScenario();
                }
                else
                {
                    if (GameOverStatus == GameOverStatus.Won)
                    {
                        StrategicMap.Current.GameResult = StrategicMapGameResult.Succeeded;
                        campaign = GetEngine().Campaign;
                        campaign?.FinishScenario();
                    }
                }
            }

            if (GameOverStatus == GameOverStatus.Lost)
            {
                BeginUpdate();
                MagicControl.RemoveEnchantments();
                PlayerStructureList structures = Structures;
                for (int i = structures.Count - 1; i >= 0; i--)
                {
                    Structures.GetStructure(i).SetPlayer(0);
                }

                ArmyCollection armies = Armies;
                for (int i = armies.Count - 1; i >= 0; i--)
                {
                    Armies.GetArmy(i).SetPlayer(0);
                }

                MagicControl.SetMana(0);
                Gems = 0;
                EndUpdate();
                StrategicMap.Current.Players.Update();
            }

            GameOverEventLog eventValue = new(null)
            {
                Holder = CreationHolder(),
                Player = PlayerIndex
            };
            if (value == GameOverStatus.Lost)
            {
                string raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNamesPluralTexts[PlayerRace]);
                string name = GetName();
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_gameOverDefeatMessageFormat), arguments: [StrategicMap.Current.Day.ToString(CultureInfo.InvariantCulture), raceNamesPlural, name]);
                eventValue.Message.Text = format;
            }
            else
            {
                string raceNamesPluralText = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNamesPluralTexts[PlayerRace]);
                string name = GetName();
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_gameOverVictoryMessageFormat), arguments: [StrategicMap.Current.Day.ToString(CultureInfo.InvariantCulture), raceNamesPluralText, name]);
                eventValue.Message.Text = format;
            }

            StrategicMap.Current.ExecuteGameOverEvent(eventValue);
            _ = eventValue.Release();
            if (GameOverStatus == GameOverStatus.Lost)
            {
                StrategicMap.Current.GlobalMagicControl.RemovePlayerEnchantments(PlayerIndex);
            }
        }
    }

    // Player.PAS:484
    public bool Surrender()
    {
        if (!GetBusy() && GameOverStatus == GameOverStatus.None)
        {
            GameOverTokenEvent tokenEvent = new(null)
            {
                Holder = CreationHolder(),
                Player = PlayerIndex,
                GameOverStatus = GameOverStatus.Lost
            };
            StrategicMap.Current.TokenManager.GetTokenControl(PlayerIndex).AddEvent(tokenEvent);
            _ = tokenEvent.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Player.PAS:499
    public bool Win()
    {
        if (!GetBusy() && GameOverStatus == GameOverStatus.None)
        {
            GameOverTokenEvent tokenEvent = new(null)
            {
                Holder = CreationHolder(),
                Player = PlayerIndex,
                GameOverStatus = GameOverStatus.Won
            };
            StrategicMap.Current.TokenManager.GetTokenControl(PlayerIndex).AddEvent(tokenEvent);
            _ = tokenEvent.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Player.PAS:515
    public bool Validate(DelphiStringCollection errors)
    {
        bool result;
        if (PlayerIndex > 0)
        {
            result = true;
            string s;
            string leaderName;
            if (Leader.SphereCount != StrategicMap.Current.SphereCount)
            {
                string raceNames = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNames[PlayerRace]);
                leaderName = AbilityRules.Format(s_leaderFormat, arguments: [raceNames]);
                s = $"{leaderName}: {AbilityRules.TranslateResourceString(GetEngine(), S: s_notAllSpheresAssignedText)}";
                _ = errors.Add(s);
                result = false;
            }

            if (Leader.Owner is null)
            {
                string raceNamesText = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNames[PlayerRace]);
                leaderName = AbilityRules.Format(s_leaderFormat, arguments: [raceNamesText]);
                s = $"{leaderName}: {AbilityRules.TranslateResourceString(GetEngine(), S: s_notPlacedOnMapText)}";
                _ = errors.Add(s);
                result = false;
            }

            if (Leader.GetSkillPoints() != 0)
            {
                string translatedText = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNames[PlayerRace]);
                leaderName = AbilityRules.Format(s_leaderFormat, arguments: [translatedText]);
                s = $"{leaderName}: {AbilityRules.TranslateResourceString(GetEngine(), S: s_notAllSkillPointsAssignedText)}";
                _ = errors.Add(s);
            }
        }
        else
        {
            // { Original reports unassigned skill points without failing validation. }
            return true;
        }

        return result;
    }

    // Player.PAS:547
    public void LockBusy() => _busyCount += 1;

    // Player.PAS:552
    public void UnlockBusy()
    {
        Assert(_busyCount != 0, message: "Invalid busy counter usage", file: "Player.PAS", line: 554);
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
        _ = Structures.BeginUpdate();
        _ = _armies.BeginUpdate();
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
        _armies.EndUpdate();
        MagicControl.EndUpdate();
    }

    // Player.PAS:586
    public void ArmiesChanged(object sender)
    {
        if (Armies.Count == 0)
        {
            StrategicMap.Current.GameOver(PlayerIndex, status: GameOverStatus.Lost);
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
            StrategicMap.Current.GameOver(PlayerIndex, status: GameOverStatus.Won);
        }
    }

    // Player.PAS:600
    public void UpdateIncome()
    {
        Income = BaseIncome;
        for (int i = _incomeSources.Count - 1; i >= 0; i--)
        {
            int income = _incomeSources[i].Income();
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
                    Income = Round(Income * 2.5) + 50;
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
            _ = _incomeSources.AddChild(source);
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
            if (Leader is not null)
            {
                result = Leader.HeroName;
                if (!IsEmpty(Leader.GetNickname()))
                {
                    string nickname = Leader.GetNickname();
                    result = $"{result} {nickname}";
                }
            }
            else
            {
                result = Name;
            }

            if (IsEmpty(result))
            {
                return $"{AbilityRules.TranslateResourceString(GetEngine(), S: s_playerText)} {((int)PlayerIndex).ToString(CultureInfo.InvariantCulture)}";
            }
        }
        else
        {
            return AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.IndependentsText);
        }

        return result;
    }

    // Player.PAS:663
    public string GetNameWithRace()
    {
        if (PlayerIndex != 0)
        {
            string name = GetName();
            string raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNamesPluralTexts[PlayerRace]);
            return AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_playerNameWithRaceFormat), arguments: [name, raceNamesPlural]);
        }
        else
        {
            return GetName();
        }
    }

    // Player.PAS:671
    public void SetName(string value)
    {
        if (PlayerIndex != 0)
        {
            if (Leader is not null)
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
    public LibraryImage GetFace() => Leader?.GetFace();

    // Player.PAS:688
    public int HeroCountMax
    {
        get
        {
            int count = Statistics.TurnInfo.Count;
            int strength = Statistics.TurnInfo.GetTurnInfo(count - 1).ArmyStrength;
            int result = strength < 10
                ? 1
                : strength < 40 ? 2 : strength < 80 ? 3 : strength < 120 ? 4 : strength < 200 ? 5 : ((strength - 200) / 100) + 5;

            int maximum = StrategicMap.Current.HeroCountMax;
            return maximum < result ? maximum : result;
        }
    }

    // Player.PAS:703
    public int GetIdealHeroJoinDay()
    {
        int result = _lastHeroJoinDay;
        int average = StrategicMap.Current.Players.GetAverageHeroCount();
        int maximum = StrategicMap.Current.Players.GetMaxHeroCount();
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
        int result = GetIdealHeroJoinDay();
        int average = StrategicMap.Current.Players.GetAverageHeroCount();
        int maximum = StrategicMap.Current.Players.GetMaxHeroCount();
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

        int earliest = _lastHeroEmergeDay + 5;
        return earliest > result ? earliest : result;
    }

    // Player.PAS:733
    public void ResetBaseHeroJoinDay() => _lastHeroJoinDay = StrategicMap.Current.Day;

    // Player.PAS:738
    public void MakeHeroEmerge(HeroEmergeMode mode)
    {
        DoubleItemList<Hero> heroes = new();
        int i;
        Hero hero;
        if (mode == HeroEmergeMode.Normal)
        {
            i = StrategicMap.Current.HeroControl.GetCount() - 1;
            for (; i >= 0; i -= 1)
            {
                hero = StrategicMap.Current.HeroControl[i];
                if (!hero.HeroStatus.Contains(HeroStatuses.Died) && hero.Player <= 0 && hero.Owner is null && hero.CanJoin(PlayerIndex))
                {
                    int chance = GetHeroJoinChance(hero.GetLevel());
                    if (chance > 0 && hero.JoinAmount(PlayerIndex) < Gems)
                    {
                        _ = heroes.AddAbility(hero, chance * chance);
                    }
                }
            }
        }
        else
        {
            if (mode == HeroEmergeMode.FirstHero)
            {
                i = StrategicMap.Current.HeroControl.GetCount() - 1;
                for (; i >= 0; i -= 1)
                {
                    hero = StrategicMap.Current.HeroControl[i];
                    if (!hero.HeroStatus.Contains(HeroStatuses.Died) && hero.Player <= 0 && hero.Owner is null && hero.GetAbilityEnabled(52) && hero.GetLevel() >= 2 && hero.GetLevel() <= 3)
                    {
                        Status playerRelation = StrategicMap.Current.Races.GetRace(hero.Race).GetPlayerRelation(PlayerIndex);
                        if (playerRelation is Session.Status.Polite or Session.Status.Friendly && hero.JoinAmount(PlayerIndex) < Gems)
                        {
                            sbyte level = hero.GetLevel();
                            _ = heroes.AddAbility(hero, ((level - 2) * 1_000) + 1);
                        }
                    }
                }
            }
        }

        if (heroes.Count != 0)
        {
            MapCoordinateList locations = new();
            i = Structures.Count - 1;
            Structure structure;
            for (; i >= 0; i -= 1)
            {
                structure = Structures.GetStructure(i);
                if (structure.EngineClass.ClassId == City.ClassId)
                {
                    _ = locations.AddMapCoordinate(mapLevel: structure.HexLevel, y: structure.HexY, x: structure.HexX);
                }
            }

            if (locations.Count != 0)
            {
                _lastHeroEmergeDay = StrategicMap.Current.Day;
                RandSeed = StrategicMap.Current.Random(1_048_575);
                i = heroes.RandomChanceItemB(0);
                DoubleItem<Hero> doubleItem = heroes[i];
                hero = doubleItem.First;
                i = StrategicMap.Current.Random(locations.Count);
                EmergingHeroId = hero.UnitId;
                hero.UpdateSettings();
                MapCoordinate item = locations[i];
                MapCoordinate locationsItem = locations[i];
                _ = hero.Place(locations[i].X, locationsItem.Y, L: item.MapLevel, player: 0, mode: UnitPlaceModes.ScanArea | UnitPlaceModes.CreateNewArmy);
                MapCoordinate item2 = locations[i];
                MapCoordinate item3 = locations[i];
                MapCoordinate item4 = locations[i];
                structure = StrategicMap.Current.MapContainer.GetField(item4.X, item3.Y, item2.MapLevel).FindNoneTransparentHexSprite<Structure>();
                HeroEmergeEventLog log = new(null) { Holder = CreationHolder() };
                log.Setup(hero, player: PlayerIndex, mode: HeroEmergeEventLog.HeroJoinMode.Emerged);
                log.LocationName = structure is not null
                    ? structure.GetName()
                    : AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.UnknownText);

                EventLogbook.AddEvent(log);
                if (PlayerIndex == StrategicMap.Current.SeatedPlayer && StrategicMap.Current.Players.GetPlayers(PlayerIndex).PlayerType == PlayerType.Human)
                {
                    _ = log.Execute(false, trigger: ExecuteTrigger.Event, done: null);
                }

                _ = log.Release();
            }

            locations?.Dispose();
        }

        heroes?.Dispose();
    }

    // Player.PAS:812
    public void UpdateHeroJoin()
    {
        for (int i = Heroes.Count - 1; i >= 0; i--)
        {
            if (Heroes.GetHero(i).GetLevel() > _highestHeroLevel)
            {
                _highestHeroLevel = Heroes.GetHero(i).GetLevel();
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

        if (Heroes.Count == HeroCountMax)
        {
            ResetBaseHeroJoinDay();
        }

        EmergingHeroId = 0;
        int days = GetIdealHeroEmergeDay() - StrategicMap.Current.Day;
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
        int maximum = GetHeroJoinLevelMax();
        int result;
        if (level <= maximum)
        {
            result = Round((double)((double)(maximum + 2) / (level + 2)) * 20);
            if (result > 40)
            {
                return 40;
            }
        }
        else
        {
            return 0;
        }

        return result;
    }

    // Player.PAS:849
    public int GetHeroJoinLevelMax()
    {
        int result = BaseHeroJoinLevel;
        int idealHeroJoinDay = GetIdealHeroJoinDay();
        int days = StrategicMap.Current.Day - idealHeroJoinDay;
        if (days > 0)
        {
            result += days / 5;
        }

        if (days < 0)
        {
            result += Round((double)((result * days) + 2.5) / 5);
            if (result < 0)
            {
                result = 0;
            }
        }

        return result < 3 && !StrategicMap.Current.FixedLeaders && StrategicMap.Current.Day == 1 ? 3 : result;
    }

    // Player.PAS:865
    public void ValidateLeader()
    {
        if (StrategicMap.Current is not null && PlayerIndex != 0)
        {
            HeroType heroType = HeroType.HeroLeader;
            int resourceIndex;
            if (Leader is not null && Leader.HeroType != heroType)
            {
                string name = Leader.HeroName;
                int faceIndex = Leader.FaceIndex;
                Leader.Deactivate();
                resourceIndex = GamePack.Current.HeroResources.Find(PlayerRace, heroType);
                Leader.SetHeroResourceIndex(resourceIndex);
                Leader.HeroName = name;
                Leader.SetFaceIndex(faceIndex);
                Leader.Activate();
            }
            else
            {
                if (Leader is null)
                {
                    resourceIndex = GamePack.Current.HeroResources.Find(PlayerRace, heroType);
                    if (resourceIndex != -1)
                    {
                        Leader = new Leader(null) { Holder = CreationHolder() };
                        Leader.SetHeroResourceIndex(resourceIndex);
                        Leader.Player = PlayerIndex;
                        Leader.Activate();
                    }
                }
                else
                {
                    if (Leader is not null && !StrategicMap.Current.FixedLeaders && StrategicMap.Current.GameType != GameType.Campaign && !StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
                    {
                        Leader.Owner = null;
                    }
                }
            }
        }
    }

    // Player.PAS:902
    public void SetLeader(Leader value) => Leader = value;

    // Player.PAS:907
    public void SetRace(sbyte value) => PlayerRace = value;

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
                EventLog logs = EventLogbook[0];
                condition = StrategicMap.Current.Day - logs.Day >= 3;
            }

            if (!condition)
            {
                break;
            }

            EventLog fEventLogbookLogs = EventLogbook[0];
            EventLogbook.RemoveEvent(fEventLogbookLogs);
        }
    }

    // Player.PAS:928
    public void UpdateTurnInfo()
    {
        TurnInfoItem info = new(Statistics.TurnInfo);
        if (GameOverStatus != GameOverStatus.Lost)
        {
            info.Gold = Gems;
            info.Mana = MagicControl.Mana;
            info.Structures = Structures.Count;
            info.Heroes = (byte)Heroes.Count;
            for (int i = 0, last = Armies.Count - 1; i <= last; i++)
            {
                Army army = Armies.GetArmy(i);
                for (int j = army.Units.Count - 1; j >= 0; j--)
                {
                    Troop troop = army.Units.GetUnit(j);
                    if (info.ArmyStrength < 1_073_741_823)
                    {
                        int strengthMax = troop.GetStrengthMax(AbilityRules.AllDamageTypes & ~DamageTypes.Wall);
                        info.ArmyStrength += strengthMax;
                    }
                }
            }
        }
    }

    // Player.PAS:955
    public void UpdateAlliedPlayersSeen()
    {
        for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            if (i != PlayerIndex)
            {
                if (StrategicMap.Current.Players.GetDiplomaticRelation(PlayerIndex, otherPlayer: (sbyte)i) == DiplomaticRelation.Alliance)
                {
                    PlayerSeen((sbyte)i);
                }
            }
        }
    }

    // Player.PAS:965
    public void SetupAIValidAttackPlayers()
    {
        for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            if (i != PlayerIndex)
            {
                DiplomaticRelation diplomaticRelation = DiplomaticRelations.GetDiplomaticRelation((sbyte)i);
                AIValidAttackPlayers[i] = diplomaticRelation == DiplomaticRelation.War;
            }
        }
    }

    // Player.PAS:1108
    public int GetFirstDaySeen(sbyte player) => player != PlayerIndex ? player < FirstDaySeen.Count ? FirstDaySeen[player] : -1 : 0;

    // Player.PAS:1116
    public int GetLastDaySeen(sbyte player) => player != PlayerIndex ? player < _lastDaySeen.Count ? _lastDaySeen[player] : -1 : StrategicMap.Current.Day;

    // Player.PAS:1124
    public void PlayerSeen(sbyte player)
    {
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting) && (player >= _lastDaySeen.Count || _lastDaySeen[player] != StrategicMap.Current.Day))
        {
            _lastDaySeen[player] = StrategicMap.Current.Day;
            if (GetFirstDaySeen(player) == -1)
            {
                FirstDaySeen[player] = StrategicMap.Current.Day;
            }
        }
    }

    // Player.PAS:1134
    public void GetLeaderLocation(out sbyte X, out sbyte Y, out sbyte L)
    {
        if (Leader?.UnitHolder is { } holder)
        {
            MapCoordinate coordinate24 = holder.GetLocation();
            X = coordinate24.X;
            Y = coordinate24.Y;
            L = coordinate24.MapLevel;
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
        PlayerCommunicationControl control = StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl;
        if (control is not null)
        {
            PlayerCommunicationSettings players = control.GetPlayers(PlayerIndex);
            return players.ConnectionMode is ConnectionMode.Local or ConnectionMode.Global;
        }
        else
        {
            return true;
        }
    }

    // Player.PAS:1159
    public void RegisterVictoryLocation(PlayerStructure structure) => _victoryLocations.AddChild(structure);

    // Player.PAS:1164
    public void UnregisterVictoryLocation(PlayerStructure structure) => _victoryLocations.RemoveChild(structure);

    // Player.PAS:1169
    public void CheckCaptureLocations()
    {
        if (StrategicMap.Current.VictoryCondition == StrategicMapVictoryCondition.CaptureLocations)
        {
            if (CaptureLocationsLeft() == 0)
            {
                StrategicMap.Current.GameOver(PlayerIndex, status: GameOverStatus.Won);
            }
        }
    }

    // Player.PAS:1175
    public int CaptureLocationsLeft()
    {
        int result = 0;
        for (int i = 0, last = _victoryLocations.Count - 1; i <= last; i++)
        {
            if (_victoryLocations.GetStructure(i).Player != PlayerIndex)
            {
                bool condition = _victoryLocations.GetStructure(i).Player > 0;
                if (condition)
                {
                    PlayerStructure structure = _victoryLocations.GetStructure(i);
                    condition = StrategicMap.Current.Players.GetDiplomaticRelation(structure.Player, otherPlayer: PlayerIndex) == DiplomaticRelation.Alliance;
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
            result = new DiplomaticAction(null)
            {
                Holder = CreationHolder(),
                Source = PlayerIndex,
                Target = target,
                OtherPlayer = other,
                Proposal = proposal
            };
            result.RetrieveDefaultMessage(PlayerIndex, target, other, reaction: DiplomaticActionReaction.None);
        }
        else
        {
            return null;
        }

        return result;
    }

    // Player.PAS:1203
    public bool DiplomaticActionAlreadySend(byte kind, sbyte target, sbyte other)
    {
        EventLogbook logbook = StrategicMap.Current.Players.GetPlayers(target).EventLogbook;
        bool result = false;
        int i = logbook.GetCount();
        while (!result && i != 0)
        {
            i -= 1;
            if (logbook[i].Day == StrategicMap.Current.Day)
            {
                if (logbook[i] is DiplomaticActionEventLog diplomatic)
                {
                    DiplomaticAction item = diplomatic.Action;
                    result = item.Proposal.ToFields().Kind == kind && item.Source == PlayerIndex && item.Target == target && (other == -1 || item.OtherPlayer == other);
                }
            }
        }

        return result;
    }

    // Player.PAS:1226
    public int ListDiplomaticActions(sbyte player, DiplomaticActionList actions)
    {
        int count;
        if (player != PlayerIndex && GetFirstDaySeen(player) != -1)
        {
            count = actions.Count;
            DiplomaticRelation relation = StrategicMap.Current.Players.GetDiplomaticRelation(player, otherPlayer: PlayerIndex);
            DiplomaticAction action;
            if (PlayerType == PlayerType.Human && StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                action = CreateDiplomaticAction(new SendMessage(), target: player, other: -1);
                if (action is not null)
                {
                    _ = actions.AddChild(action);
                    _ = action.Release();
                }
            }

            if (relation is DiplomaticRelation.War or DiplomaticRelation.Unknown && !DiplomaticActionAlreadySend(OfferPeace.Kind, target: player, other: -1))
            {
                action = CreateDiplomaticAction(new OfferPeace(), target: player, other: -1);
                if (action is not null)
                {
                    _ = actions.AddChild(action);
                    _ = action.Release();
                }
            }

            if (relation is not DiplomaticRelation.War and not DiplomaticRelation.Alliance)
            {
                action = CreateDiplomaticAction(new DeclareWar(), target: player, other: -1);
                if (action is not null)
                {
                    _ = actions.AddChild(action);
                    _ = action.Release();
                }
            }

            if (relation == DiplomaticRelation.Peace && !DiplomaticActionAlreadySend(ProposeAlliance.Kind, target: player, other: -1))
            {
                action = CreateDiplomaticAction(new ProposeAlliance(), target: player, other: -1);
                if (action is not null)
                {
                    _ = actions.AddChild(action);
                    _ = action.Release();
                }
            }

            if (relation is DiplomaticRelation.Peace or DiplomaticRelation.Unknown && !DiplomaticActionAlreadySend(ThreatenToAttack.Kind, target: player, other: -1))
            {
                action = CreateDiplomaticAction(new ThreatenToAttack(), target: player, other: -1);
                if (action is not null)
                {
                    _ = actions.AddChild(action);
                    _ = action.Release();
                }
            }

            action = CreateDiplomaticAction(new MakeTribute(), target: player, other: -1);
            if (action is not null)
            {
                _ = actions.AddChild(action);
                _ = action.Release();
            }

            if (relation == DiplomaticRelation.Alliance)
            {
                action = CreateDiplomaticAction(new BreakAlliance(), target: player, other: -1);
                if (action is not null)
                {
                    _ = actions.AddChild(action);
                    _ = action.Release();
                }
            }

            for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
            {
                if (i != player && i != PlayerIndex && StrategicMap.Current.Players.GetPlayers(i).Status == PlayerStatus.Playing)
                {
                    bool condition = !DiplomaticActionAlreadySend(SuggestWarDeclaration.Kind, target: player, other: (sbyte)i) && StrategicMap.Current.Players.GetPlayers(PlayerIndex).GetFirstDaySeen((sbyte)i) != -1 && StrategicMap.Current.Players.GetPlayers(player).GetFirstDaySeen((sbyte)i) != -1;
                    if (condition)
                    {
                        DiplomaticRelation diplomaticRelation = StrategicMap.Current.Players.GetDiplomaticRelation(player, otherPlayer: (sbyte)i);
                        condition = diplomaticRelation is DiplomaticRelation.Peace or DiplomaticRelation.Unknown;
                    }

                    if (condition)
                    {
                        action = CreateDiplomaticAction(new SuggestWarDeclaration(), target: player, other: (sbyte)i);
                        if (action is not null)
                        {
                            _ = actions.AddChild(action);
                            _ = action.Release();
                        }
                    }
                }
            }

            for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
            {
                if (i != player && i != PlayerIndex && StrategicMap.Current.Players.GetPlayers(i).Status == PlayerStatus.Playing)
                {
                    if (!DiplomaticActionAlreadySend(SuggestAllianceBreak.Kind, target: player, other: (sbyte)i) && StrategicMap.Current.Players.GetPlayers(PlayerIndex).GetFirstDaySeen((sbyte)i) != -1 && StrategicMap.Current.Players.GetPlayers(player).GetFirstDaySeen((sbyte)i) != -1 && StrategicMap.Current.Players.GetDiplomaticRelation(player, otherPlayer: (sbyte)i) == DiplomaticRelation.Alliance)
                    {
                        action = CreateDiplomaticAction(new SuggestAllianceBreak(), target: player, other: (sbyte)i);
                        if (action is not null)
                        {
                            _ = actions.AddChild(action);
                            _ = action.Release();
                        }
                    }
                }
            }

            if (PlayerRace == 5 && HeroEmerging && PlayerType == PlayerType.Human && StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
                {
                    if (i != player && i != PlayerIndex && StrategicMap.Current.Players.GetPlayers(i).Status == PlayerStatus.Playing && StrategicMap.Current.Players.GetPlayers(i).PlayerType == PlayerType.Human)
                    {
                        if (StrategicMap.Current.Players.GetPlayers(PlayerIndex).GetFirstDaySeen((sbyte)i) != -1 && StrategicMap.Current.Players.GetPlayers(player).GetFirstDaySeen((sbyte)i) != -1 && StrategicMap.Current.Players.GetPlayers(i).GetFirstDaySeen(player) != -1)
                        {
                            action = CreateDiplomaticAction(new ImpersonatedMessage(), target: player, other: (sbyte)i);
                            if (action is not null)
                            {
                                _ = actions.AddChild(action);
                                _ = action.Release();
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
        Assert(PlayerIndex != 0, message: "", file: "Player.PAS", line: 1390);
        if (PlayerType is (>= PlayerType.Cpu1 and <= PlayerType.Cpu3) or PlayerType.Cpu4 or PlayerType.Cpu5)
        {
            return AIValidAttackPlayers[player];
        }
        else
        {
            DiplomaticRelation diplomaticRelation = AbilityRules.DiplomaticRelation(PlayerIndex, independentRelation: 0, otherPlayer: player, otherIndependentRelation: independentRelation);
            return diplomaticRelation is DiplomaticRelation.War or DiplomaticRelation.Unknown;
        }
    }

    // Player.PAS:1398
    public bool ChangePlayerSettings(string settings, string error)
    {
        if (!GetBusy())
        {
            ChangePlayerSettingsTokenEvent eventValue = new(null)
            {
                Holder = CreationHolder(),
                Player = PlayerIndex,
                Settings = settings
            };
            StrategicMap.Current.TokenManager.GetTokenControl(PlayerIndex).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
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
    public sealed record ThreatenToAttack : DiplomaticProposal
    {
        public const byte Kind = 4;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Threat;

        public override int Icon => 46;

        public override string Name(DiplomaticAction action) => AbilityRules.TranslateResourceString(action.GetEngine(), S: ThreatenToAttackText);

        public override string LogText(DiplomaticAction action) => AbilityRules.TranslateResourceString(action.GetEngine(), S: ThreatText);

        public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation)
            => text.Proposing(112);

        public override void Sent(DiplomaticAction action) => Deliver(action);

        internal static string ThreatenToAttackText { get; } = "Threaten to attack";
        internal static string ThreatText { get; } = "Threat";
    }

    // Kind 5: the source pays the target a tribute.
    public sealed record MakeTribute(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 5;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 48;

        public override string Name(DiplomaticAction action) => AbilityRules.TranslateResourceString(action.GetEngine(), S: MakeTributeText);

        public override string LogText(DiplomaticAction action)
        {
            string subject = AbilityRules.TranslateResourceString(action.GetEngine(), S: TributeText);
            return Outcome(action, proposed: subject, subject, accepted: AcceptedLine(subject, engine: action.GetEngine()), otherwise: "");
        }

        public override string LoggedSample => "SFX\\EVENTS\\MESSAGE2.WAV";

        public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation)
            => text.Exchange(reaction, proposed: 118, accepted: 133, rejected: 136);

        public override bool CanSend(DiplomaticAction action, ref string errorText)
        {
            if (Gold == 0 && Mana == 0)
            {
                errorText = AbilityRules.TranslateResourceString(action.GetEngine(), S: NoGoldManaSpecifiedText);
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
            _ = relation is DiplomaticRelation.Peace or DiplomaticRelation.Alliance
                ? action.Accept(errorText)
                : StrategicMap.Current.Random(200) + 50 < Gold + Mana ? action.Accept(errorText) : action.Reject(errorText);
        }

        internal static string MakeTributeText { get; } = "Make tribute";
        internal static string NoGoldManaSpecifiedText { get; } = "No gold/mana specified";
        internal static string TributeText { get; } = "Tribute";
    }

    // Kind 7: the source writes to the target in the name of the other player, offered between human
    // players to a source of race 5 whose hero emerges (Player.PAS:1226).
    public sealed record ImpersonatedMessage : DiplomaticProposal
    {
        public const byte Kind = 7;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Message;

        public override bool SignedAsOtherPlayer => true;

        public override string Name(DiplomaticAction action)
        {
            string name = StrategicMap.Current.Players.GetPlayers(action.OtherPlayer).GetName();
            return AbilityRules.Format(AbilityRules.TranslateResourceString(action.GetEngine(), S: s_sendMessageFormat), arguments: [name]);
        }

        public override string LogText(DiplomaticAction action) => AbilityRules.TranslateResourceString(action.GetEngine(), S: GameText.DiplomaticMessageText);

        public override void Sent(DiplomaticAction action) => Deliver(action);

        private static readonly string s_sendMessageFormat = "Send message as %s";
    }

    // Kind 8: the source asks the target to end its alliance with the other player.
    public sealed record SuggestAllianceBreak(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 8;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 47;

        public override string Name(DiplomaticAction action)
        {
            string name = StrategicMap.Current.Players.GetPlayers(action.OtherPlayer).GetName();
            return AbilityRules.Format(AbilityRules.TranslateResourceString(action.GetEngine(), S: s_suggestAllianceBreakFormat), arguments: [name]);
        }

        public override string LogText(DiplomaticAction action)
        {
            string subject = AbilityRules.TranslateResourceString(action.GetEngine(), S: GameText.BreakAllianceText);
            return Outcome(action, proposed: AbilityRules.TranslateResourceString(action.GetEngine(), S: BreakAllianceSuggestionText), subject, accepted: AcceptedLine(subject, engine: action.GetEngine()), otherwise: "");
        }

        public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation)
            => text.Exchange(reaction, proposed: 160, accepted: 139, rejected: 142, aboutOther: true);

        public override void Sent(DiplomaticAction action) => Deliver(action);

        public override bool LapsesUnanswered(DiplomaticAction action)
            => Relation(action.Target, second: action.OtherPlayer) != DiplomaticRelation.Alliance;

        public override void Accepted(DiplomaticAction action)
        {
            if (Relation(action.Target, second: action.OtherPlayer) == DiplomaticRelation.Alliance)
            {
                StrategicMap.Current.Players.SetDiplomaticRelation((sbyte)(byte)action.Target, otherPlayer: (sbyte)(byte)action.OtherPlayer, relation: DiplomaticRelation.Peace);
                DiplomaticAction broken = StrategicMap.Current.Players.GetPlayers(action.Target).CreateDiplomaticAction(new BreakAlliance(), target: action.OtherPlayer, other: -1);
                if (broken is not null)
                {
                    Broadcast(broken, exclude: action.Source, viewer: action.OtherPlayer);
                    _ = broken.Release();
                }
            }

            ReturnToSource(action);
        }

        public override DiplomaticActionReaction RejectReaction(DiplomaticAction action)
            => Relation(action.Target, second: action.OtherPlayer) == DiplomaticRelation.Alliance ? DiplomaticActionReaction.Reject : DiplomaticActionReaction.Ignore;

        public override void AnswerAI(DiplomaticAction action, sbyte player, DiplomaticRelation relation, ByteList idealPeace, ByteList idealWar)
        {
            string errorText = "";
            _ = Relation(action.Target, second: action.OtherPlayer) != DiplomaticRelation.Alliance
                ? action.Accept(errorText)
                : action.Reject(errorText);
        }

        public override int RelationModifier(DiplomaticAction action, sbyte player, sbyte race)
            => player == action.Target && Relation(action.Target, second: action.OtherPlayer) == DiplomaticRelation.Alliance
                ? RaceModifier(action.OtherPlayer, race, modifier: -40)
                : 0;

        private static readonly string s_suggestAllianceBreakFormat = "Suggest alliance break with %s";

        internal static string BreakAllianceSuggestionText { get; } = "Break alliance suggestion";
    }

    // Kind 9: the source asks the target to declare war on the other player.
    public sealed record SuggestWarDeclaration(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 9;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 47;

        public override string Name(DiplomaticAction action)
        {
            string name = StrategicMap.Current.Players.GetPlayers(action.OtherPlayer).GetName();
            return AbilityRules.Format(AbilityRules.TranslateResourceString(action.GetEngine(), S: s_suggestWarFormat), arguments: [name]);
        }

        public override string LogText(DiplomaticAction action)
        {
            string subject = AbilityRules.TranslateResourceString(action.GetEngine(), S: GameText.DeclareWarText);
            return Outcome(action, proposed: AbilityRules.TranslateResourceString(action.GetEngine(), S: DeclareWarSuggestionText), subject, accepted: AcceptedLine(subject, engine: action.GetEngine()), otherwise: "");
        }

        public override int MiddleIcon(DiplomaticAction action)
            => action.Reaction == DiplomaticActionReaction.Accept ? 0x15 : 0x16;

        public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation)
            => text.Exchange(reaction, proposed: 157, accepted: 145, rejected: 148, aboutOther: true);

        public override void Sent(DiplomaticAction action) => Deliver(action);

        public override bool LapsesUnanswered(DiplomaticAction action)
            => Relation(action.Target, second: action.OtherPlayer) != DiplomaticRelation.War;

        public override void Accepted(DiplomaticAction action)
        {
            if (Relation(action.Target, second: action.OtherPlayer) != DiplomaticRelation.War)
            {
                StrategicMap.Current.Players.GetPlayers(action.OtherPlayer).PlayerSeen((sbyte)(byte)action.Target);
                StrategicMap.Current.Players.SetDiplomaticRelation((sbyte)(byte)action.Target, otherPlayer: (sbyte)(byte)action.OtherPlayer, relation: DiplomaticRelation.War);
                DiplomaticAction war = StrategicMap.Current.Players.GetPlayers(action.Target).CreateDiplomaticAction(new DeclareWar(), target: action.OtherPlayer, other: -1);
                if (war is not null)
                {
                    Broadcast(war, exclude: action.Source, viewer: action.OtherPlayer);
                    _ = war.Release();
                }
            }

            ReturnToSource(action);
        }

        public override DiplomaticActionReaction RejectReaction(DiplomaticAction action)
            => Relation(action.Target, second: action.OtherPlayer) != DiplomaticRelation.War ? DiplomaticActionReaction.Reject : DiplomaticActionReaction.Ignore;

        public override void AnswerAI(DiplomaticAction action, sbyte player, DiplomaticRelation relation, ByteList idealPeace, ByteList idealWar)
        {
            string errorText = "";
            _ = Relation(action.Target, second: action.OtherPlayer) == DiplomaticRelation.War
                ? action.Accept(errorText)
                : idealWar.IndexOf((byte)action.OtherPlayer) != -1
                    ? StrategicMap.Current.Players.GetPlayers(action.Target).AIControl.CanDeclareWar((byte)action.OtherPlayer)
                        ? StrategicMap.Current.Random(5) == 1 && StrategicMap.Current.Random(550) + 250 < action.EscrowGold + action.EscrowMana
                            ? action.Accept(errorText)
                            : action.Reject(errorText)
                        : action.Reject(errorText)
                    : action.Reject(errorText);
        }

        public override int RelationModifier(DiplomaticAction action, sbyte player, sbyte race)
        {
            return player == action.Target && Relation(action.Target, second: action.OtherPlayer) != DiplomaticRelation.War
? RaceModifier(modifier: Relation(action.Target, second: action.OtherPlayer) == DiplomaticRelation.Alliance
                    ? -60
                    : -20, other: action.OtherPlayer, race: race)
                : 0;
        }

        private static readonly string s_suggestWarFormat = "Suggest war with %s";

        internal static string DeclareWarSuggestionText { get; } = "Declare war suggestion";
    }
}

public enum PlayerBehavior : byte
{
    Normal = 0, Aggressor = 1, Defender = 2, Expander = 3, Scorcher = 4, Random = 5
}

public enum PlayerStatus : byte
{
    Idle = 0, Playing = 1, Terminated = 2
}
