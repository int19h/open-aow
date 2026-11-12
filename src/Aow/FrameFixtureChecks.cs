using System.Runtime.CompilerServices;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Game;
using Aow.Game.Audio;
using Aow.Game.Draw;

using Aow.Game.Host;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Dialogs;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Platform;
using Aow.Rtl;
using Aow.Rtl.Host;

namespace Aow;

internal static class FrameFixtureChecks
{
    public static int Run(string gameRoot)
    {
        using var startup = new StartupScope(gameRoot);
        int checks = 0;
        void Check(bool condition, string label)
        {
            if (!condition) throw new InvalidOperationException(label);
            checks++;
        }
        // The fixture host replays native scripts, so the game writes original names under it.
        Check(!RtlUnit.WritesOwnExtensions && FileBoundary.WriteName("roundtrip.asg") == "roundtrip.asg"
            && FileBoundary.WriteExtension(DocumentKind.Save) == ".asg", "fixture host writes original extensions");
        var instance = new object();
        Check(FrameFixtureInputs.AnimationOffset(instance) == ((int)((uint)RuntimeHelpers.GetHashCode(instance) >> 2) & 65535), "ordinary animation phase");
        Check(!new GDPlay().Connected && new GDPlay().ConnectionType == ConnectionType.None, "offline connection queries");
        Check(ControlWindow.UnitMoveText(32) == "   32", "native unit movement label spacing");
        FrameFixtureInputs.AddressAnimationPhase = 0;
        Check(FrameFixtureInputs.AnimationOffset(instance) == 0, "capture phase");
        FrameFixtureInputs.AddressAnimationPhase = null;
        var clock = new FixtureClock(new(2000, 1, 1, 12, 0, 0));
        clock.WaitMilliseconds(90000);
        Check(clock.Now == new DateTime(2000, 1, 1, 12, 0, 0) && clock.ElapsedMilliseconds == 90000, "wall time stays fixed while pacing advances");
        Check(RtlUnit.DateTimeToStr(RtlUnit.EncodeDate(2000, 1, 1) + RtlUnit.EncodeTime(12, 0, 0, 0)) == "1/1/2000 12:00:00 PM", "native English file date includes seconds and the day period");

        var displayedFileTime = new DateTime(2000, 1, 1, 13, 0, 0);
        Check(FrameFixture.FileStampUtc(displayedFileTime, TimeSpan.FromHours(-7)) ==
            new DateTime(2000, 1, 1, 20, 0, 0, DateTimeKind.Utc), "daylight file stamp preserves captured 1 PM text");
        Check(FrameFixture.FileStampUtc(displayedFileTime, TimeSpan.FromHours(-8)) ==
            new DateTime(2000, 1, 1, 21, 0, 0, DateTimeKind.Utc), "standard file stamp preserves captured 1 PM text");

        var oldWindow = DialogHosts.MapWindow;
        var oldMap = AoWHSMap.Current;
        GameEngine oldEngine = GameTables.GameEngine;
        var oldSettings = RtlUnit.Settings;
        try
        {
            var mapWindow = new Window(null) { _modal = true };
            mapWindow.SetVisibleWithoutUpdate(true);
            DialogHosts.MapWindow = new(null) { MainWindow = mapWindow };
            var menu = new StartMenu(null) { _modal = false };
            menu.SetVisibleWithoutUpdate(true);
            var hidden = new Window(null) { _modal = true };
            hidden.SetVisibleWithoutUpdate(false);
            var manager = new WindowManager(null) { Children = new DelphiList() };
            manager.Children.Add(mapWindow);
            manager.Children.Add(menu);
            manager.Children.Add(hidden);
            var controller = new General(null) { Manager = manager };
            Check(!General.HasModalWindow(controller), "modeless menu and hidden window do not block the map");
            menu._modal = true;
            Check(General.HasModalWindow(controller), "modal sibling menu blocks input");
            controller.HideTransientWindows();
            Check(!menu.Visible && mapWindow.Visible, "hide modal sibling and retain map");

            var players = new PlayerList(null!) { _children = new EngineObjectList() };
            players._children.Add(new Player(null!) { Status = PlayerStatus.Playing });
            players._children.Add(new Player(null!) { Status = PlayerStatus.Terminated });
            players._children.Add(new Player(null!) { Status = PlayerStatus.Playing });
            AoWHSMap.Current = new AoWHSMap(null!);
            AoWHSMap.Current.Players = players;
            TaskBar bar = new TaskBar(null)
            {
                PInd = new(null) { Aligned = true },
                TBMessEdit = new(null) { Aligned = true }
            };
            bar.TBMessEdit.Alignment.LeftOffset = 48;
            bar.UpdatePlayerIndicator();
            Check(bar.PInd.WinWidth == 14 && bar.TBMessEdit.Alignment.RightOffset == 292 &&
                bar.TBMessEdit.Alignment.LeftOffset == 48, "indicator counts active players and reserves the right side");
            Check(!bar.PInd.Aligned && !bar.TBMessEdit.Aligned, "indicator invalidates alignment");
            Check(GameTables.s_magicSphereTexts[(int)MagicSphere.Life] == "Life" &&
                GameTables.s_magicSphereTexts[(int)MagicSphere.Cosmos] == "Cosmos", "sphere names are initialized");
            Check(GameTables.s_playerBehaviorTexts[(int)PlayerBehavior.Normal] == "Normal" &&
                GameTables.s_playerBehaviorTexts[(int)PlayerBehavior.Random] == "Random", "setup behavior names are initialized");

            RtlUnit.Settings = new MemorySettingsStore();
            GameTables.GameEngine = oldEngine;
            GameTables.GameEngine.Registry.SetMovementSpeed(GameSettings.MovementSpeed.Normal);
            FrameFixtureInputs.MeasuredFrameRate = 9;
            var world = new MapWindow(null) { MainWindow = mapWindow };
            world.MapWindowUpdate();
            Check(AoWHSMap.Current.FPS == 9, "captured FPS reaches the strategic movement input");
            var army = new MovementArmy { MoveStep = -1, UnitGraphicsResource = new(null) { MoveAnimationSpeed = 2 } };
            army.MoveFirst(1, false);
            Check(army.MoveSteps == 4, "native capture advances each eight-step move in four frames");
            foreach (int fps in new[] { 1, 6, 8, 9, 14 })
            {
                AoWHSMap.Current.UpdateFPS(fps);
                army.MoveStep = -1;
                army.MoveFirst(1, false);
                Check(army.MoveSteps == 4, "positive FPS below 15 preserves the captured movement tier: " + fps);
            }
            AoWHSMap.Current.UpdateFPS(30);
            army.MoveStep = -1;
            army.MoveFirst(1, false);
            Check(army.MoveSteps == 8, "normal measured FPS retains the eight-step animation");
        }
        finally
        {
            DialogHosts.MapWindow = oldWindow;
            AoWHSMap.Current = oldMap;
            GameTables.GameEngine = oldEngine;
            RtlUnit.Settings = oldSettings;
            FrameFixtureInputs.AddressAnimationPhase = null;
            FrameFixtureInputs.MeasuredFrameRate = null;
        }
        Console.WriteLine($"fixture input checks: {checks} passed, 0 failed");
        return 0;
    }

    private sealed class StartupScope : IDisposable
    {
        private readonly IFileSystem _files = RtlUnit.Files;
        private readonly ISettingsStore? _settings = RtlUnit.Settings;
        private readonly IDialogService? _dialogs = RtlUnit.Dialogs;
        private readonly IClock _clock = RtlUnit.Clock;
        private readonly string _executable = RtlUnit.Application.ExeName;
        private readonly bool _ownExtensions = RtlUnit.WritesOwnExtensions;
        private readonly string _scratch = Path.Combine(Path.GetTempPath(), "aow-frame-inputs-" + Guid.NewGuid().ToString("N"));

        internal StartupScope(string root)
        {
            Directory.CreateDirectory(_scratch);
            RtlUnit.InstallHost(new FrameFixture.FixtureHost(root, _scratch, [], _clock, null));
            Startup.InitializeAll();
            NullSoundSystem.Install();
            GameTables.ValidateGameEngine();
        }

        public void Dispose()
        {
            GameTables.GameEngine.Dispose();
            RtlUnit.Files = _files;
            RtlUnit.Settings = _settings;
            RtlUnit.Dialogs = _dialogs;
            RtlUnit.Clock = _clock;
            RtlUnit.Application.ExeName = _executable;
            RtlUnit.WritesOwnExtensions = _ownExtensions;
            Directory.Delete(_scratch, true);
        }
    }

    private sealed class MovementArmy : ArmyHexSprite
    {
        public MovementArmy() : base(null!) { }

        public override void CalculateShowPriority() { }

        public override sbyte GetHexLevel() => 0;
    }
}
