// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class MindDecayCombatAction : ExclusiveCombatSpellCombatAction
{
    // CombatSpells.pas:1162
    public new const int ClassId = 131787;

    public MindDecayCombatAction(ECustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:1168
    public override void Execute(Battle combat)
    {
        CombatObject source;
        CombatUnit target;
        base.Execute(combat);
        if (Succeeded)
        {
            source = combat.CombatData.FindId(SourceId);
            target = (CombatUnit)combat.CombatData.FindId(TargetId);
            source.SetExperience(source.GetExperience() + target.GetLevel());
            for (int I = 0; I <= 2; I++)
            {
                ((CommandAbility)AoWHSSet.Current.AbilityControl.GetAbility(AbilityRules.CommandAbilityIds[I])).ResetCommandedUnits(target);
            }

            for (int I = 0; I <= 2; I++)
            {
                ((CommandAbility)AoWHSSet.Current.AbilityControl.GetAbility(AbilityRules.CommandAbilityIds[I])).Uncommand(target);
            }

            AoWHSSet.Current.AbilityControl.GetAbility(131).Expand(target.Troop);
            target.SetCombatPlayer((byte)source.CombatPlayer);
        }
    }

}
