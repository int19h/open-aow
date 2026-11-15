// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class EntangleSpellCombatAction : ExclusiveCombatSpellCombatAction
{
    // CombatSpells.pas:1328
    public new const int ClassId = 131784;

    public EntangleSpellCombatAction(CustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:1334
    public override void Execute(Battle combat)
    {
        CombatObject source;
        CombatUnit target;
        UnitEnchantmentAbilityData data;
        base.Execute(combat);
        if (Succeeded)
        {
            source = combat.CombatData.FindId(SourceId);
            target = combat.CombatData.FindId<CombatUnit>(TargetId);
            GamePack.Current.AbilityControl.GetAbility(94).Expand(target.Troop);
            data = (UnitEnchantmentAbilityData)target.Troop.GetAbilityData(94);
            if (data != null)
            {
                data.EnchantmentPlayerIndex = source.CombatPlayer;
            }
        }
    }

}
