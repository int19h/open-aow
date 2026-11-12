using Aow.Formats;

using Aow.Game.Saves;
using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Tests;

/// <summary>
/// The game's own writes through the live engine: the autosave and a save beside an original. The
/// engine holds a bare map, which the save's validation refuses after it names the target, so the
/// cases check the target the write takes and that nothing touches the original.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class OwnExtensionWriteTests
{
    private readonly List<FileLogRecord> _log = [];

    private static string Save(string name) => Path.Combine(EngineFixture.Root, "Save", name);

    private void Run(Action<Game.GameEngine, AoWHSMap> action)
    {
        Directory.CreateDirectory(Path.Combine(EngineFixture.Root, "Save"));
        Game.GameEngine engine = EngineFixture.Engine;
        var map = new AoWHSMap(null!);
        var previousMap = engine.HexSpriteMap;
        var previousLog = FileBoundary.Log;
        engine.SetHexSpriteMapWithoutCurrentMap(map);
        AoWHSMap.Current = map;
        FileBoundary.Log = _log.Add;
        try
        {
            action(engine, map);
        }
        finally
        {
            FileBoundary.Log = previousLog;
            engine.SetHexSpriteMapWithoutCurrentMap(previousMap);
            AoWHSMap.Current = null!;
            foreach (var file in Directory.GetFiles(Path.Combine(EngineFixture.Root, "Save"))) File.Delete(file);
        }
    }

    [Fact]
    public void TheAutosaveWritesTheOwnExtension() => Run((_, _) =>
    {
        new PlayerControl(null!).AutoSave();
        var write = Assert.Single(_log);
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
            new PlayerControl(null!).AutoSave();
            Assert.Equal(Save("autosave.asg"), Assert.Single(_log).Path, ignoreCase: true);
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
        public string ExecutableName => inner.ExecutableName;
        public string GameRoot => inner.GameRoot;
        public bool WritesOwnExtensions => false;
    }

    [Fact]
    public void ASaveBesideAnOriginalLeavesTheOriginalUntouched() => Run((engine, _) =>
    {
        byte[] original = [1, 2, 3];
        File.WriteAllBytes(Save("Foo.asg"), original);
        Assert.Throws<GameFileErrorException>(() => engine.SaveHsm(FileBoundary.WriteName(Save("Foo.asg")), null!, GameProfile.WorldMap));
        var write = Assert.Single(_log);
        Assert.Equal(Save("Foo.oasg"), write.Path, ignoreCase: true);
        Assert.Equal(original, File.ReadAllBytes(Save("Foo.asg")));
    });
}
