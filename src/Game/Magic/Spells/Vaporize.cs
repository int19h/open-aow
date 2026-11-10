// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class Vaporize : CombatSpell
{
    // CombatSpells.pas:1570
    public Vaporize(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(VaporizeRStr);
        FSpellID = 121;
        FDamageMode = 1;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 7;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
    }

    private static readonly string VaporizeRStr = "Vaporize";
}
