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
    public new const int ClassID = 131088;

    public CustomImageLibrary FGeneralImages;
    public CustomImageLibrary FCursorImages;
    public CustomImageLibrary FCombatImages;
    public RaceResourceList FRaceResources;
    public PadControl FPadControl;
    public UnitResourceList FUnitResources;
    public UnitGFXResourceList FUnitGFXResources;
    public HeroResourceList FHeroResources;
    public ItemGFXList FItemGFXResources;
    public ItemLibraryManager FItemLibraries;
    public HeroLibraryManager FHeroLibraries;
    public FaceResourceList FFaceResources;
    public FlagControl FFlagControl;
    public FireControl FFireControl;
    public AbilityControl FAbilityControl;
    public SpellControl FSpellControl;
    public MapObjectResourceList FCombatResources;
    public HexagonControl FCombatHexagons;
    public RoadControl FCombatRoads;
    public ImageSequenceList FImageSequences;
    public TextControl FTextControl;
    public int FHSSCRC;

    // Ability.PAS:74
    public override void Dispose()
    {
        FFireControl?.Dispose();
        FImageSequences?.Dispose();
        FTextControl?.Dispose();
        FAbilityControl.UnregisterAll();
        FSpellControl.UnregisterAll();
        FItemLibraries?.Dispose();
        FHeroLibraries?.Dispose();
        base.Dispose();
        FAbilityControl?.Dispose();
        FSpellControl?.Dispose();
    }

    // Ability.PAS:131
    public override void ReadWrite(EStorageStream Stream)
    {
        string Path = "";
        bool Loaded = default;
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            Stream.ShowProgress(this, AbilityRules.TranslateRStr(LoadingGamePackRStr), 0);
        }

        base.ReadWrite(Stream);
        Stream.rwEObject(FRaceResources, 10);
        Stream.rwEObject(FItemLibraries, 11);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Path = ExtractFilePath(GameTables.AoWEngine.HSSFileName);
            if (Stream.StorageMode().Contains(StorageModeElement.Read))
            {
                Loaded = GameTables.AoWEngine.ReadFromFileCRC(Path + "ABILITY.PFS", FAbilityControl, null);
                Stream.ShowProgress(this, AbilityRules.TranslateRStr(LoadingUnitsRStr), 70);
                Loaded = Loaded && GameTables.AoWEngine.ReadFromFileCRC(Path + "UNITRES.PFS", FUnitResources, null);
                Loaded = Loaded && GameTables.AoWEngine.ReadFromFileCRC(Path + "UNITGFX.PFS", FUnitGFXResources, null);
                Stream.ShowProgress(this, AbilityRules.TranslateRStr(LoadingHeroesRStr), 80);
                Loaded = Loaded && GameTables.AoWEngine.ReadFromFileCRC(Path + "HERORES.PFS", FHeroResources, null);
                Stream.ShowProgress(this, AbilityRules.TranslateRStr(LoadingItemsRStr), 90);
                Loaded = Loaded && GameTables.AoWEngine.ReadFromFileCRC(Path + "ITEMGFX.PFS", FItemGFXResources, null);
                Loaded = Loaded && GameTables.AoWEngine.ReadFromFileCRC(Path + "FACERES.PFS", FFaceResources, null);
                Loaded = Loaded && GameTables.AoWEngine.ReadFromFileCRC(Path + "COMB_RES.PFS", FCombatResources, null);
                Loaded = Loaded && GameTables.AoWEngine.ReadFromFileCRC(Path + "SPELLS.PFS", FSpellControl, null);
                Loaded = Loaded && GameTables.AoWEngine.ReadFromFileCRC(Path + "TEXT.PFS", FTextControl, null);
                if (!Loaded)
                {
                    throw new InvalidOperationException("CRC-CHECK FAILED");
                }

                Stream.ShowProgress(this, AbilityRules.TranslateRStr(GamePackLoadedRStr), 100);
            }
            else
            {
                GameTables.AoWEngine.WriteToFileCRC(Path + "ABILITY.PFS", FAbilityControl, null);
                GameTables.AoWEngine.WriteToFileCRC(Path + "UNITRES.PFS", FUnitResources, null);
                GameTables.AoWEngine.WriteToFileCRC(Path + "UNITGFX.PFS", FUnitGFXResources, null);
                GameTables.AoWEngine.WriteToFileCRC(Path + "HERORES.PFS", FHeroResources, null);
                GameTables.AoWEngine.WriteToFileCRC(Path + "ITEMGFX.PFS", FItemGFXResources, null);
                GameTables.AoWEngine.WriteToFileCRC(Path + "FACERES.PFS", FFaceResources, null);
                GameTables.AoWEngine.WriteToFileCRC(Path + "COMB_RES.PFS", FCombatResources, null);
                GameTables.AoWEngine.WriteToFileCRC(Path + "SPELLS.PFS", FSpellControl, null);
                GameTables.AoWEngine.WriteToFileCRC(Path + "TEXT.PFS", FTextControl, null);
            }
        }
        else
        {
            Stream.rwEObject(FUnitResources, 111);
            Stream.rwEObject(FUnitGFXResources, 112);
            Stream.rwEObject(FHeroResources, 113);
            Stream.rwEObject(FItemGFXResources, 114);
            Stream.rwEObject(FFaceResources, 115);
            Stream.rwEObject(FSpellControl, 117);
            Stream.rwEObject(FAbilityControl, 118);
            Stream.rwEObject(FTextControl, 119);
        }

        Stream.rwEObject(FHeroLibraries, 17);
        Stream.rwStreamProc(18, FImageSequences.LoadFromStream, FImageSequences.SaveToStream);
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            FImageSequences.LinkToIL();
            FHSSCRC = RetrieveHSSCRC();
        }
    }

    // Ability.PAS:194
    public override void MainReadWrite(EStorageStream Stream)
    {
        string DictionaryFile;
        base.MainReadWrite(Stream);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            DictionaryFile = GameTables.AoWEngine.StartupDirectory + "Dict\\" + "Release" + ".mld";
            GameTables.AoWEngine.TranslateEObjectFromFile(this, DictionaryFile);
        }
    }

    private protected AoWHSSet(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
        BitList Enabled;
        FCombatResources = new MapObjectResourceList(this);
        FCombatResources.SetControlStyle(Set<EControlStyleElement>.Of(EControlStyleElement.SaveLoad, EControlStyleElement.FixedChild));
        FCombatHexagons = new HexagonControl(this);
        FCombatHexagons.MOClass = typeof(AbstractHexagon);
        FCombatRoads = new RoadControl(this);
        FCombatRoads.MOClass = typeof(AbstractRoad);
        ImageLibraryManager.ExecutePathSearch(GameTables.AoWEngine.StartupDirectory + "IMAGES");
        SFXManager.ExecutePathSearch(GameTables.AoWEngine.StartupDirectory + "SFX");
        FImageSequences = new ImageSequenceList(ImageLibraryManager, null);
        var water = AbilityRules.TranslateRStr(GameTables.WaterRStr);
        TerrainTypes.Add(water);
        var aoWETranslateRStr = AbilityRules.TranslateRStr(GrassRStr);
        TerrainTypes.Add(aoWETranslateRStr);
        var desert = AbilityRules.TranslateRStr(DesertRStr);
        TerrainTypes.Add(desert);
        var snow = AbilityRules.TranslateRStr(SnowRStr);
        TerrainTypes.Add(snow);
        var steppe = AbilityRules.TranslateRStr(SteppeRStr);
        TerrainTypes.Add(steppe);
        var wasteland = AbilityRules.TranslateRStr(WastelandRStr);
        TerrainTypes.Add(wasteland);
        var ice = AbilityRules.TranslateRStr(IceRStr);
        TerrainTypes.Add(ice);
        var earth = AbilityRules.TranslateRStr(GameTables.EarthRStr);
        TerrainTypes.Add(earth);
        var rock = AbilityRules.TranslateRStr(RockRStr);
        TerrainTypes.Add(rock);
        var lava = AbilityRules.TranslateRStr(LavaRStr);
        TerrainTypes.Add(lava);
        var waterText = AbilityRules.TranslateRStr(GameTables.WaterRStr);
        TerrainTypes.Add(waterText);
        var reserved = AbilityRules.TranslateRStr(ReservedRStr);
        TerrainTypes.Add(reserved);
        var dirt = AbilityRules.TranslateRStr(DirtRStr);
        TerrainTypes.Add(dirt);
        var iceText = AbilityRules.TranslateRStr(IceRStr);
        TerrainTypes.Add(iceText);
        var coast = AbilityRules.TranslateRStr(CoastRStr);
        TerrainTypes.Add(coast);
        var border = AbilityRules.TranslateRStr(BorderRStr);
        TerrainTypes.Add(border);
        var mountain = AbilityRules.TranslateRStr(GameText.MountainRStr);
        OverlayTypes.Add(mountain);
        var denseVegetation = AbilityRules.TranslateRStr(DenseVegetationRStr);
        OverlayTypes.Add(denseVegetation);
        var hill = AbilityRules.TranslateRStr(GameText.HillRStr);
        OverlayTypes.Add(hill);
        var vegetation = AbilityRules.TranslateRStr(VegetationRStr);
        OverlayTypes.Add(vegetation);
        var road = AbilityRules.TranslateRStr(RoadRStr);
        OverlayTypes.Add(road);
        var bridge = AbilityRules.TranslateRStr(BridgeRStr);
        OverlayTypes.Add(bridge);
        var rubble = AbilityRules.TranslateRStr(RubbleRStr);
        OverlayTypes.Add(rubble);
        var structure = AbilityRules.TranslateRStr(GameText.StructureRStr);
        OverlayTypes.Add(structure);
        var obstacle = AbilityRules.TranslateRStr(GameText.ObstacleRStr);
        OverlayTypes.Add(obstacle);
        var lightVegetation = AbilityRules.TranslateRStr(LightVegetationRStr);
        OverlayTypes.Add(lightVegetation);
        var solid = AbilityRules.TranslateRStr(SolidRStr);
        OverlayTypes.Add(solid);
        var none = AbilityRules.TranslateRStr(GameTables.NoneRStr);
        DecorationTypes.Add(none);
        var rocks = AbilityRules.TranslateRStr(RocksRStr);
        DecorationTypes.Add(rocks);
        var bones = AbilityRules.TranslateRStr(BonesRStr);
        DecorationTypes.Add(bones);
        var deadTree = AbilityRules.TranslateRStr(DeadTreeRStr);
        DecorationTypes.Add(deadTree);
        var toadstools = AbilityRules.TranslateRStr(ToadstoolsRStr);
        DecorationTypes.Add(toadstools);
        FPadControl = new PadControl(this);
        Enabled = FPadControl.FTerrainTypes;
        Enabled[1] = true;
        Enabled[2] = true;
        Enabled[3] = true;
        Enabled[4] = true;
        Enabled[5] = true;
        Enabled[6] = true;
        Enabled[12] = true;
        FRaceResources = new RaceResourceList(this);
        FFlagControl = FlagControl.Create(this);
        FFireControl = new FireControl();
        FUnitResources = new UnitResourceList(this);
        FUnitGFXResources = new UnitGFXResourceList(this);
        FHeroResources = new HeroResourceList(this);
        FItemGFXResources = new ItemGFXList(this);
        FFaceResources = new FaceResourceList(this);
        FItemLibraries = new ItemLibraryManager(null);
        FHeroLibraries = new HeroLibraryManager(null);
        FAbilityControl = new AbilityControl(null);
        FSpellControl = new SpellControl(null);
        FTextControl = new TextControl(null);
        FGeneralImages = ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "GENERAL.ILB");
        FCursorImages = ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "CURSORS.ILB");
        FCombatImages = ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "COMBAT.ILB");
    }

    // Ability.PAS:206
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        FItemLibraries.MsgProc(ref Msg, Sender);
    }

    // Ability.PAS:91
    public int GetFileCRC(string FileName, bool AtEnd)
    {
        int Result;
        Stream FileStream;
        Result = 0;
        FileStream = OpenFile(FileName, 0 | 32);
        try
        {
            if (AtEnd)
            {
                FileStream.Position = FileStream.Length - 4;
                FileStream.ReadBytes(Bytes.Of(ref Result, 4));
            }
            else
            {
                FileStream.ReadBytes(Bytes.Of(ref Result, 4));
            }
        }
        finally
        {
            FileStream?.Dispose();
        }

        return Result;
    }

    // Ability.PAS:109
    public int RetrieveHSSCRC()
    {
        IntegerList CRCs;
        string Path;
        CRCs = new IntegerList();
        Path = ExtractFilePath(GameTables.AoWEngine.HSSFileName);
        // { Original includes ABILITY.PFS twice and does not free CRCs on a read error. }
        CRCs.Add(GetFileCRC(Path + "ABILITY.PFS", false));
        CRCs.Add(GetFileCRC(Path + "ABILITY.PFS", false));
        CRCs.Add(GetFileCRC(Path + "UNITRES.PFS", false));
        CRCs.Add(GetFileCRC(Path + "UNITGFX.PFS", false));
        CRCs.Add(GetFileCRC(Path + "HERORES.PFS", false));
        CRCs.Add(GetFileCRC(Path + "ITEMGFX.PFS", false));
        CRCs.Add(GetFileCRC(Path + "FACERES.PFS", false));
        CRCs.Add(GetFileCRC(Path + "COMB_RES.PFS", false));
        CRCs.Add(GetFileCRC(Path + "SPELLS.PFS", false));
        // The scenario set's stored CRC comes from its engine image, so an own document agrees
        // with its original (own-formats.md 4.1). The PFS files keep their original bytes.
        CRCs.Add(FileBoundary.ImageTailInt32(FileBoundary.ReadName(Path + "RELEASE.HSS"), GameProfile.ScenarioSet));
        int Result = EngineServices.GetCRC32(MemoryMarshal.AsBytes(CRCs.List.AsSpan()), CRCs.Count * 4);
        CRCs?.Dispose();
        return Result;
    }

    // intf_P1_10.inc:44
    public static AoWHSSet Create(ECustomNode owner) =>
        ConstructionContext.Create(owner, static (context, owner) => new AoWHSSet(context, owner));

    private static readonly string GrassRStr = "Grass";

    private static readonly string DesertRStr = "Desert";

    private static readonly string SnowRStr = "Snow";

    private static readonly string SteppeRStr = "Steppe";

    private static readonly string WastelandRStr = "Wasteland";

    private static readonly string IceRStr = "Ice";

    private static readonly string RockRStr = "Rock";

    private static readonly string LavaRStr = "Lava";

    private static readonly string ReservedRStr = "Reserved";

    private static readonly string DirtRStr = "Dirt";

    private static readonly string CoastRStr = "Coast";

    private static readonly string BorderRStr = "Border";

    private static readonly string DenseVegetationRStr = "Dense Vegetation";

    private static readonly string VegetationRStr = "Vegetation";

    private static readonly string RoadRStr = "Road";

    private static readonly string BridgeRStr = "Bridge";

    private static readonly string RubbleRStr = "Rubble";

    private static readonly string LightVegetationRStr = "Light Vegetation";

    private static readonly string SolidRStr = "Solid";

    private static readonly string RocksRStr = "Rocks";

    private static readonly string BonesRStr = "Bones";

    private static readonly string DeadTreeRStr = "Dead Tree";

    private static readonly string ToadstoolsRStr = "Toadstools";

    private static readonly string LoadingGamePackRStr = "Loading Game Pack";

    private static readonly string LoadingUnitsRStr = "Loading Units";

    private static readonly string LoadingHeroesRStr = "Loading Heroes";

    private static readonly string LoadingItemsRStr = "Loading Items";

    private static readonly string GamePackLoadedRStr = "Game Pack Loaded";

    public static AoWHSSet Current;

}
