using System.Text;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Game.Saves;
using Aow.Platform;

namespace Aow.Game.Tests;

/// <summary>
/// The session preview cache (own-formats.md 4.3): a preview reuses a validated image only for
/// the same layer, profile, schema version and bytes. Every lookup rereads and hashes the file,
/// and a final open always validates the actual bytes.
/// </summary>
// The file service and the file log are process globals, so the case runs in the engine collection.
[Collection("Engine")]
public sealed class PreviewCacheTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"open-aow-preview-{Guid.NewGuid():N}");
    private readonly IFileSystem _previous = RtlHost.Files;
    private readonly Action<FileLogRecord>? _previousLog = FileBoundary.Log;
    private readonly List<FileLogRecord> _log = [];

    public PreviewCacheTests()
    {
        _ = Directory.CreateDirectory(Path.Combine(_directory, "root", "User"));
        _ = Directory.CreateDirectory(Path.Combine(_directory, "user"));
        FileBoundary.Log = _log.Add;
        FileBoundary.Previews.Clear();
    }

    public void Dispose()
    {
        RtlHost.Files = _previous;
        FileBoundary.Log = _previousLog;
        FileBoundary.Previews.Clear();
        Directory.Delete(_directory, recursive: true);
    }

    // An own hero list whose name has a fixed length, so an edit keeps the file length.
    private static byte[] HeroList(string name) => Encoding.UTF8.GetBytes(
        $"{{\"$format\": \"OpenAoW\", \"$version\": {OwnFormat.CurrentVersion}, \"$kind\": \"hero-list\", \"$container\": {{\"profile\": \"hero-list\", \"compression\": \"none\"}}, \"root\": {{\"LibraryName\": \"{name}\"}}}}");

    private string Shipped(string name) => Path.Combine(_directory, "root", "User", name);

    private string UserCopy(string name) => Path.Combine(_directory, "user", "User", name);

    private void Install() => RtlHost.Files = new LayeredFileSystem(Path.Combine(_directory, "root"), userDirectory: Path.Combine(_directory, "user"));

    private static string Name(byte[] image)
        => ((JsonString)((JsonObject)GameFiles.ImportImage(image, GameProfile.HeroList.Profile, kind: "hero-list").Document["root"]!)["LibraryName"]!).Value;

    [Fact]
    public void SecondPreviewOfTheSameBytesComesFromTheCache()
    {
        File.WriteAllBytes(Shipped("a.ahl"), bytes: HeroList("Alpha"));
        Install();
        Assert.Equal(FileBoundary.Load(Shipped("a.ahl"), profile: GameProfile.HeroList, operation: "preview"), actual: FileBoundary.Load(Shipped("a.ahl"), profile: GameProfile.HeroList, operation: "preview"));
        Assert.Equal([false, true], _log.Select(r => r.Cached));
        Assert.Equal("root", _log[1].Layer);
    }

    [Fact]
    public void FinalOpenValidatesTheActualBytes()
    {
        File.WriteAllBytes(Shipped("a.ahl"), bytes: HeroList("Alpha"));
        Install();
        _ = FileBoundary.Load(Shipped("a.ahl"), profile: GameProfile.HeroList, operation: "preview");
        _ = FileBoundary.Load(Shipped("a.ahl"), profile: GameProfile.HeroList);
        Assert.Equal([false, false], _log.Select(r => r.Cached));
        Assert.Equal("open", _log[1].Report.Operation);
    }

    [Fact]
    public void SameLengthHandEditInvalidatesThePreview()
    {
        File.WriteAllBytes(Shipped("a.ahl"), bytes: HeroList("Alpha"));
        Install();
        Assert.Equal("Alpha", Name(FileBoundary.Load(Shipped("a.ahl"), profile: GameProfile.HeroList, operation: "preview")!));
        DateTime time = File.GetLastWriteTimeUtc(Shipped("a.ahl"));
        File.WriteAllBytes(Shipped("a.ahl"), bytes: HeroList("Alphb"));
        File.SetLastWriteTimeUtc(Shipped("a.ahl"), lastWriteTimeUtc: time);
        Assert.Equal("Alphb", Name(FileBoundary.Load(Shipped("a.ahl"), profile: GameProfile.HeroList, operation: "preview")!));
        Assert.All(_log, r => Assert.False(r.Cached));
    }

    [Fact]
    public void NewShadowingInvalidatesThePreview()
    {
        File.WriteAllBytes(Shipped("a.ahl"), bytes: HeroList("Alpha"));
        Install();
        Assert.Equal("Alpha", Name(FileBoundary.Load(Shipped("a.ahl"), profile: GameProfile.HeroList, operation: "preview")!));
        _ = Directory.CreateDirectory(Path.GetDirectoryName(UserCopy("a.ahl"))!);
        File.WriteAllBytes(UserCopy("a.ahl"), bytes: HeroList("Alpha"));
        Install();
        Assert.Equal("Alpha", Name(FileBoundary.Load(Shipped("a.ahl"), profile: GameProfile.HeroList, operation: "preview")!));
        Assert.Equal(["root", "user"], _log.Select(r => r.Layer));
        Assert.All(_log, r => Assert.False(r.Cached));
    }

    // A Save folder of more than 32 files stays cached through a second listing.
    [Fact]
    public void ListingOfFortyFilesComesFromTheCacheTheSecondTime()
    {
        string[] names = [.. Enumerable.Range(0, count: 40).Select(i => $"h{i:D2}.ahl")];
        foreach (string? name in names)
        {
            File.WriteAllBytes(Shipped(name), bytes: HeroList($"N{name[1..3]}xx"));
        }

        Install();
        foreach (string? name in names)
        {
            Assert.NotNull(FileBoundary.Load(Shipped(name), profile: GameProfile.HeroList, operation: "preview"));
        }

        foreach (string? name in names)
        {
            Assert.NotNull(FileBoundary.Load(Shipped(name), profile: GameProfile.HeroList, operation: "preview"));
        }

        Assert.Equal(40, _log.Take(40).Count(r => !r.Cached));
        Assert.Equal(40, _log.Skip(40).Count(r => r.Cached));
        Assert.Equal(40, FileBoundary.Previews.Count);
    }

    [Fact]
    public void CacheIsBoundedByImageBytesAndDropsTheOldestFirst()
    {
        PreviewCache cache = new(100);
        GameFileReport report = new() { Operation = "preview", Profile = GameProfile.HeroList };
        static PreviewKey Key(string name) => new("root", Path: name, Profile: "hero-list", Version: 1, Sha256: name);
        cache.Add(Key("a"), entry: new PreviewEntry(new byte[40], report));
        cache.Add(Key("b"), entry: new PreviewEntry(new byte[40], report));
        cache.Add(Key("c"), entry: new PreviewEntry(new byte[40], report));
        Assert.False(cache.TryGet(Key("a"), entry: out _));
        Assert.True(cache.TryGet(Key("b"), entry: out _));
        Assert.True(cache.TryGet(Key("c"), entry: out _));
        Assert.Equal(80, cache.Bytes);
        cache.Add(Key("d"), entry: new PreviewEntry(new byte[101], report));
        Assert.False(cache.TryGet(Key("d"), entry: out _));
    }

    [Fact]
    public void CorruptPreviewReportsItsFileAndLeavesOthersUsable()
    {
        File.WriteAllBytes(Shipped("a.ahl"), bytes: HeroList("Alpha"));
        File.WriteAllBytes(Shipped("b.ahl"), bytes: HeroList("Bravo"));
        Install();
        _ = FileBoundary.Load(Shipped("a.ahl"), profile: GameProfile.HeroList, operation: "preview");
        _ = FileBoundary.Load(Shipped("b.ahl"), profile: GameProfile.HeroList, operation: "preview");
        File.WriteAllBytes(Shipped("b.ahl"), bytes: "{\"$format\": \"OpenAoW\""u8.ToArray());
        Assert.Null(FileBoundary.Load(Shipped("b.ahl"), profile: GameProfile.HeroList, operation: "preview"));
        Assert.Equal(Shipped("b.ahl"), FileBoundary.LastFailure!.Path);
        Assert.Equal("Alpha", Name(FileBoundary.Load(Shipped("a.ahl"), profile: GameProfile.HeroList, operation: "preview")!));
        Assert.True(_log[^1].Cached);
    }
}
