using System.Globalization;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats.Tree;

/// <summary>
/// Writes a tree value as an original property-table image and reads that image back.
/// The codecs write the bytes. This class walks the schema property table.
/// A mapped class and a dynamic member without a slot base stay refused. Layout residue is not kept.
/// </summary>
internal static class ImageTree
{
    internal static byte[] Write(ObjectNode node, SchemaClass classValue, FormatSchema schema, string engine)
    {
        var context = new EncodeContext(schema, engine, engine, null, FormatLimits.Default, ExportOptions.Default);
        var output = new ByteBuffer();
        WriteBody(node, classValue, context, output, DocumentNodePath.Root, false);
        return output.ToArray();
    }

    internal static ObjectNode Read(byte[] image, SchemaClass classValue, FormatSchema schema, string engine)
    {
        var context = new DecodeContext(image, schema, engine, engine, null, FormatLimits.Default, new LayoutCounts());
        return ReadBody(context, 0, image.Length, classValue, DocumentNodePath.Root, null, null);
    }

    internal static void WriteBody(ObjectNode node, SchemaClass classValue, EncodeContext context, ByteBuffer data, DocumentNodePath path, bool allowIdentity)
    {
        RefuseMapping(classValue, path);
        if (!allowIdentity)
        {
            if (node.ClassName is not null)
                throw context.Fail(Codes.ReservedMember, "Reserved member \"$class\" is not allowed here.", null, path, "$class");
            if (node.Slot is not null)
                throw context.Fail(Codes.ReservedMember, "Reserved member \"$slot\" is not allowed here.", null, path, "$slot");
        }
        var marker = new JsonObject();
        context.Enter(marker, path);
        var writer = new ObjectWriter();
        var seen = new HashSet<string>();
        foreach (var prop in classValue.Properties)
        {
            var value = node.Find(prop.Name);
            if (value is null) continue;
            var memberPath = path.Member(prop.Name);
            if (prop.IsDynamic)
            {
                var slots = new ImageFrame { Property = prop, Id = prop.DynamicMinimum!.Value, Path = memberPath, Parent = node };
                prop.Codec.WriteSlotsImage(value, context, writer, slots);
                seen.Add(prop.Name);
                continue;
            }
            if (prop.Codec.ImageRefusal is string reason)
                throw context.Fail(Codes.Type, reason, null, memberPath);
            if (!prop.Codec.StoresImage(value))
            {
                seen.Add(prop.Name);
                continue;
            }
            if (prop.Id is not int id)
                throw new InvalidOperationException(prop + " has no stored id.");
            writer.Begin(id, prop);
            var frame = new ImageFrame { Property = prop, Id = id, Path = memberPath, Parent = node };
            prop.Codec.WriteImage(value, context, writer.Data, frame);
            writer.End();
            seen.Add(prop.Name);
        }
        foreach (var (name, _) in node.Members)
            if (!seen.Contains(name))
                throw context.Fail(Codes.UnknownMember, "Class " + classValue.Name + " has no member \"" + name + "\".", null, path.Member(name), name);
        ObjectImage.Place(context, writer, null, null, null, marker, path, data);
        context.Leave();
    }

    internal static ObjectNode ReadBody(DecodeContext context, int start, int end, SchemaClass classValue, DocumentNodePath path, string? className, int? slot)
    {
        RefuseMapping(classValue, path);
        context.Enter(start, path);
        var table = PropertyTable.Read(context, start, end, path);
        int db = table.DataBase;
        int length = end - db;
        var located = new List<Located>(table.Entries.Count);
        var seen = new Dictionary<int, TableEntry>();
        foreach (var entry in table.Entries)
        {
            var prop = classValue.PropertyById(entry.Id)
                ?? throw context.Fail(Codes.UnknownProperty, "Class " + classValue.Name + " has no property " + entry.Id + ".", db + entry.Offset, path, entry.Id.ToString(CultureInfo.InvariantCulture));
            if (!seen.TryAdd(entry.Id, entry))
                throw context.Fail(Codes.Duplicate, (prop.IsDynamic ? "Dynamic id " : "Property ") + entry.Id + " of class " + classValue.Name + " is stored twice.", start, path);
            located.Add(new Located(entry, prop));
        }
        var boundaries = located.Select(item => item.Entry.Offset).Append(length).Distinct().Order().ToArray();
        var shared = new Dictionary<int, int>();
        foreach (var item in located)
        {
            item.Start = db + item.Entry.Offset;
            if (item.Property.Codec.ZeroSize) item.End = item.Start;
            else
            {
                int next = Array.BinarySearch(boundaries, item.Entry.Offset);
                item.End = db + (next + 1 < boundaries.Length ? boundaries[next + 1] : length);
                shared[item.Entry.Offset] = shared.GetValueOrDefault(item.Entry.Offset) + 1;
                if (shared[item.Entry.Offset] > 1)
                    throw context.Fail(Codes.Layout, "Two valued properties of class " + classValue.Name + " share offset " + item.Entry.Offset + ".", item.Start, path);
            }
        }
        var node = new ObjectNode(className, slot);
        var byProperty = new Dictionary<SchemaProperty, List<Located>>();
        foreach (var item in located)
        {
            if (!byProperty.TryGetValue(item.Property, out var list)) byProperty[item.Property] = list = [];
            list.Add(item);
        }
        foreach (var prop in classValue.Properties)
        {
            if (!byProperty.TryGetValue(prop, out var list)) continue;
            var memberPath = path.Member(prop.Name);
            if (prop.IsDynamic)
            {
                // The entries in data order, the order the converter lists them in the document.
                var entries = list.OrderBy(item => item.Entry.Offset).ThenBy(item => item.Entry.Index)
                    .Select(item => new SlotImage(item.Entry.Id, item.Start, item.End)).ToList();
                var slots = new ImageFrame { Property = prop, Id = prop.DynamicMinimum!.Value, Path = memberPath, Parent = node };
                node.Add(prop.Name, prop.Codec.ReadSlotsImage(context, entries, slots));
                continue;
            }
            if (prop.Codec.ImageRefusal is string reason)
                throw context.Fail(Codes.Type, reason, list[0].Start, memberPath);
            if (prop.DataId is not null)
                throw context.Fail(Codes.Type, "PE-2c still needs a campaign map. The direct path does not store it.", list[0].Start, memberPath);
            var one = list[0];
            var frame = new ImageFrame { Property = prop, Id = one.Entry.Id, Path = memberPath, Parent = node };
            node.Add(prop.Name, prop.Codec.ReadImage(context, one.Start, one.End, frame));
        }
        context.Leave();
        return node;
    }

    private static void RefuseMapping(SchemaClass classValue, DocumentNodePath path)
    {
        if (classValue.Mapping is null) return;
        throw new ConversionException(new FormatDiagnostic(Codes.Mapping,
            "Class " + classValue.Name + " uses a case mapping. The original backend does not reproduce it.")
        { Path = path.ToString(), Value = classValue.Name });
    }

    private sealed class Located(TableEntry entry, SchemaProperty property)
    {
        public TableEntry Entry { get; } = entry;
        public SchemaProperty Property { get; } = property;
        public int Start { get; set; }
        public int End { get; set; }
    }
}
