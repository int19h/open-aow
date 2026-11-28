#nullable disable warnings
// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class Slow : CombatSpell
{
    // CombatSpells.pas:1284
    public Slow(CustomNode? owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.SlowText);
        SpellId = 110;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 9;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = DamageTypes.NonSpecific;
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;

        CombatInfo = spellCombatInfo;
    }

    // CombatSpells.pas:1313
    public override CombatAction CreateCombatAction(CombatObject? source, CombatObject target, bool chargeMana)
    {
        SlowCombatAction action = new(null)
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

        if (!target.GetAbilityEnabled(132))
        {
            int resistance = target.GetResistance();
            action.Succeeded = HitChance.HitRole(CombatInfo.Attack - resistance);
        }

        return action;
    }

    // CombatSpells.pas:1300
    public override void GetCombatDamageValues(CombatObject? source, CombatObject? target, ref AbilityDamageValues values)
    {
        ref AbilityDamageValues abilityDamageValues = ref values;
        abilityDamageValues.SourceDamage = 0;
        abilityDamageValues.SourceCost = 0;
        abilityDamageValues.TargetDamage = 0;
        abilityDamageValues.TargetCost = 0;
        abilityDamageValues.DamageTypes = DamageTypes.None;
    }
}
