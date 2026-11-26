#nullable disable warnings
// Translated from AoWE.pas.
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Aow.Engine;
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.Units;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.World.Map;

// map name, heroes, game mode, stream mode, fixed leaders, allied victory, day limit, turn
// order, day/turn events, event-log locks, diplomacy events, initial seed, random seed, debug
// save, communicator, token log Text, logging enabled
public sealed partial class StrategicMap : GameMap
{

    public new const int ClassId = 131_089;

    public SelectedArmy SelectedArmy { get; }
    public bool IgnoreStreamErrors { get; private set; }
    public TargetSelectionControl TargetSelection { get; }

    public string MapName { get; set; } = "";

    private string _author = "";

    public int SphereCount { get; private set; }

    public DelphiStringList Description { get; }
    public ItemControl? ItemControl { get; private set; }
    public HeroControl? HeroControl { get; set; }
    public UnitControl? UnitControl { get; set; }
    public StructureControl? StructureControl { get; private set; }

    private StrategicMapIndependentAIMode _independentAIMode;
    private readonly DelphiStringList _objectives;

    public int HeroCountMax { get; private set; }

    public StartupSongList StartupSongs { get; }
    public SongList GameSongs { get; }

    public StrategicMapGameStatus GameStatus { get; private set; }
    public StrategicMapGameResult GameResult { get; set; }
    public GameType GameType { get; private set; }

    public CombatMap? CombatMap { get; set; }
    public Battle? Combat { get; private set; }
    public TutorialControl TutorialControl { get; }
    public AIExecutor AIExecutor { get; internal set; }
    public AIPlayerActionManager AIPlayerActionManager { get; }
    public AIGroupManager? AIGroupManager { get; private set; }
    public HexSpriteMapKeyboard Keyboard { get; }

    public bool UnlimitedMoves { get; set; }

    public bool RequestCombatType { get; set; }

    public PlayerConnectionType ConnectionType { get; set; }

    public TextControl TextControl { get; }
    public PlayerCollection Players { get; internal set; }
    public RaceList Races { get; internal set; }

    public bool FixedLeaders { get; set; }
    public bool LeadersFixed { get; set; }
    public bool StartingGame { get; set; }
    public bool AlliedVictory { get; set; }
    public int VictoryMode { get; set; }
    public StrategicMapVictoryCondition VictoryCondition { get; private set; }
    public int VictoryMana { get; private set; }
    public int VictoryGold { get; private set; }
    public int DayLimit { get; set; }
    public int TurnTime { get; set; }

    private int _turnTimeElapsed;
    private int _turnTimeBeep;
    private int _turnTimerStart;

    public StrategicMapTurnOrderMode TurnOrderMode { get; private set; }

    public ByteList TurnOrder { get; }

    public int Day { get; set; }

    private string _sessionName = "";

    public event NotifyEvent<Leader>? LeaderChanged;

    internal void OnLeaderChanged(Leader leader)
    {
        NotifyEvent<Leader>? handlers = LeaderChanged;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(LeaderChanged));
        handlers?.Invoke(leader);
    }

    public bool ShowIndependentMoves { get; set; }
    public MoveControl MoveControl { get; }
    public GlobalMagicControl? GlobalMagicControl { get; set; }
    public MapCampaignSettings CampaignSettings { get; }
    public PlayerId? EditorPlayer { get; } = PlayerId.Independent;

    public bool ShowAllPlayers { get; private set; }

    private int _relationsChangedLockCount;

    public event NotifyEvent<StrategicMap>? PlayerChanged;

    public event NotifyEvent<PlayerControl>? DayStarted;

    internal void OnDayStarted(PlayerControl playerControl)
    {
        NotifyEvent<PlayerControl>? handlers = DayStarted;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(DayStarted));
        handlers?.Invoke(playerControl);
    }

    public event NotifyEvent<sbyte>? TurnStarted;

    internal void OnTurnStarted(sbyte value)
    {
        NotifyEvent<sbyte>? handlers = TurnStarted;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(TurnStarted));
        handlers?.Invoke(value);
    }

    public event NotifyEvent<sbyte>? TurnEnded;

    public event NotifyEvent<StrategicMap>? CurrentPlayerChanged;

    public event NotifyEvent<StrategicMap>? SeatedPlayerChanged;

    public event NotifyEvent<Battle>? CombatTriggered;

    public event NotifyEvent<sbyte>? PlayerTerminated;

    public event NotifyEvent<StrategicMap>? GameEnded;

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event GameTerminated keeps this backing field until the port raises it.")]
    private NotifyEvent<StrategicMap>? _gameTerminated;

    public event NotifyEvent<StrategicMap>? GameTerminated { add => _gameTerminated += value; remove => _gameTerminated -= value; }

    public bool ExitPending { get; private set; }

    public event NotifyEvent<StrategicMap>? ExitingGame;

    private int _gameOverLockCount;
    private readonly EventLogList _executeEventLogBuffer;

    public event NotifyEvent<ExecuteEventLogNotification>? ExecutingEventLog;

    private readonly ExecuteEventLogCallbackList _executeEventLogCallBacks;

    public int AILockCount { get; private set; }

    private int _executeEventLogDelay;

    public event NotifyEvent<AbilityActivation>? AbilityActivated;

    internal void OnAbilityActivated(AbilityActivation abilityActivation)
    {
        NotifyEvent<AbilityActivation>? handlers = AbilityActivated;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(AbilityActivated));
        handlers?.Invoke(abilityActivation);
    }

    public event NotifyEvent<EngineObject>? DiplomaticRelationsChanged;

    internal void OnDiplomaticRelationsChanged(EngineObject engineObject)
    {
        NotifyEvent<EngineObject>? handlers = DiplomaticRelationsChanged;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(DiplomaticRelationsChanged));
        handlers?.Invoke(engineObject);
    }

    public event NotifyEvent<StrategicMap>? VisibilityUpdated;

    public List<WorldEffectHexSprite> Effects { get; }

    private int _updateVisibilityLockCount;
    private bool _visibilityChanged;
    private int _armyUpdateCount;
    private int _structureUpdateCount;
    private int _unitUpdateCount;

    // Issue 326 keeps the ordered store: TargetSelectionControl.UnitChanged deactivates the target
    // selection when its source dies, and MapControl's relay refreshes the unit window before it.
    private readonly EventStore<Troop?> _unitChangedEventsStore;

    public event NotifyEvent<Troop> UnitChanged
    {
        add => _unitChangedEventsStore.Add(value);
        remove => _unitChangedEventsStore.Remove(value);
    }

    public event NotifyEvent<Army>? ArmyChanged;

    public event NotifyEvent<Structure>? StructureChanged;

    public event NotifyEvent<Troop>? UnitDestroyed;

    public event NotifyEvent<Army>? ArmyDestroyed;

    public event NotifyEvent<Structure>? StructureDestroyed;

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event TokenControlActivated keeps this backing field until the port raises it.")]
    private NotifyEvent<StrategicMap>? _tokenControlActivated;

    public event NotifyEvent<StrategicMap>? TokenControlActivated { add => _tokenControlActivated += value; remove => _tokenControlActivated -= value; }

    public event NotifyEvent<StrategicMap>? CombatMapCreated;

    internal void OnCombatMapCreated(StrategicMap strategicMap) => CombatMapCreated?.Invoke(strategicMap);

    public event NotifyEvent<StrategicMap>? CombatMapDestroyed;

    internal void OnCombatMapDestroyed(StrategicMap strategicMap) => CombatMapDestroyed?.Invoke(strategicMap);

    public int InitialRandomSeed { get; set; }
    public int DefendersSeed { get; set; }

    private int _synchronizeCount;

    public bool DebugSave { get; private set; }

    public PlayMode PlayMode { get; private set; }

    public TokenManager? TokenManager { get; set; }
    public PlayerCommunicator PlayerCommunicator { get; }
    public PlayerControl PlayerControl { get; private set; }
    public bool ShowUnitShadows { get; }
    public bool TokenEventLogging { get; private set; }

    // AoWHSMap.PAS:1336
    public override void Dispose()
    {
        StopTokenEventLogging();
        LockMapChangedEvent();
        SetViewLevelHandler(null);
        SetCenterViewHandler(null);
        GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "GENERAL.ILB");
        GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "CURSORS.ILB");
        GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "COMBAT.ILB");
        MoveControl?.Dispose();
        for (int i = Effects.Count - 1; i >= 0; i--)
        {
            WorldEffectHexSprite animation = Effects[i];
            _ = DynamicFixedVisibleHexSprite.RemoveChild(animation);
            WorldEffectHexSprite fEffectsItem = Effects[i];
            _ = DynamicVisibleHexSprite.RemoveChild(fEffectsItem);
            (Effects[i] as IDisposable)?.Dispose();
        }

        Effects?.Clear();
        TutorialControl?.Dispose();
        Players.BeginUpdate();
        ArmyChanged = null;
        StructureChanged = null;
        _unitChangedEventsStore.Clear();
        SeatedPlayerChanged = null;
        CurrentPlayerChanged = null;
        TurnStarted = null;
        DayStarted = null;
        LeaderChanged = null;
        Description.Clear();
        _objectives.Clear();
        TurnOrder.Clear();
        StructureDestroyed = null;
        UnitDestroyed = null;
        CombatMapCreated = null;
        CombatMapDestroyed = null;
        VisibilityUpdated = null;
        DiplomaticRelationsChanged = null;
        AbilityActivated = null;
        _tokenControlActivated = null;
        PlayerChanged = null;
        ExitingGame = null;
        Mouse.Moved -= MouseMoved;
        if (Combat is not null)
        {
            DestroyCombat();
        }

        if (CombatMap is not null)
        {
            CombatMap?.Dispose();
            CombatMap = null;
        }

        base.Dispose();
        ItemControl.Deactivate();
        GlobalMagicControl?.Dispose();
        GlobalMagicControl = null;
        PlayerCommunicator?.Dispose();
        TokenManager?.Dispose();
        TokenManager = null;
        CampaignSettings?.Dispose();
        AIExecutor?.Dispose();
        SelectedArmy?.Dispose();
        SeatedPlayerChanged = null;
        _executeEventLogBuffer?.Dispose();
        TurnStarted = null;
        TurnEnded = null;
        CurrentPlayerChanged = null;
        DayStarted = null;
        ArmyChanged = null;
        StructureChanged = null;
        _unitChangedEventsStore.Clear();
        ArmyDestroyed = null;
        StructureDestroyed = null;
        UnitDestroyed = null;
        LeaderChanged = null;
        PlayerControl?.Dispose();
        TextControl?.Dispose();
        CombatTriggered = null;
        PlayerChanged = null;
        ExecutingEventLog = null;
        AbilityActivated = null;
        GameSongs?.Dispose();
        StartupSongs?.Dispose();
        ExitingGame = null;
        ItemControl?.Dispose();
        ItemControl = null;
        HeroControl?.Dispose();
        HeroControl = null;
        UnitControl?.Dispose();
        UnitControl = null;
        StructureControl?.Dispose();
        StructureControl = null;
        AIGroupManager?.Dispose();
        AIGroupManager = null;
        AIPlayerActionManager?.Dispose();
        Players?.Dispose();
        Races?.Dispose();
        PlayerTerminated = null;
        GameEnded = null;
        _gameTerminated = null;
        TargetSelection?.Dispose();
        CombatMapCreated = null;
        CombatMapDestroyed = null;
        DiplomaticRelationsChanged = null;
        Keyboard?.Dispose();
        VisibilityUpdated = null;
        _tokenControlActivated = null;
        UnlockMapChangedEvent();
    }

    // AoWHSMap.PAS:2160. A load shows its progress to the load's handler where the original calls
    // Stream.ShowProgress. The editor's error handler of a load is the one its caller passes to the
    // tree load (persistence-tree.md 7). A preview leaves the hero control and the startup songs
    // unread, and a network map leaves the game songs unread, so a load skips those members as the
    // original does.
    public override void PersistFields(IPersister persister)
    {
        if (persister.Loading)
        {
            persister.ShowProgress(sender: this, message: AbilityRules.TranslateResourceString(GetEngine(), S: s_oneMomentPleaseText), percent: 0);
        }

        bool translating = persister.Translating();
        if (!persister.Loading && !translating)
        {
            ResetModified();
        }

        IgnoreStreamErrors = false;
        if (translating && Status.Contains(HexSpriteMapStatuses.Editting))
        {
            // The original sets the editor's handler on every stream it runs on. A tree load takes the
            // handler its caller passes, and the translate stream still takes it here.
            persister.SetTranslateErrorHandler(handler: OnStreamError);
        }

        ConnectionType = persister.Enum("ConnectionType", value: ConnectionType);
        int x;
        int y;
        int level;
        if (ConnectionType == PlayerConnectionType.Network)
        {
            (x, y, level) = (ViewPixelX, ViewPixelY, ViewMapLevel);
            (ViewPixelX, ViewPixelY, ViewMapLevel) = (0, 0, 0);
        }
        else
        {
            (x, y, level) = (0, 0, 0);
        }

        base.PersistFields(persister);
        if (ConnectionType == PlayerConnectionType.Network)
        {
            (ViewPixelX, ViewPixelY, ViewMapLevel) = (x, y, level);
        }

        _author = persister.String("Author", value: _author);
        persister.Lines(name: "Description", lines: Description);
        persister.Lines(name: "Objectives", lines: _objectives);
        _independentAIMode = persister.Enum("IndependentAIMode", value: _independentAIMode);
        CurrentPlayer = persister.OptionalPlayer(name: "CurrentPlayer", value: CurrentPlayer, width: IdWidth.UnsignedByte, fallback: 0);
        TurnOrderMode = persister.Enum("TurnOrderMode", value: TurnOrderMode);
        persister.ByteList(name: "TurnOrder", list: TurnOrder);
        Day = persister.Int32("Day", value: Day);
        _sessionName = persister.String("SessionName", value: _sessionName);
        persister.Object("Players", objectValue: Players);
        persister.Object("Races", objectValue: Races);
        ExplorationEnabled = persister.Boolean("ExplorationEnabled", value: ExplorationEnabled);
        if (!persister.Preview)
        {
            persister.Object("HeroControl", objectValue: HeroControl);
        }
        else
        {
            persister.Skip("HeroControl");
        }

        MapName = persister.String("MapName", value: MapName);
        FogEnabled = persister.Boolean("FogEnabled", value: FogEnabled);
        HeroCountMax = persister.UnsignedByte(name: "HeroCountMax", value: HeroCountMax.TruncateToSByte());
        DefendersSeed = persister.Int32("DefendersSeed", value: DefendersSeed);
        // The original leaves the songs out under text only, which the translate stream's mode holds.
        if (!persister.Preview && !translating)
        {
            persister.Object("StartupSongs", objectValue: StartupSongs);
        }
        else
        {
            persister.Skip("StartupSongs");
        }

        persister.Object("ItemControl", objectValue: ItemControl);
        GameStatus = persister.Enum("GameStatus", value: GameStatus);
        GameResult = persister.Enum("GameResult", value: GameResult);
        InitialRandomSeed = persister.Int32("InitialRandomSeed", value: InitialRandomSeed);
        GameType = persister.Enum("GameType", value: GameType);
        persister.Object("UnitControl", objectValue: UnitControl);
        PlayMode = persister.Enum("PlayMode", value: PlayMode);
        persister.Object("GlobalMagicControl", objectValue: GlobalMagicControl);
        SphereCount = persister.Int32("SphereCount", value: SphereCount, fallback: 5);
        persister.Object("TextControl", objectValue: TextControl);
        persister.Object("CampaignSettings", objectValue: CampaignSettings);
        // The original stores the hero control twice.
        persister.Object("HeroControlCopy", objectValue: HeroControl);
        VictoryCondition = persister.Enum("VictoryCondition", value: VictoryCondition);
        VictoryMana = persister.Int32("VictoryMana", value: VictoryMana);
        VictoryGold = persister.Int32("VictoryGold", value: VictoryGold);
        FixedLeaders = persister.Boolean("FixedLeaders", value: FixedLeaders);
        AlliedVictory = persister.Boolean("AlliedVictory", value: AlliedVictory);
        persister.Object("AIGroupManager", objectValue: AIGroupManager);
        DayLimit = persister.Int32("DayLimit", value: DayLimit);
        TurnTime = persister.Int32("TurnTime", value: TurnTime);
        UnlimitedMoves = persister.Boolean("UnlimitedMoves", value: UnlimitedMoves);
        persister.Object("StructureControl", objectValue: StructureControl);
        if (persister.Loading || ConnectionType != PlayerConnectionType.Network)
        {
            _turnTimeElapsed = persister.Int32("TurnTimeElapsed", value: _turnTimeElapsed);
        }

        persister.Object("TutorialControl", objectValue: TutorialControl);
        if (persister.Loading || ConnectionType != PlayerConnectionType.Network)
        {
            _turnTimeBeep = persister.Int32("TurnTimeBeep", value: _turnTimeBeep);
        }

        LeadersFixed = persister.Boolean("LeadersFixed", value: LeadersFixed);
        if (!persister.Preview && !translating && ConnectionType != PlayerConnectionType.Network)
        {
            persister.Object("GameSongs", objectValue: GameSongs);
        }
        else
        {
            persister.Skip("GameSongs");
        }

        StartingGame = persister.Boolean("StartingGame", value: StartingGame);
        ShowAllPlayers = persister.Boolean("ShowAllPlayers", value: ShowAllPlayers);
        VictoryMode = persister.RawByte("VictoryMode", value: VictoryMode.TruncateToByte());
        RandSeed = DefendersSeed;
    }

    // AoWHSMap.PAS:2241
    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (persister.Loading && !persister.Preview && Day == 0)
        {
            string fileName = GameType == GameType.Campaign
                ? $"{GetEngine().StartupDirectory}Dict\\CampMaps.mld"
                : $"{GetEngine().StartupDirectory}Dict\\CustMaps.mld";

            if (RtlHost.Files.FileExists(fileName))
            {
                GetEngine().TranslateEngineObjectFromFile(this, fileName);
            }
        }
    }

    // AoWHSMap.PAS:1238
    public StrategicMap(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        ItemControl = new ItemControl(null) { Holder = CreationHolder() };
        TokenManager = new TokenManager(GetEngine(), message: OnTokenManagerMessage);
        TokenManager.TokenExecutor.TacticalBattleActivating += TokenControlActivatedEvent;
        TokenManager.TokenExecutor.SetExecuteTokenEventHandler(OnExecuteTokenEvent);
        PlayerCommunicator = new PlayerCommunicator(null)
        {
            Holder = CreationHolder(),
            Engine = GetEngine()
        };
        ConnectionType = PlayerConnectionType.Local;
        AIExecutor = new AIExecutor(null) { Holder = CreationHolder() };
        AIGroupManager = new AIGroupManager(null) { Holder = CreationHolder() };
        AIPlayerActionManager = new AIPlayerActionManager(null) { Holder = CreationHolder() };
        AIPlayerActionManager.RegisterClasses([AIPlayerActionClass.Of<EndTurnAIPlayerAction>(), AIPlayerActionClass.Of<GroupControlAIPlayerAction>(), AIPlayerActionClass.Of<GroupControlLoopAIPlayerAction>(), AIPlayerActionClass.Of<InitializeAIPlayerAction>(), AIPlayerActionClass.Of<BudgetManagerAIPlayerAction>(), AIPlayerActionClass.Of<AutoProduceAIPlayerAction>(), AIPlayerActionClass.Of<DiplomacyAIPlayerAction>(), AIPlayerActionClass.Of<ProcessEventLogAIPlayerAction>(), AIPlayerActionClass.Of<ResearchSpellAIPlayerAction>(), AIPlayerActionClass.Of<CastSpellAIPlayerAction>(), AIPlayerActionClass.Of<StructurePhase1AIPlayerAction>(), AIPlayerActionClass.Of<StructurePhase2AIPlayerAction>()]);
        TokenManager.TokenExecutor.EventsExecuted += AIExecutor.TokenControlDoneEvent;
        AIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<HuntAIGroup>()]);
        PlayMode = PlayMode.None;
        Keyboard = new HexSpriteMapKeyboard(null) { Holder = CreationHolder() };
        TargetSelection = new TargetSelectionControl();
        Effects = [];
        DefendersSeed = 0;
        HeroCountMax = 5;
        SelectedArmy = new SelectedArmy(this);
        Mouse.RectangleSelected += SelectedArmy.RectangleSelectEvent;
        MoveControl = new MoveControl(this);
        GlobalMagicControl = new GlobalMagicControl(null) { Holder = CreationHolder() };
        MapName = "noname";
        _author = "";
        _turnTimerStart = -1;
        SphereCount = 5;
        VictoryGold = 5_000;
        VictoryMana = 5_000;
        Description = [];
        _objectives = [];
        _independentAIMode = StrategicMapIndependentAIMode.Normal;
        Players = new PlayerCollection(null) { Holder = CreationHolder() };
        Races = new RaceList(null) { Holder = CreationHolder() };
        FixedLeaders = true;
        AlliedVictory = false;
        CurrentPlayer = PlayerId.Independent;
        _executeEventLogBuffer = new EventLogList(null) { Holder = CreationHolder() };
        DebugSave = false;
        TutorialControl = new TutorialControl(null) { Holder = CreationHolder() };
        TurnOrderMode = StrategicMapTurnOrderMode.Fixed;
        TurnOrder = [];
        Day = 0;
        GameStatus = StrategicMapGameStatus.Idle;
        GameResult = StrategicMapGameResult.None;
        _sessionName = "";
        DetailLevel detailLevel = GetEngine().Registry.GetDetailLevel();
        ShowUnitShadows = detailLevel is DetailLevel.Normal or DetailLevel.High;
        HeroControl = new HeroControl(null) { Holder = CreationHolder() };
        UnitControl = new UnitControl(null) { Holder = CreationHolder() };
        StructureControl = new StructureControl(null) { Holder = CreationHolder() };
        TextControl = new TextControl(null) { Holder = CreationHolder() };
        _unitChangedEventsStore = new();
        UnitChanged += TargetSelection.UnitChanged;
        ArmyDestroyed += SelectedArmy.OnDestroyArmy;
        _executeEventLogCallBacks = new ExecuteEventLogCallbackList();
        StartupSongs = new StartupSongList(null) { Holder = CreationHolder() };
        GameSongs = new SongList(null) { Holder = CreationHolder() };
        CampaignSettings = new MapCampaignSettings(null) { Holder = CreationHolder() };
        Mouse.Moved += MouseMoved;
    }

    // AoWHSMap.PAS:2826
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        if (message.MessageId == 536870920)
        {
            SynchronizeBegin();
            GlobalMagicControl.BeginUpdate();
            Players.BeginUpdate();
            BeginUnitUpdate();
            LockUpdateVisibility();
            ItemControl.Activate();
            StructureControl.Activate();
            if (Day == 0)
            {
                ResetMapLevels();
            }
        }

        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            ActivateMessage();
            EndUnitUpdate();
            Players.EndUpdate();
            GlobalMagicControl.EndUpdate();
            UnLockUpdateVisibility();
            SynchronizeEnd();
        }
        else
        {
            if (message.MessageId == 537001989)
            {
                NotifyEvent<StrategicMap>? handlers = VisibilityUpdated;
                EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(VisibilityUpdated));
                handlers?.Invoke(this);
            }
        }
    }

    // AoWHSMap.PAS:2853
    public override void SendMessage(ref EngineMessage message, EngineObject? sender)
    {
        base.SendMessage(ref message, sender);
        GlobalMagicControl?.MessageProc(ref message, sender);

        Players.MessageProc(ref message, sender);
        Races.MessageProc(ref message, sender);
        ItemControl.MessageProc(ref message, sender);
        HeroControl.MessageProc(ref message, sender);
        AIGroupManager.MessageProc(ref message, sender);
        if (message.MessageId == 537001986 && message.Payload.Int == PlayerIdExtensions.ToOriginal(Current.SeatedPlayer))
        {
            if (SelectedArmy.ArmyHexSprite is not null)
            {
                SelectedArmy.Update();
            }
        }
    }

    // AoWHSMap.PAS:1549
    public override GameMapMouse CreateMouse() => new AowHexSpriteMapMouse();

    // AoWHSMap.PAS:2116
    public override void NotifyProgress(in MapProgress progress)
    {
        if (HasProgressHandlers)
        {
            string defaultText = AbilityRules.TranslateResourceString(GetEngine(), S: s_oneMomentPleaseText);
            string text;
            int percent;
            int total;
            switch (progress.ProgressType)
            {
                case MapProgressType.Loading:
                    total = progress.Percent + (progress.Level * 100);
                    percent = Round((double)((double)total / 3) / MapContainer.LevelCount);
                    text = GetEngine().Debug
                        ? AbilityRules.TranslateResourceString(GetEngine(), S: s_loadingLevelText) + progress.Level.ToString(CultureInfo.InvariantCulture)
                        : defaultText;

                    RaiseProgress(this, message: text, progression: percent);
                    break;
                case MapProgressType.Initializing:
                    total = progress.Percent + (progress.Level * 100);
                    percent = Round((double)((double)(total / 4.5) / MapContainer.LevelCount) + ((double)100 / 3));
                    text = GetEngine().Debug
                        ? AbilityRules.TranslateResourceString(GetEngine(), S: s_initializingLevelText) + progress.Level.ToString(CultureInfo.InvariantCulture)
                        : defaultText;

                    RaiseProgress(this, message: text, progression: percent);
                    break;
                case MapProgressType.UpdatingTerrain:
                    total = progress.Percent + (progress.Level * 100);
                    percent = Round((double)((double)(total / 4.5) / MapContainer.LevelCount) + ((double)500 / 9));
                    text = GetEngine().Debug
                        ? AbilityRules.TranslateResourceString(GetEngine(), S: s_updatingTerrainLevelText) + progress.Level.ToString(CultureInfo.InvariantCulture)
                        : defaultText;

                    RaiseProgress(this, message: text, progression: percent);
                    break;
                case MapProgressType.Activating:
                    total = progress.Percent + (progress.Level * 100);
                    percent = Round((double)((double)(total / 4.5) / MapContainer.LevelCount) + ((double)700 / 9));
                    text = GetEngine().Debug
                        ? AbilityRules.TranslateResourceString(GetEngine(), S: s_activatingLevelText) + progress.Level.ToString(CultureInfo.InvariantCulture)
                        : defaultText;

                    RaiseProgress(this, message: text, progression: percent);
                    break;
                case MapProgressType.Saving:
                    string savingLevel = AbilityRules.TranslateResourceString(GetEngine(), S: s_savingLevelText);
                    RaiseProgress(this, message: savingLevel + progress.Level.ToString(CultureInfo.InvariantCulture), progression: progress.Percent);
                    break;
                case MapProgressType.Destroying:
                    string translatedText = AbilityRules.TranslateResourceString(GetEngine(), S: s_destroyingLevelText);
                    RaiseProgress(this, message: translatedText + progress.Level.ToString(CultureInfo.InvariantCulture), progression: progress.Percent);
                    break;
                default:
                    break;
            }
        }
    }

    // AoWHSMap.PAS:2290
    public override void InitializeNewMap()
    {
        Races.Update();
        Players.SetNumberOfPlayers(2);
        Players.ValidateRaces(Races);
        for (int l = 0, last = MapContainer.LevelCount - 1; l <= last; l++)
        {
            InitializeMapLevel(l.TruncateToSByte());
        }

        for (int i = 0, last = GamePack.Current.ItemLibraries.SystemLibrary.Items.Count - 1; i <= last; i++)
        {
            Item copiedItem = GamePack.Current.ItemLibraries.SystemLibrary.Items.GetItem(i).Copy<Item>(GetEngine());
            ItemControl.RegisterItem(copiedItem);
            _ = copiedItem.Release();
        }

        for (int i = 0, last = GamePack.Current.HeroLibraries.SystemLibrary.Heroes.Count - 1; i <= last; i++)
        {
            Hero heroValue = GamePack.Current.HeroLibraries.SystemLibrary.Heroes.GetHero(i).Copy<Hero>(GetEngine());
            _ = HeroControl.RegisterHero(heroValue);
            _ = heroValue.Release();
        }
    }

    // AoWHSMap.PAS:2315
    public override bool AddMapLevel()
    {
        if (MapContainer.LevelCount < 3 && MapContainer.AddLevel(MapLevel.ClassId))
        {
            InitializeMapLevel((MapContainer.LevelCount - 1).TruncateToSByte());
            return true;
        }
        else
        {
            return false;
        }
    }

    // AoWHSMap.PAS:2654
    public override void Activate()
    {
        SynchronizeBegin();
        if (Day == 0 || GameSongs.Songs.Count == 0)
        {
            GameSongs.Songs.Clear();
            for (int i = 0, last = Current.StartupSongs.Songs.Count - 1; i <= last; i++)
            {
                if (CompareText(b: AbilityRules.TranslateResourceString(GetEngine(), S: GameText.FileNotFoundText), a: Current.StartupSongs.SongNames[i]) != 0)
                {
                    string item = Current.StartupSongs.Songs[i];
                    _ = GameSongs.Songs.Add(item);
                }
            }
        }

        GameStatus = StrategicMapGameStatus.Playing;
        DebugSave = GetEngine().Registry.GetAutoDebugSave();
        GlobalMagicControl.Update();
        Players.BeginUpdate();
        Players.EndUpdate();
        for (int i = 0, last = Players.Count - 1; i <= last; i++)
        {
            if (Players.GetPlayers(i).Behavior == PlayerBehavior.Random)
            {
                int random = Current.Random(5);
                Players.GetPlayers(i).Behavior = (PlayerBehavior)random;
            }
        }

        if (PlayerControl is null)
        {
            if (PlayMode == PlayMode.None)
            {
                PlayMode = PlayMode.Turn;
            }

            if (Players.LocalHumanPlayers.Count > 1)
            {
                PlayMode = PlayMode.Turn;
            }

            SetupPlayerControl(PlayMode, connection: ConnectionType);
        }

        TokenManager.Initialize(PlayerCommunicator);
        base.Activate();
        if (!Status.Contains(HexSpriteMapStatuses.Editting))
        {
            if (Day == 0)
            {
                StartNewMap();
            }
            else
            {
                StartLoadedMap();
            }

            ShowMessage("");
        }

        if (GetEngine().Registry.GetLogTokenEvents())
        {
            StartTokenEventLogging();
        }

        SynchronizeEnd();
    }

    // AoWHSMap.PAS:2326
    public override bool CreateNewMap(NewHexSpriteMapSettings settings) => base.CreateNewMap(settings);

    // AoWHSMap.PAS:3130
    public override int ChangeTerrain(EngineObject? mapObject, int X, int Y, int L, int radius, int terrainType)
    {
        RandSeed = Current.Random(16777215);
        return base.ChangeTerrain(mapObject, X, Y, L, radius, terrainType);
    }

    // AoWHSMap.PAS:3137
    public override int ChangeTerrain(EngineObject? mapObject, int X, int Y, int L, int radius, ChangeTerrainProc changeTerrainProc, TerrainChangedProc? terrainChangedProc)
    {
        RandSeed = Current.Random(16777215);
        return base.ChangeTerrain(mapObject, X, Y, L, radius, changeTerrainProc, terrainChangedProc);
    }

    // AoWHSMap.PAS:2983
    public override void DrawDynamicScene(ref LockedSurface lockedSurface)
    {
        base.DrawDynamicScene(ref lockedSurface);
        DrawFog(lockedSurface);
        if (ShowGrid)
        {
            DrawGrid(lockedSurface);
        }

        SelectedArmy.ShowMovePaths(lockedSurface);
        TargetSelection.ShowSelection(ViewPixelX, ViewPixelY, lockedSurface);
        if (Mouse.SelectionRectangle.Left != -1)
        {
            DrawSelectionRectangle(ViewPixelX, ViewPixelY, R: Mouse.SelectionRectangle, lockedSurface);
        }
    }

    // AoWHSMap.PAS:2568
    public override void NewFrame()
    {
        base.NewFrame();
        if (Status.Contains(HexSpriteMapStatuses.Activated))
        {
            PlayerControl.NewFrame();
            PlayerCommunicator.Process();
            if (CombatMap is null && GameStatus != StrategicMapGameStatus.Terminated)
            {
                AIExecutor.Process();
            }

            TokenManager.Process();
            UpdateCursor();
            UpdateExecuteEventLogDelay();
            UpdateTurnTimeBeep();
            UpdateEffects();
            CombatMap? combatMap = CombatMap;
            combatMap?.NewFrame();
        }
    }

    // AoWHSMap.PAS:1554
    public override void SetSeatedPlayer(PlayerId? player) => PlayerControl.SetSeatedPlayer(player);

    // AoWHSMap.PAS:1887
    public void TriggerCombatEvent(Battle combat)
    {
        NotifyEvent<Battle>? handlers = CombatTriggered;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(CombatTriggered));
        handlers?.Invoke(combat);
    }

    // AoWHSMap.PAS:1892
    public void TriggerGameTerminatedEvent()
    {
        GameStatus = StrategicMapGameStatus.Terminated;
        GameEnded?.Invoke(this);
    }

    // AoWHSMap.PAS:2012
    public void TriggerPlayerTerminatedEvent(PlayerId? player)
    {
        NotifyEvent<sbyte>? handlers = PlayerTerminated;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(PlayerTerminated));
        handlers?.Invoke((sbyte)PlayerIdExtensions.ToOriginal(player));
    }

    // AoWHSMap.PAS:2586
    public void StartNewMap()
    {
        TurnOrder.Clear();
        if (TurnOrderMode == StrategicMapTurnOrderMode.Fixed)
        {
            for (int i = 0, last = Players.Count - 1; i <= last; i++)
            {
                _ = TurnOrder.Add(i.TruncateToByte());
            }
        }

        if (TurnOrderMode == StrategicMapTurnOrderMode.Random)
        {
            GenerateNewTurnOrder();
        }

        for (int i = 0, last = Players.Count - 1; i <= last; i++)
        {
            Player? players = Players.GetPlayers(i);
            Players.GetPlayers(i).SetGems(players.InitialGold);
            Player? fPlayersPlayers = Players.GetPlayers(i);
            Players.GetPlayers(i).MagicControl.SetMana(fPlayersPlayers.MagicControl.ExternalMana);
            switch (Players.GetPlayers(i).PlayerType)
            {
                case PlayerType.Cpu4:
                    Player? players2 = Players.GetPlayers(i);
                    Players.GetPlayers(i).SetGems(players2.Gems + 250);
                    Player? players3 = Players.GetPlayers(i);
                    Players.GetPlayers(i).MagicControl.SetMana(players3.MagicControl.Mana + 250);
                    break;
                case PlayerType.Cpu5:
                    Player? players4 = Players.GetPlayers(i);
                    Players.GetPlayers(i).SetGems(players4.Gems + 500);
                    Player? players5 = Players.GetPlayers(i);
                    Players.GetPlayers(i).MagicControl.SetMana(players5.MagicControl.Mana + 500);
                    break;
                case PlayerType.Human:
                case PlayerType.Cpu1:
                case PlayerType.Cpu2:
                case PlayerType.Cpu3:
                case PlayerType.Independent:
                case PlayerType.Free:
                default:
                    break;
            }
        }

        PlayerControl.StartNewMap();
    }

    // AoWHSMap.PAS:2615
    public void StartLoadedMap() => PlayerControl.StartLoadedMap();

    // AoWHSMap.PAS:1996
    public void TriggerExecuteEventLog(EventLog log, bool immediate, ExecuteDoneEvent? done)
    {
        ExecuteEventLogNotification notification = new();
        ExecuteEventLogInfo info = notification.Info;
        info.Log = log;
        info.Done = done;
        info.Immediate = immediate;
        if (!immediate)
        {
            _ = log.AddRef();
            _executeEventLogCallBacks.Add(info);
            if (AILockCount == 0)
            {
                ExecuteNextEventLog();

                notification.Info = info;
            }
        }
        else
        {
            notification.Info = info;
            ExecutingEventLog?.Invoke(notification);
        }
    }

    // TriggerExecuteEventLog for a done callback that takes the log's own class. The compiler infers TLog
    // from the log, so the callback and the log it is registered for have the same class.
    public void TriggerTypedExecuteEventLog<TLog>(TLog log, bool immediate, Action<TLog> done) where TLog : EventLog
    {
        ExecuteDoneEvent? callback = null;
        if (done is not null)
        {
            // ClearExecuteEventLogBuffer passes the next queued log to the first callback
            // (AoWHSMap.PAS:1931). A log of another kind throws on this cast, the same way the old cast did.
            callback = stored => done((TLog)stored);
        }

        TriggerExecuteEventLog(log, immediate: immediate, done: callback);
    }

    // AoWHSMap.PAS:1476
    public void SynchronizeBegin() => _synchronizeCount += 1;

    // AoWHSMap.PAS:1481
    public void SynchronizeEnd() => _synchronizeCount -= 1;

    // AoWHSMap.PAS:1486
    public bool Synchronized => _synchronizeCount != 0 || TokenManager.TokenExecutor.Executing || Status.Contains(HexSpriteMapStatuses.Editting);

    // AoWHSMap.PAS:1509
    public void PlayerDisconnected(PlayerId? player) => PlayerControl.PlayerDisconnected(player);

    // AoWHSMap.PAS:1514
    public void PlayerConnectionLost() => PlayerControl.ConnectionLost();

    // AoWHSMap.PAS:1525
    public StrategicMapIndependentAIMode IndependentAIMode
    {
        get
        {
            StrategicMapIndependentAIMode result = _independentAIMode;
            return !Current.Status.Contains(HexSpriteMapStatuses.Editting) && Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentTypes.FireMastery)
                ? result == StrategicMapIndependentAIMode.Off ? StrategicMapIndependentAIMode.Normal : StrategicMapIndependentAIMode.Aggressive
                : result;
        }
    }

    // AoWHSMap.PAS:1534
    public bool SaveGame(string fileName)
    {
        if (Current.SeatedPlayer is not null)
        {
            SaveGameTokenEvent eventValue = new(null)
            {
                Holder = CreationHolder(),
                FileName = ExpandFileName(fileName)
            };
            Current.TokenManager.GetTokenControl(Current.SeatedPlayer).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // AoWHSMap.PAS:1559
    public bool TerrainVisible(int X, int Y, int L, int radius)
    {
        // { Original returns true immediately when exploration is enabled. }
        bool result = Current.ExplorationEnabled;
        MapLevel mapLevel = MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
        while (walk.Radius <= radius && !result)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                MapField field = mapLevel.GetField(walk.X, walk.Y);
                result = field.GetExplored(Current.SeatedPlayer);
            }

            if (!result)
            {
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        return result;
    }

    // AoWHSMap.PAS:1580
    public bool Visible(int X, int Y, int L, int radius)
    {
        bool result = false;
        MapLevel mapLevel = MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
        while (walk.Radius <= radius && !result)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                MapField field = mapLevel.GetField(walk.X, walk.Y);
                result = (field.FogCount != 0 || !Current.FogEnabled) && (field.GetExplored(Current.SeatedPlayer) || !Current.ExplorationEnabled);
            }

            if (!result)
            {
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        return result;
    }

    // AoWHSMap.PAS:1602
    public bool WatchingTerrain(int X, int Y, int L, int radius)
    {
        bool result = false;
        MapLevel mapLevel = MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
        while (walk.Radius <= radius && !result)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                MapField field = mapLevel.GetField(walk.X, walk.Y);
                result = (field.GetExplored(Current.SeatedPlayer) || !Current.ExplorationEnabled) && Viewing(walk.X, walk.Y, L);
            }

            if (!result)
            {
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        return result;
    }

    // AoWHSMap.PAS:1624
    public bool Watching(int X, int Y, int L, int radius)
    {
        bool result = false;
        MapLevel mapLevel = MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
        while (walk.Radius <= radius && !result)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                MapField field = mapLevel.GetField(walk.X, walk.Y);
                result = (field.FogCount != 0 || !Current.FogEnabled) && (field.GetExplored(Current.SeatedPlayer) || !Current.ExplorationEnabled) && Viewing(walk.X, walk.Y, L);
            }

            if (!result)
            {
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        return result;
    }

    // AoWHSMap.PAS:1647
    public bool ValidateTerrainObservation(PlayerId? player, int X, int Y, int L, int radius)
    {
        bool result;
        if (ShowIndependentMoves && player != Current.SeatedPlayer)
        {
            result = false;
            MapLevel mapLevel = MapContainer.GetMapLevel(L);
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
            while (walk.Radius <= radius && !result)
            {
                int wx = walk.X;
                if (wx >= 0 && walk.Y >= 0 && wx < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    MapField field = mapLevel.GetField(wx, walk.Y);
                    result = field.GetExplored(Current.SeatedPlayer) || !Current.ExplorationEnabled;
                }

                if (!result)
                {
                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }

            if (result)
            {
                MakeVisible(walk.X, walk.Y, L, radius: 3);
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // AoWHSMap.PAS:1704
    public void UnlockGameOverEventCallback() => UnlockGameOverEvent();

    // AoWHSMap.PAS:1709
    public bool SelectNextHero()
    {
        Player? player = Players.GetPlayers(SeatedPlayer);
        if (player.Armies.Count != 0)
        {
            int left = player.Armies.Count;
            int i = player.HeroJoinDay;
            if (i < 0 || i >= player.Armies.Count)
            {
                i = 0;
            }

            while (true)
            {
                i += 1;
                left -= 1;
                if (i == player.Armies.Count)
                {
                    i = 0;
                }

                if (left == 0 || player.Armies.GetArmy(i).Hero(255, types: HeroTypes.Hero | HeroTypes.Leader | HeroTypes.HeroLeader | HeroTypes.Merchant) is not null)
                {
                    break;
                }
            }

            if (player.Armies.GetArmy(i).Hero(255, types: HeroTypes.Hero | HeroTypes.Leader | HeroTypes.HeroLeader | HeroTypes.Merchant) is not null)
            {
                Army army = Players.GetPlayers(SeatedPlayer).Armies.GetArmy(i);
                CenterViewTo(army.Location.X, army.Location.Y, army.Location.MapLevel);
                Selection.UnselectLocation();
                HexGeometry.HexToPixel(army.Location.X, army.Location.Y, out int pixelX, out int pixelY);
                Selection.SelectLocation(pixelX, pixelY, lHitPoints: army.Location.MapLevel);
                player.HeroJoinDay = i;
            }
            else
            {
                Selection.UnselectLocation();
            }
        }

        return true;
    }

    // AoWHSMap.PAS:1741
    public bool SelectNextArmy()
    {
        Player? player = Players.GetPlayers(SeatedPlayer);
        if (player.Armies.Count != 0)
        {
            int left = player.Armies.Count;
            int i = player.BaseHeroJoinDay;
            if (i < 0 || i >= player.Armies.Count)
            {
                i = 0;
            }

            while (true)
            {
                i += 1;
                left -= 1;
                if (i == player.Armies.Count)
                {
                    i = 0;
                }

                if (left == 0 || (player.Armies.GetArmy(i).MovePoints(255) > 1 && (player.Armies.GetArmy(i).UserMode & (ArmyUserModes.Guard | ArmyUserModes.TurnDone)) == ArmyUserModes.None))
                {
                    break;
                }
            }

            if (player.Armies.GetArmy(i).MovePoints(255) > 1 && (player.Armies.GetArmy(i).UserMode & (ArmyUserModes.Guard | ArmyUserModes.TurnDone)) == ArmyUserModes.None)
            {
                Army army = Players.GetPlayers(SeatedPlayer).Armies.GetArmy(i);
                CenterViewTo(army.Location.X, army.Location.Y, army.Location.MapLevel);
                Selection.UnselectLocation();
                HexGeometry.HexToPixel(army.Location.X, army.Location.Y, out int pixelX, out int pixelY);
                Selection.SelectLocation(pixelX, pixelY, lHitPoints: army.Location.MapLevel);
                player.BaseHeroJoinDay = i;
            }
            else
            {
                Selection.UnselectLocation();
            }
        }

        return true;
    }

    // AoWHSMap.PAS:1775
    public bool SelectPreviousArmy()
    {
        Player? player = Players.GetPlayers(SeatedPlayer);
        if (player.Armies.Count != 0)
        {
            int i = player.BaseHeroJoinDay;
            if (i < 0 || i >= player.Armies.Count)
            {
                i = 0;
            }

            int left = player.Armies.Count;
            while (true)
            {
                i -= 1;
                left -= 1;
                if (i < 0)
                {
                    i = player.Armies.Count - 1;
                }

                if (left == 0 || (player.Armies.GetArmy(i).MovePoints(255) > 1 && (player.Armies.GetArmy(i).UserMode & (ArmyUserModes.Guard | ArmyUserModes.TurnDone)) == ArmyUserModes.None))
                {
                    break;
                }
            }

            if (player.Armies.GetArmy(i).MovePoints(255) > 1 && (player.Armies.GetArmy(i).UserMode & (ArmyUserModes.Guard | ArmyUserModes.TurnDone)) == ArmyUserModes.None)
            {
                Army army = Players.GetPlayers(SeatedPlayer).Armies.GetArmy(i);
                CenterViewTo(army.Location.X, army.Location.Y, army.Location.MapLevel);
                Selection.UnselectLocation();
                HexGeometry.HexToPixel(army.Location.X, army.Location.Y, out int pixelX, out int pixelY);
                Selection.SelectLocation(pixelX, pixelY, lHitPoints: army.Location.MapLevel);
                player.BaseHeroJoinDay = i;
            }
            else
            {
                Selection.UnselectLocation();
            }
        }

        return true;
    }

    // AoWHSMap.PAS:1809
    public void NotifySeatedPlayerChanged()
    {
        Selection.UnselectAll();
        EngineMessage.InitializeMessage(out EngineMessage message, messageId: 537001990, sender: this);
        MapContainer.SendMessage(ref message, sender: this);
        EngineMessage.InitializeMessage(out message, messageId: 537001993, sender: this);
        MapContainer.SendMessage(ref message, sender: this);
        NotifyEvent<StrategicMap>? handlers = SeatedPlayerChanged;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(SeatedPlayerChanged));
        handlers?.Invoke(Current);
        ShowMessage("");
    }

    // AoWHSMap.PAS:1822
    public void BeginUnitUpdate() => _unitUpdateCount += 1;

    // AoWHSMap.PAS:1827
    public void EndUnitUpdate()
    {
        _unitUpdateCount -= 1;
        if (_unitUpdateCount == 0)
        {
            NotifyUnitChanged(null);
        }
    }

    // AoWHSMap.PAS:1833
    public void NotifyUnitChanged(Troop? troop)
    {
        if (_unitUpdateCount == 0)
        {
            _unitChangedEventsStore.Raise(troop);
        }
    }

    // AoWHSMap.PAS:1838
    public void NotifyUnitDestroyed(Troop troop)
    {
        if (!Current.Status.Contains(HexSpriteMapStatuses.Destroying))
        {
            NotifyEvent<Troop>? handlers = UnitDestroyed;
            EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(UnitDestroyed));
            handlers?.Invoke(troop);
        }
    }

    // AoWHSMap.PAS:1844
    public void BeginArmyUpdate() => _armyUpdateCount += 1;

    // AoWHSMap.PAS:1849
    public void EndArmyUpdate()
    {
        _armyUpdateCount -= 1;
        if (_armyUpdateCount == 0)
        {
            NotifyArmyChanged(null);
        }
    }

    // AoWHSMap.PAS:1855
    public void NotifyArmyChanged(Army? army)
    {
        if (_armyUpdateCount == 0)
        {
            NotifyEvent<Army>? handlers = ArmyChanged;
            EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(ArmyChanged));
            handlers?.Invoke(army);
        }
    }

    // AoWHSMap.PAS:1860
    public void NotifyArmyDestroyed(Army army)
        // TArmy.Destroy passes its Self pointer
        => ArmyDestroyed?.Invoke(army);

    // AoWHSMap.PAS:1866
    // TArmy.Destroy passes TArmy
    public void BeginStructureUpdate() => _structureUpdateCount += 1;

    // AoWHSMap.PAS:1871
    public void EndStructureUpdate()
    {
        _structureUpdateCount -= 1;
        if (_structureUpdateCount == 0)
        {
            NotifyStructureChanged(null);
        }
    }

    // AoWHSMap.PAS:1877
    public void NotifyStructureChanged(Structure? structure)
    {
        if (_structureUpdateCount == 0)
        {
            NotifyEvent<Structure>? handlers = StructureChanged;
            EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(StructureChanged));
            handlers?.Invoke(structure);
        }
    }

    // AoWHSMap.PAS:1882
    public void NotifyStructureDestroyed(Structure structure)
    {
        NotifyEvent<Structure>? handlers = StructureDestroyed;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(StructureDestroyed));
        handlers?.Invoke(structure);
    }

    // AoWHSMap.PAS:1898
    public void ExecuteExitGame()
    {
        ExitPending = true;
        NotifyEvent<StrategicMap>? handlers = ExitingGame;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(ExitingGame));
        handlers?.Invoke(this);
    }

    // AoWHSMap.PAS:1904
    public void ExitGame(ExitGameMode mode, ExitGameCallBack? done)
    {
        if (!ExitPending)
        {
            PlayerControl playerControl = PlayerControl;
            if (playerControl is not null)
            {
                playerControl.ExitGame(mode, done);
            }
            else
            {
                ExecuteExitGame();
                done?.Invoke(mode, Exited: true);
            }
        }
        else
        {
            done?.Invoke(mode, Exited: true);
        }
    }

    // AoWHSMap.PAS:1921
    public void TriggerEndTurnEvent(PlayerId? player)
    {
        NotifyEvent<sbyte>? handlers = TurnEnded;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(TurnEnded));
        handlers?.Invoke((sbyte)PlayerIdExtensions.ToOriginal(player));
    }

    // AoWHSMap.PAS:1926
    public void ClearExecuteEventLogBuffer()
    {
        while (_executeEventLogCallBacks.GetCount() > 0)
        {
            if (_executeEventLogCallBacks.GetItem(0).Done is not null)
            {
                ExecuteEventLogInfo item = _executeEventLogCallBacks.GetItem(1);
                _executeEventLogCallBacks.GetItem(0).Done(item.Log);
            }

            // { Original passes the second record's log to the first callback, even for a one-item list. }
            _executeEventLogCallBacks.Delete(0);
        }
    }

    // AoWHSMap.PAS:1937
    public void ExecuteNextEventLog()
    {
        ExecuteEventLogNotification notification = new();
        ExecuteEventLogInfo info = notification.Info;
        if (_executeEventLogCallBacks.GetCount() != 0)
        {
            AILockCount += 1;
            info = _executeEventLogCallBacks.GetItem(0);
            info.Done = ExecuteEventLogDone;
            notification.Info = info;
            ExecutingEventLog?.Invoke(notification);
        }
    }

    // AoWHSMap.PAS:1950
    public void SetExecuteEventLogDelay(int value)
    {
        if (_executeEventLogDelay == 0)
        {
            _executeEventLogDelay = value;
            LockExecuteEventLog();
        }
        else
        {
            if (value > _executeEventLogDelay)
            {
                _executeEventLogDelay = value;
            }
        }
    }

    // AoWHSMap.PAS:1960
    public void UpdateExecuteEventLogDelay()
    {
        if (_executeEventLogDelay != 0)
        {
            _executeEventLogDelay -= 1;
            if (_executeEventLogDelay == 0)
            {
                UnlockExecuteEventLog();
            }
        }
    }

    // AoWHSMap.PAS:1969
    public void ExecuteEventLogDone(EventLog log)
    {
        if (_executeEventLogCallBacks.GetCount() > 0 && log == _executeEventLogCallBacks.GetItem(0).Log)
        {
            ExecuteEventLogInfo info = _executeEventLogCallBacks.GetItem(0);
            _executeEventLogCallBacks.Delete(0);
            info.Done?.Invoke(info.Log);

            _ = info.Log.Release();
        }

        SetExecuteEventLogDelay(2);
        UnlockExecuteEventLog();
    }

    // AoWHSMap.PAS:1985
    public void LockExecuteEventLog() => AILockCount += 1;

    // AoWHSMap.PAS:1990
    public void UnlockExecuteEventLog()
    {
        AILockCount -= 1;
        if (AILockCount == 0)
        {
            ExecuteNextEventLog();
        }
    }

    // AoWHSMap.PAS:2018
    public void GameOverEventHandled(PlayerId? player) => PlayerControl.GameOverEventHandled(player);

    // AoWHSMap.PAS:2024
    public void ExecuteGameOverEvent(GameOverEventLog eventValue)
    {
        for (int i = 1, last = Current.Players.Count - 1; i <= last; i++)
        {
            Player? player = Current.Players.GetPlayers(i);
            if (i == PlayerIdExtensions.ToOriginal(eventValue.Player) || player.GameOverStatus == GameOverStatus.None)
            {
                GameOverEventLog log = eventValue.Copy<GameOverEventLog>(GetEngine());
                if (_gameOverLockCount == 0)
                {
                    player.EventLogbook.AddEvent(log);
                    if (i == PlayerIdExtensions.ToOriginal(Current.SeatedPlayer))
                    {
                        _ = log.Execute(false, trigger: ExecuteTrigger.Event, done: null);
                    }
                }
                else
                {
                    log.TargetPlayer = PlayerId.FromOriginal((sbyte)i);
                    _executeEventLogBuffer.AddEvent(log);
                }

                _ = log.Release();
            }
        }
    }

    // AoWHSMap.PAS:2052
    public void UnlockGameOverEvent()
    {
        _gameOverLockCount -= 1;
        if (_gameOverLockCount == 0)
        {
            while (_executeEventLogBuffer.Count != 0)
            {
                // ExecuteGameOverEvent stores only game over logs in this buffer.
                GameOverEventLog stored = _executeEventLogBuffer.GetChild<GameOverEventLog>(0);
                sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(stored.TargetPlayer);
                EventLog log = stored.Copy<EventLog>(GetEngine());
                Current.Players.GetPlayers(player).EventLogbook.AddEvent(log);
                if (player == PlayerIdExtensions.ToOriginal(Current.SeatedPlayer))
                {
                    _ = log.Execute(false, trigger: ExecuteTrigger.Event, done: null);
                }

                _ = log.Release();
                EventLog events = _executeEventLogBuffer.GetEvents(0);
                _executeEventLogBuffer.RemoveEvent(events);
            }
        }
    }

    // AoWHSMap.PAS:2071
    public void LockGameOverEvent() => _gameOverLockCount += 1;

    // AoWHSMap.PAS:2077
    public void GameOver(PlayerId? player, GameOverStatus status)
    {
        if (PlayerIdExtensions.ToOriginal(player) > 0 && Status.Contains(HexSpriteMapStatuses.Activated) && !Status.Contains(HexSpriteMapStatuses.Editting) && !Status.Contains(HexSpriteMapStatuses.Destroying))
        {
            PlayerControl.GameOver(player, status);
        }
    }

    // AoWHSMap.PAS:2085
    public void VerifyAddOnPacks()
    {
    }

    // AoWHSMap.PAS:2088
    public bool ValidateMap(DelphiStringCollection messages)
    {
        EngineMessage.InitializeMessage(out EngineMessage message, messageId: 537002243, sender: this);
        message.Payload = messages;
        Array4<int> data = message.Data;
        data[0] = 0;
        message.Data = data;
        Current.SendMessage(ref message, sender: this);
        bool result = message.Data[0] == 0;
        _ = result
            ? messages.Add(AbilityRules.TranslateResourceString(GetEngine(), S: s_mapSuccessfullyValidatedText))
            : messages.Add(AbilityRules.TranslateResourceString(GetEngine(), S: s_mapValidationFailedText));

        return result;
    }

    // AoWHSMap.PAS:2261
    public void InitializeMapLevel(int level)
    {
        HexControl control = Engine.GamePack.HexControl;
        if (level == 0)
        {
            for (int x = 6, last = MapContainer.Width - 7; x <= last; x++)
            {
                for (int y = 6, lastY = MapContainer.Height - 7; y <= lastY; y++)
                {
                    _ = control.Place(MapContainer, x.TruncateToSByte(), y.TruncateToSByte(), L: 0, terrainType: 0);
                }
            }
        }
        else
        {
            for (int x = 1, last = MapContainer.Width - 7; x <= last; x++)
            {
                for (int y = 1, lastY = MapContainer.Height - 7; y <= lastY; y++)
                {
                    _ = control.Place(MapContainer, x.TruncateToSByte(), y.TruncateToSByte(), L: level.TruncateToSByte(), terrainType: 7);
                }
            }
        }

        for (int x = 0, last = MapContainer.Width - 1; x <= last; x++)
        {
            for (int y = 0; y <= 5; y++)
            {
                _ = control.Place(MapContainer, x.TruncateToSByte(), y.TruncateToSByte(), L: level.TruncateToSByte(), terrainType: 15);
            }

            for (int y = MapContainer.Height - 6, lastY = MapContainer.Height - 1; y <= lastY; y++)
            {
                _ = control.Place(MapContainer, x.TruncateToSByte(), y.TruncateToSByte(), L: level.TruncateToSByte(), terrainType: 15);
            }
        }

        for (int y = 6, lastY = MapContainer.Height - 7; y <= lastY; y++)
        {
            for (int x = 0; x <= 5; x++)
            {
                _ = control.Place(MapContainer, x.TruncateToSByte(), y.TruncateToSByte(), L: level.TruncateToSByte(), terrainType: 15);
            }

            for (int x = MapContainer.Width - 6, last = MapContainer.Width - 1; x <= last; x++)
            {
                _ = control.Place(MapContainer, x.TruncateToSByte(), y.TruncateToSByte(), L: level.TruncateToSByte(), terrainType: 15);
            }
        }
    }

    // AoWHSMap.PAS:2332
    public void NotifyPlayerChanged(int X, int Y, int L, int radius)
    {
        MapFieldMessage message = default;
        MapLevel mapLevel = MapContainer.GetMapLevel(L);
        message.MessageId = 4365;
        message.Result = 1;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                MapField field = mapLevel.GetField(walk.X, walk.Y);
                if ((field.FieldFlags & MapField.PlayerChangeListenerFlag) != 0)
                {
                    MapFieldMessageParameters param = message.Param;
                    param[0] = walk.Radius;
                    message.Param = param;
                    field.SendMapFieldMessage(ref message);
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // AoWHSMap.PAS:2359
    public void UpdatePlayerSeen(PlayerId? player, int X, int Y, int L, int sightRadius, int radius)
    {
        MapFieldMessage message = default;
        MapLevel mapLevel = MapContainer.GetMapLevel(L);
        message.MessageId = 4400;
        MapFieldMessageParameters param = message.Param;
        param[0] = Session.MsgArgIds.FromPlayer(player);
        message.Param = param;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
        while (walk.Radius <= sightRadius || walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                MapField field = mapLevel.GetField(walk.X, walk.Y);
                if ((field.FieldFlags & MapField.OwnerFlag) != 0)
                {
                    message.Result = -1;
                    if (walk.Radius <= radius)
                    {
                        // { Original leaves the flag uninitialized outside Radius. }
                        MapFieldMessageParameters param2 = message.Param;
                        param2[1] = 1;
                        message.Param = param2;
                    }

                    field.SendMapFieldMessage(ref message);
                    if (message.Result.Int != -1 && message.Result.Int != PlayerIdExtensions.ToOriginal(player))
                    {
                        Current.Players.GetPlayers(player).PlayerSeen(PlayerId.FromOriginal((sbyte)message.Result.Int));
                        for (int i = 0, last = Players.Count - 1; i <= last; i++)
                        {
                            if (i != PlayerIdExtensions.ToOriginal(player) && Players.GetDiplomaticRelation(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(player)), otherPlayer: PlayerId.FromOriginal((sbyte)i)) == DiplomaticRelation.Alliance)
                            {
                                Current.Players.GetPlayers(i).PlayerSeen(PlayerId.FromOriginal((sbyte)message.Result.Int));
                            }
                        }
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // AoWHSMap.PAS:2395
    public void NotifyPlayerChanged(int X, int Y, int L) => NotifyPlayerChanged(X, Y, L, radius: 12);

    // AoWHSMap.PAS:2401
    public bool VisibleForPlayer(int X, int Y, int level, PlayerId? player)
    {
        MapFieldMessage message = default;
        MapLevel mapLevel = MapContainer.GetMapLevel(level);
        if (PlayerIdExtensions.ToOriginal(player) >= 0)
        {
            message.MessageId = 4362;
            message.Result = 0;
            MapFieldMessageParameters param = message.Param;
            param[1] = Session.MsgArgIds.FromPlayer(player);
            message.Param = param;
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
            while (message.Result.Int == 0 && walk.Radius <= 12)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    MapField field = mapLevel.GetField(walk.X, walk.Y);
                    if ((field.FieldFlags & MapField.ViewerFlag) != 0)
                    {
                        MapFieldMessageParameters param2 = message.Param;
                        param2[0] = walk.Radius;
                        message.Param = param2;
                        field.SendMapFieldMessage(ref message);
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            return message.Result.Int != 0;
        }
        else
        {
            return false;
        }
    }

    // AoWHSMap.PAS:2434
    public bool FoggedForPlayer(int X, int Y, int L, PlayerId? player)
    {
        MapFieldMessage message = default;
        MapLevel mapLevel = MapContainer.GetMapLevel(L);
        if (!ExplorationEnabled || mapLevel.GetField(X, Y).GetExplored(player))
        {
            if (FogEnabled)
            {
                if (player != Current.SeatedPlayer)
                {
                    message.MessageId = 4362;
                    message.Result = 0;
                    MapFieldMessageParameters param = message.Param;
                    param[1] = Session.MsgArgIds.FromPlayer(player);
                    message.Param = param;
                    HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
                    while (message.Result.Int == 0 && walk.Radius <= 12)
                    {
                        if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                        {
                            MapField field = mapLevel.GetField(walk.X, walk.Y);
                            if ((field.FieldFlags & MapField.ViewerFlag) != 0)
                            {
                                MapFieldMessageParameters param2 = message.Param;
                                param2[0] = walk.Radius;
                                message.Param = param2;
                                field.SendMapFieldMessage(ref message);
                            }
                        }

                        HexNumberWalkData.WalkToNextHexNumber(ref walk);
                    }

                    return message.Result.Int == 0;
                }
                else
                {
                    MapField fieldValue = Current.MapContainer.GetField(X, Y, L);
                    return fieldValue.FogCount == 0;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    // AoWHSMap.PAS:2474
    public void MouseMoved(object sender)
    {
        // The original tests Xhx only and with Yhx -1 read memory before the field block. The
        // port treats that point as off the map.
        if (Mouse.Capture is null && Mouse.HexX != -1 && Mouse.HexY != -1)
        {
            int hexY = Mouse.HexY;
            int hexX = Mouse.HexX;
            MapField field = MapContainer.GetField(hexX, hexY, ViewMapLevel);
            if (Current.ExplorationEnabled && !field.GetExplored(Current.SeatedPlayer))
            {
                return;
            }

            if (!Current.FogEnabled || field.FogCount != 0)
            {
                int pixelY = Mouse.PixelY;
                int pixelX = Mouse.PixelX;
                Mouse.CursorMode = Selection.CanSelectLocation(pixelX, pixelY, lHitPoints: ViewMapLevel) && !Mouse.SelectingRectangle
                    ? 257
                    : 0;
            }
        }
    }

    // AoWHSMap.PAS:2490
    public void UpdateCursor()
    {
        switch (Mouse.CursorMode)
        {
            case 0:
                Mouse.Cursor = 0;
                break;
            case 257:
                Mouse.Cursor = ((Frame >>> 2) % 4) + 52;
                break;
            case 258:
                Mouse.Cursor = ((Frame >>> 2) & 1) + 49;
                break;
            case 259:
                Mouse.Cursor = ((Frame >>> 2) % 3) + 58;
                break;
            default:
                break;
        }

        Mouse.CursorVisible = Mouse.Cursor != 0;
    }

    // AoWHSMap.PAS:2502
    public void ResetSeatedPlayerView()
    {
        if (SeatedPlayer is not null)
        {
            Player? player = Players.GetPlayers(SeatedPlayer);
            MapCoordinate location;
            if (player.View.X > 0)
            {
                location = new MapCoordinate(player.View.X, player.View.Y, player.View.MapLevel);
            }
            else
            {
                player.GetLeaderLocation(out int locationX, out int locationY, L: out int locationMapLevel);
                location = new MapCoordinate((sbyte)locationX, (sbyte)locationY, (sbyte)locationMapLevel);
                if (location.X == -1)
                {
                    if (player.Armies.Count == 0)
                    {
                        location = new MapCoordinate(0, 0, 0);
                    }
                    else
                    {
                        Army army = player.Armies.GetArmy(0);
                        location = new MapCoordinate(army.Location.X, army.Location.Y, army.Location.MapLevel);
                    }
                }
            }

            CenterViewTo(location.X, location.Y, location.MapLevel);
        }
    }

    // AoWHSMap.PAS:2538
    public void Select(HexSprite? sprite, bool center)
    {
        // { This eliminated copy preserves the original saved registers. }
        Selection.UnselectLocation();
        Selection.SelectLocationHexSprite(sprite);
        if (center)
        {
            int hexLevel = sprite.HexLevel;
            int hexY = sprite.HexY;
            CenterViewTo(sprite.HexX, hexY, hexLevel);
        }
    }

    // AoWHSMap.PAS:2546
    public void NotifyPlayerChanged()
    {
        NotifyEvent<StrategicMap>? handlers = CurrentPlayerChanged;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(CurrentPlayerChanged));
        handlers?.Invoke(this);
    }

    // AoWHSMap.PAS:2552
    public void TokenControlActivatedEvent(object sender)
    {
        if (CombatMap is null)
        {
            int player = TokenManager.TokenExecutor.ActiveTacticalBattleIndex;
            if (player != PlayerIdExtensions.ToOriginal(CurrentPlayer))
            {
                CurrentPlayer = PlayerId.FromOriginal((sbyte)player);
                NotifyPlayerChanged();
            }
        }
    }

    // AoWHSMap.PAS:2621
    public void SetupPlayerControl(PlayMode mode, PlayerConnectionType connection)
    {
        switch (mode)
        {
            case PlayMode.Turn:
                if (ConnectionType == PlayerConnectionType.Email)
                {
                    if (PlayerControl is not PbemPlayerControl)
                    {
                        PlayerControl?.Dispose();
                        PlayerControl = new PbemPlayerControl(null) { Holder = CreationHolder() };
                    }
                }
                else
                {
                    // { Original tests TPBEMPlayerControl even in the non-email branch. }
                    if (PlayerControl is not PbemPlayerControl)
                    {
                        PlayerControl?.Dispose();
                        PlayerControl = new TurnPlayerControl(null) { Holder = CreationHolder() };
                    }
                }

                break;
            case PlayMode.Simultaneous:
                if (PlayerControl is not SimultaneousPlayerControl)
                {
                    PlayerControl?.Dispose();
                }

                // { Original replaces an existing control without freeing it. }
                PlayerControl = new SimultaneousPlayerControl(null) { Holder = CreationHolder() };
                break;
            case PlayMode.None:
            default:
                break;
        }

        ConnectionType = connection;
        PlayMode = mode;
        PlayerControl?.Setup();
    }

    // AoWHSMap.PAS:2692
    public void ActivateMessage()
    {
        if (Day == 0)
        {
            Races.ResetDefaultRelations();
        }

        if (!Status.Contains(HexSpriteMapStatuses.Editting) && Day == 0)
        {
            ExplorationEnabled = true;
            FogEnabled = true;
        }
    }

    // AoWHSMap.PAS:2703
    public int Random(int range)
    {
        int result;
        if (!Current.Status.Contains(HexSpriteMapStatuses.Destroying))
        {
            if (Synchronized)
            {
                RandSeed = DefendersSeed;
                result = DelphiRandom.Random(range);
                DefendersSeed = RandSeed;
            }
            else
            {
                LegacyApplication.ShowMessage("Invalid AoWHSMap.Random use");
                return 0;
            }
        }
        else
        {
            return DelphiRandom.Random(range);
        }

        return result;
    }

    // AoWHSMap.PAS:2723
    public void GenerateNewTurnOrder()
    {
        ByteList list = [];
        for (int i = 1, last = Players.Count - 1; i <= last; i++)
        {
            _ = list.Add(i.TruncateToByte());
        }

        TurnOrder.Clear();
        _ = TurnOrder.Add(0);
        while (list.Count != 0)
        {
            int index = Random(list.Count);
            int item = list[index];
            _ = TurnOrder.Add(item.TruncateToByte());
            list.Delete(index);
        }
    }

    // AoWHSMap.PAS:2749
    public bool EndTurn() => SeatedPlayer is not null && (!RequestCombatType || _executeEventLogCallBacks.GetCount() == 0) && TryEndPlayerTurn(SeatedPlayer);

    // AoWHSMap.PAS:2777
    public bool CancelEndTurn() => SeatedPlayer is not null && CancelPlayerTurn(SeatedPlayer);

    // AoWHSMap.PAS:2784
    public void LockUpdateVisibility() => _updateVisibilityLockCount += 1;

    // AoWHSMap.PAS:2790
    public void UnLockUpdateVisibility()
    {
        _updateVisibilityLockCount -= 1;
        if (_updateVisibilityLockCount == 0 && _visibilityChanged)
        {
            UpdateVisibility();
        }
    }

    // AoWHSMap.PAS:2797
    public void UpdateVisibility()
    {
        if (_updateVisibilityLockCount == 0)
        {
            if (!Status.Contains(HexSpriteMapStatuses.Destroying))
            {
                LockMapChangedEvent();
                EngineMessage.InitializeMessage(out EngineMessage message, messageId: 537001992, sender: this);
                Current.SendMessage(ref message, sender: this);
                UnlockMapChangedEvent();
                Current.InvalidateMap();
            }

            _visibilityChanged = false;
        }
        else
        {
            _visibilityChanged = true;
        }
    }

    // AoWHSMap.PAS:2817
    public void ResetMapLevels()
    {
        if (MapContainer is not null)
        {
            for (int i = 0, last = MapContainer.LevelCount - 1; i <= last; i++)
            {
                MapContainer.GetMapLevel(i).Reset();
            }
        }
    }

    // AoWHSMap.PAS:2867
    public void ExecuteNegotiateMove(int armyId, int X, int Y, int L, int movePoints)
    {
    }

    // AoWHSMap.PAS:2870
    public void InitializeCombatMap(Battle combat, string fileName)
    {
        if (CombatMap is null)
        {
            if (SeatedPlayer is not null)
            {
                Player? player = Players.GetPlayers(SeatedPlayer);
                GetViewCenter(out int x, out int y, out int l);
                player.View = new MapCoordinate(unchecked((sbyte)(byte)x), unchecked((sbyte)(byte)y), unchecked((sbyte)(byte)l));
            }

            // The tactical profile loads a combat map, and the loader returns the game map it loads.
            CombatMap = GetEngine().LoadHsmMap<CombatMap>(FileBoundary.ReadName(GetEngine().StartupDirectory + fileName), edit: false, progress: null, profile: GameProfile.TacticalMap);
            CombatMapCreated?.Invoke(this);
        }
    }

    // AoWHSMap.PAS:2891
    public void ReleaseCombatMap()
    {
        if (CombatMap is not null)
        {
            CombatMapDestroyed?.Invoke(this);
            CombatMap?.Dispose();
            CombatMap = null;
            ResetSeatedPlayerView();
        }
    }

    // AoWHSMap.PAS:2903
    public Battle CreateCombat(int classId)
    {
        Battle result;
        if (Combat is null)
        {
            // The class id selects a battle class, and the factory returns the engine object it creates.
            result = GetEngine().CreateEngineObject<Battle>(classId, owner: null);
            Combat = result;
            if (GameType == GameType.Tutorial && classId == 2_228_496)
            {
                TutorialControl.ShowMessage(53, immediate: false, onceOnly: true);
            }

            if (classId == 2_228_496)
            {
                Current.Mouse.ResetRectangleSelection();
                Current.Selection.UnselectLocation();
                TargetSelection.Deactivate();
                DisableTurnTimer();
            }
        }
        else
        {
            throw new InvalidOperationException("Combat already created");
        }

        return result;
    }

    // AoWHSMap.PAS:2922
    public void ShowSeatedPlayerCallback(EventLog log)
    {
        SetSeatedPlayer(Current.CurrentPlayer);
        EnableTurnTimer();
    }

    // AoWHSMap.PAS:2929
    public void DestroyCombat()
    {
        if (Combat is not null)
        {
            Current.LockGameOverEvent();
            if (Combat.Status.Contains(CombatStatuses.Activated))
            {
                Combat.Deactivate();
            }

            if (Combat.Status.Contains(CombatStatuses.Executing))
            {
                Combat.Finalize();
                sbyte player = Combat.CombatData.Players.GetPlayers(Combat.CombatData.AttackerPlayer).Player;
                if (Current.ConnectionType == PlayerConnectionType.Network && PlayMode == PlayMode.Simultaneous)
                {
                    int count = Current.Players.Count;
                    player = Combat.CombatData.Players.GetPlayers(Combat.CombatData.AttackerPlayer).Player;
                    while (count != 0 && Current.Players.GetPlayers(player).Status != PlayerStatus.Playing)
                    {
                        player = (sbyte)(player + 1);
                        if (player == Current.Players.Count)
                        {
                            player = 0;
                        }

                        count -= 1;
                    }
                }

                Current.TokenManager.TokenExecutor.ActivateTacticalBattle(player);
                if (PlayMode == PlayMode.Simultaneous)
                {
                    Current.TokenManager.TokenExecutor.HoldTacticalBattle = false;
                }
            }

            if (RequestCombatType && Combat.EngineClass.ClassId != FastBattle.ClassId && Combat.CombatData.Round != 0 && Combat.ExplorationSite != 1 && Players.GetPlayers(CurrentPlayer).PlayerType == PlayerType.Human)
            {
                LockExecuteEventLog();
                NewTurnEventLog log = new(null)
                {
                    Holder = CreationHolder(),
                    TurnPlayer = CurrentPlayer,
                    Mode = NewTurnEventLogMode.Dialog,
                    DialogStyle = NewTurnEventLogDialogStyles.Closeable,
                    Text = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.YourTurnContinuesText)
                };
                TriggerExecuteEventLog(log, immediate: false, done: ShowSeatedPlayerCallback);
                _ = log.Release();
                UnlockGameOverEvent();
                SetSeatedPlayer(null);
                UnlockExecuteEventLog();
            }
            else
            {
                if (Combat.EngineClass.ClassId == TacticalBattle.ClassId)
                {
                    EnableTurnTimer();
                }

                UnlockGameOverEvent();
            }

            Combat?.Dispose();
            Combat = null;
        }
    }

    // AoWHSMap.PAS:3026
    public void TriggerRelationsChanged()
    {
        if (Status.Contains(HexSpriteMapStatuses.Activated) && _relationsChangedLockCount == 0)
        {
            BeginUnitUpdate();
            UnitControl.TriggerUnitsChanged();
            NotifyEvent<StrategicMap>? handlers = PlayerChanged;
            EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(PlayerChanged));
            handlers?.Invoke(this);
            EndUnitUpdate();
        }
    }

    // AoWHSMap.PAS:3038
    public void LockRelationsChanged() => _relationsChangedLockCount += 1;

    // AoWHSMap.PAS:3044
    public void UnlockRelationsChanged()
    {
        Assert(_relationsChangedLockCount != 0, message: "Invalid MapChanged Lock usage", file: "AoWHSMap.PAS", line: 3056);
        _relationsChangedLockCount -= 1;
    }

    // AoWHSMap.PAS:3061
    public void UpdateTurnTimeBeep()
    {
        if (TurnTimerEnabled)
        {
            int seconds = TurnTimeLeft / 1_000;
            if (_turnTimeBeep != 0 && seconds < _turnTimeBeep && CombatMap is null)
            {
                _turnTimeBeep = seconds;
                _ = _turnTimeBeep == 0 ? AudioPlayback.PlaySample("SFX\\EVENTS\\CLOCK.WAV") : AudioPlayback.PlaySample("SFX\\EVENTS\\CLOCK.WAV");
            }
        }
    }

    // AoWHSMap.PAS:3078
    public void ResetTurnTimer()
    {
        _turnTimeElapsed = TurnTime * 1_000 * 60;
        _turnTimeBeep = 11;
        _turnTimerStart = -1;
    }

    // AoWHSMap.PAS:3086
    public int TurnTimeLeft
    {
        get
        {
            int left = _turnTimeElapsed;
            if (_turnTimerStart != -1)
            {
                int time = PixelFormats.ElapsedMilliseconds;
                left = time >= _turnTimerStart ? left - (time - _turnTimerStart) : 0;
            }

            if (left < 0)
            {
                left = 0;
            }

            return left;
        }
    }

    // AoWHSMap.PAS:3102
    public bool TurnTimerEnabled => TurnTime > 0 && _turnTimerStart != -1;

    // AoWHSMap.PAS:3108
    public void EnableTurnTimer()
    {
        if (_turnTimerStart == -1 && TurnTime > 0)
        {
            _turnTimerStart = PixelFormats.ElapsedMilliseconds;
        }
    }

    // AoWHSMap.PAS:3114
    public void DisableTurnTimer()
    {
        int time = PixelFormats.ElapsedMilliseconds - _turnTimerStart;
        if (time > 0)
        {
            _turnTimeElapsed -= time;
        }

        _turnTimerStart = -1;
    }

    // AoWHSMap.PAS:3124
    public void ZeroTurnTimer() => _turnTimeElapsed = 0;

    // AoWHSMap.PAS:3144
    public void CenterOnLeader(PlayerId? player, bool select, bool animate)
    {
        if (Current.Players.GetPlayers(player).Leader is not null)
        {
            MapCoordinate coordinate32 = Current.Players.GetPlayers(player).Leader.GetLocation();
            int x = coordinate32.X;
            int y = coordinate32.Y;
            int l = coordinate32.MapLevel;
            // { Original excludes X = 0. }
            if (x > 0)
            {
                Current.CenterViewTo(x, y, l);
                if (animate)
                {
                    Current.PlayCenterAnimation(x, y, l);
                }

                ArmyHexSprite army = Current.MapContainer.GetField(x, y, l).GetArmyHexSprite();
                if (army is not null)
                {
                    Selection.UnselectLocation();
                    Selection.SelectLocationHexSprite(army);
                }
            }
        }
    }

    // AoWHSMap.PAS:3167
    public void PlaceTerrain(int X, int Y, int L, int terrainType, int overlayType)
    {
        RandSeed = Current.Random(16777215);
        MapObjectResourceList resources = GamePack.Current.MapObjectResourceList;
        if ((byte)overlayType is not (4 or 5))
        {
            DelphiList<MapObjectResource> candidates = [];
            candidates.Clear();
            for (int i = 0, last = resources.Count - 1; i <= last; i++)
            {
                MapObjectResource resource = resources[i];
                if (resource.EngineClass.ClassId == Hexes.TerrainMapObjectResource.ClassId && resource.TerrainList.UsedTerrain() == 1 && resource.TerrainList.Terrain[0] == terrainType && resource.TerrainList.Overlay[0] == overlayType)
                {
                    _ = candidates.Add(resource);
                }
            }

            if (candidates.Count != 0)
            {
                _ = candidates[Random(candidates.Count)].PlaceResourceUser(Current, X, Y, L);
            }
        }
        else
        {
            _ = GamePack.Current.RoadControl.Place(Current.MapContainer, X, Y, L, terrainType);
        }
    }

    // AoWHSMap.PAS:3195
    public void UpdateEffects()
    {
        if (!Status.Contains(HexSpriteMapStatuses.Editting))
        {
            int i = 0;
            while (i < Effects.Count)
            {
                WorldEffectHexSprite effect = Effects[i];
                if (effect.Done)
                {
                    _ = DynamicFixedVisibleHexSprite.RemoveChild(effect);
                    _ = DynamicVisibleHexSprite.RemoveChild(effect);
                    effect?.Dispose();
                    Effects.RemoveAt(i);
                }
                else
                {
                    i += 1;
                }
            }
        }
    }

    // AoWHSMap.PAS:3219
    public WorldEffectHexSprite AddEffect(WorldEffectType effectType)
    {
        WorldEffectHexSprite effect = new(null) { Holder = CreationHolder() };
        Effects.Add(effect);
        effect.Activate();
        effect.EffectType = effectType;
        _ = DynamicFixedVisibleHexSprite.AddChild(effect);
        return effect;
    }

    public bool ValidateObservation(PlayerId? player, int x, int y, int level, int radius)
    {
        if (!ShowIndependentMoves || player == Current.SeatedPlayer)
        {
            return false;
        }

        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, radius: 0);
        MapLevel? mapLevel = null;
        bool result = false;
        while (walk.Radius <= radius && !result)
        {
            // The original rejects negative coordinates before dereferencing its level pointer.
            // A caster without a map location supplies (-1, -1, -1) with radius zero.
            if (walk.X >= 0 && walk.Y >= 0)
            {
                mapLevel ??= MapContainer.GetMapLevel(level);
                if (walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    MapField field = mapLevel.GetField(walk.X, walk.Y);
                    result = (field.FogCount != 0 || !Current.FogEnabled) &&
                        (field.GetExplored(Current.SeatedPlayer) || !Current.ExplorationEnabled);
                }
            }
            if (!result)
            {
                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
        if (result)
        {
            MakeVisible(walk.X, walk.Y, level, radius: 3);
        }

        return result;
    }

    private static readonly string s_mapSuccessfullyValidatedText = "Map validated successfully";

    private static readonly string s_mapValidationFailedText = "Map validation failed!";

    private static readonly string s_loadingLevelText = "Loading Level ";

    private static readonly string s_initializingLevelText = "Initializing Level ";

    private static readonly string s_updatingTerrainLevelText = "Updating Terrain Level ";

    private static readonly string s_activatingLevelText = "Activating Level ";

    private static readonly string s_savingLevelText = "Saving Level ";

    private static readonly string s_destroyingLevelText = "Destroying Level ";

    private bool TryEndPlayerTurn(PlayerId? player) => player is not null && !Current.Players.GetPlayers(player).GetBusy() && !Current.Players.GetPlayers(player).SingleProcessCycle && AbilityRules.EndPlayerTurn(player, engine: GetEngine());

    private bool CancelPlayerTurn(PlayerId? player)
    {
        if (!Current.Players.GetPlayers(player).GetBusy() && Current.Players.GetPlayers(player).SingleProcessCycle)
        {
            EndTurnTokenEvent eventValue = new(null)
            {
                Holder = CreationHolder(),
                Player = player,
                Day = Current.Day,
                Mode = EndTurnTokenEventMode.CancelEndTurn
            };
            Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_oneMomentPleaseText = "One moment please...";

    public static StrategicMap? Current { get; set; }

    // { Interface declaration run owned by P5. }
    // allied victory. . points. gold. mana. next scenario
    public sealed class MapCampaignSettings(CustomNode? owner) : EngineObject(owner)
    {

        public bool AlliedVictory { get; private set; }
        public int TransferPointsMax { get; private set; } = 1_000;
        public int TransferGoldMax { get; private set; } = 100;
        public int TransferManaMax { get; private set; } = 100;
        public int HeroLevelMax { get; private set; } = -1;

        // Diplomacy.pas:1726
        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            TransferPointsMax = persister.Int32("TransferPointsMax", value: TransferPointsMax);
            TransferGoldMax = persister.Int32("TransferGoldMax", value: TransferGoldMax);
            TransferManaMax = persister.Int32("TransferManaMax", value: TransferManaMax);
            HeroLevelMax = persister.Int32("HeroLevelMax", value: HeroLevelMax);
            AlliedVictory = persister.Boolean("AlliedVictory", value: AlliedVictory);
        }
    }
}

public enum StrategicMapTurnOrderMode : byte
{
    Fixed = 0, Random = 1, Changing = 2
}

public enum StrategicMapVictoryCondition : byte
{
    None = 0, CaptureLocations = 1, AccumulateGold = 2, AccumulateMana = 3
}

public enum StrategicMapGameResult : byte
{
    None = 0, Failed = 1, Succeeded = 2
}

public enum StrategicMapGameStatus : byte
{
    Idle = 0, Playing = 1, Terminated = 2
}

public enum StrategicMapIndependentAIMode : byte
{
    Off = 0, Passive = 1, Normal = 2, Aggressive = 3
}
