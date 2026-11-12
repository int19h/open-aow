// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;
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
public partial class AoWHSMap : GameMap
{

    public new const int ClassId = 131089;

    public SelectedArmy SelectedArmy;
    public bool IgnoreStreamErrors;
    public TargetSelectionControl TargetSelection;
    public string MapName = "";
    public string Author = "";
    public int SphereCount;
    public StringList Description;
    public ItemControl ItemControl;
    public HeroControl HeroControl;
    public UnitControl UnitControl;
    public StructureControl StructureControl;
    public AoWHSMapIndependentAIMode IndependentAIMode;
    public StringList Objectives;
    public sbyte HeroCountMax;
    public StartupSongList StartupSongs;
    public SongList GameSongs;
    public AoWHSMapGameStatus GameStatus;
    public AoWHSMapGameResult GameResult;
    public GameType GameType;
    public CustomCombatMap CombatMap;
    public Battle Combat;
    public TutorialControl TutorialControl;
    public AIExecutor AIExecutor;
    public AIPlayerActionManager AIPlayerActionManager;
    public AIGroupManager AIGroupManager;
    public HexSpriteMapKeyboard Keyboard;
    public bool UnlimitedMoves;
    public bool RequestCombatType;
    public PlayerConnectionType ConnectionType;
    public TextControl TextControl;
    public PlayerList Players;
    public RaceList Races;
    public bool FixedLeaders;
    public bool LeadersFixed;
    public bool StartingGame;
    public bool AlliedVictory;
    public byte VictoryMode;
    public AoWHSMapVictoryCondition VictoryCondition;
    public int VictoryMana;
    public int VictoryGold;
    public int DayLimit;
    public int TurnTime;
    public int TurnTimeElapsed;
    public int TurnTimeBeep;
    public int TurnTimerStart;
    public AoWHSMapTurnOrderMode TurnOrderMode;
    public ByteList TurnOrder;
    public int Day;
    public string SessionName = "";

    internal EventStore<Leader> _onLeaderChangedStore;

    public event NotifyEvent<Leader> FOnLeaderChanged
    {
        add => _onLeaderChangedStore.Add(value);
        remove => _onLeaderChangedStore.Remove(value);
    }

    public bool ShowIndependentMoves;
    public MoveControl MoveControl;
    public GlobalMagicControl GlobalMagicControl;
    public MapCampaignSettings CampaignSettings;
    public sbyte EditorPlayer;
    public bool ShowAllPlayers;
    public int RelationsChangedLockCount;

    internal EventStore<AoWHSMap> _playerChangedEventsStore;

    public event NotifyEvent<AoWHSMap> FPlayerChangedEvents
    {
        add => _playerChangedEventsStore.Add(value);
        remove => _playerChangedEventsStore.Remove(value);
    }

    internal EventStore<PlayerControl> _newDayEventsStore;

    public event NotifyEvent<PlayerControl> FNewDayEvents
    {
        add => _newDayEventsStore.Add(value);
        remove => _newDayEventsStore.Remove(value);
    }

    internal EventStore<sbyte> _newTurnEventsStore;

    public event NotifyEvent<sbyte> FNewTurnEvents
    {
        add => _newTurnEventsStore.Add(value);
        remove => _newTurnEventsStore.Remove(value);
    }

    internal EventStore<sbyte> _endTurnEventsStore;

    public event NotifyEvent<sbyte> FEndTurnEvents
    {
        add => _endTurnEventsStore.Add(value);
        remove => _endTurnEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> _currentPlayerChangedEventsStore;

    public event NotifyEvent<AoWHSMap> FCurrentPlayerChangedEvents
    {
        add => _currentPlayerChangedEventsStore.Add(value);
        remove => _currentPlayerChangedEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> _seatedPlayerChangedEventsStore;

    public event NotifyEvent<AoWHSMap> FSeatedPlayerChangedEvents
    {
        add => _seatedPlayerChangedEventsStore.Add(value);
        remove => _seatedPlayerChangedEventsStore.Remove(value);
    }

    internal EventStore<Battle> _combatEventsStore;

    public event NotifyEvent<Battle> FCombatEvents
    {
        add => _combatEventsStore.Add(value);
        remove => _combatEventsStore.Remove(value);
    }

    internal EventStore<sbyte> _playerTerminatedEventsStore;

    public event NotifyEvent<sbyte> FPlayerTerminatedEvents
    {
        add => _playerTerminatedEventsStore.Add(value);
        remove => _playerTerminatedEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> _gameOverEventsStore;

    public event NotifyEvent<AoWHSMap> FGameOverEvents
    {
        add => _gameOverEventsStore.Add(value);
        remove => _gameOverEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> _gameTerminatedEventsStore;

    public event NotifyEvent<AoWHSMap> FGameTerminatedEvents
    {
        add => _gameTerminatedEventsStore.Add(value);
        remove => _gameTerminatedEventsStore.Remove(value);
    }

    public bool ExitPending;

    internal EventStore<AoWHSMap> _exitEventsStore;

    public event NotifyEvent<AoWHSMap> FExitEvents
    {
        add => _exitEventsStore.Add(value);
        remove => _exitEventsStore.Remove(value);
    }

    public int GameOverLockCount;
    public EventLogList ExecuteEventLogBuffer;

    internal EventStore<ExecuteEventLogNotification> _executeEventLogEventsStore;

    public event NotifyEvent<ExecuteEventLogNotification> FExecuteEventLogEvents
    {
        add => _executeEventLogEventsStore.Add(value);
        remove => _executeEventLogEventsStore.Remove(value);
    }

    public ExecuteEventLogCallbackList ExecuteEventLogCallBacks;
    public int AILockCount;
    public int ExecuteEventLogDelay;

    internal EventStore<SpellCaster> _cosmeticSurgeryEventsStore;

    public event NotifyEvent<SpellCaster> FCosmeticSurgeryEvents
    {
        add => _cosmeticSurgeryEventsStore.Add(value);
        remove => _cosmeticSurgeryEventsStore.Remove(value);
    }

    internal EventStore<AbilityActivation> _onActivateAbilityStore;

    public event NotifyEvent<AbilityActivation> FOnActivateAbility
    {
        add => _onActivateAbilityStore.Add(value);
        remove => _onActivateAbilityStore.Remove(value);
    }

    internal EventStore<EngineObject> _diplomaticRelationsChangedEventsStore;

    public event NotifyEvent<EngineObject> FDiplomaticRelationsChangedEvents
    {
        add => _diplomaticRelationsChangedEventsStore.Add(value);
        remove => _diplomaticRelationsChangedEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> _updateVisibilityEventsStore;

    public event NotifyEvent<AoWHSMap> FUpdateVisibilityEvents
    {
        add => _updateVisibilityEventsStore.Add(value);
        remove => _updateVisibilityEventsStore.Remove(value);
    }

    public List<WorldEffectHexSprite> Effects;
    public int UpdateVisibilityLockCount;
    public bool VisibilityChanged;
    public int ArmyUpdateCount;
    public int StructureUpdateCount;
    public int UnitUpdateCount;

    internal EventStore<Troop> _unitChangedEventsStore;

    public event NotifyEvent<Troop> FUnitChangedEvents
    {
        add => _unitChangedEventsStore.Add(value);
        remove => _unitChangedEventsStore.Remove(value);
    }

    internal EventStore<Army> _armyChangedEventsStore;

    public event NotifyEvent<Army> FArmyChangedEvents
    {
        add => _armyChangedEventsStore.Add(value);
        remove => _armyChangedEventsStore.Remove(value);
    }

    internal EventStore<Structure> _structureChangedEventsStore;

    public event NotifyEvent<Structure> FStructureChangedEvents
    {
        add => _structureChangedEventsStore.Add(value);
        remove => _structureChangedEventsStore.Remove(value);
    }

    internal EventStore<Troop> _unitDestroyedEventsStore;

    public event NotifyEvent<Troop> FUnitDestroyedEvents
    {
        add => _unitDestroyedEventsStore.Add(value);
        remove => _unitDestroyedEventsStore.Remove(value);
    }

    internal EventStore<Army> _armyDestroyedEventsStore;

    public event NotifyEvent<Army> FArmyDestroyedEvents
    {
        add => _armyDestroyedEventsStore.Add(value);
        remove => _armyDestroyedEventsStore.Remove(value);
    }

    internal EventStore<Structure> _structureDestroyedEventsStore;

    public event NotifyEvent<Structure> FStructureDestroyedEvents
    {
        add => _structureDestroyedEventsStore.Add(value);
        remove => _structureDestroyedEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> _tokenControlActivatedEventsStore;

    public event NotifyEvent<AoWHSMap> FTokenControlActivatedEvents
    {
        add => _tokenControlActivatedEventsStore.Add(value);
        remove => _tokenControlActivatedEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> _combatMapCreatedEventsStore;

    public event NotifyEvent<AoWHSMap> FCombatMapCreatedEvents
    {
        add => _combatMapCreatedEventsStore.Add(value);
        remove => _combatMapCreatedEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> _combatMapDestroyedEventsStore;

    public event NotifyEvent<AoWHSMap> FCombatMapDestroyedEvents
    {
        add => _combatMapDestroyedEventsStore.Add(value);
        remove => _combatMapDestroyedEventsStore.Remove(value);
    }

    public int InitialRandomSeed;
    public int DefendersSeed;
    public int SynchronizeCount;
    public bool DebugSave;
    public PlayMode PlayMode;
    public TokenManager TokenManager;
    public PlayerCommunicator PlayerCommunicator;
    public PlayerControl PlayerControl;
    public bool UseTurnTimer;
    public bool ShowUnitShadows;
    public Text TokenEventLog;
    public bool TokenEventLogging;

    // AoWHSMap.PAS:1336
    public override void Dispose()
    {
        StopTokenEventLogging();
        LockMapChangedEvent();
        OnViewLevel = null;
        OnCenterView = null;
        AoWHSSet.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "GENERAL.ILB");
        AoWHSSet.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "CURSORS.ILB");
        AoWHSSet.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "COMBAT.ILB");
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
        _newTurnEventsStore.Clear();
        _newDayEventsStore.Clear();
        _onLeaderChangedStore.Clear();
        Description.Clear();
        Objectives.Clear();
        TurnOrder.Clear();
        _structureDestroyedEventsStore.Clear();
        _unitDestroyedEventsStore.Clear();
        _combatMapCreatedEventsStore.Clear();
        _combatMapDestroyedEventsStore.Clear();
        _updateVisibilityEventsStore.Clear();
        _diplomaticRelationsChangedEventsStore.Clear();
        _cosmeticSurgeryEventsStore.Clear();
        _onActivateAbilityStore.Clear();
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
        ExecuteEventLogBuffer?.Dispose();
        _newTurnEventsStore.Clear();
        _endTurnEventsStore.Clear();
        _currentPlayerChangedEventsStore.Clear();
        _newDayEventsStore.Clear();
        _armyChangedEventsStore.Clear();
        _structureChangedEventsStore.Clear();
        _unitChangedEventsStore.Clear();
        _armyDestroyedEventsStore.Clear();
        _structureDestroyedEventsStore.Clear();
        _unitDestroyedEventsStore.Clear();
        _onLeaderChangedStore.Clear();
        Description?.Dispose();
        Objectives?.Dispose();
        TurnOrder?.Dispose();
        PlayerControl?.Dispose();
        TextControl?.Dispose();
        _combatEventsStore.Clear();
        _playerChangedEventsStore.Clear();
        _executeEventLogEventsStore.Clear();
        ExecuteEventLogCallBacks?.Dispose();
        _cosmeticSurgeryEventsStore.Clear();
        _onActivateAbilityStore.Clear();
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
        _combatMapCreatedEventsStore.Clear();
        _combatMapDestroyedEventsStore.Clear();
        _diplomaticRelationsChangedEventsStore.Clear();
        Keyboard?.Dispose();
        _updateVisibilityEventsStore.Clear();
        _tokenControlActivatedEventsStore.Clear();
        UnlockMapChangedEvent();
    }

    // AoWHSMap.PAS:2160
    public override void ReadWrite(EStorageStream stream)
    {
        int X = default;
        int Y = default;
        int L = default;
        if (stream.StorageMode().Contains(StorageModeElement.Read) && !stream.StorageMode().Contains(StorageModeElement.Translate))
        {
            stream.ShowProgress(this, AbilityRules.TranslateResourceString(s_oneMomentPleaseText), 0);
        }

        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Write, StorageModeElement.TextOnly)) == Set<StorageModeElement>.Of(StorageModeElement.Write))
        {
            ResetModified();
        }

        IgnoreStreamErrors = false;
        if (Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            stream.OnError = OnStreamError;
        }

        stream.rwData(Bytes.Of(ref ConnectionType, 1), 1, 31);
        if (ConnectionType == PlayerConnectionType.Network)
        {
            X = _viewPixelX;
            _viewPixelX = 0;
            Y = _viewPixelY;
            _viewPixelY = 0;
            L = _viewL;
            _viewL = 0;
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
            _viewL = L;
        }

        stream.rwString(ref Author, 7);
        stream.rwStrings(Description, 11);
        stream.rwStrings(Objectives, 12);
        stream.rwData(Bytes.Of(ref IndependentAIMode, 1), 1, 13);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref CurrentPlayer), 16);
        stream.rwData(Bytes.Of(ref TurnOrderMode, 1), 1, 17);
        stream.rwStreamProc(18, TurnOrder.LoadFromStream, TurnOrder.SaveToStream);
        stream.rwInteger(ref Day, 19);
        stream.rwString(ref SessionName, 20);
        stream.rwEObject(Players, 21);
        stream.rwEObject(Races, 22);
        stream.rwBool(ref ExplorationEnabled, 23);
        if (!stream.StorageMode().Contains(StorageModeElement.Preview))
        {
            stream.rwEObject(HeroControl, 24);
        }

        stream.rwString(ref MapName, 25);
        stream.rwBool(ref FogEnabled, 26);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref HeroCountMax), 27);
        stream.rwInteger(ref DefendersSeed, 28);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly) && !stream.StorageMode().Contains(StorageModeElement.Preview) && !stream.StorageMode().Contains(StorageModeElement.Compare))
        {
            stream.rwEObject(StartupSongs, 29);
        }

        stream.rwEObject(ItemControl, 30);
        stream.rwData(Bytes.Of(ref GameStatus, 1), 1, 34);
        stream.rwData(Bytes.Of(ref GameResult, 1), 1, 35);
        stream.rwInteger(ref InitialRandomSeed, 36);
        stream.rwData(Bytes.Of(ref GameType, 1), 1, 37);
        stream.rwEObject(UnitControl, 38);
        stream.rwData(Bytes.Of(ref PlayMode, 1), 1, 39);
        stream.rwEObject(GlobalMagicControl, 40);
        stream.rwIntegerEx(ref SphereCount, 5, 41);
        stream.rwEObject(TextControl, 42);
        stream.rwEObject(CampaignSettings, 43);
        // { Original stores the hero control twice. }
        stream.rwEObject(HeroControl, 44);
        stream.rwData(Bytes.Of(ref VictoryCondition, 1), 1, 45);
        stream.rwInteger(ref VictoryMana, 46);
        stream.rwInteger(ref VictoryGold, 47);
        stream.rwBool(ref FixedLeaders, 48);
        stream.rwBool(ref AlliedVictory, 49);
        stream.rwEObject(AIGroupManager, 50);
        stream.rwInteger(ref DayLimit, 51);
        stream.rwInteger(ref TurnTime, 52);
        stream.rwBool(ref UnlimitedMoves, 53);
        stream.rwEObject(StructureControl, 54);
        if (stream.StorageMode().Contains(StorageModeElement.Read) || ConnectionType != PlayerConnectionType.Network)
        {
            stream.rwInteger(ref TurnTimeElapsed, 55);
        }

        stream.rwEObject(TutorialControl, 56);
        if (stream.StorageMode().Contains(StorageModeElement.Read) || ConnectionType != PlayerConnectionType.Network)
        {
            stream.rwInteger(ref TurnTimeBeep, 58);
        }

        stream.rwBool(ref LeadersFixed, 59);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly) && !stream.StorageMode().Contains(StorageModeElement.Preview) && ConnectionType != PlayerConnectionType.Network)
        {
            stream.rwEObject(GameSongs, 60);
        }

        stream.rwBool(ref StartingGame, 61);
        stream.rwBool(ref ShowAllPlayers, 62);
        stream.rwData(Bytes.Of(ref VictoryMode, 1), 1, 63);
        RandSeed = DefendersSeed;
    }

    // AoWHSMap.PAS:2241
    public override void MainReadWrite(EStorageStream stream)
    {
        string fileName;
        base.MainReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Preview, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read) && Day == 0)
        {
            if (GameType == GameType.Campaign)
            {
                fileName = GameTables.GameEngine.StartupDirectory + "Dict\\" + "CampMaps" + ".mld";
            }
            else
            {
                fileName = GameTables.GameEngine.StartupDirectory + "Dict\\" + "CustMaps" + ".mld";
            }

            if (FileExists(fileName))
            {
                GameTables.GameEngine.TranslateEngineObjectFromFile(this, fileName);
            }
        }
    }

    // AoWHSMap.PAS:1238
    public AoWHSMap(ECustomNode owner) : base(owner)
    {
        ItemControl = new ItemControl(null);
        TokenManager = new AoWTokenManager(GameTables.GameEngine);
        TokenManager.TokenExecutor.OnActivateTacticalBattle += TokenControlActivatedEvent;
        TokenManager.TokenExecutor.OnExecuteTokenEvent = OnExecuteTokenEvent;
        TokenManager.OnMessage = OnTokenManagerMessage;
        PlayerCommunicator = new PlayerCommunicator(null);
        PlayerCommunicator.Engine = GameTables.GameEngine;
        ConnectionType = PlayerConnectionType.Local;
        AIExecutor = new AIExecutor(null);
        AIGroupManager = new AIGroupManager(null);
        AIPlayerActionManager = new AIPlayerActionManager(null);
        AIPlayerActionManager.RegisterClasses([AIPlayerActionClass.Of<EndTurnAIPlayerAction>(), AIPlayerActionClass.Of<GroupControlAIPlayerAction>(), AIPlayerActionClass.Of<GroupControlLoopAIPlayerAction>(), AIPlayerActionClass.Of<InitializeAIPlayerAction>(), AIPlayerActionClass.Of<BudgetManagerAIPlayerAction>(), AIPlayerActionClass.Of<AutoProduceAIPlayerAction>(), AIPlayerActionClass.Of<DiplomacyAIPlayerAction>(), AIPlayerActionClass.Of<ProcessEventLogAIPlayerAction>(), AIPlayerActionClass.Of<ResearchSpellAIPlayerAction>(), AIPlayerActionClass.Of<CastSpellAIPlayerAction>(), AIPlayerActionClass.Of<StructurePhase1AIPlayerAction>(), AIPlayerActionClass.Of<StructurePhase2AIPlayerAction>()]);
        TokenManager.TokenExecutor.OnEventsExecuted += AIExecutor.TokenControlDoneEvent;
        AIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<HuntAIGroup>()]);
        PlayMode = PlayMode.None;
        Keyboard = new HexSpriteMapKeyboard(null);
        TargetSelection = new TargetSelectionControl();
        Effects = new List<WorldEffectHexSprite>();
        DefendersSeed = 0;
        HeroCountMax = 5;
        SelectedArmy = new SelectedArmy();
        ((GameMapMouse)Mouse).FRectangleSelectEvents += SelectedArmy.RectangleSelectEvent;
        MoveControl = new MoveControl(this);
        GlobalMagicControl = new GlobalMagicControl(null);
        MapName = "noname";
        Author = "";
        TurnTimerStart = -1;
        SphereCount = 5;
        VictoryGold = 5000;
        VictoryMana = 5000;
        Description = new StringList();
        Objectives = new StringList();
        IndependentAIMode = AoWHSMapIndependentAIMode.Normal;
        Players = new PlayerList(null);
        Races = new RaceList(null);
        _playerChangedEventsStore = new();
        FixedLeaders = true;
        AlliedVictory = false;
        CurrentPlayer = 0;
        UseTurnTimer = true;
        ExecuteEventLogBuffer = new EventLogList(null);
        DebugSave = false;
        TutorialControl = new TutorialControl(null);
        TurnOrderMode = AoWHSMapTurnOrderMode.Fixed;
        TurnOrder = new ByteList();
        Day = 0;
        GameStatus = AoWHSMapGameStatus.Idle;
        GameResult = AoWHSMapGameResult.None;
        SessionName = "";
        var detailLevel = GameTables.GameEngine.Registry.GetDetailLevel();
        ShowUnitShadows = Set<DetailLevel>.Of(DetailLevel.Normal, DetailLevel.High).Contains(detailLevel);
        HeroControl = new HeroControl(null);
        UnitControl = new UnitControl(null);
        StructureControl = new StructureControl(null);
        _onLeaderChangedStore = new();
        TextControl = new TextControl(null);
        _newDayEventsStore = new();
        _newTurnEventsStore = new();
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
        FUnitChangedEvents += TargetSelection.UnitChanged;
        FArmyDestroyedEvents += SelectedArmy.OnDestroyArmy;
        ExecuteEventLogCallBacks = new ExecuteEventLogCallbackList();
        _cosmeticSurgeryEventsStore = new();
        _onActivateAbilityStore = new();
        StartupSongs = new StartupSongList(null);
        GameSongs = new SongList(null);
        _playerTerminatedEventsStore = new();
        _gameOverEventsStore = new();
        _gameTerminatedEventsStore = new();
        _diplomaticRelationsChangedEventsStore = new();
        _updateVisibilityEventsStore = new();
        _combatMapCreatedEventsStore = new();
        _combatMapDestroyedEventsStore = new();
        _exitEventsStore = new();
        CampaignSettings = new MapCampaignSettings(null);
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
        if (OnProgress != null)
        {
            defaultText = AbilityRules.TranslateResourceString(s_oneMomentPleaseText);
            switch (progress.ProgressType)
            {
                case MapProgressType.Loading:
                    total = progress.Percent + progress.Level * 100;
                    percent = Round((double)((double)total / 3) / MapContainer.LevelCount);
                    if (GameTables.GameEngine.Debug)
                    {
                        text = AbilityRules.TranslateResourceString(s_loadingLevelText) + IntToStr(progress.Level);
                    }
                    else
                    {
                        text = defaultText;
                    }

                    OnProgress(this, text, percent);
                    break;
                case MapProgressType.Initializing:
                    total = progress.Percent + progress.Level * 100;
                    percent = Round((double)((double)((double)total / 4.5) / MapContainer.LevelCount) + (double)100 / 3);
                    if (GameTables.GameEngine.Debug)
                    {
                        text = AbilityRules.TranslateResourceString(s_initializingLevelText) + IntToStr(progress.Level);
                    }
                    else
                    {
                        text = defaultText;
                    }

                    OnProgress(this, text, percent);
                    break;
                case MapProgressType.UpdatingTerrain:
                    total = progress.Percent + progress.Level * 100;
                    percent = Round((double)((double)((double)total / 4.5) / MapContainer.LevelCount) + (double)500 / 9);
                    if (GameTables.GameEngine.Debug)
                    {
                        text = AbilityRules.TranslateResourceString(s_updatingTerrainLevelText) + IntToStr(progress.Level);
                    }
                    else
                    {
                        text = defaultText;
                    }

                    OnProgress(this, text, percent);
                    break;
                case MapProgressType.Activating:
                    total = progress.Percent + progress.Level * 100;
                    percent = Round((double)((double)((double)total / 4.5) / MapContainer.LevelCount) + (double)700 / 9);
                    if (GameTables.GameEngine.Debug)
                    {
                        text = AbilityRules.TranslateResourceString(s_activatingLevelText) + IntToStr(progress.Level);
                    }
                    else
                    {
                        text = defaultText;
                    }

                    OnProgress(this, text, percent);
                    break;
                case MapProgressType.Saving:
                    var savingLevel = AbilityRules.TranslateResourceString(s_savingLevelText);
                    OnProgress(this, savingLevel + IntToStr(progress.Level), progress.Percent);
                    break;
                case MapProgressType.Destroying:
                    var translatedText = AbilityRules.TranslateResourceString(s_destroyingLevelText);
                    OnProgress(this, translatedText + IntToStr(progress.Level), progress.Percent);
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

        for (int I = 0, last = AoWHSSet.Current.ItemLibraries.SystemLibrary.Count - 1; I <= last; I++)
        {
            objectValue = AoWHSSet.Current.ItemLibraries.SystemLibrary.GetItem(I).Copy(GameTables.GameEngine);
            ItemControl.RegisterItem((Item)objectValue);
            objectValue.Release();
        }

        for (int I = 0, last = AoWHSSet.Current.HeroLibraries.SystemLibrary.Count - 1; I <= last; I++)
        {
            objectValue = AoWHSSet.Current.HeroLibraries.SystemLibrary.GetHero(I).Copy(GameTables.GameEngine);
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
                var fileNotFound = AbilityRules.TranslateResourceString(GameText.s_fileNotFoundText);
                if (CompareText(Current.StartupSongs.SongNames[I], fileNotFound) != 0)
                {
                    var item = Current.StartupSongs.Songs[I];
                    GameSongs.Songs.Add(item);
                }
            }
        }

        GameStatus = AoWHSMapGameStatus.Playing;
        DebugSave = GameTables.GameEngine.Registry.GetAutoDebugSave();
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

        if (GameTables.GameEngine.Registry.GetLogTokenEvents())
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
        CustomCombatMap combatMap;
        base.NewFrame();
        if (Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            PlayerControl.NewFrame();
            PlayerCommunicator.Process();
            if (CombatMap == null && GameStatus != AoWHSMapGameStatus.Terminated)
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
        GameStatus = AoWHSMapGameStatus.Terminated;
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
        if (TurnOrderMode == AoWHSMapTurnOrderMode.Fixed)
        {
            for (int I = 0, last = Players.Count - 1; I <= last; I++)
            {
                TurnOrder.Add((byte)I);
            }
        }

        if (TurnOrderMode == AoWHSMapTurnOrderMode.Random)
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
            ExecuteEventLogCallBacks.Add(info);
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
        SynchronizeCount += 1;
    }

    // AoWHSMap.PAS:1481
    public void SynchronizeEnd()
    {
        SynchronizeCount -= 1;
    }

    // AoWHSMap.PAS:1486
    public bool GetSynchronized()
    {
        bool result;
        if (SynchronizeCount == 0 && !TokenManager.TokenExecutor.Executing && !Status.Contains(HexSpriteMapStatusElement.Editting))
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
    public AoWHSMapIndependentAIMode GetIndependentAIMode()
    {
        AoWHSMapIndependentAIMode result = IndependentAIMode;
        if (!Current.Status.Contains(HexSpriteMapStatusElement.Editting) && Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentType.FireMastery))
        {
            if (result == AoWHSMapIndependentAIMode.Off)
            {
                result = AoWHSMapIndependentAIMode.Normal;
            }
            else
            {
                result = AoWHSMapIndependentAIMode.Aggressive;
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
            eventValue = new SaveGameTokenEvent(null);
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
        AoWMapField field;
        HexNumberWalkData walk = default;
        // { Original returns true immediately when exploration is enabled. }
        bool result = Current.ExplorationEnabled;
        mapLevel = MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
        while (walk.Radius <= radius && !result)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                field = (AoWMapField)mapLevel.GetField(walk.X, walk.Y);
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
        AoWMapField field;
        HexNumberWalkData walk = default;
        bool result = false;
        mapLevel = MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
        while (walk.Radius <= radius && !result)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                field = (AoWMapField)mapLevel.GetField(walk.X, walk.Y);
                result = (Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref field.FieldFlags).Hi).Lo != 0 || !Current.FogEnabled) && (field.GetExplored(Current.SeatedPlayer) || !Current.ExplorationEnabled);
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
        AoWMapField field;
        HexNumberWalkData walk = default;
        bool result = false;
        mapLevel = MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
        while (walk.Radius <= radius && !result)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                field = (AoWMapField)mapLevel.GetField(walk.X, walk.Y);
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
        AoWMapField field;
        HexNumberWalkData walk = default;
        bool result = false;
        mapLevel = MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
        while (walk.Radius <= radius && !result)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                field = (AoWMapField)mapLevel.GetField(walk.X, walk.Y);
                result = (Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref field.FieldFlags).Hi).Lo != 0 || !Current.FogEnabled) && (field.GetExplored(Current.SeatedPlayer) || !Current.ExplorationEnabled) && Viewing(walk.X, walk.Y, L);
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
        AoWMapField field;
        HexNumberWalkData walk = default;
        int wx;
        if (ShowIndependentMoves && player != Current.SeatedPlayer)
        {
            result = false;
            mapLevel = MapContainer.GetMapLevel(L);
            HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
            while (walk.Radius <= radius && !result)
            {
                wx = walk.X;
                if (wx >= 0 && walk.Y >= 0 && wx < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    field = (AoWMapField)mapLevel.GetField(wx, walk.Y);
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
                HexGeometry.HexToPixel(army.X, army.Y, ref pixelX, ref pixelY);
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
                HexGeometry.HexToPixel(army.X, army.Y, ref pixelX, ref pixelY);
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
                HexGeometry.HexToPixel(army.X, army.Y, ref pixelX, ref pixelY);
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
    public void SeatedPlayerChanged()
    {
        EngineMessage message = default;
        Selection.UnselectAll();
        EngineMessage.InitializeMessage(ref message, 537001990, this);
        MapContainer.SendMessage(ref message, this);
        EngineMessage.InitializeMessage(ref message, 537001993, this);
        MapContainer.SendMessage(ref message, this);
        _seatedPlayerChangedEventsStore.Raise(Current);
        ShowMessage("");
    }

    // AoWHSMap.PAS:1822
    public void BeginUnitUpdate()
    {
        UnitUpdateCount += 1;
    }

    // AoWHSMap.PAS:1827
    public void EndUnitUpdate()
    {
        UnitUpdateCount -= 1;
        if (UnitUpdateCount == 0)
        {
            UnitChanged(null);
        }
    }

    // AoWHSMap.PAS:1833
    public void UnitChanged(Troop troop)
    {
        if (UnitUpdateCount == 0)
        {
            _unitChangedEventsStore.Raise(troop);
        }
    }

    // AoWHSMap.PAS:1838
    public void UnitDestroyed(Troop troop)
    {
        if (!Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            _unitDestroyedEventsStore.Raise(troop);
        }
    }

    // AoWHSMap.PAS:1844
    public void BeginArmyUpdate()
    {
        ArmyUpdateCount += 1;
    }

    // AoWHSMap.PAS:1849
    public void EndArmyUpdate()
    {
        ArmyUpdateCount -= 1;
        if (ArmyUpdateCount == 0)
        {
            ArmyChanged(null);
        }
    }

    // AoWHSMap.PAS:1855
    public void ArmyChanged(Army army)
    {
        if (ArmyUpdateCount == 0)
        {
            _armyChangedEventsStore.Raise(army);
        }
    }

    // AoWHSMap.PAS:1860
    public void ArmyDestroyed(Army army)
    {
        // TArmy.Destroy passes its Self pointer
        _armyDestroyedEventsStore.Raise(army);
    }

    // AoWHSMap.PAS:1866
    // TArmy.Destroy passes TArmy
    public void BeginStructureUpdate()
    {
        StructureUpdateCount += 1;
    }

    // AoWHSMap.PAS:1871
    public void EndStructureUpdate()
    {
        StructureUpdateCount -= 1;
        if (StructureUpdateCount == 0)
        {
            StructureChanged(null);
        }
    }

    // AoWHSMap.PAS:1877
    public void StructureChanged(Structure structure)
    {
        if (StructureUpdateCount == 0)
        {
            _structureChangedEventsStore.Raise(structure);
        }
    }

    // AoWHSMap.PAS:1882
    public void StructureDestroyed(Structure structure)
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
        while (ExecuteEventLogCallBacks.GetCount() > 0)
        {
            if (ExecuteEventLogCallBacks.GetItem(0).Done != null)
            {
                var item = ExecuteEventLogCallBacks.GetItem(1);
                ExecuteEventLogCallBacks.GetItem(0).Done(item.Log);
            }

            // { Original passes the second record's log to the first callback, even for a one-item list. }
            ExecuteEventLogCallBacks.Delete(0);
        }
    }

    // AoWHSMap.PAS:1937
    public void ExecuteNextEventLog()
    {
        var notification = new ExecuteEventLogNotification();
        ref var info = ref notification.Info;
        if (ExecuteEventLogCallBacks.GetCount() != 0)
        {
            AILockCount += 1;
            info = ExecuteEventLogCallBacks.GetItem(0);
            info.Done = ExecuteEventLogDone;
            _executeEventLogEventsStore.Raise(notification);
        }
    }

    // AoWHSMap.PAS:1950
    public void SetExecuteEventLogDelay(int value)
    {
        if (ExecuteEventLogDelay == 0)
        {
            ExecuteEventLogDelay = value;
            LockExecuteEventLog();
        }
        else
        {
            if (value > ExecuteEventLogDelay)
            {
                ExecuteEventLogDelay = value;
            }
        }
    }

    // AoWHSMap.PAS:1960
    public void UpdateExecuteEventLogDelay()
    {
        if (ExecuteEventLogDelay != 0)
        {
            ExecuteEventLogDelay -= 1;
            if (ExecuteEventLogDelay == 0)
            {
                UnlockExecuteEventLog();
            }
        }
    }

    // AoWHSMap.PAS:1969
    public void ExecuteEventLogDone(EventLog log)
    {
        ExecuteEventLogInfo info;
        if (ExecuteEventLogCallBacks.GetCount() > 0 && log == ExecuteEventLogCallBacks.GetItem(0).Log)
        {
            info = ExecuteEventLogCallBacks.GetItem(0);
            ExecuteEventLogCallBacks.Delete(0);
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
                log = (GameOverEventLog)eventValue.Copy(GameTables.GameEngine);
                if (GameOverLockCount == 0)
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
                    ExecuteEventLogBuffer.AddEvent(log);
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
        GameOverLockCount -= 1;
        if (GameOverLockCount == 0)
        {
            while (ExecuteEventLogBuffer.Count != 0)
            {
                player = ((GameOverEventLog)ExecuteEventLogBuffer.GetEvents(0)).TargetPlayer;
                log = (EventLog)ExecuteEventLogBuffer.GetEvents(0).Copy(GameTables.GameEngine);
                Current.Players.GetPlayers(player).EventLogbook.AddEvent(log);
                if (player == Current.SeatedPlayer)
                {
                    log.Execute(false, ExecuteTrigger.Event, null);
                }

                log.Release();
                var events = ExecuteEventLogBuffer.GetEvents(0);
                ExecuteEventLogBuffer.RemoveEvent(events);
            }
        }
    }

    // AoWHSMap.PAS:2071
    public void LockGameOverEvent()
    {
        GameOverLockCount += 1;
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
        EngineMessage.InitializeMessage(ref message, 537002243, this);
        message.Payload = messages;
        message.Data[0] = 0;
        Current.SendMessage(ref message, this);
        bool result = message.Data[0] == 0;
        if (result)
        {
            messages.Add(AbilityRules.TranslateResourceString(s_mapSuccessfullyValidatedText));
        }
        else
        {
            messages.Add(AbilityRules.TranslateResourceString(s_mapValidationFailedText));
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
        AoWMapField field;
        mapLevel = MapContainer.GetMapLevel(L);
        message.MessageId = 4365;
        message.Result = 1;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                field = (AoWMapField)mapLevel.GetField(walk.X, walk.Y);
                if ((Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref field.FieldFlags).Hi).Hi & 1) != 0)
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
        AoWMapField field;
        mapLevel = MapContainer.GetMapLevel(L);
        message.MessageId = 4400;
        message.Param[0] = player;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
        while (walk.Radius <= sightRadius || walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                field = (AoWMapField)mapLevel.GetField(walk.X, walk.Y);
                if ((Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref field.FieldFlags).Hi).Hi & 2) != 0)
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
        AoWMapField field;
        mapLevel = MapContainer.GetMapLevel(level);
        if (player >= 0)
        {
            message.MessageId = 4362;
            message.Result = 0;
            message.Param[1] = player;
            HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
            while (message.Result.Int == 0 && walk.Radius <= 12)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    field = (AoWMapField)mapLevel.GetField(walk.X, walk.Y);
                    if ((Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref field.FieldFlags).Lo).Hi & 16) != 0)
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
        AoWMapField field;
        mapLevel = MapContainer.GetMapLevel(L);
        if (!ExplorationEnabled || ((AoWMapField)mapLevel.GetField(X, Y)).GetExplored(player))
        {
            if (FogEnabled)
            {
                if (player != Current.SeatedPlayer)
                {
                    message.MessageId = 4362;
                    message.Result = 0;
                    message.Param[1] = player;
                    HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
                    while (message.Result.Int == 0 && walk.Radius <= 12)
                    {
                        if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                        {
                            field = (AoWMapField)mapLevel.GetField(walk.X, walk.Y);
                            if ((Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref field.FieldFlags).Lo).Hi & 16) != 0)
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
                    result = Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((AoWMapField)fieldValue).FieldFlags).Hi).Lo == 0;
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
        AoWMapField field;
        // The original tests Xhx only and with Yhx -1 read memory before the field block. The
        // port treats that point as off the map.
        if (Mouse.Capture == null && Mouse.HexX != -1 && Mouse.HexY != -1)
        {
            var hexY = Mouse.HexY;
            var hexX = Mouse.HexX;
            field = (AoWMapField)MapContainer.GetField(hexX, hexY, _viewL);
            if (Current.ExplorationEnabled && !field.GetExplored(Current.SeatedPlayer))
            {
                return;
            }

            if (!Current.FogEnabled || Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref field.FieldFlags).Hi).Lo != 0)
            {
                var pixelY = Mouse.PixelY;
                var pixelX = Mouse.PixelX;
                if (Selection.CanSelectLocation(pixelX, pixelY, _viewL) && !((GameMapMouse)Mouse).GetSelectingRectangle())
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
        int mapCoordinate = default;
        if (SeatedPlayer != -1)
        {
            player = Players.GetPlayers(SeatedPlayer);
            if ((sbyte)player.ViewX > 0)
            {
                Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Lo).Lo = player.ViewX;
                Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Lo).Hi = player.ViewY;
                Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Hi).Lo = player.ViewL;
            }
            else
            {
                player.GetLeaderLocation(ref Unsafe.As<byte, sbyte>(ref Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Lo).Lo), ref Unsafe.As<byte, sbyte>(ref Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Lo).Hi), ref Unsafe.As<byte, sbyte>(ref Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Hi).Lo));
                if ((sbyte)Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Lo).Lo == -1)
                {
                    if (player.Armies.Count == 0)
                    {
                        Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Lo).Lo = 0;
                        Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Lo).Hi = 0;
                        Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Hi).Lo = 0;
                    }
                    else
                    {
                        army = player.Armies.GetArmy(0);
                        Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Lo).Lo = (byte)army.X;
                        Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Lo).Hi = (byte)army.Y;
                        Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Hi).Lo = (byte)army.MapLevel;
                    }
                }
            }

            CenterViewTo((sbyte)Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Lo).Lo, (sbyte)Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Lo).Hi, (sbyte)Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref mapCoordinate).Hi).Lo);
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
    public void PlayerChanged()
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
                PlayerChanged();
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
                        PlayerControl = new PbemPlayerControl(null);
                    }
                }
                else
                {
                    // { Original tests TPBEMPlayerControl even in the non-email branch. }
                    if (!(PlayerControl is PbemPlayerControl))
                    {
                        PlayerControl?.Dispose();
                        PlayerControl = new TurnPlayerControl(null);
                    }
                }

                break;
            case PlayMode.Simultaneous:
                if (!(PlayerControl is SimultaneousPlayerControl))
                {
                    PlayerControl?.Dispose();
                }

                // { Original replaces an existing control without freeing it. }
                PlayerControl = new SimultaneousPlayerControl(null);
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
            if (!RequestCombatType || ExecuteEventLogCallBacks.GetCount() == 0)
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
        UpdateVisibilityLockCount += 1;
    }

    // AoWHSMap.PAS:2790
    public void UnLockUpdateVisibility()
    {
        UpdateVisibilityLockCount -= 1;
        if (UpdateVisibilityLockCount == 0 && VisibilityChanged)
        {
            UpdateVisibility();
        }
    }

    // AoWHSMap.PAS:2797
    public void UpdateVisibility()
    {
        EngineMessage message = default;
        if (UpdateVisibilityLockCount == 0)
        {
            if (!Status.Contains(HexSpriteMapStatusElement.Destroying))
            {
                LockMapChangedEvent();
                EngineMessage.InitializeMessage(ref message, 537001992, this);
                Current.SendMessage(ref message, this);
                UnlockMapChangedEvent();
                Current.InvalidateMap();
            }

            VisibilityChanged = false;
        }
        else
        {
            VisibilityChanged = true;
        }
    }

    // AoWHSMap.PAS:2817
    public void ResetMapLevels()
    {
        if (MapContainer != null)
        {
            for (int I = 0, last = MapContainer.LevelCount - 1; I <= last; I++)
            {
                ((AoWMapLevel)MapContainer.GetMapLevel(I)).Reset();
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
                GetViewCenter(ref X, ref Y, ref L);
                player.ViewX = (byte)X;
                player.ViewY = (byte)Y;
                player.ViewL = (byte)L;
            }

            CombatMap = (CustomCombatMap)GameTables.GameEngine.LoadHsmMap(FileBoundary.ReadName(GameTables.GameEngine.StartupDirectory + fileName), false, null, GameProfile.TacticalMap);
            _combatMapCreatedEventsStore.Raise(this);
        }
    }

    // AoWHSMap.PAS:2891
    public void ReleaseCombatMap()
    {
        if (CombatMap != null)
        {
            _combatMapDestroyedEventsStore.Raise(this);
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
            result = (Battle)GameTables.GameEngine.CreateEngineObject(classId, null);
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
                log = new NewTurnEventLog(null);
                log.TurnPlayer = CurrentPlayer;
                log.Mode = NewTurnEventLogMode.Dialog;
                log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                log.Text = AbilityRules.TranslateResourceString(GameText.s_yourTurnContinuesText);
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
        if (Status.Contains(HexSpriteMapStatusElement.Activated) && RelationsChangedLockCount == 0)
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
        RelationsChangedLockCount += 1;
    }

    // AoWHSMap.PAS:3044
    public void UnlockRelationsChanged()
    {
        Assert(RelationsChangedLockCount != 0, "Invalid MapChanged Lock usage", "AoWHSMap.PAS", 3056);
        RelationsChangedLockCount -= 1;
    }

    // AoWHSMap.PAS:3061
    public void UpdateTurnTimeBeep()
    {
        int seconds;
        if (GetTurnTimerEnabled())
        {
            seconds = GetTurnTimeLeft() / 1000;
            if (TurnTimeBeep != 0 && seconds < TurnTimeBeep && CombatMap == null)
            {
                TurnTimeBeep = seconds;
                if (TurnTimeBeep == 0)
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
        TurnTimeElapsed = TurnTime * 1000 * 60;
        TurnTimeBeep = 11;
        TurnTimerStart = -1;
    }

    // AoWHSMap.PAS:3086
    public int GetTurnTimeLeft()
    {
        int time;
        int left;
        left = TurnTimeElapsed;
        if (TurnTimerStart != -1)
        {
            time = PixelFormats.GetElapsedMilliseconds();
            if (time >= TurnTimerStart)
            {
                left = left - (time - TurnTimerStart);
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
    public bool GetTurnTimerEnabled() => TurnTime > 0 && TurnTimerStart != -1;

    // AoWHSMap.PAS:3108
    public void EnableTurnTimer()
    {
        if (TurnTimerStart == -1 && TurnTime > 0)
        {
            TurnTimerStart = PixelFormats.GetElapsedMilliseconds();
        }
    }

    // AoWHSMap.PAS:3114
    public void DisableTurnTimer()
    {
        int time;
        time = PixelFormats.GetElapsedMilliseconds() - TurnTimerStart;
        if (time > 0)
        {
            TurnTimeElapsed -= time;
        }

        TurnTimerStart = -1;
    }

    // AoWHSMap.PAS:3124
    public void ZeroTurnTimer()
    {
        TurnTimeElapsed = 0;
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
            Current.Players.GetPlayers(player).Leader.GetLocation(ref X, ref Y, ref L);
            // { Original excludes X = 0. }
            if (X > 0)
            {
                Current.CenterViewTo(X, Y, L);
                if (animate)
                {
                    Current.PlayCenterAnimation(X, Y, L);
                }

                army = ((AoWMapField)Current.MapContainer.GetField(X, Y, L)).GetArmyHexSprite();
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
        EngineObjectList resources;
        DelphiList candidates;
        MapObjectResource resource;
        RandSeed = Current.Random(16777215);
        resources = AoWHSSet.Current.MapObjectResourceList._children;
        if (!Set<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)overlayType))
        {
            candidates = new DelphiList();
            candidates.Clear();
            for (int I = 0, last = resources.Count - 1; I <= last; I++)
            {
                resource = (MapObjectResource)resources.List[I];
                if (resource.EngineClass.ClassId == 131431 && resource.TerrainList.UsedTerrain() == 1 && resource.TerrainList.Terrain[0] == terrainType && resource.TerrainList.Overlay[0] == overlayType)
                {
                    candidates.Add(resource);
                }
            }

            if (candidates.Count != 0)
            {
                ((MapObjectResource)candidates[Random(candidates.Count)]).PlaceResourceUser(Current, X, Y, L);
            }

            candidates?.Dispose();
        }
        else
        {
            AoWHSSet.Current.RoadControl.Place(Current.MapContainer, X, Y, L, terrainType);
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
        effect = new WorldEffectHexSprite(null);
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
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, x, y, 0);
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
                    var field = (AoWMapField)mapLevel.GetField(walk.X, walk.Y);
                    result = (((field.FieldFlags >> 16) & 0xff) != 0 || !Current.FogEnabled) &&
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

    private static bool TryEndPlayerTurn(sbyte player)
    {
        bool result;
        if (player != -1 && !Current.Players.GetPlayers(player).GetBusy() && !Current.Players.GetPlayers(player).SingleProcessCycle)
        {
            result = AbilityRules.EndPlayerTurn(player);
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static bool CancelPlayerTurn(sbyte player)
    {
        bool result;
        EndTurnTokenEvent eventValue;
        if (!Current.Players.GetPlayers(player).GetBusy() && Current.Players.GetPlayers(player).SingleProcessCycle)
        {
            eventValue = new EndTurnTokenEvent(null);
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

    public static AoWHSMap Current;

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
        public override void ReadWrite(EStorageStream stream)
        {
            base.ReadWrite(stream);
            stream.rwInteger(ref TransferPointsMax, 20);
            stream.rwInteger(ref TransferGoldMax, 21);
            stream.rwInteger(ref TransferManaMax, 22);
            stream.rwInteger(ref HeroLevelMax, 23);
            stream.rwBool(ref AlliedVictory, 24);
        }

        // intf_P5_08.inc:13

    }

}

public enum AoWHSMapTurnOrderMode : byte
{
    Fixed = 0, Random = 1, Changing = 2
}

public enum AoWHSMapVictoryCondition : byte
{
    None = 0, CaptureLocations = 1, AccumulateGold = 2, AccumulateMana = 3
}

public enum AoWHSMapGameResult : byte
{
    None = 0, Failed = 1, Succeeded = 2
}

public enum AoWHSMapGameStatus : byte
{
    Idle = 0, Playing = 1, Terminated = 2
}

public enum AoWHSMapIndependentAIMode : byte
{
    Off = 0, Passive = 1, Normal = 2, Aggressive = 3
}
