using System.Runtime.CompilerServices;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Magic;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
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
        Check(!RtlUnit.WritesOwnExtensions && Aow.Game.Saves.FileBoundary.WriteName("roundtrip.asg") == "roundtrip.asg"
            && Aow.Game.Saves.FileBoundary.WriteExtension(Aow.Formats.DocumentKind.Save) == ".asg", "fixture host writes original extensions");
        var instance = new object();
        Check(FrameFixtureInputs.AnimationOffset(instance) == ((int)((uint)RuntimeHelpers.GetHashCode(instance) >> 2) & 65535), "ordinary animation phase");
        Check(!new TGDPlay().Connected && new TGDPlay().ConnectionType == TConnectionType.conNone, "offline connection queries");
        Check(Aow.Game.Ui.Dialogs.TControlWin.UnitMoveText(32) == "   32", "native unit movement label spacing");
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

        var oldWindow = Aow.Game.Ui.Dialogs.DialogHosts.MWindow;
        var oldMap = Aow.Game.World.Map.TAoWHSMap.AoWHSMap;
        var oldEngine = Aow.Game.Magic.Abilities.GameTables.AoWEngine;
        var oldSettings = RtlUnit.Settings;
        try
        {
            var mapWindow = new TAOWWindow(null) { FVisible = true, FModal = true };
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow = new(null) { MapWindow = mapWindow };
            var menu = new TStartMenu(null) { FVisible = true, FModal = false };
            var hidden = new TAOWWindow(null) { FVisible = false, FModal = true };
            var manager = new TAOWWinManager(null) { FChildren = new TList() };
            manager.FChildren.Add(mapWindow);
            manager.FChildren.Add(menu);
            manager.FChildren.Add(hidden);
            var controller = new TGeneral(null) { Manager = manager };
            Check(!Aow.Game.Ui.Dialogs.TGeneral.HasModalWindow(controller), "modeless menu and hidden window do not block the map");
            menu.FModal = true;
            Check(Aow.Game.Ui.Dialogs.TGeneral.HasModalWindow(controller), "modal sibling menu blocks input");
            controller.HideTransientWindows();
            Check(!menu.FVisible && mapWindow.FVisible, "hide modal sibling and retain map");

            var players = new TPlayerList(null!) { FChildren = new Aow.Game.Engine.Nodes.TEObjectList() };
            players.FChildren.Add(new TPlayer(null!) { FStatus = TPlayerStatus.psPlaying });
            players.FChildren.Add(new TPlayer(null!) { FStatus = TPlayerStatus.psTerminated });
            players.FChildren.Add(new TPlayer(null!) { FStatus = TPlayerStatus.psPlaying });
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap = new TAoWHSMap(null!);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers = players;
            var bar = new Aow.Game.Ui.Dialogs.TTBWindow(null)
            {
                PInd = new(null) { FAligned = true },
                TBMessEdit = new(null) { FAligned = true }
            };
            bar.TBMessEdit.Alignment.LeftOffset = 48;
            bar.UpdatePlayerIndicator();
            Check(bar.PInd.WinWidth == 14 && bar.TBMessEdit.Alignment.RightOffset == 292 &&
                bar.TBMessEdit.Alignment.LeftOffset == 48, "indicator counts active players and reserves the right side");
            Check(!bar.PInd.FAligned && !bar.TBMessEdit.FAligned, "indicator invalidates alignment");
            Check(Aow.Game.Magic.Abilities.GameTables.TMagicSphereRStr[(int)TMagicSphere.msLife] == "Life" &&
                Aow.Game.Magic.Abilities.GameTables.TMagicSphereRStr[(int)TMagicSphere.msCosmos] == "Cosmos", "sphere names are initialized");
            Check(Aow.Game.Magic.Abilities.GameTables.TPlayerBehaviorRStr[(int)TPlayerBehavior.pbNormal] == "Normal" &&
                Aow.Game.Magic.Abilities.GameTables.TPlayerBehaviorRStr[(int)TPlayerBehavior.pbRandom] == "Random", "setup behavior names are initialized");

            RtlUnit.Settings = new MemorySettingsStore();
            Aow.Game.Magic.Abilities.GameTables.AoWEngine = oldEngine;
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetMovementSpeed(Aow.Game.Engine.TMovementSpeed.mmsNormal);
            FrameFixtureInputs.MeasuredFrameRate = 9;
            var world = new Aow.Game.Ui.Dialogs.TMWindow(null) { MapWindow = mapWindow };
            world.MapWindowUpdate();
            Check(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FFPS == 9, "captured FPS reaches the strategic movement input");
            var army = new MovementArmy { FMoveStep = -1, FUnitGFXResource = new(null) { F_UnitGFXResource_30 = 2 } };
            army.MoveFirst(1, false);
            Check(army.FMoveSteps == 4, "native capture advances each eight-step move in four frames");
            foreach (int fps in new[] { 1, 6, 8, 9, 14 })
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.UpdateFPS(fps);
                army.FMoveStep = -1;
                army.MoveFirst(1, false);
                Check(army.FMoveSteps == 4, "positive FPS below 15 preserves the captured movement tier: " + fps);
            }
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.UpdateFPS(30);
            army.FMoveStep = -1;
            army.MoveFirst(1, false);
            Check(army.FMoveSteps == 8, "normal measured FPS retains the eight-step animation");
        }
        finally
        {
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow = oldWindow;
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap = oldMap;
            Aow.Game.Magic.Abilities.GameTables.AoWEngine = oldEngine;
            RtlUnit.Settings = oldSettings;
            FrameFixtureInputs.AddressAnimationPhase = null;
            FrameFixtureInputs.MeasuredFrameRate = null;
        }
        Console.WriteLine($"fixture input checks: {checks} passed, 0 failed");
        return 0;
    }

    private sealed class StartupScope : IDisposable
    {
        private readonly IFileSystem files = RtlUnit.Files;
        private readonly ISettingsStore? settings = RtlUnit.Settings;
        private readonly IDialogService? dialogs = RtlUnit.Dialogs;
        private readonly IClock clock = RtlUnit.Clock;
        private readonly string exe = RtlUnit.Application.ExeName;
        private readonly bool ownExtensions = RtlUnit.WritesOwnExtensions;
        private readonly string scratch = Path.Combine(Path.GetTempPath(), "aow-frame-inputs-" + Guid.NewGuid().ToString("N"));

        internal StartupScope(string root)
        {
            Directory.CreateDirectory(scratch);
            RtlUnit.InstallHost(new FrameFixture.FixtureHost(root, scratch, [], clock, null));
            Aow.Game.Engine.Startup.InitializeAll();
            Aow.Game.Audio.NullSoundSystem.Install();
            Aow.Game.Magic.Abilities.GameTables.ValidateAoWEngine();
        }

        public void Dispose()
        {
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.Dispose();
            RtlUnit.Files = files;
            RtlUnit.Settings = settings;
            RtlUnit.Dialogs = dialogs;
            RtlUnit.Clock = clock;
            RtlUnit.Application.ExeName = exe;
            RtlUnit.WritesOwnExtensions = ownExtensions;
            Directory.Delete(scratch, true);
        }
    }

    private sealed class MovementArmy : TArmyHS
    {
        public MovementArmy() : base(null!) { }

        public override void CalcShowPriority() { }

        public override sbyte GetLhx() => 0;
    }
}
