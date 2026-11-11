// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class MindDecayCombatAction : ExclusiveCombatSpellCombatAction
{
    // CombatSpells.pas:1162
    public new const int ClassID = 131787;

    public MindDecayCombatAction(ECustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:1168
    public override void Execute(Battle Combat)
    {
        CombatObject Source;
        CombatUnit Target;
        base.Execute(Combat);
        if (FSucceeded)
        {
            Source = Combat.FCombatData.FindID(FSourceID);
            Target = (CombatUnit)Combat.FCombatData.FindID(FTargetID);
            Source.SetExperience(Source.GetExperience() + Target.GetLevel());
            for (int I = 0; I <= 2; I++)
            {
                ((CommandAbility)AoWHSSet.Current.FAbilityControl.GetAbility(AbilityRules.CommandAbilityIDs[I])).ResetCommandedUnits(Target);
            }

            for (int I = 0; I <= 2; I++)
            {
                ((CommandAbility)AoWHSSet.Current.FAbilityControl.GetAbility(AbilityRules.CommandAbilityIDs[I])).Uncommand(Target);
            }

            AoWHSSet.Current.FAbilityControl.GetAbility(131).Expand(Target.FUnit);
            Target.SetCombatPlayer((byte)Source.FCombatPlayer);
        }
    }

}
