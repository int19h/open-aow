#nullable disable warnings
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game.Commands;
using Aow.Game.Net;
using Aow.Game.World.Movement;

namespace Aow.Game.Tests;

// PE-4-a: every token event and every packet. The tree writes the object image, a load of that
// image ends on the same bytes, and StoreEngineObject writes the packet wrapper (rwCreateEObject
// at id 0).
public sealed partial class PersistEquivalenceTests
{
    [Fact]
    public void EveryTokenEventAndPacketIsOnTheTree()
    {
        List<string> missing = [];
        foreach (EngineClass engineClass in EngineClass.All)
        {
            Type type = engineClass.Type;
            if (!PacketFamily(type))
            {
                continue;
            }

            if (EngineObject.TreePersistenceRefusal(type) is string reason)
            {
                missing.Add($"{type.Name} ({reason})");
            }
            else if (s_schema.Find(engineClass.StorageKey) is null)
            {
                missing.Add($"{type.Name} (no schema)");
            }
        }
        Assert.True(missing.Count == 0, string.Join(", ", missing));
    }

    [Theory]
    [MemberData(nameof(PacketClasses))]
    public void ATokenEventOrPacketWritesAndLoadsAlike(string className) => WithDestroyingMap(_ => Alike(className, objectValue: Blank(className)));

    [Fact]
    public void AMovePathPacketMatchesReadWrite()
    {
        // The payload of MoveTokenEvent id 20. The default's fourth byte is 9, and the steps
        // carry costs 3, -1 and 127.
        byte[] payload =
        [
            0xFF, 0xFF, 0xFF, 0x09,
            0x05, 0x00, 0x00, 0x00,
            0x01, 0x02, 0x00, 0x03,
            0xFF, 0xFF, 0xFF, 0x09, 0x02, 0x00, 0x00, 0x00,
            0xFE, 0x01, 0x01, 0xFF,
            0x7F, 0x7F, 0x02, 0x7F,
        ];
        MovePath path = new();
        using (MemoryStream input = new(payload))
        {
            path.LoadFromStream(input);
        }

        try
        {
            MoveArmyTokenEvent move = new(null) { Holder = EngineFixture.Engine };
            move.Path.Assign(path);
            Alike("MoveArmyTokenEvent", objectValue: move);
        }
        finally
        {
            path.Dispose();
        }
    }

    [Fact]
    public void AUnitSpellPacketMatchesReadWrite()
    {
        UnitSpellTokenEvent spell = new(null) { Holder = EngineFixture.Engine };
        _ = spell.TargetUnitIds.Add(3);
        _ = spell.TargetUnitIds.Add(9);
        spell.SpellData[0] = 11;
        spell.SpellData[1] = -4;
        spell.TargetPlayer = 2;
        Alike("UnitSpellTokenEvent", objectValue: spell);
    }

    [Fact]
    public void ADataPackageWithPartiesMatchesReadWrite()
    {
        DataPackage package = new(null) { Holder = EngineFixture.Engine, Sender = 4, Receiver = 7 };
        Alike("DataPackage", objectValue: package);
    }

    [Fact]
    public void ANilPacketIsAnEmptyObjectInsideTheTable()
    {
        byte[] packet = GameBoundary.WritePacket(root: null, classId: 0, className: null);
        using MemoryStream stored = new();
        EngineFixture.Engine.StoreEngineObject(stored, objectValue: null, progress: null);
        EqualBytes(packet, actual: stored.ToArray(), label: "nil store");
        Assert.Equal(0, GameBoundary.ReadPacketClassId(packet));
        Assert.Null(EngineFixture.Engine.RetrieveEngineObject(new MemoryStream(packet), progress: null));
    }

    // A tactical frame sends a token package whose list holds a combat move. The stream resolves
    // that class in the tactical table. StoreEngineObject writes the package and
    // RetrieveEngineObject reads the same bytes back.
    [Fact]
    public void ATokenPackageWithACombatMoveRoundTripsAsAPacket()
    {
        EngineClass descriptor = EngineClass.Of<CombatMoveTokenEvent>();
        bool added = !EngineFixture.Engine.ClassTable.ContainsKey(descriptor.ClassId);
        if (added)
        {
            EngineFixture.Engine.RegisterEngineClasses([descriptor]);
        }

        CombatMoveTokenEvent move = new(null) { Holder = EngineFixture.Engine, TokenCounter = 4 };
        try
        {
            using TokenPackage package = new(null) { Holder = EngineFixture.Engine };
            package.TokenEvents.Add(move);
            using MemoryStream memory = new();
            EngineFixture.Engine.StoreEngineObject(memory, objectValue: package, progress: null);
            byte[] sent = memory.ToArray();
            memory.Position = 0;
            using TokenPackage received = Assert.IsType<TokenPackage>(EngineFixture.Engine.RetrieveEngineObject(memory, progress: null));
            CombatMoveTokenEvent child = Assert.IsType<CombatMoveTokenEvent>(received.TokenEvents[0]);
            Assert.Equal(4, child.TokenCounter);
            using MemoryStream again = new();
            EngineFixture.Engine.StoreEngineObject(again, objectValue: received, progress: null);
            EqualBytes(sent, actual: again.ToArray(), label: "combat token package resent");
        }
        finally
        {
            _ = move.Release();
            if (added)
            {
                EngineFixture.Engine.UnregisterEngineClasses([descriptor]);
            }
        }
    }

    private static void Alike(string className, EngineObject objectValue)
    {
        EngineObject? viaPersist = null;
        EngineObject? loaded = null;
        try
        {
            SchemaClass classValue = s_schema.Get(className);
            string engine = GameBoundary.PacketClassTable(classValue);
            byte[] image = Image(objectValue);
            ObjectNode tree = TreeWriter.Write(objectValue, factory: Factory);
            EqualBytes(image, actual: OriginalBackend.Write(tree, classValue: classValue, schema: s_schema, engine: engine), label: $"{className} original");
            EqualJson(PilotImage.Decode(image, className: className, engine: engine), actual: DocumentBackend.Write(tree, classValue: classValue, schema: s_schema, engine: engine), label: $"{className} write");
            byte[] packet = GameBoundary.WritePacket(tree, classId: objectValue.EngineClass.ClassId, className: Factory.ClassName(objectValue));
            using (MemoryStream stored = new())
            {
                EngineFixture.Engine.StoreEngineObject(stored, objectValue: objectValue, progress: null);
                EqualBytes(packet, actual: stored.ToArray(), label: $"{className} store");
            }
            viaPersist = Blank(className);
            Load(viaPersist, image: image);
            EqualBytes(image, actual: Image(viaPersist), label: $"{className} read");
            bool registered = EngineFixture.Engine.ClassTable.TryGetValue(objectValue.EngineClass.ClassId, out EngineClass registeredClass)
                && registeredClass is not null
                && registeredClass.Type == objectValue.GetType();
            if (!registered)
            {
                return;
            }

            loaded = EngineFixture.Engine.RetrieveEngineObject(new MemoryStream(packet), progress: null);
            Assert.Equal(objectValue.GetType(), loaded?.GetType());
            if (loaded is null)
            {
                return;
            }

            EqualBytes(image, actual: Image(loaded), label: $"{className} packet read");
        }
        finally
        {
            Discard(loaded);
            Discard(viaPersist);
            Discard(objectValue);
        }
    }

    public static TheoryData<string> PacketClasses()
    {
        SortedSet<string> names = [with(StringComparer.Ordinal)];
        foreach (EngineClass engineClass in EngineClass.All)
        {
            Type type = engineClass.Type;
            if (!PacketFamily(type) || type.IsAbstract)
            {
                continue;
            }

            if (EngineObject.TreePersistenceRefusal(type) is not null)
            {
                continue;
            }

            if (s_schema.Find(engineClass.StorageKey) is null)
            {
                continue;
            }

            _ = names.Add(engineClass.StorageKey);
        }
        TheoryData<string> data = [.. names];

        return data;
    }

    private static bool PacketFamily(Type type)
        => type.Assembly == typeof(TokenEvent).Assembly && !type.IsAbstract && (
            typeof(TokenEvent).IsAssignableFrom(type)
            || type == typeof(DataPackage)
            || type == typeof(TokenPackage)
            || type == typeof(TokenEventList)
            || type == typeof(PlayerCommunicationSettings));
}
