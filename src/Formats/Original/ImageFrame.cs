using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Original;

/// <summary>Where one tree value sits while the direct path reads or writes an original image.</summary>
internal sealed class ImageFrame
{
    public required SchemaProperty Property { get; init; }
    public required int Id { get; init; }
    public required DocumentNodePath Path { get; init; }
    public JsonObject Owner { get; init; } = new();
    public ObjectNode? Parent { get; init; }

    public ValueSite Value(DecodeContext ctx) => new()
    {
        Property = Property,
        Id = Id,
        Path = Path,
        Owner = Owner,
    };

    public ValueSite Value(EncodeContext ctx) => new()
    {
        Property = Property,
        Id = Id,
        Path = Path,
        Owner = Owner,
    };

    public DocumentSite Document(EncodeContext ctx) => new()
    {
        Property = Property,
        Path = Path,
        Schema = ctx.Schema,
        Engine = ctx.Engine,
        Context = ctx,
    };

    public DocumentSite Document(DecodeContext ctx) => new()
    {
        Property = Property,
        Path = Path,
        Schema = ctx.Schema,
        Engine = ctx.Engine,
        Context = new EncodeContext(ctx.Schema, ctx.Engine, ctx.Profile, ctx.File, ctx.Limits, ExportOptions.Default),
    };
}
