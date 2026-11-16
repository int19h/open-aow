// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class TurnUndeadSpellCombatAction : CombatSpellCombatAction
{
    // CombatSpells.pas:861
    public new const int ClassId = 131788;

    public TurnUndeadSpellCombatAction(CustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:867
    public override void Execute(Battle combat)
    {
        CombatObject source;
        CombatUnit target;
        UnitEnchantmentAbilityData data;
        base.Execute(combat);
        if (Damage != 0)
        {
            source = combat.CombatData.FindId(SourceId);
            target = combat.CombatData.FindId<CombatUnit>(TargetId);
            if (!target.Status.Contains(CombatObjectStatusElement.Destroyed))
            {
                GamePack.Current.AbilityControl.GetAbility(34).Expand(target.Troop);
                data = target.Troop.GetAbilityData<UnitEnchantmentAbilityData>(34);
                data?.EnchantmentPlayerIndex = source.CombatPlayer;
            }
        }
    }

}
