using Aow.Game.Host;
using Aow.Game.Session;
using Aow.Headless;

namespace Aow.Game.Tests;

/// <summary>
/// Issue 344, the campaign victory run. The host hook runs first in Player.SetGameOverStatus, before
/// the game tests the status or the player's type (Player.PAS:425), so a campaign seat that plays as
/// a CPU player can turn human before the campaign branch reads its type. HeadlessSeatTests pins the
/// second hook, BeforeStartLoadedMap, and the seat conversion and its turn back to human. The run of
/// FinishScenario and Retrieve needs a loaded campaign map, so the oracle's campaign_victory scenario
/// covers it, and so do the Program rules on the options, which need a full host run.
/// </summary>
[Collection("Engine")]
public sealed class CampaignVictoryHookTests
{
    [Fact]
    public void TheHookSeesEveryCallFirstEvenWithoutAChange()
    {
        Player player = new(null) { Holder = EngineFixture.Engine };
        List<(Player Player, GameOverStatus Status)> calls = [];
        HeadlessHooks.BeforeSetGameOverStatus = (target, status) => calls.Add((target, status));
        try
        {
            // The status stays None, so the game changes nothing after the hook.
            player.SetGameOverStatus(GameOverStatus.None);
        }
        finally
        {
            HeadlessHooks.BeforeSetGameOverStatus = null;
        }

        (Player called, GameOverStatus requested) = Assert.Single(calls);
        Assert.Same(player, called);
        Assert.Equal(GameOverStatus.None, requested);
        Assert.Equal(GameOverStatus.None, player.GameOverStatus);
    }

    [Fact]
    public void TheVictoryRunIsOffUnlessAsked()
    {
        Assert.False(HostRequest.Parse(["--load", "save.acg", "--turns", "1", "--out", "out"]).CampaignVictory);
        Assert.True(HostRequest.Parse(["--load", "save.acg", "--turns", "1", "--out", "out", "--campaign-victory"]).CampaignVictory);
    }

    [Fact]
    public void TheLeaderAttackCanMoveTheWholeArmy()
    {
        Assert.Equal(new LeaderAttackTarget(Player: 1, UnitId: 64, WholeArmy: true), LeaderAttackTarget.Parse("1:64:army"));
        Assert.False(LeaderAttackTarget.Parse("1:64").WholeArmy);
        ArgumentException error = Assert.Throws<ArgumentException>(() => LeaderAttackTarget.Parse("1:64:armies"));
        Assert.Contains("PLAYER:UNIT:army", error.Message);
    }
}
