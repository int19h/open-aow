// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public class MindDecayCombatAction(CustomNode owner) : ExclusiveCombatSpellCombatAction(owner)
{
    // CombatSpells.pas:1162
    public new const int ClassId = 131787;

    // CombatSpells.pas:1168
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        if (Succeeded)
        {
            CombatObject source = combat.CombatData.FindId(SourceId);
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(TargetId);
            source.SetExperience(source.GetExperience() + target.GetLevel());
            for (int i = 0; i <= 2; i++)
            {
                GamePack.Current.AbilityControl.GetAbility<CommandAbility>(AbilityRules.CommandAbilityIds[i]).ResetCommandedUnits(target);
            }

            for (int i = 0; i <= 2; i++)
            {
                GamePack.Current.AbilityControl.GetAbility<CommandAbility>(AbilityRules.CommandAbilityIds[i]).Uncommand(target);
            }

            _ = GamePack.Current.AbilityControl.GetAbility(131).Expand(target.Troop);
            target.SetCombatPlayer((byte)source.CombatPlayer);
        }
    }
}
