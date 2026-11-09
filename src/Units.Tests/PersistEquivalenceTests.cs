using System.Runtime.CompilerServices;
using System.Text;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Units;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using Aow.Units.AoWSetup;
using Aow.Units.City;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using Aow.Units.GlobalEnchantmentSpells;
using Aow.Units.HSEngine;
using Game = Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Tests;

/// <summary>
/// Pilot objects persist through the tree to the same version 3 JSON the converter decodes from
/// the image, and a fresh object loaded from that JSON writes the image the converter encodes.
/// The original backend writes and reads that same image with no ReadWrite on its path.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class PersistEquivalenceTests
{
    private static readonly FormatSchema Schema = FormatSchema.Current;
    private static EnginePersistableFactory? factory;

    private static EnginePersistableFactory Factory => factory ??= new(EngineFixture.Engine);

    [Fact]
    public void TEnchantmentMatchesTheConverter()
    {
        Check("TEnchantment", new TEnchantment(null!));
        var zero = new TEnchantment(null!) { FDispelResistance = 0, FSource = 0, FUpkeep = 0, FEnchantmentID = 0 };
        Check("TEnchantment", zero);
        var edge = new TEnchantment(null!)
        {
            FDispelResistance = -1,
            FSource = 127,
            FUpkeep = int.MinValue,
            FEnchantmentID = int.MaxValue,
        };
        Check("TEnchantment", edge);
        Check("TEnchantment", new TEnchantment(null!) { FDispelResistance = -128, FSource = 0, FUpkeep = int.MaxValue, FEnchantmentID = int.MinValue });
    }

    [Fact]
    public void TEventLogMatchesTheConverter()
    {
        Check("TEventLog", new TEventLog(null!));
        Check("TEventLog", new TEventLog(null!) { FDay = 0, FPlayer = 0, FID = 0 });
        Check("TEventLog", new TEventLog(null!) { FDay = int.MinValue, FPlayer = -128, FID = int.MaxValue });
        Check("TEventLog", new TEventLog(null!) { FDay = int.MaxValue, FPlayer = 127, FID = int.MinValue });
    }

    [Fact]
    public void TMessageEventLogMatchesTheConverter()
    {
        Check("TMessageEventLog", new TMessageEventLog(null!));
        var empty = new TMessageEventLog(null!) { FDay = 0, FPlayer = 0, FID = 0, FTitle = "", FLayout = 0, FButtons = 0 };
        Check("TMessageEventLog", empty);
        var full = new TMessageEventLog(null!)
        {
            FDay = int.MaxValue,
            FPlayer = -1,
            FID = int.MinValue,
            FTitle = "Jörg",
            FLayout = TMessageEventLogLayout.melHeader,
            FButtons = TMessageEventLogButtons.mebYesNoCancel,
        };
        full.FMessage.Add("one");
        full.FMessage.Add("");
        full.FMessage.Add("two");
        Check("TMessageEventLog", full);
        var edge = new TMessageEventLog(null!) { FLayout = (TMessageEventLogLayout)255, FButtons = (TMessageEventLogButtons)255, FTitle = "" };
        edge.FMessage.Add("line");
        Check("TMessageEventLog", edge);
    }

    [Fact]
    public void TEResourceEditSettingsMatchesTheConverter()
    {
        Check("TEControlStyleObject", new TEControlStyleObject(null!));
        var zeroStyle = new TEControlStyleObject(null!) { FControlStyle = TSet<TEControlStyleElement>.FromMask(0) };
        Check("TEControlStyleObject", zeroStyle);
        var bits = new TEControlStyleObject(null!) { FControlStyle = TSet<TEControlStyleElement>.FromMask(0xA5) };
        Check("TEControlStyleObject", bits);
        Check("TEResourceEditSettings", new TEResourceEditSettings(null!));
        var editors = new TEResourceEditSettings(null!) { FControlStyle = TSet<TEControlStyleElement>.FromMask(0) };
        editors.FEditors.AddObject("", Runtime.IntToObject(0));
        editors.FEditors.AddObject("Gate", Runtime.IntToObject(4));
        Check("TEResourceEditSettings", editors);
    }

    [Fact]
    public void TItemGfxMatchesTheConverter()
    {
        Check("TEResource", new TEResource(null!));
        Check("TEResource", new TEResource(null!) { FResourceID = int.MinValue, FEditID = int.MaxValue, FEditIndex = 0 });
        Check("TItemGFX", new TItemGFX(null!));
        var zero = new TItemGFX(null!)
        {
            FResourceID = 0,
            FEditID = 0,
            FEditIndex = 0,
            FImageLibraryName = "",
            FImageIndex = 0,
            FItemType = 0,
            FHidden = false,
        };
        Check("TItemGFX", zero);
        var full = new TItemGFX(null!)
        {
            FResourceID = int.MaxValue,
            FEditID = int.MinValue,
            FEditIndex = 3,
            FImageLibraryName = "Jörg",
            FImageIndex = int.MinValue,
            FItemType = (TItemType)255,
            FHidden = true,
        };
        Check("TItemGFX", full);
    }

    [Fact]
    public void TAIBudgetMatchesTheConverter()
    {
        Check("TAIBudget", new TAIBudget(null!));
        Check("TAISavingsBudget", new TAISavingsBudget(null!));
        Check("TAITransporterBudget", new TAITransporterBudget(null!));
        Check("TAICityFortifyBudget", new TAICityFortifyBudget(null!));
        Check("TAICityUpgradeBudget", new TAICityUpgradeBudget(null!));
        Check("TAICityResearchBudget", new TAICityResearchBudget(null!));
        var edge = new TAIBudget(null!)
        {
            FPriority = int.MinValue,
            FRequiredGold = int.MaxValue,
            FRequiredMana = int.MinValue,
            FGoldUpkeep = int.MaxValue,
            FManaUpkeep = 0,
            FGold = int.MinValue,
            FMana = int.MaxValue,
            FDays = int.MinValue,
            FDayGoldMax = int.MaxValue,
            FDayManaMax = int.MinValue,
            FDayGold = int.MaxValue,
            FDayMana = int.MinValue,
        };
        Check("TAIBudget", edge);
        var manager = new TAIBudgetManager(null!);
        Check("TAIBudgetManager", manager);
        var populated = new TAIBudgetManager(null!)
        {
            FGold = int.MaxValue,
            FMana = int.MinValue,
            FGoldUpkeep = 1,
            FManaUpkeep = 2,
            FProductionGoldUpkeep = 3,
            FProductionManaUpkeep = 4,
        };
        new TAISavingsBudget(null!).SetOwner(populated.FBudgets);
        new TAITransporterBudget(null!).SetOwner(populated.FBudgets);
        new TAICityResearchBudget(null!).SetOwner(populated.FBudgets);
        Check("TAIBudgetManager", populated, converterRead: false);
    }

    [Fact]
    public void TFrozenWaterHsMatchesTheConverter()
    {
        Check("TFrozenWaterHS", new TFrozenWaterHS(null!));
        Check("TFrozenWaterHS", new TFrozenWaterHS(null!) { FDaysRemaining = 0, FCasterPlayer = 0 });
        Check("TFrozenWaterHS", new TFrozenWaterHS(null!) { FDaysRemaining = -128, FCasterPlayer = 127 });
        Check("TFrozenWaterHS", new TFrozenWaterHS(null!) { FDaysRemaining = 127, FCasterPlayer = -1 });
    }

    [Fact]
    public void TFloodEnchantmentMatchesTheConverter()
    {
        Check("TGlobalEnchantment", new TGlobalEnchantment(null!));
        var named = new TGlobalEnchantment(null!)
        {
            FEnchantmentID = int.MinValue,
            FEnchantmentType = (TGlobalEnchantmentType)255,
            FPlayerIndex = -1,
            FName = "Jörg",
            FEnchantmentManaUpkeep = int.MaxValue,
            FDispelMana = 0,
        };
        Check("TGlobalEnchantment", named);
        Check("TFloodControl", new TFloodControl(null!));
        var terrain = new TFloodControl(null!) { FWidth = 2, FHeight = 1 };
        terrain.FOriginalTerrain = [0, -1, 127, -128];
        Check("TFloodControl", terrain);
        Check("TFloodEnchantment", new TFloodEnchantment(null!));
        var flood = new TFloodEnchantment(null!)
        {
            FEnchantmentID = 9,
            FEnchantmentType = (TGlobalEnchantmentType)1,
            FPlayerIndex = 4,
            FName = "",
            FEnchantmentManaUpkeep = int.MinValue,
            FDispelMana = int.MaxValue,
            FTurnsActive = int.MaxValue,
            FRemoved = true,
            FDispelPlayer = 127,
            FRestoreDays = 0,
        };
        flood.FFloodControl.FWidth = 1;
        flood.FFloodControl.FHeight = 2;
        flood.FFloodControl.FOriginalTerrain = [1, 2, 3, 4];
        Check("TFloodEnchantment", flood);
        Check("TFloodEnchantment", new TFloodEnchantment(null!) { FRemoved = false, FRestoreDays = -128, FDispelPlayer = -128, FTurnsActive = int.MinValue });
    }

    [Fact]
    public void TUnitEnchantmentMatchesTheConverter()
    {
        Check("TUnitEnchantment", new TUnitEnchantment(null!));
        Check("TUnitEnchantment", new TUnitEnchantment(null!) { FEnchantmentAbilityID = int.MinValue, FUnitID = int.MaxValue, FSource = 0, FDispelResistance = 0, FUpkeep = 0 });
        Check("TCosmeticSurgeryEnchantment", new TCosmeticSurgeryEnchantment(null!) { FFaceIndex = int.MaxValue });
        Check("TCosmeticSurgeryEnchantment", new TCosmeticSurgeryEnchantment(null!) { FFaceIndex = int.MinValue, FEnchantmentID = 0, FUpkeep = 0, FSource = 0, FDispelResistance = 0 });
        RefuseAbilityData(new TUnitEnchantmentAbilityData(null!));
        var absent = new TUnitEnchantmentAbilityData(null!)
        {
            FEnchantmentAbilityID = 0,
            FEnchantmentPlayerIndex = -1,
            FEnchantmentDuration = -1,
            FEnchantment = null,
        };
        RefuseAbilityData(absent);
        var present = new TUnitEnchantmentAbilityData(null!)
        {
            FEnchantmentAbilityID = 65535,
            FEnchantmentPlayerIndex = 0,
            FEnchantmentDuration = -128,
            FEnchantment = new TCosmeticSurgeryEnchantment(null!) { FFaceIndex = 3, FUnitID = int.MinValue },
        };
        RefuseAbilityData(present);
        var edge = new TUnitEnchantmentAbilityData(null!)
        {
            FEnchantmentAbilityID = 1,
            FEnchantmentPlayerIndex = 127,
            FEnchantmentDuration = 127,
            FEnchantment = new TUnitEnchantment(null!) { FEnchantmentAbilityID = int.MaxValue },
        };
        RefuseAbilityData(edge);
    }

    [Fact]
    public void DeclaredEnumMembersAndAnUndeclaredByteRoundTrip()
    {
        Each<TDiplomaticRelation>(value => Check("TDiplomaticRelationHistoryItem", new TDiplomaticRelationHistoryItem(null!) { FDay = 3, FRelation = value }));
        Check("TDiplomaticRelationHistoryItem", new TDiplomaticRelationHistoryItem(null!) { FRelation = (TDiplomaticRelation)255, FDay = int.MinValue });
        Each<TChangeGlobalMagicMode>(value => Check("TChangeGlobalMagicTE", new TChangeGlobalMagicTE(null!) { FMode = value, FPlayerIndex = 1, FEnchantmentID = 2, FValue = 3 }));
        Check("TChangeGlobalMagicTE", new TChangeGlobalMagicTE(null!) { FMode = (TChangeGlobalMagicMode)255, FPlayerIndex = 255, FEnchantmentID = int.MaxValue, FValue = int.MinValue });
        Each<TGlobalEnchantmentType>(value => Check("TGlobalEnchantment", new TGlobalEnchantment(null!) { FEnchantmentType = value }));
        Each<TItemType>(value => Check("TItemGFX", new TItemGFX(null!) { FItemType = value }));
        Each<TMessageEventLogLayout>(value => Check("TMessageEventLog", new TMessageEventLog(null!) { FLayout = value }));
        Each<TMessageEventLogButtons>(value => Check("TMessageEventLog", new TMessageEventLog(null!) { FButtons = value }));
        Each<TCityEnchantmentID>(value =>
        {
            var control = new TGlobalMagicControl(null!);
            var city = new TCityEnchantment(null!) { FEnchantmentID = value };
            city.FEnchantmentLocation.XYL = 9;
            city.SetOwner(control.FEnchantments);
            Check("TGlobalMagicControl", control, converterRead: false);
        });
    }

    [Fact]
    public void SetsCoverEmptyFullAndUnnamedBits()
    {
        Check("TEControlStyleObject", new TEControlStyleObject(null!) { FControlStyle = TSet<TEControlStyleElement>.FromMask(0xFF) });
        Check("TSingleTargetCA", new TSingleTargetCA(null!) { FMode = TSet<TSingleTargetModeElement>.FromMask(0), FSourceID = 0, FTargetID = 0 });
        Check("TSingleTargetCA", new TSingleTargetCA(null!) { FMode = TSet<TSingleTargetModeElement>.Of(TSingleTargetModeElement.smDefensive), FSourceID = 255, FTargetID = 1 });
        Check("TSingleTargetCA", new TSingleTargetCA(null!) { FMode = TSet<TSingleTargetModeElement>.FromMask(0xFF), FSourceID = 4, FTargetID = 5 });
    }

    [Fact]
    public void FixedArraysCoverZeroMixedAndBoundaryValues()
    {
        var zero = new TUnitSpellCA(null!);
        Check("TUnitSpellCA", zero);
        var mixed = new TUnitSpellCA(null!) { FSpellID = 8, FSourceID = 1, FTargetID = 2, FMana = -4 };
        mixed.FSpellData[0] = 7;
        mixed.FSpellData[1] = -1;
        Check("TUnitSpellCA", mixed);
        var bounds = new TUnitSpellCA(null!) { FSpellID = int.MinValue, FMana = int.MaxValue };
        bounds.FSpellData[0] = int.MinValue;
        bounds.FSpellData[1] = int.MaxValue;
        Check("TUnitSpellCA", bounds);
        var control = new TGlobalMagicControl(null!);
        Check("TGlobalMagicControl", control);
        var spheres = new TGlobalMagicControl(null!) { FNextEnchantmentID = int.MaxValue };
        for (int i = 0; i < 9; i++) spheres.FDoubleSphereManaCount[i] = i == 8 ? int.MinValue : i;
        var hatred = new THatredEnchantment(null!) { FDaysActive = 12 };
        hatred.SetOwner(spheres.FEnchantments);
        var harmony = new THarmonyEnchantment(null!) { FDaysActive = int.MinValue };
        harmony.SetOwner(spheres.FEnchantments);
        Check("TGlobalMagicControl", spheres, converterRead: false);
    }

    [Fact]
    public void ArmyViewCoversBothArrayWidths()
    {
        var previous = Game.AoWHSMap;
        var map = new TAoWHSMap(null!);
        map.FStatus = TSet<THSMapStatusElement>.Of(THSMapStatusElement.msDestroying);
        Game.AoWHSMap = map;
        try
        {
            Check("TArmy", new TArmy(null!));
            Each<TGroupBehavior>(value =>
            {
                var army = new TArmy(null!) { FOldAIBehavior = value, FPlayer = -1 };
                army.FMode = TSet<TArmyModeElement>.Of(TArmyModeElement.amFixedBehavior);
                army.FUserMode = TSet<TArmyUserModeElement>.FromMask(0);
                Check("TArmy", army);
            });
            var full = new TArmy(null!);
            full.FMode = TSet<TArmyModeElement>.FromMask(0xFF) - TSet<TArmyModeElement>.Of(TArmyModeElement.amMoving);
            full.FUserMode = TSet<TArmyUserModeElement>.FromMask(0xFF);
            Check("TArmy", full);
            var view = new TArmyView(null!);
            Check("TArmyView", view);
            var widths = new TArmyView(null!) { FConcealedForPlayers = ushort.MaxValue };
            for (int i = 0; i < 8; i++)
            {
                widths.FUnitConcealment[i] = (ushort)(i == 0 ? 0 : ushort.MaxValue);
                widths.FMoraleValues[i] = i % 2 == 0 ? int.MinValue : int.MaxValue;
            }
            Check("TArmyView", widths);
        }
        finally
        {
            Game.AoWHSMap = previous!;
            map.Dispose();
        }
    }

    [Fact]
    public void AnOverlongSpellDataListIsRefusedOnExport()
    {
        using var spell = new TUnitSpellCA(null!);
        var cls = Schema.Get("TUnitSpellCA");
        var tree = TreeWriter.Write(spell, Factory);
        var list = (ListNode)tree.Find("SpellData")!;
        list.Items.Add(new IntegerNode(4, true, 1));
        var document = DocumentBackend.Write(tree, cls, Schema, "game");
        Assert.Equal(3, ((JsonArray)document["SpellData"]!).Items.Count);
        var error = Assert.Throws<ConversionException>(() => OriginalBackend.Write(tree, cls, Schema, "game"));
        Assert.Equal(Codes.Range, error.Diagnostic.Code);
        Assert.Equal("root.SpellData", error.Diagnostic.Path);
        Assert.Contains("holds at most 2", error.Diagnostic.Message);
        Assert.Contains("Export does not truncate", error.Diagnostic.Message);
    }

    [Fact]
    public void ANetworkArmyOmitsDestinationsAndStillReadsThem()
    {
        WithDestroyingMap(map =>
        {
            map.FConnectionType = TPlayerConnectionType.pcNetwork;
            var army = new TArmy(null!);
            army.FDestinationX = 3;
            army.FDestinationY = 4;
            army.FDestinationL = 1;
            army.FUserMode = TSet<TArmyUserModeElement>.Of(TArmyUserModeElement.aumGuard);
            var cls = Schema.Get("TArmy");
            var document = DocumentBackend.Write(TreeWriter.Write(army, Factory), cls, Schema, "game");
            Assert.Null(document.Find("DestinationX"));
            Assert.Null(document.Find("DestinationY"));
            Assert.Null(document.Find("DestinationL"));
            Assert.Null(document.Find("UserMode"));
            Check("TArmy", army);

            map.FConnectionType = TPlayerConnectionType.pcLocal;
            var source = new TArmy(null!) { FDestinationX = 3, FDestinationY = 4, FDestinationL = 1 };
            source.FUserMode = TSet<TArmyUserModeElement>.Of(TArmyUserModeElement.aumGuard);
            var image = Image(source);
            source.Dispose();
            map.FConnectionType = TPlayerConnectionType.pcNetwork;
            var viaPersist = (TArmy)Factory.Create("TArmy");
            var viaReadWrite = (TArmy)Factory.Create("TArmy");
            try
            {
                TreeReader.Read(OriginalBackend.Read(image, cls, Schema, "game"), viaPersist, Factory);
                Load(viaReadWrite, image);
                Assert.Equal(3, viaPersist.FDestinationX);
                Assert.Equal(4, viaPersist.FDestinationY);
                Assert.Equal(1, viaPersist.FDestinationL);
                Assert.True(viaPersist.FUserMode.Contains(TArmyUserModeElement.aumGuard));
                Assert.Equal(3, viaReadWrite.FDestinationX);
                Assert.Equal(4, viaReadWrite.FDestinationY);
                Assert.Equal(1, viaReadWrite.FDestinationL);
                Assert.True(viaReadWrite.FUserMode.Contains(TArmyUserModeElement.aumGuard));
                EqualBytes(Image(viaPersist), Image(viaReadWrite), "TArmy network load");
            }
            finally
            {
                viaReadWrite.Dispose();
                viaPersist.Dispose();
            }
        });
    }

    [Fact]
    public void LoadingAnArmyClearsMoving()
    {
        WithDestroyingMap(_ =>
        {
            var source = new TArmy(null!);
            source.FMode = TSet<TArmyModeElement>.Of(TArmyModeElement.amMoving, TArmyModeElement.amFixedBehavior);
            var image = Image(source);
            source.Dispose();
            var cls = Schema.Get("TArmy");
            var viaPersist = (TArmy)Factory.Create("TArmy");
            var viaReadWrite = (TArmy)Factory.Create("TArmy");
            try
            {
                TreeReader.Read(OriginalBackend.Read(image, cls, Schema, "game"), viaPersist, Factory);
                Load(viaReadWrite, image);
                Assert.False(viaPersist.FMode.Contains(TArmyModeElement.amMoving));
                Assert.False(viaReadWrite.FMode.Contains(TArmyModeElement.amMoving));
                Assert.True(viaPersist.FMode.Contains(TArmyModeElement.amFixedBehavior));
                Assert.True(viaReadWrite.FMode.Contains(TArmyModeElement.amFixedBehavior));
                EqualBytes(Image(viaPersist), Image(viaReadWrite), "TArmy moving");
            }
            finally
            {
                viaReadWrite.Dispose();
                viaPersist.Dispose();
            }
        });
    }

    [Fact]
    public void AnArmyWithAUnitRefusesThroughTUnit()
    {
        WithDestroyingMap(_ =>
        {
            using var army = new TArmy(null!);
            new TUnit(null!).SetOwner(army);
            var error = Assert.Throws<InvalidOperationException>(() => TreeWriter.Write(army, Factory));
            Assert.Equal("Class TUnit overrides ReadWrite and has no PersistFields.", error.Message);
        });
    }

    [Fact]
    public void TAnimationMatchesTheConverter()
    {
        Check("TAnimation", new TAnimation(null!));
        var edge = new TAnimation(null!)
        {
            FDimensions = new TRect { Left = int.MinValue, Top = -1, Right = int.MaxValue, Bottom = 4 },
            FXhx = -1,
            FYhx = sbyte.MinValue,
            FLhx = sbyte.MaxValue,
            FFrameCounter = int.MinValue,
        };
        Check("TAnimation", edge);
    }

    [Fact]
    public void TDamageCaMatchesTheConverter()
    {
        Check("TDamageCA", new TDamageCA(null!));
        var edge = new TDamageCA(null!)
        {
            FDamage = 255,
            FSourceID = 255,
            FTargetID = 1,
            FEffects = new TP3PackedDamageTypes { Value = TSet<TDamageType>.FromMask((1UL << 9) | (1UL << 15)) },
            FDamageTypes = new TP3PackedDamageTypes { Value = TSet<TDamageType>.Of(TDamageType.dtFire, TDamageType.dtNone) },
        };
        Check("TDamageCA", edge);
    }

    [Fact]
    public void TRemotePlayerMatchesTheConverter()
    {
        Check("TRemotePlayer", new TRemotePlayer(null!));
        var zero = new TRemotePlayer(null!) { FRemoteID = 0, FPreferredRace = 0, FMapValidation = TMapValidationInfo.mviUnknown, FVersion = 0 };
        Check("TRemotePlayer", zero);
        Each<TMapValidationInfo>(value => Check("TRemotePlayer", new TRemotePlayer(null!)
        {
            FMapValidation = value,
            FName = "Jörg",
            FRemoteID = int.MinValue,
            FPreferredRace = 255,
            FVersion = int.MaxValue,
        }));
    }

    [Fact]
    public void TAIGroupMatchesTheConverter()
    {
        Check("TAIGroup", new TAIGroup(null!));
        var zero = new TAIGroup(null!)
        {
            FGroupID = 0,
            FTransporterID = 0,
            FLeaderID = 0,
            FPlayer = 0,
            FIndependentRelation = 0,
            FProcessPriority = 0,
            FRecruitmentPriority = 0,
            FTargetMode = 0,
            FTargetType = TAIGroupTargetStatus.gtsNone,
        };
        Check("TAIGroup", zero);
        Each<TAIGroupTargetStatus>(value =>
        {
            var group = new TAIGroup(null!)
            {
                FGroupID = int.MinValue,
                FTransporterID = int.MaxValue,
                FLeaderID = int.MinValue,
                FPlayer = -1,
                FIndependentRelation = sbyte.MinValue,
                FProcessPriority = 255,
                FRecruitmentPriority = 1,
                FGoldBudget = int.MaxValue,
                FManaBudget = int.MinValue,
                FTargetMovePoints = int.MaxValue,
                FTargetMode = 255,
                FTargetType = value,
            };
            group.FTargetXYL.XYL = int.MinValue;
            Check("TAIGroup", group);
        });
    }

    [Fact]
    public void TSetupSettingsMatchesTheConverter()
    {
        Check("TSetupSettings", new TSetupSettings(null!));
        var zero = new TSetupSettings(null!)
        {
            FPlayMode = 0,
            FVictoryCondition = 0,
            FIndependentAIMode = 0,
            FConnectionType = 0,
            FTacticalCombatAvailability = 0,
            FExplorationEnabled = false,
            FLeadersOnMap = false,
        };
        Check("TSetupSettings", zero);
        var edge = new TSetupSettings(null!)
        {
            FPlayMode = 255,
            FVictoryCondition = 255,
            FIndependentAIMode = 255,
            FConnectionType = 255,
            FTacticalCombatAvailability = 255,
            FExplorationEnabled = true,
            FLeadersOnMap = true,
            FCustomizeLeaders = true,
            FAlliedVictory = true,
            FAlwaysViewTC = true,
            FFileName = "Jörg",
            FMapName = "Gate",
            FRandomSeed = int.MinValue,
            FDayLimit = int.MaxValue,
            FTurnTime = int.MinValue,
            FHostRemoteID = int.MaxValue,
            FMapDay = int.MinValue,
            FMapCRC = int.MaxValue,
            FSetCRC = int.MinValue,
        };
        Check("TSetupSettings", edge);
    }

    [Fact]
    public void TCustomAbilityListMatchesTheConverter()
    {
        Check("TCustomAbilityList", new TCustomAbilityList(null!));
        var emptyByte = new TCustomAbilityList(null!);
        emptyByte.SetAbCount(8);
        Check("TCustomAbilityList", emptyByte);
        var residue = new TCustomAbilityList(null!);
        residue.SetAbCount(1);
        residue.FAbilityBits[0] = 0x80;
        Check("TCustomAbilityList", residue);
    }

    [Fact]
    public void RegisteredAbilityBitsSurviveTheCleanup()
    {
        var placed = new List<TAbility>();
        try
        {
            placed.Add(PlaceAbility(0));
            placed.Add(PlaceAbility(1));
            placed.Add(PlaceAbility(3));
            var list = new TCustomAbilityList(null!);
            list.SetAbCount(8);
            list.SetAbSet(0, 1);
            list.SetAbSet(1, 1);
            list.SetAbSet(3, 1);
            Check("TCustomAbilityList", list);
        }
        finally
        {
            foreach (var ability in placed) ReleaseAbility(ability);
        }
    }

    [Fact]
    public void AMissingAbilityBitIsClearedAndResidueStays()
    {
        // Image writes the bits and then the cleanup clears the missing bit in memory. The tree
        // write runs after that bit is set again, so both stored images still hold it. Both loads clear it.
        TAbility? placed = null;
        var list = new TCustomAbilityList(null!);
        try
        {
            placed = PlaceAbility(0);
            list.SetAbCount(3);
            list.SetAbSet(0, 1);
            list.SetAbSet(1, 1);
            list.FAbilityBits[0] |= 0xF0;
            var image = Image(list);
            list.SetAbSet(1, 1);
            var cls = Schema.Get("TCustomAbilityList");
            var tree = TreeWriter.Write(list, Factory);
            var expected = PilotImage.Decode(image, "TCustomAbilityList");
            EqualJson(expected, DocumentBackend.Write(tree, cls, Schema, "game"), "missing bit write");
            EqualBytes(image, OriginalBackend.Write(tree, cls, Schema, "game"), "missing bit original");
            EqualBytes(PilotImage.Encode(expected, "TCustomAbilityList"), image, "missing bit converter");
            var viaReadWrite = (TCustomAbilityList)Blank("TCustomAbilityList");
            var viaTree = (TCustomAbilityList)Blank("TCustomAbilityList");
            var fromDocument = (TCustomAbilityList)Blank("TCustomAbilityList");
            try
            {
                Load(viaReadWrite, image);
                TreeReader.Read(OriginalBackend.Read(image, cls, Schema, "game"), viaTree, Factory);
                TreeReader.Read(DocumentBackend.Read(expected, cls, Schema, "game"), fromDocument, Factory);
                Assert.True(viaReadWrite.GetAbSet(0));
                Assert.False(viaReadWrite.GetAbSet(1));
                Assert.Equal(0xF0, viaReadWrite.FAbilityBits[0] & 0xF0);
                Assert.True(viaTree.GetAbSet(0));
                Assert.False(viaTree.GetAbSet(1));
                Assert.Equal(0xF0, viaTree.FAbilityBits[0] & 0xF0);
                EqualBytes(Image(viaReadWrite), Image(viaTree), "missing bit loads");
                EqualBytes(Image(viaReadWrite), Image(fromDocument), "missing bit document");
            }
            finally
            {
                fromDocument.Dispose();
                viaTree.Dispose();
                viaReadWrite.Dispose();
            }
        }
        finally
        {
            list.Dispose();
            if (placed is not null) ReleaseAbility(placed);
        }
    }

    [Fact]
    public void AChildWhosePersistThrowsIsDroppedUnlessTheHandlerStops()
    {
        var descriptor = EClass.Of<THatredEnchantment>();
        bool added = EngineFixture.Engine.ClassTable.Find(descriptor.ClassID) == null;
        if (added) EngineFixture.Engine.RegisterEClasses([descriptor]);
        var control = new TGlobalMagicControl(null!);
        Func<TECustomNode, TEObject>? previous = null;
        try
        {
            var hatred = new THatredEnchantment(null!) { FDaysActive = 12 };
            hatred.SetOwner(control.FEnchantments);
            var image = Image(control);
            previous = descriptor.SubstituteCreate(static owner => new ThrowingHatred(owner));
            var cls = Schema.Get("TGlobalMagicControl");
            var continued = (TGlobalMagicControl)Blank("TGlobalMagicControl");
            var continuedTree = (TGlobalMagicControl)Blank("TGlobalMagicControl");
            try
            {
                Load(continued, image);
                TreeReader.Read(OriginalBackend.Read(image, cls, Schema, "game"), continuedTree, Factory);
                Assert.Equal(0, continued.FEnchantments.GetCount());
                Assert.Equal(0, continuedTree.FEnchantments.GetCount());
                EqualBytes(Image(continued), Image(continuedTree), "dropped child");
            }
            finally
            {
                continuedTree.Dispose();
                continued.Dispose();
            }
            var stopped = (TGlobalMagicControl)Blank("TGlobalMagicControl");
            var stoppedTree = (TGlobalMagicControl)Blank("TGlobalMagicControl");
            try
            {
                var streamError = Assert.Throws<InvalidOperationException>(() =>
                    Load(stopped, image, static (TEStorageStream _, string _, ref bool cont) => cont = false));
                var treeError = Assert.Throws<InvalidOperationException>(() =>
                    TreeReader.Read(OriginalBackend.Read(image, cls, Schema, "game"), stoppedTree, Factory,
                        static (string _, ref bool cont) => cont = false));
                Assert.Equal("hatred failed", streamError.Message);
                Assert.Equal("hatred failed", treeError.Message);
            }
            finally
            {
                stoppedTree.Dispose();
                stopped.Dispose();
            }
        }
        finally
        {
            if (previous is not null) descriptor.SubstituteCreate(previous);
            control.Dispose();
            if (added) EngineFixture.Engine.UnregisterEClasses([descriptor]);
        }
    }

    [Fact]
    public void AFixedObjectWhosePersistThrowsStaysUnlessTheHandlerStops()
    {
        using var source = new TGlobalMagicControl(null!);
        source.FNextEnchantmentID = 9;
        source.FDoubleSphereManaCount[0] = 3;
        var image = Image(source);
        var cls = Schema.Get("TGlobalMagicControl");
        var continued = Faulted(4);
        var continuedTree = Faulted(4);
        var continuedList = continued.FEnchantments;
        var continuedTreeList = continuedTree.FEnchantments;
        try
        {
            Load(continued, image);
            TreeReader.Read(OriginalBackend.Read(image, cls, Schema, "game"), continuedTree, Factory);
            Assert.Same(continuedList, continued.FEnchantments);
            Assert.Same(continuedTreeList, continuedTree.FEnchantments);
            Assert.Equal(9, continued.FNextEnchantmentID);
            Assert.Equal(9, continuedTree.FNextEnchantmentID);
            Assert.Equal(3, continued.FDoubleSphereManaCount[0]);
            Assert.Equal(3, continuedTree.FDoubleSphereManaCount[0]);
            for (int i = 1; i < 9; i++)
            {
                Assert.Equal(0, continued.FDoubleSphereManaCount[i]);
                Assert.Equal(continued.FDoubleSphereManaCount[i], continuedTree.FDoubleSphereManaCount[i]);
            }
        }
        finally
        {
            continuedTree.Dispose();
            continued.Dispose();
        }
        var stopped = Faulted(4);
        var stoppedTree = Faulted(4);
        var stoppedList = stopped.FEnchantments;
        var stoppedTreeList = stoppedTree.FEnchantments;
        try
        {
            var streamError = Assert.Throws<InvalidOperationException>(() =>
                Load(stopped, image, static (TEStorageStream _, string _, ref bool cont) => cont = false));
            var treeError = Assert.Throws<InvalidOperationException>(() =>
                TreeReader.Read(OriginalBackend.Read(image, cls, Schema, "game"), stoppedTree, Factory,
                    static (string _, ref bool cont) => cont = false));
            Assert.Equal("enchantments failed", streamError.Message);
            Assert.Equal("enchantments failed", treeError.Message);
            Assert.Same(stoppedList, stopped.FEnchantments);
            Assert.Same(stoppedTreeList, stoppedTree.FEnchantments);
            Assert.Equal(4, stopped.FNextEnchantmentID);
            Assert.Equal(4, stoppedTree.FNextEnchantmentID);
        }
        finally
        {
            stoppedTree.Dispose();
            stopped.Dispose();
        }
    }

    private static TGlobalMagicControl Faulted(int sentinel)
    {
        var control = new TGlobalMagicControl(null!);
        control.FEnchantments?.Dispose();
        control.FEnchantments = new ThrowingEnchantments(null!);
        control.FNextEnchantmentID = sentinel;
        return control;
    }

    [Fact]
    public void SetupPlayerBehaviorTransfersTheReservedBytes()
    {
        var probe = new BehaviorProbe
        {
            Behavior = new TSetupPlayerBehavior
            {
                PlayerType = 2,
                Flags = 9,
                Reserved = FixedArray_byte_3.From([1, 0, 2]),
            },
        };
        var cls = Schema.Get("TPlayerSetupSettings");
        var tree = TreeWriter.Write(probe, Factory);
        var stored = (JsonObject)DocumentBackend.Write(tree, cls, Schema, "game")["Behavior"]!;
        Assert.Equal(2, ((JsonInteger)stored["PlayerType"]!).Value);
        Assert.Equal("010002", ((JsonString)stored["$reserved"]!).Value);
        Assert.Equal(9, ((JsonInteger)stored["Flags"]!).Value);
        var back = OriginalBackend.Read(OriginalBackend.Write(tree, cls, Schema, "game"), cls, Schema, "game");
        var loaded = new BehaviorProbe();
        TreeReader.Read(back, loaded, Factory);
        Assert.Equal(2, loaded.Behavior.PlayerType);
        Assert.Equal(9, loaded.Behavior.Flags);
        Assert.Equal(1, loaded.Behavior.Reserved[0]);
        Assert.Equal(0, loaded.Behavior.Reserved[1]);
        Assert.Equal(2, loaded.Behavior.Reserved[2]);
    }

    [Fact]
    public void AChampionByteOtherThanZeroOrOneRoundTripsThroughReadWrite()
    {
        using var source = new TUnitResource(null!);
        Unsafe.As<bool, byte>(ref source.FChampion) = 2;
        var image = Image(source);
        using var loaded = new TUnitResource(null!);
        Load(loaded, image);
        Assert.Equal(2, Unsafe.As<bool, byte>(ref loaded.FChampion));
        EqualBytes(image, Image(loaded), "champion 02");
    }

    [Fact]
    public void CsSaveLoadIsTheStoredNameOfTheAlias()
    {
        var info = EnumTypeInfo.Describe(typeof(TEControlStyleElement));
        Assert.Equal("csSaveLoad", info.ByValue[0]);
        Assert.False(info.ByName.ContainsKey("csStored"));
        Assert.Equal(0, info.ByName["csSaveLoad"]);
    }

    private static void WithDestroyingMap(Action<TAoWHSMap> body)
    {
        var previous = Game.AoWHSMap;
        var map = new TAoWHSMap(null!);
        map.FStatus = TSet<THSMapStatusElement>.Of(THSMapStatusElement.msDestroying);
        Game.AoWHSMap = map;
        try
        {
            body(map);
        }
        finally
        {
            Game.AoWHSMap = previous!;
            map.Dispose();
        }
    }

    private static void Each<TEnum>(Action<TEnum> check) where TEnum : struct, Enum
    {
        foreach (var value in Enum.GetValues<TEnum>()) check(value);
    }

    // Three constructed states own a child the live table does not enlist. Both loads drop that
    // child, and the converter document still holds it, so those states pass converterRead false.
    // The record pilots have no children, so their tree read also matches the converter encode.
    private static void Check(string className, TEObject obj, bool converterRead = true)
    {
        TEObject? loaded = null;
        TEObject? fromImage = null;
        TEObject? viaReadWrite = null;
        try
        {
            var cls = Schema.Get(className);
            var image = Image(obj);
            var expected = PilotImage.Decode(image, className);
            var tree = TreeWriter.Write(obj, Factory);
            var actual = DocumentBackend.Write(tree, cls, Schema, "game");
            EqualJson(expected, actual, className + " write");
            EqualBytes(image, OriginalBackend.Write(tree, cls, Schema, "game"), className + " original");
            viaReadWrite = Blank(className);
            Load(viaReadWrite, image);
            var read = Image(viaReadWrite);
            loaded = Blank(className);
            TreeReader.Read(DocumentBackend.Read(expected, cls, Schema, "game"), loaded, Factory);
            if (converterRead)
                EqualBytes(PilotImage.Encode(expected, className), Image(loaded), className + " converter read");
            EqualBytes(read, Image(loaded), className + " read");
            fromImage = Blank(className);
            TreeReader.Read(OriginalBackend.Read(image, cls, Schema, "game"), fromImage, Factory);
            EqualBytes(read, Image(fromImage), className + " original read");
        }
        finally
        {
            fromImage?.Dispose();
            loaded?.Dispose();
            viaReadWrite?.Dispose();
            obj.Dispose();
        }
    }

    private static void RefuseAbilityData(TUnitEnchantmentAbilityData obj)
    {
        try
        {
            var error = Assert.Throws<InvalidOperationException>(() => TreeWriter.Write(obj, Factory));
            Assert.Equal("Class TAbilityData overrides ReadWrite and has no PersistFields.", error.Message);
        }
        finally
        {
            obj.Dispose();
        }
    }

    private static TEObject Blank(string className)
    {
        foreach (var eclass in EClass.All)
            if (eclass.ClassName == className) return eclass.Create(null!);
        throw new InvalidOperationException("The engine has no class " + className + ".");
    }

    private static void Load(TEObject obj, byte[] image, TStorageStreamError? onError = null)
    {
        using var memory = new TMemoryStream();
        memory.Write(image);
        memory.Position = 0;
        var reader = new TEReadStorageStream(memory, EngineFixture.Engine, false);
        if (onError is not null) reader.OnError = onError;
        try
        {
            obj.MainReadWrite(reader);
        }
        finally
        {
            reader.Dispose();
        }
    }

    private static TAbility PlaceAbility(int id)
    {
        var ability = new TAbility(null!);
        ability.FAbilityID = id;
        var list = Game.AoWHSSet.FAbilityControl.FAbilities;
        while (id >= list.Count) list.Add(null);
        list[id] = ability;
        return ability;
    }

    private static void ReleaseAbility(TAbility ability)
    {
        var list = Game.AoWHSSet.FAbilityControl.FAbilities;
        int id = ability.FAbilityID;
        if ((uint)id < (uint)list.Count && ReferenceEquals(list[id], ability)) list[id] = null;
        ability.Dispose();
    }

    private sealed class ThrowingHatred : THatredEnchantment
    {
        public ThrowingHatred(TECustomNode owner) : base(owner)
        {
        }

        public override void ReadWrite(TEStorageStream Stream) => throw new InvalidOperationException("hatred failed");

        public override void PersistFields(IPersister persister) => throw new InvalidOperationException("hatred failed");
    }

    private sealed class ThrowingEnchantments : TGlobalEnchantmentList
    {
        public ThrowingEnchantments(TECustomNode owner) : base(owner)
        {
        }

        public override void ReadWrite(TEStorageStream Stream) => throw new InvalidOperationException("enchantments failed");

        public override void PersistFields(IPersister persister) => throw new InvalidOperationException("enchantments failed");
    }

    private sealed class BehaviorProbe : IPersistable
    {
        public TSetupPlayerBehavior Behavior;

        public void Persist(IPersister persister) => PersistSupport.Record(persister, "Behavior", ref Behavior);
    }

    private static byte[] Image(TEObject obj)
    {
        using var memory = new TMemoryStream();
        using (var writer = new TEWriteStorageStream(memory, EngineFixture.Engine, false))
        {
            obj.MainReadWrite(writer);
        }
        return memory.Memory[..memory.Size];
    }

    private static void EqualJson(JsonObject expected, JsonObject actual, string label)
    {
        var left = JsonWriter.ToUtf8(expected);
        var right = JsonWriter.ToUtf8(actual);
        if (left.AsSpan().SequenceEqual(right)) return;
        Assert.Fail(label + "\n" + Encoding.UTF8.GetString(right));
    }

    private static void EqualBytes(byte[] expected, byte[] actual, string label)
    {
        if (expected.AsSpan().SequenceEqual(actual)) return;
        Assert.Fail(label + " at " + expected.AsSpan().CommonPrefixLength(actual));
    }
}
