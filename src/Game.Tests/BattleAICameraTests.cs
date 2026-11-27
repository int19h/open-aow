using Aow.Game.Combat;

namespace Aow.Game.Tests;

// Issue 346: after the battle AI moves a unit, the camera centres on it when the unit belongs
// to the seated player. The original compared the unit's combat slot with the seated game
// player number, so the camera followed a unit only when the two numbers happened to match.
[Collection("Engine")]
[EngineState]
public sealed class BattleAICameraTests
{
    private sealed class BattleSetup : IDisposable
    {
        private readonly IDisposable _previousCombatScope = EngineFixture.UseCombatMap(CombatMap.Current);
        private readonly CombatMap _map = new(null, holder: EngineFixture.Engine);
        private readonly TacticalBattle _combat = new(null) { Holder = EngineFixture.Engine };

        public BattleSetup(sbyte seatedSlot)
        {
            // Slot 0 holds game player 2, slot 1 game player 5 and slot 2 game player 7. Seated at
            // slot 0, the original comparison matches the slot 2 unit against game player 2.
            _ = new CombatPlayer(_combat.CombatData.Players) { SideIndex = 0, Player = 2 };
            _ = new CombatPlayer(_combat.CombatData.Players) { SideIndex = 1, Player = 5 };
            _ = new CombatPlayer(_combat.CombatData.Players) { SideIndex = 1, Player = 7 };
            _map.SetCombat(_combat);
            _map.SetSeatedCombatPlayer(seatedSlot);
        }

        public void Dispose()
        {
            _map.Dispose();
            _previousCombatScope.Dispose();
        }

        public TacticalBattleUnit AIUnit(byte slot)
        {
            TacticalBattleUnit unit = new(_combat.CombatData.CombatObjects) { Combat = _combat };
            unit.SetCombatPlayer(slot);
            return unit;
        }
    }

    [Fact]
    public void TheCameraFollowsTheSeatedPlayersUnitAfterAnAIMove()
    {
        using BattleSetup battle = new(seatedSlot: 0);
        Assert.True(BattleAI.CameraFollowsAIUnit(battle.AIUnit(slot: 0)));
    }

    [Fact]
    public void TheCameraIgnoresTheOtherSlotsUnitAfterAnAIMove()
    {
        using BattleSetup battle = new(seatedSlot: 0);
        Assert.False(BattleAI.CameraFollowsAIUnit(battle.AIUnit(slot: 1)));
    }

    [Fact]
    public void TheCameraIgnoresAUnitWhoseSlotEqualsTheSeatedGamePlayerNumber()
    {
        using BattleSetup battle = new(seatedSlot: 0);
        Assert.False(BattleAI.CameraFollowsAIUnit(battle.AIUnit(slot: 2)));
    }

    [Fact]
    public void TheCameraFollowsTheDefendersUnitWhenTheDefenderIsSeated()
    {
        using BattleSetup battle = new(seatedSlot: 1);
        Assert.True(BattleAI.CameraFollowsAIUnit(battle.AIUnit(slot: 1)));
        Assert.False(BattleAI.CameraFollowsAIUnit(battle.AIUnit(slot: 0)));
    }
}
