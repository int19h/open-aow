using System.Collections.Concurrent;
using Aow.Engine;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Tree;
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Microsoft.Extensions.Logging;

namespace Aow.Game.Tests;

/// <summary>
/// Issue 384. A player-facing load names each object it goes on without, the load itself goes on
/// as the original does, and the report shows the list once in the message dialog and writes it to
/// the log. The test document is a campaign save with one transfer unit. The unit stores the index
/// of its resource in the set, and a set without that resource makes the unit read fail
/// (Unit.PAS:2150).
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class DroppedObjectReportTests
{
    [Fact]
    public void ACampaignWhoseTransferUnitCannotBeReadListsIt()
    {
        using TransferSetup setup = new(name: "dropped");
        setup.Engine.GamePack.UnitResources = setup.Previous;
        List<DroppedObject> dropped = [];
        setup.Engine.LoadCampaign(setup.Saved, dropped: dropped);

        WorldMapCampaign? loaded = setup.Engine.Campaign;
        Assert.NotNull(loaded);
        Assert.Equal(0, loaded.TransferSettings.Units.Count);
        DroppedObject unit = Assert.Single(dropped);
        Assert.Equal(nameof(Unit), unit.ClassName);
        Assert.StartsWith("root.TransferSettings.", unit.Path, comparisonType: StringComparison.Ordinal);
        Assert.EndsWith("[0]", unit.Path, comparisonType: StringComparison.Ordinal);
        Assert.Equal("unit resource missing (73)", unit.Message);
        _ = Assert.IsType<KeyNotFoundException>(unit.Error);
    }

    [Fact]
    public void ACleanCampaignLoadListsNothing()
    {
        using TransferSetup setup = new(name: "clean");
        List<DroppedObject> dropped = [];
        setup.Engine.LoadCampaign(setup.Saved, dropped: dropped);

        WorldMapCampaign? loaded = setup.Engine.Campaign;
        Assert.NotNull(loaded);
        Assert.Equal(1, loaded.TransferSettings.Units.Count);
        Assert.Empty(dropped);
    }

    // The campaign and its map load as the file dialog loads them: one report over both, one
    // message, one summary in the log. The map holds a dungeon whose class the fixture engine
    // does not register, so the map load goes on without it.
    [Fact]
    public void ACampaignAndItsMapGiveOneReport()
    {
        GameEngine engine = EngineFixture.Engine;
        Assert.False(engine.ClassTable.ContainsKey(EngineClass.Of<Dungeon>().ClassId));
        GameMap? previous = engine.HexSpriteMap;
        engine.SetHexSpriteMapWithoutCurrentMap(null);
        using DialogHost host = new();
        using CollectingLoggerFactory logs = new();
        try
        {
            using TransferSetup setup = new(name: "with-map", map: GameFiles.Write(MapDocument()));
            setup.Engine.GamePack.UnitResources = setup.Previous;
            int loadCount = 0;
            DroppedObjectReport report;
            using (RtlHost.IsolateLoggerFactory(logs))
            {
                report = DroppedObjectReport.Load(host.UI, fileName: setup.Saved, load: dropped =>
                {
                    loadCount++;
                    engine.LoadCampaign(setup.Saved, dropped: dropped);
                    engine.LoadCampaignMap(null, dropped: dropped);
                });
            }

            Assert.Equal(1, loadCount);
            Assert.NotNull(engine.HexSpriteMap);
            Assert.Equal([nameof(Unit), nameof(Dungeon)], report.Dropped.Select(static item => item.ClassName));
            Assert.True(report.IsReported);
            Assert.True(host.Dialog.Window.Visible);
            Assert.Equal("The game loaded with 2 objects that could not be read:", host.Dialog.MessageMemo.Items[0]);
            Assert.Equal(1, logs.Entries.Count(static entry => entry.EventId.Id == 35_514));
            Assert.Equal(2, logs.Entries.Count(static entry => entry.EventId.Id == 35_515));
        }
        finally
        {
            if (engine.HexSpriteMap is StrategicMap loaded && loaded != previous)
            {
                EngineFixture.SetMap(loaded);
            }

            if (engine.HexSpriteMap != previous)
            {
                engine.HexSpriteMap?.Dispose();
            }

            engine.SetHexSpriteMapWithoutCurrentMap(previous);
            EngineFixture.SetMap(previous as StrategicMap ?? null);
        }
    }

    [Fact]
    public void TheMessageListsTheDroppedObjectsAndTheRestCounted()
    {
        DroppedObjectReport one = new("one.oacg");
        one.Dropped.Add(Dropped(index: 0));
        string[] oneLines = [.. one.Lines(EngineFixture.Engine)];
        Assert.Equal(
        [
            "The game loaded with 1 object that could not be read:",
            "Unit at root.TransferSettings.Units[0]: unit resource missing (0)",
            "The full list is in openaow.log.",
        ], oneLines);

        DroppedObjectReport many = new("many.oacg");
        for (int i = 0; i < 10; i++)
        {
            many.Dropped.Add(Dropped(index: i));
        }

        many.Dropped[0] = many.Dropped[0] with { Path = "root", ClassName = nameof(WorldMapCampaign), Message = $"{new string('x', count: 120)}\nsecond line" };
        many.Dropped[1] = many.Dropped[1] with { Path = $"root.{new string('p', count: 200)}", Message = "short" };
        string[] lines = [.. many.Lines(EngineFixture.Engine)];
        Assert.Equal(11, lines.Length);
        Assert.Equal("The game loaded with 10 objects that could not be read:", lines[0]);
        // The finished line is cut, whatever part of it is long.
        Assert.Equal($"WorldMapCampaign at root: {new string('x', count: 94)}...", lines[1]);
        Assert.Equal($"Unit at root.{new string('p', count: 107)}...", lines[2]);
        Assert.Equal("Unit at root.TransferSettings.Units[7]: unit resource missing (7)", lines[8]);
        Assert.Equal("... and 2 more.", lines[9]);
        Assert.Equal("The full list is in openaow.log.", lines[10]);
    }

    [Fact]
    public void TheReportShowsTheMessageOnceAndLogsEveryObject()
    {
        using DialogHost host = new();
        MessageDialog dialog = host.Dialog;
        using CollectingLoggerFactory logs = new();
        DroppedObjectReport report = new("dropped.oacg");
        report.Dropped.Add(Dropped(index: 3));
        report.Dropped.Add(Dropped(index: 4));
        using (RtlHost.IsolateLoggerFactory(logs))
        {
            DroppedObjectReport clean = new("clean.oacg");
            Assert.False(clean.Report(host.UI));
            Assert.False(dialog.Window.Visible);
            Assert.Empty(logs.Entries);

            Assert.True(report.Report(host.UI));
            Assert.True(report.IsReported);
            Assert.True(dialog.Window.Visible);
            Assert.True(dialog.OKPanel.Visible);
            Assert.False(dialog.YesNoPanel.Visible);
            string[] expected = [.. report.Lines(EngineFixture.Engine)];
            string[] shown = [.. dialog.MessageMemo.Items];
            Assert.Equal(expected, shown);

            dialog.OKBtnClick();
            Assert.False(dialog.Window.Visible);
            Assert.False(report.Report(host.UI));
            Assert.False(dialog.Window.Visible);
        }

        LogEntry[] entries = logs.Entries;
        Assert.Equal(
        [
            "Load of dropped.oacg went on without 2 objects that could not be read",
            "Load of dropped.oacg could not read Unit at root.TransferSettings.Units[3]: unit resource missing (3)",
            "Load of dropped.oacg could not read Unit at root.TransferSettings.Units[4]: unit resource missing (4)",
        ], entries.Select(static entry => entry.Text));
        Assert.All(entries, static entry => Assert.Equal(LogLevel.Warning, entry.Level));
        Assert.Equal([35_514, 35_515, 35_515], entries.Select(static entry => entry.EventId.Id));
        Assert.Null(entries[0].Error);
        Assert.Same(report.Dropped[0].Error, entries[1].Error);
        Assert.Same(report.Dropped[1].Error, entries[2].Error);
    }

    // A context that has not loaded the message dialog logs the list and that the player was not
    // shown it. The report keeps its one showing for a context that has the dialog, and does not
    // log the list again.
    [Fact]
    public void AContextWithoutTheDialogLogsAndKeepsTheShowing()
    {
        DroppedObjectReport report = new("early.oacg");
        report.Dropped.Add(Dropped(index: 5));
        using CollectingLoggerFactory logs = new();
        using DialogHost host = new();
        using UIContext bare = new(EngineFixture.Engine);
        using (RtlHost.IsolateLoggerFactory(logs))
        {
            Assert.False(report.Report(bare));
            Assert.False(report.IsReported);
            Assert.True(report.Report(host.UI));
            Assert.True(report.IsReported);
            Assert.True(host.Dialog.Window.Visible);
        }

        LogEntry[] entries = logs.Entries;
        Assert.Equal([35_514, 35_515, 35_509], entries.Select(static entry => entry.EventId.Id));
        Assert.Contains("message dialog is not loaded", entries[2].Text, comparisonType: StringComparison.Ordinal);
    }

    private static DroppedObject Dropped(int index)
    {
        string message = $"unit resource missing ({index})";
        return new DroppedObject(nameof(Unit), Path: $"root.TransferSettings.Units[{index}]", Message: message, Error: new KeyNotFoundException(message));
    }

    // A one-field map with a dungeon on the field, in the shape PrisonersSaveListingTests loads.
    private static JsonObject MapDocument()
    {
        JsonObject dungeon = new JsonObject()
            .Add("$class", new JsonString("Dungeon"))
            .Add("ResourceId", new JsonInteger(424_242));
        JsonObject level = new JsonObject()
            .Add("$class", new JsonString("MapLevel"))
            .Add("$slot", new JsonInteger(0))
            .Add("Width", new JsonInteger(1))
            .Add("Height", new JsonInteger(1))
            .Add("Fields", new JsonArray().Add(new JsonArray().Add(new JsonObject().Add("HexSpriteList", new JsonArray().Add(dungeon)))));
        JsonObject container = new JsonObject()
            .Add("$class", new JsonString("MapContainer"))
            .Add("$slot", new JsonInteger(6))
            .Add("Width", new JsonInteger(48))
            .Add("Height", new JsonInteger(1))
            .Add("LevelCount", new JsonInteger(1))
            .Add("Children", new JsonArray().Add(level));
        JsonObject players = new JsonObject().Add("Children", new JsonArray()
            .Add(new JsonObject().Add("$class", new JsonString("Player")).Add("$slot", new JsonInteger(0)).Add("PlayerIndex", new JsonInteger(0)))
            .Add(new JsonObject().Add("$class", new JsonString("Player")).Add("$slot", new JsonInteger(1)).Add("PlayerIndex", new JsonInteger(1))));
        JsonObject root = new JsonObject()
            .Add("$class", new JsonString("StrategicMap"))
            .Add("Day", new JsonInteger(4))
            .Add("Players", players)
            .Add("Children", new JsonArray().Add(container));
        return new JsonObject()
            .Add("$format", new JsonString(OwnFormat.FormatName))
            .Add("$version", new JsonInteger(OwnFormat.CurrentVersion))
            .Add("$kind", new JsonString("save"))
            .Add("$container", new JsonObject().Add("profile", new JsonString("world-hsm")).Add("compression", new JsonString("cfs")).Add("level", new JsonInteger(2)))
            .Add("root", root);
    }

    // The message dialog loaded from its layout with the modules it reads, as the game loads it.
    // Image libraries resolve under the engine fixture root, and a missing library shows no message.
    private sealed class DialogHost : IDisposable
    {
        public DialogHost()
        {
            string previousImagePath = ImageLibraries.BaseImageLibraryPath;
            Action<string> previousMessage = LegacyApplication.MessageHandler;
            LegacyApplication.MessageHandler = static _ => { };
            ImageLibraries.BaseImageLibraryPath = EngineFixture.Root + Path.DirectorySeparatorChar;
            try
            {
                LayoutSession session = new();
                _ = UI.Load(session, module: new UIGraphics());
                _ = UI.Load(session, module: new UIFonts());
                _ = UI.Load(session, module: new General(UI));
                Dialog = UI.Load(session, module: new MessageDialog(UI));
            }
            catch
            {
                Dispose();
                throw;
            }
            finally
            {
                ImageLibraries.BaseImageLibraryPath = previousImagePath;
                LegacyApplication.MessageHandler = previousMessage;
            }
        }

        public UIContext UI { get; } = new(EngineFixture.Engine);

        public MessageDialog Dialog { get; }

        public void Dispose()
        {
            UI.MessageDialog?.Dispose();
            UI.General?.Dispose();
            UI.Dispose();
        }
    }

    private readonly record struct LogEntry(EventId EventId, LogLevel Level, string Text, Exception? Error);

    private sealed class CollectingLoggerFactory : ILoggerFactory
    {
        private readonly ConcurrentQueue<LogEntry> _entries = new();

        public LogEntry[] Entries => [.. _entries];

        public ILogger CreateLogger(string categoryName) => new Collector(_entries);

        public void AddProvider(ILoggerProvider provider)
        {
        }

        public void Dispose()
        {
        }

        private sealed class Collector(ConcurrentQueue<LogEntry> entries) : ILogger
        {
            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
                => entries.Enqueue(new LogEntry(eventId, Level: logLevel, Text: formatter(state, exception), Error: exception));
        }
    }

    // A campaign save with one transfer unit whose resource sits at index 73 of a set the case
    // installs, and optionally a nested map document. The case can put the fixture's empty set
    // back before the load.
    private sealed class TransferSetup : IDisposable
    {
        private readonly IFileSystem _previousFiles;
        private readonly UnitResourceList _resources;
        private readonly bool _isRegistered;
        private bool _isDisposed;

        public TransferSetup(string name, byte[]? map = null)
        {
            Engine = EngineFixture.Engine;
            Previous = Engine.GamePack.UnitResources;
            _previousFiles = RtlHost.Files;
            CampaignClass = EngineClass.Of<WorldMapCampaign>();
            Assert.Null(Previous.GetUnitResource(73));
            _resources = new UnitResourceList(null) { Holder = Engine };
            UnitResource resource = new(null) { Holder = Engine };
            resource.SetArrayOwner(_resources, index: 73);
            Engine.GamePack.UnitResources = _resources;
            _isRegistered = !Engine.ClassTable.ContainsKey(CampaignClass.ClassId);
            if (_isRegistered)
            {
                Engine.RegisterEngineClasses([CampaignClass]);
            }

            Saved = "";
            try
            {
                Engine.NewCampaign(CampaignClass.ClassId);
                WorldMapCampaign? campaign = Engine.Campaign;
                Assert.NotNull(campaign);
                Unit unit = new(null) { Holder = Engine, UnitResource = resource };
                unit.SetOwner(campaign.TransferSettings.Units);
                if (map is not null)
                {
                    campaign.Stream.Write(map);
                }

                // The fixture host does not replace a file. The save uses the physical service.
                PhysicalFileSystem physical = new();
                _ = physical.SetCurrentDirectory(EngineFixture.Root);
                RtlHost.Files = physical;
                string directory = Path.Combine(EngineFixture.Root, "Save");
                _ = Directory.CreateDirectory(directory);
                Saved = Path.Combine(directory, $"dropped-{name}{FileBoundary.WriteExtension(DocumentKind.CampaignSave)}");
                Engine.StoreInFile(Saved, objectValue: campaign, progress: null, profile: GameProfile.Campaign);
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        public GameEngine Engine { get; }

        public UnitResourceList Previous { get; }

        public EngineClass CampaignClass { get; }

        public string Saved { get; private set; }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            Engine.CloseCampaign();
            if (!ReferenceEquals(Engine.GamePack.UnitResources, Previous))
            {
                Engine.GamePack.UnitResources = Previous;
            }

            _resources.Dispose();
            if (_isRegistered)
            {
                Engine.UnregisterEngineClasses([CampaignClass]);
            }

            RtlHost.Files = _previousFiles;
            if (Saved.Length != 0 && File.Exists(Saved))
            {
                File.Delete(Saved);
            }
        }
    }
}
