using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.World.Map;

namespace Aow.Units.Tests;

/// <summary>
/// Ctrl+E and Ctrl+Enter on the strategic map press the end-turn button. A port addition.
/// Combat keeps its own handling of those keys.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class EndTurnShortcutTests
{
    [Fact]
    public void ShortcutChecksTheButtonAndASecondPressCancels()
    {
        using var play = new Play();

        play.Press(69);

        Assert.True(play.Button.Checked);
        Assert.False(play.Window.MapWindow.FHandled);
        Assert.Equal(EndTurnTE.EndTurnMode.EndTurn, play.QueuedMode());

        // The end-turn token sets the cycle when it runs. A second press then cancels,
        // the state a second click sees while the turn waits. Running that token on this
        // bare map also starts the next day, so the test leaves the cycle set and drops
        // the queued event.
        play.LeaveTurnWaiting();

        play.Press(13);

        Assert.False(play.Button.Checked);
        Assert.Equal(EndTurnTE.EndTurnMode.CancelEndTurn, play.QueuedMode());
    }

    [Fact]
    public void ShortcutDoesNothingWhileTheButtonAnimates()
    {
        using var play = new Play();
        play.Button.FAnimating = true;

        play.Press(69);
        play.Press(13);

        Assert.False(play.Button.Checked);
        Assert.True(play.Button.FAnimating);
        Assert.Equal(0, play.QueuedCount());
    }

    [Fact]
    public void CombatKeepsItsOwnShortcut()
    {
        using var play = new Play();
        play.BeginCombat();

        play.Press(69);

        Assert.False(play.Button.Checked);
        Assert.True(play.Window.MapWindow.FHandled);
        Assert.Equal(0, play.QueuedCount());

        play.Press(13);

        Assert.False(play.Button.Checked);
        Assert.True(play.Window.MapWindow.FHandled);
        Assert.Equal(0, play.QueuedCount());
    }

    private sealed class Play : IDisposable
    {
        private readonly AoWHSMap previousMap = AoWHSMap.Current;
        private readonly AoWCombatMap previousCombat = AoWCombatMap.Current;
        private readonly TaskBar previousWindow = DialogHosts.TBWindow;
        private AoWCombatMap? combat;

        internal AoWHSMap Map { get; }
        internal MWindow Window { get; }
        internal AOWTurnButton Button { get; }

        public Play()
        {
            Map = new AoWHSMap(null!);
            AoWHSMap.Current = Map;
            var pcc = new LocalPCC();
            pcc.Setup(Map.FPlayers.Count);
            for (int i = 0; i < Map.FPlayers.Count; i++)
            {
                var mode = Map.FPlayers.GetPlayers(i).FPlayerType == PlayerType.Human
                    ? ConnectionMode.Local
                    : ConnectionMode.Global;
                pcc.SetConnectionMode(i, mode);
            }

            Map.FPlayerCommunicator.ActivatePCC(pcc);
            pcc.Release();
            Map.FTokenManager.Initialize(Map.FPlayerCommunicator);
            Window = new MWindow(null!);
            Window.MapWindow = new AOWWindow(null!);
            Window.MapWindow.FVisible = true;
            Window.MapWindow.OnKeyDown = Window.MapWindowKeyDown;
            Window.MapViewer = new HSMapViewer(null!);
            TaskBar bar = new TaskBar(null!);
            Button = new AOWTurnButton(null!);
            bar.TurnButton = Button;
            Button.OnChange = _ => bar.TurnButtonChange();
            DialogHosts.TBWindow = bar;
        }

        private TaskBar Bar => DialogHosts.TBWindow;

        public void Press(ushort key)
        {
            Window.MapWindow.CheckKeyDown(ref key, Set<ShiftState>.Of(ShiftState.Ctrl));
        }

        public void BeginCombat()
        {
            combat = new AoWCombatMap(null!);
        }

        public int QueuedCount() => Control().GetCount();

        public EndTurnTE.EndTurnMode QueuedMode()
        {
            var events = Control().FEvents;
            Assert.Equal(1, events.Count);
            return Assert.IsType<EndTurnTE>(events.GetEvent(0)).FMode;
        }

        public void LeaveTurnWaiting()
        {
            var events = Control().FEvents;
            while (events.Count != 0)
            {
                events.Delete(0);
            }

            Map.FPlayers.GetPlayers(Map.FSeatedPlayer).SetEndTurn(true);
            Button.Checked = true;
        }

        private TokenControl Control() => Map.FTokenManager.GetTokenControl(Map.FSeatedPlayer);

        public void Dispose()
        {
            combat?.Dispose();
            AoWCombatMap.Current = previousCombat;
            Window.MapViewer.Dispose();
            Window.MapWindow.Dispose();
            Window.Dispose();
            Button.Dispose();
            Bar.Dispose();
            DialogHosts.TBWindow = previousWindow;
            AoWHSMap.Current = previousMap;
            Map.Dispose();
        }
    }
}
