// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class SacredWrath : CombatSpell
{
    // CombatSpells.pas:1649
    public SacredWrath(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_sacredWrathText);
        SpellId = 125;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = DamageTypes.Holy;
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 8;
        spellCombatInfo.Radius = 3;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_sacredWrathText = "Sacred Wrath";
}
