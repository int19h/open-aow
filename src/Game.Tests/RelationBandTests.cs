using Aow.Game.Audio;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
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

    private sealed class RecordingRelWindow : RelWindow
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
            DialogHosts.General.Manager = new AOWWinManager(null!);
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
        window.P1I = new AOWImage(null!);
        window.P2I = new AOWImage(null!);
        window.P3I = new AOWImage(null!);
        window.P4I = new AOWImage(null!);
        window.P5I = new AOWImage(null!);
        window.P6I = new AOWImage(null!);
        window.P7I = new AOWImage(null!);
        window.P8I = new AOWImage(null!);
        window.P9I = new AOWImage(null!);
        window.P10I = new AOWImage(null!);
        window.P11I = new AOWImage(null!);
        window.P12I = new AOWImage(null!);
        window.R1I = new AOWImage(null!);
        window.R2I = new AOWImage(null!);
        window.R3I = new AOWImage(null!);
        window.R4I = new AOWImage(null!);
        window.R5I = new AOWImage(null!);
        window.R6I = new AOWImage(null!);
        window.R7I = new AOWImage(null!);
        window.R8I = new AOWImage(null!);
        window.R9I = new AOWImage(null!);
        window.R10I = new AOWImage(null!);
        window.R11I = new AOWImage(null!);
        window.R12I = new AOWImage(null!);
        window.P1CB = new AOWCheckBox(null!);
        window.P2CB = new AOWCheckBox(null!);
        window.P3CB = new AOWCheckBox(null!);
        window.P4CB = new AOWCheckBox(null!);
        window.P5CB = new AOWCheckBox(null!);
        window.P6CB = new AOWCheckBox(null!);
        window.P7CB = new AOWCheckBox(null!);
        window.P8CB = new AOWCheckBox(null!);
        window.P9CB = new AOWCheckBox(null!);
        window.P10CB = new AOWCheckBox(null!);
        window.P11CB = new AOWCheckBox(null!);
        window.P12CB = new AOWCheckBox(null!);
        window.R1CB = new AOWCheckBox(null!);
        window.R2CB = new AOWCheckBox(null!);
        window.R3CB = new AOWCheckBox(null!);
        window.R4CB = new AOWCheckBox(null!);
        window.R5CB = new AOWCheckBox(null!);
        window.R6CB = new AOWCheckBox(null!);
        window.R7CB = new AOWCheckBox(null!);
        window.R8CB = new AOWCheckBox(null!);
        window.R9CB = new AOWCheckBox(null!);
        window.R10CB = new AOWCheckBox(null!);
        window.R11CB = new AOWCheckBox(null!);
        window.R12CB = new AOWCheckBox(null!);
        window.RelWin = new AOWWindow(null!);
        window.RelPnl = new AOWPanel(null!);
        window.PlyrGlow = new AOWImage(null!);
        window.RNewActionPnl = new AOWPanel(null!);
        window.RIncomingPnl = new AOWPanel(null!);
        window.RMsgEditPnl = new AOWPanel(null!);
        window.RSBribePnl = new AOWPanel(null!);
        window.RMsgRecPnl = new AOWPanel(null!);
        window.IncommingInfoPnl = new AOWPanel(null!);
        window.ActionsLB = new AOWListBox(null!);
        window.SMemo = new AOWMemo(null!);
        window.MMemo = new AOWMemo(null!);
        window.ASelect = new AOWButton(null!);
        window.RelWindowCreate();
        return window;
    }

    private static void MarkChecks(RelWindow window)
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

        scope.Map.FPlayers.SetNumberOfPlayers(12);
        scope.Map.FSeatedPlayer = 1;
        var seated = scope.Map.FPlayers.GetPlayers(1);
        for (int i = 1; i <= 12; i++)
        {
            var player = scope.Map.FPlayers.GetPlayers(i);
            player.FStatus = Player.Status.Playing;
            player.FName = "P" + i;
            if (i != 1)
            {
                seated.FFirstDaySeen[i] = 0;
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
        window.P1I.FChecked = true;
        window.R12I.FChecked = true;

        window.ShowRelations(0);
        Assert.False(window.P1CB.Checked);
        Assert.False(window.P12CB.Checked);
        Assert.True(window.R12I.FChecked);
        Assert.Equal(0, window.ShowCount);
        Assert.False(window.RelPnl.FAligned);
        Assert.False(window.RelWin.FDrawn);

        window.ShowRelations(-23);
        Assert.False(window.P1CB.Checked);
        Assert.True(window.P1I.FChecked);
        Assert.Equal(0, window.ShowCount);

        window.ShowRelations(1);
        Assert.True(window.P1CB.Checked);
        Assert.False(window.P12CB.Checked);
        Assert.Equal(0, window.ShowCount);

        window.ShowRelations(12);
        Assert.False(window.P1CB.Checked);
        Assert.True(window.P12CB.Checked);
        Assert.Equal(0, window.ShowCount);

        scope.Map.FPlayers.SetNumberOfPlayers(12);
        scope.Map.FSeatedPlayer = 12;
        var seated = scope.Map.FPlayers.GetPlayers(12);
        for (int i = 1; i <= 12; i++)
        {
            var player = scope.Map.FPlayers.GetPlayers(i);
            player.FStatus = Player.Status.Playing;
            player.FName = "S" + i;
            if (i != 12)
            {
                seated.FFirstDaySeen[i] = 0;
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
        Assert.Equal(-1, window.FSelectedIndex);
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
        Assert.Equal(-1, window.FSelectedIndex);
        Assert.False(window.P1CB.Checked);
        Assert.False(window.P12CB.Checked);
        Assert.Equal(10, window.P1CB.Alignment.TopOffset);
        Assert.Equal(2, window.P1I.CurrentImage);
        Assert.Equal(2, window.P12I.CurrentImage);
        Assert.Equal(5, window.R1I.CurrentImage);
        Assert.Equal(5, window.R12I.CurrentImage);

        scope.Map.FSeatedPlayer = 0;
        window.FPlayers.Add(0);
        window.HighlightPlayer(0);
        Assert.Equal(0, window.FSelectedIndex);
        Assert.True(window.P1CB.Checked);
        Assert.False(window.P12CB.Checked);
        Assert.Equal(20, window.P1CB.Alignment.TopOffset);
        Assert.Equal(20, window.PlyrGlow.Alignment.TopOffset);
        Assert.Equal(2, window.P1I.CurrentImage);

        window.FPlayers.Clear();
        for (int i = 1; i <= 12; i++)
        {
            window.FPlayers.Add(i);
        }

        scope.Map.FPlayers.SetNumberOfPlayers(1);
        scope.Map.FSeatedPlayer = 0;
        window.HighlightPlayer(12);
        Assert.Equal(11, window.FSelectedIndex);
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
        Assert.Equal(7, window.FSelectedRace);
        Assert.Equal(-1, window.FSelectedPlayer);
        Assert.Equal(19, window.FSelectedIndex);
        Assert.True(window.P12CB.Checked);
        Assert.Equal(175, window.P12CB.Alignment.TopOffset);
        Assert.False(window.R1CB.Checked);
        Assert.False(window.R12CB.Checked);
        Assert.Equal(185, window.R1CB.Alignment.TopOffset);
        Assert.Equal(2, window.P1I.CurrentImage);
        Assert.Equal(2, window.P12I.CurrentImage);
        Assert.Equal(5, window.R1I.CurrentImage);
        Assert.Equal(5, window.R12I.CurrentImage);

        window.FRaces.Add(7);
        window.HighlightRace(7);
        Assert.Equal(20, window.FSelectedIndex);
        Assert.True(window.R1CB.Checked);
        Assert.False(window.P12CB.Checked);
        Assert.Equal(175, window.R1CB.Alignment.TopOffset);

        window.FRaces.Clear();
        for (int i = 0; i <= 11; i++)
        {
            window.FRaces.Add(i);
        }

        window.HighlightRace(11);
        Assert.Equal(31, window.FSelectedIndex);
        Assert.True(window.R12CB.Checked);
        Assert.False(window.P12CB.Checked);
        Assert.Equal(175, window.R12CB.Alignment.TopOffset);
        Assert.Same(window.R12CB, window.FChecks[23]);
    }
}
