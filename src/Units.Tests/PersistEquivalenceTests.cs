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

namespace Aow.Units.Tests;

/// <summary>
/// Pilot objects persist through the tree to the same version 1 JSON the converter decodes from
/// the image, and a fresh object loaded from that JSON writes the image the converter encodes.
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
        Check("TUnitEnchantmentAbilityData", new TUnitEnchantmentAbilityData(null!));
        var absent = new TUnitEnchantmentAbilityData(null!)
        {
            FEnchantmentAbilityID = 0,
            FEnchantmentPlayerIndex = -1,
            FEnchantmentDuration = -1,
            FEnchantment = null,
        };
        Check("TUnitEnchantmentAbilityData", absent);
        var present = new TUnitEnchantmentAbilityData(null!)
        {
            FEnchantmentAbilityID = 65535,
            FEnchantmentPlayerIndex = 0,
            FEnchantmentDuration = -128,
            FEnchantment = new TCosmeticSurgeryEnchantment(null!) { FFaceIndex = 3, FUnitID = int.MinValue },
        };
        Check("TUnitEnchantmentAbilityData", present);
        var edge = new TUnitEnchantmentAbilityData(null!)
        {
            FEnchantmentAbilityID = 1,
            FEnchantmentPlayerIndex = 127,
            FEnchantmentDuration = 127,
            FEnchantment = new TUnitEnchantment(null!) { FEnchantmentAbilityID = int.MaxValue },
        };
        Check("TUnitEnchantmentAbilityData", edge);
    }

    private static void Check(string className, TEObject obj)
    {
        try
        {
            var cls = Schema.Get(className);
            var expected = PilotImage.Decode(Image(obj), className);
            var actual = DocumentBackend.Write(TreeWriter.Write(obj, Factory), cls, Schema, "game");
            EqualJson(expected, actual, className + " write");
            var loaded = (TEObject)Factory.Create(className);
            try
            {
                var node = DocumentBackend.Read(expected, cls, Schema, "game");
                TreeReader.Read(node, loaded, Factory);
                EqualBytes(PilotImage.Encode(expected, className), Image(loaded), className + " read");
            }
            finally
            {
                loaded.Destroy();
            }
        }
        finally
        {
            obj.Destroy();
        }
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
