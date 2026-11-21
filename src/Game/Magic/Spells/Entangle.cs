#nullable disable
// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class Entangle : CombatSpell
{
    // CombatSpells.pas:1352
    public Entangle(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.EntangleText);
        SpellId = 111;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 7;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = DamageTypes.NonSpecific;
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;

        CombatInfo = spellCombatInfo;
    }

    // CombatSpells.pas:1368
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        EntangleSpellCombatAction action = new(null)
        {
            Holder = CreationHolder(),
            SourceId = source.Id,
            TargetId = target.Id,
            SpellId = SpellId
        };
        if (chargeMana)
        {
            action.ManaCost = BaseManaCost;
        }

        if (!target.GetAbilityEnabled(1) && !target.GetAbilityEnabled(94))
        {
            sbyte resistance = target.GetResistance();
            action.Succeeded = HitChance.HitRole(CombatInfo.Attack - resistance);
        }

        return action;
    }

    // CombatSpells.pas:1383
    public override void GetCombatDamageValues(CombatObject source, CombatObject target, ref AbilityDamageValues values)
    {
        if (!target.GetAbilityEnabled(1) && !target.GetAbilityEnabled(94))
        {
            sbyte resistance = target.GetResistance();
            float hitRoleProbability = HitChance.HitRoleProbability(CombatInfo.Attack - resistance);
            sbyte hitPoints = target.HitPoints;
            values.SourceDamage = Round((double)(hitPoints * 100) * hitRoleProbability);
            sbyte targetHitPoints = target.HitPoints;
            values.SourceCost = targetHitPoints;
            values.TargetDamage = 0;
            values.TargetCost = 0;
            values.DamageTypes = DamageTypes.NonSpecific;
        }
        else
        {
            values.SourceDamage = 0;
            values.SourceCost = 0;
            values.TargetDamage = 0;
            values.TargetCost = 0;
            values.DamageTypes = DamageTypes.None;
        }
    }
}
