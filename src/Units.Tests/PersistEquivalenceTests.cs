using System.Text;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Units.AoWE;
using Aow.Units.City;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using Aow.Units.GlobalEnchantmentSpells;
using Aow.Units.HSEngine;
using Game = Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Tests;

/// <summary>
/// Pilot objects persist through the tree to the same version 2 JSON the converter decodes from
/// the image, and a fresh object loaded from that JSON writes the image the converter encodes.
/// The original backend writes and reads that same image with no ReadWrite on its path.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class PersistEquivalenceTests
{
    private static readonly FormatSchema Schema = FormatSchema.Current;
    private static readonly EnginePersistableFactory Factory = EnginePersistableFactory.Instance;

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
        Check("TAIBudgetManager", populated);
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
            Check("TGlobalMagicControl", control);
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
        Check("TGlobalMagicControl", spheres);
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
            map.Destroy();
        }
    }

    [Fact]
    public void AnOverlongSpellDataListIsRefusedOnExport()
    {
        var spell = new TUnitSpellCA(null!);
        try
        {
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
        finally
        {
            spell.Destroy();
        }
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
            source.Destroy();
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
                viaReadWrite.Destroy();
                viaPersist.Destroy();
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
            source.Destroy();
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
                viaReadWrite.Destroy();
                viaPersist.Destroy();
            }
        });
    }

    [Fact]
    public void AnArmyWithAUnitRefusesThroughTUnit()
    {
        WithDestroyingMap(_ =>
        {
            var army = new TArmy(null!);
            try
            {
                new TUnit(null!).SetOwner(army);
                var error = Assert.Throws<InvalidOperationException>(() => TreeWriter.Write(army, Factory));
                Assert.Equal("Class TUnit overrides ReadWrite and has no PersistFields.", error.Message);
            }
            finally
            {
                army.Destroy();
            }
        });
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
            map.Destroy();
        }
    }

    private static void Each<TEnum>(Action<TEnum> check) where TEnum : struct, Enum
    {
        foreach (var value in Enum.GetValues<TEnum>()) check(value);
    }

    private static void Check(string className, TEObject obj)
    {
        TEObject? loaded = null;
        TEObject? fromImage = null;
        try
        {
            var cls = Schema.Get(className);
            var image = Image(obj);
            var expected = PilotImage.Decode(image, className);
            var tree = TreeWriter.Write(obj, Factory);
            var actual = DocumentBackend.Write(tree, cls, Schema, "game");
            EqualJson(expected, actual, className + " write");
            EqualBytes(image, OriginalBackend.Write(tree, cls, Schema, "game"), className + " original");
            loaded = (TEObject)Factory.Create(className);
            TreeReader.Read(DocumentBackend.Read(expected, cls, Schema, "game"), loaded, Factory);
            EqualBytes(PilotImage.Encode(expected, className), Image(loaded), className + " read");
            fromImage = (TEObject)Factory.Create(className);
            TreeReader.Read(OriginalBackend.Read(image, cls, Schema, "game"), fromImage, Factory);
            EqualBytes(image, Image(fromImage), className + " original read");
            var viaReadWrite = (TEObject)Factory.Create(className);
            try
            {
                Load(viaReadWrite, image);
                EqualBytes(Image(fromImage), Image(viaReadWrite), className + " readwrite");
            }
            finally
            {
                viaReadWrite.Destroy();
            }
        }
        finally
        {
            fromImage?.Destroy();
            loaded?.Destroy();
            obj.Destroy();
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
            obj.Destroy();
        }
    }

    private static void Load(TEObject obj, byte[] image)
    {
        EngineFixture.WithRegisteredClasses(() =>
        {
            using var memory = new TMemoryStream();
            memory.Write(image);
            memory.Position = 0;
            var reader = new TEReadStorageStream(memory, EngineFixture.Engine, false);
            try
            {
                obj.MainReadWrite(reader);
            }
            finally
            {
                reader.Destroy();
            }
        });
    }

    private static byte[] Image(TEObject obj)
    {
        using var memory = new TMemoryStream();
        var writer = new TEWriteStorageStream(memory, EngineFixture.Engine, false);
        try
        {
            obj.MainReadWrite(writer);
        }
        finally
        {
            writer.Destroy();
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
