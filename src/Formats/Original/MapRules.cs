using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Original;

/// <summary>
/// The fields of a map level on the tree and the direct path. The grid's width and height are the
/// level's Width and Height members, which the schema stores before the fields.
/// </summary>
internal sealed partial class MapFieldsCodec
{
    /// <summary>The field class the level's class names (MapFieldClass).</summary>
    private static SchemaClass TreeFieldClass(FormatSchema schema, SchemaClass? ownerClass, SchemaProperty property) =>
        schema.Get((ownerClass ?? property.Owner).FieldClass ?? "MapField");

    private static int NodeDimension(ObjectNode? parent, string name) => parent?.Find(name) is IntegerNode n ? (int)n.Value : 0;

    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not GridNode grid) throw context.Fail(Codes.Type, "Fields is a grid of fields.", null, frame.Path);
        int width = NodeDimension(frame.Parent, "Width"), height = NodeDimension(frame.Parent, "Height");
        CheckGrid(grid, width, height, (code, message, stated, constraint) => context.Fail(code, message, null, frame.Path, stated, constraint));
        var fieldClass = TreeFieldClass(context.Schema, frame.OwnerClass, frame.Property);
        for (int i = 0; i < grid.Fields.Count; i++)
        {
            int at = data.Length;
            data.Int32(0);
            ImageTree.WriteBody(grid.Fields[i], fieldClass, context, data, FieldPath(frame.Path, i, width), false);
            data.PatchInt32(at, data.Length - at - 4);
        }
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        int width = NodeDimension(frame.Parent, "Width"), height = NodeDimension(frame.Parent, "Height");
        long count = (long)width * height;
        if (count > context.Limits.CollectionItems) throw context.Fail(Codes.Limit, "The map has too many fields.", start, frame.Path, Text(count));
        var fieldClass = TreeFieldClass(context.Schema, frame.OwnerClass, frame.Property);
        var fields = new ObjectNode[count];
        int position = start;
        for (int i = 0; i < fields.Length; i++)
        {
            int y = Math.DivRem(i, width, out int x);
            if (position + 4 > end) throw context.Fail(Codes.Dimensions, $"The field stream ends before field [{y}][{x}] of a {width} by {height} level.", position, frame.Path);
            int length = context.Int32(position);
            if (length <= 0 || position + 4L + length > end) throw context.Fail(Codes.Corrupt, $"Field [{y}][{x}] has an invalid length {length}.", position, frame.Path.Item(y).Item(x));
            fields[i] = ImageTree.ReadBody(context, position + 4, position + 4 + length, fieldClass, frame.Path.Item(y).Item(x), null, null);
            position += 4 + length;
        }
        if (position != end) throw context.Fail(Codes.Dimensions, $"The field stream holds more than {width} by {height} fields.", position, frame.Path);
        return new GridNode(width, height, fields);
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not GridNode grid) throw site.Mismatch("Fields is a grid of fields.");
        int width = NodeDimension(site.Parent, "Width"), height = NodeDimension(site.Parent, "Height");
        CheckGrid(grid, width, height, (code, message, stated, constraint) => site.Fail(code, message, null, stated, constraint));
        var fieldClass = TreeFieldClass(site.Schema, site.OwnerClass, site.Property!);
        var rows = new JsonArray { Layout = ArrayLayout.RowsOnLines };
        for (int y = 0; y < height; y++)
        {
            var row = new JsonArray();
            for (int x = 0; x < width; x++)
                row.Add(DocumentBackend.WriteObject(grid.Fields[y * width + x], fieldClass, site.At(site.Path.Item(y).Item(x), element: false), created: false, slotted: false));
            rows.Add(row);
        }
        return rows;
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        int width = site.Owner is { } owner ? Dimension(owner, "Width") : 0;
        int height = site.Owner is { } level ? Dimension(level, "Height") : 0;
        if (value is not JsonArray rows) throw site.Mismatch("Fields is an array of rows.", value);
        if (rows.Items.Count != height)
            throw site.Fail(Codes.Dimensions, $"Fields has {rows.Items.Count} rows and Height is {height}.", rows, Text(rows.Items.Count), $"{height} rows");
        if ((long)width * height > site.Context.Limits.CollectionItems)
            throw site.Fail(Codes.Limit, "The map has too many fields.", rows, Text((long)width * height));
        var fieldClass = TreeFieldClass(site.Schema, site.OwnerClass, site.Property!);
        var fields = new ObjectNode[width * height];
        for (int y = 0; y < height; y++)
        {
            var rowSite = site.At(site.Path.Item(y), element: false);
            if (rows.Items[y] is not JsonArray row) throw rowSite.Mismatch("A row is an array of fields.", rows.Items[y]);
            if (row.Items.Count != width)
                throw rowSite.Fail(Codes.Dimensions, $"Row {y} has {row.Items.Count} fields and Width is {width}.", row, Text(row.Items.Count), $"{width} fields");
            for (int x = 0; x < width; x++)
            {
                var fieldSite = site.At(site.Path.Item(y).Item(x), element: false);
                if (row.Items[x] is not JsonObject field) throw fieldSite.Mismatch("A field is an object.", row.Items[x]);
                fields[y * width + x] = DocumentBackend.ReadObject(field, fieldClass, fieldSite, created: false, slotted: false);
            }
        }
        return new GridNode(width, height, fields);
    }

    private delegate ConversionException GridFailure(string code, string message, string? value, string? constraint);

    private static void CheckGrid(GridNode grid, int width, int height, GridFailure fail)
    {
        if (grid.Width != width || grid.Height != height)
            throw fail(Codes.Dimensions, $"Fields is a {grid.Width} by {grid.Height} grid and the level is {width} by {height}.", $"{grid.Width} by {grid.Height}", $"{width} by {height}");
        if (grid.Fields.Count != (long)width * height)
            throw fail(Codes.Dimensions, $"Fields holds {grid.Fields.Count} fields for a {width} by {height} level.", Text(grid.Fields.Count), Text((long)width * height));
    }

    private static DocumentNodePath FieldPath(DocumentNodePath grid, int index, int width)
    {
        int y = Math.DivRem(index, width, out int x);
        return grid.Item(y).Item(x);
    }
}

/// <summary>
/// The campaign map on the tree and the direct path. The tree holds the whole HSM file as the
/// engine keeps it. The image stores its size (id 5) and its bytes (id 6). The document holds the
/// file as a nested document of profile world-hsm, imported and exported through that profile.
/// The retained compressed bytes (the nested-compression part) belong to the converter alone, so
/// the direct path compresses the exported payload again at the level the container names.
/// </summary>
internal sealed partial class CampaignMapCodec
{
    internal override void WriteMemberImage(TreeValue value, EncodeContext context, ObjectWriter writer, ImageFrame frame)
    {
        var image = Image(value, (code, message) => context.Fail(code, message, null, frame.Path));
        writer.Begin(frame.Property.Id!.Value, frame.Property);
        writer.Data.Int32(image.Length);
        writer.End();
        writer.Begin(frame.Property.DataId!.Value, frame.Property);
        writer.Data.Bytes(image);
        writer.End();
    }

    internal override TreeValue ReadPairImage(DecodeContext context, (int Start, int End)? first, (int Start, int End)? second, ImageFrame frame)
    {
        if (first is not { } size || second is not { } bytes)
            throw context.Fail(Codes.Corrupt, "A campaign map needs both its size (id 5) and its bytes (id 6).", (first ?? second)!.Value.Start, frame.Path);
        if (size.End - size.Start != 4) throw context.Fail(Codes.Corrupt, "The campaign map size is not a 32-bit integer.", size.Start, frame.Path);
        int length = context.Int32(size.Start);
        if (length <= 0 || length != bytes.End - bytes.Start)
            throw context.Fail(Codes.Corrupt, $"The campaign map size {length} differs from its {bytes.End - bytes.Start} stored bytes.", size.Start, frame.Path);
        return new NestedDocumentNode(Nested.Name, context.Data.AsSpan(bytes.Start, length).ToArray());
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        var image = Image(value, (code, message) => site.Fail(code, message));
        var session = new ImportSession
        {
            Schema = site.Schema,
            Limits = site.Context.Limits,
            Counts = new LayoutCounts(),
            File = site.Context.File,
            Nested = site.Path.ToString(),
        };
        var (container, root) = Nested.Import(image, session);
        return new JsonObject().Add("$container", container).Add("root", root);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonObject map) throw site.Mismatch("Map is a nested document.", value);
        foreach (var m in map.Members)
            if (m.Name is not ("$container" or "root" or "$layout"))
                throw site.At(site.Path.Member(m.Name), element: false).Fail(m.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember, $"Map has no member \"{m.Name}\".", m.Value, m.Name);
        var container = map["$container"] as JsonObject ?? throw site.Fail(Codes.MissingMember, "Map needs $container.", map);
        var root = map["root"] ?? throw site.Fail(Codes.MissingMember, "Map needs root.", map);
        if (container["profile"] is not JsonString { Value: "world-hsm" })
            throw site.At(site.Path.Member("$container").Member("profile"), element: false).Fail(Codes.Profile, "A campaign map has profile world-hsm.", container["profile"] ?? container);
        var nested = new EncodeContext(site.Schema, Nested.Engine, Nested.Name, site.Context.File, site.Context.Limits, site.Context.Options);
        var payload = Nested.ExportPayload(container, root, nested, out string compression, out int level);
        return new NestedDocumentNode(Nested.Name, compression == "cfs" ? Cfs.Wrap(payload, level) : payload);
    }

    private static byte[] Image(TreeValue value, Func<string, string, ConversionException> fail)
    {
        if (value is not NestedDocumentNode node) throw fail(Codes.Type, "Map is a nested document.");
        if (node.Profile != Nested.Name) throw fail(Codes.Profile, "A campaign map has profile world-hsm.");
        if (node.Image.Length == 0) throw fail(Codes.Corrupt, "A stored campaign map is not empty.");
        return node.Image;
    }
}
