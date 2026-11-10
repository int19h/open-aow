// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited spell ID, combat statistics, damage mode
public partial class Shockwave : CombatSpell
{
    // CombatSpells.pas:1919
    public Shockwave(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(ShockwaveRStr);
        FSpellID = 127;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 8;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical, DamageType.Wall);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
    }

    private static readonly string ShockwaveRStr = "Shock Wave";
}
