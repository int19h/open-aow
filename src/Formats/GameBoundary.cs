using System.Globalization;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

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

/// <summary>
/// A prepared load or save. A load holds the engine image of the validated document, which the CRC
/// forms, the header listings and the network transfer read, and the document. A save holds the
/// document bytes. A refusal holds neither.
/// </summary>
public sealed class GameFileResult
{
    public byte[]? Bytes { get; init; }
    public JsonObject? Document { get; init; }
    public required GameFileReport Report { get; init; }

    /// <summary>Whether a load read an own document, which the tree reads through the document backend.</summary>
    public bool Own { get; init; }

    public bool Succeeded => Bytes is not null;
}

/// <summary>
/// The tree of a prepared load (persistence-tree.md 6): the root object and its stored class, by
/// name and by the id of the profile's engine table (0 for a class the table does not name).
/// </summary>
public sealed record GameFileTree(ObjectNode Root, string ClassName, int ClassId);

/// <summary>
/// The conversion side of the game's file boundary (own-formats.md 4.1). Original loads import,
/// validate for game context, export for an unmasked G1 comparison and read the tree from the
/// image through the original backend. Own loads classify, parse, validate and read the tree
/// through the document backend. Saves write the tree through the document backend, validate and
/// serialize. Nothing here creates a game object, touches a file or falls back to the original
/// bytes, and the game's own storage code writes no image on this path.
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
        return new GameFileResult { Bytes = image, Document = read.Document, Report = report, Own = true };
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

    /// <summary>
    /// The validation before a destructive load step and before a save is written
    /// (<see cref="DocumentValidation.Check"/>): the strict image encoding, then the game
    /// validators. The first finding refuses. It returns the engine image of the document.
    /// </summary>
    private static byte[] Validate(JsonObject document, string? fileName, IReadOnlyList<IDocumentValidator> validators, GameFileReport report)
    {
        report.Validation = "refused";
        var check = DocumentValidation.Check(document, validators: validators, file: fileName);
        if (check.Findings.Count != 0) throw new ConversionException(check.Findings[0]);
        report.Validation = "passed";
        return check.Image!;
    }

    /// <summary>
    /// The tree of a prepared load. An own document reads through the document backend, an
    /// original through the original backend over the image its G1 check passed.
    /// </summary>
    public static GameFileTree ReadTree(GameFileResult result, GameProfile use)
    {
        if (result.Bytes is null) throw new InvalidOperationException("A refused load has no tree.");
        if (!result.Own || result.Document is null) return ReadImageTree(result.Bytes, use: use);
        var schema = FormatSchema.Current;
        var profile = use.Profile;
        var root = (JsonObject)result.Document["root"]!;
        SchemaClass classValue;
        if (profile.RootHasClass)
        {
            // The validation resolved the class. The root's $class names the stored class and is
            // no member of it.
            classValue = schema.Get(((JsonString)root["$class"]!).Value);
            var members = new JsonObject();
            foreach (var member in root.Members)
                if (member.Name != "$class") members.Members.Add(member);
            root = members;
        }
        else classValue = schema.Get(profile.RootFamily);
        return new GameFileTree(Root: DocumentBackend.Read(root, classValue: classValue, schema: schema, engine: profile.Engine), ClassName: classValue.Name, ClassId: classValue.Id ?? 0);
    }

    /// <summary>
    /// The tree of an engine image of a profile: a validated image, or an original image the game
    /// holds in memory (a campaign map, a map a network host sent). The original backend reads
    /// the root object.
    /// </summary>
    public static GameFileTree ReadImageTree(byte[] image, GameProfile use)
    {
        var schema = FormatSchema.Current;
        var profile = use.Profile;
        var context = new DecodeContext(image, schema: schema, engine: profile.Engine, profile: profile.Name, file: null, limits: FormatLimits.Default, counts: new LayoutCounts());
        var (classValue, start, end) = profile.RootImage(context);
        return new GameFileTree(Root: ImageTree.Read(context, start: start, end: end, classValue: classValue), ClassName: classValue.Name, ClassId: classValue.Id ?? 0);
    }

    /// <summary>
    /// The engine image of a tree in the profile's game container, before any compression: the
    /// image SaveHSMtoStream compresses into a campaign. The original backend writes the root.
    /// </summary>
    public static byte[] WriteImage(ObjectNode root, string className, GameProfile use)
    {
        var schema = FormatSchema.Current;
        var profile = use.Profile;
        var classValue = profile.RootHasClass ? schema.Get(className) : schema.Get(profile.RootFamily);
        var context = new EncodeContext(schema, engine: profile.Engine, profile: profile.Name, file: null, limits: FormatLimits.Default, options: ExportOptions.Default);
        var rootImage = OriginalBackend.Write(root, classValue: classValue, schema: schema, engine: profile.Engine);
        return profile.WrapRootImage(rootImage, classValue: classValue, context: context);
    }

    /// <summary>
    /// Converts a tree the game wrote to the canonical document bytes (own-formats.md 4.2): the
    /// document backend writes the root under the profile's root class, and the document is
    /// validated and serialized. <paramref name="className"/> is the stored class of the root,
    /// which a profile with a class in its root stores as $class. <paramref name="headerName"/> is
    /// the name a scenario set stores in its header.
    /// </summary>
    public static GameFileResult PrepareSave(ObjectNode root, string className, string? fileName, GameProfile use, IReadOnlyList<IDocumentValidator> validators, string? headerName = null)
    {
        var report = new GameFileReport
        {
            Operation = "write",
            Profile = use,
            Kind = use.WriteKind,
            Format = DetectedFormat.OwnJson.ToString(),
            Version = OwnFormat.CurrentVersion,
            OriginalRoundTrip = "not applicable: own document",
        };
        try
        {
            var schema = FormatSchema.Current;
            var profile = use.Profile;
            var classValue = profile.RootHasClass ? schema.Get(className) : schema.Get(profile.RootFamily);
            var rootDocument = DocumentBackend.Write(root, classValue: classValue, schema: schema, engine: profile.Engine);
            if (profile.RootHasClass) rootDocument.Members.Insert(0, new JsonMember("$class", value: new JsonString(classValue.Name)));
            var document = GameFiles.NewDocument(use.WriteKind, container: profile.SaveContainer(headerName), root: rootDocument);
            Validate(document, fileName: fileName, validators: validators, report: report);
            return new GameFileResult { Bytes = GameFiles.Write(document), Document = document, Report = report };
        }
        catch (ConversionException error)
        {
            report.Error = error.Diagnostic;
            return new GameFileResult { Report = report };
        }
    }
}
