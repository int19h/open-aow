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
        Assert.False(play.Window.MainWindow.Handled);
        Assert.Equal(EndTurnTokenEventMode.EndTurn, play.QueuedMode());

        // The end-turn token sets the cycle when it runs. A second press then cancels,
        // the state a second click sees while the turn waits. Running that token on this
        // bare map also starts the next day, so the test leaves the cycle set and drops
        // the queued event.
        play.LeaveTurnWaiting();

        play.Press(13);

        Assert.False(play.Button.Checked);
        Assert.Equal(EndTurnTokenEventMode.CancelEndTurn, play.QueuedMode());
    }

    [Fact]
    public void ShortcutDoesNothingWhileTheButtonAnimates()
    {
        using var play = new Play();
        play.Button.Animating = true;

        play.Press(69);
        play.Press(13);

        Assert.False(play.Button.Checked);
        Assert.True(play.Button.Animating);
        Assert.Equal(0, play.QueuedCount());
    }

    [Fact]
    public void CombatKeepsItsOwnShortcut()
    {
        using var play = new Play();
        play.BeginCombat();

        play.Press(69);

        Assert.False(play.Button.Checked);
        Assert.True(play.Window.MainWindow.Handled);
        Assert.Equal(0, play.QueuedCount());

        play.Press(13);

        Assert.False(play.Button.Checked);
        Assert.True(play.Window.MainWindow.Handled);
        Assert.Equal(0, play.QueuedCount());
    }

    private sealed class Play : IDisposable
    {
        private readonly StrategicMap _previousMap = StrategicMap.Current;
        private readonly CombatMap _previousCombat = CombatMap.Current;
        private readonly TaskBar _previousWindow = DialogHosts.TaskBar;
        private CombatMap? _combat;

        internal StrategicMap Map { get; }
        internal MapWindow Window { get; }
        internal TurnButton Button { get; }

        public Play()
        {
            Map = new StrategicMap(null!);
            StrategicMap.Current = Map;
            var playerCommunicationControl = new LocalPlayerCommunicationControl();
            playerCommunicationControl.Setup(Map.Players.Count);
            for (int i = 0; i < Map.Players.Count; i++)
            {
                var mode = Map.Players.GetPlayers(i).PlayerType == PlayerType.Human
                    ? ConnectionMode.Local
                    : ConnectionMode.Global;
                playerCommunicationControl.SetConnectionMode(i, mode);
            }

            Map.PlayerCommunicator.ActivatePlayerCommunicationControl(playerCommunicationControl);
            playerCommunicationControl.Release();
            Map.TokenManager.Initialize(Map.PlayerCommunicator);
            Window = new MapWindow(null!);
            Window.MainWindow = new Window(null!);
            Window.MainWindow.SetVisibleWithoutUpdate(true);
            Window.MainWindow.OnKeyDown = Window.MapWindowKeyDown;
            Window.MapViewer = new HexSpriteMapViewer(null!);
            TaskBar bar = new TaskBar(null!);
            Button = new TurnButton(null!);
            bar.WorldTurnButton = Button;
            Button.OnChange = _ => bar.TurnButtonChange();
            DialogHosts.TaskBar = bar;
        }

        private TaskBar Bar => DialogHosts.TaskBar;

        public void Press(ushort key)
        {
            Window.MainWindow.CheckKeyDown(ref key, Set<ShiftState>.Of(ShiftState.Ctrl));
        }

        public void BeginCombat()
        {
            _combat = new CombatMap(null!);
        }

        public int QueuedCount() => Control().GetCount();

        public EndTurnTokenEventMode QueuedMode()
        {
            var events = Control().Events;
            Assert.Equal(1, events.Count);
            return Assert.IsType<EndTurnTokenEvent>(events.GetEvent(0)).Mode;
        }

        public void LeaveTurnWaiting()
        {
            var events = Control().Events;
            while (events.Count != 0)
            {
                events.Delete(0);
            }

            Map.Players.GetPlayers(Map.SeatedPlayer).SetEndTurn(true);
            Button.Checked = true;
        }

        private TokenControl Control() => Map.TokenManager.GetTokenControl(Map.SeatedPlayer);

        public void Dispose()
        {
            _combat?.Dispose();
            CombatMap.Current = _previousCombat;
            Window.MapViewer.Dispose();
            Window.MainWindow.Dispose();
            Window.Dispose();
            Button.Dispose();
            Bar.Dispose();
            DialogHosts.TaskBar = _previousWindow;
            StrategicMap.Current = _previousMap;
            Map.Dispose();
        }
    }
}
