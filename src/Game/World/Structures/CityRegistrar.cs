
using Aow.Game.AI;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public partial class CityRegistrar
{
    // City.pas:3306
    public static void MapLoaded(GameMap sender)
    {
        if (sender == StrategicMap.Current)
        {
            StrategicMap.Current.AIPlayerActionManager.RegisterClasses([AIPlayerActionClass.Of<CityAIPlayerAction>()]);
        }
    }

    internal static void SubscribeMapLoaded()
    {
        if (GameTables.ValidateGameEngine())
            GameTables.GameEngine.OnHsmLoaded += MapLoaded;
    }

    internal static void UnsubscribeMapLoaded()
    {
        if (GameTables.GameEngine != null)
            GameTables.GameEngine.OnHsmLoaded -= MapLoaded;
    }
}
