using Aow.Formats.Json;
using Aow.Formats.Original;

namespace Aow.Formats;

/// <summary>
/// A data-only validator of a migrated document (own-formats.md 2.7). Formats owns structural
/// validation through the strict export. Units supplies family, ownership, reference and
/// content validators from 9.7-3 on. A validator reads the document and read-only content
/// context, creates no game object and never repairs anything.
/// </summary>
public interface IDocumentValidator
{
    /// <summary>A stable name for logs and coverage reports.</summary>
    string Name { get; }

    IEnumerable<FormatDiagnostic> Validate(ValidationContext context);
}

/// <summary>What a validator sees: the document, its profile and kind, and diagnostic helpers.</summary>
public sealed class ValidationContext(JsonObject document, FileProfile profile, string kind, string? file)
{
    public JsonObject Document { get; } = document;
    public FileProfile Profile { get; } = profile;
    public string Kind { get; } = kind;
    public string? File { get; } = file;
    public JsonObject Root => (JsonObject)Document["root"]!;

    /// <summary>A located diagnostic for a value: path, line and column, rejected value and constraint.</summary>
    public FormatDiagnostic At(string code, string message, JsonValue value, DocumentNodePath path, string? rejected = null, string? constraint = null) =>
        new(code, message)
        {
            File = File,
            Span = value.Span,
            Path = path.ToString(),
            Value = rejected,
            Constraint = constraint,
        };
}

/// <summary>
/// What <see cref="DocumentValidation.Check"/> found: every finding, and the engine image of the
/// document when the structural step passed.
/// </summary>
public sealed record DocumentCheck(IReadOnlyList<FormatDiagnostic> Findings, byte[]? Image);

public static class DocumentValidation
{
    /// <summary>
    /// Runs the structural validation, then every validator. It returns every finding, so a
    /// caller refuses before any destructive load step and leaves the current state intact.
    /// </summary>
    public static IReadOnlyList<FormatDiagnostic> Run(JsonObject document, IEnumerable<IDocumentValidator> validators, string? file = null) =>
        Check(document, validators, file).Findings;

    /// <summary>
    /// <see cref="Run"/> that also returns the engine image the structural step encoded. The
    /// structural step is the strict encoding of the export without its compression
    /// (<see cref="GameFiles.ExportImage"/>), so it refuses whatever the export refuses. A
    /// validator runs only when the structure passed.
    /// </summary>
    public static DocumentCheck Check(JsonObject document, IEnumerable<IDocumentValidator> validators, string? file = null)
    {
        FileProfile profile;
        string kind;
        byte[] image;
        try
        {
            (profile, kind, _, _) = GameFiles.Envelope(document, file);
            image = GameFiles.ExportImage(document, file);
        }
        catch (ConversionException error)
        {
            return new DocumentCheck(Findings: [error.Diagnostic], Image: null);
        }
        var findings = new List<FormatDiagnostic>();
        var context = new ValidationContext(document, profile, kind, file);
        foreach (var validator in validators) findings.AddRange(validator.Validate(context));
        return new DocumentCheck(Findings: findings, Image: image);
    }
}
