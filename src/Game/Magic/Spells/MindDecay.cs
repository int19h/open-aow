// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class MindDecay : CombatSpell
{
    // CombatSpells.pas:1190
    public MindDecay(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(MindDecayRStr);
        FSpellID = 106;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 5;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.None);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 3;
    }

    // CombatSpells.pas:1207
    public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
    {
        MindDecayCombatAction Action;
        Action = new MindDecayCombatAction(null);
        Action.FSourceID = Source.FID;
        Action.FTargetID = Target.FID;
        Action.FSpellID = FSpellID;
        if (ChargeMana)
        {
            Action.FManaCost = FBaseManaCost;
        }

        if (Target is CombatUnit && ((CombatUnit)Target).FUnit.GetUnitType() != UnitType.Machine && ((CombatUnit)Target).FUnit.GetRace() != 11 && !((CombatUnit)Target).FUnit.GetAbilityEnabled(129) && !(((CombatUnit)Target).FUnit is Leader))
        {
            var resistance = Target.GetResistance();
            Action.FSucceeded = HitChance.HitRole(FCombatInfo.Attack - resistance);
        }

        return Action;
    }

    private static readonly string MindDecayRStr = "Mind Decay";
}
