using System.Diagnostics.CodeAnalysis;
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

    /// <summary>The class of the object that holds the value. A map level's class names its field class.</summary>
    public SchemaClass? OwnerClass { get; init; }

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The context parameter disambiguates this overload from the encode overload.")]
    public ValueSite Value(DecodeContext context) => new()
    {
        Property = Property,
        Id = Id,
        Path = Path,
        Owner = Owner,
    };

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The context parameter disambiguates this overload from the decode overload.")]
    public ValueSite Value(EncodeContext context) => new()
    {
        Property = Property,
        Id = Id,
        Path = Path,
        Owner = Owner,
    };

    public DocumentSite Document(EncodeContext context) => new()
    {
        Property = Property,
        Path = Path,
        Schema = context.Schema,
        Engine = context.Engine,
        Context = context,
    };

    public DocumentSite Document(DecodeContext context) => new()
    {
        Property = Property,
        Path = Path,
        Schema = context.Schema,
        Engine = context.Engine,
        Context = new EncodeContext(context.Schema, context.Engine, context.Profile, context.File, context.Limits),
    };
}
