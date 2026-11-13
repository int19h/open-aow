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
    public FireBall(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_fireBallText);
        SpellId = 117;
        ref var spellCombatInfo = ref CombatInfo;
        spellCombatInfo.Attack = 8;
        spellCombatInfo.Damage = 6;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Fire, DamageType.Wall);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;
    }

    private static readonly string s_fireBallText = "Fireball";
}
