// Translated from AoWE.pas.
using System.Runtime.InteropServices;

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

// inherited . fields listed below. Shared resources and controls for a game pack
public partial class AoWHSSet : HSSet
{
    public new const int ClassId = 131088;

    public CustomImageLibrary GeneralImages;
    public CustomImageLibrary CursorImages;
    public CustomImageLibrary CombatImages;
    public RaceResourceList RaceResources;
    public PadControl PadControl;
    public UnitResourceList UnitResources;
    public UnitGFXResourceList UnitGraphicsResources;
    public HeroResourceList HeroResources;
    public ItemGFXList ItemGraphicsResources;
    public ItemLibraryManager ItemLibraries;
    public HeroLibraryManager HeroLibraries;
    public FaceResourceList FaceResources;
    public FlagControl FlagControl;
    public FireControl FireControl;
    public AbilityControl AbilityControl;
    public SpellControl SpellControl;
    public MapObjectResourceList CombatResources;
    public HexControl CombatHexes;
    public RoadControl CombatRoads;
    public ImageSequenceList ImageSequences;
    public TextControl TextControl;
    public int HssCrc;

    // Ability.PAS:74
    public override void Dispose()
    {
        FireControl?.Dispose();
        ImageSequences?.Dispose();
        TextControl?.Dispose();
        AbilityControl.UnregisterAll();
        SpellControl.UnregisterAll();
        ItemLibraries?.Dispose();
        HeroLibraries?.Dispose();
        base.Dispose();
        AbilityControl?.Dispose();
        SpellControl?.Dispose();
    }

    // Ability.PAS:131
    public override void ReadWrite(EStorageStream stream)
    {
        string path = "";
        bool loaded = default;
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            stream.ShowProgress(this, AbilityRules.TranslateResourceString(s_loadingGamePackText), 0);
        }

        base.ReadWrite(stream);
        stream.rwEObject(RaceResources, 10);
        stream.rwEObject(ItemLibraries, 11);
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
            stream.rwEObject(UnitResources, 111);
            stream.rwEObject(UnitGraphicsResources, 112);
            stream.rwEObject(HeroResources, 113);
            stream.rwEObject(ItemGraphicsResources, 114);
            stream.rwEObject(FaceResources, 115);
            stream.rwEObject(SpellControl, 117);
            stream.rwEObject(AbilityControl, 118);
            stream.rwEObject(TextControl, 119);
        }

        stream.rwEObject(HeroLibraries, 17);
        stream.rwStreamProc(18, ImageSequences.LoadFromStream, ImageSequences.SaveToStream);
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            ImageSequences.LinkToImageLibrary();
            HssCrc = RetrieveHssCrc();
        }
    }

    // Ability.PAS:194
    public override void MainReadWrite(EStorageStream stream)
    {
        string dictionaryFile;
        base.MainReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            dictionaryFile = GameTables.GameEngine.StartupDirectory + "Dict\\" + "Release" + ".mld";
            GameTables.GameEngine.TranslateEngineObjectFromFile(this, dictionaryFile);
        }
    }

    private protected AoWHSSet(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
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
        var water = AbilityRules.TranslateResourceString(GameTables.s_waterText);
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
        var earth = AbilityRules.TranslateResourceString(GameTables.s_earthText);
        TerrainTypes.Add(earth);
        var rock = AbilityRules.TranslateResourceString(s_rockText);
        TerrainTypes.Add(rock);
        var lava = AbilityRules.TranslateResourceString(s_lavaText);
        TerrainTypes.Add(lava);
        var waterText = AbilityRules.TranslateResourceString(GameTables.s_waterText);
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
        var mountain = AbilityRules.TranslateResourceString(GameText.s_mountainText);
        OverlayTypes.Add(mountain);
        var denseVegetation = AbilityRules.TranslateResourceString(s_denseVegetationText);
        OverlayTypes.Add(denseVegetation);
        var hill = AbilityRules.TranslateResourceString(GameText.s_hillText);
        OverlayTypes.Add(hill);
        var vegetation = AbilityRules.TranslateResourceString(s_vegetationText);
        OverlayTypes.Add(vegetation);
        var road = AbilityRules.TranslateResourceString(s_roadText);
        OverlayTypes.Add(road);
        var bridge = AbilityRules.TranslateResourceString(s_bridgeText);
        OverlayTypes.Add(bridge);
        var rubble = AbilityRules.TranslateResourceString(s_rubbleText);
        OverlayTypes.Add(rubble);
        var structure = AbilityRules.TranslateResourceString(GameText.s_structureText);
        OverlayTypes.Add(structure);
        var obstacle = AbilityRules.TranslateResourceString(GameText.s_obstacleText);
        OverlayTypes.Add(obstacle);
        var lightVegetation = AbilityRules.TranslateResourceString(s_lightVegetationText);
        OverlayTypes.Add(lightVegetation);
        var solid = AbilityRules.TranslateResourceString(s_solidText);
        OverlayTypes.Add(solid);
        var none = AbilityRules.TranslateResourceString(GameTables.s_noneText);
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

    // Ability.PAS:206
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        ItemLibraries.MessageProc(ref message, sender);
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
    public static AoWHSSet Create(ECustomNode owner) =>
        ConstructionContext.Create(owner, static (context, owner) => new AoWHSSet(context, owner));

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

    public static AoWHSSet Current;

}
