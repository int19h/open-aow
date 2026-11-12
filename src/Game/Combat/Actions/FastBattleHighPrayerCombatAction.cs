// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class FastBattleHighPrayerCombatAction : MultiTargetCombatSpellCombatAction
{
    // CombatSpells.pas:937
    public new const int ClassId = 131783;

    public FastBattleHighPrayerCombatAction(ECustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:943
    public override void Execute(Battle combat)
    {
        CombatObject target;
        base.Execute(combat);
        for (int I = TargetIds.Count - 1; I >= 0; I--)
        {
            var targetId = TargetIds[I];
            target = combat.CombatData.FindId(targetId);
            target.SetHitPoints((byte)(target.GetHitPoints() + 5));
            GamePack.Current.AbilityControl.GetAbility(100).Expand(((CombatUnit)target).Troop);
        }
    }

    // CombatSpells.pas:958
    public void Generate(CombatObject source)
    {
        CombatSide Side;
        CombatObject target;
        SpellId = 102;
        SourceId = source.Id;
        var combat = source.Combat;
        var side = source.GetSide();
        Side = combat.Sides[(int)side];

        for (int I = Side.GetCount() - 1; I >= 0; I--)
        {
            target = Side.GetObjects(I);
            if (target is CombatUnit && !target.Status.Contains(CombatObjectStatusElement.Destroyed))
            {
                TargetIds.Add(target.Id);
            }
        }
    }

}
