
using Aow.Game.AI;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public sealed class CityRegistrar
{
    // City.pas:3306
    public static void MapLoaded(GameMap sender)
    {
        StrategicMap? strategicMap = sender.GetEngine()?.StrategicMap;
        if (strategicMap is not null && sender == strategicMap)
        {
            strategicMap.AIPlayerActionManager.RegisterClasses([AIPlayerActionClass.Of<CityAIPlayerAction>()]);
        }
    }

    internal static void SubscribeMapLoaded(GameEngine engine) => engine.HexSpriteMapLoaded += MapLoaded;
}
