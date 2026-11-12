// Translated from IndependentAG.pas.
using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

public partial class IndependentRegistrar
{
    // IndependentAG.pas:136
    public static void MapLoaded(GameMap sender)
    {
        if (sender == StrategicMap.Current)
        {
            StrategicMap.Current.AIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<GuardAIGroup>(), AIGroupClass.Of<GuardAreaAIGroup>(), AIGroupClass.Of<PatrolAIGroup>(), AIGroupClass.Of<RefugeAIGroup>(), AIGroupClass.Of<RaidAIGroup>(), AIGroupClass.Of<ScoutAIGroup>()]);
        }
    }

    internal static void SubscribeMapLoaded(GameEngine engine)
    {
        engine.OnHsmLoaded += MapLoaded;
    }
}
