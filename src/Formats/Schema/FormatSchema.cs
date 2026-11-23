
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
    private readonly Dictionary<string, SchemaClass> _classes = [with(StringComparer.Ordinal)];
    private readonly Dictionary<string, Dictionary<int, SchemaClass>> _engines = [with(StringComparer.Ordinal)];
    private readonly List<string> _ambiguities = [];
    private readonly List<(string Engine, int Id)> _ambiguous = [];

    /// <summary>Class ids that one engine table enlists for two classes, measured at load.</summary>
    public IReadOnlyList<string> RegistrationAmbiguities => _ambiguities;

    /// <summary>The engine tables, by context name.</summary>
    public IReadOnlyCollection<string> Engines => _engines.Keys;

    public int Version { get; }

    /// <summary>The schema stores a generic list. The test schema sets this. Product schemas do not.</summary>
    public bool StoresLists { get; }

    /// <summary>SHA-256 of the normalized schema text, pinned in versions.json.</summary>
    public string Hash { get; }

    public IReadOnlyCollection<SchemaClass> Classes => _classes.Values;

    private static readonly Lazy<FormatSchema> s_embedded = new(LoadEmbedded);

    /// <summary>
    /// A test installs a schema here so one migration step can be observed not to read <see cref="Current"/>.
    /// Production leaves it null. <see cref="Current"/> is then the embedded schema after one null comparison.
    /// The persist path does not read this field.
    /// </summary>
    internal static FormatSchema? Substituted { get; }

    public static FormatSchema Current => Substituted ?? s_embedded.Value;

    private static FormatSchema LoadEmbedded()
    {
        using Stream stream = typeof(FormatSchema).Assembly.GetManifestResourceStream("Aow.Formats.schema.json")
            ?? throw new InvalidOperationException("The embedded schema is missing.");
        using MemoryStream memory = new();
        stream.CopyTo(memory);
        return Load(memory.ToArray(), file: "schema.json");
    }

    public static FormatSchema Load(byte[] utf8, string file)
    {
        JsonObject root = JsonReader.Parse(utf8, file) as JsonObject
            ?? throw new InvalidOperationException("The schema is not an object.");
        return new FormatSchema(root, hash: Convert.ToHexStringLower(SHA256.HashData(JsonWriter.ToUtf8(root))));
    }

    private FormatSchema(JsonObject root, string hash)
    {
        Hash = hash;
        Version = (int)((JsonInteger)root["version"]!).Value;
        StoresLists = root["lists"] is JsonBoolean { Value: true };
        JsonArray list = (JsonArray)root["classes"]!;
        foreach (JsonValue item in list.Items)
        {
            SchemaClass c = new((JsonObject)item, schema: this);
            _classes.Add(c.Name, c);
        }
        if (root["legacyShapes"] is JsonArray shapes)
        {
            foreach (JsonValue item in shapes.Items)
            {
                JsonObject shape = (JsonObject)item;
                SchemaClass c = new(shape, schema: this, shapeOf: ((JsonString)shape["class"]!).Value);
                _legacyShapes.Add((((JsonString)shape["context"]!).Value, c.Name), (((JsonString)shape["name"]!).Value, c));
            }
        }

        foreach (SchemaClass c in _classes.Values)
        {
            c.Link(this);
        }

        foreach ((_, SchemaClass? c) in _legacyShapes.Values)
        {
            c.Complete();
        }

        foreach (SchemaClass c in _classes.Values)
        {
            c.Complete();
        }

        foreach (SchemaClass c in _classes.Values)
        {
            foreach (string engine in c.Engines)
            {
                if (c.Id is not int id)
                {
                    throw new InvalidOperationException($"Class {c.Name} is enlisted without an id.");
                }

                if (!_engines.TryGetValue(engine, out Dictionary<int, SchemaClass>? table))
                {
                    _engines[engine] = table = [];
                }

                if (!table.TryAdd(id, c))
                {
                    _ambiguities.Add($"engine {engine} class id {id}: {table[id].Name} and {c.Name}");
                    _ambiguous.Add((engine, id));
                }
            }
        }
        // An _ambiguous id resolves to nothing, so a stored instance of it is refused, not guessed.
        foreach ((string? engine, int id) in _ambiguous)
        {
            _ = _engines[engine].Remove(id);
        }
    }

    private readonly Dictionary<(string Context, string Class), (string Rule, SchemaClass Shape)> _legacyShapes = [];

    /// <summary>A named legacy shape of a class in one context, such as LEGACY_VALLEY01_SPRITE.</summary>
    public (string Rule, SchemaClass Shape)? LegacyShape(string context, string className)
    {
        foreach (string candidate in Contexts(context))
        {
            (string Rule, SchemaClass Shape)? found = Lookup(candidate, className);
            if (found is not null)
            {
                return found;
            }
        }
        return null;
    }

    private (string Rule, SchemaClass Shape)? Lookup(string context, string className)
    {
        if (_legacyShapes.TryGetValue((context, className), out (string Rule, SchemaClass Shape) shape))
        {
            return shape;
        }

        string? resolved = NameTable.ResolveStored(className);
        return resolved is null || resolved == className || !_legacyShapes.TryGetValue((context, resolved), out shape) ? null : shape;
    }

    // The stored context names the class with its current name or with the Delphi name.
    private static IEnumerable<string> Contexts(string context)
    {
        yield return context;
        int dot = context.LastIndexOf('.');
        if (dot <= 0)
        {
            yield break;
        }

        string head = context[..dot];
        string tail = context[(dot + 1)..];
        string? delphi = NameTable.DelphiName(head);
        if (delphi is not null && delphi != head)
        {
            yield return $"{delphi}.{tail}";
        }

        string? stored = NameTable.ResolveStored(head);
        if (stored is not null && stored != head)
        {
            yield return $"{stored}.{tail}";
        }
    }

    public SchemaClass? Find(string name)
    {
        if (_classes.TryGetValue(name, out SchemaClass? found))
        {
            return found;
        }

        string? resolved = NameTable.ResolveStored(name);
        if (resolved is not null && resolved != name && _classes.TryGetValue(resolved, out found))
        {
            return found;
        }
        // A current schema answers a Delphi name. A version 1 or 2 schema answers the current name.
        string? delphi = resolved is null ? null : NameTable.DelphiName(resolved);
        return delphi is not null && delphi != name && _classes.TryGetValue(delphi, out found) ? found : null;
    }

    public SchemaClass Get(string name)
        => Find(name) ?? throw new InvalidOperationException($"The schema has no class {name}.");

    /// <summary>Resolves a stored class id through one engine table only (own-formats.md 2.2).</summary>
    public SchemaClass? Resolve(string engine, int classId)
        => _engines.TryGetValue(engine, out Dictionary<int, SchemaClass>? table) ? table.GetValueOrDefault(classId) : null;

    public IReadOnlyDictionary<int, SchemaClass> EngineTable(string engine) => _engines[engine];
}

/// <summary>
/// The current version's pins in versions.json (own-formats.md 2.6 and 3.1): the persisted
/// classes whose game load needs a unit mapping registered by Aow.Game, and the kind classes
/// that keep their accepted flat mapping until the briefs that map their cases.
/// </summary>
public static class FormatVersions
{
    private static readonly Lazy<JsonObject> s_current = new(() =>
    {
        using Stream stream = typeof(FormatSchema).Assembly.GetManifestResourceStream("Aow.Formats.versions.json")
            ?? throw new InvalidOperationException("The embedded versions.json is missing.");
        using MemoryStream memory = new();
        stream.CopyTo(memory);
        JsonObject current = (JsonObject)JsonReader.Parse(memory.ToArray(), file: "versions.json");
        return current["version"] is not JsonInteger { Value: OwnFormat.CurrentVersion }
            ? throw new InvalidOperationException("versions.json does not pin the current version.")
            : current;
    });

    public static IReadOnlyList<string> UnitMappings => Names("unitMappings");

    public static IReadOnlyList<string> FlatMappings => Names("flatMappings");

    private static string[] Names(string member)
        => s_current.Value[member] is JsonArray names ? [.. names.Items.Select(n => ((JsonString)n).Value)] : [];
}

public sealed class SchemaClass
{
    private readonly JsonObject _source;
    private readonly List<SchemaProperty> _own = [];
    private readonly List<int> _ownSkippedIds = [];
    private List<SchemaProperty> _effective = [];
    private readonly Dictionary<int, SchemaProperty> _byId = [];
    private readonly HashSet<int> _skippedIds = [];
    private readonly Dictionary<string, SchemaProperty> _byName = [with(StringComparer.Ordinal)];
    private readonly List<SchemaProperty> _dynamic = [];
    private readonly string? _parentName;
    private bool _completed;

    internal SchemaClass(JsonObject source, FormatSchema schema, string? shapeOf = null)
    {
        _source = source;
        Name = shapeOf ?? ((JsonString)source["name"]!).Value;
        if (shapeOf is not null)
        {
            Inherits = false;
            KeepsChildren = false;
            Engines = [];
            foreach (JsonValue p in ((JsonArray)source["properties"]!).Items)
            {
                _own.Add(new SchemaProperty((JsonObject)p, owner: this, schema));
            }

            if (source["skippedIds"] is JsonArray shapeSkipped)
            {
                ReadSkippedIds(shapeSkipped);
            }

            return;
        }
        _parentName = (source["parent"] as JsonString)?.Value;
        Id = source["id"] is JsonInteger id ? (int)id.Value : null;
        Engines = source["engines"] is JsonArray e ? e.Items.Select(i => ((JsonString)i).Value).ToArray() : [];
        Inherits = source["inherits"] is not JsonBoolean { Value: false };
        KeepsChildren = source["children"] is not JsonBoolean { Value: false };
        FieldClassName = (source["fieldClass"] as JsonString)?.Value;
        Mapping = source["mapping"] is JsonObject mapping ? new SchemaMapping(mapping) : null;
        if (source["properties"] is JsonArray props)
        {
            foreach (JsonValue p in props.Items)
            {
                _own.Add(new SchemaProperty((JsonObject)p, owner: this, schema));
            }
        }
        if (source["skippedIds"] is JsonArray skipped)
        {
            ReadSkippedIds(skipped);
        }
    }

    private void ReadSkippedIds(JsonArray skipped)
    {
        foreach (JsonValue item in skipped.Items)
        {
            if (item is not JsonInteger number || number.Value is < int.MinValue or > int.MaxValue)
            {
                throw new InvalidOperationException($"Class {Name} has a non-integer skipped id.");
            }

            _ownSkippedIds.Add((int)number.Value);
        }
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

    public IReadOnlyList<SchemaProperty> Own => _own;

    /// <summary>The members in canonical order.</summary>
    public IReadOnlyList<SchemaProperty> Properties => _effective;

    /// <summary>Property ids ignored on original read, not kept in documents and not written.</summary>
    public IReadOnlySet<int> SkippedIds => _skippedIds;

    internal void Link(FormatSchema schema)
    {
        if (_parentName is not null)
        {
            Parent = schema.Find(_parentName)
            ?? throw new InvalidOperationException($"Class {Name} names an unknown parent {_parentName}.");
        }
    }

    public bool InheritsFrom(SchemaClass ancestor)
    {
        for (SchemaClass? c = this; c is not null; c = c.Parent)
        {
            if (c == ancestor)
            {
                return true;
            }
        }

        return false;
    }

    public bool InheritsFrom(string ancestor)
    {
        ArgumentNullException.ThrowIfNull(ancestor);
        string? resolved = StoredName(ancestor);
        for (SchemaClass? c = this; c is not null; c = c.Parent)
        {
            if (c.Name == ancestor || (resolved is not null && c.Name == resolved))
            {
                return true;
            }

            string? stored = StoredName(c.Name);
            if (stored is not null && (stored == ancestor || (resolved is not null && stored == resolved)))
            {
                return true;
            }
        }
        return false;
    }

    // An ambiguous Delphi name has no single stored name. The exact class name still matches above.
    private static string? StoredName(string name)
    {
        try
        {
            return NameTable.ResolveStored(name);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    internal void Complete()
    {
        if (_completed)
        {
            return;
        }

        _completed = true;
        Parent?.Complete();
        List<SchemaProperty> readWrite = [];
        List<SchemaProperty> children = [];
        bool keepChildren = true;
        for (SchemaClass? c = this; c is not null; c = c.Parent)
        {
            if (!c.KeepsChildren)
            {
                keepChildren = false;
            }
        }
        // Walk the chain base first, stopping at the first class whose ReadWrite skips its base.
        List<SchemaClass> chain = [];
        for (SchemaClass? c = this; c is not null; c = c.Parent)
        {
            chain.Add(c);
            if (!c.Inherits)
            {
                break;
            }
        }
        chain.Reverse();
        foreach (SchemaClass c in chain)
        {
            foreach (SchemaProperty p in c._own)
            {
                // A subclass property with an inherited id shadows the inherited one: the base
                // never writes it for this class (THuntAG id 30, TAoWTerrainResource id 20).
                // A coordinate shadows on any of its three ids, and a subclass id that matches
                // any of those three drops the whole coordinate.
                _ = readWrite.RemoveAll(q => SharesImageId(q, incoming: p));
                (p.IsChildrenPhase ? children : readWrite).Add(p);
            }
        }

        if (keepChildren && children.Count == 0)
        {
            // Children come from the custom node's main visit even when the field visit skips the base.
            for (SchemaClass? c = Parent; c is not null; c = c.Parent)
            {
                foreach (SchemaProperty p in c._own)
                {
                    if (p.IsChildrenPhase && !children.Contains(p))
                    {
                        children.Add(p);
                    }
                }
            }
        }
        if (!keepChildren)
        {
            children.Clear();
        }

        List<SchemaProperty> all = [.. readWrite, .. children];
        if (_source["order"] is JsonArray order)
        {
            Dictionary<string, SchemaProperty> named = all.ToDictionary(p => p.Name, comparer: StringComparer.Ordinal);
            List<SchemaProperty> ordered = [];
            foreach (JsonValue item in order.Items)
            {
                string n = ((JsonString)item).Value;
                if (!named.Remove(n, out SchemaProperty? p))
                {
                    throw new InvalidOperationException($"Class {Name} orders an unknown member {n}.");
                }

                ordered.Add(p);
            }
            if (named.Count != 0)
            {
                throw new InvalidOperationException($"Class {Name} leaves {string.Join(", ", values: named.Keys)} out of its order.");
            }

            all = ordered;
        }
        _effective = all;
        foreach (SchemaProperty p in _effective)
        {
            if (!_byName.TryAdd(p.Name, p))
            {
                throw new InvalidOperationException($"Class {Name} has two members named {p.Name}.");
            }

            foreach (int imageId in p.ImageIds)
            {
                if (!_byId.TryAdd(imageId, p))
                {
                    throw new InvalidOperationException($"Class {Name} has two properties with id {imageId}.");
                }
            }

            if (p.DynamicMinimum is not null)
            {
                _dynamic.Add(p);
            }
        }
        _dynamic.Sort((a, b) => b.DynamicMinimum!.Value.CompareTo(a.DynamicMinimum!.Value));
        foreach (SchemaClass c in chain)
        {
            foreach (int skippedId in c._ownSkippedIds)
            {
                if (c == this && _own.Any(p => p.ImageIds.Contains(skippedId)))
                {
                    throw new InvalidOperationException($"Class {Name} both declares and skips property id {skippedId}.");
                }

                if (!_byId.ContainsKey(skippedId))
                {
                    _ = _skippedIds.Add(skippedId);
                }
            }
        }
    }

    // True when the two properties share an original id. A coordinate counts each part id.
    private static bool SharesImageId(SchemaProperty existing, SchemaProperty incoming) => incoming.ImageIds.Any(item => existing.ImageIds.Contains(item));

    public SchemaProperty? PropertyById(int id)
    {
        if (_byId.TryGetValue(id, out SchemaProperty? p))
        {
            return p;
        }

        foreach (SchemaProperty d in _dynamic)
        {
            if (id >= d.DynamicMinimum)
            {
                return d;
            }
        }

        return null;
    }

    /// <summary>Whether this class ignores stored entries of <paramref name="id"/> on original read.</summary>
    public bool IsSkippedId(int id) => _skippedIds.Contains(id);

    /// <summary>The property of that stored name.</summary>
    public SchemaProperty? PropertyByName(string name) => _byName.GetValueOrDefault(name);

    public int Ordinal(SchemaProperty property) => _effective.IndexOf(property);

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
        Codec = PropertyCodec.Create(source, schema, path: $"{owner.Name}.{Name}");
        ImageIds = ImageIdList(Id, DataId, Codec);
    }

    private static int[] ImageIdList(int? id, int? dataId, PropertyCodec codec)
    {
        List<int> ids = [];
        if (id is int own)
        {
            ids.Add(own);
        }

        if (dataId is int data && !ids.Contains(data))
        {
            ids.Add(data);
        }

        if (codec is CoordinateCodec coordinate)
        {
            ids.AddRange(coordinate.ImageIds.Where(item => !ids.Contains(item)));
        }

        return [.. ids];
    }

    public SchemaClass Owner { get; }
    public string Name { get; }
    public int? Id { get; }

    /// <summary>The second id of a codec that stores two properties, the campaign map's data.</summary>
    public int? DataId { get; }

    /// <summary>Every original id of the property. A coordinate lists its three part ids.</summary>
    public IReadOnlyList<int> ImageIds { get; }
    public int? DynamicMinimum { get; }

    /// <summary>The id of a dynamic member minus this value is its slot. Null for sprites.</summary>
    public int? SlotBase { get; }
    public bool IsChildrenPhase { get; }

    public PropertyCodec Codec { get; }
    public bool IsDynamic => DynamicMinimum is not null;

    public override string ToString() => $"{Owner.Name}.{Name}";
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
            .Select(c => new SchemaCase(((JsonInteger)c["key"]!).Value, Name: ((JsonString)c["case"]!).Value, Fields: Fields(c), Refused: Refused(c))).ToArray();
        JsonObject unknown = (JsonObject)source["unknown"]!;
        Unknown = new SchemaCase(null, Name: ((JsonString)unknown["case"]!).Value, Fields: Fields(unknown), Refused: Refused(unknown));
        StoredMembers = [.. Cases.Append(Unknown).SelectMany(c => c.Fields.Select(f => f.Member)).Append(Discriminator).Distinct()];
    }

    private static (string Field, string Member)[] Fields(JsonObject c)
        => [.. ((JsonArray)c["fields"]!).Items.Cast<JsonArray>().Select(f => (((JsonString)f.Items[0]).Value, ((JsonString)f.Items[1]).Value))];

    private static HashSet<string> Refused(JsonObject c)
        => c["refuse"] is JsonArray list
            ? [with(StringComparer.Ordinal), .. list.Items.Cast<JsonString>().Select(s => s.Value)]
            : [with(StringComparer.Ordinal)];

    /// <summary>The name of the unit mapping, such as ProductionOrder.</summary>
    public string Name { get; }

    public string Discriminator { get; }

    public IReadOnlyList<SchemaCase> Cases { get; }

    public SchemaCase Unknown { get; }

    /// <summary>The stored members the mapping owns.</summary>
    public IReadOnlyList<string> StoredMembers { get; }
}

/// <summary>One case: its key (null for the unknown case), its name, its fields and the unused stored members refused when nonzero.</summary>
public sealed record SchemaCase(long? Key, string Name, IReadOnlyList<(string Field, string Member)> Fields, IReadOnlySet<string> Refused);
