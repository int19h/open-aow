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
        private readonly StrategicMap previous = StrategicMap.Current;

        public MapScope()
        {
            Map = new StrategicMap(null!);
            StrategicMap.Current = Map;
        }

        public StrategicMap Map { get; }

        public void Dispose() => StrategicMap.Current = previous;
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
        window.Player1Image = new Image(null!);
        window.Player2Image = new Image(null!);
        window.Player3Image = new Image(null!);
        window.Player4Image = new Image(null!);
        window.Player5Image = new Image(null!);
        window.Player6Image = new Image(null!);
        window.Player7Image = new Image(null!);
        window.Player8Image = new Image(null!);
        window.Player9Image = new Image(null!);
        window.Player10Image = new Image(null!);
        window.Player11Image = new Image(null!);
        window.Player12Image = new Image(null!);
        window.Race1Image = new Image(null!);
        window.Race2Image = new Image(null!);
        window.Race3Image = new Image(null!);
        window.Race4Image = new Image(null!);
        window.Race5Image = new Image(null!);
        window.Race6Image = new Image(null!);
        window.Race7Image = new Image(null!);
        window.Race8Image = new Image(null!);
        window.Race9Image = new Image(null!);
        window.Race10Image = new Image(null!);
        window.Race11Image = new Image(null!);
        window.Race12Image = new Image(null!);
        window.Player1CheckBox = new CheckBox(null!);
        window.Player2CheckBox = new CheckBox(null!);
        window.Player3CheckBox = new CheckBox(null!);
        window.Player4CheckBox = new CheckBox(null!);
        window.Player5CheckBox = new CheckBox(null!);
        window.Player6CheckBox = new CheckBox(null!);
        window.Player7CheckBox = new CheckBox(null!);
        window.Player8CheckBox = new CheckBox(null!);
        window.Player9CheckBox = new CheckBox(null!);
        window.Player10CheckBox = new CheckBox(null!);
        window.Player11CheckBox = new CheckBox(null!);
        window.Player12CheckBox = new CheckBox(null!);
        window.Race1CheckBox = new CheckBox(null!);
        window.Race2CheckBox = new CheckBox(null!);
        window.Race3CheckBox = new CheckBox(null!);
        window.Race4CheckBox = new CheckBox(null!);
        window.Race5CheckBox = new CheckBox(null!);
        window.Race6CheckBox = new CheckBox(null!);
        window.Race7CheckBox = new CheckBox(null!);
        window.Race8CheckBox = new CheckBox(null!);
        window.Race9CheckBox = new CheckBox(null!);
        window.Race10CheckBox = new CheckBox(null!);
        window.Race11CheckBox = new CheckBox(null!);
        window.Race12CheckBox = new CheckBox(null!);
        window.MainWindow = new Window(null!);
        window.RelationPanel = new Panel(null!);
        window.PlayerGlow = new Image(null!);
        window.NewActionPanel = new Panel(null!);
        window.IncomingPanel = new Panel(null!);
        window.MessageEditPanel = new Panel(null!);
        window.SendBribePanel = new Panel(null!);
        window.ReceivedMessagePanel = new Panel(null!);
        window.IncomingInfoPanel = new Panel(null!);
        window.ActionsList = new ListBox<DiplomaticAction>(null!);
        window.SendMessageMemo = new Memo(null!);
        window.IncomingMemo = new Memo(null!);
        window.SelectActionButton = new Button(null!);
        window.RelationWindowCreate();
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
        window.Race1CheckBox.Visible = false;
        window.FillPlayers();
        Assert.False(window.Player1CheckBox.Visible);
        Assert.False(window.Player12CheckBox.Visible);
        Assert.False(window.Race1CheckBox.Visible);

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
        Assert.True(window.Player1CheckBox.Visible);
        Assert.True(window.Player12CheckBox.Visible);
        Assert.False(window.Race1CheckBox.Visible);
        Assert.Equal("P1", window.Player1CheckBox.ToolTipText[0]);
        Assert.Equal("P12", window.Player12CheckBox.ToolTipText[0]);
        Assert.Same(window.Player1CheckBox, window.FChecks[0]);
        Assert.Same(window.Player12CheckBox, window.FChecks[11]);
    }

    [Fact]
    public void ShowRelationsSkipsImageSlotsAndChecksThePlayerBand()
    {
        using var scope = new MapScope();
        using var dialogs = new DialogScope();
        var window = CreateWindow();
        window.Player1Image._checked = true;
        window.Race12Image._checked = true;

        window.ShowRelations(0);
        Assert.False(window.Player1CheckBox.Checked);
        Assert.False(window.Player12CheckBox.Checked);
        Assert.True(window.Race12Image._checked);
        Assert.Equal(0, window.ShowCount);
        Assert.False(window.RelationPanel.Aligned);
        Assert.False(window.MainWindow.Drawn);

        window.ShowRelations(-23);
        Assert.False(window.Player1CheckBox.Checked);
        Assert.True(window.Player1Image._checked);
        Assert.Equal(0, window.ShowCount);

        window.ShowRelations(1);
        Assert.True(window.Player1CheckBox.Checked);
        Assert.False(window.Player12CheckBox.Checked);
        Assert.Equal(0, window.ShowCount);

        window.ShowRelations(12);
        Assert.False(window.Player1CheckBox.Checked);
        Assert.True(window.Player12CheckBox.Checked);
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
        Assert.True(window.Player1CheckBox.Checked);
        Assert.True(window.Player12CheckBox.Checked);
        Assert.Equal(1, window.ShowCount);
        Assert.Equal(12, window.ShownPlayer);
    }

    [Fact]
    public void ResetChecksClearsBothCheckBands()
    {
        using var dialogs = new DialogScope();
        var window = CreateWindow();
        MarkChecks(window);
        window.Player1Image.CurrentImage = 7;
        window.ResetChecks();
        Assert.Equal(-1, window.SelectedIndex);
        Assert.False(window.Player1CheckBox.Checked);
        Assert.False(window.Player12CheckBox.Checked);
        Assert.False(window.Race1CheckBox.Checked);
        Assert.False(window.Race12CheckBox.Checked);
        Assert.Equal(10, window.Player1CheckBox.Alignment.TopOffset);
        Assert.Equal(10, window.Player12CheckBox.Alignment.TopOffset);
        Assert.Equal(185, window.Race1CheckBox.Alignment.TopOffset);
        Assert.Equal(185, window.Race12CheckBox.Alignment.TopOffset);
        Assert.Equal(7, window.Player1Image.CurrentImage);
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
        Assert.False(window.Player1CheckBox.Checked);
        Assert.False(window.Player12CheckBox.Checked);
        Assert.Equal(10, window.Player1CheckBox.Alignment.TopOffset);
        Assert.Equal(2, window.Player1Image.CurrentImage);
        Assert.Equal(2, window.Player12Image.CurrentImage);
        Assert.Equal(5, window.Race1Image.CurrentImage);
        Assert.Equal(5, window.Race12Image.CurrentImage);

        scope.Map.SeatedPlayer = 0;
        window.Players.Add(0);
        window.HighlightPlayer(0);
        Assert.Equal(0, window.SelectedIndex);
        Assert.True(window.Player1CheckBox.Checked);
        Assert.False(window.Player12CheckBox.Checked);
        Assert.Equal(20, window.Player1CheckBox.Alignment.TopOffset);
        Assert.Equal(20, window.PlayerGlow.Alignment.TopOffset);
        Assert.Equal(2, window.Player1Image.CurrentImage);

        window.Players.Clear();
        for (int i = 1; i <= 12; i++)
        {
            window.Players.Add(i);
        }

        scope.Map.Players.SetNumberOfPlayers(1);
        scope.Map.SeatedPlayer = 0;
        window.HighlightPlayer(12);
        Assert.Equal(11, window.SelectedIndex);
        Assert.False(window.Player1CheckBox.Checked);
        Assert.True(window.Player12CheckBox.Checked);
        Assert.Equal(20, window.Player12CheckBox.Alignment.TopOffset);
        Assert.Equal(10, window.PlayerGlow.Alignment.TopOffset);
        Assert.Same(window.Player12CheckBox, window.FChecks[11]);
        Assert.Same(window.Player1Image, window.FImages[0]);
        Assert.Same(window.Race12Image, window.FImages[23]);
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
        Assert.True(window.Player12CheckBox.Checked);
        Assert.Equal(175, window.Player12CheckBox.Alignment.TopOffset);
        Assert.False(window.Race1CheckBox.Checked);
        Assert.False(window.Race12CheckBox.Checked);
        Assert.Equal(185, window.Race1CheckBox.Alignment.TopOffset);
        Assert.Equal(2, window.Player1Image.CurrentImage);
        Assert.Equal(2, window.Player12Image.CurrentImage);
        Assert.Equal(5, window.Race1Image.CurrentImage);
        Assert.Equal(5, window.Race12Image.CurrentImage);

        window.Races.Add(7);
        window.HighlightRace(7);
        Assert.Equal(20, window.SelectedIndex);
        Assert.True(window.Race1CheckBox.Checked);
        Assert.False(window.Player12CheckBox.Checked);
        Assert.Equal(175, window.Race1CheckBox.Alignment.TopOffset);

        window.Races.Clear();
        for (int i = 0; i <= 11; i++)
        {
            window.Races.Add(i);
        }

        window.HighlightRace(11);
        Assert.Equal(31, window.SelectedIndex);
        Assert.True(window.Race12CheckBox.Checked);
        Assert.False(window.Player12CheckBox.Checked);
        Assert.Equal(175, window.Race12CheckBox.Alignment.TopOffset);
        Assert.Same(window.Race12CheckBox, window.FChecks[23]);
    }
}
