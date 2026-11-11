// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class TurnUndead : CombatSpell
{
    // CombatSpells.pas:888
    public TurnUndead(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.TurnUndeadRStr);
        FSpellID = 101;
        FDamageMode = 1;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Holy);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
    }

    // CombatSpells.pas:905
    public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
    {
        TurnUndeadSpellCombatAction Action;
        Action = new TurnUndeadSpellCombatAction(null);
        Action.FSpellID = FSpellID;
        Action.FSourceID = Source.FID;
        Action.FTargetID = Target.FID;
        if (ChargeMana)
        {
            Action.FManaCost = FBaseManaCost;
        }

        bool condition = Target is CombatUnit;
        if (condition)
        {
            var unitType = ((CombatUnit)Target).FUnit.GetUnitType();
            condition = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType);
        }

        if (condition && (((CombatUnit)Target).FUnit.GetRace() == 11 || ((CombatUnit)Target).FUnit.GetAbilityEnabled(129)))
        {
            Action.GenerateEx(Source, Target, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, FDamageMode);
        }

        return Action;
    }

    // CombatSpells.pas:922
    public override void fcGetDamageValueEx(CombatObject Source, CombatObject Target, WallSituation WallSituation, ref AbilityDamageValues Values)
    {
        bool condition = Target is CombatUnit;
        if (condition)
        {
            var unitType = ((CombatUnit)Target).FUnit.GetUnitType();
            condition = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType);
        }

        if (condition && (((CombatUnit)Target).FUnit.GetRace() == 11 || ((CombatUnit)Target).FUnit.GetAbilityEnabled(129)))
        {
            base.fcGetDamageValueEx(Source, Target, WallSituation, ref Values);
        }
        else
        {
            Values.SourceDamage = 0;
            Values.SourceCost = 0;
            Values.TargetDamage = 0;
            Values.TargetCost = 0;
            Values.DamageTypes = Set<DamageType>.Of();
        }
    }
}
