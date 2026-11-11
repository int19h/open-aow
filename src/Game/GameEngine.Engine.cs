// Translated from Engine.pas.
using System.Runtime.InteropServices;
using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;
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
// FDebugIDs, FDebugEvents, FStartupDirectory, FOptions. Every engine is in the Engines list
public partial class GameEngine : ENode
{
    internal int FMsgCount;
    internal List<EngineClass> FEClasses;
    internal Dictionary<int, EngineClass> FClassTable;
    internal EngineObjectEditorList FEditors;
    internal EControlStyleNode FControlNode;
    public EngineComponent FComponent;
    internal StringList FDebugIDs;

    internal EventStore<EngineDebugNotification> FDebugEventsStore;

    internal event NotifyEvent<EngineDebugNotification> FDebugEvents
    {
        add => FDebugEventsStore.Add(value);
        remove => FDebugEventsStore.Remove(value);
    }

    internal string FStartupDirectory = "";
    public Set<EngineUserElement> FUser;
    internal Set<EngineModeElement> FOptions;

    // Create sets this before the instance exists. InitializeBeforeInherited reads it from the
    // base constructor, which runs before the body below.
    private static bool fullConstruction;

    // The map and game dispose work runs only after Create's constructor finishes.
    private bool fullInstance;

    // Engine.pas:8776
    internal int CreateUniqueDebugID()
    {
        int Result = 0;
        while (FDebugIDs.IndexOfObject(IntToObject(Result)) != -1)
        {
            Result += 1;
        }

        return Result;
    }

    // Engine.pas:8814
    public override GameEngine GetEngine() => this;

    public GameEngine(ECustomNode owner) : this(null, owner) { }

    private protected GameEngine(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
        UpdateStartupDirectory();
        FOptions = Set<EngineModeElement>.Of(eoVerifyClasses);
        FDebugEventsStore = new();
        FEClasses = new List<EngineClass>();
        FClassTable = new Dictionary<int, EngineClass>();
        FEditors = new EngineObjectEditorList();
        FControlNode = new EControlStyleNode(this);
        FControlNode.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.csShared));
        FDebugIDs = new StringList();
        FDebugIDs.AddObject(UserStr, IntToObject(-1));
        RegisterEClasses([EngineClass.Of<ControlStyleObject>(), EngineClass.Of<EControlStyleNode>(), EngineClass.Of<ECustomOwner>(), EngineClass.Of<ResourceEditSettings>()]);
        EngineHost.Engines.Add(this);
        if (!fullConstruction) return;

        FOnHSMLoadedStore = new();
        FOnHSMCloseStore = new();
        FOnHSSLoadedStore = new();
        FOnHSSCloseStore = new();
        RegisterEClasses([EngineClass.Of<MapContainer>(), EngineClass.Of<MapLevel>(), EngineClass.Of<HSMap>(), EngineClass.Of<HSSet>()]);
        RegisterEClasses([EngineClass.Of<MapObjectResource>(), EngineClass.Of<MultiHexMapObjectResource>()]);
        RegisterEClasses([EngineClass.Of<AbstractHexagon>(), EngineClass.Of<AbstractBorderHexagon>(), EngineClass.Of<AbstractRoad>()]);
        HSEngineDID = RegisterDebugID("HS-Engine");
        FHSSFileName = "noname.hss";
        CreateHSSet();

        Options = Options - Set<EngineModeElement>.Of(EngineModeElement.VerifyEObjects);
        RegisterEClasses([EngineClass.Of<AoWHSSet>(), EngineClass.Of<AoWHSMap>(), EngineClass.Of<AoWMapLevel>(), EngineClass.Of<Campaign>(), EngineClass.Of<CampaignScenario>()]);
        RegisterEClasses([EngineClass.Of<Player>(), EngineClass.Of<Race>(), EngineClass.Of<Unit>(), EngineClass.Of<RaceResource>(), EngineClass.Of<UnitGraphicsResource>(), EngineClass.Of<UnitResource>(), EngineClass.Of<UnitHexSprite>()]);
        RegisterEClasses([EngineClass.Of<Hero>(), EngineClass.Of<HeroResource>(), EngineClass.Of<HeroUpgradeTokenEvent>(), EngineClass.Of<HeroJoinEventLog>(), EngineClass.Of<HeroEmergeEventLog>(), EngineClass.Of<Leader>(), EngineClass.Of<ItemGraphics>(), EngineClass.Of<Item>(), EngineClass.Of<ItemTokenEvent>(), EngineClass.Of<ItemHexSprite>(), EngineClass.Of<FaceResource>()]);
        RegisterEClasses([EngineClass.Of<Army>(), EngineClass.Of<ArmyHexSprite>(), EngineClass.Of<ArmyView>(), EngineClass.Of<ArmyEventLog>(), EngineClass.Of<ArmyId>(), EngineClass.Of<TurnInfoItem>()]);
        RegisterEClasses([EngineClass.Of<ArmyDefaultMoveTokenEvent>(), EngineClass.Of<ArmyCombatMoveTokenEvent>(), EngineClass.Of<ArmyCombineMoveTokenEvent>(), EngineClass.Of<ArmyNegotiateMoveTokenEvent>(), EngineClass.Of<NegotiateTokenEvent>(), EngineClass.Of<SaveGameTokenEvent>(), EngineClass.Of<ChangePlayerMagicTokenEvent>(), EngineClass.Of<ChangeGlobalMagicTokenEvent>()]);
        RegisterEClasses([EngineClass.Of<CombatLogbook>(), EngineClass.Of<CombatParty>(), EngineClass.Of<CombatData>(), EngineClass.Of<DamageCombatAction>(), EngineClass.Of<FleeCombatAction>(), EngineClass.Of<CombatUnit>()]);
        RegisterEClasses([EngineClass.Of<FastBattle>(), EngineClass.Of<CombatWall>(), EngineClass.Of<FastBattleUnit>(), EngineClass.Of<MoveWallCombatAction>(), EngineClass.Of<ProductionEventLog>()]);
        RegisterEClasses([EngineClass.Of<UpdateHeaderCombatAction>(), EngineClass.Of<NewCombatTurnCombatAction>(), EngineClass.Of<NewCombatRoundCombatAction>(), EngineClass.Of<CombatObjectTurnDoneCombatAction>()]);
        RegisterEClasses([EngineClass.Of<CombatEventLog>(), EngineClass.Of<CombatPlayer>(), EngineClass.Of<DiplomaticAction>(), EngineClass.Of<EmailGame>(), EngineClass.Of<RestartGameEventLog>()]);
        RegisterEClasses([EngineClass.Of<Structure>(), EngineClass.Of<StructureResource>(), EngineClass.Of<StructureTokenEvent>(), EngineClass.Of<ConstructStructureTokenEvent>(), EngineClass.Of<ProductionTokenEvent>(), EngineClass.Of<HeroUpdateTokenEvent>()]);
        RegisterEClasses([EngineClass.Of<PlayerStructure>(), EngineClass.Of<PlayerStructureResource>(), EngineClass.Of<ProductionItem>(), EngineClass.Of<UnitProduction>()]);
        RegisterEClasses([EngineClass.Of<Fire>(), EngineClass.Of<Fire.Resource>()]);
        RegisterEClasses([EngineClass.Of<NewDayEventLog>(), EngineClass.Of<ArmyInfoEventLog>(), EngineClass.Of<TextControlItem>(), EngineClass.Of<HeroUpgradeEventLog>()]);
        RegisterEClasses([EngineClass.Of<DiplomaticActionTokenEvent>(), EngineClass.Of<DiplomaticActionEventLog>(), EngineClass.Of<PlayerMagicEventLog>(), EngineClass.Of<DiplomaticRelationHistoryItem>(), EngineClass.Of<DiplomaticRelationHistoryList>()]);
        RegisterEClasses([EngineClass.Of<HuntAIGroup>(), EngineClass.Of<AIBudget>(), EngineClass.Of<AISavingsBudget>(), EngineClass.Of<AITransporterBudget>()]);
        P2RegisterGroupControls();
        RegisterSoundEngineClasses(this);
        RegisterNetworkEngine(this);
        P2RegisterMoveEvents(this);
        RegisterEventLog(this);
        P2RegisterAbilities(FGameSet.FAbilityControl);
        P2RegisterSpells(this);
        CombatAbilityTables.CombatDID = RegisterDebugID("Combat");
        AbilityRules.AIDID = RegisterDebugID("AI");
        AbilityRules.TMPDID = RegisterDebugID("TMP");
        MAGICDID = RegisterDebugID("MAGIC");
        Editors.RegisterClass(EditorClass.Of<MapObjectEditor>());
        LoadExternalPlugIns(StartupDirectory + "PLUGINS");
        FEmailGames.Load();
        fullInstance = true;
    }

    // Engine.pas:8745, then the HSEngine and AoWEngine dispose bodies when Create built the instance.
    // Create's failure path disposes the instance before it clears the flag, so that path runs the same body.
    public override void Dispose()
    {
        if (fullInstance || fullConstruction)
        {
            CloseCampaign();
            CloseHSM(null);
            UnregisterCoreSpells(this);
            UnregisterCoreAbilities(FGameSet.FAbilityControl);
            FOnHSMLoadedStore.Clear();
            FOnHSMCloseStore.Clear();
            CloseHSM(null);
        }

        EngineHost.Engines.Remove(this);
        FDebugEventsStore.Clear();
        FDebugIDs?.Dispose();
        base.Dispose();
        FEClasses?.Clear();
        if (fullInstance || fullConstruction)
        {
            FOnHSMLoadedStore.Clear();
            FOnHSMCloseStore.Clear();
            FOnHSSLoadedStore.Clear();
            FOnHSSCloseStore.Clear();
            FDictionary?.Dispose();
            FEmailGames?.Dispose();
        }
    }

    // Engine.pas:8771
    public virtual void SetStartupDirectory(string Value)
    {
        FStartupDirectory = Value;
    }

    // Engine.pas:8766. A bare engine has no registry, so it keeps this body. Create has already
    // run InitializeBeforeInherited, and that path keeps the AoWEngine body.
    public virtual void UpdateStartupDirectory()
    {
        if (FRegistry is null)
        {
            FStartupDirectory = ExtractFilePath(RtlUnit.Application.ExeName);
            return;
        }

        if (!IsEmpty(FRegistry.GetStartupDirectory()))
        {
            SetStartupDirectory(FRegistry.GetStartupDirectory());
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
        FDictionary.FFileName = StartupDirectory + "Dict\\" + "ResStr" + ".mld";
        FDictionary.Open();
    }

    // Engine.pas:8887
    public virtual EngineObject CreateEObject(int AClassID, ECustomNode AOwner)
    {
        EngineClass C;
        C = GetEClass(AClassID);
        return C.Create(AOwner);
    }

    // Engine.pas:8860
    public virtual EngineClass GetEClass(int AClassID)
    {
        if (!FClassTable.TryGetValue(AClassID, out EngineClass Result) || Result == null)
        {
            throw new KeyNotFoundException("Unregistered E-ClassID found (" + IntToStr(AClassID) + ")");
        }

        return Result;
    }

    // Engine.pas:8782
    public int RegisterDebugID(string Name)
    {
        int Result = CreateUniqueDebugID();
        FDebugIDs.AddObject(Name, IntToObject(Result));
        return Result;
    }

    // Engine.pas:8788
    public void UnregisterDebugID(int ID)
    {
        int I;
        I = FDebugIDs.IndexOfObject(IntToObject(ID));
        if (I != -1)
        {
            FDebugIDs.Delete(I);
        }
    }

    // Engine.pas:8796
    public void OutputDebugStr(string S)
    {
        var notification = new EngineDebugNotification();
        ref EngineDebugNotification.DebugInfo Info = ref notification.Info;
        Info.ID = -1;
        Info.Text = S;
        FDebugEventsStore.Raise(notification);
    }

    // Engine.pas:8805
    public void OutputDebugMessage(int ID, string S)
    {
        var notification = new EngineDebugNotification();
        ref EngineDebugNotification.DebugInfo Info = ref notification.Info;
        Info.ID = ID;
        Info.Text = S;
        FDebugEventsStore.Raise(notification);
    }

    // Engine.pas:8819
    public void ResetMsgCount()
    {
        EngineMessage Msg = default;
        InitMsgEx(ref Msg, 1, this, FMsgCount + 1);
        MsgProc(ref Msg, this);
        FMsgCount = 0;
    }

    // Engine.pas:8828
    public int IncMsgCount()
    {
        FMsgCount += 1;
        if (FMsgCount > 268435455)
        {
            ResetMsgCount();
        }

        return FMsgCount;
    }

    // Engine.pas:8835
    public void RegisterEClasses(ReadOnlySpan<EngineClass> AClasses)
    {
        for (int I = 0, last = AClasses.Length - 1; I <= last; I++)
        {
            if (FOptions.Contains(eoVerifyClasses))
            {
                VerifyEObjectClass(AClasses[I]);
            }

            var classID = AClasses[I].ClassID;
            if (FClassTable.ContainsKey(classID))
            {
                throw new InvalidOperationException("EObject Class ID double defined!");
            }

            FEClasses.Add(AClasses[I]);
            HashTables.Add(FClassTable, key: AClasses[I].ClassID, value: AClasses[I]);
        }
    }

    // Engine.pas:8849
    public void UnregisterEClasses(ReadOnlySpan<EngineClass> AClasses)
    {
        for (int I = 0, last = AClasses.Length - 1; I <= last; I++)
        {
            FEClasses.Remove(AClasses[I]);
            var classID = AClasses[I].ClassID;
            FClassTable.Remove(classID);
        }
    }

    // Engine.pas:8867
    public EngineObjectClassList GetDescendantClasses(Type AClass)
    {
        EngineObjectClassList L;
        L = new EngineObjectClassList();
        for (int I = 0, last = FEClasses.Count - 1; I <= last; I++)
        {
            if ((FEClasses[I]).InheritsFrom(AClass))
            {
                L.Add(FEClasses[I]);
            }
        }

        return L;
    }

    // Engine.pas:8878
    public void ActivateEObject(EngineObject Obj)
    {
        EngineMessage Msg = default;
        EngineMessage.InitMsg(ref Msg, 536870920, this);
        Msg.Payload = this;
        Obj.MsgProc(ref Msg, this);
    }

    // Engine.pas:8895
    public void WriteEObject(Stream Stream, EngineObject Obj, Engine.ProgressEvent AProgress)
    {
        EStorageStream S;
        S = new EWriteStorageStream(Stream, this, false);
        S.OnProgress = AProgress;
        try
        {
            S.rwEObject(Obj, 0);
        }
        finally
        {
            S?.Dispose();
        }
    }

    // Engine.pas:8908
    public void ReadEObject(Stream Stream, EngineObject Obj, Engine.ProgressEvent AProgress)
    {
        EStorageStream S;
        S = new EReadStorageStream(Stream, this, false);
        S.OnProgress = AProgress;
        try
        {
            S.rwEObject(Obj, 0);
        }
        finally
        {
            S?.Dispose();
        }
    }

    // Engine.pas:8921
    public void WritePreviewEObject(Stream Stream, EngineObject Obj, Engine.ProgressEvent AProgress)
    {
        EStorageStream S;
        S = new EWriteStorageStream(Stream, this, true);
        S.OnProgress = AProgress;
        try
        {
            S.rwEObject(Obj, 0);
        }
        finally
        {
            S?.Dispose();
        }
    }

    // Engine.pas:8934
    public void ReadPreviewEObject(Stream Stream, EngineObject Obj, Engine.ProgressEvent AProgress)
    {
        EStorageStream S;
        S = new EReadStorageStream(Stream, this, true);
        S.OnProgress = AProgress;
        try
        {
            S.rwEObject(Obj, 0);
        }
        finally
        {
            S?.Dispose();
        }
    }

    // Engine.pas:8947
    public void StoreEObject(Stream Stream, EngineObject Obj, Engine.ProgressEvent AProgress)
    {
        EStorageStream S;
        S = new EWriteStorageStream(Stream, this, false);
        S.OnProgress = AProgress;
        try
        {
            S.rwCreateEObject(ref Obj, null, 0);
        }
        finally
        {
            S?.Dispose();
        }
    }

    // Engine.pas:8960
    public EngineObject RetrieveEObject(Stream Stream, Engine.ProgressEvent AProgress)
    {
        EngineObject Result = default;
        EStorageStream S;
        S = new EReadStorageStream(Stream, this, false);
        S.OnProgress = AProgress;
        try
        {
            S.rwCreateEObject(ref Result, null, 0);
        }
        finally
        {
            S?.Dispose();
        }

        return Result;
    }

    // Engine.pas:8973
    // The file goes through the game's file boundary with the caller's profile. A refused
    // preflight raises EGameFileError before an object is created (own-formats.md 4.1).
    public EngineObject RetrieveFromFile(string FileName, Engine.ProgressEvent AProgress, GameProfile Profile)
    {
        return RetrieveFromImage(FileBoundary.LoadOrRaise(FileName, Profile), AProgress);
    }

    // Retrieves the object of an engine image whose preflight has passed.
    public EngineObject RetrieveFromImage(byte[] Image, Engine.ProgressEvent AProgress)
    {
        EngineObject Result = default;
        MemoryStream M;
        M = ImageStream(Image);
        try
        {
            Result = RetrieveEObject(M, AProgress);
        }
        finally
        {
            M?.Dispose();
        }

        return Result;
    }

    // Engine.pas:8985
    // The engine writes the image in memory. The boundary converts, validates and replaces the
    // file only after the whole document is serialized.
    public void StoreInFile(string FileName, EngineObject Obj, Engine.ProgressEvent AProgress, GameProfile Profile)
    {
        MemoryStream M;
        M = new MemoryStream();
        try
        {
            StoreEObject(M, Obj, AProgress);
            FileBoundary.Save(FileName, Profile, M.GetBuffer().AsSpan(0, (int)M.Length).ToArray());
        }
        finally
        {
            M?.Dispose();
        }
    }

    // An engine image in a memory stream at position 0, for the routines that read it.
    public static MemoryStream ImageStream(byte[] Image)
    {
        MemoryStream M = new MemoryStream();
        M.WriteBuffer(Image);
        M.Position = 0;
        return M;
    }

    // The in-scope form of WriteToFile for the hero and item lists (own-formats.md 4.1). The
    // three-argument form below stays the original byte path of the excluded PFS callers.
    public void WriteToFile(string FileName, EngineObject Obj, Engine.ProgressEvent AProgress, GameProfile Profile)
    {
        MemoryStream M;
        M = new MemoryStream();
        try
        {
            WriteEObject(M, Obj, AProgress);
            FileBoundary.Save(FileName, Profile, M.GetBuffer().AsSpan(0, (int)M.Length).ToArray());
        }
        finally
        {
            M?.Dispose();
        }
    }

    // The in-scope form of ReadFromFile. A refused preflight raises before Obj is read.
    public void ReadFromFile(string FileName, EngineObject Obj, Engine.ProgressEvent AProgress, GameProfile Profile)
    {
        MemoryStream M;
        M = ImageStream(FileBoundary.LoadOrRaise(FileName, Profile));
        try
        {
            ReadEObject(M, Obj, AProgress);
        }
        finally
        {
            M?.Dispose();
        }
    }

    // Engine.pas:8997
    public void WriteToFile(string FileName, EngineObject Obj, Engine.ProgressEvent AProgress)
    {
        MemoryStream M;
        Stream F = default;
        M = new MemoryStream();
        try
        {
            WriteEObject(M, Obj, AProgress);
            F = OpenFile(FileName, 65535);
            try
            {
                M.Position = 0;
                F.CopyFrom(M, (int)M.Length);
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

    // Engine.pas:9017
    public void ReadFromFile(string FileName, EngineObject Obj, Engine.ProgressEvent AProgress)
    {
        MemoryStream M;
        Stream F;
        M = new MemoryStream();
        F = OpenFile(FileName, 0 | 32);
        try
        {
            M.LoadFromStream(F);
        }
        finally
        {
            F?.Dispose();
        }

        M.Position = 0;
        try
        {
            ReadEObject(M, Obj, AProgress);
        }
        finally
        {
            M?.Dispose();
        }
    }

    // Engine.pas:9037
    public void WriteToFileCRC(string FileName, EngineObject Obj, Engine.ProgressEvent AProgress)
    {
        int CRC1 = default;
        int CRC2 = default;
        MemoryStream M;
        Stream F = default;
        M = new MemoryStream();
        try
        {
            WriteEObject(M, Obj, AProgress);
            var size = (int)M.Length;
            CRC1 = EngineServices.GetCRC32(M.GetBuffer(), size);
            M.Write(Bytes.Of(ref CRC1, 4));
            var mSize = (int)M.Length;
            CRC2 = EngineServices.GetCRC32(M.GetBuffer(), mSize);
            F = OpenFile(FileName, 65535);
            try
            {
                F.Write(Bytes.Of(ref CRC2, 4));
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
    public bool ReadFromFileCRC(string FileName, EngineObject Obj, Engine.ProgressEvent AProgress)
    {
        MemoryStream M;
        Stream F;
        int CRC2;
        int CRC1;
        bool Result = false;
        M = new MemoryStream();
        F = OpenFile(FileName, 0 | 32);
        try
        {
            M.LoadFromStream(F);
            M.Position = 0;
            if (M.Length > 8)
            {
                CRC1 = MemoryMarshal.Read<int>(M.GetBuffer().AsSpan(0, 4));
                var size = (int)M.Length;
                CRC2 = MemoryMarshal.Read<int>(M.GetBuffer().AsSpan(size - 4, 4));
                var mSize = (int)M.Length;
                bool condition = CRC1 == EngineServices.GetCRC32(M.GetBuffer().AsSpan(4), mSize - 4);
                if (condition)
                {
                    var size2 = (int)M.Length;
                    condition = CRC2 == EngineServices.GetCRC32(M.GetBuffer().AsSpan(4), size2 - 8);
                }

                if (condition)
                {
                    M.Position = 4;
                    ReadEObject(M, Obj, AProgress);
                    Result = true;
                }
            }
        }
        finally
        {
            F?.Dispose();
            M?.Dispose();
        }

        return Result;
    }

    // Engine.pas:9091
    public void ReadTextFromFile(string FileName, EngineObject Obj)
    {
        EStorageStream S;
        Stream F;
        F = OpenFile(FileName, 0 | 32);
        try
        {
            S = new EReadTextStorageStream(F, this, 0);
            try
            {
                S.rwEObject(Obj, 0);
            }
            finally
            {
                S?.Dispose();
            }
        }
        finally
        {
            F?.Dispose();
        }
    }

    // Engine.pas:9109
    public void WriteTextToFile(string FileName, EngineObject Obj)
    {
        EStorageStream S;
        Stream F;
        F = OpenFile(FileName, 65535);
        try
        {
            S = new EWriteTextStorageStream(F, this);
            try
            {
                S.rwEObject(Obj, 0);
            }
            finally
            {
                S?.Dispose();
            }
        }
        finally
        {
            F?.Dispose();
        }
    }

    public List<EngineClass> EClasses { get => FEClasses; }
    public Dictionary<int, EngineClass> ClassTable { get => FClassTable; }
    public EngineObjectEditorList Editors { get => FEditors; }
    public EControlStyleNode ControlNode { get => FControlNode; }
    public StringList DebugIDs { get => FDebugIDs; }

    public event NotifyEvent<EngineDebugNotification> DebugEvents
    {
        add => FDebugEventsStore.Add(value);
        remove => FDebugEventsStore.Remove(value);
    }

    public string StartupDirectory { get => FStartupDirectory; set => SetStartupDirectory(value); }
    public Set<EngineModeElement> Options { get => FOptions; set => FOptions = value; }

    // Engine.pas:1126
    public bool VerifyEObjectClass(EngineClass type)
    {
        // The native allocator byte total has no corresponding managed heap measure.
        // Exercise the same constructor and destructor without comparing GC generations.
        type.Create(null)?.Dispose();
        return true;
    }

    internal const EngineModeElement eoVerifyClasses = 0;

    private static void InitMsgEx(ref EngineMessage Msg, int MsgID, EngineObject Sender, int Seq)
    {
        Msg = default;
        Msg.MsgID = MsgID;
        Msg.Seq = Seq;
        Msg.Sender = Sender;
        Msg.Result = -1;
    }

    private static readonly string UserStr = "User";

    internal static void UnregisterSoundEngineClasses(GameEngine AEngine)
    {
        AEngine.UnregisterEClasses([EngineClass.Of<SfxSequence>(), EngineClass.Of<SfxLibrary>(), EngineClass.Of<Sfx>()]);
    }

    internal static void UnRegisterStorms(GameEngine AEngine)
    {
        SpellControl Spells;
        Spells = AEngine.FGameSet.FSpellControl;
        Spells.UnregisterSpellID(3);
        Spells.UnregisterSpellID(5);
        Spells.UnregisterSpellID(6);
        Spells.UnregisterSpellID(7);
        Spells.UnregisterSpellID(4);
        Spells.UnregisterSpellID(9);
    }

    internal static void InitializePlugIns(GameEngine AEngine)
    {
        for (int I = 0, last = EngineHost.PlugIns.Count - 1; I <= last; I++)
        {
            (EngineHost.PlugIns[I]).Initialize(AEngine);
        }
    }

    internal static void FinalizePlugIns(GameEngine AEngine)
    {
        for (int I = 0, last = EngineHost.PlugIns.Count - 1; I <= last; I++)
        {
            (EngineHost.PlugIns[I]).Finalize(AEngine);
        }
    }

    internal static GameEngine CopyEngine;

    internal static void UnRegisterMountain(GameEngine AEngine)
    {
        AEngine.UnregisterEClasses([EngineClass.Of<MountainMapObject>(), EngineClass.Of<MountainMapObjectResource>(), EngineClass.Of<RaiseTerrainTokenEvent>(), EngineClass.Of<RaisedMountain>(), EngineClass.Of<RaisedMountain.Resource>()]);
        // { Original unregisters spell 12, although Raise Terrain registers as spell 19. }
        AEngine.FGameSet.FSpellControl.UnregisterSpellID(12);
    }

    internal static void RegisterBorderHexagon(GameEngine Engine)
    {
        Engine.RegisterEClasses([EngineClass.Of<BorderHexagon>(), EngineClass.Of<BorderHexagon.Resource>()]);
    }

    internal static void RegisterHexagon(GameEngine Engine)
    {
        Engine.RegisterEClasses([EngineClass.Of<Hexagon>(), EngineClass.Of<HexagonResource>()]);
    }

    internal static void RegisterIsometricHexagon(GameEngine Engine)
    {
        Engine.RegisterEClasses([EngineClass.Of<IsometricHex>(), EngineClass.Of<IsometricHexResource>(), EngineClass.Of<UpperIsometricHex>(), EngineClass.Of<UpperIsometricHex.Resource>(), EngineClass.Of<LowerIsometricHex>(), EngineClass.Of<LowerIsometricHex.Resource>()]);
    }

    internal static void RegisterRoad(GameEngine Engine)
    {
        Engine.RegisterEClasses([EngineClass.Of<Road>(), EngineClass.Of<Road.Resource>()]);
    }

    internal static void RegisterTerrain(GameEngine Engine)
    {
        Engine.RegisterEClasses([EngineClass.Of<TerrainMapObject>(), EngineClass.Of<TerrainMapObjectResource>()]);
    }

    internal static void RegisterHill(GameEngine AEngine)
    {
    }

    internal static void RegisterAltar(GameEngine AEngine)
    {
        AEngine.RegisterEClasses([EngineClass.Of<Altar>(), EngineClass.Of<AltarResource>()]);
    }

    internal static void RegisterRandomNode(GameEngine AEngine)
    {
    }

    internal static void UnRegisterHolyGround(GameEngine Engine)
    {
        Engine.UnregisterEClasses([EngineClass.Of<HolyGround>(), EngineClass.Of<HolyGround.Resource>(), EngineClass.Of<HolyGroundSpellTokenEvent>()]);
        Engine.FGameSet.FSpellControl.UnregisterSpellID(13);
    }

    internal static void UnRegisterOverlayObject(GameEngine AEngine)
    {
        AEngine.UnregisterEClasses([EngineClass.Of<OverlayObject>(), EngineClass.Of<OverlayObject.Resource>()]);
    }

}
