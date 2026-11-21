// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class Stoning : CombatSpell
{
    // CombatSpells.pas:1587
    public Stoning(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_stoningText);
        SpellId = 122;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 5;
        spellCombatInfo.Damage = 2;
        spellCombatInfo.DamageTypes = DamageTypes.Physical;
        spellCombatInfo.RepeatCount = 6;
        spellCombatInfo.Range = 12;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_stoningText = "Stoning";
}
