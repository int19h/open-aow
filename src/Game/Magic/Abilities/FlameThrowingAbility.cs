// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

namespace Aow.Game.Magic.Abilities;

public class FlameThrowingAbility : BreathAbility
{
    // AoWAb.PAS:4934
    public FlameThrowingAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityId = 30;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_flameThrowingText);
        AttackRangedAttack = 4;
        DamageRangedAttack = 3;
        DamageTypesRangedAttack = DamageTypes.Fire;
    }

    // AoWAb.PAS:4943
    public override void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref DamageTypes types)
    {
        sbyte attackRepeatRangedAttack = GetAttackRepeatRangedAttack(owner);
        sbyte damageRangedAttack = GetDamageRangedAttack(owner);
        strength = AbilityRules.LimitedDamage(GetAttackRangedAttack(owner), defense: damageRangedAttack, damage: attackRepeatRangedAttack);
        strength *= 2;
        types = GetDamageTypesRangedAttack(owner);
    }

    private static readonly string s_flameThrowingText = "Flame Throwing";
}
