// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class FlameThrowingAbility : BreathAbility
{
    // AoWAb.PAS:4934
    public FlameThrowingAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 30;
        FAbilityName = AbilityRules.TranslateRStr(FlameThrowingRStr);
        FAttackRA = 4;
        FDamageRA = 3;
        FDamageTypesRA.Value = Set<DamageType>.Of(DamageType.Fire);
    }

    // AoWAb.PAS:4943
    public override void GetOffensiveStrength(AbilityOwner Owner, ref int Strength, ref Set<DamageType> Types)
    {
        var attackRepeatRA = GetAttackRepeatRA(Owner);
        var damageRA = GetDamageRA(Owner);
        Strength = AbilityRules.P3LimitedDamage(GetAttackRA(Owner), damageRA, attackRepeatRA);
        Strength = Strength * 2;
        Types = GetDamageTypesRA(Owner);
    }

    private static readonly string FlameThrowingRStr = "Flame Throwing";
}
