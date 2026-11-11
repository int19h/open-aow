// Translated from the Delphi units AGCPlugins, AOWChatWin, AOWControlWin, AOWDialogWin, AOWGauge, AOWInterfaceImgLib, AOWMWindow, Menu, MenuBar, AOWMultiWin, AOWPyreScrn, AOWTitleWindow, ToolTip, WindowManager, AirNode, AlgnmBox, AltDeath, AltFire, AltIce, AltLife, AltLight, AltPest, Altar, Animation, AoWArenaDlg, AoWArmyEventDlg, AoWArmyInfoDlg, ArnoLine, AoWBaseWin, BookWindow, AoWButton, AoWCampaignJoin, AoWCampaignSettings, AoWCampaignTransferDlg, AoWCastUnitSpellDlg, AoWCheckBox, AoWCityJoinDlg, AoWCityRebelDlg, AoWCityScreen, AoWComboBox, AoWComp, AoWConstructDlg, AoWControl, AoWDeformFaceDlg, AoWDisjunctionDlg, AoWE, AoWEReg, AoWEdit, AoWEmailGameSettingsDlg, AoWEmailSettingsDlg, AoWExeForms, AoWFileDialog, Frame, AoWGamePlayDlg, AoWHScroll, AoWHTMLMemo, AoWHeroInfoDlg, AoWHeroJoinDlg, AoWHeroUpgradeDlg, AoWHex, AoWImage, AoWInfoBanner, AoWInfoWin, AoWIntroScene, AoWItemBanner, AoWItemsFoundDlg, AoWLabel, AoWLeaderSetup, AoWLenLine, AoWListBox, AoWMagic, AoWMemo, AoWMessageDlg, AoWMsgReceiveDlg, AoWNewTurnDialog, AoWPInfoWin, AoWPOP3Dlg, AoWPWindows, AoWPanel, AoWPartyJoin, AoWPasswordDlg, AoWPowerDlg, AoWProdScreen, AoWProgressDialog, AoWRealm, AoWReg, AoWRelations, AoWResultsWin, AoWRoad, AoWSMTPDlg, AoWSWindow, AoWScenSetup, AoWSelFactionWin, AoWSendEmailGameDlg, AoWSetup, AoWSoundDlg, AoWSpellBook, AoWSyncDlg, AoWTBWindow, AoWTC, AoWTCReg, AoWTCScanner, AoWTCUnit, AoWTabPanel, AoWTargetSelection, AoWTer, AoWTitleScene, AoWTutorialWin, AoWUnitBanner, AoWUnitSelDlg, AoWUnitWindow, AoWVScroll, AoWVideoDlg, AoWWinControl, AoWWinGeneric, AoWWinMan, AoWWindow, AoWWizardsTowerDlg, AoWWorldMap, AowUserWindow, Arena, BehaviorAG, BldGuild, BloodShrine, BorderHx, Bridge, CMapCtrl, Cave, CenterViewLoc, City, CitySpells, CityWall, CombatFire, CombatHex, CombatILTer, CombatObstacle, CombatRoad, CombatSpells, CombatStructure, CombatTE, CombatTerrain, Crops, Crypt, DeathNode, DisplayC, Dungeon, E1GameFlow, E1Globals, E1Resources, E1TacticalSupport, E2CampaignSupport, E2DirectSound, E2GameFlow, E2Globals, E2WindowLayout, E3EventDialogs, E3Globals, E3IMSTypes, E3StructureInfo, EList, EarthNode, Engine, EventLog, ExploreS, FMemStr, Farm, FastCombatWin, FileSecurity, FireNode, FlagEvents, FleeLoc, FontMod, GFXE, Gems, GlobalEnchantmentSpells, GlobalSpells, GlobalTargetSpells, HSEngine, HSLib, HashTabU, HeroCtrl, HeroGrid, Hexagon, Hill, HolyG, House, HxBridge, ILTer, ImageLib, IndependentAG, IsoCombatHex, IsoHex, ItemCtrl, ItemExpS, ItemGrid, LeadrGrd, LicenseF, LifeNode, MagicSanctuary, MainAG, MainStructure, ManaCrystals, MapCtrl, Merchant, Mine, MonsterL, Mountain, MoveOnEvents, NetworkE, OverlayObject, OverviewUnit, Pad, PageTurnCtrl, PassiveAb, PlayerBox, PlugInM, PoisonC, PoisonP, PowerNode, PrdPlace, Pyramid, RaceBox, RandomNode, ReflectingPool, RndHex, Road, Ruin, Scanner, Shipyard, Sign, SingleCombatHex, SmokeCloud, Sound, Spring, StartMenu, Storms, SummonSpells, TCFCDlg, Teleport, Terrain, Tower, Townhall, TurnButton, UnHolyG, UnitGrid, UnitIDlg, UnitIGrd, UnitSpells, UnitTabC, WaterNode, WizardsTower, WorkShop, aowFX, aowHelpWin, aowIntGfxMod, aowMapEvents, aowSetupEvents, aowTCMapEvents, aowUserDrawBox.
using System.Runtime.CompilerServices;

namespace Aow.Game.World.Map;

[InlineArray(2)]
public struct XYTableEntryArray8By2
{
    private XYTableEntryArray8 element0;

    public static XYTableEntryArray8By2 From(ReadOnlySpan<XYTableEntryArray8> values)
    {
        XYTableEntryArray8By2 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}
