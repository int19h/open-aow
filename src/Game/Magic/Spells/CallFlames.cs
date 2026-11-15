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
    public CallFlames(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_callFlamesText);
        SpellId = 113;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 9;
        spellCombatInfo.Damage = 3;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Fire);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 8;
        spellCombatInfo.Radius = 1;

        CombatInfo = spellCombatInfo;
    }

    private static readonly string s_callFlamesText = "Call Flames";
}
