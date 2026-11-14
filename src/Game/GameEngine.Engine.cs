// Translated from Engine.pas.
using System.Runtime.InteropServices;
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
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;

using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game;

// FMsgCount, FEClasses, FClassTable, FEditors, FControlNode, FComponent (the owning component),
// DebugMessage, FStartupDirectory, FOptions
public partial class GameEngine : ENode
{
    private int _messageCount;
    private readonly List<EngineClass> _engineClasses;
    private readonly Dictionary<int, EngineClass> _classTable;
    private readonly EngineObjectEditorList _editors;
    private readonly EControlStyleNode _controlNode;

    internal EventStore<EngineDebugNotification> DebugEventsStore { get; set; }

    internal event NotifyEvent<EngineDebugNotification> DebugMessage
    {
        add => DebugEventsStore.Add(value);
        remove => DebugEventsStore.Remove(value);
    }

    private string _startupDirectory = "";

    public Set<EngineUserElement> User { get; private set; }

    private Set<EngineModeElement> _options;

    // Create sets this before the instance exists. InitializeBeforeInherited reads it from the
    // base constructor, which runs before the body below.
    private static bool s_fullConstruction;

    // The map and game dispose work runs only after Create's constructor finishes.
    private readonly bool _fullInstance;

    // Engine.pas:8814
    public override GameEngine GetEngine() => this;

    public GameEngine(ECustomNode owner) : this(null, owner) { }

    private protected GameEngine(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
        UpdateStartupDirectory();
        _options = Set<EngineModeElement>.Of(VerifyClasses);
        DebugEventsStore = new();
        _engineClasses = new List<EngineClass>();
        _classTable = new Dictionary<int, EngineClass>();
        _editors = new EngineObjectEditorList();
        _controlNode = new EControlStyleNode(this);
        _controlNode.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Shared));
        RegisterEngineClasses([EngineClass.Of<ControlStyleObject>(), EngineClass.Of<EControlStyleNode>(), EngineClass.Of<ECustomOwner>(), EngineClass.Of<ResourceEditSettings>()]);
        if (!s_fullConstruction) return;

        OnHsmLoadedStore = new();
        OnHsmCloseStore = new();
        _onHssLoadedStore = new();
        _onHssCloseStore = new();
        // HSEngine registered the engine level (65794) and set (66049) here. Issue 200 folded them
        // into the game classes, which the game registers below under their own ids. No file stores
        // the engine ids.
        RegisterEngineClasses([EngineClass.Of<MapContainer>(), EngineClass.Of<GameMap>()]);
        RegisterEngineClasses([EngineClass.Of<MapObjectResource>(), EngineClass.Of<MultiHexMapObjectResource>()]);
        RegisterEngineClasses([EngineClass.Of<AbstractHexagon>(), EngineClass.Of<AbstractBorderHexagon>(), EngineClass.Of<AbstractRoad>()]);
        _hssFileName = "noname.hss";
        CreateHexSpriteSet();

        Options = Options - Set<EngineModeElement>.Of(EngineModeElement.VerifyEObjects);
        RegisterEngineClasses([EngineClass.Of<GamePack>(), EngineClass.Of<StrategicMap>(), EngineClass.Of<MapLevel>(), EngineClass.Of<Campaign>(), EngineClass.Of<CampaignScenario>()]);
        RegisterEngineClasses([EngineClass.Of<Player>(), EngineClass.Of<Race>(), EngineClass.Of<Unit>(), EngineClass.Of<RaceResource>(), EngineClass.Of<UnitGraphicsResource>(), EngineClass.Of<UnitResource>(), EngineClass.Of<UnitHexSprite>()]);
        RegisterEngineClasses([EngineClass.Of<Hero>(), EngineClass.Of<HeroResource>(), EngineClass.Of<HeroUpgradeTokenEvent>(), EngineClass.Of<HeroJoinEventLog>(), EngineClass.Of<HeroEmergeEventLog>(), EngineClass.Of<Leader>(), EngineClass.Of<ItemGraphics>(), EngineClass.Of<Item>(), EngineClass.Of<ItemTokenEvent>(), EngineClass.Of<ItemHexSprite>(), EngineClass.Of<FaceResource>()]);
        RegisterEngineClasses([EngineClass.Of<Army>(), EngineClass.Of<ArmyHexSprite>(), EngineClass.Of<ArmyView>(), EngineClass.Of<ArmyEventLog>(), EngineClass.Of<ArmyId>(), EngineClass.Of<TurnInfoItem>()]);
        RegisterEngineClasses([EngineClass.Of<ArmyDefaultMoveTokenEvent>(), EngineClass.Of<ArmyCombatMoveTokenEvent>(), EngineClass.Of<ArmyCombineMoveTokenEvent>(), EngineClass.Of<ArmyNegotiateMoveTokenEvent>(), EngineClass.Of<NegotiateTokenEvent>(), EngineClass.Of<SaveGameTokenEvent>(), EngineClass.Of<ChangePlayerMagicTokenEvent>(), EngineClass.Of<ChangeGlobalMagicTokenEvent>()]);
        RegisterEngineClasses([EngineClass.Of<CombatLogbook>(), EngineClass.Of<CombatParty>(), EngineClass.Of<CombatData>(), EngineClass.Of<DamageCombatAction>(), EngineClass.Of<FleeCombatAction>(), EngineClass.Of<CombatUnit>()]);
        RegisterEngineClasses([EngineClass.Of<FastBattle>(), EngineClass.Of<CombatWall>(), EngineClass.Of<FastBattleUnit>(), EngineClass.Of<MoveWallCombatAction>(), EngineClass.Of<ProductionEventLog>()]);
        RegisterEngineClasses([EngineClass.Of<UpdateHeaderCombatAction>(), EngineClass.Of<NewCombatTurnCombatAction>(), EngineClass.Of<NewCombatRoundCombatAction>(), EngineClass.Of<CombatObjectTurnDoneCombatAction>()]);
        RegisterEngineClasses([EngineClass.Of<CombatEventLog>(), EngineClass.Of<CombatPlayer>(), EngineClass.Of<DiplomaticAction>(), EngineClass.Of<EmailGame>(), EngineClass.Of<RestartGameEventLog>()]);
        RegisterEngineClasses([EngineClass.Of<Structure>(), EngineClass.Of<StructureResource>(), EngineClass.Of<StructureTokenEvent>(), EngineClass.Of<ConstructStructureTokenEvent>(), EngineClass.Of<ProductionTokenEvent>(), EngineClass.Of<HeroUpdateTokenEvent>()]);
        RegisterEngineClasses([EngineClass.Of<PlayerStructure>(), EngineClass.Of<PlayerStructureResource>(), EngineClass.Of<ProductionItem>(), EngineClass.Of<UnitProduction>()]);
        RegisterEngineClasses([EngineClass.Of<Fire>(), EngineClass.Of<Fire.Resource>()]);
        RegisterEngineClasses([EngineClass.Of<NewDayEventLog>(), EngineClass.Of<ArmyInfoEventLog>(), EngineClass.Of<TextControlItem>(), EngineClass.Of<HeroUpgradeEventLog>()]);
        RegisterEngineClasses([EngineClass.Of<DiplomaticActionTokenEvent>(), EngineClass.Of<DiplomaticActionEventLog>(), EngineClass.Of<PlayerMagicEventLog>(), EngineClass.Of<DiplomaticRelationHistoryItem>(), EngineClass.Of<DiplomaticRelationHistoryList>()]);
        RegisterEngineClasses([EngineClass.Of<HuntAIGroup>(), EngineClass.Of<AIBudget>(), EngineClass.Of<AISavingsBudget>(), EngineClass.Of<AITransporterBudget>()]);
        RegisterGroupControls();
        RegisterSoundEngineClasses(this);
        RegisterNetworkEngine(this);
        RegisterMoveEvents(this);
        RegisterEventLog(this);
        RegisterAbilities(GameSet.AbilityControl);
        RegisterSpells(this);
        Editors.RegisterClass(EditorClass.Of<MapObjectEditor>());
        EmailGames.Load();
        _fullInstance = true;
    }

    // Engine.pas:8745, then the HSEngine and AoWEngine dispose bodies when Create built the instance.
    // Create's failure path disposes the instance before it clears the flag, so that path runs the same body.
    public override void Dispose()
    {
        if (_fullInstance || s_fullConstruction)
        {
            CloseCampaign();
            CloseHsm(null);
            UnregisterCoreSpells(this);
            UnregisterCoreAbilities(GameSet.AbilityControl);
            OnHsmLoadedStore.Clear();
            OnHsmCloseStore.Clear();
            CloseHsm(null);
        }

        DebugEventsStore.Clear();
        base.Dispose();
        _engineClasses?.Clear();
        if (_fullInstance || s_fullConstruction)
        {
            OnHsmLoadedStore.Clear();
            OnHsmCloseStore.Clear();
            _onHssLoadedStore.Clear();
            _onHssCloseStore.Clear();
            Dictionary?.Dispose();
            EmailGames?.Dispose();
        }
    }

    // Engine.pas:8771
    public virtual void SetStartupDirectory(string value)
    {
        _startupDirectory = value;
    }

    // Engine.pas:8766. A bare engine has no registry, so it keeps this body. Create has already
    // run InitializeBeforeInherited, and that path keeps the AoWEngine body.
    public virtual void UpdateStartupDirectory()
    {
        if (Registry is null)
        {
            _startupDirectory = ExtractFilePath(RtlUnit.Application.ExeName);
            return;
        }

        if (!IsEmpty(Registry.GetStartupDirectory()))
        {
            SetStartupDirectory(Registry.GetStartupDirectory());
        }
        else
        {
            SetStartupDirectory(ExtractFilePath(RtlUnit.Application.ExeName));
        }

        if (StartupDirectory[StartupDirectory.Length - 1] != 92)
        {
            SetStartupDirectory(StartupDirectory + "\\");
        }

        ImageLibraries.BaseImageLibraryPath = StartupDirectory;
        AudioHost.BaseSoundEnginePath = StartupDirectory;
        Dictionary.FileName = StartupDirectory + "Dict\\" + "ResStr" + ".mld";
        Dictionary.Open();
    }

    // Engine.pas:8887
    public virtual EngineObject CreateEngineObject(int classId, ECustomNode owner)
    {
        EngineClass C;
        C = GetEngineClass(classId);
        return C.CreateInstance(owner, holder: this);
    }

    // Engine.pas:8860
    public virtual EngineClass GetEngineClass(int classId)
    {
        if (!_classTable.TryGetValue(classId, out EngineClass result) || result == null)
        {
            throw new KeyNotFoundException("Unregistered E-ClassID found (" + IntToStr(classId) + ")");
        }

        return result;
    }

    // Engine.pas:8796
    public void OutputDebugString(string S)
    {
        var notification = new EngineDebugNotification();
        ref EngineDebugNotification.DebugInfo info = ref notification.Info;
        info.Channel = DebugChannel.User;
        info.Text = S;
        DebugEventsStore.Raise(notification);
    }

    // Engine.pas:8805
    public void OutputDebugMessage(DebugChannel channel, string S)
    {
        var notification = new EngineDebugNotification();
        ref EngineDebugNotification.DebugInfo info = ref notification.Info;
        info.Channel = channel;
        info.Text = S;
        DebugEventsStore.Raise(notification);
    }

    // Engine.pas:8819
    public void ResetMessageCount()
    {
        EngineMessage message = default;
        InitializeMessage(out message, 1, this, _messageCount + 1);
        MessageProc(ref message, this);
        _messageCount = 0;
    }

    // Engine.pas:8828
    public int IncrementMessageCount()
    {
        _messageCount += 1;
        if (_messageCount > 268435455)
        {
            ResetMessageCount();
        }

        return _messageCount;
    }

    // Engine.pas:8835
    public void RegisterEngineClasses(ReadOnlySpan<EngineClass> classes)
    {
        for (int I = 0, last = classes.Length - 1; I <= last; I++)
        {
            if (_options.Contains(VerifyClasses))
            {
                VerifyEngineObjectClass(classes[I]);
            }

            var classId = classes[I].ClassId;
            if (_classTable.ContainsKey(classId))
            {
                throw new InvalidOperationException("EObject Class ID double defined!");
            }

            _engineClasses.Add(classes[I]);
            HashTables.Add(_classTable, key: classes[I].ClassId, value: classes[I]);
        }
    }

    // Engine.pas:8849
    public void UnregisterEngineClasses(ReadOnlySpan<EngineClass> classes)
    {
        for (int I = 0, last = classes.Length - 1; I <= last; I++)
        {
            _engineClasses.Remove(classes[I]);
            var classId = classes[I].ClassId;
            _classTable.Remove(classId);
        }
    }

    // Engine.pas:8867
    public EngineObjectClassList GetDescendantClasses(Type classValue)
    {
        EngineObjectClassList L;
        L = new EngineObjectClassList();
        for (int I = 0, last = _engineClasses.Count - 1; I <= last; I++)
        {
            if (classValue.IsAssignableFrom(_engineClasses[I].Type))
            {
                L.Add(_engineClasses[I]);
            }
        }

        return L;
    }

    // Engine.pas:8878
    public void ActivateEngineObject(EngineObject objectValue)
    {
        EngineMessage message = default;
        EngineMessage.InitializeMessage(out message, 536870920, this);
        message.Payload = this;
        objectValue.MessageProc(ref message, this);
    }

    // Engine.pas:8947
    public void StoreEngineObject(Stream stream, EngineObject objectValue, Engine.ProgressEvent progress)
    {
        StorageStream S;
        S = new StorageWriter(stream, this, false);
        S.OnProgress += progress;
        try
        {
            S.Created(ref objectValue, null, 0);
        }
        finally
        {
            S?.Dispose();
        }
    }

    // Engine.pas:8960
    public EngineObject RetrieveEngineObject(Stream stream, Engine.ProgressEvent progress)
    {
        EngineObject result = default;
        StorageStream S;
        S = new StorageReader(stream, this, false);
        S.OnProgress += progress;
        try
        {
            S.Created(ref result, null, 0);
        }
        finally
        {
            S?.Dispose();
        }

        return result;
    }

    // Engine.pas:8973
    // The file goes through the game's file boundary with the caller's profile. A refused
    // preflight raises EGameFileError before an object is created (own-formats.md 4.1).
    public EngineObject RetrieveFromFile(string fileName, Engine.ProgressEvent progress, GameProfile profile)
    {
        return RetrieveFromTree(FileBoundary.OpenOrRaise(fileName, profile: profile), progress: progress);
    }

    // Retrieves the object of a tree whose preflight has passed, as RetrieveEObject retrieves the
    // object of a stream: the stored class is created and loaded, and a failure gives nil.
    public EngineObject RetrieveFromTree(GameFileTree tree, Engine.ProgressEvent progress)
    {
        return (EngineObject)TreeReader.ReadCreated(tree.Root, className: tree.ClassName, factory: new EnginePersistableFactory(this, progress: progress));
    }

    // Loads the tree of a prepared file into an object the caller holds, as ReadEObject loads a
    // stream (persistence-tree.md 6 and 7). A failure of the object goes to the error handler as
    // rwEObject passes it to OnError. No handler continues.
    public void ReadTree(GameFileTree tree, EngineObject objectValue, Engine.ProgressEvent progress, bool preview = false, LoadErrorHandler onError = null)
    {
        TreeReader.ReadObject(tree.Root, objectValue: objectValue, factory: new EnginePersistableFactory(this, progress: progress), onError: onError, preview: preview);
    }

    // Engine.pas:8985
    // The tree of the object goes to the file boundary, which converts, validates and replaces the
    // file only after the whole document is serialized.
    public void StoreInFile(string fileName, EngineObject objectValue, Engine.ProgressEvent progress, GameProfile profile)
    {
        FileBoundary.Save(fileName, profile: profile, root: objectValue, progress: progress);
    }

    // An engine image in a memory stream at position 0, for the routines that read it.
    public static MemoryStream ImageStream(byte[] image)
    {
        MemoryStream M = new MemoryStream();
        M.WriteBuffer(image);
        M.Position = 0;
        return M;
    }

    // The in-scope form of WriteToFile for the hero and item lists (own-formats.md 4.1). The
    // three-argument form below stays the original byte path of the excluded PFS callers.
    public void WriteToFile(string fileName, EngineObject objectValue, Engine.ProgressEvent progress, GameProfile profile)
    {
        FileBoundary.Save(fileName, profile: profile, root: objectValue, progress: progress);
    }

    // The in-scope form of ReadFromFile. A refused preflight raises before Obj is read.
    public void ReadFromFile(string fileName, EngineObject objectValue, Engine.ProgressEvent progress, GameProfile profile)
    {
        ReadTree(FileBoundary.OpenOrRaise(fileName, profile: profile), objectValue: objectValue, progress: progress);
    }

    // Engine.pas:8997
    // The original image of the object goes to the file: the tree through the original backend,
    // as WriteEObject wrote it (persistence-tree.md 13). The system hero and item libraries and
    // the e-mail game index stay original files.
    public void WriteToFile(string fileName, EngineObject objectValue, Engine.ProgressEvent progress)
    {
        var image = WriteObjectImage(objectValue, progress: progress, classTable: "game");
        Stream F = OpenFile(fileName, 65535);
        try
        {
            F.Write(image);
        }
        finally
        {
            F?.Dispose();
        }
    }

    // Engine.pas:9017
    public void ReadFromFile(string fileName, EngineObject objectValue, Engine.ProgressEvent progress)
    {
        MemoryStream M;
        Stream F;
        M = new MemoryStream();
        F = OpenFile(fileName, 0 | 32);
        try
        {
            M.LoadFromStream(F);
        }
        finally
        {
            F?.Dispose();
        }

        try
        {
            ReadObjectImage(M.ToArray(), objectValue: objectValue, progress: progress, classTable: "game");
        }
        finally
        {
            M?.Dispose();
        }
    }

    // Engine.pas:9037
    // A game set file: the object image between two CRC words. classTable names the engine table
    // of the classes the file holds ("tactical" for the combat resources).
    public void WriteToFileCrc(string fileName, EngineObject objectValue, Engine.ProgressEvent progress, string classTable = "game")
    {
        int crc1 = default;
        int crc2 = default;
        MemoryStream M;
        Stream F = default;
        M = new MemoryStream();
        try
        {
            M.Write(WriteObjectImage(objectValue, progress: progress, classTable: classTable));
            var size = (int)M.Length;
            crc1 = EngineServices.GetCrc32(M.GetBuffer(), size);
            M.Write(Bytes.Of(ref crc1, 4));
            var mSize = (int)M.Length;
            crc2 = EngineServices.GetCrc32(M.GetBuffer(), mSize);
            F = OpenFile(fileName, 65535);
            try
            {
                F.Write(Bytes.Of(ref crc2, 4));
                var size2 = (int)M.Length;
                F.Write(M.GetBuffer().AsSpan(0, size2));
            }
            finally
            {
                F?.Dispose();
            }
        }
        finally
        {
            M?.Dispose();
        }
    }

    // Engine.pas:9061
    public bool ReadFromFileCrc(string fileName, EngineObject objectValue, Engine.ProgressEvent progress, string classTable = "game")
    {
        MemoryStream M;
        Stream F;
        int crc2;
        int crc1;
        bool result = false;
        M = new MemoryStream();
        F = OpenFile(fileName, 0 | 32);
        try
        {
            M.LoadFromStream(F);
            M.Position = 0;
            if (M.Length > 8)
            {
                crc1 = MemoryMarshal.Read<int>(M.GetBuffer().AsSpan(0, 4));
                var size = (int)M.Length;
                crc2 = MemoryMarshal.Read<int>(M.GetBuffer().AsSpan(size - 4, 4));
                var mSize = (int)M.Length;
                bool condition = crc1 == EngineServices.GetCrc32(M.GetBuffer().AsSpan(4), mSize - 4);
                if (condition)
                {
                    var size2 = (int)M.Length;
                    condition = crc2 == EngineServices.GetCrc32(M.GetBuffer().AsSpan(4), size2 - 8);
                }

                if (condition)
                {
                    var imageSize = (int)M.Length - 8;
                    ReadObjectImage(M.GetBuffer().AsSpan(4, imageSize).ToArray(), objectValue: objectValue, progress: progress, classTable: classTable);
                    result = true;
                }
            }
        }
        finally
        {
            F?.Dispose();
            M?.Dispose();
        }

        return result;
    }

    // The image TEngine.WriteEObject writes for an object (Engine.pas:8895): the tree of the object
    // through the original backend under its class, inside the outer table of id 0. classTable
    // names the engine table of the class ids of the created objects inside.
    public byte[] WriteObjectImage(EngineObject objectValue, Engine.ProgressEvent progress, string classTable)
    {
        var factory = new EnginePersistableFactory(this, progress: progress);
        var root = TreeWriter.Write(objectValue, factory: factory);
        return GameBoundary.WriteWrittenObject(root, className: factory.ClassName(objectValue), classTable: classTable);
    }

    // Loads an object image as TEngine.ReadEObject loads it (Engine.pas:8908): the original backend
    // reads the root under the class of the object, and the tree loads into the object. A failure
    // of the object goes to the load's handler as rwEObject passes it to OnError, and no handler
    // continues.
    public void ReadObjectImage(byte[] image, EngineObject objectValue, Engine.ProgressEvent progress, string classTable)
    {
        var factory = new EnginePersistableFactory(this, progress: progress);
        var root = GameBoundary.ReadWrittenObject(image, className: factory.ClassName(objectValue), classTable: classTable);
        TreeReader.ReadObject(root, objectValue: objectValue, factory: factory);
    }

    public List<EngineClass> EngineClasses { get => _engineClasses; }
    public Dictionary<int, EngineClass> ClassTable { get => _classTable; }
    public EngineObjectEditorList Editors { get => _editors; }
    public EControlStyleNode ControlNode { get => _controlNode; }

    public event NotifyEvent<EngineDebugNotification> DebugEvents
    {
        add => DebugEventsStore.Add(value);
        remove => DebugEventsStore.Remove(value);
    }

    public string StartupDirectory { get => _startupDirectory; set => SetStartupDirectory(value); }
    public Set<EngineModeElement> Options { get => _options; set => _options = value; }

    // Engine.pas:1126
    public bool VerifyEngineObjectClass(EngineClass type)
    {
        // The native allocator byte total has no corresponding managed heap measure.
        // Exercise the same constructor and destructor without comparing GC generations.
        type.CreateInstance(null, holder: this)?.Dispose();
        return true;
    }

    internal const EngineModeElement VerifyClasses = 0;

    private static void InitializeMessage(out EngineMessage message, int messageId, EngineObject sender, int sequence)
    {
        message = default;
        message.MessageId = messageId;
        message.Sequence = sequence;
        message.Sender = sender;
        message.Result = -1;
    }

    internal static void UnregisterSoundEngineClasses(GameEngine engine)
    {
        engine.UnregisterEngineClasses([EngineClass.Of<SfxSequence>(), EngineClass.Of<SfxLibrary>(), EngineClass.Of<Sfx>()]);
    }

    internal static void UnregisterStorms(GameEngine engine)
    {
        SpellControl spells;
        spells = engine.GameSet.SpellControl;
        spells.UnregisterSpellId(3);
        spells.UnregisterSpellId(5);
        spells.UnregisterSpellId(6);
        spells.UnregisterSpellId(7);
        spells.UnregisterSpellId(4);
        spells.UnregisterSpellId(9);
    }

    internal static void UnregisterMountain(GameEngine engine)
    {
        engine.UnregisterEngineClasses([EngineClass.Of<MountainMapObject>(), EngineClass.Of<MountainMapObjectResource>(), EngineClass.Of<RaiseTerrainTokenEvent>(), EngineClass.Of<RaisedMountain>(), EngineClass.Of<RaisedMountain.Resource>()]);
        // { Original unregisters spell 12, although Raise Terrain registers as spell 19. }
        engine.GameSet.SpellControl.UnregisterSpellId(12);
    }

    internal static void RegisterBorderHex(GameEngine engine)
    {
        engine.RegisterEngineClasses([EngineClass.Of<BorderHex>(), EngineClass.Of<BorderHex.Resource>()]);
    }

    internal static void RegisterHex(GameEngine engine)
    {
        engine.RegisterEngineClasses([EngineClass.Of<Hex>(), EngineClass.Of<HexagonResource>()]);
    }

    internal static void RegisterIsometricHex(GameEngine engine)
    {
        engine.RegisterEngineClasses([EngineClass.Of<IsometricHex>(), EngineClass.Of<IsometricHexResource>(), EngineClass.Of<UpperIsometricHex>(), EngineClass.Of<UpperIsometricHex.Resource>(), EngineClass.Of<LowerIsometricHex>(), EngineClass.Of<LowerIsometricHex.Resource>()]);
    }

    internal static void RegisterRoad(GameEngine engine)
    {
        engine.RegisterEngineClasses([EngineClass.Of<Road>(), EngineClass.Of<Road.Resource>()]);
    }

    internal static void RegisterTerrain(GameEngine engine)
    {
        engine.RegisterEngineClasses([EngineClass.Of<TerrainMapObject>(), EngineClass.Of<TerrainMapObjectResource>()]);
    }

    internal static void RegisterHill(GameEngine engine)
    {
    }

    internal static void RegisterAltar(GameEngine engine)
    {
        engine.RegisterEngineClasses([EngineClass.Of<Altar>(), EngineClass.Of<AltarResource>()]);
    }

    internal static void RegisterRandomNode(GameEngine engine)
    {
    }

    internal static void UnregisterHolyGround(GameEngine engine)
    {
        engine.UnregisterEngineClasses([EngineClass.Of<HolyGround>(), EngineClass.Of<HolyGround.Resource>(), EngineClass.Of<HolyGroundSpellTokenEvent>()]);
        engine.GameSet.SpellControl.UnregisterSpellId(13);
    }

    internal static void UnregisterOverlayObject(GameEngine engine)
    {
        engine.UnregisterEngineClasses([EngineClass.Of<OverlayObject>(), EngineClass.Of<OverlayObject.Resource>()]);
    }

}
