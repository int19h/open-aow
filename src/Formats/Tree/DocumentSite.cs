using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats.Tree;

/// <summary>Where a tree value sits while a document is read or written.</summary>
internal sealed class DocumentSite
{
    public SchemaProperty? Property { get; init; }
    public required DocPath Path { get; init; }
    public required FormatSchema Schema { get; init; }
    public required string Engine { get; init; }
    public required EncodeContext Context { get; init; }
    public bool Element { get; init; }

    public DocumentSite For(SchemaProperty property) => new()
    {
        Property = property,
        Path = Path.Member(property.Name),
        Schema = Schema,
        Engine = Engine,
        Context = Context,
    };

    public DocumentSite At(DocPath path, bool element) => new()
    {
        Property = Property,
        Path = path,
        Schema = Schema,
        Engine = Engine,
        Context = Context,
        Element = element,
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
