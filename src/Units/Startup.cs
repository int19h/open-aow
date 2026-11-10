// Translated from the Delphi units AGCPlugins, AOWChatWin, AOWControlWin, AOWDialogWin, AOWGauge, AOWInterfaceImgLib, AOWMWindow, AOWMenu, AOWMenuBar, AOWMultiWin, AOWPyreScrn, AOWTitleWindow, AOWToolTip, AOWWinManager, AirNode, AlgnmBox, AltDeath, AltFire, AltIce, AltLife, AltLight, AltPest, Altar, Animation, AoWArenaDlg, AoWArmyEventDlg, AoWArmyInfoDlg, AoWArnoLine, AoWBaseWin, AoWBookWindow, AoWButton, AoWCampaignJoin, AoWCampaignSettings, AoWCampaignTransferDlg, AoWCastUnitSpellDlg, AoWCheckBox, AoWCityJoinDlg, AoWCityRebelDlg, AoWCityScreen, AoWComboBox, AoWComp, AoWConstructDlg, AoWControl, AoWDeformFaceDlg, AoWDisjunctionDlg, AoWE, AoWEReg, AoWEdit, AoWEmailGameSettingsDlg, AoWEmailSettingsDlg, AoWExeForms, AoWFileDialog, AoWFrame, AoWGamePlayDlg, AoWHScroll, AoWHTMLMemo, AoWHeroInfoDlg, AoWHeroJoinDlg, AoWHeroUpgradeDlg, AoWHex, AoWImage, AoWInfoBanner, AoWInfoWin, AoWIntroScene, AoWItemBanner, AoWItemsFoundDlg, AoWLabel, AoWLeaderSetup, AoWLenLine, AoWListBox, AoWMagic, AoWMemo, AoWMessageDlg, AoWMsgReceiveDlg, AoWNewTurnDialog, AoWPInfoWin, AoWPOP3Dlg, AoWPWindows, AoWPanel, AoWPartyJoin, AoWPasswordDlg, AoWPowerDlg, AoWProdScreen, AoWProgressDialog, AoWRealm, AoWReg, AoWRelations, AoWResultsWin, AoWRoad, AoWSMTPDlg, AoWSWindow, AoWScenSetup, AoWSelFactionWin, AoWSendEmailGameDlg, AoWSetup, AoWSoundDlg, AoWSpellBook, AoWSyncDlg, AoWTBWindow, AoWTC, AoWTCReg, AoWTCScanner, AoWTCUnit, AoWTabPanel, AoWTargetSelection, AoWTer, AoWTitleScene, AoWTutorialWin, AoWUnitBanner, AoWUnitSelDlg, AoWUnitWindow, AoWVScroll, AoWVideoDlg, AoWWinControl, AoWWinGeneric, AoWWinMan, AoWWindow, AoWWizardsTowerDlg, AoWWorldMap, AowUserWindow, Arena, BehaviorAG, BldGuild, BloodShrine, BorderHx, Bridge, CMapCtrl, Cave, CenterViewLoc, City, CitySpells, CityWall, CombatFire, CombatHex, CombatILTer, CombatObstacle, CombatRoad, CombatSpells, CombatStructure, CombatTE, CombatTerrain, Crops, Crypt, DeathNode, DisplayC, Dungeon, E1GameFlow, E1Globals, E1Resources, E1TacticalSupport, E2CampaignSupport, E2DirectSound, E2GameFlow, E2Globals, E2WindowLayout, E3EventDialogs, E3Globals, E3IMSTypes, E3StructureInfo, EList, EarthNode, Engine, EventLog, ExploreS, FMemStr, Farm, FastCombatWin, FileSecurity, FireNode, FlagEvents, FleeLoc, FontMod, GFXE, Gems, GlobalEnchantmentSpells, GlobalSpells, GlobalTargetSpells, HSEngine, HSLib, HashTabU, HeroCtrl, HeroGrid, Hexagon, Hill, HolyG, House, HxBridge, ILTer, ImageLib, IndependentAG, IsoCombatHex, IsoHex, ItemCtrl, ItemExpS, ItemGrid, LeadrGrd, LicenseF, LifeNode, MagicSanctuary, MainAG, MainStructure, ManaCrystals, MapCtrl, Merchant, Mine, MonsterL, Mountain, MoveOnEvents, NetworkE, OverlayObject, OverviewUnit, Pad, PageTurnCtrl, PassiveAb, PlayerBox, PlugInM, PoisonC, PoisonP, PowerNode, PrdPlace, Pyramid, RaceBox, RandomNode, ReflectingPool, RndHex, Road, Ruin, Scanner, Shipyard, Sign, SingleCombatHex, SmokeCloud, Sound, Spring, StartMenu, Storms, SummonSpells, TCFCDlg, Teleport, Terrain, Tower, Townhall, TurnButton, UnHolyG, UnitGrid, UnitIDlg, UnitIGrd, UnitSpells, UnitTabC, WaterNode, WizardsTower, WorkShop, aowFX, aowHelpWin, aowIntGfxMod, aowMapEvents, aowSetupEvents, aowTCMapEvents, aowUserDrawBox.
using System.Runtime.ExceptionServices;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Abilities.Passive;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.City;
using Aow.Game.Magic.Spells.Combat;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Magic.Spells.GlobalEnchantment;
using Aow.Game.Magic.Spells.GlobalTarget;
using Aow.Game.Magic.Spells.Summon;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;

namespace Aow.Game.Engine;

/// <summary>One unit of the program in initialization order, with its initialization and
/// finalization sections. A unit without a section has a null delegate.</summary>
internal sealed record StartupUnit(string Name, Action Initialize, Action Finalize);

public static class Startup
{
    // The units in the order the Delphi program initializes them. Finalization runs the table
    // backwards.
    internal static readonly StartupUnit[] Units =
    [
        new("GFXE", GFXEUnit.Initialize, GFXEUnit.Finalize),
        new("HashTabU", null, null),
        new("ImageLib", ImageLibUnit.Initialize, ImageLibUnit.Finalize),
        new("aowFX", null, null),
        new("FMemStr", null, null),
        new("Engine", EngineUnit.Initialize, EngineUnit.Finalize),
        new("AoWReg", AoWRegUnit.Initialize, AoWRegUnit.Finalize),
        new("Sound", SoundUnit.Initialize, SoundUnit.Finalize),
        new("NetworkE", null, null),
        new("EList", null, null),
        new("HSEngine", null, null),
        new("Animation", AnimationUnit.Initialize, AnimationUnit.Finalize),
        new("ILTer", null, null),
        new("EventLog", EventLogUnit.Initialize, EventLogUnit.Finalize),
        new("Pad", PadUnit.Initialize, PadUnit.Finalize),
        new("Terrain", null, null),
        new("AoWE", AoWEUnit.Initialize, AoWEUnit.Finalize),
        new("PassiveAb", PassiveAbUnit.Initialize, PassiveAbUnit.Finalize),
        new("CombatSpells", CombatSpellsUnit.Initialize, CombatSpellsUnit.Finalize),
        new("GlobalEnchantmentSpells", GlobalEnchantmentSpellsUnit.Initialize, GlobalEnchantmentSpellsUnit.Finalize),
        new("SmokeCloud", SmokeCloudUnit.Initialize, SmokeCloudUnit.Finalize),
        new("GlobalTargetSpells", GlobalTargetSpellsUnit.Initialize, GlobalTargetSpellsUnit.Finalize),
        new("GlobalSpells", GlobalSpellsUnit.Initialize, GlobalSpellsUnit.Finalize),
        new("UnitSpells", UnitSpellsUnit.Initialize, UnitSpellsUnit.Finalize),
        new("SummonSpells", SummonSpellsUnit.Initialize, SummonSpellsUnit.Finalize),
        new("PoisonC", PoisonCUnit.Initialize, PoisonCUnit.Finalize),
        new("Storms", StormsUnit.Initialize, StormsUnit.Finalize),
        new("BloodShrine", BloodShrineUnit.Initialize, BloodShrineUnit.Finalize),
        new("ExploreS", ExploreSUnit.Initialize, ExploreSUnit.Finalize),
        new("ItemExpS", ItemExpSUnit.Initialize, ItemExpSUnit.Finalize),
        new("AoWSetup", AoWSetupUnit.Initialize, AoWSetupUnit.Finalize),
        new("Crops", CropsUnit.Initialize, CropsUnit.Finalize),
        new("City", CityUnit.Initialize, CityUnit.Finalize),
        new("AGCPlugins", AGCPluginsUnit.Initialize, AGCPluginsUnit.Finalize),
        new("BehaviorAG", BehaviorAGUnit.Initialize, BehaviorAGUnit.Finalize),
        new("MainAG", MainAGUnit.Initialize, MainAGUnit.Finalize),
        new("IndependentAG", IndependentAGUnit.Initialize, IndependentAGUnit.Finalize),
        new("Arena", ArenaUnit.Initialize, ArenaUnit.Finalize),
        new("WizardsTower", WizardsTowerUnit.Initialize, WizardsTowerUnit.Finalize),
        new("ReflectingPool", ReflectingPoolUnit.Initialize, ReflectingPoolUnit.Finalize),
        new("MagicSanctuary", MagicSanctuaryUnit.Initialize, MagicSanctuaryUnit.Finalize),
        new("OverlayObject", OverlayObjectUnit.Initialize, OverlayObjectUnit.Finalize),
        new("MoveOnEvents", MoveOnEventsUnit.Initialize, MoveOnEventsUnit.Finalize),
        new("Altar", AltarUnit.Initialize, AltarUnit.Finalize),
        new("AltPest", AltPestUnit.Initialize, AltPestUnit.Finalize),
        new("AltDeath", AltDeathUnit.Initialize, AltDeathUnit.Finalize),
        new("AltLife", AltLifeUnit.Initialize, AltLifeUnit.Finalize),
        new("AltLight", AltLightUnit.Initialize, AltLightUnit.Finalize),
        new("FlagEvents", FlagEventsUnit.Initialize, FlagEventsUnit.Finalize),
        new("PowerNode", PowerNodeUnit.Initialize, PowerNodeUnit.Finalize),
        new("AltIce", AltIceUnit.Initialize, AltIceUnit.Finalize),
        new("AltFire", AltFireUnit.Initialize, AltFireUnit.Finalize),
        new("PrdPlace", PrdPlaceUnit.Initialize, PrdPlaceUnit.Finalize),
        new("BldGuild", BldGuildUnit.Initialize, BldGuildUnit.Finalize),
        new("Merchant", MerchantUnit.Initialize, MerchantUnit.Finalize),
        new("UnHolyG", UnHolyGUnit.Initialize, UnHolyGUnit.Finalize),
        new("HolyG", HolyGUnit.Initialize, HolyGUnit.Finalize),
        new("Shipyard", ShipyardUnit.Initialize, ShipyardUnit.Finalize),
        new("Pyramid", PyramidUnit.Initialize, PyramidUnit.Finalize),
        new("Ruin", RuinUnit.Initialize, RuinUnit.Finalize),
        new("Crypt", CryptUnit.Initialize, CryptUnit.Finalize),
        new("Dungeon", DungeonUnit.Initialize, DungeonUnit.Finalize),
        new("Gems", GemsUnit.Initialize, GemsUnit.Finalize),
        new("PoisonP", PoisonPUnit.Initialize, PoisonPUnit.Finalize),
        new("Tower", TowerUnit.Initialize, TowerUnit.Finalize),
        new("MonsterL", MonsterLUnit.Initialize, MonsterLUnit.Finalize),
        new("AirNode", AirNodeUnit.Initialize, AirNodeUnit.Finalize),
        new("WaterNode", WaterNodeUnit.Initialize, WaterNodeUnit.Finalize),
        new("LifeNode", LifeNodeUnit.Initialize, LifeNodeUnit.Finalize),
        new("DeathNode", DeathNodeUnit.Initialize, DeathNodeUnit.Finalize),
        new("EarthNode", EarthNodeUnit.Initialize, EarthNodeUnit.Finalize),
        new("FireNode", FireNodeUnit.Initialize, FireNodeUnit.Finalize),
        new("ManaCrystals", ManaCrystalsUnit.Initialize, ManaCrystalsUnit.Finalize),
        new("Mine", MineUnit.Initialize, MineUnit.Finalize),
        new("Cave", CaveUnit.Initialize, CaveUnit.Finalize),
        new("Farm", FarmUnit.Initialize, FarmUnit.Finalize),
        new("CitySpells", CitySpellsUnit.Initialize, CitySpellsUnit.Finalize),
        new("Teleport", TeleportUnit.Initialize, TeleportUnit.Finalize),
        new("Sign", SignUnit.Initialize, SignUnit.Finalize),
        new("Spring", SpringUnit.Initialize, SpringUnit.Finalize),
        new("Hexagon", null, null),
        new("BorderHx", null, null),
        new("Road", null, null),
        new("Bridge", null, null),
        new("AoWTer", AoWTerUnit.Initialize, AoWTerUnit.Finalize),
        new("Mountain", MountainUnit.Initialize, MountainUnit.Finalize),
        new("AoWRoad", AoWRoadUnit.Initialize, AoWRoadUnit.Finalize),
        new("RndHex", null, null),
        new("IsoHex", null, null),
        new("AoWHex", AoWHexUnit.Initialize, AoWHexUnit.Finalize),
        new("HxBridge", HxBridgeUnit.Initialize, HxBridgeUnit.Finalize),
        new("AoWEReg", AoWERegUnit.Initialize, AoWERegUnit.Finalize),
        new("LicenseF", LicenseFUnit.Initialize, LicenseFUnit.Finalize),
        new("DisplayC", null, null),
        new("AOWInterfaceImgLib", null, null),
        new("AoWComp", null, null),
        new("AoWWinControl", null, null),
        new("AOWWinManager", null, null),
        new("AoWBaseWin", null, null),
        new("AoWWinGeneric", null, null),
        new("AoWWindow", null, null),
        new("AOWToolTip", null, null),
        new("AoWControl", null, null),
        new("AoWVScroll", null, null),
        new("AOWMenuBar", null, null),
        new("AOWMenu", null, null),
        new("AoWButton", null, null),
        new("AoWCheckBox", null, null),
        new("FleeLoc", FleeLocUnit.Initialize, FleeLocUnit.Finalize),
        new("CombatObstacle", CombatObstacleUnit.Initialize, CombatObstacleUnit.Finalize),
        new("CombatILTer", CombatILTerUnit.Initialize, CombatILTerUnit.Finalize),
        new("CityWall", CityWallUnit.Initialize, CityWallUnit.Finalize),
        new("CombatFire", CombatFireUnit.Initialize, CombatFireUnit.Finalize),
        new("CombatRoad", CombatRoadUnit.Initialize, CombatRoadUnit.Finalize),
        new("CombatTerrain", CombatTerrainUnit.Initialize, CombatTerrainUnit.Finalize),
        new("CombatTE", CombatTEUnit.Initialize, CombatTEUnit.Finalize),
        new("CombatHex", CombatHexUnit.Initialize, CombatHexUnit.Finalize),
        new("AoWTC", Aow.Game.Combat.AoWTCUnit.Initialize, Aow.Game.Combat.AoWTCUnit.Finalize),
        new("AoWFrame", null, null),
        new("AoWTabPanel", null, null),
        new("AoWPanel", null, null),
        new("MapCtrl", MapCtrlUnit.Initialize, MapCtrlUnit.Finalize),
        new("TurnButton", TurnButtonUnit.Initialize, TurnButtonUnit.Finalize),
        new("AoWEdit", null, null),
        new("AoWLabel", null, null),
        new("AoWImage", null, null),
        new("AoWMemo", null, null),
        new("AoWHTMLMemo", null, null),
        new("aowIntGfxMod", null, null),
        new("aowHelpWin", null, null),
        new("AOWGauge", null, null),
        new("AoWHScroll", null, null),
        new("AoWListBox", null, null),
        new("aowUserDrawBox", null, null),
        new("CMapCtrl", CMapCtrlUnit.Initialize, CMapCtrlUnit.Finalize),
        new("Scanner", ScannerUnit.Initialize, ScannerUnit.Finalize),
        new("AoWComboBox", null, null),
        new("PageTurnCtrl", PageTurnCtrlUnit.Initialize, PageTurnCtrlUnit.Finalize),
        new("AoWBookWindow", AoWBookWindowUnit.Initialize, AoWBookWindowUnit.Finalize),
        new("AowUserWindow", AowUserWindowUnit.Initialize, AowUserWindowUnit.Finalize),
        new("StartMenu", StartMenuUnit.Initialize, StartMenuUnit.Finalize),
        new("FontMod", null, null),
        new("AoWEmailSettingsDlg", null, null),
        new("AoWArnoLine", AoWArnoLineUnit.Initialize, AoWArnoLineUnit.Finalize),
        new("AoWLenLine", AoWLenLineUnit.Initialize, AoWLenLineUnit.Finalize),
        new("AoWPInfoWin", null, null),
        new("CenterViewLoc", CenterViewLocUnit.Initialize, CenterViewLocUnit.Finalize),
        new("CombatStructure", CombatStructureUnit.Initialize, CombatStructureUnit.Finalize),
        new("MainStructure", MainStructureUnit.Initialize, MainStructureUnit.Finalize),
        new("SingleCombatHex", SingleCombatHexUnit.Initialize, SingleCombatHexUnit.Finalize),
        new("House", HouseUnit.Initialize, HouseUnit.Finalize),
        new("Townhall", TownhallUnit.Initialize, TownhallUnit.Finalize),
        new("IsoCombatHex", IsoCombatHexUnit.Initialize, IsoCombatHexUnit.Finalize),
        new("WorkShop", WorkShopUnit.Initialize, WorkShopUnit.Finalize),
        new("AoWTCReg", AoWTCRegUnit.Initialize, AoWTCRegUnit.Finalize),
        new("AoWTutorialWin", null, null),
        new("AoWMessageDlg", null, null),
        new("AoWEmailGameSettingsDlg", null, null),
        new("AoWScenSetup", null, null),
        new("AoWSoundDlg", null, null),
        new("AoWIntroScene", null, null),
        new("AoWPowerDlg", null, null),
        new("AoWSpellBook", null, null),
        new("AoWMagic", null, null),
        new("AoWTCUnit", null, null),
        new("AoWTCScanner", null, null),
        new("AOWChatWin", null, null),
        new("OverviewUnit", OverviewUnitUnit.Initialize, OverviewUnitUnit.Finalize),
        new("AoWCityScreen", null, null),
        new("AoWInfoWin", null, null),
        new("AOWPyreScrn", null, null),
        new("AoWRelations", null, null),
        new("AoWGamePlayDlg", null, null),
        new("AoWPWindows", null, null),
        new("AoWResultsWin", null, null),
        new("AoWCampaignJoin", null, null),
        new("AoWSelFactionWin", null, null),
        new("AOWTitleWindow", null, null),
        new("AoWProgressDialog", null, null),
        new("AoWTitleScene", AoWTitleSceneUnit.Initialize, null),
        new("AoWFileDialog", null, null),
        new("AOWMWindow", null, null),
        new("AoWSWindow", null, null),
        new("AoWVideoDlg", null, null),
        new("AoWPOP3Dlg", null, null),
        new("AOWMultiWin", null, null),
        new("AoWSyncDlg", null, null),
        new("aowSetupEvents", null, null),
        new("AoWLeaderSetup", null, null),
        new("AoWCampaignSettings", null, null),
        new("AoWCampaignTransferDlg", null, null),
        new("AoWWorldMap", null, null),
        new("AoWPasswordDlg", null, null),
        new("AoWSMTPDlg", null, null),
        new("AoWSendEmailGameDlg", null, null),
        new("AoWItemsFoundDlg", null, null),
        new("AoWWizardsTowerDlg", null, null),
        new("AoWUnitBanner", null, null),
        new("AoWPartyJoin", null, null),
        new("AoWHeroInfoDlg", null, null),
        new("AoWArmyInfoDlg", null, null),
        new("AoWHeroUpgradeDlg", null, null),
        new("AoWDeformFaceDlg", null, null),
        new("AoWUnitSelDlg", null, null),
        new("AoWTargetSelection", null, null),
        new("TCFCDlg", null, null),
        new("AoWNewTurnDialog", null, null),
        new("AoWMsgReceiveDlg", null, null),
        new("AoWHeroJoinDlg", null, null),
        new("AoWCityJoinDlg", null, null),
        new("AoWCityRebelDlg", null, null),
        new("AoWArmyEventDlg", null, null),
        new("FastCombatWin", null, null),
        new("AoWDisjunctionDlg", null, null),
        new("AoWCastUnitSpellDlg", null, null),
        new("AoWConstructDlg", null, null),
        new("aowMapEvents", null, null),
        new("AOWDialogWin", null, null),
        new("AoWArenaDlg", null, null),
        new("AoWWinMan", null, null),
        new("AoWItemBanner", null, null),
        new("AoWUnitWindow", null, null),
        new("AOWControlWin", null, null),
        new("AoWInfoBanner", null, null),
        new("AoWProdScreen", null, null),
        new("AoWRealm", null, null),
        new("AoWTBWindow", null, null),
        new("aowTCMapEvents", null, null),
    ];

    private static readonly bool[] initialized = new bool[Units.Length];

    public static void InitializeAll()
    {
        InitializeSelected(null);
    }

    public static void FinalizeAll()
    {
        FinalizeSelected(null);
    }

    public static void InitializeSelected(string[] selection)
    {
        InitializeUnits(Units, initialized, selection);
    }

    public static void FinalizeSelected(string[] selection)
    {
        FinalizeUnits(Units, initialized, selection);
    }

    /// <summary>Runs the initialization of every selected unit not yet initialized, in table
    /// order. A null selection selects every unit. A unit counts as initialized once its section
    /// returns, so an exception stops the run and leaves the failing unit uninitialized.</summary>
    internal static void InitializeUnits(StartupUnit[] units, bool[] state, string[] selection)
    {
        for (var index = 0; index < units.Length; index++)
        {
            var unit = units[index];
            if ((selection == null || Array.IndexOf(selection, unit.Name) >= 0) && !state[index])
            {
                unit.Initialize?.Invoke();
                state[index] = true;
            }
        }
    }

    /// <summary>Runs the finalization of every selected initialized unit, in reverse table order.
    /// A unit counts as finalized before its section runs. As in the chain of nested finally
    /// blocks this loop replaced, where each section ran inside the finally block of the one
    /// before, an exception does not stop the run: every remaining unit is finalized and the last
    /// exception propagates at the end.</summary>
    internal static void FinalizeUnits(StartupUnit[] units, bool[] state, string[] selection)
    {
        ExceptionDispatchInfo failure = null;
        for (var index = units.Length - 1; index >= 0; index--)
        {
            var unit = units[index];
            try
            {
                if ((selection == null || Array.IndexOf(selection, unit.Name) >= 0) && state[index])
                {
                    state[index] = false;
                    unit.Finalize?.Invoke();
                }
            }
            catch (System.Exception exception)
            {
                failure = ExceptionDispatchInfo.Capture(exception);
            }
        }
        failure?.Throw();
    }
}
