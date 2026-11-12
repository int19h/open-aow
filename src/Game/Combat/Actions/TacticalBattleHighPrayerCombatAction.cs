// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class TacticalBattleHighPrayerCombatAction : ExclusiveCombatSpellCombatAction
{
    // CombatSpells.pas:976
    public new const int ClassId = 131786;

    public TacticalBattleHighPrayerCombatAction(ECustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:982
    public override void Execute(Battle combat)
    {
        CombatObject target;
        base.Execute(combat);
        if (Succeeded)
        {
            target = combat.CombatData.FindId(TargetId);
            target.SetHitPoints((byte)(target.GetHitPoints() + 5));
            GamePack.Current.AbilityControl.GetAbility(100).Expand(((CombatUnit)target).Troop);
        }
    }

}
