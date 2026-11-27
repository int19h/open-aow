#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Combat.Actions;

public class DispelMagicAbilityCombatAction(CustomNode owner) : DispelMagicCombatAction(owner)
{
    public new const int ClassId = 131_775;

    public override bool TouchSucceeded => EnchantmentIds.Count > 0;

    // AoWAb.PAS:4018
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        CombatUnit source = combat.CombatData.FindId<CombatUnit>(SourceId);
        GetEngine().GamePack.AbilityControl.GetAbility<DispelMagicAbility>(60).SetEnabled(source.Troop, enabled: false);
    }
}
