// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class Slow : CombatSpell
{
    // CombatSpells.pas:1284
    public Slow(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.SlowRStr);
        FSpellID = 110;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 9;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.None);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
    }

    // CombatSpells.pas:1313
    public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
    {
        SlowCombatAction Action;
        Action = new SlowCombatAction(null);
        Action.FSourceID = Source.FID;
        Action.FTargetID = Target.FID;
        Action.FSpellID = FSpellID;
        if (ChargeMana)
        {
            Action.FManaCost = FBaseManaCost;
        }

        if (!Target.GetAbilityEnabled(132))
        {
            var resistance = Target.GetResistance();
            Action.FSucceeded = HitChance.HitRole(FCombatInfo.Attack - resistance);
        }

        return Action;
    }

    // CombatSpells.pas:1300
    public override void GetCombatDamageValueEx(CombatObject Source, CombatObject Target, ref AbilityDamageValues Values)
    {
        ref var abilityDamageValues = ref Values;
        abilityDamageValues.SourceDamage = 0;
        abilityDamageValues.SourceCost = 0;
        abilityDamageValues.TargetDamage = 0;
        abilityDamageValues.TargetCost = 0;
        abilityDamageValues.DamageTypes = Set<DamageType>.Of();
    }
}
