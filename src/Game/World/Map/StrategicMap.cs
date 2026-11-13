// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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

using Aow.Game.Magic.Spells;

using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.Units;

using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

// map name, heroes, game mode, stream mode, fixed leaders, allied victory, day limit, turn
// order, day/turn events, event-log locks, diplomacy events, initial seed, random seed, debug
// save, communicator, token log Text, logging enabled
public partial class StrategicMap : GameMap
{

    public new const int ClassId = 131089;

    public SelectedArmy SelectedArmy { get; }
    public bool IgnoreStreamErrors { get; private set; }
    public TargetSelectionControl TargetSelection { get; }

    public string MapName = "";
    private string _author = "";
    public int SphereCount;

    public StringList Description { get; }
    public ItemControl ItemControl { get; private set; }
    public HeroControl HeroControl { get; set; }
    public UnitControl UnitControl { get; set; }
    public StructureControl StructureControl { get; private set; }

    private StrategicMapIndependentAIMode _independentAIMode;
    private readonly StringList _objectives;
    public sbyte HeroCountMax;

    public StartupSongList StartupSongs { get; }
    public SongList GameSongs { get; }

    public StrategicMapGameStatus GameStatus;
    public StrategicMapGameResult GameResult;
    public GameType GameType;

    public CombatMap CombatMap { get; set; }
    public Battle Combat { get; private set; }
    public TutorialControl TutorialControl { get; }
    public AIExecutor AIExecutor { get; set; }
    public AIPlayerActionManager AIPlayerActionManager { get; }
    public AIGroupManager AIGroupManager { get; private set; }
    public HexSpriteMapKeyboard Keyboard { get; }

    public bool UnlimitedMoves;

    public bool RequestCombatType { get; set; }

    public PlayerConnectionType ConnectionType;

    public TextControl TextControl { get; }
    public PlayerList Players { get; set; }
    public RaceList Races { get; set; }

    public bool FixedLeaders;
    public bool LeadersFixed;
    public bool StartingGame;
    public bool AlliedVictory;
    public byte VictoryMode;
    public StrategicMapVictoryCondition VictoryCondition;
    public int VictoryMana;
    public int VictoryGold;
    public int DayLimit;
    public int TurnTime;
    private int _turnTimeElapsed;
    private int _turnTimeBeep;
    private int _turnTimerStart;
    public StrategicMapTurnOrderMode TurnOrderMode;

    public ByteList TurnOrder { get; }

    public int Day;
    private string _sessionName = "";

    internal EventStore<Leader> OnLeaderChangedStore { get; }

    public event NotifyEvent<Leader> LeaderChanged
    {
        add => OnLeaderChangedStore.Add(value);
        remove => OnLeaderChangedStore.Remove(value);
    }

    public bool ShowIndependentMoves { get; set; }
    public MoveControl MoveControl { get; }
    public GlobalMagicControl GlobalMagicControl { get; set; }
    public MapCampaignSettings CampaignSettings { get; }
    public sbyte EditorPlayer { get; }

    public bool ShowAllPlayers;
    private int _relationsChangedLockCount;

    private readonly EventStore<StrategicMap> _playerChangedEventsStore;

    public event NotifyEvent<StrategicMap> PlayerChanged
    {
        add => _playerChangedEventsStore.Add(value);
        remove => _playerChangedEventsStore.Remove(value);
    }

    internal EventStore<PlayerControl> NewDayEventsStore { get; }

    public event NotifyEvent<PlayerControl> DayStarted
    {
        add => NewDayEventsStore.Add(value);
        remove => NewDayEventsStore.Remove(value);
    }

    internal EventStore<sbyte> NewTurnEventsStore { get; }

    public event NotifyEvent<sbyte> TurnStarted
    {
        add => NewTurnEventsStore.Add(value);
        remove => NewTurnEventsStore.Remove(value);
    }

    private readonly EventStore<sbyte> _endTurnEventsStore;

    public event NotifyEvent<sbyte> TurnEnded
    {
        add => _endTurnEventsStore.Add(value);
        remove => _endTurnEventsStore.Remove(value);
    }

    private readonly EventStore<StrategicMap> _currentPlayerChangedEventsStore;

    public event NotifyEvent<StrategicMap> CurrentPlayerChanged
    {
        add => _currentPlayerChangedEventsStore.Add(value);
        remove => _currentPlayerChangedEventsStore.Remove(value);
    }

    private readonly EventStore<StrategicMap> _seatedPlayerChangedEventsStore;

    public event NotifyEvent<StrategicMap> SeatedPlayerChanged
    {
        add => _seatedPlayerChangedEventsStore.Add(value);
        remove => _seatedPlayerChangedEventsStore.Remove(value);
    }

    private readonly EventStore<Battle> _combatEventsStore;

    public event NotifyEvent<Battle> CombatTriggered
    {
        add => _combatEventsStore.Add(value);
        remove => _combatEventsStore.Remove(value);
    }

    private readonly EventStore<sbyte> _playerTerminatedEventsStore;

    public event NotifyEvent<sbyte> PlayerTerminated
    {
        add => _playerTerminatedEventsStore.Add(value);
        remove => _playerTerminatedEventsStore.Remove(value);
    }

    private readonly EventStore<StrategicMap> _gameOverEventsStore;

    public event NotifyEvent<StrategicMap> GameEnded
    {
        add => _gameOverEventsStore.Add(value);
        remove => _gameOverEventsStore.Remove(value);
    }

    private readonly EventStore<StrategicMap> _gameTerminatedEventsStore;

    public event NotifyEvent<StrategicMap> GameTerminated
    {
        add => _gameTerminatedEventsStore.Add(value);
        remove => _gameTerminatedEventsStore.Remove(value);
    }

    public bool ExitPending { get; private set; }

    private readonly EventStore<StrategicMap> _exitEventsStore;

    public event NotifyEvent<StrategicMap> ExitingGame
    {
        add => _exitEventsStore.Add(value);
        remove => _exitEventsStore.Remove(value);
    }

    private int _gameOverLockCount;
    private readonly EventLogList _executeEventLogBuffer;

    private readonly EventStore<ExecuteEventLogNotification> _executeEventLogEventsStore;

    public event NotifyEvent<ExecuteEventLogNotification> ExecutingEventLog
    {
        add => _executeEventLogEventsStore.Add(value);
        remove => _executeEventLogEventsStore.Remove(value);
    }

    private readonly ExecuteEventLogCallbackList _executeEventLogCallBacks;

    public int AILockCount { get; private set; }

    private int _executeEventLogDelay;

    internal EventStore<SpellCaster> CosmeticSurgeryEventsStore { get; }

    public event NotifyEvent<SpellCaster> CosmeticSurgeryCast
    {
        add => CosmeticSurgeryEventsStore.Add(value);
        remove => CosmeticSurgeryEventsStore.Remove(value);
    }

    internal EventStore<AbilityActivation> OnActivateAbilityStore { get; }

    public event NotifyEvent<AbilityActivation> AbilityActivated
    {
        add => OnActivateAbilityStore.Add(value);
        remove => OnActivateAbilityStore.Remove(value);
    }

    internal EventStore<EngineObject> DiplomaticRelationsChangedEventsStore { get; }

    public event NotifyEvent<EngineObject> DiplomaticRelationsChanged
    {
        add => DiplomaticRelationsChangedEventsStore.Add(value);
        remove => DiplomaticRelationsChangedEventsStore.Remove(value);
    }

    private readonly EventStore<StrategicMap> _updateVisibilityEventsStore;

    public event NotifyEvent<StrategicMap> VisibilityUpdated
    {
        add => _updateVisibilityEventsStore.Add(value);
        remove => _updateVisibilityEventsStore.Remove(value);
    }

    public List<WorldEffectHexSprite> Effects { get; }

    private int _updateVisibilityLockCount;
    private bool _visibilityChanged;
    private int _armyUpdateCount;
    private int _structureUpdateCount;
    private int _unitUpdateCount;

    private readonly EventStore<Troop> _unitChangedEventsStore;

    public event NotifyEvent<Troop> UnitChanged
    {
        add => _unitChangedEventsStore.Add(value);
        remove => _unitChangedEventsStore.Remove(value);
    }

    private readonly EventStore<Army> _armyChangedEventsStore;

    public event NotifyEvent<Army> ArmyChanged
    {
        add => _armyChangedEventsStore.Add(value);
        remove => _armyChangedEventsStore.Remove(value);
    }

    private readonly EventStore<Structure> _structureChangedEventsStore;

    public event NotifyEvent<Structure> StructureChanged
    {
        add => _structureChangedEventsStore.Add(value);
        remove => _structureChangedEventsStore.Remove(value);
    }

    private readonly EventStore<Troop> _unitDestroyedEventsStore;

    public event NotifyEvent<Troop> UnitDestroyed
    {
        add => _unitDestroyedEventsStore.Add(value);
        remove => _unitDestroyedEventsStore.Remove(value);
    }

    private readonly EventStore<Army> _armyDestroyedEventsStore;

    public event NotifyEvent<Army> ArmyDestroyed
    {
        add => _armyDestroyedEventsStore.Add(value);
        remove => _armyDestroyedEventsStore.Remove(value);
    }

    private readonly EventStore<Structure> _structureDestroyedEventsStore;

    public event NotifyEvent<Structure> StructureDestroyed
    {
        add => _structureDestroyedEventsStore.Add(value);
        remove => _structureDestroyedEventsStore.Remove(value);
    }

    private readonly EventStore<StrategicMap> _tokenControlActivatedEventsStore;

    public event NotifyEvent<StrategicMap> TokenControlActivated
    {
        add => _tokenControlActivatedEventsStore.Add(value);
        remove => _tokenControlActivatedEventsStore.Remove(value);
    }

    internal EventStore<StrategicMap> CombatMapCreatedEventsStore { get; }

    public event NotifyEvent<StrategicMap> CombatMapCreated
    {
        add => CombatMapCreatedEventsStore.Add(value);
        remove => CombatMapCreatedEventsStore.Remove(value);
    }

    internal EventStore<StrategicMap> CombatMapDestroyedEventsStore { get; }

    public event NotifyEvent<StrategicMap> CombatMapDestroyed
    {
        add => CombatMapDestroyedEventsStore.Add(value);
        remove => CombatMapDestroyedEventsStore.Remove(value);
    }

    public int InitialRandomSeed;
    public int DefendersSeed;
    private int _synchronizeCount;

    public bool DebugSave { get; private set; }

    public PlayMode PlayMode;

    public TokenManager TokenManager { get; set; }
    public PlayerCommunicator PlayerCommunicator { get; }
    public PlayerControl PlayerControl { get; private set; }
    public bool ShowUnitShadows { get; }
    public bool TokenEventLogging { get; private set; }

    // AoWHSMap.PAS:1336
    public override void Dispose()
    {
        StopTokenEventLogging();
        LockMapChangedEvent();
        ClearViewLevelHandler();
        ClearCenterViewHandler();
        GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "GENERAL.ILB");
        GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "CURSORS.ILB");
        GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "COMBAT.ILB");
        MoveControl?.Dispose();
        for (int I = Effects.Count - 1; I >= 0; I--)
        {
            var animation = Effects[I];
            DynamicFixedVisibleHexSprite.RemoveChild((EngineObject)animation);
            var fEffectsItem = Effects[I];
            DynamicVisibleHexSprite.RemoveChild((EngineObject)fEffectsItem);
            (Effects[I] as IDisposable)?.Dispose();
        }

        Effects?.Clear();
        TutorialControl?.Dispose();
        Players.BeginUpdate();
        _armyChangedEventsStore.Clear();
        _structureChangedEventsStore.Clear();
        _unitChangedEventsStore.Clear();
        _seatedPlayerChangedEventsStore.Clear();
        _currentPlayerChangedEventsStore.Clear();
        NewTurnEventsStore.Clear();
        NewDayEventsStore.Clear();
        OnLeaderChangedStore.Clear();
        Description.Clear();
        _objectives.Clear();
        TurnOrder.Clear();
        _structureDestroyedEventsStore.Clear();
        _unitDestroyedEventsStore.Clear();
        CombatMapCreatedEventsStore.Clear();
        CombatMapDestroyedEventsStore.Clear();
        _updateVisibilityEventsStore.Clear();
        DiplomaticRelationsChangedEventsStore.Clear();
        CosmeticSurgeryEventsStore.Clear();
        OnActivateAbilityStore.Clear();
        _tokenControlActivatedEventsStore.Clear();
        _playerChangedEventsStore.Clear();
        _exitEventsStore.Clear();
        Mouse.OnMove -= MouseMoved;
        if (Combat != null)
        {
            DestroyCombat();
        }

        if (CombatMap != null)
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
        _seatedPlayerChangedEventsStore.Clear();
        _executeEventLogBuffer?.Dispose();
        NewTurnEventsStore.Clear();
        _endTurnEventsStore.Clear();
        _currentPlayerChangedEventsStore.Clear();
        NewDayEventsStore.Clear();
        _armyChangedEventsStore.Clear();
        _structureChangedEventsStore.Clear();
        _unitChangedEventsStore.Clear();
        _armyDestroyedEventsStore.Clear();
        _structureDestroyedEventsStore.Clear();
        _unitDestroyedEventsStore.Clear();
        OnLeaderChangedStore.Clear();
        Description?.Dispose();
        _objectives?.Dispose();
        TurnOrder?.Dispose();
        PlayerControl?.Dispose();
        TextControl?.Dispose();
        _combatEventsStore.Clear();
        _playerChangedEventsStore.Clear();
        _executeEventLogEventsStore.Clear();
        _executeEventLogCallBacks?.Dispose();
        CosmeticSurgeryEventsStore.Clear();
        OnActivateAbilityStore.Clear();
        GameSongs?.Dispose();
        StartupSongs?.Dispose();
        _exitEventsStore.Clear();
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
        _playerTerminatedEventsStore.Clear();
        _gameOverEventsStore.Clear();
        _gameTerminatedEventsStore.Clear();
        TargetSelection?.Dispose();
        CombatMapCreatedEventsStore.Clear();
        CombatMapDestroyedEventsStore.Clear();
        DiplomaticRelationsChangedEventsStore.Clear();
        Keyboard?.Dispose();
        _updateVisibilityEventsStore.Clear();
        _tokenControlActivatedEventsStore.Clear();
        UnlockMapChangedEvent();
    }

    // AoWHSMap.PAS:2160
    public override void ReadWrite(StorageStream stream)
    {
        int X = default;
        int Y = default;
        int L = default;
        if (stream.StorageMode().Contains(StorageModeElement.Read) && !stream.StorageMode().Contains(StorageModeElement.Translate))
        {
            stream.ShowProgress(this, AbilityRules.TranslateResourceString(GetEngine(), s_oneMomentPleaseText), 0);
        }

        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Write, StorageModeElement.TextOnly)) == Set<StorageModeElement>.Of(StorageModeElement.Write))
        {
            ResetModified();
        }

        IgnoreStreamErrors = false;
        if (Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            stream.SetErrorHandler(OnStreamError);
        }

        stream.Bytes(Bytes.Of(ref ConnectionType, 1), 1, 31);
        if (ConnectionType == PlayerConnectionType.Network)
        {
            X = _viewPixelX;
            _viewPixelX = 0;
            Y = _viewPixelY;
            _viewPixelY = 0;
            L = _viewMapLevel;
            _viewMapLevel = 0;
        }
        else
        {
            X = 0;
            Y = 0;
            L = 0;
        }

        base.ReadWrite(stream);
        if (ConnectionType == PlayerConnectionType.Network)
        {
            _viewPixelX = X;
            _viewPixelY = Y;
            _viewMapLevel = L;
        }

        stream.String(ref _author, 7);
        stream.Lines(Description, 11);
        stream.Lines(_objectives, 12);
        stream.Bytes(Bytes.Of(ref _independentAIMode, 1), 1, 13);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref CurrentPlayer), 16);
        stream.Bytes(Bytes.Of(ref TurnOrderMode, 1), 1, 17);
        stream.Payload(18, TurnOrder.LoadFromStream, TurnOrder.SaveToStream);
        stream.Int32(ref Day, 19);
        stream.String(ref _sessionName, 20);
        stream.Object(Players, 21);
        stream.Object(Races, 22);
        stream.Boolean(ref ExplorationEnabled, 23);
        if (!stream.StorageMode().Contains(StorageModeElement.Preview))
        {
            stream.Object(HeroControl, 24);
        }

        stream.String(ref MapName, 25);
        stream.Boolean(ref FogEnabled, 26);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref HeroCountMax), 27);
        stream.Int32(ref DefendersSeed, 28);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly) && !stream.StorageMode().Contains(StorageModeElement.Preview) && !stream.StorageMode().Contains(StorageModeElement.Compare))
        {
            stream.Object(StartupSongs, 29);
        }

        stream.Object(ItemControl, 30);
        stream.Bytes(Bytes.Of(ref GameStatus, 1), 1, 34);
        stream.Bytes(Bytes.Of(ref GameResult, 1), 1, 35);
        stream.Int32(ref InitialRandomSeed, 36);
        stream.Bytes(Bytes.Of(ref GameType, 1), 1, 37);
        stream.Object(UnitControl, 38);
        stream.Bytes(Bytes.Of(ref PlayMode, 1), 1, 39);
        stream.Object(GlobalMagicControl, 40);
        stream.Int32(ref SphereCount, 5, 41);
        stream.Object(TextControl, 42);
        stream.Object(CampaignSettings, 43);
        // { Original stores the hero control twice. }
        stream.Object(HeroControl, 44);
        stream.Bytes(Bytes.Of(ref VictoryCondition, 1), 1, 45);
        stream.Int32(ref VictoryMana, 46);
        stream.Int32(ref VictoryGold, 47);
        stream.Boolean(ref FixedLeaders, 48);
        stream.Boolean(ref AlliedVictory, 49);
        stream.Object(AIGroupManager, 50);
        stream.Int32(ref DayLimit, 51);
        stream.Int32(ref TurnTime, 52);
        stream.Boolean(ref UnlimitedMoves, 53);
        stream.Object(StructureControl, 54);
        if (stream.StorageMode().Contains(StorageModeElement.Read) || ConnectionType != PlayerConnectionType.Network)
        {
            stream.Int32(ref _turnTimeElapsed, 55);
        }

        stream.Object(TutorialControl, 56);
        if (stream.StorageMode().Contains(StorageModeElement.Read) || ConnectionType != PlayerConnectionType.Network)
        {
            stream.Int32(ref _turnTimeBeep, 58);
        }

        stream.Boolean(ref LeadersFixed, 59);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly) && !stream.StorageMode().Contains(StorageModeElement.Preview) && ConnectionType != PlayerConnectionType.Network)
        {
            stream.Object(GameSongs, 60);
        }

        stream.Boolean(ref StartingGame, 61);
        stream.Boolean(ref ShowAllPlayers, 62);
        stream.Bytes(Bytes.Of(ref VictoryMode, 1), 1, 63);
        RandSeed = DefendersSeed;
    }

    // The tree form of ReadWrite. A load shows its progress to the load's handler where ReadWrite
    // calls Stream.ShowProgress. The editor's error handler is the one its caller passes to the
    // tree load (persistence-tree.md 7), so none is set here. A preview leaves the hero control and
    // the startup songs unread, a network map leaves the game songs unread, and ReadWrite never
    // reads UnknownProperty57, so a load skips those members as ReadWrite does.
    public override void PersistFields(IPersister persister)
    {
        if (persister.Loading)
        {
            PersistSupport.ShowProgress(persister, sender: this, message: AbilityRules.TranslateResourceString(GetEngine(), s_oneMomentPleaseText), percent: 0);
        }

        if (!persister.Loading)
        {
            ResetModified();
        }

        IgnoreStreamErrors = false;
        ConnectionType = persister.Enum("ConnectionType", value: ConnectionType);
        int x;
        int y;
        int level;
        if (ConnectionType == PlayerConnectionType.Network)
        {
            (x, y, level) = (_viewPixelX, _viewPixelY, _viewMapLevel);
            (_viewPixelX, _viewPixelY, _viewMapLevel) = (0, 0, 0);
        }
        else
        {
            (x, y, level) = (0, 0, 0);
        }

        base.PersistFields(persister);
        if (ConnectionType == PlayerConnectionType.Network)
        {
            (_viewPixelX, _viewPixelY, _viewMapLevel) = (x, y, level);
        }

        _author = persister.String("Author", value: _author);
        PersistSupport.Lines(persister, name: "Description", lines: Description);
        PersistSupport.Lines(persister, name: "Objectives", lines: _objectives);
        _independentAIMode = persister.Enum("IndependentAIMode", value: _independentAIMode);
        CurrentPlayer = PersistSupport.UnsignedByte(persister, name: "CurrentPlayer", value: CurrentPlayer);
        TurnOrderMode = persister.Enum("TurnOrderMode", value: TurnOrderMode);
        PersistSupport.ByteList(persister, name: "TurnOrder", list: TurnOrder);
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
        HeroCountMax = PersistSupport.UnsignedByte(persister, name: "HeroCountMax", value: HeroCountMax);
        DefendersSeed = persister.Int32("DefendersSeed", value: DefendersSeed);
        if (!persister.Preview)
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
        if (!persister.Preview && ConnectionType != PlayerConnectionType.Network)
        {
            persister.Object("GameSongs", objectValue: GameSongs);
        }
        else
        {
            persister.Skip("GameSongs");
        }

        StartingGame = persister.Boolean("StartingGame", value: StartingGame);
        ShowAllPlayers = persister.Boolean("ShowAllPlayers", value: ShowAllPlayers);
        VictoryMode = persister.RawByte("VictoryMode", value: VictoryMode);
        persister.Skip("UnknownProperty57");
        RandSeed = DefendersSeed;
    }

    // AoWHSMap.PAS:2241
    public override void MainReadWrite(StorageStream stream)
    {
        string fileName;
        base.MainReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Preview, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read) && Day == 0)
        {
            if (GameType == GameType.Campaign)
            {
                fileName = GetEngine().StartupDirectory + "Dict\\" + "CampMaps" + ".mld";
            }
            else
            {
                fileName = GetEngine().StartupDirectory + "Dict\\" + "CustMaps" + ".mld";
            }

            if (FileExists(fileName))
            {
                GetEngine().TranslateEngineObjectFromFile(this, fileName);
            }
        }
    }

    public override void MainPersist(IPersister persister)
    {
        string fileName;
        base.MainPersist(persister);
        if (persister.Loading && !persister.Preview && Day == 0)
        {
            if (GameType == GameType.Campaign)
            {
                fileName = GetEngine().StartupDirectory + "Dict\\" + "CampMaps" + ".mld";
            }
            else
            {
                fileName = GetEngine().StartupDirectory + "Dict\\" + "CustMaps" + ".mld";
            }

            if (FileExists(fileName))
            {
                GetEngine().TranslateEngineObjectFromFile(this, fileName);
            }
        }
    }

    // AoWHSMap.PAS:1238
    public StrategicMap(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        ItemControl = new ItemControl(null) { Holder = CreationHolder() };
        TokenManager = new TokenManager(GetEngine());
        TokenManager.TokenExecutor.OnActivateTacticalBattle += TokenControlActivatedEvent;
        TokenManager.TokenExecutor.SetExecuteTokenEventHandler(OnExecuteTokenEvent);
        TokenManager.OnMessage += OnTokenManagerMessage;
        PlayerCommunicator = new PlayerCommunicator(null) { Holder = CreationHolder() };
        PlayerCommunicator.Engine = GetEngine();
        ConnectionType = PlayerConnectionType.Local;
        AIExecutor = new AIExecutor(null) { Holder = CreationHolder() };
        AIGroupManager = new AIGroupManager(null) { Holder = CreationHolder() };
        AIPlayerActionManager = new AIPlayerActionManager(null) { Holder = CreationHolder() };
        AIPlayerActionManager.RegisterClasses([AIPlayerActionClass.Of<EndTurnAIPlayerAction>(), AIPlayerActionClass.Of<GroupControlAIPlayerAction>(), AIPlayerActionClass.Of<GroupControlLoopAIPlayerAction>(), AIPlayerActionClass.Of<InitializeAIPlayerAction>(), AIPlayerActionClass.Of<BudgetManagerAIPlayerAction>(), AIPlayerActionClass.Of<AutoProduceAIPlayerAction>(), AIPlayerActionClass.Of<DiplomacyAIPlayerAction>(), AIPlayerActionClass.Of<ProcessEventLogAIPlayerAction>(), AIPlayerActionClass.Of<ResearchSpellAIPlayerAction>(), AIPlayerActionClass.Of<CastSpellAIPlayerAction>(), AIPlayerActionClass.Of<StructurePhase1AIPlayerAction>(), AIPlayerActionClass.Of<StructurePhase2AIPlayerAction>()]);
        TokenManager.TokenExecutor.OnEventsExecuted += AIExecutor.TokenControlDoneEvent;
        AIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<HuntAIGroup>()]);
        PlayMode = PlayMode.None;
        Keyboard = new HexSpriteMapKeyboard(null) { Holder = CreationHolder() };
        TargetSelection = new TargetSelectionControl();
        Effects = new List<WorldEffectHexSprite>();
        DefendersSeed = 0;
        HeroCountMax = 5;
        SelectedArmy = new SelectedArmy(this);
        ((GameMapMouse)Mouse).RectangleSelected += SelectedArmy.RectangleSelectEvent;
        MoveControl = new MoveControl(this);
        GlobalMagicControl = new GlobalMagicControl(null) { Holder = CreationHolder() };
        MapName = "noname";
        _author = "";
        _turnTimerStart = -1;
        SphereCount = 5;
        VictoryGold = 5000;
        VictoryMana = 5000;
        Description = new StringList();
        _objectives = new StringList();
        _independentAIMode = StrategicMapIndependentAIMode.Normal;
        Players = new PlayerList(null) { Holder = CreationHolder() };
        Races = new RaceList(null) { Holder = CreationHolder() };
        _playerChangedEventsStore = new();
        FixedLeaders = true;
        AlliedVictory = false;
        CurrentPlayer = 0;
        _executeEventLogBuffer = new EventLogList(null) { Holder = CreationHolder() };
        DebugSave = false;
        TutorialControl = new TutorialControl(null) { Holder = CreationHolder() };
        TurnOrderMode = StrategicMapTurnOrderMode.Fixed;
        TurnOrder = new ByteList();
        Day = 0;
        GameStatus = StrategicMapGameStatus.Idle;
        GameResult = StrategicMapGameResult.None;
        _sessionName = "";
        var detailLevel = GetEngine().Registry.GetDetailLevel();
        ShowUnitShadows = Set<DetailLevel>.Of(DetailLevel.Normal, DetailLevel.High).Contains(detailLevel);
        HeroControl = new HeroControl(null) { Holder = CreationHolder() };
        UnitControl = new UnitControl(null) { Holder = CreationHolder() };
        StructureControl = new StructureControl(null) { Holder = CreationHolder() };
        OnLeaderChangedStore = new();
        TextControl = new TextControl(null) { Holder = CreationHolder() };
        NewDayEventsStore = new();
        NewTurnEventsStore = new();
        _endTurnEventsStore = new();
        _currentPlayerChangedEventsStore = new();
        _seatedPlayerChangedEventsStore = new();
        _armyChangedEventsStore = new();
        _structureChangedEventsStore = new();
        _tokenControlActivatedEventsStore = new();
        _unitChangedEventsStore = new();
        _armyDestroyedEventsStore = new();
        _structureDestroyedEventsStore = new();
        _unitDestroyedEventsStore = new();
        _combatEventsStore = new();
        _executeEventLogEventsStore = new();
        UnitChanged += TargetSelection.UnitChanged;
        ArmyDestroyed += SelectedArmy.OnDestroyArmy;
        _executeEventLogCallBacks = new ExecuteEventLogCallbackList();
        CosmeticSurgeryEventsStore = new();
        OnActivateAbilityStore = new();
        StartupSongs = new StartupSongList(null) { Holder = CreationHolder() };
        GameSongs = new SongList(null) { Holder = CreationHolder() };
        _playerTerminatedEventsStore = new();
        _gameOverEventsStore = new();
        _gameTerminatedEventsStore = new();
        DiplomaticRelationsChangedEventsStore = new();
        _updateVisibilityEventsStore = new();
        CombatMapCreatedEventsStore = new();
        CombatMapDestroyedEventsStore = new();
        _exitEventsStore = new();
        CampaignSettings = new MapCampaignSettings(null) { Holder = CreationHolder() };
        Mouse.OnMove += MouseMoved;
    }

    // AoWHSMap.PAS:2826
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
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
                _updateVisibilityEventsStore.Raise(this);
            }
        }
    }

    // AoWHSMap.PAS:2853
    public override void SendMessage(ref EngineMessage message, EngineObject sender)
    {
        base.SendMessage(ref message, sender);
        if (GlobalMagicControl != null)
        {
            GlobalMagicControl.MessageProc(ref message, sender);
        }

        Players.MessageProc(ref message, sender);
        Races.MessageProc(ref message, sender);
        ItemControl.MessageProc(ref message, sender);
        HeroControl.MessageProc(ref message, sender);
        AIGroupManager.MessageProc(ref message, sender);
        if (message.MessageId == 537001986 && message.Payload.Int == Current.SeatedPlayer)
        {
            if (SelectedArmy.GetArmyHexSprite() != null)
            {
                SelectedArmy.Update();
            }
        }
    }

    // AoWHSMap.PAS:1549
    public override HexSpriteMapMouse CreateMouse() => new AowHexSpriteMapMouse();

    // AoWHSMap.PAS:2116
    public override void NotifyProgress(in MapProgress progress)
    {
        string defaultText;
        string text = "";
        int percent = default;
        int total = default;
        if (HasProgressHandlers)
        {
            defaultText = AbilityRules.TranslateResourceString(GetEngine(), s_oneMomentPleaseText);
            switch (progress.ProgressType)
            {
                case MapProgressType.Loading:
                    total = progress.Percent + progress.Level * 100;
                    percent = Round((double)((double)total / 3) / MapContainer.LevelCount);
                    if (GetEngine().Debug)
                    {
                        text = AbilityRules.TranslateResourceString(GetEngine(), s_loadingLevelText) + IntToStr(progress.Level);
                    }
                    else
                    {
                        text = defaultText;
                    }

                    RaiseProgress(this, text, percent);
                    break;
                case MapProgressType.Initializing:
                    total = progress.Percent + progress.Level * 100;
                    percent = Round((double)((double)((double)total / 4.5) / MapContainer.LevelCount) + (double)100 / 3);
                    if (GetEngine().Debug)
                    {
                        text = AbilityRules.TranslateResourceString(GetEngine(), s_initializingLevelText) + IntToStr(progress.Level);
                    }
                    else
                    {
                        text = defaultText;
                    }

                    RaiseProgress(this, text, percent);
                    break;
                case MapProgressType.UpdatingTerrain:
                    total = progress.Percent + progress.Level * 100;
                    percent = Round((double)((double)((double)total / 4.5) / MapContainer.LevelCount) + (double)500 / 9);
                    if (GetEngine().Debug)
                    {
                        text = AbilityRules.TranslateResourceString(GetEngine(), s_updatingTerrainLevelText) + IntToStr(progress.Level);
                    }
                    else
                    {
                        text = defaultText;
                    }

                    RaiseProgress(this, text, percent);
                    break;
                case MapProgressType.Activating:
                    total = progress.Percent + progress.Level * 100;
                    percent = Round((double)((double)((double)total / 4.5) / MapContainer.LevelCount) + (double)700 / 9);
                    if (GetEngine().Debug)
                    {
                        text = AbilityRules.TranslateResourceString(GetEngine(), s_activatingLevelText) + IntToStr(progress.Level);
                    }
                    else
                    {
                        text = defaultText;
                    }

                    RaiseProgress(this, text, percent);
                    break;
                case MapProgressType.Saving:
                    var savingLevel = AbilityRules.TranslateResourceString(GetEngine(), s_savingLevelText);
                    RaiseProgress(this, savingLevel + IntToStr(progress.Level), progress.Percent);
                    break;
                case MapProgressType.Destroying:
                    var translatedText = AbilityRules.TranslateResourceString(GetEngine(), s_destroyingLevelText);
                    RaiseProgress(this, translatedText + IntToStr(progress.Level), progress.Percent);
                    break;
            }
        }
    }

    // AoWHSMap.PAS:2290
    public override void InitializeNewMap()
    {
        EngineObject objectValue;
        Races.Update();
        Players.SetNumberOfPlayers(2);
        Players.ValidateRaces(Races);
        for (int L = 0, last = MapContainer.LevelCount - 1; L <= last; L++)
        {
            InitializeMapLevel((sbyte)L);
        }

        for (int I = 0, last = GamePack.Current.ItemLibraries.SystemLibrary.Count - 1; I <= last; I++)
        {
            objectValue = GamePack.Current.ItemLibraries.SystemLibrary.GetItem(I).Copy(GetEngine());
            ItemControl.RegisterItem((Item)objectValue);
            objectValue.Release();
        }

        for (int I = 0, last = GamePack.Current.HeroLibraries.SystemLibrary.Count - 1; I <= last; I++)
        {
            objectValue = GamePack.Current.HeroLibraries.SystemLibrary.GetHero(I).Copy(GetEngine());
            HeroControl.RegisterHero((Hero)objectValue);
            objectValue.Release();
        }
    }

    // AoWHSMap.PAS:2315
    public override bool AddMapLevel()
    {
        bool result;
        if (MapContainer.LevelCount < 3 && MapContainer.AddLevel(131091))
        {
            InitializeMapLevel((sbyte)(MapContainer.LevelCount - 1));
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWHSMap.PAS:2654
    public override void Activate()
    {
        SynchronizeBegin();
        if (Day == 0 || GameSongs.Songs.Count == 0)
        {
            GameSongs.Songs.Clear();
            for (int I = 0, last = Current.StartupSongs.Songs.Count - 1; I <= last; I++)
            {
                var fileNotFound = AbilityRules.TranslateResourceString(GetEngine(), GameText.FileNotFoundText);
                if (CompareText(Current.StartupSongs.SongNames[I], fileNotFound) != 0)
                {
                    var item = Current.StartupSongs.Songs[I];
                    GameSongs.Songs.Add(item);
                }
            }
        }

        GameStatus = StrategicMapGameStatus.Playing;
        DebugSave = GetEngine().Registry.GetAutoDebugSave();
        GlobalMagicControl.Update();
        Players.BeginUpdate();
        Players.EndUpdate();
        for (int I = 0, last = Players.Count - 1; I <= last; I++)
        {
            if (Players.GetPlayers(I).Behavior == PlayerBehavior.Random)
            {
                var random = Current.Random(5);
                Players.GetPlayers(I).Behavior = (PlayerBehavior)random;
            }
        }

        if (PlayerControl == null)
        {
            if (PlayMode == PlayMode.None)
            {
                PlayMode = PlayMode.Turn;
            }

            if (Players.LocalHumanPlayers.Count > 1)
            {
                PlayMode = PlayMode.Turn;
            }

            SetupPlayerControl(PlayMode, ConnectionType);
        }

        TokenManager.Initialize(PlayerCommunicator);
        base.Activate();
        if (!Status.Contains(HexSpriteMapStatusElement.Editting))
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
    public override int ChangeTerrain(EngineObject mapObject, int X, int Y, int L, int radius, int terrainType)
    {
        RandSeed = Current.Random(16777215);
        return base.ChangeTerrain(mapObject, X, Y, L, radius, terrainType);
    }

    // AoWHSMap.PAS:3137
    public override int ChangeTerrain(EngineObject mapObject, int X, int Y, int L, int radius, ChangeTerrainProc changeTerrainProc, TerrainChangedProc terrainChangedProc)
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
        TargetSelection.ShowSelection(_viewPixelX, _viewPixelY, lockedSurface);
        if (((GameMapMouse)Mouse).SelectionRectangle.Left != -1)
        {
            DrawSelectionRectangle(_viewPixelX, _viewPixelY, ((GameMapMouse)Mouse).SelectionRectangle, lockedSurface);
        }
    }

    // AoWHSMap.PAS:2568
    public override void NewFrame()
    {
        CombatMap combatMap;
        base.NewFrame();
        if (Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            PlayerControl.NewFrame();
            PlayerCommunicator.Process();
            if (CombatMap == null && GameStatus != StrategicMapGameStatus.Terminated)
            {
                AIExecutor.Process();
            }

            TokenManager.Process();
            UpdateCursor();
            UpdateExecuteEventLogDelay();
            UpdateTurnTimeBeep();
            UpdateEffects();
            combatMap = CombatMap;
            if (combatMap != null)
            {
                combatMap.NewFrame();
            }
        }
    }

    // AoWHSMap.PAS:1554
    public override void SetSeatedPlayer(sbyte player)
    {
        PlayerControl.SetSeatedPlayer(player);
    }

    // AoWHSMap.PAS:1887
    public virtual void TriggerCombatEvent(Battle combat)
    {
        _combatEventsStore.Raise(combat);
    }

    // AoWHSMap.PAS:1892
    public virtual void TriggerGameTerminatedEvent()
    {
        GameStatus = StrategicMapGameStatus.Terminated;
        _gameOverEventsStore.Raise(this);
    }

    // AoWHSMap.PAS:2012
    public virtual void TriggerPlayerTerminatedEvent(int player)
    {
        _playerTerminatedEventsStore.Raise((sbyte)player);
    }

    // AoWHSMap.PAS:2586
    public virtual void StartNewMap()
    {
        TurnOrder.Clear();
        if (TurnOrderMode == StrategicMapTurnOrderMode.Fixed)
        {
            for (int I = 0, last = Players.Count - 1; I <= last; I++)
            {
                TurnOrder.Add((byte)I);
            }
        }

        if (TurnOrderMode == StrategicMapTurnOrderMode.Random)
        {
            GenerateNewTurnOrder();
        }

        for (int I = 0, last = Players.Count - 1; I <= last; I++)
        {
            var players = Players.GetPlayers(I);
            Players.GetPlayers(I).SetGems(players.InitialGold);
            var fPlayersPlayers = Players.GetPlayers(I);
            Players.GetPlayers(I).MagicControl.SetMana(fPlayersPlayers.MagicControl.ExternalMana);
            switch (Players.GetPlayers(I).PlayerType)
            {
                case PlayerType.Cpu4:
                    var players2 = Players.GetPlayers(I);
                    Players.GetPlayers(I).SetGems(players2.Gems + 250);
                    var players3 = Players.GetPlayers(I);
                    Players.GetPlayers(I).MagicControl.SetMana(players3.MagicControl.Mana + 250);
                    break;
                case PlayerType.Cpu5:
                    var players4 = Players.GetPlayers(I);
                    Players.GetPlayers(I).SetGems(players4.Gems + 500);
                    var players5 = Players.GetPlayers(I);
                    Players.GetPlayers(I).MagicControl.SetMana(players5.MagicControl.Mana + 500);
                    break;
            }
        }

        PlayerControl.StartNewMap();
    }

    // AoWHSMap.PAS:2615
    public virtual void StartLoadedMap()
    {
        PlayerControl.StartLoadedMap();
    }

    // AoWHSMap.PAS:1996
    public virtual void TriggerExecuteEventLog(EventLog log, bool immediate, ExecuteDoneEvent done)
    {
        var notification = new ExecuteEventLogNotification();
        ref var info = ref notification.Info;
        info.Log = log;
        info.Done = done;
        info.Immediate = immediate;
        if (!immediate)
        {
            log.AddRef();
            _executeEventLogCallBacks.Add(info);
            if (AILockCount == 0)
            {
                ExecuteNextEventLog();
            }
        }
        else
        {
            _executeEventLogEventsStore.Raise(notification);
        }
    }

    // AoWHSMap.PAS:1476
    public void SynchronizeBegin()
    {
        _synchronizeCount += 1;
    }

    // AoWHSMap.PAS:1481
    public void SynchronizeEnd()
    {
        _synchronizeCount -= 1;
    }

    // AoWHSMap.PAS:1486
    public bool GetSynchronized()
    {
        bool result;
        if (_synchronizeCount == 0 && !TokenManager.TokenExecutor.Executing && !Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // AoWHSMap.PAS:1509
    public void PlayerDisconnected(sbyte player)
    {
        PlayerControl.PlayerDisconnected(player);
    }

    // AoWHSMap.PAS:1514
    public void PlayerConnectionLost()
    {
        PlayerControl.ConnectionLost();
    }

    // AoWHSMap.PAS:1525
    public StrategicMapIndependentAIMode GetIndependentAIMode()
    {
        StrategicMapIndependentAIMode result = _independentAIMode;
        if (!Current.Status.Contains(HexSpriteMapStatusElement.Editting) && Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentType.FireMastery))
        {
            if (result == StrategicMapIndependentAIMode.Off)
            {
                result = StrategicMapIndependentAIMode.Normal;
            }
            else
            {
                result = StrategicMapIndependentAIMode.Aggressive;
            }
        }

        return result;
    }

    // AoWHSMap.PAS:1534
    public bool SaveGame(string fileName)
    {
        bool result;
        SaveGameTokenEvent eventValue;
        if (Current.SeatedPlayer != -1)
        {
            eventValue = new SaveGameTokenEvent(null) { Holder = CreationHolder() };
            eventValue.FileName = ExpandFileName(fileName);
            Current.TokenManager.GetTokenControl(Current.SeatedPlayer).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWHSMap.PAS:1559
    public bool TerrainVisible(sbyte X, sbyte Y, sbyte L, int radius)
    {
        MapLevel mapLevel;
        MapField field;
        HexNumberWalkData walk = default;
        // { Original returns true immediately when exploration is enabled. }
        bool result = Current.ExplorationEnabled;
        mapLevel = MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
        while (walk.Radius <= radius && !result)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                field = mapLevel.GetField(walk.X, walk.Y);
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
    public bool Visible(sbyte X, sbyte Y, sbyte L, int radius)
    {
        MapLevel mapLevel;
        MapField field;
        HexNumberWalkData walk = default;
        bool result = false;
        mapLevel = MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
        while (walk.Radius <= radius && !result)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                field = mapLevel.GetField(walk.X, walk.Y);
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
    public bool WatchingTerrain(sbyte X, sbyte Y, sbyte L, int radius)
    {
        MapLevel mapLevel;
        MapField field;
        HexNumberWalkData walk = default;
        bool result = false;
        mapLevel = MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
        while (walk.Radius <= radius && !result)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                field = mapLevel.GetField(walk.X, walk.Y);
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
    public bool Watching(sbyte X, sbyte Y, sbyte L, int radius)
    {
        MapLevel mapLevel;
        MapField field;
        HexNumberWalkData walk = default;
        bool result = false;
        mapLevel = MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
        while (walk.Radius <= radius && !result)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                field = mapLevel.GetField(walk.X, walk.Y);
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
    public bool ValidateTerrainObservation(sbyte player, sbyte X, sbyte Y, sbyte L, int radius)
    {
        bool result;
        MapLevel mapLevel;
        MapField field;
        HexNumberWalkData walk = default;
        int wx;
        if (ShowIndependentMoves && player != Current.SeatedPlayer)
        {
            result = false;
            mapLevel = MapContainer.GetMapLevel(L);
            HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
            while (walk.Radius <= radius && !result)
            {
                wx = walk.X;
                if (wx >= 0 && walk.Y >= 0 && wx < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    field = mapLevel.GetField(wx, walk.Y);
                    result = field.GetExplored(Current.SeatedPlayer) || !Current.ExplorationEnabled;
                }

                if (!result)
                {
                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }

            if (result)
            {
                MakeVisible(walk.X, walk.Y, L, 3);
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWHSMap.PAS:1704
    public void UnlockGameOverEventCallback()
    {
        UnlockGameOverEvent();
    }

    // AoWHSMap.PAS:1709
    public bool SelectNextHero()
    {
        Player player;
        int I;
        int left;
        Army army;
        int pixelX = default;
        int pixelY = default;
        player = Players.GetPlayers(SeatedPlayer);
        if (player.Armies.Count != 0)
        {
            left = player.Armies.Count;
            I = player.HeroJoinDay;
            if (I < 0 || I >= player.Armies.Count)
            {
                I = 0;
            }

            while (true)
            {
                I += 1;
                left -= 1;
                if (I == player.Armies.Count)
                {
                    I = 0;
                }

                if (left == 0 || player.Armies.GetArmy(I).Hero(255, Set<HeroType>.Of(HeroType.Hero, HeroType.Leader, HeroType.HeroLeader, HeroType.Merchant)) != null)
                    break;
            }

            if (player.Armies.GetArmy(I).Hero(255, Set<HeroType>.Of(HeroType.Hero, HeroType.Leader, HeroType.HeroLeader, HeroType.Merchant)) != null)
            {
                army = Players.GetPlayers(SeatedPlayer).Armies.GetArmy(I);
                CenterViewTo(army.X, army.Y, army.MapLevel);
                Selection.UnselectLocation();
                HexGeometry.HexToPixel(army.X, army.Y, out pixelX, out pixelY);
                Selection.SelectLocation(pixelX, pixelY, army.MapLevel);
                player.HeroJoinDay = I;
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
        Player player;
        int I;
        int left;
        Army army;
        int pixelX = default;
        int pixelY = default;
        player = Players.GetPlayers(SeatedPlayer);
        if (player.Armies.Count != 0)
        {
            left = player.Armies.Count;
            I = player.BaseHeroJoinDay;
            if (I < 0 || I >= player.Armies.Count)
            {
                I = 0;
            }

            while (true)
            {
                I += 1;
                left -= 1;
                if (I == player.Armies.Count)
                {
                    I = 0;
                }

                if (left == 0 || player.Armies.GetArmy(I).MovePoints(255) > 1 && (player.Armies.GetArmy(I).UserMode & Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard, ArmyUserModeElement.TurnDone)) == Set<ArmyUserModeElement>.Of())
                    break;
            }

            if (player.Armies.GetArmy(I).MovePoints(255) > 1 && (player.Armies.GetArmy(I).UserMode & Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard, ArmyUserModeElement.TurnDone)) == Set<ArmyUserModeElement>.Of())
            {
                army = Players.GetPlayers(SeatedPlayer).Armies.GetArmy(I);
                CenterViewTo(army.X, army.Y, army.MapLevel);
                Selection.UnselectLocation();
                HexGeometry.HexToPixel(army.X, army.Y, out pixelX, out pixelY);
                Selection.SelectLocation(pixelX, pixelY, army.MapLevel);
                player.BaseHeroJoinDay = I;
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
        Player player;
        int I;
        int left;
        Army army;
        int pixelX = default;
        int pixelY = default;
        player = Players.GetPlayers(SeatedPlayer);
        if (player.Armies.Count != 0)
        {
            I = player.BaseHeroJoinDay;
            if (I < 0 || I >= player.Armies.Count)
            {
                I = 0;
            }

            left = player.Armies.Count;
            while (true)
            {
                I -= 1;
                left -= 1;
                if (I < 0)
                {
                    I = player.Armies.Count - 1;
                }

                if (left == 0 || player.Armies.GetArmy(I).MovePoints(255) > 1 && (player.Armies.GetArmy(I).UserMode & Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard, ArmyUserModeElement.TurnDone)) == Set<ArmyUserModeElement>.Of())
                    break;
            }

            if (player.Armies.GetArmy(I).MovePoints(255) > 1 && (player.Armies.GetArmy(I).UserMode & Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard, ArmyUserModeElement.TurnDone)) == Set<ArmyUserModeElement>.Of())
            {
                army = Players.GetPlayers(SeatedPlayer).Armies.GetArmy(I);
                CenterViewTo(army.X, army.Y, army.MapLevel);
                Selection.UnselectLocation();
                HexGeometry.HexToPixel(army.X, army.Y, out pixelX, out pixelY);
                Selection.SelectLocation(pixelX, pixelY, army.MapLevel);
                player.BaseHeroJoinDay = I;
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
        EngineMessage message = default;
        Selection.UnselectAll();
        EngineMessage.InitializeMessage(out message, 537001990, this);
        MapContainer.SendMessage(ref message, this);
        EngineMessage.InitializeMessage(out message, 537001993, this);
        MapContainer.SendMessage(ref message, this);
        _seatedPlayerChangedEventsStore.Raise(Current);
        ShowMessage("");
    }

    // AoWHSMap.PAS:1822
    public void BeginUnitUpdate()
    {
        _unitUpdateCount += 1;
    }

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
    public void NotifyUnitChanged(Troop troop)
    {
        if (_unitUpdateCount == 0)
        {
            _unitChangedEventsStore.Raise(troop);
        }
    }

    // AoWHSMap.PAS:1838
    public void NotifyUnitDestroyed(Troop troop)
    {
        if (!Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            _unitDestroyedEventsStore.Raise(troop);
        }
    }

    // AoWHSMap.PAS:1844
    public void BeginArmyUpdate()
    {
        _armyUpdateCount += 1;
    }

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
    public void NotifyArmyChanged(Army army)
    {
        if (_armyUpdateCount == 0)
        {
            _armyChangedEventsStore.Raise(army);
        }
    }

    // AoWHSMap.PAS:1860
    public void NotifyArmyDestroyed(Army army)
    {
        // TArmy.Destroy passes its Self pointer
        _armyDestroyedEventsStore.Raise(army);
    }

    // AoWHSMap.PAS:1866
    // TArmy.Destroy passes TArmy
    public void BeginStructureUpdate()
    {
        _structureUpdateCount += 1;
    }

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
    public void NotifyStructureChanged(Structure structure)
    {
        if (_structureUpdateCount == 0)
        {
            _structureChangedEventsStore.Raise(structure);
        }
    }

    // AoWHSMap.PAS:1882
    public void NotifyStructureDestroyed(Structure structure)
    {
        _structureDestroyedEventsStore.Raise(structure);
    }

    // AoWHSMap.PAS:1898
    public void ExecuteExitGame()
    {
        ExitPending = true;
        _exitEventsStore.Raise(this);
    }

    // AoWHSMap.PAS:1904
    public void ExitGame(ExitGameMode mode, ExitGameCallBack done)
    {
        PlayerControl playerControl;
        if (!ExitPending)
        {
            playerControl = PlayerControl;
            if (playerControl != null)
            {
                playerControl.ExitGame(mode, done);
            }
            else
            {
                ExecuteExitGame();
                if (done != null)
                {
                    done(mode, true);
                }
            }
        }
        else
        {
            if (done != null)
            {
                done(mode, true);
            }
        }
    }

    // AoWHSMap.PAS:1921
    public void TriggerEndTurnEvent(sbyte player)
    {
        _endTurnEventsStore.Raise(player);
    }

    // AoWHSMap.PAS:1926
    public void ClearExecuteEventLogBuffer()
    {
        while (_executeEventLogCallBacks.GetCount() > 0)
        {
            if (_executeEventLogCallBacks.GetItem(0).Done != null)
            {
                var item = _executeEventLogCallBacks.GetItem(1);
                _executeEventLogCallBacks.GetItem(0).Done(item.Log);
            }

            // { Original passes the second record's log to the first callback, even for a one-item list. }
            _executeEventLogCallBacks.Delete(0);
        }
    }

    // AoWHSMap.PAS:1937
    public void ExecuteNextEventLog()
    {
        var notification = new ExecuteEventLogNotification();
        ref var info = ref notification.Info;
        if (_executeEventLogCallBacks.GetCount() != 0)
        {
            AILockCount += 1;
            info = _executeEventLogCallBacks.GetItem(0);
            info.Done = ExecuteEventLogDone;
            _executeEventLogEventsStore.Raise(notification);
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
        ExecuteEventLogInfo info;
        if (_executeEventLogCallBacks.GetCount() > 0 && log == _executeEventLogCallBacks.GetItem(0).Log)
        {
            info = _executeEventLogCallBacks.GetItem(0);
            _executeEventLogCallBacks.Delete(0);
            if (info.Done != null)
            {
                info.Done(info.Log);
            }

            info.Log.Release();
        }

        SetExecuteEventLogDelay(2);
        UnlockExecuteEventLog();
    }

    // AoWHSMap.PAS:1985
    public void LockExecuteEventLog()
    {
        AILockCount += 1;
    }

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
    public void GameOverEventHandled(sbyte player)
    {
        PlayerControl.GameOverEventHandled(player);
    }

    // AoWHSMap.PAS:2024
    public void ExecuteGameOverEvent(GameOverEventLog eventValue)
    {
        Player player;
        GameOverEventLog log;
        for (int I = 1, last = Current.Players.Count - 1; I <= last; I++)
        {
            player = Current.Players.GetPlayers(I);
            if (I == eventValue._player || player.GameOverStatus == GameOverStatus.None)
            {
                log = (GameOverEventLog)eventValue.Copy(GetEngine());
                if (_gameOverLockCount == 0)
                {
                    player.EventLogbook.AddEvent(log);
                    if (I == Current.SeatedPlayer)
                    {
                        log.Execute(false, ExecuteTrigger.Event, null);
                    }
                }
                else
                {
                    log.TargetPlayer = (sbyte)I;
                    _executeEventLogBuffer.AddEvent(log);
                }

                log.Release();
            }
        }
    }

    // AoWHSMap.PAS:2052
    public void UnlockGameOverEvent()
    {
        sbyte player;
        EventLog log;
        _gameOverLockCount -= 1;
        if (_gameOverLockCount == 0)
        {
            while (_executeEventLogBuffer.Count != 0)
            {
                player = ((GameOverEventLog)_executeEventLogBuffer.GetEvents(0)).TargetPlayer;
                log = (EventLog)_executeEventLogBuffer.GetEvents(0).Copy(GetEngine());
                Current.Players.GetPlayers(player).EventLogbook.AddEvent(log);
                if (player == Current.SeatedPlayer)
                {
                    log.Execute(false, ExecuteTrigger.Event, null);
                }

                log.Release();
                var events = _executeEventLogBuffer.GetEvents(0);
                _executeEventLogBuffer.RemoveEvent(events);
            }
        }
    }

    // AoWHSMap.PAS:2071
    public void LockGameOverEvent()
    {
        _gameOverLockCount += 1;
    }

    // AoWHSMap.PAS:2077
    public void GameOver(sbyte player, GameOverStatus status)
    {
        if (player > 0 && this.Status.Contains(HexSpriteMapStatusElement.Activated) && !this.Status.Contains(HexSpriteMapStatusElement.Editting) && !this.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            PlayerControl.GameOver(player, status);
        }
    }

    // AoWHSMap.PAS:2085
    public void VerifyAddOnPacks()
    {
    }

    // AoWHSMap.PAS:2088
    public bool ValidateMap(StringCollection messages)
    {
        EngineMessage message = default;
        EngineMessage.InitializeMessage(out message, 537002243, this);
        message.Payload = messages;
        message.Data[0] = 0;
        Current.SendMessage(ref message, this);
        bool result = message.Data[0] == 0;
        if (result)
        {
            messages.Add(AbilityRules.TranslateResourceString(GetEngine(), s_mapSuccessfullyValidatedText));
        }
        else
        {
            messages.Add(AbilityRules.TranslateResourceString(GetEngine(), s_mapValidationFailedText));
        }

        return result;
    }

    // AoWHSMap.PAS:2261
    public void InitializeMapLevel(sbyte level)
    {
        HexControl control;
        control = Engine.HexSpriteSet.HexControl;
        if (level == 0)
        {
            for (int X = 6, last = MapContainer.Width - 7; X <= last; X++)
            {
                for (int Y = 6, lastY = MapContainer.Height - 7; Y <= lastY; Y++)
                {
                    control.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 0);
                }
            }
        }
        else
        {
            for (int X = 1, last = MapContainer.Width - 7; X <= last; X++)
            {
                for (int Y = 1, lastY = MapContainer.Height - 7; Y <= lastY; Y++)
                {
                    control.Place(MapContainer, (sbyte)X, (sbyte)Y, level, 7);
                }
            }
        }

        for (int X = 0, last = MapContainer.Width - 1; X <= last; X++)
        {
            for (int Y = 0; Y <= 5; Y++)
            {
                control.Place(MapContainer, (sbyte)X, (sbyte)Y, level, 15);
            }

            for (int Y = MapContainer.Height - 6, lastY = MapContainer.Height - 1; Y <= lastY; Y++)
            {
                control.Place(MapContainer, (sbyte)X, (sbyte)Y, level, 15);
            }
        }

        for (int Y = 6, lastY = MapContainer.Height - 7; Y <= lastY; Y++)
        {
            for (int X = 0; X <= 5; X++)
            {
                control.Place(MapContainer, (sbyte)X, (sbyte)Y, level, 15);
            }

            for (int X = MapContainer.Width - 6, last = MapContainer.Width - 1; X <= last; X++)
            {
                control.Place(MapContainer, (sbyte)X, (sbyte)Y, level, 15);
            }
        }
    }

    // AoWHSMap.PAS:2332
    public void NotifyPlayerChanged(int X, int Y, int L, int radius)
    {
        MapLevel mapLevel;
        MapFieldMessage message = default;
        HexNumberWalkData walk = default;
        MapField field;
        mapLevel = MapContainer.GetMapLevel(L);
        message.MessageId = 4365;
        message.Result = 1;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                field = mapLevel.GetField(walk.X, walk.Y);
                if ((field.FieldFlags & MapField.PlayerChangeListenerFlag) != 0)
                {
                    message.Param[0] = walk.Radius;
                    field.SendMapFieldMessage(ref message);
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // AoWHSMap.PAS:2359
    public void UpdatePlayerSeen(int player, int X, int Y, int L, int sightRadius, int radius)
    {
        MapLevel mapLevel;
        MapFieldMessage message = default;
        HexNumberWalkData walk = default;
        MapField field;
        mapLevel = MapContainer.GetMapLevel(L);
        message.MessageId = 4400;
        message.Param[0] = player;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
        while (walk.Radius <= sightRadius || walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                field = mapLevel.GetField(walk.X, walk.Y);
                if ((field.FieldFlags & MapField.OwnerFlag) != 0)
                {
                    message.Result = -1;
                    if (walk.Radius <= radius)
                    {
                        // { Original leaves the flag uninitialized outside Radius. }
                        message.Param[1] = 1;
                    }

                    field.SendMapFieldMessage(ref message);
                    if (message.Result.Int != -1 && message.Result.Int != player)
                    {
                        Current.Players.GetPlayers(player).PlayerSeen((sbyte)message.Result.Int);
                        for (int I = 0, last = Players.Count - 1; I <= last; I++)
                        {
                            if (I != player && Players.GetDiplomaticRelation((sbyte)player, (sbyte)I) == DiplomaticRelation.Alliance)
                            {
                                Current.Players.GetPlayers(I).PlayerSeen((sbyte)message.Result.Int);
                            }
                        }
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // AoWHSMap.PAS:2395
    public void NotifyPlayerChanged(int X, int Y, int L)
    {
        NotifyPlayerChanged(X, Y, L, 12);
    }

    // AoWHSMap.PAS:2401
    public bool VisibleForPlayer(int X, int Y, int level, sbyte player)
    {
        bool result;
        MapLevel mapLevel;
        MapFieldMessage message = default;
        HexNumberWalkData walk = default;
        MapField field;
        mapLevel = MapContainer.GetMapLevel(level);
        if (player >= 0)
        {
            message.MessageId = 4362;
            message.Result = 0;
            message.Param[1] = player;
            HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
            while (message.Result.Int == 0 && walk.Radius <= 12)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    field = mapLevel.GetField(walk.X, walk.Y);
                    if ((field.FieldFlags & MapField.ViewerFlag) != 0)
                    {
                        message.Param[0] = walk.Radius;
                        field.SendMapFieldMessage(ref message);
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            result = message.Result.Int != 0;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWHSMap.PAS:2434
    public bool FoggedForPlayer(int X, int Y, int L, sbyte player)
    {
        bool result;
        MapLevel mapLevel;
        MapFieldMessage message = default;
        HexNumberWalkData walk = default;
        MapField field;
        mapLevel = MapContainer.GetMapLevel(L);
        if (!ExplorationEnabled || mapLevel.GetField(X, Y).GetExplored(player))
        {
            if (FogEnabled)
            {
                if (player != Current.SeatedPlayer)
                {
                    message.MessageId = 4362;
                    message.Result = 0;
                    message.Param[1] = player;
                    HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
                    while (message.Result.Int == 0 && walk.Radius <= 12)
                    {
                        if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                        {
                            field = mapLevel.GetField(walk.X, walk.Y);
                            if ((field.FieldFlags & MapField.ViewerFlag) != 0)
                            {
                                message.Param[0] = walk.Radius;
                                field.SendMapFieldMessage(ref message);
                            }
                        }

                        HexNumberWalkData.WalkToNextHexNumber(ref walk);
                    }

                    result = message.Result.Int == 0;
                }
                else
                {
                    var fieldValue = Current.MapContainer.GetField(X, Y, L);
                    result = fieldValue.FogCount == 0;
                }
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    // AoWHSMap.PAS:2474
    public void MouseMoved(object sender)
    {
        MapField field;
        // The original tests Xhx only and with Yhx -1 read memory before the field block. The
        // port treats that point as off the map.
        if (Mouse.Capture == null && Mouse.HexX != -1 && Mouse.HexY != -1)
        {
            var hexY = Mouse.HexY;
            var hexX = Mouse.HexX;
            field = MapContainer.GetField(hexX, hexY, _viewMapLevel);
            if (Current.ExplorationEnabled && !field.GetExplored(Current.SeatedPlayer))
            {
                return;
            }

            if (!Current.FogEnabled || field.FogCount != 0)
            {
                var pixelY = Mouse.PixelY;
                var pixelX = Mouse.PixelX;
                if (Selection.CanSelectLocation(pixelX, pixelY, _viewMapLevel) && !((GameMapMouse)Mouse).GetSelectingRectangle())
                {
                    Mouse.CursorMode = 257;
                }
                else
                {
                    Mouse.CursorMode = 0;
                }
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
                Mouse.Cursor = (int)(((uint)Frame) >> 2) % 4 + 52;
                break;
            case 258:
                Mouse.Cursor = (((int)(((uint)Frame) >> 2)) & 1) + 49;
                break;
            case 259:
                Mouse.Cursor = (int)(((uint)Frame) >> 2) % 3 + 58;
                break;
        }

        Mouse.CursorVisible = Mouse.Cursor != 0;
    }

    // AoWHSMap.PAS:2502
    public void ResetSeatedPlayerView()
    {
        Player player;
        Army army;
        MapCoordinate location = default;
        if (SeatedPlayer != -1)
        {
            player = Players.GetPlayers(SeatedPlayer);
            if ((sbyte)player.ViewX > 0)
            {
                location = new MapCoordinate(X: (sbyte)player.ViewX, Y: (sbyte)player.ViewY, MapLevel: (sbyte)player.ViewMapLevel);
            }
            else
            {
                player.GetLeaderLocation(out var locationX, out var locationY, out var locationMapLevel);
                location = new MapCoordinate(X: locationX, Y: locationY, MapLevel: locationMapLevel);
                if (location.X == -1)
                {
                    if (player.Armies.Count == 0)
                    {
                        location = new MapCoordinate(X: 0, Y: 0, MapLevel: 0);
                    }
                    else
                    {
                        army = player.Armies.GetArmy(0);
                        location = new MapCoordinate(X: army.X, Y: army.Y, MapLevel: army.MapLevel);
                    }
                }
            }

            CenterViewTo(location.X, location.Y, location.MapLevel);
        }
    }

    // AoWHSMap.PAS:2538
    public void Select(HexSprite sprite, bool center)
    {
        // { This eliminated copy preserves the original saved registers. }
        Selection.UnselectLocation();
        Selection.SelectLocationHexSprite(sprite);
        if (center)
        {
            var hexLevel = sprite.GetHexLevel();
            var hexY = sprite.GetHexY();
            CenterViewTo(sprite.GetHexX(), hexY, hexLevel);
        }
    }

    // AoWHSMap.PAS:2546
    public void NotifyPlayerChanged()
    {
        _currentPlayerChangedEventsStore.Raise(this);
    }

    // AoWHSMap.PAS:2552
    public void TokenControlActivatedEvent(object sender)
    {
        int player;
        if (CombatMap == null)
        {
            player = TokenManager.TokenExecutor.ActiveTacticalBattleIndex;
            if (player != CurrentPlayer)
            {
                CurrentPlayer = (sbyte)player;
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
                    if (!(PlayerControl is PbemPlayerControl))
                    {
                        PlayerControl?.Dispose();
                        PlayerControl = new PbemPlayerControl(null) { Holder = CreationHolder() };
                    }
                }
                else
                {
                    // { Original tests TPBEMPlayerControl even in the non-email branch. }
                    if (!(PlayerControl is PbemPlayerControl))
                    {
                        PlayerControl?.Dispose();
                        PlayerControl = new TurnPlayerControl(null) { Holder = CreationHolder() };
                    }
                }

                break;
            case PlayMode.Simultaneous:
                if (!(PlayerControl is SimultaneousPlayerControl))
                {
                    PlayerControl?.Dispose();
                }

                // { Original replaces an existing control without freeing it. }
                PlayerControl = new SimultaneousPlayerControl(null) { Holder = CreationHolder() };
                break;
        }

        ConnectionType = connection;
        PlayMode = mode;
        if (PlayerControl != null)
        {
            PlayerControl.Setup();
        }
    }

    // AoWHSMap.PAS:2692
    public void ActivateMessage()
    {
        if (Day == 0)
        {
            Races.ResetDefaultRelations();
        }

        if (!Status.Contains(HexSpriteMapStatusElement.Editting) && Day == 0)
        {
            ExplorationEnabled = true;
            FogEnabled = true;
        }
    }

    // AoWHSMap.PAS:2703
    public int Random(int range)
    {
        int result;
        if (!Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            if (GetSynchronized())
            {
                RandSeed = DefendersSeed;
                result = RtlUnit.Random(range);
                DefendersSeed = RandSeed;
            }
            else
            {
                RtlUnit.ShowMessage("Invalid AoWHSMap.Random use");
                result = 0;
            }
        }
        else
        {
            result = RtlUnit.Random(range);
        }

        return result;
    }

    // AoWHSMap.PAS:2723
    public void GenerateNewTurnOrder()
    {
        int index;
        ByteList list;
        list = new ByteList();
        for (int I = 1, last = Players.Count - 1; I <= last; I++)
        {
            list.Add((byte)I);
        }

        TurnOrder.Clear();
        TurnOrder.Add(0);
        while (list.Count != 0)
        {
            index = Random(list.Count);
            var item = list[index];
            TurnOrder.Add(item);
            list.Delete(index);
        }

        list?.Dispose();
    }

    // AoWHSMap.PAS:2749
    public bool EndTurn()
    {
        bool result;
        if (SeatedPlayer != -1)
        {
            if (!RequestCombatType || _executeEventLogCallBacks.GetCount() == 0)
            {
                result = TryEndPlayerTurn(SeatedPlayer);
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWHSMap.PAS:2777
    public bool CancelEndTurn()
    {
        bool result;
        if (SeatedPlayer != -1)
        {
            result = CancelPlayerTurn(SeatedPlayer);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWHSMap.PAS:2784
    public void LockUpdateVisibility()
    {
        _updateVisibilityLockCount += 1;
    }

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
        EngineMessage message = default;
        if (_updateVisibilityLockCount == 0)
        {
            if (!Status.Contains(HexSpriteMapStatusElement.Destroying))
            {
                LockMapChangedEvent();
                EngineMessage.InitializeMessage(out message, 537001992, this);
                Current.SendMessage(ref message, this);
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
        if (MapContainer != null)
        {
            for (int I = 0, last = MapContainer.LevelCount - 1; I <= last; I++)
            {
                MapContainer.GetMapLevel(I).Reset();
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
        Player player = default;
        int X = default;
        int Y = default;
        int L = default;
        if (CombatMap == null)
        {
            if (SeatedPlayer != -1)
            {
                player = Players.GetPlayers(SeatedPlayer);
                GetViewCenter(out X, out Y, out L);
                player.ViewX = (byte)X;
                player.ViewY = (byte)Y;
                player.ViewMapLevel = (byte)L;
            }

            CombatMap = (CombatMap)GetEngine().LoadHsmMap(FileBoundary.ReadName(GetEngine().StartupDirectory + fileName), false, null, GameProfile.TacticalMap);
            CombatMapCreatedEventsStore.Raise(this);
        }
    }

    // AoWHSMap.PAS:2891
    public void ReleaseCombatMap()
    {
        if (CombatMap != null)
        {
            CombatMapDestroyedEventsStore.Raise(this);
            CombatMap?.Dispose();
            CombatMap = null;
            ResetSeatedPlayerView();
        }
    }

    // AoWHSMap.PAS:2903
    public Battle CreateCombat(int classId)
    {
        Battle result;
        if (Combat == null)
        {
            result = (Battle)GetEngine().CreateEngineObject(classId, null);
            Combat = result;
            if (GameType == GameType.Tutorial && classId == 2228496)
            {
                TutorialControl.ShowMessage(53, false, true);
            }

            if (classId == 2228496)
            {
                ((GameMapMouse)Current.Mouse).ResetRectangleSelection();
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
        sbyte player = default;
        int count = default;
        NewTurnEventLog log = default;
        if (Combat != null)
        {
            Current.LockGameOverEvent();
            if (Combat.Status.Contains(CombatStatusElement.Activated))
            {
                Combat.Deactivate();
            }

            if (Combat.Status.Contains(CombatStatusElement.Executing))
            {
                Combat.Finalize();
                player = Combat.CombatData.Players.GetPlayers(Combat.CombatData.AttackerPlayer).Player;
                if (Current.ConnectionType == PlayerConnectionType.Network && PlayMode == PlayMode.Simultaneous)
                {
                    count = Current.Players.Count;
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

            if (RequestCombatType && Combat.EngineClass.ClassId != 131760 && Combat.CombatData.Round != 0 && Combat.ExplorationSite != 1 && Players.GetPlayers(CurrentPlayer).PlayerType == PlayerType.Human)
            {
                LockExecuteEventLog();
                log = new NewTurnEventLog(null) { Holder = CreationHolder() };
                log.TurnPlayer = CurrentPlayer;
                log.Mode = NewTurnEventLogMode.Dialog;
                log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                log.Text = AbilityRules.TranslateResourceString(GetEngine(), GameText.YourTurnContinuesText);
                TriggerExecuteEventLog(log, false, ShowSeatedPlayerCallback);
                log.Release();
                UnlockGameOverEvent();
                SetSeatedPlayer(-1);
                UnlockExecuteEventLog();
            }
            else
            {
                if (Combat.EngineClass.ClassId == 2228496)
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
        if (Status.Contains(HexSpriteMapStatusElement.Activated) && _relationsChangedLockCount == 0)
        {
            BeginUnitUpdate();
            UnitControl.TriggerUnitsChanged();
            _playerChangedEventsStore.Raise(this);
            EndUnitUpdate();
        }
    }

    // AoWHSMap.PAS:3038
    public void LockRelationsChanged()
    {
        _relationsChangedLockCount += 1;
    }

    // AoWHSMap.PAS:3044
    public void UnlockRelationsChanged()
    {
        Assert(_relationsChangedLockCount != 0, "Invalid MapChanged Lock usage", "AoWHSMap.PAS", 3056);
        _relationsChangedLockCount -= 1;
    }

    // AoWHSMap.PAS:3061
    public void UpdateTurnTimeBeep()
    {
        int seconds;
        if (GetTurnTimerEnabled())
        {
            seconds = GetTurnTimeLeft() / 1000;
            if (_turnTimeBeep != 0 && seconds < _turnTimeBeep && CombatMap == null)
            {
                _turnTimeBeep = seconds;
                if (_turnTimeBeep == 0)
                {
                    AudioPlayback.PlaySample("SFX\\EVENTS\\CLOCK.WAV");
                }
                else
                {
                    AudioPlayback.PlaySample("SFX\\EVENTS\\CLOCK.WAV");
                }
            }
        }
    }

    // AoWHSMap.PAS:3078
    public void ResetTurnTimer()
    {
        _turnTimeElapsed = TurnTime * 1000 * 60;
        _turnTimeBeep = 11;
        _turnTimerStart = -1;
    }

    // AoWHSMap.PAS:3086
    public int GetTurnTimeLeft()
    {
        int time;
        int left;
        left = _turnTimeElapsed;
        if (_turnTimerStart != -1)
        {
            time = PixelFormats.GetElapsedMilliseconds();
            if (time >= _turnTimerStart)
            {
                left = left - (time - _turnTimerStart);
            }
            else
            {
                left = 0;
            }
        }

        if (left < 0)
        {
            left = 0;
        }

        return left;
    }

    // AoWHSMap.PAS:3102
    public bool GetTurnTimerEnabled() => TurnTime > 0 && _turnTimerStart != -1;

    // AoWHSMap.PAS:3108
    public void EnableTurnTimer()
    {
        if (_turnTimerStart == -1 && TurnTime > 0)
        {
            _turnTimerStart = PixelFormats.GetElapsedMilliseconds();
        }
    }

    // AoWHSMap.PAS:3114
    public void DisableTurnTimer()
    {
        int time;
        time = PixelFormats.GetElapsedMilliseconds() - _turnTimerStart;
        if (time > 0)
        {
            _turnTimeElapsed -= time;
        }

        _turnTimerStart = -1;
    }

    // AoWHSMap.PAS:3124
    public void ZeroTurnTimer()
    {
        _turnTimeElapsed = 0;
    }

    // AoWHSMap.PAS:3144
    public void CenterOnLeader(sbyte player, bool select, bool animate)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        ArmyHexSprite army;
        if (Current.Players.GetPlayers(player).Leader != null)
        {
            Current.Players.GetPlayers(player).Leader.GetLocation(out X, out Y, out L);
            // { Original excludes X = 0. }
            if (X > 0)
            {
                Current.CenterViewTo(X, Y, L);
                if (animate)
                {
                    Current.PlayCenterAnimation(X, Y, L);
                }

                army = Current.MapContainer.GetField(X, Y, L).GetArmyHexSprite();
                if (army != null)
                {
                    Selection.UnselectLocation();
                    Selection.SelectLocationHexSprite(army);
                }
            }
        }
    }

    // AoWHSMap.PAS:3167
    public void PlaceTerrain(sbyte X, sbyte Y, sbyte L, sbyte terrainType, sbyte overlayType)
    {
        MapObjectResourceList resources;
        DelphiList<MapObjectResource> candidates;
        MapObjectResource resource;
        RandSeed = Current.Random(16777215);
        resources = GamePack.Current.MapObjectResourceList;
        if (!Set<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)overlayType))
        {
            candidates = new DelphiList<MapObjectResource>();
            candidates.Clear();
            for (int I = 0, last = resources.Count - 1; I <= last; I++)
            {
                resource = resources[I];
                if (resource.EngineClass.ClassId == 131431 && resource.TerrainList.UsedTerrain() == 1 && resource.TerrainList.Terrain[0] == terrainType && resource.TerrainList.Overlay[0] == overlayType)
                {
                    candidates.Add(resource);
                }
            }

            if (candidates.Count != 0)
            {
                candidates[Random(candidates.Count)].PlaceResourceUser(Current, X, Y, L);
            }

            candidates?.Dispose();
        }
        else
        {
            GamePack.Current.RoadControl.Place(Current.MapContainer, X, Y, L, terrainType);
        }
    }

    // AoWHSMap.PAS:3195
    public void UpdateEffects()
    {
        int I;
        WorldEffectHexSprite effect;
        if (!Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            I = 0;
            while (I < Effects.Count)
            {
                effect = Effects[I];
                if (effect.Done)
                {
                    DynamicFixedVisibleHexSprite.RemoveChild(effect);
                    DynamicVisibleHexSprite.RemoveChild(effect);
                    effect?.Dispose();
                    Effects.RemoveAt(I);
                }
                else
                {
                    I += 1;
                }
            }
        }
    }

    // AoWHSMap.PAS:3219
    public WorldEffectHexSprite AddEffect(WorldEffectType effectType)
    {
        WorldEffectHexSprite effect;
        effect = new WorldEffectHexSprite(null) { Holder = CreationHolder() };
        Effects.Add(effect);
        effect.Activate();
        effect.EffectType = effectType;
        DynamicFixedVisibleHexSprite.AddChild(effect);
        return effect;
    }

    public bool ValidateObservation(sbyte player, sbyte x, sbyte y, sbyte level, int radius)
    {
        if (!ShowIndependentMoves || player == Current.SeatedPlayer) return false;
        HexNumberWalkData walk = default;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, x, y, 0);
        MapLevel mapLevel = null;
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
                    var field = mapLevel.GetField(walk.X, walk.Y);
                    result = (field.FogCount != 0 || !Current.FogEnabled) &&
                        (field.GetExplored(Current.SeatedPlayer) || !Current.ExplorationEnabled);
                }
            }
            if (!result) HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
        if (result) MakeVisible(walk.X, walk.Y, level, 3);
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

    private bool TryEndPlayerTurn(sbyte player)
    {
        bool result;
        if (player != -1 && !Current.Players.GetPlayers(player).GetBusy() && !Current.Players.GetPlayers(player).SingleProcessCycle)
        {
            result = AbilityRules.EndPlayerTurn(player, engine: GetEngine());
        }
        else
        {
            result = false;
        }

        return result;
    }

    private bool CancelPlayerTurn(sbyte player)
    {
        bool result;
        EndTurnTokenEvent eventValue;
        if (!Current.Players.GetPlayers(player).GetBusy() && Current.Players.GetPlayers(player).SingleProcessCycle)
        {
            eventValue = new EndTurnTokenEvent(null) { Holder = CreationHolder() };
            eventValue.Player = player;
            eventValue.Day = Current.Day;
            eventValue.Mode = EndTurnTokenEventMode.CancelEndTurn;
            Current.TokenManager.GetTokenControl(player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_oneMomentPleaseText = "One moment please...";

    public static StrategicMap Current { get; set; }

    // { Interface declaration run owned by P5. }
    // allied victory. . points. gold. mana. next scenario
    public partial class MapCampaignSettings : EngineObject
    {
        // Diplomacy.pas:1716
        public MapCampaignSettings(ECustomNode owner) : base(owner)
        {
            TransferPointsMax = 1000;
            TransferGoldMax = 100;
            TransferManaMax = 100;
            HeroLevelMax = -1;
            AlliedVictory = false;
        }

        public bool AlliedVictory;
        public int TransferPointsMax;
        public int TransferGoldMax;
        public int TransferManaMax;
        public int HeroLevelMax;

        // Diplomacy.pas:1726
        public override void ReadWrite(StorageStream stream)
        {
            base.ReadWrite(stream);
            stream.Int32(ref TransferPointsMax, 20);
            stream.Int32(ref TransferGoldMax, 21);
            stream.Int32(ref TransferManaMax, 22);
            stream.Int32(ref HeroLevelMax, 23);
            stream.Boolean(ref AlliedVictory, 24);
        }

        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            TransferPointsMax = persister.Int32("TransferPointsMax", value: TransferPointsMax);
            TransferGoldMax = persister.Int32("TransferGoldMax", value: TransferGoldMax);
            TransferManaMax = persister.Int32("TransferManaMax", value: TransferManaMax);
            HeroLevelMax = persister.Int32("HeroLevelMax", value: HeroLevelMax);
            AlliedVictory = persister.Boolean("AlliedVictory", value: AlliedVictory);
        }

        // intf_P5_08.inc:13

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
