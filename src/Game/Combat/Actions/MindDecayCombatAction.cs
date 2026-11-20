// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public class MindDecayCombatAction(CustomNode owner) : ExclusiveCombatSpellCombatAction(owner)
{
    // CombatSpells.pas:1162
    public new const int ClassId = 131_787;

    // CombatSpells.pas:1168
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        if (Succeeded)
        {
            CombatObject source = combat.CombatData.FindId(SourceId);
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(TargetId);
            source.SetExperience(source.Experience + target.Level);
            for (int i = 0; i <= 2; i++)
            {
                GamePack.Current.AbilityControl.GetAbility<CommandAbility>(AbilityRules.CommandAbilityIds[i]).ResetCommandedUnits(target);
            }

            for (int i = 0; i <= 2; i++)
            {
                GamePack.Current.AbilityControl.GetAbility<CommandAbility>(AbilityRules.CommandAbilityIds[i]).Uncommand(target);
            }

            _ = GamePack.Current.AbilityControl[131].Expand(target.Troop);
            target.SetCombatPlayer((byte)source.CombatPlayer);
        }
    }
}
