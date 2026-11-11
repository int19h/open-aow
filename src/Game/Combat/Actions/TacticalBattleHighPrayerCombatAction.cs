// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class TacticalBattleHighPrayerCombatAction : ExclusiveCombatSpellCombatAction
{
    // CombatSpells.pas:976
    public new const int ClassID = 131786;

    public TacticalBattleHighPrayerCombatAction(ECustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:982
    public override void Execute(Battle Combat)
    {
        CombatObject Target;
        base.Execute(Combat);
        if (FSucceeded)
        {
            Target = Combat.FCombatData.FindID(FTargetID);
            Target.SetHitPoints((byte)(Target.GetHitPoints() + 5));
            AoWHSSet.Current.FAbilityControl.GetAbility(100).Expand(((CombatUnit)Target).FUnit);
        }
    }

}
