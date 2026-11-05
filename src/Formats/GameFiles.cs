using System.Globalization;
using System.IO.Compression;
using Aow.Formats.Json;
using Aow.Formats.Migrations;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats;

public static class OwnFormat
{
    public const string FormatName = "OpenAoW";
    public const int CurrentVersion = 1;
}

/// <summary>What content detection found (own-formats.md 2.9).</summary>
public enum DetectedFormat
{
    OwnJson,
    GzipOwnJson,
    Original,
    MalformedOwn,
    UnsupportedVersion,
    UnsupportedKind,
    Unknown,
}

public sealed record Classification(DetectedFormat Format, FileProfile? Profile, string? Kind, int? Version, FormatDiagnostic? Error);

public sealed class ImportResult
{
    public required JsonObject Document { get; init; }
    public required LayoutCounts Counts { get; init; }
}

public sealed class ExportResult
{
    public required byte[] Bytes { get; init; }
    public required FileProfile Profile { get; init; }
    public required IReadOnlyList<FormatDiagnostic> Invalidated { get; init; }
}

public sealed class ReadResult
{
    public required JsonObject Document { get; init; }
    public required int DeclaredVersion { get; init; }
    public required IReadOnlyList<string> Migrations { get; init; }
    public required bool Gzip { get; init; }
}

/// <summary>
/// The file boundary of the in-scope profiles (own-formats.md 4.1). Conversion never creates a
/// game object: originals import to documents and documents export to originals through the
/// schema alone.
/// </summary>
public static class GameFiles
{
    public static FormatSchema Schema => FormatSchema.Current;

    /// <summary>The kind an original extension suggests. A .hsm needs its header to pick a profile.</summary>
    public static string? KindForExtension(string? extension) => extension?.ToLowerInvariant() switch
    {
        ".asg" => "save",
        ".hsm" => "map",
        ".csm" => "campaign-map",
        ".cam" => "campaign",
        ".acg" => "campaign-save",
        ".hss" => "scenario-set",
        ".ahl" => "hero-list",
        ".ail" => "item-list",
        _ => null,
    };

    /// <summary>
    /// Picks the original profile for a kind, using the HSM header class where world and
    /// tactical maps share the .hsm extension. Ambiguous input returns null.
    /// </summary>
    public static FileProfile? ProfileFor(string kind, byte[] file)
    {
        var candidates = FileProfile.All.Where(p => p.Kinds.Contains(kind)).ToList();
        if (candidates.Count == 1) return candidates[0];
        if (candidates.All(p => p is HsmProfile) && HsmProfile.HeaderClassId(file) is int id)
        {
            var matching = candidates.Where(p => Schema.Resolve(p.Engine, id) is { } c && c.InheritsFrom(p.RootFamily)).ToList();
            if (matching.Count == 1) return matching[0];
        }
        return null;
    }

    public static ImportResult Import(byte[] file, FileProfile profile, string kind, string? fileName = null, FormatLimits? limits = null)
    {
        if (!profile.Kinds.Contains(kind))
            throw new ConversionException(new FormatDiagnostic(Codes.Kind, $"Profile {profile.Name} does not hold kind {kind}.") { File = fileName, Profile = profile.Name });
        var counts = new LayoutCounts();
        var session = new ImportSession { Schema = Schema, Limits = limits ?? FormatLimits.Default, Counts = counts, File = fileName };
        if (file.LongLength > session.Limits.DecompressedBytes)
            throw new ConversionException(new FormatDiagnostic(Codes.Limit, "The file is too large.") { File = fileName, Offset = 0, Profile = profile.Name });
        var (container, root) = profile.Import(file, session);
        var document = new JsonObject()
            .Add("$format", new JsonString(OwnFormat.FormatName))
            .Add("$version", new JsonInteger(OwnFormat.CurrentVersion))
            .Add("$kind", new JsonString(kind))
            .Add("$container", container)
            .Add("root", root);
        return new ImportResult { Document = document, Counts = counts };
    }

    /// <summary>Strict export: refuses unrepresentable state, writes nothing partial.</summary>
    public static ExportResult Export(JsonObject document, ExportOptions? options = null, string? fileName = null, FormatLimits? limits = null)
    {
        var (profile, _, container, root) = Envelope(document, fileName);
        var ctx = new EncodeContext(Schema, profile.Engine, profile.Name, fileName, limits ?? FormatLimits.Default, options ?? ExportOptions.Default);
        var bytes = profile.Export(container, root, ctx);
        return new ExportResult { Bytes = bytes, Profile = profile, Invalidated = ctx.Invalidated };
    }

    /// <summary>Validates the envelope of a current-version document.</summary>
    public static (FileProfile Profile, string Kind, JsonObject Container, JsonValue Root) Envelope(JsonObject document, string? fileName)
    {
        ConversionException Fail(string code, string message, JsonValue at, string path, string? value = null) =>
            new(new FormatDiagnostic(code, message) { File = fileName, Span = at.Span, Path = path, Value = value, Migration = at.Origin });
        foreach (var m in document.Members)
        {
            if (m.Name is "$format" or "$version" or "$kind" or "$container" or "root") continue;
            throw new ConversionException(new FormatDiagnostic(m.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                m.Name is "$mods" or "$ext" ? $"Reserved member \"{m.Name}\" is not accepted in version {OwnFormat.CurrentVersion}." : $"The document has no member \"{m.Name}\".")
            { File = fileName, Span = m.NameSpan, Path = m.Name, Value = m.Name });
        }
        if (document["$format"] is not JsonString { Value: OwnFormat.FormatName })
            throw Fail(Codes.Format, "$format is not \"OpenAoW\".", document["$format"] ?? document, "$format");
        if (document["$version"] is not JsonInteger { Value: OwnFormat.CurrentVersion } version)
            throw Fail(Codes.Version, $"The document is not version {OwnFormat.CurrentVersion}.", document["$version"] ?? document, "$version");
        _ = version;
        if (document["$kind"] is not JsonString kind || !FileProfile.AllKinds.Contains(kind.Value))
            throw Fail(Codes.Kind, "$kind is not a supported kind.", document["$kind"] ?? document, "$kind", (document["$kind"] as JsonString)?.Value);
        if (document["$container"] is not JsonObject container)
            throw Fail(Codes.MissingMember, "The document needs $container.", document["$container"] ?? document, "$container");
        if (container["profile"] is not JsonString profileName || FileProfile.Find(profileName.Value) is not { } profile)
            throw Fail(Codes.Profile, "$container.profile is not a supported profile.", container["profile"] ?? container, "$container.profile");
        if (!profile.Kinds.Contains(kind.Value))
            throw Fail(Codes.Kind, $"Kind {kind.Value} does not belong to profile {profile.Name}.", kind, "$kind", kind.Value);
        var root = document["root"] ?? throw Fail(Codes.MissingMember, "The document needs root.", document, "root");
        return (profile, kind.Value, container, root);
    }

    /// <summary>The canonical own bytes. Writers never emit gzip or a byte order mark.</summary>
    public static byte[] Write(JsonObject document) => JsonWriter.ToUtf8(document);

    public static bool IsGzip(ReadOnlySpan<byte> bytes) => bytes.Length >= 2 && bytes[0] == 0x1F && bytes[1] == 0x8B;

    /// <summary>Whether the first non-whitespace character after an optional BOM is '{'.</summary>
    public static bool IsJsonCandidate(ReadOnlySpan<byte> bytes)
    {
        int i = bytes.StartsWith("\uFEFF"u8) ? 3 : 0;
        while (i < bytes.Length && bytes[i] is (byte)' ' or (byte)'\t' or (byte)'\r' or (byte)'\n') i++;
        return i < bytes.Length && bytes[i] == (byte)'{';
    }

    /// <summary>
    /// Reads an own document: bounded gunzip when gzip magic is present, strict parsing, the
    /// envelope check, then migration to the current version. Newer versions fail with both
    /// numbers. Nothing here falls back to the original formats.
    /// </summary>
    public static ReadResult Read(byte[] bytes, string? fileName = null, FormatLimits? limits = null, MigrationChain? chain = null)
    {
        limits ??= FormatLimits.Default;
        chain ??= MigrationChain.Default;
        bool gzip = IsGzip(bytes);
        if (gzip) bytes = Gunzip(bytes, limits, fileName);
        var value = JsonReader.Parse(bytes, fileName, gzip, limits);
        if (value is not JsonObject document)
            throw new ConversionException(new FormatDiagnostic(Codes.Format, "The document is not a JSON object.") { File = fileName, Span = value.Span, Decompressed = gzip });
        if (document["$format"] is not JsonString { Value: OwnFormat.FormatName })
            throw new ConversionException(new FormatDiagnostic(Codes.Format, "$format is not \"OpenAoW\".")
            { File = fileName, Span = (document["$format"] ?? document).Span, Path = "$format", Decompressed = gzip });
        if (document["$version"] is not JsonInteger { Value: >= 1 } versionValue)
            throw new ConversionException(new FormatDiagnostic(Codes.Version, "$version is a positive integer.")
            { File = fileName, Span = (document["$version"] ?? document).Span, Path = "$version", Decompressed = gzip });
        if (versionValue.Value > chain.Current)
            throw new ConversionException(new FormatDiagnostic(Codes.Version,
                $"The document has version {versionValue.Value}, newer than the supported version {chain.Current}.")
            { File = fileName, Span = versionValue.Span, Path = "$version", Value = versionValue.Value.ToString(CultureInfo.InvariantCulture), Decompressed = gzip });
        int declared = (int)versionValue.Value;
        var applied = new List<string>();
        document = chain.Migrate(document, declared, applied);
        return new ReadResult { Document = document, DeclaredVersion = declared, Migrations = applied, Gzip = gzip };
    }

    private static byte[] Gunzip(byte[] bytes, FormatLimits limits, string? fileName)
    {
        try
        {
            using var input = new MemoryStream(bytes, false);
            using var gz = new GZipStream(input, CompressionMode.Decompress);
            using var output = new MemoryStream();
            var buffer = new byte[81920];
            int read;
            while ((read = gz.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (output.Length + read > limits.DecompressedBytes)
                    throw new ConversionException(new FormatDiagnostic(Codes.Limit, $"The decompressed document exceeds {limits.DecompressedBytes} bytes.") { File = fileName, Offset = 0 });
                output.Write(buffer, 0, read);
            }
            return output.ToArray();
        }
        catch (InvalidDataException error)
        {
            throw new ConversionException(new FormatDiagnostic(Codes.Compression, "The gzip document is corrupt: " + error.Message) { File = fileName, Offset = 0 });
        }
    }

    /// <summary>
    /// N(D): parse, migrate, validate and write canonical JSON. For the version 1 schema every
    /// valid document is original-representable, so validation is the strict export and the
    /// canonical form is the import of its result. Valid residue survives and encoding
    /// invalidated by edits is reported.
    /// </summary>
    public static (byte[] Bytes, IReadOnlyList<FormatDiagnostic> Invalidated) Normalize(byte[] own, string? fileName = null, FormatLimits? limits = null, MigrationChain? chain = null)
    {
        var read = Read(own, fileName, limits, chain);
        var (profile, kind, _, _) = Envelope(read.Document, fileName);
        var exported = Export(read.Document, null, fileName, limits);
        var imported = Import(exported.Bytes, profile, kind, fileName, limits);
        return (Write(imported.Document), exported.Invalidated);
    }

    /// <summary>Content detection (own-formats.md 2.9). It never retries binary after a JSON error.</summary>
    public static Classification Classify(byte[] bytes, string? extension, string? kind = null, string? profileName = null)
    {
        if (IsGzip(bytes) || IsJsonCandidate(bytes))
        {
            try
            {
                var read = Read(bytes);
                var (profile, docKind, _, _) = Envelope(read.Document, null);
                return new Classification(read.Gzip ? DetectedFormat.GzipOwnJson : DetectedFormat.OwnJson, profile, docKind, read.DeclaredVersion, null);
            }
            catch (ConversionException error)
            {
                var format = error.Diagnostic.Code switch
                {
                    Codes.Version => DetectedFormat.UnsupportedVersion,
                    Codes.Kind => DetectedFormat.UnsupportedKind,
                    _ => DetectedFormat.MalformedOwn,
                };
                return new Classification(format, null, null, null, error.Diagnostic);
            }
        }
        kind ??= KindForExtension(extension);
        if (kind is null) return new Classification(DetectedFormat.Unknown, null, null, null, null);
        var p = profileName is not null ? FileProfile.Find(profileName) : ProfileFor(kind, bytes);
        if (p is null || !p.Kinds.Contains(kind)) return new Classification(DetectedFormat.Unknown, null, kind, null, null);
        return new Classification(DetectedFormat.Original, p, kind, null, null);
    }
}
