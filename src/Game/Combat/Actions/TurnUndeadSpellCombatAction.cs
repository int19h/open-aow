// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public class TurnUndeadSpellCombatAction(CustomNode owner) : CombatSpellCombatAction(owner)
{
    // CombatSpells.pas:861
    public new const int ClassId = 131788;

    // CombatSpells.pas:867
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        if (Damage != 0)
        {
            CombatObject source = combat.CombatData.FindId(SourceId);
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(TargetId);
            if (!target.Status.Contains(CombatObjectStatusElement.Destroyed))
            {
                _ = GamePack.Current.AbilityControl.GetAbility(34).Expand(target.Troop);
                UnitEnchantmentAbilityData data = target.Troop.GetAbilityData<UnitEnchantmentAbilityData>(34);
                data?.EnchantmentPlayerIndex = source.CombatPlayer;
            }
        }
    }
}
