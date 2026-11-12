// Translated from MainAG.pas.

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// no fields. map-load registration callback
public partial class MainRegistrar
{
    // MainAG.pas:115
    public static void MapLoaded(GameMap sender)
    {
        if (sender == StrategicMap.Current)
        {
            StrategicMap.Current.AIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<PassiveAIGroup>(), AIGroupClass.Of<SuicidalAIGroup>(), AIGroupClass.Of<FortifyAIGroup>(), AIGroupClass.Of<TransportAIGroup>()]);
        }
    }

    internal static void SubscribeMapLoaded(GameEngine engine)
    {
        engine.OnHsmLoaded += MapLoaded;
    }
}
