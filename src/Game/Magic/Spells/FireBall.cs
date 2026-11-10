// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public partial class FireBall : CombatSpell
{
    // CombatSpells.pas:1489
    public FireBall(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(FireBallRStr);
        FSpellID = 117;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 8;
        spellCombatInfo.Damage = 6;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Fire, DamageType.Wall);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
    }

    private static readonly string FireBallRStr = "Fireball";
}
