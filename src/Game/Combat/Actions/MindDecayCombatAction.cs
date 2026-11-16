// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class MindDecayCombatAction : ExclusiveCombatSpellCombatAction
{
    // CombatSpells.pas:1162
    public new const int ClassId = 131787;

    public MindDecayCombatAction(CustomNode owner) : base(owner)
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
            target = combat.CombatData.FindId<CombatUnit>(TargetId);
            source.SetExperience(source.GetExperience() + target.GetLevel());
            for (int i = 0; i <= 2; i++)
            {
                GamePack.Current.AbilityControl.GetAbility(AbilityRules.CommandAbilityIds[i]).ResetCommandedUnits(target);
            }

            for (int i = 0; i <= 2; i++)
            {
                GamePack.Current.AbilityControl.GetAbility(AbilityRules.CommandAbilityIds[i]).Uncommand(target);
            }

            GamePack.Current.AbilityControl.GetAbility(131).Expand(target.Troop);
            target.SetCombatPlayer((byte)source.CombatPlayer);
        }
    }

}
