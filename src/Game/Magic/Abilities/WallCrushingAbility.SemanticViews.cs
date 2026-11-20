
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class WallCrushingAbility
{
    // AoWAb.PAS:2081
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command)
    {
        CombatObject target = command.Target;
        if (target.GetEnabled())
        {
            DamageCombatAction action = CreateDamageCombatAction(source, target, attack: 6, damage: 6, damageTypes: Set<DamageType>.Of(DamageType.Wall));
            source.Combat.ExecuteCombatAction(action);
            _ = action.Release();
        }
        return true;
    }
}
