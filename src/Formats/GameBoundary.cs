using System.Globalization;
using Aow.Formats.Json;
using Aow.Formats.Original;

namespace Aow.Formats;

/// <summary>
/// A caller's profile at the game's file boundary (own-formats.md 1.1 and 4.1). It names the
/// original profile, the document kinds the caller accepts and the kind the caller writes. An
/// extension alone never establishes a profile: every in-scope engine routine passes one.
/// </summary>
public sealed class GameProfile
{
    private GameProfile(string name, string profile, string[] kinds)
    {
        Name = name;
        Profile = FileProfile.Find(profile) ?? throw new InvalidOperationException("Unknown file profile " + profile + ".");
        Kinds = kinds;
        foreach (var kind in kinds)
            if (!Profile.Kinds.Contains(kind)) throw new InvalidOperationException($"Profile {profile} does not hold kind {kind}.");
    }

    /// <summary>The name in file logs.</summary>
    public string Name { get; }

    public FileProfile Profile { get; }

    /// <summary>The accepted kinds. The first one is the kind the caller writes.</summary>
    public IReadOnlyList<string> Kinds { get; }

    public string WriteKind => Kinds[0];

    /// <summary>World maps and saves: THSEngine.LoadHSM, SaveHSM and the previews.</summary>
    public static GameProfile WorldMap { get; } = new("world-map", "world-hsm", ["save", "map", "campaign-map"]);

    /// <summary>Tactical combat maps under TCMaps: THSEngine.LoadHSMEx in combat context.</summary>
    public static GameProfile TacticalMap { get; } = new("tactical-map", "tactical-hsm", ["map"]);

    /// <summary>Campaign saves and definitions: TAoWEngine.SaveCampaign and LoadCampaign.</summary>
    public static GameProfile Campaign { get; } = new("campaign", "campaign", ["campaign-save", "campaign"]);

    /// <summary>Scenario sets: THSEngine.LoadHSS and SaveHSS.</summary>
    public static GameProfile ScenarioSet { get; } = new("scenario-set", "scenario-set", ["scenario-set"]);

    /// <summary>User hero lists: THeroLibrary.LoadFromFile and SaveToFile.</summary>
    public static GameProfile HeroList { get; } = new("hero-list", "hero-list", ["hero-list"]);

    /// <summary>User item lists: TItemLibrary.LoadFromFile and SaveToFile.</summary>
    public static GameProfile ItemList { get; } = new("item-list", "item-list", ["item-list"]);

    public override string ToString() => Name;
}

/// <summary>
/// What one open or write at the boundary found, as the hosts' file logs record it
/// (own-formats.md 4.2). A failed preflight fills the fields it reached.
/// </summary>
public sealed class GameFileReport
{
    public required string Operation { get; init; }
    public required GameProfile Profile { get; init; }
    public string? Kind { get; set; }
    public string? Format { get; set; }
    public int? Version { get; set; }

    /// <summary>"equal", "different at 0x...", or "not applicable: " and the reason.</summary>
    public string OriginalRoundTrip { get; set; } = "not applicable: not reached";

    /// <summary>"passed", "refused" or "not reached".</summary>
    public string Validation { get; set; } = "not reached";

    public FormatDiagnostic? Error { get; set; }
}

/// <summary>A prepared load or save: the engine image or the document bytes, or the refusal.</summary>
public sealed class GameFileResult
{
    public byte[]? Bytes { get; init; }
    public JsonObject? Document { get; init; }
    public required GameFileReport Report { get; init; }
    public bool Succeeded => Bytes is not null;
}

/// <summary>
/// The conversion side of the game's file boundary (own-formats.md 4.1). Original loads import,
/// validate for game context, export for an unmasked G1 comparison and yield the engine image.
/// Own loads classify, parse, migrate, validate and yield the engine image. Saves import the
/// engine-written image, validate, check that the document reproduces the image and serialize.
/// Nothing here creates a game object, touches a file or falls back to the original bytes.
/// </summary>
public static class GameBoundary
{
    public static GameFileResult PrepareLoad(byte[] bytes, string? fileName, GameProfile use, IReadOnlyList<IDocumentValidator> validators, string operation = "open")
    {
        var report = new GameFileReport { Operation = operation, Profile = use };
        try
        {
            return GameFiles.IsGzip(bytes) || GameFiles.IsJsonCandidate(bytes)
                ? LoadOwn(bytes, fileName, use, validators, report)
                : LoadOriginal(bytes, fileName, use, validators, report);
        }
        catch (ConversionException error)
        {
            report.Error = error.Diagnostic;
            return new GameFileResult { Report = report };
        }
    }

    private static GameFileResult LoadOwn(byte[] bytes, string? fileName, GameProfile use, IReadOnlyList<IDocumentValidator> validators, GameFileReport report)
    {
        report.Format = (GameFiles.IsGzip(bytes) ? DetectedFormat.GzipOwnJson : DetectedFormat.OwnJson).ToString();
        report.OriginalRoundTrip = "not applicable: own document";
        var read = GameFiles.Read(bytes, fileName);
        report.Version = read.DeclaredVersion;
        var (profile, kind, _, _) = GameFiles.Envelope(read.Document, fileName);
        report.Kind = kind;
        if (profile != use.Profile)
            throw new ConversionException(new FormatDiagnostic(Codes.Profile, $"The document has profile {profile.Name}, the game opens it as {use.Profile.Name}.")
            { File = fileName, Path = "$container.profile", Value = profile.Name, Constraint = use.Profile.Name, Span = ((JsonObject)read.Document["$container"]!)["profile"]!.Span });
        if (!use.Kinds.Contains(kind))
            throw new ConversionException(new FormatDiagnostic(Codes.Kind, $"The document has kind {kind}, the game opens {string.Join(", ", use.Kinds)} here.")
            { File = fileName, Path = "$kind", Value = kind, Span = read.Document["$kind"]!.Span });
        var image = Validate(read.Document, fileName, validators, report);
        return new GameFileResult { Bytes = image, Document = read.Document, Report = report };
    }

    private static GameFileResult LoadOriginal(byte[] bytes, string? fileName, GameProfile use, IReadOnlyList<IDocumentValidator> validators, GameFileReport report)
    {
        report.Format = DetectedFormat.Original.ToString();
        string? suggested = GameFiles.KindForExtension(fileName is null ? null : Path.GetExtension(fileName));
        string kind = suggested is not null && use.Kinds.Contains(suggested) ? suggested : use.Kinds[0];
        report.Kind = kind;
        var imported = GameFiles.Import(bytes, use.Profile, kind, fileName);
        var image = Validate(imported.Document, fileName, validators, report);
        var container = (JsonObject)imported.Document["$container"]!;
        if (GameFiles.FirstG1Difference(bytes, use.Profile, container, image) is long offset)
        {
            report.OriginalRoundTrip = "different at 0x" + offset.ToString("x", CultureInfo.InvariantCulture);
            throw new ConversionException(new FormatDiagnostic(Codes.OriginalRoundTripDiffers, "The reconstructed original differs from the file.")
            { File = fileName, Offset = offset, Profile = use.Profile.Name, Decompressed = container["compression"] is JsonString { Value: "cfs" } });
        }
        report.OriginalRoundTrip = "equal";
        return new GameFileResult { Bytes = image, Document = imported.Document, Report = report };
    }

    /// <summary>Structural validation through the strict image encoding, then the game validators.</summary>
    private static byte[] Validate(JsonObject document, string? fileName, IReadOnlyList<IDocumentValidator> validators, GameFileReport report)
    {
        report.Validation = "refused";
        var image = GameFiles.ExportImage(document, fileName);
        var (profile, kind, _, _) = GameFiles.Envelope(document, fileName);
        var context = new ValidationContext(document, profile, kind, fileName);
        foreach (var validator in validators)
            foreach (var finding in validator.Validate(context))
                throw new ConversionException(finding);
        report.Validation = "passed";
        return image;
    }

    /// <summary>
    /// Converts an engine-written image to the canonical document bytes. The document is
    /// validated and its own image must equal the engine's, so a reload gives the engine the
    /// image it wrote.
    /// </summary>
    public static GameFileResult PrepareSave(byte[] image, string? fileName, GameProfile use, IReadOnlyList<IDocumentValidator> validators)
    {
        var report = new GameFileReport { Operation = "write", Profile = use, Kind = use.WriteKind, Format = DetectedFormat.OwnJson.ToString(), Version = OwnFormat.CurrentVersion };
        try
        {
            var document = GameFiles.ImportImage(image, use.Profile, use.WriteKind, fileName).Document;
            var written = Validate(document, fileName, validators, report);
            int common = written.AsSpan().CommonPrefixLength(image);
            if (common != written.Length || common != image.Length)
            {
                report.OriginalRoundTrip = "different at 0x" + common.ToString("x", CultureInfo.InvariantCulture);
                throw new ConversionException(new FormatDiagnostic(Codes.OriginalRoundTripDiffers, "The document does not reproduce the engine image.")
                { File = fileName, Offset = common, Profile = use.Profile.Name, Decompressed = true });
            }
            report.OriginalRoundTrip = "equal";
            return new GameFileResult { Bytes = GameFiles.Write(document), Document = document, Report = report };
        }
        catch (ConversionException error)
        {
            report.Error = error.Diagnostic;
            return new GameFileResult { Report = report };
        }
    }
}
