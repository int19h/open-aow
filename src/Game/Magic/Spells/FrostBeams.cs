// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class FrostBeams : CombatSpell
{
    // CombatSpells.pas:1553
    public FrostBeams(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(FrostBeamsRStr);
        FSpellID = 120;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 9;
        spellCombatInfo.Damage = 3;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Cold);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 3;
    }

    private static readonly string FrostBeamsRStr = "Frost Beam";
}
