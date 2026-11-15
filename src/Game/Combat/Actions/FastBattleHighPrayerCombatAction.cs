// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class FastBattleHighPrayerCombatAction : MultiTargetCombatSpellCombatAction
{
    // CombatSpells.pas:937
    public new const int ClassId = 131783;

    public FastBattleHighPrayerCombatAction(CustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:943
    public override void Execute(Battle combat)
    {
        CombatUnit target;
        base.Execute(combat);
        for (int i = TargetIds.Count - 1; i >= 0; i--)
        {
            var targetId = TargetIds[i];
            target = combat.CombatData.FindId<CombatUnit>(targetId);
            target.SetHitPoints((byte)(target.GetHitPoints() + 5));
            GamePack.Current.AbilityControl.GetAbility(100).Expand(target.Troop);
        }
    }

    // CombatSpells.pas:958
    public void Generate(CombatObject source)
    {
        CombatSide combatSide;
        CombatObject target;
        SpellId = 102;
        SourceId = source.Id;
        var combat = source.Combat;
        var side = source.GetSide();
        combatSide = combat.Sides[(int)side];

        for (int i = combatSide.GetCount() - 1; i >= 0; i--)
        {
            target = combatSide.GetObjects(i);
            if (target is CombatUnit && !target.Status.Contains(CombatObjectStatusElement.Destroyed))
            {
                TargetIds.Add(target.Id);
            }
        }
    }

}
