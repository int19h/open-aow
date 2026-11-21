// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class SolarFlare : CombatSpell
{
    // CombatSpells.pas:845
    public SolarFlare(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_solarFlareText);
        SpellId = 100;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 8;
        spellCombatInfo.Damage = 4;
        spellCombatInfo.DamageTypes = DamageTypes.Holy;
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_solarFlareText = "Solar Flare";
}
