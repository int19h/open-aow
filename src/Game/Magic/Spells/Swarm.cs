// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class Swarm : CombatSpell
{
    // CombatSpells.pas:1455
    public Swarm(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SwarmRStr);
        FSpellID = 115;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 2;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Fire);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
        spellCombatInfo.Radius = 5;
    }

    private static readonly string SwarmRStr = "Swarm";
}
