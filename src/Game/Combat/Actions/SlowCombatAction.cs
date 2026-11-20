// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

namespace Aow.Game.Combat.Actions;

public class SlowCombatAction(CustomNode owner) : ExclusiveCombatSpellCombatAction(owner)
{
    // CombatSpells.pas:1257
    public new const int ClassId = 131792;

    // CombatSpells.pas:1263
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        if (Succeeded)
        {
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(TargetId);
            CombatObject source = combat.CombatData.FindId(SourceId);
            if (target.Troop.ExpandAbility(132))
            {
                UnitEnchantment enchantment = target.Troop.GetAbilityData<UnitEnchantmentAbilityData>(132).Enchantment;
                enchantment.SetUpkeep(0);
                enchantment.SetSource(source.GetPlayer());
            }
        }
    }
}
