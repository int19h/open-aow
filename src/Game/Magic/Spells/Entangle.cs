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
    public Entangle(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.EntangleText);
        SpellId = 111;
        ref var spellCombatInfo = ref CombatInfo;
        spellCombatInfo.Attack = 7;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.None);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
    }

    // CombatSpells.pas:1368
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        EntangleSpellCombatAction action;
        action = new EntangleSpellCombatAction(null) { Holder = CreationHolder() };
        action.SourceId = source.Id;
        action.TargetId = target.Id;
        action.SpellId = SpellId;
        if (chargeMana)
        {
            action.ManaCost = BaseManaCost;
        }

        if (!target.GetAbilityEnabled(1) && !target.GetAbilityEnabled(94))
        {
            var resistance = target.GetResistance();
            action.Succeeded = HitChance.HitRole(CombatInfo.Attack - resistance);
        }

        return action;
    }

    // CombatSpells.pas:1383
    public override void GetCombatDamageValues(CombatObject source, CombatObject target, ref AbilityDamageValues values)
    {
        if (!target.GetAbilityEnabled(1) && !target.GetAbilityEnabled(94))
        {
            var resistance = target.GetResistance();
            var hitRoleProbability = HitChance.HitRoleProbability(CombatInfo.Attack - resistance);
            var hitPoints = target.GetHitPoints();
            values.SourceDamage = Round((double)(hitPoints * 100) * hitRoleProbability);
            var targetHitPoints = target.GetHitPoints();
            values.SourceCost = targetHitPoints;
            values.TargetDamage = 0;
            values.TargetCost = 0;
            values.DamageTypes = Set<DamageType>.Of(DamageType.None);
        }
        else
        {
            values.SourceDamage = 0;
            values.SourceCost = 0;
            values.TargetDamage = 0;
            values.TargetCost = 0;
            values.DamageTypes = Set<DamageType>.Of();
        }
    }
}
