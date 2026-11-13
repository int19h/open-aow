using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Commands;
using Aow.Game.Magic;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

// The packed coordinate slots of issue 191: each owner keeps the fourth byte of its stored integer as
// UnknownBytes, the AI target keeps it as the marker, and the move path keeps the move costs. Every
// stored byte round-trips.
public sealed partial class PersistEquivalenceTests
{
    // X 5, Y -2, level 1 and a fourth byte $A5.
    private const int PackedWithFourthByte = unchecked((int)0xA501_FE05);

    [Theory]
    [InlineData("GuardAreaAIGroup", 50)]
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
    [InlineData("AIGroup", 23)]
    public void APackedLocationKeepsItsFourthByte(string className, int id)
    {
        var property = s_schema.Get(className).PropertyById(id) ?? throw new InvalidOperationException(className + " has no id " + id.ToString(CultureInfo.InvariantCulture) + ".");
        EngineObject? blank = null;
        EngineObject? loaded = null;
        try
        {
            blank = Blank(className);
            var document = PilotImage.Decode(Image(blank), className);
            document.Remove(property.Name);
            document.Add(property.Name, new JsonInteger(PackedWithFourthByte));
            var image = PilotImage.Encode(document, className);
            loaded = Blank(className);
            Load(loaded, image);
            EqualBytes(image, Image(loaded), className + " id " + id.ToString(CultureInfo.InvariantCulture));
            switch (loaded)
            {
                case PatrolAIGroup patrol:
                    Assert.Equal(id == 40 ? 0x00A5 : 0xA500, patrol.UnknownBytes);
                    Assert.Equal(new MapCoordinate(X: 5, Y: -2, MapLevel: 1), id == 40 ? patrol.PatrolFrom : patrol.PatrolTo);
                    break;
                case TownGateTokenEvent gate when id == 30:
                    Assert.Equal((byte)0xA5, gate.TeleportUnknownBytes);
                    Assert.Equal(new MapCoordinate(X: 5, Y: -2, MapLevel: 1), gate.TeleportLocation);
                    break;
                case AIGroup group when id == 23:
                    Assert.Equal(new AITargetLocation(Location: new MapCoordinate(X: 5, Y: -2, MapLevel: 1), Marker: 0xA5), group.TargetMapCoordinate);
                    break;
                default:
                    Assert.Equal((byte)0xA5, loaded.GetType().GetProperty("UnknownBytes")!.GetValue(loaded));
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
    public void TheTreeKeepsTheFourthByteOfACityEnchantmentAndAnArmyId()
    {
        var control = new GlobalMagicControl(null!) { Holder = EngineFixture.Engine };
        var city = new CityEnchantment(null!)
        {
            Holder = EngineFixture.Engine,
            CityEnchantmentId = CityEnchantmentId.GoldRush,
            EnchantmentLocation = new MapCoordinate(X: 12, Y: 34, MapLevel: 1),
            UnknownBytes = 0x80,
        };
        city.SetOwner(control.Enchantments);
        Check("GlobalMagicControl", control, converterRead: false);
        var army = new ArmyId(null!) { Holder = EngineFixture.Engine, Player = 2, X = -1, Y = 7, MapLevel = 2, UnknownBytes = 0xFF };
        Check("ArmyId", army);
    }

    [Fact]
    public void TheAITargetKeepsTheNoneValueTheClearedMarkerAndEveryFourthByte()
    {
        foreach (var target in new[]
        {
            AITargetLocation.None,
            AITargetLocation.Cleared,
            AITargetLocation.FromStored(StoredCoordinateImage.FromInt32(int.MinValue)),
            AITargetLocation.At(new MapCoordinate(X: 40, Y: 21, MapLevel: 1)),
        })
        {
            var group = new AIGroup(null!) { Holder = EngineFixture.Engine, TargetMapCoordinate = target };
            Check("AIGroup", group);
        }
        Assert.Equal(0x00FF_FFFF, AITargetLocation.None.ToStored().ToInt32());
        Assert.Equal(-1, AITargetLocation.Cleared.ToStored().ToInt32());
        Assert.NotEqual(AITargetLocation.None, AITargetLocation.Cleared);
        var minimum = AITargetLocation.FromStored(StoredCoordinateImage.FromInt32(int.MinValue));
        Assert.Equal(new AITargetLocation(Location: new MapCoordinate(X: 0, Y: 0, MapLevel: 0), Marker: 0x80), minimum);
        Assert.Equal(int.MinValue, minimum.ToStored().ToInt32());
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
        var path = new MovePath();
        using (var input = new MemoryStream(payload)) path.LoadFromStream(input);
        var stored = new PathStep(Location: MapCoordinate.None, MoveCost: 9);
        Assert.Equal(stored, path.Default);
        Assert.Equal(5, path.Count);
        Assert.Equal(new PathStep(Location: new MapCoordinate(X: 1, Y: 2, MapLevel: 0), MoveCost: 3), path[0]);
        Assert.Equal(stored, path[1]);
        Assert.Equal(stored, path[2]);
        Assert.Equal(new PathStep(Location: new MapCoordinate(X: -2, Y: 1, MapLevel: 1), MoveCost: -1), path[3]);
        Assert.Equal(new PathStep(Location: new MapCoordinate(X: 127, Y: 127, MapLevel: 2), MoveCost: 127), path[4]);
        using (var output = new MemoryStream())
        {
            path.SaveToStream(output);
            Assert.Equal(payload, output.ToArray());
        }

        // The same path in a move packet: ReadWrite writes it and a fresh event reads it back equal.
        MoveTokenEvent? move = null;
        MoveTokenEvent? read = null;
        try
        {
            move = new MoveArmyTokenEvent(null!) { Holder = EngineFixture.Engine };
            move.Path.Assign(path);
            var image = Image(move);
            read = new MoveArmyTokenEvent(null!) { Holder = EngineFixture.Engine };
            Load(read, image);
            Assert.True(read.Path.Compare(move.Path));
            Assert.Equal(stored, read.Path.Default);
            EqualBytes(image, Image(read), "MoveArmyTokenEvent");
            EqualBytes(image, PilotImage.Encode(PilotImage.Decode(image, "MoveArmyTokenEvent"), "MoveArmyTokenEvent"), "MoveArmyTokenEvent converter");
        }
        finally
        {
            read?.Dispose();
            move?.Dispose();
            path.Dispose();
        }
    }
}
