// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

// inherited spell ID, combat statistics, damage mode
public class SacrificialFlame : CombatSpell
{
    // CombatSpells.pas:1934
    public SacrificialFlame(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_sacrificialFlameText);
        SpellId = 128;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 8;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = DamageTypes.Fire;
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 3;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_sacrificialFlameText = "Sacrificial Flame";
}
