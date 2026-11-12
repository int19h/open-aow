
using Aow.Engine.Buffers;

using Aow.Game.Combat;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

internal static class TroopConstants
{

    internal static Set<AbilitySelectionType> DefaultAbilitySelectionTypes { get; } = Set<AbilitySelectionType>.FromMask(1UL);

    internal static Set<DamageType> AllDamageTypes { get; } = Set<DamageType>.FromMask(1023UL);

}
