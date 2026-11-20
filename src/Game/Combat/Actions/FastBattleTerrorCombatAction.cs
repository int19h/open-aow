// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class FastBattleTerrorCombatAction(CustomNode owner) : MultiTargetCombatSpellCombatAction(owner)
{
    // CombatSpells.pas:1666
    public new const int ClassId = 131777;

    // CombatSpells.pas:1672
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        for (int i = TargetIds.Count - 1; i >= 0; i--)
        {
            byte targetId = TargetIds[i];
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(targetId);
            _ = GamePack.Current.AbilityControl.GetAbility(108).Expand(target.Troop);
        }
    }

    // CombatSpells.pas:1686
    public void Generate(CombatObject source)
    {
        SpellId = 105;
        SourceId = source.Id;
        Battle combat = source.Combat;
        CombatSideIndex opponentSide = source.GetOpponentSide();
        CombatSide side = combat.Sides[(int)opponentSide];

        for (int i = side.GetCount() - 1; i >= 0; i--)
        {
            CombatObject target = side.GetObjects(i);
            if (!target.Status.Contains(CombatObjectStatusElement.Destroyed) && !target.GetAbilityEnabled(108) && !target.GetAbilityEnabled(67) && target is CombatUnit targetUnit && !target.GetImmunityTypes().Contains(DamageType.Death) && targetUnit.Troop.GetUnitType() != UnitType.Machine && HitChance.HitRole(6 - target.GetResistance()))
            {
                _ = TargetIds.Add(target.Id);
            }
        }
    }

}
