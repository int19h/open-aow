using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats;
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
        ProductionOrder order = ProductionOrder.FromFields(kind, parameter, groupId);
        ProductionItem item = new(null!) { Holder = EngineFixture.Engine, Order = order };
        JsonObject document = DocumentBackend.Write(TreeWriter.Write(item, Factory), classValue: s_schema.Get("ProductionItem"), s_schema, engine: "game");
        _ = Assert.IsType<JsonString>(document["$case"]);
        Check("ProductionItem", item);
        using ProductionItem loaded = new(null!) { Holder = EngineFixture.Engine };
        TreeReader.Read(DocumentBackend.Read(document, classValue: s_schema.Get("ProductionItem"), s_schema, engine: "game"), objectValue: loaded, Factory);
        Assert.Equal(order, loaded.Order);
    }

    // A case drops a stored field it does not keep: the kind writes back what FromFields keeps,
    // on both loads.
    [Fact]
    public void AProductionItemDropsAFieldItsCaseDoesNotKeepOnBothLoads()
    {
        SchemaClass classValue = s_schema.Get("ProductionItem");
        byte[] image = PilotImage.Encode(new JsonObject().Add("$case", new JsonString("Unknown")).Add("Kind", new JsonInteger(99))
            .Add("Parameter", new JsonInteger(6)).Add("GroupId", new JsonInteger(3)), className: "ProductionItem");
        using ProductionItem viaReadWrite = new(null!) { Holder = EngineFixture.Engine };
        using ProductionItem viaPersist = new(null!) { Holder = EngineFixture.Engine };
        Load(viaReadWrite, image);
        TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: viaPersist, Factory);
        Assert.Equal(viaReadWrite.Order, viaPersist.Order);
        EqualBytes(Image(viaReadWrite), Image(viaPersist), label: "ProductionItem unknown kind");
    }

    [Fact]
    public void AProductionQueueWithItemsMatchesTheConverter()
    {
        ProductionQueue queue = new(null!) { Holder = EngineFixture.Engine };
        new ProductionItem(null!) { Holder = EngineFixture.Engine, Order = ProductionOrder.FromFields(1, parameter: 4, groupId: 2) }.SetOwner(queue);
        new ProductionItem(null!) { Holder = EngineFixture.Engine, Order = ProductionOrder.FromFields(12, parameter: 0, groupId: 0) }.SetOwner(queue);
        Check("ProductionQueue", queue);
    }

    // MapContainer.ReadWrite makes its owner's map hold the container on a load, so the container
    // loads into a map on both paths.
    [Fact]
    public void AMapContainerLoadsIntoItsMapOnBothPaths()
    {
        WithDestroyingMap(map =>
        {
            SchemaClass classValue = s_schema.Get("MapContainer");
            using MapContainer source = new(map) { Holder = EngineFixture.Engine };
            byte[] image = Image(source);
            ObjectNode tree = TreeWriter.Write(source, Factory);
            EqualJson(PilotImage.Decode(image, className: "MapContainer"), DocumentBackend.Write(tree, classValue, s_schema, engine: "game"), label: "MapContainer write");
            EqualBytes(image, OriginalBackend.Write(tree, classValue, s_schema, engine: "game"), label: "MapContainer original");
            using MapContainer viaReadWrite = new(map) { Holder = EngineFixture.Engine };
            Load(viaReadWrite, image);
            Assert.Same(viaReadWrite, map.MapContainer);
            using MapContainer viaPersist = new(map) { Holder = EngineFixture.Engine };
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: viaPersist, Factory);
            Assert.Same(viaPersist, map.MapContainer);
            EqualBytes(Image(viaReadWrite), Image(viaPersist), label: "MapContainer read");
            map.MapContainer = null;
        });
    }

    // The original map's children are the selection, four visibility lists and the timer, so its
    // map container stores at slot 6. The timer is gone (PE-3-i part 2): the live list holds the
    // container at index 5 with no placeholder, and both backends still store and read it at slot
    // 6, which loads back at index 5.
    [Fact]
    public void AMapStoresItsContainerAtTheOriginalSlotSix()
    {
        using GameMap map = new(null) { Holder = EngineFixture.Engine };
        MapContainer container = new(map) { Holder = EngineFixture.Engine };
        Assert.Equal(6, map.Count);
        Assert.Same(container, map.GetChild(5));
        SchemaClass classValue = s_schema.Get("GameMap");
        ObjectNode tree = TreeWriter.Write(map, Factory);
        Assert.Equal([6], ChildSlots(tree));
        ObjectNode read = OriginalBackend.Read(OriginalBackend.Write(tree, classValue, s_schema, engine: "game"), classValue: classValue, schema: s_schema, engine: "game");
        Assert.Equal([6], ChildSlots(read));
        Assert.Equal([6], ChildSlots(DocumentBackend.Read(DocumentBackend.Write(tree, classValue, s_schema, engine: "game"), classValue, s_schema, engine: "game")));
        using GameMap loaded = new(null) { Holder = EngineFixture.Engine };
        TreeReader.Read(read, objectValue: loaded, Factory);
        Assert.Equal(6, loaded.Count);
        _ = Assert.IsType<MapContainer>(loaded.GetChild(5));
        Assert.Same(loaded.GetChild(5), loaded.MapContainer);
        Assert.True(tree.ValueEquals(TreeWriter.Write(loaded, Factory)));
    }

    private static List<int> ChildSlots(ObjectNode tree)
        => [.. tree.Members.Where(member => member.Name == "Children").SelectMany(member => ((ChildListNode)member.Value).Children).Select(child => child.Slot ?? -1)];

    // Sign.ReadWrite stores TextSeen only when a map is current and is not a network map. A
    // network map's write leaves it out, and both loads leave a stored one unread.
    [Fact]
    public void ASignOnANetworkMapLeavesItsTextSeenOnBothPaths()
    {
        WithDestroyingMap(map =>
        {
            SchemaClass classValue = s_schema.Get("Sign");
            using Sign source = new(null) { Holder = EngineFixture.Engine };
            _ = source.Text.Add("Welcome");
            source.TextSeen[2] = true;
            byte[] local = Image(source);
            Assert.NotNull(DocumentBackend.Write(TreeWriter.Write(source, Factory), classValue, s_schema, engine: "game").Find("TextSeen"));
            map.ConnectionType = PlayerConnectionType.Network;
            byte[] network = Image(source);
            ObjectNode tree = TreeWriter.Write(source, Factory);
            Assert.Null(DocumentBackend.Write(tree, classValue, s_schema, engine: "game").Find("TextSeen"));
            EqualBytes(network, OriginalBackend.Write(tree, classValue, s_schema, engine: "game"), label: "Sign network write");
            using Sign viaReadWrite = new(null) { Holder = EngineFixture.Engine };
            using Sign viaPersist = new(null) { Holder = EngineFixture.Engine };
            _ = AlikeOutcome(() => Load(viaReadWrite, image: local), viaPersist: () => TreeReader.Read(OriginalBackend.Read(local, classValue, s_schema, engine: "game"), objectValue: viaPersist, Factory), label: "Sign network load");
            Assert.False(viaPersist.TextSeen[2]);
            Assert.Equal(viaReadWrite.TextSeen[2], actual: viaPersist.TextSeen[2]);
        });
    }

    // The game's loads take the stored bits only, for each of the three stored bit lists (issue
    // 396, deviations.md). The count is raised to the capacity as TBitList.SetCapacity raises it on
    // every growth, and the bits past the stored ones keep the default. The image after the list
    // holds bytes the original load would take, and the document of the same tree loads alike.
    [Theory]
    [InlineData("Sign", "TextSeen")]
    [InlineData("PlayerStructure", "VictoryLocations")]
    [InlineData("Player", "AIValidAttackPlayers")]
    public void TheGamesLoadTakesOnlyTheStoredBits(string className, string member)
    {
        SchemaClass classValue = s_schema.Get(className);
        BitListProbe source = new(member);
        source.List[1] = true;
        Assert.Equal(2, source.List.Count);
        byte[] stored = OriginalBackend.Write(TreeWriter.Write(source, Factory), classValue, s_schema, engine: "game");
        ObjectNode tree = ReadPrefix([.. stored, 0x03, 0x00, 0x1F, 0x00], end: stored.Length, classValue: classValue);
        Assert.False(Factory.EmulateOriginalBugs);
        BitListProbe fromImage = new(member);
        TreeReader.Read(tree, objectValue: fromImage, Factory);
        Assert.Equal(32, fromImage.List.Count);
        Assert.Equal([1], SetBits(fromImage.List));
        BitListProbe fromDocument = new(member);
        TreeReader.Read(DocumentBackend.Read(DocumentBackend.Write(tree, classValue, s_schema, engine: "game"), classValue, s_schema, engine: "game"), objectValue: fromDocument, Factory);
        Assert.Equal(32, fromDocument.List.Count);
        Assert.Equal([1], SetBits(fromDocument.List));
        BitListProbe emulated = new(member);
        TreeReader.Read(tree, objectValue: emulated, factory: EmulatingFactory);
        Assert.Equal([1, 8, 9, 24, 25, 26, 27, 28], SetBits(emulated.List));
    }

    // Issue 396: TBitList.LoadFromStream raises a stored count to its capacity, a multiple of 32,
    // and reads the bit bytes of the raised count, so a list whose count does not fill 32 bits
    // takes the bytes the stream holds after it (Engine.pas 4743 to 4789). The Northern Trade
    // Route sign of the user's cmp12 save stores TextSeen with count 2 and bit 1, as the first Put
    // on an empty list leaves it, and the map fields after it store 03 00 1F. The original load
    // takes those bytes as bits 8, 9 and 24 to 28. The port fixes that bug (deviations.md), and a
    // load that emulates it, as the hosts that compare with the original run, takes them from the
    // image. The document of that tree stores the count and bit the image stores. The probe moves
    // the sign's TextSeen as Sign.PersistFields does, without the map object resource a sign's
    // load links.
    [Fact]
    public void ASignLoadThatEmulatesTheOriginalTakesTheBytesPastItsStoredTextSeen()
    {
        SchemaClass classValue = s_schema.Get("Sign");
        TextSeenProbe source = new();
        source.TextSeen[1] = true;
        Assert.Equal(2, source.TextSeen.Count);
        byte[] sign = OriginalBackend.Write(TreeWriter.Write(source, Factory), classValue, s_schema, engine: "game");
        byte[] image = [.. sign, 0x03, 0x00, 0x1F, 0x00];
        ObjectNode tree = ReadPrefix(image, end: sign.Length, classValue: classValue);
        Assert.Equal([0x03, 0x00, 0x1F], ((BitListNode)tree.Find("TextSeen")!).ReadPast!.Value.ToArray());
        TextSeenProbe loaded = new();
        TreeReader.Read(tree, objectValue: loaded, factory: EmulatingFactory);
        Assert.Equal(32, loaded.TextSeen.Count);
        Assert.Equal([1, 8, 9, 24, 25, 26, 27, 28], SetBits(loaded.TextSeen));
        JsonObject document = DocumentBackend.Write(tree, classValue, s_schema, engine: "game");
        Assert.Equal("""{"default":false,"count":2,"bits":[1]}""", CompactJson(document["TextSeen"]!));
        // A save stores the raised count and the bits the load took, as the original's save does.
        BitListNode saved = (BitListNode)TreeWriter.Write(loaded, Factory).Find("TextSeen")!;
        Assert.Equal(32, saved.Count);
        Assert.Equal([1, 8, 9, 24, 25, 26, 27, 28], saved.Indices);

        // At the end of the stream the read takes the bytes there are, and the rest of the bits
        // keep the default SetCapacity put there.
        TextSeenProbe shortLoaded = new();
        TreeReader.Read(ReadPrefix([.. sign, 0x03], end: sign.Length, classValue: classValue), objectValue: shortLoaded, factory: EmulatingFactory);
        Assert.Equal(32, shortLoaded.TextSeen.Count);
        Assert.Equal([1, 8, 9], SetBits(shortLoaded.TextSeen));

        // A list that ends the image reads no byte, and the raised count keeps the default.
        ObjectNode endTree = ReadPrefix(sign, end: sign.Length, classValue: classValue);
        Assert.True(((BitListNode)endTree.Find("TextSeen")!).ReadPast!.Value.IsEmpty);
        TextSeenProbe endLoaded = new();
        TreeReader.Read(endTree, objectValue: endLoaded, factory: EmulatingFactory);
        Assert.Equal(32, endLoaded.TextSeen.Count);
        Assert.Equal([1], SetBits(endLoaded.TextSeen));

        // A document carries no image, so its load leaves the bits past the stored ones at the default.
        // Only the owner rebuild of Player.AIValidAttackPlayers emulates a document load.
        TextSeenProbe fromDocument = new();
        TreeReader.Read(DocumentBackend.Read(document, classValue, s_schema, engine: "game"), objectValue: fromDocument, factory: EmulatingFactory);
        Assert.Equal(32, fromDocument.TextSeen.Count);
        Assert.Equal([1], SetBits(fromDocument.TextSeen));
    }

    // A count that fills its 32-bit words reads no byte past the stored bits, also when the load
    // emulates the original.
    [Fact]
    public void AFullBitListReadsNoBytePastItsStoredBits()
    {
        SchemaClass classValue = s_schema.Get("Sign");
        TextSeenProbe source = new();
        source.TextSeen[1] = true;
        source.TextSeen.Count = 32;
        byte[] sign = OriginalBackend.Write(TreeWriter.Write(source, Factory), classValue, s_schema, engine: "game");
        ObjectNode tree = ReadPrefix([.. sign, 0x03, 0x00, 0x1F, 0x00], end: sign.Length, classValue: classValue);
        Assert.True(((BitListNode)tree.Find("TextSeen")!).ReadPast!.Value.IsEmpty);
        TextSeenProbe loaded = new();
        TreeReader.Read(tree, objectValue: loaded, factory: EmulatingFactory);
        Assert.Equal(32, loaded.TextSeen.Count);
        Assert.Equal([1], SetBits(loaded.TextSeen));
    }

    // A load that emulates the original's bugs, as the hosts that compare with the original run.
    private static EnginePersistableFactory EmulatingFactory => field ??= new(EngineFixture.Engine) { EmulateOriginalBugs = true };

    // One stored bit list, as its owner's PersistFields moves it.
    private sealed class BitListProbe(string name) : IPersistable
    {
        public BitList List { get; } = [];

        public void Persist(IPersister persister) => _ = PersistSupport.BitList(persister, name: name, list: List);
    }

    // The sign's TextSeen as Sign.PersistFields moves it.
    private sealed class TextSeenProbe : IPersistable
    {
        public BitList TextSeen { get; } = [];

        public void Persist(IPersister persister) => _ = PersistSupport.BitList(persister, name: "TextSeen", list: TextSeen);
    }

    private static List<int> SetBits(BitList list)
    {
        List<int> set = [];
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i])
            {
                set.Add(i);
            }
        }

        return set;
    }

    // The object image that ends at an offset of a longer image, as a load reads it from a stream.
    private static ObjectNode ReadPrefix(byte[] image, int end, SchemaClass classValue)
        => ImageTree.Read(new DecodeContext(image, schema: s_schema, engine: "game", profile: "game", file: null, limits: FormatLimits.Default), start: 0, end: end, classValue: classValue);

    private static string CompactJson(JsonValue value)
        => System.Text.Encoding.UTF8.GetString(JsonWriter.ToUtf8(value)).Replace(" ", "", comparisonType: StringComparison.Ordinal).Replace("\n", "", comparisonType: StringComparison.Ordinal);

    // PE-3-i part 2: the world classes have no ReadWrite, so the translate stream visits them
    // through their tree methods and visits what the original ReadWrite visited under its mode
    // (read, text only, translate). A text item's lines but not its name, a sign's lines and not its
    // resource, a structure resource's name and description but not its sound, and a map level's
    // fields with their stored sprites. A map visits its name, author and description, leaves its
    // songs out and keeps its modified flag, where a save resets it.
    [Fact]
    public void TheTranslateStreamVisitsTheWorldStringsTheOriginalReadWriteVisited()
    {
        WithDestroyingMap(map =>
        {
            Dictionary<string, string> dictionary = new()
            {
                ["Welcome"] = "Willkommen",
                ["Intro"] = "Einleitung",
                ["Beware"] = "Vorsicht",
                ["Tower"] = "Turm",
                ["bell.wav"] = "glocke.wav",
                ["A tall tower"] = "Ein hoher Turm",
            };
            using TextControl texts = new(null!) { Holder = EngineFixture.Engine };
            TextControlItem item = new(null!) { Holder = EngineFixture.Engine, Id = 3, Name = "Intro" };
            _ = item.Text.Add("Welcome");
            item.SetOwner(texts);
            using (DictionaryStream stream = new(dictionary))
            {
                stream.Object(texts, id: 0);
                Assert.Equal(["lines 1"], stream.Visited);
                Assert.Equal(["TextControl", "TextControlItem"], stream.Objects);
            }
            Assert.Equal("Willkommen", item.Text[0]);
            Assert.Equal("Intro", item.Name);

            using Sign sign = new(null!) { Holder = EngineFixture.Engine };
            _ = sign.Text.Add("Beware");
            using (DictionaryStream stream = new(dictionary))
            {
                stream.Object(sign, id: 0);
                Assert.Equal(["lines 1"], stream.Visited);
            }
            Assert.Equal("Vorsicht", sign.Text[0]);

            using StructureResource resource = new(null!) { Holder = EngineFixture.Engine };
            _ = resource.Description.Add("A tall tower");
            typeof(StructureResource).GetProperty(nameof(StructureResource.SelectSfx))!.SetValue(resource, value: "bell.wav");
            using (DictionaryStream stream = new(dictionary))
            {
                stream.Object(resource, id: 0);
                Assert.Contains("lines 1", collection: stream.Visited);
                Assert.DoesNotContain("bell.wav", collection: stream.Visited);
            }
            Assert.Equal("Ein hoher Turm", resource.Description[0]);
            Assert.Equal("bell.wav", resource.SelectSfx);

            using MapLevel level = new(null!) { Holder = EngineFixture.Engine };
            level.CreateMapFieldList(width: 1, height: 1);
            MapField field = level.GetField(0, 0);
            Sign post = new(null!) { Holder = EngineFixture.Engine };
            _ = post.Text.Add("Beware");
            // A sign's show priority reads its resource, which the fixture does not load, so the
            // sprite goes into the field's list directly.
            post.SetOwnerWithoutNotify(field);
            field.HexSpriteList = [post];
            field.StoredCount = 1;
            using (DictionaryStream stream = new(dictionary))
            {
                stream.Object(level, id: 0);
                Assert.Equal(["lines 1"], stream.Visited);
                Assert.Equal(["MapLevel", "MapField", "Sign"], stream.Objects);
            }
            Assert.Equal("Vorsicht", post.Text[0]);

            map.MapName = "Tower";
            _ = map.Description.Add("Welcome");
            map.SetModified();
            using (DictionaryStream stream = new(dictionary))
            {
                stream.Object(map, id: 0);
                Assert.Contains("Tower", collection: stream.Visited);
                Assert.Contains("TextControl", collection: stream.Objects);
                Assert.DoesNotContain("StartupSongList", collection: stream.Objects);
                Assert.DoesNotContain("SongList", collection: stream.Objects);
            }
            Assert.Equal("Turm", map.MapName);
            Assert.Equal("Willkommen", map.Description[0]);
            Assert.True(map.Status.Contains(HexSpriteMapStatuses.Modified));
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
        WithDestroyingMap(destroyingMap =>
        {
            SchemaClass classValue = s_schema.Get(className);
            EngineObject source = Blank(className);
            EngineObject? viaReadWrite = null;
            EngineObject? viaPersist = null;
            try
            {
                if (source is ItemHexSprite sprite)
                {
                    PlainItem(itemId: 3, name: "Ring").SetOwner(sprite.Items);
                }

                byte[]? image = null;
                ObjectNode? tree = null;
                _ = AlikeOutcome(() => image = Image(source), viaPersist: () => tree = TreeWriter.Write(source, Factory), label: $"{className} write");
                if (image is null || tree is null)
                {
                    return;
                }

                EqualJson(PilotImage.Decode(image, className), DocumentBackend.Write(tree, classValue, s_schema, engine: "game"), label: $"{className} write");
                EqualBytes(image, OriginalBackend.Write(tree, classValue, s_schema, engine: "game"), label: $"{className} original");
                EngineObject stream = viaReadWrite = Blank(className);
                EngineObject persisted = viaPersist = Blank(className);
                bool loaded = AlikeOutcome(() => Load(stream, image), viaPersist: () => TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, engine: "game"), objectValue: persisted, Factory), label: $"{className} load");
                if (loaded)
                {
                    EqualBytes(Image(stream), Image(persisted), label: $"{className} read");
                }
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
        SortedSet<string> names = [with(StringComparer.Ordinal)];
        foreach (EngineClass? engineClass in EngineClass.All)
        {
            Type type = engineClass.Type;
            if (type.Assembly != typeof(GamePack).Assembly || type.IsAbstract)
            {
                continue;
            }

            if (type.Namespace is not { } space || !(space.StartsWith("Aow.Game.World", comparisonType: StringComparison.Ordinal) || space.StartsWith("Aow.Game.Draw", comparisonType: StringComparison.Ordinal)))
            {
                continue;
            }

            if (EngineObject.TreePersistenceRefusal(type) is not null || s_worldExceptions.Contains(engineClass.StorageKey))
            {
                continue;
            }

            if (FormatSchema.Current.Find(engineClass.StorageKey) is null)
            {
                continue;
            }

            _ = names.Add(engineClass.StorageKey);
        }
        TheoryData<string> data = [.. names];

        return data;
    }

    // Runs both sides and requires the same outcome: both finish, or both throw the same error
    // type with the same message. True when both finish.
    private static bool AlikeOutcome(Action viaReadWrite, Action viaPersist, string label)
    {
        Exception? streamError = Record.Exception(viaReadWrite);
        Exception? treeError = Record.Exception(viaPersist);
        Assert.True(streamError?.GetType() == treeError?.GetType(),
            userMessage: $"{label}: ReadWrite {streamError?.GetType().Name ?? "finished"}, the tree {treeError?.GetType().Name ?? "finished"}. {treeError ?? streamError}");
        Assert.Equal(streamError?.Message, treeError?.Message);
        return streamError is null;
    }
}
