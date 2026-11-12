using Aow.Game.Combat;

namespace Aow.Game.Tests;

// The tactical AI's damage estimates (TSpellCastingAbility.tcGetDamageValueEx into
// TCombatSpell.fcGetDamageValueEx) ask the combat for its round distance and wall situation. The
// original casts the combat to TFastCombat without a check, and the helpers read only the combat
// data, so a tactical combat answers them too. A playtest attack on a city threw an invalid cast.
[Collection("Engine")]
[EngineState]
public sealed class TacticalBattleRoundDistanceTests
{
    private static TacticalBattle Build(byte round, byte sideIndex)
    {
        var combat = new TacticalBattle(null!);
        _ = new CombatPlayer(combat.CombatData.Players) { SideIndex = sideIndex };
        combat.CombatData.Round = round;
        combat.CombatData.CurrentPlayer = 0;
        return combat;
    }

    [Theory]
    [InlineData(3, 0, 5)]
    [InlineData(3, 1, 4)]
    [InlineData(1, 0, 1)]
    public void ATacticalBattleAnswersTheRoundDistance(byte round, byte sideIndex, int distance)
    {
        Battle combat = Build(round, sideIndex);
        Assert.Equal(distance, combat.GetRoundDistance());
    }

    [Fact]
    public void ATacticalBattleWithoutAWallHasNoWallSituation()
    {
        var combat = Build(1, 0);
        var source = new TacticalBattleUnit(combat.CombatData.CombatObjects) { Combat = combat, Position = 0x12 };
        var target = new TacticalBattleUnit(combat.CombatData.CombatObjects) { Combat = combat, Position = 0x34 };
        Assert.Equal(WallSituation.None, combat.GetWallSituation(source, target));
    }
}
