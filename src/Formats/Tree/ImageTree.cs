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
    // The value a diagnostic of an object image points at. The tree has no document position.
    private static readonly JsonObject s_marker = new();

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
        context.Enter(s_marker, path);
        var writer = new ObjectWriter();
        int matched = 0;
        foreach (var prop in classValue.Properties)
        {
            var value = node.Find(prop.Name);
            if (value is null) continue;
            var memberPath = path.Member(prop.Name);
            if (prop.IsDynamic)
            {
                var slots = new ImageFrame { Property = prop, Id = prop.DynamicMinimum!.Value, Path = memberPath, Parent = node, OwnerClass = classValue };
                prop.Codec.WriteSlotsImage(value, context, writer, slots);
                matched++;
                continue;
            }
            if (prop.Codec.ImageRefusal is string reason)
                throw context.Fail(Codes.Type, reason, null, memberPath);
            matched++;
            if (!prop.Codec.StoresImage(value)) continue;
            if (prop.Id is not int id)
                throw new InvalidOperationException(prop + " has no stored id.");
            var frame = new ImageFrame { Property = prop, Id = id, Path = memberPath, Parent = node, OwnerClass = classValue };
            prop.Codec.WriteMemberImage(value, context, writer, frame);
        }
        // The tree stores each name once, so a count short of the members names a member the class does not have.
        if (matched != node.Members.Count)
            foreach (var (name, _) in node.Members)
                if (classValue.PropertyByName(name) is null)
                    throw context.Fail(Codes.UnknownMember, "Class " + classValue.Name + " has no member \"" + name + "\".", null, path.Member(name), name);
        ObjectImage.Place(context, writer, null, null, null, s_marker, path, data);
        context.Leave();
    }

    internal static ObjectNode ReadBody(DecodeContext context, int start, int end, SchemaClass classValue, DocumentNodePath path, string? className, int? slot)
    {
        RefuseMapping(classValue, path);
        context.Enter(start, path);
        var table = PropertyTable.Read(context, start, end, path);
        int db = table.DataBase;
        int length = end - db;
        var entries = table.Entries;
        int count = entries.Count;
        // The entries with their properties, sorted by schema order and then by data order. A map
        // stores millions of small objects, so this pass allocates one array and no dictionary.
        var located = new Located[count];
        for (int i = 0; i < count; i++)
        {
            var entry = entries[i];
            var prop = classValue.PropertyById(entry.Id)
                ?? throw context.Fail(Codes.UnknownProperty, "Class " + classValue.Name + " has no property " + entry.Id + ".", db + entry.Offset, path, entry.Id.ToString(CultureInfo.InvariantCulture));
            for (int k = 0; k < i; k++)
                if (entries[k].Id == entry.Id)
                    throw context.Fail(Codes.Duplicate, (prop.IsDynamic ? "Dynamic id " : "Property ") + entry.Id + " of class " + classValue.Name + " is stored twice.", start, path);
            located[i] = new Located(entry, prop, classValue.Ordinal(prop), db + entry.Offset, db + entry.Offset);
        }
        // Each valued entry runs to the next larger stored offset, or to the end of the object.
        Array.Sort(located, static (left, right) => left.Entry.Offset != right.Entry.Offset
            ? left.Entry.Offset.CompareTo(right.Entry.Offset) : left.Entry.Index.CompareTo(right.Entry.Index));
        int valuedAt = -1;
        for (int i = 0; i < count; i++)
        {
            if (located[i].Property.Codec.ZeroSize) continue;
            int offset = located[i].Entry.Offset;
            if (offset == valuedAt)
                throw context.Fail(Codes.Layout, "Two valued properties of class " + classValue.Name + " share offset " + offset + ".", db + offset, path);
            valuedAt = offset;
            int next = length;
            for (int k = i + 1; k < count; k++)
                if (located[k].Entry.Offset > offset)
                {
                    next = located[k].Entry.Offset;
                    break;
                }
            located[i] = located[i] with { End = db + next };
        }
        Array.Sort(located, static (left, right) => left.Ordinal != right.Ordinal ? left.Ordinal.CompareTo(right.Ordinal)
            : left.Entry.Offset != right.Entry.Offset ? left.Entry.Offset.CompareTo(right.Entry.Offset) : left.Entry.Index.CompareTo(right.Entry.Index));
        var node = new ObjectNode(className, slot);
        for (int first = 0; first < count;)
        {
            var prop = located[first].Property;
            int last = first + 1;
            while (last < count && located[last].Ordinal == located[first].Ordinal) last++;
            var memberPath = path.Member(prop.Name);
            if (prop.IsDynamic)
            {
                // The entries in data order, the order the converter lists them in the document.
                var images = new SlotImage[last - first];
                for (int k = first; k < last; k++) images[k - first] = new SlotImage(located[k].Entry.Id, located[k].Start, located[k].End);
                var slots = new ImageFrame { Property = prop, Id = prop.DynamicMinimum!.Value, Path = memberPath, Parent = node, OwnerClass = classValue };
                node.Add(prop.Name, prop.Codec.ReadSlotsImage(context, images, slots));
            }
            else if (prop.Codec.ImageRefusal is string reason)
                throw context.Fail(Codes.Type, reason, located[first].Start, memberPath);
            else if (prop.DataId is int dataId)
            {
                // A member of two entries, its id and its data id. Either one can be absent.
                (int Start, int End)? pairFirst = null, pairSecond = null;
                for (int k = first; k < last; k++)
                {
                    if (located[k].Entry.Id == prop.Id) pairFirst = (located[k].Start, located[k].End);
                    else if (located[k].Entry.Id == dataId) pairSecond = (located[k].Start, located[k].End);
                }
                var pair = new ImageFrame { Property = prop, Id = prop.Id!.Value, Path = memberPath, Parent = node, OwnerClass = classValue };
                node.Add(prop.Name, prop.Codec.ReadPairImage(context, pairFirst, pairSecond, pair));
            }
            else
            {
                var one = located[first];
                var frame = new ImageFrame { Property = prop, Id = one.Entry.Id, Path = memberPath, Parent = node, OwnerClass = classValue };
                node.Add(prop.Name, prop.Codec.ReadImage(context, one.Start, one.End, frame));
            }
            first = last;
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

    private readonly record struct Located(TableEntry Entry, SchemaProperty Property, int Ordinal, int Start, int End);
}
