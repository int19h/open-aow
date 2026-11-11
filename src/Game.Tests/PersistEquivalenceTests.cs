using System.Runtime.CompilerServices;
using System.Text;
using Aow.Engine;
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

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
        Check("Enchantment", new Enchantment(null!));
        var zero = new Enchantment(null!) { FDispelResistance = 0, FSource = 0, FUpkeep = 0, FEnchantmentID = 0 };
        Check("Enchantment", zero);
        var edge = new Enchantment(null!)
        {
            FDispelResistance = -1,
            FSource = 127,
            FUpkeep = int.MinValue,
            FEnchantmentID = int.MaxValue,
        };
        Check("Enchantment", edge);
        Check("Enchantment", new Enchantment(null!) { FDispelResistance = -128, FSource = 0, FUpkeep = int.MaxValue, FEnchantmentID = int.MinValue });
    }

    [Fact]
    public void TEventLogMatchesTheConverter()
    {
        Check("EventLog", new EventLog(null!));
        Check("EventLog", new EventLog(null!) { FDay = 0, FPlayer = 0, FID = 0 });
        Check("EventLog", new EventLog(null!) { FDay = int.MinValue, FPlayer = -128, FID = int.MaxValue });
        Check("EventLog", new EventLog(null!) { FDay = int.MaxValue, FPlayer = 127, FID = int.MinValue });
    }

    [Fact]
    public void TMessageEventLogMatchesTheConverter()
    {
        Check("MessageEventLog", new MessageEventLog(null!));
        var empty = new MessageEventLog(null!) { FDay = 0, FPlayer = 0, FID = 0, FTitle = "", FLayout = 0, FButtons = 0 };
        Check("MessageEventLog", empty);
        var full = new MessageEventLog(null!)
        {
            FDay = int.MaxValue,
            FPlayer = -1,
            FID = int.MinValue,
            FTitle = "Jörg",
            FLayout = MessageEventLog.Layout.Header,
            FButtons = MessageEventLog.Buttons.YesNoCancel,
        };
        full.FMessage.Add("one");
        full.FMessage.Add("");
        full.FMessage.Add("two");
        Check("MessageEventLog", full);
        var edge = new MessageEventLog(null!) { FLayout = (MessageEventLog.Layout)255, FButtons = (MessageEventLog.Buttons)255, FTitle = "" };
        edge.FMessage.Add("line");
        Check("MessageEventLog", edge);
    }

    [Fact]
    public void TEResourceEditSettingsMatchesTheConverter()
    {
        Check("ControlStyleObject", new ControlStyleObject(null!));
        var zeroStyle = new ControlStyleObject(null!) { FControlStyle = Set<ControlStyleElement>.FromMask(0) };
        Check("ControlStyleObject", zeroStyle);
        var bits = new ControlStyleObject(null!) { FControlStyle = Set<ControlStyleElement>.FromMask(0xA5) };
        Check("ControlStyleObject", bits);
        Check("ResourceEditSettings", new ResourceEditSettings(null!));
        var editors = new ResourceEditSettings(null!) { FControlStyle = Set<ControlStyleElement>.FromMask(0) };
        editors.FEditors.AddObject("", Runtime.IntToObject(0));
        editors.FEditors.AddObject("Gate", Runtime.IntToObject(4));
        Check("ResourceEditSettings", editors);
    }

    [Fact]
    public void TItemGfxMatchesTheConverter()
    {
        Check("EngineResource", new EngineResource(null!));
        Check("EngineResource", new EngineResource(null!) { FResourceID = int.MinValue, FEditID = int.MaxValue, FEditIndex = 0 });
        Check("ItemGraphics", new ItemGraphics(null!));
        var zero = new ItemGraphics(null!)
        {
            FResourceID = 0,
            FEditID = 0,
            FEditIndex = 0,
            FImageLibraryName = "",
            FImageIndex = 0,
            FItemType = 0,
            FHidden = false,
        };
        Check("ItemGraphics", zero);
        var full = new ItemGraphics(null!)
        {
            FResourceID = int.MaxValue,
            FEditID = int.MinValue,
            FEditIndex = 3,
            FImageLibraryName = "Jörg",
            FImageIndex = int.MinValue,
            FItemType = (ItemType)255,
            FHidden = true,
        };
        Check("ItemGraphics", full);
    }

    [Fact]
    public void TAIBudgetMatchesTheConverter()
    {
        Check("AIBudget", new AIBudget(null!));
        Check("AISavingsBudget", new AISavingsBudget(null!));
        Check("AITransporterBudget", new AITransporterBudget(null!));
        Check("AICityFortifyBudget", new AICityFortifyBudget(null!));
        Check("AICityUpgradeBudget", new AICityUpgradeBudget(null!));
        Check("AICityResearchBudget", new AICityResearchBudget(null!));
        var edge = new AIBudget(null!)
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
        Check("AIBudget", edge);
        var manager = new AIBudgetManager(null!);
        Check("AIBudgetManager", manager);
        var populated = new AIBudgetManager(null!)
        {
            FGold = int.MaxValue,
            FMana = int.MinValue,
            FGoldUpkeep = 1,
            FManaUpkeep = 2,
            FProductionGoldUpkeep = 3,
            FProductionManaUpkeep = 4,
        };
        new AISavingsBudget(null!).SetOwner(populated.FBudgets);
        new AITransporterBudget(null!).SetOwner(populated.FBudgets);
        new AICityResearchBudget(null!).SetOwner(populated.FBudgets);
        Check("AIBudgetManager", populated, converterRead: false);
    }

    [Fact]
    public void TFrozenWaterHsMatchesTheConverter()
    {
        Check("FrozenWaterHexSprite", new FrozenWaterHexSprite(null!));
        Check("FrozenWaterHexSprite", new FrozenWaterHexSprite(null!) { FDaysRemaining = 0, FCasterPlayer = 0 });
        Check("FrozenWaterHexSprite", new FrozenWaterHexSprite(null!) { FDaysRemaining = -128, FCasterPlayer = 127 });
        Check("FrozenWaterHexSprite", new FrozenWaterHexSprite(null!) { FDaysRemaining = 127, FCasterPlayer = -1 });
    }

    [Fact]
    public void TFloodEnchantmentMatchesTheConverter()
    {
        Check("GlobalEnchantment", new GlobalEnchantment(null!));
        var named = new GlobalEnchantment(null!)
        {
            FEnchantmentID = int.MinValue,
            FEnchantmentType = (GlobalEnchantmentType)255,
            FPlayerIndex = -1,
            FName = "Jörg",
            FEnchantmentManaUpkeep = int.MaxValue,
            FDispelMana = 0,
        };
        Check("GlobalEnchantment", named);
        Check("FloodControl", new FloodControl(null!));
        var terrain = new FloodControl(null!) { FWidth = 2, FHeight = 1 };
        terrain.FOriginalTerrain = [0, -1, 127, -128];
        Check("FloodControl", terrain);
        Check("FloodEnchantment", new FloodEnchantment(null!));
        var flood = new FloodEnchantment(null!)
        {
            FEnchantmentID = 9,
            FEnchantmentType = (GlobalEnchantmentType)1,
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
        Check("FloodEnchantment", flood);
        Check("FloodEnchantment", new FloodEnchantment(null!) { FRemoved = false, FRestoreDays = -128, FDispelPlayer = -128, FTurnsActive = int.MinValue });
    }

    [Fact]
    public void TUnitEnchantmentMatchesTheConverter()
    {
        Check("UnitEnchantment", new UnitEnchantment(null!));
        Check("UnitEnchantment", new UnitEnchantment(null!) { FEnchantmentAbilityID = int.MinValue, FUnitID = int.MaxValue, FSource = 0, FDispelResistance = 0, FUpkeep = 0 });
        Check("CosmeticSurgeryEnchantment", new CosmeticSurgeryEnchantment(null!) { FFaceIndex = int.MaxValue });
        Check("CosmeticSurgeryEnchantment", new CosmeticSurgeryEnchantment(null!) { FFaceIndex = int.MinValue, FEnchantmentID = 0, FUpkeep = 0, FSource = 0, FDispelResistance = 0 });
        RefuseAbilityData(new UnitEnchantmentAbilityData(null!));
        var absent = new UnitEnchantmentAbilityData(null!)
        {
            FEnchantmentAbilityID = 0,
            FEnchantmentPlayerIndex = -1,
            FEnchantmentDuration = -1,
            FEnchantment = null,
        };
        RefuseAbilityData(absent);
        var present = new UnitEnchantmentAbilityData(null!)
        {
            FEnchantmentAbilityID = 65535,
            FEnchantmentPlayerIndex = 0,
            FEnchantmentDuration = -128,
            FEnchantment = new CosmeticSurgeryEnchantment(null!) { FFaceIndex = 3, FUnitID = int.MinValue },
        };
        RefuseAbilityData(present);
        var edge = new UnitEnchantmentAbilityData(null!)
        {
            FEnchantmentAbilityID = 1,
            FEnchantmentPlayerIndex = 127,
            FEnchantmentDuration = 127,
            FEnchantment = new UnitEnchantment(null!) { FEnchantmentAbilityID = int.MaxValue },
        };
        RefuseAbilityData(edge);
    }

    [Fact]
    public void DeclaredEnumMembersAndAnUndeclaredByteRoundTrip()
    {
        Each<DiplomaticRelation>(value => Check("DiplomaticRelationHistoryItem", new DiplomaticRelationHistoryItem(null!) { FDay = 3, FRelation = value }));
        Check("DiplomaticRelationHistoryItem", new DiplomaticRelationHistoryItem(null!) { FRelation = (DiplomaticRelation)255, FDay = int.MinValue });
        Each<ChangeGlobalMagicTokenEvent.Mode>(value => Check("ChangeGlobalMagicTokenEvent", new ChangeGlobalMagicTokenEvent(null!) { FMode = value, FPlayerIndex = 1, FEnchantmentID = 2, FValue = 3 }));
        Check("ChangeGlobalMagicTokenEvent", new ChangeGlobalMagicTokenEvent(null!) { FMode = (ChangeGlobalMagicTokenEvent.Mode)255, FPlayerIndex = 255, FEnchantmentID = int.MaxValue, FValue = int.MinValue });
        Each<GlobalEnchantmentType>(value => Check("GlobalEnchantment", new GlobalEnchantment(null!) { FEnchantmentType = value }));
        Each<ItemType>(value => Check("ItemGraphics", new ItemGraphics(null!) { FItemType = value }));
        Each<MessageEventLog.Layout>(value => Check("MessageEventLog", new MessageEventLog(null!) { FLayout = value }));
        Each<MessageEventLog.Buttons>(value => Check("MessageEventLog", new MessageEventLog(null!) { FButtons = value }));
        Each<CityEnchantmentId>(value =>
        {
            var control = new GlobalMagicControl(null!);
            var city = new CityEnchantment(null!) { FEnchantmentID = value };
            city.FEnchantmentLocation.Packed = 9;
            city.SetOwner(control.FEnchantments);
            Check("GlobalMagicControl", control, converterRead: false);
        });
    }

    [Fact]
    public void SetsCoverEmptyFullAndUnnamedBits()
    {
        Check("ControlStyleObject", new ControlStyleObject(null!) { FControlStyle = Set<ControlStyleElement>.FromMask(0xFF) });
        Check("SingleTargetCombatAction", new SingleTargetCombatAction(null!) { FMode = Set<SingleTargetModeElement>.FromMask(0), FSourceID = 0, FTargetID = 0 });
        Check("SingleTargetCombatAction", new SingleTargetCombatAction(null!) { FMode = Set<SingleTargetModeElement>.Of(SingleTargetModeElement.Defensive), FSourceID = 255, FTargetID = 1 });
        Check("SingleTargetCombatAction", new SingleTargetCombatAction(null!) { FMode = Set<SingleTargetModeElement>.FromMask(0xFF), FSourceID = 4, FTargetID = 5 });
    }

    [Fact]
    public void FixedArraysCoverZeroMixedAndBoundaryValues()
    {
        var zero = new UnitSpellCombatAction(null!);
        Check("UnitSpellCombatAction", zero);
        var mixed = new UnitSpellCombatAction(null!) { FSpellID = 8, FSourceID = 1, FTargetID = 2, FMana = -4 };
        mixed.FSpellData[0] = 7;
        mixed.FSpellData[1] = -1;
        Check("UnitSpellCombatAction", mixed);
        var bounds = new UnitSpellCombatAction(null!) { FSpellID = int.MinValue, FMana = int.MaxValue };
        bounds.FSpellData[0] = int.MinValue;
        bounds.FSpellData[1] = int.MaxValue;
        Check("UnitSpellCombatAction", bounds);
        var control = new GlobalMagicControl(null!);
        Check("GlobalMagicControl", control);
        var spheres = new GlobalMagicControl(null!) { FNextEnchantmentID = int.MaxValue };
        for (int i = 0; i < 9; i++) spheres.FDoubleSphereManaCount[i] = i == 8 ? int.MinValue : i;
        var hatred = new HatredEnchantment(null!) { FDaysActive = 12 };
        hatred.SetOwner(spheres.FEnchantments);
        var harmony = new HarmonyEnchantment(null!) { FDaysActive = int.MinValue };
        harmony.SetOwner(spheres.FEnchantments);
        Check("GlobalMagicControl", spheres, converterRead: false);
    }

    [Fact]
    public void ArmyViewCoversBothArrayWidths()
    {
        var previous = AoWHSMap.Current;
        var map = new AoWHSMap(null!);
        map.FStatus = Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Destroying);
        AoWHSMap.Current = map;
        try
        {
            Check("Army", new Army(null!));
            Each<GroupBehavior>(value =>
            {
                var army = new Army(null!) { FOldAIBehavior = value, FPlayer = -1 };
                army.FMode = Set<ArmyModeElement>.Of(ArmyModeElement.FixedBehavior);
                army.FUserMode = Set<ArmyUserModeElement>.FromMask(0);
                Check("Army", army);
            });
            var full = new Army(null!);
            full.FMode = Set<ArmyModeElement>.FromMask(0xFF) - Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
            full.FUserMode = Set<ArmyUserModeElement>.FromMask(0xFF);
            Check("Army", full);
            var view = new ArmyView(null!);
            Check("ArmyView", view);
            var widths = new ArmyView(null!) { FConcealedForPlayers = ushort.MaxValue };
            for (int i = 0; i < 8; i++)
            {
                widths.FUnitConcealment[i] = (ushort)(i == 0 ? 0 : ushort.MaxValue);
                widths.FMoraleValues[i] = i % 2 == 0 ? int.MinValue : int.MaxValue;
            }
            Check("ArmyView", widths);
        }
        finally
        {
            AoWHSMap.Current = previous!;
            map.Dispose();
        }
    }

    [Fact]
    public void AnOverlongSpellDataListIsRefusedOnExport()
    {
        using var spell = new UnitSpellCombatAction(null!);
        var cls = Schema.Get("UnitSpellCombatAction");
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
            map.FConnectionType = PlayerConnectionType.Network;
            var army = new Army(null!);
            army.FDestinationX = 3;
            army.FDestinationY = 4;
            army.FDestinationL = 1;
            army.FUserMode = Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
            var cls = Schema.Get("Army");
            var document = DocumentBackend.Write(TreeWriter.Write(army, Factory), cls, Schema, "game");
            Assert.Null(document.Find("DestinationX"));
            Assert.Null(document.Find("DestinationY"));
            Assert.Null(document.Find("DestinationL"));
            Assert.Null(document.Find("UserMode"));
            Check("Army", army);

            map.FConnectionType = PlayerConnectionType.Local;
            var source = new Army(null!) { FDestinationX = 3, FDestinationY = 4, FDestinationL = 1 };
            source.FUserMode = Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
            var image = Image(source);
            source.Dispose();
            map.FConnectionType = PlayerConnectionType.Network;
            var viaPersist = (Army)Factory.Create("Army");
            var viaReadWrite = (Army)Factory.Create("Army");
            try
            {
                TreeReader.Read(OriginalBackend.Read(image, cls, Schema, "game"), viaPersist, Factory);
                Load(viaReadWrite, image);
                Assert.Equal(3, viaPersist.FDestinationX);
                Assert.Equal(4, viaPersist.FDestinationY);
                Assert.Equal(1, viaPersist.FDestinationL);
                Assert.True(viaPersist.FUserMode.Contains(ArmyUserModeElement.Guard));
                Assert.Equal(3, viaReadWrite.FDestinationX);
                Assert.Equal(4, viaReadWrite.FDestinationY);
                Assert.Equal(1, viaReadWrite.FDestinationL);
                Assert.True(viaReadWrite.FUserMode.Contains(ArmyUserModeElement.Guard));
                EqualBytes(Image(viaPersist), Image(viaReadWrite), "Army network load");
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
            var source = new Army(null!);
            source.FMode = Set<ArmyModeElement>.Of(ArmyModeElement.Moving, ArmyModeElement.FixedBehavior);
            var image = Image(source);
            source.Dispose();
            var cls = Schema.Get("Army");
            var viaPersist = (Army)Factory.Create("Army");
            var viaReadWrite = (Army)Factory.Create("Army");
            try
            {
                TreeReader.Read(OriginalBackend.Read(image, cls, Schema, "game"), viaPersist, Factory);
                Load(viaReadWrite, image);
                Assert.False(viaPersist.FMode.Contains(ArmyModeElement.Moving));
                Assert.False(viaReadWrite.FMode.Contains(ArmyModeElement.Moving));
                Assert.True(viaPersist.FMode.Contains(ArmyModeElement.FixedBehavior));
                Assert.True(viaReadWrite.FMode.Contains(ArmyModeElement.FixedBehavior));
                EqualBytes(Image(viaPersist), Image(viaReadWrite), "Army moving");
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
            using var army = new Army(null!);
            new Unit(null!).SetOwner(army);
            var error = Assert.Throws<InvalidOperationException>(() => TreeWriter.Write(army, Factory));
            Assert.Equal("Class Unit overrides ReadWrite and has no PersistFields.", error.Message);
        });
    }

    [Fact]
    public void TAnimationMatchesTheConverter()
    {
        Check("Animation", new Animation(null!));
        var edge = new Animation(null!)
        {
            FDimensions = new Rect { Left = int.MinValue, Top = -1, Right = int.MaxValue, Bottom = 4 },
            FXhx = -1,
            FYhx = sbyte.MinValue,
            FLhx = sbyte.MaxValue,
            FFrameCounter = int.MinValue,
        };
        Check("Animation", edge);
    }

    [Fact]
    public void TDamageCaMatchesTheConverter()
    {
        Check("DamageCombatAction", new DamageCombatAction(null!));
        var edge = new DamageCombatAction(null!)
        {
            FDamage = 255,
            FSourceID = 255,
            FTargetID = 1,
            FEffects = new PackedDamageTypes { Value = Set<DamageType>.FromMask((1UL << 9) | (1UL << 15)) },
            FDamageTypes = new PackedDamageTypes { Value = Set<DamageType>.Of(DamageType.Fire, DamageType.None) },
        };
        Check("DamageCombatAction", edge);
    }

    [Fact]
    public void TRemotePlayerMatchesTheConverter()
    {
        Check("RemotePlayer", new RemotePlayer(null!));
        var zero = new RemotePlayer(null!) { FRemoteID = 0, FPreferredRace = 0, FMapValidation = MapValidationInfo.Unknown, FVersion = 0 };
        Check("RemotePlayer", zero);
        Each<MapValidationInfo>(value => Check("RemotePlayer", new RemotePlayer(null!)
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
        Check("AIGroup", new AIGroup(null!));
        var zero = new AIGroup(null!)
        {
            FGroupID = 0,
            FTransporterID = 0,
            FLeaderID = 0,
            FPlayer = 0,
            FIndependentRelation = 0,
            FProcessPriority = 0,
            FRecruitmentPriority = 0,
            FTargetMode = 0,
            FTargetType = AIGroupTargetStatus.None,
        };
        Check("AIGroup", zero);
        Each<AIGroupTargetStatus>(value =>
        {
            var group = new AIGroup(null!)
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
            group.FTargetXYL.Packed = int.MinValue;
            Check("AIGroup", group);
        });
    }

    [Fact]
    public void TSetupSettingsMatchesTheConverter()
    {
        Check("SetupSettings", new SetupSettings(null!));
        var zero = new SetupSettings(null!)
        {
            FPlayMode = 0,
            FVictoryCondition = 0,
            FIndependentAIMode = 0,
            FConnectionType = 0,
            FTacticalCombatAvailability = 0,
            FExplorationEnabled = false,
            FLeadersOnMap = false,
        };
        Check("SetupSettings", zero);
        var edge = new SetupSettings(null!)
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
        Check("SetupSettings", edge);
    }

    [Fact]
    public void TCustomAbilityListMatchesTheConverter()
    {
        Check("CustomAbilityList", new CustomAbilityList(null!));
        var emptyByte = new CustomAbilityList(null!);
        emptyByte.SetAbCount(8);
        Check("CustomAbilityList", emptyByte);
        var residue = new CustomAbilityList(null!);
        residue.SetAbCount(1);
        residue.FAbilityBits[0] = 0x80;
        Check("CustomAbilityList", residue);
    }

    [Fact]
    public void RegisteredAbilityBitsSurviveTheCleanup()
    {
        var placed = new List<Ability>();
        try
        {
            placed.Add(PlaceAbility(0));
            placed.Add(PlaceAbility(1));
            placed.Add(PlaceAbility(3));
            var list = new CustomAbilityList(null!);
            list.SetAbCount(8);
            list.SetAbSet(0, 1);
            list.SetAbSet(1, 1);
            list.SetAbSet(3, 1);
            Check("CustomAbilityList", list);
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
        Ability? placed = null;
        var list = new CustomAbilityList(null!);
        try
        {
            placed = PlaceAbility(0);
            list.SetAbCount(3);
            list.SetAbSet(0, 1);
            list.SetAbSet(1, 1);
            list.FAbilityBits[0] |= 0xF0;
            var image = Image(list);
            list.SetAbSet(1, 1);
            var cls = Schema.Get("CustomAbilityList");
            var tree = TreeWriter.Write(list, Factory);
            var expected = PilotImage.Decode(image, "CustomAbilityList");
            EqualJson(expected, DocumentBackend.Write(tree, cls, Schema, "game"), "missing bit write");
            EqualBytes(image, OriginalBackend.Write(tree, cls, Schema, "game"), "missing bit original");
            EqualBytes(PilotImage.Encode(expected, "CustomAbilityList"), image, "missing bit converter");
            var viaReadWrite = (CustomAbilityList)Blank("CustomAbilityList");
            var viaTree = (CustomAbilityList)Blank("CustomAbilityList");
            var fromDocument = (CustomAbilityList)Blank("CustomAbilityList");
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
        var descriptor = EngineClass.Of<HatredEnchantment>();
        bool added = !EngineFixture.Engine.ClassTable.ContainsKey(descriptor.ClassID);
        if (added) EngineFixture.Engine.RegisterEClasses([descriptor]);
        var control = new GlobalMagicControl(null!);
        Func<ECustomNode, EngineObject>? previous = null;
        try
        {
            var hatred = new HatredEnchantment(null!) { FDaysActive = 12 };
            hatred.SetOwner(control.FEnchantments);
            var image = Image(control);
            previous = descriptor.SubstituteCreate(static owner => new ThrowingHatred(owner));
            var cls = Schema.Get("GlobalMagicControl");
            var continued = (GlobalMagicControl)Blank("GlobalMagicControl");
            var continuedTree = (GlobalMagicControl)Blank("GlobalMagicControl");
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
            var stopped = (GlobalMagicControl)Blank("GlobalMagicControl");
            var stoppedTree = (GlobalMagicControl)Blank("GlobalMagicControl");
            try
            {
                var streamError = Assert.Throws<InvalidOperationException>(() =>
                    Load(stopped, image, static (EStorageStream _, string _, ref bool cont) => cont = false));
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
        using var source = new GlobalMagicControl(null!);
        source.FNextEnchantmentID = 9;
        source.FDoubleSphereManaCount[0] = 3;
        var image = Image(source);
        var cls = Schema.Get("GlobalMagicControl");
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
                Load(stopped, image, static (EStorageStream _, string _, ref bool cont) => cont = false));
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

    private static GlobalMagicControl Faulted(int sentinel)
    {
        var control = new GlobalMagicControl(null!);
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
            Behavior = new SetupPlayerBehavior
            {
                PlayerType = 2,
                Flags = 9,
                Reserved = ByteArray3.From([1, 0, 2]),
            },
        };
        var cls = Schema.Get("PlayerSetupSettings");
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
        using var source = new UnitResource(null!);
        Unsafe.As<bool, byte>(ref source.FChampion) = 2;
        var image = Image(source);
        using var loaded = new UnitResource(null!);
        Load(loaded, image);
        Assert.Equal(2, Unsafe.As<bool, byte>(ref loaded.FChampion));
        EqualBytes(image, Image(loaded), "champion 02");
    }

    [Fact]
    public void CsSaveLoadIsTheStoredNameOfTheAlias()
    {
        var info = EnumTypeInfo.Describe(typeof(ControlStyleElement));
        Assert.Equal("SaveLoad", info.ByValue[0]);
        Assert.False(info.ByName.ContainsKey("csStored"));
        Assert.Equal(0, info.ByName["SaveLoad"]);
    }

    private static void WithDestroyingMap(Action<AoWHSMap> body)
    {
        var previous = AoWHSMap.Current;
        var map = new AoWHSMap(null!);
        map.FStatus = Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Destroying);
        AoWHSMap.Current = map;
        try
        {
            body(map);
        }
        finally
        {
            AoWHSMap.Current = previous!;
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
    private static void Check(string className, EngineObject obj, bool converterRead = true)
    {
        EngineObject? loaded = null;
        EngineObject? fromImage = null;
        EngineObject? viaReadWrite = null;
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

    private static void RefuseAbilityData(UnitEnchantmentAbilityData obj)
    {
        try
        {
            var error = Assert.Throws<InvalidOperationException>(() => TreeWriter.Write(obj, Factory));
            Assert.Equal("Class AbilityData overrides ReadWrite and has no PersistFields.", error.Message);
        }
        finally
        {
            obj.Dispose();
        }
    }

    private static EngineObject Blank(string className)
    {
        foreach (var eclass in EngineClass.All)
            if (eclass.ClassName == className) return eclass.Create(null!);
        throw new InvalidOperationException("The engine has no class " + className + ".");
    }

    private static void Load(EngineObject obj, byte[] image, EStorageStream.StorageStreamError? onError = null)
    {
        using var memory = new MemoryStream();
        memory.Write(image);
        memory.Position = 0;
        var reader = new EReadStorageStream(memory, EngineFixture.Engine, false);
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

    private static Ability PlaceAbility(int id)
    {
        var ability = new Ability(null!);
        ability.FAbilityID = id;
        var list = AoWHSSet.Current.FAbilityControl.FAbilities;
        while (id >= list.Count) list.Add(null);
        list[id] = ability;
        return ability;
    }

    private static void ReleaseAbility(Ability ability)
    {
        var list = AoWHSSet.Current.FAbilityControl.FAbilities;
        int id = ability.FAbilityID;
        if ((uint)id < (uint)list.Count && ReferenceEquals(list[id], ability)) list[id] = null;
        ability.Dispose();
    }

    private sealed class ThrowingHatred : HatredEnchantment
    {
        public ThrowingHatred(ECustomNode owner) : base(owner)
        {
        }

        public override void ReadWrite(EStorageStream Stream) => throw new InvalidOperationException("hatred failed");

        public override void PersistFields(IPersister persister) => throw new InvalidOperationException("hatred failed");
    }

    private sealed class ThrowingEnchantments : GlobalEnchantmentList
    {
        public ThrowingEnchantments(ECustomNode owner) : base(owner)
        {
        }

        public override void ReadWrite(EStorageStream Stream) => throw new InvalidOperationException("enchantments failed");

        public override void PersistFields(IPersister persister) => throw new InvalidOperationException("enchantments failed");
    }

    private sealed class BehaviorProbe : IPersistable
    {
        public SetupPlayerBehavior Behavior;

        public void Persist(IPersister persister) => PersistSupport.Record(persister, "Behavior", ref Behavior);
    }

    private static byte[] Image(EngineObject obj)
    {
        using var memory = new MemoryStream();
        using (var writer = new EWriteStorageStream(memory, EngineFixture.Engine, false))
        {
            obj.MainReadWrite(writer);
        }
        return memory.ToArray();
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
