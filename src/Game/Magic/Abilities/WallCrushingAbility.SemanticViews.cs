
using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class WallCrushingAbility
{
    // AoWAb.PAS:2081
    public override bool fcExecuteCombatCommand(CombatObject source, FastCombatCommand command)
    {
        var target = command.FTarget;
        if (target.GetEnabled())
        {
            var action = CreateDamageCA(source, target, 6, 6, Set<DamageType>.Of(DamageType.Wall));
            source.FCombat.ExecuteCombatAction(action);
            action.Release();
        }
        return true;
    }
}
