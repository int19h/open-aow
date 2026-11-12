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
        SpellName = AbilityRules.TranslateResourceString(GameText.SlowText);
        SpellId = 110;
        ref var spellCombatInfo = ref CombatInfo;
        spellCombatInfo.Attack = 9;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.None);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
    }

    // CombatSpells.pas:1313
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        SlowCombatAction action;
        action = new SlowCombatAction(null);
        action.SourceId = source.Id;
        action.TargetId = target.Id;
        action.SpellId = SpellId;
        if (chargeMana)
        {
            action.ManaCost = BaseManaCost;
        }

        if (!target.GetAbilityEnabled(132))
        {
            var resistance = target.GetResistance();
            action.Succeeded = HitChance.HitRole(CombatInfo.Attack - resistance);
        }

        return action;
    }

    // CombatSpells.pas:1300
    public override void GetCombatDamageValues(CombatObject source, CombatObject target, ref AbilityDamageValues values)
    {
        ref var abilityDamageValues = ref values;
        abilityDamageValues.SourceDamage = 0;
        abilityDamageValues.SourceCost = 0;
        abilityDamageValues.TargetDamage = 0;
        abilityDamageValues.TargetCost = 0;
        abilityDamageValues.DamageTypes = Set<DamageType>.Of();
    }
}
