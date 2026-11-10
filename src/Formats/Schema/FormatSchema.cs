using System.Reflection;
using System.Security.Cryptography;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Rtl;

namespace Aow.Formats.Schema;

/// <summary>
/// The embedded, hand-owned schema (own-formats.md 2.2): stable class names, parents, class ids
/// and engine enlistments, and per property its stable name, legacy id, codec and canonical
/// ordinal. Ordinary members follow the effective order of each class, which is the inherited
/// ReadWrite call order with the stored children last.
/// </summary>
public sealed class FormatSchema
{
    private readonly Dictionary<string, SchemaClass> classes = new(StringComparer.Ordinal);
    private readonly Dictionary<string, Dictionary<int, SchemaClass>> engines = new(StringComparer.Ordinal);
    private readonly List<string> ambiguities = [];
    private readonly List<(string Engine, int Id)> ambiguous = [];

    /// <summary>Class ids that one engine table enlists for two classes, measured at load.</summary>
    public IReadOnlyList<string> RegistrationAmbiguities => ambiguities;

    /// <summary>The engine tables, by context name.</summary>
    public IReadOnlyCollection<string> Engines => engines.Keys;

    public int Version { get; }

    /// <summary>The schema stores a generic list. The test schema sets this. Product schemas do not.</summary>
    public bool StoresLists { get; }

    /// <summary>SHA-256 of the normalized schema text, pinned in versions.json.</summary>
    public string Hash { get; }

    public IReadOnlyCollection<SchemaClass> Classes => classes.Values;

    private static readonly Lazy<FormatSchema> embedded = new(LoadEmbedded);

    /// <summary>
    /// A test installs a schema here so one migration step can be observed not to read <see cref="Current"/>.
    /// Production leaves it null. <see cref="Current"/> is then the embedded schema after one null comparison.
    /// The persist path does not read this field.
    /// </summary>
    internal static FormatSchema? Substituted;

    public static FormatSchema Current => Substituted ?? embedded.Value;

    private static FormatSchema LoadEmbedded()
    {
        using var stream = typeof(FormatSchema).Assembly.GetManifestResourceStream("Aow.Formats.schema.json")
            ?? throw new InvalidOperationException("The embedded schema is missing.");
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return Load(memory.ToArray(), "schema.json");
    }

    public static FormatSchema Load(byte[] utf8, string file)
    {
        var root = JsonReader.Parse(utf8, file) as JsonObject
            ?? throw new InvalidOperationException("The schema is not an object.");
        return new FormatSchema(root, Convert.ToHexStringLower(SHA256.HashData(JsonWriter.ToUtf8(root))));
    }

    private FormatSchema(JsonObject root, string hash)
    {
        Hash = hash;
        Version = (int)((JsonInteger)root["version"]!).Value;
        StoresLists = root["lists"] is JsonBool { Value: true };
        var list = (JsonArray)root["classes"]!;
        foreach (var item in list.Items)
        {
            var c = new SchemaClass((JsonObject)item, this);
            classes.Add(c.Name, c);
        }
        if (root["legacyShapes"] is JsonArray shapes)
            foreach (var item in shapes.Items)
            {
                var shape = (JsonObject)item;
                var c = new SchemaClass(shape, this, ((JsonString)shape["class"]!).Value);
                legacyShapes.Add((((JsonString)shape["context"]!).Value, c.Name), (((JsonString)shape["name"]!).Value, c));
            }
        foreach (var c in classes.Values) c.Link(this);
        foreach (var (_, c) in legacyShapes.Values) c.Complete();
        foreach (var c in classes.Values) c.Complete();
        foreach (var c in classes.Values)
        {
            foreach (var engine in c.Engines)
            {
                if (c.Id is not int id) throw new InvalidOperationException($"Class {c.Name} is enlisted without an id.");
                if (!engines.TryGetValue(engine, out var table)) engines[engine] = table = [];
                if (!table.TryAdd(id, c))
                {
                    ambiguities.Add($"engine {engine} class id {id}: {table[id].Name} and {c.Name}");
                    ambiguous.Add((engine, id));
                }
            }
        }
        // An ambiguous id resolves to nothing, so a stored instance of it is refused, not guessed.
        foreach (var (engine, id) in ambiguous) engines[engine].Remove(id);
    }

    private readonly Dictionary<(string Context, string Class), (string Rule, SchemaClass Shape)> legacyShapes = [];

    /// <summary>A named legacy shape of a class in one context, such as LEGACY_VALLEY01_SPRITE.</summary>
    public (string Rule, SchemaClass Shape)? LegacyShape(string context, string className)
    {
        foreach (var candidate in Contexts(context))
        {
            var found = Lookup(candidate, className);
            if (found is not null) return found;
        }
        return null;
    }

    private (string Rule, SchemaClass Shape)? Lookup(string context, string className)
    {
        if (legacyShapes.TryGetValue((context, className), out var shape)) return shape;
        string? resolved = NameTable.ResolveStored(className);
        if (resolved is null || resolved == className || !legacyShapes.TryGetValue((context, resolved), out shape)) return null;
        return shape;
    }

    // The stored context names the class with its current name or with the Delphi name.
    private static IEnumerable<string> Contexts(string context)
    {
        yield return context;
        int dot = context.LastIndexOf('.');
        if (dot <= 0) yield break;
        string head = context[..dot];
        string tail = context[(dot + 1)..];
        string? delphi = NameTable.DelphiName(head);
        if (delphi is not null && delphi != head) yield return delphi + "." + tail;
        string? stored = NameTable.ResolveStored(head);
        if (stored is not null && stored != head) yield return stored + "." + tail;
    }

    public SchemaClass? Find(string name)
    {
        if (classes.TryGetValue(name, out var found)) return found;
        string? resolved = NameTable.ResolveStored(name);
        if (resolved is not null && resolved != name && classes.TryGetValue(resolved, out found)) return found;
        // A current schema answers a Delphi name. A version 1 or 2 schema answers the current name.
        string? delphi = resolved is null ? null : NameTable.DelphiName(resolved);
        if (delphi is not null && delphi != name && classes.TryGetValue(delphi, out found)) return found;
        return null;
    }

    public SchemaClass Get(string name) =>
        Find(name) ?? throw new InvalidOperationException($"The schema has no class {name}.");

    /// <summary>Resolves a stored class id through one engine table only (own-formats.md 2.2).</summary>
    public SchemaClass? Resolve(string engine, int classId) =>
        engines.TryGetValue(engine, out var table) ? table.GetValueOrDefault(classId) : null;

    public IReadOnlyDictionary<int, SchemaClass> EngineTable(string engine) => engines[engine];
}

/// <summary>
/// The current version's pins in versions.json (own-formats.md 2.6 and 3.1): the persisted
/// classes whose game load needs a unit mapping registered by Aow.Units, and the kind classes
/// that keep their accepted flat mapping until the briefs that map their cases.
/// </summary>
public static class FormatVersions
{
    private static readonly Lazy<JsonObject> current = new(() =>
    {
        using var stream = typeof(FormatSchema).Assembly.GetManifestResourceStream("Aow.Formats.versions.json")
            ?? throw new InvalidOperationException("The embedded versions.json is missing.");
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        var versions = (JsonObject)JsonReader.Parse(memory.ToArray(), "versions.json");
        return ((JsonArray)versions["versions"]!).Items.Cast<JsonObject>().Single(v => ((JsonInteger)v["version"]!).Value == OwnFormat.CurrentVersion);
    });

    public static IReadOnlyList<string> UnitMappings => Names("unitMappings");

    public static IReadOnlyList<string> FlatMappings => Names("flatMappings");

    private static string[] Names(string member) =>
        current.Value[member] is JsonArray names ? names.Items.Select(n => ((JsonString)n).Value).ToArray() : [];
}

public sealed class SchemaClass
{
    private readonly JsonObject source;
    private readonly List<SchemaProperty> own = [];
    private List<SchemaProperty> effective = [];
    private readonly Dictionary<int, SchemaProperty> byId = [];
    private readonly Dictionary<string, SchemaProperty> byName = new(StringComparer.Ordinal);
    private readonly List<SchemaProperty> dynamic = [];
    private readonly string? parentName;
    private bool completed;

    internal SchemaClass(JsonObject source, FormatSchema schema, string? shapeOf = null)
    {
        this.source = source;
        Name = shapeOf ?? ((JsonString)source["name"]!).Value;
        if (shapeOf is not null)
        {
            Inherits = false;
            KeepsChildren = false;
            Engines = [];
            foreach (var p in ((JsonArray)source["properties"]!).Items) own.Add(new SchemaProperty((JsonObject)p, this, schema));
            return;
        }
        parentName = (source["parent"] as JsonString)?.Value;
        Id = source["id"] is JsonInteger id ? (int)id.Value : null;
        Engines = source["engines"] is JsonArray e ? e.Items.Select(i => ((JsonString)i).Value).ToArray() : [];
        Inherits = source["inherits"] is not JsonBool { Value: false };
        KeepsChildren = source["children"] is not JsonBool { Value: false };
        FieldClassName = (source["fieldClass"] as JsonString)?.Value;
        Mapping = source["mapping"] is JsonObject mapping ? new SchemaMapping(mapping) : null;
        if (source["properties"] is JsonArray props)
            foreach (var p in props.Items) own.Add(new SchemaProperty((JsonObject)p, this, schema));
    }

    public string Name { get; }
    public int? Id { get; }
    public IReadOnlyList<string> Engines { get; }
    public SchemaClass? Parent { get; private set; }
    public bool Inherits { get; }
    public bool KeepsChildren { get; }
    public string? FieldClassName { get; }

    /// <summary>The case table of a class whose document form is a unit mapping, or null.</summary>
    public SchemaMapping? Mapping { get; }

    /// <summary>The class of the fields of a map level, inherited along the chain.</summary>
    public string? FieldClass => FieldClassName ?? Parent?.FieldClass;

    public IReadOnlyList<SchemaProperty> Own => own;

    /// <summary>The members in canonical order.</summary>
    public IReadOnlyList<SchemaProperty> Properties => effective;

    internal void Link(FormatSchema schema)
    {
        if (parentName is not null) Parent = schema.Find(parentName)
            ?? throw new InvalidOperationException($"Class {Name} names an unknown parent {parentName}.");
    }

    public bool InheritsFrom(SchemaClass ancestor)
    {
        for (var c = this; c is not null; c = c.Parent)
            if (c == ancestor) return true;
        return false;
    }

    public bool InheritsFrom(string ancestor)
    {
        ArgumentNullException.ThrowIfNull(ancestor);
        string? resolved = StoredName(ancestor);
        for (var c = this; c is not null; c = c.Parent)
        {
            if (c.Name == ancestor || resolved is not null && c.Name == resolved) return true;
            string? stored = StoredName(c.Name);
            if (stored is not null && (stored == ancestor || resolved is not null && stored == resolved)) return true;
        }
        return false;
    }

    // An ambiguous Delphi name has no single stored name. The exact class name still matches above.
    private static string? StoredName(string name)
    {
        try { return NameTable.ResolveStored(name); }
        catch (InvalidOperationException) { return null; }
    }

    internal void Complete()
    {
        if (completed) return;
        completed = true;
        Parent?.Complete();
        var readWrite = new List<SchemaProperty>();
        var children = new List<SchemaProperty>();
        bool keepChildren = true;
        for (var c = this; c is not null; c = c.Parent)
            if (!c.KeepsChildren) keepChildren = false;
        // Walk the chain base first, stopping at the first class whose ReadWrite skips its base.
        var chain = new List<SchemaClass>();
        for (var c = this; c is not null; c = c.Parent)
        {
            chain.Add(c);
            if (!c.Inherits) break;
        }
        chain.Reverse();
        foreach (var c in chain)
            foreach (var p in c.own)
            {
                // A subclass property with an inherited id shadows the inherited one: the base
                // never writes it for this class (THuntAG id 30, TAoWTerrainResource id 20).
                if (p.Id is int shadowed) readWrite.RemoveAll(q => q.Id == shadowed);
                (p.IsChildrenPhase ? children : readWrite).Add(p);
            }
        if (keepChildren && children.Count == 0)
        {
            // Children come from TECustomNode.MainReadWrite even when ReadWrite skips the base.
            for (var c = Parent; c is not null; c = c.Parent)
                foreach (var p in c.own)
                    if (p.IsChildrenPhase && !children.Contains(p)) children.Add(p);
        }
        if (!keepChildren) children.Clear();
        var all = readWrite.Concat(children).ToList();
        if (source["order"] is JsonArray order)
        {
            var named = all.ToDictionary(p => p.Name, StringComparer.Ordinal);
            var ordered = new List<SchemaProperty>();
            foreach (var item in order.Items)
            {
                string n = ((JsonString)item).Value;
                if (!named.Remove(n, out var p)) throw new InvalidOperationException($"Class {Name} orders an unknown member {n}.");
                ordered.Add(p);
            }
            if (named.Count != 0) throw new InvalidOperationException($"Class {Name} leaves {string.Join(", ", named.Keys)} out of its order.");
            all = ordered;
        }
        effective = all;
        foreach (var p in effective)
        {
            if (!byName.TryAdd(p.Name, p)) throw new InvalidOperationException($"Class {Name} has two members named {p.Name}.");
            if (p.Id is int id && !byId.TryAdd(id, p)) throw new InvalidOperationException($"Class {Name} has two properties with id {id}.");
            if (p.DataId is int dataId && !byId.TryAdd(dataId, p)) throw new InvalidOperationException($"Class {Name} has two properties with id {dataId}.");
            if (p.DynamicMinimum is not null) dynamic.Add(p);
        }
        dynamic.Sort((a, b) => b.DynamicMinimum!.Value.CompareTo(a.DynamicMinimum!.Value));
    }

    public SchemaProperty? PropertyById(int id)
    {
        if (byId.TryGetValue(id, out var p)) return p;
        foreach (var d in dynamic)
            if (id >= d.DynamicMinimum) return d;
        return null;
    }

    public SchemaProperty? PropertyByName(string name) => byName.GetValueOrDefault(name);

    public int Ordinal(SchemaProperty property) => effective.IndexOf(property);

    public override string ToString() => Name;
}

public sealed class SchemaProperty
{
    internal SchemaProperty(JsonObject source, SchemaClass owner, FormatSchema schema)
    {
        Owner = owner;
        Name = ((JsonString)source["name"]!).Value;
        Id = source["id"] is JsonInteger id ? (int)id.Value : null;
        DataId = source["dataId"] is JsonInteger dataId ? (int)dataId.Value : null;
        DynamicMinimum = source["dynamic"] is JsonInteger d ? (int)d.Value : null;
        SlotBase = source["slotBase"] is JsonInteger s ? (int)s.Value : null;
        IsChildrenPhase = source["phase"] is JsonString { Value: "children" };
        Codec = PropertyCodec.Create(source, schema, owner.Name + "." + Name);
    }

    public SchemaClass Owner { get; }
    public string Name { get; }
    public int? Id { get; }

    /// <summary>The second id of a codec that stores two properties, the campaign map's data.</summary>
    public int? DataId { get; }
    public int? DynamicMinimum { get; }

    /// <summary>The id of a dynamic member minus this value is its slot. Null for sprites.</summary>
    public int? SlotBase { get; }
    public bool IsChildrenPhase { get; }
    public PropertyCodec Codec { get; }
    public bool IsDynamic => DynamicMinimum is not null;

    public override string ToString() => Owner.Name + "." + Name;
}

/// <summary>
/// The case table of a unit mapping (own-formats.md 2.2 and 2.3): the stored discriminator,
/// the cases by their registration key, and the unknown case that keeps every stored value.
/// Each case field names the stored member it holds.
/// </summary>
public sealed class SchemaMapping
{
    internal SchemaMapping(JsonObject source)
    {
        Name = ((JsonString)source["name"]!).Value;
        Discriminator = ((JsonString)source["discriminator"]!).Value;
        Cases = ((JsonArray)source["cases"]!).Items.Cast<JsonObject>()
            .Select(c => new SchemaCase(((JsonInteger)c["key"]!).Value, ((JsonString)c["case"]!).Value, Fields(c))).ToArray();
        var unknown = (JsonObject)source["unknown"]!;
        Unknown = new SchemaCase(null, ((JsonString)unknown["case"]!).Value, Fields(unknown));
        StoredMembers = [.. Cases.Append(Unknown).SelectMany(c => c.Fields.Select(f => f.Member)).Append(Discriminator).Distinct()];
    }

    private static (string Field, string Member)[] Fields(JsonObject c) =>
        ((JsonArray)c["fields"]!).Items.Cast<JsonArray>().Select(f => (((JsonString)f.Items[0]).Value, ((JsonString)f.Items[1]).Value)).ToArray();

    /// <summary>The name of the unit mapping, such as ProductionOrder.</summary>
    public string Name { get; }

    public string Discriminator { get; }

    public IReadOnlyList<SchemaCase> Cases { get; }

    public SchemaCase Unknown { get; }

    /// <summary>The stored members the mapping owns.</summary>
    public IReadOnlyList<string> StoredMembers { get; }
}

/// <summary>One case: its key (null for the unknown case), its name and its fields.</summary>
public sealed record SchemaCase(long? Key, string Name, IReadOnlyList<(string Field, string Member)> Fields);
