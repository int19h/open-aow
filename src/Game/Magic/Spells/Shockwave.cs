// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited spell ID, combat statistics, damage mode
public partial class Shockwave : CombatSpell
{
    // CombatSpells.pas:1919
    public Shockwave(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_shockwaveText);
        SpellId = 127;
        var spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 8;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical, DamageType.Wall);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_shockwaveText = "Shock Wave";
}
