using System.Text;
using Aow.Formats;

using Aow.Game.Saves;
using Aow.Game.UI.Dialogs;
using Aow.Platform;

namespace Aow.Game.Tests;

/// <summary>
/// The names of the game's files (own-formats.md 2.9 and 4.2): writes take the own extension and
/// leave an original of the same name untouched, a derived name opens its own sibling first and
/// falls back to the original name, content decides the format, and listings show both forms.
/// </summary>
// The file service and the file log are process globals, so the case runs in the engine collection.
[Collection("Engine")]
public sealed class OwnExtensionNameTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "open-aow-names-" + Guid.NewGuid().ToString("N"));
    private readonly IFileSystem _previous = Runtime.Files;
    private readonly Action<FileLogRecord>? _previousLog = FileBoundary.Log;
    private readonly bool _previousOwn = Runtime.WritesOwnExtensions;
    private readonly List<FileLogRecord> _log = [];

    // The shipped hosts write own extensions.
    public OwnExtensionNameTests()
    {
        Runtime.WritesOwnExtensions = true;
        foreach (var name in new[] { "Save", "User", "EmailIn" }) Directory.CreateDirectory(Path.Combine(_directory, "root", name));
        Directory.CreateDirectory(Path.Combine(_directory, "user"));
        Install();
        FileBoundary.Log = _log.Add;
        FileBoundary.Previews.Clear();
    }

    public void Dispose()
    {
        Runtime.Files = _previous;
        Runtime.WritesOwnExtensions = _previousOwn;
        FileBoundary.Log = _previousLog;
        FileBoundary.Previews.Clear();
        Directory.Delete(_directory, true);
    }

    // The layered file service caches its listings, so a case installs it again after it writes
    // files behind its back.
    private void Install() => Runtime.Files = new LayeredFileSystem(Path.Combine(_directory, "root"), Path.Combine(_directory, "user"));

    private string Root(string relative) => Path.Combine(_directory, "root", relative);

    private static byte[] HeroList(string name) => Encoding.UTF8.GetBytes(
        "{\"$format\": \"OpenAoW\", \"$version\": 1, \"$kind\": \"hero-list\", \"$container\": {\"profile\": \"hero-list\", \"compression\": \"none\"}, \"root\": {\"LibraryName\": \""
        + name + "\"}}");

    [Fact]
    public void AWriteTakesTheOwnExtension()
    {
        Assert.Equal(Root(@"Save\Foo.oasg"), FileBoundary.WriteName(Root(@"Save\Foo.asg")));
        Assert.Equal(Root(@"Save\Foo.oacg"), FileBoundary.WriteName(Root(@"Save\Foo.ACG")));
        Assert.Equal(Root(@"User\Heroes.oahl"), FileBoundary.WriteName(Root(@"User\Heroes.ahl")));
        Assert.Equal(Root(@"Save\Foo.oasg"), FileBoundary.WriteName(Root(@"Save\Foo.oasg")));
        Assert.Equal(Root(@"Save\notes.txt"), FileBoundary.WriteName(Root(@"Save\notes.txt")));
    }

    // The play-by-e-mail exchange keeps the original names under EmailIn.
    [Fact]
    public void AnEmailGameKeepsItsName()
    {
        Assert.Equal(Root(@"EmailIn\Duel.asg"), FileBoundary.WriteName(Root(@"EmailIn\Duel.asg")));
        Assert.True(FileBoundary.IsEmailExchange(Root(@"emailin\Duel.asg")));
        Assert.False(FileBoundary.IsEmailExchange(Root(@"Save\Duel.asg")));
        // A relative name decides by the directory it is given, not the current one.
        Assert.Equal("Duel.asg", FileBoundary.WriteName("Duel.asg", Root(@"EmailIn\")));
        Assert.Equal("Duel.oasg", FileBoundary.WriteName("Duel.asg", Root(@"Save\")));
    }

    [Fact]
    public void ADerivedNameOpensItsOwnSiblingFirst()
    {
        string original = Root(@"User\List.ahl");
        Assert.Equal(original, FileBoundary.ReadName(original));
        File.WriteAllBytes(original, HeroList("Original"));
        Install();
        Assert.Equal(original, FileBoundary.ReadName(original));
        File.WriteAllBytes(Root(@"User\List.oahl"), HeroList("Own"));
        Install();
        Assert.Equal(Root(@"User\List.oahl"), FileBoundary.ReadName(original));
        Assert.Equal(Root(@"User\notes.txt"), FileBoundary.ReadName(Root(@"User\notes.txt")));
    }

    // A derived own name whose file is gone opens the original sibling (a map name a host sent,
    // a scenario set name a save carries).
    [Fact]
    public void AMissingOwnNameOpensItsOriginalSibling()
    {
        string own = Root(@"User\Other.oahl");
        Assert.Equal(own, FileBoundary.ReadName(own));
        File.WriteAllBytes(Root(@"User\Other.ahl"), HeroList("Original"));
        Install();
        Assert.Equal(Root(@"User\Other.ahl"), FileBoundary.ReadName(own));
        File.WriteAllBytes(own, HeroList("Own"));
        Install();
        Assert.Equal(own, FileBoundary.ReadName(own));
    }

    // A list saved again by the port loads once, from its own document, and both forms list.
    [Fact]
    public void AListingShowsBothFormsAndPrefersTheOwnDocument()
    {
        File.WriteAllBytes(Root(@"User\A.ahl"), HeroList("A"));
        File.WriteAllBytes(Root(@"User\B.ahl"), HeroList("B"));
        File.WriteAllBytes(Root(@"User\B.oahl"), HeroList("B"));
        File.WriteAllBytes(Root(@"User\C.oahl"), HeroList("C"));
        File.WriteAllBytes(Root(@"User\D.ail"), HeroList("D"));
        Install();
        Assert.Equal(["A.ahl", "B.oahl", "C.oahl"], FileBoundary.FindDocuments(Root(@"User\"), DocumentKind.HeroList));
    }

    // The original save, its own document under the original name and the own document beside it.
    [InstalledFact]
    public void SavesLoadByContentAndTheDerivedNamePrefersTheOwnDocument()
    {
        byte[] original = File.ReadAllBytes(InstalledSave.Save);
        var document = GameFiles.Import(original, GameProfile.WorldMap.Profile, "save", "After the Fall.asg").Document;
        byte[] own = GameFiles.Write(document);
        File.WriteAllBytes(Root(@"Save\Original.asg"), original);
        File.WriteAllBytes(Root(@"Save\Json.asg"), own);
        Install();
        byte[]? fromOriginal = FileBoundary.Load(Root(@"Save\Original.asg"), GameProfile.WorldMap);
        byte[]? fromJson = FileBoundary.Load(Root(@"Save\Json.asg"), GameProfile.WorldMap);
        Assert.NotNull(fromOriginal);
        Assert.Equal(fromOriginal, fromJson);
        Assert.Equal([DetectedFormat.Original.ToString(), DetectedFormat.OwnJson.ToString()], _log.Select(r => r.Report.Format));

        File.WriteAllBytes(Root(@"Save\Original.oasg"), own);
        Install();
        string derived = FileBoundary.ReadName(Root(@"Save\Original.asg"));
        Assert.Equal(Root(@"Save\Original.oasg"), derived);
        Assert.Equal(fromOriginal, FileBoundary.Load(derived, GameProfile.WorldMap));
        Assert.Equal(DetectedFormat.OwnJson.ToString(), _log[^1].Report.Format);
        Assert.Equal(original, File.ReadAllBytes(Root(@"Save\Original.asg")));
    }

    // The load and save dialogs list each family under both extensions (AoWFileDialog FindFiles).
    [Theory]
    [InlineData(0, false, ".asg", true)]
    [InlineData(0, false, ".oasg", true)]
    [InlineData(0, false, ".acg", true)]
    [InlineData(0, false, ".oacg", true)]
    [InlineData(1, false, ".oasg", true)]
    [InlineData(1, false, ".oacg", false)]
    [InlineData(1, true, ".oacg", true)]
    [InlineData(1, true, ".acg", true)]
    [InlineData(2, false, ".hsm", true)]
    [InlineData(2, false, ".ohsm", true)]
    [InlineData(2, false, ".oasg", true)]
    [InlineData(2, false, ".csm", false)]
    [InlineData(0, false, ".ohsm", false)]
    [InlineData(0, false, ".oahl", false)]
    public void TheDialogListsBothExtensions(int mode, bool campaign, string extension, bool listed) =>
        Assert.Equal(listed, FileDialogWindow.ListsFile(mode, campaign, extension));
}
