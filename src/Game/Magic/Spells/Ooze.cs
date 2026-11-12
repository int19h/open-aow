// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class Ooze : CombatSpell
{
    // CombatSpells.pas:1619
    public Ooze(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_oozeText);
        SpellId = 124;
        ref var spellCombatInfo = ref CombatInfo;
        spellCombatInfo.Attack = 0;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of();
        spellCombatInfo.RepeatCount = 0;
        spellCombatInfo.Range = 8;
    }

    // CombatSpells.pas:1635
    public override void GetCombatDamageValues(CombatObject source, CombatObject target, ref AbilityDamageValues values)
    {
        target.GetProtectionTypes();
        target.GetImmunityTypes();
        values.DamageTypes = CombatInfo.DamageTypes;
        var resistance = target.GetResistance();
        var hitRoleProbability = HitChance.HitRoleProbability(CombatInfo.Attack - resistance);
        values.SourceDamage = Round((double)hitRoleProbability * 200);
        values.SourceCost = 200;
        values.TargetDamage = 0;
        values.TargetCost = 0;
    }

    private static readonly string s_oozeText = "Ooze";
}
