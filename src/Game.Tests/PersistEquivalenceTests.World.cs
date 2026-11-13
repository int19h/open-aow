using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

using Aow.Game.Draw;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

// PE-3-c: the classes of Game/World and Game/Draw whose instances the installed corpus does not
// prove, and both sides of each branch that depends on game state.
public sealed partial class PersistEquivalenceTests
{
    // Every case of the production order mapping and a key no case registers. The document names
    // the case the stored kind selects, and both loads give the same order.
    [Theory]
    [InlineData(1, 3, 2)]
    [InlineData(2, 5, 0)]
    [InlineData(10, 4, 0)]
    [InlineData(11, 0, 0)]
    [InlineData(12, 0, 0)]
    [InlineData(13, 2, 0)]
    [InlineData(14, 0, 0)]
    [InlineData(99, 7, 1)]
    public void AProductionItemStoresItsCaseOnBothPaths(int kind, int parameter, int groupId)
    {
        var order = ProductionOrder.FromFields(kind, parameter, groupId);
        var item = new ProductionItem(null!) { Holder = EngineFixture.Engine, Order = order };
        var document = DocumentBackend.Write(TreeWriter.Write(item, Factory), s_schema.Get("ProductionItem"), s_schema, "game");
        Assert.IsType<JsonString>(document["$case"]);
        Check("ProductionItem", item);
        using var loaded = new ProductionItem(null!) { Holder = EngineFixture.Engine };
        TreeReader.Read(DocumentBackend.Read(document, s_schema.Get("ProductionItem"), s_schema, "game"), loaded, Factory);
        Assert.Equal(order, loaded.Order);
    }

    // A case drops a stored field it does not keep: the kind writes back what FromFields keeps,
    // on both loads.
    [Fact]
    public void AProductionItemDropsAFieldItsCaseDoesNotKeepOnBothLoads()
    {
        var classValue = s_schema.Get("ProductionItem");
        var document = new JsonObject().Add("$case", new JsonString("Unknown")).Add("Kind", new JsonInteger(99))
            .Add("Parameter", new JsonInteger(6)).Add("GroupId", new JsonInteger(3));
        var image = PilotImage.Encode(document, "ProductionItem");
        using var viaReadWrite = new ProductionItem(null!) { Holder = EngineFixture.Engine };
        using var viaPersist = new ProductionItem(null!) { Holder = EngineFixture.Engine };
        Load(viaReadWrite, image);
        TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), viaPersist, Factory);
        Assert.Equal(viaReadWrite.Order, viaPersist.Order);
        EqualBytes(Image(viaReadWrite), Image(viaPersist), "ProductionItem unknown kind");
    }

    [Fact]
    public void AProductionQueueWithItemsMatchesTheConverter()
    {
        var queue = new ProductionQueue(null!) { Holder = EngineFixture.Engine };
        new ProductionItem(null!) { Holder = EngineFixture.Engine, Order = ProductionOrder.FromFields(1, 4, 2) }.SetOwner(queue);
        new ProductionItem(null!) { Holder = EngineFixture.Engine, Order = ProductionOrder.FromFields(12, 0, 0) }.SetOwner(queue);
        Check("ProductionQueue", queue);
    }

    // MapContainer.ReadWrite makes its owner's map hold the container on a load, so the container
    // loads into a map on both paths.
    [Fact]
    public void AMapContainerLoadsIntoItsMapOnBothPaths()
    {
        WithDestroyingMap(map =>
        {
            var classValue = s_schema.Get("MapContainer");
            using var source = new MapContainer(map) { Holder = EngineFixture.Engine };
            var image = Image(source);
            var tree = TreeWriter.Write(source, Factory);
            EqualJson(PilotImage.Decode(image, "MapContainer"), DocumentBackend.Write(tree, classValue, s_schema, "game"), "MapContainer write");
            EqualBytes(image, OriginalBackend.Write(tree, classValue, s_schema, "game"), "MapContainer original");
            using var viaReadWrite = new MapContainer(map) { Holder = EngineFixture.Engine };
            Load(viaReadWrite, image);
            Assert.Same(viaReadWrite, map.MapContainer);
            using var viaPersist = new MapContainer(map) { Holder = EngineFixture.Engine };
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), viaPersist, Factory);
            Assert.Same(viaPersist, map.MapContainer);
            EqualBytes(Image(viaReadWrite), Image(viaPersist), "MapContainer read");
            map.MapContainer = null!;
        });
    }

    // Sign.ReadWrite stores TextSeen only when a map is current and is not a network map. A
    // network map's write leaves it out, and both loads leave a stored one unread.
    [Fact]
    public void ASignOnANetworkMapLeavesItsTextSeenOnBothPaths()
    {
        WithDestroyingMap(map =>
        {
            var classValue = s_schema.Get("Sign");
            using var source = new Sign(null!) { Holder = EngineFixture.Engine };
            source.Text.Add("Welcome");
            source.TextSeen[2] = true;
            var local = Image(source);
            Assert.NotNull(DocumentBackend.Write(TreeWriter.Write(source, Factory), classValue, s_schema, "game").Find("TextSeen"));
            map.ConnectionType = PlayerConnectionType.Network;
            var network = Image(source);
            var tree = TreeWriter.Write(source, Factory);
            Assert.Null(DocumentBackend.Write(tree, classValue, s_schema, "game").Find("TextSeen"));
            EqualBytes(network, OriginalBackend.Write(tree, classValue, s_schema, "game"), "Sign network write");
            using var viaReadWrite = new Sign(null!) { Holder = EngineFixture.Engine };
            using var viaPersist = new Sign(null!) { Holder = EngineFixture.Engine };
            AlikeOutcome(() => Load(viaReadWrite, local), () => TreeReader.Read(OriginalBackend.Read(local, classValue, s_schema, "game"), viaPersist, Factory), "Sign network load");
            Assert.False(viaPersist.TextSeen[2]);
            Assert.Equal(viaReadWrite.TextSeen[2], viaPersist.TextSeen[2]);
        });
    }

    // Every class of Game/World and Game/Draw on the tree, blank: the tree write matches the
    // converter's document and the stream image, and a load of that image ends alike on both
    // paths, with the same state or the same error. A hexagon sprite's load needs its resource in
    // the game set, which the fixture does not hold, so both loads refuse it with the same message.
    // An item sprite with no item removes itself when its item list changes, so it gets one item.
    [Theory]
    [MemberData(nameof(WorldClasses))]
    public void AWorldClassWritesAndLoadsAlikeOnBothPaths(string className)
    {
        WithDestroyingMap(_ =>
        {
            var classValue = s_schema.Get(className);
            var source = Blank(className);
            EngineObject? viaReadWrite = null;
            EngineObject? viaPersist = null;
            try
            {
                if (source is ItemHexSprite sprite) PlainItem(itemId: 3, name: "Ring").SetOwner(sprite.Items);
                byte[]? image = null;
                ObjectNode? tree = null;
                AlikeOutcome(() => image = Image(source), () => tree = TreeWriter.Write(source, Factory), className + " write");
                if (image is null || tree is null) return;
                EqualJson(PilotImage.Decode(image, className), DocumentBackend.Write(tree, classValue, s_schema, "game"), className + " write");
                EqualBytes(image, OriginalBackend.Write(tree, classValue, s_schema, "game"), className + " original");
                var stream = viaReadWrite = Blank(className);
                var persisted = viaPersist = Blank(className);
                bool loaded = AlikeOutcome(() => Load(stream, image), () => TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), persisted, Factory), className + " load");
                if (loaded) EqualBytes(Image(stream), Image(persisted), className + " read");
            }
            finally
            {
                Discard(viaPersist);
                Discard(viaReadWrite);
                Discard(source);
            }
        });
    }

    // A combat sprite's Deactivate reads the combat map and its battle, which the fixture does not
    // have, so its disposal fails after the comparison is done. The comparison does not depend on it.
    private static void Discard(EngineObject? objectValue)
    {
        try
        {
            objectValue?.Dispose();
        }
        catch (NullReferenceException)
        {
        }
    }

    // The maps, the game set and the map container have tests of their own or need state the
    // fixture does not give a blank instance. A map level and its fields are proved on the direct
    // path by the corpus and by the constructed level of PE-2c-4. A dynamic isometric hexagon and
    // the flag control are created only with their owner, and neither declares a storage method.
    private static readonly HashSet<string> s_worldExceptions =
        ["GameMap", "StrategicMap", "CombatMap", "GamePack", "MapContainer", "MapLevel", "CombatMapLevel", "MapField", "DynamicIsometricHex", "FlagControl"];

    public static TheoryData<string> WorldClasses()
    {
        var names = new SortedSet<string>(StringComparer.Ordinal);
        foreach (var engineClass in EngineClass.All)
        {
            var type = engineClass.Type;
            if (type.Assembly != typeof(GamePack).Assembly || type.IsAbstract) continue;
            if (type.Namespace is not { } space || !(space.StartsWith("Aow.Game.World", StringComparison.Ordinal) || space.StartsWith("Aow.Game.Draw", StringComparison.Ordinal))) continue;
            if (EngineObject.TreePersistenceRefusal(type) is not null || s_worldExceptions.Contains(engineClass.StorageKey)) continue;
            if (FormatSchema.Current.Find(engineClass.StorageKey) is null) continue;
            names.Add(engineClass.StorageKey);
        }
        var data = new TheoryData<string>();
        foreach (var name in names) data.Add(name);
        return data;
    }

    // Runs both sides and requires the same outcome: both finish, or both throw the same error
    // type with the same message. True when both finish.
    private static bool AlikeOutcome(Action viaReadWrite, Action viaPersist, string label)
    {
        var streamError = Record.Exception(viaReadWrite);
        var treeError = Record.Exception(viaPersist);
        Assert.True(streamError?.GetType() == treeError?.GetType(),
            label + ": ReadWrite " + (streamError?.GetType().Name ?? "finished") + ", the tree " + (treeError?.GetType().Name ?? "finished") + ". " + (treeError ?? streamError));
        Assert.Equal(streamError?.Message, treeError?.Message);
        return streamError is null;
    }
}
