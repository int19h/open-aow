// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class DeathRay : CombatSpell
{
    // CombatSpells.pas:1130
    public DeathRay(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(DeathRayRStr);
        FSpellID = 103;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 8;
        spellCombatInfo.Damage = 4;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Death);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
    }

    private static readonly string DeathRayRStr = "Death Ray";
}
