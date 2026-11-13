using Aow.Formats.Schema;

namespace Aow.Formats.Tree;

/// <summary>
/// The original image of one tree. Codecs write the bytes and read them back. There is no
/// document between the tree and the image. A mapped class stores its flat members, as its
/// ReadWrite does, and the document backend names its case.
/// The serializers name members only.
/// </summary>
public static class OriginalBackend
{
    public static byte[] Write(ObjectNode node, SchemaClass classValue, FormatSchema schema, string engine) =>
        ImageTree.Write(node, classValue, schema, engine);

    public static ObjectNode Read(byte[] image, SchemaClass classValue, FormatSchema schema, string engine) =>
        ImageTree.Read(image, classValue, schema, engine);
}
