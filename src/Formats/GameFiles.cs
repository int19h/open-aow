using System.Globalization;
using System.IO.Compression;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats;

public static class OwnFormat
{
    public const string FormatName = "OpenAoW";
    public const int CurrentVersion = 8;
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
    public required IReadOnlyDictionary<string, int> LegacyRules { get; init; }
}

public sealed class ExportResult
{
    public required byte[] Bytes { get; init; }
    public required FileProfile Profile { get; init; }
}

public sealed class ReadResult
{
    public required JsonObject Document { get; init; }
    public required int DeclaredVersion { get; init; }
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

    /// <summary>
    /// The kind an original or own extension suggests (DocumentKind). A .hsm or .ohsm needs its
    /// header to pick a profile.
    /// </summary>
    public static string? KindForExtension(string? extension) => DocumentKind.ForExtension(extension)?.Name;

    /// <summary>
    /// Picks the original profile for a kind, using the HSM header class where world and
    /// tactical maps share the .hsm extension. Ambiguous input returns null.
    /// </summary>
    public static FileProfile? ProfileFor(string kind, byte[] file)
    {
        List<FileProfile> candidates = [.. FileProfile.All.Where(p => p.Kinds.Contains(kind))];
        if (candidates.Count == 1)
        {
            return candidates[0];
        }

        if (candidates.All(p => p is HsmProfile) && HsmProfile.HeaderClassId(file) is int id)
        {
            List<FileProfile> matching = [.. candidates.Where(p => Schema.Resolve(p.Engine, classId: id) is { } c && c.InheritsFrom(p.RootFamily))];
            if (matching.Count == 1)
            {
                return matching[0];
            }
        }
        return null;
    }

    public static ImportResult Import(byte[] file, FileProfile profile, string kind, string? fileName = null, FormatLimits? limits = null)
    {
        if (!profile.Kinds.Contains(kind))
        {
            throw new ConversionException(new FormatDiagnostic(Codes.Kind, Message: $"Profile {profile.Name} does not hold kind {kind}.") { File = fileName, Profile = profile.Name });
        }

        Dictionary<string, int> legacyRules = [with(StringComparer.Ordinal)];
        ImportSession session = new() { Schema = Schema, Limits = limits ?? FormatLimits.Default, LegacyRules = legacyRules, File = fileName };
        if (file.LongLength > session.Limits.DecompressedBytes)
        {
            throw new ConversionException(new FormatDiagnostic(Codes.Limit, Message: "The file is too large.") { File = fileName, Offset = 0, Profile = profile.Name });
        }

        (JsonObject? container, JsonObject? root) = profile.Import(file, session);
        return new ImportResult { Document = NewDocument(kind, container, root), LegacyRules = legacyRules };
    }

    /// <summary>Strict export: refuses unrepresentable state, writes nothing partial.</summary>
    public static ExportResult Export(JsonObject document, string? fileName = null, FormatLimits? limits = null)
    {
        (FileProfile? profile, string _, JsonObject? container, JsonValue? root) = Envelope(document, fileName);
        EncodeContext context = new(Schema, profile.Engine, profile: profile.Name, file: fileName, limits: limits ?? FormatLimits.Default);
        byte[] bytes = profile.Export(container, root, context);
        return new ExportResult { Bytes = bytes, Profile = profile };
    }

    /// <summary>
    /// The engine image of a current-version document (own-formats.md 4.1). It runs the same strict
    /// encoding as the export, so it refuses whatever the export refuses, but it never compresses:
    /// the image is what the engine's stream code reads.
    /// </summary>
    public static byte[] ExportImage(JsonObject document, string? fileName = null, FormatLimits? limits = null)
    {
        (FileProfile? profile, string _, JsonObject? container, JsonValue? root) = Envelope(document, fileName);
        return profile.ExportImage(context: new EncodeContext(Schema, profile.Engine, profile: profile.Name, file: fileName, limits: limits ?? FormatLimits.Default), container: container, root: root);
    }

    /// <summary>Imports an image the engine wrote, in the profile's game container.</summary>
    public static ImportResult ImportImage(byte[] image, FileProfile profile, string kind, string? fileName = null, FormatLimits? limits = null)
    {
        if (!profile.Kinds.Contains(kind))
        {
            throw new ConversionException(new FormatDiagnostic(Codes.Kind, Message: $"Profile {profile.Name} does not hold kind {kind}.") { File = fileName, Profile = profile.Name });
        }

        Dictionary<string, int> legacyRules = [with(StringComparer.Ordinal)];
        ImportSession session = new() { Schema = Schema, Limits = limits ?? FormatLimits.Default, LegacyRules = legacyRules, File = fileName };
        if (image.LongLength > session.Limits.DecompressedBytes)
        {
            throw new ConversionException(new FormatDiagnostic(Codes.Limit, Message: "The image is too large.") { File = fileName, Offset = 0, Profile = profile.Name });
        }

        (JsonObject? container, JsonObject? root) = profile.ImportImage(image, session);
        return new ImportResult { Document = NewDocument(kind, container, root), LegacyRules = legacyRules };
    }

    /// <summary>
    /// Rule-based G1 comparison of an original file against its exported round trip
    /// (own-formats.md 2.5 and 3.3).
    /// </summary>
    public static OriginalComparisonResult CompareG1(byte[] file, byte[] exported, FileProfile profile, JsonObject document, string kind, FormatLimits? limits = null)
        => OriginalComparison.CompareFile(file, exportedFile: exported, profile: profile, originalDocument: document, kind: kind, schema: Schema, limits: limits ?? FormatLimits.Default);

    /// <summary>
    /// Rule-based G1 comparison of an original file against its exported engine image without
    /// recompressing.
    /// </summary>
    public static OriginalComparisonResult CompareG1Image(byte[] file, FileProfile profile, JsonObject container, byte[] image, JsonObject? document = null, string? kind = null, FormatLimits? limits = null)
        => OriginalComparison.CompareImage(file, exportedImage: image, profile: profile, container: container, originalDocument: document, kind: kind, schema: Schema, limits: limits ?? FormatLimits.Default);

    /// <summary>
    /// G1 check for one original file (own-formats.md 3.3): returns the first differing offset
    /// outside declared don't-care rules, or null when the comparison passes.
    /// </summary>
    public static long? FirstG1Difference(byte[] file, FileProfile profile, JsonObject container, byte[] image, FormatLimits? limits = null)
    {
        OriginalComparisonResult result = CompareG1Image(file, profile: profile, container: container, image: image, document: null, kind: null, limits: limits);
        return result.Kind == OriginalMatchKind.Mismatch ? result.FirstDifferenceOffset : null;
    }

    internal static JsonObject NewDocument(string kind, JsonObject container, JsonObject root) => new JsonObject()
        .Add("$format", new JsonString(OwnFormat.FormatName))
        .Add("$version", new JsonInteger(OwnFormat.CurrentVersion))
        .Add("$kind", new JsonString(kind))
        .Add("$container", container)
        .Add("root", root);

    /// <summary>Validates the envelope of a current-version document.</summary>
    public static (FileProfile Profile, string Kind, JsonObject Container, JsonValue Root) Envelope(JsonObject document, string? fileName)
    {
        ConversionException Fail(string code, string message, JsonValue at, string path, string? value = null)
            => new(new FormatDiagnostic(code, message) { File = fileName, Span = at.Span, Path = path, Value = value });
        foreach (JsonMember m in document.Members)
        {
            if (m.Name is "$format" or "$version" or "$kind" or "$container" or "root")
            {
                continue;
            }

            throw new ConversionException(new FormatDiagnostic(m.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                Message: m.Name is "$mods" or "$ext" ? $"Reserved member \"{m.Name}\" is not accepted in version {OwnFormat.CurrentVersion}." : $"The document has no member \"{m.Name}\".")
            { File = fileName, Span = m.NameSpan, Path = m.Name, Value = m.Name });
        }
        if (document["$format"] is not JsonString { Value: OwnFormat.FormatName })
        {
            throw Fail(Codes.Format, message: "$format is not \"OpenAoW\".", at: document["$format"] ?? document, path: "$format");
        }

        if (document["$version"] is not JsonInteger { Value: OwnFormat.CurrentVersion } version)
        {
            throw Fail(Codes.Version, message: $"The document is not version {OwnFormat.CurrentVersion}.", at: document["$version"] ?? document, path: "$version");
        }

        _ = version;
        if (document["$kind"] is not JsonString kind || !FileProfile.AllKinds.Contains(kind.Value))
        {
            throw Fail(Codes.Kind, message: "$kind is not a supported kind.", at: document["$kind"] ?? document, path: "$kind", (document["$kind"] as JsonString)?.Value);
        }

        if (document["$container"] is not JsonObject container)
        {
            throw Fail(Codes.MissingMember, message: "The document needs $container.", at: document["$container"] ?? document, path: "$container");
        }

        if (container["profile"] is not JsonString profileName || FileProfile.Find(profileName.Value) is not { } profile)
        {
            throw Fail(Codes.Profile, message: "$container.profile is not a supported profile.", at: container["profile"] ?? container, path: "$container.profile");
        }

        if (!profile.Kinds.Contains(kind.Value))
        {
            throw Fail(Codes.Kind, message: $"Kind {kind.Value} does not belong to profile {profile.Name}.", at: kind, path: "$kind", kind.Value);
        }

        JsonValue root = document["root"] ?? throw Fail(Codes.MissingMember, message: "The document needs root.", at: document, path: "root");
        return (profile, kind.Value, container, root);
    }

    /// <summary>The canonical own bytes. Writers never emit gzip or a byte order mark.</summary>
    public static byte[] Write(JsonObject document) => JsonWriter.ToUtf8(document);

    public static bool IsGzip(ReadOnlySpan<byte> bytes) => bytes.Length >= 2 && bytes[0] == 0x1F && bytes[1] == 0x8B;

    /// <summary>Whether the first non-whitespace character after an optional BOM is '{'.</summary>
    public static bool IsJsonCandidate(ReadOnlySpan<byte> bytes)
    {
        int i = bytes.StartsWith("\uFEFF"u8) ? 3 : 0;
        while (i < bytes.Length && bytes[i] is (byte)' ' or (byte)'\t' or (byte)'\r' or (byte)'\n')
        {
            i++;
        }

        return i < bytes.Length && bytes[i] == (byte)'{';
    }

    /// <summary>
    /// Reads an own document: bounded gunzip when gzip magic is present, strict parsing and the
    /// envelope check. Own formats are not stable yet, so only the current version reads. A
    /// document of another version fails with both numbers, and an older one asks for the
    /// original file to be converted again. Nothing here falls back to the original formats.
    /// </summary>
    public static ReadResult Read(byte[] bytes, string? fileName = null, FormatLimits? limits = null)
    {
        limits ??= FormatLimits.Default;
        bool gzip = IsGzip(bytes);
        if (gzip)
        {
            bytes = Gunzip(bytes, limits, fileName);
        }

        JsonValue value = JsonReader.Parse(bytes, file: fileName, decompressed: gzip, limits);
        return value is not JsonObject document
            ? throw new ConversionException(new FormatDiagnostic(Codes.Format, Message: "The document is not a JSON object.") { File = fileName, Span = value.Span, Decompressed = gzip })
            : document["$format"] is not JsonString { Value: OwnFormat.FormatName }
            ? throw new ConversionException(new FormatDiagnostic(Codes.Format, Message: "$format is not \"OpenAoW\".")
            { File = fileName, Span = (document["$format"] ?? document).Span, Path = "$format", Decompressed = gzip })
            : document["$version"] is not JsonInteger { Value: >= 1 } versionValue
            ? throw new ConversionException(new FormatDiagnostic(Codes.Version, Message: "$version is a positive integer.")
            { File = fileName, Span = (document["$version"] ?? document).Span, Path = "$version", Decompressed = gzip })
            : versionValue.Value != OwnFormat.CurrentVersion
            ? throw new ConversionException(new FormatDiagnostic(Codes.Version, Message: versionValue.Value > OwnFormat.CurrentVersion
                ? $"The document has version {versionValue.Value}, newer than the supported version {OwnFormat.CurrentVersion}."
                : $"The document has version {versionValue.Value}, older than the supported version {OwnFormat.CurrentVersion}. Convert the original file again.")
            { File = fileName, Span = versionValue.Span, Path = "$version", Value = versionValue.Value.ToString(CultureInfo.InvariantCulture), Decompressed = gzip })
            : new ReadResult { Document = document, DeclaredVersion = (int)versionValue.Value, Gzip = gzip };
    }

    private static byte[] Gunzip(byte[] bytes, FormatLimits limits, string? fileName)
    {
        try
        {
            using MemoryStream input = new(bytes, writable: false);
            using GZipStream gz = new(input, mode: CompressionMode.Decompress);
            using MemoryStream output = new();
            byte[] buffer = new byte[81920];
            int read;
            while ((read = gz.Read(buffer, offset: 0, count: buffer.Length)) > 0)
            {
                if (output.Length + read > limits.DecompressedBytes)
                {
                    throw new ConversionException(new FormatDiagnostic(Codes.Limit, Message: $"The decompressed document exceeds {limits.DecompressedBytes} bytes.") { File = fileName, Offset = 0 });
                }

                output.Write(buffer, offset: 0, count: read);
            }
            return output.ToArray();
        }
        catch (InvalidDataException error)
        {
            throw new ConversionException(new FormatDiagnostic(Codes.Compression, Message: $"The gzip document is corrupt: {error.Message}") { File = fileName, Offset = 0 });
        }
    }

    /// <summary>
    /// N(D): parse, validate and write canonical JSON. Strict export validates the document and
    /// importing its canonical binary produces the canonical JSON output.
    /// </summary>
    public static byte[] Normalize(byte[] own, string? fileName = null, FormatLimits? limits = null)
    {
        ReadResult read = Read(own, fileName, limits);
        (FileProfile? profile, string? kind, JsonObject _, JsonValue _) = Envelope(read.Document, fileName);
        ExportResult exported = Export(read.Document, fileName, limits);
        ImportResult imported = Import(exported.Bytes, profile, kind, fileName, limits);
        return Write(imported.Document);
    }

    /// <summary>Content detection (own-formats.md 2.9). It never retries binary after a JSON error.</summary>
    public static Classification Classify(byte[] bytes, string? extension, string? kind = null, string? profileName = null)
    {
        if (IsGzip(bytes) || IsJsonCandidate(bytes))
        {
            try
            {
                ReadResult read = Read(bytes);
                (FileProfile? profile, string? docKind, JsonObject _, JsonValue _) = Envelope(read.Document, fileName: null);
                return new Classification(read.Gzip ? DetectedFormat.GzipOwnJson : DetectedFormat.OwnJson, profile, Kind: docKind, Version: read.DeclaredVersion, Error: null);
            }
            catch (ConversionException error)
            {
                return new Classification(error.Diagnostic.Code switch
                {
                    Codes.Version => DetectedFormat.UnsupportedVersion,
                    Codes.Kind => DetectedFormat.UnsupportedKind,
                    _ => DetectedFormat.MalformedOwn,
                }, Profile: null, Kind: null, Version: null, Error: error.Diagnostic);
            }
        }
        kind ??= KindForExtension(extension);
        if (kind is null)
        {
            return new Classification(DetectedFormat.Unknown, Profile: null, Kind: null, Version: null, Error: null);
        }

        FileProfile? p = profileName is not null ? FileProfile.Find(profileName) : ProfileFor(kind, file: bytes);
        return p is null || !p.Kinds.Contains(kind)
            ? new Classification(DetectedFormat.Unknown, Profile: null, kind, Version: null, Error: null)
            : new Classification(DetectedFormat.Original, Profile: p, kind, Version: null, Error: null);
    }
}
