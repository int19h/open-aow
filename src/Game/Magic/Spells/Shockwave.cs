// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

// inherited spell ID, combat statistics, damage mode
public class Shockwave : CombatSpell
{
    // CombatSpells.pas:1919
    public Shockwave(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_shockwaveText);
        SpellId = 127;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 8;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = DamageTypes.Physical | DamageTypes.Wall;
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_shockwaveText = "Shock Wave";
}
