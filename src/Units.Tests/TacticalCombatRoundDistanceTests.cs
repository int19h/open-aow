using Aow.Game.Combat;

namespace Aow.Units.Tests;

// The tactical AI's damage estimates (TSpellCastingAbility.tcGetDamageValueEx into
// TCombatSpell.fcGetDamageValueEx) ask the combat for its round distance and wall situation. The
// original casts the combat to TFastCombat without a check, and the helpers read only the combat
// data, so a tactical combat answers them too. A playtest attack on a city threw an invalid cast.
[Collection("Engine")]
[EngineState]
public sealed class TacticalCombatRoundDistanceTests
{
    private static TacticalBattle Build(byte round, byte sideIndex)
    {
        var combat = new TacticalBattle(null!);
        _ = new CombatPlayer(combat.FCombatData.FPlayers) { FSideIndex = sideIndex };
        combat.FCombatData.FRound = round;
        combat.FCombatData.FCurrentPlayer = 0;
        return combat;
    }

    [Theory]
    [InlineData(3, 0, 5)]
    [InlineData(3, 1, 4)]
    [InlineData(1, 0, 1)]
    public void ATacticalCombatAnswersTheRoundDistance(byte round, byte sideIndex, int distance)
    {
        Battle combat = Build(round, sideIndex);
        Assert.Equal(distance, combat.GetRoundDistance());
    }

    [Fact]
    public void ATacticalCombatWithoutAWallHasNoWallSituation()
    {
        var combat = Build(1, 0);
        var source = new TacticalCombatUnit(combat.FCombatData.FCombatObjects) { FCombat = combat, FPosition = 0x12 };
        var target = new TacticalCombatUnit(combat.FCombatData.FCombatObjects) { FCombat = combat, FPosition = 0x34 };
        Assert.Equal(WallSituation.None, combat.GetWallSituation(source, target));
    }
}
