// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class DeathRay : CombatSpell
{
    // CombatSpells.pas:1130
    public DeathRay(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_deathRayText);
        SpellId = 103;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 8;
        spellCombatInfo.Damage = 4;
        spellCombatInfo.DamageTypes = DamageTypes.Death;
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_deathRayText = "Death Ray";
}
