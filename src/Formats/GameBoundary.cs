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
        {
            if (!Profile.Kinds.Contains(kind))
            {
                throw new InvalidOperationException($"Profile {profile} does not hold kind {kind}.");
            }
        }
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
/// A prepared load or save. <see cref="Bytes"/> is an engine image only when <see cref="HasImage"/>
/// is true. A load of a document the original image cannot carry leaves <see cref="Bytes"/> null,
/// keeps the validated document and records <see cref="ImageRefusal"/>. A save holds the document
/// bytes and <see cref="HasImage"/> is false. A refusal holds neither bytes nor a document.
/// </summary>
public sealed class GameFileResult
{
    /// <summary>
    /// The engine image when <see cref="HasImage"/> is true. The document bytes when this is a
    /// save. Null when a load's document has no engine image, and null on a refusal.
    /// </summary>
    public byte[]? Bytes { get; init; }

    public JsonObject? Document { get; init; }
    public required GameFileReport Report { get; init; }

    /// <summary>Whether a load read an own document, which the tree reads through the document backend.</summary>
    public bool Own { get; init; }

    /// <summary>
    /// Whether <see cref="Bytes"/> is an engine image. A save is false. A load of a document the
    /// original image cannot carry is false, and <see cref="Bytes"/> is null.
    /// </summary>
    public bool HasImage { get; init; }

    /// <summary>
    /// The export finding of a load that has no engine image. The load itself succeeded. A
    /// transfer refuses again through the packet profile and does not send these bytes.
    /// </summary>
    public FormatDiagnostic? ImageRefusal { get; init; }

    /// <summary>A refusal has an error and no document. A load or a save that passed has a document.</summary>
    public bool Succeeded => Report.Error is null && Document is not null;
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
        {
            throw new ConversionException(new FormatDiagnostic(Codes.Profile, $"The document has profile {profile.Name}, the game opens it as {use.Profile.Name}.")
            { File = fileName, Path = "$container.profile", Value = profile.Name, Constraint = use.Profile.Name, Span = ((JsonObject)read.Document["$container"]!)["profile"]!.Span });
        }

        if (!use.Kinds.Contains(kind))
        {
            throw new ConversionException(new FormatDiagnostic(Codes.Kind, $"The document has kind {kind}, the game opens {string.Join(", ", use.Kinds)} here.")
            { File = fileName, Path = "$kind", Value = kind, Span = read.Document["$kind"]!.Span });
        }

        Validate(read.Document, fileName: fileName, validators: validators, report: report);
        try
        {
            byte[] image = GameFiles.ExportImage(read.Document, fileName);
            return new GameFileResult { Bytes = image, Document = read.Document, Report = report, Own = true, HasImage = true };
        }
        catch (ConversionException error) when (LacksImage(error))
        {
            // The file bytes stay out of Bytes. Callers that want an image see HasImage false.
            return new GameFileResult { Document = read.Document, Report = report, Own = true, ImageRefusal = error.Diagnostic };
        }
        catch (ConversionException)
        {
            report.Validation = "refused";
            throw;
        }
    }

    private static GameFileResult LoadOriginal(byte[] bytes, string? fileName, GameProfile use, IReadOnlyList<IDocumentValidator> validators, GameFileReport report)
    {
        report.Format = DetectedFormat.Original.ToString();
        string? suggested = GameFiles.KindForExtension(fileName is null ? null : Path.GetExtension(fileName));
        string kind = suggested is not null && use.Kinds.Contains(suggested) ? suggested : use.Kinds[0];
        report.Kind = kind;
        var imported = GameFiles.Import(bytes, use.Profile, kind, fileName);
        Validate(imported.Document, fileName: fileName, validators: validators, report: report);
        byte[] image;
        try
        {
            image = GameFiles.ExportImage(imported.Document, fileName);
        }
        catch (ConversionException)
        {
            report.Validation = "refused";
            throw;
        }
        var container = (JsonObject)imported.Document["$container"]!;
        if (GameFiles.FirstG1Difference(bytes, use.Profile, container, image) is long offset)
        {
            report.OriginalRoundTrip = "different at 0x" + offset.ToString("x", CultureInfo.InvariantCulture);
            throw new ConversionException(new FormatDiagnostic(Codes.OriginalRoundTripDiffers, "The reconstructed original differs from the file.")
            { File = fileName, Offset = offset, Profile = use.Profile.Name, Decompressed = container["compression"] is JsonString { Value: "cfs" } });
        }
        report.OriginalRoundTrip = "equal";
        return new GameFileResult { Bytes = image, Document = imported.Document, Report = report, HasImage = true };
    }

    /// <summary>
    /// The validation before a destructive load step and before a save is written
    /// (<see cref="DocumentValidation.Check"/>): a document read, then the game validators. The
    /// first finding refuses. The read accepts port-only state. Export keeps its own refusal.
    /// </summary>
    private static void Validate(JsonObject document, string? fileName, IReadOnlyList<IDocumentValidator> validators, GameFileReport report)
    {
        report.Validation = "refused";
        var check = DocumentValidation.Check(document, validators: validators, file: fileName);
        if (check.Findings.Count != 0)
        {
            throw new ConversionException(check.Findings[0]);
        }

        report.Validation = "passed";
    }

    /// <summary>
    /// A valid own document whose original image cannot carry a present member. The truncation
    /// refusal is the fixed-array codec. Every other export failure still fails the load. A
    /// campaign map keeps this document and nests it. Strict export still refuses it.
    /// </summary>
    public static bool LacksImage(ConversionException error) =>
        error.Diagnostic.Code == Codes.Unrepresentable
        || (error.Diagnostic.Code == Codes.Range && error.Diagnostic.Message.Contains("does not truncate", StringComparison.Ordinal));

    /// <summary>
    /// The tree of a prepared load. An own document reads through the document backend, an
    /// original through the original backend over the image its G1 check passed.
    /// </summary>
    public static GameFileTree ReadTree(GameFileResult result, GameProfile use)
    {
        if (!result.Succeeded)
        {
            throw new InvalidOperationException("A refused load has no tree.");
        }
        // An own document is read here, including one whose original image cannot carry it.
        // Bytes of that load is null and is not an image.
        if (result.Own && result.Document is not null)
        {
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
                {
                    if (member.Name != "$class")
                    {
                        members.Members.Add(member);
                    }
                }

                root = members;
            }
            else
            {
                classValue = schema.Get(profile.RootFamily);
            }

            return new GameFileTree(Root: DocumentBackend.Read(root, classValue: classValue, schema: schema, engine: profile.Engine), ClassName: classValue.Name, ClassId: classValue.Id ?? 0);
        }
        if (result.HasImage && result.Bytes is not null)
        {
            return ReadImageTree(result.Bytes, use: use);
        }

        throw new InvalidOperationException("A load without an image has no tree.");
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
        return profile.WrapRootImage(context: new EncodeContext(schema, engine: profile.Engine, profile: profile.Name, file: null, limits: FormatLimits.Default, options: ExportOptions.Default), rootImage: OriginalBackend.Write(root, classValue: classValue, schema: schema, engine: profile.Engine), classValue: classValue);
    }

    /// <summary>
    /// The tree of an object image the engine writes without a class id (<c>TEngine.WriteEObject</c>):
    /// the outer table with the root at id 0, then the object of <paramref name="className"/>. The
    /// game set files, the system hero and item libraries and the e-mail game index hold such an
    /// image and stay original files (own-formats.md 1.1). <paramref name="classTable"/> names the
    /// engine table that resolves the class ids of the created objects inside. The original
    /// backend reads the root.
    /// </summary>
    public static ObjectNode ReadWrittenObject(byte[] image, string className, string classTable)
    {
        var schema = FormatSchema.Current;
        var profile = WrittenObject(className, classTable: classTable);
        var context = new DecodeContext(image, schema: schema, engine: classTable, profile: profile.Name, file: null, limits: FormatLimits.Default, counts: new LayoutCounts());
        var (classValue, start, end) = profile.RootImage(context);
        return ImageTree.Read(context, start: start, end: end, classValue: classValue);
    }

    /// <summary>
    /// The object image <c>TEngine.WriteEObject</c> writes for a tree of <paramref name="className"/>:
    /// the original backend writes the root, inside the outer table of id 0.
    /// </summary>
    public static byte[] WriteWrittenObject(ObjectNode root, string className, string classTable)
    {
        var schema = FormatSchema.Current;
        var profile = WrittenObject(className, classTable: classTable);
        var classValue = schema.Get(className);
        return profile.WrapRootImage(context: new EncodeContext(schema, engine: classTable, profile: profile.Name, file: null, limits: FormatLimits.Default, options: ExportOptions.Default), rootImage: OriginalBackend.Write(root, classValue: classValue, schema: schema, engine: classTable), classValue: classValue);
    }

    private static WrittenObjectProfile WrittenObject(string className, string classTable) =>
        new("written-object", kinds: ["written-object"], rootClass: className, engine: classTable);

    /// <summary>
    /// The image <c>TEngine.StoreEObject</c> writes (<c>rwCreateEObject</c> with id 0). A class id
    /// and then the object image sit inside the outer table of id 0. Class id 0 is nil and stores
    /// no object image. The original backend writes the object in the engine table of
    /// <paramref name="className"/>, under <see cref="TransportProfile"/>. A value that profile
    /// cannot carry is refused with its path and the packet is not written (persistence-tree.md 16).
    /// </summary>
    public static byte[] WritePacket(ObjectNode? root, int classId, string? className) =>
        WritePacket(root, classId: classId, className: className, schema: FormatSchema.Current);

    /// <summary>
    /// The packet image of <see cref="WritePacket(ObjectNode?, int, string?)"/> read against
    /// <paramref name="schema"/>. The game passes the disk schema. A test passes a schema with a
    /// port-range member, and the same profile refuses that member when the tree holds it.
    /// </summary>
    internal static byte[] WritePacket(ObjectNode? root, int classId, string? className, FormatSchema schema)
    {
        string classTable = "game";
        var body = new ByteBuffer();
        body.Int32(classId);
        if (classId != 0)
        {
            if (root is null || className is null)
            {
                throw new ArgumentException("A packet with a class id stores its object.");
            }

            var classValue = schema.Get(className);
            classTable = PacketClassTable(classValue);
            body.Bytes(OriginalBackend.Write(root, classValue: classValue, schema: schema, engine: classTable, profile: TransportProfile.Name));
        }
        var context = new EncodeContext(schema, engine: classTable, profile: TransportProfile.Name, file: null, limits: FormatLimits.Default, options: ExportOptions.Default);
        var output = new ByteBuffer(body.Length + 16);
        FileProfile.WriteTop(context, root: body.ToArray(), layout: null, at: new JsonObject(), output: output);
        return output.ToArray();
    }

    /// <summary>
    /// The class id of a packet image. 0 is nil. An empty property table, the image a failed
    /// write leaves, is nil too.
    /// </summary>
    public static int ReadPacketClassId(byte[] image)
    {
        var context = new DecodeContext(schema: FormatSchema.Current, data: image, engine: "game", profile: TransportProfile.Name, file: null, limits: FormatLimits.Default, counts: new LayoutCounts());
        var table = PropertyTable.Read(context, start: 0, end: image.Length, path: DocumentNodePath.Envelope);
        if (table.Entries.Count == 0)
        {
            return 0;
        }

        int start = FileProfile.ReadTop(context, start: 0, end: image.Length, layout: new LayoutBuilder());
        context.Need(start, count: 4, end: image.Length, path: DocumentNodePath.Root, what: "class id");
        return context.Int32(start);
    }

    /// <summary>
    /// The object of a packet whose class id is not 0, decoded as <paramref name="className"/>.
    /// The original backend reads the object in that class's engine table.
    /// </summary>
    public static ObjectNode ReadPacket(byte[] image, string className)
    {
        var schema = FormatSchema.Current;
        var classValue = schema.Get(className);
        var context = new DecodeContext(engine: PacketClassTable(classValue), data: image, schema: schema, profile: TransportProfile.Name, file: null, limits: FormatLimits.Default, counts: new LayoutCounts());
        int start = FileProfile.ReadTop(context, start: 0, end: image.Length, layout: new LayoutBuilder());
        context.Need(start, count: 4, end: image.Length, path: DocumentNodePath.Root, what: "class id");
        if (context.Int32(start) == 0)
        {
            throw new InvalidOperationException("A nil packet has no object.");
        }

        return ImageTree.Read(context, start: start + 4, end: image.Length, classValue: classValue);
    }

    /// <summary>
    /// The engine table of the class ids inside a packet. The stream resolved those ids through
    /// the engine that owned the stream. That engine registered the combat classes, and the
    /// tactical table holds those classes and every class of the strategic table, at the same
    /// ids. A class enlisted in the tactical table uses it. A class enlisted in none uses the
    /// strategic table. Any other class uses the first table it lists.
    /// </summary>
    public static string PacketClassTable(SchemaClass classValue) =>
        classValue.Engines.Contains("tactical") ? "tactical"
        : classValue.Engines.Count == 0 ? "game"
        : classValue.Engines[0];

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
            if (profile.RootHasClass)
            {
                rootDocument.Members.Insert(0, new JsonMember("$class", value: new JsonString(classValue.Name)));
            }

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
