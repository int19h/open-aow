using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats.Tree;

/// <summary>
/// The direct document backend. It maps a tree to a versioned JSON object and back, using the
/// schema for names, order and codecs. Version 1 writes the same object the converter writes
/// for the same state. There is no engine image and no property table on this path.
/// </summary>
public static class DocumentBackend
{
    public static JsonObject Write(ObjectNode node, SchemaClass cls, FormatSchema schema, string engine, DocPath? path = null) =>
        WriteObject(node, cls, Site(schema, engine, path ?? DocPath.Root), created: false, slotted: false);

    public static ObjectNode Read(JsonObject document, SchemaClass cls, FormatSchema schema, string engine, DocPath? path = null) =>
        ReadObject(document, cls, Site(schema, engine, path ?? DocPath.Root), created: false, slotted: false);

    internal static JsonObject WriteObject(ObjectNode node, SchemaClass cls, DocumentSite site, bool created, bool slotted)
    {
        if (site.Schema.Version == 1) node.RejectVersion(1, site.Path);
        var json = new JsonObject();
        if (created)
        {
            if (node.ClassName is not string className)
                throw site.Fail(Codes.MissingMember, "A created object needs a class.");
            json.Add("$class", new JsonString(className));
        }
        else if (node.ClassName is not null)
            throw site.Fail(Codes.ReservedMember, "Reserved member \"$class\" is not allowed here.", value: "$class");
        if (slotted)
        {
            if (node.Slot is not int slot || slot < 0)
                throw site.Fail(Codes.MissingMember, "A child needs a non-negative integer $slot.");
            json.Add("$slot", new JsonInteger(slot));
        }
        else if (node.Slot is not null)
            throw site.Fail(Codes.ReservedMember, "Reserved member \"$slot\" is not allowed here.", value: "$slot");

        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var prop in cls.Properties)
        {
            var value = node.Find(prop.Name);
            if (value is null) continue;
            var member = site.For(prop);
            if (prop.IsDynamic)
                throw member.Fail(Codes.Type, "PE-1 does not store a dynamic member.");
            var written = prop.Codec.WriteTree(value, member);
            if (written is not null) json.Add(prop.Name, written);
            seen.Add(prop.Name);
        }
        foreach (var (name, _) in node.Members)
            if (!seen.Contains(name))
                throw site.ForName(name).Fail(Codes.UnknownMember, $"Class {cls.Name} has no member \"{name}\".", value: name);
        return json;
    }

    internal static ObjectNode ReadObject(JsonObject document, SchemaClass cls, DocumentSite site, bool created, bool slotted)
    {
        string? className = null;
        int? slot = null;
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var member in document.Members)
        {
            if (!seen.Add(member.Name))
                throw site.ForName(member.Name).Fail(Codes.Duplicate, $"Member \"{member.Name}\" is stored twice.", member.Value, member.Name);
            if (member.Name.StartsWith('$'))
            {
                if (member.Name == "$layout") continue;
                if (created && member.Name == "$class")
                {
                    className = ClassName(member.Value, site);
                    continue;
                }
                if (slotted && member.Name == "$slot")
                {
                    if (member.Value is not JsonInteger { Value: >= 0 and <= int.MaxValue } number)
                        throw site.ForName("$slot").Fail(Codes.MissingMember, "A child needs a non-negative integer $slot.", member.Value);
                    slot = (int)number.Value;
                    continue;
                }
                throw site.ForName(member.Name).Fail(Codes.ReservedMember,
                    Reserved.IsKnown(member.Name) ? $"Reserved member \"{member.Name}\" is not allowed here." : $"Unknown reserved member \"{member.Name}\".",
                    member.Value, member.Name);
            }
            if (cls.PropertyByName(member.Name) is null)
                throw site.ForName(member.Name).Fail(Codes.UnknownMember, $"Class {cls.Name} has no member \"{member.Name}\".", member.Value, member.Name);
        }
        if (created && className is null)
            throw site.Fail(Codes.MissingMember, "A created object needs $class.");
        if (slotted && slot is null)
            throw site.Fail(Codes.MissingMember, "A child needs a non-negative integer $slot.");

        var node = new ObjectNode(className, slot);
        foreach (var prop in cls.Properties)
        {
            var member = document.Find(prop.Name);
            if (member is null) continue;
            var at = site.For(prop);
            if (prop.IsDynamic)
                throw at.Fail(Codes.Type, "PE-1 does not store a dynamic member.", member.Value);
            var value = prop.Codec.ReadTree(member.Value, at);
            if (value is not null) node.Add(prop.Name, value);
        }
        return node;
    }

    internal static SchemaClass RequireClass(DocumentSite site, string name, string? family, JsonValue? at)
    {
        var cls = site.Schema.Find(name) ?? throw site.Fail(Codes.UnknownClass, $"Unknown class {name}.", at, name);
        if (site.Schema.Version == 1 && (cls.Id is not int id || site.Schema.Resolve(site.Engine, id) != cls))
            throw site.Fail(Codes.UnknownClass, $"Class {cls.Name} is not in the {site.Engine} engine table.", at, cls.Name);
        if (family is not null && !cls.InheritsFrom(family))
            throw site.Fail(Codes.ClassFamily, $"Class {cls.Name} is not a {family}.", at, cls.Name, family);
        return cls;
    }

    private static string ClassName(JsonValue value, DocumentSite site)
    {
        if (value is JsonNull) throw site.ForName("$class").Fail(Codes.Type, "A stored nil object is null, not $class null.", value);
        if (value is not JsonString name) throw site.ForName("$class").Fail(Codes.Type, "$class is a class name.", value, value.KindName);
        return name.Value;
    }

    private static DocumentSite Site(FormatSchema schema, string engine, DocPath path) => new()
    {
        Path = path,
        Schema = schema,
        Engine = engine,
        Context = new EncodeContext(schema, engine, "tree", null, FormatLimits.Default, ExportOptions.Default),
    };
}

static class DocumentSiteNames
{
    public static DocumentSite ForName(this DocumentSite site, string name) => site.At(site.Path.Member(name), false);
}
