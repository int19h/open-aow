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
        FormatSchema schema = FormatSchema.Current;
        SchemaClass classValue = schema.Get(className);
        DecodeContext context = new(image, schema, engine, profile: engine, file: null, limits: FormatLimits.Default);
        JsonObject objectValue = new();
        ObjectImage.Decode(context, start: 0, end: image.Length, classValue, path: DocumentNodePath.Root, target: objectValue);
        return objectValue;
    }

    public static byte[] Encode(JsonObject objectValue, string className, string engine = "game")
    {
        FormatSchema schema = FormatSchema.Current;
        SchemaClass classValue = schema.Get(className);
        EncodeContext context = new(schema, engine, profile: engine, file: null, limits: FormatLimits.Default);
        ByteBuffer output = new();
        ObjectImage.Encode(context, objectValue, classValue, output, path: DocumentNodePath.Root, "$class", "$slot");
        return output.ToArray();
    }
}
