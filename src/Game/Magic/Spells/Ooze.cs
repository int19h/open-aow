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
        FSpellName = AbilityRules.TranslateRStr(OozeRStr);
        FSpellID = 124;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 0;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of();
        spellCombatInfo.RepeatCount = 0;
        spellCombatInfo.Range = 8;
    }

    // CombatSpells.pas:1635
    public override void GetCombatDamageValueEx(CombatObject Source, CombatObject Target, ref AbilityDamageValues Values)
    {
        Target.GetProtectionTypes();
        Target.GetImmunityTypes();
        Values.DamageTypes = FCombatInfo.DamageTypes;
        var resistance = Target.GetResistance();
        var hitRoleProbability = HitChance.HitRoleProbability(FCombatInfo.Attack - resistance);
        Values.SourceDamage = Round((double)hitRoleProbability * 200);
        Values.SourceCost = 200;
        Values.TargetDamage = 0;
        Values.TargetCost = 0;
    }

    private static readonly string OozeRStr = "Ooze";
}
