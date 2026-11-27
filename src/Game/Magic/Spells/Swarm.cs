// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class Swarm : CombatSpell
{
    // CombatSpells.pas:1455
    public Swarm(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_swarmText);
        SpellId = 115;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 2;
        spellCombatInfo.DamageTypes = DamageTypes.Fire;
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 5;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_swarmText = "Swarm";
}
