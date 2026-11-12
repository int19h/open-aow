// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class Stoning : CombatSpell
{
    // CombatSpells.pas:1587
    public Stoning(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_stoningText);
        SpellId = 122;
        ref var spellCombatInfo = ref CombatInfo;
        spellCombatInfo.Attack = 5;
        spellCombatInfo.Damage = 2;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
        spellCombatInfo.RepeatCount = 6;
        spellCombatInfo.Range = 12;
    }

    private static readonly string s_stoningText = "Stoning";
}
