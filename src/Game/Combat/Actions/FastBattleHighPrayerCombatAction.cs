#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public class FastBattleHighPrayerCombatAction(CustomNode owner) : MultiTargetCombatSpellCombatAction(owner)
{
    // CombatSpells.pas:937
    public new const int ClassId = 131_783;

    // CombatSpells.pas:943
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        for (int i = TargetIds.Count - 1; i >= 0; i--)
        {
            byte targetId = TargetIds[i];
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(targetId);
            target.SetHitPoints((byte)(target.HitPoints + 5));
            _ = GamePack.Current.AbilityControl[100].Expand(target.Troop);
        }
    }

    // CombatSpells.pas:958
    public void Generate(CombatObject source)
    {
        SpellId = 102;
        SourceId = source.Id;
        Battle combat = source.Combat;
        CombatSideIndex side = source.Side;
        CombatSide combatSide = combat.Sides[(int)side];

        for (int i = combatSide.GetCount() - 1; i >= 0; i--)
        {
            CombatObject target = combatSide.GetObjects(i);
            if (target is CombatUnit && !target.Status.Contains(CombatObjectStatuses.Destroyed))
            {
                _ = TargetIds.Add(target.Id);
            }
        }
    }
}
