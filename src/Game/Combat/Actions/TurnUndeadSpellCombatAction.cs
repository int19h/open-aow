// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class TurnUndeadSpellCombatAction : CombatSpellCombatAction
{
    // CombatSpells.pas:861
    public new const int ClassID = 131788;

    public TurnUndeadSpellCombatAction(ECustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:867
    public override void Execute(Battle Combat)
    {
        CombatObject Source;
        CombatUnit Target;
        UnitEnchantmentAbilityData Data;
        base.Execute(Combat);
        if (FDamage != 0)
        {
            Source = Combat.FCombatData.FindID(FSourceID);
            Target = (CombatUnit)Combat.FCombatData.FindID(FTargetID);
            if (!Target.FStatus.Contains(CombatObjectStatusElement.Destroyed))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(34).Expand(Target.FUnit);
                Data = (UnitEnchantmentAbilityData)Target.FUnit.GetAbilityData(34);
                if (Data != null)
                {
                    Data.FEnchantmentPlayerIndex = Source.FCombatPlayer;
                }
            }
        }
    }

}
