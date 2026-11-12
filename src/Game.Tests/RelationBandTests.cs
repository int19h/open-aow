using Aow.Game.Audio;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Dialogs;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Index bands of the relations window after the image list and the check box list split.
[Collection("Engine")]
[EngineState]
public sealed class RelationBandTests
{
    private sealed class MapScope : IDisposable
    {
        private readonly AoWHSMap previous = AoWHSMap.Current;

        public MapScope()
        {
            Map = new AoWHSMap(null!);
            AoWHSMap.Current = Map;
        }

        public AoWHSMap Map { get; }

        public void Dispose() => AoWHSMap.Current = previous;
    }

    private sealed class RecordingRelWindow : RelationWindow
    {
        public RecordingRelWindow() : base(null!)
        {
        }

        public int ShownPlayer { get; private set; } = -2;

        public int ShowCount { get; private set; }

        public override void ShowPlayerActions(sbyte player)
        {
            ShowCount += 1;
            ShownPlayer = player;
        }
    }

    private sealed class DialogScope : IDisposable
    {
        private readonly General previousGeneral = DialogHosts.General;
        private readonly SoundSystem previousSound = AudioHost.SoundSystem;

        public DialogScope()
        {
            AudioHost.SoundSystem = null;
            DialogHosts.General = new General(null!);
            DialogHosts.General.Manager = new WindowManager(null!);
        }

        public void Dispose()
        {
            DialogHosts.General = previousGeneral;
            AudioHost.SoundSystem = previousSound;
        }
    }

    private static RecordingRelWindow CreateWindow()
    {
        var window = new RecordingRelWindow();
        window.P1I = new Image(null!);
        window.P2I = new Image(null!);
        window.P3I = new Image(null!);
        window.P4I = new Image(null!);
        window.P5I = new Image(null!);
        window.P6I = new Image(null!);
        window.P7I = new Image(null!);
        window.P8I = new Image(null!);
        window.P9I = new Image(null!);
        window.P10I = new Image(null!);
        window.P11I = new Image(null!);
        window.P12I = new Image(null!);
        window.R1I = new Image(null!);
        window.R2I = new Image(null!);
        window.R3I = new Image(null!);
        window.R4I = new Image(null!);
        window.R5I = new Image(null!);
        window.R6I = new Image(null!);
        window.R7I = new Image(null!);
        window.R8I = new Image(null!);
        window.R9I = new Image(null!);
        window.R10I = new Image(null!);
        window.R11I = new Image(null!);
        window.R12I = new Image(null!);
        window.P1CB = new CheckBox(null!);
        window.P2CB = new CheckBox(null!);
        window.P3CB = new CheckBox(null!);
        window.P4CB = new CheckBox(null!);
        window.P5CB = new CheckBox(null!);
        window.P6CB = new CheckBox(null!);
        window.P7CB = new CheckBox(null!);
        window.P8CB = new CheckBox(null!);
        window.P9CB = new CheckBox(null!);
        window.P10CB = new CheckBox(null!);
        window.P11CB = new CheckBox(null!);
        window.P12CB = new CheckBox(null!);
        window.R1CB = new CheckBox(null!);
        window.R2CB = new CheckBox(null!);
        window.R3CB = new CheckBox(null!);
        window.R4CB = new CheckBox(null!);
        window.R5CB = new CheckBox(null!);
        window.R6CB = new CheckBox(null!);
        window.R7CB = new CheckBox(null!);
        window.R8CB = new CheckBox(null!);
        window.R9CB = new CheckBox(null!);
        window.R10CB = new CheckBox(null!);
        window.R11CB = new CheckBox(null!);
        window.R12CB = new CheckBox(null!);
        window.RelWin = new Window(null!);
        window.RelPnl = new Panel(null!);
        window.PlyrGlow = new Image(null!);
        window.RNewActionPnl = new Panel(null!);
        window.RIncomingPnl = new Panel(null!);
        window.RMsgEditPnl = new Panel(null!);
        window.RSBribePnl = new Panel(null!);
        window.RMsgRecPnl = new Panel(null!);
        window.IncommingInfoPnl = new Panel(null!);
        window.ActionsLB = new ListBox(null!);
        window.SMemo = new Memo(null!);
        window.MMemo = new Memo(null!);
        window.ASelect = new Button(null!);
        window.RelWindowCreate();
        return window;
    }

    private static void MarkChecks(RelationWindow window)
    {
        foreach (var check in window.FChecks)
        {
            check.Checked = true;
            check.Alignment.TopOffset = 99;
        }
    }

    [Fact]
    public void FillPlayersUsesThePlayerCheckBand()
    {
        using var scope = new MapScope();
        using var dialogs = new DialogScope();
        var window = CreateWindow();
        window.R1CB.Visible = false;
        window.FillPlayers();
        Assert.False(window.P1CB.Visible);
        Assert.False(window.P12CB.Visible);
        Assert.False(window.R1CB.Visible);

        scope.Map.Players.SetNumberOfPlayers(12);
        scope.Map.SeatedPlayer = 1;
        var seated = scope.Map.Players.GetPlayers(1);
        for (int i = 1; i <= 12; i++)
        {
            var player = scope.Map.Players.GetPlayers(i);
            player.Status = PlayerStatus.Playing;
            player.Name = "P" + i;
            if (i != 1)
            {
                seated.FirstDaySeen[i] = 0;
            }
        }

        window.FillPlayers();
        Assert.True(window.P1CB.Visible);
        Assert.True(window.P12CB.Visible);
        Assert.False(window.R1CB.Visible);
        Assert.Equal("P1", window.P1CB.ToolTipText[0]);
        Assert.Equal("P12", window.P12CB.ToolTipText[0]);
        Assert.Same(window.P1CB, window.FChecks[0]);
        Assert.Same(window.P12CB, window.FChecks[11]);
    }

    [Fact]
    public void ShowRelationsSkipsImageSlotsAndChecksThePlayerBand()
    {
        using var scope = new MapScope();
        using var dialogs = new DialogScope();
        var window = CreateWindow();
        window.P1I._checked = true;
        window.R12I._checked = true;

        window.ShowRelations(0);
        Assert.False(window.P1CB.Checked);
        Assert.False(window.P12CB.Checked);
        Assert.True(window.R12I._checked);
        Assert.Equal(0, window.ShowCount);
        Assert.False(window.RelPnl.Aligned);
        Assert.False(window.RelWin.Drawn);

        window.ShowRelations(-23);
        Assert.False(window.P1CB.Checked);
        Assert.True(window.P1I._checked);
        Assert.Equal(0, window.ShowCount);

        window.ShowRelations(1);
        Assert.True(window.P1CB.Checked);
        Assert.False(window.P12CB.Checked);
        Assert.Equal(0, window.ShowCount);

        window.ShowRelations(12);
        Assert.False(window.P1CB.Checked);
        Assert.True(window.P12CB.Checked);
        Assert.Equal(0, window.ShowCount);

        scope.Map.Players.SetNumberOfPlayers(12);
        scope.Map.SeatedPlayer = 12;
        var seated = scope.Map.Players.GetPlayers(12);
        for (int i = 1; i <= 12; i++)
        {
            var player = scope.Map.Players.GetPlayers(i);
            player.Status = PlayerStatus.Playing;
            player.Name = "S" + i;
            if (i != 12)
            {
                seated.FirstDaySeen[i] = 0;
            }
        }

        window.ShowRelations(1);
        Assert.True(window.P1CB.Checked);
        Assert.True(window.P12CB.Checked);
        Assert.Equal(1, window.ShowCount);
        Assert.Equal(12, window.ShownPlayer);
    }

    [Fact]
    public void ResetChecksClearsBothCheckBands()
    {
        using var dialogs = new DialogScope();
        var window = CreateWindow();
        MarkChecks(window);
        window.P1I.CurrentImage = 7;
        window.ResetChecks();
        Assert.Equal(-1, window.SelectedIndex);
        Assert.False(window.P1CB.Checked);
        Assert.False(window.P12CB.Checked);
        Assert.False(window.R1CB.Checked);
        Assert.False(window.R12CB.Checked);
        Assert.Equal(10, window.P1CB.Alignment.TopOffset);
        Assert.Equal(10, window.P12CB.Alignment.TopOffset);
        Assert.Equal(185, window.R1CB.Alignment.TopOffset);
        Assert.Equal(185, window.R12CB.Alignment.TopOffset);
        Assert.Equal(7, window.P1I.CurrentImage);
    }

    [Fact]
    public void HighlightPlayerReachesThePlayerImageAndCheckForEachEnd()
    {
        using var scope = new MapScope();
        using var dialogs = new DialogScope();
        var window = CreateWindow();
        MarkChecks(window);
        window.HighlightPlayer(4);
        Assert.Equal(-1, window.SelectedIndex);
        Assert.False(window.P1CB.Checked);
        Assert.False(window.P12CB.Checked);
        Assert.Equal(10, window.P1CB.Alignment.TopOffset);
        Assert.Equal(2, window.P1I.CurrentImage);
        Assert.Equal(2, window.P12I.CurrentImage);
        Assert.Equal(5, window.R1I.CurrentImage);
        Assert.Equal(5, window.R12I.CurrentImage);

        scope.Map.SeatedPlayer = 0;
        window.Players.Add(0);
        window.HighlightPlayer(0);
        Assert.Equal(0, window.SelectedIndex);
        Assert.True(window.P1CB.Checked);
        Assert.False(window.P12CB.Checked);
        Assert.Equal(20, window.P1CB.Alignment.TopOffset);
        Assert.Equal(20, window.PlyrGlow.Alignment.TopOffset);
        Assert.Equal(2, window.P1I.CurrentImage);

        window.Players.Clear();
        for (int i = 1; i <= 12; i++)
        {
            window.Players.Add(i);
        }

        scope.Map.Players.SetNumberOfPlayers(1);
        scope.Map.SeatedPlayer = 0;
        window.HighlightPlayer(12);
        Assert.Equal(11, window.SelectedIndex);
        Assert.False(window.P1CB.Checked);
        Assert.True(window.P12CB.Checked);
        Assert.Equal(20, window.P12CB.Alignment.TopOffset);
        Assert.Equal(10, window.PlyrGlow.Alignment.TopOffset);
        Assert.Same(window.P12CB, window.FChecks[11]);
        Assert.Same(window.P1I, window.FImages[0]);
        Assert.Same(window.R12I, window.FImages[23]);
    }

    [Fact]
    public void HighlightRaceOnAMissingRaceUsesP12CB()
    {
        using var dialogs = new DialogScope();
        var window = CreateWindow();
        MarkChecks(window);
        window.HighlightRace(7);
        Assert.Equal(7, window.SelectedRace);
        Assert.Equal(-1, window.SelectedPlayer);
        Assert.Equal(19, window.SelectedIndex);
        Assert.True(window.P12CB.Checked);
        Assert.Equal(175, window.P12CB.Alignment.TopOffset);
        Assert.False(window.R1CB.Checked);
        Assert.False(window.R12CB.Checked);
        Assert.Equal(185, window.R1CB.Alignment.TopOffset);
        Assert.Equal(2, window.P1I.CurrentImage);
        Assert.Equal(2, window.P12I.CurrentImage);
        Assert.Equal(5, window.R1I.CurrentImage);
        Assert.Equal(5, window.R12I.CurrentImage);

        window.Races.Add(7);
        window.HighlightRace(7);
        Assert.Equal(20, window.SelectedIndex);
        Assert.True(window.R1CB.Checked);
        Assert.False(window.P12CB.Checked);
        Assert.Equal(175, window.R1CB.Alignment.TopOffset);

        window.Races.Clear();
        for (int i = 0; i <= 11; i++)
        {
            window.Races.Add(i);
        }

        window.HighlightRace(11);
        Assert.Equal(31, window.SelectedIndex);
        Assert.True(window.R12CB.Checked);
        Assert.False(window.P12CB.Checked);
        Assert.Equal(175, window.R12CB.Alignment.TopOffset);
        Assert.Same(window.R12CB, window.FChecks[23]);
    }
}
