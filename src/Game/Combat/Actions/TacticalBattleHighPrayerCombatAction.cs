#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;
using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

public class TacticalBattleHighPrayerCombatAction(CustomNode owner) : ExclusiveCombatSpellCombatAction(owner)
{
    // CombatSpells.pas:976
    public new const int ClassId = 131_786;

    // CombatSpells.pas:982
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        if (Succeeded)
        {
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(TargetId);
            target.SetHitPoints((target.HitPoints + 5).TruncateToByte());
            _ = GetEngine().GamePack.AbilityControl[100].Expand(target.Troop);
        }
    }
}
