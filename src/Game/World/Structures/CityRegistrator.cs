// Translated from City.pas.
using Aow.Game.Ai;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public partial class CityRegistrator
{
    // City.pas:3306
    public static void MapLoaded(HSMap Sender)
    {
        if (Sender == AoWHSMap.Current)
        {
            AoWHSMap.Current.FAIPlayerActionManager.RegisterClasses([AIPlayerActionClass.Of<CityAIPA>()]);
        }
    }

    internal static void SubscribeMapLoaded()
    {
        if (GameTables.ValidateAoWEngine())
            GameTables.AoWEngine.OnHSMLoaded += MapLoaded;
    }

    internal static void UnsubscribeMapLoaded()
    {
        if (GameTables.AoWEngine != null)
            GameTables.AoWEngine.OnHSMLoaded -= MapLoaded;
    }
}
