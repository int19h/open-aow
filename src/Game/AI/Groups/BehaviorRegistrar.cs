// Translated from BehaviorAG.pas.
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

public partial class BehaviorRegistrar
{
    // BehaviorAG.pas:136
    public static void MapLoaded(HSMap Sender)
    {
        if (Sender == AoWHSMap.Current)
        {
            AoWHSMap.Current.FAIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<DefenderAIGroup>(), AIGroupClass.Of<AggressorAIGroup>(), AIGroupClass.Of<ExpanderAIGroup>(), AIGroupClass.Of<ScorcherAIGroup>(), AIGroupClass.Of<NormalAIGroup>(), AIGroupClass.Of<SkirmishAIGroup>()]);
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
