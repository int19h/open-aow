using Aow.Platform;
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.AoWWinMan;
using Aow.Units.AOWWinManager;
using Aow.Units.AoWWindow;
using Aow.Units.StartMenu;
using GameState = Aow.Units.AoWE.AoWE;

namespace Aow.Game;

internal static class FrameFixtureChecks
{
    private sealed class RawWorldMap : TAoWHSMap { }

    public static int Run()
    {
        int checks = 0;
        void Check(bool condition, string label)
        {
            if (!condition) throw new InvalidOperationException(label);
            checks++;
        }
        var instance = new TObject();
        Check(FrameFixtureInputs.AnimationOffset(instance) == ((int)((uint)instance.InstanceId >> 2) & 65535), "ordinary animation phase");
        Check(!new TGDPlay().Connected && new TGDPlay().ConnectionType == TConnectionType.conNone, "offline connection queries");
        Check(Aow.Units.AOWControlWin.TControlWin.UnitMoveText(32) == "   32", "native unit movement label spacing");
        FrameFixtureInputs.AddressAnimationPhase = 0;
        Check(FrameFixtureInputs.AnimationOffset(instance) == 0, "capture phase");
        FrameFixtureInputs.AddressAnimationPhase = null;
        var clock = new FixtureClock(new(2000, 1, 1, 12, 0, 0));
        clock.WaitMilliseconds(90000);
        Check(clock.Now == new DateTime(2000, 1, 1, 12, 0, 0) && clock.ElapsedMilliseconds == 90000, "wall time stays fixed while pacing advances");
        Check(Aow.Rtl.Rtl.DateTimeToStr(Aow.Rtl.Rtl.EncodeDate(2000, 1, 1) + Aow.Rtl.Rtl.EncodeTime(12, 0, 0, 0)) == "1/1/2000 12:00:00 PM", "native English file date includes seconds and the day period");

        var displayedFileTime = new DateTime(2000, 1, 1, 13, 0, 0);
        Check(FrameFixture.FileStampUtc(displayedFileTime, TimeSpan.FromHours(-7)) ==
            new DateTime(2000, 1, 1, 20, 0, 0, DateTimeKind.Utc), "daylight file stamp preserves captured 1 PM text");
        Check(FrameFixture.FileStampUtc(displayedFileTime, TimeSpan.FromHours(-8)) ==
            new DateTime(2000, 1, 1, 21, 0, 0, DateTimeKind.Utc), "standard file stamp preserves captured 1 PM text");

        var oldWindow = Aow.Units.AOWMWindow.AOWMWindow.MWindow;
        var oldMap = GameState.AoWHSMap;
        var oldEngine = GameState.AoWEngine;
        var oldSettings = Aow.Rtl.Rtl.Settings;
        try
        {
            var mapWindow = new TAOWWindow(null) { FVisible = true, FModal = true };
            Aow.Units.AOWMWindow.AOWMWindow.MWindow = new(null) { MapWindow = mapWindow };
            var menu = new TStartMenu(null) { FVisible = true, FModal = false };
            var hidden = new TAOWWindow(null) { FVisible = false, FModal = true };
            var manager = new TAOWWinManager(null) { FChildren = new TList() };
            manager.FChildren.Add(mapWindow);
            manager.FChildren.Add(menu);
            manager.FChildren.Add(hidden);
            var controller = new TGeneral(null) { Manager = manager };
            Check(!Aow.Units.E2GameFlow.E2GameFlow.HasModalWindow(controller), "modeless menu and hidden window do not block the map");
            menu.FModal = true;
            Check(Aow.Units.E2GameFlow.E2GameFlow.HasModalWindow(controller), "modal sibling menu blocks input");
            controller.HideTransientWindows();
            Check(!menu.FVisible && mapWindow.FVisible, "hide modal sibling and retain map");

            var players = new TPlayerList { FChildren = new Aow.Units.Engine.TEObjectList() };
            players.FChildren.Add(new TPlayer { FStatus = TPlayerStatus.psPlaying });
            players.FChildren.Add(new TPlayer { FStatus = TPlayerStatus.psTerminated });
            players.FChildren.Add(new TPlayer { FStatus = TPlayerStatus.psPlaying });
            GameState.AoWHSMap = new RawWorldMap { FPlayers = players };
            var bar = new Aow.Units.AoWTBWindow.TTBWindow(null)
            {
                PInd = new(null) { FAligned = true },
                TBMessEdit = new(null) { FAligned = true }
            };
            bar.TBMessEdit.Alignment.LeftOffset = 48;
            bar.UpdatePlayerIndicator();
            Check(bar.PInd.WinWidth == 14 && bar.TBMessEdit.Alignment.RightOffset == 292 &&
                bar.TBMessEdit.Alignment.LeftOffset == 48, "indicator counts active players and reserves the right side");
            Check(!bar.PInd.FAligned && !bar.TBMessEdit.FAligned, "indicator invalidates alignment");
            Check(GameState.TMagicSphereRStr[(int)TMagicSphere.msLife] == "Life" &&
                GameState.TMagicSphereRStr[(int)TMagicSphere.msCosmos] == "Cosmos", "sphere names are initialized");
            Check(GameState.TPlayerBehaviorRStr[(int)TPlayerBehavior.pbNormal] == "Normal" &&
                GameState.TPlayerBehaviorRStr[(int)TPlayerBehavior.pbRandom] == "Random", "setup behavior names are initialized");

            Aow.Rtl.Rtl.Settings = new MemorySettingsStore();
            GameState.AoWEngine = new() { FRegistry = new() };
            GameState.AoWEngine.FRegistry.SetMovementSpeed(Aow.Units.AoWReg.TMovementSpeed.mmsNormal);
            FrameFixtureInputs.MeasuredFrameRate = 9;
            var world = new Aow.Units.AOWMWindow.TMWindow(null) { MapWindow = mapWindow };
            world.MapWindowUpdate();
            Check(GameState.AoWHSMap.FFPS == 9, "captured FPS reaches the strategic movement input");
            var army = new MovementArmy { FMoveStep = -1, FUnitGFXResource = new() { F_UnitGFXResource_30 = 2 } };
            army.MoveFirst(1, false);
            Check(army.FMoveSteps == 4, "native capture advances each eight-step move in four frames");
            foreach (int fps in new[] { 1, 6, 8, 9, 14 })
            {
                GameState.AoWHSMap.UpdateFPS(fps);
                army.FMoveStep = -1;
                army.MoveFirst(1, false);
                Check(army.FMoveSteps == 4, "positive FPS below 15 preserves the captured movement tier: " + fps);
            }
            GameState.AoWHSMap.UpdateFPS(30);
            army.FMoveStep = -1;
            army.MoveFirst(1, false);
            Check(army.FMoveSteps == 8, "normal measured FPS retains the eight-step animation");
        }
        finally
        {
            Aow.Units.AOWMWindow.AOWMWindow.MWindow = oldWindow;
            GameState.AoWHSMap = oldMap;
            GameState.AoWEngine = oldEngine;
            Aow.Rtl.Rtl.Settings = oldSettings;
            FrameFixtureInputs.AddressAnimationPhase = null;
            FrameFixtureInputs.MeasuredFrameRate = null;
        }
        Console.WriteLine($"fixture input checks: {checks} passed, 0 failed");
        return 0;
    }

    private sealed class MovementArmy : TArmyHS
    {
        public override void CalcShowPriority() { }
        public override sbyte GetLhx() => 0;
    }
}
