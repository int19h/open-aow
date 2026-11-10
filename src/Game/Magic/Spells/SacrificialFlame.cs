// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited spell ID, combat statistics, damage mode
public partial class SacrificialFlame : CombatSpell
{
    // CombatSpells.pas:1934
    public SacrificialFlame(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SacrificialFlameRStr);
        FSpellID = 128;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 8;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Fire);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 3;
    }

    private static readonly string SacrificialFlameRStr = "Sacrificial Flame";
}
