#nullable disable warnings
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
        private readonly IDisposable previousScope = EngineFixture.UseMap(StrategicMap.Current);

        public MapScope()
        {
            Map = new StrategicMap(null, holder: EngineFixture.Engine);
            EngineFixture.SetMap(Map);
        }

        public StrategicMap Map { get; }

        public void Dispose() => previousScope.Dispose();
    }

    private sealed class RecordingRelWindow(UIContext ui) : RelationWindow(ui)
    {
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
        private readonly SoundSystem? previousSound = AudioHost.SoundSystem;

        public DialogScope()
        {
            AudioHost.SoundSystem = null;
            UI.Add(new General(UI)).Manager = new WindowManager(null);
        }

        public UIContext UI { get; } = new(EngineFixture.Engine);

        public void Dispose() => AudioHost.SoundSystem = previousSound;
    }

    private static RecordingRelWindow CreateWindow(UIContext ui)
    {
        RecordingRelWindow window = new(ui)
        {
            Player1Image = new Image(null),
            Player2Image = new Image(null),
            Player3Image = new Image(null),
            Player4Image = new Image(null),
            Player5Image = new Image(null),
            Player6Image = new Image(null),
            Player7Image = new Image(null),
            Player8Image = new Image(null),
            Player9Image = new Image(null),
            Player10Image = new Image(null),
            Player11Image = new Image(null),
            Player12Image = new Image(null),
            Race1Image = new Image(null),
            Race2Image = new Image(null),
            Race3Image = new Image(null),
            Race4Image = new Image(null),
            Race5Image = new Image(null),
            Race6Image = new Image(null),
            Race7Image = new Image(null),
            Race8Image = new Image(null),
            Race9Image = new Image(null),
            Race10Image = new Image(null),
            Race11Image = new Image(null),
            Race12Image = new Image(null),
            Player1CheckBox = new CheckBox(null),
            Player2CheckBox = new CheckBox(null),
            Player3CheckBox = new CheckBox(null),
            Player4CheckBox = new CheckBox(null),
            Player5CheckBox = new CheckBox(null),
            Player6CheckBox = new CheckBox(null),
            Player7CheckBox = new CheckBox(null),
            Player8CheckBox = new CheckBox(null),
            Player9CheckBox = new CheckBox(null),
            Player10CheckBox = new CheckBox(null),
            Player11CheckBox = new CheckBox(null),
            Player12CheckBox = new CheckBox(null),
            Race1CheckBox = new CheckBox(null),
            Race2CheckBox = new CheckBox(null),
            Race3CheckBox = new CheckBox(null),
            Race4CheckBox = new CheckBox(null),
            Race5CheckBox = new CheckBox(null),
            Race6CheckBox = new CheckBox(null),
            Race7CheckBox = new CheckBox(null),
            Race8CheckBox = new CheckBox(null),
            Race9CheckBox = new CheckBox(null),
            Race10CheckBox = new CheckBox(null),
            Race11CheckBox = new CheckBox(null),
            Race12CheckBox = new CheckBox(null),
            MainWindow = new Window(null),
            RelationPanel = new Panel(null),
            PlayerGlow = new Image(null),
            NewActionPanel = new Panel(null),
            IncomingPanel = new Panel(null),
            MessageEditPanel = new Panel(null),
            SendBribePanel = new Panel(null),
            ReceivedMessagePanel = new Panel(null),
            IncomingInfoPanel = new Panel(null),
            ActionsList = new ListBox<DiplomaticAction>(null),
            SendMessageMemo = new Memo(null),
            IncomingMemo = new Memo(null),
            SelectActionButton = new Button(null)
        };
        window.RelationWindowCreate();
        return window;
    }

    private static void MarkChecks(RelationWindow window)
    {
        foreach (CheckBox check in window.FChecks)
        {
            check.Checked = true;
            check.Alignment.TopOffset = 99;
        }
    }

    [Fact]
    public void FillPlayersUsesThePlayerCheckBand()
    {
        using MapScope scope = new();
        using DialogScope dialogs = new();
        RecordingRelWindow window = CreateWindow(dialogs.UI);
        window.Race1CheckBox.Visible = false;
        window.FillPlayers();
        Assert.False(window.Player1CheckBox.Visible);
        Assert.False(window.Player12CheckBox.Visible);
        Assert.False(window.Race1CheckBox.Visible);

        scope.Map.Players.SetNumberOfPlayers(12);
        scope.Map.SeatedPlayer = 1;
        Player? seated = scope.Map.Players.GetPlayers(1);
        for (int i = 1; i <= 12; i++)
        {
            Player? player = scope.Map.Players.GetPlayers(i);
            player.Status = PlayerStatus.Playing;
            player.Name = $"P{i}";
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
        using MapScope scope = new();
        using DialogScope dialogs = new();
        RecordingRelWindow window = CreateWindow(dialogs.UI);
        window.Player1Image.Checked = true;
        window.Race12Image.Checked = true;

        window.ShowRelations(0);
        Assert.False(window.Player1CheckBox.Checked);
        Assert.False(window.Player12CheckBox.Checked);
        Assert.True(window.Race12Image.Checked);
        Assert.Equal(0, window.ShowCount);
        Assert.False(window.RelationPanel.Aligned);
        Assert.False(window.MainWindow.IsDrawn);

        window.ShowRelations(-23);
        Assert.False(window.Player1CheckBox.Checked);
        Assert.True(window.Player1Image.Checked);
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
        Player? seated = scope.Map.Players.GetPlayers(12);
        for (int i = 1; i <= 12; i++)
        {
            Player? player = scope.Map.Players.GetPlayers(i);
            player.Status = PlayerStatus.Playing;
            player.Name = $"S{i}";
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
        using DialogScope dialogs = new();
        RecordingRelWindow window = CreateWindow(dialogs.UI);
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
        using MapScope scope = new();
        using DialogScope dialogs = new();
        RecordingRelWindow window = CreateWindow(dialogs.UI);
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
        _ = window.Players.Add(0);
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
            _ = window.Players.Add(i);
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
        using DialogScope dialogs = new();
        RecordingRelWindow window = CreateWindow(dialogs.UI);
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

        _ = window.Races.Add(7);
        window.HighlightRace(7);
        Assert.Equal(20, window.SelectedIndex);
        Assert.True(window.Race1CheckBox.Checked);
        Assert.False(window.Player12CheckBox.Checked);
        Assert.Equal(175, window.Race1CheckBox.Alignment.TopOffset);

        window.Races.Clear();
        for (int i = 0; i <= 11; i++)
        {
            _ = window.Races.Add(i);
        }

        window.HighlightRace(11);
        Assert.Equal(31, window.SelectedIndex);
        Assert.True(window.Race12CheckBox.Checked);
        Assert.False(window.Player12CheckBox.Checked);
        Assert.Equal(175, window.Race12CheckBox.Alignment.TopOffset);
        Assert.Same(window.Race12CheckBox, window.FChecks[23]);
    }
}
