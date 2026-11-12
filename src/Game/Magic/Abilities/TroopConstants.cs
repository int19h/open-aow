
using Aow.Engine.Buffers;

using Aow.Game.Combat;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

internal static class TroopConstants
{
    internal static AbstractAoWHSMap_UnFogExploreArea__TAssertText TAbstractAoWHSMap_UnFogExploreArea__AssertFile = new AbstractAoWHSMap_UnFogExploreArea__TAssertText
    {
        RefCount = -1,
        Len = 31,
        Text = ByteArray128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 65, 111, 87, 72, 83, 77, 97, 112, 46, 80, 65, 83, 0])
    };

    internal static AbstractAoWHSMap_UnFogExploreArea__TAssertText TAbstractAoWHSMap_UnFogExploreArea__AssertMessage = new AbstractAoWHSMap_UnFogExploreArea__TAssertText
    {
        RefCount = -1,
        Len = 19,
        Text = ByteArray128.From([73, 110, 118, 97, 108, 105, 100, 32, 85, 110, 102, 111, 103, 32, 82, 97, 110, 103, 101, 0])
    };

    internal static Set<AbilitySelectionType> s_defaultAbilitySelectionTypes = Set<AbilitySelectionType>.FromMask(1UL);

    internal static Set<DamageType> s_allDamageTypes = Set<DamageType>.FromMask(1023UL);

}
