using Aow.Formats;
using Aow.Platform;
using Aow.Units.AoWE;
using Aow.Units.AoWFileDialog;
using Game = Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Tests;

/// <summary>
/// The names of the game's files (own-formats.md 2.9 and 4.2): writes take the own extension and
/// leave an original of the same name untouched, a derived name opens its own sibling first and
/// falls back to the original name, content decides the format, and listings show both forms.
/// </summary>
// The file service and the file log are process globals, so the case runs in the engine collection.
[Collection("Engine")]
public sealed class OwnExtensionNameTests : IDisposable
{
    private readonly string directory = Path.Combine(Path.GetTempPath(), "open-aow-names-" + Guid.NewGuid().ToString("N"));
    private readonly IFileSystem previous = Runtime.Files;
    private readonly Action<FileLogRecord>? previousLog = FileBoundary.Log;
    private readonly bool previousOwn = Runtime.WritesOwnExtensions;
    private readonly List<FileLogRecord> log = [];

    // The shipped hosts write own extensions.
    public OwnExtensionNameTests()
    {
        Runtime.WritesOwnExtensions = true;
        foreach (var name in new[] { "Save", "User", "EmailIn" }) Directory.CreateDirectory(Path.Combine(directory, "root", name));
        Directory.CreateDirectory(Path.Combine(directory, "user"));
        Install();
        FileBoundary.Log = log.Add;
        FileBoundary.Previews.Clear();
    }

    public void Dispose()
    {
        Runtime.Files = previous;
        Runtime.WritesOwnExtensions = previousOwn;
        FileBoundary.Log = previousLog;
        FileBoundary.Previews.Clear();
        Directory.Delete(directory, true);
    }

    // The layered file service caches its listings, so a case installs it again after it writes
    // files behind its back.
    private void Install() => Runtime.Files = new LayeredFileSystem(Path.Combine(directory, "root"), Path.Combine(directory, "user"));

    private string Root(string relative) => Path.Combine(directory, "root", relative);

    private static byte[] HeroList(string name) => System.Text.Encoding.UTF8.GetBytes(
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
        Assert.Equal([DetectedFormat.Original.ToString(), DetectedFormat.OwnJson.ToString()], log.Select(r => r.Report.Format));

        File.WriteAllBytes(Root(@"Save\Original.oasg"), own);
        Install();
        string derived = FileBoundary.ReadName(Root(@"Save\Original.asg"));
        Assert.Equal(Root(@"Save\Original.oasg"), derived);
        Assert.Equal(fromOriginal, FileBoundary.Load(derived, GameProfile.WorldMap));
        Assert.Equal(DetectedFormat.OwnJson.ToString(), log[^1].Report.Format);
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
        Assert.Equal(listed, TFileDlgWindow.ListsFile(mode, campaign, extension));
}

/// <summary>
/// The game's own writes through the live engine: the autosave and a save beside an original. The
/// engine holds a bare map, which the save's validation refuses after it names the target, so the
/// cases check the target the write takes and that nothing touches the original.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class OwnExtensionWriteTests
{
    private readonly List<FileLogRecord> log = [];

    private static string Save(string name) => Path.Combine(EngineFixture.Root, "Save", name);

    private void Run(Action<TAoWEngine, TAoWHSMap> action)
    {
        Directory.CreateDirectory(Path.Combine(EngineFixture.Root, "Save"));
        var engine = EngineFixture.Engine;
        var map = new TAoWHSMap(null!);
        var previousMap = engine.FHSMap;
        var previousLog = FileBoundary.Log;
        engine.FHSMap = map;
        Game.AoWHSMap = map;
        FileBoundary.Log = log.Add;
        try
        {
            action(engine, map);
        }
        finally
        {
            FileBoundary.Log = previousLog;
            engine.FHSMap = previousMap;
            Game.AoWHSMap = null!;
            foreach (var file in Directory.GetFiles(Path.Combine(EngineFixture.Root, "Save"))) File.Delete(file);
        }
    }

    [Fact]
    public void TheAutosaveWritesTheOwnExtension() => Run((_, _) =>
    {
        new TPlayerControl(null!).AutoSave();
        var write = Assert.Single(log);
        Assert.Equal("write", write.Report.Operation);
        Assert.Equal(Save("autosave.oasg"), write.Path, ignoreCase: true);
        Assert.False(File.Exists(Save("autosave.asg")));
    });

    // A host that replays the original game's scripts (the frame fixtures) writes original names.
    [Fact]
    public void AHostThatWritesOriginalExtensionsKeepsTheOriginalNames() => Run((_, _) =>
    {
        Runtime.InstallHost(new OriginalNamesHost(EngineFixture.Services));
        try
        {
            Assert.False(Runtime.WritesOwnExtensions);
            new TPlayerControl(null!).AutoSave();
            Assert.Equal(Save("autosave.asg"), Assert.Single(log).Path, ignoreCase: true);
            Assert.Equal(Save("Foo.asg"), FileBoundary.WriteName(Save("Foo.asg")));
            Assert.Equal(".acg", FileBoundary.WriteExtension(DocumentKind.CampaignSave));
        }
        finally
        {
            EngineFixture.InstallServices();
        }
        Assert.True(Runtime.WritesOwnExtensions);
    });

    private sealed class OriginalNamesHost(IRtlHost inner) : IRtlHost
    {
        public IFileSystem Files => inner.Files;
        public IDialogService Dialogs => inner.Dialogs;
        public IClock Clock => inner.Clock;
        public ISettingsStore Settings => inner.Settings;
        public string ExeName => inner.ExeName;
        public string GameRoot => inner.GameRoot;
        public bool WritesOwnExtensions => false;
    }

    [Fact]
    public void ASaveBesideAnOriginalLeavesTheOriginalUntouched() => Run((engine, _) =>
    {
        byte[] original = [1, 2, 3];
        File.WriteAllBytes(Save("Foo.asg"), original);
        Assert.Throws<EGameFileError>(() => engine.SaveHSM(FileBoundary.WriteName(Save("Foo.asg")), null!, GameProfile.WorldMap));
        var write = Assert.Single(log);
        Assert.Equal(Save("Foo.oasg"), write.Path, ignoreCase: true);
        Assert.Equal(original, File.ReadAllBytes(Save("Foo.asg")));
    });
}
