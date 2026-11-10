// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class CallFlames : CombatSpell
{
    // CombatSpells.pas:1421
    public CallFlames(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(CallFlamesRStr);
        FSpellID = 113;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 9;
        spellCombatInfo.Damage = 3;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Fire);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 8;
        spellCombatInfo.Radius = 1;
    }

    private static readonly string CallFlamesRStr = "Call Flames";
}
