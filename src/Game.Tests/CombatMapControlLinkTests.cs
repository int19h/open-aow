using Aow.Game.Combat;
using Aow.Game.Session;
using Aow.Game.UI.Grids;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

/// <summary>
/// The combat map control links its relays once per battle and releases them when the battle
/// ends (issue 326, review risk 1). The original never released them at the end of a battle, so
/// each battle added one more copy to the player events (AoWHSMap.PAS:2891, CMapCtrl.pas:128).
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class CombatMapControlLinkTests
{
    [Fact]
    public void TwoBattlesLeaveOneCombatRelayOnEachPlayerEvent()
    {
        using IDisposable mapScope = EngineFixture.UseMap(StrategicMap.Current);
        using IDisposable combatScope = EngineFixture.UseCombatMap(CombatMap.Current);
        using StrategicMap world = new(null, holder: EngineFixture.Engine);
        world.Players.SetNumberOfPlayers(2);
        EngineFixture.SetMap(world);
        using Component owner = new(null);
        using CombatMapControl control = new(owner);
        for (int battle = 0; battle < 2; battle++)
        {
            CombatMap combat = new(null, holder: EngineFixture.Engine);
            EngineFixture.SetCombatMap(combat);
            world.CombatMap = combat;
            control.LinkToHsm(combat, world: world);
            control.LinkToHsm(combat, world: world);
            Assert.Equal(1, RelayCount(world));
            Assert.Equal(1, EventHandlers.Count(combat, eventName: nameof(CombatMap.TurnChanged)));
            world.ReleaseCombatMap();
            EngineFixture.SetCombatMap(null);
            Assert.Equal(0, RelayCount(world));
        }

        Assert.Equal(0, EventHandlers.Count(world, eventName: nameof(StrategicMap.CombatMapDestroyed)));
    }

    // The most copies of the combat relays any player event or the event log holds.
    private static int RelayCount(StrategicMap world)
    {
        int most = EventHandlers.Count(world, eventName: nameof(StrategicMap.ExecutingEventLog));
        for (int i = 0; i < world.Players.Count; i++)
        {
            Player player = world.Players.GetPlayers(i)!;
            foreach (string eventName in new[] { nameof(Player.GemsChanged), nameof(Player.IncomeChanged), nameof(Player.UpkeepChanged), nameof(Player.LogbookChanged) })
            {
                most = Math.Max(most, EventHandlers.Count(player, eventName: eventName));
            }
        }

        return most;
    }
}
