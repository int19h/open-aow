// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class ChainLightning : CombatSpell
{
    // CombatSpells.pas:1224
    public ChainLightning(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_chainLightningText);
        SpellId = 107;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 4;
        spellCombatInfo.DamageTypes = DamageTypes.Lightning;
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 3;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_chainLightningText = "Chain Lighting";
}
