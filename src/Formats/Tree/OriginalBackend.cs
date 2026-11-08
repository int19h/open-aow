using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats.Tree;

/// <summary>
/// The legacy adapter. It maps one object's tree to the property-table image and back through the
/// same codecs the converter uses. There is no <c>ReadWrite</c> on this path. A class with a case
/// mapping is refused. The serializers name members only.
/// </summary>
public static class OriginalBackend
{
    public static byte[] Write(ObjectNode node, SchemaClass cls, FormatSchema schema, string engine)
    {
        Refuse(cls);
        var document = DocumentBackend.Write(node, cls, schema, engine);
        var ctx = new EncodeContext(schema, engine, engine, null, FormatLimits.Default, ExportOptions.Default);
        var output = new ByteBuffer();
        ObjectImage.Encode(ctx, document, cls, output, DocPath.Root, "$class", "$slot");
        return output.ToArray();
    }

    public static ObjectNode Read(byte[] image, SchemaClass cls, FormatSchema schema, string engine)
    {
        Refuse(cls);
        var ctx = new DecodeContext(image, schema, engine, engine, null, FormatLimits.Default, new LayoutCounts());
        var document = new JsonObject();
        ObjectImage.Decode(ctx, 0, image.Length, cls, DocPath.Root, document);
        return DocumentBackend.Read(document, cls, schema, engine);
    }

    private static void Refuse(SchemaClass cls)
    {
        if (cls.Mapping is null) return;
        throw new ConversionException(new FormatDiagnostic(Codes.Mapping,
            "Class " + cls.Name + " uses a case mapping. The original backend does not reproduce it.")
        { Path = "root", Value = cls.Name });
    }
}
