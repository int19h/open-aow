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
    /// <summary>
    /// Writes the original image. <paramref name="profile"/> names the use. Null uses
    /// <paramref name="engine"/>, the disk image. <see cref="TransportProfile.Name"/> is the
    /// legacy packet, and it refuses a member that image cannot carry.
    /// </summary>
    public static byte[] Write(ObjectNode node, SchemaClass classValue, FormatSchema schema, string engine, string? profile = null) =>
        ImageTree.Write(node, classValue: classValue, schema: schema, engine: engine, profile: profile);

    public static ObjectNode Read(byte[] image, SchemaClass classValue, FormatSchema schema, string engine) =>
        ImageTree.Read(image, classValue, schema, engine);

    /// <summary>
    /// The data of the object image <see cref="Write"/> writes, without its property table: the
    /// values of the members the tree holds, in the order the image stores them. A load that
    /// reads past one stored value reads these bytes (<c>TBitList.LoadFromStream</c>).
    /// </summary>
    public static byte[] WriteData(ObjectNode node, SchemaClass classValue, FormatSchema schema, string engine) =>
        ImageTree.WriteData(node, classValue: classValue, schema: schema, engine: engine);
}
