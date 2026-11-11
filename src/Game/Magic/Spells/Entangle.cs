// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class Entangle : CombatSpell
{
    // CombatSpells.pas:1352
    public Entangle(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.EntangleRStr);
        FSpellID = 111;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 7;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.None);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
    }

    // CombatSpells.pas:1368
    public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
    {
        EntangleSpellCombatAction Action;
        Action = new EntangleSpellCombatAction(null);
        Action.FSourceID = Source.FID;
        Action.FTargetID = Target.FID;
        Action.FSpellID = FSpellID;
        if (ChargeMana)
        {
            Action.FManaCost = FBaseManaCost;
        }

        if (!Target.GetAbilityEnabled(1) && !Target.GetAbilityEnabled(94))
        {
            var resistance = Target.GetResistance();
            Action.FSucceeded = HitChance.HitRole(FCombatInfo.Attack - resistance);
        }

        return Action;
    }

    // CombatSpells.pas:1383
    public override void GetCombatDamageValueEx(CombatObject Source, CombatObject Target, ref AbilityDamageValues Values)
    {
        if (!Target.GetAbilityEnabled(1) && !Target.GetAbilityEnabled(94))
        {
            var resistance = Target.GetResistance();
            var hitRoleProbability = HitChance.HitRoleProbability(FCombatInfo.Attack - resistance);
            var hitPoints = Target.GetHitPoints();
            Values.SourceDamage = Round((double)(hitPoints * 100) * hitRoleProbability);
            var targetHitPoints = Target.GetHitPoints();
            Values.SourceCost = targetHitPoints;
            Values.TargetDamage = 0;
            Values.TargetCost = 0;
            Values.DamageTypes = Set<DamageType>.Of(DamageType.None);
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
