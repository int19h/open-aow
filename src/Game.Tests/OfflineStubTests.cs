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
    // The game windows the bodies reach, saved and restored around each test.
    private static readonly FieldInfo[] s_globals =
    [
        Field(typeof(DialogHosts), nameof(DialogHosts.SetupEvents)),
        Field(typeof(DialogHosts), nameof(DialogHosts.General)),
        Field(typeof(DialogHosts), nameof(DialogHosts.MultiWindow)),
        Field(typeof(DialogHosts), nameof(DialogHosts.ScenarioSetup)),
        Field(typeof(DialogHosts), nameof(DialogHosts.TitleWindow)),
        Field(typeof(DialogHosts), nameof(DialogHosts.TitleScene)),
        Field(typeof(DialogHosts), nameof(DialogHosts.VideoDialog)),
        Field(typeof(AudioHost), nameof(AudioHost.SongPlayer)),
        Field(typeof(DialogHosts), nameof(DialogHosts.HelpWindow)),
    ];

    private static FieldInfo Field(Type type, string name) => MemberNames.Field(type, name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)!;

    private sealed class SavedGlobals : IDisposable
    {
        private readonly object?[] _values = s_globals.Select(field => field.GetValue(null)).ToArray();
        private readonly AoWHSMap _map = AoWHSMap.Current;
        private readonly TextControl _text = AoWHSSet.Current.TextControl;
        private readonly HeroResourceList _heroes = AoWHSSet.Current.HeroResources;

        public void Dispose()
        {
            for (int i = 0; i < s_globals.Length; i++) s_globals[i].SetValue(null, _values[i]);
            AoWHSMap.Current = _map;
            AoWHSSet.Current.TextControl = _text;
            AoWHSSet.Current.HeroResources = _heroes;
        }
    }

    // Creates every component field of a module, as its layout does, without the layout's
    // images, fonts and event bindings.
    private static T Populate<T>(T module) where T : Component
    {
        foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            if (field.FieldType.IsSubclassOf(typeof(Component)) && field.FieldType.GetConstructor([typeof(Component)]) is { } constructor)
                field.SetValue(module, constructor.Invoke([module]));
        }
        return module;
    }

    private static SetupEvents OfflineSetupEvents()
    {
        var events = new SetupEvents(null!) { DPlay = new GDPlay { PlayerName = "No Name" } };
        events.SetupControl = new SetupControl(events);
        DialogHosts.SetupEvents = events;
        return events;
    }

    private static MultiWindow NewGameWindow()
    {
        var window = Populate(new MultiWindow(null!));
        DialogHosts.MultiWindow = window;
        DialogHosts.TitleWindow = Populate(new TitleWindow(null!));
        DialogHosts.VideoDialog = new VideoDialog(null!);
        var songs = new SongPlayer();
        AudioHost.SongPlayer = songs;
        DialogHosts.General = new General(null!);
        return window;
    }

    [Fact]
    public void OfflineComponentNeverConnects()
    {
        var play = new GDPlay();
        Assert.False(play.Connected);
        Assert.False(play.InSession);
        Assert.Equal(0, play.GetConnections());
        Assert.Equal(1, play.Connections.Count);
        Assert.Equal("Internet TCP/IP Connection For DirectPlay", play.Connections[0]);
        var provider = Assert.IsType<GDConnection>(Assert.Single(play.ConnectionItems.List.Take(play.ConnectionItems.Count)));
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
        using var saved = new SavedGlobals();
        var events = OfflineSetupEvents();
        var heroes = new HeroResourceList(null!);
        new HeroResource(heroes);
        AoWHSSet.Current.HeroResources = heroes;
        var window = Populate(new LeaderSetupWindow(null!));
        var control = new LeaderSetupControl(null!) { OriginalLeader = new Leader(null!) };
        bool done = false;
        control.OnDone = sender => done = ReferenceEquals(sender, control);
        control.AddRef();
        window.Setup = control;
        window.Leader = new Leader(null!) { HeroName = "Gorbag" };
        window.Leader.SetHeroResourceIndex(0);
        window.LeaderPnl.Visible = true;

        window.Finish();

        Assert.True(done);
        Assert.Equal(LeaderSetupControl.LeaderSetupStatus.Done, control.Status);
        Assert.Equal("Gorbag", control.OriginalLeader.HeroName);
        Assert.Null(window.Setup);
        Assert.False(window.LeaderPnl.Visible);
        Assert.Equal("No Name", events.DPlay.PlayerName);

        events.SetupControl.Status = Set<SetupStatusElement>.Of(SetupStatusElement.Remote);
        events.UpdatePlayerName("Remote");
        Assert.Equal("No Name", events.DPlay.PlayerName);
    }

    // Row 6: AOWMultiWin.pas Execute shows a provider button only for a provider that DirectPlay
    // lists. The offline component lists the TCP/IP provider alone, so TCP shows and IPX stays
    // hidden.
    [Fact]
    public void NewGameWindowShowsTcpAndHidesIpx()
    {
        using var saved = new SavedGlobals();
        OfflineSetupEvents();
        var window = NewGameWindow();
        window.TCPBtn.Visible = false;
        window.IPXBtn.Visible = true;

        window.Execute("");

        Assert.True(window.MultiPnl.Visible);
        Assert.True(window.GamePanel.Visible);
        Assert.True(window.TCPBtn.Visible);
        Assert.False(window.IPXBtn.Visible);
        Assert.True(DialogHosts.TitleWindow.TitleWin.Visible);
    }

    // AOWMultiWin.pas:200 and 215: Find and Host connect the TCP/IP provider first. The offline
    // connection fails, the port says so, and the TCP panel stays as it was.
    [Fact]
    public void TcpFindAndHostSayNetworkPlayIsNotAvailable()
    {
        using var saved = new SavedGlobals();
        var events = OfflineSetupEvents();
        var window = NewGameWindow();
        var setup = Populate(new ScenarioSetup(null!));
        DialogHosts.ScenarioSetup = setup;
        setup.ScenSetupPnl.Visible = false;
        var messages = new List<string>();
        Runtime.MessageHandler = messages.Add;
        window.Execute("");
        window.TCPBtnClick();
        window.TCPName.Text = "Gorbag";

        window.FindClick();
        window.HostClick();

        Assert.Equal([MultiWindow.NetworkPlayNotAvailableText, MultiWindow.NetworkPlayNotAvailableText], messages);
        Assert.Equal("Network play is not available in this version.", MultiWindow.NetworkPlayNotAvailableText);
        Assert.True(window.MultiPnl.Visible);
        Assert.True(window.TCPPnl.Visible);
        Assert.False(window.GamePanel.Visible);
        Assert.False(window.JoinPnl.Visible);
        Assert.False(setup.ScenSetupPnl.Visible);
        Assert.Equal("No Name", events.DPlay.PlayerName);
    }

    // Row 3: AoWScenSetup.pas:257 closes the setup and returns to the New Game window.
    [Fact]
    public void ScenarioSetupExitReturnsToTheNewGameWindow()
    {
        using var saved = new SavedGlobals();
        OfflineSetupEvents();
        var window = NewGameWindow();
        var setup = Populate(new ScenarioSetup(null!));
        DialogHosts.ScenarioSetup = setup;
        setup.SetupControl = new SetupControl(setup);
        setup.ScenSetupPnl.Visible = true;
        window.MultiPnl.Visible = false;
        window.TCPBtn.Visible = false;

        setup.ExitGameClick();

        Assert.False(setup.ScenSetupPnl.Visible);
        Assert.Null(setup.SetupControl);
        Assert.True(window.MultiPnl.Visible);
        Assert.True(window.TCPBtn.Visible);
    }

    // Row 1: AOWMultiWin.pas:103 hides the New Game window and hands over to the title scene.
    // The title scene needs the host display, which this test has not, so the test stops there.
    [Fact]
    public void BackToTitleHidesTheNewGameWindowAndShowsTheTitleScene()
    {
        using var saved = new SavedGlobals();
        OfflineSetupEvents();
        var window = NewGameWindow();
        DialogHosts.TitleScene = new TitleScene(null!);
        DialogHosts.TitleWindow.TitleWin.Visible = true;
        window.MultiPnl.Visible = true;

        var error = Record.Exception(window.CloseBtnClick);

        Assert.False(DialogHosts.TitleWindow.TitleWin.Visible);
        Assert.False(window.MultiPnl.Visible);
        Assert.IsNotType<NotImplementedException>(error);
        Assert.Contains(nameof(TitleScene.ShowScene), error?.StackTrace ?? "");
    }

    // Row 4: AoWPWindows.pas:107 shows help topics 507 and 500. The third item asks the host to
    // open the web site, which the test host records.
    [Fact]
    public void HelpMenuShowsTheTwoHelpTopics()
    {
        using var saved = new SavedGlobals();
        var text = new TextControl(null!);
        foreach (var (id, header) in new[] { (507, "Topic 507"), (500, "Topic 500") })
        {
            var item = text.New();
            item.Id = id;
            item.Text.Text = header + "\r\nBody " + id;
        }
        AoWHSSet.Current.TextControl = text;
        var help = Populate(new HelpWindow(null!));
        DialogHosts.HelpWindow = help;
        var popups = new PopupWindows(null!);
        var menu = new Menu(null!);

        menu.SelectedItem = 0;
        popups.HelpMenuSelect(menu);
        Assert.True(help.HelpWin.Visible);
        Assert.Equal("Topic 507", help.HeaderMemo.Items[0]);
        Assert.Equal("Body 507", help.HelpMemo.Items[0]);

        menu.SelectedItem = 1;
        popups.HelpMenuSelect(menu);
        Assert.Equal("Topic 500", help.HeaderMemo.Items[0]);
        Assert.Equal("Body 500", help.HelpMemo.Items[0]);

        var shell = new RecordingShell();
        Runtime.Shell = shell;
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
        using var saved = new SavedGlobals();
        var recorder = new RecordingImage();
        var library = new CustomImageLibrary(null!);
        var images = new CustomImageLibrary.ImageItem[128];
        for (int i = 0; i < 128; i++) images[i] = new() { Image = recorder, Reference = -1 };
        SetField(library, "Images", images);
        SetField(library, "_count", 128);
        var level = new MapLevel(null!);
        SetField(level, "_width", 2);
        int[] overlaid = [0, 4, 3];
        MapField[] fields = Enumerable.Range(0, 6).Select(i => (MapField)new AoWMapField(null!)
        { HexX = (sbyte)(i % 2), HexY = (sbyte)(i / 2), OverlayType = (sbyte)(overlaid.Contains(i) ? 4 : 0) }).ToArray();
        SetField(level, "_fields", fields);
        var container = new MapContainer(null!);
        var levels = new EngineObjectList();
        levels.Add(level);
        SetField(container, "_children", levels);
        var map = new AoWHSMap(null!);
        SetField(map, "_mapContainer", container);
        SetField(map, "_viewWidthHex", 2);
        SetField(map, "_viewHeightHex", 3);
        AoWHSMap.Current = map;
        var sequence = new ImageSequence(null!);
        SetField(sequence, "_imageLibrary", library);
        sequence.SequenceMode = SequenceMode.Up;
        var cast = new ImageSequenceList(null!, null!);
        ((DelphiList)cast).Add(sequence);
        var spell = new EarthMastery(null!);
        spell.CastImages?.Dispose();
        spell.CastImages = cast;
        var animation = new FinishedAnimation { FrameCounter = 100 };

        spell.ShowAnimation(animation, 0, 0, default);
        Assert.Equal([(24, 16), (24, 80), (56, 64)], recorder.Calls);
        Assert.False(animation.Finished);

        animation.FrameCounter = 50;
        spell.ShowAnimation(animation, 0, 0, default);
        Assert.True(animation.Finished);
    }

    private static void SetField(object target, string name, object? value)
    {
        (MemberNames.Field(target.GetType(), name) ?? throw new MissingFieldException(name)).SetValue(target, value);
    }

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
