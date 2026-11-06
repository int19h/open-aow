using System.Text;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Platform;

namespace Aow.Units.Tests;

/// <summary>
/// The session preview cache (own-formats.md 4.3): a preview reuses a validated image only for
/// the same layer, profile, schema version and bytes. Every lookup rereads and hashes the file,
/// and a final open always validates the actual bytes.
/// </summary>
// The file service and the file log are process globals, so the case runs in the engine collection.
[Collection("Engine")]
public sealed class PreviewCacheTests : IDisposable
{
    private readonly string directory = Path.Combine(Path.GetTempPath(), "open-aow-preview-" + Guid.NewGuid().ToString("N"));
    private readonly IFileSystem previous = Runtime.Files;
    private readonly Action<FileLogRecord>? previousLog = FileBoundary.Log;
    private readonly List<FileLogRecord> log = [];

    public PreviewCacheTests()
    {
        Directory.CreateDirectory(Path.Combine(directory, "root", "User"));
        Directory.CreateDirectory(Path.Combine(directory, "user"));
        FileBoundary.Log = log.Add;
        FileBoundary.Previews.Clear();
    }

    public void Dispose()
    {
        Runtime.Files = previous;
        FileBoundary.Log = previousLog;
        FileBoundary.Previews.Clear();
        Directory.Delete(directory, true);
    }

    // An own hero list whose name has a fixed length, so an edit keeps the file length.
    private static byte[] HeroList(string name) => Encoding.UTF8.GetBytes(
        "{\"$format\": \"OpenAoW\", \"$version\": 1, \"$kind\": \"hero-list\", \"$container\": {\"profile\": \"hero-list\", \"compression\": \"none\"}, \"root\": {\"LibraryName\": \""
        + name + "\"}}");

    private string Shipped(string name) => Path.Combine(directory, "root", "User", name);

    private string UserCopy(string name) => Path.Combine(directory, "user", "User", name);

    private void Install() => Runtime.Files = new LayeredFileSystem(Path.Combine(directory, "root"), Path.Combine(directory, "user"));

    private static string Name(byte[] image) =>
        ((JsonString)((JsonObject)GameFiles.ImportImage(image, GameProfile.HeroList.Profile, "hero-list").Document["root"]!)["LibraryName"]!).Value;

    [Fact]
    public void SecondPreviewOfTheSameBytesComesFromTheCache()
    {
        File.WriteAllBytes(Shipped("a.ahl"), HeroList("Alpha"));
        Install();
        var first = FileBoundary.Load(Shipped("a.ahl"), GameProfile.HeroList, "preview");
        var second = FileBoundary.Load(Shipped("a.ahl"), GameProfile.HeroList, "preview");
        Assert.Equal(first, second);
        Assert.Equal([false, true], log.Select(r => r.Cached));
        Assert.Equal("root", log[1].Layer);
    }

    [Fact]
    public void FinalOpenValidatesTheActualBytes()
    {
        File.WriteAllBytes(Shipped("a.ahl"), HeroList("Alpha"));
        Install();
        FileBoundary.Load(Shipped("a.ahl"), GameProfile.HeroList, "preview");
        FileBoundary.Load(Shipped("a.ahl"), GameProfile.HeroList);
        Assert.Equal([false, false], log.Select(r => r.Cached));
        Assert.Equal("open", log[1].Report.Operation);
    }

    [Fact]
    public void SameLengthHandEditInvalidatesThePreview()
    {
        File.WriteAllBytes(Shipped("a.ahl"), HeroList("Alpha"));
        Install();
        Assert.Equal("Alpha", Name(FileBoundary.Load(Shipped("a.ahl"), GameProfile.HeroList, "preview")!));
        var time = File.GetLastWriteTimeUtc(Shipped("a.ahl"));
        File.WriteAllBytes(Shipped("a.ahl"), HeroList("Alphb"));
        File.SetLastWriteTimeUtc(Shipped("a.ahl"), time);
        Assert.Equal("Alphb", Name(FileBoundary.Load(Shipped("a.ahl"), GameProfile.HeroList, "preview")!));
        Assert.All(log, r => Assert.False(r.Cached));
    }

    [Fact]
    public void NewShadowingInvalidatesThePreview()
    {
        File.WriteAllBytes(Shipped("a.ahl"), HeroList("Alpha"));
        Install();
        Assert.Equal("Alpha", Name(FileBoundary.Load(Shipped("a.ahl"), GameProfile.HeroList, "preview")!));
        Directory.CreateDirectory(Path.GetDirectoryName(UserCopy("a.ahl"))!);
        File.WriteAllBytes(UserCopy("a.ahl"), HeroList("Alpha"));
        Install();
        Assert.Equal("Alpha", Name(FileBoundary.Load(Shipped("a.ahl"), GameProfile.HeroList, "preview")!));
        Assert.Equal(["root", "user"], log.Select(r => r.Layer));
        Assert.All(log, r => Assert.False(r.Cached));
    }

    [Fact]
    public void CorruptPreviewReportsItsFileAndLeavesOthersUsable()
    {
        File.WriteAllBytes(Shipped("a.ahl"), HeroList("Alpha"));
        File.WriteAllBytes(Shipped("b.ahl"), HeroList("Bravo"));
        Install();
        FileBoundary.Load(Shipped("a.ahl"), GameProfile.HeroList, "preview");
        FileBoundary.Load(Shipped("b.ahl"), GameProfile.HeroList, "preview");
        File.WriteAllBytes(Shipped("b.ahl"), "{\"$format\": \"OpenAoW\""u8.ToArray());
        Assert.Null(FileBoundary.Load(Shipped("b.ahl"), GameProfile.HeroList, "preview"));
        Assert.Equal(Shipped("b.ahl"), FileBoundary.LastFailure!.Path);
        Assert.Equal("Alpha", Name(FileBoundary.Load(Shipped("a.ahl"), GameProfile.HeroList, "preview")!));
        Assert.True(log[^1].Cached);
    }
}
