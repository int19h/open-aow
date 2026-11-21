#nullable disable warnings
// Translated from HSEngine.pas and AoWE.pas.
using System.Runtime.InteropServices;
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Tree;
using Aow.Game.Audio;
using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiStreams;

namespace Aow.Game.World.Map;

// _status, _hexControl, _roadControl, _activatedEngine, _mapObjectResourceList,
// _resourceEditSettings, _imageLibraryManager, _sfxManager, _pluginControl, _name,
// FTerrainTypes, FOverlayTypes, FDecorationTypes. The hexagon sprite set: the resources a map
// uses (an.hss file, THSEngine.LoadHSS). ReadWrite stores the name (id 4), the map object
// resources (5), the resource edit settings (6) and the plug-ins (7). the type name lists are
// filled by the game (TAoWHSSet.Create) and not stored. The game adds the shared resources and
// controls of the game pack: the combat resources, the unit, hero, item and face resources, the
// abilities, spells and texts read from the PFS files beside the set.
public class GamePack : EngineNode, IChildList<GamePackChild>
{
    // Ability.PAS (TAoWHSSet). HSEngine.pas:3394 gives the engine set 66049.
    public new const int ClassId = 131_088;

    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only the kinds of HSSetChild, so the read never fails.
    public GamePackChild this[int index] => GamePackChild.Of(GetChild(index));

    protected sealed override bool AcceptsChild(EngineObject child) => child is ResourceCollection or MapObjectResourceTerrainControl or ControlStyleObject;

    int IChildSink<GamePackChild>.AddChild(GamePackChild child) => AddChild(child.Node);

    public GamePack(CustomNode? owner, EngineObject holder) : this(null, owner, holder) { }

    private protected GamePack(ConstructionContext? context, CustomNode? owner, EngineObject holder) : base(context, owner)
    {
        Holder = holder;
        SetControlStyle(GetControlStyle() | EngineServices.Reserved2);
        PluginControl = new PluginControl(null) { Holder = CreationHolder() };
        TerrainTypes = [];
        OverlayTypes = [];
        DecorationTypes = [];
        HexControl = new HexControl(this)
        {
            MapObjectClass = typeof(AbstractHexagon)
        };
        RoadControl = new RoadControl(this)
        {
            MapObjectClass = typeof(AbstractRoad)
        };
        MapObjectResourceList = new MapObjectResourceList(this);
        MapObjectResourceList.SetControlStyle(EngineServices.Stored | EngineServices.Shared);
        ResourceEditSettings = new ResourceEditSettings(this);
        ResourceEditSettings.SetControlStyle(EngineServices.Stored | EngineServices.Shared);
        ImageLibraryManager = new ImageLibraryManager(null);
        SfxManager = new SfxManager(null);

        CombatResources = new MapObjectResourceList(this);
        CombatResources.SetControlStyle(ControlStyles.SaveLoad | ControlStyles.FixedChild);
        CombatHexes = new HexControl(this)
        {
            MapObjectClass = typeof(AbstractHexagon)
        };
        CombatRoads = new RoadControl(this)
        {
            MapObjectClass = typeof(AbstractRoad)
        };
        ImageLibraryManager.ExecutePathSearch($"{GetEngine().StartupDirectory}IMAGES");
        SfxManager.ExecutePathSearch($"{GetEngine().StartupDirectory}SFX");
        ImageSequences = new ImageSequenceList(ImageLibraryManager, imageLibrary: null);
        string water = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.WaterText);
        _ = TerrainTypes.Add(water);
        string translatedText = AbilityRules.TranslateResourceString(GetEngine(), S: s_grassText);
        _ = TerrainTypes.Add(translatedText);
        string desert = AbilityRules.TranslateResourceString(GetEngine(), S: s_desertText);
        _ = TerrainTypes.Add(desert);
        string snow = AbilityRules.TranslateResourceString(GetEngine(), S: s_snowText);
        _ = TerrainTypes.Add(snow);
        string steppe = AbilityRules.TranslateResourceString(GetEngine(), S: s_steppeText);
        _ = TerrainTypes.Add(steppe);
        string wasteland = AbilityRules.TranslateResourceString(GetEngine(), S: s_wastelandText);
        _ = TerrainTypes.Add(wasteland);
        string ice = AbilityRules.TranslateResourceString(GetEngine(), S: s_iceText);
        _ = TerrainTypes.Add(ice);
        string earth = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.EarthText);
        _ = TerrainTypes.Add(earth);
        string rock = AbilityRules.TranslateResourceString(GetEngine(), S: s_rockText);
        _ = TerrainTypes.Add(rock);
        string lava = AbilityRules.TranslateResourceString(GetEngine(), S: s_lavaText);
        _ = TerrainTypes.Add(lava);
        string waterText = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.WaterText);
        _ = TerrainTypes.Add(waterText);
        string reserved = AbilityRules.TranslateResourceString(GetEngine(), S: s_reservedText);
        _ = TerrainTypes.Add(reserved);
        string dirt = AbilityRules.TranslateResourceString(GetEngine(), S: s_dirtText);
        _ = TerrainTypes.Add(dirt);
        string iceText = AbilityRules.TranslateResourceString(GetEngine(), S: s_iceText);
        _ = TerrainTypes.Add(iceText);
        string coast = AbilityRules.TranslateResourceString(GetEngine(), S: s_coastText);
        _ = TerrainTypes.Add(coast);
        string border = AbilityRules.TranslateResourceString(GetEngine(), S: s_borderText);
        _ = TerrainTypes.Add(border);
        string mountain = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.MountainText);
        _ = OverlayTypes.Add(mountain);
        string denseVegetation = AbilityRules.TranslateResourceString(GetEngine(), S: s_denseVegetationText);
        _ = OverlayTypes.Add(denseVegetation);
        string hill = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.HillText);
        _ = OverlayTypes.Add(hill);
        string vegetation = AbilityRules.TranslateResourceString(GetEngine(), S: s_vegetationText);
        _ = OverlayTypes.Add(vegetation);
        string road = AbilityRules.TranslateResourceString(GetEngine(), S: s_roadText);
        _ = OverlayTypes.Add(road);
        string bridge = AbilityRules.TranslateResourceString(GetEngine(), S: s_bridgeText);
        _ = OverlayTypes.Add(bridge);
        string rubble = AbilityRules.TranslateResourceString(GetEngine(), S: s_rubbleText);
        _ = OverlayTypes.Add(rubble);
        string structure = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.StructureText);
        _ = OverlayTypes.Add(structure);
        string obstacle = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.ObstacleText);
        _ = OverlayTypes.Add(obstacle);
        string lightVegetation = AbilityRules.TranslateResourceString(GetEngine(), S: s_lightVegetationText);
        _ = OverlayTypes.Add(lightVegetation);
        string solid = AbilityRules.TranslateResourceString(GetEngine(), S: s_solidText);
        _ = OverlayTypes.Add(solid);
        string none = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.NoneText);
        _ = DecorationTypes.Add(none);
        string rocks = AbilityRules.TranslateResourceString(GetEngine(), S: s_rocksText);
        _ = DecorationTypes.Add(rocks);
        string bones = AbilityRules.TranslateResourceString(GetEngine(), S: s_bonesText);
        _ = DecorationTypes.Add(bones);
        string deadTree = AbilityRules.TranslateResourceString(GetEngine(), S: s_deadTreeText);
        _ = DecorationTypes.Add(deadTree);
        string toadstools = AbilityRules.TranslateResourceString(GetEngine(), S: s_toadstoolsText);
        _ = DecorationTypes.Add(toadstools);
        PadControl = new PadControl(this);
        BitList enabled = PadControl.TerrainTypes;
        enabled[1] = true;
        enabled[2] = true;
        enabled[3] = true;
        enabled[4] = true;
        enabled[5] = true;
        enabled[6] = true;
        enabled[12] = true;
        RaceResources = new RaceResourceList(this);
        FlagControl = FlagControl.Create(this);
        FireControl = new FireControl();
        UnitResources = new UnitResourceList(this);
        UnitGraphicsResources = new UnitGFXResourceList(this);
        HeroResources = new HeroResourceList(this);
        ItemGraphicsResources = new ItemGFXList(this);
        FaceResources = new FaceResourceList(this);
        ItemLibraries = new ItemLibraryManager(null) { Holder = CreationHolder() };
        HeroLibraries = new HeroLibraryManager(null) { Holder = CreationHolder() };
        AbilityControl = new AbilityControl(null) { Holder = CreationHolder() };
        SpellControl = new SpellControl(null) { Holder = CreationHolder() };
        TextControl = new TextControl(null) { Holder = CreationHolder() };
        GeneralImages = ImageLibraryManager.LinkToImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "GENERAL.ILB");
        CursorImages = ImageLibraryManager.LinkToImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "CURSORS.ILB");
        CombatImages = ImageLibraryManager.LinkToImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "COMBAT.ILB");
    }

    public GameEngine? ActivatedEngine { get; private set; }

    // Ability.PAS:74 around HSEngine.pas:3368.
    public override void Dispose()
    {
        FireControl?.Dispose();
        ImageSequences?.Dispose();
        TextControl?.Dispose();
        AbilityControl.UnregisterAll();
        SpellControl.UnregisterAll();
        ItemLibraries?.Dispose();
        HeroLibraries?.Dispose();
        Status |= HexSpriteSetStatuses.Destroying;
        EngineMessage.InitializeMessage(out EngineMessage message, messageId: 66564, sender: this);
        if (ActivatedEngine is not null && ActivatedEngine.ControlNode is not null)
        {
            ActivatedEngine.ControlNode.MessageProc(ref message, sender: this);
        }

        base.Dispose();
        PluginControl?.Dispose();
        TerrainTypes?.Dispose();
        TerrainTypes = null;
        OverlayTypes?.Dispose();
        OverlayTypes = null;
        DecorationTypes?.Dispose();
        DecorationTypes = null;
        ImageLibraryManager?.Dispose();
        ImageLibraryManager = null;
        SfxManager?.Dispose();
        SfxManager = null;
        EngineMessage.InitializeMessage(out message, messageId: 66565, sender: this);
        if (ActivatedEngine is not null && ActivatedEngine.ControlNode is not null)
        {
            ActivatedEngine.ControlNode.MessageProc(ref message, sender: this);
        }

        Status &= ~HexSpriteSetStatuses.Destroying;

        AbilityControl?.Dispose();
        SpellControl?.Dispose();
    }

    // Ability.PAS:131 around HSEngine.pas:3410. A load shows its progress to the load's handler at
    // the points where the original calls Stream.ShowProgress. The game set files beside the scenario
    // set are read and written as the original does. Its text-only branch, which stores them inline,
    // runs only under the translate stream, which visits them in place.
    public override void PersistFields(IPersister persister)
    {
        if (persister.Loading)
        {
            persister.ShowProgress(sender: this, message: AbilityRules.TranslateResourceString(GetEngine(), S: s_loadingGamePackText), percent: 0);
        }

        GameEngine engine = persister.Engine();
        SetEngine(engine);
        PluginControl.Engine = engine;
        base.PersistFields(persister);
        Name = persister.String("Name", value: Name);
        persister.Object("MapObjectResourceList", objectValue: MapObjectResourceList);
        persister.Object("ResourceEditSettings", objectValue: ResourceEditSettings);
        persister.Object("PluginControl", objectValue: PluginControl);
        persister.Object("RaceResources", objectValue: RaceResources);
        persister.Object("ItemLibraries", objectValue: ItemLibraries);
        string path = ExtractFilePath(GetEngine().HssFileName);
        if (persister.Translating())
        {
            // The translate stream's mode holds text only, under which the original visits the game
            // set inline (ids 111 to 119) and reads and writes no file.
            persister.Object("UnitResources", objectValue: UnitResources);
            persister.Object("UnitGraphicsResources", objectValue: UnitGraphicsResources);
            persister.Object("HeroResources", objectValue: HeroResources);
            persister.Object("ItemGraphicsResources", objectValue: ItemGraphicsResources);
            persister.Object("FaceResources", objectValue: FaceResources);
            persister.Object("SpellControl", objectValue: SpellControl);
            persister.Object("AbilityControl", objectValue: AbilityControl);
            persister.Object("TextControl", objectValue: TextControl);
        }
        else if (persister.Loading)
        {
            bool loaded = GetEngine().ReadFromFileCrc($"{path}ABILITY.PFS", objectValue: AbilityControl, progress: null);
            persister.ShowProgress(sender: this, message: AbilityRules.TranslateResourceString(GetEngine(), S: s_loadingUnitsText), percent: 70);
            loaded = loaded && GetEngine().ReadFromFileCrc($"{path}UNITRES.PFS", objectValue: UnitResources, progress: null);
            loaded = loaded && GetEngine().ReadFromFileCrc($"{path}UNITGFX.PFS", objectValue: UnitGraphicsResources, progress: null);
            persister.ShowProgress(sender: this, message: AbilityRules.TranslateResourceString(GetEngine(), S: s_loadingHeroesText), percent: 80);
            loaded = loaded && GetEngine().ReadFromFileCrc($"{path}HERORES.PFS", objectValue: HeroResources, progress: null);
            persister.ShowProgress(sender: this, message: AbilityRules.TranslateResourceString(GetEngine(), S: s_loadingItemsText), percent: 90);
            loaded = loaded && GetEngine().ReadFromFileCrc($"{path}ITEMGFX.PFS", objectValue: ItemGraphicsResources, progress: null);
            loaded = loaded && GetEngine().ReadFromFileCrc($"{path}FACERES.PFS", objectValue: FaceResources, progress: null);
            loaded = loaded && GetEngine().ReadFromFileCrc($"{path}COMB_RES.PFS", objectValue: CombatResources, progress: null, classTable: "tactical");
            loaded = loaded && GetEngine().ReadFromFileCrc($"{path}SPELLS.PFS", objectValue: SpellControl, progress: null);
            loaded = loaded && GetEngine().ReadFromFileCrc($"{path}TEXT.PFS", objectValue: TextControl, progress: null);
            if (!loaded)
            {
                throw new InvalidOperationException("CRC-CHECK FAILED");
            }

            persister.ShowProgress(sender: this, message: AbilityRules.TranslateResourceString(GetEngine(), S: s_gamePackLoadedText), percent: 100);
        }
        else
        {
            GetEngine().WriteToFileCrc($"{path}ABILITY.PFS", objectValue: AbilityControl, progress: null);
            GetEngine().WriteToFileCrc($"{path}UNITRES.PFS", objectValue: UnitResources, progress: null);
            GetEngine().WriteToFileCrc($"{path}UNITGFX.PFS", objectValue: UnitGraphicsResources, progress: null);
            GetEngine().WriteToFileCrc($"{path}HERORES.PFS", objectValue: HeroResources, progress: null);
            GetEngine().WriteToFileCrc($"{path}ITEMGFX.PFS", objectValue: ItemGraphicsResources, progress: null);
            GetEngine().WriteToFileCrc($"{path}FACERES.PFS", objectValue: FaceResources, progress: null);
            GetEngine().WriteToFileCrc($"{path}COMB_RES.PFS", objectValue: CombatResources, progress: null, classTable: "tactical");
            GetEngine().WriteToFileCrc($"{path}SPELLS.PFS", objectValue: SpellControl, progress: null);
            GetEngine().WriteToFileCrc($"{path}TEXT.PFS", objectValue: TextControl, progress: null);
        }

        persister.Object("HeroLibraries", objectValue: HeroLibraries);
        persister.ImageSequences(name: "ImageSequences", list: ImageSequences);
        // The translate stream's mode holds read, so the original links and takes the CRC there too.
        if (persister.Loading || persister.Translating())
        {
            ImageSequences.LinkToImageLibrary();
            HssCrc = RetrieveHssCrc();
        }
    }

    // HSEngine.pas:3405
    public override void Clear() => base.Clear();

    // HSEngine.pas:3399, then Ability.PAS:206.
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            ActivatedEngine = message.Payload.As<GameEngine>();
        }

        ItemLibraries.MessageProc(ref message, sender);
    }

    // HSEngine.pas:3421
    public void Activate(bool edit)
    {
        EngineMessage.InitializeMessage(out EngineMessage message, messageId: 66560, sender: this);
        GetEngine().ControlNode.MessageProc(ref message, sender: this);
        Status |= HexSpriteSetStatuses.Activated;
        if (edit)
        {
            EngineMessage.InitializeMessage(out message, messageId: 66562, sender: this);
            GetEngine().ControlNode.MessageProc(ref message, sender: this);
            Status |= HexSpriteSetStatuses.Editting;
        }
    }

    // HSEngine.pas:3436
    public void DeActivate()
    {
        if (Status.Contains(HexSpriteSetStatuses.Activated))
        {
            EngineMessage.InitializeMessage(out EngineMessage message, messageId: 66561, sender: this);
            GetEngine().ControlNode.MessageProc(ref message, sender: this);
            Status &= ~HexSpriteSetStatuses.Activated;
            if (Status.Contains(HexSpriteSetStatuses.Editting))
            {
                EngineMessage.InitializeMessage(out message, messageId: 66563, sender: this);
                GetEngine().ControlNode.MessageProc(ref message, sender: this);
                Status &= ~HexSpriteSetStatuses.Editting;
            }
        }
    }

    // HSEngine.pas:3454
    public void SetModified()
    {
        if (Status.Contains(HexSpriteSetStatuses.Editting))
        {
            Status |= HexSpriteSetStatuses.Modified;
        }
    }

    // HSEngine.pas:3459
    public void ResetModified() => Status &= ~HexSpriteSetStatuses.Modified;

    // HSEngine.pas:3464
    public void SetEditting() => Status |= HexSpriteSetStatuses.Editting;

    // HSEngine.pas:3468
    public void ResetEditting() => Status &= ~HexSpriteSetStatuses.Editting;

    public HexSpriteSetStatuses Status { get; private set; }
    public HexControl HexControl { get; }
    public RoadControl RoadControl { get; }
    public MapObjectResourceList MapObjectResourceList { get; }
    public ResourceEditSettings ResourceEditSettings { get; }
    public ImageLibraryManager? ImageLibraryManager { get; private set; }
    public SfxManager? SfxManager { get; private set; }
    public PluginControl PluginControl { get; }
    public string Name { get; private set; } = "";
    public DelphiStringList? TerrainTypes { get; private set; }
    public DelphiStringList? OverlayTypes { get; private set; }
    public DelphiStringList? DecorationTypes { get; private set; }

    public CustomImageLibrary? GeneralImages { get; }
    public CustomImageLibrary? CursorImages { get; }
    public CustomImageLibrary? CombatImages { get; }
    public RaceResourceList RaceResources { get; }
    public PadControl PadControl { get; }
    public UnitResourceList UnitResources { get; internal set; }
    public UnitGFXResourceList? UnitGraphicsResources { get; internal set; }
    public HeroResourceList HeroResources { get; internal set; }
    public ItemGFXList ItemGraphicsResources { get; }
    public ItemLibraryManager ItemLibraries { get; }
    public HeroLibraryManager HeroLibraries { get; }
    public FaceResourceList FaceResources { get; }
    public FlagControl FlagControl { get; }
    public FireControl FireControl { get; }
    public AbilityControl AbilityControl { get; internal set; }
    public SpellControl SpellControl { get; }
    public MapObjectResourceList CombatResources { get; internal set; }
    public HexControl CombatHexes { get; }
    public RoadControl CombatRoads { get; }
    public ImageSequenceList ImageSequences { get; }
    public TextControl TextControl { get; internal set; }
    public int HssCrc { get; private set; }

    // Ability.PAS:194
    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (persister.Loading)
        {
            string dictionaryFile = $"{GetEngine().StartupDirectory}Dict\\Release.mld";
            GetEngine().TranslateEngineObjectFromFile(this, fileName: dictionaryFile);
        }
    }

    // Ability.PAS:91
    public int GetFileCrc(string fileName, bool atEnd)
    {
        int result = 0;
        Stream fileStream = OpenFile(fileName, mode: FileOpenMode.Read | FileOpenMode.ShareDenyWrite);
        try
        {
            if (atEnd)
            {
                fileStream.Position = fileStream.Length - 4;
                fileStream.ReadExactly(Bytes.Of(ref result));
            }
            else
            {
                fileStream.ReadExactly(Bytes.Of(ref result));
            }
        }
        finally
        {
            fileStream?.Dispose();
        }

        return result;
    }

    // Ability.PAS:109
    public int RetrieveHssCrc()
    {
        Int32List crCs = [];
        string path = ExtractFilePath(GetEngine().HssFileName);
        // { Original includes ABILITY.PFS twice and does not free CRCs on a read error. }
        _ = crCs.Add(GetFileCrc($"{path}ABILITY.PFS", atEnd: false));
        _ = crCs.Add(GetFileCrc($"{path}ABILITY.PFS", atEnd: false));
        _ = crCs.Add(GetFileCrc($"{path}UNITRES.PFS", atEnd: false));
        _ = crCs.Add(GetFileCrc($"{path}UNITGFX.PFS", atEnd: false));
        _ = crCs.Add(GetFileCrc($"{path}HERORES.PFS", atEnd: false));
        _ = crCs.Add(GetFileCrc($"{path}ITEMGFX.PFS", atEnd: false));
        _ = crCs.Add(GetFileCrc($"{path}FACERES.PFS", atEnd: false));
        _ = crCs.Add(GetFileCrc($"{path}COMB_RES.PFS", atEnd: false));
        _ = crCs.Add(GetFileCrc($"{path}SPELLS.PFS", atEnd: false));
        // The scenario set's stored CRC is the tail of its engine image, so an own document agrees
        // with its original (own-formats.md 4.1). A document with no image has no stored CRC and
        // the call refuses through the packet profile. The PFS files keep their original bytes.
        _ = crCs.Add(FileBoundary.ImageTailInt32(FileBoundary.ReadName($"{path}RELEASE.HSS"), profile: GameProfile.ScenarioSet));
        int result = EngineServices.GetCrc32(MemoryMarshal.AsBytes(crCs.List.AsSpan()), size: crCs.Count * 4);
        crCs?.Dispose();
        return result;
    }

    // intf_P1_10.inc:44
    public static GamePack Create(CustomNode? owner, EngineObject holder)
        => ConstructionContext.Create((owner, holder), static (context, value) => new GamePack(context, value.owner, value.holder));

    private static readonly string s_grassText = "Grass";

    private static readonly string s_desertText = "Desert";

    private static readonly string s_snowText = "Snow";

    private static readonly string s_steppeText = "Steppe";

    private static readonly string s_wastelandText = "Wasteland";

    private static readonly string s_iceText = "Ice";

    private static readonly string s_rockText = "Rock";

    private static readonly string s_lavaText = "Lava";

    private static readonly string s_reservedText = "Reserved";

    private static readonly string s_dirtText = "Dirt";

    private static readonly string s_coastText = "Coast";

    private static readonly string s_borderText = "Border";

    private static readonly string s_denseVegetationText = "Dense Vegetation";

    private static readonly string s_vegetationText = "Vegetation";

    private static readonly string s_roadText = "Road";

    private static readonly string s_bridgeText = "Bridge";

    private static readonly string s_rubbleText = "Rubble";

    private static readonly string s_lightVegetationText = "Light Vegetation";

    private static readonly string s_solidText = "Solid";

    private static readonly string s_rocksText = "Rocks";

    private static readonly string s_bonesText = "Bones";

    private static readonly string s_deadTreeText = "Dead Tree";

    private static readonly string s_toadstoolsText = "Toadstools";

    private static readonly string s_loadingGamePackText = "Loading Game Pack";

    private static readonly string s_loadingUnitsText = "Loading Units";

    private static readonly string s_loadingHeroesText = "Loading Heroes";

    private static readonly string s_loadingItemsText = "Loading Items";

    private static readonly string s_gamePackLoadedText = "Game Pack Loaded";

    public static GamePack? Current { get; set; }
}
