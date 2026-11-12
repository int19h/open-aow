using Aow.Formats.Schema;

namespace Aow.Formats.Tree;

/// <summary>
/// The original image of one tree. Codecs write the bytes and read them back. There is no
/// document between the tree and the image. A class with a case mapping is refused.
/// The serializers name members only.
/// </summary>
public static class OriginalBackend
{
    public static byte[] Write(ObjectNode node, SchemaClass classValue, FormatSchema schema, string engine)
    {
        Refuse(classValue);
        return ImageTree.Write(node, classValue, schema, engine);
    }

    public static ObjectNode Read(byte[] image, SchemaClass classValue, FormatSchema schema, string engine)
    {
        Refuse(classValue);
        return ImageTree.Read(image, classValue, schema, engine);
    }

    private static void Refuse(SchemaClass classValue)
    {
        if (classValue.Mapping is null) return;
        throw new ConversionException(new FormatDiagnostic(Codes.Mapping,
            "Class " + classValue.Name + " uses a case mapping. The original backend does not reproduce it.")
        { Path = "root", Value = classValue.Name });
    }
}
