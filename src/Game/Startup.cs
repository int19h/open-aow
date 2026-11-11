// Translated from the Delphi units AGCPlugins, AOWChatWin, AOWControlWin, AOWDialogWin, AOWGauge, AOWInterfaceImgLib, AOWMWindow, Menu, MenuBar, AOWMultiWin, AOWPyreScrn, AOWTitleWindow, ToolTip, WindowManager, AirNode, AlgnmBox, AltDeath, AltFire, AltIce, AltLife, AltLight, AltPest, Altar, Animation, AoWArenaDlg, AoWArmyEventDlg, AoWArmyInfoDlg, ArnoLine, AoWBaseWin, BookWindow, AoWButton, AoWCampaignJoin, AoWCampaignSettings, AoWCampaignTransferDlg, AoWCastUnitSpellDlg, AoWCheckBox, AoWCityJoinDlg, AoWCityRebelDlg, AoWCityScreen, AoWComboBox, AoWComp, AoWConstructDlg, AoWControl, AoWDeformFaceDlg, AoWDisjunctionDlg, AoWE, AoWEReg, AoWEdit, AoWEmailGameSettingsDlg, AoWEmailSettingsDlg, AoWExeForms, AoWFileDialog, Frame, AoWGamePlayDlg, AoWHScroll, AoWHTMLMemo, AoWHeroInfoDlg, AoWHeroJoinDlg, AoWHeroUpgradeDlg, AoWHex, AoWImage, AoWInfoBanner, AoWInfoWin, AoWIntroScene, AoWItemBanner, AoWItemsFoundDlg, AoWLabel, AoWLeaderSetup, AoWLenLine, AoWListBox, AoWMagic, AoWMemo, AoWMessageDlg, AoWMsgReceiveDlg, AoWNewTurnDialog, AoWPInfoWin, AoWPOP3Dlg, AoWPWindows, AoWPanel, AoWPartyJoin, AoWPasswordDlg, AoWPowerDlg, AoWProdScreen, AoWProgressDialog, AoWRealm, AoWReg, AoWRelations, AoWResultsWin, AoWRoad, AoWSMTPDlg, AoWSWindow, AoWScenSetup, AoWSelFactionWin, AoWSendEmailGameDlg, AoWSetup, AoWSoundDlg, AoWSpellBook, AoWSyncDlg, AoWTBWindow, AoWTC, AoWTCReg, AoWTCScanner, AoWTCUnit, AoWTabPanel, AoWTargetSelection, AoWTer, AoWTitleScene, AoWTutorialWin, AoWUnitBanner, AoWUnitSelDlg, AoWUnitWindow, AoWVScroll, AoWVideoDlg, AoWWinControl, AoWWinGeneric, AoWWinMan, AoWWindow, AoWWizardsTowerDlg, AoWWorldMap, AowUserWindow, Arena, BehaviorAG, BldGuild, BloodShrine, BorderHx, Bridge, CMapCtrl, Cave, CenterViewLoc, City, CitySpells, CityWall, CombatFire, CombatHex, CombatILTer, CombatObstacle, CombatRoad, CombatSpells, CombatStructure, CombatTE, CombatTerrain, Crops, Crypt, DeathNode, DisplayC, Dungeon, E1GameFlow, E1Globals, E1Resources, E1TacticalSupport, E2CampaignSupport, E2DirectSound, E2GameFlow, E2Globals, E2WindowLayout, E3EventDialogs, E3Globals, E3IMSTypes, E3StructureInfo, EList, EarthNode, Engine, EventLog, ExploreS, FMemStr, Farm, FastCombatWin, FileSecurity, FireNode, FlagEvents, FleeLoc, FontMod, GFXE, Gems, GlobalEnchantmentSpells, GlobalSpells, GlobalTargetSpells, HSEngine, HSLib, HashTabU, HeroCtrl, HeroGrid, Hexagon, Hill, HolyG, House, HxBridge, ILTer, ImageLib, IndependentAG, IsoCombatHex, IsoHex, ItemCtrl, ItemExpS, ItemGrid, LeadrGrd, LicenseF, LifeNode, MagicSanctuary, MainAG, MainStructure, ManaCrystals, MapCtrl, Merchant, Mine, MonsterL, Mountain, MoveOnEvents, NetworkE, OverlayObject, OverviewUnit, Pad, PageTurnCtrl, PassiveAb, PlayerBox, PlugInM, PoisonC, PoisonP, PowerNode, PrdPlace, Pyramid, RaceBox, RandomNode, ReflectingPool, RndHex, Road, Ruin, Scanner, Shipyard, Sign, SingleCombatHex, SmokeCloud, Sound, Spring, StartMenu, Storms, SummonSpells, TCFCDlg, Teleport, Terrain, Tower, Townhall, TurnButton, UnHolyG, UnitGrid, UnitIDlg, UnitIGrd, UnitSpells, UnitTabC, WaterNode, WizardsTower, WorkShop, aowFX, aowHelpWin, aowIntGfxMod, aowMapEvents, aowSetupEvents, aowTCMapEvents, aowUserDrawBox.
using System.Runtime.ExceptionServices;
using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Audio;

using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Dialogs;
using Aow.Game.World.Structures;

namespace Aow.Game;

public static class Startup
{
    // The units in the order the Delphi program initializes them. Finalization runs the table
    // backwards.
    internal static readonly StartupUnit[] Units =
    [
        new("GFXE", GraphicsState.Initialize, GraphicsState.Finalize),
        new("ImageLib", ImageLibraries.Initialize, ImageLibraries.Finalize),
        new("Engine", EngineHost.Initialize, EngineHost.Finalize),
        new("Sound", AudioHost.Initialize, AudioHost.Finalize),
        new("AoWE", GameTables.Initialize, GameTables.Finalize),
        new("City", CityRegistrar.SubscribeMapLoaded, CityRegistrar.UnsubscribeMapLoaded),
        new("BehaviorAG", BehaviorRegistrar.SubscribeMapLoaded, BehaviorRegistrar.UnsubscribeMapLoaded),
        new("MainAG", MainRegistrar.SubscribeMapLoaded, MainRegistrar.UnsubscribeMapLoaded),
        new("IndependentAG", IndependentRegistrar.SubscribeMapLoaded, IndependentRegistrar.UnsubscribeMapLoaded),
        new("AoWEReg", GameClasses.Register, null),
        new("AoWTitleScene", TitleCaptions.Initialize, null),
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
            catch (Exception exception)
            {
                failure = ExceptionDispatchInfo.Capture(exception);
            }
        }
        failure?.Throw();
    }
}
