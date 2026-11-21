#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from MainAG.pas.

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// no fields. map-load registration callback
public sealed class MainRegistrar
{
    // MainAG.pas:115
    public static void MapLoaded(GameMap sender)
    {
        if (sender == StrategicMap.Current)
        {
            StrategicMap.Current.AIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<PassiveAIGroup>(), AIGroupClass.Of<SuicidalAIGroup>(), AIGroupClass.Of<FortifyAIGroup>(), AIGroupClass.Of<TransportAIGroup>()]);
        }
    }

    internal static void SubscribeMapLoaded(GameEngine engine) => engine.HexSpriteMapLoaded += MapLoaded;
}
