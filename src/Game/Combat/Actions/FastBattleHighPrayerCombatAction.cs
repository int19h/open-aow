// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class FastBattleHighPrayerCombatAction(CustomNode owner) : MultiTargetCombatSpellCombatAction(owner)
{
    // CombatSpells.pas:937
    public new const int ClassId = 131783;

    // CombatSpells.pas:943
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        for (int i = TargetIds.Count - 1; i >= 0; i--)
        {
            byte targetId = TargetIds[i];
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(targetId);
            target.SetHitPoints((byte)(target.GetHitPoints() + 5));
            _ = GamePack.Current.AbilityControl.GetAbility(100).Expand(target.Troop);
        }
    }

    // CombatSpells.pas:958
    public void Generate(CombatObject source)
    {
        SpellId = 102;
        SourceId = source.Id;
        Battle combat = source.Combat;
        CombatSideIndex side = source.GetSide();
        CombatSide combatSide = combat.Sides[(int)side];

        for (int i = combatSide.GetCount() - 1; i >= 0; i--)
        {
            CombatObject target = combatSide.GetObjects(i);
            if (target is CombatUnit && !target.Status.Contains(CombatObjectStatusElement.Destroyed))
            {
                _ = TargetIds.Add(target.Id);
            }
        }
    }

}
