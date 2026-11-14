using System.Buffers.Binary;
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Formats;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game.Draw.Images;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

/// <summary>
/// The resource payload adapters of <see cref="PersistSupport"/> load what the stream methods load.
/// An image sequence list loaded through the tree draws <c>Random</c> once per frame-table layer, in
/// the order and at the point of <c>TImageSequence.LoadFromStream</c>, so a load from the same seed
/// ends on the same seed with the same image indices. The terrain list, the ILB library and the
/// unit index list collection load the same state as their stream loads, through both backends.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class ResourcePayloadTests
{
    private const int Seed = 20261112;

    private static readonly FormatSchema s_schema = FormatSchema.Current;
    private static EnginePersistableFactory? s_factory;

    private static EnginePersistableFactory Factory => s_factory ??= new(EngineFixture.Engine);

    [Fact]
    public void ASequenceListLoadedThroughTheTreeDrawsWhereTheStreamLoadDraws()
    {
        var payload = Sequences();
        int seed = RtlUnit.RandSeed;
        try
        {
            using var viaStream = new ImageSequenceList(null, null);
            RtlUnit.RandSeed = Seed;
            viaStream.LoadFromStream(new MemoryStream(payload));
            int streamSeed = RtlUnit.RandSeed;
            // Three frame-table layers draw three times.
            Assert.Equal(Advance(Seed, draws: 3), streamSeed);
            var stored = Stored("MapObjectResource", "ImageSequenceList", payload);
            foreach (var tree in Backends("MapObjectResource", stored))
            {
                var viaTree = new Holder((persister, self) => PersistSupport.ImageSequences(persister, name: "ImageSequenceList", list: self.Sequences));
                RtlUnit.RandSeed = Seed;
                TreeReader.Read(tree, objectValue: viaTree, factory: Factory);
                Assert.Equal(streamSeed, RtlUnit.RandSeed);
                Assert.Equal(Indices(viaStream), Indices(viaTree.Sequences));
                Assert.Equal(Saved(viaStream.SaveToStream), Saved(viaTree.Sequences.SaveToStream));
                viaTree.Dispose();
            }
            // The save writes what SaveToStream writes and draws nothing.
            RtlUnit.RandSeed = Seed;
            var written = TreeWriter.Write(new Holder((persister, _) => PersistSupport.ImageSequences(persister, name: "ImageSequenceList", list: viaStream)), factory: Factory);
            Assert.Equal(Seed, RtlUnit.RandSeed);
            Assert.Equal(Saved(viaStream.SaveToStream), Payload("MapObjectResource", "ImageSequenceList", written));
        }
        finally
        {
            RtlUnit.RandSeed = seed;
        }
    }

    [Fact]
    public void AnAbsentSequenceListLeavesTheListAndDrawsNothing()
    {
        int seed = RtlUnit.RandSeed;
        try
        {
            var holder = new Holder((persister, self) => PersistSupport.ImageSequences(persister, name: "ImageSequenceList", list: self.Sequences));
            holder.Sequences.LoadFromStream(new MemoryStream(Sequences()));
            RtlUnit.RandSeed = Seed;
            TreeReader.Read(new ObjectNode(null, null), objectValue: holder, factory: Factory);
            Assert.Equal(Seed, RtlUnit.RandSeed);
            Assert.Equal(3, holder.Sequences.Count);
            holder.Dispose();
        }
        finally
        {
            RtlUnit.RandSeed = seed;
        }
    }

    // TTerrainList.SaveToStream (HSEngine.pas:1569) packs the trailing pairs of none and writes the
    // count and the pairs, LoadFromStream (HSEngine.pas:1562) reads them back. The stream methods
    // are gone with the last ReadWrite that called them (issue 185 PE-3-i), so the image is spelled
    // out: two pairs, terrain 1 with no overlay (-2), then no terrain with overlay 4.
    [Fact]
    public void ATerrainListStoresAndLoadsThePackedPairs()
    {
        var source = new TerrainList();
        source.SetTerrain(0, 1);
        source.SetOverlay(1, 4);
        source.SetTerrain(3, -2);
        var written = TreeWriter.Write(new Holder((persister, _) => PersistSupport.TerrainList(persister, name: "TerrainList", list: source)), factory: Factory);
        byte[] payload = [2, 0, 0, 0, 1, 0xFE, 0xFE, 4];
        Assert.Equal(2, source.Count);
        Assert.Equal(payload, Payload("MapObjectResource", "TerrainList", written));
        foreach (var tree in Backends("MapObjectResource", Stored("MapObjectResource", "TerrainList", payload)))
        {
            var loaded = new TerrainList();
            loaded.SetTerrain(5, 9);
            TreeReader.Read(tree, objectValue: new Holder((persister, _) => PersistSupport.TerrainList(persister, name: "TerrainList", list: loaded)), factory: Factory);
            Assert.Equal(2, loaded.Count);
            Assert.Equal([1, -2], new[] { loaded.GetTerrain(0), loaded.GetTerrain(1) });
            Assert.Equal([-2, 4], new[] { loaded.GetOverlay(0), loaded.GetOverlay(1) });
        }
    }

    [Fact]
    public void AnImageLibraryLoadsThroughItsStreamLoad()
    {
        var payload = Library();
        using var viaStream = new ImageLibrary(null!);
        viaStream.LoadFromStream(new MemoryStream(payload));
        foreach (var tree in Backends("MapObjectResource", Stored("MapObjectResource", "ImageLibrary", payload)))
        {
            using var loaded = new ImageLibrary(null!);
            TreeReader.Read(tree, objectValue: new Holder((persister, _) => PersistSupport.ImageLibrary(persister, name: "ImageLibrary", library: loaded)), factory: Factory);
            Assert.Equal(viaStream.Count, loaded.Count);
        }
    }

    [Fact]
    public void AUnitIndexListCollectionIsItsFlatList()
    {
        using var source = new UnitIndexListCollection();
        source.Units.LoadFromStream(new MemoryStream(Flat(-1, [198, 201, 199, -1, -1, -1, -1, -1, 203, 200])));
        var written = TreeWriter.Write(new Holder((persister, _) => PersistSupport.UnitIndexListCollection(persister, name: "Defenders", collection: source)), factory: Factory);
        var payload = Saved(source.Units.SaveToStream);
        Assert.Equal(payload, Payload("StructureResource", "Defenders", written));
        var document = DocumentBackend.Write(written, s_schema.Get("StructureResource"), s_schema, "game");
        Assert.Equal("""{"groups": [[198, 201, 199], [203, 200]], "count": 10}""", Compact(document["Defenders"]!));
        foreach (var tree in Backends("StructureResource", Stored("StructureResource", "Defenders", payload)))
        {
            using var loaded = new UnitIndexListCollection();
            TreeReader.Read(tree, objectValue: new Holder((persister, _) => PersistSupport.UnitIndexListCollection(persister, name: "Defenders", collection: loaded)), factory: Factory);
            Assert.Equal(payload, Saved(loaded.Units.SaveToStream));
            Assert.Equal(3, loaded.GetCollectionCount(0));
            Assert.Equal(2, loaded.GetCollectionCount(1));
        }
    }

    // A load sets the capacity in whole 32-bit words and extends the count to it, as Delphi's
    // TBitList.SetCapacity does through Put, so a stored list loads whole only at a count of a
    // multiple of 32. A list of count 13 saves 2 bytes. Count 32 with the default byte 2 loads the
    // same image through the stream and through the tree.
    [Fact]
    public void ABitListLoadsTheImageTheStreamLoads()
    {
        var counted = new BitList { Count = 13 };
        counted[0] = true;
        counted[2] = true;
        counted[12] = true;
        var saved = TreeWriter.Write(new Holder((persister, _) => PersistSupport.BitList(persister, name: "AIValidAttackPlayers", list: counted)), factory: Factory);
        Assert.Equal(Saved(counted.SaveToStream), Payload("Player", "AIValidAttackPlayers", saved));
        Assert.Equal(7, Saved(counted.SaveToStream).Length);

        byte[] payload = [2, 32, 0, 0, 0, 0x05, 0x10, 0x00, 0x80];
        var source = new BitList();
        source.LoadFromStream(new MemoryStream(payload));
        var written = TreeWriter.Write(new Holder((persister, _) => PersistSupport.BitList(persister, name: "AIValidAttackPlayers", list: source)), factory: Factory);
        Assert.Equal(payload, Payload("Player", "AIValidAttackPlayers", written));
        foreach (var tree in Backends("Player", Stored("Player", "AIValidAttackPlayers", payload)))
        {
            var loaded = new BitList();
            loaded.Add(true);
            TreeReader.Read(tree, objectValue: new Holder((persister, _) => PersistSupport.BitList(persister, name: "AIValidAttackPlayers", list: loaded)), factory: Factory);
            Assert.Equal(payload, Saved(loaded.SaveToStream));
            Assert.True(loaded[31]);
            Assert.False(loaded[30]);
        }
        var untouched = new BitList();
        untouched.Add(true);
        var before = Saved(untouched.SaveToStream);
        TreeReader.Read(new ObjectNode(null, null), objectValue: new Holder((persister, _) => PersistSupport.BitList(persister, name: "AIValidAttackPlayers", list: untouched)), factory: Factory);
        Assert.Equal(before, Saved(untouched.SaveToStream));
    }

    // The seed after the given number of Random calls.
    private static int Advance(int seed, int draws)
    {
        for (int i = 0; i < draws; i++) seed = unchecked(seed * 0x08088405 + 1);
        return seed;
    }

    // The stored image index of every layer, in entry and layer order.
    private static List<int> Indices(ImageSequenceList list)
    {
        var indices = new List<int>();
        for (int i = 0; i < list.Count; i++)
            for (var sequence = list.GetImageSequence(i); sequence != null; sequence = sequence.Next)
                indices.Add(sequence.ImageIndex);
        return indices;
    }

    private static byte[] Saved(Action<Stream> save)
    {
        using var memory = new MemoryStream();
        save(memory);
        return memory.ToArray();
    }

    // The tree of one stored payload, read by the direct path from its bytes.
    private static ObjectNode Stored(string className, string property, byte[] payload)
    {
        var prop = s_schema.Get(className).PropertyByName(property)!;
        var context = new DecodeContext(payload, s_schema, "game", "game", null, FormatLimits.Default, new LayoutCounts());
        var node = new ObjectNode(null, null);
        node.Add(property, prop.Codec.ReadImage(context, 0, payload.Length, new ImageFrame { Property = prop, Id = prop.Id!.Value, Path = DocumentNodePath.Root.Member(property) }));
        return node;
    }

    // The bytes the direct path writes for one member of a tree.
    private static byte[] Payload(string className, string property, ObjectNode tree)
    {
        var prop = s_schema.Get(className).PropertyByName(property)!;
        var context = new EncodeContext(s_schema, "game", "game", null, FormatLimits.Default, ExportOptions.Default);
        var data = new ByteBuffer();
        prop.Codec.WriteImage(tree.Find(property)!, context, data, new ImageFrame { Property = prop, Id = prop.Id!.Value, Path = DocumentNodePath.Root.Member(property) });
        return data.ToArray();
    }

    // The same tree read back from its object image and from its document.
    private static IEnumerable<ObjectNode> Backends(string className, ObjectNode tree)
    {
        var classValue = s_schema.Get(className);
        yield return OriginalBackend.Read(OriginalBackend.Write(tree, classValue, s_schema, "game"), classValue, s_schema, "game");
        yield return DocumentBackend.Read(DocumentBackend.Write(tree, classValue, s_schema, "game"), classValue, s_schema, "game");
    }

    // Three entries: a chain of a FrameTable, a RandomFrameTable and a plain layer, a nil entry, and
    // a chain of a Random layer and a FrameTable layer. The frame-table layers store image indices
    // the load replaces.
    private static byte[] Sequences()
    {
        using var memory = new MemoryStream();
        using var writer = new BinaryWriter(memory);
        writer.Write(3);
        writer.Write((byte)0);
        Layer(writer, library: "Units", mode: SequenceMode.FrameTable, imageIndex: 99, frames: [3, 5, 7], next: true);
        Layer(writer, library: "Units", mode: SequenceMode.RandomFrameTable, imageIndex: 0, frames: [1, 2], next: true);
        Layer(writer, library: "", mode: SequenceMode.None, imageIndex: 6, frames: null, next: false);
        writer.Write((byte)1);
        writer.Write((byte)0);
        Layer(writer, library: "fx", mode: SequenceMode.Random, imageIndex: 4, frames: null, next: true);
        Layer(writer, library: "fx", mode: SequenceMode.FrameTable, imageIndex: 70, frames: [8], next: false);
        writer.Flush();
        return memory.ToArray();
    }

    private static void Layer(BinaryWriter writer, string library, SequenceMode mode, int imageIndex, int[]? frames, bool next)
    {
        writer.Write(library.Length);
        writer.Write(Cp1252.Encode(library));
        writer.Write((byte)1);
        writer.Write((byte)2);
        writer.Write((byte)mode);
        writer.Write(imageIndex);
        writer.Write(-3);
        writer.Write(5);
        if (frames is not null)
        {
            writer.Write(0);
            writer.Write(frames.Length);
            foreach (int frame in frames) writer.Write(frame);
        }
        writer.Write(next ? (byte)1 : (byte)0);
    }

    // The smallest library the writer makes: the header, no palette and the index terminator.
    private static byte[] Library()
    {
        var bytes = new byte[32];
        "\u0004ILB\0"u8.CopyTo(bytes);
        BinaryPrimitives.WriteSingleLittleEndian(bytes.AsSpan(8), 4f);
        BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(12), 24);
        BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(16), 32);
        BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(20), 32);
        BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(28), -1);
        return bytes;
    }

    // A TIntegerList image with its canonical runs.
    private static byte[] Flat(int defaultValue, int[] items)
    {
        using var memory = new MemoryStream();
        using var writer = new BinaryWriter(memory);
        writer.Write(defaultValue);
        writer.Write(items.Length);
        int i = 0;
        while (i < items.Length)
        {
            if (items[i] != defaultValue)
            {
                writer.Write(items[i++]);
                continue;
            }
            int run = 0;
            while (i < items.Length && items[i] == defaultValue)
            {
                run++;
                i++;
            }
            writer.Write(defaultValue);
            writer.Write(run);
        }
        writer.Flush();
        return memory.ToArray();
    }

    private static string Compact(Formats.Json.JsonValue value)
    {
        var text = new System.Text.StringBuilder();
        Formats.Json.JsonWriter.WriteCompact(text, value);
        return text.ToString();
    }

    // One object whose persist body is the adapter under test.
    private sealed class Holder(Action<IPersister, Holder> body) : IPersistable, IDisposable
    {
        public ImageSequenceList Sequences { get; } = new(null, null);

        public void Persist(IPersister persister) => body(persister, this);

        public void Dispose() => Sequences.Dispose();
    }
}
