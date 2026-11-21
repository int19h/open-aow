#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from BehaviorAG.pas.

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

public sealed class BehaviorRegistrar
{
    // BehaviorAG.pas:136
    public static void MapLoaded(GameMap sender)
    {
        if (sender == StrategicMap.Current)
        {
            StrategicMap.Current.AIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<DefenderAIGroup>(), AIGroupClass.Of<AggressorAIGroup>(), AIGroupClass.Of<ExpanderAIGroup>(), AIGroupClass.Of<ScorcherAIGroup>(), AIGroupClass.Of<NormalAIGroup>(), AIGroupClass.Of<SkirmishAIGroup>()]);
        }
    }

    internal static void SubscribeMapLoaded(GameEngine engine) => engine.OnHsmLoaded += MapLoaded;
}
