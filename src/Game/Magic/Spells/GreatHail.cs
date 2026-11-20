// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class GreatHail : CombatSpell
{
    // CombatSpells.pas:1472
    public GreatHail(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_greatHailText);
        SpellId = 116;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 5;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical, DamageType.Wall);
        spellCombatInfo.RepeatCount = 3;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 2;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_greatHailText = "Great Hail";
}
