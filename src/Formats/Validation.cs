using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats;

/// <summary>
/// A data-only validator of a migrated document (own-formats.md 2.7). Formats owns structural
/// validation through a document read. Units supplies family, ownership, reference and content
/// validators from 9.7-3 on. A validator reads the document and read-only content context,
/// creates no game object and never repairs anything.
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
    public FormatDiagnostic At(string code, string message, JsonValue value, DocumentNodePath path, string? rejected = null, string? constraint = null)
        => new(code, message)
        {
            File = File,
            Span = value.Span,
            Path = path.ToString(),
            Value = rejected,
            Constraint = constraint,
        };
}

/// <summary>What <see cref="DocumentValidation.Check"/> found. The structural step reads the document and does not encode an image.</summary>
public sealed record DocumentCheck(IReadOnlyList<FormatDiagnostic> Findings);

public static class DocumentValidation
{
    /// <summary>
    /// Runs the structural validation, then every validator. It returns every finding, so a
    /// caller refuses before any destructive load step and leaves the current state intact.
    /// </summary>
    public static IReadOnlyList<FormatDiagnostic> Run(JsonObject document, IEnumerable<IDocumentValidator> validators, string? file = null)
        => Check(document, validators, file).Findings;

    /// <summary>
    /// <see cref="Run"/> with the same findings. The structural step reads the root through
    /// <see cref="DocumentBackend"/> after the profile checks the root class. A document that
    /// holds port-only state stays valid. The original export keeps its own refusal. A validator
    /// runs only when the structure passed.
    /// </summary>
    public static DocumentCheck Check(JsonObject document, IEnumerable<IDocumentValidator> validators, string? file = null)
    {
        FileProfile profile;
        string kind;
        try
        {
            JsonValue rootValue;
            (profile, kind, _, rootValue) = GameFiles.Envelope(document, fileName: file);
            if (rootValue is not JsonObject root)
            {
                throw new ConversionException(new FormatDiagnostic(Codes.Type, Message: "The root is an object.")
                { File = file, Span = rootValue.Span, Path = "root" });
            }

            FormatSchema schema = GameFiles.Schema;
            SchemaClass classValue = profile.RootClass(new EncodeContext(schema, profile.Engine, profile: profile.Name, file, limits: FormatLimits.Default), root: root);
            JsonObject body = root;
            if (profile.RootHasClass)
            {
                // The copy drops $class, which is not a member of the class. The member objects
                // stay, so a refusal of a root member still names the line and column.
                body = new JsonObject { Span = root.Span };
                body.Members.AddRange(root.Members.Where(item => item.Name != "$class"));
            }
            _ = DocumentBackend.Read(body, classValue: classValue, schema: schema, engine: profile.Engine, file: file);
        }
        catch (ConversionException error)
        {
            FormatDiagnostic diagnostic = error.Diagnostic;
            if (diagnostic.File is null && file is not null)
            {
                diagnostic = diagnostic with { File = file };
            }

            return new DocumentCheck(Findings: [diagnostic]);
        }
        List<FormatDiagnostic> findings = [];
        ValidationContext context = new(document, profile, kind, file);
        foreach (IDocumentValidator validator in validators)
        {
            findings.AddRange(validator.Validate(context));
        }

        return new DocumentCheck(Findings: findings);
    }
}
