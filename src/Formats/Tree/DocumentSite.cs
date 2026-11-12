using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats.Tree;

/// <summary>Where a tree value sits while a document is read or written.</summary>
internal sealed class DocumentSite
{
    public SchemaProperty? Property { get; init; }
    public required DocumentNodePath Path { get; init; }
    public required FormatSchema Schema { get; init; }
    public required string Engine { get; init; }
    public required EncodeContext Context { get; init; }
    public bool Element { get; init; }

    /// <summary>The object being read. A bit array takes its count from a sibling on this object.</summary>
    public JsonObject? Owner { get; init; }

    /// <summary>The tree object being written. A map level's grid checks its width and height against it.</summary>
    public ObjectNode? Parent { get; init; }

    /// <summary>The class of the object being read or written. A map level's class names its field class.</summary>
    public SchemaClass? OwnerClass { get; init; }

    public DocumentSite For(SchemaProperty property) => For(property, Parent, OwnerClass);

    /// <summary>The site of a member of <paramref name="parent"/>, an object of <paramref name="ownerClass"/>.</summary>
    public DocumentSite For(SchemaProperty property, ObjectNode? parent, SchemaClass? ownerClass) => new()
    {
        Property = property,
        Path = Path.Member(property.Name),
        Schema = Schema,
        Engine = Engine,
        Context = Context,
        Owner = Owner,
        Parent = parent,
        OwnerClass = ownerClass,
    };

    public DocumentSite At(DocumentNodePath path, bool element) => new()
    {
        Property = Property,
        Path = path,
        Schema = Schema,
        Engine = Engine,
        Context = Context,
        Element = element,
        Owner = Owner,
        Parent = Parent,
        OwnerClass = OwnerClass,
    };

    public DocumentSite WithOwner(JsonObject owner) => new()
    {
        Property = Property,
        Path = Path,
        Schema = Schema,
        Engine = Engine,
        Context = Context,
        Element = Element,
        Owner = owner,
        Parent = Parent,
        OwnerClass = OwnerClass,
    };

    public ConversionException Fail(string code, string message, JsonValue? at = null, string? value = null, string? constraint = null) =>
        TreeErrors.Fail(code, message, Path, at, value, constraint);

    public ConversionException Mismatch(string message, JsonValue? at = null) =>
        Fail(Codes.Type, message, at, at?.KindName);

    public void RequireText(string text, JsonValue? at = null)
    {
        if (TextCodec.Problem(text, Context.Limits.StringBytes) is { } problem)
            throw Fail(problem.Code, problem.Message, at, problem.Value, problem.Constraint);
    }
}
