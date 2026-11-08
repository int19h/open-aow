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
        var cls = schema.Get(className);
        var ctx = new DecodeContext(image, schema, engine, engine, null, FormatLimits.Default, new LayoutCounts());
        var obj = new JsonObject();
        ObjectImage.Decode(ctx, 0, image.Length, cls, DocPath.Root, obj);
        return obj;
    }

    public static byte[] Encode(JsonObject obj, string className, string engine = "game")
    {
        var schema = FormatSchema.Current;
        var cls = schema.Get(className);
        var ctx = new EncodeContext(schema, engine, engine, null, FormatLimits.Default, ExportOptions.Default);
        var output = new ByteBuffer();
        ObjectImage.Encode(ctx, obj, cls, output, DocPath.Root, "$class", "$slot");
        return output.ToArray();
    }
}
