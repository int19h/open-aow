// Translated from HSEngine.pas and AoWE.pas.
using System.Runtime.InteropServices;
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;
using Aow.Game.Audio;
using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

// _status, _hexControl, _roadControl, _activatedEngine, _mapObjectResourceList,
// _resourceEditSettings, _imageLibraryManager, _sfxManager, _pluginControl, _name,
// FTerrainTypes, FOverlayTypes, FDecorationTypes. The hexagon sprite set: the resources a map
// uses (an.hss file, THSEngine.LoadHSS). ReadWrite stores the name (id 4), the map object
// resources (5), the resource edit settings (6) and the plug-ins (7). the type name lists are
// filled by the game (TAoWHSSet.Create) and not stored. The game adds the shared resources and
// controls of the game pack: the combat resources, the unit, hero, item and face resources, the
// abilities, spells and texts read from the PFS files beside the set.
public partial class GamePack : ECustomOwner, IChildList<GamePackChild>
{
    // Ability.PAS (TAoWHSSet). HSEngine.pas:3394 gives the engine set 66049.
    public new const int ClassId = 131088;

    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only the kinds of HSSetChild, so the read never fails.
    public GamePackChild this[int index] => GamePackChild.Of(GetChild(index));

    protected sealed override bool AcceptsChild(EngineObject child) => child is EResourceList or MapObjectResourceTerrainControl or ControlStyleObject;

    int IChildSink<GamePackChild>.AddChild(GamePackChild child) => AddChild(child.Node);

    public GamePack(ECustomNode owner) : this(null, owner) { }

    private protected GamePack(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
        SetControlStyle(GetControlStyle() + Set<ControlStyleElement>.Of(EngineServices.Reserved2));
        _pluginControl = new PluginControl(null);
        _terrainTypes = new StringList();
        _overlayTypes = new StringList();
        _decorationTypes = new StringList();
        _hexControl = new HexControl(this);
        _hexControl.MapObjectClass = typeof(AbstractHexagon);
        _roadControl = new RoadControl(this);
        _roadControl.MapObjectClass = typeof(AbstractRoad);
        _mapObjectResourceList = new MapObjectResourceList(this);
        _mapObjectResourceList.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Stored, EngineServices.Shared));
        _resourceEditSettings = new ResourceEditSettings(this);
        _resourceEditSettings.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Stored, EngineServices.Shared));
        _imageLibraryManager = new ImageLibraryManager(null);
        _sfxManager = new SfxManager(null);

        BitList enabled;
        CombatResources = new MapObjectResourceList(this);
        CombatResources.SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild));
        CombatHexes = new HexControl(this);
        CombatHexes.MapObjectClass = typeof(AbstractHexagon);
        CombatRoads = new RoadControl(this);
        CombatRoads.MapObjectClass = typeof(AbstractRoad);
        ImageLibraryManager.ExecutePathSearch(GameTables.GameEngine.StartupDirectory + "IMAGES");
        SfxManager.ExecutePathSearch(GameTables.GameEngine.StartupDirectory + "SFX");
        ImageSequences = new ImageSequenceList(ImageLibraryManager, null);
        var water = AbilityRules.TranslateResourceString(GameTables.WaterText);
        TerrainTypes.Add(water);
        var translatedText = AbilityRules.TranslateResourceString(s_grassText);
        TerrainTypes.Add(translatedText);
        var desert = AbilityRules.TranslateResourceString(s_desertText);
        TerrainTypes.Add(desert);
        var snow = AbilityRules.TranslateResourceString(s_snowText);
        TerrainTypes.Add(snow);
        var steppe = AbilityRules.TranslateResourceString(s_steppeText);
        TerrainTypes.Add(steppe);
        var wasteland = AbilityRules.TranslateResourceString(s_wastelandText);
        TerrainTypes.Add(wasteland);
        var ice = AbilityRules.TranslateResourceString(s_iceText);
        TerrainTypes.Add(ice);
        var earth = AbilityRules.TranslateResourceString(GameTables.EarthText);
        TerrainTypes.Add(earth);
        var rock = AbilityRules.TranslateResourceString(s_rockText);
        TerrainTypes.Add(rock);
        var lava = AbilityRules.TranslateResourceString(s_lavaText);
        TerrainTypes.Add(lava);
        var waterText = AbilityRules.TranslateResourceString(GameTables.WaterText);
        TerrainTypes.Add(waterText);
        var reserved = AbilityRules.TranslateResourceString(s_reservedText);
        TerrainTypes.Add(reserved);
        var dirt = AbilityRules.TranslateResourceString(s_dirtText);
        TerrainTypes.Add(dirt);
        var iceText = AbilityRules.TranslateResourceString(s_iceText);
        TerrainTypes.Add(iceText);
        var coast = AbilityRules.TranslateResourceString(s_coastText);
        TerrainTypes.Add(coast);
        var border = AbilityRules.TranslateResourceString(s_borderText);
        TerrainTypes.Add(border);
        var mountain = AbilityRules.TranslateResourceString(GameText.MountainText);
        OverlayTypes.Add(mountain);
        var denseVegetation = AbilityRules.TranslateResourceString(s_denseVegetationText);
        OverlayTypes.Add(denseVegetation);
        var hill = AbilityRules.TranslateResourceString(GameText.HillText);
        OverlayTypes.Add(hill);
        var vegetation = AbilityRules.TranslateResourceString(s_vegetationText);
        OverlayTypes.Add(vegetation);
        var road = AbilityRules.TranslateResourceString(s_roadText);
        OverlayTypes.Add(road);
        var bridge = AbilityRules.TranslateResourceString(s_bridgeText);
        OverlayTypes.Add(bridge);
        var rubble = AbilityRules.TranslateResourceString(s_rubbleText);
        OverlayTypes.Add(rubble);
        var structure = AbilityRules.TranslateResourceString(GameText.StructureText);
        OverlayTypes.Add(structure);
        var obstacle = AbilityRules.TranslateResourceString(GameText.ObstacleText);
        OverlayTypes.Add(obstacle);
        var lightVegetation = AbilityRules.TranslateResourceString(s_lightVegetationText);
        OverlayTypes.Add(lightVegetation);
        var solid = AbilityRules.TranslateResourceString(s_solidText);
        OverlayTypes.Add(solid);
        var none = AbilityRules.TranslateResourceString(GameTables.NoneText);
        DecorationTypes.Add(none);
        var rocks = AbilityRules.TranslateResourceString(s_rocksText);
        DecorationTypes.Add(rocks);
        var bones = AbilityRules.TranslateResourceString(s_bonesText);
        DecorationTypes.Add(bones);
        var deadTree = AbilityRules.TranslateResourceString(s_deadTreeText);
        DecorationTypes.Add(deadTree);
        var toadstools = AbilityRules.TranslateResourceString(s_toadstoolsText);
        DecorationTypes.Add(toadstools);
        PadControl = new PadControl(this);
        enabled = PadControl.TerrainTypes;
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
        ItemLibraries = new ItemLibraryManager(null);
        HeroLibraries = new HeroLibraryManager(null);
        AbilityControl = new AbilityControl(null);
        SpellControl = new SpellControl(null);
        TextControl = new TextControl(null);
        GeneralImages = ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "GENERAL.ILB");
        CursorImages = ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "CURSORS.ILB");
        CombatImages = ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "COMBAT.ILB");
    }

    private Set<HexSpriteSetStatusElement> _status;
    private readonly HexControl _hexControl;
    private readonly RoadControl _roadControl;
    private GameEngine _activatedEngine;

    public GameEngine ActivatedEngine { get => _activatedEngine; }

    private readonly MapObjectResourceList _mapObjectResourceList;
    private readonly ResourceEditSettings _resourceEditSettings;
    private ImageLibraryManager _imageLibraryManager;
    private SfxManager _sfxManager;
    private readonly PluginControl _pluginControl;
    private string _name = "";
    private StringList _terrainTypes;
    private StringList _overlayTypes;
    private StringList _decorationTypes;

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
        EngineMessage message = default;
        _status = _status + Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Destroying);
        EngineMessage.InitializeMessage(out message, 66564, this);
        if (_activatedEngine != null && _activatedEngine.ControlNode != null)
        {
            _activatedEngine.ControlNode.MessageProc(ref message, this);
        }

        base.Dispose();
        _pluginControl?.Dispose();
        _terrainTypes?.Dispose();
        _terrainTypes = null;
        _overlayTypes?.Dispose();
        _overlayTypes = null;
        _decorationTypes?.Dispose();
        _decorationTypes = null;
        _imageLibraryManager?.Dispose();
        _imageLibraryManager = null;
        _sfxManager?.Dispose();
        _sfxManager = null;
        EngineMessage.InitializeMessage(out message, 66565, this);
        if (_activatedEngine != null && _activatedEngine.ControlNode != null)
        {
            _activatedEngine.ControlNode.MessageProc(ref message, this);
        }

        _status = _status - Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Destroying);

        AbilityControl?.Dispose();
        SpellControl?.Dispose();
    }

    // Ability.PAS:131 around HSEngine.pas:3410.
    public override void ReadWrite(StorageStream stream)
    {
        string path = "";
        bool loaded = default;
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            stream.ShowProgress(this, AbilityRules.TranslateResourceString(s_loadingGamePackText), 0);
        }

        SetEngine(stream.Engine);
        _pluginControl.Engine = stream.Engine;
        base.ReadWrite(stream);
        stream.String(ref _name, 4);
        stream.Object(_mapObjectResourceList, 5);
        stream.Object(_resourceEditSettings, 6);
        stream.Object(_pluginControl, 7);

        stream.Object(RaceResources, 10);
        stream.Object(ItemLibraries, 11);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            path = ExtractFilePath(GameTables.GameEngine.HssFileName);
            if (stream.StorageMode().Contains(StorageModeElement.Read))
            {
                loaded = GameTables.GameEngine.ReadFromFileCrc(path + "ABILITY.PFS", AbilityControl, null);
                stream.ShowProgress(this, AbilityRules.TranslateResourceString(s_loadingUnitsText), 70);
                loaded = loaded && GameTables.GameEngine.ReadFromFileCrc(path + "UNITRES.PFS", UnitResources, null);
                loaded = loaded && GameTables.GameEngine.ReadFromFileCrc(path + "UNITGFX.PFS", UnitGraphicsResources, null);
                stream.ShowProgress(this, AbilityRules.TranslateResourceString(s_loadingHeroesText), 80);
                loaded = loaded && GameTables.GameEngine.ReadFromFileCrc(path + "HERORES.PFS", HeroResources, null);
                stream.ShowProgress(this, AbilityRules.TranslateResourceString(s_loadingItemsText), 90);
                loaded = loaded && GameTables.GameEngine.ReadFromFileCrc(path + "ITEMGFX.PFS", ItemGraphicsResources, null);
                loaded = loaded && GameTables.GameEngine.ReadFromFileCrc(path + "FACERES.PFS", FaceResources, null);
                loaded = loaded && GameTables.GameEngine.ReadFromFileCrc(path + "COMB_RES.PFS", CombatResources, null);
                loaded = loaded && GameTables.GameEngine.ReadFromFileCrc(path + "SPELLS.PFS", SpellControl, null);
                loaded = loaded && GameTables.GameEngine.ReadFromFileCrc(path + "TEXT.PFS", TextControl, null);
                if (!loaded)
                {
                    throw new InvalidOperationException("CRC-CHECK FAILED");
                }

                stream.ShowProgress(this, AbilityRules.TranslateResourceString(s_gamePackLoadedText), 100);
            }
            else
            {
                GameTables.GameEngine.WriteToFileCrc(path + "ABILITY.PFS", AbilityControl, null);
                GameTables.GameEngine.WriteToFileCrc(path + "UNITRES.PFS", UnitResources, null);
                GameTables.GameEngine.WriteToFileCrc(path + "UNITGFX.PFS", UnitGraphicsResources, null);
                GameTables.GameEngine.WriteToFileCrc(path + "HERORES.PFS", HeroResources, null);
                GameTables.GameEngine.WriteToFileCrc(path + "ITEMGFX.PFS", ItemGraphicsResources, null);
                GameTables.GameEngine.WriteToFileCrc(path + "FACERES.PFS", FaceResources, null);
                GameTables.GameEngine.WriteToFileCrc(path + "COMB_RES.PFS", CombatResources, null);
                GameTables.GameEngine.WriteToFileCrc(path + "SPELLS.PFS", SpellControl, null);
                GameTables.GameEngine.WriteToFileCrc(path + "TEXT.PFS", TextControl, null);
            }
        }
        else
        {
            stream.Object(UnitResources, 111);
            stream.Object(UnitGraphicsResources, 112);
            stream.Object(HeroResources, 113);
            stream.Object(ItemGraphicsResources, 114);
            stream.Object(FaceResources, 115);
            stream.Object(SpellControl, 117);
            stream.Object(AbilityControl, 118);
            stream.Object(TextControl, 119);
        }

        stream.Object(HeroLibraries, 17);
        stream.Payload(18, ImageSequences.LoadFromStream, ImageSequences.SaveToStream);
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            ImageSequences.LinkToImageLibrary();
            HssCrc = RetrieveHssCrc();
        }
    }

    // HSEngine.pas:3405
    public override void Clear()
    {
        base.Clear();
    }

    // HSEngine.pas:3399, then Ability.PAS:206.
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            _activatedEngine = message.Payload.As<GameEngine>();
        }

        ItemLibraries.MessageProc(ref message, sender);
    }

    // HSEngine.pas:3421
    public void Activate(bool edit)
    {
        EngineMessage message = default;
        EngineMessage.InitializeMessage(out message, 66560, this);
        GetEngine().ControlNode.MessageProc(ref message, this);
        _status = _status + Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Activated);
        if (edit)
        {
            EngineMessage.InitializeMessage(out message, 66562, this);
            GetEngine().ControlNode.MessageProc(ref message, this);
            _status = _status + Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Editting);
        }
    }

    // HSEngine.pas:3436
    public void DeActivate()
    {
        EngineMessage message = default;
        if (_status.Contains(HexSpriteSetStatusElement.Activated))
        {
            EngineMessage.InitializeMessage(out message, 66561, this);
            GetEngine().ControlNode.MessageProc(ref message, this);
            _status = _status - Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Activated);
            if (_status.Contains(HexSpriteSetStatusElement.Editting))
            {
                EngineMessage.InitializeMessage(out message, 66563, this);
                GetEngine().ControlNode.MessageProc(ref message, this);
                _status = _status - Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Editting);
            }
        }
    }

    // HSEngine.pas:3454
    public void SetModified()
    {
        if (_status.Contains(HexSpriteSetStatusElement.Editting))
        {
            _status = _status + Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Modified);
        }
    }

    // HSEngine.pas:3459
    public void ResetModified()
    {
        _status = _status - Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Modified);
    }

    // HSEngine.pas:3464
    public void SetEditting()
    {
        _status = _status + Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Editting);
    }

    // HSEngine.pas:3468
    public void ResetEditting()
    {
        _status = _status - Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Editting);
    }

    public Set<HexSpriteSetStatusElement> Status { get => _status; }
    public HexControl HexControl { get => _hexControl; }
    public RoadControl RoadControl { get => _roadControl; }
    public MapObjectResourceList MapObjectResourceList { get => _mapObjectResourceList; }
    public ResourceEditSettings ResourceEditSettings { get => _resourceEditSettings; }
    public ImageLibraryManager ImageLibraryManager { get => _imageLibraryManager; }
    public SfxManager SfxManager { get => _sfxManager; }
    public PluginControl PluginControl { get => _pluginControl; }
    public string Name { get => _name; set => _name = value; }
    public StringList TerrainTypes { get => _terrainTypes; }
    public StringList OverlayTypes { get => _overlayTypes; }
    public StringList DecorationTypes { get => _decorationTypes; }

    public CustomImageLibrary GeneralImages { get; }
    public CustomImageLibrary CursorImages { get; }
    public CustomImageLibrary CombatImages { get; }
    public RaceResourceList RaceResources { get; }
    public PadControl PadControl { get; }
    public UnitResourceList UnitResources { get; set; }
    public UnitGFXResourceList UnitGraphicsResources { get; set; }
    public HeroResourceList HeroResources { get; set; }
    public ItemGFXList ItemGraphicsResources { get; }
    public ItemLibraryManager ItemLibraries { get; }
    public HeroLibraryManager HeroLibraries { get; }
    public FaceResourceList FaceResources { get; }
    public FlagControl FlagControl { get; }
    public FireControl FireControl { get; }
    public AbilityControl AbilityControl { get; set; }
    public SpellControl SpellControl { get; }
    public MapObjectResourceList CombatResources { get; set; }
    public HexControl CombatHexes { get; }
    public RoadControl CombatRoads { get; }
    public ImageSequenceList ImageSequences { get; }
    public TextControl TextControl { get; set; }
    public int HssCrc { get; private set; }

    // Ability.PAS:194
    public override void MainReadWrite(StorageStream stream)
    {
        string dictionaryFile;
        base.MainReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            dictionaryFile = GameTables.GameEngine.StartupDirectory + "Dict\\" + "Release" + ".mld";
            GameTables.GameEngine.TranslateEngineObjectFromFile(this, dictionaryFile);
        }
    }

    // Ability.PAS:91
    public int GetFileCrc(string fileName, bool atEnd)
    {
        int result;
        Stream fileStream;
        result = 0;
        fileStream = OpenFile(fileName, 0 | 32);
        try
        {
            if (atEnd)
            {
                fileStream.Position = fileStream.Length - 4;
                fileStream.ReadExactly(Bytes.Of(ref result, 4));
            }
            else
            {
                fileStream.ReadExactly(Bytes.Of(ref result, 4));
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
        Int32List crCs;
        string path;
        crCs = new Int32List();
        path = ExtractFilePath(GameTables.GameEngine.HssFileName);
        // { Original includes ABILITY.PFS twice and does not free CRCs on a read error. }
        crCs.Add(GetFileCrc(path + "ABILITY.PFS", false));
        crCs.Add(GetFileCrc(path + "ABILITY.PFS", false));
        crCs.Add(GetFileCrc(path + "UNITRES.PFS", false));
        crCs.Add(GetFileCrc(path + "UNITGFX.PFS", false));
        crCs.Add(GetFileCrc(path + "HERORES.PFS", false));
        crCs.Add(GetFileCrc(path + "ITEMGFX.PFS", false));
        crCs.Add(GetFileCrc(path + "FACERES.PFS", false));
        crCs.Add(GetFileCrc(path + "COMB_RES.PFS", false));
        crCs.Add(GetFileCrc(path + "SPELLS.PFS", false));
        // The scenario set's stored CRC comes from its engine image, so an own document agrees
        // with its original (own-formats.md 4.1). The PFS files keep their original bytes.
        crCs.Add(FileBoundary.ImageTailInt32(FileBoundary.ReadName(path + "RELEASE.HSS"), GameProfile.ScenarioSet));
        int result = EngineServices.GetCrc32(MemoryMarshal.AsBytes(crCs.List.AsSpan()), crCs.Count * 4);
        crCs?.Dispose();
        return result;
    }

    // intf_P1_10.inc:44
    public static GamePack Create(ECustomNode owner) =>
        ConstructionContext.Create(owner, static (context, owner) => new GamePack(context, owner));

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

    public static GamePack Current { get; set; }
}
