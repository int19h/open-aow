
namespace Aow.Formats.Tree;

/// <summary>Failures of the tree and of the direct document backend. The path is the document path.</summary>
internal static class TreeErrors
{
    public static ConversionException Fail(string code, string message, DocumentNodePath path, Json.JsonValue? at = null, string? value = null, string? constraint = null) =>
        new(new FormatDiagnostic(code, message)
        {
            Span = at?.Span ?? default,
            Path = path.ToString(),
            Value = value,
            Constraint = constraint,
            Migration = at?.Origin,
        });
}
