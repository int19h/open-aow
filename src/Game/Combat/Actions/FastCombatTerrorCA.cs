// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class FastCombatTerrorCA : MultiTargetCombatSpellCA
{
    // CombatSpells.pas:1666
    public new const int ClassID = 131777;

    public FastCombatTerrorCA(ECustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:1672
    public override void Execute(Battle Combat)
    {
        CombatUnit Target;
        base.Execute(Combat);
        for (int I = FTargetIDs.Count - 1; I >= 0; I--)
        {
            var targetID = FTargetIDs[I];
            Target = (CombatUnit)Combat.FCombatData.FindID(targetID);
            AoWHSSet.Current.FAbilityControl.GetAbility(108).Expand(Target.FUnit);
        }
    }

    // CombatSpells.pas:1686
    public void Generate(CombatObject Source)
    {
        CombatSide Side;
        CombatObject Target;
        FSpellID = 105;
        FSourceID = Source.FID;
        var combat = Source.FCombat;
        var opponentSide = Source.GetOpponentSide();
        Side = combat.FSides[(int)opponentSide];

        for (int I = Side.GetCount() - 1; I >= 0; I--)
        {
            Target = Side.GetObjects(I);
            if (!Target.FStatus.Contains(CombatObjectStatusElement.Destroyed) && !Target.GetAbilityEnabled(108) && !Target.GetAbilityEnabled(67) && Target is CombatUnit && !Target.GetImmunityTypes().Contains(DamageType.Death) && ((CombatUnit)Target).FUnit.GetUnitType() != UnitType.Machine && HitChance.HitRole(6 - Target.GetResistance()))
            {
                FTargetIDs.Add(Target.FID);
            }
        }
    }

}
