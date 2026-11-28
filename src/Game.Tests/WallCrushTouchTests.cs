using Aow.Game.Combat;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Tests;

/// <summary>
/// Ability 117 on a tactical map reads TCAttack and TCDamage (AoWTC.pas:566, CombatTE.pas:708).
/// The image stores attack (6, 10) and damage (6, 1). Ability 117 deals the original's wall damage.
/// A 7 point wooden wall falls in two hits of the seed 16 roll.
/// </summary>
public sealed class WallCrushTouchTests
{
    [Fact]
    public void TacticalBattleDamageTablesMatchTheImageAndSeed16Deals4()
    {
        Assert.Equal(6, TacticalTables.TacticalBattleAttack[0]);
        Assert.Equal(10, TacticalTables.TacticalBattleAttack[1]);
        Assert.Equal(6, TacticalTables.TacticalBattleDamage[0]);
        Assert.Equal(1, TacticalTables.TacticalBattleDamage[1]);

        int seed = DelphiRandom.RandSeed;
        try
        {
            // CityWall.GetDefense is -2 (CityWall.pas:197). Seed 16 rolls 10, the same roll the fast wall crush uses.
            DelphiRandom.RandSeed = 16;
            int defense = -2;
            int damage = AbilityRules.ExecuteDamageRole(TacticalTables.TacticalBattleDamage[0], attackDefense: TacticalTables.TacticalBattleAttack[0] - defense);
            // Seed 16 rolls 10. Attack 6 against defense -2 deals 4, and a second hit of 4 finishes a 7 point wooden wall.
            Assert.Equal(4, damage);
        }
        finally
        {
            DelphiRandom.RandSeed = seed;
        }
    }
}
