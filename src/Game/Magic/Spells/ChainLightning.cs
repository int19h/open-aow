// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class ChainLightning : CombatSpell
{
    // CombatSpells.pas:1224
    public ChainLightning(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(ChainLightningRStr);
        FSpellID = 107;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 4;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Lightning);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 3;
    }

    private static readonly string ChainLightningRStr = "Chain Lighting";
}
