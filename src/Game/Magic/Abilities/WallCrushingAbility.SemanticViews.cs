
using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class WallCrushingAbility
{
    // AoWAb.PAS:2081
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        var target = command.Target;
        if (target.GetEnabled())
        {
            var action = CreateDamageCombatAction(source, target, 6, 6, Set<DamageType>.Of(DamageType.Wall));
            source.Combat.ExecuteCombatAction(action);
            action.Release();
        }
        return true;
    }
}
