// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class FrostBeams : CombatSpell
{
    // CombatSpells.pas:1553
    public FrostBeams(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_frostBeamsText);
        SpellId = 120;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 9;
        spellCombatInfo.Damage = 3;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Cold);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 3;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_frostBeamsText = "Frost Beam";
}
