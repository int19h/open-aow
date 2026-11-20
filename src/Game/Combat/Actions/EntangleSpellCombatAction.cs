// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public class EntangleSpellCombatAction(CustomNode owner) : ExclusiveCombatSpellCombatAction(owner)
{
    // CombatSpells.pas:1328
    public new const int ClassId = 131_784;

    // CombatSpells.pas:1334
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        if (Succeeded)
        {
            CombatObject source = combat.CombatData.FindId(SourceId);
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(TargetId);
            _ = GamePack.Current.AbilityControl[94].Expand(target.Troop);
            UnitEnchantmentAbilityData data = target.Troop.GetAbilityData<UnitEnchantmentAbilityData>(94);
            data?.EnchantmentPlayerIndex = source.CombatPlayer;
        }
    }
}
