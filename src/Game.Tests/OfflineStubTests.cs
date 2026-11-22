#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using System.Reflection;
using Aow.Engine.Nodes;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// The transpiler stubs that single-player play reaches (todo-audit.md rows 1 to 6): the ported
// bodies run against the offline DirectPlay component and keep only their network branches.
[Collection("Engine")]
[EngineState]
public sealed class OfflineStubTests
{
    // The globals the bodies reach, saved and restored around each test. Each test builds its own
    // UI context with the game windows it needs.
    private static readonly FieldInfo[] s_globals =
    [
        Field(typeof(AudioHost), name: nameof(AudioHost.SongPlayer)),
    ];

    private static FieldInfo Field(Type type, string name) => MemberNames.Field(type, name, flags: BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)!;

    private sealed class SavedGlobals : IDisposable
    {
        private readonly object?[] _values = [.. s_globals.Select(field => field.GetValue(null))];
        private readonly StrategicMap _map = StrategicMap.Current;
        private readonly TextControl _text = GamePack.Current.TextControl;
        private readonly HeroResourceList _heroes = GamePack.Current.HeroResources;

        public void Dispose()
        {
            for (int i = 0; i < s_globals.Length; i++)
            {
                s_globals[i].SetValue(null, value: _values[i]);
            }

            StrategicMap.Current = _map;
            GamePack.Current.TextControl = _text;
            GamePack.Current.HeroResources = _heroes;
        }
    }

    // Creates every layout component of a module, as its layout does, without the layout's
    // images, fonts and event bindings. Issue 254 stores those components in properties. A private
    // setter is the layout's own write. A public setter is a property the test assigns itself.
    private static T Populate<T>(T module) where T : Component
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        foreach (PropertyInfo property in typeof(T).GetProperties(flags))
        {
            if (!property.PropertyType.IsSubclassOf(typeof(Component)))
            {
                continue;
            }

            if (property.GetSetMethod(nonPublic: true) is not { IsPublic: false } setter)
            {
                continue;
            }

            if (property.PropertyType.GetConstructor([typeof(Component)]) is not { } constructor)
            {
                continue;
            }

            _ = setter.Invoke(module, parameters: [constructor.Invoke([module])]);
        }
        return module;
    }

    private static SetupEvents OfflineSetupEvents(UIContext ui)
    {
        SetupEvents events = ui.Add(new SetupEvents(ui) { DirectPlay = new GDPlay { PlayerName = "No Name" } });
        events.SetupControl = new SetupControl(events);
        return events;
    }

    private static MultiWindow NewGameWindow(UIContext ui)
    {
        MultiWindow window = ui.Add(Populate(new MultiWindow(ui)));
        _ = ui.Add(Populate(new TitleWindow(ui)));
        _ = ui.Add(new VideoDialog(ui));
        SongPlayer songs = new();
        AudioHost.SongPlayer = songs;
        _ = ui.Add(new General(ui));
        return window;
    }

    [Fact]
    public void OfflineComponentNeverConnects()
    {
        GDPlay play = new();
        Assert.False(play.Connected);
        Assert.False(play.InSession);
        Assert.Equal(0, play.GetConnections());
        Assert.Equal(1, play.Connections.Count);
        Assert.Equal("Internet TCP/IP Connection For DirectPlay", play.Connections[0]);
        GDConnection provider = Assert.IsType<GDConnection>(Assert.Single(play.ConnectionItems.List.Take(play.ConnectionItems.Count)));
        Assert.Equal(ConnectionType.TCP, provider.FType);
        Assert.Equal(GDPlay.E_FAIL, play.Connect(0));
        Assert.Equal(GDPlay.E_FAIL, play.CreateSession());
        play.CloseSession();
        play.DisConnect();
        play.PlayerName = "Leader";
        Assert.Equal("Leader", play.PlayerName);
        Assert.False(play.Connected);
    }

    // Row 2: aowSetupEvents.pas:84 renames the session player only for a remote setup with a
    // connection. The offline call is a no-op and lets TLeaderSetupWin.Finish commit the leader.
    [Fact]
    public void LeaderSetupFinishCommitsTheLeaderOffline()
    {
        using SavedGlobals saved = new();
        UIContext ui = new(EngineFixture.Engine);
        SetupEvents events = OfflineSetupEvents(ui);
        HeroResourceList heroes = new(null) { Holder = EngineFixture.Engine };
        _ = new HeroResource(heroes);
        GamePack.Current.HeroResources = heroes;
        LeaderSetupWindow window = Populate(new LeaderSetupWindow(ui));
        LeaderSetupControl control = new(null) { Holder = EngineFixture.Engine, OriginalLeader = new Leader(null) { Holder = EngineFixture.Engine } };
        bool done = false;
        control.SetDoneHandler(sender => done = ReferenceEquals(sender, control));
        _ = control.AddRef();
        window.Setup = control;
        window.Leader = new Leader(null) { Holder = EngineFixture.Engine, HeroName = "Gorbag" };
        window.Leader.SetHeroResourceIndex(0);
        window.LeaderPanel.Visible = true;

        window.Finish();

        Assert.True(done);
        Assert.Equal(LeaderSetupControl.LeaderSetupStatus.Done, control.Status);
        Assert.Equal("Gorbag", control.OriginalLeader.HeroName);
        Assert.Null(window.Setup);
        Assert.False(window.LeaderPanel.Visible);
        Assert.Equal("No Name", events.DirectPlay.PlayerName);

        events.SetupControl.Status = SetupStatuses.Remote;
        events.UpdatePlayerName("Remote");
        Assert.Equal("No Name", events.DirectPlay.PlayerName);
    }

    // Row 6: AOWMultiWin.pas Execute shows a provider button only for a provider that DirectPlay
    // lists. The offline component lists the TCP/IP provider alone, so TCP shows and IPX stays
    // hidden.
    [Fact]
    public void NewGameWindowShowsTcpAndHidesIpx()
    {
        using SavedGlobals saved = new();
        UIContext ui = new(EngineFixture.Engine);
        _ = OfflineSetupEvents(ui);
        MultiWindow window = NewGameWindow(ui);
        window.TcpButton.Visible = false;
        window.IpxButton.Visible = true;

        window.Execute("");

        Assert.True(window.MultiPanel.Visible);
        Assert.True(window.GamePanel.Visible);
        Assert.True(window.TcpButton.Visible);
        Assert.False(window.IpxButton.Visible);
        Assert.True(ui.TitleWindow.MainWindow.Visible);
    }

    // AOWMultiWin.pas:200 and 215: Find and Host connect the TCP/IP provider first. The offline
    // connection fails, the port says so, and the TCP panel stays as it was.
    [Fact]
    public void TcpFindAndHostSayNetworkPlayIsNotAvailable()
    {
        using SavedGlobals saved = new();
        UIContext ui = new(EngineFixture.Engine);
        SetupEvents events = OfflineSetupEvents(ui);
        MultiWindow window = NewGameWindow(ui);
        ScenarioSetup setup = ui.Add(Populate(new ScenarioSetup(ui)));
        setup.ScenarioSetupPanel.Visible = false;
        List<string> messages = [];
        LegacyApplication.MessageHandler = messages.Add;
        window.Execute("");
        window.TCPBtnClick();
        window.TcpNameEdit.Text = "Gorbag";

        window.FindClick();
        window.HostClick();

        Assert.Equal([MultiWindow.NetworkPlayNotAvailableText, MultiWindow.NetworkPlayNotAvailableText], messages);
        Assert.Equal("Network play is not available in this version.", MultiWindow.NetworkPlayNotAvailableText);
        Assert.True(window.MultiPanel.Visible);
        Assert.True(window.TcpPanel.Visible);
        Assert.False(window.GamePanel.Visible);
        Assert.False(window.JoinPanel.Visible);
        Assert.False(setup.ScenarioSetupPanel.Visible);
        Assert.Equal("No Name", events.DirectPlay.PlayerName);
    }

    // Row 3: AoWScenSetup.pas:257 closes the setup and returns to the New Game window.
    [Fact]
    public void ScenarioSetupExitReturnsToTheNewGameWindow()
    {
        using SavedGlobals saved = new();
        UIContext ui = new(EngineFixture.Engine);
        _ = OfflineSetupEvents(ui);
        MultiWindow window = NewGameWindow(ui);
        ScenarioSetup setup = ui.Add(Populate(new ScenarioSetup(ui)));
        setup.SetupControl = new SetupControl(setup);
        setup.ScenarioSetupPanel.Visible = true;
        window.MultiPanel.Visible = false;
        window.TcpButton.Visible = false;

        setup.ExitGameClick();

        Assert.False(setup.ScenarioSetupPanel.Visible);
        Assert.Null(setup.SetupControl);
        Assert.True(window.MultiPanel.Visible);
        Assert.True(window.TcpButton.Visible);
    }

    // Row 1: AOWMultiWin.pas:103 hides the New Game window and hands over to the title scene.
    // The title scene needs the host display, which this test has not, so the test stops there.
    [Fact]
    public void BackToTitleHidesTheNewGameWindowAndShowsTheTitleScene()
    {
        using SavedGlobals saved = new();
        UIContext ui = new(EngineFixture.Engine);
        _ = OfflineSetupEvents(ui);
        MultiWindow window = NewGameWindow(ui);
        _ = ui.Add(new TitleScene(ui));
        ui.TitleWindow.MainWindow.Visible = true;
        window.MultiPanel.Visible = true;

        Exception error = Record.Exception(window.BackButtonClick);

        Assert.False(ui.TitleWindow.MainWindow.Visible);
        Assert.False(window.MultiPanel.Visible);
        Assert.IsNotType<NotImplementedException>(error);
        Assert.Contains(nameof(TitleScene.ShowScene), error?.StackTrace ?? "");
    }

    // Row 4: AoWPWindows.pas:107 shows help topics 507 and 500. The third item asks the host to
    // open the web site, which the test host records.
    [Fact]
    public void HelpMenuShowsTheTwoHelpTopics()
    {
        using SavedGlobals saved = new();
        UIContext ui = new(EngineFixture.Engine);
        TextControl text = new(null) { Holder = EngineFixture.Engine };
        foreach ((int id, string? header) in new[] { (507, "Topic 507"), (500, "Topic 500") })
        {
            TextControlItem item = text.New();
            item.Id = id;
            item.Text.Text = $"{header}\r\nBody {id}";
        }
        GamePack.Current.TextControl = text;
        HelpWindow help = ui.Add(Populate(new HelpWindow(ui)));
        PopupWindows popups = new(ui);
        Menu menu = new(null)
        {
            SelectedItem = 0
        };
        popups.HelpMenuSelect(menu);
        Assert.True(help.MainWindow.Visible);
        Assert.Equal("Topic 507", help.HeaderMemo.Items[0]);
        Assert.Equal("Body 507", help.HelpMemo.Items[0]);

        menu.SelectedItem = 1;
        popups.HelpMenuSelect(menu);
        Assert.Equal("Topic 500", help.HeaderMemo.Items[0]);
        Assert.Equal("Body 500", help.HelpMemo.Items[0]);

        RecordingShell shell = new();
        RtlHost.Shell = shell;
        menu.SelectedItem = 2;
        popups.HelpMenuSelect(menu);
        Assert.Equal(["http://www.ageofwonders.com"], shell.Urls);
        text?.Dispose();
    }

    // Row 5: GlobalEnchantmentSpells.pas:1890 walks each view column one row at a time and
    // draws the cast images on fields with overlay 4, down to the last map row.
    [Fact]
    public void EarthMasteryWalksEveryViewRow()
    {
        using SavedGlobals saved = new();
        RecordingImage recorder = new();
        CustomImageLibrary library = new(null);
        CustomImageLibrary.ImageItem[] images = new CustomImageLibrary.ImageItem[128];
        for (int i = 0; i < 128; i++)
        {
            images[i] = new() { Image = recorder, Reference = -1 };
        }

        SetField(library, "Images", images);
        SetField(library, "_count", 128);
        MapLevel level = new(null) { Holder = EngineFixture.Engine };
        SetField(level, "Width", 2);
        int[] overlaid = [0, 4, 3];
        MapField[] fields = [.. Enumerable.Range(0, count: 6).Select(i => new MapField(null)
        { Holder = EngineFixture.Engine, HexX = i % 2, HexY = (i / 2).TruncateToSByte(), OverlayType = (sbyte)(overlaid.Contains(i) ? 4 : 0) })];
        SetField(level, "_fields", fields);
        MapContainer container = new(null) { Holder = EngineFixture.Engine };
        EngineObjectList levels = [];
        _ = levels.Add(level);
        SetField(container, "_children", levels);
        StrategicMap map = new(null, holder: EngineFixture.Engine);
        SetField(map, "MapContainer", container);
        SetField(map, "ViewWidthHex", 2);
        SetField(map, "ViewHeightHex", 3);
        StrategicMap.Current = map;
        ImageSequence sequence = new(null);
        SetField(sequence, "ImageLibrary", library);
        sequence.SequenceMode = SequenceMode.Up;
        ImageSequenceList cast = new(null, imageLibrary: null);
        _ = cast.Add(sequence);
        EarthMastery spell = new(null, holder: EngineFixture.Engine);
        spell.CastImages?.Dispose();
        spell.CastImages = cast;
        FinishedAnimation animation = new() { FrameCounter = 100 };

        spell.ShowAnimation(animation, 0, 0, lockedSurface: default);
        Assert.Equal([(24, 16), (24, 80), (56, 64)], recorder.Calls);
        Assert.False(animation.Finished);

        animation.FrameCounter = 50;
        spell.ShowAnimation(animation, 0, 0, lockedSurface: default);
        Assert.True(animation.Finished);
    }

    private static void SetField(object target, string name, object? value) => (MemberNames.Field(target.GetType(), name) ?? throw new MissingFieldException(name)).SetValue(target, value);

    private sealed class RecordingImage() : LibraryImage(null!)
    {
        public List<(int, int)> Calls { get; } = [];

        public override void Show(int x, int y, in LockedSurface surface) => Calls.Add((x, y));
    }

    private sealed class FinishedAnimation() : Animation(null!)
    {
        public bool Finished { get; private set; }

        public override void Dispose() => Finished = true;
    }
}
