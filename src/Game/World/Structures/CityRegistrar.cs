
using Aow.Game.AI;

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

    internal static void SubscribeMapLoaded(GameEngine engine)
    {
        engine.OnHsmLoaded += MapLoaded;
    }
}
