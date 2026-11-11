// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

namespace Aow.Game.Combat.Actions;

public partial class SlowCombatAction : ExclusiveCombatSpellCombatAction
{
    // CombatSpells.pas:1257
    public new const int ClassID = 131792;

    public SlowCombatAction(ECustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:1263
    public override void Execute(Battle Combat)
    {
        CombatUnit Target;
        CombatObject Source;
        UnitEnchantment Enchantment;
        base.Execute(Combat);
        if (FSucceeded)
        {
            Target = (CombatUnit)Combat.FCombatData.FindID(FTargetID);
            Source = Combat.FCombatData.FindID(FSourceID);
            if (Target.FUnit.ExpandAbility(132))
            {
                Enchantment = ((UnitEnchantmentAbilityData)Target.FUnit.GetAbilityData(132)).FEnchantment;
                Enchantment.SetUpkeep(0);
                Enchantment.SetSource(Source.GetPlayer());
            }
        }
    }

}
