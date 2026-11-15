// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class ColdBreath : CombatSpell
{
    // CombatSpells.pas:1241
    public ColdBreath(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_coldBreathText);
        SpellId = 108;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Cold);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 4;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_coldBreathText = "Cold Breath";
}
