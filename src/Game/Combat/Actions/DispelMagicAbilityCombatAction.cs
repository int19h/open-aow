// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class DispelMagicAbilityCombatAction(CustomNode owner) : DispelMagicCombatAction(owner)
{
    public new const int ClassId = 131775;

    public override bool TouchSucceeded => EnchantmentIds.Count > 0;

    // AoWAb.PAS:4018
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        CombatUnit source = combat.CombatData.FindId<CombatUnit>(SourceId);
        GamePack.Current.AbilityControl.GetAbility<DispelMagicAbility>(60).SetEnabled(source.Troop, false);
    }

    // intf_P4_02.inc:660

}
