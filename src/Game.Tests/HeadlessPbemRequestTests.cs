using Aow.Game.Session;
using Aow.Headless;

namespace Aow.Game.Tests;

/// <summary>
/// Issue 344 package 4, the host side of a play by email turn with rule-known outcomes: the
/// options, the refusals before the activation, and the width of the map view around the side
/// windows. The turn itself (the end of the seat's turn, the turn file written in place and the
/// send dialog for the next seat) runs in the oracle's pbem scenario, because it needs a full
/// host run on a turn file.
/// </summary>
public sealed class HeadlessPbemRequestTests
{
    [Fact]
    public void ARunIsNoPbemTurnUnlessAsked()
    {
        HostRequest request = HostRequest.Parse(["--load", "save.oasg", "--turns", "1", "--out", "out"]);

        Assert.False(request.Pbem);
        Assert.Null(request.PbemAttack);
    }

    [Fact]
    public void ThePbemOptionTakesNoValue()
    {
        HostRequest request = HostRequest.Parse(["--load", "turn.asg", "--pbem", "--turns", "0", "--out", "out"]);

        Assert.True(request.Pbem);
        Assert.Equal(0, request.Turns);
    }

    [Fact]
    public void TheOrderNamesAUnitAndATarget()
    {
        HostRequest request = HostRequest.Parse(["--load", "turn.asg", "--pbem", "--pbem-attack", "203:243", "--turns", "0", "--out", "out"]);

        Assert.Equal(new PbemOrder(UnitId: 203, TargetUnitId: 243), request.PbemAttack);
    }

    [Theory]
    [InlineData("203")]
    [InlineData("0:243")]
    [InlineData("203:0")]
    [InlineData("203:203")]
    [InlineData("a:b")]
    [InlineData("1:2:3")]
    public void AMalformedOrderIsRefused(string text)
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => PbemOrder.Parse(text));

        Assert.Contains("UNIT:TARGET", error.Message);
    }

    [Fact]
    public void AnEmailGameInTurnsWithAHumanCurrentPlayerIsAccepted()
    {
        Assert.Null(PbemTurn.Refusal(PlayerConnectionType.Email, mode: PlayMode.Turn, control: null, current: PlayerType.Human));
        Assert.Null(PbemTurn.Refusal(PlayerConnectionType.Email, mode: PlayMode.None, control: typeof(PbemPlayerControl), current: PlayerType.Human));
    }

    [Theory]
    [InlineData(PlayerConnectionType.Local)]
    [InlineData(PlayerConnectionType.Network)]
    public void AGameThatIsNotAnEmailGameIsRefused(PlayerConnectionType connection) => Assert.Equal("--pbem needs an email game", PbemTurn.Refusal(connection, mode: PlayMode.Turn, control: null, current: PlayerType.Human));

    [Fact]
    public void ASimultaneousGameIsRefused() => Assert.Equal("--pbem needs a game played in turns", PbemTurn.Refusal(PlayerConnectionType.Email, mode: PlayMode.Simultaneous, control: null, current: PlayerType.Human));

    [Fact]
    public void AnotherPlayerControlIsRefused() => Assert.Equal("--pbem needs the play by email player control", PbemTurn.Refusal(PlayerConnectionType.Email, mode: PlayMode.Turn, control: typeof(TurnPlayerControl), current: PlayerType.Human));

    [Theory]
    [InlineData(PlayerType.Cpu3)]
    [InlineData(PlayerType.Independent)]
    [InlineData(null)]
    public void ACurrentPlayerThatIsNotHumanIsRefused(PlayerType? current) => Assert.Equal("--pbem needs a human current player", PbemTurn.Refusal(PlayerConnectionType.Email, mode: PlayMode.Turn, control: null, current: current));

    [Fact]
    public void TheViewNarrowsOnTheSecondFrameAfterAHumanIsSeated()
    {
        SideWindowsLayout layout = new();
        int[] widths = new int[6];
        for (int frame = 1; frame <= widths.Length; frame++)
        {
            layout.Observe(frame, isHumanSeated: frame >= 2);
            widths[frame - 1] = layout.Width(1_600);
        }

        Assert.Equal([1_696, 1_696, 1_696, 1_600, 1_600, 1_600], widths);
    }

    [Fact]
    public void ASeatThatLeavesBeforeTheLayoutKeepsTheViewWide()
    {
        SideWindowsLayout layout = new();
        layout.Observe(1, isHumanSeated: true);
        layout.Observe(2, isHumanSeated: false);
        layout.Observe(3, isHumanSeated: false);

        Assert.Equal(1_696, layout.Width(1_600));
        layout.Observe(4, isHumanSeated: true);
        layout.Observe(5, isHumanSeated: true);
        Assert.Equal(1_696, layout.Width(1_600));
        layout.Observe(6, isHumanSeated: true);
        Assert.Equal(1_600, layout.Width(1_600));
    }

    [Fact]
    public void TheViewStaysNarrowAfterTheSeatLeaves()
    {
        SideWindowsLayout layout = new();
        for (int frame = 1; frame <= 3; frame++)
        {
            layout.Observe(frame, isHumanSeated: true);
        }

        layout.Observe(4, isHumanSeated: false);

        Assert.Equal(1_600, layout.Width(1_600));
    }

    [Fact]
    public void TheViewStaysWideWithoutAHumanSeat()
    {
        SideWindowsLayout layout = new();
        layout.Observe(1, isHumanSeated: false);

        Assert.Equal(1_696, layout.Width(1_600));
    }
}
