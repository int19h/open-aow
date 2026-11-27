// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class FireBreath : CombatSpell
{
    // CombatSpells.pas:1438
    public FireBreath(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_fireBreathText);
        SpellId = 114;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = DamageTypes.Fire;
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 4;
        spellCombatInfo.Radius = 3;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_fireBreathText = "Fire Breath";
}
