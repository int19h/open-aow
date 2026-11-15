// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class FastBattleTerrorCombatAction : MultiTargetCombatSpellCombatAction
{
    // CombatSpells.pas:1666
    public new const int ClassId = 131777;

    public FastBattleTerrorCombatAction(ECustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:1672
    public override void Execute(Battle combat)
    {
        CombatUnit target;
        base.Execute(combat);
        for (int i = TargetIds.Count - 1; i >= 0; i--)
        {
            var targetId = TargetIds[i];
            target = (CombatUnit)combat.CombatData.FindId(targetId);
            GamePack.Current.AbilityControl.GetAbility(108).Expand(target.Troop);
        }
    }

    // CombatSpells.pas:1686
    public void Generate(CombatObject source)
    {
        CombatSide side;
        CombatObject target;
        SpellId = 105;
        SourceId = source.Id;
        var combat = source.Combat;
        var opponentSide = source.GetOpponentSide();
        side = combat.Sides[(int)opponentSide];

        for (int i = side.GetCount() - 1; i >= 0; i--)
        {
            target = side.GetObjects(i);
            if (!target.Status.Contains(CombatObjectStatusElement.Destroyed) && !target.GetAbilityEnabled(108) && !target.GetAbilityEnabled(67) && target is CombatUnit && !target.GetImmunityTypes().Contains(DamageType.Death) && ((CombatUnit)target).Troop.GetUnitType() != UnitType.Machine && HitChance.HitRole(6 - target.GetResistance()))
            {
                TargetIds.Add(target.Id);
            }
        }
    }

}
