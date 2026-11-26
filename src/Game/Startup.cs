#nullable disable warnings
// Translated from the Delphi units AGCPlugins, AOWChatWin, AOWControlWin, AOWDialogWin, AOWGauge, AOWInterfaceImgLib, AOWMWindow, Menu, MenuBar, AOWMultiWin, AOWPyreScrn, AOWTitleWindow, ToolTip, WindowManager, AirNode, AlgnmBox, AltDeath, AltFire, AltIce, AltLife, AltLight, AltPest, Altar, Animation, AoWArenaDlg, AoWArmyEventDlg, AoWArmyInfoDlg, ArnoLine, AoWBaseWin, BookWindow, AoWButton, AoWCampaignJoin, AoWCampaignSettings, AoWCampaignTransferDlg, AoWCastUnitSpellDlg, AoWCheckBox, AoWCityJoinDlg, AoWCityRebelDlg, AoWCityScreen, AoWComboBox, AoWComp, AoWConstructDlg, AoWControl, AoWDeformFaceDlg, AoWDisjunctionDlg, AoWE, AoWEReg, AoWEdit, AoWEmailGameSettingsDlg, AoWEmailSettingsDlg, AoWExeForms, AoWFileDialog, Frame, AoWGamePlayDlg, AoWHScroll, AoWHTMLMemo, AoWHeroInfoDlg, AoWHeroJoinDlg, AoWHeroUpgradeDlg, AoWHex, AoWImage, AoWInfoBanner, AoWInfoWin, AoWIntroScene, AoWItemBanner, AoWItemsFoundDlg, AoWLabel, AoWLeaderSetup, AoWLenLine, AoWListBox, AoWMagic, AoWMemo, AoWMessageDlg, AoWMsgReceiveDlg, AoWNewTurnDialog, AoWPInfoWin, AoWPOP3Dlg, AoWPWindows, AoWPanel, AoWPartyJoin, AoWPasswordDlg, AoWPowerDlg, AoWProdScreen, AoWProgressDialog, AoWRealm, AoWReg, AoWRelations, AoWResultsWin, AoWRoad, AoWSMTPDlg, AoWSWindow, AoWScenSetup, AoWSelFactionWin, AoWSendEmailGameDlg, AoWSetup, AoWSoundDlg, AoWSpellBook, AoWSyncDlg, AoWTBWindow, AoWTC, AoWTCReg, AoWTCScanner, AoWTCUnit, AoWTabPanel, AoWTargetSelection, AoWTer, AoWTitleScene, AoWTutorialWin, AoWUnitBanner, AoWUnitSelDlg, AoWUnitWindow, AoWVScroll, AoWVideoDlg, AoWWinControl, AoWWinGeneric, AoWWinMan, AoWWindow, AoWWizardsTowerDlg, AoWWorldMap, AowUserWindow, Arena, BehaviorAG, BldGuild, BloodShrine, BorderHx, Bridge, CMapCtrl, Cave, CenterViewLoc, City, CitySpells, CityWall, CombatFire, CombatHex, CombatILTer, CombatObstacle, CombatRoad, CombatSpells, CombatStructure, CombatTE, CombatTerrain, Crops, Crypt, DeathNode, DisplayC, Dungeon, E1GameFlow, E1Globals, E1Resources, E1TacticalSupport, E2CampaignSupport, E2DirectSound, E2GameFlow, E2Globals, E2WindowLayout, E3EventDialogs, E3Globals, E3IMSTypes, E3StructureInfo, EList, EarthNode, Engine, EventLog, ExploreS, FMemStr, Farm, FastCombatWin, FileSecurity, FireNode, FlagEvents, FleeLoc, FontMod, GFXE, Gems, GlobalEnchantmentSpells, GlobalSpells, GlobalTargetSpells, HSEngine, HSLib, HashTabU, HeroCtrl, HeroGrid, Hexagon, Hill, HolyG, House, HxBridge, ILTer, ImageLib, IndependentAG, IsoCombatHex, IsoHex, ItemCtrl, ItemExpS, ItemGrid, LeadrGrd, LicenseF, LifeNode, MagicSanctuary, MainAG, MainStructure, ManaCrystals, MapCtrl, Merchant, Mine, MonsterL, Mountain, MoveOnEvents, NetworkE, OverlayObject, OverviewUnit, Pad, PageTurnCtrl, PassiveAb, PlayerBox, PlugInM, PoisonC, PoisonP, PowerNode, PrdPlace, Pyramid, RaceBox, RandomNode, ReflectingPool, RndHex, Road, Ruin, Scanner, Shipyard, Sign, SingleCombatHex, SmokeCloud, Sound, Spring, StartMenu, Storms, SummonSpells, TCFCDlg, Teleport, Terrain, Tower, Townhall, TurnButton, UnHolyG, UnitGrid, UnitIDlg, UnitIGrd, UnitSpells, UnitTabC, WaterNode, WizardsTower, WorkShop, aowFX, aowHelpWin, aowIntGfxMod, aowMapEvents, aowSetupEvents, aowTCMapEvents, aowUserDrawBox.
using System.Runtime.ExceptionServices;
using Aow.Game.AI.Groups;
using Aow.Game.Audio;

using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Session;
using Aow.Game.World.Structures;

namespace Aow.Game;

public static class Startup
{
    // The units in the order the Delphi program initializes them. Finalization runs the table
    // backwards.
    internal static StartupUnit[] Units { get; } = [
        new("GFXE", GraphicsState.Initialize, GraphicsState.Finalize),
        new("ImageLib", ImageLibraries.Initialize, ImageLibraries.Finalize),
        new("Sound", AudioHost.Initialize, AudioHost.Finalize),
    ];

    private static readonly bool[] s_initialized = new bool[Units.Length];

    public static void InitializeAll() => InitializeSelected(null);

    /// <summary>Creates the game engine the host owns, after the units. The engine registers the
    /// map-load handlers of the city and AI units and then the game's classes. The Delphi units
    /// City, BehaviorAG, MainAG and IndependentAG added their handlers in that order after AoWE
    /// created the engine, and the engine's list ran them last to first. HexSpriteMapLoaded runs
    /// its handlers first to last (issue 326, decision 4), so they subscribe in the reverse order
    /// and still run IndependentAG first and City last.</summary>
    public static GameEngine CreateEngine(IPlayerInput playerInput)
    {
        GameEngine engine = GameEngine.Create(null, playerInput: playerInput);
        IndependentRegistrar.SubscribeMapLoaded(engine);
        MainRegistrar.SubscribeMapLoaded(engine);
        BehaviorRegistrar.SubscribeMapLoaded(engine);
        CityRegistrar.SubscribeMapLoaded(engine);
        GameClasses.Register(engine);
        return engine;
    }

    public static void InitializeSelected(string[]? selection) => InitializeUnits(Units, state: s_initialized, selection);

    public static void FinalizeSelected(string[] selection) => FinalizeUnits(Units, state: s_initialized, selection);

    /// <summary>Runs the initialization of every selected unit not yet initialized, in table
    /// order. A null selection selects every unit. A unit counts as initialized once its section
    /// returns, so an exception stops the run and leaves the failing unit uninitialized.</summary>
    internal static void InitializeUnits(StartupUnit[] units, bool[] state, string[]? selection)
    {
        for (int index = 0; index < units.Length; index++)
        {
            StartupUnit unit = units[index];
            if ((selection is null || Array.IndexOf(selection, value: unit.Name) >= 0) && !state[index])
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
    internal static void FinalizeUnits(StartupUnit[] units, bool[] state, string[]? selection)
    {
        ExceptionDispatchInfo? failure = null;
        for (int index = units.Length - 1; index >= 0; index--)
        {
            StartupUnit unit = units[index];
            try
            {
                if ((selection is null || Array.IndexOf(selection, value: unit.Name) >= 0) && state[index])
                {
                    state[index] = false;
                    unit.Finalize?.Invoke();
                }
            }
            catch (Exception exception)
            {
                failure = ExceptionDispatchInfo.Capture(exception);
            }
        }
        failure?.Throw();
    }
}
