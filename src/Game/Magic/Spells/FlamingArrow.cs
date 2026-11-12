// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class FlamingArrow : CombatSpell
{
    // CombatSpells.pas:1603
    public FlamingArrow(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_flamingArrowText);
        SpellId = 123;
        ref var spellCombatInfo = ref CombatInfo;
        spellCombatInfo.Attack = 8;
        spellCombatInfo.Damage = 4;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Fire, DamageType.Physical);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
    }

    private static readonly string s_flamingArrowText = "Flaming Arrow";
}
