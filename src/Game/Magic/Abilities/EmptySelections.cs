
using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

internal static class EmptySelections
{
    internal static Set<AbilitySelectionType> P1EmptySelectionTypes = Set<AbilitySelectionType>.FromMask(0UL);

    internal static ushort P4EmptySelection = 0;

    internal static Set<DamageType> P4EmptyDamageTypes = Set<DamageType>.FromMask(0UL);

    internal static Set<CombatMode> P4EmptyCombatModes = Set<CombatMode>.FromMask(0UL);

}
