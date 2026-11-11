using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

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
        Assert.False(play.Window.MainWindow.FHandled);
        Assert.Equal(EndTurnTokenEvent.Mode.EndTurn, play.QueuedMode());

        // The end-turn token sets the cycle when it runs. A second press then cancels,
        // the state a second click sees while the turn waits. Running that token on this
        // bare map also starts the next day, so the test leaves the cycle set and drops
        // the queued event.
        play.LeaveTurnWaiting();

        play.Press(13);

        Assert.False(play.Button.Checked);
        Assert.Equal(EndTurnTokenEvent.Mode.CancelEndTurn, play.QueuedMode());
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
        Assert.True(play.Window.MainWindow.FHandled);
        Assert.Equal(0, play.QueuedCount());

        play.Press(13);

        Assert.False(play.Button.Checked);
        Assert.True(play.Window.MainWindow.FHandled);
        Assert.Equal(0, play.QueuedCount());
    }

    private sealed class Play : IDisposable
    {
        private readonly AoWHSMap previousMap = AoWHSMap.Current;
        private readonly CombatMap previousCombat = CombatMap.Current;
        private readonly TaskBar previousWindow = DialogHosts.TBWindow;
        private CombatMap? combat;

        internal AoWHSMap Map { get; }
        internal MapWindow Window { get; }
        internal TurnButton Button { get; }

        public Play()
        {
            Map = new AoWHSMap(null!);
            AoWHSMap.Current = Map;
            var pcc = new LocalPlayerCommunicationControl();
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
            Window = new MapWindow(null!);
            Window.MainWindow = new Window(null!);
            Window.MainWindow.FVisible = true;
            Window.MainWindow.OnKeyDown = Window.MapWindowKeyDown;
            Window.MapViewer = new HexSpriteMapViewer(null!);
            TaskBar bar = new TaskBar(null!);
            Button = new TurnButton(null!);
            bar.TurnButton = Button;
            Button.OnChange = _ => bar.TurnButtonChange();
            DialogHosts.TBWindow = bar;
        }

        private TaskBar Bar => DialogHosts.TBWindow;

        public void Press(ushort key)
        {
            Window.MainWindow.CheckKeyDown(ref key, Set<ShiftState>.Of(ShiftState.Ctrl));
        }

        public void BeginCombat()
        {
            combat = new CombatMap(null!);
        }

        public int QueuedCount() => Control().GetCount();

        public EndTurnTokenEvent.Mode QueuedMode()
        {
            var events = Control().FEvents;
            Assert.Equal(1, events.Count);
            return Assert.IsType<EndTurnTokenEvent>(events.GetEvent(0)).FMode;
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
            CombatMap.Current = previousCombat;
            Window.MapViewer.Dispose();
            Window.MainWindow.Dispose();
            Window.Dispose();
            Button.Dispose();
            Bar.Dispose();
            DialogHosts.TBWindow = previousWindow;
            AoWHSMap.Current = previousMap;
            Map.Dispose();
        }
    }
}
