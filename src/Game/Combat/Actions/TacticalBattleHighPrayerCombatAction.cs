// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public class TacticalBattleHighPrayerCombatAction(CustomNode owner) : ExclusiveCombatSpellCombatAction(owner)
{
    // CombatSpells.pas:976
    public new const int ClassId = 131786;

    // CombatSpells.pas:982
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        if (Succeeded)
        {
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(TargetId);
            target.SetHitPoints((byte)(target.HitPoints + 5));
            _ = GamePack.Current.AbilityControl[100].Expand(target.Troop);
        }
    }
}
