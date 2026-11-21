// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class IceShards : CombatSpell
{
    // CombatSpells.pas:1537
    public IceShards(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_iceShardsText);
        SpellId = 119;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 4;
        spellCombatInfo.DamageTypes = DamageTypes.Physical;
        spellCombatInfo.RepeatCount = 3;
        spellCombatInfo.Range = 12;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_iceShardsText = "Ice Shards";
}
