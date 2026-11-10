// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class SacredWrath : CombatSpell
{
    // CombatSpells.pas:1649
    public SacredWrath(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SacredWrathRStr);
        FSpellID = 125;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Holy);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 8;
        spellCombatInfo.Radius = 3;
    }

    private static readonly string SacredWrathRStr = "Sacred Wrath";
}
