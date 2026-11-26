using Aow.Game.Session;
using Aow.Headless;

namespace Aow.Game.Tests;

/// <summary>
/// Issue 344, the host options of the leader-death and campaign transfer inputs. The leader
/// attack names a player and a unit, as leader_target.py prints them. The campaign seat type is a
/// stored CPU player type (Player.PAS TPlayerType), never Human, Independent or Free. The rule
/// that the seat type needs a campaign transfer or a campaign victory sits in Program and runs in
/// the oracle's campaign_transfer and campaign_victory scenarios, because a Program test needs a
/// full host run.
/// </summary>
public sealed class HeadlessDirectedInputTests
{
    [Fact]
    public void TheInputsAreOffUnlessAsked()
    {
        HostRequest request = HostRequest.Parse(["--load", "save.oasg", "--turns", "1", "--out", "out"]);

        Assert.Null(request.LeaderAttack);
        Assert.Null(request.CampaignSeatType);
    }

    [Fact]
    public void TheLeaderAttackNamesAPlayerAndAUnit()
    {
        HostRequest request = HostRequest.Parse(["--load", "save.oasg", "--turns", "1", "--out", "out", "--leader-attack", "1:219"]);

        Assert.Equal(new LeaderAttackTarget(Player: 1, UnitId: 219), request.LeaderAttack);
    }

    [Theory]
    [InlineData("1")]
    [InlineData("0:5")]
    [InlineData("1:0")]
    [InlineData("1:-5")]
    [InlineData("a:b")]
    [InlineData("1:2:3")]
    public void AMalformedLeaderAttackIsRefused(string text)
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => LeaderAttackTarget.Parse(text));

        Assert.Contains("PLAYER:UNIT", error.Message);
    }

    [Theory]
    [InlineData("1", PlayerType.Cpu1)]
    [InlineData("2", PlayerType.Cpu2)]
    [InlineData("3", PlayerType.Cpu3)]
    [InlineData("6", PlayerType.Cpu4)]
    [InlineData("7", PlayerType.Cpu5)]
    public void TheSeatTypeIsAStoredCpuType(string text, PlayerType expected)
    {
        HostRequest request = HostRequest.Parse(["--load", "save.oasg", "--turns", "1", "--out", "out", "--campaign-seat-type", text]);

        Assert.Equal(expected, request.CampaignSeatType);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("4")]
    [InlineData("5")]
    [InlineData("8")]
    public void AHumanOrNonPlayingSeatTypeIsRefused(string text)
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => HostRequest.Parse(["--load", "save.oasg", "--turns", "1", "--out", "out", "--campaign-seat-type", text]));

        Assert.Contains("CPU player type", error.Message);
    }

    [Fact]
    public void AnOriginalCampaignSaveIsRecognizedByItsRootTable()
    {
        // Engine.pas StoreInFile: one short root entry, id 0 at offset 0, then the class id.
        Assert.Equal(SaveKind.Campaign, SaveDocument.Kind([0x01, 0x00, 0x00, 0x00, 0x50, 0x22, 0x00, 0x82]));
        Assert.Equal(SaveKind.WorldMap, SaveDocument.Kind([0x43, 0x46, 0x53, 0x00, 0x02, 0x78, 0x9C, 0x00]));
        Assert.Equal(SaveKind.WorldMap, SaveDocument.Kind([0x10, 0x00, 0x00, 0x00, 0x48, 0x53, 0x4D, 0x00]));
    }
}
