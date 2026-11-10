// Translated from the Delphi units AGCPlugins, AOWChatWin, AOWControlWin, AOWDialogWin, AOWGauge, AOWInterfaceImgLib, AOWMWindow, AOWMenu, AOWMenuBar, AOWMultiWin, AOWPyreScrn, AOWTitleWindow, AOWToolTip, AOWWinManager, AirNode, AlgnmBox, AltDeath, AltFire, AltIce, AltLife, AltLight, AltPest, Altar, Animation, AoWArenaDlg, AoWArmyEventDlg, AoWArmyInfoDlg, AoWArnoLine, AoWBaseWin, AoWBookWindow, AoWButton, AoWCampaignJoin, AoWCampaignSettings, AoWCampaignTransferDlg, AoWCastUnitSpellDlg, AoWCheckBox, AoWCityJoinDlg, AoWCityRebelDlg, AoWCityScreen, AoWComboBox, AoWComp, AoWConstructDlg, AoWControl, AoWDeformFaceDlg, AoWDisjunctionDlg, AoWE, AoWEReg, AoWEdit, AoWEmailGameSettingsDlg, AoWEmailSettingsDlg, AoWExeForms, AoWFileDialog, AoWFrame, AoWGamePlayDlg, AoWHScroll, AoWHTMLMemo, AoWHeroInfoDlg, AoWHeroJoinDlg, AoWHeroUpgradeDlg, AoWHex, AoWImage, AoWInfoBanner, AoWInfoWin, AoWIntroScene, AoWItemBanner, AoWItemsFoundDlg, AoWLabel, AoWLeaderSetup, AoWLenLine, AoWListBox, AoWMagic, AoWMemo, AoWMessageDlg, AoWMsgReceiveDlg, AoWNewTurnDialog, AoWPInfoWin, AoWPOP3Dlg, AoWPWindows, AoWPanel, AoWPartyJoin, AoWPasswordDlg, AoWPowerDlg, AoWProdScreen, AoWProgressDialog, AoWRealm, AoWReg, AoWRelations, AoWResultsWin, AoWRoad, AoWSMTPDlg, AoWSWindow, AoWScenSetup, AoWSelFactionWin, AoWSendEmailGameDlg, AoWSetup, AoWSoundDlg, AoWSpellBook, AoWSyncDlg, AoWTBWindow, AoWTC, AoWTCReg, AoWTCScanner, AoWTCUnit, AoWTabPanel, AoWTargetSelection, AoWTer, AoWTitleScene, AoWTutorialWin, AoWUnitBanner, AoWUnitSelDlg, AoWUnitWindow, AoWVScroll, AoWVideoDlg, AoWWinControl, AoWWinGeneric, AoWWinMan, AoWWindow, AoWWizardsTowerDlg, AoWWorldMap, AowUserWindow, Arena, BehaviorAG, BldGuild, BloodShrine, BorderHx, Bridge, CMapCtrl, Cave, CenterViewLoc, City, CitySpells, CityWall, CombatFire, CombatHex, CombatILTer, CombatObstacle, CombatRoad, CombatSpells, CombatStructure, CombatTE, CombatTerrain, Crops, Crypt, DeathNode, DisplayC, Dungeon, E1GameFlow, E1Globals, E1Resources, E1TacticalSupport, E2CampaignSupport, E2DirectSound, E2GameFlow, E2Globals, E2WindowLayout, E3EventDialogs, E3Globals, E3IMSTypes, E3StructureInfo, EList, EarthNode, Engine, EventLog, ExploreS, FMemStr, Farm, FastCombatWin, FileSecurity, FireNode, FlagEvents, FleeLoc, FontMod, GFXE, Gems, GlobalEnchantmentSpells, GlobalSpells, GlobalTargetSpells, HSEngine, HSLib, HashTabU, HeroCtrl, HeroGrid, Hexagon, Hill, HolyG, House, HxBridge, ILTer, ImageLib, IndependentAG, IsoCombatHex, IsoHex, ItemCtrl, ItemExpS, ItemGrid, LeadrGrd, LicenseF, LifeNode, MagicSanctuary, MainAG, MainStructure, ManaCrystals, MapCtrl, Merchant, Mine, MonsterL, Mountain, MoveOnEvents, NetworkE, OverlayObject, OverviewUnit, Pad, PageTurnCtrl, PassiveAb, PlayerBox, PlugInM, PoisonC, PoisonP, PowerNode, PrdPlace, Pyramid, RaceBox, RandomNode, ReflectingPool, RndHex, Road, Ruin, Scanner, Shipyard, Sign, SingleCombatHex, SmokeCloud, Sound, Spring, StartMenu, Storms, SummonSpells, TCFCDlg, Teleport, Terrain, Tower, Townhall, TurnButton, UnHolyG, UnitGrid, UnitIDlg, UnitIGrd, UnitSpells, UnitTabC, WaterNode, WizardsTower, WorkShop, aowFX, aowHelpWin, aowIntGfxMod, aowMapEvents, aowSetupEvents, aowTCMapEvents, aowUserDrawBox.
using System.Runtime.CompilerServices;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Engine.Buffers;

[InlineArray(10)]
public struct FixedArray_string_10
{
    private string element0;

    public static FixedArray_string_10 From(ReadOnlySpan<string> values)
    {
        FixedArray_string_10 result = default;
        ((Span<string>)result).Fill("");
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(13)]
public struct FixedArray_string_13
{
    private string element0;

    public static FixedArray_string_13 From(ReadOnlySpan<string> values)
    {
        FixedArray_string_13 result = default;
        ((Span<string>)result).Fill("");
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(19)]
public struct FixedArray_string_19
{
    private string element0;

    public static FixedArray_string_19 From(ReadOnlySpan<string> values)
    {
        FixedArray_string_19 result = default;
        ((Span<string>)result).Fill("");
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(2)]
public struct FixedArray_string_2
{
    private string element0;

    public static FixedArray_string_2 From(ReadOnlySpan<string> values)
    {
        FixedArray_string_2 result = default;
        ((Span<string>)result).Fill("");
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(3)]
public struct FixedArray_string_3
{
    private string element0;

    public static FixedArray_string_3 From(ReadOnlySpan<string> values)
    {
        FixedArray_string_3 result = default;
        ((Span<string>)result).Fill("");
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(4)]
public struct FixedArray_string_4
{
    private string element0;

    public static FixedArray_string_4 From(ReadOnlySpan<string> values)
    {
        FixedArray_string_4 result = default;
        ((Span<string>)result).Fill("");
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(5)]
public struct FixedArray_string_5
{
    private string element0;

    public static FixedArray_string_5 From(ReadOnlySpan<string> values)
    {
        FixedArray_string_5 result = default;
        ((Span<string>)result).Fill("");
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(6)]
public struct FixedArray_string_6
{
    private string element0;

    public static FixedArray_string_6 From(ReadOnlySpan<string> values)
    {
        FixedArray_string_6 result = default;
        ((Span<string>)result).Fill("");
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(7)]
public struct FixedArray_string_7
{
    private string element0;

    public static FixedArray_string_7 From(ReadOnlySpan<string> values)
    {
        FixedArray_string_7 result = default;
        ((Span<string>)result).Fill("");
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(8)]
public struct FixedArray_string_8
{
    private string element0;

    public static FixedArray_string_8 From(ReadOnlySpan<string> values)
    {
        FixedArray_string_8 result = default;
        ((Span<string>)result).Fill("");
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(9)]
public struct FixedArray_string_9
{
    private string element0;

    public static FixedArray_string_9 From(ReadOnlySpan<string> values)
    {
        FixedArray_string_9 result = default;
        ((Span<string>)result).Fill("");
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(10)]
public struct FixedArray_Aow_Rtl_TList_10
{
    private TList element0;
}

[InlineArray(286)]
public struct FixedArray_Aow_Rtl_TList_286
{
    private TList element0;
}

[InlineArray(256)]
public struct FixedArray_Aow_Rtl_TPaletteEntry_256
{
    private Rtl.TPaletteEntry element0;
}

[InlineArray(256)]
public struct FixedArray_Aow_Rtl_TRGBQuad_256
{
    private TRGBQuad element0;
}

[InlineArray(3)]
public struct FixedArray_Aow_Units_AoWE_TAbilityOwner_3
{
    private TAbilityOwner element0;
}

[InlineArray(5)]
public struct FixedArray_Aow_Units_AoWE_TAbilityTypeList_5
{
    private TAbilityTypeList element0;
}

[InlineArray(1)]
public struct FixedArray_Aow_Units_AoWE_TAbility_1
{
    private TAbility element0;
}

[InlineArray(2)]
public struct FixedArray_Aow_Units_AoWE_TCombatPredictorSide_2
{
    private TCombatPredictorSide element0;
}

[InlineArray(2)]
public struct FixedArray_Aow_Units_AoWE_TCombatSide_2
{
    private TCombatSide element0;
}

[InlineArray(7)]
public struct FixedArray_Aow_Units_AoWE_TDamageType_7
{
    private TDamageType element0;

    public static FixedArray_Aow_Units_AoWE_TDamageType_7 From(ReadOnlySpan<TDamageType> values)
    {
        FixedArray_Aow_Units_AoWE_TDamageType_7 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(10)]
public struct FixedArray_Aow_Units_AoWE_TGroupBehavior_10
{
    private TGroupBehavior element0;

    public static FixedArray_Aow_Units_AoWE_TGroupBehavior_10 From(ReadOnlySpan<TGroupBehavior> values)
    {
        FixedArray_Aow_Units_AoWE_TGroupBehavior_10 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(7)]
public struct FixedArray_Aow_Units_AoWE_THeroItemPosition_7
{
    private THeroItemPosition element0;

    public static FixedArray_Aow_Units_AoWE_THeroItemPosition_7 From(ReadOnlySpan<THeroItemPosition> values)
    {
        FixedArray_Aow_Units_AoWE_THeroItemPosition_7 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(6)]
public struct FixedArray_Aow_Units_AoWE_TItemType_6
{
    private TItemType element0;

    public static FixedArray_Aow_Units_AoWE_TItemType_6 From(ReadOnlySpan<TItemType> values)
    {
        FixedArray_Aow_Units_AoWE_TItemType_6 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(6)]
public struct FixedArray_Aow_Units_AoWE_TP2HeroLevel_6
{
    private TP2HeroLevel element0;

    public static FixedArray_Aow_Units_AoWE_TP2HeroLevel_6 From(ReadOnlySpan<TP2HeroLevel> values)
    {
        FixedArray_Aow_Units_AoWE_TP2HeroLevel_6 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(12)]
public struct FixedArray_Aow_Units_AoWE_TP2RaceTerrain_12
{
    private TP2RaceTerrain element0;

    public static FixedArray_Aow_Units_AoWE_TP2RaceTerrain_12 From(ReadOnlySpan<TP2RaceTerrain> values)
    {
        FixedArray_Aow_Units_AoWE_TP2RaceTerrain_12 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(12)]
public struct FixedArray_Aow_Units_AoWE_TRaceTerrainInfo_12
{
    private TRaceTerrainInfo element0;

    public static FixedArray_Aow_Units_AoWE_TRaceTerrainInfo_12 From(ReadOnlySpan<TRaceTerrainInfo> values)
    {
        FixedArray_Aow_Units_AoWE_TRaceTerrainInfo_12 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(12)]
public struct FixedArray_Aow_Units_AoWE_TStatus_12
{
    private Aow.Game.Engine.TStatus element0;

    public static FixedArray_Aow_Units_AoWE_TStatus_12 From(ReadOnlySpan<Aow.Game.Engine.TStatus> values)
    {
        FixedArray_Aow_Units_AoWE_TStatus_12 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(131)]
public struct FixedArray_Aow_Units_AoWTC_TfxType_131
{
    private TfxType element0;

    public static FixedArray_Aow_Units_AoWTC_TfxType_131 From(ReadOnlySpan<TfxType> values)
    {
        FixedArray_Aow_Units_AoWTC_TfxType_131 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(170)]
public struct FixedArray_Aow_Units_AoWTC_TfxType_170
{
    private TfxType element0;

    public static FixedArray_Aow_Units_AoWTC_TfxType_170 From(ReadOnlySpan<TfxType> values)
    {
        FixedArray_Aow_Units_AoWTC_TfxType_170 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(256)]
public struct FixedArray_Aow_Units_Engine_TIntegerHashList_256
{
    private TIntegerHashList element0;
}

[InlineArray(256)]
public struct FixedArray_Aow_Units_Engine_TStringHashList_256
{
    private TStringHashList element0;
}

[InlineArray(4)]
public struct FixedArray_Aow_Units_FixedArray_Aow_Units_AoWE_TGroupBehavior_10_4
{
    private FixedArray_Aow_Units_AoWE_TGroupBehavior_10 element0;

    public static FixedArray_Aow_Units_FixedArray_Aow_Units_AoWE_TGroupBehavior_10_4 From(ReadOnlySpan<FixedArray_Aow_Units_AoWE_TGroupBehavior_10> values)
    {
        FixedArray_Aow_Units_FixedArray_Aow_Units_AoWE_TGroupBehavior_10_4 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(5)]
public struct FixedArray_Aow_Units_FixedArray_Aow_Units_AoWE_TGroupBehavior_10_5
{
    private FixedArray_Aow_Units_AoWE_TGroupBehavior_10 element0;

    public static FixedArray_Aow_Units_FixedArray_Aow_Units_AoWE_TGroupBehavior_10_5 From(ReadOnlySpan<FixedArray_Aow_Units_AoWE_TGroupBehavior_10> values)
    {
        FixedArray_Aow_Units_FixedArray_Aow_Units_AoWE_TGroupBehavior_10_5 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(12)]
public struct FixedArray_Aow_Units_FixedArray_Aow_Units_AoWE_TStatus_12_12
{
    private FixedArray_Aow_Units_AoWE_TStatus_12 element0;

    public static FixedArray_Aow_Units_FixedArray_Aow_Units_AoWE_TStatus_12_12 From(ReadOnlySpan<FixedArray_Aow_Units_AoWE_TStatus_12> values)
    {
        FixedArray_Aow_Units_FixedArray_Aow_Units_AoWE_TStatus_12_12 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(256)]
public struct FixedArray_Aow_Units_FixedArray_Aow_Units_FixedArray_sbyte_16_16_256
{
    private FixedArray_Aow_Units_FixedArray_sbyte_16_16 element0;
}

[InlineArray(8)]
public struct FixedArray_Aow_Units_FixedArray_Aow_Units_FixedArray_sbyte_16_16_8
{
    private FixedArray_Aow_Units_FixedArray_sbyte_16_16 element0;

    public static FixedArray_Aow_Units_FixedArray_Aow_Units_FixedArray_sbyte_16_16_8 From(ReadOnlySpan<FixedArray_Aow_Units_FixedArray_sbyte_16_16> values)
    {
        FixedArray_Aow_Units_FixedArray_Aow_Units_FixedArray_sbyte_16_16_8 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(2)]
public struct FixedArray_Aow_Units_FixedArray_Aow_Units_HSEngine_XYTableEntry_8_2
{
    private FixedArray_Aow_Units_HSEngine_XYTableEntry_8 element0;

    public static FixedArray_Aow_Units_FixedArray_Aow_Units_HSEngine_XYTableEntry_8_2 From(ReadOnlySpan<FixedArray_Aow_Units_HSEngine_XYTableEntry_8> values)
    {
        FixedArray_Aow_Units_FixedArray_Aow_Units_HSEngine_XYTableEntry_8_2 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(6)]
public struct FixedArray_Aow_Units_FixedArray_byte_3_6
{
    private FixedArray_byte_3 element0;

    public static FixedArray_Aow_Units_FixedArray_byte_3_6 From(ReadOnlySpan<FixedArray_byte_3> values)
    {
        FixedArray_Aow_Units_FixedArray_byte_3_6 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(37)]
public struct FixedArray_Aow_Units_FixedArray_int_2_37
{
    private FixedArray_int_2 element0;

    public static FixedArray_Aow_Units_FixedArray_int_2_37 From(ReadOnlySpan<FixedArray_int_2> values)
    {
        FixedArray_Aow_Units_FixedArray_int_2_37 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(7)]
public struct FixedArray_Aow_Units_FixedArray_int_2_7
{
    private FixedArray_int_2 element0;

    public static FixedArray_Aow_Units_FixedArray_int_2_7 From(ReadOnlySpan<FixedArray_int_2> values)
    {
        FixedArray_Aow_Units_FixedArray_int_2_7 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(2)]
public struct FixedArray_Aow_Units_FixedArray_int_8_2
{
    private FixedArray_int_8 element0;
}

[InlineArray(16)]
public struct FixedArray_Aow_Units_FixedArray_sbyte_16_16
{
    private FixedArray_sbyte_16 element0;

    public static FixedArray_Aow_Units_FixedArray_sbyte_16_16 From(ReadOnlySpan<FixedArray_sbyte_16> values)
    {
        FixedArray_Aow_Units_FixedArray_sbyte_16_16 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(7)]
public struct FixedArray_Aow_Units_FixedArray_sbyte_7_7
{
    private FixedArray_sbyte_7 element0;

    public static FixedArray_Aow_Units_FixedArray_sbyte_7_7 From(ReadOnlySpan<FixedArray_sbyte_7> values)
    {
        FixedArray_Aow_Units_FixedArray_sbyte_7_7 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(101)]
public struct FixedArray_Aow_Units_GFXE_TBlendTable_101
{
    private TBlendTable element0;
}

[InlineArray(256)]
public struct FixedArray_Aow_Units_HSEngine_THexagonSprite_256
{
    private THexagonSprite element0;
}

[InlineArray(8)]
public struct FixedArray_Aow_Units_HSEngine_XYTableEntry_8
{
    private XYTableEntry element0;

    public static FixedArray_Aow_Units_HSEngine_XYTableEntry_8 From(ReadOnlySpan<XYTableEntry> values)
    {
        FixedArray_Aow_Units_HSEngine_XYTableEntry_8 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(256)]
public struct FixedArray_Aow_Units_HashTabU_TIntegerHashList_256
{
    private Aow.Game.Engine.Hash.TIntegerHashList element0;
}

[InlineArray(256)]
public struct FixedArray_Aow_Units_HashTabU_TStringHashList_256
{
    private Aow.Game.Engine.Hash.TStringHashList element0;
}

[InlineArray(5)]
public struct FixedArray_Aow_Units_Sound_TSFXLibrary_5
{
    private TSFXLibrary element0;
}

[InlineArray(8)]
public struct FixedArray_bool_8
{
    private bool element0;
}

[InlineArray(1)]
public struct FixedArray_byte_1
{
    private byte element0;
}

[InlineArray(10)]
public struct FixedArray_byte_10
{
    private byte element0;
}

[InlineArray(100)]
public struct FixedArray_byte_100
{
    private byte element0;
}

[InlineArray(109)]
public struct FixedArray_byte_109
{
    private byte element0;
}

[InlineArray(11)]
public struct FixedArray_byte_11
{
    private byte element0;
}

[InlineArray(115)]
public struct FixedArray_byte_115
{
    private byte element0;
}

[InlineArray(12)]
public struct FixedArray_byte_12
{
    private byte element0;
}

[InlineArray(121)]
public struct FixedArray_byte_121
{
    private byte element0;
}

[InlineArray(128)]
public struct FixedArray_byte_128
{
    private byte element0;

    public static FixedArray_byte_128 From(ReadOnlySpan<byte> values)
    {
        FixedArray_byte_128 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(13)]
public struct FixedArray_byte_13
{
    private byte element0;
}

[InlineArray(148)]
public struct FixedArray_byte_148
{
    private byte element0;
}

[InlineArray(15)]
public struct FixedArray_byte_15
{
    private byte element0;
}

[InlineArray(16)]
public struct FixedArray_byte_16
{
    private byte element0;
}

[InlineArray(161)]
public struct FixedArray_byte_161
{
    private byte element0;
}

[InlineArray(164)]
public struct FixedArray_byte_164
{
    private byte element0;
}

[InlineArray(165)]
public struct FixedArray_byte_165
{
    private byte element0;
}

[InlineArray(18)]
public struct FixedArray_byte_18
{
    private byte element0;
}

[InlineArray(2)]
public struct FixedArray_byte_2
{
    private byte element0;
}

[InlineArray(20)]
public struct FixedArray_byte_20
{
    private byte element0;
}

[InlineArray(23)]
public struct FixedArray_byte_23
{
    private byte element0;
}

[InlineArray(24)]
public struct FixedArray_byte_24
{
    private byte element0;
}

[InlineArray(240)]
public struct FixedArray_byte_240
{
    private byte element0;
}

[InlineArray(252)]
public struct FixedArray_byte_252
{
    private byte element0;
}

[InlineArray(256)]
public struct FixedArray_byte_256
{
    private byte element0;
}

[InlineArray(260)]
public struct FixedArray_byte_260
{
    private byte element0;
}

[InlineArray(28)]
public struct FixedArray_byte_28
{
    private byte element0;
}

[InlineArray(3)]
public struct FixedArray_byte_3
{
    private byte element0;

    public static FixedArray_byte_3 From(ReadOnlySpan<byte> values)
    {
        FixedArray_byte_3 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(30)]
public struct FixedArray_byte_30
{
    private byte element0;
}

[InlineArray(32)]
public struct FixedArray_byte_32
{
    private byte element0;
}

[InlineArray(36)]
public struct FixedArray_byte_36
{
    private byte element0;
}

[InlineArray(37)]
public struct FixedArray_byte_37
{
    private byte element0;

    public static FixedArray_byte_37 From(ReadOnlySpan<byte> values)
    {
        FixedArray_byte_37 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(3770)]
public struct FixedArray_byte_3770
{
    private byte element0;

    public static FixedArray_byte_3770 From(ReadOnlySpan<byte> values)
    {
        FixedArray_byte_3770 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(4)]
public struct FixedArray_byte_4
{
    private byte element0;

    public static FixedArray_byte_4 From(ReadOnlySpan<byte> values)
    {
        FixedArray_byte_4 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(40)]
public struct FixedArray_byte_40
{
    private byte element0;
}

[InlineArray(41)]
public struct FixedArray_byte_41
{
    private byte element0;
}

[InlineArray(42)]
public struct FixedArray_byte_42
{
    private byte element0;
}

[InlineArray(44)]
public struct FixedArray_byte_44
{
    private byte element0;
}

[InlineArray(45)]
public struct FixedArray_byte_45
{
    private byte element0;
}

[InlineArray(48)]
public struct FixedArray_byte_48
{
    private byte element0;
}

[InlineArray(49)]
public struct FixedArray_byte_49
{
    private byte element0;
}

[InlineArray(5)]
public struct FixedArray_byte_5
{
    private byte element0;
}

[InlineArray(50)]
public struct FixedArray_byte_50
{
    private byte element0;
}

[InlineArray(51)]
public struct FixedArray_byte_51
{
    private byte element0;
}

[InlineArray(52)]
public struct FixedArray_byte_52
{
    private byte element0;
}

[InlineArray(56)]
public struct FixedArray_byte_56
{
    private byte element0;
}

[InlineArray(584)]
public struct FixedArray_byte_584
{
    private byte element0;
}

[InlineArray(6)]
public struct FixedArray_byte_6
{
    private byte element0;

    public static FixedArray_byte_6 From(ReadOnlySpan<byte> values)
    {
        FixedArray_byte_6 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(68)]
public struct FixedArray_byte_68
{
    private byte element0;
}

[InlineArray(7)]
public struct FixedArray_byte_7
{
    private byte element0;
}

[InlineArray(8)]
public struct FixedArray_byte_8
{
    private byte element0;

    public static FixedArray_byte_8 From(ReadOnlySpan<byte> values)
    {
        FixedArray_byte_8 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(80)]
public struct FixedArray_byte_80
{
    private byte element0;
}

[InlineArray(9)]
public struct FixedArray_byte_9
{
    private byte element0;

    public static FixedArray_byte_9 From(ReadOnlySpan<byte> values)
    {
        FixedArray_byte_9 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(90)]
public struct FixedArray_byte_90
{
    private byte element0;
}

[InlineArray(92)]
public struct FixedArray_byte_92
{
    private byte element0;
}

[InlineArray(96)]
public struct FixedArray_byte_96
{
    private byte element0;
}

[InlineArray(20)]
public struct FixedArray_float_20
{
    private float element0;
}

[InlineArray(512)]
public struct FixedArray_float_512
{
    private float element0;
}

[InlineArray(1)]
public struct FixedArray_int_1
{
    private int element0;

    public static FixedArray_int_1 From(ReadOnlySpan<int> values)
    {
        FixedArray_int_1 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(10)]
public struct FixedArray_int_10
{
    private int element0;
}

[InlineArray(11)]
public struct FixedArray_int_11
{
    private int element0;

    public static FixedArray_int_11 From(ReadOnlySpan<int> values)
    {
        FixedArray_int_11 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(12)]
public struct FixedArray_int_12
{
    private int element0;

    public static FixedArray_int_12 From(ReadOnlySpan<int> values)
    {
        FixedArray_int_12 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(13)]
public struct FixedArray_int_13
{
    private int element0;

    public static FixedArray_int_13 From(ReadOnlySpan<int> values)
    {
        FixedArray_int_13 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(14)]
public struct FixedArray_int_14
{
    private int element0;

    public static FixedArray_int_14 From(ReadOnlySpan<int> values)
    {
        FixedArray_int_14 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(18)]
public struct FixedArray_int_18
{
    private int element0;

    public static FixedArray_int_18 From(ReadOnlySpan<int> values)
    {
        FixedArray_int_18 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(19)]
public struct FixedArray_int_19
{
    private int element0;

    public static FixedArray_int_19 From(ReadOnlySpan<int> values)
    {
        FixedArray_int_19 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(2)]
public struct FixedArray_int_2
{
    private int element0;

    public static FixedArray_int_2 From(ReadOnlySpan<int> values)
    {
        FixedArray_int_2 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(20)]
public struct FixedArray_int_20
{
    private int element0;
}

[InlineArray(3)]
public struct FixedArray_int_3
{
    private int element0;

    public static FixedArray_int_3 From(ReadOnlySpan<int> values)
    {
        FixedArray_int_3 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(305)]
public struct FixedArray_int_305
{
    private int element0;
}

[InlineArray(308)]
public struct FixedArray_int_308
{
    private int element0;
}

[InlineArray(4)]
public struct FixedArray_int_4
{
    private int element0;
}

[InlineArray(48)]
public struct FixedArray_int_48
{
    private int element0;

    public static FixedArray_int_48 From(ReadOnlySpan<int> values)
    {
        FixedArray_int_48 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(5)]
public struct FixedArray_int_5
{
    private int element0;

    public static FixedArray_int_5 From(ReadOnlySpan<int> values)
    {
        FixedArray_int_5 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(60)]
public struct FixedArray_int_60
{
    private int element0;

    public static FixedArray_int_60 From(ReadOnlySpan<int> values)
    {
        FixedArray_int_60 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(7)]
public struct FixedArray_int_7
{
    private int element0;

    public static FixedArray_int_7 From(ReadOnlySpan<int> values)
    {
        FixedArray_int_7 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(72)]
public struct FixedArray_int_72
{
    private int element0;

    public static FixedArray_int_72 From(ReadOnlySpan<int> values)
    {
        FixedArray_int_72 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(8)]
public struct FixedArray_int_8
{
    private int element0;
}

[InlineArray(9)]
public struct FixedArray_int_9
{
    private int element0;
}

[InlineArray(512)]
public struct FixedArray_object_512
{
    private object element0;
}

[InlineArray(16)]
public struct FixedArray_sbyte_16
{
    private sbyte element0;

    public static FixedArray_sbyte_16 From(ReadOnlySpan<sbyte> values)
    {
        FixedArray_sbyte_16 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(2)]
public struct FixedArray_sbyte_2
{
    private sbyte element0;
}

[InlineArray(3)]
public struct FixedArray_sbyte_3
{
    private sbyte element0;

    public static FixedArray_sbyte_3 From(ReadOnlySpan<sbyte> values)
    {
        FixedArray_sbyte_3 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(5)]
public struct FixedArray_sbyte_5
{
    private sbyte element0;

    public static FixedArray_sbyte_5 From(ReadOnlySpan<sbyte> values)
    {
        FixedArray_sbyte_5 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(7)]
public struct FixedArray_sbyte_7
{
    private sbyte element0;

    public static FixedArray_sbyte_7 From(ReadOnlySpan<sbyte> values)
    {
        FixedArray_sbyte_7 result = default;
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
}

[InlineArray(8)]
public struct FixedArray_ushort_8
{
    private ushort element0;
}
