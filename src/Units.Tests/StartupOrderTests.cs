using Aow.Game.Engine;
namespace Aow.Units.Tests;

/// <summary>Pins the unit table of <see cref="Startup"/>: the order of the initialization and
/// finalization sections, for the full set and for a selection, as the nested chain before the
/// table ran them.</summary>
public sealed class StartupOrderTests
{
    private const string Names = """
        GFXE HashTabU ImageLib aowFX FMemStr Engine AoWReg Sound NetworkE EList HSEngine Animation
        ILTer EventLog Pad Terrain AoWE PassiveAb CombatSpells GlobalEnchantmentSpells SmokeCloud
        GlobalTargetSpells GlobalSpells UnitSpells SummonSpells PoisonC Storms BloodShrine ExploreS
        ItemExpS AoWSetup Crops City AGCPlugins BehaviorAG MainAG IndependentAG Arena WizardsTower
        ReflectingPool MagicSanctuary OverlayObject MoveOnEvents Altar AltPest AltDeath AltLife
        AltLight FlagEvents PowerNode AltIce AltFire PrdPlace BldGuild Merchant UnHolyG HolyG
        Shipyard Pyramid Ruin Crypt Dungeon Gems PoisonP Tower MonsterL AirNode WaterNode LifeNode
        DeathNode EarthNode FireNode ManaCrystals Mine Cave Farm CitySpells Teleport Sign Spring
        Hexagon BorderHx Road Bridge AoWTer Mountain AoWRoad RndHex IsoHex AoWHex HxBridge AoWEReg
        LicenseF DisplayC AOWInterfaceImgLib AoWComp AoWWinControl AOWWinManager AoWBaseWin
        AoWWinGeneric AoWWindow AOWToolTip AoWControl AoWVScroll AOWMenuBar AOWMenu AoWButton
        AoWCheckBox FleeLoc CombatObstacle CombatILTer CityWall CombatFire CombatRoad CombatTerrain
        CombatTE CombatHex AoWTC AoWFrame AoWTabPanel AoWPanel MapCtrl TurnButton AoWEdit AoWLabel
        AoWImage AoWMemo AoWHTMLMemo aowIntGfxMod aowHelpWin AOWGauge AoWHScroll AoWListBox
        aowUserDrawBox CMapCtrl Scanner AoWComboBox PageTurnCtrl AoWBookWindow AowUserWindow
        StartMenu FontMod AoWEmailSettingsDlg AoWArnoLine AoWLenLine AoWPInfoWin CenterViewLoc
        CombatStructure MainStructure SingleCombatHex House Townhall IsoCombatHex WorkShop AoWTCReg
        AoWTutorialWin AoWMessageDlg AoWEmailGameSettingsDlg AoWScenSetup AoWSoundDlg AoWIntroScene
        AoWPowerDlg AoWSpellBook AoWMagic AoWTCUnit AoWTCScanner AOWChatWin OverviewUnit
        AoWCityScreen AoWInfoWin AOWPyreScrn AoWRelations AoWGamePlayDlg AoWPWindows AoWResultsWin
        AoWCampaignJoin AoWSelFactionWin AOWTitleWindow AoWProgressDialog AoWTitleScene
        AoWFileDialog AOWMWindow AoWSWindow AoWVideoDlg AoWPOP3Dlg AOWMultiWin AoWSyncDlg
        aowSetupEvents AoWLeaderSetup AoWCampaignSettings AoWCampaignTransferDlg AoWWorldMap
        AoWPasswordDlg AoWSMTPDlg AoWSendEmailGameDlg AoWItemsFoundDlg AoWWizardsTowerDlg
        AoWUnitBanner AoWPartyJoin AoWHeroInfoDlg AoWArmyInfoDlg AoWHeroUpgradeDlg AoWDeformFaceDlg
        AoWUnitSelDlg AoWTargetSelection TCFCDlg AoWNewTurnDialog AoWMsgReceiveDlg AoWHeroJoinDlg
        AoWCityJoinDlg AoWCityRebelDlg AoWArmyEventDlg FastCombatWin AoWDisjunctionDlg
        AoWCastUnitSpellDlg AoWConstructDlg aowMapEvents AOWDialogWin AoWArenaDlg AoWWinMan
        AoWItemBanner AoWUnitWindow AOWControlWin AoWInfoBanner AoWProdScreen AoWRealm AoWTBWindow
        aowTCMapEvents
        """;

    // The units with an initialization section, in the order they run.
    private const string Initializations = """
        GFXE ImageLib Engine AoWReg Sound Animation EventLog Pad AoWE PassiveAb CombatSpells
        GlobalEnchantmentSpells SmokeCloud GlobalTargetSpells GlobalSpells UnitSpells SummonSpells
        PoisonC Storms BloodShrine ExploreS ItemExpS AoWSetup Crops City AGCPlugins BehaviorAG
        MainAG IndependentAG Arena WizardsTower ReflectingPool MagicSanctuary OverlayObject
        MoveOnEvents Altar AltPest AltDeath AltLife AltLight FlagEvents PowerNode AltIce AltFire
        PrdPlace BldGuild Merchant UnHolyG HolyG Shipyard Pyramid Ruin Crypt Dungeon Gems PoisonP
        Tower MonsterL AirNode WaterNode LifeNode DeathNode EarthNode FireNode ManaCrystals Mine
        Cave Farm CitySpells Teleport Sign Spring AoWTer Mountain AoWRoad AoWHex HxBridge AoWEReg
        LicenseF FleeLoc CombatObstacle CombatILTer CityWall CombatFire CombatRoad CombatTerrain
        CombatTE CombatHex AoWTC MapCtrl TurnButton CMapCtrl Scanner PageTurnCtrl AoWBookWindow
        AowUserWindow StartMenu AoWArnoLine AoWLenLine CenterViewLoc CombatStructure MainStructure
        SingleCombatHex House Townhall IsoCombatHex WorkShop AoWTCReg OverviewUnit AoWTitleScene
        """;

    // The units with a finalization section, in the order they run.
    private const string Finalizations = """
        OverviewUnit AoWTCReg WorkShop IsoCombatHex Townhall House SingleCombatHex MainStructure
        CombatStructure CenterViewLoc AoWLenLine AoWArnoLine StartMenu AowUserWindow AoWBookWindow
        PageTurnCtrl Scanner CMapCtrl TurnButton MapCtrl AoWTC CombatHex CombatTE CombatTerrain
        CombatRoad CombatFire CityWall CombatILTer CombatObstacle FleeLoc LicenseF AoWEReg HxBridge
        AoWHex AoWRoad Mountain AoWTer Spring Sign Teleport CitySpells Farm Cave Mine ManaCrystals
        FireNode EarthNode DeathNode LifeNode WaterNode AirNode MonsterL Tower PoisonP Gems Dungeon
        Crypt Ruin Pyramid Shipyard HolyG UnHolyG Merchant BldGuild PrdPlace AltFire AltIce
        PowerNode FlagEvents AltLight AltLife AltDeath AltPest Altar MoveOnEvents OverlayObject
        MagicSanctuary ReflectingPool WizardsTower Arena IndependentAG MainAG BehaviorAG AGCPlugins
        City Crops AoWSetup ItemExpS ExploreS BloodShrine Storms PoisonC SummonSpells UnitSpells
        GlobalSpells GlobalTargetSpells SmokeCloud GlobalEnchantmentSpells CombatSpells PassiveAb
        AoWE Pad EventLog Animation Sound AoWReg Engine ImageLib GFXE
        """;

    private static string[] Split(string text) => text.Split((char[])[' ', '\n'], StringSplitOptions.RemoveEmptyEntries);

    private static string Section(Action section, string unit, string name)
    {
        Assert.Equal(name, section.Method.Name);
        var type = section.Method.DeclaringType!;
        Assert.Equal(unit + "Unit", type.Name);
        Assert.StartsWith("Aow.Game.", type.Namespace ?? "");
        return unit;
    }

    private static (StartupUnit[] Units, List<string> Calls) Recording()
    {
        var calls = new List<string>();
        var units = Startup.Units.Select(u => new StartupUnit(u.Name,
            u.Initialize is null ? null! : () => calls.Add("I " + Section(u.Initialize, u.Name, "Initialize")),
            u.Finalize is null ? null! : () => calls.Add("F " + Section(u.Finalize, u.Name, "Finalize")))).ToArray();
        return (units, calls);
    }

    [Fact]
    public void TableKeepsTheUnitOrder()
    {
        Assert.Equal(Split(Names), Startup.Units.Select(u => u.Name));
    }

    [Fact]
    public void FullSetInitializesInOrderAndFinalizesInReverse()
    {
        var (units, calls) = Recording();
        var state = new bool[units.Length];
        Startup.InitializeUnits(units, state, null!);
        Assert.Equal(Split(Initializations).Select(n => "I " + n), calls);
        Assert.All(state, Assert.True);
        calls.Clear();
        Startup.InitializeUnits(units, state, null!);
        Assert.Empty(calls);
        Startup.FinalizeUnits(units, state, null!);
        Assert.Equal(Split(Finalizations).Select(n => "F " + n), calls);
        Assert.All(state, Assert.False);
    }

    [Fact]
    public void SelectionLimitsInitializationAndFinalization()
    {
        var (units, calls) = Recording();
        var state = new bool[units.Length];
        Startup.InitializeUnits(units, state, ["AoWTitleScene", "HashTabU", "Engine", "ImageLib", "AoWTC", "Sound"]);
        Assert.Equal(["I ImageLib", "I Engine", "I Sound", "I AoWTC", "I AoWTitleScene"], calls);
        calls.Clear();
        // Units outside the finalization selection stay initialized.
        Startup.FinalizeUnits(units, state, ["Engine", "AoWTitleScene", "AoWTC", "GFXE"]);
        Assert.Equal(["F AoWTC", "F Engine"], calls);
        calls.Clear();
        // Only initialized units are finalized: GFXE was never initialized.
        Startup.FinalizeUnits(units, state, null!);
        Assert.Equal(["F Sound", "F ImageLib"], calls);
        Assert.All(state, Assert.False);
    }

    [Fact]
    public void FailingInitializationStopsTheRun()
    {
        var calls = new List<string>();
        StartupUnit[] units = [new("A", () => calls.Add("A"), null!), new("B", () => throw new InvalidOperationException("B"), null!),
            new("C", () => calls.Add("C"), null!)];
        var state = new bool[units.Length];
        Assert.Throws<InvalidOperationException>(() => Startup.InitializeUnits(units, state, null!));
        Assert.Equal(["A"], calls);
        Assert.Equal([true, false, false], state);
    }

    [Fact]
    public void FailingFinalizationFinalizesTheRestAndRethrowsTheLastFailure()
    {
        var calls = new List<string>();
        StartupUnit[] units = [new("A", null!, () => throw new InvalidOperationException("A")), new("B", null!, () => calls.Add("B")),
            new("C", null!, () => throw new InvalidOperationException("C")), new("D", null!, () => calls.Add("D"))];
        bool[] state = [true, true, true, true];
        var failure = Assert.Throws<InvalidOperationException>(() => Startup.FinalizeUnits(units, state, null!));
        Assert.Equal("A", failure.Message);
        Assert.Equal(["D", "B"], calls);
        Assert.All(state, Assert.False);
    }
}
