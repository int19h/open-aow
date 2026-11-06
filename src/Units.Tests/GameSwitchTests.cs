using System.Text;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Platform;
using Aow.Units.AoWE;
using Aow.Units.HSEngine;

namespace Aow.Units.Tests;

/// <summary>Whether the installed game is present at AOW_GAME_ROOT or the default root.</summary>
internal static class InstalledSave
{
    public static string Root => Path.GetFullPath(Environment.GetEnvironmentVariable("AOW_GAME_ROOT") is { Length: > 0 } root ? root : @"D:\Games\GOG\Age of Wonders");

    /// <summary>A shipped original save, the source of the edited documents.</summary>
    public static string Save => Path.Combine(Root, "Save", "After the Fall.asg");

    public static string? SkipReason => File.Exists(Save) ? null : $"The installed game is absent: {Root}. Set AOW_GAME_ROOT to the game installation.";
}

public sealed class InstalledFactAttribute : FactAttribute
{
    public InstalledFactAttribute()
    {
        if (InstalledSave.SkipReason is string reason) Skip = reason;
    }
}

public sealed class InstalledTheoryAttribute : TheoryAttribute
{
    public InstalledTheoryAttribute()
    {
        if (InstalledSave.SkipReason is string reason) Skip = reason;
    }
}

/// <summary>
/// The game's switch to its own documents (own-formats.md 2.7, 4.1 and 4.2): every refusal of
/// the 2.7 table happens before the engine touches the live game, a valid edit reaches the
/// engine image unchanged, and saves replace their files atomically.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class GameSwitchTests
{
    private static JsonObject ShippedSave() => GameFiles.Import(File.ReadAllBytes(InstalledSave.Save), GameProfile.WorldMap.Profile, "save", "After the Fall.asg").Document;

    private static JsonObject Root(JsonObject document) => (JsonObject)document["root"]!;

    private static JsonArray Children(JsonObject value) => (JsonArray)value["Children"]!;

    private static JsonObject Player(JsonObject document, long slot) =>
        Children((JsonObject)Root(document)["Players"]!).Items.Cast<JsonObject>().Single(p => ((JsonInteger)p["$slot"]!).Value == slot);

    private static JsonObject Level(JsonObject document) => (JsonObject)Children((JsonObject)Children(Root(document)).Items[0]).Items[0];

    private static void Replace(JsonObject value, string member, JsonValue replacement) => value.Find(member)!.Value = replacement;

    public static TheoryData<string, string> Refusals => new()
    {
        { "removed-referenced-child", Codes.Reference },
        { "unknown-class", Codes.UnknownClass },
        { "incompatible-class", Codes.ClassFamily },
        { "duplicate-slot", Codes.Slot },
        { "row-length", Codes.Dimensions },
        { "dimensions", Codes.Dimensions },
        { "storage-range", Codes.Range },
        { "unknown-member", Codes.UnknownMember },
        { "mistyped-reserved-member", Codes.ReservedMember },
        { "missing-required-child", Codes.Required },
        { "dangling-turn-order", Codes.Reference },
    };

    private static void Edit(JsonObject document, string edit)
    {
        var root = Root(document);
        var players = Children((JsonObject)root["Players"]!);
        switch (edit)
        {
            case "removed-referenced-child":
                // root.CurrentPlayer names player slot 1. Slots 0 and 2 to 12 keep their numbers.
                players.Items.Remove(Player(document, ((JsonInteger)root["CurrentPlayer"]!).Value));
                break;
            case "unknown-class":
                Replace(Player(document, 2), "$class", new JsonString("TNoSuchPlayer"));
                break;
            case "incompatible-class":
                // A known class of the game table that is not a world map root.
                Replace(root, "$class", new JsonString("TAoWHSSet"));
                break;
            case "duplicate-slot":
                Replace(Player(document, 3), "$slot", new JsonInteger(2));
                break;
            case "row-length":
                ((JsonArray)((JsonArray)Level(document)["Fields"]!).Items[0]).Items.RemoveAt(0);
                break;
            case "dimensions":
                Replace(Level(document), "Height", new JsonInteger(((JsonInteger)Level(document)["Height"]!).Value + 1));
                break;
            case "storage-range":
                Replace(root, "Day", new JsonInteger(1L << 40));
                break;
            case "unknown-member":
                root.Add("Dya", new JsonInteger(3));
                break;
            case "mistyped-reserved-member":
                Player(document, 2).Add("$slott", new JsonInteger(2));
                break;
            case "missing-required-child":
                Children(root).Items.Clear();
                break;
            case "dangling-turn-order":
                ((JsonArray)((JsonObject)root["TurnOrder"]!)["items"]!).Items.Add(new JsonInteger(40));
                break;
            default:
                throw new ArgumentException(edit);
        }
    }

    // own-formats.md 2.7: each refusal names its code and location, and the live map object, its
    // state and the source file stay exactly as they were.
    [InstalledTheory]
    [MemberData(nameof(Refusals))]
    public void RefusedEditLeavesTheLiveGameAndTheFileIntact(string edit, string code)
    {
        var document = ShippedSave();
        Edit(document, edit);
        string directory = Path.Combine(EngineFixture.Root, "Save");
        Directory.CreateDirectory(directory);
        string file = Path.Combine(directory, edit + ".asg");
        byte[] bytes = GameFiles.Write(document);
        File.WriteAllBytes(file, bytes);
        var engine = EngineFixture.Engine;
        var live = EngineFixture.Configure(new TAoWHSMap(null!), value => { value.FDay = 7; value.FMapName = "live"; });
        var previous = engine.FHSMap;
        engine.FHSMap = live;
        try
        {
            engine.LoadHSM(file, false, null!, GameProfile.WorldMap);
            Assert.Same(live, engine.FHSMap);
            Assert.Equal(7, live.FDay);
            Assert.Equal("live", live.FMapName);
            Assert.Equal(bytes, File.ReadAllBytes(file));
            var failure = FileBoundary.LastFailure;
            Assert.NotNull(failure);
            Assert.Equal(file, failure.Path);
            Assert.Equal(code, failure.Report.Error!.Code);
            Assert.True(failure.Report.Error.Span.IsKnown, "an own-document refusal points at a line and column");
            Assert.Equal(DetectedFormat.OwnJson.ToString(), failure.Report.Format);
        }
        finally
        {
            engine.FHSMap = previous;
            File.Delete(file);
        }
    }

    [InstalledFact]
    public void RemovedReferencedChildNamesTheReferenceAndItsDomain()
    {
        var document = ShippedSave();
        Edit(document, "removed-referenced-child");
        var result = GameBoundary.PrepareLoad(GameFiles.Write(document), "edited.asg", GameProfile.WorldMap, FormatMapping.Validators);
        Assert.False(result.Succeeded);
        Assert.Equal("refused", result.Report.Validation);
        Assert.Equal("root.CurrentPlayer", result.Report.Error!.Path);
        Assert.Equal("1", result.Report.Error.Value);
        Assert.Contains("root.Players.Children", result.Report.Error.Message);
    }

    [InstalledFact]
    public void RemovedUnreferencedChildIsAccepted()
    {
        // own-formats.md 2.7: a surviving slot keeps its number. Slot 12 is in TurnOrder, so the
        // turn order loses it too.
        var document = ShippedSave();
        var root = Root(document);
        Children((JsonObject)root["Players"]!).Items.Remove(Player(document, 12));
        ((JsonArray)((JsonObject)root["TurnOrder"]!)["items"]!).Items.RemoveAt(12);
        var result = GameBoundary.PrepareLoad(GameFiles.Write(document), "edited.asg", GameProfile.WorldMap, FormatMapping.Validators);
        Assert.True(result.Succeeded, result.Report.Error?.ToString());
        var image = GameFiles.ImportImage(result.Bytes!, GameProfile.WorldMap.Profile, "save").Document;
        Assert.Equal(Enumerable.Range(0, 12).Select(i => (long)i),
            Children((JsonObject)Root(image)["Players"]!).Items.Cast<JsonObject>().Select(p => ((JsonInteger)p["$slot"]!).Value));
    }

    // own-formats.md 2.10 and 5.2: a valid hand edit reaches the engine image, and the image read
    // back is the edited document with nothing else changed.
    [InstalledFact]
    public void ValidEditKeepsEverythingElseIdentical()
    {
        var document = ShippedSave();
        var root = Root(document);
        long day = ((JsonInteger)root["Day"]!).Value;
        Replace(root, "Day", new JsonInteger(day + 1));
        Replace(root, "DefendersSeed", new JsonInteger(123456789));
        byte[] edited = GameFiles.Write(document);
        var result = GameBoundary.PrepareLoad(edited, "edited.asg", GameProfile.WorldMap, FormatMapping.Validators);
        Assert.True(result.Succeeded, result.Report.Error?.ToString());
        Assert.Equal("not applicable: own document", result.Report.G1);
        var reread = GameFiles.ImportImage(result.Bytes!, GameProfile.WorldMap.Profile, "save").Document;
        Assert.Equal(edited, GameFiles.Write(reread));
        Assert.Equal(day + 1, ((JsonInteger)Root(reread)["Day"]!).Value);
    }

    // own-formats.md 4.1: the CRC forms of in-scope files cover the engine image, so a user copy
    // in the own format gives the CRC of its original.
    private static JsonArray Groups(JsonObject document) => Children((JsonObject)Root(document)["AIGroupManager"]!);

    private static GameFileResult Prepare(JsonObject document) =>
        GameBoundary.PrepareLoad(GameFiles.Write(document), "edited.asg", GameProfile.WorldMap, FormatMapping.Validators);

    // own-formats.md 2.7, allowed class change: the new class's inherited and own fields are
    // validated, and an old field the new class lacks is refused by name, never read again
    // under its numeric id.
    [InstalledFact]
    public void AllowedClassChangeValidatesTheNewClassFields()
    {
        var document = ShippedSave();
        var groups = Groups(document);
        int passive = groups.Items.FindIndex(g => ((JsonObject)g)["$class"] is JsonString { Value: "TPassiveAG" });
        string before = Encoding.UTF8.GetString(JsonWriter.ToUtf8(groups.Items[passive]));
        Replace((JsonObject)groups.Items[passive], "$class", new JsonString("TFortifyAG"));
        var result = Prepare(document);
        Assert.True(result.Succeeded, result.Report.Error?.ToString());
        var reread = Groups(GameFiles.ImportImage(result.Bytes!, GameProfile.WorldMap.Profile, "save").Document);
        Assert.Equal(before.Replace("\"TPassiveAG\"", "\"TFortifyAG\""), Encoding.UTF8.GetString(JsonWriter.ToUtf8(reread.Items[passive])));

        document = ShippedSave();
        var area = Groups(document).Items.Cast<JsonObject>().First(g => g["$class"] is JsonString { Value: "TGuardAreaAG" } && g["GuardLocation"] is not null);
        Replace(area, "$class", new JsonString("TPassiveAG"));
        var refused = Prepare(document);
        Assert.Equal(Codes.UnknownMember, refused.Report.Error!.Code);
        Assert.EndsWith(".GuardLocation", refused.Report.Error.Path);
    }

    // own-formats.md 2.7, removed property: an absent rwData member is absent from the engine
    // image, and the engine's read of an absent rwData id leaves the constructor's value, not 0.
    [InstalledFact]
    public void RemovedRwDataPropertyKeepsTheConstructorDefault()
    {
        var document = ShippedSave();
        var groups = Groups(document);
        int index = groups.Items.FindIndex(g => ((JsonObject)g)["IndependentRelation"] is not null);
        Assert.True(index >= 0, "a stored AI group relation");
        ((JsonObject)groups.Items[index]).Remove("IndependentRelation");
        var result = Prepare(document);
        Assert.True(result.Succeeded, result.Report.Error?.ToString());
        var reread = Groups(GameFiles.ImportImage(result.Bytes!, GameProfile.WorldMap.Profile, "save").Document);
        Assert.Null(((JsonObject)reread.Items[index])["IndependentRelation"]);

        // The id table is weak since C12, so the unlinked group needs no teardown.
        var group = new Aow.Units.MainAG.TPassiveAG(null!);
        Assert.Equal(-1, group.FIndependentRelation);
        sbyte relation = group.FIndependentRelation;
        using var memory = new TMemoryStream();
        memory.Write(new byte[] { 0 });
        memory.Position = 0;
        var reader = new Aow.Units.Engine.TEReadStorageStream(memory, EngineFixture.Engine, false);
        try { reader.rwData(Bytes.Of(ref relation, 1), 1, 26); }
        finally { reader.Destroy(); }
        Assert.Equal(-1, relation);
    }

    [InstalledFact]
    public void OriginalAndItsOwnDocumentGiveOneCrc()
    {
        string directory = Path.Combine(EngineFixture.Root, "Crc");
        Directory.CreateDirectory(directory);
        try
        {
            foreach (var (source, profile) in new[] { (InstalledSave.Save, GameProfile.WorldMap), (Path.Combine(InstalledSave.Root, "Release", "Release.hss"), GameProfile.ScenarioSet) })
            {
                byte[] original = File.ReadAllBytes(source);
                string originalPath = Path.Combine(directory, "original" + Path.GetExtension(source));
                string ownPath = Path.Combine(directory, "own" + Path.GetExtension(source));
                File.WriteAllBytes(originalPath, original);
                var imported = GameFiles.Import(original, profile.Profile, profile.Kinds.First(k => GameFiles.KindForExtension(Path.GetExtension(source)) == k), source);
                File.WriteAllBytes(ownPath, GameFiles.Write(imported.Document));
                Assert.NotEqual(0, FileBoundary.ImageCrc32(originalPath, profile));
                Assert.Equal(FileBoundary.ImageCrc32(originalPath, profile), FileBoundary.ImageCrc32(ownPath, profile));
                if (profile == GameProfile.ScenarioSet)
                {
                    Assert.Equal(BitConverter.ToInt32(original, original.Length - 4), FileBoundary.ImageTailInt32(ownPath, profile));
                    Assert.Equal(BitConverter.ToInt32(original, original.Length - 4), FileBoundary.ImageTailInt32(originalPath, profile));
                }
            }
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [InstalledFact]
    public void OriginalLoadPassesUnmaskedG1()
    {
        byte[] file = File.ReadAllBytes(InstalledSave.Save);
        var result = GameBoundary.PrepareLoad(file, InstalledSave.Save, GameProfile.WorldMap, FormatMapping.Validators);
        Assert.True(result.Succeeded, result.Report.Error?.ToString());
        Assert.Equal("equal", result.Report.G1);
        Assert.Equal("passed", result.Report.Validation);
        Assert.Equal(GameProfile.WorldMap.Profile.Comparable(file, FormatLimits.Default)[5..], result.Bytes);
    }

    [InstalledFact]
    public void ReconstructedOriginalMismatchNamesTheFirstDifferingOffset()
    {
        byte[] file = File.ReadAllBytes(InstalledSave.Save);
        var document = GameFiles.Import(file, GameProfile.WorldMap.Profile, "save").Document;
        byte[] image = GameFiles.ExportImage(document);
        var container = (JsonObject)document["$container"]!;
        Assert.Null(GameFiles.FirstG1Difference(file, GameProfile.WorldMap.Profile, container, image));
        image[1000] ^= 0xFF;
        // B of a CFS file is the five-byte prefix and the inflated payload.
        Assert.Equal(5 + 1000, GameFiles.FirstG1Difference(file, GameProfile.WorldMap.Profile, container, image));
        image[1000] ^= 0xFF;
        Assert.Equal(5 + image.Length - 1, GameFiles.FirstG1Difference(file, GameProfile.WorldMap.Profile, container, image[..^1]));
    }

    [InstalledFact]
    public void OwnDocumentOfAnotherProfileIsRefused()
    {
        byte[] own = GameFiles.Write(ShippedSave());
        var result = GameBoundary.PrepareLoad(own, "map.hsm", GameProfile.TacticalMap, FormatMapping.Validators);
        Assert.Equal(Codes.Profile, result.Report.Error!.Code);
    }

    // One shipped file per caller profile. `aowconv check` runs the unit validators over the
    // whole corpus and reports its Validation line.
    public static TheoryData<string, string> InstalledFiles => new()
    {
        { "Scenario/Campaign/Valley01.CSM", "world-map" },
        { "Save/After the Fall.asg", "world-map" },
        { "TCMaps/0126c000.HSM", "tactical-map" },
        { "Scenario/Campaign/Keepers.CAM", "campaign" },
        { "Release/Release.hss", "scenario-set" },
        { "Release/High Level.ahl", "hero-list" },
    };

    // A shipped file of each profile passes the whole game preflight, the unit validators
    // included: no validator refuses a file the original game loads.
    [InstalledTheory]
    [MemberData(nameof(InstalledFiles))]
    public void ShippedFilePassesThePreflight(string relative, string use)
    {
        var profile = new[] { GameProfile.WorldMap, GameProfile.TacticalMap, GameProfile.Campaign, GameProfile.ScenarioSet, GameProfile.HeroList, GameProfile.ItemList }.Single(p => p.Name == use);
        string file = Path.Combine(InstalledSave.Root, relative.Replace('/', Path.DirectorySeparatorChar));
        var result = GameBoundary.PrepareLoad(File.ReadAllBytes(file), file, profile, FormatMapping.Validators);
        Assert.True(result.Succeeded, result.Report.Error?.ToString());
        Assert.Equal("equal", result.Report.G1);
    }

    [Fact]
    public void MalformedOwnInputNeverFallsBackToBinary()
    {
        var result = GameBoundary.PrepareLoad("{\"$format\": \"OpenAoW\", "u8.ToArray(), "broken.asg", GameProfile.WorldMap, FormatMapping.Validators);
        Assert.False(result.Succeeded);
        Assert.Equal(DetectedFormat.OwnJson.ToString(), result.Report.Format);
        Assert.Equal(Codes.Syntax, result.Report.Error!.Code);
    }

    [Fact]
    public void FileLogRecordNamesEveryField()
    {
        var report = new GameFileReport { Operation = "open", Profile = GameProfile.WorldMap, Kind = "save", Format = "OwnJson", Version = 1, G1 = "not applicable: own document", Validation = "refused", Error = new FormatDiagnostic(Codes.Reference, "x") { File = "a.asg" } };
        string line = new FileLogRecord(@"C:\Save\a.asg", "user", report).ToJson();
        Assert.Equal("{\"operation\":\"open\",\"path\":\"C:\\\\Save\\\\a.asg\",\"layer\":\"user\",\"profile\":\"world-hsm\",\"use\":\"world-map\",\"kind\":\"save\",\"format\":\"OwnJson\",\"version\":1,\"g1\":\"not applicable: own document\",\"validation\":\"refused\",\"error\":\"E_REFERENCE\",\"diagnostic\":\"a.asg E_REFERENCE\\nx\",\"cached\":false}", line);
    }
}

/// <summary>
/// Atomic replacement at the file boundary (own-formats.md 4.2): serialization and validation
/// finish before anything is written, a sibling temporary is written and closed, and only a
/// complete temporary replaces the old file. Every failure keeps the old file.
/// </summary>
// The file service and the file log are process globals, so the case runs in the engine collection.
[Collection("Engine")]
public sealed class AtomicReplacementTests : IDisposable
{
    private readonly string directory = Path.Combine(Path.GetTempPath(), "open-aow-replace-" + Guid.NewGuid().ToString("N"));
    private readonly IFileSystem previous = Runtime.Files;
    private readonly Action<FileLogRecord>? previousLog = FileBoundary.Log;
    private readonly List<FileLogRecord> log = [];

    public AtomicReplacementTests()
    {
        Directory.CreateDirectory(Path.Combine(directory, "root", "User"));
        Directory.CreateDirectory(Path.Combine(directory, "user"));
        FileBoundary.Log = log.Add;
    }

    public void Dispose()
    {
        Runtime.Files = previous;
        FileBoundary.Log = previousLog;
        Directory.Delete(directory, true);
    }

    /// <summary>The engine image of an empty hero list: the stored image of one root property.</summary>
    private static byte[] HeroListImage(string name = "") => GameFiles.ExportImage((JsonObject)JsonReader.Parse(
        "{\"$format\": \"OpenAoW\", \"$version\": 1, \"$kind\": \"hero-list\", \"$container\": {\"profile\": \"hero-list\", \"compression\": \"none\"}, \"root\": {"
        + (name.Length == 0 ? "" : "\"LibraryName\": \"" + name + "\"") + "}}"));

    private string Old => Path.Combine(directory, "user", "User", "list.ahl");
    private string Logical => Path.Combine(directory, "root", "User", "list.ahl");
    private string Temporary => FileBoundary.TemporaryFor(Old);

    private LayeredFileSystem Layers(bool withOld = true)
    {
        if (withOld)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Old)!);
            File.WriteAllBytes(Old, HeroListImage());
        }
        var files = new LayeredFileSystem(Path.Combine(directory, "root"), Path.Combine(directory, "user"));
        Runtime.Files = files;
        return files;
    }

    [Fact]
    public void OriginalUserSaveIsReplacedByItsDocument()
    {
        Layers();
        byte[] original = File.ReadAllBytes(Old);
        FileBoundary.Save(Logical, GameProfile.HeroList, original);
        byte[] written = File.ReadAllBytes(Old);
        Assert.True(GameFiles.IsJsonCandidate(written));
        Assert.False(File.Exists(Temporary));
        var reread = GameBoundary.PrepareLoad(written, Logical, GameProfile.HeroList, FormatMapping.Validators);
        Assert.Equal(original, reread.Bytes);
        var record = Assert.Single(log);
        Assert.True(record.Succeeded);
        Assert.Equal("user", record.Layer);
        Assert.Equal("write", record.Report.Operation);
    }

    [Fact]
    public void SerializationFailureKeepsTheOldFileAndWritesNothing()
    {
        Layers();
        byte[] before = File.ReadAllBytes(Old);
        var error = Assert.Throws<EGameFileError>(() => FileBoundary.Save(Logical, GameProfile.HeroList, [1, 2, 3]));
        Assert.Equal(before, File.ReadAllBytes(Old));
        Assert.False(File.Exists(Temporary));
        Assert.False(error.Record.Succeeded);
    }

    [Fact]
    public void WriteFailureKeepsTheOldFileAndRemovesTheTemporary()
    {
        var layers = Layers();
        byte[] before = File.ReadAllBytes(Old);
        Runtime.Files = new FailingFiles(layers, failWrite: true);
        Assert.Throws<EGameFileError>(() => FileBoundary.Save(Logical, GameProfile.HeroList, HeroListImage("New")));
        Assert.Equal(before, File.ReadAllBytes(Old));
        Assert.False(File.Exists(Temporary));
        Assert.Equal(Codes.Output, Assert.Single(log).Report.Error!.Code);
    }

    [Fact]
    public void ReplaceFailureKeepsTheOldFileAndRemovesTheTemporary()
    {
        var layers = Layers();
        byte[] before = File.ReadAllBytes(Old);
        Runtime.Files = new FailingFiles(layers, failReplace: true);
        Assert.Throws<EGameFileError>(() => FileBoundary.Save(Logical, GameProfile.HeroList, HeroListImage("New")));
        Assert.Equal(before, File.ReadAllBytes(Old));
        Assert.False(File.Exists(Temporary));
    }

    [Fact]
    public void ShippedFileIsShadowedNeverWritten()
    {
        Layers(withOld: false);
        string shipped = Path.Combine(directory, "root", "User", "list.ahl");
        File.WriteAllBytes(shipped, HeroListImage());
        Runtime.Files = new LayeredFileSystem(Path.Combine(directory, "root"), Path.Combine(directory, "user"));
        FileBoundary.Save(Logical, GameProfile.HeroList, HeroListImage("New"));
        Assert.Equal(HeroListImage(), File.ReadAllBytes(shipped));
        Assert.True(GameFiles.IsJsonCandidate(File.ReadAllBytes(Old)));
        Assert.Equal("user", Runtime.Files.Layer(Logical));
    }

    [Fact]
    public void FileServiceWithoutReplacementRefusesTheSave()
    {
        var layers = Layers();
        byte[] before = File.ReadAllBytes(Old);
        Runtime.Files = new NoReplaceFiles(layers);
        Assert.Throws<EGameFileError>(() => FileBoundary.Save(Logical, GameProfile.HeroList, HeroListImage("New")));
        Assert.Equal(before, File.ReadAllBytes(Old));
        Assert.False(File.Exists(Temporary));
    }

    private sealed class FailingFiles(IFileSystem inner, bool failWrite = false, bool failReplace = false) : IFileSystem
    {
        public string CurrentDirectory => inner.CurrentDirectory;
        public string ExpandPath(string path) => inner.ExpandPath(path);
        public Stream Open(string path, int mode)
        {
            var stream = inner.Open(path, mode);
            return failWrite && mode == Runtime.fmCreate ? new BrokenStream(stream) : stream;
        }
        public bool FileExists(string path) => inner.FileExists(path);
        public bool DirectoryExists(string path) => inner.DirectoryExists(path);
        public bool DeleteFile(string path) => inner.DeleteFile(path);
        public bool RenameFile(string source, string target) => inner.RenameFile(source, target);
        public bool ReplaceFile(string source, string target) => !failReplace && inner.ReplaceFile(source, target);
        public string Layer(string path) => inner.Layer(path);
        public bool CreateDirectory(string path) => inner.CreateDirectory(path);
        public DateTime? LastWriteTime(string path) => inner.LastWriteTime(path);
        public IEnumerable<RtlFileEntry> Enumerate(string directory) => inner.Enumerate(directory);
    }

    // A file service written before the replacement primitive: the interface default refuses.
    private sealed class NoReplaceFiles(IFileSystem inner) : IFileSystem
    {
        public string CurrentDirectory => inner.CurrentDirectory;
        public string ExpandPath(string path) => inner.ExpandPath(path);
        public Stream Open(string path, int mode) => inner.Open(path, mode);
        public bool FileExists(string path) => inner.FileExists(path);
        public bool DirectoryExists(string path) => inner.DirectoryExists(path);
        public bool DeleteFile(string path) => inner.DeleteFile(path);
        public bool RenameFile(string source, string target) => inner.RenameFile(source, target);
        public bool CreateDirectory(string path) => inner.CreateDirectory(path);
        public DateTime? LastWriteTime(string path) => inner.LastWriteTime(path);
        public IEnumerable<RtlFileEntry> Enumerate(string directory) => inner.Enumerate(directory);
    }

    // The disk fills after the first byte.
    private sealed class BrokenStream(Stream inner) : Stream
    {
        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => true;
        public override long Length => inner.Length;
        public override long Position { get => inner.Position; set => inner.Position = value; }
        public override void Flush() => inner.Flush();
        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => inner.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count)
        {
            inner.Write(buffer, offset, Math.Min(count, 1));
            throw new IOException("There is not enough space on the disk.");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) inner.Dispose();
            base.Dispose(disposing);
        }
    }
}
