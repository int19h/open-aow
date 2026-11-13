using System.Text;

namespace Aow.Formats;

/// <summary>A location in a JSON text: zero-based character offset, one-based line and column.</summary>
public readonly record struct TextSpan(int Offset, int Line, int Column)
{
    public bool IsKnown => Line > 0;
}

/// <summary>
/// One diagnostic, shared by the converter, the game and the logs (own-formats.md 2.8). A text
/// location carries line and column, a binary location carries a byte offset and the profile,
/// never a line number.
/// </summary>
public sealed record FormatDiagnostic(string Code, string Message)
{
    public string? File { get; init; }
    public string? Path { get; init; }
    public TextSpan Span { get; init; }
    public long? Offset { get; init; }
    public string? Profile { get; init; }
    public string? Value { get; init; }
    public string? Constraint { get; init; }
    public bool Decompressed { get; init; }

    public override string ToString()
    {
        var head = new StringBuilder();
        head.Append(File ?? "<input>");
        if (Span.IsKnown) head.Append(':').Append(Span.Line).Append(':').Append(Span.Column);
        else if (Offset is long offset) head.Append(":0x").Append(offset.ToString("x", System.Globalization.CultureInfo.InvariantCulture));
        if (Decompressed) head.Append(" (decompressed)");
        head.Append(' ').Append(Code);
        if (Path is not null) head.Append(' ').Append(Path);
        if (Profile is not null && Offset is not null) head.Append(" [").Append(Profile).Append(']');
        var text = new StringBuilder(head.ToString()).Append('\n').Append(Message);
        if (Value is not null) text.Append(" Value: ").Append(Value).Append('.');
        if (Constraint is not null) text.Append(" Constraint: ").Append(Constraint).Append('.');
        return text.ToString();
    }
}

/// <summary>The stable diagnostic codes.</summary>
public static class Codes
{
    public const string Syntax = "E_SYNTAX";
    public const string Utf8 = "E_UTF8";
    public const string Duplicate = "E_DUPLICATE";
    public const string Number = "E_NUMBER";
    public const string Surrogate = "E_SURROGATE";
    public const string Limit = "E_LIMIT";
    public const string Format = "E_FORMAT";
    public const string Version = "E_VERSION";
    public const string Kind = "E_KIND";
    public const string Profile = "E_PROFILE";
    public const string UnknownMember = "E_UNKNOWN_MEMBER";
    public const string ReservedMember = "E_RESERVED_MEMBER";
    public const string MissingMember = "E_MISSING_MEMBER";
    public const string Type = "E_TYPE";
    public const string Range = "E_RANGE";
    public const string Text = "E_TEXT";
    public const string UnknownClass = "E_UNKNOWN_CLASS";
    public const string ClassFamily = "E_CLASS_FAMILY";
    public const string UnknownProperty = "E_UNKNOWN_PROPERTY";
    public const string Slot = "E_SLOT";
    public const string Dimensions = "E_DIMENSIONS";
    public const string Layout = "E_LAYOUT";
    public const string Corrupt = "E_CORRUPT";
    public const string Truncated = "E_TRUNCATED";
    public const string Checksum = "E_CHECKSUM";
    public const string Compression = "E_COMPRESSION";
    public const string Unrepresentable = "E_UNREPRESENTABLE";
    public const string Reference = "E_REFERENCE";
    public const string Required = "E_REQUIRED";
    public const string OriginalRoundTripDiffers = "E_G1";
    public const string Case = "E_CASE";
    public const string Mapping = "E_MAPPING";
    public const string LegacyShape = "E_LEGACY_SHAPE";
    public const string Output = "E_OUTPUT";
    public const string Unknown = "E_UNKNOWN_FORMAT";
}

/// <summary>A conversion failure. It carries the diagnostic and leaves no partial result.</summary>
public sealed class ConversionException(FormatDiagnostic diagnostic) : Exception(diagnostic.ToString())
{
    public FormatDiagnostic Diagnostic { get; } = diagnostic;
}
