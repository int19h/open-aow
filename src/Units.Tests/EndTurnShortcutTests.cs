using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.World.Map;
using Combat = Aow.Game.Combat.AoWTCUnit;

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
        Assert.Equal(TEndTurnMode.emEndTurn, play.QueuedMode());

        // The end-turn token sets the cycle when it runs. A second press then cancels,
        // the state a second click sees while the turn waits. Running that token on this
        // bare map also starts the next day, so the test leaves the cycle set and drops
        // the queued event.
        play.LeaveTurnWaiting();

        play.Press(13);

        Assert.False(play.Button.Checked);
        Assert.Equal(TEndTurnMode.emCancelEndTurn, play.QueuedMode());
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
        private readonly TAoWHSMap previousMap = Aow.Game.Magic.Abilities.AoWEUnit.AoWHSMap;
        private readonly TAoWCombatMap previousCombat = Combat.AoWCombatMap;
        private readonly TTBWindow previousWindow = AoWExeFormsUnit.TBWindow;
        private TAoWCombatMap? combat;

        internal TAoWHSMap Map { get; }
        internal TMWindow Window { get; }
        internal TAOWTurnButton Button { get; }

        public Play()
        {
            Map = new TAoWHSMap(null!);
            Aow.Game.Magic.Abilities.AoWEUnit.AoWHSMap = Map;
            var pcc = new TLocalPCC();
            pcc.Setup(Map.FPlayers.Count);
            for (int i = 0; i < Map.FPlayers.Count; i++)
            {
                var mode = Map.FPlayers.GetPlayers(i).FPlayerType == TPlayerType.ptHuman
                    ? TConnectionMode.cmLocal
                    : TConnectionMode.cmGlobal;
                pcc.SetConnectionMode(i, mode);
            }

            Map.FPlayerCommunicator.ActivatePCC(pcc);
            pcc.Release();
            Map.FTokenManager.Initialize(Map.FPlayerCommunicator);
            Window = new TMWindow(null!);
            Window.MapWindow = new TAOWWindow(null!);
            Window.MapWindow.FVisible = true;
            Window.MapWindow.OnKeyDown = Window.MapWindowKeyDown;
            Window.MapViewer = new THSMapViewer(null!);
            var bar = new TTBWindow(null!);
            Button = new TAOWTurnButton(null!);
            bar.TurnButton = Button;
            Button.OnChange = _ => bar.TurnButtonChange();
            AoWExeFormsUnit.TBWindow = bar;
        }

        private TTBWindow Bar => AoWExeFormsUnit.TBWindow;

        public void Press(ushort key)
        {
            Window.MapWindow.CheckKeyDown(ref key, TSet<TShiftState>.Of(TShiftState.ssCtrl));
        }

        public void BeginCombat()
        {
            combat = new TAoWCombatMap(null!);
        }

        public int QueuedCount() => Control().GetCount();

        public TEndTurnMode QueuedMode()
        {
            var events = Control().FEvents;
            Assert.Equal(1, events.Count);
            return Assert.IsType<TEndTurnTE>(events.GetEvent(0)).FMode;
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

        private TTokenControl Control() => Map.FTokenManager.GetTokenControl(Map.FSeatedPlayer);

        public void Dispose()
        {
            combat?.Dispose();
            Combat.AoWCombatMap = previousCombat;
            Window.MapViewer.Dispose();
            Window.MapWindow.Dispose();
            Window.Dispose();
            Button.Dispose();
            Bar.Dispose();
            AoWExeFormsUnit.TBWindow = previousWindow;
            Aow.Game.Magic.Abilities.AoWEUnit.AoWHSMap = previousMap;
            Map.Dispose();
        }
    }
}
