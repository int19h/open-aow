using Aow.Game.Combat;

namespace Aow.Game.Magic.Abilities;

internal static class TroopConstants
{

    internal static AbilitySelectionTypes DefaultAbilitySelectionTypes { get; } = AbilitySelectionTypes.Unit;

    internal static DamageTypes AllDamageTypes { get; } = DamageTypes.Fire | DamageTypes.Cold | DamageTypes.Lightning | DamageTypes.Magic | DamageTypes.Poison | DamageTypes.Death | DamageTypes.Holy | DamageTypes.Physical | DamageTypes.Wall | DamageTypes.NonSpecific;

}
