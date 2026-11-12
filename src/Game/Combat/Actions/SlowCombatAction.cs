// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

namespace Aow.Game.Combat.Actions;

public partial class SlowCombatAction : ExclusiveCombatSpellCombatAction
{
    // CombatSpells.pas:1257
    public new const int ClassId = 131792;

    public SlowCombatAction(ECustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:1263
    public override void Execute(Battle combat)
    {
        CombatUnit target;
        CombatObject source;
        UnitEnchantment enchantment;
        base.Execute(combat);
        if (Succeeded)
        {
            target = (CombatUnit)combat.CombatData.FindId(TargetId);
            source = combat.CombatData.FindId(SourceId);
            if (target.Troop.ExpandAbility(132))
            {
                enchantment = ((UnitEnchantmentAbilityData)target.Troop.GetAbilityData(132)).Enchantment;
                enchantment.SetUpkeep(0);
                enchantment.SetSource(source.GetPlayer());
            }
        }
    }

}
