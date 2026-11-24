using System.Globalization;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Game.AI.Groups;
using Aow.Game.Commands;
using Aow.Game.Magic;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

// The packed coordinate slots of issue 191 and #192 follow-up: every Delphi writer stores 0 in the
// fourth byte. GuardLocation and the AI target refuse a non-zero fourth byte on load (the target
// keeps -1 for null), every other packed location ignores it on load and writes 0 on save, and the
// move path keeps the move costs. Constant stored fields ignore loaded values and omit on save.
public sealed partial class PersistEquivalenceTests
{
    // X 5, Y -2, level 1 with fourth byte $A5, and the same location with fourth byte 0.
    private const int PackedWithFourthByte = unchecked((int)0xA501_FE05);
    private const int PackedWithZeroFourthByte = 0x0001_FE05;

    [Theory]
    [InlineData("PatrolAIGroup", 40)]
    [InlineData("PatrolAIGroup", 41)]
    [InlineData("CityTargetSpellTokenEvent", 20)]
    [InlineData("GlobalTargetSpellTokenEvent", 20)]
    [InlineData("TownGateTokenEvent", 20)]
    [InlineData("TownGateTokenEvent", 30)]
    [InlineData("WarpPartyTokenEvent", 30)]
    [InlineData("PlayerMagicEventLog", 24)]
    [InlineData("JoinEventLog", 26)]
    [InlineData("CityEnchantment", 21)]
    [InlineData("ArmyId", 11)]
    public void APackedLocationIgnoresItsFourthByteAndSavesZero(string className, int id)
    {
        SchemaProperty property = s_schema.Get(className).PropertyById(id) ?? throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, handler: $"{className} has no id {id}."));
        EngineObject? blank = null;
        EngineObject? loaded = null;
        try
        {
            blank = Blank(className);
            JsonObject inputDocument = PilotImage.Decode(Image(blank), className);
            _ = inputDocument.Remove(property.Name);
            _ = inputDocument.Add(property.Name, new JsonInteger(PackedWithFourthByte));
            byte[] inputImage = PilotImage.Encode(inputDocument, className);

            JsonObject expectedDocument = PilotImage.Decode(Image(blank), className);
            _ = expectedDocument.Remove(property.Name);
            _ = expectedDocument.Add(property.Name, new JsonInteger(PackedWithZeroFourthByte));
            byte[] expectedImage = PilotImage.Encode(expectedDocument, className);

            loaded = Blank(className);
            Load(loaded, image: inputImage);
            EqualBytes(expectedImage, Image(loaded), label: string.Create(CultureInfo.InvariantCulture, handler: $"{className} id {id}"));
            switch (loaded)
            {
                case PatrolAIGroup patrol:
                    Assert.Equal(new MapCoordinate(5, -2, 1), id == 40 ? patrol.PatrolFrom : patrol.PatrolTo);
                    break;
                case TownGateTokenEvent gate when id == 30:
                    Assert.Equal(new MapCoordinate(5, -2, 1), gate.TeleportLocation);
                    break;
                case WarpPartyTokenEvent warp:
                    Assert.Equal(new MapCoordinate(5, -2, 1), warp.WarpLocation);
                    break;
                case GlobalTargetSpellTokenEvent globalSpell:
                    Assert.Equal(new MapCoordinate(5, -2, 1), globalSpell.Location);
                    break;
                case CityEnchantment city:
                    Assert.Equal(new MapCoordinate(5, -2, 1), city.EnchantmentLocation);
                    break;
                case ArmyId army:
                    Assert.Equal(new MapCoordinate(5, -2, 1), army.Location);
                    break;
                default:
                    break;
            }
        }
        finally
        {
            loaded?.Dispose();
            blank?.Dispose();
        }
    }

    [Fact]
    public void GuardLocationRoundTripsWithZeroFourthByteAndRefusesNonZero()
    {
        foreach (MapCoordinate location in new[] { MapCoordinate.None, new MapCoordinate(0, 0, 0), new MapCoordinate(5, -2, 1) })
        {
            GuardAreaAIGroup group = new(null!) { Holder = EngineFixture.Engine, GuardLocation = location };
            Check("GuardAreaAIGroup", group);
        }

        RefuseStoredGuardLocation(PackedWithFourthByte);
        RefuseStoredGuardLocation(-1);
        RefuseStoredGuardLocation(int.MinValue);
    }

    private static void RefuseStoredGuardLocation(int stored)
    {
        EngineObject? blank = null;
        EngineObject? loaded = null;
        try
        {
            blank = Blank("GuardAreaAIGroup");
            JsonObject document = PilotImage.Decode(Image(blank), className: "GuardAreaAIGroup");
            _ = document.Remove("GuardLocation");
            _ = document.Add("GuardLocation", new JsonInteger(stored));
            byte[] image = PilotImage.Encode(document, className: "GuardAreaAIGroup");
            loaded = Blank("GuardAreaAIGroup");
            InvalidOperationException error = Assert.Throws<InvalidOperationException>(() => Load(loaded, image));
            Assert.Contains("GuardLocation", error.Message, comparisonType: StringComparison.Ordinal);
            Assert.Contains("outside its storage range", error.Message, comparisonType: StringComparison.Ordinal);
        }
        finally
        {
            loaded?.Dispose();
            blank?.Dispose();
        }
    }

    [Fact]
    public void TheTreeRoundTripsACityEnchantmentAndAnArmyId()
    {
        GlobalMagicControl control = new(null!) { Holder = EngineFixture.Engine };
        CityEnchantment city = new(null!)
        {
            Holder = EngineFixture.Engine,
            CityEnchantmentId = CityEnchantmentId.GoldRush,
            EnchantmentLocation = new MapCoordinate(12, 34, 1),
        };
        city.SetOwner(control.Enchantments);
        Check("GlobalMagicControl", control, converterRead: false);
        ArmyId army = new(null!) { Holder = EngineFixture.Engine, Player = PlayerId.Player2, Location = new MapCoordinate(-1, 7, 2) };
        Check("ArmyId", army);
    }

    [Theory]
    [InlineData("ProductionTokenEvent", 27, false)]
    [InlineData("WorldMapCampaignScenario", 15, true)]
    [InlineData("ChangePlayerMagicTokenEvent", 23, false)]
    [InlineData("StructureTokenEvent", 23, false)]
    [InlineData("NegotiateTokenEvent", 39, false)]
    [InlineData("NegotiateTokenEvent", 40, false)]
    [InlineData("NegotiateRequestEventLog", 44, false)]
    [InlineData("NegotiateRequestEventLog", 45, false)]
    public void ConstantStoredPropertiesIgnoreLoadedValuesAndOmitOnSave(string className, int id, bool isBoolean)
    {
        SchemaClass classValue = s_schema.Get(className);
        SchemaProperty property = classValue.PropertyById(id) ?? throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, handler: $"{className} has no id {id}."));
        string engine = classValue.Engines.Count == 0 || classValue.Engines.Contains("game") ? "game" : classValue.Engines[0];
        EngineClass[] enlisted = EnlistSessionClasses();
        EngineObject? blank = null;
        EngineObject? loaded = null;
        try
        {
            blank = Blank(className);
            byte[] blankImage = Image(blank);
            JsonObject document = PilotImage.Decode(blankImage, className: className, engine: engine);
            _ = document.Remove(property.Name);
            _ = document.Add(property.Name, isBoolean ? new JsonBoolean(true) : new JsonInteger(42));
            byte[] inputImage = PilotImage.Encode(document, className: className, engine: engine);

            loaded = Blank(className);
            Load(loaded, image: inputImage);
            byte[] savedImage = Image(loaded);
            EqualBytes(blankImage, savedImage, label: string.Create(CultureInfo.InvariantCulture, handler: $"{className} id {id}"));
            Assert.Null(PilotImage.Decode(savedImage, className: className, engine: engine).Find(property.Name));
        }
        finally
        {
            loaded?.Dispose();
            blank?.Dispose();
            EngineFixture.Engine.UnregisterEngineClasses(enlisted);
        }
    }

    [Fact]
    public void TheAITargetRoundTripsNullNoneAndALocationAndRefusesAnotherFourthByte()
    {
        foreach (MapCoordinate? target in new MapCoordinate?[] { null, MapCoordinate.None, new MapCoordinate(0, 0, 0), new MapCoordinate(40, 21, 1) })
        {
            AIGroup group = new(null) { Holder = EngineFixture.Engine, TargetLocation = target };
            Check("AIGroup", group);
        }

        Assert.NotEqual(expected: (MapCoordinate?)null, actual: MapCoordinate.None);
        RefuseStoredTarget(unchecked((int)0xA501_FE05));
        RefuseStoredTarget(int.MinValue);
    }

    private static void RefuseStoredTarget(int stored)
    {
        EngineObject? blank = null;
        EngineObject? loaded = null;
        try
        {
            blank = Blank("AIGroup");
            JsonObject document = PilotImage.Decode(Image(blank), className: "AIGroup");
            _ = document.Remove("TargetLocation");
            _ = document.Add("TargetLocation", new JsonInteger(stored));
            byte[] image = PilotImage.Encode(document, className: "AIGroup");
            loaded = Blank("AIGroup");
            InvalidOperationException error = Assert.Throws<InvalidOperationException>(() => Load(loaded, image));
            Assert.Contains("outside its storage range", error.Message, comparisonType: StringComparison.Ordinal);
        }
        finally
        {
            loaded?.Dispose();
            blank?.Dispose();
        }
    }

    [Fact]
    public void AMovePathWithCostsAndAStoredDefaultWritesTheSameBytes()
    {
        // The payload of MoveTokenEvent id 20 (HSEngine.pas TXYLList.SaveToStream): the default item,
        // the count, then the items, where an item equal to the default is followed by its run length.
        // The default's fourth byte is 9, and the steps carry costs 3, -1 and 127.
        byte[] payload =
        [
            0xFF, 0xFF, 0xFF, 0x09,
            0x05, 0x00, 0x00, 0x00,
            0x01, 0x02, 0x00, 0x03,
            0xFF, 0xFF, 0xFF, 0x09, 0x02, 0x00, 0x00, 0x00,
            0xFE, 0x01, 0x01, 0xFF,
            0x7F, 0x7F, 0x02, 0x7F,
        ];
        MovePath path = [];
        using (MemoryStream input = new(payload))
        {
            path.LoadFromStream(input);
        }

        PathStep stored = new(Location: MapCoordinate.None, MoveCost: 9);
        Assert.Equal(stored, path.Default);
        Assert.Equal(5, path.Count);
        Assert.Equal(new PathStep(Location: new MapCoordinate(1, 2, 0), MoveCost: 3), path[0]);
        Assert.Equal(stored, path[1]);
        Assert.Equal(stored, path[2]);
        Assert.Equal(new PathStep(Location: new MapCoordinate(-2, 1, 1), MoveCost: -1), path[3]);
        Assert.Equal(new PathStep(Location: new MapCoordinate(127, 127, 2), MoveCost: 127), path[4]);
        using (MemoryStream output = new())
        {
            path.SaveToStream(output);
            Assert.Equal(payload, output.ToArray());
        }

        // The same path in a move packet: ReadWrite writes it and a fresh event reads it back equal.
        MoveTokenEvent? move = null;
        MoveTokenEvent? read = null;
        try
        {
            move = new MoveArmyTokenEvent(null) { Holder = EngineFixture.Engine };
            move.Path.Assign(path);
            byte[] image = Image(move);
            read = new MoveArmyTokenEvent(null) { Holder = EngineFixture.Engine };
            Load(read, image);
            Assert.True(read.Path.Compare(move.Path));
            Assert.Equal(stored, read.Path.Default);
            EqualBytes(image, Image(read), label: "MoveArmyTokenEvent");
            EqualBytes(image, PilotImage.Encode(PilotImage.Decode(image, className: "MoveArmyTokenEvent"), className: "MoveArmyTokenEvent"), label: "MoveArmyTokenEvent converter");
        }
        finally
        {
            read?.Dispose();
            move?.Dispose();
            path.Dispose();
        }
    }
}
