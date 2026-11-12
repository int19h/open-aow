// Translated from BehaviorAG.pas.
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

public partial class BehaviorRegistrar
{
    // BehaviorAG.pas:136
    public static void MapLoaded(GameMap sender)
    {
        if (sender == StrategicMap.Current)
        {
            StrategicMap.Current.AIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<DefenderAIGroup>(), AIGroupClass.Of<AggressorAIGroup>(), AIGroupClass.Of<ExpanderAIGroup>(), AIGroupClass.Of<ScorcherAIGroup>(), AIGroupClass.Of<NormalAIGroup>(), AIGroupClass.Of<SkirmishAIGroup>()]);
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
