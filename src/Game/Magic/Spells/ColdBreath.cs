// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class ColdBreath : CombatSpell
{
    // CombatSpells.pas:1241
    public ColdBreath(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(ColdBreathRStr);
        FSpellID = 108;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Cold);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 4;
    }

    private static readonly string ColdBreathRStr = "Cold Breath";
}
