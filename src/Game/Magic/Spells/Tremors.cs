// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class Tremors : CombatSpell
{
    // CombatSpells.pas:1404
    public Tremors(ECustomNode owner) : base(owner)
    {
        // { Original leaves this spell name untranslated. }
        SpellName = s_tremorsText;
        SpellId = 112;
        ref var spellCombatInfo = ref CombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical, DamageType.Wall);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = -1;
        spellCombatInfo.Radius = 5;
    }

    private static readonly string s_tremorsText = "Tremors";
}
