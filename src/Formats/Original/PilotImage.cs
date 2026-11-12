using Aow.Formats.Json;
using Aow.Formats.Schema;

namespace Aow.Formats.Original;

/// <summary>
/// Decodes and encodes one object image under a named class. Tests compare this with the direct
/// document backend. It is not part of the public format API.
/// </summary>
internal static class PilotImage
{
    public static JsonObject Decode(byte[] image, string className, string engine = "game")
    {
        var schema = FormatSchema.Current;
        var classValue = schema.Get(className);
        var context = new DecodeContext(image, schema, engine, engine, null, FormatLimits.Default, new LayoutCounts());
        var objectValue = new JsonObject();
        ObjectImage.Decode(context, 0, image.Length, classValue, DocumentNodePath.Root, objectValue);
        return objectValue;
    }

    public static byte[] Encode(JsonObject objectValue, string className, string engine = "game")
    {
        var schema = FormatSchema.Current;
        var classValue = schema.Get(className);
        var context = new EncodeContext(schema, engine, engine, null, FormatLimits.Default, ExportOptions.Default);
        var output = new ByteBuffer();
        ObjectImage.Encode(context, objectValue, classValue, output, DocumentNodePath.Root, "$class", "$slot");
        return output.ToArray();
    }
}
