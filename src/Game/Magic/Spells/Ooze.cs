// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class Ooze : CombatSpell
{
    // CombatSpells.pas:1619
    public Ooze(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_oozeText);
        SpellId = 124;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 0;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of();
        spellCombatInfo.RepeatCount = 0;
        spellCombatInfo.Range = 8;

        CombatInfo = spellCombatInfo;
    }

    // CombatSpells.pas:1635
    public override void GetCombatDamageValues(CombatObject source, CombatObject target, ref AbilityDamageValues values)
    {
        _ = target.GetProtectionTypes();
        _ = target.GetImmunityTypes();
        values.DamageTypes = CombatInfo.DamageTypes;
        sbyte resistance = target.GetResistance();
        float hitRoleProbability = HitChance.HitRoleProbability(CombatInfo.Attack - resistance);
        values.SourceDamage = Round((double)hitRoleProbability * 200);
        values.SourceCost = 200;
        values.TargetDamage = 0;
        values.TargetCost = 0;
    }

    private static readonly string s_oozeText = "Ooze";
}
