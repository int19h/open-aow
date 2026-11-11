// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;

using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
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
using Aow.Game.Ui.Controls;
using Aow.Game.Units;

using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

// map name, heroes, game mode, stream mode, fixed leaders, allied victory, day limit, turn
// order, day/turn events, event-log locks, diplomacy events, initial seed, random seed, debug
// save, communicator, token log Text, logging enabled
public partial class AoWHSMap : AbstractAoWHSMap
{

    public new const int ClassID = 131089;

    public SelectedArmy FSelectedArmy;
    public bool FIgnoreStreamErrors;
    public TargetSelectionControl FTargetSelection;
    public string FMapName = "";
    public string FAuthor = "";
    public int FSphereCount;
    public StringList FDescription;
    public ItemControl FItemControl;
    public HeroControl FHeroControl;
    public UnitControl FUnitControl;
    public StructureControl FStructureControl;
    public IndependentAIMode FIndependentAIMode;
    public StringList FObjectives;
    public sbyte FHeroCountMax;
    public StartupSongList FStartupSongs;
    public Songlist FGameSongs;
    public GameStatus FGameStatus;
    public GameResult FGameResult;
    public GameType FGameType;
    public AbstractAoWCombatMap FCombatMap;
    public Combat.Battle FCombat;
    public TutorialControl FTutorialControl;
    public AIExecuter FAIExecuter;
    public AIPlayerActionManager FAIPlayerActionManager;
    public AIGroupManager FAIGroupManager;
    public AoWHSMapKeyboard FKeyboard;
    public bool FUnlimitedMoves;
    public bool FRequestCombatType;
    public PlayerConnectionType FConnectionType;
    public TextControl FTextControl;
    public PlayerList FPlayers;
    public RaceList FRaces;
    public bool FFixedLeaders;
    public bool FLeadersFixed;
    public bool FStartingGame;
    public bool FAlliedVictory;
    public byte FVictoryMode;
    public VictoryCondition FVictoryCondition;
    public int FVictoryMana;
    public int FVictoryGold;
    public int FDayLimit;
    public int FTurnTime;
    public int FTurnTimeElapsed;
    public int FTurnTimeBeep;
    public int FTurnTimerStart;
    public TurnOrderMode FTurnOrderMode;
    public ByteList FTurnOrder;
    public int FDay;
    public string FSessionName = "";

    internal EventStore<Leader> FOnLeaderChangedStore;

    public event NotifyEvent<Leader> FOnLeaderChanged
    {
        add => FOnLeaderChangedStore.Add(value);
        remove => FOnLeaderChangedStore.Remove(value);
    }

    public bool FShowIndependentMoves;
    public MoveControl FMoveControl;
    public GlobalMagicControl FGlobalMagicControl;
    public AoWMapCampaignSettings FCampaignSettings;
    public sbyte FEditorPlayer;
    public bool FShowAllPlayers;
    public int FRelationsChangedLockCount;

    internal EventStore<AoWHSMap> FPlayerChangedEventsStore;

    public event NotifyEvent<AoWHSMap> FPlayerChangedEvents
    {
        add => FPlayerChangedEventsStore.Add(value);
        remove => FPlayerChangedEventsStore.Remove(value);
    }

    internal EventStore<PlayerControl> FNewDayEventsStore;

    public event NotifyEvent<PlayerControl> FNewDayEvents
    {
        add => FNewDayEventsStore.Add(value);
        remove => FNewDayEventsStore.Remove(value);
    }

    internal EventStore<sbyte> FNewTurnEventsStore;

    public event NotifyEvent<sbyte> FNewTurnEvents
    {
        add => FNewTurnEventsStore.Add(value);
        remove => FNewTurnEventsStore.Remove(value);
    }

    internal EventStore<sbyte> FEndTurnEventsStore;

    public event NotifyEvent<sbyte> FEndTurnEvents
    {
        add => FEndTurnEventsStore.Add(value);
        remove => FEndTurnEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> FCurrentPlayerChangedEventsStore;

    public event NotifyEvent<AoWHSMap> FCurrentPlayerChangedEvents
    {
        add => FCurrentPlayerChangedEventsStore.Add(value);
        remove => FCurrentPlayerChangedEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> FSeatedPlayerChangedEventsStore;

    public event NotifyEvent<AoWHSMap> FSeatedPlayerChangedEvents
    {
        add => FSeatedPlayerChangedEventsStore.Add(value);
        remove => FSeatedPlayerChangedEventsStore.Remove(value);
    }

    internal EventStore<Combat.Battle> FCombatEventsStore;

    public event NotifyEvent<Combat.Battle> FCombatEvents
    {
        add => FCombatEventsStore.Add(value);
        remove => FCombatEventsStore.Remove(value);
    }

    internal EventStore<sbyte> FPlayerTerminatedEventsStore;

    public event NotifyEvent<sbyte> FPlayerTerminatedEvents
    {
        add => FPlayerTerminatedEventsStore.Add(value);
        remove => FPlayerTerminatedEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> FGameOverEventsStore;

    public event NotifyEvent<AoWHSMap> FGameOverEvents
    {
        add => FGameOverEventsStore.Add(value);
        remove => FGameOverEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> FGameTerminatedEventsStore;

    public event NotifyEvent<AoWHSMap> FGameTerminatedEvents
    {
        add => FGameTerminatedEventsStore.Add(value);
        remove => FGameTerminatedEventsStore.Remove(value);
    }

    public bool FExitPending;

    internal EventStore<AoWHSMap> FExitEventsStore;

    public event NotifyEvent<AoWHSMap> FExitEvents
    {
        add => FExitEventsStore.Add(value);
        remove => FExitEventsStore.Remove(value);
    }

    public int FGameOverLockCount;
    public EventLogList FExecuteEventLogBuffer;

    internal EventStore<ExecuteEventLogNotification> FExecuteEventLogEventsStore;

    public event NotifyEvent<ExecuteEventLogNotification> FExecuteEventLogEvents
    {
        add => FExecuteEventLogEventsStore.Add(value);
        remove => FExecuteEventLogEventsStore.Remove(value);
    }

    public ExecuteEventLogCallBackList FExecuteEventLogCallBacks;
    public int FAILockCount;
    public int FExecuteEventLogDelay;

    internal EventStore<SpellCaster> FCosmeticSurgeryEventsStore;

    public event NotifyEvent<SpellCaster> FCosmeticSurgeryEvents
    {
        add => FCosmeticSurgeryEventsStore.Add(value);
        remove => FCosmeticSurgeryEventsStore.Remove(value);
    }

    internal EventStore<ActivateAbilityInfo> FOnActivateAbilityStore;

    public event NotifyEvent<ActivateAbilityInfo> FOnActivateAbility
    {
        add => FOnActivateAbilityStore.Add(value);
        remove => FOnActivateAbilityStore.Remove(value);
    }

    internal EventStore<EObject> FDiplomaticRelationsChangedEventsStore;

    public event NotifyEvent<EObject> FDiplomaticRelationsChangedEvents
    {
        add => FDiplomaticRelationsChangedEventsStore.Add(value);
        remove => FDiplomaticRelationsChangedEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> FUpdateVisibilityEventsStore;

    public event NotifyEvent<AoWHSMap> FUpdateVisibilityEvents
    {
        add => FUpdateVisibilityEventsStore.Add(value);
        remove => FUpdateVisibilityEventsStore.Remove(value);
    }

    public List<WorldFXHS> FEffects;
    public int FUpdateVisibilityLockCount;
    public bool FVisibilityChanged;
    public int FArmyUpdateCount;
    public int FStructureUpdateCount;
    public int FUnitUpdateCount;

    internal EventStore<AbstractUnit> FUnitChangedEventsStore;

    public event NotifyEvent<AbstractUnit> FUnitChangedEvents
    {
        add => FUnitChangedEventsStore.Add(value);
        remove => FUnitChangedEventsStore.Remove(value);
    }

    internal EventStore<Army> FArmyChangedEventsStore;

    public event NotifyEvent<Army> FArmyChangedEvents
    {
        add => FArmyChangedEventsStore.Add(value);
        remove => FArmyChangedEventsStore.Remove(value);
    }

    internal EventStore<Structure> FStructureChangedEventsStore;

    public event NotifyEvent<Structure> FStructureChangedEvents
    {
        add => FStructureChangedEventsStore.Add(value);
        remove => FStructureChangedEventsStore.Remove(value);
    }

    internal EventStore<AbstractUnit> FUnitDestroyedEventsStore;

    public event NotifyEvent<AbstractUnit> FUnitDestroyedEvents
    {
        add => FUnitDestroyedEventsStore.Add(value);
        remove => FUnitDestroyedEventsStore.Remove(value);
    }

    internal EventStore<Army> FArmyDestroyedEventsStore;

    public event NotifyEvent<Army> FArmyDestroyedEvents
    {
        add => FArmyDestroyedEventsStore.Add(value);
        remove => FArmyDestroyedEventsStore.Remove(value);
    }

    internal EventStore<Structure> FStructureDestroyedEventsStore;

    public event NotifyEvent<Structure> FStructureDestroyedEvents
    {
        add => FStructureDestroyedEventsStore.Add(value);
        remove => FStructureDestroyedEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> FTokenControlActivatedEventsStore;

    public event NotifyEvent<AoWHSMap> FTokenControlActivatedEvents
    {
        add => FTokenControlActivatedEventsStore.Add(value);
        remove => FTokenControlActivatedEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> FCombatMapCreatedEventsStore;

    public event NotifyEvent<AoWHSMap> FCombatMapCreatedEvents
    {
        add => FCombatMapCreatedEventsStore.Add(value);
        remove => FCombatMapCreatedEventsStore.Remove(value);
    }

    internal EventStore<AoWHSMap> FCombatMapDestroyedEventsStore;

    public event NotifyEvent<AoWHSMap> FCombatMapDestroyedEvents
    {
        add => FCombatMapDestroyedEventsStore.Add(value);
        remove => FCombatMapDestroyedEventsStore.Remove(value);
    }

    public int FInitialRandomSeed;
    public int FDefendersSeed;
    public int FSynchroniseCount;
    public bool FDebugSave;
    public PlayMode FPlayMode;
    public TokenManager FTokenManager;
    public PlayerCommunicator FPlayerCommunicator;
    public PlayerControl FPlayerControl;
    public bool FUseTurnTimer;
    public bool FShowUnitShadows;
    public Text FTokenEventLog;
    public bool FTokenEventLogging;

    // AoWHSMap.PAS:1336
    public override void Dispose()
    {
        StopTokenEventLogging();
        LockMapChangedEvent();
        OnViewLevel = null;
        OnCenterView = null;
        AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "GENERAL.ILB");
        AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "CURSORS.ILB");
        AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "COMBAT.ILB");
        FMoveControl?.Dispose();
        for (int I = FEffects.Count - 1; I >= 0; I--)
        {
            var animation = FEffects[I];
            DynamicFixedVisibleHS.RemoveChild((EObject)animation);
            var fEffectsItem = FEffects[I];
            DynamicVisibleHS.RemoveChild((EObject)fEffectsItem);
            (FEffects[I] as IDisposable)?.Dispose();
        }

        FEffects?.Clear();
        FTutorialControl?.Dispose();
        FPlayers.BeginUpdate();
        FArmyChangedEventsStore.Clear();
        FStructureChangedEventsStore.Clear();
        FUnitChangedEventsStore.Clear();
        FSeatedPlayerChangedEventsStore.Clear();
        FCurrentPlayerChangedEventsStore.Clear();
        FNewTurnEventsStore.Clear();
        FNewDayEventsStore.Clear();
        FOnLeaderChangedStore.Clear();
        FDescription.Clear();
        FObjectives.Clear();
        FTurnOrder.Clear();
        FStructureDestroyedEventsStore.Clear();
        FUnitDestroyedEventsStore.Clear();
        FCombatMapCreatedEventsStore.Clear();
        FCombatMapDestroyedEventsStore.Clear();
        FUpdateVisibilityEventsStore.Clear();
        FDiplomaticRelationsChangedEventsStore.Clear();
        FCosmeticSurgeryEventsStore.Clear();
        FOnActivateAbilityStore.Clear();
        FTokenControlActivatedEventsStore.Clear();
        FPlayerChangedEventsStore.Clear();
        FExitEventsStore.Clear();
        Mouse.OnMove -= MouseMoved;
        if (FCombat != null)
        {
            DestroyCombat();
        }

        if (FCombatMap != null)
        {
            FCombatMap?.Dispose();
            FCombatMap = null;
        }

        base.Dispose();
        FItemControl.Deactivate();
        FGlobalMagicControl?.Dispose();
        FGlobalMagicControl = null;
        FPlayerCommunicator?.Dispose();
        FTokenManager?.Dispose();
        FTokenManager = null;
        FCampaignSettings?.Dispose();
        FAIExecuter?.Dispose();
        FSelectedArmy?.Dispose();
        FSeatedPlayerChangedEventsStore.Clear();
        FExecuteEventLogBuffer?.Dispose();
        FNewTurnEventsStore.Clear();
        FEndTurnEventsStore.Clear();
        FCurrentPlayerChangedEventsStore.Clear();
        FNewDayEventsStore.Clear();
        FArmyChangedEventsStore.Clear();
        FStructureChangedEventsStore.Clear();
        FUnitChangedEventsStore.Clear();
        FArmyDestroyedEventsStore.Clear();
        FStructureDestroyedEventsStore.Clear();
        FUnitDestroyedEventsStore.Clear();
        FOnLeaderChangedStore.Clear();
        FDescription?.Dispose();
        FObjectives?.Dispose();
        FTurnOrder?.Dispose();
        FPlayerControl?.Dispose();
        FTextControl?.Dispose();
        FCombatEventsStore.Clear();
        FPlayerChangedEventsStore.Clear();
        FExecuteEventLogEventsStore.Clear();
        FExecuteEventLogCallBacks?.Dispose();
        FCosmeticSurgeryEventsStore.Clear();
        FOnActivateAbilityStore.Clear();
        FGameSongs?.Dispose();
        FStartupSongs?.Dispose();
        FExitEventsStore.Clear();
        FItemControl?.Dispose();
        FItemControl = null;
        FHeroControl?.Dispose();
        FHeroControl = null;
        FUnitControl?.Dispose();
        FUnitControl = null;
        FStructureControl?.Dispose();
        FStructureControl = null;
        FAIGroupManager?.Dispose();
        FAIGroupManager = null;
        FAIPlayerActionManager?.Dispose();
        FPlayers?.Dispose();
        FRaces?.Dispose();
        FPlayerTerminatedEventsStore.Clear();
        FGameOverEventsStore.Clear();
        FGameTerminatedEventsStore.Clear();
        FTargetSelection?.Dispose();
        FCombatMapCreatedEventsStore.Clear();
        FCombatMapDestroyedEventsStore.Clear();
        FDiplomaticRelationsChangedEventsStore.Clear();
        FKeyboard?.Dispose();
        FUpdateVisibilityEventsStore.Clear();
        FTokenControlActivatedEventsStore.Clear();
        UnlockMapChangedEvent();
    }

    // AoWHSMap.PAS:2160
    public override void ReadWrite(EStorageStream Stream)
    {
        int X = default;
        int Y = default;
        int L = default;
        if (Stream.StorageMode().Contains(StorageModeElement.Read) && !Stream.StorageMode().Contains(StorageModeElement.Translate))
        {
            Stream.ShowProgress(this, AbilityRules.TranslateRStr(OneMomentPleaseStr), 0);
        }

        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Write, StorageModeElement.TextOnly)) == Set<StorageModeElement>.Of(StorageModeElement.Write))
        {
            ResetModified();
        }

        FIgnoreStreamErrors = false;
        if (Status.Contains(HSMapStatusElement.Editting))
        {
            Stream.OnError = OnStreamError;
        }

        Stream.rwData(Bytes.Of(ref FConnectionType, 1), 1, 31);
        if (FConnectionType == PlayerConnectionType.Network)
        {
            X = FViewXhp;
            FViewXhp = 0;
            Y = FViewYhp;
            FViewYhp = 0;
            L = FViewL;
            FViewL = 0;
        }
        else
        {
            X = 0;
            Y = 0;
            L = 0;
        }

        base.ReadWrite(Stream);
        if (FConnectionType == PlayerConnectionType.Network)
        {
            FViewXhp = X;
            FViewYhp = Y;
            FViewL = L;
        }

        Stream.rwString(ref FAuthor, 7);
        Stream.rwStrings(FDescription, 11);
        Stream.rwStrings(FObjectives, 12);
        Stream.rwData(Bytes.Of(ref FIndependentAIMode, 1), 1, 13);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FCurrentPlayer), 16);
        Stream.rwData(Bytes.Of(ref FTurnOrderMode, 1), 1, 17);
        Stream.rwStreamProc(18, FTurnOrder.LoadFromStream, FTurnOrder.SaveToStream);
        Stream.rwInteger(ref FDay, 19);
        Stream.rwString(ref FSessionName, 20);
        Stream.rwEObject(FPlayers, 21);
        Stream.rwEObject(FRaces, 22);
        Stream.rwBool(ref FExplorationEnabled, 23);
        if (!Stream.StorageMode().Contains(StorageModeElement.Preview))
        {
            Stream.rwEObject(FHeroControl, 24);
        }

        Stream.rwString(ref FMapName, 25);
        Stream.rwBool(ref FFogEnabled, 26);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FHeroCountMax), 27);
        Stream.rwInteger(ref FDefendersSeed, 28);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly) && !Stream.StorageMode().Contains(StorageModeElement.Preview) && !Stream.StorageMode().Contains(StorageModeElement.Compare))
        {
            Stream.rwEObject(FStartupSongs, 29);
        }

        Stream.rwEObject(FItemControl, 30);
        Stream.rwData(Bytes.Of(ref FGameStatus, 1), 1, 34);
        Stream.rwData(Bytes.Of(ref FGameResult, 1), 1, 35);
        Stream.rwInteger(ref FInitialRandomSeed, 36);
        Stream.rwData(Bytes.Of(ref FGameType, 1), 1, 37);
        Stream.rwEObject(FUnitControl, 38);
        Stream.rwData(Bytes.Of(ref FPlayMode, 1), 1, 39);
        Stream.rwEObject(FGlobalMagicControl, 40);
        Stream.rwIntegerEx(ref FSphereCount, 5, 41);
        Stream.rwEObject(FTextControl, 42);
        Stream.rwEObject(FCampaignSettings, 43);
        // { Original stores the hero control twice. }
        Stream.rwEObject(FHeroControl, 44);
        Stream.rwData(Bytes.Of(ref FVictoryCondition, 1), 1, 45);
        Stream.rwInteger(ref FVictoryMana, 46);
        Stream.rwInteger(ref FVictoryGold, 47);
        Stream.rwBool(ref FFixedLeaders, 48);
        Stream.rwBool(ref FAlliedVictory, 49);
        Stream.rwEObject(FAIGroupManager, 50);
        Stream.rwInteger(ref FDayLimit, 51);
        Stream.rwInteger(ref FTurnTime, 52);
        Stream.rwBool(ref FUnlimitedMoves, 53);
        Stream.rwEObject(FStructureControl, 54);
        if (Stream.StorageMode().Contains(StorageModeElement.Read) || FConnectionType != PlayerConnectionType.Network)
        {
            Stream.rwInteger(ref FTurnTimeElapsed, 55);
        }

        Stream.rwEObject(FTutorialControl, 56);
        if (Stream.StorageMode().Contains(StorageModeElement.Read) || FConnectionType != PlayerConnectionType.Network)
        {
            Stream.rwInteger(ref FTurnTimeBeep, 58);
        }

        Stream.rwBool(ref FLeadersFixed, 59);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly) && !Stream.StorageMode().Contains(StorageModeElement.Preview) && FConnectionType != PlayerConnectionType.Network)
        {
            Stream.rwEObject(FGameSongs, 60);
        }

        Stream.rwBool(ref FStartingGame, 61);
        Stream.rwBool(ref FShowAllPlayers, 62);
        Stream.rwData(Bytes.Of(ref FVictoryMode, 1), 1, 63);
        RandSeed = FDefendersSeed;
    }

    // AoWHSMap.PAS:2241
    public override void MainReadWrite(EStorageStream Stream)
    {
        string FileName;
        base.MainReadWrite(Stream);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Preview, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read) && FDay == 0)
        {
            if (FGameType == GameType.Campaign)
            {
                FileName = GameTables.AoWEngine.StartupDirectory + "Dict\\" + "CampMaps" + ".mld";
            }
            else
            {
                FileName = GameTables.AoWEngine.StartupDirectory + "Dict\\" + "CustMaps" + ".mld";
            }

            if (FileExists(FileName))
            {
                GameTables.AoWEngine.TranslateEObjectFromFile(this, FileName);
            }
        }
    }

    // AoWHSMap.PAS:1238
    public AoWHSMap(ECustomNode owner) : base(owner)
    {
        FItemControl = new ItemControl(null);
        FTokenManager = new AoWTokenManager(GameTables.AoWEngine);
        FTokenManager.TokenExecuter.OnActivateTC += TokenControlActivatedEvent;
        FTokenManager.TokenExecuter.OnExecuteTokenEvent = OnExecuteTokenEvent;
        FTokenManager.OnMessage = OnTokenManagerMessage;
        FPlayerCommunicator = new PlayerCommunicator(null);
        FPlayerCommunicator.Engine = GameTables.AoWEngine;
        FConnectionType = PlayerConnectionType.Local;
        FAIExecuter = new AIExecuter(null);
        FAIGroupManager = new AIGroupManager(null);
        FAIPlayerActionManager = new AIPlayerActionManager(null);
        FAIPlayerActionManager.RegisterClasses([AIPlayerActionClass.Of<EndTurnAIPA>(), AIPlayerActionClass.Of<GroupControlAIPA>(), AIPlayerActionClass.Of<GroupControlLoopAIPA>(), AIPlayerActionClass.Of<InitializeAIPA>(), AIPlayerActionClass.Of<BudgetManagerAIPA>(), AIPlayerActionClass.Of<AutoProduceAIPA>(), AIPlayerActionClass.Of<DiplomacyAIPA>(), AIPlayerActionClass.Of<ProcessEventlogAIPA>(), AIPlayerActionClass.Of<ResearchSpellAIPA>(), AIPlayerActionClass.Of<CastSpellAIPA>(), AIPlayerActionClass.Of<StructurePhase1AIPA>(), AIPlayerActionClass.Of<StructurePhase2AIPA>()]);
        FTokenManager.TokenExecuter.OnEventsExecuted += FAIExecuter.TokenControlDoneEvent;
        FAIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<HuntAG>()]);
        FPlayMode = PlayMode.None;
        FKeyboard = new AoWHSMapKeyboard(null);
        FTargetSelection = new TargetSelectionControl();
        FEffects = new List<WorldFXHS>();
        FDefendersSeed = 0;
        FHeroCountMax = 5;
        FSelectedArmy = new SelectedArmy();
        ((AbstractAoWHSMapMouse)Mouse).FRectangleSelectEvents += FSelectedArmy.RectangleSelectEvent;
        FMoveControl = new MoveControl(this);
        FGlobalMagicControl = new GlobalMagicControl(null);
        FMapName = "noname";
        FAuthor = "";
        FTurnTimerStart = -1;
        FSphereCount = 5;
        FVictoryGold = 5000;
        FVictoryMana = 5000;
        FDescription = new StringList();
        FObjectives = new StringList();
        FIndependentAIMode = IndependentAIMode.Normal;
        FPlayers = new PlayerList(null);
        FRaces = new RaceList(null);
        FPlayerChangedEventsStore = new();
        FFixedLeaders = true;
        FAlliedVictory = false;
        FCurrentPlayer = 0;
        FUseTurnTimer = true;
        FExecuteEventLogBuffer = new EventLogList(null);
        FDebugSave = false;
        FTutorialControl = new TutorialControl(null);
        FTurnOrderMode = TurnOrderMode.Fixed;
        FTurnOrder = new ByteList();
        FDay = 0;
        FGameStatus = GameStatus.Idle;
        FGameResult = GameResult.None;
        FSessionName = "";
        var detailLevel = GameTables.AoWEngine.FRegistry.GetDetailLevel();
        FShowUnitShadows = Set<DetailLevel>.Of(DetailLevel.Normal, DetailLevel.High).Contains(detailLevel);
        FHeroControl = new HeroControl(null);
        FUnitControl = new UnitControl(null);
        FStructureControl = new StructureControl(null);
        FOnLeaderChangedStore = new();
        FTextControl = new TextControl(null);
        FNewDayEventsStore = new();
        FNewTurnEventsStore = new();
        FEndTurnEventsStore = new();
        FCurrentPlayerChangedEventsStore = new();
        FSeatedPlayerChangedEventsStore = new();
        FArmyChangedEventsStore = new();
        FStructureChangedEventsStore = new();
        FTokenControlActivatedEventsStore = new();
        FUnitChangedEventsStore = new();
        FArmyDestroyedEventsStore = new();
        FStructureDestroyedEventsStore = new();
        FUnitDestroyedEventsStore = new();
        FCombatEventsStore = new();
        FExecuteEventLogEventsStore = new();
        FUnitChangedEvents += FTargetSelection.UnitChanged;
        FArmyDestroyedEvents += FSelectedArmy.OnDestroyArmy;
        FExecuteEventLogCallBacks = new ExecuteEventLogCallBackList();
        FCosmeticSurgeryEventsStore = new();
        FOnActivateAbilityStore = new();
        FStartupSongs = new StartupSongList(null);
        FGameSongs = new Songlist(null);
        FPlayerTerminatedEventsStore = new();
        FGameOverEventsStore = new();
        FGameTerminatedEventsStore = new();
        FDiplomaticRelationsChangedEventsStore = new();
        FUpdateVisibilityEventsStore = new();
        FCombatMapCreatedEventsStore = new();
        FCombatMapDestroyedEventsStore = new();
        FExitEventsStore = new();
        FCampaignSettings = new AoWMapCampaignSettings(null);
        Mouse.OnMove += MouseMoved;
    }

    // AoWHSMap.PAS:2826
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        if (Msg.MsgID == 536870920)
        {
            SynchroniseBegin();
            FGlobalMagicControl.BeginUpdate();
            FPlayers.BeginUpdate();
            BeginUnitUpdate();
            LockUpdateVisibility();
            FItemControl.Activate();
            FStructureControl.Activate();
            if (FDay == 0)
            {
                ResetMapLevels();
            }
        }

        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            ActivateMsg();
            EndUnitUpdate();
            FPlayers.EndUpdate();
            FGlobalMagicControl.EndUpdate();
            UnLockUpdateVisibility();
            SynchroniseEnd();
        }
        else
        {
            if (Msg.MsgID == 537001989)
            {
                FUpdateVisibilityEventsStore.Raise(this);
            }
        }
    }

    // AoWHSMap.PAS:2853
    public override void SendMsg(ref EMsg Msg, EObject Sender)
    {
        base.SendMsg(ref Msg, Sender);
        if (FGlobalMagicControl != null)
        {
            FGlobalMagicControl.MsgProc(ref Msg, Sender);
        }

        FPlayers.MsgProc(ref Msg, Sender);
        FRaces.MsgProc(ref Msg, Sender);
        FItemControl.MsgProc(ref Msg, Sender);
        FHeroControl.MsgProc(ref Msg, Sender);
        FAIGroupManager.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986 && Msg.Payload.Int == Current.FSeatedPlayer)
        {
            if (FSelectedArmy.GetArmyHS() != null)
            {
                FSelectedArmy.Update();
            }
        }
    }

    // AoWHSMap.PAS:1549
    public override HSMapMouse CreateMouse() => new AoWHSMapMouse();

    // AoWHSMap.PAS:2116
    public override void NotifyProgress(in MapProgress Progress)
    {
        string DefaultText;
        string Text = "";
        int Percent = default;
        int Total = default;
        if (OnProgress != null)
        {
            DefaultText = AbilityRules.TranslateRStr(OneMomentPleaseStr);
            switch (Progress.ProgressType)
            {
                case MapProgressType.Loading:
                    Total = Progress.Percent + Progress.Level * 100;
                    Percent = Round((double)((double)Total / 3) / MapContainer.LevelCount);
                    if (GameTables.AoWEngine.FDebug)
                    {
                        Text = AbilityRules.TranslateRStr(LoadingLevelRStr) + IntToStr(Progress.Level);
                    }
                    else
                    {
                        Text = DefaultText;
                    }

                    OnProgress(this, Text, Percent);
                    break;
                case MapProgressType.Initializing:
                    Total = Progress.Percent + Progress.Level * 100;
                    Percent = Round((double)((double)((double)Total / 4.5) / MapContainer.LevelCount) + (double)100 / 3);
                    if (GameTables.AoWEngine.FDebug)
                    {
                        Text = AbilityRules.TranslateRStr(InitializingLevelRStr) + IntToStr(Progress.Level);
                    }
                    else
                    {
                        Text = DefaultText;
                    }

                    OnProgress(this, Text, Percent);
                    break;
                case MapProgressType.UpdatingTerrain:
                    Total = Progress.Percent + Progress.Level * 100;
                    Percent = Round((double)((double)((double)Total / 4.5) / MapContainer.LevelCount) + (double)500 / 9);
                    if (GameTables.AoWEngine.FDebug)
                    {
                        Text = AbilityRules.TranslateRStr(UpdatingTerrainLevelRStr) + IntToStr(Progress.Level);
                    }
                    else
                    {
                        Text = DefaultText;
                    }

                    OnProgress(this, Text, Percent);
                    break;
                case MapProgressType.Activating:
                    Total = Progress.Percent + Progress.Level * 100;
                    Percent = Round((double)((double)((double)Total / 4.5) / MapContainer.LevelCount) + (double)700 / 9);
                    if (GameTables.AoWEngine.FDebug)
                    {
                        Text = AbilityRules.TranslateRStr(ActivatingLevelRStr) + IntToStr(Progress.Level);
                    }
                    else
                    {
                        Text = DefaultText;
                    }

                    OnProgress(this, Text, Percent);
                    break;
                case MapProgressType.Saving:
                    var savingLevel = AbilityRules.TranslateRStr(SavingLevelRStr);
                    OnProgress(this, savingLevel + IntToStr(Progress.Level), Progress.Percent);
                    break;
                case MapProgressType.Destroying:
                    var aoWETranslateRStr = AbilityRules.TranslateRStr(DestroyingLevelRStr);
                    OnProgress(this, aoWETranslateRStr + IntToStr(Progress.Level), Progress.Percent);
                    break;
            }
        }
    }

    // AoWHSMap.PAS:2290
    public override void InitializeNewMap()
    {
        EObject Obj;
        FRaces.Update();
        FPlayers.SetNumberOfPlayers(2);
        FPlayers.ValidateRaces(FRaces);
        for (int L = 0, last = MapContainer.LevelCount - 1; L <= last; L++)
        {
            InitializeMapLevel((sbyte)L);
        }

        for (int I = 0, last = AoWHSSet.Current.FItemLibraries.FSystemLibrary.Count - 1; I <= last; I++)
        {
            Obj = AoWHSSet.Current.FItemLibraries.FSystemLibrary.GetItem(I).Copy(GameTables.AoWEngine);
            FItemControl.RegisterItem((Item)Obj);
            Obj.Release();
        }

        for (int I = 0, last = AoWHSSet.Current.FHeroLibraries.FSystemLibrary.Count - 1; I <= last; I++)
        {
            Obj = AoWHSSet.Current.FHeroLibraries.FSystemLibrary.GetHero(I).Copy(GameTables.AoWEngine);
            FHeroControl.RegisterHero((Hero)Obj);
            Obj.Release();
        }
    }

    // AoWHSMap.PAS:2315
    public override bool AddMapLevel()
    {
        bool Result;
        if (MapContainer.LevelCount < 3 && MapContainer.AddLevel(131091))
        {
            InitializeMapLevel((sbyte)(MapContainer.LevelCount - 1));
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWHSMap.PAS:2654
    public override void Activate()
    {
        SynchroniseBegin();
        if (FDay == 0 || FGameSongs.Songs.Count == 0)
        {
            FGameSongs.Songs.Clear();
            for (int I = 0, last = Current.FStartupSongs.Songs.Count - 1; I <= last; I++)
            {
                var fileNotFound = AbilityRules.TranslateRStr(GameText.FileNotFoundRStr);
                if (CompareText(Current.FStartupSongs.SongNames[I], fileNotFound) != 0)
                {
                    var item = Current.FStartupSongs.Songs[I];
                    FGameSongs.Songs.Add(item);
                }
            }
        }

        FGameStatus = GameStatus.Playing;
        FDebugSave = GameTables.AoWEngine.FRegistry.GetAutoDebugSave();
        FGlobalMagicControl.Update();
        FPlayers.BeginUpdate();
        FPlayers.EndUpdate();
        for (int I = 0, last = FPlayers.Count - 1; I <= last; I++)
        {
            if (FPlayers.GetPlayers(I).FBehavior == Player.Behavior.Random)
            {
                var random = Current.Random(5);
                FPlayers.GetPlayers(I).FBehavior = (Player.Behavior)random;
            }
        }

        if (FPlayerControl == null)
        {
            if (FPlayMode == PlayMode.None)
            {
                FPlayMode = PlayMode.Turn;
            }

            if (FPlayers.FLocalHumanPlayers.Count > 1)
            {
                FPlayMode = PlayMode.Turn;
            }

            SetupPlayerControl(FPlayMode, FConnectionType);
        }

        FTokenManager.Initialize(FPlayerCommunicator);
        base.Activate();
        if (!Status.Contains(HSMapStatusElement.Editting))
        {
            if (FDay == 0)
            {
                StartNewMap();
            }
            else
            {
                StartLoadedMap();
            }

            ShowMessage("");
        }

        if (GameTables.AoWEngine.FRegistry.GetLogTokenEvents())
        {
            StartTokenEventLogging();
        }

        SynchroniseEnd();
    }

    // AoWHSMap.PAS:2326
    public override bool CreateNewMap(NewHSMapSettings Settings) => base.CreateNewMap(Settings);

    // AoWHSMap.PAS:3130
    public override int ChangeTerrain(EObject MO, int X, int Y, int L, int Radius, int TerrainType)
    {
        RandSeed = Current.Random(16777215);
        return base.ChangeTerrain(MO, X, Y, L, Radius, TerrainType);
    }

    // AoWHSMap.PAS:3137
    public override int ChangeTerrainEx(EObject MO, int X, int Y, int L, int Radius, ChangeTerrainProc ChangeTerrainProc, TerrainChangedProc TerrainChangedProc)
    {
        RandSeed = Current.Random(16777215);
        return base.ChangeTerrainEx(MO, X, Y, L, Radius, ChangeTerrainProc, TerrainChangedProc);
    }

    // AoWHSMap.PAS:2983
    public override void DrawDynamicScene(ref SurfaceDesc SD)
    {
        base.DrawDynamicScene(ref SD);
        DrawFog(SD);
        if (FShowGrid)
        {
            DrawGrid(SD);
        }

        FSelectedArmy.ShowMovePaths(SD);
        FTargetSelection.ShowSelection(FViewXhp, FViewYhp, SD);
        if (((AbstractAoWHSMapMouse)Mouse).FSelectionRectangle.Left != -1)
        {
            DrawSelectionRect(FViewXhp, FViewYhp, ((AbstractAoWHSMapMouse)Mouse).FSelectionRectangle, SD);
        }
    }

    // AoWHSMap.PAS:2568
    public override void NewFrame()
    {
        AbstractAoWCombatMap CombatMap;
        base.NewFrame();
        if (Status.Contains(HSMapStatusElement.Activated))
        {
            FPlayerControl.NewFrame();
            FPlayerCommunicator.Process();
            if (FCombatMap == null && FGameStatus != GameStatus.Terminated)
            {
                FAIExecuter.Process();
            }

            FTokenManager.Process();
            UpdateCursor();
            UpdateExecuteEventLogDelay();
            UpdateTurnTimeBeep();
            UpdateEffects();
            CombatMap = FCombatMap;
            if (CombatMap != null)
            {
                CombatMap.NewFrame();
            }
        }
    }

    // AoWHSMap.PAS:1554
    public override void SetSeatedPlayer(sbyte Player)
    {
        FPlayerControl.SetSeatedPlayer(Player);
    }

    // AoWHSMap.PAS:1887
    public virtual void TriggerCombatEvent(Combat.Battle Combat)
    {
        FCombatEventsStore.Raise(Combat);
    }

    // AoWHSMap.PAS:1892
    public virtual void TriggerGameTerminatedEvent()
    {
        FGameStatus = GameStatus.Terminated;
        FGameOverEventsStore.Raise(this);
    }

    // AoWHSMap.PAS:2012
    public virtual void TriggerPlayerTerminatedEvent(int Player)
    {
        FPlayerTerminatedEventsStore.Raise((sbyte)Player);
    }

    // AoWHSMap.PAS:2586
    public virtual void StartNewMap()
    {
        FTurnOrder.Clear();
        if (FTurnOrderMode == TurnOrderMode.Fixed)
        {
            for (int I = 0, last = FPlayers.Count - 1; I <= last; I++)
            {
                FTurnOrder.Add((byte)I);
            }
        }

        if (FTurnOrderMode == TurnOrderMode.Random)
        {
            GenerateNewTurnOrder();
        }

        for (int I = 0, last = FPlayers.Count - 1; I <= last; I++)
        {
            var players = FPlayers.GetPlayers(I);
            FPlayers.GetPlayers(I).SetGems(players.FInitialGold);
            var fPlayersPlayers = FPlayers.GetPlayers(I);
            FPlayers.GetPlayers(I).FMagicControl.SetMana(fPlayersPlayers.FMagicControl.FExternalMana);
            switch (FPlayers.GetPlayers(I).FPlayerType)
            {
                case PlayerType.CPU4:
                    var players2 = FPlayers.GetPlayers(I);
                    FPlayers.GetPlayers(I).SetGems(players2.FGems + 250);
                    var players3 = FPlayers.GetPlayers(I);
                    FPlayers.GetPlayers(I).FMagicControl.SetMana(players3.FMagicControl.FMana + 250);
                    break;
                case PlayerType.CPU5:
                    var players4 = FPlayers.GetPlayers(I);
                    FPlayers.GetPlayers(I).SetGems(players4.FGems + 500);
                    var players5 = FPlayers.GetPlayers(I);
                    FPlayers.GetPlayers(I).FMagicControl.SetMana(players5.FMagicControl.FMana + 500);
                    break;
            }
        }

        FPlayerControl.StartNewMap();
    }

    // AoWHSMap.PAS:2615
    public virtual void StartLoadedMap()
    {
        FPlayerControl.StartLoadedMap();
    }

    // AoWHSMap.PAS:1996
    public virtual void TriggerExecuteEventLog(EventLog Log, bool Immediate, ExecuteDoneEvent Done)
    {
        var notification = new ExecuteEventLogNotification();
        ref var Info = ref notification.Info;
        Info.Log = Log;
        Info.Done = Done;
        Info.Immediate = Immediate;
        if (!Immediate)
        {
            Log.AddRef();
            FExecuteEventLogCallBacks.Add(Info);
            if (FAILockCount == 0)
            {
                ExecuteNextEventLog();
            }
        }
        else
        {
            FExecuteEventLogEventsStore.Raise(notification);
        }
    }

    // AoWHSMap.PAS:1476
    public void SynchroniseBegin()
    {
        FSynchroniseCount += 1;
    }

    // AoWHSMap.PAS:1481
    public void SynchroniseEnd()
    {
        FSynchroniseCount -= 1;
    }

    // AoWHSMap.PAS:1486
    public bool GetSynchronised()
    {
        bool Result;
        if (FSynchroniseCount == 0 && !FTokenManager.TokenExecuter.Executing && !Status.Contains(HSMapStatusElement.Editting))
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // AoWHSMap.PAS:1509
    public void PlayerDisconnected(sbyte Player)
    {
        FPlayerControl.PlayerDisconnected(Player);
    }

    // AoWHSMap.PAS:1514
    public void PlayerConnectionLost()
    {
        FPlayerControl.ConnectionLost();
    }

    // AoWHSMap.PAS:1525
    public IndependentAIMode GetIndependentAIMode()
    {
        IndependentAIMode Result = FIndependentAIMode;
        if (!Current.Status.Contains(HSMapStatusElement.Editting) && Current.FGlobalMagicControl.FActiveEnchantments.Contains(GlobalEnchantmentType.FireMastery))
        {
            if (Result == IndependentAIMode.Off)
            {
                Result = IndependentAIMode.Normal;
            }
            else
            {
                Result = IndependentAIMode.Aggressive;
            }
        }

        return Result;
    }

    // AoWHSMap.PAS:1534
    public bool SaveGame(string FileName)
    {
        bool Result;
        SaveGameTE Event;
        if (Current.FSeatedPlayer != -1)
        {
            Event = new SaveGameTE(null);
            Event.FFileName = ExpandFileName(FileName);
            Current.FTokenManager.GetTokenControl(Current.FSeatedPlayer).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWHSMap.PAS:1559
    public bool TerrainVisible(sbyte X, sbyte Y, sbyte L, int Radius)
    {
        MapLevel MapLevel;
        AoWMapField Field;
        HNWalkData Walk = default;
        // { Original returns true immediately when exploration is enabled. }
        bool Result = Current.FExplorationEnabled;
        MapLevel = MapContainer.GetMapLevel(L);
        HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
        while (Walk.Radius <= Radius && !Result)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
            {
                Field = (AoWMapField)MapLevel.GetField(Walk.X, Walk.Y);
                Result = Field.GetExplored(Current.FSeatedPlayer);
            }

            if (!Result)
            {
                HNWalkData.WalkToNextHN(ref Walk);
            }
        }

        return Result;
    }

    // AoWHSMap.PAS:1580
    public bool Visible(sbyte X, sbyte Y, sbyte L, int Radius)
    {
        MapLevel MapLevel;
        AoWMapField Field;
        HNWalkData Walk = default;
        bool Result = false;
        MapLevel = MapContainer.GetMapLevel(L);
        HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
        while (Walk.Radius <= Radius && !Result)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
            {
                Field = (AoWMapField)MapLevel.GetField(Walk.X, Walk.Y);
                Result = (Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref Field.FFieldFlags).Hi).Lo != 0 || !Current.FFogEnabled) && (Field.GetExplored(Current.FSeatedPlayer) || !Current.FExplorationEnabled);
            }

            if (!Result)
            {
                HNWalkData.WalkToNextHN(ref Walk);
            }
        }

        return Result;
    }

    // AoWHSMap.PAS:1602
    public bool WatchingTerrain(sbyte X, sbyte Y, sbyte L, int Radius)
    {
        MapLevel MapLevel;
        AoWMapField Field;
        HNWalkData Walk = default;
        bool Result = false;
        MapLevel = MapContainer.GetMapLevel(L);
        HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
        while (Walk.Radius <= Radius && !Result)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
            {
                Field = (AoWMapField)MapLevel.GetField(Walk.X, Walk.Y);
                Result = (Field.GetExplored(Current.FSeatedPlayer) || !Current.FExplorationEnabled) && Viewing(Walk.X, Walk.Y, L);
            }

            if (!Result)
            {
                HNWalkData.WalkToNextHN(ref Walk);
            }
        }

        return Result;
    }

    // AoWHSMap.PAS:1624
    public bool Watching(sbyte X, sbyte Y, sbyte L, int Radius)
    {
        MapLevel MapLevel;
        AoWMapField Field;
        HNWalkData Walk = default;
        bool Result = false;
        MapLevel = MapContainer.GetMapLevel(L);
        HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
        while (Walk.Radius <= Radius && !Result)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
            {
                Field = (AoWMapField)MapLevel.GetField(Walk.X, Walk.Y);
                Result = (Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref Field.FFieldFlags).Hi).Lo != 0 || !Current.FFogEnabled) && (Field.GetExplored(Current.FSeatedPlayer) || !Current.FExplorationEnabled) && Viewing(Walk.X, Walk.Y, L);
            }

            if (!Result)
            {
                HNWalkData.WalkToNextHN(ref Walk);
            }
        }

        return Result;
    }

    // AoWHSMap.PAS:1647
    public bool ValidateTerrainObservation(sbyte Player, sbyte X, sbyte Y, sbyte L, int Radius)
    {
        bool Result;
        MapLevel MapLevel;
        AoWMapField Field;
        HNWalkData Walk = default;
        int WX;
        if (FShowIndependentMoves && Player != Current.FSeatedPlayer)
        {
            Result = false;
            MapLevel = MapContainer.GetMapLevel(L);
            HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= Radius && !Result)
            {
                WX = Walk.X;
                if (WX >= 0 && Walk.Y >= 0 && WX < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    Field = (AoWMapField)MapLevel.GetField(WX, Walk.Y);
                    Result = Field.GetExplored(Current.FSeatedPlayer) || !Current.FExplorationEnabled;
                }

                if (!Result)
                {
                    HNWalkData.WalkToNextHN(ref Walk);
                }
            }

            if (Result)
            {
                MakeVisible(Walk.X, Walk.Y, L, 3);
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWHSMap.PAS:1704
    public void UnlockGameOverEventCallBack()
    {
        UnlockGameOverEvent();
    }

    // AoWHSMap.PAS:1709
    public bool SelectNextHero()
    {
        Player Player;
        int I;
        int Left;
        Army Army;
        int Xhp = default;
        int Yhp = default;
        Player = FPlayers.GetPlayers(FSeatedPlayer);
        if (Player.FArmies.Count != 0)
        {
            Left = Player.FArmies.Count;
            I = Player.FHeroJoinDay;
            if (I < 0 || I >= Player.FArmies.Count)
            {
                I = 0;
            }

            while (true)
            {
                I += 1;
                Left -= 1;
                if (I == Player.FArmies.Count)
                {
                    I = 0;
                }

                if (Left == 0 || Player.FArmies.GetArmy(I).Hero(255, Set<HeroType>.Of(HeroType.Hero, HeroType.Leader, HeroType.HeroLeader, HeroType.Merchant)) != null)
                    break;
            }

            if (Player.FArmies.GetArmy(I).Hero(255, Set<HeroType>.Of(HeroType.Hero, HeroType.Leader, HeroType.HeroLeader, HeroType.Merchant)) != null)
            {
                Army = FPlayers.GetPlayers(FSeatedPlayer).FArmies.GetArmy(I);
                CenterViewTo(Army.FX, Army.FY, Army.FL);
                Selection.UnselectLocation();
                HexGeometry.HXtoHP(Army.FX, Army.FY, ref Xhp, ref Yhp);
                Selection.SelectLocation(Xhp, Yhp, Army.FL);
                Player.FHeroJoinDay = I;
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
        Player Player;
        int I;
        int Left;
        Army Army;
        int Xhp = default;
        int Yhp = default;
        Player = FPlayers.GetPlayers(FSeatedPlayer);
        if (Player.FArmies.Count != 0)
        {
            Left = Player.FArmies.Count;
            I = Player.FBaseHeroJoinDay;
            if (I < 0 || I >= Player.FArmies.Count)
            {
                I = 0;
            }

            while (true)
            {
                I += 1;
                Left -= 1;
                if (I == Player.FArmies.Count)
                {
                    I = 0;
                }

                if (Left == 0 || Player.FArmies.GetArmy(I).MovePoints(255) > 1 && (Player.FArmies.GetArmy(I).FUserMode & Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard, ArmyUserModeElement.TurnDone)) == Set<ArmyUserModeElement>.Of())
                    break;
            }

            if (Player.FArmies.GetArmy(I).MovePoints(255) > 1 && (Player.FArmies.GetArmy(I).FUserMode & Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard, ArmyUserModeElement.TurnDone)) == Set<ArmyUserModeElement>.Of())
            {
                Army = FPlayers.GetPlayers(FSeatedPlayer).FArmies.GetArmy(I);
                CenterViewTo(Army.FX, Army.FY, Army.FL);
                Selection.UnselectLocation();
                HexGeometry.HXtoHP(Army.FX, Army.FY, ref Xhp, ref Yhp);
                Selection.SelectLocation(Xhp, Yhp, Army.FL);
                Player.FBaseHeroJoinDay = I;
            }
            else
            {
                Selection.UnselectLocation();
            }
        }

        return true;
    }

    // AoWHSMap.PAS:1775
    public bool SelectPrevArmy()
    {
        Player Player;
        int I;
        int Left;
        Army Army;
        int Xhp = default;
        int Yhp = default;
        Player = FPlayers.GetPlayers(FSeatedPlayer);
        if (Player.FArmies.Count != 0)
        {
            I = Player.FBaseHeroJoinDay;
            if (I < 0 || I >= Player.FArmies.Count)
            {
                I = 0;
            }

            Left = Player.FArmies.Count;
            while (true)
            {
                I -= 1;
                Left -= 1;
                if (I < 0)
                {
                    I = Player.FArmies.Count - 1;
                }

                if (Left == 0 || Player.FArmies.GetArmy(I).MovePoints(255) > 1 && (Player.FArmies.GetArmy(I).FUserMode & Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard, ArmyUserModeElement.TurnDone)) == Set<ArmyUserModeElement>.Of())
                    break;
            }

            if (Player.FArmies.GetArmy(I).MovePoints(255) > 1 && (Player.FArmies.GetArmy(I).FUserMode & Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard, ArmyUserModeElement.TurnDone)) == Set<ArmyUserModeElement>.Of())
            {
                Army = FPlayers.GetPlayers(FSeatedPlayer).FArmies.GetArmy(I);
                CenterViewTo(Army.FX, Army.FY, Army.FL);
                Selection.UnselectLocation();
                HexGeometry.HXtoHP(Army.FX, Army.FY, ref Xhp, ref Yhp);
                Selection.SelectLocation(Xhp, Yhp, Army.FL);
                Player.FBaseHeroJoinDay = I;
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
        EMsg Msg = default;
        Selection.UnselectAll();
        EMsg.InitMsg(ref Msg, 537001990, this);
        MapContainer.SendMsg(ref Msg, this);
        EMsg.InitMsg(ref Msg, 537001993, this);
        MapContainer.SendMsg(ref Msg, this);
        FSeatedPlayerChangedEventsStore.Raise(Current);
        ShowMessage("");
    }

    // AoWHSMap.PAS:1822
    public void BeginUnitUpdate()
    {
        FUnitUpdateCount += 1;
    }

    // AoWHSMap.PAS:1827
    public void EndUnitUpdate()
    {
        FUnitUpdateCount -= 1;
        if (FUnitUpdateCount == 0)
        {
            UnitChanged(null);
        }
    }

    // AoWHSMap.PAS:1833
    public void UnitChanged(AbstractUnit AUnit)
    {
        if (FUnitUpdateCount == 0)
        {
            FUnitChangedEventsStore.Raise(AUnit);
        }
    }

    // AoWHSMap.PAS:1838
    public void UnitDestroyed(AbstractUnit AUnit)
    {
        if (!Current.Status.Contains(HSMapStatusElement.Destroying))
        {
            FUnitDestroyedEventsStore.Raise(AUnit);
        }
    }

    // AoWHSMap.PAS:1844
    public void BeginArmyUpdate()
    {
        FArmyUpdateCount += 1;
    }

    // AoWHSMap.PAS:1849
    public void EndArmyUpdate()
    {
        FArmyUpdateCount -= 1;
        if (FArmyUpdateCount == 0)
        {
            ArmyChanged(null);
        }
    }

    // AoWHSMap.PAS:1855
    public void ArmyChanged(Army Army)
    {
        if (FArmyUpdateCount == 0)
        {
            FArmyChangedEventsStore.Raise(Army);
        }
    }

    // AoWHSMap.PAS:1860
    public void ArmyDestroyed(Army Army)
    {
        // TArmy.Destroy passes its Self pointer
        FArmyDestroyedEventsStore.Raise(Army);
    }

    // AoWHSMap.PAS:1866
    // TArmy.Destroy passes TArmy
    public void BeginStructureUpdate()
    {
        FStructureUpdateCount += 1;
    }

    // AoWHSMap.PAS:1871
    public void EndStructureUpdate()
    {
        FStructureUpdateCount -= 1;
        if (FStructureUpdateCount == 0)
        {
            StructureChanged(null);
        }
    }

    // AoWHSMap.PAS:1877
    public void StructureChanged(Structure Structure)
    {
        if (FStructureUpdateCount == 0)
        {
            FStructureChangedEventsStore.Raise(Structure);
        }
    }

    // AoWHSMap.PAS:1882
    public void StructureDestroyed(Structure Structure)
    {
        FStructureDestroyedEventsStore.Raise(Structure);
    }

    // AoWHSMap.PAS:1898
    public void ExecuteExitGame()
    {
        FExitPending = true;
        FExitEventsStore.Raise(this);
    }

    // AoWHSMap.PAS:1904
    public void ExitGame(ExitGameMode Mode, ExitGameCallBack Done)
    {
        PlayerControl PlayerControl;
        if (!FExitPending)
        {
            PlayerControl = FPlayerControl;
            if (PlayerControl != null)
            {
                PlayerControl.ExitGame(Mode, Done);
            }
            else
            {
                ExecuteExitGame();
                if (Done != null)
                {
                    Done(Mode, true);
                }
            }
        }
        else
        {
            if (Done != null)
            {
                Done(Mode, true);
            }
        }
    }

    // AoWHSMap.PAS:1921
    public void TriggerEndTurnEvent(sbyte Player)
    {
        FEndTurnEventsStore.Raise(Player);
    }

    // AoWHSMap.PAS:1926
    public void ClearExecuteEventLogBuffer()
    {
        while (FExecuteEventLogCallBacks.GetCount() > 0)
        {
            if (FExecuteEventLogCallBacks.GetItem(0).Done != null)
            {
                var item = FExecuteEventLogCallBacks.GetItem(1);
                FExecuteEventLogCallBacks.GetItem(0).Done(item.Log);
            }

            // { Original passes the second record's log to the first callback, even for a one-item list. }
            FExecuteEventLogCallBacks.Delete(0);
        }
    }

    // AoWHSMap.PAS:1937
    public void ExecuteNextEventLog()
    {
        var notification = new ExecuteEventLogNotification();
        ref var Info = ref notification.Info;
        if (FExecuteEventLogCallBacks.GetCount() != 0)
        {
            FAILockCount += 1;
            Info = FExecuteEventLogCallBacks.GetItem(0);
            Info.Done = ExecuteEventLogDone;
            FExecuteEventLogEventsStore.Raise(notification);
        }
    }

    // AoWHSMap.PAS:1950
    public void SetExecuteEventLogDelay(int Value)
    {
        if (FExecuteEventLogDelay == 0)
        {
            FExecuteEventLogDelay = Value;
            LockExecuteEventLog();
        }
        else
        {
            if (Value > FExecuteEventLogDelay)
            {
                FExecuteEventLogDelay = Value;
            }
        }
    }

    // AoWHSMap.PAS:1960
    public void UpdateExecuteEventLogDelay()
    {
        if (FExecuteEventLogDelay != 0)
        {
            FExecuteEventLogDelay -= 1;
            if (FExecuteEventLogDelay == 0)
            {
                UnlockExecuteEventLog();
            }
        }
    }

    // AoWHSMap.PAS:1969
    public void ExecuteEventLogDone(EventLog Log)
    {
        ExecuteEventLogInfo Info;
        if (FExecuteEventLogCallBacks.GetCount() > 0 && Log == FExecuteEventLogCallBacks.GetItem(0).Log)
        {
            Info = FExecuteEventLogCallBacks.GetItem(0);
            FExecuteEventLogCallBacks.Delete(0);
            if (Info.Done != null)
            {
                Info.Done(Info.Log);
            }

            Info.Log.Release();
        }

        SetExecuteEventLogDelay(2);
        UnlockExecuteEventLog();
    }

    // AoWHSMap.PAS:1985
    public void LockExecuteEventLog()
    {
        FAILockCount += 1;
    }

    // AoWHSMap.PAS:1990
    public void UnlockExecuteEventLog()
    {
        FAILockCount -= 1;
        if (FAILockCount == 0)
        {
            ExecuteNextEventLog();
        }
    }

    // AoWHSMap.PAS:2018
    public void GameOverEventHandled(sbyte Player)
    {
        FPlayerControl.GameOverEventHandled(Player);
    }

    // AoWHSMap.PAS:2024
    public void ExecuteGameOverEvent(GameOverEventLog Event)
    {
        Player Player;
        GameOverEventLog Log;
        for (int I = 1, last = Current.FPlayers.Count - 1; I <= last; I++)
        {
            Player = Current.FPlayers.GetPlayers(I);
            if (I == Event.FPlayer || Player.FGameOverStatus == GameOverStatus.None)
            {
                Log = (GameOverEventLog)Event.Copy(GameTables.AoWEngine);
                if (FGameOverLockCount == 0)
                {
                    Player.FEventLogbook.AddEvent(Log);
                    if (I == Current.FSeatedPlayer)
                    {
                        Log.Execute(false, ExecuteTrigger.Event, null);
                    }
                }
                else
                {
                    Log.FTargetPlayer = (sbyte)I;
                    FExecuteEventLogBuffer.AddEvent(Log);
                }

                Log.Release();
            }
        }
    }

    // AoWHSMap.PAS:2052
    public void UnlockGameOverEvent()
    {
        sbyte Player;
        EventLog Log;
        FGameOverLockCount -= 1;
        if (FGameOverLockCount == 0)
        {
            while (FExecuteEventLogBuffer.Count != 0)
            {
                Player = ((GameOverEventLog)FExecuteEventLogBuffer.GetEvents(0)).FTargetPlayer;
                Log = (EventLog)FExecuteEventLogBuffer.GetEvents(0).Copy(GameTables.AoWEngine);
                Current.FPlayers.GetPlayers(Player).FEventLogbook.AddEvent(Log);
                if (Player == Current.FSeatedPlayer)
                {
                    Log.Execute(false, ExecuteTrigger.Event, null);
                }

                Log.Release();
                var events = FExecuteEventLogBuffer.GetEvents(0);
                FExecuteEventLogBuffer.RemoveEvent(events);
            }
        }
    }

    // AoWHSMap.PAS:2071
    public void LockGameOverEvent()
    {
        FGameOverLockCount += 1;
    }

    // AoWHSMap.PAS:2077
    public void GameOver(sbyte Player, GameOverStatus Status)
    {
        if (Player > 0 && this.Status.Contains(HSMapStatusElement.Activated) && !this.Status.Contains(HSMapStatusElement.Editting) && !this.Status.Contains(HSMapStatusElement.Destroying))
        {
            FPlayerControl.GameOver(Player, Status);
        }
    }

    // AoWHSMap.PAS:2085
    public void VerifyAddOnPacks()
    {
    }

    // AoWHSMap.PAS:2088
    public bool ValidateMap(StringCollection Messages)
    {
        EMsg Msg = default;
        EMsg.InitMsg(ref Msg, 537002243, this);
        Msg.Payload = Messages;
        Msg.Data[0] = 0;
        Current.SendMsg(ref Msg, this);
        bool Result = Msg.Data[0] == 0;
        if (Result)
        {
            Messages.Add(AbilityRules.TranslateRStr(MapSuccessfullyValidatedRStr));
        }
        else
        {
            Messages.Add(AbilityRules.TranslateRStr(MapValidationFailedRStr));
        }

        return Result;
    }

    // AoWHSMap.PAS:2261
    public void InitializeMapLevel(sbyte Level)
    {
        HexagonControl Control;
        Control = Engine.HSSet.HexagonControl;
        if (Level == 0)
        {
            for (int X = 6, last = MapContainer.Width - 7; X <= last; X++)
            {
                for (int Y = 6, lastY = MapContainer.Height - 7; Y <= lastY; Y++)
                {
                    Control.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 0);
                }
            }
        }
        else
        {
            for (int X = 1, last = MapContainer.Width - 7; X <= last; X++)
            {
                for (int Y = 1, lastY = MapContainer.Height - 7; Y <= lastY; Y++)
                {
                    Control.Place(MapContainer, (sbyte)X, (sbyte)Y, Level, 7);
                }
            }
        }

        for (int X = 0, last = MapContainer.Width - 1; X <= last; X++)
        {
            for (int Y = 0; Y <= 5; Y++)
            {
                Control.Place(MapContainer, (sbyte)X, (sbyte)Y, Level, 15);
            }

            for (int Y = MapContainer.Height - 6, lastY = MapContainer.Height - 1; Y <= lastY; Y++)
            {
                Control.Place(MapContainer, (sbyte)X, (sbyte)Y, Level, 15);
            }
        }

        for (int Y = 6, lastY = MapContainer.Height - 7; Y <= lastY; Y++)
        {
            for (int X = 0; X <= 5; X++)
            {
                Control.Place(MapContainer, (sbyte)X, (sbyte)Y, Level, 15);
            }

            for (int X = MapContainer.Width - 6, last = MapContainer.Width - 1; X <= last; X++)
            {
                Control.Place(MapContainer, (sbyte)X, (sbyte)Y, Level, 15);
            }
        }
    }

    // AoWHSMap.PAS:2332
    public void NotifyPlayerChangedEx(int X, int Y, int L, int Radius)
    {
        MapLevel MapLevel;
        MapFieldMsg Msg = default;
        HNWalkData Walk = default;
        AoWMapField Field;
        MapLevel = MapContainer.GetMapLevel(L);
        Msg.MsgID = 4365;
        Msg.Result = 1;
        HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
        while (Walk.Radius <= Radius)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
            {
                Field = (AoWMapField)MapLevel.GetField(Walk.X, Walk.Y);
                if ((Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref Field.FFieldFlags).Hi).Hi & 1) != 0)
                {
                    Msg.Param[0] = Walk.Radius;
                    Field.SendMapFieldMsg(ref Msg);
                }
            }

            HNWalkData.WalkToNextHN(ref Walk);
        }
    }

    // AoWHSMap.PAS:2359
    public void UpdatePlayerSeen(int Player, int X, int Y, int L, int SightRadius, int Radius)
    {
        MapLevel MapLevel;
        MapFieldMsg Msg = default;
        HNWalkData Walk = default;
        AoWMapField Field;
        MapLevel = MapContainer.GetMapLevel(L);
        Msg.MsgID = 4400;
        Msg.Param[0] = Player;
        HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
        while (Walk.Radius <= SightRadius || Walk.Radius <= Radius)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
            {
                Field = (AoWMapField)MapLevel.GetField(Walk.X, Walk.Y);
                if ((Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref Field.FFieldFlags).Hi).Hi & 2) != 0)
                {
                    Msg.Result = -1;
                    if (Walk.Radius <= Radius)
                    {
                        // { Original leaves the flag uninitialized outside Radius. }
                        Msg.Param[1] = 1;
                    }

                    Field.SendMapFieldMsg(ref Msg);
                    if (Msg.Result.Int != -1 && Msg.Result.Int != Player)
                    {
                        Current.FPlayers.GetPlayers(Player).PlayerSeen((sbyte)Msg.Result.Int);
                        for (int I = 0, last = FPlayers.Count - 1; I <= last; I++)
                        {
                            if (I != Player && FPlayers.GetDiplomaticRelation((sbyte)Player, (sbyte)I) == DiplomaticRelation.Alliance)
                            {
                                Current.FPlayers.GetPlayers(I).PlayerSeen((sbyte)Msg.Result.Int);
                            }
                        }
                    }
                }
            }

            HNWalkData.WalkToNextHN(ref Walk);
        }
    }

    // AoWHSMap.PAS:2395
    public void NotifyPlayerChanged(int X, int Y, int L)
    {
        NotifyPlayerChangedEx(X, Y, L, 12);
    }

    // AoWHSMap.PAS:2401
    public bool VisibleForPlayer(int X, int Y, int Level, sbyte Player)
    {
        bool Result;
        MapLevel MapLevel;
        MapFieldMsg Msg = default;
        HNWalkData Walk = default;
        AoWMapField Field;
        MapLevel = MapContainer.GetMapLevel(Level);
        if (Player >= 0)
        {
            Msg.MsgID = 4362;
            Msg.Result = 0;
            Msg.Param[1] = Player;
            HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
            while (Msg.Result.Int == 0 && Walk.Radius <= 12)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    Field = (AoWMapField)MapLevel.GetField(Walk.X, Walk.Y);
                    if ((Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref Field.FFieldFlags).Lo).Hi & 16) != 0)
                    {
                        Msg.Param[0] = Walk.Radius;
                        Field.SendMapFieldMsg(ref Msg);
                    }
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }

            Result = Msg.Result.Int != 0;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWHSMap.PAS:2434
    public bool FoggedForPlayer(int X, int Y, int L, sbyte Player)
    {
        bool Result;
        MapLevel MapLevel;
        MapFieldMsg Msg = default;
        HNWalkData Walk = default;
        AoWMapField Field;
        MapLevel = MapContainer.GetMapLevel(L);
        if (!FExplorationEnabled || ((AoWMapField)MapLevel.GetField(X, Y)).GetExplored(Player))
        {
            if (FFogEnabled)
            {
                if (Player != Current.FSeatedPlayer)
                {
                    Msg.MsgID = 4362;
                    Msg.Result = 0;
                    Msg.Param[1] = Player;
                    HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
                    while (Msg.Result.Int == 0 && Walk.Radius <= 12)
                    {
                        if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                        {
                            Field = (AoWMapField)MapLevel.GetField(Walk.X, Walk.Y);
                            if ((Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref Field.FFieldFlags).Lo).Hi & 16) != 0)
                            {
                                Msg.Param[0] = Walk.Radius;
                                Field.SendMapFieldMsg(ref Msg);
                            }
                        }

                        HNWalkData.WalkToNextHN(ref Walk);
                    }

                    Result = Msg.Result.Int == 0;
                }
                else
                {
                    var fieldValue = Current.MapContainer.GetField(X, Y, L);
                    Result = Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((AoWMapField)fieldValue).FFieldFlags).Hi).Lo == 0;
                }
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // AoWHSMap.PAS:2474
    public void MouseMoved(object Sender)
    {
        AoWMapField Field;
        // The original tests Xhx only and with Yhx -1 read memory before the field block. The
        // port treats that point as off the map.
        if (Mouse.Capture == null && Mouse.Xhx != -1 && Mouse.Yhx != -1)
        {
            var yhx = Mouse.Yhx;
            var xhx = Mouse.Xhx;
            Field = (AoWMapField)MapContainer.GetField(xhx, yhx, FViewL);
            if (Current.FExplorationEnabled && !Field.GetExplored(Current.FSeatedPlayer))
            {
                return;
            }

            if (!Current.FFogEnabled || Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref Field.FFieldFlags).Hi).Lo != 0)
            {
                var yhp = Mouse.Yhp;
                var xhp = Mouse.Xhp;
                if (Selection.CanSelectLocation(xhp, yhp, FViewL) && !((AbstractAoWHSMapMouse)Mouse).GetSelectingRectangle())
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
        Player Player;
        Army Army;
        int XYL = default;
        if (FSeatedPlayer != -1)
        {
            Player = FPlayers.GetPlayers(FSeatedPlayer);
            if ((sbyte)Player.FViewX > 0)
            {
                Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Lo).Lo = Player.FViewX;
                Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Lo).Hi = Player.FViewY;
                Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Hi).Lo = Player.FViewL;
            }
            else
            {
                Player.GetLeaderLocation(ref Unsafe.As<byte, sbyte>(ref Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Lo).Lo), ref Unsafe.As<byte, sbyte>(ref Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Lo).Hi), ref Unsafe.As<byte, sbyte>(ref Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Hi).Lo));
                if ((sbyte)Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Lo).Lo == -1)
                {
                    if (Player.FArmies.Count == 0)
                    {
                        Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Lo).Lo = 0;
                        Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Lo).Hi = 0;
                        Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Hi).Lo = 0;
                    }
                    else
                    {
                        Army = Player.FArmies.GetArmy(0);
                        Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Lo).Lo = (byte)Army.FX;
                        Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Lo).Hi = (byte)Army.FY;
                        Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Hi).Lo = (byte)Army.FL;
                    }
                }
            }

            CenterViewTo((sbyte)Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Lo).Lo, (sbyte)Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Lo).Hi, (sbyte)Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref XYL).Hi).Lo);
        }
    }

    // AoWHSMap.PAS:2538
    public void Select(HexagonSprite Sprite, bool Center)
    {
        // { This eliminated copy preserves the original saved registers. }
        Selection.UnselectLocation();
        Selection.SelectLocationHS(Sprite);
        if (Center)
        {
            var lhx = Sprite.GetLhx();
            var yhx = Sprite.GetYhx();
            CenterViewTo(Sprite.GetXhx(), yhx, lhx);
        }
    }

    // AoWHSMap.PAS:2546
    public void PlayerChanged()
    {
        FCurrentPlayerChangedEventsStore.Raise(this);
    }

    // AoWHSMap.PAS:2552
    public void TokenControlActivatedEvent(object Sender)
    {
        int Player;
        if (FCombatMap == null)
        {
            Player = FTokenManager.TokenExecuter.ActiveTCIndex;
            if (Player != FCurrentPlayer)
            {
                FCurrentPlayer = (sbyte)Player;
                PlayerChanged();
            }
        }
    }

    // AoWHSMap.PAS:2621
    public void SetupPlayerControl(PlayMode Mode, PlayerConnectionType Connection)
    {
        switch (Mode)
        {
            case PlayMode.Turn:
                if (FConnectionType == PlayerConnectionType.Email)
                {
                    if (!(FPlayerControl is PBEMPlayerControl))
                    {
                        FPlayerControl?.Dispose();
                        FPlayerControl = new PBEMPlayerControl(null);
                    }
                }
                else
                {
                    // { Original tests TPBEMPlayerControl even in the non-email branch. }
                    if (!(FPlayerControl is PBEMPlayerControl))
                    {
                        FPlayerControl?.Dispose();
                        FPlayerControl = new TurnPlayerControl(null);
                    }
                }

                break;
            case PlayMode.Simultaneous:
                if (!(FPlayerControl is SimultaniousPlayerControl))
                {
                    FPlayerControl?.Dispose();
                }

                // { Original replaces an existing control without freeing it. }
                FPlayerControl = new SimultaniousPlayerControl(null);
                break;
        }

        FConnectionType = Connection;
        FPlayMode = Mode;
        if (FPlayerControl != null)
        {
            FPlayerControl.Setup();
        }
    }

    // AoWHSMap.PAS:2692
    public void ActivateMsg()
    {
        if (FDay == 0)
        {
            FRaces.ResetDefaultRelations();
        }

        if (!Status.Contains(HSMapStatusElement.Editting) && FDay == 0)
        {
            FExplorationEnabled = true;
            FFogEnabled = true;
        }
    }

    // AoWHSMap.PAS:2703
    public int Random(int Range)
    {
        int Result;
        if (!Current.Status.Contains(HSMapStatusElement.Destroying))
        {
            if (GetSynchronised())
            {
                RandSeed = FDefendersSeed;
                Result = RtlUnit.Random(Range);
                FDefendersSeed = RandSeed;
            }
            else
            {
                RtlUnit.ShowMessage("Invalid AoWHSMap.Random use");
                Result = 0;
            }
        }
        else
        {
            Result = RtlUnit.Random(Range);
        }

        return Result;
    }

    // AoWHSMap.PAS:2723
    public void GenerateNewTurnOrder()
    {
        int Index;
        ByteList List;
        List = new ByteList();
        for (int I = 1, last = FPlayers.Count - 1; I <= last; I++)
        {
            List.Add((byte)I);
        }

        FTurnOrder.Clear();
        FTurnOrder.Add(0);
        while (List.Count != 0)
        {
            Index = Random(List.Count);
            var item = List[Index];
            FTurnOrder.Add(item);
            List.Delete(Index);
        }

        List?.Dispose();
    }

    // AoWHSMap.PAS:2749
    public bool EndTurn()
    {
        bool Result;
        if (FSeatedPlayer != -1)
        {
            if (!FRequestCombatType || FExecuteEventLogCallBacks.GetCount() == 0)
            {
                Result = TryEndPlayerTurn(FSeatedPlayer);
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWHSMap.PAS:2777
    public bool CancelEndTurn()
    {
        bool Result;
        if (FSeatedPlayer != -1)
        {
            Result = CancelPlayerTurn(FSeatedPlayer);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWHSMap.PAS:2784
    public void LockUpdateVisibility()
    {
        FUpdateVisibilityLockCount += 1;
    }

    // AoWHSMap.PAS:2790
    public void UnLockUpdateVisibility()
    {
        FUpdateVisibilityLockCount -= 1;
        if (FUpdateVisibilityLockCount == 0 && FVisibilityChanged)
        {
            UpdateVisibility();
        }
    }

    // AoWHSMap.PAS:2797
    public void UpdateVisibility()
    {
        EMsg Msg = default;
        if (FUpdateVisibilityLockCount == 0)
        {
            if (!Status.Contains(HSMapStatusElement.Destroying))
            {
                LockMapChangedEvent();
                EMsg.InitMsg(ref Msg, 537001992, this);
                Current.SendMsg(ref Msg, this);
                UnlockMapChangedEvent();
                Current.InvalidateMap();
            }

            FVisibilityChanged = false;
        }
        else
        {
            FVisibilityChanged = true;
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
    public void ExecuteNegotiateMove(int ArmyID, int X, int Y, int L, int MovePoints)
    {
    }

    // AoWHSMap.PAS:2870
    public void InitializeCombatMap(Combat.Battle Combat, string FileName)
    {
        Player Player = default;
        int X = default;
        int Y = default;
        int L = default;
        if (FCombatMap == null)
        {
            if (FSeatedPlayer != -1)
            {
                Player = FPlayers.GetPlayers(FSeatedPlayer);
                GetViewCenter(ref X, ref Y, ref L);
                Player.FViewX = (byte)X;
                Player.FViewY = (byte)Y;
                Player.FViewL = (byte)L;
            }

            FCombatMap = (AbstractAoWCombatMap)GameTables.AoWEngine.LoadHSMEx(FileBoundary.ReadName(GameTables.AoWEngine.StartupDirectory + FileName), false, null, GameProfile.TacticalMap);
            FCombatMapCreatedEventsStore.Raise(this);
        }
    }

    // AoWHSMap.PAS:2891
    public void ReleaseCombatMap()
    {
        if (FCombatMap != null)
        {
            FCombatMapDestroyedEventsStore.Raise(this);
            FCombatMap?.Dispose();
            FCombatMap = null;
            ResetSeatedPlayerView();
        }
    }

    // AoWHSMap.PAS:2903
    public Combat.Battle CreateCombat(int AClassID)
    {
        Combat.Battle Result;
        if (FCombat == null)
        {
            Result = (Combat.Battle)GameTables.AoWEngine.CreateEObject(AClassID, null);
            FCombat = Result;
            if (FGameType == GameType.Tutorial && AClassID == 2228496)
            {
                FTutorialControl.ShowMessage(53, false, true);
            }

            if (AClassID == 2228496)
            {
                ((AbstractAoWHSMapMouse)Current.Mouse).ResetRectangleSelection();
                Current.Selection.UnselectLocation();
                FTargetSelection.Deactivate();
                DisableTurnTimer();
            }
        }
        else
        {
            throw new InvalidOperationException("Combat already created");
        }

        return Result;
    }

    // AoWHSMap.PAS:2922
    public void ShowSeatedPlayerCallBack(EventLog Log)
    {
        SetSeatedPlayer(Current.FCurrentPlayer);
        EnableTurnTimer();
    }

    // AoWHSMap.PAS:2929
    public void DestroyCombat()
    {
        sbyte Player = default;
        int Count = default;
        NewTurnEventLog Log = default;
        if (FCombat != null)
        {
            Current.LockGameOverEvent();
            if (FCombat.FStatus.Contains(CombatStatusElement.Activated))
            {
                FCombat.Deactivate();
            }

            if (FCombat.FStatus.Contains(CombatStatusElement.Executing))
            {
                FCombat.Finalize();
                Player = FCombat.FCombatData.FPlayers.GetPlayers(FCombat.FCombatData.FAttackerPlayer).FPlayer;
                if (Current.FConnectionType == PlayerConnectionType.Network && FPlayMode == PlayMode.Simultaneous)
                {
                    Count = Current.FPlayers.Count;
                    Player = FCombat.FCombatData.FPlayers.GetPlayers(FCombat.FCombatData.FAttackerPlayer).FPlayer;
                    while (Count != 0 && Current.FPlayers.GetPlayers(Player).FStatus != Session.Player.Status.Playing)
                    {
                        Player = (sbyte)(Player + 1);
                        if (Player == Current.FPlayers.Count)
                        {
                            Player = 0;
                        }

                        Count -= 1;
                    }
                }

                Current.FTokenManager.TokenExecuter.ActivateTC(Player);
                if (FPlayMode == PlayMode.Simultaneous)
                {
                    Current.FTokenManager.TokenExecuter.HoldTC = false;
                }
            }

            if (FRequestCombatType && FCombat.EClass.ClassID != 131760 && FCombat.FCombatData.FRound != 0 && FCombat.FExplorationSite != 1 && FPlayers.GetPlayers(FCurrentPlayer).FPlayerType == PlayerType.Human)
            {
                LockExecuteEventLog();
                Log = new NewTurnEventLog(null);
                Log.FTurnPlayer = FCurrentPlayer;
                Log.FMode = NewTurnEventLog.Mode.Dialog;
                Log.FDialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                Log.FText = AbilityRules.TranslateRStr(GameText.YourTurnContinuesRStr);
                TriggerExecuteEventLog(Log, false, ShowSeatedPlayerCallBack);
                Log.Release();
                UnlockGameOverEvent();
                SetSeatedPlayer(-1);
                UnlockExecuteEventLog();
            }
            else
            {
                if (FCombat.EClass.ClassID == 2228496)
                {
                    EnableTurnTimer();
                }

                UnlockGameOverEvent();
            }

            FCombat?.Dispose();
            FCombat = null;
        }
    }

    // AoWHSMap.PAS:3026
    public void TriggerRelationsChanged()
    {
        if (Status.Contains(HSMapStatusElement.Activated) && FRelationsChangedLockCount == 0)
        {
            BeginUnitUpdate();
            FUnitControl.TriggerUnitsChanged();
            FPlayerChangedEventsStore.Raise(this);
            EndUnitUpdate();
        }
    }

    // AoWHSMap.PAS:3038
    public void LockRelationsChanged()
    {
        FRelationsChangedLockCount += 1;
    }

    // AoWHSMap.PAS:3044
    public void UnlockRelationsChanged()
    {
        Assert(FRelationsChangedLockCount != 0, "Invalid MapChanged Lock usage", "AoWHSMap.PAS", 3056);
        FRelationsChangedLockCount -= 1;
    }

    // AoWHSMap.PAS:3061
    public void UpdateTurnTimeBeep()
    {
        int Seconds;
        if (GetTurnTimerEnabled())
        {
            Seconds = GetTurnTimeLeft() / 1000;
            if (FTurnTimeBeep != 0 && Seconds < FTurnTimeBeep && FCombatMap == null)
            {
                FTurnTimeBeep = Seconds;
                if (FTurnTimeBeep == 0)
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
        FTurnTimeElapsed = FTurnTime * 1000 * 60;
        FTurnTimeBeep = 11;
        FTurnTimerStart = -1;
    }

    // AoWHSMap.PAS:3086
    public int GetTurnTimeLeft()
    {
        int Time;
        int Left;
        Left = FTurnTimeElapsed;
        if (FTurnTimerStart != -1)
        {
            Time = PixelFormats.GetElapsedMilliSeconds();
            if (Time >= FTurnTimerStart)
            {
                Left = Left - (Time - FTurnTimerStart);
            }
            else
            {
                Left = 0;
            }
        }

        if (Left < 0)
        {
            Left = 0;
        }

        return Left;
    }

    // AoWHSMap.PAS:3102
    public bool GetTurnTimerEnabled() => FTurnTime > 0 && FTurnTimerStart != -1;

    // AoWHSMap.PAS:3108
    public void EnableTurnTimer()
    {
        if (FTurnTimerStart == -1 && FTurnTime > 0)
        {
            FTurnTimerStart = PixelFormats.GetElapsedMilliSeconds();
        }
    }

    // AoWHSMap.PAS:3114
    public void DisableTurnTimer()
    {
        int Time;
        Time = PixelFormats.GetElapsedMilliSeconds() - FTurnTimerStart;
        if (Time > 0)
        {
            FTurnTimeElapsed -= Time;
        }

        FTurnTimerStart = -1;
    }

    // AoWHSMap.PAS:3124
    public void ZeroTurnTimer()
    {
        FTurnTimeElapsed = 0;
    }

    // AoWHSMap.PAS:3144
    public void CenterOnLeader(sbyte Player, bool Select, bool Animate)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        ArmyHS Army;
        if (Current.FPlayers.GetPlayers(Player).FLeader != null)
        {
            Current.FPlayers.GetPlayers(Player).FLeader.GetLocation(ref X, ref Y, ref L);
            // { Original excludes X = 0. }
            if (X > 0)
            {
                Current.CenterViewTo(X, Y, L);
                if (Animate)
                {
                    Current.PlayCenterAnimation(X, Y, L);
                }

                Army = ((AoWMapField)Current.MapContainer.GetField(X, Y, L)).GetArmyHS();
                if (Army != null)
                {
                    Selection.UnselectLocation();
                    Selection.SelectLocationHS(Army);
                }
            }
        }
    }

    // AoWHSMap.PAS:3167
    public void PlaceTerrain(sbyte X, sbyte Y, sbyte L, sbyte TerrainType, sbyte OverlayType)
    {
        EObjectList Resources;
        DelphiList Candidates;
        MapObjectResource Resource;
        RandSeed = Current.Random(16777215);
        Resources = AoWHSSet.Current.MapObjectResourceList.FChildren;
        if (!Set<byte>.WithBounds(0, 255).With(4).With(5).Contains((byte)OverlayType))
        {
            Candidates = new DelphiList();
            Candidates.Clear();
            for (int I = 0, last = Resources.Count - 1; I <= last; I++)
            {
                Resource = (MapObjectResource)Resources.List[I];
                if (Resource.EClass.ClassID == 131431 && Resource.TerrainList.UsedTerrain() == 1 && Resource.TerrainList.Terrain[0] == TerrainType && Resource.TerrainList.Overlay[0] == OverlayType)
                {
                    Candidates.Add(Resource);
                }
            }

            if (Candidates.Count != 0)
            {
                ((MapObjectResource)Candidates[Random(Candidates.Count)]).PlaceResourceUser(Current, X, Y, L);
            }

            Candidates?.Dispose();
        }
        else
        {
            AoWHSSet.Current.RoadControl.Place(Current.MapContainer, X, Y, L, TerrainType);
        }
    }

    // AoWHSMap.PAS:3195
    public void UpdateEffects()
    {
        int I;
        WorldFXHS Effect;
        if (!Status.Contains(HSMapStatusElement.Editting))
        {
            I = 0;
            while (I < FEffects.Count)
            {
                Effect = FEffects[I];
                if (Effect.FDone)
                {
                    DynamicFixedVisibleHS.RemoveChild(Effect);
                    DynamicVisibleHS.RemoveChild(Effect);
                    Effect?.Dispose();
                    FEffects.RemoveAt(I);
                }
                else
                {
                    I += 1;
                }
            }
        }
    }

    // AoWHSMap.PAS:3219
    public WorldFXHS AddEffect(TworldFXType EffectType)
    {
        WorldFXHS Effect;
        Effect = new WorldFXHS(null);
        FEffects.Add(Effect);
        Effect.Activate();
        Effect.FEffectType = EffectType;
        DynamicFixedVisibleHS.AddChild(Effect);
        return Effect;
    }

    public bool ValidateObservation(sbyte player, sbyte x, sbyte y, sbyte level, int radius)
    {
        if (!FShowIndependentMoves || player == Current.FSeatedPlayer) return false;
        HNWalkData walk = default;
        HNWalkData.InitHNWalk(ref walk, x, y, 0);
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
                    result = (((field.FFieldFlags >> 16) & 0xff) != 0 || !Current.FFogEnabled) &&
                        (field.GetExplored(Current.FSeatedPlayer) || !Current.FExplorationEnabled);
                }
            }
            if (!result) HNWalkData.WalkToNextHN(ref walk);
        }
        if (result) MakeVisible(walk.X, walk.Y, level, 3);
        return result;
    }

    private static readonly string MapSuccessfullyValidatedRStr = "Map validated successfully";

    private static readonly string MapValidationFailedRStr = "Map validation failed!";

    private static readonly string LoadingLevelRStr = "Loading Level ";

    private static readonly string InitializingLevelRStr = "Initializing Level ";

    private static readonly string UpdatingTerrainLevelRStr = "Updating Terrain Level ";

    private static readonly string ActivatingLevelRStr = "Activating Level ";

    private static readonly string SavingLevelRStr = "Saving Level ";

    private static readonly string DestroyingLevelRStr = "Destroying Level ";

    private static bool TryEndPlayerTurn(sbyte Player)
    {
        bool Result;
        if (Player != -1 && !Current.FPlayers.GetPlayers(Player).GetBusy() && !Current.FPlayers.GetPlayers(Player).FSingleProcessCycle)
        {
            Result = AbilityRules.EndPlayerTurn(Player);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static bool CancelPlayerTurn(sbyte Player)
    {
        bool Result;
        EndTurnTE Event;
        if (!Current.FPlayers.GetPlayers(Player).GetBusy() && Current.FPlayers.GetPlayers(Player).FSingleProcessCycle)
        {
            Event = new EndTurnTE(null);
            Event.FPlayer = Player;
            Event.FDay = Current.FDay;
            Event.FMode = EndTurnTE.EndTurnMode.CancelEndTurn;
            Current.FTokenManager.GetTokenControl(Player).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static readonly string OneMomentPleaseStr = "One moment please...";

    public static AoWHSMap Current;

    // { Interface declaration run owned by P5. }
    // allied victory. . points. gold. mana. next scenario
    public partial class AoWMapCampaignSettings : EObject
    {
        // Diplomacy.pas:1716
        public AoWMapCampaignSettings(ECustomNode owner) : base(owner)
        {
            FTransferPointsMax = 1000;
            FTransferGoldMax = 100;
            FTransferManaMax = 100;
            FHeroLevelMax = -1;
            FAlliedVictory = false;
        }

        public bool FAlliedVictory;
        public int FTransferPointsMax;
        public int FTransferGoldMax;
        public int FTransferManaMax;
        public int FHeroLevelMax;

        // Diplomacy.pas:1726
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTransferPointsMax, 20);
            Stream.rwInteger(ref FTransferGoldMax, 21);
            Stream.rwInteger(ref FTransferManaMax, 22);
            Stream.rwInteger(ref FHeroLevelMax, 23);
            Stream.rwBool(ref FAlliedVictory, 24);
        }

        // intf_P5_08.inc:13

    }

    public enum GameStatus : byte
    {
        Idle = 0, Playing = 1, Terminated = 2
    }

    public enum GameResult : byte
    {
        None = 0, Failed = 1, Succeeded = 2
    }

    public enum VictoryCondition : byte
    {
        None = 0, CaptureLocations = 1, AccumulateGold = 2, AccumulateMana = 3
    }

    public enum TurnOrderMode : byte
    {
        Fixed = 0, Random = 1, Changing = 2
    }

    public enum IndependentAIMode : byte
    {
        Off = 0, Passive = 1, Normal = 2, Aggressive = 3
    }

}
