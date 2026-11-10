using System.Reflection;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells.GlobalEnchantment;
using Aow.Game.Session;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using E2 = Aow.Game.Ui.Dialogs.E2GlobalsUnit;
using Exe = Aow.Game.Ui.Dialogs.AoWExeFormsUnit;

namespace Aow.Units.Tests;

// The transpiler stubs that single-player play reaches (todo-audit.md rows 1 to 6): the ported
// bodies run against the offline DirectPlay component and keep only their network branches.
[Collection("Engine")]
[EngineState]
public sealed class OfflineStubTests
{
    // The game windows the bodies reach, saved and restored around each test.
    private static readonly FieldInfo[] Globals =
    [
        Field(typeof(aowSetupEventsUnit), nameof(aowSetupEventsUnit.SetupEvents)),
        Field(typeof(AoWWinManUnit), nameof(AoWWinManUnit.General)),
        Field(typeof(Exe), nameof(Exe.MultiWindow)),
        Field(typeof(Exe), nameof(Exe.ScenSetup)),
        Field(typeof(E2), nameof(E2.TitleWindow)),
        Field(typeof(E2), nameof(E2.TitleScene)),
        Field(typeof(AoWVideoDlgUnit), nameof(AoWVideoDlgUnit.VideoDialog)),
        Field(typeof(SoundUnit), nameof(SoundUnit.SongPlayer)),
        Field(typeof(aowHelpWinUnit), nameof(aowHelpWinUnit.HelpWindow)),
    ];

    private static FieldInfo Field(Type type, string name) => type.GetField(name, BindingFlags.Public | BindingFlags.Static)!;

    private sealed class SavedGlobals : IDisposable
    {
        private readonly object?[] values = Globals.Select(field => field.GetValue(null)).ToArray();
        private readonly TAoWHSMap map = Aow.Game.Magic.Abilities.AoWEUnit.AoWHSMap;
        private readonly TTextControl text = Aow.Game.Magic.Abilities.AoWEUnit.AoWHSSet.FTextControl;
        private readonly THeroResourceList heroes = Aow.Game.Magic.Abilities.AoWEUnit.AoWHSSet.FHeroResources;

        public void Dispose()
        {
            for (int i = 0; i < Globals.Length; i++) Globals[i].SetValue(null, values[i]);
            Aow.Game.Magic.Abilities.AoWEUnit.AoWHSMap = map;
            Aow.Game.Magic.Abilities.AoWEUnit.AoWHSSet.FTextControl = text;
            Aow.Game.Magic.Abilities.AoWEUnit.AoWHSSet.FHeroResources = heroes;
        }
    }

    // Creates every component field of a module, as its layout does, without the layout's
    // images, fonts and event bindings.
    private static T Populate<T>(T module) where T : TComponent
    {
        foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            if (field.Name == "captureOrdinal") continue;
            if (field.FieldType.IsSubclassOf(typeof(TComponent)) && field.FieldType.GetConstructor([typeof(TComponent)]) is { } constructor)
                field.SetValue(module, constructor.Invoke([module]));
        }
        return module;
    }

    private static TSetupEvents OfflineSetupEvents()
    {
        var events = new TSetupEvents(null!) { DPlay = new TGDPlay { PlayerName = "No Name" } };
        events.SetupControl = new TSetupControl(events);
        aowSetupEventsUnit.SetupEvents = events;
        return events;
    }

    private static TMultiWindow NewGameWindow()
    {
        var window = Populate(new TMultiWindow(null!));
        Exe.MultiWindow = window;
        E2.TitleWindow = Populate(new TTitleWindow(null!));
        AoWVideoDlgUnit.VideoDialog = new TVideoDialog(null!);
        var songs = new TSongPlayer();
        SoundUnit.SongPlayer = songs;
        AoWWinManUnit.General = new TGeneral(null!);
        return window;
    }

    [Fact]
    public void OfflineComponentNeverConnects()
    {
        var play = new TGDPlay();
        Assert.False(play.Connected);
        Assert.False(play.InSession);
        Assert.Equal(0, play.GetConnections());
        Assert.Equal(1, play.Connections.Count);
        Assert.Equal("Internet TCP/IP Connection For DirectPlay", play.Connections[0]);
        var provider = Assert.IsType<TGDConnection>(Assert.Single(play.ConnectionItems.List.Take(play.ConnectionItems.Count)));
        Assert.Equal(TConnectionType.conTCP, provider.FType);
        Assert.Equal(TGDPlay.E_FAIL, play.Connect(0));
        Assert.Equal(TGDPlay.E_FAIL, play.CreateSession());
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
        var heroes = new THeroResourceList(null!);
        new THeroResource(heroes);
        Aow.Game.Magic.Abilities.AoWEUnit.AoWHSSet.FHeroResources = heroes;
        var window = Populate(new TLeaderSetupWin(null!));
        var control = new TLeaderSetupControl(null!) { FOriginalLeader = new TLeader(null!) };
        bool done = false;
        control.FOnDone = sender => done = ReferenceEquals(sender, control);
        control.AddRef();
        window.FSetup = control;
        window.FLeader = new TLeader(null!) { FHeroName = "Gorbag" };
        window.FLeader.SetHeroResourceIndex(0);
        window.LeaderPnl.Visible = true;

        window.Finish();

        Assert.True(done);
        Assert.Equal(TLeaderSetupStatus.lssDone, control.FStatus);
        Assert.Equal("Gorbag", control.FOriginalLeader.FHeroName);
        Assert.Null(window.FSetup);
        Assert.False(window.LeaderPnl.Visible);
        Assert.Equal("No Name", events.DPlay.PlayerName);

        events.SetupControl.FStatus = TSet<TSetupStatusElement>.Of(TSetupStatusElement.ssRemote);
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
        Assert.True(E2.TitleWindow.TitleWin.Visible);
    }

    // AOWMultiWin.pas:200 and 215: Find and Host connect the TCP/IP provider first. The offline
    // connection fails, the port says so, and the TCP panel stays as it was.
    [Fact]
    public void TcpFindAndHostSayNetworkPlayIsNotAvailable()
    {
        using var saved = new SavedGlobals();
        var events = OfflineSetupEvents();
        var window = NewGameWindow();
        var setup = Populate(new TScenSetup(null!));
        Exe.ScenSetup = setup;
        setup.ScenSetupPnl.Visible = false;
        var messages = new List<string>();
        Runtime.MessageHandler = messages.Add;
        window.Execute("");
        window.TCPBtnClick();
        window.TCPName.Text = "Gorbag";

        window.FindClick();
        window.HostClick();

        Assert.Equal([AOWMultiWinUnit.NetworkPlayNotAvailableText, AOWMultiWinUnit.NetworkPlayNotAvailableText], messages);
        Assert.Equal("Network play is not available in this version.", AOWMultiWinUnit.NetworkPlayNotAvailableText);
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
        var setup = Populate(new TScenSetup(null!));
        Exe.ScenSetup = setup;
        setup.FSetupControl = new TSetupControl(setup);
        setup.ScenSetupPnl.Visible = true;
        window.MultiPnl.Visible = false;
        window.TCPBtn.Visible = false;

        setup.ExitGameClick();

        Assert.False(setup.ScenSetupPnl.Visible);
        Assert.Null(setup.FSetupControl);
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
        E2.TitleScene = new TTitleScene(null!);
        E2.TitleWindow.TitleWin.Visible = true;
        window.MultiPnl.Visible = true;

        var error = Record.Exception(window.CloseBtnClick);

        Assert.False(E2.TitleWindow.TitleWin.Visible);
        Assert.False(window.MultiPnl.Visible);
        Assert.IsNotType<NotImplementedException>(error);
        Assert.Contains(nameof(TTitleScene.ShowScene), error?.StackTrace ?? "");
    }

    // Row 4: AoWPWindows.pas:107 shows help topics 507 and 500. The third item asks the host to
    // open the web site, which the test host records.
    [Fact]
    public void HelpMenuShowsTheTwoHelpTopics()
    {
        using var saved = new SavedGlobals();
        var text = new TTextControl(null!);
        foreach (var (id, header) in new[] { (507, "Topic 507"), (500, "Topic 500") })
        {
            var item = text.New();
            item.FID = id;
            item.FText.Text = header + "\r\nBody " + id;
        }
        Aow.Game.Magic.Abilities.AoWEUnit.AoWHSSet.FTextControl = text;
        var help = Populate(new THelpWindow(null!));
        aowHelpWinUnit.HelpWindow = help;
        var popups = new TPopupWindows(null!);
        var menu = new TAOWMenu(null!);

        menu.FSelectedItem = 0;
        popups.HelpMenuSelect(menu);
        Assert.True(help.HelpWin.Visible);
        Assert.Equal("Topic 507", help.HeaderMemo.Items[0]);
        Assert.Equal("Body 507", help.HelpMemo.Items[0]);

        menu.FSelectedItem = 1;
        popups.HelpMenuSelect(menu);
        Assert.Equal("Topic 500", help.HeaderMemo.Items[0]);
        Assert.Equal("Body 500", help.HelpMemo.Items[0]);

        var shell = new RecordingShell();
        Runtime.Shell = shell;
        menu.FSelectedItem = 2;
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
        var library = new TCustomImageLibrary(null!);
        var images = new TImageItem[128];
        for (int i = 0; i < 128; i++) images[i] = new() { Image = recorder, Reference = -1 };
        SetField(library, "FImages", images);
        SetField(library, "FCount", 128);
        var level = new TMapLevel(null!);
        SetField(level, "FWidth", 2);
        int[] overlaid = [0, 4, 3];
        TMapField[] fields = Enumerable.Range(0, 6).Select(i => (TMapField)new TAoWMapField(null!)
        { Xhx = (sbyte)(i % 2), Yhx = (sbyte)(i / 2), OverlayType = (sbyte)(overlaid.Contains(i) ? 4 : 0) }).ToArray();
        SetField(level, "fields", fields);
        var container = new TMapContainer(null!);
        var levels = new TEObjectList();
        levels.Add(level);
        SetField(container, "FChildren", levels);
        var map = new TAoWHSMap(null!);
        SetField(map, "FMapContainer", container);
        SetField(map, "FViewWidthHX", 2);
        SetField(map, "FViewHeightHX", 3);
        Aow.Game.Magic.Abilities.AoWEUnit.AoWHSMap = map;
        var sequence = new TImageSequence(null!);
        SetField(sequence, "FImageLibrary", library);
        sequence.SequenceMode = TSequenceMode.smUp;
        var cast = new TImageSequenceList(null!, null!);
        ((TList)cast).Add(sequence);
        var spell = new TEarthMastery(null!);
        spell.FCastImages?.Dispose();
        spell.FCastImages = cast;
        var animation = new FinishedAnimation { FFrameCounter = 100 };

        spell.ShowAnimation(animation, 0, 0, default);
        Assert.Equal([(24, 16), (24, 80), (56, 64)], recorder.Calls);
        Assert.False(animation.Finished);

        animation.FFrameCounter = 50;
        spell.ShowAnimation(animation, 0, 0, default);
        Assert.True(animation.Finished);
    }

    private static void SetField(object target, string name, object? value)
    {
        for (Type? type = target.GetType(); type is not null; type = type.BaseType)
            if (type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly) is { } field)
            {
                field.SetValue(target, value);
                return;
            }
        throw new MissingFieldException(name);
    }

    private sealed class RecordingImage() : Aow.Game.Draw.Images.TLibraryImage(null!)
    {
        public readonly List<(int, int)> Calls = [];

        public override void Show(int x, int y, in TSurfaceDesc surface) => Calls.Add((x, y));
    }

    private sealed class FinishedAnimation() : TAnimation(null!)
    {
        public bool Finished;

        public override void Dispose() => Finished = true;
    }
}
