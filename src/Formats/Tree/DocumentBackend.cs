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
    public static JsonObject Write(ObjectNode node, SchemaClass classValue, FormatSchema schema, string engine, DocumentNodePath? path = null)
        => WriteObject(node, classValue, site: Site(schema, engine, path: path ?? DocumentNodePath.Root), created: false, slotted: false);

    public static ObjectNode Read(JsonObject document, SchemaClass classValue, FormatSchema schema, string engine, DocumentNodePath? path = null, string? file = null)
        => ReadObject(document, classValue, site: Site(schema, engine, path: path ?? DocumentNodePath.Root, file), created: false, slotted: false);

    internal static JsonObject WriteObject(ObjectNode node, SchemaClass classValue, DocumentSite site, bool created, bool slotted)
    {
        node.RejectVersion(site.Schema.Version, site.Path);
        JsonObject json = new();
        if (created)
        {
            if (node.ClassName is not string className)
            {
                throw site.Fail(Codes.MissingMember, message: "A created object needs a class.");
            }

            _ = json.Add("$class", new JsonString(className));
        }
        else if (node.ClassName is not null)
        {
            throw site.Fail(Codes.ReservedMember, message: "Reserved member \"$class\" is not allowed here.", value: "$class");
        }

        if (slotted)
        {
            if (node.Slot is not int slot || slot < 0)
            {
                throw site.Fail(Codes.MissingMember, message: "A child needs a non-negative integer $slot.");
            }

            _ = json.Add("$slot", new JsonInteger(slot));
        }
        else if (node.Slot is not null)
        {
            throw site.Fail(Codes.ReservedMember, message: "Reserved member \"$slot\" is not allowed here.", value: "$slot");
        }

        HashSet<string> seen = [with(StringComparer.Ordinal)];
        foreach (SchemaProperty prop in classValue.Properties)
        {
            TreeValue? value = node.Find(prop.Name);
            if (value is null)
            {
                continue;
            }

            DocumentSite member = site.For(prop, parent: node, ownerClass: classValue);
            if (prop.IsDynamic)
            {
                _ = json.Add(prop.Name, prop.Codec.WriteSlotsTree(value, site: member));
                _ = seen.Add(prop.Name);
                continue;
            }
            if (site.Schema.Version >= 2 && value is ListNode && !prop.Codec.AcceptsList(site.Schema.Version))
            {
                throw member.Fail(Codes.Type, message: ListOnly(site.Schema.Version));
            }

            JsonValue? written = prop.Codec.WriteTree(value, site: member);
            if (written is not null)
            {
                _ = json.Add(prop.Name, written);
            }

            _ = seen.Add(prop.Name);
        }
        foreach ((string? name, _) in node.Members)
        {
            if (!seen.Contains(name))
            {
                throw site.ForName(name).Fail(Codes.UnknownMember, message: $"Class {classValue.Name} has no member \"{name}\".", value: name);
            }
        }
        // A mapped class stores its case: the stored discriminator selects it, as on the converter.
        if (classValue.Mapping is { } mapping)
        {
            CaseCodec.ToDocument(classValue, mapping, target: json, site);
        }

        return json;
    }

    internal static ObjectNode ReadObject(JsonObject document, SchemaClass classValue, DocumentSite site, bool created, bool slotted)
    {
        // A mapped class names its case. The flat stored members of the case are read as any others.
        if (classValue.Mapping is { } mapping)
        {
            document = CaseCodec.FromDocument(site.Context, classValue, mapping, objectValue: document, site.Path);
        }

        site = site.WithOwner(document);
        HashSet<string> seen = [with(StringComparer.Ordinal)];
        foreach (JsonMember member in document.Members)
        {
            if (!seen.Add(member.Name))
            {
                throw site.ForName(member.Name).Fail(Codes.Duplicate, message: $"Member \"{member.Name}\" is stored twice.", at: member.Value, value: member.Name);
            }
        }

        List<string> reserved = [with(2)];
        if (created)
        {
            reserved.Add("$class");
        }

        if (slotted)
        {
            reserved.Add("$slot");
        }

        ObjectImage.CheckMembers(site.Context, objectValue: document, classValue, site.Path, reserved: [.. reserved]);

        string? className = null;
        int? slot = null;
        if (created)
        {
            if (document["$class"] is not JsonValue classValueLocal)
            {
                throw site.Fail(Codes.MissingMember, message: "A created object needs $class.");
            }

            className = ClassName(classValueLocal, site);
        }
        if (slotted)
        {
            if (document["$slot"] is not JsonInteger { Value: >= 0 and <= int.MaxValue } number)
            {
                throw site.ForName("$slot").Fail(Codes.MissingMember, message: "A child needs a non-negative integer $slot.", at: document["$slot"]);
            }

            slot = (int)number.Value;
        }

        ObjectNode node = new(className, slot);
        foreach (SchemaProperty prop in classValue.Properties)
        {
            JsonMember? member = document.Find(prop.Name);
            if (member is null)
            {
                continue;
            }

            DocumentSite at = site.For(prop, parent: null, ownerClass: classValue);
            if (prop.IsDynamic)
            {
                node.Add(prop.Name, prop.Codec.ReadSlotsTree(member.Value, site: at));
                continue;
            }
            if (site.Schema.Version >= 2 && member.Value is JsonArray && !prop.Codec.AcceptsDocumentArray(site.Schema.Version))
            {
                throw at.Fail(Codes.Type, message: ListOnly(site.Schema.Version), at: member.Value);
            }

            TreeValue? value = prop.Codec.ReadTree(member.Value, site: at);
            if (value is not null)
            {
                node.Add(prop.Name, value);
            }
        }
        return node;
    }

    private static string ListOnly(int version)
        => string.Create(System.Globalization.CultureInfo.InvariantCulture, handler: $"Version {version} stores a list only for a fixed integer array.");

    internal static SchemaClass RequireClass(DocumentSite site, string name, string? family, JsonValue? at)
    {
        SchemaClass classValue = site.Schema.Find(name) ?? throw site.Fail(Codes.UnknownClass, message: $"Unknown class {name}.", at, value: name);
        return site.Schema.Version == 1 && (classValue.Id is not int id || site.Schema.Resolve(site.Engine, classId: id) != classValue)
            ? throw site.Fail(Codes.UnknownClass, message: $"Class {classValue.Name} is not in the {site.Engine} engine table.", at, value: classValue.Name)
            : family is not null && !classValue.InheritsFrom(family)
            ? throw site.Fail(Codes.ClassFamily, message: $"Class {classValue.Name} is not a {family}.", at, value: classValue.Name, constraint: family)
            : classValue;
    }

    private static string ClassName(JsonValue value, DocumentSite site)
    {
        return value is JsonNull
            ? throw site.ForName("$class").Fail(Codes.Type, message: "A stored nil object is null, not $class null.", at: value)
            : value is not JsonString name
            ? throw site.ForName("$class").Fail(Codes.Type, message: "$class is a class name.", at: value, value: value.KindName)
            : name.Value;
    }

    private static DocumentSite Site(FormatSchema schema, string engine, DocumentNodePath path, string? file = null) => new()
    {
        Path = path,
        Schema = schema,
        Engine = engine,
        Context = new EncodeContext(schema, engine, profile: "tree", file, limits: FormatLimits.Default),
    };
}

internal static class DocumentSiteNames
{
    public static DocumentSite ForName(this DocumentSite site, string name) => site.At(site.Path.Member(name), element: false);
}
