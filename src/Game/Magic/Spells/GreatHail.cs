// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class GreatHail : CombatSpell
{
    // CombatSpells.pas:1472
    public GreatHail(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GreatHailRStr);
        FSpellID = 116;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 5;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical, DamageType.Wall);
        spellCombatInfo.RepeatCount = 3;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 2;
    }

    private static readonly string GreatHailRStr = "Great Hail";
}
