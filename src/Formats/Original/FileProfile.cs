using System.Buffers.Binary;
using System.Globalization;
using Aow.Formats.Json;
using Aow.Formats.Schema;

namespace Aow.Formats.Original;

/// <summary>
/// An original file profile (own-formats.md 1.1): the container, the engine class table, the
/// root family, the header and checksum rules and the document kinds. Each profile imports and
/// exports through its own virtual methods, so a new profile joins without a switch.
/// </summary>
public abstract class FileProfile
{
    public abstract string Name { get; }
    public abstract string Engine { get; }
    public abstract IReadOnlyList<string> Kinds { get; }
    public abstract string RootFamily { get; }

    /// <summary>Whether the document root carries $class (the stored image names its class).</summary>
    public abstract bool RootHasClass { get; }

    public static IReadOnlyList<FileProfile> All { get; } =
    [
        new HsmProfile("world-hsm", "game", ["save", "map", "campaign-map"], "StrategicMap"),
        new HsmProfile("tactical-hsm", "tactical", ["map"], "CombatMap"),
        new StoredObjectProfile(),
        new ScenarioSetProfile(),
        new WrittenObjectProfile("hero-list", ["hero-list"], "HeroLibrary"),
        new WrittenObjectProfile("item-list", ["item-list"], "ItemLibrary"),
    ];

    /// <summary>The names of the document kinds (DocumentKind.All), which own the extensions.</summary>
    public static IReadOnlyList<string> AllKinds { get; } = [.. DocumentKind.All.Select(k => k.Name)];

    public static FileProfile? Find(string name) => All.FirstOrDefault(p => p.Name == name);

    /// <summary>The bytes G1 compares: the whole file, or for CFS the prefix and the inflated payload.</summary>
    public virtual byte[] Comparable(byte[] file, FormatLimits limits) => file;

    /// <summary>Imports a whole original file into its $container and root.</summary>
    internal abstract (JsonObject Container, JsonObject Root) Import(byte[] file, ImportSession session);

    /// <summary>Exports a document's $container and root to the original file.</summary>
    internal abstract byte[] Export(JsonObject container, JsonValue root, EncodeContext context);

    /// <summary>
    /// The engine image of a document (own-formats.md 3.1 and 4.1): the bytes the engine's stream
    /// code reads once the container is undone. It is a separate operation from the original
    /// export. For an unwrapped profile the image is the original file.
    /// </summary>
    internal virtual byte[] ExportImage(JsonObject container, JsonValue root, EncodeContext context) => Export(container, root, context);

    /// <summary>Imports an engine-written image. The game writes new images in the profile's game container.</summary>
    internal virtual (JsonObject Container, JsonObject Root) ImportImage(byte[] image, ImportSession session) => Import(image, session);

    /// <summary>The bytes G1 compares for the export of a document, built from its engine image and container.</summary>
    internal virtual byte[] ComparableImage(JsonObject container, byte[] image) => image;

    internal DecodeContext Context(byte[] data, ImportSession session, bool inflated) =>
        new(data, session.Schema, Engine, Name, session.File, session.Limits, session.Counts) { Nested = session.Nested, Inflated = inflated };

    internal JsonObject NewContainer(string compression, int? level)
    {
        var container = new JsonObject().Add("profile", new JsonString(Name)).Add("compression", new JsonString(compression));
        if (level is int l) container.Add("level", new JsonInteger(l));
        return container;
    }

    /// <summary>Reads the outer table of a stored image: one entry, id 0, the root.</summary>
    internal static int ReadTop(DecodeContext context, int start, int end, LayoutBuilder layout)
    {
        var table = PropertyTable.Read(context, start, end, DocumentNodePath.Envelope);
        if (table.Entries.Count != 1 || table.Entries[0].Id != 0)
            throw context.Fail(Codes.Corrupt, "The stored image holds other properties than the root, id 0.", start, DocumentNodePath.Envelope);
        var e = table.Entries[0];
        if (e.Long || e.Offset != 0 || table.EmptyLongMarker)
        {
            layout.Table(table.Entries, table.EmptyLongMarker, null);
            if (e.Offset > 0) layout.Gap(0, context.Data.AsSpan(table.DataBase, e.Offset), null);
        }
        return table.DataBase + e.Offset;
    }

    internal static void WriteTop(EncodeContext context, byte[] root, LayoutReader? layout, JsonValue at, ByteBuffer output)
    {
        var writer = new ObjectWriter();
        writer.Begin(0, null);
        writer.Data.Bytes(root);
        writer.End();
        ObjectImage.Place(context, writer, layout, null, null, at, DocumentNodePath.Envelope, output);
    }

    /// <summary>Decodes the root object, with $class when the image stores it.</summary>
    internal JsonObject DecodeRoot(DecodeContext context, int start, int end, SchemaClass classValue)
    {
        var root = new JsonObject();
        if (RootHasClass) root.Add("$class", new JsonString(classValue.Name));
        ObjectImage.Decode(context, start, end, classValue, DocumentNodePath.Root, root);
        return root;
    }

    internal SchemaClass RootClass(EncodeContext context, JsonObject root)
    {
        if (!RootHasClass)
        {
            if (root.Find("$class") is { } m) throw context.Fail(Codes.ReservedMember, $"The {Name} root has a fixed class and no $class.", m.Value, DocumentNodePath.Root.Member("$class"));
            return context.Schema.Get(RootFamily);
        }
        if (root["$class"] is not JsonString name) throw context.Fail(Codes.MissingMember, "The root needs $class.", root, DocumentNodePath.Root);
        var classValue = context.Schema.Find(name.Value) ?? throw context.Fail(Codes.UnknownClass, $"Unknown class {name.Value}.", name, DocumentNodePath.Root.Member("$class"), name.Value);
        if (classValue.Id is not int id || context.Schema.Resolve(Engine, id) != classValue)
            throw context.Fail(Codes.UnknownClass, $"Class {classValue.Name} is not in the {Engine} engine table.", name, DocumentNodePath.Root.Member("$class"), classValue.Name);
        if (!classValue.InheritsFrom(RootFamily))
            throw context.Fail(Codes.ClassFamily, $"Class {classValue.Name} is not a {RootFamily} root for profile {Name}.", name, DocumentNodePath.Root.Member("$class"), classValue.Name, RootFamily);
        return classValue;
    }

    internal static void CheckContainer(EncodeContext context, JsonObject container, params string[] allowed)
    {
        foreach (var m in container.Members)
            if (m.Name != "$layout" && Array.IndexOf(allowed, m.Name) < 0)
                throw new ConversionException(new FormatDiagnostic(m.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                    $"$container has no member \"{m.Name}\".")
                { File = context.File, Span = m.NameSpan, Path = "$container." + m.Name });
    }

    internal static string CompressionOf(EncodeContext context, JsonObject container, out int level)
    {
        level = 0;
        string compression = container["compression"] is JsonString c ? c.Value
            : throw context.Fail(Codes.MissingMember, "$container needs compression.", container, DocumentNodePath.Named("$container"));
        var levelValue = container["level"];
        if (compression == "cfs")
        {
            if (levelValue is not JsonInteger { Value: >= 0 and <= 3 } l)
                throw context.Fail(Codes.Range, "A CFS container needs level 0, 1, 2 or 3.", levelValue ?? container, DocumentNodePath.Named("$container.level"));
            level = (int)l.Value;
        }
        else if (compression == "none")
        {
            if (levelValue is not null) throw context.Fail(Codes.UnknownMember, "level exists only for CFS.", levelValue, DocumentNodePath.Named("$container.level"));
        }
        else throw context.Fail(Codes.Range, "compression is cfs or none.", c, DocumentNodePath.Named("$container.compression"), compression);
        return compression;
    }

    public override string ToString() => Name;
}

/// <summary>Shared state of one import, including a nested campaign map.</summary>
internal sealed class ImportSession
{
    public required FormatSchema Schema { get; init; }
    public required FormatLimits Limits { get; init; }
    public required LayoutCounts Counts { get; init; }
    public string? File { get; init; }
    public string? Nested { get; init; }
}

/// <summary>HSM maps and saves: an optional CFS prefix, the 16-byte header, then the root.</summary>
public sealed class HsmProfile(string name, string engine, string[] kinds, string rootFamily) : FileProfile
{
    public override string Name => name;
    public override string Engine => engine;
    public override IReadOnlyList<string> Kinds => kinds;
    public override string RootFamily => rootFamily;
    public override bool RootHasClass => true;

    public static ReadOnlySpan<byte> Magic => "HSM\0"u8;

    public override byte[] Comparable(byte[] file, FormatLimits limits)
    {
        if (!Cfs.HasMagic(file) || file[4] == 0) return file;
        var inflated = Cfs.Inflate(file.AsSpan(5), limits.DecompressedBytes, m => new ConversionException(new FormatDiagnostic(Codes.Compression, m)));
        var result = new byte[5 + inflated.Length];
        file.AsSpan(0, 5).CopyTo(result);
        inflated.CopyTo(result, 5);
        return result;
    }

    /// <summary>The class id in the header, for detecting the profile of a .hsm file.</summary>
    public static int? HeaderClassId(byte[] file)
    {
        byte[] head;
        if (Cfs.HasMagic(file))
        {
            if (file[4] == 0) head = file[5..Math.Min(file.Length, 21)];
            else
            {
                try
                {
                    head = Cfs.InflatePrefix(file.AsSpan(5), 16);
                }
                catch (InvalidDataException)
                {
                    return null;
                }
            }
        }
        else head = file[..Math.Min(file.Length, 16)];
        if (head.Length < 16 || !head.AsSpan(4, 4).SequenceEqual(Magic)) return null;
        return BinaryPrimitives.ReadInt32LittleEndian(head.AsSpan(12, 4));
    }

    internal override (JsonObject Container, JsonObject Root) Import(byte[] file, ImportSession session)
    {
        JsonObject container;
        byte[] payload;
        bool inflated = false;
        if (Cfs.HasMagic(file))
        {
            int level = file[4];
            if (level > 3)
                throw new ConversionException(new FormatDiagnostic(Codes.Compression, $"CFS level {level} is not 0, 1, 2 or 3.") { File = session.File, Offset = 4, Profile = Name });
            container = NewContainer("cfs", level);
            if (level == 0) payload = file[5..];
            else
            {
                payload = Cfs.Inflate(file.AsSpan(5), session.Limits.DecompressedBytes,
                    m => new ConversionException(new FormatDiagnostic(Codes.Compression, m) { File = session.File, Offset = 5, Profile = Name }));
                inflated = true;
            }
        }
        else
        {
            container = NewContainer("none", null);
            payload = file;
        }
        return ImportPayload(payload, container, inflated, session);
    }

    /// <summary>The game writes its map and save images in CFS level 2 (TCompressStream.SaveToFile).</summary>
    public const int GameLevel = 2;

    internal override (JsonObject Container, JsonObject Root) ImportImage(byte[] image, ImportSession session) =>
        ImportPayload(image, NewContainer("cfs", GameLevel), true, session);

    internal override byte[] ComparableImage(JsonObject container, byte[] image)
    {
        if (container["compression"] is not JsonString { Value: "cfs" } || container["level"] is not JsonInteger level) return image;
        var result = new byte[5 + image.Length];
        Cfs.Magic.CopyTo(result);
        result[4] = (byte)level.Value;
        image.CopyTo(result, 5);
        return result;
    }

    internal override byte[] ExportImage(JsonObject container, JsonValue root, EncodeContext context) => ExportPayload(container, root, context, out _, out _);

    private (JsonObject Container, JsonObject Root) ImportPayload(byte[] payload, JsonObject container, bool inflated, ImportSession session)
    {
        var context = Context(payload, session, inflated);
        if (payload.Length < 16) throw context.Fail(Codes.Truncated, "The HSM header is truncated.", 0);
        int size = context.Int32(0);
        if (!payload.AsSpan(4, 4).SequenceEqual(Magic)) throw context.Fail(Codes.Format, "The HSM header has no HSM magic.", 4);
        if (size < 16 || size > payload.Length) throw context.Fail(Codes.Corrupt, "The HSM header size is outside the payload.", 0, null, size.ToString(CultureInfo.InvariantCulture), $"16 to {payload.Length}");
        int reserved = context.Int32(8);
        int classId = context.Int32(12);
        if (reserved != 0) container.Add("header", new JsonObject().Add("Reserved", new JsonInteger(reserved)));
        var classValue = session.Schema.Resolve(Engine, classId)
            ?? throw context.Fail(Codes.UnknownClass, $"Root class id {classId} is not in the {Engine} engine table.", 12, DocumentNodePath.Root, classId.ToString(CultureInfo.InvariantCulture));
        if (!classValue.InheritsFrom(RootFamily)) throw context.Fail(Codes.ClassFamily, $"Root class {classValue.Name} is not a {RootFamily}.", 12, DocumentNodePath.Root, classValue.Name, RootFamily);
        var layout = new LayoutBuilder();
        if (size > 16) layout.Header(16, payload.AsSpan(16, size - 16));
        int rootStart = ReadTop(context, size, payload.Length, layout);
        var root = DecodeRoot(context, rootStart, payload.Length, classValue);
        layout.Attach(container, Name, session.Counts);
        return (container, root);
    }

    internal byte[] ExportPayload(JsonObject container, JsonValue root, EncodeContext context, out string compression, out int level)
    {
        CheckContainer(context, container, "profile", "compression", "level", "header");
        compression = CompressionOf(context, container, out level);
        int reserved = 0;
        if (container["header"] is { } header)
        {
            if (header is not JsonObject h) throw context.Fail(Codes.Type, "header is an object.", header, DocumentNodePath.Named("$container.header"));
            foreach (var m in h.Members)
            {
                if (m.Name != "Reserved") throw context.Fail(Codes.UnknownMember, $"The HSM header has no member \"{m.Name}\".", m.Value, DocumentNodePath.Named("$container.header." + m.Name));
                if (m.Value is not JsonInteger { Value: >= int.MinValue and <= int.MaxValue } r)
                    throw context.Fail(Codes.Range, "Reserved is a 32-bit integer.", m.Value, DocumentNodePath.Named("$container.header.Reserved"));
                reserved = (int)r.Value;
            }
        }
        if (root is not JsonObject rootObject) throw context.Fail(Codes.Type, "root is an object.", root, DocumentNodePath.Root);
        var classValue = RootClass(context, rootObject);
        var layout = LayoutReader.Read(context, container, DocumentNodePath.Named("$container"));
        var output = new ByteBuffer(1 << 16);
        output.Int32(16);
        output.Bytes(Magic);
        output.Int32(reserved);
        output.Int32(classValue.Id!.Value);
        if (layout is not null)
            foreach (var (offset, bytes, at) in layout.Headers)
            {
                if (offset != 16) throw context.Fail(Codes.Layout, "An HSM header extension starts at offset 16.", at, DocumentNodePath.Named("$container.$layout"));
                output.Bytes(bytes);
                output.PatchInt32(0, 16 + bytes.Length);
            }
        var body = new ByteBuffer(1 << 16);
        ObjectImage.Encode(context, rootObject, classValue, body, DocumentNodePath.Root, "$class");
        WriteTop(context, body.ToArray(), layout, container, output);
        return output.ToArray();
    }

    internal override byte[] Export(JsonObject container, JsonValue root, EncodeContext context)
    {
        var payload = ExportPayload(container, root, context, out string compression, out int level);
        return compression == "cfs" ? Cfs.Wrap(payload, level) : payload;
    }
}

/// <summary>Scenario sets: the 52-byte header, the TAoWHSSet image and a trailing CRC-32.</summary>
public sealed class ScenarioSetProfile : FileProfile
{
    public override string Name => "scenario-set";
    public override string Engine => "game";
    public override IReadOnlyList<string> Kinds => ["scenario-set"];
    public override string RootFamily => "GamePack";
    public override bool RootHasClass => false;

    private const int HeaderSize = 52;

    private static ReadOnlySpan<byte> Id => [4, (byte)'H', (byte)'S', (byte)'S', 0];

    internal override (JsonObject Container, JsonObject Root) Import(byte[] file, ImportSession session)
    {
        var context = Context(file, session, false);
        if (file.Length < HeaderSize + 4) throw context.Fail(Codes.Truncated, "The scenario set header is truncated.", 0);
        if (!file.AsSpan(4, 5).SequenceEqual(Id)) throw context.Fail(Codes.Format, "The scenario set header has no HSS identifier.", 4);
        int size = context.Int32(0);
        if (size < HeaderSize || size > file.Length - 4)
            throw context.Fail(Codes.Corrupt, "The scenario set header size is outside the file.", 0, null, size.ToString(CultureInfo.InvariantCulture), $"{HeaderSize} to {file.Length - 4}");
        uint stored = BinaryPrimitives.ReadUInt32LittleEndian(file.AsSpan(file.Length - 4));
        uint actual = Crc32.Compute(file.AsSpan(0, file.Length - 4));
        if (stored != actual)
            throw context.Fail(Codes.Checksum, "The scenario set checksum does not match its contents.", file.Length - 4, null, stored.ToString("x8", CultureInfo.InvariantCulture), actual.ToString("x8", CultureInfo.InvariantCulture));
        int nameLength = file[9];
        if (nameLength > 40) throw context.Fail(Codes.Corrupt, "The scenario set name is longer than 40 bytes.", 9);
        var container = NewContainer("none", null);
        container.Add("header", new JsonObject().Add("Name", new JsonString(TextCodec.Decode(file.AsSpan(10, nameLength)))));
        var layout = new LayoutBuilder();
        int tail = 10 + nameLength;
        if (file.AsSpan(tail, HeaderSize - tail).ContainsAnyExcept((byte)0)) layout.Header(tail, file.AsSpan(tail, HeaderSize - tail));
        if (size > HeaderSize) layout.Header(HeaderSize, file.AsSpan(HeaderSize, size - HeaderSize));
        int rootStart = ReadTop(context, size, file.Length - 4, layout);
        var root = DecodeRoot(context, rootStart, file.Length - 4, session.Schema.Get(RootFamily));
        layout.Attach(container, Name, session.Counts);
        return (container, root);
    }

    internal override byte[] Export(JsonObject container, JsonValue root, EncodeContext context)
    {
        CheckContainer(context, container, "profile", "compression", "header");
        if (CompressionOf(context, container, out _) != "none") throw context.Fail(Codes.Range, "A scenario set is not compressed.", container["compression"], DocumentNodePath.Named("$container.compression"));
        var header = container["header"] as JsonObject ?? throw context.Fail(Codes.MissingMember, "A scenario set needs header.Name.", container, DocumentNodePath.Named("$container"));
        foreach (var m in header.Members)
            if (m.Name != "Name") throw context.Fail(Codes.UnknownMember, $"The scenario set header has no member \"{m.Name}\".", m.Value, DocumentNodePath.Named("$container.header." + m.Name));
        if (header["Name"] is not JsonString name) throw context.Fail(Codes.MissingMember, "header.Name is a string.", header, DocumentNodePath.Named("$container.header"));
        var nameBytes = TextCodec.Encode(context, name.Value, name, DocumentNodePath.Named("$container.header.Name"));
        if (nameBytes.Length > 40) throw context.Fail(Codes.Range, "The scenario set name holds at most 40 characters.", name, DocumentNodePath.Named("$container.header.Name"));
        if (root is not JsonObject rootObject) throw context.Fail(Codes.Type, "root is an object.", root, DocumentNodePath.Root);
        var classValue = RootClass(context, rootObject);
        var layout = LayoutReader.Read(context, container, DocumentNodePath.Named("$container"));
        var head = new byte[HeaderSize];
        BinaryPrimitives.WriteInt32LittleEndian(head, HeaderSize);
        Id.CopyTo(head.AsSpan(4));
        head[9] = (byte)nameBytes.Length;
        nameBytes.CopyTo(head, 10);
        var extension = Array.Empty<byte>();
        if (layout is not null)
            foreach (var (offset, bytes, at) in layout.Headers)
            {
                if (offset == HeaderSize) extension = bytes;
                else if (offset == 10 + nameBytes.Length && offset + bytes.Length == HeaderSize) bytes.CopyTo(head, offset);
                else throw context.Fail(Codes.Layout, "A scenario set header part does not fit the header.", at, DocumentNodePath.Named("$container.$layout"));
            }
        BinaryPrimitives.WriteInt32LittleEndian(head, HeaderSize + extension.Length);
        var output = new ByteBuffer(1 << 20);
        output.Bytes(head);
        output.Bytes(extension);
        var body = new ByteBuffer(1 << 20);
        ObjectImage.Encode(context, rootObject, classValue, body, DocumentNodePath.Root);
        WriteTop(context, body.ToArray(), layout, container, output);
        output.Int32((int)Crc32.Compute(output.Span));
        return output.ToArray();
    }
}

/// <summary>Campaigns and campaign saves: the root stored with its class id.</summary>
public sealed class StoredObjectProfile : FileProfile
{
    public override string Name => "campaign";
    public override string Engine => "campaign";
    public override IReadOnlyList<string> Kinds => ["campaign", "campaign-save"];
    public override string RootFamily => "Campaign";
    public override bool RootHasClass => true;

    internal override (JsonObject Container, JsonObject Root) Import(byte[] file, ImportSession session)
    {
        var context = Context(file, session, false);
        var container = NewContainer("none", null);
        var layout = new LayoutBuilder();
        int start = ReadTop(context, 0, file.Length, layout);
        context.Need(start, 4, file.Length, DocumentNodePath.Root, "root class id");
        int classId = context.Int32(start);
        var classValue = session.Schema.Resolve(Engine, classId)
            ?? throw context.Fail(Codes.UnknownClass, $"Root class id {classId} is not in the {Engine} engine table.", start, DocumentNodePath.Root, classId.ToString(CultureInfo.InvariantCulture));
        if (!classValue.InheritsFrom(RootFamily)) throw context.Fail(Codes.ClassFamily, $"Root class {classValue.Name} is not a {RootFamily}.", start, DocumentNodePath.Root, classValue.Name, RootFamily);
        var root = DecodeRoot(context, start + 4, file.Length, classValue);
        layout.Attach(container, Name, session.Counts);
        return (container, root);
    }

    internal override byte[] Export(JsonObject container, JsonValue root, EncodeContext context)
    {
        CheckContainer(context, container, "profile", "compression");
        if (CompressionOf(context, container, out _) != "none") throw context.Fail(Codes.Range, "A campaign is not compressed.", container["compression"], DocumentNodePath.Named("$container.compression"));
        if (root is not JsonObject rootObject) throw context.Fail(Codes.Type, "root is an object.", root, DocumentNodePath.Root);
        var classValue = RootClass(context, rootObject);
        var layout = LayoutReader.Read(context, container, DocumentNodePath.Named("$container"));
        var body = new ByteBuffer(1 << 16);
        body.Int32(classValue.Id!.Value);
        ObjectImage.Encode(context, rootObject, classValue, body, DocumentNodePath.Root, "$class");
        var output = new ByteBuffer(1 << 16);
        WriteTop(context, body.ToArray(), layout, container, output);
        return output.ToArray();
    }
}

/// <summary>
/// Hero and item lists: the root written without a class id, fixed by the caller. A test reads a
/// game set file through such a profile, in the engine table its classes come from.
/// </summary>
public sealed class WrittenObjectProfile(string name, string[] kinds, string rootClass, string engine = "game") : FileProfile
{
    public override string Name => name;
    public override string Engine => engine;
    public override IReadOnlyList<string> Kinds => kinds;
    public override string RootFamily => rootClass;
    public override bool RootHasClass => false;

    internal override (JsonObject Container, JsonObject Root) Import(byte[] file, ImportSession session)
    {
        var context = Context(file, session, false);
        var container = NewContainer("none", null);
        var layout = new LayoutBuilder();
        int start = ReadTop(context, 0, file.Length, layout);
        var root = DecodeRoot(context, start, file.Length, session.Schema.Get(RootFamily));
        layout.Attach(container, Name, session.Counts);
        return (container, root);
    }

    internal override byte[] Export(JsonObject container, JsonValue root, EncodeContext context)
    {
        CheckContainer(context, container, "profile", "compression");
        if (CompressionOf(context, container, out _) != "none") throw context.Fail(Codes.Range, $"A {Name} is not compressed.", container["compression"], DocumentNodePath.Named("$container.compression"));
        if (root is not JsonObject rootObject) throw context.Fail(Codes.Type, "root is an object.", root, DocumentNodePath.Root);
        var classValue = RootClass(context, rootObject);
        var layout = LayoutReader.Read(context, container, DocumentNodePath.Named("$container"));
        var body = new ByteBuffer(1 << 12);
        ObjectImage.Encode(context, rootObject, classValue, body, DocumentNodePath.Root);
        var output = new ByteBuffer(1 << 12);
        WriteTop(context, body.ToArray(), layout, container, output);
        return output.ToArray();
    }
}
