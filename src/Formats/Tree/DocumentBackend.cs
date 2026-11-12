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
    public static JsonObject Write(ObjectNode node, SchemaClass classValue, FormatSchema schema, string engine, DocumentNodePath? path = null) =>
        WriteObject(node, classValue, Site(schema, engine, path ?? DocumentNodePath.Root), created: false, slotted: false);

    public static ObjectNode Read(JsonObject document, SchemaClass classValue, FormatSchema schema, string engine, DocumentNodePath? path = null) =>
        ReadObject(document, classValue, Site(schema, engine, path ?? DocumentNodePath.Root), created: false, slotted: false);

    internal static JsonObject WriteObject(ObjectNode node, SchemaClass classValue, DocumentSite site, bool created, bool slotted)
    {
        RefuseMapping(classValue, site);
        node.RejectVersion(site.Schema.Version, site.Path);
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
        foreach (var prop in classValue.Properties)
        {
            var value = node.Find(prop.Name);
            if (value is null) continue;
            var member = site.For(prop);
            if (prop.IsDynamic)
                throw member.Fail(Codes.Type, "PE-1 does not store a dynamic member.");
            if (site.Schema.Version >= 2 && value is ListNode && !prop.Codec.AcceptsList(site.Schema.Version))
                throw member.Fail(Codes.Type, ListOnly(site.Schema.Version));
            var written = prop.Codec.WriteTree(value, member);
            if (written is not null) json.Add(prop.Name, written);
            seen.Add(prop.Name);
        }
        foreach (var (name, _) in node.Members)
            if (!seen.Contains(name))
                throw site.ForName(name).Fail(Codes.UnknownMember, $"Class {classValue.Name} has no member \"{name}\".", value: name);
        return json;
    }

    internal static ObjectNode ReadObject(JsonObject document, SchemaClass classValue, DocumentSite site, bool created, bool slotted)
    {
        site = site.WithOwner(document);
        RefuseMapping(classValue, site);
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var member in document.Members)
            if (!seen.Add(member.Name))
                throw site.ForName(member.Name).Fail(Codes.Duplicate, $"Member \"{member.Name}\" is stored twice.", member.Value, member.Name);
        var reserved = new List<string>(2);
        if (created) reserved.Add("$class");
        if (slotted) reserved.Add("$slot");
        ObjectImage.CheckMembers(site.Context, document, classValue, site.Path, [.. reserved]);

        string? className = null;
        int? slot = null;
        if (created)
        {
            if (document["$class"] is not JsonValue classValueLocal)
                throw site.Fail(Codes.MissingMember, "A created object needs $class.");
            className = ClassName(classValueLocal, site);
        }
        if (slotted)
        {
            if (document["$slot"] is not JsonInteger { Value: >= 0 and <= int.MaxValue } number)
                throw site.ForName("$slot").Fail(Codes.MissingMember, "A child needs a non-negative integer $slot.", document["$slot"]);
            slot = (int)number.Value;
        }

        var node = new ObjectNode(className, slot);
        foreach (var prop in classValue.Properties)
        {
            var member = document.Find(prop.Name);
            if (member is null) continue;
            var at = site.For(prop);
            if (prop.IsDynamic)
                throw at.Fail(Codes.Type, "PE-1 does not store a dynamic member.", member.Value);
            if (site.Schema.Version >= 2 && member.Value is JsonArray && !prop.Codec.AcceptsDocumentArray(site.Schema.Version))
                throw at.Fail(Codes.Type, ListOnly(site.Schema.Version), member.Value);
            var value = prop.Codec.ReadTree(member.Value, at);
            if (value is not null) node.Add(prop.Name, value);
        }
        return node;
    }

    private static void RefuseMapping(SchemaClass classValue, DocumentSite site)
    {
        if (classValue.Mapping is null) return;
        throw site.Fail(Codes.Mapping, "Class " + classValue.Name + " uses a case mapping. The document backend does not reproduce it.", value: classValue.Name);
    }

    private static string ListOnly(int version) =>
        "Version " + version.ToString(System.Globalization.CultureInfo.InvariantCulture) + " stores a list only for a fixed integer array.";

    internal static SchemaClass RequireClass(DocumentSite site, string name, string? family, JsonValue? at)
    {
        var classValue = site.Schema.Find(name) ?? throw site.Fail(Codes.UnknownClass, $"Unknown class {name}.", at, name);
        if (site.Schema.Version == 1 && (classValue.Id is not int id || site.Schema.Resolve(site.Engine, id) != classValue))
            throw site.Fail(Codes.UnknownClass, $"Class {classValue.Name} is not in the {site.Engine} engine table.", at, classValue.Name);
        if (family is not null && !classValue.InheritsFrom(family))
            throw site.Fail(Codes.ClassFamily, $"Class {classValue.Name} is not a {family}.", at, classValue.Name, family);
        return classValue;
    }

    private static string ClassName(JsonValue value, DocumentSite site)
    {
        if (value is JsonNull) throw site.ForName("$class").Fail(Codes.Type, "A stored nil object is null, not $class null.", value);
        if (value is not JsonString name) throw site.ForName("$class").Fail(Codes.Type, "$class is a class name.", value, value.KindName);
        return name.Value;
    }

    private static DocumentSite Site(FormatSchema schema, string engine, DocumentNodePath path) => new()
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
