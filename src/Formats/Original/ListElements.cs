using Aow.Formats.Json;
using Aow.Formats.Tree;

namespace Aow.Formats.Original;

/// <summary>
/// How one value of a list with a stored default appears in the document from version 4
/// (own-formats.md 2.3). The image and the tree hold the stored number. The default and every
/// item are written through the same element.
/// </summary>
internal abstract class ListElement
{
    /// <summary>The document value of a stored number.</summary>
    internal abstract JsonValue Write(long value, DecodeContext context, DocumentNodePath path);

    /// <summary>The stored number of a document value.</summary>
    internal abstract long Read(JsonValue value, EncodeContext context, DocumentNodePath path);

    /// <summary>A context for <see cref="Write"/> where only an encode context exists.</summary>
    internal static DecodeContext Decoding(EncodeContext context) =>
        new([], context.Schema, context.Engine, context.Profile, context.File, context.Limits, new LayoutCounts());
}

/// <summary>An enum byte: the C# member name, or the integer when the enum declares no such member.</summary>
internal sealed class EnumListElement(string typeName, int width) : ListElement
{
    private readonly EnumCodec _codec = new(typeName, width, signed: width == 4);

    public string TypeName { get; } = typeName;

    internal override JsonValue Write(long value, DecodeContext context, DocumentNodePath path) =>
        _codec.MemberText(value, context.File, path.ToString());

    internal override long Read(JsonValue value, EncodeContext context, DocumentNodePath path) =>
        _codec.Parse(context, value, path, "A list value");
}

/// <summary>
/// A 4-byte record, such as a map coordinate of a move path. The document is the record's
/// fields with zero fields omitted.
/// </summary>
internal sealed class RecordListElement : ListElement
{
    private readonly CatalogRecordCodec _codec;

    public RecordListElement(string typeName) => _codec = new CatalogRecordCodec(typeName, 4);

    /// <summary>The version 3 to 4 step builds its records from this layout and does not consult the live catalog.</summary>
    internal RecordListElement(RecordTypeInfo pinned) => _codec = new CatalogRecordCodec(pinned);

    public string TypeName => _codec.TypeName;

    internal override JsonValue Write(long value, DecodeContext context, DocumentNodePath path)
    {
        var bytes = IntegerBytes.Bytes(value, 4);
        var nested = new DecodeContext(bytes, context.Schema, context.Engine, context.Profile, context.File, context.Limits, new LayoutCounts());
        return _codec.Decode(nested, 0, bytes.Length, Site(path));
    }

    internal override long Read(JsonValue value, EncodeContext context, DocumentNodePath path)
    {
        var buffer = new ByteBuffer(4);
        _codec.Encode(context, value, buffer, Site(path));
        return IntegerBytes.Read(buffer.Span, 4, signed: true);
    }

    private static ValueSite Site(DocumentNodePath path) => new() { Property = null!, Id = 0, Path = path, Owner = new JsonObject() };
}
