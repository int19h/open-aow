// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class EntangleSpellCombatAction : ExclusiveCombatSpellCombatAction
{
    // CombatSpells.pas:1328
    public new const int ClassID = 131784;

    public EntangleSpellCombatAction(ECustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:1334
    public override void Execute(Battle Combat)
    {
        CombatObject Source;
        CombatUnit Target;
        UnitEnchantmentAbilityData Data;
        base.Execute(Combat);
        if (FSucceeded)
        {
            Source = Combat.FCombatData.FindID(FSourceID);
            Target = (CombatUnit)Combat.FCombatData.FindID(FTargetID);
            AoWHSSet.Current.FAbilityControl.GetAbility(94).Expand(Target.FUnit);
            Data = (UnitEnchantmentAbilityData)Target.FUnit.GetAbilityData(94);
            if (Data != null)
            {
                Data.FEnchantmentPlayerIndex = Source.FCombatPlayer;
            }
        }
    }

}
