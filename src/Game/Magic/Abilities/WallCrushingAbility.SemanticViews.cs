#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

namespace Aow.Game.Magic.Abilities;

public partial class WallCrushingAbility
{
    // AoWAb.PAS:2081
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command)
    {
        CombatObject target = command.Target;
        if (target.GetEnabled())
        {
            DamageCombatAction action = CreateDamageCombatAction(source, target, attack: 6, damage: 6, damageTypes: DamageTypes.Wall);
            source.Combat.ExecuteCombatAction(action);
            _ = action.Release();
        }
        return true;
    }
}
